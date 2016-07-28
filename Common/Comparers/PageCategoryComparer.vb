
''' <summary>
''' Compares <see cref="PageCategory"/> objects according to their <see cref="PageCategory.Title"/> properties.
''' </summary>
Public Class PageCategoryComparer
	Implements IEqualityComparer(Of PageCategory)

	Public Shadows Function Equals(x As PageCategory, y As PageCategory) As Boolean Implements IEqualityComparer(Of PageCategory).Equals

		Return x.Title.Equals(y.Title)

	End Function

	Public Shadows Function GetHashCode(obj As PageCategory) As Integer Implements IEqualityComparer(Of PageCategory).GetHashCode

		Return obj.Title.GetHashCode

	End Function

End Class