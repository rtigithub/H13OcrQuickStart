// ***********************************************************************
// Assembly         : H13OcrQuickStart
// Author           : 
// Created          : 11-30-2017
// Last Modified On : 12-05-2017
// <copyright file="RadioButtonCheckedToEnumConverter.cs" company="Resolution Technology, Inc.">
//     Copyright ©  2016, 2017 Resolution Technology, Inc.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace H13OcrQuickStart.View
{
     using System;
     using System.Windows.Data;

     /// <summary>
     /// IValueConverter from radio button checked property to an enumeration type determined from usage.
     /// </summary>
     /// <seealso cref="System.Windows.Data.IValueConverter" />
     public class RadioButtonCheckedToEnumConverter : IValueConverter
     {
          #region Public Methods

          /// <summary>
          /// Converts from the enumeration type to the value type, a boolean for the IsChecked property.
          /// </summary>
          /// <param name="value">The value of the radio button.</param>
          /// <param name="enumType">The type of the enumeration.</param>
          /// <param name="parameter">The converter parameter.</param>
          /// <param name="culture">The culture.</param>
          /// <returns>The boolean value.</returns>
          public object Convert(object value, Type enumType, object parameter, System.Globalization.CultureInfo culture) =>
                 parameter.Equals(value);

          /// <summary>
          /// Returns the enumeration value of the parameter if value is true.
          /// </summary>
          /// <param name="value">The value of the radio button.</param>
          /// <param name="enumType">The type of the enumeration.</param>
          /// <param name="parameter">The converter parameter.</param>
          /// <param name="culture">The culture.</param>
          /// <returns>The enumeration value.</returns>
          public object ConvertBack(object value, Type enumType, object parameter, System.Globalization.CultureInfo culture) =>
              value.Equals(true) ? parameter : Binding.DoNothing;

          #endregion Public Methods
     }
}