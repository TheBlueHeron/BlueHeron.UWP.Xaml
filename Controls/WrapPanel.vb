
Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization

Namespace Controls

	''' <summary>
	''' Positions child elements sequentially from left to right or top to bottom. 
	''' When elements extend beyond the panel edge, elements are positioned in the next row or column.
	''' </summary>
	''' <QualityBand>Mature</QualityBand>
	Public Class WrapPanel
		Inherits Panel

#Region " Objects and variables "

		''' <summary>
		''' A value indicating whether a dependency property change handler should ignore the next change notification.
		''' This is used to reset the value of properties without performing any of the actions in their change handlers.
		''' </summary>
		Private m_IgnorePropertyChange As Boolean

		''' <summary>
		''' Identifies the <see cref="P:WinRTXamlToolkit.Controls.WrapPanel.ItemHeight" /> dependency property.
		''' </summary>
		Public Shared ReadOnly ItemHeightProperty As DependencyProperty = DependencyProperty.Register("ItemHeight", GetType(Double), GetType(WrapPanel), New PropertyMetadata(Double.NaN, AddressOf OnItemHeightOrWidthPropertyChanged))

		''' <summary>
		''' Identifies the <see cref="P:WinRTXamlToolkit.Controls.WrapPanel.ItemWidth" /> dependency property.
		''' </summary>
		Public Shared ReadOnly ItemWidthProperty As DependencyProperty = DependencyProperty.Register("ItemWidth", GetType(Double), GetType(WrapPanel), New PropertyMetadata(Double.NaN, AddressOf OnItemHeightOrWidthPropertyChanged))

		''' <summary>
		''' Identifies the <see cref="P:WinRTXamlToolkit.Controls.WrapPanel.Orientation" /> dependency property.
		''' </summary>
		Public Shared ReadOnly OrientationProperty As DependencyProperty = DependencyProperty.Register("Orientation", GetType(Orientation), GetType(WrapPanel), New PropertyMetadata(Orientation.Horizontal, AddressOf OnOrientationPropertyChanged))

#End Region

#Region " Properties "

		''' <summary>
		''' Gets or sets the height of the layout area for each item that is contained in a <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" />.
		''' </summary>
		''' <value>
		''' The height applied to the layout area of each item that is contained within a <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" />.
		''' The default value is <see cref="F:System.Double.NaN" />.
		''' </value>
		Public Property ItemHeight As Double
			Get
				Return CDbl(GetValue(ItemHeightProperty))
			End Get
			Set(value As Double)
				SetValue(ItemHeightProperty, value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets the width of the layout area for each item that is contained in a <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" />.
		''' </summary>
		''' <value>
		''' The width that applies to the layout area of each item that is contained in a <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" />.
		''' The default value is <see cref="F:System.Double.NaN" />.
		''' </value>
		Public Property ItemWidth As Double
			Get
				Return CDbl(GetValue(ItemWidthProperty))
			End Get
			Set(value As Double)
				SetValue(ItemWidthProperty, value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets the direction in which child elements are arranged.
		''' </summary>
		''' <value>
		''' One of the <see cref="T:Windows.UI.Xaml.Controls.Orientation" /> values.
		''' The default is <see cref="F:Windows.UI.Xaml.Controls.Orientation.Horizontal" />.
		''' </value>
		Public Property Orientation As Orientation
			Get
				Return CType(GetValue(OrientationProperty), Orientation)
			End Get
			Set(value As Orientation)
				SetValue(OrientationProperty, value)
			End Set
		End Property

#End Region

#Region " Private methods and functions "

		''' <summary>
		''' Arrange a sequence of elements in a single line.
		''' </summary>
		''' <param name="lineStart">
		''' Index of the first element in the sequence to arrange.
		''' </param>
		''' <param name="lineEnd">
		''' Index of the last element in the sequence to arrange.
		''' </param>
		''' <param name="directDelta">
		''' Optional fixed growth in the primary direction.
		''' </param>
		''' <param name="indirectOffset">
		''' Offset of the line in the indirect direction.
		''' </param>
		''' <param name="indirectGrowth">
		''' Shared indirect growth of the elements on this line.
		''' </param>
		Private Sub ArrangeLine(lineStart As Integer, lineEnd As Integer, directDelta As Double?, indirectOffset As Double, indirectGrowth As Double)
			Dim directOffset As Double = 0.0
			Dim o As Orientation = Me.Orientation
			Dim isHorizontal = (o = Orientation.Horizontal)
			Dim children As UIElementCollection = Me.Children

			For index As Integer = lineStart To lineEnd - 1
				' Get the size of the element
				Dim element As UIElement = children(index)
				Dim elementSize As New OrientedSize(o, element.DesiredSize.Width, element.DesiredSize.Height)

                ' Determine if we should use the element's desired size or the fixed item width Or height
                Dim directGrowth As Double = If(directDelta.HasValue, directDelta.Value, elementSize.Direct)
				' Arrange the element
				Dim bounds As Rect = If(isHorizontal, New Rect(directOffset, indirectOffset, directGrowth, indirectGrowth), New Rect(indirectOffset, directOffset, indirectGrowth, directGrowth))
				element.Arrange(bounds)

				directOffset += directGrowth
			Next

		End Sub

		''' <summary>
		''' OrientationProperty property changed handler.
		''' </summary>
		''' <param name="d">WrapPanel that changed its Orientation.</param>
		''' <param name="e">Event arguments.</param>
		<SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification:="Almost always set from the CLR property.")>
		Private Shared Sub OnOrientationPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
			Dim source As WrapPanel = CType(d, WrapPanel)
			Dim value As Orientation = CType(e.NewValue, Orientation)

            ' Ignore the change if requested
            If source.m_IgnorePropertyChange Then
				source.m_IgnorePropertyChange = False
				Exit Sub
			End If

            ' Validate the Orientation
            If (value <> Orientation.Horizontal) AndAlso (value <> Orientation.Vertical) Then
				' Reset the property to its original state before throwing
				source.m_IgnorePropertyChange = True
				source.SetValue(OrientationProperty, CType(e.OldValue, Orientation))

				Dim message As String = String.Format(CultureInfo.InvariantCulture, "Properties.Resources.WrapPanel_OnOrientationPropertyChanged_InvalidValue", value)
				Throw New ArgumentException(message, "value")
			End If

			source.InvalidateMeasure() ' Orientation affects measuring

		End Sub

		''' <summary>
		''' Property changed handler for ItemHeight And ItemWidth.
		''' </summary>
		''' <param name="d">
		''' WrapPanel that changed its ItemHeight Or ItemWidth.
		''' </param>
		''' <param name="e">Event arguments.</param>
		<SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification:="Almost always set from the CLR property.")>
		Private Shared Sub OnItemHeightOrWidthPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
			Dim source As WrapPanel = CType(d, WrapPanel)
			Dim value As Double = CDbl(e.NewValue)

            ' Ignore the change if requested
            If source.m_IgnorePropertyChange Then
				source.m_IgnorePropertyChange = False
				Exit Sub
			End If

            ' Validate the length (which must either be NaN Or a positive, finite number)
            If Not Double.IsNaN(value) AndAlso ((value <= 0.0) OrElse Double.IsPositiveInfinity(value)) Then
				' Reset the property to its original state before throwing
				source.m_IgnorePropertyChange = True
				source.SetValue(e.Property, CDbl(e.OldValue))

				Dim message As String = String.Format(CultureInfo.InvariantCulture, "Properties.Resources.WrapPanel_OnItemHeightOrWidthPropertyChanged_InvalidValue", value)
				Throw New ArgumentException(message, "value")
			End If

			source.InvalidateMeasure() ' The length properties affect measuring

		End Sub

#End Region

#Region " Overrides "

		''' <summary>
		''' Arranges And sizes the <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" /> control and its child elements.
		''' </summary>
		''' <param name="finalSize">
		''' The area within the parent that the <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" /> should use arrange itself and its children.
		''' </param>
		''' <returns>
		''' The actual size used by the <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" />.
		''' </returns>
		Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
			' Variables tracking the size of the current line, and the maximum size available to fill. Note that the line might represent a row or a column depending on the orientation.
			Dim o As Orientation = Me.Orientation
			Dim lineSize As New OrientedSize(o)
			Dim maximumSize As New OrientedSize(o, finalSize.Width, finalSize.Height)

            ' Determine the constraints for individual items
            Dim itemWidth As Double = Me.ItemWidth
			Dim itemHeight As Double = Me.ItemHeight
			Dim hasFixedWidth As Boolean = Not itemWidth.IsNaN()
			Dim hasFixedHeight As Boolean = Not itemHeight.IsNaN()
			Dim indirectOffset As Double = 0
			Dim directDelta As Double? = If(o = Orientation.Horizontal, If(hasFixedWidth, CType(itemWidth, Nullable(Of Double)), Nothing), If(hasFixedHeight, CType(itemHeight, Nullable(Of Double)), Nothing))

            ' Measure each of the Children. Pocess the elements one line at a time, just like during measure, but wait until an entire line of elements is completed before arranging them.
            ' The lineStart And lineEnd variables track the size of the currently arranged line.
            Dim children As UIElementCollection = Me.Children
			Dim count As Integer = children.Count
			Dim lineStart As Integer = 0

			For lineEnd As Integer = 0 To count - 1
				Dim element As UIElement = children(lineEnd)
				' Get the size of the element
				Dim elementSize As New OrientedSize(o, If(hasFixedWidth, itemWidth, element.DesiredSize.Width), If(hasFixedHeight, itemHeight, element.DesiredSize.Height))
				' If this element falls of the edge of the line
				If (lineSize.Direct + elementSize.Direct).IsGreaterThan(maximumSize.Direct) Then
					' Then we just completed a line And we should arrange it
					ArrangeLine(lineStart, lineEnd, directDelta, indirectOffset, lineSize.Indirect)
					' Move the current element to a new line
					indirectOffset += lineSize.Indirect
					lineSize = elementSize

                    ' If the current element Is larger than the maximum size
                    If elementSize.Direct.IsGreaterThan(maximumSize.Direct) Then
						' Arrange the element as a single line
						ArrangeLine(lineEnd, ++lineEnd, directDelta, indirectOffset, elementSize.Indirect)
						' Move to a New line
						indirectOffset += lineSize.Indirect
						lineSize = New OrientedSize(o)
					End If

                    ' Advance the start index to a new line after arranging
                    lineStart = lineEnd
				Else
					' Otherwise just add the element to the end of the line
					lineSize.Direct += elementSize.Direct
					lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect)
				End If
			Next

            ' Arrange any elements on the last line
            If lineStart < count Then
				ArrangeLine(lineStart, count, directDelta, indirectOffset, lineSize.Indirect)
			End If

			Return finalSize

		End Function

		''' <summary>
		''' Measures the child elements of a' <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" /> in anticipation of arranging them during the <see cref="Windows.UI.Xaml.FrameworkElement.ArrangeOverride(Windows.Foundation.Size)" /> pass.
		''' </summary>
		''' <param name="constraint">
		''' The size available to child elements of the wrap panel.
		''' </param>
		''' <returns>
		''' The size required by the <see cref="T:WinRTXamlToolkit.Controls.WrapPanel" /> and its elements.
		''' </returns>
		<SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId:="0#", Justification:="Compat with WPF.")>
		Protected Overrides Function MeasureOverride(constraint As Size) As Size
			' Variables tracking the size of the current line, the total size measured so far, and the maximum size available to fill.
			'Note that the line might represent a row or a column depending on the orientation.
			Dim o As Orientation = Me.Orientation
			Dim lineSize As New OrientedSize(o)
			Dim totalSize As New OrientedSize(o)
			Dim maximumSize As New OrientedSize(o, constraint.Width, constraint.Height)
			' Determine the constraints for individual items
			Dim itemWidth As Double = Me.ItemWidth
			Dim itemHeight As Double = Me.ItemHeight
			Dim hasFixedWidth As Boolean = Not Double.IsNaN(itemWidth)
			Dim hasFixedHeight As Boolean = Not Double.IsNaN(itemHeight)
			Dim itemSize As New Size(If(hasFixedWidth, itemWidth, constraint.Width), If(hasFixedHeight, itemHeight, constraint.Height))

            ' Measure each of the Children
            For Each element As UIElement In Children
				' Determine the size of the element
				element.Measure(itemSize)
				Dim elementSize As New OrientedSize(o, If(hasFixedWidth, itemWidth, element.DesiredSize.Width), If(hasFixedHeight, itemHeight, element.DesiredSize.Height))

                ' If this element falls of the edge of the line
                If (lineSize.Direct + elementSize.Direct).IsGreaterThan(maximumSize.Direct) Then
					' Update the total size with the direct and indirect growth for the current line
					totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct)
					totalSize.Indirect += lineSize.Indirect

                    ' Move the element to a new line
                    lineSize = elementSize

                    ' If the current element is larger than the maximum size, place it on a line by itself
                    If elementSize.Direct.IsGreaterThan(maximumSize.Direct) Then
						' Update the total size for the line occupied by this single element
						totalSize.Direct = Math.Max(elementSize.Direct, totalSize.Direct)
						totalSize.Indirect += elementSize.Indirect
						' Move to a new line
						lineSize = New OrientedSize(o)
					End If
				Else
					' Otherwise just add the element to the end of the line
					lineSize.Direct += elementSize.Direct
					lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect)
				End If
			Next

            ' Update the total size with the elements on the last line
            totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct)
			totalSize.Indirect += lineSize.Indirect

            ' Return the total size required as an un-oriented quantity
            Return New Size(totalSize.Width, totalSize.Height)

		End Function

#End Region

	End Class

End Namespace