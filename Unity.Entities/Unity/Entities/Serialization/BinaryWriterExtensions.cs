using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000368 RID: 872
	public static class BinaryWriterExtensions
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x0005821A File Offset: 0x0005641A
		public unsafe static void Write(this BinaryWriter writer, byte value)
		{
			writer.WriteBytes((void*)(&value), 1);
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00058226 File Offset: 0x00056426
		public unsafe static void Write(this BinaryWriter writer, int value)
		{
			writer.WriteBytes((void*)(&value), 4);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00058232 File Offset: 0x00056432
		public unsafe static void Write(this BinaryWriter writer, ulong value)
		{
			writer.WriteBytes((void*)(&value), 8);
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00058240 File Offset: 0x00056440
		public unsafe static void Write(this BinaryWriter writer, byte[] bytes)
		{
			fixed (byte[] array = bytes)
			{
				byte* ptr;
				if (bytes == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				writer.WriteBytes((void*)ptr, bytes.Length);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00058271 File Offset: 0x00056471
		public static void WriteArray<T>(this BinaryWriter writer, NativeArray<T> data) where T : struct
		{
			writer.WriteBytes(data.GetUnsafeReadOnlyPtr<T>(), data.Length * UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0005828C File Offset: 0x0005648C
		public unsafe static void WriteList<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this BinaryWriter writer, NativeList<T> data) where T : struct, ValueType
		{
			writer.WriteBytes((void*)data.GetUnsafePtr<T>(), data.Length * UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000582A8 File Offset: 0x000564A8
		public unsafe static void WriteList<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this BinaryWriter writer, NativeList<T> data, int index, int count) where T : struct, ValueType
		{
			if (index + count > data.Length)
			{
				throw new ArgumentException("index + count must not go beyond the end of the list");
			}
			int num = UnsafeUtility.SizeOf<T>();
			writer.WriteBytes((void*)(data.GetUnsafePtr<T>() + num * index / sizeof(T)), count * num);
		}
	}
}
