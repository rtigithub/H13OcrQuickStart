﻿// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="ProcessorBase.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.Models
{
     using System;
     using ReactiveUI;
     using Rti.DisplayUtilities;

     /// <summary>
     /// Base class for specific process models.
     /// </summary>
     /// <seealso cref="ReactiveUI.ReactiveObject" />
     /// <seealso cref="System.IDisposable" />
     /// <seealso cref="H13OcrQuickStart.Models.IProcessor" />
     public abstract class ProcessorBase : ReactiveObject, IDisposable, IProcessor
     {
          #region Private Fields

          /// <summary>
          /// Stores the display collection for any debug output.
          /// </summary>
          private DisplayCollection debugDisplay = new DisplayCollection();

          /// <summary>
          /// Stores the CompositeDisposable that holds all subscription disposables.
          /// </summary>
          private System.Reactive.Disposables.CompositeDisposable disposeCollection =
              new System.Reactive.Disposables.CompositeDisposable();

          /// <summary>
          /// Stores the error code.
          /// </summary>
          private ProcessingErrorCode errorCode = ProcessingErrorCode.NoError;

          /// <summary>
          /// Stores the error message.
          /// </summary>
          private string errorMessage = "No Errors";

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          #endregion Private Fields

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the ProcessorBase class.
          /// </summary>
          public ProcessorBase()
          {
          }

          #endregion Public Constructors

          #region Private Destructors

          /// <summary>
          /// Finalizes an instance of the ProcessorBase class.
          /// </summary>
          ~ProcessorBase() => this.Dispose(false);

          #endregion Private Destructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the display collection for any debug output.
          /// </summary>
          /// <value>The debug display.</value>
          public DisplayCollection DebugDisplay
          {
               get => this.debugDisplay;

               set => this.RaiseAndSetIfChanged(ref this.debugDisplay, value);
          }

          /// <summary>
          /// Gets or sets the CompositeDisposable object.
          /// </summary>
          /// <value>The dispose collection.</value>
          public System.Reactive.Disposables.CompositeDisposable DisposeCollection
          {
               get => this.disposeCollection;

               set => this.disposeCollection = value;
          }

          /// <summary>
          /// Gets or sets the error code.
          /// </summary>
          /// <value>The error code.</value>
          public ProcessingErrorCode ErrorCode
          {
               get => this.errorCode;

               set => this.RaiseAndSetIfChanged(ref this.errorCode, value);
          }

          /// <summary>
          /// Gets or sets the error message.
          /// </summary>
          /// <value>The error message.</value>
          public string ErrorMessage
          {
               get => this.errorMessage;

               set => this.RaiseAndSetIfChanged(ref this.errorMessage, value);
          }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Implements the Dispose method of IDisposable.
          /// </summary>
          public void Dispose()
          {
               this.Dispose(true);
               GC.SuppressFinalize(this);
          }

          /// <summary>
          /// Performs the process for this class.
          /// </summary>
          /// <returns>A structure containing the processing results and error information.</returns>
          public virtual ProcessingResult Process()
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
          /// Performs the process for this class.
          /// </summary>
          /// <param name="parameters">A non-generic Tuple containing any parameters.</param>
          /// <returns>A structure containing the processing results and error information.</returns>
          public virtual ProcessingResult Process(object parameters)
          {
               this.ErrorCode = ProcessingErrorCode.NoError;
               this.ErrorMessage = "No errors detected.";
               ProcessingResult result = new ProcessingResult();

               // In overrides, change this to the types being passes and parse them.
               if ((Tuple<object>)parameters != null)
               {
                    object obj = ((Tuple<object>)parameters).Item1;
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

          #endregion Public Methods

          #region Protected Methods

          /// <summary>
          /// Implements the Dispose method of IDisposable that actually disposes of managed resources.
          /// </summary>
          /// <param name="disposing">A boolean value indicating whether the class is being disposed.</param>
          protected virtual void Dispose(bool disposing)
          {
               if (!this.isDisposed)
               {
                    if (disposing)
                    {
                         // Code to dispose the managed resources held by the class
                         this.disposeCollection?.Dispose();
                    }

                    this.debugDisplay?.Dispose();
               }

               this.isDisposed = true;
          }

          #endregion Protected Methods
     }
}