Imports System.Reflection
Imports Windows.UI

''' <summary>
''' Commonly used functions.
''' </summary>
Friend Module Util

	Friend Function GetColorFromString(value As String) As Color
		Dim prop As PropertyInfo = GetType(Colors).GetRuntimeProperty(value) ' Try to get the brush from the Colors shared class

		If Not prop Is Nothing Then
			Dim color As Object = prop.GetValue(Nothing)

			If Not color Is Nothing Then
				Return DirectCast(color, Color)
			End If
		End If

		Return Colors.Black

	End Function

End Module