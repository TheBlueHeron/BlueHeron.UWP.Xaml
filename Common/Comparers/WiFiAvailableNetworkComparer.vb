Imports Windows.Devices.WiFi

''' <summary>
''' Compares <see cref="WiFiAvailableNetwork"/> objects according to their <see cref="WiFiAvailableNetwork.Ssid"/> properties.
''' </summary>
Public Class WiFiAvailableNetworkComparer
	Implements IEqualityComparer(Of WiFiAvailableNetwork)

	Public Shadows Function Equals(x As WiFiAvailableNetwork, y As WiFiAvailableNetwork) As Boolean Implements IEqualityComparer(Of WiFiAvailableNetwork).Equals

		Return x.Ssid.Equals(y.Ssid)

	End Function

	Public Shadows Function GetHashCode(obj As WiFiAvailableNetwork) As Integer Implements IEqualityComparer(Of WiFiAvailableNetwork).GetHashCode

		Return obj.Ssid.GetHashCode

	End Function

End Class