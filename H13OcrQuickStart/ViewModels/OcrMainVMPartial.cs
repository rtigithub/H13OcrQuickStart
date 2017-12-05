// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="OcrMainVMPartial.cs" company="Resolution Technology, Inc.">
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
     /// Main view model partial with added code for the processor module.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.ViewModels.MainViewModelBase" />
     public partial class MainViewModel
     {
          #region Private Fields

          /// <summary>
          /// Stores the instance of the Processor View Model.
          /// </summary>
          private OcrViewModel processorOcrVM;

          #endregion Private Fields

          #region Public Properties

          /// <summary>
          /// Gets the instance of the Ocr View Model.
          /// </summary>
          /// <value>The ocr vm.</value>
          public OcrViewModel OcrVM => this.processorOcrVM;

          #endregion Public Properties

          #region Private Methods

          /// <summary>
          /// Initializes the Ocr View Model.
          /// </summary>
          private void InitializeOcrViewModel()
          {
               this.processorOcrVM = new OcrViewModel(this, new OcrProcessor());

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.OcrVM.ProcessingResults)
                    .Where(x => x != null)
                    .Select(e => e.ErrorMessage)
                    .Subscribe(e => this.StatusText = e));

               //// Add any desired result to the data set for UI display or use.
               //// Template:
               ////this.DisposeCollection.Add(
               ////    this.WhenAnyValue(x => x.OcrVM.ProcessingResults)
               ////    .Where(x => x != null)
               ////    .Where(x => x.ResultsCollection.ContainsKey("MyNamedResultValue"))
               ////    .Subscribe(x => this.ProcessingResultsDataSet.Tables[0].Rows.Add(x.ResultsCollection["MyNamedResultValue"])));

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.OcrVM.ProcessingResults)
                   .Where(x => x != null)
                   .Where(x => x.ResultsCollection.ContainsKey("OcrResults"))
                   .Subscribe(x =>
                   {
                        var result = x.ResultsCollection["OcrResults"];
                        this.ProcessingResultsDataSet.Tables[0].Rows.Add(result);
                   }
                   ));
          }

          #endregion Private Methods
     }
}