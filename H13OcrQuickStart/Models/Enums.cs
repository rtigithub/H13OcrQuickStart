﻿// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="Enums.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName",
    Justification = "Allow the name of the file to be used.")]

namespace H13OcrQuickStart.Models
{
     /// <summary>
     /// Enumeration of error codes.
     /// </summary>
     public enum ProcessingErrorCode
     {
          /// <summary>
          /// No error occurred.
          /// </summary>
          NoError,

          /// <summary>
          /// Example error code. To be changed or removed.
          /// </summary>
          ProcessingStep1Error,

          /// <summary>
          /// Example error code. To be changed or removed.
          /// </summary>
          ProcessingStep2Error,

          /// <summary>
          /// An undefined error occurred.
          /// </summary>
          UndefinedError,

          /// <summary>
          /// Wrong Tuple types supplied in parameters.
          /// </summary>
          ParameterError,

          /// <summary>
          /// An error occurred while acquiring an image.
          /// </summary>
          AcquisitionError,

          /// <summary>
          /// An error occurred while loading an image.
          /// </summary>
          LoadImageError,

          /// <summary>
          /// An error occurred while calibrating the camera.
          /// </summary>
          CalibrationError,

          /// <summary>
          /// An error occurred while initializing the camera.
          /// </summary>
          InitializingCameraError,

          /// <summary>
          /// The HALCON library threw an exception.
          /// </summary>
          HalconException
     }

     /// <summary>
     /// Enumeration of radio button options.
     /// </summary>
     public enum RadioButtonSelection
     {
          /// <summary>
          /// No selection. This is used to toggle the selection so that a reset can be done
          /// regardless of the current setting.
          /// </summary>
          None
     }
}