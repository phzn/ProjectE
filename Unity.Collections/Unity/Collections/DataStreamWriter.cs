using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting.APIUpdating;

namespace Unity.Collections
{
	// Token: 0x02000047 RID: 71
	[MovedFrom(true, "Unity.Networking.Transport", "Unity.Networking.Transport", null)]
	[GenerateTestsForBurstCompatibility]
	public struct DataStreamWriter
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00005E7C File Offset: 0x0000407C
		public unsafe static bool IsLittleEndian
		{
			get
			{
				uint num = 1U;
				byte* ptr = (byte*)(&num);
				return *ptr == 1;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005E94 File Offset: 0x00004094
		public DataStreamWriter(int length, AllocatorManager.AllocatorHandle allocator)
		{
			DataStreamWriter.Initialize(out this, CollectionHelper.CreateNativeArray<byte>(length, allocator, NativeArrayOptions.ClearMemory));
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005EA4 File Offset: 0x000040A4
		public DataStreamWriter(NativeArray<byte> data)
		{
			DataStreamWriter.Initialize(out this, data);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005EB0 File Offset: 0x000040B0
		public unsafe DataStreamWriter(byte* data, int length)
		{
			NativeArray<byte> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)data, length, Allocator.Invalid);
			DataStreamWriter.Initialize(out this, nativeArray);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005ECD File Offset: 0x000040CD
		public unsafe NativeArray<byte> AsNativeArray()
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)this.m_Data.buffer, this.Length, Allocator.Invalid);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005EE8 File Offset: 0x000040E8
		private unsafe static void Initialize(out DataStreamWriter self, NativeArray<byte> data)
		{
			self.m_SendHandleData = IntPtr.Zero;
			self.m_Data.capacity = data.Length;
			self.m_Data.length = 0;
			self.m_Data.buffer = (byte*)data.GetUnsafePtr<byte>();
			self.m_Data.bitBuffer = 0UL;
			self.m_Data.bitIndex = 0;
			self.m_Data.failedWrites = 0;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000557D File Offset: 0x0000377D
		private static short ByteSwap(short val)
		{
			return (short)(((int)(val & 255) << 8) | ((val >> 8) & 255));
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005593 File Offset: 0x00003793
		private static int ByteSwap(int val)
		{
			return ((val & 255) << 24) | ((val & 65280) << 8) | ((val >> 8) & 65280) | ((val >> 24) & 255);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00005F54 File Offset: 0x00004154
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.buffer != null;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00005F68 File Offset: 0x00004168
		public readonly bool HasFailedWrites
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.failedWrites > 0;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00005F78 File Offset: 0x00004178
		public readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.capacity;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005F85 File Offset: 0x00004185
		public int Length
		{
			get
			{
				this.SyncBitData();
				return this.m_Data.length + (this.m_Data.bitIndex + 7 >> 3);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005FA8 File Offset: 0x000041A8
		public int LengthInBits
		{
			get
			{
				this.SyncBitData();
				return this.m_Data.length * 8 + this.m_Data.bitIndex;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005FCC File Offset: 0x000041CC
		private unsafe void SyncBitData()
		{
			int i = this.m_Data.bitIndex;
			if (i <= 0)
			{
				return;
			}
			ulong num = this.m_Data.bitBuffer;
			int num2 = 0;
			while (i > 0)
			{
				this.m_Data.buffer[this.m_Data.length + num2] = (byte)num;
				i -= 8;
				num >>= 8;
				num2++;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006028 File Offset: 0x00004228
		public unsafe void Flush()
		{
			while (this.m_Data.bitIndex > 0)
			{
				ref byte buffer = ref *this.m_Data.buffer;
				int length = this.m_Data.length;
				this.m_Data.length = length + 1;
				*((ref buffer) + length) = (byte)this.m_Data.bitBuffer;
				this.m_Data.bitIndex = this.m_Data.bitIndex - 8;
				this.m_Data.bitBuffer = this.m_Data.bitBuffer >> 8;
			}
			this.m_Data.bitIndex = 0;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000060A0 File Offset: 0x000042A0
		private unsafe bool WriteBytesInternal(byte* data, int bytes)
		{
			if (this.m_Data.length + (this.m_Data.bitIndex + 7 >> 3) + bytes > this.m_Data.capacity)
			{
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
				return false;
			}
			this.Flush();
			UnsafeUtility.MemCpy((void*)(this.m_Data.buffer + this.m_Data.length), (void*)data, (long)bytes);
			this.m_Data.length = this.m_Data.length + bytes;
			return true;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000611F File Offset: 0x0000431F
		public unsafe bool WriteByte(byte value)
		{
			return this.WriteBytesInternal(&value, 1);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000612B File Offset: 0x0000432B
		public unsafe bool WriteBytes(NativeArray<byte> value)
		{
			return this.WriteBytesInternal((byte*)value.GetUnsafeReadOnlyPtr<byte>(), value.Length);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006140 File Offset: 0x00004340
		public unsafe bool WriteBytes(Span<byte> value)
		{
			fixed (byte* pinnableReference = value.GetPinnableReference())
			{
				byte* ptr = pinnableReference;
				return this.WriteBytesInternal(ptr, value.Length);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006166 File Offset: 0x00004366
		public unsafe bool WriteShort(short value)
		{
			return this.WriteBytesInternal((byte*)(&value), 2);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006166 File Offset: 0x00004366
		public unsafe bool WriteUShort(ushort value)
		{
			return this.WriteBytesInternal((byte*)(&value), 2);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00006172 File Offset: 0x00004372
		public unsafe bool WriteInt(int value)
		{
			return this.WriteBytesInternal((byte*)(&value), 4);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006172 File Offset: 0x00004372
		public unsafe bool WriteUInt(uint value)
		{
			return this.WriteBytesInternal((byte*)(&value), 4);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000617E File Offset: 0x0000437E
		public unsafe bool WriteLong(long value)
		{
			return this.WriteBytesInternal((byte*)(&value), 8);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000617E File Offset: 0x0000437E
		public unsafe bool WriteULong(ulong value)
		{
			return this.WriteBytesInternal((byte*)(&value), 8);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000618C File Offset: 0x0000438C
		public unsafe bool WriteShortNetworkByteOrder(short value)
		{
			short num = (DataStreamWriter.IsLittleEndian ? DataStreamWriter.ByteSwap(value) : value);
			return this.WriteBytesInternal((byte*)(&num), 2);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000061B4 File Offset: 0x000043B4
		public bool WriteUShortNetworkByteOrder(ushort value)
		{
			return this.WriteShortNetworkByteOrder((short)value);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000061C0 File Offset: 0x000043C0
		public unsafe bool WriteIntNetworkByteOrder(int value)
		{
			int num = (DataStreamWriter.IsLittleEndian ? DataStreamWriter.ByteSwap(value) : value);
			return this.WriteBytesInternal((byte*)(&num), 4);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000061E8 File Offset: 0x000043E8
		public bool WriteUIntNetworkByteOrder(uint value)
		{
			return this.WriteIntNetworkByteOrder((int)value);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000061F4 File Offset: 0x000043F4
		public bool WriteFloat(float value)
		{
			return this.WriteInt((int)new UIntFloat
			{
				floatValue = value
			}.intValue);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006220 File Offset: 0x00004420
		public bool WriteDouble(double value)
		{
			return this.WriteLong((long)new UIntFloat
			{
				doubleValue = value
			}.longValue);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000624C File Offset: 0x0000444C
		private unsafe void FlushBits()
		{
			while (this.m_Data.bitIndex >= 8)
			{
				ref byte buffer = ref *this.m_Data.buffer;
				int length = this.m_Data.length;
				this.m_Data.length = length + 1;
				*((ref buffer) + length) = (byte)this.m_Data.bitBuffer;
				this.m_Data.bitIndex = this.m_Data.bitIndex - 8;
				this.m_Data.bitBuffer = this.m_Data.bitBuffer >> 8;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000062B5 File Offset: 0x000044B5
		private void WriteRawBitsInternal(uint value, int numbits)
		{
			this.m_Data.bitBuffer = this.m_Data.bitBuffer | ((ulong)value << this.m_Data.bitIndex);
			this.m_Data.bitIndex = this.m_Data.bitIndex + numbits;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000062E8 File Offset: 0x000044E8
		public bool WriteRawBits(uint value, int numbits)
		{
			if (this.m_Data.length + (this.m_Data.bitIndex + numbits + 7 >> 3) > this.m_Data.capacity)
			{
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
				return false;
			}
			this.WriteRawBitsInternal(value, numbits);
			this.FlushBits();
			return true;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006340 File Offset: 0x00004540
		public unsafe bool WritePackedUInt(uint value, in StreamCompressionModel model)
		{
			int num = model.CalculateBucket(value);
			uint num2 = *((ref model.bucketOffsets.FixedElementField) + (IntPtr)num * 4);
			int num3 = (int)(*((ref model.bucketSizes.FixedElementField) + num));
			ushort num4 = *((ref model.encodeTable.FixedElementField) + (IntPtr)num * 2);
			if (this.m_Data.length + (this.m_Data.bitIndex + (int)(num4 & 255) + num3 + 7 >> 3) > this.m_Data.capacity)
			{
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
				return false;
			}
			this.WriteRawBitsInternal((uint)(num4 >> 8), (int)(num4 & 255));
			this.WriteRawBitsInternal(value - num2, num3);
			this.FlushBits();
			return true;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000063F0 File Offset: 0x000045F0
		public unsafe bool WritePackedULong(ulong value, in StreamCompressionModel model)
		{
			uint* ptr = (uint*)(&value);
			return this.WritePackedUInt(*ptr, in model) & this.WritePackedUInt(ptr[1], in model);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006418 File Offset: 0x00004618
		public bool WritePackedInt(int value, in StreamCompressionModel model)
		{
			uint num = (uint)((value >> 31) ^ (value << 1));
			return this.WritePackedUInt(num, in model);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006438 File Offset: 0x00004638
		public bool WritePackedLong(long value, in StreamCompressionModel model)
		{
			ulong num = (ulong)((value >> 63) ^ (value << 1));
			return this.WritePackedULong(num, in model);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006456 File Offset: 0x00004656
		public bool WritePackedFloat(float value, in StreamCompressionModel model)
		{
			return this.WritePackedFloatDelta(value, 0f, in model);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006465 File Offset: 0x00004665
		public bool WritePackedDouble(double value, in StreamCompressionModel model)
		{
			return this.WritePackedDoubleDelta(value, 0.0, in model);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006478 File Offset: 0x00004678
		public bool WritePackedUIntDelta(uint value, uint baseline, in StreamCompressionModel model)
		{
			int num = (int)(baseline - value);
			return this.WritePackedInt(num, in model);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006494 File Offset: 0x00004694
		public bool WritePackedIntDelta(int value, int baseline, in StreamCompressionModel model)
		{
			int num = baseline - value;
			return this.WritePackedInt(num, in model);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000064B0 File Offset: 0x000046B0
		public bool WritePackedLongDelta(long value, long baseline, in StreamCompressionModel model)
		{
			long num = baseline - value;
			return this.WritePackedLong(num, in model);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000064CC File Offset: 0x000046CC
		public bool WritePackedULongDelta(ulong value, ulong baseline, in StreamCompressionModel model)
		{
			long num = (long)(baseline - value);
			return this.WritePackedLong(num, in model);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000064E8 File Offset: 0x000046E8
		public bool WritePackedFloatDelta(float value, float baseline, in StreamCompressionModel model)
		{
			int num = 0;
			if (value != baseline)
			{
				num = 32;
			}
			if (this.m_Data.length + (this.m_Data.bitIndex + 1 + num + 7 >> 3) > this.m_Data.capacity)
			{
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
				return false;
			}
			if (num == 0)
			{
				this.WriteRawBitsInternal(0U, 1);
			}
			else
			{
				this.WriteRawBitsInternal(1U, 1);
				this.WriteRawBitsInternal(new UIntFloat
				{
					floatValue = value
				}.intValue, num);
			}
			this.FlushBits();
			return true;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006578 File Offset: 0x00004778
		public unsafe bool WritePackedDoubleDelta(double value, double baseline, in StreamCompressionModel model)
		{
			int num = 0;
			if (value != baseline)
			{
				num = 64;
			}
			if (this.m_Data.length + (this.m_Data.bitIndex + 1 + num + 7 >> 3) > this.m_Data.capacity)
			{
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
				return false;
			}
			if (num == 0)
			{
				this.WriteRawBitsInternal(0U, 1);
			}
			else
			{
				this.WriteRawBitsInternal(1U, 1);
				UIntFloat uintFloat = default(UIntFloat);
				uintFloat.doubleValue = value;
				uint* ptr = (uint*)(&uintFloat.longValue);
				this.WriteRawBitsInternal(*ptr, 32);
				this.FlushBits();
				this.WriteRawBitsInternal(ptr[1], 32);
			}
			this.FlushBits();
			return true;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006620 File Offset: 0x00004820
		public unsafe bool WriteFixedString32(FixedString32Bytes str)
		{
			int num = (int)(*(ushort*)(&str) + 2);
			byte* ptr = (byte*)(&str);
			return this.WriteBytesInternal(ptr, num);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006640 File Offset: 0x00004840
		public unsafe bool WriteFixedString64(FixedString64Bytes str)
		{
			int num = (int)(*(ushort*)(&str) + 2);
			byte* ptr = (byte*)(&str);
			return this.WriteBytesInternal(ptr, num);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006660 File Offset: 0x00004860
		public unsafe bool WriteFixedString128(FixedString128Bytes str)
		{
			int num = (int)(*(ushort*)(&str) + 2);
			byte* ptr = (byte*)(&str);
			return this.WriteBytesInternal(ptr, num);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006680 File Offset: 0x00004880
		public unsafe bool WriteFixedString512(FixedString512Bytes str)
		{
			int num = (int)(*(ushort*)(&str) + 2);
			byte* ptr = (byte*)(&str);
			return this.WriteBytesInternal(ptr, num);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000066A0 File Offset: 0x000048A0
		public unsafe bool WriteFixedString4096(FixedString4096Bytes str)
		{
			int num = (int)(*(ushort*)(&str) + 2);
			byte* ptr = (byte*)(&str);
			return this.WriteBytesInternal(ptr, num);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000066C0 File Offset: 0x000048C0
		public unsafe bool WritePackedFixedString32Delta(FixedString32Bytes str, FixedString32Bytes baseline, in StreamCompressionModel model)
		{
			ushort num = *(ushort*)(&str);
			byte* ptr = (byte*)(&str) + 2;
			return this.WritePackedFixedStringDelta(ptr, (uint)num, (byte*)(&baseline) + 2, (uint)(*(ushort*)(&baseline)), in model);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000066EC File Offset: 0x000048EC
		public unsafe bool WritePackedFixedString64Delta(FixedString64Bytes str, FixedString64Bytes baseline, in StreamCompressionModel model)
		{
			ushort num = *(ushort*)(&str);
			byte* ptr = (byte*)(&str) + 2;
			return this.WritePackedFixedStringDelta(ptr, (uint)num, (byte*)(&baseline) + 2, (uint)(*(ushort*)(&baseline)), in model);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00006718 File Offset: 0x00004918
		public unsafe bool WritePackedFixedString128Delta(FixedString128Bytes str, FixedString128Bytes baseline, in StreamCompressionModel model)
		{
			ushort num = *(ushort*)(&str);
			byte* ptr = (byte*)(&str) + 2;
			return this.WritePackedFixedStringDelta(ptr, (uint)num, (byte*)(&baseline) + 2, (uint)(*(ushort*)(&baseline)), in model);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006744 File Offset: 0x00004944
		public unsafe bool WritePackedFixedString512Delta(FixedString512Bytes str, FixedString512Bytes baseline, in StreamCompressionModel model)
		{
			ushort num = *(ushort*)(&str);
			byte* ptr = (byte*)(&str) + 2;
			return this.WritePackedFixedStringDelta(ptr, (uint)num, (byte*)(&baseline) + 2, (uint)(*(ushort*)(&baseline)), in model);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006770 File Offset: 0x00004970
		public unsafe bool WritePackedFixedString4096Delta(FixedString4096Bytes str, FixedString4096Bytes baseline, in StreamCompressionModel model)
		{
			ushort num = *(ushort*)(&str);
			byte* ptr = (byte*)(&str) + 2;
			return this.WritePackedFixedStringDelta(ptr, (uint)num, (byte*)(&baseline) + 2, (uint)(*(ushort*)(&baseline)), in model);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000679C File Offset: 0x0000499C
		private unsafe bool WritePackedFixedStringDelta(byte* data, uint length, byte* baseData, uint baseLength, in StreamCompressionModel model)
		{
			DataStreamWriter.StreamData data2 = this.m_Data;
			if (!this.WritePackedUIntDelta(length, baseLength, in model))
			{
				return false;
			}
			bool flag = false;
			if (length <= baseLength)
			{
				for (uint num = 0U; num < length; num += 1U)
				{
					flag |= !this.WritePackedUIntDelta((uint)data[num], (uint)baseData[num], in model);
				}
			}
			else
			{
				for (uint num2 = 0U; num2 < baseLength; num2 += 1U)
				{
					flag |= !this.WritePackedUIntDelta((uint)data[num2], (uint)baseData[num2], in model);
				}
				for (uint num3 = baseLength; num3 < length; num3 += 1U)
				{
					flag |= !this.WritePackedUInt((uint)data[num3], in model);
				}
			}
			if (flag)
			{
				this.m_Data = data2;
				this.m_Data.failedWrites = this.m_Data.failedWrites + 1;
			}
			return !flag;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006854 File Offset: 0x00004A54
		public void Clear()
		{
			this.m_Data.length = 0;
			this.m_Data.bitIndex = 0;
			this.m_Data.bitBuffer = 0UL;
			this.m_Data.failedWrites = 0;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006887 File Offset: 0x00004A87
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckAllocator(AllocatorManager.AllocatorHandle allocator)
		{
			if (allocator.ToAllocator != Allocator.Temp)
			{
				throw new InvalidOperationException("DataStreamWriters can only be created with temp memory");
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000068A0 File Offset: 0x00004AA0
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckBits(uint value, int numBits)
		{
			if (numBits < 0 || numBits > 32)
			{
				throw new ArgumentOutOfRangeException(string.Format("Invalid number of bits specified: {0}! Valid range is (0, 32) inclusive.", numBits));
			}
			ulong num = 1UL << numBits;
			if ((ulong)value >= num)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value {0} does not fit in the specified number of bits: {1}! Range (inclusive) is (0, {2})!", value, numBits, num - 1UL));
			}
		}

		// Token: 0x040000B2 RID: 178
		[NativeDisableUnsafePtrRestriction]
		private DataStreamWriter.StreamData m_Data;

		// Token: 0x040000B3 RID: 179
		public IntPtr m_SendHandleData;

		// Token: 0x02000048 RID: 72
		private struct StreamData
		{
			// Token: 0x040000B4 RID: 180
			public unsafe byte* buffer;

			// Token: 0x040000B5 RID: 181
			public int length;

			// Token: 0x040000B6 RID: 182
			public int capacity;

			// Token: 0x040000B7 RID: 183
			public ulong bitBuffer;

			// Token: 0x040000B8 RID: 184
			public int bitIndex;

			// Token: 0x040000B9 RID: 185
			public int failedWrites;
		}
	}
}
