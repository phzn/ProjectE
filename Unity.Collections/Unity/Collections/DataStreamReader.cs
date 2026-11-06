using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting.APIUpdating;

namespace Unity.Collections
{
	// Token: 0x02000045 RID: 69
	[MovedFrom(true, "Unity.Networking.Transport", null, null)]
	[GenerateTestsForBurstCompatibility]
	public struct DataStreamReader
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00005546 File Offset: 0x00003746
		public DataStreamReader(NativeArray<byte> array)
		{
			DataStreamReader.Initialize(out this, array);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000554F File Offset: 0x0000374F
		private unsafe static void Initialize(out DataStreamReader self, NativeArray<byte> array)
		{
			self.m_BufferPtr = (byte*)array.GetUnsafeReadOnlyPtr<byte>();
			self.m_Length = array.Length;
			self.m_Context = default(DataStreamReader.Context);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00005576 File Offset: 0x00003776
		public static bool IsLittleEndian
		{
			get
			{
				return DataStreamWriter.IsLittleEndian;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000557D File Offset: 0x0000377D
		private static short ByteSwap(short val)
		{
			return (short)(((int)(val & 255) << 8) | ((val >> 8) & 255));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005593 File Offset: 0x00003793
		private static int ByteSwap(int val)
		{
			return ((val & 255) << 24) | ((val & 65280) << 8) | ((val >> 8) & 65280) | ((val >> 24) & 255);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000055BE File Offset: 0x000037BE
		public readonly bool HasFailedReads
		{
			get
			{
				return this.m_Context.m_FailedReads > 0;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000055CE File Offset: 0x000037CE
		public readonly int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000055D6 File Offset: 0x000037D6
		public readonly bool IsCreated
		{
			get
			{
				return this.m_BufferPtr != null;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000055E8 File Offset: 0x000037E8
		private unsafe void ReadBytesInternal(byte* data, int length)
		{
			if (this.GetBytesRead() + length > this.m_Length)
			{
				this.m_Context.m_FailedReads = this.m_Context.m_FailedReads + 1;
				UnsafeUtility.MemClear((void*)data, (long)length);
				return;
			}
			this.Flush();
			UnsafeUtility.MemCpy((void*)data, (void*)(this.m_BufferPtr + this.m_Context.m_ReadByteIndex), (long)length);
			this.m_Context.m_ReadByteIndex = this.m_Context.m_ReadByteIndex + length;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000564E File Offset: 0x0000384E
		public unsafe void ReadBytes(NativeArray<byte> array)
		{
			this.ReadBytesInternal((byte*)array.GetUnsafePtr<byte>(), array.Length);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005664 File Offset: 0x00003864
		public unsafe void ReadBytes(Span<byte> span)
		{
			fixed (byte* pinnableReference = span.GetPinnableReference())
			{
				byte* ptr = pinnableReference;
				this.ReadBytesInternal(ptr, span.Length);
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000568D File Offset: 0x0000388D
		public int GetBytesRead()
		{
			return this.m_Context.m_ReadByteIndex - (this.m_Context.m_BitIndex >> 3);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000056A8 File Offset: 0x000038A8
		public int GetBitsRead()
		{
			return (this.m_Context.m_ReadByteIndex << 3) - this.m_Context.m_BitIndex;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000056C4 File Offset: 0x000038C4
		public void SeekSet(int pos)
		{
			if (pos > this.m_Length)
			{
				this.m_Context.m_FailedReads = this.m_Context.m_FailedReads + 1;
				return;
			}
			this.m_Context.m_ReadByteIndex = pos;
			this.m_Context.m_BitIndex = 0;
			this.m_Context.m_BitBuffer = 0UL;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005710 File Offset: 0x00003910
		public unsafe byte ReadByte()
		{
			byte b;
			this.ReadBytesInternal(&b, 1);
			return b;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00005728 File Offset: 0x00003928
		public unsafe short ReadShort()
		{
			short num;
			this.ReadBytesInternal((byte*)(&num), 2);
			return num;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00005740 File Offset: 0x00003940
		public unsafe ushort ReadUShort()
		{
			ushort num;
			this.ReadBytesInternal((byte*)(&num), 2);
			return num;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005758 File Offset: 0x00003958
		public unsafe int ReadInt()
		{
			int num;
			this.ReadBytesInternal((byte*)(&num), 4);
			return num;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005770 File Offset: 0x00003970
		public unsafe uint ReadUInt()
		{
			uint num;
			this.ReadBytesInternal((byte*)(&num), 4);
			return num;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00005788 File Offset: 0x00003988
		public unsafe long ReadLong()
		{
			long num;
			this.ReadBytesInternal((byte*)(&num), 8);
			return num;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000057A0 File Offset: 0x000039A0
		public unsafe ulong ReadULong()
		{
			ulong num;
			this.ReadBytesInternal((byte*)(&num), 8);
			return num;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000057B8 File Offset: 0x000039B8
		public void Flush()
		{
			this.m_Context.m_ReadByteIndex = this.m_Context.m_ReadByteIndex - (this.m_Context.m_BitIndex >> 3);
			this.m_Context.m_BitIndex = 0;
			this.m_Context.m_BitBuffer = 0UL;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000057F0 File Offset: 0x000039F0
		public unsafe short ReadShortNetworkByteOrder()
		{
			short num;
			this.ReadBytesInternal((byte*)(&num), 2);
			if (!DataStreamReader.IsLittleEndian)
			{
				return num;
			}
			return DataStreamReader.ByteSwap(num);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005816 File Offset: 0x00003A16
		public ushort ReadUShortNetworkByteOrder()
		{
			return (ushort)this.ReadShortNetworkByteOrder();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00005820 File Offset: 0x00003A20
		public unsafe int ReadIntNetworkByteOrder()
		{
			int num;
			this.ReadBytesInternal((byte*)(&num), 4);
			if (!DataStreamReader.IsLittleEndian)
			{
				return num;
			}
			return DataStreamReader.ByteSwap(num);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005846 File Offset: 0x00003A46
		public uint ReadUIntNetworkByteOrder()
		{
			return (uint)this.ReadIntNetworkByteOrder();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00005850 File Offset: 0x00003A50
		public float ReadFloat()
		{
			return new UIntFloat
			{
				intValue = (uint)this.ReadInt()
			}.floatValue;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005878 File Offset: 0x00003A78
		public double ReadDouble()
		{
			return new UIntFloat
			{
				longValue = (ulong)this.ReadLong()
			}.doubleValue;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000058A0 File Offset: 0x00003AA0
		public uint ReadPackedUInt(in StreamCompressionModel model)
		{
			return this.ReadPackedUIntInternal(6, in model);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000058AC File Offset: 0x00003AAC
		private unsafe uint ReadPackedUIntInternal(int maxSymbolLength, in StreamCompressionModel model)
		{
			this.FillBitBuffer();
			uint num = (1U << maxSymbolLength) - 1U;
			uint num2 = (uint)this.m_Context.m_BitBuffer & num;
			ushort num3 = *((ref model.decodeTable.FixedElementField) + (IntPtr)num2 * 2);
			int num4 = num3 >> 8;
			int num5 = (int)(num3 & 255);
			if (this.m_Context.m_BitIndex < num5)
			{
				this.m_Context.m_FailedReads = this.m_Context.m_FailedReads + 1;
				return 0U;
			}
			this.m_Context.m_BitBuffer = this.m_Context.m_BitBuffer >> num5;
			this.m_Context.m_BitIndex = this.m_Context.m_BitIndex - num5;
			uint num6 = *((ref model.bucketOffsets.FixedElementField) + (IntPtr)num4 * 4);
			byte b = *((ref model.bucketSizes.FixedElementField) + num4);
			return this.ReadRawBitsInternal((int)b) + num6;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00005964 File Offset: 0x00003B64
		private unsafe void FillBitBuffer()
		{
			while (this.m_Context.m_BitIndex <= 56 && this.m_Context.m_ReadByteIndex < this.m_Length)
			{
				ulong bitBuffer = this.m_Context.m_BitBuffer;
				int bufferPtr = this.m_BufferPtr;
				int readByteIndex = this.m_Context.m_ReadByteIndex;
				this.m_Context.m_ReadByteIndex = readByteIndex + 1;
				this.m_Context.m_BitBuffer = bitBuffer | ((ulong)(*(bufferPtr + readByteIndex)) << this.m_Context.m_BitIndex);
				this.m_Context.m_BitIndex = this.m_Context.m_BitIndex + 8;
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000059E0 File Offset: 0x00003BE0
		private uint ReadRawBitsInternal(int numbits)
		{
			if (this.m_Context.m_BitIndex < numbits)
			{
				this.m_Context.m_FailedReads = this.m_Context.m_FailedReads + 1;
				return 0U;
			}
			uint num = (uint)(this.m_Context.m_BitBuffer & ((1UL << numbits) - 1UL));
			this.m_Context.m_BitBuffer = this.m_Context.m_BitBuffer >> numbits;
			this.m_Context.m_BitIndex = this.m_Context.m_BitIndex - numbits;
			return num;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005A47 File Offset: 0x00003C47
		public uint ReadRawBits(int numbits)
		{
			this.FillBitBuffer();
			return this.ReadRawBitsInternal(numbits);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005A56 File Offset: 0x00003C56
		public ulong ReadPackedULong(in StreamCompressionModel model)
		{
			return (ulong)this.ReadPackedUInt(in model) | ((ulong)this.ReadPackedUInt(in model) << 32);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00005A6C File Offset: 0x00003C6C
		public int ReadPackedInt(in StreamCompressionModel model)
		{
			uint num = this.ReadPackedUInt(in model);
			return (int)((num >> 1) ^ -(int)(num & 1U));
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005A8C File Offset: 0x00003C8C
		public long ReadPackedLong(in StreamCompressionModel model)
		{
			ulong num = this.ReadPackedULong(in model);
			return (long)((num >> 1) ^ -(long)(num & 1UL));
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005AAA File Offset: 0x00003CAA
		public float ReadPackedFloat(in StreamCompressionModel model)
		{
			return this.ReadPackedFloatDelta(0f, in model);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005AB8 File Offset: 0x00003CB8
		public double ReadPackedDouble(in StreamCompressionModel model)
		{
			return this.ReadPackedDoubleDelta(0.0, in model);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00005ACC File Offset: 0x00003CCC
		public int ReadPackedIntDelta(int baseline, in StreamCompressionModel model)
		{
			int num = this.ReadPackedInt(in model);
			return baseline - num;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005AE4 File Offset: 0x00003CE4
		public uint ReadPackedUIntDelta(uint baseline, in StreamCompressionModel model)
		{
			uint num = (uint)this.ReadPackedInt(in model);
			return baseline - num;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005AFC File Offset: 0x00003CFC
		public long ReadPackedLongDelta(long baseline, in StreamCompressionModel model)
		{
			long num = this.ReadPackedLong(in model);
			return baseline - num;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005B14 File Offset: 0x00003D14
		public ulong ReadPackedULongDelta(ulong baseline, in StreamCompressionModel model)
		{
			ulong num = (ulong)this.ReadPackedLong(in model);
			return baseline - num;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005B2C File Offset: 0x00003D2C
		public float ReadPackedFloatDelta(float baseline, in StreamCompressionModel model)
		{
			this.FillBitBuffer();
			if (this.ReadRawBitsInternal(1) == 0U)
			{
				return baseline;
			}
			int num = 32;
			return new UIntFloat
			{
				intValue = this.ReadRawBitsInternal(num)
			}.floatValue;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005B6C File Offset: 0x00003D6C
		public unsafe double ReadPackedDoubleDelta(double baseline, in StreamCompressionModel model)
		{
			this.FillBitBuffer();
			if (this.ReadRawBitsInternal(1) == 0U)
			{
				return baseline;
			}
			int num = 32;
			UIntFloat uintFloat = default(UIntFloat);
			uint* ptr = (uint*)(&uintFloat.longValue);
			*ptr = this.ReadRawBitsInternal(num);
			this.FillBitBuffer();
			ptr[1] |= this.ReadRawBitsInternal(num);
			return uintFloat.doubleValue;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005BC4 File Offset: 0x00003DC4
		public unsafe FixedString32Bytes ReadFixedString32()
		{
			FixedString32Bytes fixedString32Bytes;
			byte* ptr = (byte*)(&fixedString32Bytes) + 2;
			*(short*)(&fixedString32Bytes) = (short)this.ReadFixedStringInternal(ptr, fixedString32Bytes.Capacity);
			return fixedString32Bytes;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00005BEC File Offset: 0x00003DEC
		public unsafe FixedString64Bytes ReadFixedString64()
		{
			FixedString64Bytes fixedString64Bytes;
			byte* ptr = (byte*)(&fixedString64Bytes) + 2;
			*(short*)(&fixedString64Bytes) = (short)this.ReadFixedStringInternal(ptr, fixedString64Bytes.Capacity);
			return fixedString64Bytes;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005C14 File Offset: 0x00003E14
		public unsafe FixedString128Bytes ReadFixedString128()
		{
			FixedString128Bytes fixedString128Bytes;
			byte* ptr = (byte*)(&fixedString128Bytes) + 2;
			*(short*)(&fixedString128Bytes) = (short)this.ReadFixedStringInternal(ptr, fixedString128Bytes.Capacity);
			return fixedString128Bytes;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005C3C File Offset: 0x00003E3C
		public unsafe FixedString512Bytes ReadFixedString512()
		{
			FixedString512Bytes fixedString512Bytes;
			byte* ptr = (byte*)(&fixedString512Bytes) + 2;
			*(short*)(&fixedString512Bytes) = (short)this.ReadFixedStringInternal(ptr, fixedString512Bytes.Capacity);
			return fixedString512Bytes;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005C64 File Offset: 0x00003E64
		public unsafe FixedString4096Bytes ReadFixedString4096()
		{
			FixedString4096Bytes fixedString4096Bytes;
			byte* ptr = (byte*)(&fixedString4096Bytes) + 2;
			*(short*)(&fixedString4096Bytes) = (short)this.ReadFixedStringInternal(ptr, fixedString4096Bytes.Capacity);
			return fixedString4096Bytes;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005C8A File Offset: 0x00003E8A
		public unsafe ushort ReadFixedString(NativeArray<byte> array)
		{
			return this.ReadFixedStringInternal((byte*)array.GetUnsafePtr<byte>(), array.Length);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005CA0 File Offset: 0x00003EA0
		private unsafe ushort ReadFixedStringInternal(byte* data, int maxLength)
		{
			ushort num = this.ReadUShort();
			if ((int)num > maxLength)
			{
				return 0;
			}
			this.ReadBytesInternal(data, (int)num);
			return num;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public unsafe FixedString32Bytes ReadPackedFixedString32Delta(FixedString32Bytes baseline, in StreamCompressionModel model)
		{
			FixedString32Bytes fixedString32Bytes;
			byte* ptr = (byte*)(&fixedString32Bytes) + 2;
			*(short*)(&fixedString32Bytes) = (short)this.ReadPackedFixedStringDeltaInternal(ptr, fixedString32Bytes.Capacity, (byte*)(&baseline) + 2, *(ushort*)(&baseline), in model);
			return fixedString32Bytes;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public unsafe FixedString64Bytes ReadPackedFixedString64Delta(FixedString64Bytes baseline, in StreamCompressionModel model)
		{
			FixedString64Bytes fixedString64Bytes;
			byte* ptr = (byte*)(&fixedString64Bytes) + 2;
			*(short*)(&fixedString64Bytes) = (short)this.ReadPackedFixedStringDeltaInternal(ptr, fixedString64Bytes.Capacity, (byte*)(&baseline) + 2, *(ushort*)(&baseline), in model);
			return fixedString64Bytes;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005D24 File Offset: 0x00003F24
		public unsafe FixedString128Bytes ReadPackedFixedString128Delta(FixedString128Bytes baseline, in StreamCompressionModel model)
		{
			FixedString128Bytes fixedString128Bytes;
			byte* ptr = (byte*)(&fixedString128Bytes) + 2;
			*(short*)(&fixedString128Bytes) = (short)this.ReadPackedFixedStringDeltaInternal(ptr, fixedString128Bytes.Capacity, (byte*)(&baseline) + 2, *(ushort*)(&baseline), in model);
			return fixedString128Bytes;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005D54 File Offset: 0x00003F54
		public unsafe FixedString512Bytes ReadPackedFixedString512Delta(FixedString512Bytes baseline, in StreamCompressionModel model)
		{
			FixedString512Bytes fixedString512Bytes;
			byte* ptr = (byte*)(&fixedString512Bytes) + 2;
			*(short*)(&fixedString512Bytes) = (short)this.ReadPackedFixedStringDeltaInternal(ptr, fixedString512Bytes.Capacity, (byte*)(&baseline) + 2, *(ushort*)(&baseline), in model);
			return fixedString512Bytes;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005D84 File Offset: 0x00003F84
		public unsafe FixedString4096Bytes ReadPackedFixedString4096Delta(FixedString4096Bytes baseline, in StreamCompressionModel model)
		{
			FixedString4096Bytes fixedString4096Bytes;
			byte* ptr = (byte*)(&fixedString4096Bytes) + 2;
			*(short*)(&fixedString4096Bytes) = (short)this.ReadPackedFixedStringDeltaInternal(ptr, fixedString4096Bytes.Capacity, (byte*)(&baseline) + 2, *(ushort*)(&baseline), in model);
			return fixedString4096Bytes;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public unsafe ushort ReadPackedFixedStringDelta(NativeArray<byte> data, NativeArray<byte> baseData, in StreamCompressionModel model)
		{
			return this.ReadPackedFixedStringDeltaInternal((byte*)data.GetUnsafePtr<byte>(), data.Length, (byte*)baseData.GetUnsafePtr<byte>(), (ushort)baseData.Length, in model);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005DD8 File Offset: 0x00003FD8
		private unsafe ushort ReadPackedFixedStringDeltaInternal(byte* data, int maxLength, byte* baseData, ushort baseLength, in StreamCompressionModel model)
		{
			uint num = this.ReadPackedUIntDelta((uint)baseLength, in model);
			if (num > (uint)maxLength)
			{
				return 0;
			}
			if (num <= (uint)baseLength)
			{
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					data[num2] = (byte)this.ReadPackedUIntDelta((uint)baseData[num2], in model);
					num2++;
				}
			}
			else
			{
				for (int i = 0; i < (int)baseLength; i++)
				{
					data[i] = (byte)this.ReadPackedUIntDelta((uint)baseData[i], in model);
				}
				int num3 = (int)baseLength;
				while ((long)num3 < (long)((ulong)num))
				{
					data[num3] = (byte)this.ReadPackedUInt(in model);
					num3++;
				}
			}
			return (ushort)num;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal readonly void CheckRead()
		{
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00005E58 File Offset: 0x00004058
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckBits(int numBits)
		{
			if (numBits < 0 || numBits > 32)
			{
				throw new ArgumentOutOfRangeException(string.Format("Invalid number of bits specified: {0}! Valid range is (0, 32) inclusive.", numBits));
			}
		}

		// Token: 0x040000AB RID: 171
		[NativeDisableUnsafePtrRestriction]
		internal unsafe byte* m_BufferPtr;

		// Token: 0x040000AC RID: 172
		private DataStreamReader.Context m_Context;

		// Token: 0x040000AD RID: 173
		private int m_Length;

		// Token: 0x02000046 RID: 70
		private struct Context
		{
			// Token: 0x040000AE RID: 174
			public int m_ReadByteIndex;

			// Token: 0x040000AF RID: 175
			public int m_BitIndex;

			// Token: 0x040000B0 RID: 176
			public ulong m_BitBuffer;

			// Token: 0x040000B1 RID: 177
			public int m_FailedReads;
		}
	}
}
