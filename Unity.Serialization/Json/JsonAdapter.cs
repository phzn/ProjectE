using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x0200001D RID: 29
	internal class JsonAdapter : IJsonAdapter<sbyte>, IJsonAdapter, IJsonAdapter<short>, IJsonAdapter<int>, IJsonAdapter<long>, IJsonAdapter<byte>, IJsonAdapter<ushort>, IJsonAdapter<uint>, IJsonAdapter<ulong>, IJsonAdapter<float>, IJsonAdapter<double>, IJsonAdapter<bool>, IJsonAdapter<char>, IJsonAdapter<string>, IJsonAdapter<SerializedValueView>, IJsonAdapter<Guid>, IJsonAdapter<DateTime>, IJsonAdapter<TimeSpan>, IJsonAdapter<Version>, IJsonAdapter<DirectoryInfo>, IJsonAdapter<FileInfo>, IContravariantJsonAdapter<Object>
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002900 File Offset: 0x00000B00
		void IJsonAdapter<sbyte>.Serialize(in JsonSerializationContext<sbyte> context, sbyte value)
		{
			context.Writer.WriteValue((int)value);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000291C File Offset: 0x00000B1C
		void IJsonAdapter<short>.Serialize(in JsonSerializationContext<short> context, short value)
		{
			context.Writer.WriteValue((int)value);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002938 File Offset: 0x00000B38
		void IJsonAdapter<int>.Serialize(in JsonSerializationContext<int> context, int value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002954 File Offset: 0x00000B54
		void IJsonAdapter<long>.Serialize(in JsonSerializationContext<long> context, long value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002970 File Offset: 0x00000B70
		void IJsonAdapter<byte>.Serialize(in JsonSerializationContext<byte> context, byte value)
		{
			context.Writer.WriteValue((int)value);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000298C File Offset: 0x00000B8C
		void IJsonAdapter<ushort>.Serialize(in JsonSerializationContext<ushort> context, ushort value)
		{
			context.Writer.WriteValue((int)value);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000029A8 File Offset: 0x00000BA8
		void IJsonAdapter<uint>.Serialize(in JsonSerializationContext<uint> context, uint value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000029C4 File Offset: 0x00000BC4
		void IJsonAdapter<ulong>.Serialize(in JsonSerializationContext<ulong> context, ulong value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000029E0 File Offset: 0x00000BE0
		void IJsonAdapter<float>.Serialize(in JsonSerializationContext<float> context, float value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000029FC File Offset: 0x00000BFC
		void IJsonAdapter<double>.Serialize(in JsonSerializationContext<double> context, double value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002A18 File Offset: 0x00000C18
		void IJsonAdapter<bool>.Serialize(in JsonSerializationContext<bool> context, bool value)
		{
			context.Writer.WriteValueLiteral(value ? "true" : "false");
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002A44 File Offset: 0x00000C44
		void IJsonAdapter<char>.Serialize(in JsonSerializationContext<char> context, char value)
		{
			context.Writer.WriteValue((int)value);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A60 File Offset: 0x00000C60
		void IJsonAdapter<string>.Serialize(in JsonSerializationContext<string> context, string value)
		{
			context.Writer.WriteValue(value);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A7C File Offset: 0x00000C7C
		sbyte IJsonAdapter<sbyte>.Deserialize(in JsonDeserializationContext<sbyte> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002A7C File Offset: 0x00000C7C
		short IJsonAdapter<short>.Deserialize(in JsonDeserializationContext<short> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002A7C File Offset: 0x00000C7C
		int IJsonAdapter<int>.Deserialize(in JsonDeserializationContext<int> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002A7C File Offset: 0x00000C7C
		long IJsonAdapter<long>.Deserialize(in JsonDeserializationContext<long> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002A7C File Offset: 0x00000C7C
		byte IJsonAdapter<byte>.Deserialize(in JsonDeserializationContext<byte> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002A7C File Offset: 0x00000C7C
		ushort IJsonAdapter<ushort>.Deserialize(in JsonDeserializationContext<ushort> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A7C File Offset: 0x00000C7C
		uint IJsonAdapter<uint>.Deserialize(in JsonDeserializationContext<uint> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002A7C File Offset: 0x00000C7C
		ulong IJsonAdapter<ulong>.Deserialize(in JsonDeserializationContext<ulong> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002A7C File Offset: 0x00000C7C
		float IJsonAdapter<float>.Deserialize(in JsonDeserializationContext<float> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002A7C File Offset: 0x00000C7C
		double IJsonAdapter<double>.Deserialize(in JsonDeserializationContext<double> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A7C File Offset: 0x00000C7C
		bool IJsonAdapter<bool>.Deserialize(in JsonDeserializationContext<bool> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002A7C File Offset: 0x00000C7C
		char IJsonAdapter<char>.Deserialize(in JsonDeserializationContext<char> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002A7C File Offset: 0x00000C7C
		string IJsonAdapter<string>.Deserialize(in JsonDeserializationContext<string> context)
		{
			throw new NotImplementedException("This code should never be executed. JsonPropertyReader should handle primitives in a specialized way.");
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002A88 File Offset: 0x00000C88
		void IJsonAdapter<SerializedValueView>.Serialize(in JsonSerializationContext<SerializedValueView> context, SerializedValueView value)
		{
			switch (value.Type)
			{
			case TokenType.Object:
				context.SerializeValue<SerializedObjectView>(value.AsObjectView());
				return;
			case TokenType.Array:
				context.SerializeValue<SerializedArrayView>(value.AsArrayView());
				return;
			case TokenType.String:
				context.SerializeValue<string>(value.AsStringView().ToString());
				return;
			case TokenType.Primitive:
			{
				SerializedPrimitiveView serializedPrimitiveView = value.AsPrimitiveView();
				if (serializedPrimitiveView.IsIntegral())
				{
					if (serializedPrimitiveView.IsSigned())
					{
						context.SerializeValue<long>(value.AsInt64());
						return;
					}
					context.SerializeValue<ulong>(value.AsUInt64());
					return;
				}
				else
				{
					if (serializedPrimitiveView.IsDecimal() || serializedPrimitiveView.IsInfinity() || serializedPrimitiveView.IsNaN())
					{
						context.SerializeValue<double>(value.AsDouble());
						return;
					}
					if (serializedPrimitiveView.IsBoolean())
					{
						context.SerializeValue<bool>(value.AsBoolean());
						return;
					}
					context.Writer.WriteNull();
					return;
				}
				break;
			}
			default:
				context.Writer.WriteNull();
				return;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002B85 File Offset: 0x00000D85
		SerializedValueView IJsonAdapter<SerializedValueView>.Deserialize(in JsonDeserializationContext<SerializedValueView> context)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002B8C File Offset: 0x00000D8C
		void IJsonAdapter<Guid>.Serialize(in JsonSerializationContext<Guid> context, Guid value)
		{
			context.Writer.WriteValue(value.ToString("N", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002BB8 File Offset: 0x00000DB8
		Guid IJsonAdapter<Guid>.Deserialize(in JsonDeserializationContext<Guid> context)
		{
			Guid guid;
			if (!Guid.TryParseExact(context.SerializedValue.ToString(), "N", out guid))
			{
				return default(Guid);
			}
			return guid;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002BF4 File Offset: 0x00000DF4
		void IJsonAdapter<DateTime>.Serialize(in JsonSerializationContext<DateTime> context, DateTime value)
		{
			context.Writer.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002C20 File Offset: 0x00000E20
		DateTime IJsonAdapter<DateTime>.Deserialize(in JsonDeserializationContext<DateTime> context)
		{
			DateTime dateTime;
			if (!DateTime.TryParseExact(context.SerializedValue.ToString(), "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				return default(DateTime);
			}
			return dateTime;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002C64 File Offset: 0x00000E64
		void IJsonAdapter<TimeSpan>.Serialize(in JsonSerializationContext<TimeSpan> context, TimeSpan value)
		{
			context.Writer.WriteValue(value.ToString("c", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002C90 File Offset: 0x00000E90
		TimeSpan IJsonAdapter<TimeSpan>.Deserialize(in JsonDeserializationContext<TimeSpan> context)
		{
			TimeSpan timeSpan;
			if (!TimeSpan.TryParseExact(context.SerializedValue.ToString(), "c", CultureInfo.InvariantCulture, out timeSpan))
			{
				return default(TimeSpan);
			}
			return timeSpan;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002CD0 File Offset: 0x00000ED0
		void IJsonAdapter<Version>.Serialize(in JsonSerializationContext<Version> context, Version value)
		{
			context.Writer.WriteValue(value.ToString());
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002CF4 File Offset: 0x00000EF4
		Version IJsonAdapter<Version>.Deserialize(in JsonDeserializationContext<Version> context)
		{
			return new Version(context.SerializedValue.ToString());
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002D1C File Offset: 0x00000F1C
		void IJsonAdapter<DirectoryInfo>.Serialize(in JsonSerializationContext<DirectoryInfo> context, DirectoryInfo value)
		{
			if (value == null)
			{
				context.Writer.WriteNull();
				return;
			}
			context.Writer.WriteValue(value.GetRelativePath());
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002D50 File Offset: 0x00000F50
		DirectoryInfo IJsonAdapter<DirectoryInfo>.Deserialize(in JsonDeserializationContext<DirectoryInfo> context)
		{
			if (!context.SerializedValue.AsStringView().Equals("null"))
			{
				return new DirectoryInfo(context.SerializedValue.ToString());
			}
			return null;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002D98 File Offset: 0x00000F98
		void IJsonAdapter<FileInfo>.Serialize(in JsonSerializationContext<FileInfo> context, FileInfo value)
		{
			if (value == null)
			{
				context.Writer.WriteNull();
				return;
			}
			context.Writer.WriteValue(value.GetRelativePath());
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002DCC File Offset: 0x00000FCC
		FileInfo IJsonAdapter<FileInfo>.Deserialize(in JsonDeserializationContext<FileInfo> context)
		{
			if (!context.SerializedValue.AsStringView().Equals("null"))
			{
				return new FileInfo(context.SerializedValue.ToString());
			}
			return null;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002E14 File Offset: 0x00001014
		void IContravariantJsonAdapter<Object>.Serialize(IJsonSerializationContext context, Object value)
		{
			context.Writer.WriteNull();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002E2F File Offset: 0x0000102F
		object IContravariantJsonAdapter<Object>.Deserialize(IJsonDeserializationContext context)
		{
			return null;
		}
	}
}
