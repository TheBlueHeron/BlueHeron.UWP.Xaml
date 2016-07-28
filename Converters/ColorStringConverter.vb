Imports Windows.UI

''' <summary>
''' Value converter that translates a <see cref="Color"/> or a <see cref="SolidColorBrush"/> to its string representation and back.
''' </summary>
Public Class ColorStringConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If TypeOf (value) Is Color Then
			Return DirectCast(value, Color).ToString
		ElseIf TypeOf (value) Is SolidColorBrush Then
			Return DirectCast(value, SolidColorBrush).ToString
		Else
			Return "Unknown"
		End If

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		If targetType Is GetType(Color) Then
			Return GetColorFromString(value.ToString)
		ElseIf targetType Is GetType(SolidColorBrush) Then
			Return New SolidColorBrush(GetColorFromString(value.ToString))
		Else
			Return Colors.Black
		End If

	End Function

End Class