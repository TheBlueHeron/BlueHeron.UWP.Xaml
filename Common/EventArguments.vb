
''' <summary>
''' Class used to hold the event data required when a page attempts to load state.
''' </summary>
Public Class LoadStateEventArgs
	Inherits EventArgs

#Region " Objects and variables "

	Private m_NavigationParameter As Object
	Private m_PageState As Dictionary(Of String, Object)

#End Region

#Region " Properties "

	''' <summary>
	''' The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
	''' </summary>
	Public ReadOnly Property NavigationParameter As Object
		Get
			Return m_NavigationParameter
		End Get
	End Property

	''' <summary>
	''' A dictionary of state preserved by this page during an earlier session.  This will be null the first time a page Is visited.
	''' </summary>
	Public ReadOnly Property PageState As Dictionary(Of String, Object)
		Get
			Return m_PageState
		End Get
	End Property

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a New instance of the <see cref="LoadStateEventArgs"/> class.
	''' </summary>
	''' <param name="parameter">
	''' The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
	''' </param>
	''' <param name="state">
	''' A dictionary of state preserved by this page during an earlier session.  This will be null the first time a page Is visited.
	''' </param>
	Public Sub New(parameter As Object, state As Dictionary(Of String, Object))

		MyBase.New
		m_NavigationParameter = parameter
		m_PageState = state

	End Sub

#End Region

End Class

''' <summary>
''' Class used to hold the event data required when a page attempts to save state.
''' </summary>
Public Class SaveStateEventArgs
	Inherits EventArgs

#Region " Objects and variables "

	Private m_PageState As Dictionary(Of String, Object)

#End Region

#Region " Properties "

	''' <summary>
	''' An empty dictionary to be populated with serializable state.
	''' </summary>
	Public ReadOnly Property PageState As Dictionary(Of String, Object)
		Get
			Return m_PageState
		End Get
	End Property

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a New instance of the <see cref="SaveStateEventArgs"/> class.
	''' </summary>
	''' <param name="state">
	''' An empty dictionary to be populated with serializable state.
	''' </param>
	Public Sub New(state As Dictionary(Of String, Object))

		MyBase.New
		m_PageState = state

	End Sub

#End Region

End Class