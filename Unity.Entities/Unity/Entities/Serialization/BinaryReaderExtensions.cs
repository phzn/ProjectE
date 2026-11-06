using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036A RID: 874
	public static class BinaryReaderExtensions
	{
		// Token: 0x06001771 RID: 6001 RVA: 0x000582E8 File Offset: 0x000564E8
		public unsafe static byte ReadByte(this BinaryReader reader)
		{
			byte b;
			reader.ReadBytes((void*)(&b), 1);
			return b;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00058300 File Offset: 0x00056500
		public unsafe static int ReadInt(this BinaryReader reader)
		{
			int num;
			reader.ReadBytes((void*)(&num), 4);
			return num;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00058318 File Offset: 0x00056518
		public unsafe static ulong ReadULong(this BinaryReader reader)
		{
			ulong num;
			reader.ReadBytes((void*)(&num), 8);
			return num;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00058330 File Offset: 0x00056530
		public unsafe static void ReadBytes(this BinaryReader reader, NativeArray<byte> elements, int count, int offset = 0)
		{
			byte* ptr = (byte*)elements.GetUnsafePtr<byte>() + offset;
			reader.ReadBytes((void*)ptr, count);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0005834E File Offset: 0x0005654E
		public static void ReadArray<T>(this BinaryReader reader, NativeArray<T> elements, int count) where T : struct
		{
			reader.ReadBytes(elements.GetUnsafeReadOnlyPtr<T>(), count * UnsafeUtility.SizeOf<T>());
		}
	}
}
