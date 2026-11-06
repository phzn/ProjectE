using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x020000FE RID: 254
	[GenerateTestsForBurstCompatibility]
	public static class DataStreamExtensions
	{
		// Token: 0x06000AE3 RID: 2787 RVA: 0x000212CF File Offset: 0x0001F4CF
		public unsafe static DataStreamWriter Create(byte* data, int length)
		{
			return new DataStreamWriter(NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, length, Allocator.None));
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000212E0 File Offset: 0x0001F4E0
		public unsafe static bool WriteBytesUnsafe(this DataStreamWriter writer, byte* data, int bytes)
		{
			NativeArray<byte> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, bytes, Allocator.None);
			return writer.WriteBytes(nativeArray);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00021300 File Offset: 0x0001F500
		public unsafe static void ReadBytesUnsafe(this DataStreamReader reader, byte* data, int length)
		{
			NativeArray<byte> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, length, Allocator.None);
			reader.ReadBytes(nativeArray);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00021320 File Offset: 0x0001F520
		public unsafe static ushort ReadFixedStringUnsafe(this DataStreamReader reader, byte* data, int maxLength)
		{
			NativeArray<byte> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, maxLength, Allocator.Temp);
			return reader.ReadFixedString(nativeArray);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00021340 File Offset: 0x0001F540
		public unsafe static ushort ReadPackedFixedStringDeltaUnsafe(this DataStreamReader reader, byte* data, int maxLength, byte* baseData, ushort baseLength, StreamCompressionModel model)
		{
			NativeArray<byte> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, maxLength, Allocator.Temp);
			NativeArray<byte> nativeArray2 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)baseData, (int)baseLength, Allocator.Temp);
			return reader.ReadPackedFixedStringDelta(nativeArray, nativeArray2, in model);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002136A File Offset: 0x0001F56A
		public unsafe static void* GetUnsafeReadOnlyPtr(this DataStreamReader reader)
		{
			return (void*)reader.m_BufferPtr;
		}
	}
}
