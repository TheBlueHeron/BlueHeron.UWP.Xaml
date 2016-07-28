
''' <summary>
''' Converts a value to its string representation.
''' </summary>
Public Class ValueToStringConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		Return value.ToString

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		Throw New NotImplementedException()

	End Function

End Class