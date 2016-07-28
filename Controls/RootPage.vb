

''' <summary>
''' <see cref="Page"/> that exposes a shared reference to itself, so the instance can be accessed by other <see cref="Page"/> instances.
''' </summary>
Public MustInherit Class RootPage
	Inherits Page

#Region " Objects and variables "

	Public Shared Current As RootPage

	Private m_Qualifiers As IObservableMap(Of String, String)
	Private m_RootNavigationHelper As RootFrameNavigationHelper

	Private Const _DF As String = "DeviceFamily"
	Private Const _MOB As String = "Mobile"

#End Region

#Region " Properties "

	Public ReadOnly Property IsMobile As Boolean
		Get
			If m_Qualifiers Is Nothing Then
				m_Qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues
			End If

			Return (m_Qualifiers.ContainsKey(_DF) AndAlso m_Qualifiers(_DF) = _MOB)
		End Get
	End Property

	Public ReadOnly Property IsRootFrameNavigationHelperInitialized As Boolean
		Get
			Return (Not m_RootNavigationHelper Is Nothing)
		End Get
	End Property

#End Region

#Region " Public methods and functions "

	Public MustOverride Sub NotifyUser(strMessage As String, type As NotifyType)

#End Region

#Region " Private methods and functions "

	Protected Sub InitializeRootFrameNavigationHelper(frame As Frame)

		m_RootNavigationHelper = New RootFrameNavigationHelper(frame)

	End Sub

#End Region

#Region " Construction "

	Public Sub New()

		Current = Me

	End Sub

#End Region

End Class