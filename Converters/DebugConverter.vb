
''' <summary>
''' Converter to facilitate debugging binding values.
''' </summary>
Public Class DebugConverter
	Implements IValueConverter

	Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert

		Debugger.Break()

		Return value ' do nothing

	End Function

	Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
		Throw New NotImplementedException()
	End Function

End Class

''' <summary>
''' Markup extension to debug databinding.
''' </summary>
''' <example>
''' Code in App startup:
''' <code>Windows.UI.Xaml.Resources.CustomXamlResourceLoader.Current = New DebugBindingExtension</code>
''' Xaml markup:
''' <code><Image Source="{Binding Path=WiFiImage, Converter={CustomResource Boo}}" Stretch="UniformToFill"/><!-- Boo can be whatever --></code>
''' </example>
Public Class DebugBindingExtension
	Inherits Windows.UI.Xaml.Resources.CustomXamlResourceLoader

	Protected Overrides Function GetResource(resourceId As String, objectType As String, propertyName As String, propertyType As String) As Object

		Return New DebugConverter

	End Function

End Class