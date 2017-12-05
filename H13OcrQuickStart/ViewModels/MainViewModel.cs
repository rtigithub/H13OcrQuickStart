// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="MainViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.ViewModels
{
     using System;
     using System.Data;
     using System.IO;
     using System.Reactive;
     using System.Reactive.Linq;
     using Models;
     using ReactiveUI;

     /// <summary>
     /// Main view model that contains all the others and provides interconnectivity.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.ViewModels.MainViewModelBase" />
     public partial class MainViewModel : MainViewModelBase
     {
          #region Private Fields

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// The child view model for loading images.
          /// </summary>
          private LoadProcessViewModel loadImageVM;

          /// <summary>
          /// Stores an instance of the SettingsManager class.
          /// </summary>
          private Utilities.SettingsManager settingsVM;

          #endregion Private Fields

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the MainViewModel class.
          /// </summary>
          public MainViewModel()
          {
               this.SetUpDataSet();

               this.loadImageVM = new LoadProcessViewModel(this, new LoadImageProcessor());

               //// Call the initializer for any new view models.
               //// Example: this.InitializeProcessorViewModel();
               this.InitializeAcquireAcquisitionViewModel();
               this.InitializeOcrViewModel();

               // Instantiate SettingsManager, settings.
               if (File.Exists(Utilities.SettingsManager.Location))
               {
                    this.settingsVM = Utilities.SettingsManager.LoadSettings();
                    this.settingsVM.MainViewModelRef = this;
               }
               else
               {
                    // With no settings file, the class is initialized with default settings.
                    this.settingsVM = new Utilities.SettingsManager(this);
               }

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.LoadImageVM.ProcessingResults)
                    .Where(x => x != null)
                    .Select(e => e.ErrorMessage)
                    .Subscribe(e => this.StatusText = e));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.AppState)
                   .Where(x => x != 0)
                   .StartWith(1)
                   .Subscribe(x => this.LastAppState = x));
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the last application state.
          /// </summary>
          /// <value>The last state of the application.</value>
          public int LastAppState
          {
               get;

               set;
          }

          /// <summary>
          /// Gets the loadImageVM.
          /// </summary>
          /// <value>The load image vm.</value>
          public LoadProcessViewModel LoadImageVM => this.loadImageVM;

          /// <summary>
          /// Gets the settingsVM.
          /// </summary>
          /// <value>The settings vm.</value>
          public Utilities.SettingsManager SettingsVM => this.settingsVM;

          #endregion Public Properties

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

                         this.settingsVM?.Dispose();

                         this.loadImageVM?.Dispose();

                         this.acquisitionAcquireVM?.Dispose();

                         this.calibrationAcquireVM.Dispose();

                         this.processorOcrVM?.Dispose();
                    }

                    //// Dispose of unmanaged resources here.

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          /// <summary>
          /// Resets the MenuItems according to the state of the application.
          /// </summary>
          protected override void SetMenuItems()
          {
               this.MenuItems.Clear();

               // Add menu items with display text and a reactive command to execute for the
               // appropriate AppState. Command must be of type ReactiveCommand<Unit, ProcessingResult>.
               ////this.MenuItems.Add(new MenuItemVM("Menu Text.", this.SomeProcessorViewModel.Command));

               switch (this.AppState)
               {
                    case 0:
                         break;

                    case 1:
                         break;
               }
          }

          /// <summary>
          /// Sets up the data set for output.
          /// </summary>
          protected sealed override void SetUpDataSet()
          {
               this.ProcessingResultsDataSet = new DataSet();

               DataTable resultsDataTable = new DataTable();

               //// Add fields here.
               //// Template: resultsDataTable.Columns.Add("My Column", Type.GetType("System.<the type>"));
               resultsDataTable.Columns.Add("OCR Results", Type.GetType("System.String"));

               this.ProcessingResultsDataSet.Tables.Add(resultsDataTable);
          }

          #endregion Protected Methods
     }
}