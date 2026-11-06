using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Runtime.Serialization
{
	/// <summary>Represents a base implementation of the <see cref="T:System.Runtime.Serialization.IFormatterConverter" /> interface that uses the <see cref="T:System.Convert" /> class and the <see cref="T:System.IConvertible" /> interface.</summary>
	// Token: 0x02000651 RID: 1617
	public class FormatterConverter : IFormatterConverter
	{
		/// <summary>Converts a value to the given <see cref="T:System.Type" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="type">The <see cref="T:System.Type" /> into which <paramref name="value" /> is converted.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C87 RID: 15495 RVA: 0x000D1935 File Offset: 0x000CFB35
		public object Convert(object value, Type type)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to the given <see cref="T:System.TypeCode" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="typeCode">The <see cref="T:System.TypeCode" /> into which <paramref name="value" /> is converted.</param>
		/// <returns>The converted <paramref name="value" />, or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C88 RID: 15496 RVA: 0x000D194B File Offset: 0x000CFB4B
		public object Convert(object value, TypeCode typeCode)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a <see cref="T:System.Boolean" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C89 RID: 15497 RVA: 0x000D1961 File Offset: 0x000CFB61
		public bool ToBoolean(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a Unicode character.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8A RID: 15498 RVA: 0x000D1976 File Offset: 0x000CFB76
		public char ToChar(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToChar(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a <see cref="T:System.SByte" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8B RID: 15499 RVA: 0x000D198B File Offset: 0x000CFB8B
		[CLSCompliant(false)]
		public sbyte ToSByte(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToSByte(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to an 8-bit unsigned integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8C RID: 15500 RVA: 0x000D19A0 File Offset: 0x000CFBA0
		public byte ToByte(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToByte(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 16-bit signed integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8D RID: 15501 RVA: 0x000D19B5 File Offset: 0x000CFBB5
		public short ToInt16(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToInt16(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 16-bit unsigned integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8E RID: 15502 RVA: 0x000D19CA File Offset: 0x000CFBCA
		[CLSCompliant(false)]
		public ushort ToUInt16(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToUInt16(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 32-bit signed integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C8F RID: 15503 RVA: 0x000D19DF File Offset: 0x000CFBDF
		public int ToInt32(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 32-bit unsigned integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C90 RID: 15504 RVA: 0x000D19F4 File Offset: 0x000CFBF4
		[CLSCompliant(false)]
		public uint ToUInt32(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToUInt32(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 64-bit signed integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C91 RID: 15505 RVA: 0x000D1A09 File Offset: 0x000CFC09
		public long ToInt64(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToInt64(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a 64-bit unsigned integer.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C92 RID: 15506 RVA: 0x000D1A1E File Offset: 0x000CFC1E
		[CLSCompliant(false)]
		public ulong ToUInt64(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToUInt64(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a single-precision floating-point number.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C93 RID: 15507 RVA: 0x000D1A33 File Offset: 0x000CFC33
		public float ToSingle(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a double-precision floating-point number.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C94 RID: 15508 RVA: 0x000D1A48 File Offset: 0x000CFC48
		public double ToDouble(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a <see cref="T:System.Decimal" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C95 RID: 15509 RVA: 0x000D1A5D File Offset: 0x000CFC5D
		public decimal ToDecimal(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts a value to a <see cref="T:System.DateTime" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C96 RID: 15510 RVA: 0x000D1A72 File Offset: 0x000CFC72
		public DateTime ToDateTime(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToDateTime(value, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified object to a <see cref="T:System.String" />.</summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted <paramref name="value" /> or <see langword="null" /> if the <paramref name="type" /> parameter is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C97 RID: 15511 RVA: 0x000D1A87 File Offset: 0x000CFC87
		public string ToString(object value)
		{
			if (value == null)
			{
				FormatterConverter.ThrowValueNullException();
			}
			return global::System.Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000D1A9C File Offset: 0x000CFC9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowValueNullException()
		{
			throw new ArgumentNullException("value");
		}
	}
}
