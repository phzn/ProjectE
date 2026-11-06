using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	/// <summary>Represents a single-precision floating-point number.</summary>
	// Token: 0x0200017C RID: 380
	[Serializable]
	public readonly struct Single : IComparable, IConvertible, IFormattable, IComparable<float>, IEquatable<float>, ISpanFormattable
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x0003CE39 File Offset: 0x0003B039
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsFinite(float f)
		{
			return (BitConverter.SingleToInt32Bits(f) & int.MaxValue) < 2139095040;
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to negative or positive infinity.</summary>
		/// <param name="f">A single-precision floating-point number.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.PositiveInfinity" /> or <see cref="F:System.Single.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F11 RID: 3857 RVA: 0x0003CE4E File Offset: 0x0003B04E
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(float f)
		{
			return (BitConverter.SingleToInt32Bits(f) & int.MaxValue) == 2139095040;
		}

		/// <summary>Returns a value that indicates whether the specified value is not a number (<see cref="F:System.Single.NaN" />).</summary>
		/// <param name="f">A single-precision floating-point number.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="f" /> evaluates to not a number (<see cref="F:System.Single.NaN" />); otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F12 RID: 3858 RVA: 0x0003CE63 File Offset: 0x0003B063
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(float f)
		{
			return (BitConverter.SingleToInt32Bits(f) & int.MaxValue) > 2139095040;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0003CE78 File Offset: 0x0003B078
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegative(float f)
		{
			return (BitConverter.SingleToInt32Bits(f) & int.MinValue) == int.MinValue;
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to negative infinity.</summary>
		/// <param name="f">A single-precision floating-point number.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F14 RID: 3860 RVA: 0x0003CE8D File Offset: 0x0003B08D
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegativeInfinity(float f)
		{
			return f == float.NegativeInfinity;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003CE98 File Offset: 0x0003B098
		[NonVersionable]
		public static bool IsNormal(float f)
		{
			int num = BitConverter.SingleToInt32Bits(f);
			num &= int.MaxValue;
			return num < 2139095040 && num != 0 && (num & 2139095040) != 0;
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to positive infinity.</summary>
		/// <param name="f">A single-precision floating-point number.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.PositiveInfinity" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F16 RID: 3862 RVA: 0x0003CECB File Offset: 0x0003B0CB
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPositiveInfinity(float f)
		{
			return f == float.PositiveInfinity;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0003CED8 File Offset: 0x0003B0D8
		[NonVersionable]
		public static bool IsSubnormal(float f)
		{
			int num = BitConverter.SingleToInt32Bits(f);
			num &= int.MaxValue;
			return num < 2139095040 && num != 0 && (num & 2139095040) == 0;
		}

		/// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object.</summary>
		/// <param name="value">An object to compare, or <see langword="null" />.</param>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.  
		///   Return Value  
		///
		///   Description  
		///
		///   Less than zero  
		///
		///   This instance is less than <paramref name="value" />.  
		///
		///  -or-  
		///
		///  This instance is not a number (<see cref="F:System.Single.NaN" />) and <paramref name="value" /> is a number.  
		///
		///   Zero  
		///
		///   This instance is equal to <paramref name="value" />.  
		///
		///  -or-  
		///
		///  This instance and value are both not a number (<see cref="F:System.Single.NaN" />), <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.  
		///
		///   Greater than zero  
		///
		///   This instance is greater than <paramref name="value" />.  
		///
		///  -or-  
		///
		///  This instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Single.NaN" />).  
		///
		///  -or-  
		///
		///  <paramref name="value" /> is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Single" />.</exception>
		// Token: 0x06000F18 RID: 3864 RVA: 0x0003CF0C File Offset: 0x0003B10C
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is float))
			{
				throw new ArgumentException("Object must be of type Single.");
			}
			float num = (float)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			if (this == num)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(num))
			{
				return -1;
			}
			return 0;
		}

		/// <summary>Compares this instance to a specified single-precision floating-point number and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified single-precision floating-point number.</summary>
		/// <param name="value">A single-precision floating-point number to compare.</param>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.  
		///   Return Value  
		///
		///   Description  
		///
		///   Less than zero  
		///
		///   This instance is less than <paramref name="value" />.  
		///
		///  -or-  
		///
		///  This instance is not a number (<see cref="F:System.Single.NaN" />) and <paramref name="value" /> is a number.  
		///
		///   Zero  
		///
		///   This instance is equal to <paramref name="value" />.  
		///
		///  -or-  
		///
		///  Both this instance and <paramref name="value" /> are not a number (<see cref="F:System.Single.NaN" />), <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.  
		///
		///   Greater than zero  
		///
		///   This instance is greater than <paramref name="value" />.  
		///
		///  -or-  
		///
		///  This instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Single.NaN" />).</returns>
		// Token: 0x06000F19 RID: 3865 RVA: 0x0003CF63 File Offset: 0x0003B163
		public int CompareTo(float value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			if (this == value)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(value))
			{
				return -1;
			}
			return 0;
		}

		/// <summary>Returns a value that indicates whether two specified <see cref="T:System.Single" /> values are equal.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1A RID: 3866 RVA: 0x0002842A File Offset: 0x0002662A
		[NonVersionable]
		public static bool operator ==(float left, float right)
		{
			return left == right;
		}

		/// <summary>Returns a value that indicates whether two specified <see cref="T:System.Single" /> values are not equal.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1B RID: 3867 RVA: 0x00028430 File Offset: 0x00026630
		[NonVersionable]
		public static bool operator !=(float left, float right)
		{
			return left != right;
		}

		/// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is less than another specified <see cref="T:System.Single" /> value.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1C RID: 3868 RVA: 0x00028439 File Offset: 0x00026639
		[NonVersionable]
		public static bool operator <(float left, float right)
		{
			return left < right;
		}

		/// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is greater than another specified <see cref="T:System.Single" /> value.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1D RID: 3869 RVA: 0x0002843F File Offset: 0x0002663F
		[NonVersionable]
		public static bool operator >(float left, float right)
		{
			return left > right;
		}

		/// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is less than or equal to another specified <see cref="T:System.Single" /> value.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1E RID: 3870 RVA: 0x00028445 File Offset: 0x00026645
		[NonVersionable]
		public static bool operator <=(float left, float right)
		{
			return left <= right;
		}

		/// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is greater than or equal to another specified <see cref="T:System.Single" /> value.</summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F1F RID: 3871 RVA: 0x0002844E File Offset: 0x0002664E
		[NonVersionable]
		public static bool operator >=(float left, float right)
		{
			return left >= right;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Single" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F20 RID: 3872 RVA: 0x0003CF90 File Offset: 0x0003B190
		public override bool Equals(object obj)
		{
			if (!(obj is float))
			{
				return false;
			}
			float num = (float)obj;
			return num == this || (float.IsNaN(num) && float.IsNaN(this));
		}

		/// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Single" /> object represent the same value.</summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F21 RID: 3873 RVA: 0x0003CFC6 File Offset: 0x0003B1C6
		public bool Equals(float obj)
		{
			return obj == this || (float.IsNaN(obj) && float.IsNaN(this));
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06000F22 RID: 3874 RVA: 0x0003CFE0 File Offset: 0x0003B1E0
		public override int GetHashCode()
		{
			int num = BitConverter.SingleToInt32Bits(this);
			if (((num - 1) & 2147483647) >= 2139095040)
			{
				num &= 2139095040;
			}
			return num;
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
		// Token: 0x06000F23 RID: 3875 RVA: 0x0003D00E File Offset: 0x0003B20E
		public override string ToString()
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
		// Token: 0x06000F24 RID: 3876 RVA: 0x0003D01D File Offset: 0x0003B21D
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.GetInstance(provider));
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
		/// <param name="format">A numeric format string.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.</exception>
		// Token: 0x06000F25 RID: 3877 RVA: 0x0003D02D File Offset: 0x0003B22D
		public string ToString(string format)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <param name="format">A numeric format string.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		// Token: 0x06000F26 RID: 3878 RVA: 0x0003D03C File Offset: 0x0003B23C
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003D04C File Offset: 0x0003B24C
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatSingle(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
		}

		/// <summary>Converts the string representation of a number to its single-precision floating-point number equivalent.</summary>
		/// <param name="s">A string that contains a number to convert.</param>
		/// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not represent a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		// Token: 0x06000F28 RID: 3880 RVA: 0x0003D05F File Offset: 0x0003B25F
		public static float Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseSingle(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the string representation of a number in a specified style to its single-precision floating-point number equivalent.</summary>
		/// <param name="s">A string that contains a number to convert.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <returns>A single-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		// Token: 0x06000F29 RID: 3881 RVA: 0x0003D080 File Offset: 0x0003B280
		public static float Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseSingle(s, style, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the string representation of a number in a specified culture-specific format to its single-precision floating-point number equivalent.</summary>
		/// <param name="s">A string that contains a number to convert.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
		/// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not represent a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		// Token: 0x06000F2A RID: 3882 RVA: 0x0003D0A3 File Offset: 0x0003B2A3
		public static float Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseSingle(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.GetInstance(provider));
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent.</summary>
		/// <param name="s">A string that contains a number to convert.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
		/// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not represent a numeric value.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		// Token: 0x06000F2B RID: 3883 RVA: 0x0003D0C5 File Offset: 0x0003B2C5
		public static float Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseSingle(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0003D0E9 File Offset: 0x0003B2E9
		public static float Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return Number.ParseSingle(s, style, NumberFormatInfo.GetInstance(provider));
		}

		/// <summary>Converts the string representation of a number to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <param name="s">A string representing a number to convert.</param>
		/// <param name="result">When this method returns, contains single-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not a number in a valid format, or represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F2D RID: 3885 RVA: 0x0003D0FE File Offset: 0x0003B2FE
		public static bool TryParse(string s, out float result)
		{
			if (s == null)
			{
				result = 0f;
				return false;
			}
			return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0003D122 File Offset: 0x0003B322
		public static bool TryParse(ReadOnlySpan<char> s, out float result)
		{
			return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <param name="s">A string representing a number to convert.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
		/// <param name="result">When this method returns, contains the single-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />, or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumerated constants. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		// Token: 0x06000F2F RID: 3887 RVA: 0x0003D135 File Offset: 0x0003B335
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out float result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				result = 0f;
				return false;
			}
			return float.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0003D15C File Offset: 0x0003B35C
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out float result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0003D174 File Offset: 0x0003B374
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out float result)
		{
			if (!Number.TryParseSingle(s, style, info, out result))
			{
				ReadOnlySpan<char> readOnlySpan = s.Trim();
				if (readOnlySpan.EqualsOrdinal(info.PositiveInfinitySymbol))
				{
					result = float.PositiveInfinity;
				}
				else if (readOnlySpan.EqualsOrdinal(info.NegativeInfinitySymbol))
				{
					result = float.NegativeInfinity;
				}
				else
				{
					if (!readOnlySpan.EqualsOrdinal(info.NaNSymbol))
					{
						return false;
					}
					result = float.NaN;
				}
			}
			return true;
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Single" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.Single" />.</returns>
		// Token: 0x06000F32 RID: 3890 RVA: 0x0003D1EA File Offset: 0x0003B3EA
		public TypeCode GetTypeCode()
		{
			return TypeCode.Single;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F33 RID: 3891 RVA: 0x0003D1EE File Offset: 0x0003B3EE
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		// Token: 0x06000F34 RID: 3892 RVA: 0x0003D1F7 File Offset: 0x0003B3F7
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Single", "Char"));
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.SByte" />.</returns>
		// Token: 0x06000F35 RID: 3893 RVA: 0x0003D212 File Offset: 0x0003B412
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
		// Token: 0x06000F36 RID: 3894 RVA: 0x0003D21B File Offset: 0x0003B41B
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int16" />.</returns>
		// Token: 0x06000F37 RID: 3895 RVA: 0x0003D224 File Offset: 0x0003B424
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
		// Token: 0x06000F38 RID: 3896 RVA: 0x0003D22D File Offset: 0x0003B42D
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int32" />.</returns>
		// Token: 0x06000F39 RID: 3897 RVA: 0x0003D236 File Offset: 0x0003B436
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
		// Token: 0x06000F3A RID: 3898 RVA: 0x0003D23F File Offset: 0x0003B43F
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int64" />.</returns>
		// Token: 0x06000F3B RID: 3899 RVA: 0x0003D248 File Offset: 0x0003B448
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
		// Token: 0x06000F3C RID: 3900 RVA: 0x0003D251 File Offset: 0x0003B451
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, unchanged.</returns>
		// Token: 0x06000F3D RID: 3901 RVA: 0x0003D25A File Offset: 0x0003B45A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
		// Token: 0x06000F3E RID: 3902 RVA: 0x0003D25E File Offset: 0x0003B45E
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
		// Token: 0x06000F3F RID: 3903 RVA: 0x0003D267 File Offset: 0x0003B467
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		// Token: 0x06000F40 RID: 3904 RVA: 0x0003D270 File Offset: 0x0003B470
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Single", "DateTime"));
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
		/// <param name="type">The type to which to convert this <see cref="T:System.Single" /> value.</param>
		/// <param name="provider">An object that supplies information about the format of the returned value.</param>
		/// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
		// Token: 0x06000F41 RID: 3905 RVA: 0x0003D28B File Offset: 0x0003B48B
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040012DB RID: 4827
		private readonly float m_value;

		/// <summary>Represents the smallest possible value of <see cref="T:System.Single" />. This field is constant.</summary>
		// Token: 0x040012DC RID: 4828
		public const float MinValue = -3.4028235E+38f;

		/// <summary>Represents the smallest positive <see cref="T:System.Single" /> value that is greater than zero. This field is constant.</summary>
		// Token: 0x040012DD RID: 4829
		public const float Epsilon = 1E-45f;

		/// <summary>Represents the largest possible value of <see cref="T:System.Single" />. This field is constant.</summary>
		// Token: 0x040012DE RID: 4830
		public const float MaxValue = 3.4028235E+38f;

		/// <summary>Represents positive infinity. This field is constant.</summary>
		// Token: 0x040012DF RID: 4831
		public const float PositiveInfinity = float.PositiveInfinity;

		/// <summary>Represents negative infinity. This field is constant.</summary>
		// Token: 0x040012E0 RID: 4832
		public const float NegativeInfinity = float.NegativeInfinity;

		/// <summary>Represents not a number (<see langword="NaN" />). This field is constant.</summary>
		// Token: 0x040012E1 RID: 4833
		public const float NaN = float.NaN;

		// Token: 0x040012E2 RID: 4834
		internal const float NegativeZero = -0f;
	}
}
