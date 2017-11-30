//-----------------------------------------------------------------------
// <copyright file="AcquireCalibrationModule.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart
{
     using System;
     using System.Linq;
     using System.Reactive.Linq;
     using System.Windows;
     using System.Windows.Controls.Ribbon;
     using ReactiveUI;
     using Rti.ViewROIManager;

     /// <summary>
     /// Main window partial with added code for the Acquirecalibration module.
     /// </summary>
     public sealed partial class MainWindow
     {
          #region Private Fields

          /// <summary>
          /// Stores the camera calibration page.
          /// </summary>
          private View.AcquireCalibrationPage calibrationAcquirePage = new View.AcquireCalibrationPage();

          #endregion Private Fields

          #region Private Methods

          /// <summary>
          /// Handles the click event for the Acquire calibration ribbon item.
          /// </summary>
          /// <param name="sender">The calling object.</param>
          /// <param name="e">The Routed Event Arguments</param>
          private void AcquireCalibrationRibbonItem_Click(object sender, RoutedEventArgs e)
          {
               this.Frame1.Visibility = Visibility.Visible;
               this.Frame1.Content = this.calibrationAcquirePage;
          }

          /// <summary>
          /// Sets up the reactive bindings for all Acquire camera calibration controls.
          /// </summary>
          private void BindAcquireCalibrationControls(ViewROIManager manager)
          {
               string RegexFloat = @"^-?\d+\.?\d*$";
               string RegexInteger = @"^-?\d+$";

               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.SetParametersCommand, x => x.calibrationAcquirePage.ButtonSetInitialParameters);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.AcquireCalibrationImagesCommand, x => x.calibrationAcquirePage.ButtonAcquireCalibrationImages);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.CalibrateCommand, x => x.calibrationAcquirePage.ButtonCalibrate);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.RectifyImageCommand, x => x.calibrationAcquirePage.ButtonRectifyTestImage);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.LoadCalibrationMapCommand, x => x.calibrationAcquirePage.ButtonLoadCalibrationMap);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.LoadTestCalibrationImageCommand, x => x.calibrationAcquirePage.ButtonLoadTestCalibrationImage);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.SaveRectifiedImageCommand, x => x.calibrationAcquirePage.ButtonSaveRectifiedTestImage);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.LoadCalibImagesFromFileCommand, x => x.calibrationAcquirePage.ButtonLoadCalibrationImages);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireCalibrationVM.SaveCalibrationMapCommand, x => x.calibrationAcquirePage.ButtonSaveCalibrationMap);

               // ButtonLoadCalibrationImages enabled bound to LoadCalibImagesFromFileCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.LoadCalibImagesFromFileCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.ButtonLoadCalibrationImages.IsEnabled = x));

               // ButtonAcquireCalibrationImages enabled bound to LoadCalibImagesFromFileCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.LoadCalibImagesFromFileCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.ButtonAcquireCalibrationImages.IsEnabled = x));

               // ButtonResetCalibrationImages enabled bound to LoadCalibImagesFromFileCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.LoadCalibImagesFromFileCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.ButtonResetCalibrationImages.IsEnabled = x));

               // ButtonSaveCalibrationMap enabled bound to SaveCalibrationCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.SaveCalibrationMapCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.ButtonSaveCalibrationMap.IsEnabled = x));

               // ButtonLoadTestCalibrationImage enabled bound to LoadTestImageCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.LoadTestCalibrationImageCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.ButtonLoadTestCalibrationImage.IsEnabled = x));

               // ButtonSaveRectifiedTestImage enabled bound to SaveRectifyImageCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.SaveRectifiedImageCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x =>
                   {
                        this.calibrationAcquirePage.ButtonSaveRectifiedTestImage.IsEnabled = x;
                   }));

               // CheckBoxCorrectImages enabled bound to LoadTestImageCanExecute observable.
               this.disposeCollection.Add(this.WhenAnyObservable(x => x.MainViewModel.AcquireCalibrationVM.LoadTestCalibrationImageCanExecute)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => this.calibrationAcquirePage.CheckBoxCorrectImages.IsEnabled = x));

               this.Bind(this.MainViewModel, vm => vm.AcquireCalibrationVM.CorrectNewImages, x => x.calibrationAcquirePage.CheckBoxCorrectImages.IsChecked);

               // Set cursor to wait while calibration processes are busy.  Does not affect HalconWindow. ??
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.IsBusy)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x =>
                   {
                        if (x)
                        {
                             System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        }
                        else
                        {
                             System.Windows.Input.Mouse.OverrideCursor = null;
                        }
                   }));

               // ComboBoxFocalLength
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.FocalLengthParameters)
                  .Subscribe(_ => this.calibrationAcquirePage.ComboBoxFocalLength.ItemsSource = this.MainViewModel.AcquireCalibrationVM.FocalLengthParameters.ToDArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxFocalLength.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.FocalLength));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxFocalLength.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexFloat) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.FocalLength));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.FocalLength)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxFocalLength.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxFocalLength.Items.IndexOf(MainViewModel.AcquireCalibrationVM.FocalLength)));

               // ComboBoxSensorSizeX
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.SensorSizeXParameters)
                  .Subscribe(_ => this.calibrationAcquirePage.ComboBoxSensorSizeX.ItemsSource = this.MainViewModel.AcquireCalibrationVM.SensorSizeXParameters.ToDArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxSensorSizeX.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.SensorSizeX));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxSensorSizeX.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexFloat) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.SensorSizeX));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.SensorSizeX)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxSensorSizeX.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxSensorSizeX.Items.IndexOf(MainViewModel.AcquireCalibrationVM.SensorSizeX)));

               // ComboBoxSensorSizeY
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.SensorSizeYParameters)
                  .Subscribe(_ => this.calibrationAcquirePage.ComboBoxSensorSizeY.ItemsSource = this.MainViewModel.AcquireCalibrationVM.SensorSizeYParameters.ToDArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxSensorSizeY.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.SensorSizeY));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxSensorSizeY.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexFloat) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.SensorSizeY));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.SensorSizeY)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxSensorSizeY.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxSensorSizeY.Items.IndexOf(MainViewModel.AcquireCalibrationVM.SensorSizeY)));

               // ComboBoxCalibrationImageWidth
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.ImageWidthParameters)
                  .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationImageWidth.ItemsSource = this.MainViewModel.AcquireCalibrationVM.ImageWidthParameters.ToIArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationImageWidth.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.ImageWidth));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationImageWidth.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexInteger) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.ImageWidth));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.ImageWidth)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationImageWidth.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxCalibrationImageWidth.Items.IndexOf(MainViewModel.AcquireCalibrationVM.ImageWidth)));

               // ComboBoxCalibrationImageHeight
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.ImageHeightParameters)
                  .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationImageHeight.ItemsSource = this.MainViewModel.AcquireCalibrationVM.ImageHeightParameters.ToIArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationImageHeight.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.ImageHeight));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationImageHeight.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexInteger) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.ImageHeight));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.ImageHeight)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationImageHeight.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxCalibrationImageHeight.Items.IndexOf(MainViewModel.AcquireCalibrationVM.ImageHeight)));

               // ComboBoxCalibrationCameraType
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.CameraTypeParameters)
                 .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationCameraType.ItemsSource = this.MainViewModel.AcquireCalibrationVM.CameraTypeParameters.ToSArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationCameraType.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.CameraType));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationCameraType.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.CameraType));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.CameraType)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationCameraType.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxCalibrationCameraType.Items.IndexOf(MainViewModel.AcquireCalibrationVM.CameraType)));

               // ComboBoxCalibrationPlateName
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.HalconCalibrationPlateParameters)
                 .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationPlateName.ItemsSource = this.MainViewModel.AcquireCalibrationVM.HalconCalibrationPlateParameters.ToSArr()));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationPlateName.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.HalconCalibrationPlateName));

               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.ComboBoxCalibrationPlateName.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.HalconCalibrationPlateName));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.HalconCalibrationPlateName)
                   .Subscribe(_ => this.calibrationAcquirePage.ComboBoxCalibrationPlateName.SelectedIndex =
                       this.calibrationAcquirePage.ComboBoxCalibrationPlateName.Items.IndexOf(MainViewModel.AcquireCalibrationVM.HalconCalibrationPlateName)));

               // TextBoxRectifiedImageWidth
               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.TextBoxRectifiedImageWidth.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexInteger) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.RectifiedImageWidth));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.RectifiedImageWidth)
                   .Subscribe(x => this.calibrationAcquirePage.TextBoxRectifiedImageWidth.Text = this.MainViewModel.AcquireCalibrationVM.RectifiedImageWidth.ToString()));

               // TextBoxRectifiedImageHeight
               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.TextBoxRectifiedImageHeight.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexInteger) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.RectifiedImageHeight));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.RectifiedImageHeight)
                   .Subscribe(x => this.calibrationAcquirePage.TextBoxRectifiedImageHeight.Text = this.MainViewModel.AcquireCalibrationVM.RectifiedImageHeight.ToString()));

               // TextBoxCalibratedScale
               this.disposeCollection.Add(this.calibrationAcquirePage.WhenAnyValue(x => x.TextBoxCalibratedScale.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, RegexFloat) == true)
                   .BindTo(this.MainViewModel.AcquireCalibrationVM, vm => vm.CalibratedScale));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.CalibratedScale)
                   .Subscribe(x => this.calibrationAcquirePage.TextBoxCalibratedScale.Text = this.MainViewModel.AcquireCalibrationVM.CalibratedScale.ToString("0.0######")));

               // IntegerUpDownWorldPoseIndex
               this.Bind(this.MainViewModel, x => x.AcquireCalibrationVM.WorldPoseIndex, x => x.calibrationAcquirePage.IntegerUpDownWorldPoseIndex.Value);

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.Display)
                   .Where(x => x.DisplayList.Count > 0)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => manager.ShowDisplayCollection(x)));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireCalibrationVM.DebugDisplay)
                   .Where(x => x.DisplayList.Count > 0)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => manager.ShowDisplayCollection(x)));
          }

          /// <summary>
          /// Initializes the Acquirecalibration module.
          /// </summary>
          private void InitializeAcquireCalibrationModule(ViewROIManager manager)
          {
               this.MainViewModel.AcquireCalibrationVM.GetFileName.RegisterHandler(
                     async interaction =>
                     {
                          string filename = await this.utilities.GetFileName();
                          interaction.SetOutput(filename);
                     });

               this.MainViewModel.AcquireCalibrationVM.GetSaveFileName.RegisterHandler(
                    async interaction =>
                    {
                         string filename = await this.utilities.GetFileNameToSave();
                         interaction.SetOutput(filename);
                    });

               this.MainViewModel.AcquireCalibrationVM.GetCalibratioinMapSaveFileName.RegisterHandler(
                    async interaction =>
                    {
                         string filename = await this.utilities.GetCalibrationMapFileNameToSave();
                         interaction.SetOutput(filename);
                    });

               this.MainViewModel.AcquireCalibrationVM.GetFolderName.RegisterHandler(
                    async interaction =>
                    {
                         string foldername = await this.utilities.GetFolder();
                         interaction.SetOutput(foldername);
                    });

               RibbonMenuItem cameraCalibrationMenuItem = new RibbonMenuItem()
               {
                    Name = "AcquireCalibrationItem",
                    Header = "Acquire Calibration"
               };

               cameraCalibrationMenuItem.Click += this.AcquireCalibrationRibbonItem_Click;
               this.UtilitiesTab.Items.Add(cameraCalibrationMenuItem);

               this.BindAcquireCalibrationControls(manager);
          }

          #endregion Private Methods
     }
}