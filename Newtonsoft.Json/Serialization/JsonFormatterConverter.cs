using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x0001C0A1 File Offset: 0x0001A2A1
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, [Nullable(2)] JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
		private T GetTokenValue<[Nullable(2)] T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)global::System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001C108 File Offset: 0x0001A308
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001C154 File Offset: 0x0001A354
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = value as JValue;
			return global::System.Convert.ChangeType((jvalue != null) ? jvalue.Value : value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001C18A File Offset: 0x0001A38A
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001C193 File Offset: 0x0001A393
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001C19C File Offset: 0x0001A39C
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001C1A5 File Offset: 0x0001A3A5
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001C1AE File Offset: 0x0001A3AE
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001C1B7 File Offset: 0x0001A3B7
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001C1C9 File Offset: 0x0001A3C9
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001C1D2 File Offset: 0x0001A3D2
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001C1DB File Offset: 0x0001A3DB
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001C1ED File Offset: 0x0001A3ED
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001C1F6 File Offset: 0x0001A3F6
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001C1FF File Offset: 0x0001A3FF
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001C208 File Offset: 0x0001A408
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x04000288 RID: 648
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x04000289 RID: 649
		private readonly JsonISerializableContract _contract;

		// Token: 0x0400028A RID: 650
		[Nullable(2)]
		private readonly JsonProperty _member;
	}
}
