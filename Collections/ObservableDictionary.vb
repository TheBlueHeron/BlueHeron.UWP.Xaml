
''' <summary>
''' Implementation of IObservableMap that supports reentrancy for use as a default view model.
''' </summary>
Public Class ObservableDictionary
	Implements IObservableMap(Of String, Object)

#Region " Objects and variables "

	Private Class ObservableDictionaryChangedEventArgs
		Implements IMapChangedEventArgs(Of String)

#Region " Objects and variables "

		Private m_Change As CollectionChange
		Private m_Key As String

#End Region

#Region " Properties "

		Public ReadOnly Property CollectionChange As CollectionChange Implements IMapChangedEventArgs(Of String).CollectionChange
			Get
				Return m_Change
			End Get
		End Property

		Public ReadOnly Property Key As String Implements IMapChangedEventArgs(Of String).Key
			Get
				Return m_Key
			End Get
		End Property

#End Region

#Region " Construction "

		Public Sub New(change As CollectionChange, key As String)

			m_Change = change
			m_Key = key

		End Sub

#End Region

	End Class

	Private m_Dictionary As New Dictionary(Of String, Object)

	Public Event MapChanged As MapChangedEventHandler(Of String, Object) Implements IObservableMap(Of String, Object).MapChanged

#End Region

#Region " Properties "

	Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, Object)).Count
		Get
			Return m_Dictionary.Count
		End Get
	End Property

	Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).IsReadOnly
		Get
			Return False
		End Get
	End Property

	Default Public Property Item(key As String) As Object Implements IDictionary(Of String, Object).Item
		Get
			Return m_Dictionary(key)
		End Get
		Set(value As Object)
			m_Dictionary(key) = value
			InvokeMapChanged(CollectionChange.ItemChanged, key)
		End Set
	End Property

	Public ReadOnly Property Keys As ICollection(Of String) Implements IDictionary(Of String, Object).Keys
		Get
			Return m_Dictionary.Keys
		End Get
	End Property

	Public ReadOnly Property Values As ICollection(Of Object) Implements IDictionary(Of String, Object).Values
		Get
			Return m_Dictionary.Values
		End Get
	End Property

#End Region

#Region " Public methods and functions "

	Public Sub Add(item As KeyValuePair(Of String, Object)) Implements ICollection(Of KeyValuePair(Of String, Object)).Add

		Add(item.Key, item.Value)

	End Sub

	Public Sub Add(key As String, value As Object) Implements IDictionary(Of String, Object).Add

		m_Dictionary.Add(key, value)
		InvokeMapChanged(CollectionChange.ItemInserted, key)

	End Sub

	Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, Object)).Clear
		Dim priorKeys As String() = m_Dictionary.Keys.ToArray()

		m_Dictionary.Clear()
		For Each key As String In priorKeys
			InvokeMapChanged(CollectionChange.ItemRemoved, key)
		Next

	End Sub

	Public Sub CopyTo(array() As KeyValuePair(Of String, Object), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, Object)).CopyTo
		Dim arraySize As Integer = array.Length

		For Each kv As KeyValuePair(Of String, Object) In m_Dictionary
			If (arrayIndex >= arraySize) Then
				Exit For
			End If
			array(arrayIndex) = kv
			arrayIndex += 1
		Next

	End Sub

	Public Function Contains(item As KeyValuePair(Of String, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).Contains

		Return m_Dictionary.Contains(item)

	End Function

	Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, Object).ContainsKey

		Return m_Dictionary.ContainsKey(key)

	End Function

	Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, Object)) Implements IEnumerable(Of KeyValuePair(Of String, Object)).GetEnumerator

		Return m_Dictionary.GetEnumerator

	End Function

	Public Function Remove(item As KeyValuePair(Of String, Object)) As Boolean Implements ICollection(Of KeyValuePair(Of String, Object)).Remove
		Dim currentValue As Object = Nothing

		If m_Dictionary.TryGetValue(item.Key, currentValue) AndAlso Object.Equals(item.Value, currentValue) AndAlso m_Dictionary.Remove(item.Key) Then
			InvokeMapChanged(CollectionChange.ItemRemoved, item.Key)
			Return True
		End If

		Return False

	End Function

	Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, Object).Remove

		If m_Dictionary.Remove(key) Then
			InvokeMapChanged(CollectionChange.ItemRemoved, key)
			Return True
		End If

		Return False

	End Function

	Public Function TryGetValue(key As String, ByRef value As Object) As Boolean Implements IDictionary(Of String, Object).TryGetValue

		Return m_Dictionary.TryGetValue(key, value)

	End Function

	Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator

		Return m_Dictionary.GetEnumerator

	End Function

#End Region

#Region " Private methods and functions "

	Protected Overridable Sub InvokeMapChanged(change As CollectionChange, key As String)

		RaiseEvent MapChanged(Me, New ObservableDictionaryChangedEventArgs(change, key))

	End Sub

#End Region

End Class