// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="AcquireCalibrationMainVMPartial.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.ViewModels
{
     using System;
     using System.Data;
     using System.Reactive.Linq;
     using Models;
     using ReactiveUI;

     /// <summary>
     /// Main view model partial with added code for the camera calibration module.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.ViewModels.MainViewModelBase" />
     public partial class MainViewModel
     {
          #region Private Fields

          /// <summary>
          /// Stores the AcquireCalibrationViewModel instance.
          /// </summary>
          private FirstCalibrationViewModel calibrationAcquireVM;

          #endregion Private Fields

          #region Public Properties

          /// <summary>
          /// Gets the acquire calibration vm.
          /// </summary>
          /// <value>The acquire calibration vm.</value>
          public FirstCalibrationViewModel AcquireCalibrationVM => this.calibrationAcquireVM;

          #endregion Public Properties

          #region Private Methods

          /// <summary>
          /// Initializes the AcquireCalibrationViewModel instance.
          /// </summary>
          private void InitializeAcquireCalibrationViewModel()
          {
               this.calibrationAcquireVM = new FirstCalibrationViewModel(this, new FirstCalibrationProcessor());

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.AcquireCalibrationVM.ProcessingResults)
                    .Where(x => x != null)
                    .Select(e => e.ErrorMessage)
                    .Subscribe(e => this.StatusText = e));

               //// Add any desired result to the data set for UI display or use.
               //// Template:
               ////this.DisposeCollection.Add(
               ////    this.WhenAnyValue(x => x.AcquireCalibrationVM.ProcessingResults)
               ////    .Where(x => x != null)
               ////    .Where(x => x.ResultsCollection.ContainsKey("MyNamedResultValue"))
               ////    .Subscribe(x => this.ProcessingResultsDataSet.Tables[0].Rows.Add(x.ResultsCollection["MyNamedResultValue"])));

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.AcquireCalibrationVM.ProcessingResultsCalibrate)
                   .Where(x => x != null)
                   .Where(x => x.ResultsCollection.ContainsKey("CalibrationError"))
                   .Subscribe(x => this.StatusText = "Calibration succeeded. Calibration Error = " + ((double)x.ResultsCollection["CalibrationError"]).ToString()));
          }

          #endregion Private Methods
     }
}