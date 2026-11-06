using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	/// <summary>Represents an 8-bit signed integer.</summary>
	// Token: 0x0200017A RID: 378
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct SByte : IComparable, IConvertible, IFormattable, IComparable<sbyte>, IEquatable<sbyte>, ISpanFormattable
	{
		/// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
		/// <param name="obj">An object to compare, or <see langword="null" />.</param>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="obj" />.  
		///   Return Value  
		///
		///   Description  
		///
		///   Less than zero  
		///
		///   This instance is less than <paramref name="obj" />.  
		///
		///   Zero  
		///
		///   This instance is equal to <paramref name="obj" />.  
		///
		///   Greater than zero  
		///
		///   This instance is greater than <paramref name="obj" />.  
		///
		///  -or-  
		///
		///  <paramref name="obj" /> is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="obj" /> is not an <see cref="T:System.SByte" />.</exception>
		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003CAA6 File Offset: 0x0003ACA6
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is sbyte))
			{
				throw new ArgumentException("Object must be of type SByte.");
			}
			return (int)(this - (sbyte)obj);
		}

		/// <summary>Compares this instance to a specified 8-bit signed integer and returns an indication of their relative values.</summary>
		/// <param name="value">An 8-bit signed integer to compare.</param>
		/// <returns>A signed integer that indicates the relative order of this instance and <paramref name="value" />.  
		///   Return Value  
		///
		///   Description  
		///
		///   Less than zero  
		///
		///   This instance is less than <paramref name="value" />.  
		///
		///   Zero  
		///
		///   This instance is equal to <paramref name="value" />.  
		///
		///   Greater than zero  
		///
		///   This instance is greater than <paramref name="value" />.</returns>
		// Token: 0x06000EEA RID: 3818 RVA: 0x0003CAC9 File Offset: 0x0003ACC9
		public int CompareTo(sbyte value)
		{
			return (int)(this - value);
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.SByte" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000EEB RID: 3819 RVA: 0x0003CACF File Offset: 0x0003ACCF
		public override bool Equals(object obj)
		{
			return obj is sbyte && this == (sbyte)obj;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.SByte" /> value.</summary>
		/// <param name="obj">An <see cref="T:System.SByte" /> value to compare to this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000EEC RID: 3820 RVA: 0x0003CAE5 File Offset: 0x0003ACE5
		[NonVersionable]
		public bool Equals(sbyte obj)
		{
			return this == obj;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06000EED RID: 3821 RVA: 0x0003CAEC File Offset: 0x0003ACEC
		public override int GetHashCode()
		{
			return (int)this ^ ((int)this << 8);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
		// Token: 0x06000EEE RID: 3822 RVA: 0x0003CAF5 File Offset: 0x0003ACF5
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance, as specified by <paramref name="provider" />.</returns>
		// Token: 0x06000EEF RID: 3823 RVA: 0x0003CB05 File Offset: 0x0003AD05
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, provider);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
		/// <param name="format">A standard or custom numeric format string.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.</exception>
		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003CB15 File Offset: 0x0003AD15
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <param name="format">A standard or custom numeric format string.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid.</exception>
		// Token: 0x06000EF1 RID: 3825 RVA: 0x0003CB20 File Offset: 0x0003AD20
		public string ToString(string format, IFormatProvider provider)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				return Number.FormatUInt32((uint)this & 255U, format, provider);
			}
			return Number.FormatInt32((int)this, format, provider);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0003CB78 File Offset: 0x0003AD78
		public unsafe bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			if (this < 0 && format.Length > 0 && (*format[0] == 88 || *format[0] == 120))
			{
				return Number.TryFormatUInt32((uint)this & 255U, format, provider, destination, out charsWritten);
			}
			return Number.TryFormatInt32((int)this, format, provider, destination, out charsWritten);
		}

		/// <summary>Converts the string representation of a number to its 8-bit signed integer equivalent.</summary>
		/// <param name="s">A string that represents a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
		/// <returns>An 8-bit signed integer that is equivalent to the number contained in the <paramref name="s" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not consist of an optional sign followed by a sequence of digits (zero through nine).</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003CBCD File Offset: 0x0003ADCD
		[CLSCompliant(false)]
		public static sbyte Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the string representation of a number in a specified style to its 8-bit signed integer equivalent.</summary>
		/// <param name="s">A string that contains a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
		/// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <returns>An 8-bit signed integer that is equivalent to the number specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in a format that is compliant with <paramref name="style" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.  
		/// -or-  
		/// <paramref name="s" /> includes non-zero, fractional digits.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003CBEA File Offset: 0x0003ADEA
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>Converts the string representation of a number in a specified culture-specific format to its 8-bit signed integer equivalent.</summary>
		/// <param name="s">A string that represents a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
		/// <returns>An 8-bit signed integer that is equivalent to the number specified in <paramref name="s" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003CC0D File Offset: 0x0003AE0D
		[CLSCompliant(false)]
		public static sbyte Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		/// <summary>Converts the string representation of a number that is in a specified style and culture-specific format to its 8-bit signed equivalent.</summary>
		/// <param name="s">A string that contains the number to convert. The string is interpreted by using the style specified by <paramref name="style" />.</param>
		/// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
		/// <returns>An 8-bit signed byte value that is equivalent to the number specified in the <paramref name="s" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in a format that is compliant with <paramref name="style" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.  
		/// -or-  
		/// <paramref name="s" /> includes non-zero, fractional digits.</exception>
		// Token: 0x06000EF6 RID: 3830 RVA: 0x0003CC2B File Offset: 0x0003AE2B
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003CC4F File Offset: 0x0003AE4F
		[CLSCompliant(false)]
		public static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003CC64 File Offset: 0x0003AE64
		private static sbyte Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, info);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0003CC80 File Offset: 0x0003AE80
		private static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException("Value was either too large or too small for a signed byte.", ex);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					throw new OverflowException("Value was either too large or too small for a signed byte.");
				}
				return (sbyte)num;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					throw new OverflowException("Value was either too large or too small for a signed byte.");
				}
				return (sbyte)num;
			}
		}

		/// <summary>Tries to convert the string representation of a number to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
		/// <param name="s">A string that contains a number to convert.</param>
		/// <param name="result">When this method returns, contains the 8-bit signed integer value that is equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in the correct format, or represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000EFA RID: 3834 RVA: 0x0003CCF4 File Offset: 0x0003AEF4
		[CLSCompliant(false)]
		public static bool TryParse(string s, out sbyte result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0003CD10 File Offset: 0x0003AF10
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, out sbyte result)
		{
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		/// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
		/// <param name="s">A string representing a number to convert.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
		/// <param name="result">When this method returns, contains the 8-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.  
		/// -or-  
		/// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		// Token: 0x06000EFC RID: 3836 RVA: 0x0003CD1F File Offset: 0x0003AF1F
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003CD42 File Offset: 0x0003AF42
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003CD58 File Offset: 0x0003AF58
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out sbyte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.SByte" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.SByte" />.</returns>
		// Token: 0x06000EFF RID: 3839 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		public TypeCode GetTypeCode()
		{
			return TypeCode.SByte;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is unused.</param>
		/// <returns>
		///   <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000F00 RID: 3840 RVA: 0x0003CDA7 File Offset: 0x0003AFA7
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Char" />.</returns>
		// Token: 0x06000F01 RID: 3841 RVA: 0x0003CDB0 File Offset: 0x0003AFB0
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, unchanged.</returns>
		// Token: 0x06000F02 RID: 3842 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is unused.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
		// Token: 0x06000F03 RID: 3843 RVA: 0x0003CDBD File Offset: 0x0003AFBD
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int16" />.</returns>
		// Token: 0x06000F04 RID: 3844 RVA: 0x0003CDC6 File Offset: 0x0003AFC6
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
		// Token: 0x06000F05 RID: 3845 RVA: 0x0003CDCF File Offset: 0x0003AFCF
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int32" />.</returns>
		// Token: 0x06000F06 RID: 3846 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
		// Token: 0x06000F07 RID: 3847 RVA: 0x0003CDD8 File Offset: 0x0003AFD8
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int64" />.</returns>
		// Token: 0x06000F08 RID: 3848 RVA: 0x0003CDE1 File Offset: 0x0003AFE1
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
		// Token: 0x06000F09 RID: 3849 RVA: 0x0003CDEA File Offset: 0x0003AFEA
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
		// Token: 0x06000F0A RID: 3850 RVA: 0x0003CDF3 File Offset: 0x0003AFF3
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
		// Token: 0x06000F0B RID: 3851 RVA: 0x0003CDFC File Offset: 0x0003AFFC
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
		/// <param name="provider">This parameter is unused.</param>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
		// Token: 0x06000F0C RID: 3852 RVA: 0x0003CE05 File Offset: 0x0003B005
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		/// <summary>This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <param name="provider">This parameter is ignored.</param>
		/// <returns>None. This conversion is not supported.</returns>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		// Token: 0x06000F0D RID: 3853 RVA: 0x0003CE0E File Offset: 0x0003B00E
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "SByte", "DateTime"));
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> to which to convert this <see cref="T:System.SByte" /> value.</param>
		/// <param name="provider">A <see cref="T:System.IFormatProvider" /> implementation that provides information about the format of the returned value.</param>
		/// <returns>The value of the current instance, converted to an object of type <paramref name="type" />.</returns>
		// Token: 0x06000F0E RID: 3854 RVA: 0x0003CE29 File Offset: 0x0003B029
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040012D8 RID: 4824
		private readonly sbyte m_value;

		/// <summary>Represents the largest possible value of <see cref="T:System.SByte" />. This field is constant.</summary>
		// Token: 0x040012D9 RID: 4825
		public const sbyte MaxValue = 127;

		/// <summary>Represents the smallest possible value of <see cref="T:System.SByte" />. This field is constant.</summary>
		// Token: 0x040012DA RID: 4826
		public const sbyte MinValue = -128;
	}
}
