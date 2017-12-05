//-----------------------------------------------------------------------
// <copyright file="OcrViewModel.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.ViewModels
{
     using System;
     using System.Reactive.Linq;
     using System.Threading.Tasks;
     using HalconDotNet;
     using Models;
     using ReactiveUI;
     using Rti.DisplayUtilities;
     using Rti.ViewRoiCore;
     using Rti.Halcon;

     /// <summary>
     /// View model for the Ocr process.
     /// </summary>
     public class OcrViewModel : ProcessViewModelBase<MainViewModel, OcrProcessor>
     {
          #region Private Fields

          private HImage halconImage = new HImage();

          private int imageWidth, imageHeight;

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          /// <summary>
          /// Stores the ProcessingResult returned from ProcessAsync.ToProperty call.
          /// </summary>
          private ObservableAsPropertyHelper<ProcessingResult> processingResults;

          #endregion Private Fields

          //// Create an ObservableAsPropertyHelper<ProcessingResult> for each command with a ProcessingResult you need to monitor.

          /////// <summary>
          /////// Stores the ProcessingResult returned from MyProcessAsync.ToProperty call.
          /////// </summary>
          ////private ObservableAsPropertyHelper<ProcessingResult> myProcessingResults;
          //// Create backing fields for the properties as needed.

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the OcrViewModel class.
          /// </summary>
          /// <param name="mainVM">A reference to to main view model.</param>
          /// <param name="processor">An instance of the processor class for this view model.</param>
          public OcrViewModel(IMainViewModel mainVM, IProcessor processor)
                 : base(mainVM, processor)
          {
               //// If a CanExecute condition needs to be set, do it here and recreate the Command using
               //// the CanExecute object.
               //// this.CanExecute = this.WhenAny(x => x.MainViewModelRef.<some property>, x => x.Value == false);
               //// this.Command = ReactiveCommand.CreateFromTask(_ => this.ProcessAsync(), this.CanExecute);

               // To force immediate disposal of large or Iconic objects, uncomment the Do clause line,
               // otherwise the GarbageCollection will do it after enough memory is used.
               this.Command
                    // .Do(_ => this.ProcessingResults?.Dispose())
                    .ToProperty(this, x => x.ProcessingResults, out this.processingResults);

               //// Initialize addition commands, with or without can execute reference, as needed.
               ////this.MyCommand = ReactiveCommand.CreateFromTask(_ => this.MyProcessAsync());
               ////this.MyCommand = ReactiveCommand.CreateFromTask(_ => this.MyProcessAsync(), this.MyCanExecute);
               //// Monitor ProcessingResults separately for each command.
               ////this.MyCommand.ToProperty(this, x => x.MyProcessingResults, out this.myProcessingResults);

               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.Processor.DebugDisplay)
                   .Where(x => x.DisplayList.Count > 0)
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(x =>
                   {
                        this.DebugDisplay.Dispose();
                        this.DebugDisplay = x;
                   }));

               //// Set up the display to rebuild if a reactive display property changes.
               this.DisposeCollection.Add(
                   this.WhenAnyValue(x => x.Processor.ProcessedRegion)
                   .Where(x => x.IsValid())
                   .SubscribeOn(RxApp.TaskpoolScheduler)
                   .Subscribe(_ =>
                   {
                        this.SetDisplay();
                   }));

               //// Set up the display to rebuild if the ProcessingResults change and
               //// a specific value in the collection is valid.
               ////this.DisposeCollection.Add(
               ////    this.WhenAnyValue(x => x.ProcessingResults)
               ////    .Where(x => x != null)
               ////    .Where(x => x.ResultsCollection.ContainsKey("<Key-Of-Item>"))
               ////    .Select(x => (<Type-Of-Value>)x.ResultsCollection["<Key-Of-Item>"])
               ////    .Where(y => y <condition>) // example: .Where(y => y.IsInitialized())
               ////    .SubscribeOn(RxApp.TaskpoolScheduler)
               ////    .Subscribe(_ =>
               ////    {
               ////        this.SetDisplay();
               ////    }));

               //// This reacts to the execution of the command by resetting the AppState. Modify as needed.
               this.DisposeCollection.Add(this.Command
                   .Subscribe(_ =>
                   {
                        this.MainViewModelRef.AppState = this.MainViewModelRef.AppState == 0 ? this.MainViewModelRef.LastAppState : this.MainViewModelRef.AppState;
                   }));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.HalconImage)
                    .Where(x => x.IsValid())
                    .Select(_ => System.Reactive.Unit.Default)
                    .InvokeCommand(this.Command));

               //this.DisposeCollection.Add(this.WhenAnyValue(x => x.MainViewModelRef.LoadImageVM.Image)
               //     .Where(x => x != null)
               //     .Where(x => x.IsInitialized())
               //     .Select(_ => System.Reactive.Unit.Default)
               //     .InvokeCommand(this.Command));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.MainViewModelRef.LoadImageVM.Image)
                    .Where(x => x.IsValid())
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(x => this.HalconImage = x));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.MainViewModelRef.AcquireAcquisitionVM.Image)
                    .Where(x => x.IsValid())
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(x => this.HalconImage = x));

               this.DisposeCollection.Add(this.WhenAnyValue(x => x.HalconImage)
                    .Where(x => x.IsValid())
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(x =>
                    {
                         x.GetImageSize(out int imageWidth, out int imageHeight);
                         this.ImageWidth = imageWidth;
                         this.ImageHeight = imageHeight;
                    }));
          }

          #endregion Public Constructors

          //// Create additional reactive commands as needed.

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

          #region Public Properties

          public HImage HalconImage { get => this.halconImage; set => this.RaiseAndSetIfChanged(ref this.halconImage, value); }

          public int ImageHeight { get => this.imageHeight; set => this.RaiseAndSetIfChanged(ref this.imageHeight, value); }

          public int ImageWidth { get => this.imageWidth; set => this.RaiseAndSetIfChanged(ref this.imageWidth, value); }

          /// <summary>
          /// Gets the processing results.
          /// </summary>
          public ProcessingResult ProcessingResults
          {
               get
               {
                    return this.processingResults.Value;
               }
          }

          #endregion Public Properties

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

               //// Add any display objects needed as in these sample lines.
               tempDC.AddDisplayObject(this.Processor.ProcessedImage.CopyObj(1, -1));
               ////tempDC.AddDisplayObject(this.Processor.ProcessedRegion.CopyObj(1, -1), Rti.ViewRoiCore.HalconColors.Red, 1, Rti.ViewRoiCore.DrawModes.Margin);
               tempDC.AddDisplayObject(this.Processor.ProcessedRegion.CopyObj(1, -1), true, Rti.ViewRoiCore.ColoredCounts.Twelve, 2, Rti.ViewRoiCore.DrawModes.Margin);

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
               // This default version will call the Processor.Process() method with no parameters.
               // If desired, properties can be manually set in the Processor here. Passing parameters in the second version is preferred.
               //// return await base.ProcessAsync();

               // To pass parameters, use this version to call the Processor.Process(object parameters) overload.
               // Create a defined non-generic Tuple containing the parameters.
               // Dummy parameters. Change this.
               Tuple<HImage> parameters = new Tuple<HImage>(this.HalconImage.CopyObj(1, -1));
               //// Example: Tuple<HImage, double> parameters = new Tuple<HImage, double>(this.MainViewModelRef.LoadImageVM.Image, this.PropertyNameOfDoubleType);
               return await Task.Factory.StartNew(() => this.Processor.Process(parameters));
          }

          #endregion Protected Methods

          //// Create additional async methods for commands.

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
     }
}