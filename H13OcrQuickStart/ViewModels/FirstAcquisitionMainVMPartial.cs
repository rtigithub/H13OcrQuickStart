//-----------------------------------------------------------------------
// <copyright file="AcquireAcquisitionMainVMPartial.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
     public partial class MainViewModel
     {
          /// <summary>
          /// Stores the instance of the AcquireAcquisitionViewModel.
          /// </summary>
          private FirstAcquisitionViewModel acquisitionAcquireVM;

          /// <summary>
          /// Gets the acquisitionAcquireVM. 
          /// </summary>
          public FirstAcquisitionViewModel AcquireAcquisitionVM => this.acquisitionAcquireVM;

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
     }
}
