Imports System.Reflection
Imports Windows.UI

''' <summary>
''' Converts a string value to a brush.
''' </summary>
Public Class StringToBrushConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If TypeOf (value) Is ComboBoxItem Then
			Dim brushName As String = DirectCast(value, ComboBoxItem).Content.ToString

			If Application.Current.Resources.ContainsKey(brushName) Then ' see if it's defined as a resource
				Return CType(Application.Current.Resources(brushName), Brush)
			End If

			Dim col As Color = GetColorFromString(value.ToString)

			Return New SolidColorBrush(col)
		End If

		Return New SolidColorBrush(Colors.Black)

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		Throw New NotImplementedException()

	End Function

End Class