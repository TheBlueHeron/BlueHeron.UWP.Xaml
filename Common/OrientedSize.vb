Imports System.Runtime.InteropServices

''' <summary>
''' The OrientedSize structure is used to abstract the growth direction from the layout algorithms of WrapPanel.
''' When the growth direction is oriented horizontally (ex: the next element is arranged on the side of the previous element,
''' then the Width grows directly with the placement of elements And Height grows indirectly with the size of the largest element in the row.
''' When the orientation is reversed, so is the directional growth with respect to Width And Height.
''' </summary>
<StructLayout(LayoutKind.Sequential)>
Public Structure OrientedSize

#Region " Objects and variables "

	Private m_Direct As Double
	Private m_Indirect As Double
	Private m_Orientation As Orientation

#End Region

#Region " Properties "

	''' <summary>
	''' Gets or sets the size dimension that grows directly with layout placement.
	''' </summary>
	Public Property Direct As Double
		Get
			Return m_Direct
		End Get
		Set(value As Double)
			m_Direct = value
		End Set
	End Property

	''' <summary>
	''' Gets or sets the height of the size.
	''' </summary>
	Public Property Height As Double
		Get
			Return If(m_Orientation <> Orientation.Horizontal, m_Direct, m_Indirect)
		End Get
		Set(value As Double)
			If m_Orientation = Orientation.Horizontal Then
				m_Direct = value
			Else
				m_Indirect = value
			End If
		End Set
	End Property

	''' <summary>
	''' Gets or sets the size dimension that grows indirectly with the maximum value of the layout row or column.
	''' </summary>
	Public Property Indirect As Double
		Get
			Return m_Indirect
		End Get
		Set(value As Double)
			m_Indirect = value
		End Set
	End Property

	''' <summary>
	''' Gets the orientation of the structure.
	''' </summary>
	Public ReadOnly Property Orientation As Orientation
		Get
			Return m_Orientation
		End Get
	End Property

	''' <summary>
	''' Gets or sets the width of the size.
	''' </summary>
	Public Property Width As Double
		Get
			Return If(m_Orientation = Orientation.Horizontal, m_Direct, m_Indirect)
		End Get
		Set(value As Double)
			If m_Orientation <> Orientation.Horizontal Then
				m_Direct = value
			Else
				m_Indirect = value
			End If
		End Set
	End Property

#End Region

#Region " Construction "

	''' <summary>
	''' Initializes a New OrientedSize structure.
	''' </summary>
	''' <param name="orientation">Orientation of the structure.</param>
	Public Sub New(orientation As Orientation)

		Me.New(orientation, 0.0, 0.0)

	End Sub

	''' <summary>
	''' Initializes a New OrientedSize structure.
	''' </summary>
	''' <param name="orientation">Orientation of the structure.</param>
	''' <param name="width">Un-oriented width of the structure.</param>
	''' <param name="height">Un-oriented height of the structure.</param>
	Public Sub New(orientation As Orientation, width As Double, height As Double)

		m_Orientation = orientation

		' All fields must be initialized before we access the this pointer
		m_Direct = 0.0
		m_Indirect = 0.0

		Me.Width = width
		Me.Height = height

	End Sub

#End Region

End Structure