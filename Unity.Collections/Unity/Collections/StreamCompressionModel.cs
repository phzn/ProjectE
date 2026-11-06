using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x020000DD RID: 221
	[GenerateTestsForBurstCompatibility]
	public struct StreamCompressionModel
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0001DB3C File Offset: 0x0001BD3C
		public unsafe static StreamCompressionModel Default
		{
			get
			{
				if (StreamCompressionModel.SharedStaticCompressionModel.Default.Data.m_Initialized == 1)
				{
					return *StreamCompressionModel.SharedStaticCompressionModel.Default.Data;
				}
				StreamCompressionModel.Initialize();
				StreamCompressionModel.SharedStaticCompressionModel.Default.Data.m_Initialized = 1;
				return *StreamCompressionModel.SharedStaticCompressionModel.Default.Data;
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001DB90 File Offset: 0x0001BD90
		private unsafe static void Initialize()
		{
			for (int i = 0; i < 16; i++)
			{
				*((ref StreamCompressionModel.SharedStaticCompressionModel.Default.Data.bucketSizes.FixedElementField) + i) = StreamCompressionModel.k_BucketSizes[i];
				*((ref StreamCompressionModel.SharedStaticCompressionModel.Default.Data.bucketOffsets.FixedElementField) + (IntPtr)i * 4) = StreamCompressionModel.k_BucketOffsets[i];
			}
			NativeArray<byte> nativeArray = new NativeArray<byte>(StreamCompressionModel.k_DefaultModelData.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < StreamCompressionModel.k_DefaultModelData.Length; j++)
			{
				nativeArray[j] = StreamCompressionModel.k_DefaultModelData[j];
			}
			int num = 1;
			NativeArray<byte> nativeArray2 = new NativeArray<byte>(num * 16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num2 = 0;
			byte b = nativeArray[num2++];
			for (int k = 0; k < 16; k++)
			{
				byte b2 = nativeArray[num2++];
				for (int l = 0; l < num; l++)
				{
					nativeArray2[num * l + k] = b2;
				}
			}
			int num3 = (int)nativeArray[num2] | ((int)nativeArray[num2 + 1] << 8);
			num2 += 2;
			for (int m = 0; m < num3; m++)
			{
				int num4 = (int)nativeArray[num2] | ((int)nativeArray[num2 + 1] << 8);
				num2 += 2;
				byte b3 = nativeArray[num2++];
				for (int n = 0; n < 16; n++)
				{
					byte b4 = nativeArray[num2++];
					nativeArray2[num * num4 + n] = b4;
				}
			}
			NativeArray<byte> nativeArray3 = new NativeArray<byte>(16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<ushort> nativeArray4 = new NativeArray<ushort>(64, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<byte> nativeArray5 = new NativeArray<byte>(16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int num5 = 0; num5 < num; num5++)
			{
				for (int num6 = 0; num6 < 16; num6++)
				{
					nativeArray3[num6] = nativeArray2[num * num5 + num6];
				}
				StreamCompressionModel.GenerateHuffmanCodes(nativeArray5, 0, nativeArray3, 0, 16, 6);
				StreamCompressionModel.GenerateHuffmanDecodeTable(nativeArray4, 0, nativeArray3, nativeArray5, 16, 6);
				for (int num7 = 0; num7 < 16; num7++)
				{
					*((ref StreamCompressionModel.SharedStaticCompressionModel.Default.Data.encodeTable.FixedElementField) + (IntPtr)(num5 * 16 + num7) * 2) = (ushort)(((int)nativeArray5[num7] << 8) | (int)nativeArray2[num * num5 + num7]);
				}
				for (int num8 = 0; num8 < 64; num8++)
				{
					*((ref StreamCompressionModel.SharedStaticCompressionModel.Default.Data.decodeTable.FixedElementField) + (IntPtr)(num5 * 64 + num8) * 2) = nativeArray4[num8];
				}
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001DE0C File Offset: 0x0001C00C
		private static void GenerateHuffmanCodes(NativeArray<byte> symbolCodes, int symbolCodesOffset, NativeArray<byte> symbolLengths, int symbolLengthsOffset, int alphabetSize, int maxCodeLength)
		{
			NativeArray<byte> nativeArray = new NativeArray<byte>(maxCodeLength + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<byte> nativeArray2 = new NativeArray<byte>((maxCodeLength + 1) * alphabetSize, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < alphabetSize; i++)
			{
				int num = (int)symbolLengths[i + symbolLengthsOffset];
				int num2 = (maxCodeLength + 1) * num;
				int num3 = num;
				byte b = nativeArray[num3];
				nativeArray[num3] = b + 1;
				nativeArray2[num2 + (int)b] = (byte)i;
			}
			uint num4 = 0U;
			for (int j = 1; j <= maxCodeLength; j++)
			{
				int num5 = (int)nativeArray[j];
				for (int k = 0; k < num5; k++)
				{
					int num6 = (int)nativeArray2[(maxCodeLength + 1) * j + k];
					symbolCodes[num6 + symbolCodesOffset] = (byte)StreamCompressionModel.ReverseBits(num4++, j);
				}
				num4 <<= 1;
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001DEDC File Offset: 0x0001C0DC
		private static uint ReverseBits(uint value, int num_bits)
		{
			value = ((value & 1431655765U) << 1) | ((value & 2863311530U) >> 1);
			value = ((value & 858993459U) << 2) | ((value & 3435973836U) >> 2);
			value = ((value & 252645135U) << 4) | ((value & 4042322160U) >> 4);
			value = ((value & 16711935U) << 8) | ((value & 4278255360U) >> 8);
			value = (value << 16) | (value >> 16);
			return value >> 32 - num_bits;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001DF54 File Offset: 0x0001C154
		private static void GenerateHuffmanDecodeTable(NativeArray<ushort> decodeTable, int decodeTableOffset, NativeArray<byte> symbolLengths, NativeArray<byte> symbolCodes, int alphabetSize, int maxCodeLength)
		{
			uint num = 1U << maxCodeLength;
			for (int i = 0; i < alphabetSize; i++)
			{
				int num2 = (int)symbolLengths[i];
				if (num2 > 0)
				{
					uint num3 = (uint)symbolCodes[i];
					uint num4 = 1U << num2;
					do
					{
						decodeTable[(int)((long)decodeTableOffset + (long)((ulong)num3))] = (ushort)((i << 8) | num2);
						num3 += num4;
					}
					while (num3 < num);
				}
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001DFB0 File Offset: 0x0001C1B0
		public unsafe readonly int CalculateBucket(uint value)
		{
			int num = StreamCompressionModel.k_FirstBucketCandidate[math.lzcnt(value)];
			if (num + 1 < 16 && value >= *((ref this.bucketOffsets.FixedElementField) + (IntPtr)(num + 1) * 4))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001DFEC File Offset: 0x0001C1EC
		public unsafe readonly int GetCompressedSizeInBits(uint value)
		{
			int num = this.CalculateBucket(value);
			int num2 = (int)(*((ref this.bucketSizes.FixedElementField) + num));
			return (int)(*((ref this.encodeTable.FixedElementField) + (IntPtr)num * 2) & 255) + num2;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001E02C File Offset: 0x0001C22C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckAlphabetSize(int alphabetSize)
		{
			if (alphabetSize != 16)
			{
				throw new InvalidOperationException("The alphabet size of compression models must be " + 16.ToString());
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001E058 File Offset: 0x0001C258
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckSymbolLength(NativeArray<byte> symbolLengths, int symbolLengthsOffset, int symbol, int length)
		{
			if ((int)symbolLengths[symbol + symbolLengthsOffset] != length)
			{
				throw new InvalidOperationException("Incorrect symbol length");
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001E072 File Offset: 0x0001C272
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckAlphabetAndMaxCodeLength(int alphabetSize, int maxCodeLength)
		{
			if (alphabetSize > 256 || maxCodeLength > 8)
			{
				throw new InvalidOperationException("Can only generate huffman codes up to alphabet size 256 and maximum code length 8");
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001E08B File Offset: 0x0001C28B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckExceedMaxCodeLength(int length, int maxCodeLength)
		{
			if (length > maxCodeLength)
			{
				throw new InvalidOperationException("Maximum code length exceeded");
			}
		}

		// Token: 0x04000433 RID: 1075
		internal static readonly byte[] k_BucketSizes = new byte[]
		{
			0, 0, 1, 2, 3, 4, 6, 8, 10, 12,
			15, 18, 21, 24, 27, 32
		};

		// Token: 0x04000434 RID: 1076
		internal static readonly uint[] k_BucketOffsets = new uint[]
		{
			0U, 1U, 2U, 4U, 8U, 16U, 32U, 96U, 352U, 1376U,
			5472U, 38240U, 300384U, 2397536U, 19174752U, 153392480U
		};

		// Token: 0x04000435 RID: 1077
		internal static readonly int[] k_FirstBucketCandidate = new int[]
		{
			15, 15, 15, 15, 14, 14, 14, 13, 13, 13,
			12, 12, 12, 11, 11, 11, 10, 10, 10, 9,
			9, 8, 8, 7, 7, 6, 5, 4, 3, 2,
			1, 1, 0
		};

		// Token: 0x04000436 RID: 1078
		internal static readonly byte[] k_DefaultModelData = new byte[]
		{
			16, 2, 3, 3, 3, 4, 4, 4, 5, 5,
			5, 6, 6, 6, 6, 6, 6, 0, 0
		};

		// Token: 0x04000437 RID: 1079
		internal const int k_AlphabetSize = 16;

		// Token: 0x04000438 RID: 1080
		internal const int k_MaxHuffmanSymbolLength = 6;

		// Token: 0x04000439 RID: 1081
		internal const int k_MaxContexts = 1;

		// Token: 0x0400043A RID: 1082
		private byte m_Initialized;

		// Token: 0x0400043B RID: 1083
		[FixedBuffer(typeof(ushort), 16)]
		internal StreamCompressionModel.<encodeTable>e__FixedBuffer encodeTable;

		// Token: 0x0400043C RID: 1084
		[FixedBuffer(typeof(ushort), 64)]
		internal StreamCompressionModel.<decodeTable>e__FixedBuffer decodeTable;

		// Token: 0x0400043D RID: 1085
		[FixedBuffer(typeof(byte), 16)]
		internal StreamCompressionModel.<bucketSizes>e__FixedBuffer bucketSizes;

		// Token: 0x0400043E RID: 1086
		[FixedBuffer(typeof(uint), 16)]
		internal StreamCompressionModel.<bucketOffsets>e__FixedBuffer bucketOffsets;

		// Token: 0x020000DE RID: 222
		private static class SharedStaticCompressionModel
		{
			// Token: 0x0400043F RID: 1087
			internal static readonly SharedStatic<StreamCompressionModel> Default = SharedStatic<StreamCompressionModel>.GetOrCreateUnsafe(0U, 6564095697914452312L, 0L);
		}

		// Token: 0x020000DF RID: 223
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 64)]
		public struct <bucketOffsets>e__FixedBuffer
		{
			// Token: 0x04000440 RID: 1088
			public uint FixedElementField;
		}

		// Token: 0x020000E0 RID: 224
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct <bucketSizes>e__FixedBuffer
		{
			// Token: 0x04000441 RID: 1089
			public byte FixedElementField;
		}

		// Token: 0x020000E1 RID: 225
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <decodeTable>e__FixedBuffer
		{
			// Token: 0x04000442 RID: 1090
			public ushort FixedElementField;
		}

		// Token: 0x020000E2 RID: 226
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <encodeTable>e__FixedBuffer
		{
			// Token: 0x04000443 RID: 1091
			public ushort FixedElementField;
		}
	}
}
