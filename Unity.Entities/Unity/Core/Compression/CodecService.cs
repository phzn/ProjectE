using System;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Core.Compression
{
	// Token: 0x020004D3 RID: 1235
	internal static class CodecService
	{
		// Token: 0x06001DD5 RID: 7637 RVA: 0x00067DC4 File Offset: 0x00065FC4
		public static int CompressUpperBound(Codec codec, int size)
		{
			if (codec == Codec.None)
			{
				return size;
			}
			if (codec != Codec.LZ4)
			{
				throw new ArgumentException(string.Format("Invalid codec '{0}' specified", codec));
			}
			return CodecService.CompressBoundLZ4(size);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00067DED File Offset: 0x00065FED
		public unsafe static int Compress(Codec codec, in byte* src, int srcSize, out byte* dst, Allocator allocator = Allocator.Temp)
		{
			return CodecService.Compress(codec, in src, srcSize, out dst, allocator);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x00067E00 File Offset: 0x00066000
		public unsafe static int Compress(Codec codec, in byte* src, int srcSize, out byte* dst, AllocatorManager.AllocatorHandle allocator)
		{
			int num = CodecService.CompressUpperBound(codec, srcSize);
			dst = Memory.Unmanaged.Allocate((long)num, 16, allocator);
			if (codec != Codec.None && codec == Codec.LZ4)
			{
				int num2 = CodecService.CompressLZ4(src, dst, srcSize, num);
				if (num2 < 0)
				{
					Memory.Unmanaged.Free<byte>(dst, allocator);
					dst = (IntPtr)((UIntPtr)0);
				}
				return num2;
			}
			throw new ArgumentException(string.Format("Invalid codec '{0}' specified", codec));
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00067E61 File Offset: 0x00066061
		public unsafe static bool Decompress(Codec codec, in byte* compressedData, int compressedSize, byte* decompressedData, int decompressedSize)
		{
			if (codec != Codec.None && codec == Codec.LZ4)
			{
				return CodecService.DecompressLZ4(compressedData, decompressedData, compressedSize, decompressedSize) > 0;
			}
			throw new ArgumentException(string.Format("Invalid codec '{0}' specified", codec));
		}

		// Token: 0x06001DD9 RID: 7641
		[DllImport("liblz4", EntryPoint = "LZ4_compressBound")]
		private static extern int CompressBoundLZ4(int srcSize);

		// Token: 0x06001DDA RID: 7642
		[DllImport("liblz4", EntryPoint = "LZ4_compress_default")]
		private unsafe static extern int CompressLZ4(byte* src, byte* dst, int srcSize, int dstCapacity);

		// Token: 0x06001DDB RID: 7643
		[DllImport("liblz4", EntryPoint = "LZ4_decompress_safe")]
		private unsafe static extern int DecompressLZ4(byte* src, byte* dst, int compressedSize, int dstCapacity);

		// Token: 0x04000AB1 RID: 2737
		private const string DllName = "liblz4";
	}
}
