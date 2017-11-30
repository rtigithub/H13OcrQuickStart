//-----------------------------------------------------------------------
// <copyright file="AcquireCalibrationProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.Models
{
     using System;
     using HalconDotNet;
     using ReactiveUI;

     /// <summary>
     /// Model class for a new Acquire Calibration process.
     /// </summary>
     public class FirstCalibrationProcessor : ProcessorBase, ICameraCalibrationProcessor
     {
          #region Private Declarations

          /// <summary>
          /// Stores a value indicating whether the calibration images are set.
          /// </summary>
          private bool areCalibrationImagesSet = false;

          /// <summary>
          /// Stores a value indicating whether the calibration parameters are set.
          /// </summary>
          private bool areCalibrationParametersSet = false;

          /// <summary>
          /// Stores the calibration data object.
          /// </summary>
          private HCalibData calibrationData;

          /// <summary>
          /// Stores the count of calibration images.
          /// </summary>
          private int calibrationImageCount = 0;

          /// <summary>
          /// Stores a value indicating whether the calibration is complete.
          /// </summary>
          private bool calibrationIsDone = false;

          /// <summary>
          /// Stores the projection map that describes the mapping between the image plane and a the
          /// plane z=0 of a world coordinate system.
          /// </summary>
          private HImage calibrationMap = new HImage();

          /// <summary>
          /// Stores the current image width used by the calibration.
          /// </summary>
          private int currentImageHeight = 480;

          /// <summary>
          /// Stores the current image height used by the calibration.
          /// </summary>
          private int currentImageWidth = 640;

          /// <summary>
          /// Stores the final camera parameters.
          /// </summary>
          private HTuple finalCameraParameters;

          //// Create backing fields for the properties as needed.
          /// <summary>
          /// Stores the initial camera parameters.
          /// </summary>
          private HTuple initialCameraParameters;

          /// <summary>
          /// Stores a value indicating whether the calibration map is present.
          /// </summary>
          private bool isCalibrationMapPresent = false;

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// Stores a value indicating whether the rectified test image is present.
          /// </summary>
          private bool isRectifiedTestImagePresent = false;

          /// <summary>
          /// Stores the most recently set calibration plate name.
          /// </summary>
          private string lastCalibrationPlateName = string.Empty;

          /// <summary>
          /// Stores the most recently set camera type.
          /// </summary>
          private string lastCameraType = string.Empty;

          /// <summary>
          /// Stores the rectified test image.
          /// </summary>
          private HImage rectifiedTestImage = new HImage();

          /// <summary>
          /// Stores the world pose obtained from the first calibration image object after calibration.
          /// </summary>
          private HPose worldPose = new HPose();

          #endregion Private Declarations

          #region Constructors

          /// <summary>
          /// Initializes a new instance of the AcquireCalibrationProcessor class.
          /// </summary>
          public FirstCalibrationProcessor()
                : base()
          {
          }

          #endregion Constructors



          #region Public Properties

          //// Create properties for objects and display objects set in the process methods. Not including output results.

          /// <summary>
          /// Gets or sets a value indicating whether the calibration images are set.
          /// </summary>
          public bool AreCalibrationImagesSet
          {
               get => this.areCalibrationImagesSet;

               set => this.RaiseAndSetIfChanged(ref this.areCalibrationImagesSet, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the calibration parameters are set.
          /// </summary>
          public bool AreCalibrationParametersSet
          {
               get => this.areCalibrationParametersSet;

               set => this.RaiseAndSetIfChanged(ref this.areCalibrationParametersSet, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the calibration is complete.
          /// </summary>
          public bool CalibrationIsDone
          {
               get => this.calibrationIsDone;

               set => this.RaiseAndSetIfChanged(ref this.calibrationIsDone, value);
          }

          /// <summary>
          /// Gets or sets the projection map that describes the mapping between the image plane and a the
          /// plane z=0 of a world coordinate system.
          /// </summary>
          public HImage CalibrationMap
          {
               get => this.calibrationMap;

               set => this.RaiseAndSetIfChanged(ref this.calibrationMap, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the calibration map is present.
          /// </summary>
          public bool IsCalibrationMapPresent
          {
               get => this.isCalibrationMapPresent;

               set => this.RaiseAndSetIfChanged(ref this.isCalibrationMapPresent, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the rectified test image is present.
          /// </summary>
          public bool IsRectifiedTestImagePresent
          {
               get => this.isRectifiedTestImagePresent;

               set => this.RaiseAndSetIfChanged(ref this.isRectifiedTestImagePresent, value);
          }

          /// <summary>
          /// Gets or sets rectified test image.
          /// </summary>
          public HImage RectifiedTestImage
          {
               get => this.rectifiedTestImage;

               set => this.RaiseAndSetIfChanged(ref this.rectifiedTestImage, value);
          }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Creates a camera calibration from the data.
          /// </summary>
          /// <param name="imageWidth">The image width.</param>
          /// <param name="imageHeight">The image height.</param>
          /// <param name="rectifiedImageWidth">The rectified image width.</param>
          /// <param name="rectifiedImageHeight">The rectified image height.</param>
          /// <param name="calibratedScale">The calibrated scale.</param>
          /// <param name="worldPoseIndex">The index calibration images to use as the world pose.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult Calibrate(
              int imageWidth,
              int imageHeight,
              int rectifiedImageWidth,
              int rectifiedImageHeight,
              double calibratedScale,
              int worldPoseIndex)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         double error = this.CalibrateCameras(
                             imageWidth,
                             imageHeight,
                             rectifiedImageWidth,
                             rectifiedImageHeight,
                             calibratedScale,
                             worldPoseIndex);

                         //// Store any output results as named values in the ResultsCollection object.

                         result.ResultsCollection.Add("CalibrationError", error);
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
                    result.ErrorMessage = "An error occurred during ProcessAcquiredCalibrationImage: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Loads all calibration images in a specified folder.
          /// </summary>
          /// <param name="folderName">The specified folder.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult LoadCalibrationImages(string folderName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         //// Call private methods that perform the processes here.
                         this.ReadCalibrationImages(folderName);

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
                    result.StatusCode = ProcessingErrorCode.CalibrationError;
                    result.ErrorMessage = "An error occurred during LoadCalibrationImages: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Loads a calibration map from file.
          /// </summary>
          /// <param name="fileName">the file name.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult LoadCalibrationMap(string fileName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();
               HImage image = new HImage();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         if (fileName != string.Empty)
                         {
                              //// Call private methods that perform the processes here.
                              this.CalibrationMap.Dispose();
                              image.ReadImage(fileName);
                              this.CalibrationMap = image.CopyObj(1, -1);
                              this.IsCalibrationMapPresent = true;

                              //// Store any output results as named values in the ResultsCollection object.
                              ////result.ResultsCollection.Add("MyNamedResultValue", /* A returned value from a private method. */ ));
                         }
                    }

                    // These lines pass any accumulated error information to the result class.
                    result.ErrorMessage = this.ErrorMessage;
                    result.StatusCode = this.ErrorCode;
               }
               catch (Exception ex)
               {
                    // If an exception gets here it is unexpected.
                    result.StatusCode = ProcessingErrorCode.CalibrationError;
                    result.ErrorMessage = "An error occurred during LoadCalibrationMap: " + ex.Message;
               }
               finally
               {
                    image.Dispose();
               }

               return result;
          }

          /// <summary>
          /// Implements the process for this processor class. Not used in this class as we need to pass parameters for all major methods.
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
          /// Processes the acquired calibration image.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult ProcessAcquiredCalibrationImage(HImage image)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         if (image.IsInitialized())
                         {
                              //// Call private methods that perform the processes here.
                              this.ProcessCalibrationImage(image);
                         }
                         else
                         {
                              this.ErrorMessage = "Calibration image data is empty.";
                              this.ErrorCode = ProcessingErrorCode.CalibrationError;
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
                    result.ErrorMessage = "An error occurred during ProcessAcquiredCalibrationImage: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Rectifies an image.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult RectifyImage(HImage image)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         if (image.IsInitialized())
                         {
                              //// Call private methods that perform the processes here.
                              this.RectifyTestImage(image);
                         }
                         else
                         {
                              this.ErrorMessage = "Calibration Test image data is empty.";
                              this.ErrorCode = ProcessingErrorCode.CalibrationError;
                         }
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
                    result.ErrorMessage = "An error occurred during RectifyImage: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Resets the calibration images.
          /// </summary>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult ResetCalibrationImages()
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         this.ResetCalibrationData();
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
                    result.ErrorMessage = "An error occurred during ProcessAcquiredCalibrationImage: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Saves the calibration map to file.
          /// </summary>
          /// <param name="fileName">The file name.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult SaveCalibrationMap(string fileName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         this.CalibrationMap.WriteImage("tiff", 0, fileName);
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
                    result.ErrorMessage = "An error occurred during SaveCalibrationMap: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Saves the rectified test image to file.
          /// </summary>
          /// <param name="fileName">The file name.</param>
          /// <returns>An instance of a Processing Result.</returns>
          public ProcessingResult SaveRectifiedTestImage(string fileName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         this.RectifiedTestImage.WriteImage("tiff", 0, fileName);
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
                    result.ErrorMessage = "An error occurred during SaveCalibrationMap: " + ex.Message;
               }

               return result;
          }

          /// <summary>
          /// Sets the initial camera parameters.
          /// </summary>
          /// <param name="cameraType">The camera type.</param>
          /// <param name="focalLength">The focal length.</param>
          /// <param name="imageWidth">The image width.</param>
          /// <param name="imageHeight">The image height.</param>
          /// <param name="sensorSizeX">The size of the sensor in the X direction.</param>
          /// <param name="sensorSizeY">The size of the sensor in the Y direction.</param>
          /// <param name="rotation">The rotation.</param>
          /// <param name="tilt">The tilt.</param>
          /// <param name="halconCalibrationPlateName">The name of the halcon calibration plate.</param>
          /// <returns>A Processing Result instance.</returns>
          public ProcessingResult SetInitialParameters(
              string cameraType,
              double focalLength,
              int imageWidth,
              int imageHeight,
              double sensorSizeX,
              double sensorSizeY,
              double rotation,
              double tilt,
              string halconCalibrationPlateName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               try
               {
                    if (this.ErrorCode == ProcessingErrorCode.NoError)
                    {
                         //// Call private methods that perform the processes here.
                         this.SetCalibrationData(
                             cameraType,
                             focalLength,
                             imageWidth,
                             imageHeight,
                             sensorSizeX,
                             sensorSizeY,
                             rotation,
                             tilt,
                             halconCalibrationPlateName);

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

          //// TODO: work out a way to pass the format along.

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

                    this.rectifiedTestImage?.Dispose();

                    this.calibrationData?.Dispose();

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region private methods

          /// <summary>
          /// Performs the camera calibration.
          /// </summary>
          /// <param name="imageWidth">The image width.</param>
          /// <param name="imageHeight">The image height.</param>
          /// <param name="rectifiedImageWidth">The rectified image width.</param>
          /// <param name="rectifiedImageHeight">The rectified image height.</param>
          /// <param name="calibratedScale">The calibrated scale.</param>
          /// <param name="worldPoseIndex">The index calibration images to use as the world pose.</param>
          /// <returns>The error estimate of the calibration.</returns>
          private double CalibrateCameras(
              int imageWidth,
              int imageHeight,
              int rectifiedImageWidth,
              int rectifiedImageHeight,
              double calibratedScale,
              int worldPoseIndex)
          {
               this.calibrationIsDone = false;

               try
               {
                    double error;
                    HTuple poseTuple;
                    error = this.calibrationData.CalibrateCameras();
                    this.finalCameraParameters = this.calibrationData.GetCalibData("camera", 0, "params");
                    poseTuple = this.calibrationData.GetCalibData("calib_obj_pose", new HTuple(0, worldPoseIndex), new HTuple("pose"));

                    this.worldPose.CreatePose(poseTuple[0], poseTuple[1], poseTuple[2], poseTuple[3], poseTuple[4], poseTuple[5], "Rp+T", "gba", "point");

                    HCamPar hcamPar = new HCamPar(this.finalCameraParameters);

                    hcamPar.ImagePointsToWorldPlane(
                        this.worldPose,
                        new HTuple(0.0),
                        new HTuple(0.0),
                        "m",
                        out HTuple worldX,
                        out HTuple worldY);

                    this.worldPose = this.worldPose.SetOriginPose(worldX, worldY, 0.0);

                    this.CalibrationMap = this.worldPose.GenImageToWorldPlaneMap(
                                             hcamPar,
                                             imageWidth,
                                             imageHeight,
                                             rectifiedImageWidth,
                                             rectifiedImageHeight,
                                             calibratedScale,
                                             "bilinear");

                    this.CalibrationIsDone = true;
                    this.IsCalibrationMapPresent = true;
                    return error;
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in CalibrateCameras" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    return 100.0;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in CalibrateCameras" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    return 100.0;
               }
          }

          /// <summary>
          /// Processes the acquired calibration image.
          /// </summary>
          /// <param name="calibrationImage">The calibration image.</param>
          private void ProcessCalibrationImage(HImage calibrationImage)
          {
               HTuple paramNames = new HTuple();
               HTuple paramValues = new HTuple();

               try
               {
                    calibrationImage.GetImageSize(out int width, out int height);
                    if ((width == this.currentImageWidth) && (height == this.currentImageHeight))
                    {
                         this.calibrationData.FindCalibObject(calibrationImage, 0, 0, this.calibrationImageCount, paramNames, paramValues);
                         this.calibrationImageCount++;
                    }
                    else
                    {
                         this.ErrorMessage = "Calibration image size does not match the initial calibration parameter values.";
                         this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    }
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in ProcessCalibrationImage" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in ProcessCalibrationImage" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
          }

          /// <summary>
          /// Reads and processes all calibration images in the given directory.
          /// </summary>
          /// <param name="calibrationImageDirectory">The directory.</param>
          private void ReadCalibrationImages(string calibrationImageDirectory)
          {
               if (this.areCalibrationParametersSet)
               {
                    string[] files = System.IO.Directory.GetFiles(calibrationImageDirectory);
                    int count = files.Length;
                    this.calibrationImageCount = 0;

                    if (count == 0)
                    {
                         this.ErrorMessage = "No calibration images found in the specified directory";
                    }

                    HImage image = new HImage();
                    HTuple paramNames = new HTuple();
                    HTuple paramValues = new HTuple();
                    string extension = string.Empty;

                    try
                    {
                         for (int i = 0; i < count; i++)
                         {
                              extension = System.IO.Path.GetExtension(files[i]);
                              if (extension == ".png")
                              {
                                   image.Dispose();
                                   image.ReadImage(files[i]);
                                   this.ProcessCalibrationImage(image);
                              }
                         }

                         if (this.ErrorCode == ProcessingErrorCode.NoError)
                         {
                              // Do we want a minimum count?
                              this.AreCalibrationImagesSet = true;
                         }
                    }
                    catch (HalconException hex)
                    {
                         this.ErrorMessage = "Halcon Exception in ReadCalibrationImages" + hex.Message;
                         this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    }
                    catch (SystemException ex)
                    {
                         this.ErrorMessage = "System Exception in ReadCalibrationImages" + ex.Message;
                         this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    }
                    finally
                    {
                         image.Dispose();
                    }
               }
          }

          /// <summary>
          /// Rectifies a test image.
          /// </summary>
          /// <param name="image">The image.</param>
          private void RectifyTestImage(HImage image)
          {
               HImage rectifiedImage = new HImage();

               try
               {
                    if (ViewModels.FirstCalibrationViewModel.CheckImageAgainstMap(image, this.CalibrationMap))
                    {
                         rectifiedImage = image.MapImage(this.CalibrationMap);
                         this.RectifiedTestImage = rectifiedImage.FullDomain();
                         this.IsRectifiedTestImagePresent = true;
                    }
                    else
                    {
                         this.ErrorMessage = "The calibration map cannot be applied to this image.";
                         this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    }
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in RectifyTestImage" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in RectifyTestImage" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
               finally
               {
                    rectifiedImage.Dispose();
               }
          }

          /// <summary>
          /// Resets the calibration images.
          /// </summary>
          private void ResetCalibrationData()
          {
               HTuple paramNames = new HTuple();
               HTuple paramValues = new HTuple();

               try
               {
                    this.calibrationImageCount = 0;
                    this.calibrationData.Dispose();
                    if (this.AreCalibrationParametersSet)
                    {
                         this.calibrationData.SetCalibDataCamParam(0, this.lastCameraType, new HCamPar(this.initialCameraParameters));
                         this.calibrationData.SetCalibDataCalibObject(0, this.lastCalibrationPlateName);
                    }
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in ProcessCalibrationImage" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in ProcessCalibrationImage" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
               }
          }

          /// <summary>
          /// Sets the calibration data.
          /// </summary>
          /// <param name="cameraType">The camera type.</param>
          /// <param name="focalLength">The focal length.</param>
          /// <param name="imageWidth">The image width.</param>
          /// <param name="imageHeight">The image height.</param>
          /// <param name="sensorSizeX">The size of the sensor in the X direction.</param>
          /// <param name="sensorSizeY">The size of the sensor in the Y direction.</param>
          /// <param name="rotation">The rotation.</param>
          /// <param name="tilt">The tilt.</param>
          /// <param name="halconCalibrationPlateName">The name of the halcon calibration plate.</param>
          private void SetCalibrationData(
              string cameraType,
              double focalLength,
              int imageWidth,
              int imageHeight,
              double sensorSizeX,
              double sensorSizeY,
              double rotation,
              double tilt,
              string halconCalibrationPlateName)
          {
               try
               {
                    this.lastCameraType = cameraType;
                    this.lastCalibrationPlateName = halconCalibrationPlateName;
                    this.currentImageWidth = imageWidth;
                    this.currentImageHeight = imageHeight;

                    this.calibrationData = new HCalibData("calibration_object", 1, 1);

                    switch (cameraType)
                    {
                         case "area_scan_division":
                              this.initialCameraParameters = new HTuple(
                              focalLength,
                              0.0,
                              sensorSizeX,
                              sensorSizeY,
                              imageWidth / 2.0,
                              imageHeight / 2.0,
                              imageWidth,
                              imageHeight);
                              this.AreCalibrationParametersSet = true;
                              break;

                         case "area_scan_polynomial":
                              this.initialCameraParameters = new HTuple(
                              focalLength,
                              0.0,
                              0.0,
                              0.0,
                              0.0,
                              0.0,
                              sensorSizeX,
                              sensorSizeY,
                              imageWidth / 2.0,
                              imageHeight / 2.0,
                              imageWidth,
                              imageHeight);

                              this.AreCalibrationParametersSet = true;
                              break;

                         case "area_scan_telecentric_division":
                              this.initialCameraParameters = new HTuple(
                              0.0,
                              0.0,
                              sensorSizeX,
                              sensorSizeY,
                              imageWidth / 2.0,
                              imageHeight / 2.0,
                              imageWidth,
                              imageHeight);
                              this.AreCalibrationParametersSet = true;
                              break;

                         case "area_scan_telecentric_polynomial":
                              this.initialCameraParameters = new HTuple(
                              0.0,
                              0.0,
                              0.0,
                              0.0,
                              0.0,
                              0.0,
                              sensorSizeX,
                              sensorSizeY,
                              imageWidth / 2.0,
                              imageHeight / 2.0,
                              imageWidth,
                              imageHeight);

                              this.AreCalibrationParametersSet = true;
                              break;
                    }

                    if (this.AreCalibrationParametersSet)
                    {
                         this.calibrationData.SetCalibDataCamParam(0, cameraType, new HCamPar(this.initialCameraParameters));
                         this.calibrationData.SetCalibDataCalibObject(0, halconCalibrationPlateName);
                    }
               }
               catch (HalconException hex)
               {
                    this.ErrorMessage = "Halcon Exception in SetCalibrationData" + hex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    this.AreCalibrationParametersSet = false;
               }
               catch (SystemException ex)
               {
                    this.ErrorMessage = "System Exception in SetCalibrationData" + ex.Message;
                    this.ErrorCode = ProcessingErrorCode.CalibrationError;
                    this.AreCalibrationParametersSet = false;
               }
          }

          #endregion private methods
     }
}