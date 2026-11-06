using System;
using System.Globalization;
using System.IO;
using Unity.Collections.LowLevel.Unsafe.NotBurstCompatible;
using UnityEngine;

namespace Unity.Serialization.Binary
{
	// Token: 0x0200009E RID: 158
	internal class BinaryAdapter : IBinaryAdapter<sbyte>, IBinaryAdapter, IBinaryAdapter<short>, IBinaryAdapter<int>, IBinaryAdapter<long>, IBinaryAdapter<byte>, IBinaryAdapter<ushort>, IBinaryAdapter<uint>, IBinaryAdapter<ulong>, IBinaryAdapter<float>, IBinaryAdapter<double>, IBinaryAdapter<bool>, IBinaryAdapter<char>, IBinaryAdapter<string>, IBinaryAdapter<Guid>, IBinaryAdapter<DateTime>, IBinaryAdapter<TimeSpan>, IBinaryAdapter<Version>, IBinaryAdapter<DirectoryInfo>, IBinaryAdapter<FileInfo>, IContravariantBinaryAdapter<Object>
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x0000FC84 File Offset: 0x0000DE84
		unsafe void IBinaryAdapter<sbyte>.Serialize(in BinarySerializationContext<sbyte> context, sbyte value)
		{
			context.Writer->Add<sbyte>(value);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000FC92 File Offset: 0x0000DE92
		unsafe void IBinaryAdapter<short>.Serialize(in BinarySerializationContext<short> context, short value)
		{
			context.Writer->Add<short>(value);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
		unsafe void IBinaryAdapter<int>.Serialize(in BinarySerializationContext<int> context, int value)
		{
			context.Writer->Add<int>(value);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000FCAE File Offset: 0x0000DEAE
		unsafe void IBinaryAdapter<long>.Serialize(in BinarySerializationContext<long> context, long value)
		{
			context.Writer->Add<long>(value);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000FCBC File Offset: 0x0000DEBC
		unsafe void IBinaryAdapter<byte>.Serialize(in BinarySerializationContext<byte> context, byte value)
		{
			context.Writer->Add<byte>(value);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000FCCA File Offset: 0x0000DECA
		unsafe void IBinaryAdapter<ushort>.Serialize(in BinarySerializationContext<ushort> context, ushort value)
		{
			context.Writer->Add<ushort>(value);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		unsafe void IBinaryAdapter<uint>.Serialize(in BinarySerializationContext<uint> context, uint value)
		{
			context.Writer->Add<uint>(value);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000FCE6 File Offset: 0x0000DEE6
		unsafe void IBinaryAdapter<ulong>.Serialize(in BinarySerializationContext<ulong> context, ulong value)
		{
			context.Writer->Add<ulong>(value);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		unsafe void IBinaryAdapter<float>.Serialize(in BinarySerializationContext<float> context, float value)
		{
			context.Writer->Add<float>(value);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000FD02 File Offset: 0x0000DF02
		unsafe void IBinaryAdapter<double>.Serialize(in BinarySerializationContext<double> context, double value)
		{
			context.Writer->Add<double>(value);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000FD10 File Offset: 0x0000DF10
		unsafe void IBinaryAdapter<bool>.Serialize(in BinarySerializationContext<bool> context, bool value)
		{
			context.Writer->Add<byte>(value ? 1 : 0);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000FD25 File Offset: 0x0000DF25
		unsafe void IBinaryAdapter<char>.Serialize(in BinarySerializationContext<char> context, char value)
		{
			context.Writer->Add<char>(value);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000FD33 File Offset: 0x0000DF33
		unsafe void IBinaryAdapter<string>.Serialize(in BinarySerializationContext<string> context, string value)
		{
			(ref *context.Writer).AddNBC(value);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000FD41 File Offset: 0x0000DF41
		unsafe sbyte IBinaryAdapter<sbyte>.Deserialize(in BinaryDeserializationContext<sbyte> context)
		{
			return context.Reader->ReadNext<sbyte>();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000FD4E File Offset: 0x0000DF4E
		unsafe short IBinaryAdapter<short>.Deserialize(in BinaryDeserializationContext<short> context)
		{
			return context.Reader->ReadNext<short>();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000FD5B File Offset: 0x0000DF5B
		unsafe int IBinaryAdapter<int>.Deserialize(in BinaryDeserializationContext<int> context)
		{
			return context.Reader->ReadNext<int>();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000FD68 File Offset: 0x0000DF68
		unsafe long IBinaryAdapter<long>.Deserialize(in BinaryDeserializationContext<long> context)
		{
			return context.Reader->ReadNext<long>();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000FD75 File Offset: 0x0000DF75
		unsafe byte IBinaryAdapter<byte>.Deserialize(in BinaryDeserializationContext<byte> context)
		{
			return context.Reader->ReadNext<byte>();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000FD82 File Offset: 0x0000DF82
		unsafe ushort IBinaryAdapter<ushort>.Deserialize(in BinaryDeserializationContext<ushort> context)
		{
			return context.Reader->ReadNext<ushort>();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000FD8F File Offset: 0x0000DF8F
		unsafe uint IBinaryAdapter<uint>.Deserialize(in BinaryDeserializationContext<uint> context)
		{
			return context.Reader->ReadNext<uint>();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000FD9C File Offset: 0x0000DF9C
		unsafe ulong IBinaryAdapter<ulong>.Deserialize(in BinaryDeserializationContext<ulong> context)
		{
			return context.Reader->ReadNext<ulong>();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000FDA9 File Offset: 0x0000DFA9
		unsafe float IBinaryAdapter<float>.Deserialize(in BinaryDeserializationContext<float> context)
		{
			return context.Reader->ReadNext<float>();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000FDB6 File Offset: 0x0000DFB6
		unsafe double IBinaryAdapter<double>.Deserialize(in BinaryDeserializationContext<double> context)
		{
			return context.Reader->ReadNext<double>();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000FDC3 File Offset: 0x0000DFC3
		unsafe bool IBinaryAdapter<bool>.Deserialize(in BinaryDeserializationContext<bool> context)
		{
			return context.Reader->ReadNext<byte>() == 1;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000FDD3 File Offset: 0x0000DFD3
		unsafe char IBinaryAdapter<char>.Deserialize(in BinaryDeserializationContext<char> context)
		{
			return context.Reader->ReadNext<char>();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		unsafe string IBinaryAdapter<string>.Deserialize(in BinaryDeserializationContext<string> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			return text;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000FDFB File Offset: 0x0000DFFB
		unsafe void IBinaryAdapter<Guid>.Serialize(in BinarySerializationContext<Guid> context, Guid value)
		{
			(ref *context.Writer).AddNBC(value.ToString("N", CultureInfo.InvariantCulture));
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000FE1C File Offset: 0x0000E01C
		unsafe Guid IBinaryAdapter<Guid>.Deserialize(in BinaryDeserializationContext<Guid> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			Guid guid;
			if (!Guid.TryParseExact(text, "N", out guid))
			{
				return default(Guid);
			}
			return guid;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000FE50 File Offset: 0x0000E050
		unsafe void IBinaryAdapter<DateTime>.Serialize(in BinarySerializationContext<DateTime> context, DateTime value)
		{
			(ref *context.Writer).AddNBC(value.ToString("o", CultureInfo.InvariantCulture));
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000FE70 File Offset: 0x0000E070
		unsafe DateTime IBinaryAdapter<DateTime>.Deserialize(in BinaryDeserializationContext<DateTime> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			DateTime dateTime;
			if (!DateTime.TryParseExact(text, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				return default(DateTime);
			}
			return dateTime;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000FEAE File Offset: 0x0000E0AE
		unsafe void IBinaryAdapter<TimeSpan>.Serialize(in BinarySerializationContext<TimeSpan> context, TimeSpan value)
		{
			(ref *context.Writer).AddNBC(value.ToString("c", CultureInfo.InvariantCulture));
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000FECC File Offset: 0x0000E0CC
		unsafe TimeSpan IBinaryAdapter<TimeSpan>.Deserialize(in BinaryDeserializationContext<TimeSpan> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			TimeSpan timeSpan;
			if (!TimeSpan.TryParseExact(text, "c", CultureInfo.InvariantCulture, out timeSpan))
			{
				return default(TimeSpan);
			}
			return timeSpan;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000FF05 File Offset: 0x0000E105
		unsafe void IBinaryAdapter<Version>.Serialize(in BinarySerializationContext<Version> context, Version value)
		{
			(ref *context.Writer).AddNBC(value.ToString());
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000FF18 File Offset: 0x0000E118
		unsafe Version IBinaryAdapter<Version>.Deserialize(in BinaryDeserializationContext<Version> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			return new Version(text);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000FF38 File Offset: 0x0000E138
		unsafe void IBinaryAdapter<DirectoryInfo>.Serialize(in BinarySerializationContext<DirectoryInfo> context, DirectoryInfo value)
		{
			if (value == null)
			{
				(ref *context.Writer).AddNBC("null");
				return;
			}
			(ref *context.Writer).AddNBC(value.GetRelativePath());
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000FF60 File Offset: 0x0000E160
		unsafe DirectoryInfo IBinaryAdapter<DirectoryInfo>.Deserialize(in BinaryDeserializationContext<DirectoryInfo> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			if (!text.Equals("null"))
			{
				return new DirectoryInfo(text);
			}
			return null;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000FF8F File Offset: 0x0000E18F
		unsafe void IBinaryAdapter<FileInfo>.Serialize(in BinarySerializationContext<FileInfo> context, FileInfo value)
		{
			if (value == null)
			{
				(ref *context.Writer).AddNBC("null");
				return;
			}
			(ref *context.Writer).AddNBC(value.GetRelativePath());
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		unsafe FileInfo IBinaryAdapter<FileInfo>.Deserialize(in BinaryDeserializationContext<FileInfo> context)
		{
			string text;
			(ref *context.Reader).ReadNextNBC(out text);
			if (!text.Equals("null"))
			{
				return new FileInfo(text);
			}
			return null;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000578E File Offset: 0x0000398E
		void IContravariantBinaryAdapter<Object>.Serialize(IBinarySerializationContext context, Object value)
		{
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00002E2F File Offset: 0x0000102F
		object IContravariantBinaryAdapter<Object>.Deserialize(IBinaryDeserializationContext context)
		{
			return null;
		}
	}
}
