
''' <summary>
''' Object that provides grouping for <see cref="PageDefinition">Page definitions</see>.
''' </summary>
Public Class PageCategory
	Implements INotifyPropertyChanged

#Region " Objects and variables "

	Private m_IsEnabled As Boolean = True
	Private m_IsVisible As Boolean = True

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Properties "

	''' <summary>
	''' Description of this object, that can be used as a tooltip.
	''' </summary>
	Public Property Description As String

	''' <summary>
	''' A glyph expression to symbolize this object.
	''' </summary>
	''' <seealso>https://msdn.microsoft.com/en-us/windows/uwp/style/segoe-ui-symbol-font</seealso>
	Public Property Glyph As String

	''' <summary>
	''' Determines whether this object should be enabled.
	''' </summary>
	''' <remarks>Default: True</remarks>
	Public Property IsEnabled As Boolean
		Get
			Return m_IsEnabled
		End Get
		Set(value As Boolean)
			If value <> m_IsEnabled Then
				m_IsEnabled = value
				OnPropertyChanged("IsEnabled")
			End If
		End Set
	End Property

	''' <summary>
	''' Determines whether this object should be visible.
	''' </summary>
	''' <remarks>Default: True</remarks>
	Public Property IsVisible As Boolean
		Get
			Return m_IsVisible
		End Get
		Set(value As Boolean)
			If value <> m_IsVisible Then
				m_IsVisible = value
				OnPropertyChanged("IsVisible")
			End If
		End Set
	End Property

	''' <summary>
	''' Index number that can be used for sorting.
	''' </summary>
	Public Property OrderNo As Integer

	''' <summary>
	''' Title of this object.
	''' </summary>
	Public Property Title As String

#End Region

#Region " Private methods and functions "

	Protected Async Sub OnPropertyChanged(propertyName As String)

		Await RootPage.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub()
																									  RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
																								  End Sub)

	End Sub

#End Region

End Class

''' <summary>
''' Displayable definition of a <see cref="Page"/>. 
''' </summary>
Public Class PageDefinition
    Inherits PageCategory

#Region " Properties "

    ''' <summary>
    ''' The <see cref="PageCategory">category</see> to which this definition belongs.
    ''' </summary>
    Public Property Category As PageCategory

    ''' <summary>
    ''' The type of the <see cref="Page"/> that is defined by this <see cref="PageDefinition"/>.  
    ''' </summary>
    Public Property PageType As Type

#End Region

End Class

''' <summary>
''' Obervable collection of <see cref="PageDefinition" /> objects, extended to supply <see cref="PageCategory">categories</see> and <see cref="PageDefinition"/> objects per category.
''' </summary>
Public Class PageDefinitionCollection
	Inherits ObservableCollection(Of PageDefinition)

	''' <summary>
	''' Returns a collection of available <see cref="PageCategory"/> objects.
	''' </summary>
	Public Property Categories As ReadOnlyObservableCollection(Of PageCategory)
		Get
			Return New ReadOnlyObservableCollection(Of PageCategory)(New ObservableCollection(Of PageCategory)([Select](Function(pd) pd.Category).Where(Function(c) c.IsVisible).Distinct(New PageCategoryComparer).OrderBy(Of Integer)(Function(pc) pc.OrderNo)))
		End Get
		Set(value As ReadOnlyObservableCollection(Of PageCategory))
		End Set
	End Property

	''' <summary>
	''' Returns a collection of <see cref="PageDefinition" /> objects, belonging to the <see cref="PageCategory" /> with the given title.
	''' </summary>
	Public Property Pages(category As String) As ReadOnlyObservableCollection(Of PageDefinition)
		Get
			Return New ReadOnlyObservableCollection(Of PageDefinition)(New ObservableCollection(Of PageDefinition)([Where](Function(pd) pd.IsVisible AndAlso pd.Category.Title = category).OrderBy(Of Integer)(Function(pd) pd.OrderNo)))
		End Get
		Set(value As ReadOnlyObservableCollection(Of PageDefinition))
		End Set
	End Property

End Class

Public Class PageDefinitionViewModel
	Implements INotifyPropertyChanged

#Region " Objects and variables "

	Private m_PageDefinitions As PageDefinitionCollection

	Private m_SelectedPage As PageDefinition
	Private m_SelectedCategory As PageCategory

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Properties "

	Public ReadOnly Property PageCategories As ReadOnlyObservableCollection(Of PageCategory)
		Get
			Return PageDefinitions.Categories
		End Get
	End Property

	Public ReadOnly Property PageDefinitions As PageDefinitionCollection
		Get
			If m_PageDefinitions Is Nothing Then
				m_PageDefinitions = New PageDefinitionCollection
			End If
			Return m_PageDefinitions
		End Get
	End Property

	Public ReadOnly Property Pages(category As String) As ReadOnlyObservableCollection(Of PageDefinition)
		Get
			Return PageDefinitions.Pages(category)
		End Get
	End Property

	Public Property SelectedCategory As PageCategory
		Get
			Return m_SelectedCategory
		End Get
		Set(value As PageCategory)
			If Not value Is m_SelectedCategory Then
				m_SelectedCategory = value
				OnPropertyChanged("SelectedCategory")
			End If
		End Set
	End Property

	Public Property SelectedPage As PageDefinition
		Get
			Return m_SelectedPage
		End Get
		Set(value As PageDefinition)
			If Not value Is m_SelectedPage Then
				m_SelectedPage = value
				OnPropertyChanged("SelectedPage")
			End If
		End Set
	End Property

#End Region

#Region " Public methods and functions "

	Public Sub SetPageDefinitions(definitions As List(Of PageDefinition))

		PageDefinitions.Clear()
		definitions.ForEach(Sub(d)
								m_PageDefinitions.Add(d)
							End Sub)

	End Sub

#End Region

#Region " Private methods and functions "

	Protected Async Sub OnPropertyChanged(propertyName As String)

		Await RootPage.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, Sub()
																									  RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
																								  End Sub)

	End Sub

#End Region

#Region " Construction "

	Public Sub New()
	End Sub

	Public Sub New(definitions As List(Of PageDefinition))

		SetPageDefinitions(definitions)

	End Sub

#End Region

End Class