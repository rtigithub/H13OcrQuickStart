//-----------------------------------------------------------------------
// <copyright file="AcquireAcquisitionProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.Models
{
     using System;
     using System.Collections.Generic;
     using System.Reactive.Linq;
     using HalconDotNet;
     using ReactiveUI;
     using Rti.DisplayUtilities;
     using Rti.Halcon;

     /// <summary>
     /// Model class for a new Acquire Acquisition process.
     /// </summary>
     public class FirstAcquisitionProcessor : ProcessorBase
     {
          #region Private Declarations

          /// <summary>
          /// Stores the acquired image.
          /// </summary>
          private HImage acquiredImage = new HImage();

          //// Create backing fields for the properties as needed.
          /// <summary>
          /// Stores the acquired image width.
          /// </summary>
          private int acquiredImageHeight = 1;

          /// <summary>
          /// Stores the acquired image height.
          /// </summary>
          private int acquiredImageWidth = 1;

          /// <summary>
          /// Stores a value indicating whether live video is playing.
          /// </summary>
          private bool acquiringLiveVideo = false;

          /// <summary>
          /// Stores the acquisition parameter names.
          /// </summary>
          private string[] acquisitionParameterNames;

          // Stores the calibration map in use.
          private HImage calibrationMap = new HImage();

          /// <summary>
          /// Stores a value indicating whether the next live video frame can be grabbed safely.
          /// </summary>
          private bool canGrabNextFrame = true;

          /// <summary>
          /// Stores a dictionary of acquisition parameter names and default values for the currently
          /// active acquisition interface.
          /// </summary>
          private Dictionary<string, HTuple> dictionaryParameterDefaults = new Dictionary<string, HTuple>();

          // Stores a value indicating whether to calibrate new images.
          private bool doCalibration = false;

          /// <summary>
          /// Stores the frame grabber.
          /// </summary>
          private HFramegrabber frameGrabber = new HFramegrabber();

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// Stores a value indicating whether the frame grabber is initialized.
          /// </summary>
          private bool isInitialized = false;

          #endregion Private Declarations

          #region Constructors

          /// <summary>
          /// Initializes a new instance of the AcquireAcquisitionProcessor class.
          /// </summary>
          public FirstAcquisitionProcessor()
               : base()
          {
               // not used at present.
               this.acquisitionParameterNames = new string[]
               {
                "horizontal_resolution",
                "vertical_resolution",
                "image_width",
                "image_height",
                "start_row",
                "start_column",
                "field",
                "bits_per_channel",
                "color_space",
                "generic",
                "external_trigger",
                "camera_type",
                "device",
                "port",
                "line_in"
               };

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.AcquiredImage)
                  .Where(x => x.IsValid())
                  .Subscribe(img =>
                  {
                       if (img != null)
                       {
                            if (img.IsInitialized())
                            {
                                 img.GetImageSize(out int width, out int height);
                                 this.AcquiredImageHeight = height;
                                 this.AcquiredImageWidth = width;
                            }
                       }
                  }));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.AcquiringLiveVideo, x => x.CanGrabNextFrame)
                    .Where(a => a.Item1 && a.Item2)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                         if (this.doCalibration)
                         {
                              Observable.Start(() => this.AcquireAndCalibrateImage(this.calibrationMap));
                         }
                         else
                         {
                              Observable.Start(this.AcquireImage);
                         }
                    }));
          }

          #endregion Constructors



          #region Public Properties

          //// Create properties for objects and display objects set in the process methods. Not including output results.

          /// <summary>
          /// Gets or sets the acquired image.
          /// </summary>
          public HImage AcquiredImage
          {
               get => this.acquiredImage;

               set => this.RaiseAndSetIfChanged(ref this.acquiredImage, value);
          }

          /// <summary>
          /// Gets or sets the image height.
          /// </summary>
          public int AcquiredImageHeight
          {
               get => this.acquiredImageHeight;

               set => this.RaiseAndSetIfChanged(ref this.acquiredImageHeight, value);
          }

          /// <summary>
          /// Gets or sets the image width.
          /// </summary>
          public int AcquiredImageWidth
          {
               get => this.acquiredImageWidth;

               set => this.RaiseAndSetIfChanged(ref this.acquiredImageWidth, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether live video is playing.
          /// </summary>
          public bool AcquiringLiveVideo
          {
               get => this.acquiringLiveVideo;

               set => this.RaiseAndSetIfChanged(ref this.acquiringLiveVideo, value);
          }

          /// <summary>
          /// Gets the acquisition parameter names.
          /// </summary>
          public string[] AcquisitionParameterNames => this.acquisitionParameterNames;

          /// <summary>
          /// Gets or sets a value indicating whether the next live video frame can be grabbed safely.
          /// </summary>
          public bool CanGrabNextFrame
          {
               get => this.canGrabNextFrame;

               set => this.RaiseAndSetIfChanged(ref this.canGrabNextFrame, value);
          }

          /// <summary>
          /// Gets the dictionary of parameter defaults.
          /// </summary>
          public Dictionary<string, HTuple> DictionaryParameterDefaults => this.dictionaryParameterDefaults;

          /// <summary>
          /// Gets or sets a value indicating whether the frame grabber is initialized.
          /// </summary>
          public bool IsInitialized
          {
               get => this.isInitialized;

               set => this.RaiseAndSetIfChanged(ref this.isInitialized, value);
          }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Checks that the calibration map does not attempt to access image coordinates outside of
          /// the image.
          /// </summary>
          /// <param name="image">The image</param>
          /// <param name="map">The calibration map.</param>
          /// <returns>A value indicating whether the calibration map can be applied safely.</returns>
          public static bool CheckImageAgainstMap(HImage image, HImage map)
          {
               int linearSize;
               HRegion badPixels = new HRegion();
               HRegion unionBadPixels = new HRegion();

               try
               {
                    image.GetImageSize(out int imageWidth, out int imageHeight);
                    linearSize = imageWidth * imageHeight;
                    badPixels = map.Threshold(new HTuple(int.MinValue, linearSize), new HTuple(-1, int.MaxValue));
                    unionBadPixels = badPixels.Union1();
                    return unionBadPixels.AreaCenter(out double unusedDouble1, out double unusedDouble2) <= 0;
               }
               finally
               {
                    badPixels.Dispose();
                    unionBadPixels.Dispose();
               }
          }

          /// <summary>
          /// Starts live video asynchronously or sets the boolean value to stop it.
          /// </summary>
          /// <param name="trigger">A value indicating whether to start or stop the video.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public ProcessingResult AcquireCalibratedLiveVideo(bool trigger, HImage map)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (trigger)
                    {
                         this.doCalibration = true;
                         this.calibrationMap.Dispose();
                         this.calibrationMap = map;
                         this.AcquiringLiveVideo = true;
                         this.CanGrabNextFrame = true;
                    }
                    else
                    {
                         this.AcquiringLiveVideo = false;
                    }
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.AcquisitionError;
                    result.ErrorMessage = "An error occurred during video acquisition: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Acquires an image and corrects it with the provided calibration map.
          /// </summary>
          /// <param name="calibrationMap">The calibration map.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public ProcessingResult AcquireImageWithCalibration(HImage calibrationMap)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         // Call sub methods here.
                         this.AcquireAndCalibrateImage(calibrationMap);
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.UndefinedError;
                    result.ErrorMessage = "An error occurred during processing: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Starts live video asynchronously or sets the boolean value to stop it.
          /// </summary>
          /// <param name="trigger">A value indicating whether to start or stop the video.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public ProcessingResult AcquireLiveVideo(bool trigger)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (trigger)
                    {
                         this.doCalibration = false;
                         this.AcquiringLiveVideo = true;
                         this.CanGrabNextFrame = true;
                    }
                    else
                    {
                         this.AcquiringLiveVideo = false;
                    }
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.AcquisitionError;
                    result.ErrorMessage = "An error occurred during video acquisition: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Generates the acquisition defaults dictionary.
          /// </summary>
          /// <param name="interfaceName">The selected acquisition interface name.</param>
          public void GenerateAcquisitionDefaults(string interfaceName)
          {
               string[] parameterNames =
               {
                "horizontal_resolution",
                "vertical_resolution",
                "image_width",
                "image_height",
                "start_row",
                "start_column",
                "field",
                "bits_per_channel",
                "color_space",
                "generic",
                "external_trigger",
                "camera_type",
                "device",
                "port",
                "line_in"
               };

               Rti.Halcon.HInfo.InfoFramegrabber(
                   interfaceName,
                   "defaults",
                   out HTuple valueList);

               this.dictionaryParameterDefaults.Clear();
               for (int i = 0; i < parameterNames.Length; i++)
               {
                    this.dictionaryParameterDefaults.Add(parameterNames[i], valueList[i]);
               }
          }

          /// <summary>
          /// Gets the available acquisition parameter values for a given parameter.
          /// </summary>
          /// <param name="interfaceName">The selected acquisition interface name.</param>
          /// <param name="parameterName">The parameter name.</param>
          /// <returns>A tuple of the available values.</returns>
          public HTuple GetAcquisitionParameterValues(string interfaceName, string parameterName)
          {
               Rti.Halcon.HInfo.InfoFramegrabber(
                       interfaceName,
                       parameterName,
                       out HTuple valueList);

               if (valueList.Length == 0)
               {
                    valueList = new HTuple(this.DictionaryParameterDefaults[parameterName]);
               }

               return valueList;
          }

          /// <summary>
          /// Initializes the camera.
          /// </summary>
          /// <param name="interfaceName">Name of the Halcon camera interface.</param>
          /// <param name="horizontalResolution">The horizontal resolution.</param>
          /// <param name="verticalResolution">The vertical resolution.</param>
          /// <param name="imageWidth">The image width.</param>
          /// <param name="imageHeight">The image height.</param>
          /// <param name="startRow">The start row.</param>
          /// <param name="startColumn">The start column.</param>
          /// <param name="field">The field value.</param>
          /// <param name="bitsPerChannel">The number of bits per channel.</param>
          /// <param name="colorSpace">The color space.</param>
          /// <param name="generic">The generic value.</param>
          /// <param name="externalTrigger">The external trigger value.</param>
          /// <param name="cameraType">The camera type.</param>
          /// <param name="device">The device value.</param>
          /// <param name="port">The port.</param>
          /// <param name="lineIn">The line in value.</param>
          /// <returns>An instance of a Processing result class.</returns>
          public ProcessingResult InitializeCamera(
              string interfaceName,
              int horizontalResolution,
              int verticalResolution,
              int imageWidth,
              int imageHeight,
              int startRow,
              int startColumn,
              string field,
              int bitsPerChannel,
              string colorSpace,
              string generic,
              string externalTrigger,
              string cameraType,
              string device,
              int port,
              int lineIn)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         this.frameGrabber.OpenFramegrabber(
                             interfaceName,
                             horizontalResolution,
                             verticalResolution,
                             imageWidth,
                             imageHeight,
                             startRow,
                             startColumn,
                             field,
                             bitsPerChannel,
                             colorSpace,
                             generic,
                             externalTrigger,
                             cameraType,
                             device,
                             port,
                             lineIn);

                         this.IsInitialized = true;
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.InitializingCameraError;
                    result.ErrorMessage = "An error occurred during processing: " + ex.Message;
                    this.IsInitialized = false;
               }

               return result;
          }

          /// <summary>
          /// Implements the process for this processor class, acquire an image.
          /// </summary>
          /// <returns>A Processing Result instance.</returns>
          public override ProcessingResult Process()
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         //// Call private methods that perform the processes here.
                         this.AcquireImage();
                         //// Store any output results as named values in the ResultsCollection object.
                         ////result.ResultsCollection.Add("MyNamedResultValue", /* A returned value from a private method. */ ));
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.UndefinedError;
                    result.ErrorMessage = "An error occurred during processing: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Implements the process for this processor class.
          /// </summary>
          /// <param name="parameters">A non-generic Tuple containing any parameters.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public override ProcessingResult Process(object parameters)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               // In overloads, change this to the types being passes and parse them, assigning to
               // properties as needed.
               if (parameters is Tuple<object>)
               {
                    if ((Tuple<object>)parameters != null)
                    {
                         object obj = ((Tuple<object>)parameters).Item1;
                    }
               }
               else
               {
                    this.ErrorCode = ProcessingErrorCode.ParameterError;
                    this.ErrorMessage = "Wrong Tuple types supplied in parameters.";
               }

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         //// Call private methods that perform the processes here.

                         //// Store any output results as named values in the ResultsCollection object.
                         ////result.ResultsCollection.Add("MyNamedResultValue", /* A returned value from a private method. */ ));

                         // Call additional methods in a sequence here, continuing the pattern.
                         if (this.ErrorCode == ProcessingErrorCode.NoError)
                         {
                         }
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.UndefinedError;
                    result.ErrorMessage = "An error occurred during processing: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Saves the acquired image to file.
          /// </summary>
          /// <param name="filename">The file name.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public ProcessingResult SaveImageProcess(string filename)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         this.SaveImage(filename);

                         //// Call private methods that perform the processes here.
                         //// Store any output results as named values in the ResultsCollection object.
                         ////result.ResultsCollection.Add("MyNamedResultValue", /* A returned value from a private method. */ ));
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.UndefinedError;  // define an initializingCameraError.
                    result.ErrorMessage = "An error occurred during processing: " + ex.Message;
               }

               return result;
          }

          #endregion Public Methods



          #region Protected Methods

          /// <summary>
          /// Overrides the Dispose method of IDisposable that actually disposes of managed resources.
          /// </summary>
          /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
          protected override void Dispose(bool disposing)
          {
               if (!this.isDisposed)
               {
                    if (disposing)
                    {
                         //// Dispose of managed resources here.
                    }

                    //// Dispose of unmanaged resources here.

                    this.acquiredImage?.Dispose();

                    this.calibrationMap?.Dispose();

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region private methods

          /// <summary>
          /// Acquires an image and corrects it with the provided calibration map.
          /// </summary>
          /// <param name="calibrationMap">The calibration map.</param>
          private void AcquireAndCalibrateImage(HImage calibrationMap)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               this.AcquiredImage.Dispose();

               HImage image = new HImage();
               HImage mappedImage = new HImage();
               try
               {
                    image = this.frameGrabber.GrabImageAsync(100.0);

                    // check image for valid values.
                    if (CheckImageAgainstMap(image, calibrationMap))
                    {
                         mappedImage = image.MapImage(calibrationMap);
                         this.AcquiredImage = mappedImage.FullDomain();
                    }
                    else
                    {
                         this.ErrorMessage = "The calibration map cannot be applied to this image.";
                         this.ErrorCode = ProcessingErrorCode.LoadImageError;
                    }
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "An error occurred while acquiring and calibrating an image: " + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
               catch (Exception ex)
               {
                    this.ErrorMessage = "An error occurred while acquiring and calibrating an image: " + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
               finally
               {
                    image.Dispose();
                    mappedImage.Dispose();
               }
          }

          /// <summary>
          /// Acquires an image from the frame grabber.
          /// </summary>
          private void AcquireImage()
          {
               try
               {
                    this.AcquiredImage.Dispose();
                    this.AcquiredImage = this.frameGrabber.GrabImageAsync(100.0);
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in AcquireImage" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in AcquireImage" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
          }

          /// <summary>
          /// Saves the acquired image to file.
          /// </summary>
          /// <param name="filename">The file name.</param>
          private void SaveImage(string filename)
          {
               try
               {
                    this.AcquiredImage.WriteImage("png", 0, filename);
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in SaveImage" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in SaveImage" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.AcquisitionError;
               }
          }

          #endregion private methods
     }
}