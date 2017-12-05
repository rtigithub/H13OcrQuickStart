// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="IProcessingResult.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.Models
{
     using System.Collections.Generic;

     /// <summary>
     /// Interface for ProcessingResult
     /// </summary>
     public interface IProcessingResult
     {
          #region Public Properties

          /// <summary>
          /// Gets or sets the error message.
          /// </summary>
          /// <value>The error message.</value>
          string ErrorMessage
          {
               get;
               set;
          }

          /// <summary>
          /// Gets or sets a dictionary of results.
          /// </summary>
          /// <value>The results collection.</value>
          Dictionary<string, object> ResultsCollection
          {
               get;
               set;
          }

          /// <summary>
          /// Gets or sets the error code.
          /// </summary>
          /// <value>The status code.</value>
          ProcessingErrorCode StatusCode
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

          #endregion Public Methods
     }
}