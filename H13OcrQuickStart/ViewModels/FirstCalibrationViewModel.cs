// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="CameraCalibrationViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.ViewModels
{
     using System;
     using System.Reactive;
     using System.Reactive.Linq;
     using System.Threading.Tasks;
     using HalconDotNet;
     using Models;
     using ReactiveUI;
     using Rti.DisplayUtilities;
     using Rti.Halcon;

     /// <summary>
     /// View model for the new Acquire Calibration process.
     /// </summary>
     /// <seealso cref="H13OcrQuickStart.ViewModels.ProcessViewModelBase{H13OcrQuickStart.ViewModels.MainViewModel, H13OcrQuickStart.Models.FirstCalibrationProcessor}" />
     /// <seealso cref="H13OcrQuickStart.ViewModels.ICameraCalibrationViewModel" />
     public class FirstCalibrationViewModel : ProcessViewModelBase<MainViewModel, FirstCalibrationProcessor>, ICameraCalibrationViewModel
     {
          #region Private Fields

          /// <summary>
          /// Stores a value indicating whether all parameters are valid.
          /// </summary>
          private bool areInitialParametersValid = true;

          /// <summary>
          /// Stores the calibrated scale.
          /// </summary>
          private double calibratedScale = 0.5;

          /// <summary>
          /// Stores the name of the folder where calibration images are located.
          /// </summary>
          private string calibrationImageFolderName = string.Empty;

          /// <summary>
          /// Stores the calibration map.
          /// </summary>
          private HImage calibrationMap = new HImage();

          /// <summary>
          /// Stores the file name for loading the calibration map.
          /// </summary>
          private string calibrationMapLoadFileName = string.Empty;

          /// <summary>
          /// Stores the file name for saving the calibration map.
          /// </summary>
          private string calibrationMapSaveFileName = string.Empty;

          /// <summary>
          /// Stores the calibration test image.
          /// </summary>
          private HImage calibrationTestImage = new HImage();

          /// <summary>
          /// Stores the camera type.
          /// </summary>
          private string cameraType = "area_scan_division";

          /// <summary>
          /// Stores the camera type parameters.
          /// </summary>
          private HTuple cameraTypeParameters = new HTuple();

          /// <summary>
          /// Stores a value indicating whether new images should be corrected.
          /// </summary>
          private bool correctNewImages = false;

          /// <summary>
          /// Stores the focal length.
          /// </summary>
          private double focalLength = 0.016;

          /// <summary>
          /// Stores the focal length parameters.
          /// </summary>
          private HTuple focalLengthParameters = new HTuple(1.0);

          /// <summary>
          /// Stores the Interaction to get a file name for a save calibration map operation from the user.
          /// </summary>
          private Interaction<Unit, string> getCalibratioinMapSaveFileName;

          /// <summary>
          /// Stores the Interaction to get a file name from the user.
          /// </summary>
          private Interaction<Unit, string> getFileName;

          /// <summary>
          /// Stores the Interaction to get a folder name from the user.
          /// </summary>
          private Interaction<Unit, string> getFolderName;

          /// <summary>
          /// Stores the Interaction to get a file name for a save operation from the user.
          /// </summary>
          private Interaction<Unit, string> getSaveFileName;

          /// <summary>
          /// Stores the name of the halcon calibration plate.
          /// </summary>
          private string halconCalibrationPlateName = "calplate_320mm.cpd";

          // assumes defaults are valid.
          // <summary>
          /// <summary>
          /// The halcon calibration plate parameters
          /// </summary>
          private HTuple halconCalibrationPlateParameters = new HTuple();

          /// <summary>
          /// Stores the image height.
          /// </summary>
          private int imageHeight = 480;

          /// <summary>
          /// Stores the image height parameters.
          /// </summary>
          private HTuple imageHeightParameters = new HTuple();

          /// <summary>
          /// Stores the image width.
          /// </summary>
          private int imageWidth = 640;

          /// <summary>
          /// Stores the image width parameters.
          /// </summary>
          private HTuple imageWidthParameters = new HTuple();

          /// <summary>
          /// Stores a value indicating whether the processor is busy.
          /// </summary>
          private bool isBusy = false;

          /// <summary>
          /// Stores a value indicating whether calibration test image is present.
          /// </summary>
          private bool isCalibrationTestImagePresent = false;

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// Stores a value indicating whether processor is loading a calibration test image.
          /// </summary>
          private bool loadingTestImage = false;

          /// <summary>
          /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call.
          /// </summary>
          private ObservableAsPropertyHelper<ProcessingResult> processingResults;

          /// <summary>
          /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call.
          /// </summary>
          private ObservableAsPropertyHelper<ProcessingResult> processingResultsCalibrate;

          /// <summary>
          /// Stores the rectified image height.
          /// </summary>
          private int rectifiedImageHeight = 525;

          /// <summary>
          /// Stores  a value indicating whether the rectified image size change is programmatically called.
          /// </summary>
          private bool rectifiedImageSizeProgramaticCall = false;

          /// <summary>
          /// Stores the rectified image width.
          /// </summary>
          private int rectifiedImageWidth = 700;

          /// <summary>
          /// Stores the file name for saving the rectified test image.
          /// </summary>
          private string rectifiedTestImageSaveName = string.Empty;

          /// <summary>
          /// Stores the rotation.
          /// </summary>
          private double rotation = 0.0;

          /// <summary>
          /// Stores the rotation parameters.
          /// </summary>
          private HTuple rotationParameters = new HTuple();

          /// <summary>
          /// Stores the rotation in the X direction.
          /// </summary>
          private double rotX = 0;

          /// <summary>
          /// Stores the rotation in the X direction parameters.
          /// </summary>
          private HTuple rotXParameters = new HTuple();

          /// <summary>
          /// Stores the rotation in the Y direction.
          /// </summary>
          private double rotY = 0;

          /// <summary>
          /// Stores the rotation in the Y direction parameters.
          /// </summary>
          private HTuple rotYParameters = new HTuple();

          /// <summary>
          /// Stores the rotation in the z direction.
          /// </summary>
          private double rotZ = 0;

          /// <summary>
          /// Stores the rotation in the Z direction parameters.
          /// </summary>
          private HTuple rotZParameters = new HTuple();

          /// <summary>
          /// Stores the sensor size in the X direction.
          /// </summary>
          private double sensorSizeX = 0.005;

          /// <summary>
          /// Stores the sensor size in the X direction parameters.
          /// </summary>
          private HTuple sensorSizeXParameters = new HTuple();

          /// <summary>
          /// Stores the sensor size in the Y direction.
          /// </summary>
          private double sensorSizeY = 0.005;

          /// <summary>
          /// Stores the sensor size in the Y direction parameters.
          /// </summary>
          private HTuple sensorSizeYParameters = new HTuple();

          /// <summary>
          /// Stores the tilt.
          /// </summary>
          private double tilt = 0.0;

          /// <summary>
          /// Stores the tilt parameters.
          /// </summary>
          private HTuple tiltParameters = new HTuple();

          /// <summary>
          /// Stores the translation in the X direction.
          /// </summary>
          private double transX = 0;

          /// <summary>
          /// Stores the translation in the X direction parameters.
          /// </summary>
          private HTuple transXParameters = new HTuple();

          /// <summary>
          /// Stores the translation in the Y direction.
          /// </summary>
          private double transY = 0;

          /// <summary>
          /// Stores the translation in the Y direction parameters.
          /// </summary>
          private HTuple transYParameters = new HTuple();

          /// <summary>
          /// Stores the translation in the Z direction.
          /// </summary>
          private double transZ = 0;

          /// <summary>
          /// Stores the translation in the Z direction parameters.
          /// </summary>
          private HTuple transZParameters = new HTuple();

          /// <summary>
          /// Stores the index into the poses in the camera parameters to use as the world pose.
          /// </summary>
          private int worldPoseIndex = 0;

          #endregion Private Fields

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the AcquireCalibrationViewModel class.
          /// </summary>
          /// <param name="mainVM">A reference to the main view model.</param>
          /// <param name="processor">An instance of the processor class for this view model.</param>
          public FirstCalibrationViewModel(IMainViewModel mainVM, IProcessor processor)
               : base(mainVM, processor)
          {
               //// If a CanExecute condition needs to be set, do it here and recreate the Command using
               //// the CanExecute object.
               //// this.CanExecute = this.WhenAny(x => x.MainViewModelRef.<some property>, x => x.Value == false);
               //// this.Command = ReactiveCommand.CreateFromTask(_ => this.ProcessAsync(), this.CanExecute, );

               // Unused.
               ////this.Command.ToProperty(this, x => x.ProcessingResults, out this.processingResults);

               this.getFileName = new Interaction<Unit, string>();
               this.getSaveFileName = new Interaction<Unit, string>();
               this.getFolderName = new Interaction<Unit, string>();
               this.getCalibratioinMapSaveFileName = new Interaction<Unit, string>();

               // Additional commands
               this.SetParametersCanExecute = this.WhenAnyValue(x => x.AreInitialParametersValid);
               this.SetParametersCommand = ReactiveCommand.CreateFromTask(_ => this.SetParametersAsync(), this.SetParametersCanExecute);

               this.LoadCalibImagesFromFileCanExecute = this.WhenAnyValue(x => x.Processor.AreCalibrationParametersSet)
                    .ObserveOn(RxApp.MainThreadScheduler);
               this.LoadCalibImagesFromFileCommand = ReactiveCommand.CreateFromTask(_ => this.LoadCalibImagesFromFileAsync(), this.LoadCalibImagesFromFileCanExecute);
               this.LoadCalibImagesFromFileCommand.ToProperty(this, x => x.ProcessingResults, out this.processingResults);

               this.AcquireCalibrationImagesCommand = ReactiveCommand.CreateFromTask(_ => this.AcquireCalibrationImagesAsync());

               this.ResetCalibrationImagesCommand = ReactiveCommand.CreateFromTask(_ => this.ResetCalibrationImagesAsync());

               this.CalibrateCanExecute = this.WhenAnyValue(x => x.Processor.AreCalibrationImagesSet)
                   .ObserveOn(RxApp.MainThreadScheduler); // This is required for async can execute states to reliably enable and disable view controls!
               this.CalibrateCommand = ReactiveCommand.CreateFromTask(_ => this.CalibrateAsync(), this.CalibrateCanExecute);
               this.CalibrateCommand.ToProperty(this, x => x.ProcessingResultsCalibrate, out this.processingResultsCalibrate);

               this.SaveCalibrationMapCanExecute = this.WhenAnyValue(x => x.Processor.CalibrationIsDone)
                   .ObserveOn(RxApp.MainThreadScheduler);
               this.SaveCalibrationMapCommand = ReactiveCommand.CreateFromTask(_ => this.SaveCalibrationMapAsync(), this.SaveCalibrationMapCanExecute);

               this.LoadCalibrationMapCommand = ReactiveCommand.CreateFromTask(_ => this.LoadCalibrationMapAsync());

               this.LoadTestCalibrationImageCanExecute = this.WhenAnyValue(x => x.Processor.IsCalibrationMapPresent)
                   .ObserveOn(RxApp.MainThreadScheduler);

               // All this command does is set the LoadingTestImage flag.
               this.LoadTestCalibrationImageCommand = ReactiveCommand.Create(() =>
               {
                    this.LoadingTestImage = true;
               }, this.LoadTestCalibrationImageCanExecute);

               // When executed, this command serves as an observable from which to invoke the main command of the LoadImageVM,
               // which should run asynchronously because it is an asynchronous command. ProcessingResult is handled there.
               this.LoadTestCalibrationImageCommand
                    .InvokeCommand(this.MainViewModelRef.LoadImageVM.Command);

               this.RectifyImageCanExecute = this.WhenAnyValue(x => x.IsCalibrationTestImagePresent)
                   .ObserveOn(RxApp.MainThreadScheduler);
               this.RectifyImageCommand = ReactiveCommand.CreateFromTask(_ => this.RectifyImageAsync(), this.RectifyImageCanExecute);

               this.SaveRectifiedImageCanExecute = this.WhenAnyValue(x => x.Processor.IsRectifiedTestImagePresent)
                   .ObserveOn(RxApp.MainThreadScheduler);
               this.SaveRectifiedImageCommand = ReactiveCommand.CreateFromTask(_ => this.SaveRectifiedAsync(), this.SaveRectifiedImageCanExecute);

               this.DisposeCollection.Add(
                   this.WhenAnyValue(a => a.FocalLength, b => b.ImageWidth, c => c.ImageHeight, d => d.SensorSizeX, e => e.SensorSizeY)
                   .Subscribe(_ => this.ValidateInitialParameters()));

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.Processor.DebugDisplay)
                   .Where(x => x.DisplayList.Count > 0)
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(x =>
                   {
                        this.DebugDisplay.Dispose();
                        this.DebugDisplay = x;
                   }));

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.ProcessingResults)
                   .Where(x => x != null)
                   .Subscribe(_ => this.IsBusy = false));

               // This observable catches any processor errors not returned in a ProcessingResult instance.
               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                   .Subscribe(x => this.MainViewModelRef.StatusText = x));

               //// This reacts to the execution of the command by resetting the AppState. Modify as needed.
               this.DisposeCollection.Add(this.Command
                   .Subscribe(_ =>
                   {
                        this.MainViewModelRef.AppState = this.MainViewModelRef.AppState == 0 ? this.MainViewModelRef.LastAppState : this.MainViewModelRef.AppState;
                   }));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.Processor.ErrorMessage)
                   .Subscribe(x => this.MainViewModelRef.StatusText = x));

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.MainViewModelRef.LoadImageVM.Image)
                   .Where(_ => this.LoadingTestImage)
                   .Subscribe(x =>
                   {
                        this.CalibrationTestImage.Dispose();
                        this.CalibrationTestImage = x.CopyObj(1, -1);

                        this.IsCalibrationTestImagePresent = true;
                        this.Processor.IsRectifiedTestImagePresent = false;
                        this.LoadingTestImage = false;
                   }));

               // Set up the display to rebuild if a reactive display property changes
               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.Processor.RectifiedTestImage)
                   .Where(x => x.IsInitialized())
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(_ =>
                   {
                        this.SetDisplay();
                   }));

               this.DisposeCollection.Add(
                 this.WhenAnyValue(x => x.Processor.CalibrationMap)
                 .Where(x => x.IsValid())
                 .Subscribe(x => this.CalibrationMap = x.CopyObj(1, -1)));

               this.DisposeCollection.Add(
                 this.WhenAnyValue(x => x.RectifiedImageWidth)
                 .Where(_ => !this.rectifiedImageSizeProgramaticCall)
                 .Subscribe(x => this.SetRectifiedImageHeightToAspectRatio(x)));

               this.DisposeCollection.Add(
                this.WhenAnyValue(x => x.RectifiedImageHeight)
                .Where(_ => !this.rectifiedImageSizeProgramaticCall)
                .Subscribe(x => this.SetRectifiedImageWidthToAspectRatio(x)));

               this.SetParameters();
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the command to acquire calibration images.
          /// </summary>
          /// <value>The acquire calibration images command.</value>
          public ReactiveCommand<Unit, ProcessingResult> AcquireCalibrationImagesCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets a value indicating whether all parameters are valid.
          /// </summary>
          /// <value><c>true</c> if the initial parameters are valid; otherwise, <c>false</c>.</value>
          public bool AreInitialParametersValid
          {
               get => this.areInitialParametersValid;

               set => this.RaiseAndSetIfChanged(ref this.areInitialParametersValid, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the Calibrate command can execute.
          /// </summary>
          /// <value>The calibrate can execute observable.</value>
          public IObservable<bool> CalibrateCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to perform a calibration.
          /// </summary>
          /// <value>The calibrate command.</value>
          public ReactiveCommand<Unit, ProcessingResult> CalibrateCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the calibrated scale.
          /// </summary>
          /// <value>The calibrated scale.</value>
          public double CalibratedScale
          {
               get => this.calibratedScale;

               set => this.RaiseAndSetIfChanged(ref this.calibratedScale, value);
          }

          /// <summary>
          /// Gets or sets the name of the folder where calibration images are located.
          /// </summary>
          /// <value>The name of the calibration image folder.</value>
          public string CalibrationImageFolderName
          {
               get => this.calibrationImageFolderName;

               set => this.RaiseAndSetIfChanged(ref this.calibrationImageFolderName, value);
          }

          /// <summary>
          /// Gets or sets the calibration map.
          /// </summary>
          /// <value>The calibration map.</value>
          public HImage CalibrationMap
          {
               get => this.calibrationMap;

               set => this.RaiseAndSetIfChanged(ref this.calibrationMap, value);
          }

          /// <summary>
          /// Gets or sets the file name for loading the calibration map.
          /// </summary>
          /// <value>The name of the calibration map load file.</value>
          public string CalibrationMapLoadFileName
          {
               get => this.calibrationMapLoadFileName;

               set => this.RaiseAndSetIfChanged(ref this.calibrationMapLoadFileName, value);
          }

          /// <summary>
          /// Gets or sets the file name for saving the calibration map.
          /// </summary>
          /// <value>The name of the calibration map save file.</value>
          public string CalibrationMapSaveFileName
          {
               get => this.calibrationMapSaveFileName;

               set => this.RaiseAndSetIfChanged(ref this.calibrationMapSaveFileName, value);
          }

          /// <summary>
          /// Gets or sets the calibration test image.
          /// </summary>
          /// <value>The calibration test image.</value>
          public HImage CalibrationTestImage
          {
               get => this.calibrationTestImage;

               set => this.RaiseAndSetIfChanged(ref this.calibrationTestImage, value);
          }

          /// <summary>
          /// Gets or sets the camera type.
          /// </summary>
          /// <value>The camera type.</value>
          public string CameraType
          {
               get => this.cameraType;

               set => this.RaiseAndSetIfChanged(ref this.cameraType, value);
          }

          /// <summary>
          /// Gets or sets the camera type parameters.
          /// </summary>
          /// <value>The camera type parameters.</value>
          public HTuple CameraTypeParameters
          {
               get => this.cameraTypeParameters;

               set => this.RaiseAndSetIfChanged(ref this.cameraTypeParameters, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether new images should be corrected.
          /// </summary>
          /// <value><c>true</c> if new images should be corrected; otherwise, <c>false</c>.</value>
          public bool CorrectNewImages
          {
               get => this.correctNewImages;

               set => this.RaiseAndSetIfChanged(ref this.correctNewImages, value);
          }

          /// <summary>
          /// Gets or sets the focal length.
          /// </summary>
          /// <value>The focal length.</value>
          public double FocalLength
          {
               get => this.focalLength;

               set => this.RaiseAndSetIfChanged(ref this.focalLength, value);
          }

          /// <summary>
          /// Gets or sets the focal length parameters.
          /// </summary>
          /// <value>The focal length parameters.</value>
          public HTuple FocalLengthParameters
          {
               get => this.focalLengthParameters;

               set => this.RaiseAndSetIfChanged(ref this.focalLengthParameters, value);
          }

          /// <summary>
          /// Gets the Interaction to return a file name to save a calibration map.
          /// </summary>
          /// <value>The name of the get calibratioin map save file.</value>
          public Interaction<Unit, string> GetCalibratioinMapSaveFileName => this.getCalibratioinMapSaveFileName;

          /// <summary>
          /// Gets the Interaction to return a file name to load a file.
          /// </summary>
          /// <value>The name of the get file.</value>
          public Interaction<Unit, string> GetFileName => this.getFileName;

          /// <summary>
          /// Gets the Interaction to return a folder name from the user.
          /// </summary>
          /// <value>The name of the get folder.</value>
          public Interaction<Unit, string> GetFolderName => this.getFolderName;

          /// <summary>
          /// Gets the Interaction to return a file name to save a file.
          /// </summary>
          /// <value>The name of the get save file.</value>
          public Interaction<Unit, string> GetSaveFileName => this.getSaveFileName;

          /// <summary>
          /// Gets or sets the name of the halcon calibration plate.
          /// </summary>
          /// <value>The name of the halcon calibration plate.</value>
          public string HalconCalibrationPlateName
          {
               get => this.halconCalibrationPlateName;

               set => this.RaiseAndSetIfChanged(ref this.halconCalibrationPlateName, value);
          }

          /// <summary>
          /// Gets or sets the parameters for the name of the halcon calibration plate.
          /// </summary>
          /// <value>The halcon calibration plate parameters.</value>
          public HTuple HalconCalibrationPlateParameters
          {
               get => this.halconCalibrationPlateParameters;

               set => this.RaiseAndSetIfChanged(ref this.halconCalibrationPlateParameters, value);
          }

          /// <summary>
          /// Gets or sets the image height.
          /// </summary>
          /// <value>The height of the image.</value>
          public int ImageHeight
          {
               get => this.imageHeight;

               set => this.RaiseAndSetIfChanged(ref this.imageHeight, value);
          }

          /// <summary>
          /// Gets or sets the image height parameters.
          /// </summary>
          /// <value>The image height parameters.</value>
          public HTuple ImageHeightParameters
          {
               get => this.imageHeightParameters;

               set => this.RaiseAndSetIfChanged(ref this.imageHeightParameters, value);
          }

          /// <summary>
          /// Gets or sets the image width.
          /// </summary>
          /// <value>The width of the image.</value>
          public int ImageWidth
          {
               get => this.imageWidth;

               set => this.RaiseAndSetIfChanged(ref this.imageWidth, value);
          }

          /// <summary>
          /// Gets or sets the image width parameters.
          /// </summary>
          /// <value>The image width parameters.</value>
          public HTuple ImageWidthParameters
          {
               get => this.imageWidthParameters;

               set => this.RaiseAndSetIfChanged(ref this.imageWidthParameters, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether the processor is busy.
          /// </summary>
          /// <value><c>true</c> if the process is busy; otherwise, <c>false</c>.</value>
          public bool IsBusy
          {
               get => this.isBusy;

               set => this.RaiseAndSetIfChanged(ref this.isBusy, value);
          }

          /// <summary>
          /// Gets or sets a value indicating whether calibration test image is present.
          /// </summary>
          /// <value><c>true</c> if this instance is calibration test image present; otherwise, <c>false</c>.</value>
          public bool IsCalibrationTestImagePresent
          {
               get => this.isCalibrationTestImagePresent;

               set => this.RaiseAndSetIfChanged(ref this.isCalibrationTestImagePresent, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the command to load calibration images from file can execute.
          /// </summary>
          /// <value>The load calibration images from file can execute observable.</value>
          public IObservable<bool> LoadCalibImagesFromFileCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to load calibration images from file.
          /// </summary>
          /// <value>The load calibration images from file command.</value>
          public ReactiveCommand<Unit, ProcessingResult> LoadCalibImagesFromFileCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the command to load a calibration map.
          /// </summary>
          /// <value>The load calibration map command.</value>
          public ReactiveCommand<Unit, ProcessingResult> LoadCalibrationMapCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets a value indicating whether processor is loading a calibration test image.
          /// </summary>
          /// <value><c>true</c> if the test image is loading; otherwise, <c>false</c>.</value>
          public bool LoadingTestImage
          {
               get => this.loadingTestImage;

               set => this.RaiseAndSetIfChanged(ref this.loadingTestImage, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the LoadTestCalibrationImage command can execute.
          /// </summary>
          /// <value>The load test calibration image can execute observable.</value>
          public IObservable<bool> LoadTestCalibrationImageCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to load a test calibration image from file.
          /// </summary>
          /// <value>The load test calibration image command.</value>
          public ReactiveCommand<Unit, Unit> LoadTestCalibrationImageCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets the processing results.
          /// </summary>
          /// <value>The processing results.</value>
          public ProcessingResult ProcessingResults => this.processingResults.Value;

          /// <summary>
          /// Gets the processing results calibrate.
          /// </summary>
          /// <value>The processing results for the calibrate command.</value>
          public ProcessingResult ProcessingResultsCalibrate => this.processingResultsCalibrate.Value;

          /// <summary>
          /// Gets or sets the rectified image height.
          /// </summary>
          /// <value>The height of the rectified image.</value>
          public int RectifiedImageHeight
          {
               get => this.rectifiedImageHeight;

               set => this.RaiseAndSetIfChanged(ref this.rectifiedImageHeight, value);
          }

          /// <summary>
          /// Gets or sets the rectified image width.
          /// </summary>
          /// <value>The width of the rectified image.</value>
          public int RectifiedImageWidth
          {
               get => this.rectifiedImageWidth;

               set => this.RaiseAndSetIfChanged(ref this.rectifiedImageWidth, value);
          }

          /// <summary>
          /// Gets or sets the file name for saving the rectified test image.
          /// </summary>
          /// <value>The file in which to save the rectified test image.</value>
          public string RectifiedTestImageSaveName
          {
               get => this.rectifiedTestImageSaveName;

               set => this.RaiseAndSetIfChanged(ref this.rectifiedTestImageSaveName, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the RectifyImage command can execute.
          /// </summary>
          /// <value>The rectify image can execute observable.</value>
          public IObservable<bool> RectifyImageCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to rectify an image.
          /// </summary>
          /// <value>The rectify image command.</value>
          public ReactiveCommand<Unit, ProcessingResult> RectifyImageCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the  command to reset calibration images.
          /// </summary>
          /// <value>The reset calibration images command.</value>
          public ReactiveCommand<Unit, ProcessingResult> ResetCalibrationImagesCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the rotation.
          /// </summary>
          /// <value>The rotation.</value>
          public double Rotation
          {
               get => this.rotation;

               set => this.RaiseAndSetIfChanged(ref this.rotation, value);
          }

          /// <summary>
          /// Gets or sets the rotation parameters.
          /// </summary>
          /// <value>The rotation parameters.</value>
          public HTuple RotationParameters
          {
               get => this.rotationParameters;

               set => this.RaiseAndSetIfChanged(ref this.rotationParameters, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the X direction.
          /// </summary>
          /// <value>The rotation in the x direction.</value>
          public double RotX
          {
               get => this.rotX;

               set => this.RaiseAndSetIfChanged(ref this.rotX, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the X direction parameters.
          /// </summary>
          /// <value>The rotation in the x direction parameters.</value>
          public HTuple RotXParameters
          {
               get => this.rotXParameters;

               set => this.RaiseAndSetIfChanged(ref this.rotXParameters, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the Y direction.
          /// </summary>
          /// <value>The rotation in the y direction.</value>
          public double RotY
          {
               get => this.rotY;

               set => this.RaiseAndSetIfChanged(ref this.rotY, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the Y direction parameters.
          /// </summary>
          /// <value>The rotation in the y direction parameters.</value>
          public HTuple RotYParameters
          {
               get => this.rotYParameters;

               set => this.RaiseAndSetIfChanged(ref this.rotYParameters, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the Z direction.
          /// </summary>
          /// <value>The rotation in the z direction.</value>
          public double RotZ
          {
               get => this.rotZ;

               set => this.RaiseAndSetIfChanged(ref this.rotZ, value);
          }

          /// <summary>
          /// Gets or sets the rotation in the Z direction parameters.
          /// </summary>
          /// <value>The rotation in the z direction parameters.</value>
          public HTuple RotZParameters
          {
               get => this.rotZParameters;

               set => this.RaiseAndSetIfChanged(ref this.rotZParameters, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the SaveCalibrationMap command can execute.
          /// </summary>
          /// <value>The save calibration map can execute observable.</value>
          public IObservable<bool> SaveCalibrationMapCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to save a calibration map.
          /// </summary>
          /// <value>The save calibration map command.</value>
          public ReactiveCommand<Unit, ProcessingResult> SaveCalibrationMapCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the SaveRectifiedImage command can execute.
          /// </summary>
          /// <value>The save rectified image can execute observable.</value>
          public IObservable<bool> SaveRectifiedImageCanExecute
          {
               get;

               set;
          }

          /// <summary>
          /// Gets or sets the command to save a rectified image.
          /// </summary>
          /// <value>The save rectified image command.</value>
          public ReactiveCommand<Unit, ProcessingResult> SaveRectifiedImageCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the sensor size in the X direction.
          /// </summary>
          /// <value>The sensor size x.</value>
          public double SensorSizeX
          {
               get => this.sensorSizeX;

               set => this.RaiseAndSetIfChanged(ref this.sensorSizeX, value);
          }

          /// <summary>
          /// Gets or sets the sensor size in the X direction parameters.
          /// </summary>
          /// <value>The sensor size x parameters.</value>
          public HTuple SensorSizeXParameters
          {
               get => this.sensorSizeXParameters;

               set => this.RaiseAndSetIfChanged(ref this.sensorSizeXParameters, value);
          }

          /// <summary>
          /// Gets or sets the sensor size in the Y direction.
          /// </summary>
          /// <value>The sensor size y.</value>
          public double SensorSizeY
          {
               get => this.sensorSizeY;

               set => this.RaiseAndSetIfChanged(ref this.sensorSizeY, value);
          }

          /// <summary>
          /// Gets or sets the sensor size in the Y direction parameters.
          /// </summary>
          /// <value>The sensor size y parameters.</value>
          public HTuple SensorSizeYParameters
          {
               get => this.sensorSizeYParameters;

               set => this.RaiseAndSetIfChanged(ref this.sensorSizeYParameters, value);
          }

          /// <summary>
          /// Gets or sets an observable that indicates whether the SetParameters command can execute.
          /// </summary>
          /// <value>The set parameters can execute observable.</value>
          public IObservable<bool> SetParametersCanExecute
          {
               get;

               set;
          }

          //// Create properties that must be set for the process to use instead of parameters.

          //// Create properties that expose any display object properties in the Processor model.

          /// <summary>
          /// Gets or sets the SetParametersCommand.
          /// </summary>
          /// <value>The set parameters command.</value>
          public ReactiveCommand<Unit, ProcessingResult> SetParametersCommand
          {
               get;

               protected set;
          }

          /// <summary>
          /// Gets or sets the tilt.
          /// </summary>
          /// <value>The tilt.</value>
          public double Tilt
          {
               get => this.tilt;

               set => this.RaiseAndSetIfChanged(ref this.tilt, value);
          }

          /// <summary>
          /// Gets or sets the tilt parameters.
          /// </summary>
          /// <value>The tilt parameters.</value>
          public HTuple TiltParameters
          {
               get => this.tiltParameters;

               set => this.RaiseAndSetIfChanged(ref this.tiltParameters, value);
          }

          /// <summary>
          /// Gets or sets the translation in the X direction.
          /// </summary>
          /// <value>The translation in the x direction.</value>
          public double TransX
          {
               get => this.transX;

               set => this.RaiseAndSetIfChanged(ref this.transX, value);
          }

          /// <summary>
          /// Gets or sets the translation in the X direction parameters.
          /// </summary>
          /// <value>The translation in the x direction parameters.</value>
          public HTuple TransXParameters
          {
               get => this.transXParameters;

               set => this.RaiseAndSetIfChanged(ref this.transXParameters, value);
          }

          /// <summary>
          /// Gets or sets the translation in the Y direction.
          /// </summary>
          /// <value>The translation in the y direction.</value>
          public double TransY
          {
               get => this.transY;

               set => this.RaiseAndSetIfChanged(ref this.transY, value);
          }

          /// <summary>
          /// Gets or sets the translation in the Y direction parameters.
          /// </summary>
          /// <value>The translation in the y direction parameters.</value>
          public HTuple TransYParameters
          {
               get => this.transYParameters;

               set => this.RaiseAndSetIfChanged(ref this.transYParameters, value);
          }

          /// <summary>
          /// Gets or sets the translation in the Z direction.
          /// </summary>
          /// <value>The translation in the z direction.</value>
          public double TransZ
          {
               get => this.transZ;

               set => this.RaiseAndSetIfChanged(ref this.transZ, value);
          }

          /// <summary>
          /// Gets or sets the translation in the Z direction parameters.
          /// </summary>
          /// <value>The translation in the z direction parameters.</value>
          public HTuple TransZParameters
          {
               get => this.transZParameters;

               set => this.RaiseAndSetIfChanged(ref this.transZParameters, value);
          }

          /// <summary>
          /// Gets or sets the index into the poses in the camera parameters to use as the world pose.
          /// </summary>
          /// <value>The index of the world pose.</value>
          public int WorldPoseIndex
          {
               get => this.worldPoseIndex;

               set => this.RaiseAndSetIfChanged(ref this.worldPoseIndex, value);
          }

          #endregion Public Properties

          #region Public Methods

          /// <summary>
          /// Checks that the calibration map does not attempt to access image coordinates outside of the image.
          /// </summary>
          /// <param name="image">The image</param>
          /// <param name="map">The calibration map.</param>
          /// <returns>A value indicating whether the calibration map can be applied safely.</returns>
          public static bool CheckImageAgainstMap(HImage image, HImage map)
          {
               int linearSize;
               HRegion badPixels = new HRegion();
               HRegion unionBadPixels = new HRegion();

               try
               {
                    image.GetImageSize(out int imageWidth, out int imageHeight);
                    linearSize = imageWidth * imageHeight;
                    badPixels = map.Threshold(new HTuple(int.MinValue, linearSize), new HTuple(-1, int.MaxValue));
                    unionBadPixels = badPixels.Union1();
                    return unionBadPixels.AreaCenter(out double unusedDouble1, out double unusedDouble2) <= 0;
               }
               finally
               {
                    badPixels.Dispose();
                    unionBadPixels.Dispose();
               }
          }

          #endregion Public Methods

          #region Protected Methods

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

               //// Add any display objects needed as in this sample line.
               tempDC.AddDisplayObject(this.Processor.RectifiedTestImage.CopyObj(1, -1));

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

                    this.CalibrationMap?.Dispose();

                    this.CalibrationTestImage?.Dispose();

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          /// <summary>
          /// Implements the asynchronous process method for this process.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          protected override async Task<ProcessingResult> ProcessAsync()
          {
               //// Set any properties needed by the processor Process method before calling ProcessAsync.
               // this.Processor.<Some property used by the processor Process method>  = this.MainViewModelRef.<view model>.<property to use>;
               return await base.ProcessAsync();
          }

          #endregion Protected Methods

          #region Private Methods

          /// <summary>
          /// .
          /// Implements the asynchronous method to acquire calibration images.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> AcquireCalibrationImagesAsync()
          {
               await Task.Factory.StartNew(() => this.MainViewModelRef.AcquireAcquisitionVM.Processor.Process());
               return await Task.Factory.StartNew(() => this.Processor.ProcessAcquiredCalibrationImage(this.MainViewModelRef.AcquireAcquisitionVM.Image));
          }

          /// <summary>
          /// .
          /// Implements the asynchronous method to perform the calibration.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> CalibrateAsync()
          {
               return await Task.Factory.StartNew(() => this.Processor.Calibrate(
                   this.ImageWidth,
                   this.ImageHeight,
                   this.RectifiedImageWidth,
                   this.RectifiedImageHeight,
                   this.CalibratedScale * 0.001,
                   this.WorldPoseIndex));
          }

          /// <summary>
          /// .
          /// Implements the asynchronous method to load the calibration images from file.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> LoadCalibImagesFromFileAsync()
          {
               try
               {
                    this.IsBusy = true;
                    this.CalibrationImageFolderName = await this.GetFolderName.Handle(new Unit());

                    if (this.CalibrationImageFolderName != string.Empty)
                    {
                         return await Task.Factory.StartNew(() => this.Processor.LoadCalibrationImages(this.CalibrationImageFolderName));
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
                         ErrorMessage = "The Interaction to handle getting the file name is not registered."
                    };

                    return result;
               }
          }

          /// <summary>
          /// .
          /// Implements the asynchronous method to load a calibration map.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> LoadCalibrationMapAsync()
          {
               try
               {
                    this.CalibrationMapLoadFileName = await this.GetFileName.Handle(new Unit());

                    if (this.CalibrationMapLoadFileName != string.Empty)
                    {
                         return await Task.Factory.StartNew(() => this.Processor.LoadCalibrationMap(this.CalibrationMapLoadFileName));
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
                         ErrorMessage = "The Interaction to handle getting the file name is not registered."
                    };

                    return result;
               }
          }

          /// <summary>
          /// Implements the asynchronous method to rectify an image.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> RectifyImageAsync()
          {
               return await Task.Factory.StartNew(() => this.Processor.RectifyImage(this.CalibrationTestImage));
          }

          /// <summary>
          /// .
          /// Implements the asynchronous method to set all calibration images.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> ResetCalibrationImagesAsync()
          {
               return await Task.Factory.StartNew(() => this.Processor.ResetCalibrationImages());
          }

          /// <summary>
          /// Implements the asynchronous method to save a calibration map.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> SaveCalibrationMapAsync()
          {
               try
               {
                    this.CalibrationMapSaveFileName = await this.GetCalibratioinMapSaveFileName.Handle(new Unit());

                    if (this.CalibrationMapSaveFileName != string.Empty)
                    {
                         return await Task.Factory.StartNew(() => this.Processor.SaveCalibrationMap(this.CalibrationMapSaveFileName));
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
                         ErrorMessage = "The Interaction to handle getting the file name is not registered."
                    };

                    return result;
               }
          }

          /// <summary>
          /// Implements the asynchronous method to save a rectified image.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> SaveRectifiedAsync()
          {
               try
               {
                    this.RectifiedTestImageSaveName = await this.GetSaveFileName.Handle(new Unit());

                    if (this.RectifiedTestImageSaveName != string.Empty)
                    {
                         return await Task.Factory.StartNew(() => this.Processor.SaveRectifiedTestImage(this.RectifiedTestImageSaveName));
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

          /// <summary>
          /// Sets the parameters.
          /// </summary>
          private void SetParameters()
          {
               this.FocalLengthParameters = new HTuple(this.FocalLength);
               this.SensorSizeXParameters = new HTuple(this.SensorSizeX);
               this.SensorSizeYParameters = new HTuple(this.SensorSizeY);
               this.ImageHeightParameters = new HTuple(this.ImageHeight);
               this.ImageWidthParameters = new HTuple(this.ImageWidth);
               this.CameraTypeParameters = new HTuple(
                   "area_scan_division",
                   "area_scan_polynomial",
                   "area_scan_telecentric_division",
                   "area_scan_telecentric_polynomial");
               ////"area_scan_telecentric_tilt_division",
               ////"area_scan_telecentric_tilt_polynomial",
               ////"area_scan_tilt_division",
               ////"area_scan_tilt_polynomial",
               ////"line_scan");
               this.HalconCalibrationPlateParameters = new HTuple(
                    "calplate_5mm.cpd",
                    "calplate_20mm.cpd",
                    "calplate_40mm.cpd",
                    "calplate_80mm.cpd",
                    "calplate_160mm.cpd",
                    "calplate_320mm.cpd",
                    "calplate_640mm.cpd",
                    "calplate_1200mm.cpd");
          }

          /// <summary>
          /// .
          /// Implements the asynchronous method to set the parameters.
          /// </summary>
          /// <returns>A ProcessingResult instance.</returns>
          private async Task<ProcessingResult> SetParametersAsync()
          {
               return await Task.Factory.StartNew(() => this.Processor.SetInitialParameters(
                   this.CameraType,
                   this.FocalLength,
                   this.ImageWidth,
                   this.ImageHeight,
                   this.SensorSizeX * 0.001,
                   this.SensorSizeY * 0.001,
                   0,
                   0,
                   this.HalconCalibrationPlateName));
          }

          /// <summary>
          /// Set the height of the rectified image according to its aspect ratio.
          /// </summary>
          /// <param name="rectifiedWidth">The rectified width.</param>
          private void SetRectifiedImageHeightToAspectRatio(int rectifiedWidth)
          {
               double ratio = (double)this.ImageHeight / (double)this.ImageWidth;
               this.rectifiedImageSizeProgramaticCall = true;
               this.RectifiedImageHeight = (int)Math.Round((double)rectifiedWidth * ratio);
               this.rectifiedImageSizeProgramaticCall = false;
          }

          /// <summary>
          /// Set the width of the rectified image according to its aspect ratio.
          /// </summary>
          /// <param name="rectifiedHeight">The rectified height.</param>
          private void SetRectifiedImageWidthToAspectRatio(int rectifiedHeight)
          {
               double ratio = (double)this.ImageWidth / (double)this.ImageHeight;
               this.rectifiedImageSizeProgramaticCall = true;
               this.RectifiedImageWidth = (int)Math.Round((double)rectifiedHeight * ratio);
               this.rectifiedImageSizeProgramaticCall = false;
          }

          /// <summary>
          /// Validates the initial camera parameters.
          /// </summary>
          private void ValidateInitialParameters()
          {
               bool valid = true;
               double minValue = 0.00000001;

               if (this.FocalLength <= minValue)
               {
                    valid = false;
               }

               if (this.SensorSizeX <= minValue)
               {
                    valid = false;
               }

               if (this.SensorSizeY <= minValue)
               {
                    valid = false;
               }

               if (this.ImageHeight < 1)
               {
                    valid = false;
               }

               if (this.imageWidth < 1)
               {
                    valid = false;
               }

               //// camera type must be of approved type for this application.

               this.AreInitialParametersValid = valid;
          }

          #endregion Private Methods
     }
}