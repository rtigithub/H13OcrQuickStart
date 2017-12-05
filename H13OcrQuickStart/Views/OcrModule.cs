// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="OcrModule.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
     /// <seealso cref="System.Windows.Window" />
     /// <seealso cref="ReactiveUI.IViewFor{H13OcrQuickStart.ViewModels.MainViewModel}" />
     /// <seealso cref="System.IDisposable" />
     /// <seealso cref="System.Windows.Markup.IComponentConnector" />
     public sealed partial class MainWindow
     {
          #region Private Methods

          /// <summary>
          /// Sets up the reactive bindings for all Ocr controls.
          /// </summary>
          /// <param name="manager">The ViewROIManager instance to use.</param>
          private void BindOcrControls(ViewROIManager manager)
          {
               // Bind image aspect for acquired images.
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.ImageHeight)
                  .Subscribe(x => manager.ImageHeight = x));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.ImageWidth)
                   .Subscribe(x => manager.ImageWidth = x));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.Display)
                    .Where(x => x.DisplayList.Count > 0)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => manager.ShowDisplayCollection(x)));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.OcrVM.DebugDisplay)
                    .Where(x => x.DisplayList.Count > 0)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => manager.ShowDisplayCollection(x)));
          }

          /// <summary>
          /// Initializes the new module.
          /// </summary>
          /// <param name="manager">The ViewROIManager instance to use.</param>
          private void InitializeOcrModule(ViewROIManager manager)
          {
               this.BindOcrControls(manager);
          }

          #endregion Private Methods
     }
}