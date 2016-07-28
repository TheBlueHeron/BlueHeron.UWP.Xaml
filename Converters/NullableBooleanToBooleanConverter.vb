
''' <summary>
''' Value converter that translates as Boolean? to a Boolean and back.
''' </summary>
Public Class NullableBooleanToBooleanConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If TypeOf (value) Is Nullable(Of Boolean) Then
			Return CBool(value)
		End If

		Return False

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		If TypeOf (value) Is Boolean Then
			Return CType(value, Nullable(Of Boolean))
		End If

		Return False

	End Function

End Class