
''' <summary>
''' Value converter that translates double precision values to integer values and back.
''' </summary>
Public Class DoubleToIntConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		If (Not value Is Nothing) AndAlso (TypeOf (value) Is Double) Then
			Dim retVal As Integer

			If Integer.TryParse(value.ToString, retVal) Then
				Return retVal
			End If
		End If

		Return Nothing

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

		If (Not value Is Nothing) AndAlso (TypeOf (value) Is Integer) Then
			Return CDbl(value)
		End If

		Return Nothing

	End Function

End Class