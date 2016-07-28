
''' <summary>
''' Converts a string or fontfamilyt value to a <see cref="FontFamily"/>.
''' </summary>
Public Class ValueToFontFamilyConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If Not value Is Nothing Then
			If TypeOf (value) Is FontFamily Then
				Return DirectCast(value, FontFamily)
			ElseIf TypeOf (value) Is String Then
				Return New FontFamily(value.ToString)
			End If
		End If

		Return New FontFamily("Segoe UI")

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		Throw New NotImplementedException()

	End Function

End Class