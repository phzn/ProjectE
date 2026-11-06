using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x02000691 RID: 1681
	internal static class IOUtil
	{
		// Token: 0x06003E09 RID: 15881 RVA: 0x000D60B5 File Offset: 0x000D42B5
		internal static bool FlagTest(MessageEnum flag, MessageEnum target)
		{
			return (flag & target) == target;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x000D60C0 File Offset: 0x000D42C0
		internal static void WriteStringWithCode(string value, __BinaryWriter sout)
		{
			if (value == null)
			{
				sout.WriteByte(17);
				return;
			}
			sout.WriteByte(18);
			sout.WriteString(value);
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x000D60E0 File Offset: 0x000D42E0
		internal static void WriteWithCode(Type type, object value, __BinaryWriter sout)
		{
			if (type == null)
			{
				sout.WriteByte(17);
				return;
			}
			if (type == Converter.typeofString)
			{
				IOUtil.WriteStringWithCode((string)value, sout);
				return;
			}
			InternalPrimitiveTypeE internalPrimitiveTypeE = Converter.ToCode(type);
			sout.WriteByte((byte)internalPrimitiveTypeE);
			sout.WriteValue(internalPrimitiveTypeE, value);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x000D6128 File Offset: 0x000D4328
		internal static object ReadWithCode(__BinaryParser input)
		{
			InternalPrimitiveTypeE internalPrimitiveTypeE = (InternalPrimitiveTypeE)input.ReadByte();
			if (internalPrimitiveTypeE == InternalPrimitiveTypeE.Null)
			{
				return null;
			}
			if (internalPrimitiveTypeE == InternalPrimitiveTypeE.String)
			{
				return input.ReadString();
			}
			return input.ReadValue(internalPrimitiveTypeE);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x000D6158 File Offset: 0x000D4358
		internal static object[] ReadArgs(__BinaryParser input)
		{
			int num = input.ReadInt32();
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = IOUtil.ReadWithCode(input);
			}
			return array;
		}
	}
}
