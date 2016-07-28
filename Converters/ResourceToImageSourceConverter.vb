
''' <summary>
''' Converts the resource with the given key to an <see cref="ImageSource" /> object.
''' </summary>
Public Class ResourceToImageSourceConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, application As Object, language As String) As Object Implements IValueConverter.Convert

        Return CType(DirectCast(application, Application).Resources(value.ToString), ImageSource)

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack

        Throw New NotImplementedException()

    End Function

End Class