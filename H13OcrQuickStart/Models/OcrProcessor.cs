// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="OcrProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.Models
{
     using System;
     using System.Reactive.Linq;
     using HalconDotNet;
     using ReactiveUI;
     using Rti.DisplayUtilities;
     using Rti.Halcon;

     /// <summary>
     /// Model class for a new process.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.Models.ProcessorBase" />
     public class OcrProcessor : ProcessorBase
     {
          #region Private Fields

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// The processed image
          /// </summary>
          private HImage processedImage = new HImage();

          /// <summary>
          /// The processed region
          /// </summary>
          private HRegion processedRegion = new HRegion();

          /// <summary>
          /// The text model
          /// </summary>
          private HTextModel textModel = new HTextModel("auto", "Universal_Rej.occ");

          #endregion Private Fields

          //// Create backing fields for the properties as needed.

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the Processor class.
          /// </summary>
          public OcrProcessor()
              : base()
          {
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the processed image.
          /// </summary>
          /// <value>The processed image.</value>
          public HImage ProcessedImage { get => this.processedImage; set => this.RaiseAndSetIfChanged(ref this.processedImage, value); }

          /// <summary>
          /// Gets or sets the processed region.
          /// </summary>
          /// <value>The processed region.</value>
          public HRegion ProcessedRegion { get => this.processedRegion; set => this.RaiseAndSetIfChanged(ref this.processedRegion, value); }

          #endregion Public Properties

          //// Create properties for objects and display objects set in the process methods. Not including output results.

          #region Public Methods

          /// <summary>
          /// Implements the process for this processor class.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
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
          /// Implements the process for this processor class.
          /// </summary>
          /// <param name="parameters">A non-generic Tuple containing any parameters.</param>
          /// <returns>A ProcessingResult instance.</returns>
          public override ProcessingResult Process(object parameters)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               HImage image = new HImage();

               // In overloads, change this to the types being passes and parse them, assigning to
               // properties as needed.
               if (parameters is Tuple<HImage>)
               {
                    if ((Tuple<HImage>)parameters != null)
                    {
                         image.Dispose();
                         image = ((Tuple<HImage>)parameters).Item1;
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
                         result = ProcessImage(image);
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
               finally
               {
                    image?.Dispose();
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
                         this.processedImage?.Dispose();
                         this.processedRegion?.Dispose();
                    }

                    //// Dispose of unmanaged resources here.

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region Private Methods

          /// <summary>
          /// Corrects the text orientation.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <param name="regionOfInterest">The region of interest.</param>
          private void CorrectTextOrientation(HImage image, HRegion regionOfInterest)
          {
               double phi = FindTextLineOrientation(image, regionOfInterest);
               TransformImage(image, phi);
          }

          /// <summary>
          /// Finds the text line orientation.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <param name="regionOfInterest">The region of interest.</param>
          /// <returns>System.Double.</returns>
          /// <exception cref="NotImplementedException"></exception>
          private double FindTextLineOrientation(HImage image, HRegion regionOfInterest)
          {
               throw new NotImplementedException();
          }

          /// <summary>
          /// Processes the image.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <returns>ProcessingResult.</returns>
          private ProcessingResult ProcessImage(HImage image)
          {
               var result = new ProcessingResult();

               try
               {
                    if (image.IsValid())
                    {
                         this.ProcessedImage.Dispose();
                         ProcessedImage = image.CountChannels().I == 3 ? image.Rgb1ToGray() : image.AccessChannel(1);
                         using (var textResultId = this.ProcessedImage.FindText(textModel))
                         {
                              this.ProcessedRegion.Dispose();
                              using (var textRegions = textResultId.GetTextObject("all_lines"))
                              {
                                   this.ProcessedRegion = textRegions.ToHRegion().Union1();
                              }
                              string ocrResults = String.Join(String.Empty, textResultId.GetTextResult("class").ToSArr());
                              result.ResultsCollection.Add("OcrResults", ocrResults);
                         }
                    }
                    else
                    {
                         result.StatusCode = ProcessingErrorCode.LoadImageError;
                         result.ErrorMessage = "No image loaded.";
                    }
               }
               catch (HalconException halconException)
               {
                    result.StatusCode = ProcessingErrorCode.HalconException;
                    result.ErrorMessage = "An error occurred during processing: " + halconException.Message;
               }
               finally
               {
               }
               return result;
          }

          /// <summary>
          /// Transforms the image.
          /// </summary>
          /// <param name="image">The image.</param>
          /// <param name="phi">The phi.</param>
          /// <exception cref="NotImplementedException"></exception>
          private void TransformImage(HImage image, double phi)
          {
               throw new NotImplementedException();
          }

          #endregion Private Methods
     }
}