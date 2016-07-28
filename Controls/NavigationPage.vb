
''' <summary>
''' 
''' </summary>
Public MustInherit Class NavigationPage
	Inherits Page

#Region " Objects and variables "

	Private m_NavigationHelper As NavigationHelper

#End Region

#Region " Properties "

	Public ReadOnly Property NavigationHelper As NavigationHelper
		Get
			Return m_NavigationHelper
		End Get
	End Property

#End Region

#Region " Private methods and functions "

	Protected Sub InitializeNavigationHelper(page As Page)

		m_NavigationHelper = New NavigationHelper(page)
		AddHandler m_NavigationHelper.LoadState, AddressOf NavigationHelper_LoadState
		AddHandler m_NavigationHelper.SaveState, AddressOf NavigationHelper_SaveState

	End Sub

	Protected MustOverride Sub NavigationHelper_LoadState(sender As Object, e As LoadStateEventArgs)

	Protected MustOverride Sub NavigationHelper_SaveState(sender As Object, e As SaveStateEventArgs)

#End Region

End Class