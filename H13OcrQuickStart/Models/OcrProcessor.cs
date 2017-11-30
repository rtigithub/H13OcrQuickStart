//-----------------------------------------------------------------------
// <copyright file="OcrProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.Models
{
     using System;
     using System.Reactive.Linq;
     using HalconDotNet;
     using ReactiveUI;
     using Rti.DisplayUtilities;

     /// <summary>
     /// Model class for a new process. 
     /// </summary>
     public class OcrProcessor : ProcessorBase
     {
          #region Private Declarations

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          //// Create backing fields for the properties as needed.

          #endregion Private Declarations

          #region Constructors

          /// <summary>
          /// Initializes a new instance of the Processor class.
          /// </summary>
          public OcrProcessor()
              : base()
          {
          }

          #endregion Constructors

          #region Private Destructors

          #endregion Private Destructors

          #region Public Properties

          //// Create properties for objects and display objects set in the process methods. Not including output results.

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

          #endregion Public Methods

          #region internal methods

          #endregion internal methods

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

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region private methods

          #endregion private methods
     }
}
