﻿// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="LoadImageProcessor.cs" company="Resolution Technology, Inc.">
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

     /// <summary>
     /// Processor class for Loading and displaying an image.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.Models.ProcessorBase" />
     public class LoadImageProcessor : ProcessorBase
     {
          #region Private Fields

          /// <summary>
          /// Stores the loaded unprocessed image.
          /// </summary>
          private HImage image = new HImage();

          /// <summary>
          /// Stores the image height.
          /// </summary>
          private int imageHeight = 0;

          /// <summary>
          /// Stores the image width.
          /// </summary>
          private int imageWidth = 0;

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          #endregion Private Fields

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the LoadImageProcessor class.
          /// </summary>
          public LoadImageProcessor()
              : base()
          {
               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Image)
                  .Where(x => x != null)
                  .Where(x => x.IsInitialized())
                  .Subscribe(img =>
                  {
                       if (img != null)
                       {
                            if (img.IsInitialized())
                            {
                                 img.GetImageSize(out int width, out int height);
                                 this.ImageHeight = height;
                                 this.ImageWidth = width;
                            }
                       }
                  }));
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the loaded unprocessed image.
          /// </summary>
          /// <value>The image.</value>
          public HImage Image
          {
               get => this.image;

               set => this.RaiseAndSetIfChanged(ref this.image, value);
          }

          /// <summary>
          /// Gets or sets the image height.
          /// </summary>
          /// <value>The height of the image.</value>
          public int ImageHeight
          {
               get => this.imageHeight;

               set => this.RaiseAndSetIfChanged(ref this.imageHeight, value);
          }

          /// <summary>
          /// Gets or sets the image width.
          /// </summary>
          /// <value>The width of the image.</value>
          public int ImageWidth
          {
               get => this.imageWidth;

               set => this.RaiseAndSetIfChanged(ref this.imageWidth, value);
          }

          #endregion Public Properties

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
                         // Call sub methods here.
                         ////this.LoadImage();
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

               string fileName = string.Empty;

               if (parameters is Tuple<string>)
               {
                    if ((Tuple<string>)parameters != null)
                    {
                         fileName = ((Tuple<string>)parameters).Item1;
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
                         // Call sub methods here.
                         this.LoadImage(fileName);
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

                    this.image?.Dispose();

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region Private Methods

          /// <summary>
          /// Loads an image from a file name.
          /// </summary>
          /// <param name="fileName">The filename of the image to load.</param>
          private void LoadImage(string fileName)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";

               try
               {
                    this.ErrorMessage = "No Errors.";
                    this.Image.Dispose();
                    this.Image = new HImage(fileName);
               }
               catch (Exception ex)
               {
                    this.ErrorCode = ProcessingErrorCode.LoadImageError;
                    this.ErrorMessage = "An error occurred while loading an image: " + ex.Message;
               }
          }

          #endregion Private Methods
     }
}