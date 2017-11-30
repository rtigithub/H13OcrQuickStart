//-----------------------------------------------------------------------
// <copyright file="AcquireAcquisitionModule.cs" company="Resolution Technology, Inc.">
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
     /// Main window partial with added code for the Acquire acquisition module.
     /// </summary>
     public sealed partial class MainWindow
     {
          #region Private Fields

          /// <summary>
          /// Stores the Acquire acquisition page.
          /// </summary>
          private View.AcquireAcquisitionPage acquisitionAcquirePage = new View.AcquireAcquisitionPage();

          #endregion Private Fields

          #region Private Methods

          /// <summary>
          /// Handles the click event for the Acquire acquisition ribbon item.
          /// </summary>
          /// <param name="sender">The calling object.</param>
          /// <param name="e">The Routed Event Arguments</param>
          private void AcquireAcquisitionRibbonItem_Click(object sender, RoutedEventArgs e)
          {
               this.Frame1.Visibility = Visibility.Visible;
               this.Frame1.Content = this.acquisitionAcquirePage;
          }

          /// <summary>
          /// Sets up the reactive bindings for all Acquire Acquisition controls.
          /// </summary>
          private void BindAcquireAcquisitionControls(ViewROIManager manager)
          {
               // buttonLiveVideo
               this.BindCommand(this.MainViewModel, vm => vm.AcquireAcquisitionVM.LiveVideoCommand, x => x.acquisitionAcquirePage.buttonLiveVideo);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireAcquisitionVM.SelectFileCommand, x => x.acquisitionAcquirePage.buttonSelectFile);
               this.BindCommand(this.MainViewModel, vm => vm.AcquireAcquisitionVM.SaveImageCommand, x => x.acquisitionAcquirePage.buttonSave);

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.LiveVideoMode)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x =>
                   {
                        if (x)
                        {
                             this.acquisitionAcquirePage.buttonLiveVideo.Content = "Stop Live Video";
                        }
                        else
                        {
                             this.acquisitionAcquirePage.buttonLiveVideo.Content = "Start Live Video";
                        }
                   }));

               // buttonInitializeCamera
               this.BindCommand(this.MainViewModel, vm => vm.AcquireAcquisitionVM.InitializeCommand, x => x.acquisitionAcquirePage.buttonInitializeCamera);

               // buttonAcquire
               this.BindCommand(this.MainViewModel, vm => vm.AcquireAcquisitionVM.Command, x => x.acquisitionAcquirePage.buttonAcquire);

               // Bind the buttonSelectFile visibility to SelectFileVisibility property.
               this.OneWayBind(this.MainViewModel, vm => vm.AcquireAcquisitionVM.SelectFileVisibility, y => y.acquisitionAcquirePage.buttonSelectFile.Visibility);

               // Bind image aspect for acquired images.
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.AcquiredImageHeight)
                  .Subscribe(x => manager.ImageHeight = x));
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.AcquiredImageWidth)
                   .Subscribe(x => manager.ImageWidth = x));

               // Display collection.
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.Display)
                   .Where(x => x.DisplayList.Count > 0)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x =>
                   {
                        try
                        {
                             manager.ShowDisplayCollection(x);
                        }
                        finally
                        {
                             this.MainViewModel.AcquireAcquisitionVM.IsProcessing = false;
                        }
                   }));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.DebugDisplay)
                   .Where(x => x.DisplayList.Count > 0)
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(x => manager.ShowDisplayCollection(x)));

               // comboBoxAcquisitionInterfaceName
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.AcquisitionInterfaces)
                  .Subscribe(_ => this.acquisitionAcquirePage.comboBoxAcquisitionInterfaceName.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.AcquisitionInterfaces.ToList()));
               this.acquisitionAcquirePage.comboBoxAcquisitionInterfaceName.SelectedIndex = 1;

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxAcquisitionInterfaceName.SelectedItem)
                       .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.CurrentAcquisitionInterfaceName));

               // comboBoxHorizontalResolution
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.HorizontalResolutionParameters)
                  .Subscribe(_ => this.acquisitionAcquirePage.comboBoxHorizontalResolution.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.HorizontalResolutionParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxHorizontalResolution.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.HorizontalResolution));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxHorizontalResolution.Text)
                   .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.HorizontalResolution));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.HorizontalResolution)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxHorizontalResolution.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxHorizontalResolution.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.HorizontalResolution)));

               // comboBoxVerticalResolution
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.VerticalResolutionParameters)
                  .Subscribe(_ => this.acquisitionAcquirePage.comboBoxVerticalResolution.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.VerticalResolutionParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxVerticalResolution.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.VerticalResolution));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxVerticalResolution.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.VerticalResolution));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.VerticalResolution)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxVerticalResolution.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxVerticalResolution.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.VerticalResolution)));

               // comboBoxImageWidth
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ImageWidthParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxImageWidth.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.ImageWidthParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxImageWidth.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ImageWidth));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxImageWidth.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ImageWidth));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ImageWidth)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxImageWidth.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxImageWidth.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.ImageWidth)));

               // comboBoxImageHeight
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ImageHeightParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxImageHeight.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.ImageHeightParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxImageHeight.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ImageHeight));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxImageHeight.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ImageHeight));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ImageHeight)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxImageHeight.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxImageHeight.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.ImageHeight)));

               // comboBoxStartRow
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.StartRowParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxStartRow.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.StartRowParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxStartRow.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.StartRow));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxStartRow.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.StartRow));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.StartRow)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxStartRow.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxStartRow.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.StartRow)));

               // comboBoxStartColumn
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.StartColumnParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxStartColumn.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.StartColumnParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxStartColumn.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.StartColumn));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxStartColumn.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.StartColumn));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.StartColumn)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxStartColumn.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxStartColumn.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.StartColumn)));

               // comboBoxField
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.FieldParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxField.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.FieldParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxField.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Field));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxField.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Field));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.Field)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxField.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxField.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.Field)));

               // comboBitsPerChannel
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.BitsPerChannelParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBitsPerChannel.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.BitsPerChannelParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBitsPerChannel.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.BitsPerChannel));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBitsPerChannel.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.BitsPerChannel));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.BitsPerChannel)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBitsPerChannel.SelectedIndex =
                       this.acquisitionAcquirePage.comboBitsPerChannel.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.BitsPerChannel)));

               // comboBoxColorSpace
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ColorSpaceParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxColorSpace.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.ColorSpaceParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxColorSpace.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ColorSpace));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxColorSpace.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ColorSpace));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ColorSpace)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxColorSpace.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxColorSpace.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.ColorSpace)));

               // comboBoxGeneric
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.GenericParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxGeneric.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.GenericParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxGeneric.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Generic));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxGeneric.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Generic));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.Generic)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxGeneric.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxGeneric.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.Generic)));

               // comboBoxExternalTrigger
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ExternalTriggerParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxExternalTrigger.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.ExternalTriggerParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxExternalTrigger.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ExternalTrigger));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxExternalTrigger.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.ExternalTrigger));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.ExternalTrigger)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxExternalTrigger.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxExternalTrigger.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.ExternalTrigger)));

               // comboBoxCameraType
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.CameraTypeParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxCameraType.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.CameraTypeParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxCameraType.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.CameraType));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxCameraType.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.CameraType));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.CameraType)
                   .Where(_ => this.MainViewModel.AcquireAcquisitionVM.CurrentAcquisitionInterfaceName != "File")
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxCameraType.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxCameraType.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.CameraType)));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.CameraType)
                   .Where(_ => this.MainViewModel.AcquireAcquisitionVM.CurrentAcquisitionInterfaceName == "File")
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxCameraType.Text = MainViewModel.AcquireAcquisitionVM.CameraType));

               // comboBoxDevice
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.DeviceParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxDevice.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.DeviceParameters.ToSArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxDevice.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Device));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxDevice.Text)
                  .Where(x => x != string.Empty)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Device));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.Device)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxDevice.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxDevice.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.Device)));

               // comboBoxPort
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.PortParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxPort.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.PortParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxPort.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Port));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxPort.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.Port));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.Port)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxPort.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxPort.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.Port)));

               // comboBoxLineIn
               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.LineInParameters)
                 .Subscribe(_ => this.acquisitionAcquirePage.comboBoxLineIn.ItemsSource = this.MainViewModel.AcquireAcquisitionVM.LineInParameters.ToIArr()));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxLineIn.SelectedItem)
                   .Where(x => x != null)
                   .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.LineIn));

               this.disposeCollection.Add(this.acquisitionAcquirePage.WhenAnyValue(x => x.comboBoxLineIn.Text)
                  .Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^-?\d+$") == true)
                  .BindTo(this.MainViewModel.AcquireAcquisitionVM, vm => vm.LineIn));

               this.disposeCollection.Add(this.WhenAnyValue(x => x.MainViewModel.AcquireAcquisitionVM.LineIn)
                   .Subscribe(_ => this.acquisitionAcquirePage.comboBoxLineIn.SelectedIndex =
                       this.acquisitionAcquirePage.comboBoxLineIn.Items.IndexOf(MainViewModel.AcquireAcquisitionVM.LineIn)));
          }

          /// <summary>
          /// Initializes the acquisition module.
          /// </summary>
          private void InitializeAcquireAcquisitionModule(ViewROIManager manager)
          {
               this.MainViewModel.AcquireAcquisitionVM.GetFileName.RegisterHandler(
                    async interaction =>
                    {
                         string filename = await this.utilities.GetFileName();
                         interaction.SetOutput(filename);
                    });

               this.MainViewModel.AcquireAcquisitionVM.GetSaveFileName.RegisterHandler(
                    async interaction =>
                    {
                         string filename = await this.utilities.GetFileNameToSave();
                         interaction.SetOutput(filename);
                    });

               RibbonMenuItem acquisitionMenuItem = new RibbonMenuItem()
               {
                    Name = "AcquireAcquisitionItem",
                    Header = "Acquire Acquisition"
               };

               acquisitionMenuItem.Click += this.AcquireAcquisitionRibbonItem_Click;
               this.UtilitiesTab.Items.Add(acquisitionMenuItem);

               this.BindAcquireAcquisitionControls(manager);

               this.InitializeAcquireCalibrationModule(manager);
          }

          #endregion Private Methods
     }
}