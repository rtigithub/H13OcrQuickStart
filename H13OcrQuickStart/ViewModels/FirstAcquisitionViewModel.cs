//-----------------------------------------------------------------------
// <copyright file="AcquireAcquisitionViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.ViewModels
{
     using System;
     using System.Reactive;
     using System.Reactive.Linq;
     using System.Threading.Tasks;
     using ReactiveUI;
     using Rti.DisplayUtilities;
     using Rti.Halcon;
     using HalconDotNet;
     using Models;

     /// <summary>
     /// View model for the new AcquireAcquisition process. 
     /// </summary>
     public class FirstAcquisitionViewModel : ProcessViewModelBase<MainViewModel, FirstAcquisitionProcessor>
     {
          #region Private Declarations

          /// <summary>
          /// Stores the Interaction to get a file name from the user.
          /// </summary>
          private Interaction<Unit, string> getFileName;

          /// <summary>
          /// Stores the Interaction to get a file name from the user for a save operation.
          /// </summary>
          private Interaction<Unit, string> getSaveFileName;

          /// <summary>
          /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call. 
          /// </summary>
          private ObservableAsPropertyHelper<ProcessingResult> processingResults;

          //// Create an ObservableAsPropertyHelper<ProcessingResult> for each command with a ProcessingResult you need to monitor.   

          //// Create backing fields for the properties as needed. 

          /// <summary>
          /// Stores the image width.
          /// </summary>
          private ObservableAsPropertyHelper<int> acquiredImageWidth;

          /// <summary>
          /// Stores the image height.
          /// </summary>
          private ObservableAsPropertyHelper<int> acquiredImageHeight;

          /// <summary>
          /// Stores the number of bits per channel.
          /// </summary>
          private int bitsPerChannel = -1;

          /// <summary>
          /// Store the list of bits per channel parameters.
          /// </summary>
          private HTuple bitsPerChannelParameters = new HTuple();

          /// <summary>
          /// Stores the camera type.
          /// </summary>
          private string cameraType = "default";

          /// <summary>
          /// Store the list of camera type parameters.
          /// </summary>
          private HTuple cameraTypeParameters = new HTuple();

          /// <summary>
          /// Stores the color space.
          /// </summary>
          private string colorSpace = "default";

          /// <summary>
          /// Store the list of color space parameters.
          /// </summary>
          private HTuple colorSpaceParameters = new HTuple();

          /// <summary>
          /// Stores the name of the current acquisition interface.
          /// </summary>
          private string currentAcquisitionInterfaceName = string.Empty;

          /// <summary>
          /// Stores the device value.
          /// </summary>
          private string device = "default";

          /// <summary>
          /// Store the list of device parameters.
          /// </summary>
          private HTuple deviceParameters = new HTuple();

          /// <summary>
          /// Stores the external trigger value.
          /// </summary>
          private string externalTrigger = "default";

          /// <summary>
          /// Store the list of external trigger parameters.
          /// </summary>
          private HTuple externalTriggerParameters = new HTuple();

          /// <summary>
          /// Stores the field value.
          /// </summary>
          private string field = "default";

          /// <summary>
          /// Store the list of field parameters.
          /// </summary>
          private HTuple fieldParameters = new HTuple();

          /// <summary>
          /// Stores the generic value.
          /// </summary>
          private string generic = string.Empty;

          /// <summary>
          /// Store the list of generic parameters.
          /// </summary>
          private HTuple genericParameters = new HTuple();

          /// <summary>
          /// Stores the horizontal resolution.
          /// </summary>
          private int horizontalResolution = 1;

          /// <summary>
          /// Store the list of horizontal resolution parameters.
          /// </summary>
          private HTuple horizontalResolutionParameters = new HTuple();

          /// <summary>
          /// Stores the working image.
          /// </summary>
          private HImage image = new HImage();

          /// <summary>
          /// Stores the image height.
          /// </summary>
          private int imageHeight = 0;

          /// <summary>
          /// Store the list of image height parameters.
          /// </summary>
          private HTuple imageHeightParameters = new HTuple();

          /// <summary>
          /// Stores the image width.
          /// </summary>
          private int imageWidth = 0;

          /// <summary>
          /// Store the list of image width parameters.
          /// </summary>
          private HTuple imageWidthParameters = new HTuple();

          /// <summary>
          /// Stores a value indicating whether the class has been disposed. 
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// Stores a value indicate whether the processor is processing an image for display.
          /// </summary>
          private bool isProcessing = false;

          /// <summary>
          /// Stores the line-in value.
          /// </summary>
          private int lineIn = -1;

          /// <summary>
          /// Store the list of line-in parameters.
          /// </summary>
          private HTuple lineInParameters = new HTuple();

          /// <summary>
          /// A value indicating whether the system is acquiring live video.
          /// </summary>
          private bool liveVideoMode = false;

          /// <summary>
          /// Stores the port.
          /// </summary>
          private int port = -1;

          /// <summary>
          /// Store the list of port parameters.
          /// </summary>
          private HTuple portParameters = new HTuple();

          /// <summary>
          /// Stores the file name to save the image to. 
          /// </summary>
          private string saveImageFileName = string.Empty;

          /// <summary>
          /// Stores the visibility state of the selectFile button.
          /// </summary>
          private System.Windows.Visibility selectFileVisibility = System.Windows.Visibility.Visible;

          /// <summary>
          /// Stores the start column.
          /// </summary>
          private int startColumn = 0;

          /// <summary>
          /// Store the list of start column parameters.
          /// </summary>
          private HTuple startColumnParameters = new HTuple();

          /// <summary>
          /// Stores the start row.
          /// </summary>
          private int startRow = 0;

          /// <summary>
          /// Store the list of start row parameters.
          /// </summary>
          private HTuple startRowParameters = new HTuple();

          /// <summary>
          /// Stores the vertical resolution.
          /// </summary>
          private int verticalResolution = 1;

          /// <summary>
          /// Store the list of vertical resolution parameters.
          /// </summary>
          private HTuple verticalResolutionParameters = new HTuple();

          #endregion Private Declarations

          #region Constructors

          /// <summary>
          /// Initializes a new instance of the AcquireAcquisitionViewModel class. 
          /// </summary>
          /// <param name="mainVM">A reference to the main view model.</param>
          /// <param name="processor">An instance of the processor class for this view model.</param>
          public FirstAcquisitionViewModel(IMainViewModel mainVM, IProcessor processor)
               : base(mainVM, processor)
          {
               this.getFileName = new Interaction<Unit, string>();
               this.getSaveFileName = new Interaction<Unit, string>();

               //// If a CanExecute condition needs to be set, do it here and recreate the Command using 
               //// the CanExecute object.                        
               //// this.CanExecute = this.WhenAny(x => x.MainViewModelRef.<some property>, x => x.Value == false); 
               //// this.Command = ReactiveCommand.CreateFromTask(_ => this.ProcessAsync(), this.CanExecute);  

               this.LiveVideoCanExecute = this.WhenAnyValue(x => x.Processor.IsInitialized)
                    .ObserveOn(RxApp.MainThreadScheduler);

               this.Command = ReactiveCommand.CreateFromTask(_ => this.ProcessAsync(), this.LiveVideoCanExecute);

               // To force immediate disposal of large or Iconic objects, uncomment the Do clause line, 
               // otherwise the GarbageCollection will do it after enough memory is used. 
               this.Command
                    // .Do(_ => this.ProcessingResults?.Dispose())
                    .ToProperty(this, x => x.ProcessingResults, out this.processingResults);

               this.InitializeCommand = ReactiveCommand.CreateFromTask(_ => this.InitializeCameraAsync());

               this.LiveVideoCommand = ReactiveCommand.CreateFromTask(_ => this.LiveVideoAsync(), this.LiveVideoCanExecute);

               this.CanExecute = this.WhenAny(x => x.Processor.AcquiredImage, x => x.Value.IsInitialized())
                    .ObserveOn(RxApp.MainThreadScheduler);
               this.SaveImageCommand = ReactiveCommand.CreateFromTask(_ => this.SaveImageAsync(), this.CanExecute);

               this.SelectFileCommand = ReactiveCommand.CreateFromTask(_ => this.SelectFileAsync());

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.Processor.DebugDisplay)
                   .Where(x => x.DisplayList.Count > 0)
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(x =>
                   {
                        this.DebugDisplay.Dispose();
                        this.DebugDisplay = x;
                   }));

               //// Set up the display to rebuild if a reactive display property changes            
               this.DisposeCollection.Add(this.WhenAny(x => x.Processor.AcquiredImage, x => x.Value)
                   .Where(x => x.IsValid())
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(_ =>
                   {
                        this.IsProcessing = true;
                        this.Image.Dispose();
                        this.Image = this.Processor.AcquiredImage.CopyObj(1, -1);
                        this.SetDisplay();
                        this.MainViewModelRef.AppState = 0;
                   }));

               //// This reacts to the execution of the command by resetting the AppState. Modify as needed. 
               this.DisposeCollection.Add(this.Command
                   .Subscribe(_ =>
                   {
                        this.MainViewModelRef.AppState = this.MainViewModelRef.AppState == 0 ? this.MainViewModelRef.LastAppState : this.MainViewModelRef.AppState;
                   }));

               // This observable serves to push error messages from the live video process that 
               // would not come through the ProcessingResult returned when the video was stated. 
               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                   .Subscribe(x => this.MainViewModelRef.StatusText = x));

               this.AcquisitionInterfaces = new ReactiveList<string>();
               this.AcquisitionInterfaces.AddRange(Rti.Halcon.HInfo.GetAvailableFramegrabbers());
               this.AcquisitionInterfaces.Remove("DirectFile");

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.CurrentAcquisitionInterfaceName)
                   .Where(x => x != string.Empty)
                   .SubscribeOn(RxApp.MainThreadScheduler)
                   .Subscribe(_ => this.UpdateInterface()));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.CurrentAcquisitionInterfaceName)
                    .SubscribeOn(RxApp.MainThreadScheduler)
                    .Subscribe(x =>
                   {
                        if (x == "File")
                        {
                             this.SelectFileVisibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                             this.SelectFileVisibility = System.Windows.Visibility.Hidden;
                        }
                   }));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.AcquiredImageHeight)
                   .StartWith(0)
                   .ToProperty(this, x => x.AcquiredImageHeight, out this.acquiredImageHeight));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.AcquiredImageWidth)
                   .StartWith(0)
                   .ToProperty(this, x => x.AcquiredImageWidth, out this.acquiredImageWidth));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.IsProcessing)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => this.Processor.CanGrabNextFrame = !x));
          }

          #endregion  Constructors

          #region Private Destructors

          #endregion Private Destructors

          #region Public Properties

          //// Create additional reactive commands as needed.

          /// <summary>
          /// Gets or sets the command for triggering live video. 
          /// </summary>
          public ReactiveCommand<Unit, ProcessingResult> LiveVideoCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the command for initializing the frame grabber. 
          /// </summary>
          public ReactiveCommand<Unit, ProcessingResult> InitializeCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the command for saving an image. 
          /// </summary>
          public ReactiveCommand<Unit, ProcessingResult> SaveImageCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the command for selecting a file. 
          /// </summary>
          public ReactiveCommand<Unit, ProcessingResult> SelectFileCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets the Interaction to return a file name from the user.
          /// </summary>
          public Interaction<Unit, string> GetFileName => this.getFileName;

          /// <summary>
          /// Gets the Interaction to return a file name from the user for a save operation.
          /// </summary>
          public Interaction<Unit, string> GetSaveFileName => this.getSaveFileName;


          /// <summary>
          /// Gets or sets the acquisition interface list.
          /// </summary>
          public ReactiveList<string> AcquisitionInterfaces
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the command to trigger live video can execute. 
          /// </summary>
          public IObservable<bool> LiveVideoCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the acquired image.
          /// </summary>
          public HImage Image
          {
               get => this.image;

               set => this.RaiseAndSetIfChanged(ref this.image, value);
          }

          /// <summary>
          /// Gets or sets a value indicate whether the processor is processing an image for display.
          /// </summary>
          public bool IsProcessing
          {
               get => this.isProcessing;

               set => this.RaiseAndSetIfChanged(ref this.isProcessing, value);
          }

          /// <summary>
          /// Gets the image width. 
          /// </summary>
          public int AcquiredImageWidth => this.acquiredImageWidth.Value;

          /// <summary>
          /// Gets the image height. 
          /// </summary>
          public int AcquiredImageHeight => this.acquiredImageHeight.Value;

          /// <summary>
          /// Gets or sets the bits per channel.
          /// </summary>
          public int BitsPerChannel
          {
               get => this.bitsPerChannel;

               set => this.RaiseAndSetIfChanged(ref this.bitsPerChannel, value);
          }

          /// <summary>
          /// Gets or sets the bits per channel parameters. 
          /// </summary>
          public HTuple BitsPerChannelParameters
          {
               get => this.bitsPerChannelParameters;

               set => this.RaiseAndSetIfChanged(ref this.bitsPerChannelParameters, value);
          }

          /// <summary>
          /// Gets or sets the camera type.
          /// </summary>
          public string CameraType
          {
               get => this.cameraType;

               set => this.RaiseAndSetIfChanged(ref this.cameraType, value);
          }

          /// <summary>
          /// Gets or sets the camera type parameters. 
          /// </summary>
          public HTuple CameraTypeParameters
          {
               get => this.cameraTypeParameters;

               set => this.RaiseAndSetIfChanged(ref this.cameraTypeParameters, value);
          }

          /// <summary>
          /// Gets or sets the color space.
          /// </summary>
          public string ColorSpace
          {
               get => this.colorSpace;

               set => this.RaiseAndSetIfChanged(ref this.colorSpace, value);
          }

          /// <summary>
          /// Gets or sets the color space parameters.
          /// </summary>
          public HTuple ColorSpaceParameters
          {
               get => this.colorSpaceParameters;

               set => this.RaiseAndSetIfChanged(ref this.colorSpaceParameters, value);
          }

          /// <summary>
          /// Gets or sets the current acquisition interface name.
          /// </summary>
          public string CurrentAcquisitionInterfaceName
          {
               get => this.currentAcquisitionInterfaceName;

               set => this.RaiseAndSetIfChanged(ref this.currentAcquisitionInterfaceName, value);
          }

          /// <summary>
          /// Gets or sets the device.
          /// </summary>
          public string Device
          {
               get => this.device;

               set => this.RaiseAndSetIfChanged(ref this.device, value);
          }

          /// <summary>
          /// Gets or sets the device parameters.
          /// </summary>
          public HTuple DeviceParameters
          {
               get => this.deviceParameters;

               set => this.RaiseAndSetIfChanged(ref this.deviceParameters, value);
          }

          /// <summary>
          /// Gets or sets the externalTrigger.
          /// </summary>
          public string ExternalTrigger
          {
               get => this.externalTrigger;

               set => this.RaiseAndSetIfChanged(ref this.externalTrigger, value);
          }

          /// <summary>
          /// Gets or sets the externalTrigger parameters.
          /// </summary>
          public HTuple ExternalTriggerParameters
          {
               get => this.externalTriggerParameters;

               set => this.RaiseAndSetIfChanged(ref this.externalTriggerParameters, value);
          }

          /// <summary>
          /// Gets or sets the field.
          /// </summary>
          public string Field
          {
               get => this.field;

               set => this.RaiseAndSetIfChanged(ref this.field, value);
          }

          /// <summary>
          /// Gets or sets the field parameters.
          /// </summary>
          public HTuple FieldParameters
          {
               get => this.fieldParameters;

               set => this.RaiseAndSetIfChanged(ref this.fieldParameters, value);
          }

          /// <summary>
          /// Gets or sets the generic value.
          /// </summary>
          public string Generic
          {
               get => this.generic;

               set => this.RaiseAndSetIfChanged(ref this.generic, value);
          }

          /// <summary>
          /// Gets or sets the generic parameters.
          /// </summary>
          public HTuple GenericParameters
          {
               get => this.genericParameters;

               set => this.RaiseAndSetIfChanged(ref this.genericParameters, value);
          }

          /// <summary>
          /// Gets or sets the horizontal resolution.
          /// </summary>
          public int HorizontalResolution
          {
               get => this.horizontalResolution;

               set => this.RaiseAndSetIfChanged(ref this.horizontalResolution, value);
          }

          /// <summary>
          /// Gets or sets the horizontal resolution parameters.
          /// </summary>
          public HTuple HorizontalResolutionParameters
          {
               get => this.horizontalResolutionParameters;

               set => this.RaiseAndSetIfChanged(ref this.horizontalResolutionParameters, value);
          }

          /// <summary>
          /// Gets or sets the image height.
          /// </summary>
          public int ImageHeight
          {
               get => this.imageHeight;

               set => this.RaiseAndSetIfChanged(ref this.imageHeight, value);
          }

          /// <summary>
          /// Gets or sets the imageHeight parameters.
          /// </summary>
          public HTuple ImageHeightParameters
          {
               get => this.imageHeightParameters;

               set => this.RaiseAndSetIfChanged(ref this.imageHeightParameters, value);
          }

          /// <summary>
          /// Gets or sets the image width.
          /// </summary>
          public int ImageWidth
          {
               get => this.imageWidth;

               set => this.RaiseAndSetIfChanged(ref this.imageWidth, value);
          }

          /// <summary>
          /// Gets or sets the image width parameters.
          /// </summary>
          public HTuple ImageWidthParameters
          {
               get => this.imageWidthParameters;

               set => this.RaiseAndSetIfChanged(ref this.imageWidthParameters, value);
          }

          /// <summary>
          /// Gets or sets the lineIn.
          /// </summary>
          public int LineIn
          {
               get => this.lineIn;

               set => this.RaiseAndSetIfChanged(ref this.lineIn, value);
          }

          /// <summary>
          /// Gets or sets the lineIn parameters.
          /// </summary>
          public HTuple LineInParameters
          {
               get => this.lineInParameters;

               set => this.RaiseAndSetIfChanged(ref this.lineInParameters, value);
          }

          /// <summary>
          /// Gets the processing results. 
          /// </summary>
          public ProcessingResult ProcessingResults => this.processingResults.Value;

          /// <summary>
          /// Gets or sets the port.
          /// </summary>
          public int Port
          {
               get => this.port;

               set => this.RaiseAndSetIfChanged(ref this.port, value);
          }

          /// <summary>
          /// Gets or sets the port parameters.
          /// </summary>
          public HTuple PortParameters
          {
               get => this.portParameters;

               set => this.RaiseAndSetIfChanged(ref this.portParameters, value);
          }

          /// <summary>
          /// Gets or sets file name to save the image to.
          /// </summary>
          public string SaveImageFileName
          {
               get => this.saveImageFileName;

               set => this.RaiseAndSetIfChanged(ref this.saveImageFileName, value);
          }

          /// <summary>
          /// Gets or sets visibility state of the selectFile button.
          /// </summary>
          public System.Windows.Visibility SelectFileVisibility
          {
               get => this.selectFileVisibility;

               set => this.RaiseAndSetIfChanged(ref this.selectFileVisibility, value);
          }

          /// <summary>
          /// Gets or sets the start row.
          /// </summary>
          public int StartRow
          {
               get => this.startRow;

               set => this.RaiseAndSetIfChanged(ref this.startRow, value);
          }

          /// <summary>
          /// Gets or sets the start row parameters.
          /// </summary>
          public HTuple StartRowParameters
          {
               get => this.startRowParameters;

               set => this.RaiseAndSetIfChanged(ref this.startRowParameters, value);
          }

          /// <summary>
          /// Gets or sets the start column.
          /// </summary>
          public int StartColumn
          {
               get => this.startColumn;

               set => this.RaiseAndSetIfChanged(ref this.startColumn, value);
          }

          /// <summary>
          /// Gets or sets the start column parameters.
          /// </summary>
          public HTuple StartColumnParameters
          {
               get => this.startColumnParameters;

               set => this.RaiseAndSetIfChanged(ref this.startColumnParameters, value);
          }

          /// <summary>
          /// Gets or sets the vertical resolution.
          /// </summary>
          public int VerticalResolution
          {
               get => this.verticalResolution;

               set => this.RaiseAndSetIfChanged(ref this.verticalResolution, value);
          }

          /// <summary>
          /// Gets or sets the vertical resolution parameters.
          /// </summary>
          public HTuple VerticalResolutionParameters
          {
               get => this.verticalResolutionParameters;

               set => this.RaiseAndSetIfChanged(ref this.verticalResolutionParameters, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the system is acquiring live video.
          /// </summary>
          public bool LiveVideoMode
          {
               get => this.liveVideoMode;

               set => this.RaiseAndSetIfChanged(ref this.liveVideoMode, value);
          }

          /////// <summary>
          /////// Gets or sets the MyCommand.
          /////// </summary>
          ////public ReactiveCommand<ProcessingResult> MyCommand
          ////{
          ////    get;
          ////    protected set;
          ////}

          //// Create additional CanExecute observables as needed.

          ///// <summary>
          ///// Gets or sets the can executer observable for my command. 
          ///// </summary>
          ////public IObservable<bool> MyCommandCanExecute
          ////{
          ////    get;

          ////    set;
          ////}

          //// Create a ProcessingResult property for each command with an ObservableAsPropertyHelper<ProcessingResult> you need to monitor. 

          /////// <summary>
          /////// Gets the processing results for MyCommand. 
          /////// </summary>
          ////public ProcessingResult MyProcessingResults
          ////{
          ////    get
          ////    {
          ////        return this.myProcessingResults.Value;
          ////    }
          ////}

          //// Create properties to manage View properties and any needed to pass to the Processor or other View Models.

          //// Create properties that expose any display object properties in the Processor model.

          #endregion Properties

          #region Public Methods

          #endregion public methods

          #region Protected Methods
          /// <summary>
          /// Implements the asynchronous process method for this process. 
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          protected override async Task<ProcessingResult> ProcessAsync()
          {
               this.Processor.AcquiringLiveVideo = false;
               this.LiveVideoMode = false;

               if (this.MainViewModelRef.AcquireCalibrationVM.CorrectNewImages)
               {
                    return await Task.Factory.StartNew(() =>
                         this.Processor.AcquireImageWithCalibration(
                              this.MainViewModelRef.AcquireCalibrationVM.CalibrationMap));
               }
               else
               {
                    return await base.ProcessAsync();
               }
          }

          /// <summary>
          /// Builds a DisplayCollection that will be displayed whenever a monitored display object is changed. 
          /// </summary>
          /// <returns>the DisplayCollection.</returns>
          protected override DisplayCollection BuildDisplayItem()
          {
               DisplayCollection tempDC = new DisplayCollection()
               {
                    //// Set to true to clear any existing display before adding new objects.
                    //// Set to false to display new objects over existing ones. 
                    ClearDisplayFirst = true
               };

               tempDC.AddDisplayObject(this.Processor.AcquiredImage.CopyObj(1, -1));

               return tempDC;
          }

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
                    }

                    //// Dispose of unmanaged resources here.

                    this.image?.Dispose();

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter. 
               base.Dispose(disposing);
          }

          #endregion Protected Methods

          #region Private Methods

          //// Create additional async methods for commands

          ////private async Task<ProcessingResult> MyProcessAsync()
          ////{
          ////    // No parameters:
          ////    return await Task.Factory.StartNew(() => this.Processor.MyProcess());
          ////    // Create Processor.MyProcess().
          ////    
          ////    // With parameters.
          ////    // Dummy parameters. Change this. 
          ////    Tuple<object> parameters = new Tuple<object>(new object());
          ////    return await Task.Factory.StartNew(() => this.Processor.MyProcess(parameters));
          ////    // Create Processor.MyProcess(object parameters) and handle any parameters passes. 
          ////}

          /// <summary>
          /// Implements the asynchronous method to start or stop live video. 
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> LiveVideoAsync()
          {
               this.LiveVideoMode = !this.Processor.AcquiringLiveVideo;

               if (this.MainViewModelRef.AcquireCalibrationVM.CorrectNewImages)
               {
                    return await Task.Factory.StartNew(() =>
                         this.Processor.AcquireCalibratedLiveVideo(
                              this.LiveVideoMode,
                              this.MainViewModelRef.AcquireCalibrationVM.CalibrationMap));
               }
               else
               {
                    return await Task.Factory.StartNew(() => this.Processor.AcquireLiveVideo(this.LiveVideoMode));
               }
          }

          /// <summary>
          /// Implements the asynchronous method to initialize the camera. 
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> InitializeCameraAsync()
          {
               return await Task.Factory.StartNew(() => this.Processor.InitializeCamera(
                   this.CurrentAcquisitionInterfaceName,
                   this.HorizontalResolution,
                   this.VerticalResolution,
                   this.ImageWidth,
                   this.ImageHeight,
                   this.StartRow,
                   this.StartColumn,
                   this.Field,
                   this.BitsPerChannel,
                   this.ColorSpace,
                   this.Generic,
                   this.ExternalTrigger,
                   this.CameraType,
                   this.Device,
                   this.Port,
                   this.LineIn));
          }

          /// <summary>
          /// Implements the asynchronous method to save an image. 
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> SaveImageAsync()
          {
               try
               {
                    this.SaveImageFileName = await this.GetSaveFileName.Handle(new Unit());

                    if (this.SaveImageFileName != string.Empty)
                    {
                         return await Task.Factory.StartNew(() => this.Processor.SaveImageProcess(this.SaveImageFileName));
                    }
                    else
                    {
                         return new ProcessingResult();
                    }
               }
               catch (Exception)
               {
                    ProcessingResult result = new ProcessingResult()
                    {
                         ErrorMessage = "The Interaction to handle getting the file name for the save operation is not registered."
                    };

                    return result;
               }
          }

          /// <summary>.
          /// Implements the asynchronous method to Select a File. 
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> SelectFileAsync()
          {
               try
               {
                    this.CameraType = await this.GetFileName.Handle(new Unit());

                    return new ProcessingResult();
               }
               catch (Exception)
               {
                    ProcessingResult result = new ProcessingResult()
                    {
                         ErrorMessage = "The Interaction to handle getting the file name for the save operation is not registered."
                    };

                    return result;
               }
          }

          /// <summary>
          /// Sets the acquisition parameters.
          /// </summary>
          private void SetAcquisitionParameterItemValues()
          {
               this.HorizontalResolutionParameters =
                   this.Processor.GetAcquisitionParameterValues(
                       this.CurrentAcquisitionInterfaceName,
                       "horizontal_resolution");

               this.HorizontalResolution = this.Processor.DictionaryParameterDefaults["horizontal_resolution"];

               this.VerticalResolutionParameters =
                   this.Processor.GetAcquisitionParameterValues(
                       this.CurrentAcquisitionInterfaceName,
                       "vertical_resolution");
               this.VerticalResolution = this.Processor.DictionaryParameterDefaults["vertical_resolution"];

               this.ImageWidthParameters =
                   this.Processor.GetAcquisitionParameterValues(
                       this.CurrentAcquisitionInterfaceName,
                       "image_width");
               this.ImageWidth = this.Processor.DictionaryParameterDefaults["image_width"];

               this.ImageHeightParameters =
                   this.Processor.GetAcquisitionParameterValues(
                       this.CurrentAcquisitionInterfaceName,
                       "image_height");
               this.ImageHeight = this.Processor.DictionaryParameterDefaults["image_height"];

               this.StartRowParameters =
                   this.Processor.GetAcquisitionParameterValues(
                       this.CurrentAcquisitionInterfaceName,
                       "start_row");
               this.StartRow = this.Processor.DictionaryParameterDefaults["start_row"];

               this.StartColumnParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "start_column");
               this.StartColumn = this.Processor.DictionaryParameterDefaults["start_column"];

               this.FieldParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "field");
               this.Field = this.Processor.DictionaryParameterDefaults["field"];

               this.BitsPerChannelParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "bits_per_channel");
               this.BitsPerChannel = this.Processor.DictionaryParameterDefaults["bits_per_channel"];

               this.ColorSpaceParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "color_space");
               this.ColorSpace = this.Processor.DictionaryParameterDefaults["color_space"];

               this.GenericParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "generic");
               this.Generic = this.Processor.DictionaryParameterDefaults["generic"].ToString();

               this.ExternalTriggerParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "external_trigger");
               this.ExternalTrigger = this.Processor.DictionaryParameterDefaults["external_trigger"];

               Rti.Halcon.HInfo.InfoFramegrabber(
                    this.CurrentAcquisitionInterfaceName,
                    "info_boards",
                    out HTuple cameraTypeValueList);
               this.CameraTypeParameters = cameraTypeValueList.TupleRegexpMatch(@"(?:.*] )(.+)");
               this.CameraTypeParameters = this.CameraTypeParameters.TupleRegexpSelect(@".+");
               this.CameraTypeParameters = Utilities.UtilityLibrary.Unique(this.CameraTypeParameters);
               this.CameraTypeParameters = this.CameraTypeParameters.TupleInsert(0, new HTuple("default"));
               this.CameraType = this.Processor.DictionaryParameterDefaults["camera_type"];

               HTuple cameras = new HTuple();
               HTuple testResult = new HTuple();
               cameras = this.Processor.GetAcquisitionParameterValues(
                   this.CurrentAcquisitionInterfaceName,
                   "device");
               if (cameras.Length > 0)
               {
                    testResult = cameras.TupleRegexpMatch(@"device:");
                    for (int i = 0; i < cameras.Length; i++)
                    {
                         if (testResult[i].S != string.Empty)
                         {
                              cameras[i] = cameras.TupleSelect(i).TupleRegexpMatch(@"(?:device:)(\w+)");
                         }
                    }
               }

               this.DeviceParameters = cameras;
               if (cameras.Length > 0)
               {
                    this.Device = this.DeviceParameters[0];
               }
               else
               {
                    this.Device = this.Processor.DictionaryParameterDefaults["device"];
               }

               this.PortParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "port");
               this.Port = this.Processor.DictionaryParameterDefaults["port"];

               this.LineInParameters =
                  this.Processor.GetAcquisitionParameterValues(
                      this.CurrentAcquisitionInterfaceName,
                      "line_in");
               this.LineIn = this.Processor.DictionaryParameterDefaults["line_in"];
          }

          /// <summary>
          /// Updates the Acquisition interface controls for the selected acquisition interface. 
          /// </summary>
          private void UpdateInterface()
          {
               this.Processor.GenerateAcquisitionDefaults(this.CurrentAcquisitionInterfaceName);
               this.SetAcquisitionParameterItemValues();
          }

          #endregion Private Methods
     }
}

