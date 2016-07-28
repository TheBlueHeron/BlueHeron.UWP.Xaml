Imports Windows.Devices.WiFi
Imports Windows.Networking.Connectivity
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Imaging

''' <summary>
''' Object for displaying WiFi network details.
''' </summary>
Public Class WiFiNetworkDisplay
	Implements INotifyPropertyChanged

#Region " Objects and variables "

	Private Const _FREQUENCY As String = "{0}kHz"
	Private Const _HIDDEN As String = "Hidden"
	Private Const _IMAGEPATH As String = "ms-appx:/BlueHeron.UWP.Xaml/Assets/{0}_{1}bar.png"
	Private Const _NOTCONNECTED As String = "Not Connected"
	Private Const _OPEN As String = "open"
	Private Const _RSSI As String = "{0}dBm"
	Private Const _SECURE As String = "secure"
	Private Const _SECURITY As String = "Authentication: {0}; Encryption: {1}"

	Private m_Adapter As WiFiAdapter
	Private m_AvailableNetwork As WiFiAvailableNetwork
	Private m_ConnectivityLevel As String
	Private m_WiFiImage As ImageSource

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Properties "

	''' <summary>
	''' Returns the <see cref="WiFiAdapter" /> that is used to connect. 
	''' </summary>
	Public Property Adapter As WiFiAdapter
		Get
			Return m_Adapter
		End Get
		Friend Set(value As WiFiAdapter)
			m_Adapter = value
		End Set
	End Property

	''' <summary>
	''' Returns the <see cref="AvailableNetwork"/> that will be connected.
	''' </summary>
	Public Property AvailableNetwork As WiFiAvailableNetwork
		Get
			Return m_AvailableNetwork
		End Get
		Friend Set(value As WiFiAvailableNetwork)
			m_AvailableNetwork = value
		End Set
	End Property

	''' <summary>
	''' Details on the level of connectivity.
	''' </summary>
	Public Property ConnectivityLevel As String
		Get
			Return m_ConnectivityLevel
		End Get
		Friend Set(value As String)
			m_ConnectivityLevel = value
		End Set
	End Property

	''' <summary>
	''' A <see cref="BitmapImage" /> displaying the connection strength. 
	''' </summary>
	Public Property Icon As ImageSource
		Get
			Return m_WiFiImage
		End Get
		Set(value As ImageSource)
			If ((value Is Nothing) AndAlso (Not m_WiFiImage Is Nothing)) OrElse (Not value.Equals(m_WiFiImage)) Then
				m_WiFiImage = value
				OnPropertyChanged("Icon")
			End If
		End Set
	End Property

#Region " Network details "

	Public ReadOnly Property Ssid As String
		Get
			Return If(String.IsNullOrEmpty(AvailableNetwork.Ssid), _HIDDEN, AvailableNetwork.Ssid)
		End Get
	End Property

	Public ReadOnly Property Bssid As String
		Get
			Return AvailableNetwork.Bssid
		End Get
	End Property

	Public ReadOnly Property ChannelCenterFrequency As String
		Get
			Return String.Format(_FREQUENCY, AvailableNetwork.ChannelCenterFrequencyInKilohertz)
		End Get
	End Property

	Public ReadOnly Property Rssi As String
		Get
			Return String.Format(_RSSI, AvailableNetwork.NetworkRssiInDecibelMilliwatts)
		End Get
	End Property

	Public ReadOnly Property SecuritySettings As String
		Get
			Return String.Format(_SECURITY, AvailableNetwork.SecuritySettings.NetworkAuthenticationType, AvailableNetwork.SecuritySettings.NetworkEncryptionType)
		End Get
	End Property

#End Region

#End Region

#Region " Public methods and functions "

	''' <summary>
	''' Reassesses the current connectivity.
	''' </summary>
	Public Async Function UpdateConnectivityLevel() As Task
		Dim strSsid As String = _NOTCONNECTED
		Dim connectedProfile As ConnectionProfile = Await Adapter.NetworkAdapter.GetConnectedProfileAsync()

		If (Not connectedProfile Is Nothing) AndAlso connectedProfile.IsWlanConnectionProfile AndAlso (Not connectedProfile.WlanConnectionProfileDetails Is Nothing) Then
			strSsid = connectedProfile.WlanConnectionProfileDetails.GetConnectedSsid()
		End If
		If Not String.IsNullOrEmpty(strSsid) Then
			If strSsid.Equals(AvailableNetwork.Ssid) Then
				ConnectivityLevel = connectedProfile.GetNetworkConnectivityLevel.ToString
			End If
		End If

		OnPropertyChanged("ConnectivityLevel")

	End Function

#End Region

#Region " Private methods and functions "

	Protected Async Sub OnPropertyChanged(propertyName As String)

		Await RootPage.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
																					  RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
																				  End Sub)
	End Sub

	Private Sub UpdateWiFiImage()
		Dim strImageFileName As String = String.Format(_IMAGEPATH, If(AvailableNetwork.SecuritySettings.NetworkAuthenticationType = NetworkAuthenticationType.Open80211, _OPEN, _SECURE), AvailableNetwork.SignalBars)

		Icon = New BitmapImage(New Uri(strImageFileName))

	End Sub

#End Region

#Region " Construction "

	''' <summary>
	''' Creates a new <see cref="WiFiNetworkDisplay" /> instance.
	''' </summary>
	''' <param name="availableNetwork">A <see cref="WiFiAvailableNetwork" /></param>
	''' <param name="adapter">The <see cref="WiFiAdapter" /> that will be used to connect.</param>
	Public Sub New(availableNetwork As WiFiAvailableNetwork, adapter As WiFiAdapter)

		Me.AvailableNetwork = availableNetwork
		Me.Adapter = adapter

		UpdateWiFiImage()

	End Sub

#End Region

End Class