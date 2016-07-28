Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices

Public Module Extensions

#Region " Objects and variables "

	''' <summary>
	''' NanUnion is a C++ style type union used for efficiently converting a double into an unsigned long, whose bits can be easily manipulated.
	''' </summary>
	<StructLayout(LayoutKind.Explicit)>
	Private Structure NanUnion
        ''' <summary>
        ''' Floating point representation of the union.
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification:="It is accessed through the other member of the union"), FieldOffset(0)>
        Friend floatingValue As Double
        ''' <summary>
        ''' Integer representation of the union.
        ''' </summary>
        <FieldOffset(0)>
        Friend integerValue As ULong
    End Structure

#End Region

#Region " Numeric extensions "

#If Not WINDOWS_PHONE Then

    ''' <summary>
    ''' Check if a number is zero.
    ''' </summary>
    ''' <param name="value">The number to check.</param>
    ''' <returns>True if the number is zero, false otherwise.</returns>
    <Extension()>
    Public Function IsZero(value As Double) As Boolean
        ' Consider anything within an order of magnitude of epsilon to be zero
        Return Math.Abs(value) < 0.0000000000000022204460492503131

    End Function

#End If

    ''' <summary>
    ''' Check if a number isn't really a number.
    ''' </summary>
    ''' <param name="value">The number to check.</param>
    ''' <returns>
    ''' True if the number Is Not a number, false if it Is a number.
    ''' </returns>
    ''' <remarks>An IEEE 754 double precision floating point number is NaN if its exponent equals 2047 and it has a non-zero mantissa.</remarks>
    <Extension()>
    Public Function IsNaN(value As Double) As Boolean
        Dim union As New NanUnion With {.floatingValue = value} ' Get the double as an unsigned long
        Dim exponent As ULong = union.integerValue And 18442240474082181120UL ' 0xfff0000000000000L;

        If (exponent <> 4503599627370495UL) AndAlso (exponent <> 18442240474082181120UL) Then
            Return False
        End If

        Dim mantissa As ULong = union.integerValue And 4503599627370495UL

        Return mantissa <> 0L

	End Function

	''' <summary>
	''' Determine if one number Is greater than another.
	''' </summary>
	''' <param name="left">First number.</param>
	''' <param name="right">Second number.</param>
	''' <returns>
	''' True if the first number Is greater than the second, false otherwise.
	''' </returns>
	<Extension()>
	Public Function IsGreaterThan(left As Double, right As Double) As Boolean

		Return (left > right) AndAlso Not AreClose(left, right)

	End Function

	''' <summary>
	''' Determine if two numbers are close in value.
	''' </summary>
	''' <param name="left">First number.</param>
	''' <param name="right">Second number.</param>
	''' <returns>
	''' True if the first number Is close in value to the second, false otherwise.
	''' </returns>
	<Extension()>
	Public Function AreClose(left As Double, right As Double) As Boolean

		If left = right Then
			Return True
		End If

		Dim a As Double = (Math.Abs(left) + Math.Abs(right) + 10.0) * 0.00000000000000022204460492503131
		Dim b As Double = left - right

		Return (-a < b) AndAlso (a > b)

	End Function

#If Not WINDOWS_PHONE Then

	''' <summary>
	''' Determine if one number Is less than Or close to another.
	''' </summary>
	''' <param name="left">First number.</param>
	''' <param name="right">Second number.</param>
	''' <returns>
	''' True if the first number Is less than Or close to the second, false otherwise.
	''' </returns>
	<Extension()>
	Public Function IsLessThanOrClose(left As Double, right As Double) As Boolean

		Return (left < right) OrElse AreClose(left, right)

	End Function

#End If

#End Region

End Module