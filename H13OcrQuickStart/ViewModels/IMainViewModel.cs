// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="IMainViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.ViewModels
{
     using System;

     /// <summary>
     /// Interface for the main view model classes.
     /// </summary>
     public interface IMainViewModel
     {
          #region Public Properties

          /// <summary>
          /// Gets or sets the app state.
          /// </summary>
          /// <value>The state of the application.</value>
          int AppState { get; set; }

          /// <summary>
          /// Gets the CompositeDisposable object.
          /// </summary>
          /// <value>The dispose collection.</value>
          System.Reactive.Disposables.CompositeDisposable DisposeCollection { get; }

          /// <summary>
          /// Gets or sets the reactive list of Menu Item view models.
          /// </summary>
          /// <value>The menu items.</value>
          ReactiveUI.ReactiveList<MenuItemVM> MenuItems { get; set; }

          /// <summary>
          /// Gets or sets the DataSet that stores the processing result.
          /// </summary>
          /// <value>The processing results data set.</value>
          System.Data.DataSet ProcessingResultsDataSet { get; set; }

          /// <summary>
          /// Gets or sets the status text.
          /// </summary>
          /// <value>The status text.</value>
          string StatusText { get; set; }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Implements the Dispose method of IDisposable.
          /// </summary>
          void Dispose();

          #endregion Public Methods
     }
}