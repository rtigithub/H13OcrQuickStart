// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="AcquireAcquisitionMainVMPartial.cs" company="Resolution Technology, Inc.">
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
     /// Main view model partial with added code for the acquisition module and stubs for the camera calibration module.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.ViewModels.MainViewModelBase" />
     public partial class MainViewModel
     {
          #region Private Fields

          /// <summary>
          /// Stores the instance of the AcquireAcquisitionViewModel.
          /// </summary>
          private FirstAcquisitionViewModel acquisitionAcquireVM;

          #endregion Private Fields

          #region Public Properties

          /// <summary>
          /// Gets the acquisitionAcquireVM.
          /// </summary>
          /// <value>The acquire acquisition vm.</value>
          public FirstAcquisitionViewModel AcquireAcquisitionVM => this.acquisitionAcquireVM;

          #endregion Public Properties

          #region Private Methods

          /// <summary>
          /// Initializes the AcquireAcquisitionViewModel instance.
          /// </summary>
          private void InitializeAcquireAcquisitionViewModel()
          {
               this.acquisitionAcquireVM = new FirstAcquisitionViewModel(this, new FirstAcquisitionProcessor());

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.AcquireAcquisitionVM.ProcessingResults)
                    .Where(x => x != null)
                    .Select(e => e.ErrorMessage)
                    .Subscribe(e => this.StatusText = e));

               //// Add any desired result to the data set for UI display or use.
               //// Template:
               ////this.DisposeCollection.Add(
               ////    this.WhenAnyValue(x => x.AcquireAcquisitionVM.ProcessingResults)
               ////    .Where(x => x != null)
               ////    .Where(x => x.ResultsCollection.ContainsKey("MyNamedResultValue"))
               ////    .Subscribe(x => this.ProcessingResultsDataSet.Tables[0].Rows.Add(x.ResultsCollection["MyNamedResultValue"])));

               this.InitializeAcquireCalibrationViewModel();
          }

          #endregion Private Methods
     }
}