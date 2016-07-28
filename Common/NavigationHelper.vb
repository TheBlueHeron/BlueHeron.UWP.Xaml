Imports Windows.System
Imports Windows.UI.Core
Imports Windows.UI.Input

''' <summary>
''' NavigationHelper aids in navigation between pages. It manages the backstack and integrates SuspensionManager to handle process
''' lifetime management And state management when navigating between pages.
''' </summary>
''' <example>
''' To make use of NavigationHelper, follow these two steps or start with a BasicPage or any other Page item template other than BlankPage.
''' 
''' 1) Create an instance of the NavigationHelper somewhere such as in the constructor for the page And register a callback for the LoadState and SaveState events.
''' <code>
'''     Public MyPage()
'''     {
'''         this.InitializeComponent();
'''         this.navigationHelper = New NavigationHelper(this);
'''         this.navigationHelper.LoadState += navigationHelper_LoadState;
'''         this.navigationHelper.SaveState += navigationHelper_SaveState;
'''     }
'''     
'''     private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
'''     { }
'''     private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
'''     { }
''' </code>
''' 
''' 2) Register the page to call into the NavigationManager whenever the page participates 
'''     in navigation by overriding the <see cref="Windows.UI.Xaml.Controls.Page.OnNavigatedTo"/> 
'''     And <see cref="Windows.UI.Xaml.Controls.Page.OnNavigatedFrom"/> events.
''' <code>
'''     protected override void OnNavigatedTo(NavigationEventArgs e)
'''     {
'''         navigationHelper.OnNavigatedTo(e);
'''     }
'''     
'''     protected override void OnNavigatedFrom(NavigationEventArgs e)
'''     {
'''         navigationHelper.OnNavigatedFrom(e);
'''     }
''' </code>
''' </example>
<Metadata.WebHostHidden>
Public Class NavigationHelper
	Inherits DependencyObject

#Region " Objects and variables "

	Private m_PageKey As String

	''' <summary>
	''' Handle this event to populate the page using content passed during navigation as well as any state that was saved by the SaveState event handler.
	''' </summary>
	Public Event LoadState As EventHandler(Of LoadStateEventArgs)
	''' <summary>
	''' Handle this event to save state that can be used by the LoadState event handler. Save the state in case the application is suspended or the page is discarded from the navigation cache.
	''' </summary>
	Public Event SaveState As EventHandler(Of SaveStateEventArgs)

#End Region

#Region " Properties "

	Private ReadOnly Property Frame As Frame
		Get
			Return Page.Frame
		End Get
	End Property

	Private Property Page As Page

#End Region

#Region " Public methods and functions "

	''' <summary>
	''' Invoked when this page is about to be displayed in a Frame.
	''' This method calls <see cref="LoadState"/>, where all page specific navigation and process lifetime management logic should be placed.
	''' </summary>
	''' <param name="e">Event data that describes how this page was reached.  The Parameter property provides the group to be displayed.</param>
	Public Sub OnNavigatedTo(e As NavigationEventArgs)
		Dim frameState As Dictionary(Of String, Object) = SuspensionManager.SessionStateForFrame(Me.Frame)

		m_PageKey = "Page-" & Me.Frame.BackStackDepth

		If e.NavigationMode = NavigationMode.New Then
			' Clear existing state for forward navigation when adding a new page to the navigation stack
			Dim nextPageKey As String = m_PageKey
			Dim nextPageIndex As Integer = Me.Frame.BackStackDepth

			Do While frameState.Remove(nextPageKey)
				nextPageIndex += 1
				nextPageKey = "Page-" & nextPageIndex
			Loop

			' Pass the navigation parameter to the new page
			RaiseEvent LoadState(Me, New LoadStateEventArgs(e.Parameter, Nothing))
		Else
			' Pass the navigation parameter and preserved page state to the page, using the same strategy for loading suspended state and recreating pages discarded from cache
			RaiseEvent LoadState(Me, New LoadStateEventArgs(e.Parameter, CType(frameState(m_PageKey), Dictionary(Of String, Object))))
		End If

	End Sub

	''' <summary>
	''' Invoked when this page will no longer be displayed in a Frame.
	''' This method calls <see cref="SaveState"/>, where all page specific navigation And process lifetime management logic should be placed.
	''' </summary>
	''' <param name="e">Event data that describes how this page was reached. The Parameter property provides the group to be displayed.</param>
	Public Sub OnNavigatedFrom(e As NavigationEventArgs)
		Dim frameState As Dictionary(Of String, Object) = SuspensionManager.SessionStateForFrame(Me.Frame)
		Dim pageState As New Dictionary(Of String, Object)

		RaiseEvent SaveState(Me, New SaveStateEventArgs(pageState))

		frameState(m_PageKey) = pageState

	End Sub

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a New instance of the <see cref="NavigationHelper"/> class.
	''' </summary>
	''' <param name="page">A reference to the current page used for navigation. This reference allows for frame manipulation.</param>
	Public Sub New(page As Page)

		Me.Page = page

	End Sub

#End Region

End Class

''' <summary>
''' RootFrameNavigationHelper registers for standard mouse and keyboard shortcuts used to go back and forward.
''' There should be only one RootFrameNavigationHelper per view, and it should be associated with the root frame.
''' </summary>
''' <example>
''' To make use of RootFrameNavigationHelper, create an instance of the RootNavigationHelper such as in the constructor of the root page.
''' <code>
'''     public SomeRootPage()
'''     {
'''         this.InitializeComponent();
'''         this.rootNavigationHelper = New RootNavigationHelper(rootFrame);
'''     }
''' </code>
''' </example>
<Windows.Foundation.Metadata.WebHostHidden>
Public Class RootFrameNavigationHelper

#Region " Objects and variables "

	Private m_Frame As Frame
	Private m_SystemNavigationManager As SystemNavigationManager

#End Region

#Region " Properties "

	Private ReadOnly Property Frame As Frame
		Get
			Return m_Frame
		End Get
	End Property

#End Region

#Region " Private methods and functions "

	''' <summary>
	''' Invoked on every keystroke, including system keys such as Alt key combinations.
	''' Used to detect keyboard navigation between pages even when the page itself doesn't have focus.
	''' </summary>
	''' <param name="sender">Instance that triggered the event.</param>
	''' <param name="e">Event data describing the conditions that led to the event.</param>
	Private Sub CoreDispatcher_AcceleratorKeyActivated(sender As CoreDispatcher, e As AcceleratorKeyEventArgs)
		Dim virtualKey As VirtualKey = e.VirtualKey

		' Only investigate further when Left, Right, Or the dedicated Previous Or Next keys are pressed
		If (e.EventType = CoreAcceleratorKeyEventType.SystemKeyDown OrElse e.EventType = CoreAcceleratorKeyEventType.KeyDown) AndAlso (virtualKey = VirtualKey.Left OrElse virtualKey = VirtualKey.Right OrElse CInt(virtualKey) = 166 OrElse CInt(virtualKey) = 167) Then
            Dim coreWindow As CoreWindow = Window.Current.CoreWindow
            Dim downState As CoreVirtualKeyStates = CoreVirtualKeyStates.Down
            Dim menuKey As Boolean = (coreWindow.GetKeyState(VirtualKey.Menu) And downState) = downState
            Dim controlKey As Boolean = (coreWindow.GetKeyState(VirtualKey.Control) And downState) = downState
            Dim shiftKey As Boolean = (coreWindow.GetKeyState(VirtualKey.Shift) And downState) = downState
            Dim noModifiers As Boolean = Not menuKey AndAlso Not controlKey AndAlso Not shiftKey
			Dim onlyAlt As Boolean = menuKey AndAlso Not controlKey AndAlso Not shiftKey

			If (CInt(virtualKey) = 166 AndAlso noModifiers) OrElse (virtualKey = VirtualKey.Left AndAlso onlyAlt) Then
				' When the previous key Or Alt+Left are pressed navigate back
				e.Handled = TryGoBack()
			ElseIf (CInt(virtualKey) = 167 AndAlso noModifiers) OrElse (virtualKey = VirtualKey.Right AndAlso onlyAlt) Then
				' When the next key Or Alt+Right are pressed navigate forward
				e.Handled = TryGoForward()
			End If
		End If

	End Sub

	''' <summary>
	''' Invoked on every mouse click, touch screen tap, Or equivalent interaction.
	''' Used to detect browser-style next And previous mouse button clicks
	''' to navigate between pages.
	''' </summary>
	''' <param name="sender">Instance that triggered the event.</param>
	''' <param name="e">Event data describing the conditions that led to the event.</param>
	Private Sub CoreWindow_PointerPressed(sender As CoreWindow, e As PointerEventArgs)
		Dim properties As PointerPointProperties = e.CurrentPoint.Properties

		' Ignore button chords with the left, right, And middle buttons
		If properties.IsLeftButtonPressed OrElse properties.IsRightButtonPressed OrElse properties.IsMiddleButtonPressed Then
			Return
		End If
		' If back Or foward are pressed (but Not both) navigate appropriately
		Dim backPressed As Boolean = properties.IsXButton1Pressed
		Dim forwardPressed As Boolean = properties.IsXButton2Pressed

		If backPressed Xor forwardPressed Then
			e.Handled = True
			If backPressed Then
				TryGoBack()
			End If
			If forwardPressed Then
				TryGoForward()
			End If
		End If

	End Sub

	Private Sub SystemNavigationManager_BackRequested(sender As Object, e As BackRequestedEventArgs)

		If Not e.Handled Then
			e.Handled = TryGoBack()
		End If

	End Sub

	Private Function TryGoBack() As Boolean
		Dim navigated As Boolean = False

		If Frame.CanGoBack Then
			Frame.GoBack()
			navigated = True
		End If

		Return navigated

	End Function

	Private Function TryGoForward() As Boolean
		Dim navigated As Boolean = False

		If Frame.CanGoForward Then
			Frame.GoForward()
			navigated = True
		End If

		Return navigated

	End Function

	Private Sub UpdateBackButton()

		m_SystemNavigationManager.AppViewBackButtonVisibility = If(Frame.CanGoBack, AppViewBackButtonVisibility.Visible, AppViewBackButtonVisibility.Collapsed)

	End Sub

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a new instance of the <see cref="RootFrameNavigationHelper"/> class.
	''' </summary>
	''' <param name="rootFrame">A reference to the top-level frame.
	''' This reference allows for frame manipulation And to register navigation handlers.</param>
	Public Sub New(rootFrame As Frame)

		m_Frame = rootFrame
		' Handle keyboard And mouse navigation requests
		m_SystemNavigationManager = SystemNavigationManager.GetForCurrentView()
		AddHandler m_SystemNavigationManager.BackRequested, AddressOf SystemNavigationManager_BackRequested
		UpdateBackButton()

		' Listen to the window directly so we will respond to hotkeys regardless of which element has focus.
		AddHandler Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated, AddressOf CoreDispatcher_AcceleratorKeyActivated
		AddHandler Window.Current.CoreWindow.PointerPressed, AddressOf CoreWindow_PointerPressed
		' Update the Back button whenever a navigation occurs.
		AddHandler m_Frame.Navigated, Sub(s, e) UpdateBackButton()

	End Sub

#End Region

End Class