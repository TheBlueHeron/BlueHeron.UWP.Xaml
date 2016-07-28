
''' <summary>
''' Event arguments for SuspensionManager failures.
''' </summary>
Public Class SuspensionManagerException
	Inherits Exception

	Public Sub New()

		MyBase.New

	End Sub

	Public Sub New(e As Exception)

		MyBase.New("Suspensionmanager failed.", e)

	End Sub

End Class