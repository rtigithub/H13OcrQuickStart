//-----------------------------------------------------------------------
// <copyright file="OcrModule.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart
{
     using System;
     using System.Linq;
     using System.Reactive.Linq;
     using ReactiveUI;
     using Rti.ViewROIManager;

     /// <summary>
     /// Main window partial with added code for the Ocr module. 
     /// </summary>
     public sealed partial class MainWindow
     {
          /// <summary>
          /// Initializes the new module.
          /// </summary>
          /// <param name="manager">The ViewROIManager instance to use.</param>
          private void InitializeOcrModule(ViewROIManager manager)
          {
               this.BindOcrControls(manager);
          }

          /// <summary>
          /// Sets up the reactive bindings for all Ocr controls. 
          /// </summary>
          /// <param name="manager">The ViewROIManager instance to use.</param>
          private void BindOcrControls(ViewROIManager manager)
          {
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.Display)
                    .Where(x => x.DisplayList.Count > 0)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => manager.ShowDisplayCollection(x)));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.DebugDisplay)
                    .Where(x => x.DisplayList.Count > 0)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => manager.ShowDisplayCollection(x)));
          }
     }
}
