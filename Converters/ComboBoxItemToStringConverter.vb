
''' <summary>
''' Converts the content of the given <see cref="Windows.UI.Xaml.Controls.ComboBoxItem"/> to string.
''' </summary>
Public Class ComboBoxItemToStringConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If TypeOf (value) Is Windows.UI.Xaml.Controls.ComboBoxItem Then
			Return DirectCast(value, Windows.UI.Xaml.Controls.ComboBoxItem).Content
		End If

		Return Nothing

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		Throw New NotImplementedException()

	End Function

End Class