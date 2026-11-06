using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000145 RID: 325
	[StructLayout(LayoutKind.Explicit)]
	public struct PrimitiveValue : IEquatable<PrimitiveValue>, IConvertible
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00054A0E File Offset: 0x00052C0E
		internal unsafe byte* valuePtr
		{
			get
			{
				return (byte*)UnsafeUtility.AddressOf<PrimitiveValue>(ref this) + 4;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00054A18 File Offset: 0x00052C18
		public TypeCode type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00054A20 File Offset: 0x00052C20
		public bool isEmpty
		{
			get
			{
				return this.type == TypeCode.Empty;
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00054A2B File Offset: 0x00052C2B
		public PrimitiveValue(bool value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Boolean;
			this.m_BoolValue = value;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00054A42 File Offset: 0x00052C42
		public PrimitiveValue(char value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Char;
			this.m_CharValue = value;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00054A59 File Offset: 0x00052C59
		public PrimitiveValue(byte value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Byte;
			this.m_ByteValue = value;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00054A70 File Offset: 0x00052C70
		public PrimitiveValue(sbyte value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.SByte;
			this.m_SByteValue = value;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00054A87 File Offset: 0x00052C87
		public PrimitiveValue(short value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Int16;
			this.m_ShortValue = value;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00054A9E File Offset: 0x00052C9E
		public PrimitiveValue(ushort value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.UInt16;
			this.m_UShortValue = value;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00054AB5 File Offset: 0x00052CB5
		public PrimitiveValue(int value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Int32;
			this.m_IntValue = value;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00054ACD File Offset: 0x00052CCD
		public PrimitiveValue(uint value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.UInt32;
			this.m_UIntValue = value;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00054AE5 File Offset: 0x00052CE5
		public PrimitiveValue(long value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Int64;
			this.m_LongValue = value;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00054AFD File Offset: 0x00052CFD
		public PrimitiveValue(ulong value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.UInt64;
			this.m_ULongValue = value;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00054B15 File Offset: 0x00052D15
		public PrimitiveValue(float value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Single;
			this.m_FloatValue = value;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00054B2D File Offset: 0x00052D2D
		public PrimitiveValue(double value)
		{
			this = default(PrimitiveValue);
			this.m_Type = TypeCode.Double;
			this.m_DoubleValue = value;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00054B48 File Offset: 0x00052D48
		public PrimitiveValue ConvertTo(TypeCode type)
		{
			switch (type)
			{
			case TypeCode.Empty:
				return default(PrimitiveValue);
			case TypeCode.Boolean:
				return this.ToBoolean(null);
			case TypeCode.Char:
				return this.ToChar(null);
			case TypeCode.SByte:
				return this.ToSByte(null);
			case TypeCode.Byte:
				return this.ToByte(null);
			case TypeCode.Int16:
				return this.ToInt16(null);
			case TypeCode.UInt16:
				return this.ToInt16(null);
			case TypeCode.Int32:
				return this.ToInt32(null);
			case TypeCode.UInt32:
				return this.ToInt32(null);
			case TypeCode.Int64:
				return this.ToInt64(null);
			case TypeCode.UInt64:
				return this.ToUInt64(null);
			case TypeCode.Single:
				return this.ToSingle(null);
			case TypeCode.Double:
				return this.ToDouble(null);
			}
			throw new ArgumentException(string.Format("Don't know how to convert PrimitiveValue to '{0}'", type), "type");
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00054C5C File Offset: 0x00052E5C
		public unsafe bool Equals(PrimitiveValue other)
		{
			if (this.m_Type != other.m_Type)
			{
				return false;
			}
			void* ptr = UnsafeUtility.AddressOf<double>(ref this.m_DoubleValue);
			void* ptr2 = UnsafeUtility.AddressOf<double>(ref other.m_DoubleValue);
			return UnsafeUtility.MemCmp(ptr, ptr2, 8L) == 0;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00054C9C File Offset: 0x00052E9C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is PrimitiveValue)
			{
				PrimitiveValue primitiveValue = (PrimitiveValue)obj;
				return this.Equals(primitiveValue);
			}
			return (obj is bool || obj is char || obj is byte || obj is sbyte || obj is short || obj is ushort || obj is int || obj is uint || obj is long || obj is ulong || obj is float || obj is double) && this.Equals(PrimitiveValue.FromObject(obj));
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00054D33 File Offset: 0x00052F33
		public static bool operator ==(PrimitiveValue left, PrimitiveValue right)
		{
			return left.Equals(right);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00054D3D File Offset: 0x00052F3D
		public static bool operator !=(PrimitiveValue left, PrimitiveValue right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00054D4C File Offset: 0x00052F4C
		public unsafe override int GetHashCode()
		{
			fixed (double* ptr = &this.m_DoubleValue)
			{
				double* ptr2 = ptr;
				return (this.m_Type.GetHashCode() * 397) ^ ptr2->GetHashCode();
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00054D84 File Offset: 0x00052F84
		public override string ToString()
		{
			switch (this.type)
			{
			case TypeCode.Boolean:
				if (!this.m_BoolValue)
				{
					return "false";
				}
				return "true";
			case TypeCode.Char:
				return "'" + this.m_CharValue.ToString() + "'";
			case TypeCode.SByte:
				return this.m_SByteValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Byte:
				return this.m_ByteValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Int16:
				return this.m_ShortValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.UInt16:
				return this.m_UShortValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Int32:
				return this.m_IntValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.UInt32:
				return this.m_UIntValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Int64:
				return this.m_LongValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.UInt64:
				return this.m_ULongValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Single:
				return this.m_FloatValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			case TypeCode.Double:
				return this.m_DoubleValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			default:
				return string.Empty;
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00054EE8 File Offset: 0x000530E8
		public static PrimitiveValue FromString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return default(PrimitiveValue);
			}
			if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return new PrimitiveValue(true);
			}
			if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
			{
				return new PrimitiveValue(false);
			}
			double num;
			if ((value.Contains('.') || value.Contains("e") || value.Contains("E") || value.Contains("infinity", StringComparison.InvariantCultureIgnoreCase)) && double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return new PrimitiveValue(num);
			}
			long num2;
			if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
			{
				return new PrimitiveValue(num2);
			}
			if (value.IndexOf("0x", StringComparison.InvariantCultureIgnoreCase) != -1)
			{
				string text = value.TrimStart();
				if (text.StartsWith("0x"))
				{
					text = text.Substring(2);
				}
				long num3;
				if (long.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3))
				{
					return new PrimitiveValue(num3);
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00054FE1 File Offset: 0x000531E1
		public TypeCode GetTypeCode()
		{
			return this.type;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00054FEC File Offset: 0x000531EC
		public bool ToBoolean(IFormatProvider provider = null)
		{
			switch (this.type)
			{
			case TypeCode.Boolean:
				return this.m_BoolValue;
			case TypeCode.Char:
				return this.m_CharValue > '\0';
			case TypeCode.SByte:
				return this.m_SByteValue != 0;
			case TypeCode.Byte:
				return this.m_ByteValue > 0;
			case TypeCode.Int16:
				return this.m_ShortValue != 0;
			case TypeCode.UInt16:
				return this.m_UShortValue > 0;
			case TypeCode.Int32:
				return this.m_IntValue != 0;
			case TypeCode.UInt32:
				return this.m_UIntValue > 0U;
			case TypeCode.Int64:
				return this.m_LongValue != 0L;
			case TypeCode.UInt64:
				return this.m_ULongValue > 0UL;
			case TypeCode.Single:
				return !Mathf.Approximately(this.m_FloatValue, 0f);
			case TypeCode.Double:
				return !NumberHelpers.Approximately(this.m_DoubleValue, 0.0);
			default:
				return false;
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000550CD File Offset: 0x000532CD
		public byte ToByte(IFormatProvider provider = null)
		{
			return (byte)this.ToInt64(provider);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000550D8 File Offset: 0x000532D8
		public char ToChar(IFormatProvider provider = null)
		{
			TypeCode type = this.type;
			if (type == TypeCode.Char)
			{
				return this.m_CharValue;
			}
			if (type - TypeCode.Int16 > 5)
			{
				return '\0';
			}
			return (char)this.ToInt64(provider);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00055109 File Offset: 0x00053309
		public DateTime ToDateTime(IFormatProvider provider = null)
		{
			throw new NotSupportedException("Converting PrimitiveValue to DateTime");
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00055115 File Offset: 0x00053315
		public decimal ToDecimal(IFormatProvider provider = null)
		{
			return new decimal(this.ToDouble(provider));
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00055124 File Offset: 0x00053324
		public double ToDouble(IFormatProvider provider = null)
		{
			switch (this.type)
			{
			case TypeCode.Boolean:
				if (this.m_BoolValue)
				{
					return 1.0;
				}
				return 0.0;
			case TypeCode.Char:
				return (double)this.m_CharValue;
			case TypeCode.SByte:
				return (double)this.m_SByteValue;
			case TypeCode.Byte:
				return (double)this.m_ByteValue;
			case TypeCode.Int16:
				return (double)this.m_ShortValue;
			case TypeCode.UInt16:
				return (double)this.m_UShortValue;
			case TypeCode.Int32:
				return (double)this.m_IntValue;
			case TypeCode.UInt32:
				return this.m_UIntValue;
			case TypeCode.Int64:
				return (double)this.m_LongValue;
			case TypeCode.UInt64:
				return this.m_ULongValue;
			case TypeCode.Single:
				return (double)this.m_FloatValue;
			case TypeCode.Double:
				return this.m_DoubleValue;
			default:
				return 0.0;
			}
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000551F0 File Offset: 0x000533F0
		public short ToInt16(IFormatProvider provider = null)
		{
			return (short)this.ToInt64(provider);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000551FA File Offset: 0x000533FA
		public int ToInt32(IFormatProvider provider = null)
		{
			return (int)this.ToInt64(provider);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00055204 File Offset: 0x00053404
		public long ToInt64(IFormatProvider provider = null)
		{
			switch (this.type)
			{
			case TypeCode.Boolean:
				if (this.m_BoolValue)
				{
					return 1L;
				}
				return 0L;
			case TypeCode.Char:
				return (long)((ulong)this.m_CharValue);
			case TypeCode.SByte:
				return (long)this.m_SByteValue;
			case TypeCode.Byte:
				return (long)((ulong)this.m_ByteValue);
			case TypeCode.Int16:
				return (long)this.m_ShortValue;
			case TypeCode.UInt16:
				return (long)((ulong)this.m_UShortValue);
			case TypeCode.Int32:
				return (long)this.m_IntValue;
			case TypeCode.UInt32:
				return (long)((ulong)this.m_UIntValue);
			case TypeCode.Int64:
				return this.m_LongValue;
			case TypeCode.UInt64:
				return (long)this.m_ULongValue;
			case TypeCode.Single:
				return (long)this.m_FloatValue;
			case TypeCode.Double:
				return (long)this.m_DoubleValue;
			default:
				return 0L;
			}
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x000552B8 File Offset: 0x000534B8
		public sbyte ToSByte(IFormatProvider provider = null)
		{
			return (sbyte)this.ToInt64(provider);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000552C2 File Offset: 0x000534C2
		public float ToSingle(IFormatProvider provider = null)
		{
			return (float)this.ToDouble(provider);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x000552CC File Offset: 0x000534CC
		public string ToString(IFormatProvider provider)
		{
			return this.ToString();
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000552DA File Offset: 0x000534DA
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000552E1 File Offset: 0x000534E1
		public ushort ToUInt16(IFormatProvider provider = null)
		{
			return (ushort)this.ToUInt64(null);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000552EB File Offset: 0x000534EB
		public uint ToUInt32(IFormatProvider provider = null)
		{
			return (uint)this.ToUInt64(null);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000552F8 File Offset: 0x000534F8
		public ulong ToUInt64(IFormatProvider provider = null)
		{
			switch (this.type)
			{
			case TypeCode.Boolean:
				if (this.m_BoolValue)
				{
					return 1UL;
				}
				return 0UL;
			case TypeCode.Char:
				return (ulong)this.m_CharValue;
			case TypeCode.SByte:
				return (ulong)((long)this.m_SByteValue);
			case TypeCode.Byte:
				return (ulong)this.m_ByteValue;
			case TypeCode.Int16:
				return (ulong)((long)this.m_ShortValue);
			case TypeCode.UInt16:
				return (ulong)this.m_UShortValue;
			case TypeCode.Int32:
				return (ulong)((long)this.m_IntValue);
			case TypeCode.UInt32:
				return (ulong)this.m_UIntValue;
			case TypeCode.Int64:
				return (ulong)this.m_LongValue;
			case TypeCode.UInt64:
				return this.m_ULongValue;
			case TypeCode.Single:
				return (ulong)this.m_FloatValue;
			case TypeCode.Double:
				return (ulong)this.m_DoubleValue;
			default:
				return 0UL;
			}
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000553AC File Offset: 0x000535AC
		public object ToObject()
		{
			switch (this.m_Type)
			{
			case TypeCode.Boolean:
				return this.m_BoolValue;
			case TypeCode.Char:
				return this.m_CharValue;
			case TypeCode.SByte:
				return this.m_SByteValue;
			case TypeCode.Byte:
				return this.m_ByteValue;
			case TypeCode.Int16:
				return this.m_ShortValue;
			case TypeCode.UInt16:
				return this.m_UShortValue;
			case TypeCode.Int32:
				return this.m_IntValue;
			case TypeCode.UInt32:
				return this.m_UIntValue;
			case TypeCode.Int64:
				return this.m_LongValue;
			case TypeCode.UInt64:
				return this.m_ULongValue;
			case TypeCode.Single:
				return this.m_FloatValue;
			case TypeCode.Double:
				return this.m_DoubleValue;
			default:
				return null;
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00055490 File Offset: 0x00053690
		public static PrimitiveValue From<TValue>(TValue value) where TValue : struct
		{
			Type type = typeof(TValue);
			if (type.IsEnum)
			{
				type = type.GetEnumUnderlyingType();
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				return new PrimitiveValue(Convert.ToBoolean(value));
			case TypeCode.Char:
				return new PrimitiveValue(Convert.ToChar(value));
			case TypeCode.SByte:
				return new PrimitiveValue(Convert.ToSByte(value));
			case TypeCode.Byte:
				return new PrimitiveValue(Convert.ToByte(value));
			case TypeCode.Int16:
				return new PrimitiveValue(Convert.ToInt16(value));
			case TypeCode.UInt16:
				return new PrimitiveValue(Convert.ToUInt16(value));
			case TypeCode.Int32:
				return new PrimitiveValue(Convert.ToInt32(value));
			case TypeCode.UInt32:
				return new PrimitiveValue(Convert.ToUInt32(value));
			case TypeCode.Int64:
				return new PrimitiveValue(Convert.ToInt64(value));
			case TypeCode.UInt64:
				return new PrimitiveValue(Convert.ToUInt64(value));
			case TypeCode.Single:
				return new PrimitiveValue(Convert.ToSingle(value));
			case TypeCode.Double:
				return new PrimitiveValue(Convert.ToDouble(value));
			default:
				throw new ArgumentException(string.Format("Cannot convert value '{0}' of type '{1}' to PrimitiveValue", value, typeof(TValue).Name), "value");
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000555F0 File Offset: 0x000537F0
		public static PrimitiveValue FromObject(object value)
		{
			if (value == null)
			{
				return default(PrimitiveValue);
			}
			string text = value as string;
			if (text != null)
			{
				return PrimitiveValue.FromString(text);
			}
			if (value is bool)
			{
				bool flag = (bool)value;
				return new PrimitiveValue(flag);
			}
			if (value is char)
			{
				char c = (char)value;
				return new PrimitiveValue(c);
			}
			if (value is byte)
			{
				byte b = (byte)value;
				return new PrimitiveValue(b);
			}
			if (value is sbyte)
			{
				sbyte b2 = (sbyte)value;
				return new PrimitiveValue(b2);
			}
			if (value is short)
			{
				short num = (short)value;
				return new PrimitiveValue(num);
			}
			if (value is ushort)
			{
				ushort num2 = (ushort)value;
				return new PrimitiveValue(num2);
			}
			if (value is int)
			{
				int num3 = (int)value;
				return new PrimitiveValue(num3);
			}
			if (value is uint)
			{
				uint num4 = (uint)value;
				return new PrimitiveValue(num4);
			}
			if (value is long)
			{
				long num5 = (long)value;
				return new PrimitiveValue(num5);
			}
			if (value is ulong)
			{
				ulong num6 = (ulong)value;
				return new PrimitiveValue(num6);
			}
			if (value is float)
			{
				float num7 = (float)value;
				return new PrimitiveValue(num7);
			}
			if (value is double)
			{
				double num8 = (double)value;
				return new PrimitiveValue(num8);
			}
			if (value is Enum)
			{
				switch (Type.GetTypeCode(value.GetType().GetEnumUnderlyingType()))
				{
				case TypeCode.SByte:
					return new PrimitiveValue((sbyte)value);
				case TypeCode.Byte:
					return new PrimitiveValue((byte)value);
				case TypeCode.Int16:
					return new PrimitiveValue((short)value);
				case TypeCode.UInt16:
					return new PrimitiveValue((ushort)value);
				case TypeCode.Int32:
					return new PrimitiveValue((int)value);
				case TypeCode.UInt32:
					return new PrimitiveValue((uint)value);
				case TypeCode.Int64:
					return new PrimitiveValue((long)value);
				case TypeCode.UInt64:
					return new PrimitiveValue((ulong)value);
				}
			}
			throw new ArgumentException(string.Format("Cannot convert '{0}' to primitive value", value), "value");
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000557F3 File Offset: 0x000539F3
		public static implicit operator PrimitiveValue(bool value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000557FB File Offset: 0x000539FB
		public static implicit operator PrimitiveValue(char value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00055803 File Offset: 0x00053A03
		public static implicit operator PrimitiveValue(byte value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0005580B File Offset: 0x00053A0B
		public static implicit operator PrimitiveValue(sbyte value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00055813 File Offset: 0x00053A13
		public static implicit operator PrimitiveValue(short value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0005581B File Offset: 0x00053A1B
		public static implicit operator PrimitiveValue(ushort value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00055823 File Offset: 0x00053A23
		public static implicit operator PrimitiveValue(int value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0005582B File Offset: 0x00053A2B
		public static implicit operator PrimitiveValue(uint value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00055833 File Offset: 0x00053A33
		public static implicit operator PrimitiveValue(long value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0005583B File Offset: 0x00053A3B
		public static implicit operator PrimitiveValue(ulong value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00055843 File Offset: 0x00053A43
		public static implicit operator PrimitiveValue(float value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0005584B File Offset: 0x00053A4B
		public static implicit operator PrimitiveValue(double value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00055853 File Offset: 0x00053A53
		public static PrimitiveValue FromBoolean(bool value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0005585B File Offset: 0x00053A5B
		public static PrimitiveValue FromChar(char value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00055863 File Offset: 0x00053A63
		public static PrimitiveValue FromByte(byte value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0005586B File Offset: 0x00053A6B
		public static PrimitiveValue FromSByte(sbyte value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00055873 File Offset: 0x00053A73
		public static PrimitiveValue FromInt16(short value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0005587B File Offset: 0x00053A7B
		public static PrimitiveValue FromUInt16(ushort value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00055883 File Offset: 0x00053A83
		public static PrimitiveValue FromInt32(int value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0005588B File Offset: 0x00053A8B
		public static PrimitiveValue FromUInt32(uint value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00055893 File Offset: 0x00053A93
		public static PrimitiveValue FromInt64(long value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0005589B File Offset: 0x00053A9B
		public static PrimitiveValue FromUInt64(ulong value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x000558A3 File Offset: 0x00053AA3
		public static PrimitiveValue FromSingle(float value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000558AB File Offset: 0x00053AAB
		public static PrimitiveValue FromDouble(double value)
		{
			return new PrimitiveValue(value);
		}

		// Token: 0x04000729 RID: 1833
		[FieldOffset(0)]
		private TypeCode m_Type;

		// Token: 0x0400072A RID: 1834
		[FieldOffset(4)]
		private bool m_BoolValue;

		// Token: 0x0400072B RID: 1835
		[FieldOffset(4)]
		private char m_CharValue;

		// Token: 0x0400072C RID: 1836
		[FieldOffset(4)]
		private byte m_ByteValue;

		// Token: 0x0400072D RID: 1837
		[FieldOffset(4)]
		private sbyte m_SByteValue;

		// Token: 0x0400072E RID: 1838
		[FieldOffset(4)]
		private short m_ShortValue;

		// Token: 0x0400072F RID: 1839
		[FieldOffset(4)]
		private ushort m_UShortValue;

		// Token: 0x04000730 RID: 1840
		[FieldOffset(4)]
		private int m_IntValue;

		// Token: 0x04000731 RID: 1841
		[FieldOffset(4)]
		private uint m_UIntValue;

		// Token: 0x04000732 RID: 1842
		[FieldOffset(4)]
		private long m_LongValue;

		// Token: 0x04000733 RID: 1843
		[FieldOffset(4)]
		private ulong m_ULongValue;

		// Token: 0x04000734 RID: 1844
		[FieldOffset(4)]
		private float m_FloatValue;

		// Token: 0x04000735 RID: 1845
		[FieldOffset(4)]
		private double m_DoubleValue;
	}
}
