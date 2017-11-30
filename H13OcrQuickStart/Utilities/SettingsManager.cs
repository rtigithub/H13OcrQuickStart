//-----------------------------------------------------------------------
// <copyright file="SettingsManager.cs" company="Resolution Technology, Inc.">
//     Copyright (c) Resolution Technology, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace H13OcrQuickStart.Utilities
{
     using System;
     using System.IO;
     using System.Linq.Expressions;
     using System.Reactive.Linq;
     using System.Reflection;
     using System.Xml.Serialization;
     using ReactiveUI;
     using ViewModels;

     /// <summary>
     /// SettingsManager class manages a set of reactive properties to preserve them via serialization.
     /// </summary>
     public class SettingsManager : SettingsManagerBase<MainViewModel>
     {
          //// Modify these  two constants as needed to set the directory and file name of the settings file.

          #region Private Fields

          /// <summary>
          /// Constant for Settings Directory Name.
          /// </summary>
          private const string SettingsDirectoryName = "H13OcrQuickStart";

          /// <summary>
          /// Constant for Settings File Name.
          /// </summary>
          private const string SettingsFileName = "Settings.xml";

          /// <summary>
          /// Stores a value indicating whether the class has been disposed.
          /// </summary>
          private bool isDisposed = false;

          #endregion Private Fields

          //// Copy the backing fields for all properties in other ProcessorViewModel classes that are to be included in the settings.

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the SettingsManager class.
          /// </summary>
          /// <param name="mainVM">A reference to the main view model.</param>
          public SettingsManager(IMainViewModel mainVM)
              : base(mainVM)
          {
               this.SetBindings();

               //// Add any special bindings here.
          }

          /// <summary>
          /// Initializes a new instance of the SettingsManager class. Used by the Deserializer only.
          /// </summary>
          public SettingsManager()
              : base()
          {
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets the Path and name for the settings file.
          /// </summary>
          public static string Location
          {
               get
               {
                    string userDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string photometricStereoDirectory = userDataDirectory + "\\" + SettingsDirectoryName;
                    if (!Directory.Exists(photometricStereoDirectory))
                    {
                         Directory.CreateDirectory(photometricStereoDirectory);
                    }

                    return userDataDirectory + "\\" + SettingsDirectoryName + "\\" + SettingsFileName;
               }
          }

          #endregion Public Properties

          //// Copy the properties in other ProcessorViewModel classes that are to be included in the settings exactly.
          //// The name of the property must not be changed.

          #region Public Methods

          /// <summary>
          /// Deserializes the current instance of SettingsManager class
          /// </summary>
          /// <returns>The created instance of a SettingsManager class.</returns>
          public static SettingsManager LoadSettings()
          {
               XmlSerializer deserializer = new XmlSerializer(typeof(SettingsManager));
               using (TextReader writer = new StreamReader(Location))
               {
                    return (SettingsManager)deserializer.Deserialize(writer);
               }
          }

          /// <summary>
          /// Overrides the SaveSettings method of the base class. Serializes the settings to the
          /// file in the location that is set.
          /// </summary>
          public override void SaveSettings()
          {
               XmlSerializer serializer = new XmlSerializer(typeof(SettingsManager));
               using (TextWriter writer = new StreamWriter(Location))
               {
                    serializer.Serialize(writer, this);
               }
          }

          /// <summary>
          /// Overrides the SetBindings method from the base class. Sets the synchronized bindings of
          /// properties to be included in the settings.
          /// </summary>
          public override void SetBindings()
          {
               //// Call BindingUtilities.BindSettingsProperty for each property added to this class to bind it to it's target.
               //// Example: BindingUtilities.BindSettingsProperty</*Type of the property*/,*Type of the target class*/>(/*Name of the VM class instance*/, /*Name of the property*/, this);
               //// For convenience, generate the name by calling nameof on the calling class. The name of the property is the same in both classes, so use "this.Name".

               this.IsLoading = false;
          }

          #endregion Public Methods

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
                    }

                    //// Dispose of unmanaged resources here.

                    this.isDisposed = true;
               }

               // Call base.Dispose, passing parameter.
               base.Dispose(disposing);
          }

          #endregion Protected Methods
     }
}