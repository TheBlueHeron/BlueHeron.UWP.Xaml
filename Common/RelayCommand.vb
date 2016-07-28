
''' <summary>
''' A command whose sole purpose is to relay its functionality to other objects by invoking delegates. 
''' The default return value for the CanExecute method Is 'true'.
''' <see cref="RaiseCanExecuteChanged"/> needs to be called whenever <see cref="CanExecute"/> is expected to return a different value.
''' </summary>
Public Class RelayCommand
	Implements ICommand

#Region " Objects and variables "

	Private ReadOnly m_CanExecute As Func(Of Boolean)
	Private ReadOnly m_Execute As Action

	''' <summary>
	''' Raised when RaiseCanExecuteChanged is called.
	''' </summary>
	Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

#End Region

#Region " Public methods and functions "

	Public Sub Execute(parameter As Object) Implements ICommand.Execute

		m_Execute()

	End Sub

	Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute

		Return If(m_CanExecute Is Nothing, True, m_CanExecute())

	End Function

	''' <summary>
	''' Method used to raise the <see cref="CanExecuteChanged"/> event to indicate that the return value of the <see cref="CanExecute"/> method has changed.
	''' </summary>
	Public Sub RaiseCanExecuteChanged()

		RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)

	End Sub

#End Region

#Region " Construction "

	''' <summary>
	''' Creates a new RelayCommand that can always execute.
	''' </summary>
	''' <param name="execute">The execution logic.</param>
	Public Sub New(execute As Action)

		Me.New(execute, Nothing)

	End Sub

	''' <summary>
	''' Creates a new RelayCommand.
	''' </summary>
	''' <param name="execute">The execution logic.</param>
	''' <param name="canExecute">The execution status logic.</param>
	Public Sub New(execute As Action, canExecute As Func(Of Boolean))

		If execute Is Nothing Then
			Throw New ArgumentNullException("execute")
		End If
		m_Execute = execute
		m_CanExecute = canExecute

	End Sub

#End Region

End Class