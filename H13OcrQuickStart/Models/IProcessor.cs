// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="IProcessor.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.Models
{
     using Rti.DisplayUtilities;

     /// <summary>
     /// Interface for the processor classes.
     /// </summary>
     public interface IProcessor
     {
          #region Public Properties

          /// <summary>
          /// Gets or sets the debug DisplayCollection.
          /// </summary>
          /// <value>The debug display.</value>
          DisplayCollection DebugDisplay
          {
               get;
               set;
          }

          /// <summary>
          /// Gets or sets the CompositeDisposable object.
          /// </summary>
          /// <value>The dispose collection.</value>
          System.Reactive.Disposables.CompositeDisposable DisposeCollection
          {
               get;
               set;
          }

          /// <summary>
          /// Gets or sets the error code.
          /// </summary>
          /// <value>The error code.</value>
          ProcessingErrorCode ErrorCode
          {
               get;
               set;
          }

          /// <summary>
          /// Gets or sets the error message.
          /// </summary>
          /// <value>The error message.</value>
          string ErrorMessage
          {
               get;
               set;
          }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Implements the Dispose method of IDisposable.
          /// </summary>
          void Dispose();

          /// <summary>
          /// Performs the process for this class.
          /// </summary>
          /// <returns>A structure containing the processing results and error information.</returns>
          ProcessingResult Process();

          #endregion Public Methods
     }
}