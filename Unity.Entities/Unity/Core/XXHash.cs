using System;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst;

namespace Unity.Core
{
	// Token: 0x020004CF RID: 1231
	[BurstCompile]
	public static class XXHash
	{
		// Token: 0x06001DB8 RID: 7608 RVA: 0x0006762C File Offset: 0x0006582C
		public unsafe static uint Hash32(byte* buffer, int bufferLength, uint seed = 0U)
		{
			int num = bufferLength;
			byte* ptr = buffer;
			uint num6;
			if (bufferLength >= 16)
			{
				uint num2 = seed + 2654435761U + 2246822519U;
				uint num3 = seed + 2246822519U;
				uint num4 = seed;
				uint num5 = seed - 2654435761U;
				do
				{
					num6 = XXHash.processStripe32(ref ptr, ref num2, ref num3, ref num4, ref num5);
					num -= 16;
				}
				while (num >= 16);
			}
			else
			{
				num6 = seed + 374761393U;
			}
			num6 += (uint)bufferLength;
			num6 = XXHash.processRemaining32(ptr, num6, num);
			return XXHash.avalanche32(num6);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000676A0 File Offset: 0x000658A0
		public unsafe static uint Hash32(Stream stream, uint seed = 0U)
		{
			byte[] array = new byte[16384];
			int num = stream.Read(array, 0, 16384);
			int num2 = num;
			byte[] array2;
			byte* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			byte* ptr2 = ptr;
			uint num7;
			if (num >= 16)
			{
				uint num3 = seed + 2654435761U + 2246822519U;
				uint num4 = seed + 2246822519U;
				uint num5 = seed;
				uint num6 = seed - 2654435761U;
				for (;;)
				{
					num7 = XXHash.processStripe32(ref ptr2, ref num3, ref num4, ref num5, ref num6);
					num -= 16;
					if (num < 16)
					{
						if (num == 0)
						{
							num = stream.Read(array, 0, 16384);
							ptr2 = ptr;
							num2 += num;
						}
						if (num < 16)
						{
							break;
						}
					}
				}
			}
			else
			{
				num7 = seed + 374761393U;
			}
			num7 += (uint)num2;
			num7 = XXHash.processRemaining32(ptr2, num7, num);
			array2 = null;
			return XXHash.avalanche32(num7);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0006776C File Offset: 0x0006596C
		public unsafe static ulong Hash64(byte* buffer, int bufferLength, ulong seed = 0UL)
		{
			int num = bufferLength;
			byte* ptr = buffer;
			ulong num6;
			if (bufferLength >= 32)
			{
				ulong num2 = seed + 11400714785074694791UL + 14029467366897019727UL;
				ulong num3 = seed + 14029467366897019727UL;
				ulong num4 = seed;
				ulong num5 = seed - 11400714785074694791UL;
				do
				{
					num6 = XXHash.processStripe64(ref ptr, ref num2, ref num3, ref num4, ref num5);
					num -= 32;
				}
				while (num >= 32);
			}
			else
			{
				num6 = seed + 2870177450012600261UL;
			}
			num6 += (ulong)((long)bufferLength);
			num6 = XXHash.processRemaining64(ptr, num6, num);
			return XXHash.avalanche64(num6);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000677F4 File Offset: 0x000659F4
		public unsafe static ulong Hash64(Stream stream, ulong seed = 0UL)
		{
			byte[] array = new byte[32768];
			int num = stream.Read(array, 0, 32768);
			ulong num2 = (ulong)((long)num);
			byte[] array2;
			byte* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			byte* ptr2 = ptr;
			ulong num7;
			if (num >= 32)
			{
				ulong num3 = seed + 11400714785074694791UL + 14029467366897019727UL;
				ulong num4 = seed + 14029467366897019727UL;
				ulong num5 = seed;
				ulong num6 = seed - 11400714785074694791UL;
				for (;;)
				{
					num7 = XXHash.processStripe64(ref ptr2, ref num3, ref num4, ref num5, ref num6);
					num -= 32;
					if (num < 32)
					{
						if (num == 0)
						{
							num = stream.Read(array, 0, 32768);
							ptr2 = ptr;
							num2 += (ulong)((long)num);
						}
						if (num < 32)
						{
							break;
						}
					}
				}
			}
			else
			{
				num7 = seed + 2870177450012600261UL;
			}
			num7 += num2;
			num7 = XXHash.processRemaining64(ptr2, num7, num);
			array2 = null;
			return XXHash.avalanche64(num7);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000678D8 File Offset: 0x00065AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static ulong processStripe64(ref byte* pInput, ref ulong acc1, ref ulong acc2, ref ulong acc3, ref ulong acc4)
		{
			XXHash.processLane64(ref acc1, ref pInput);
			XXHash.processLane64(ref acc2, ref pInput);
			XXHash.processLane64(ref acc3, ref pInput);
			XXHash.processLane64(ref acc4, ref pInput);
			ulong num = XXHash.Bits.RotateLeft(acc1, 1) + XXHash.Bits.RotateLeft(acc2, 7) + XXHash.Bits.RotateLeft(acc3, 12) + XXHash.Bits.RotateLeft(acc4, 18);
			XXHash.mergeAccumulator64(ref num, acc1);
			XXHash.mergeAccumulator64(ref num, acc2);
			XXHash.mergeAccumulator64(ref num, acc3);
			XXHash.mergeAccumulator64(ref num, acc4);
			return num;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00067950 File Offset: 0x00065B50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void processLane64(ref ulong accn, ref byte* pInput)
		{
			ulong num = (ulong)(*pInput);
			accn = XXHash.round64(accn, num);
			pInput += 8;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00067974 File Offset: 0x00065B74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static ulong processRemaining64(byte* pInput, ulong acc, int remainingLen)
		{
			while (remainingLen >= 8)
			{
				ulong num = (ulong)(*(long*)pInput);
				acc ^= XXHash.round64(0UL, num);
				acc = XXHash.Bits.RotateLeft(acc, 27) * 11400714785074694791UL;
				acc += 9650029242287828579UL;
				remainingLen -= 8;
				pInput += 8;
			}
			while (remainingLen >= 4)
			{
				uint num2 = *(uint*)pInput;
				acc ^= (ulong)num2 * 11400714785074694791UL;
				acc = XXHash.Bits.RotateLeft(acc, 23) * 14029467366897019727UL;
				acc += 1609587929392839161UL;
				remainingLen -= 4;
				pInput += 4;
			}
			while (remainingLen >= 1)
			{
				byte b = *pInput;
				acc ^= (ulong)b * 2870177450012600261UL;
				acc = XXHash.Bits.RotateLeft(acc, 11) * 11400714785074694791UL;
				remainingLen--;
				pInput++;
			}
			return acc;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x00067A3D File Offset: 0x00065C3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong avalanche64(ulong acc)
		{
			acc ^= acc >> 33;
			acc *= 14029467366897019727UL;
			acc ^= acc >> 29;
			acc *= 1609587929392839161UL;
			acc ^= acc >> 32;
			return acc;
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00067A72 File Offset: 0x00065C72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong round64(ulong accn, ulong lane)
		{
			accn += lane * 14029467366897019727UL;
			return XXHash.Bits.RotateLeft(accn, 31) * 11400714785074694791UL;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00067A95 File Offset: 0x00065C95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void mergeAccumulator64(ref ulong acc, ulong accn)
		{
			acc ^= XXHash.round64(0UL, accn);
			acc *= 11400714785074694791UL;
			acc += 9650029242287828579UL;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00067AC0 File Offset: 0x00065CC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static uint processStripe32(ref byte* pInput, ref uint acc1, ref uint acc2, ref uint acc3, ref uint acc4)
		{
			XXHash.processLane32(ref pInput, ref acc1);
			XXHash.processLane32(ref pInput, ref acc2);
			XXHash.processLane32(ref pInput, ref acc3);
			XXHash.processLane32(ref pInput, ref acc4);
			return XXHash.Bits.RotateLeft(acc1, 1) + XXHash.Bits.RotateLeft(acc2, 7) + XXHash.Bits.RotateLeft(acc3, 12) + XXHash.Bits.RotateLeft(acc4, 18);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00067B10 File Offset: 0x00065D10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void processLane32(ref byte* pInput, ref uint accn)
		{
			uint num = *pInput;
			accn = XXHash.round32(accn, num);
			pInput += 4;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00067B34 File Offset: 0x00065D34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static uint processRemaining32(byte* pInput, uint acc, int remainingLen)
		{
			while (remainingLen >= 4)
			{
				uint num = *(uint*)pInput;
				acc += num * 3266489917U;
				acc = XXHash.Bits.RotateLeft(acc, 17) * 668265263U;
				remainingLen -= 4;
				pInput += 4;
			}
			while (remainingLen >= 1)
			{
				byte b = *pInput;
				acc += (uint)b * 374761393U;
				acc = XXHash.Bits.RotateLeft(acc, 11) * 2654435761U;
				remainingLen--;
				pInput++;
			}
			return acc;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00067B9E File Offset: 0x00065D9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint round32(uint accn, uint lane)
		{
			accn += lane * 2246822519U;
			accn = XXHash.Bits.RotateLeft(accn, 13);
			accn *= 2654435761U;
			return accn;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00067BBF File Offset: 0x00065DBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint avalanche32(uint acc)
		{
			acc ^= acc >> 15;
			acc *= 2246822519U;
			acc ^= acc >> 13;
			acc *= 3266489917U;
			acc ^= acc >> 16;
			return acc;
		}

		// Token: 0x04000AA2 RID: 2722
		private const ulong prime64v1 = 11400714785074694791UL;

		// Token: 0x04000AA3 RID: 2723
		private const ulong prime64v2 = 14029467366897019727UL;

		// Token: 0x04000AA4 RID: 2724
		private const ulong prime64v3 = 1609587929392839161UL;

		// Token: 0x04000AA5 RID: 2725
		private const ulong prime64v4 = 9650029242287828579UL;

		// Token: 0x04000AA6 RID: 2726
		private const ulong prime64v5 = 2870177450012600261UL;

		// Token: 0x04000AA7 RID: 2727
		private const uint prime32v1 = 2654435761U;

		// Token: 0x04000AA8 RID: 2728
		private const uint prime32v2 = 2246822519U;

		// Token: 0x04000AA9 RID: 2729
		private const uint prime32v3 = 3266489917U;

		// Token: 0x04000AAA RID: 2730
		private const uint prime32v4 = 668265263U;

		// Token: 0x04000AAB RID: 2731
		private const uint prime32v5 = 374761393U;

		// Token: 0x020004D0 RID: 1232
		[BurstCompile]
		private static class Bits
		{
			// Token: 0x06001DC7 RID: 7623 RVA: 0x00067BEC File Offset: 0x00065DEC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ulong RotateLeft(ulong value, int bits)
			{
				return (value << bits) | (value >> 64 - bits);
			}

			// Token: 0x06001DC8 RID: 7624 RVA: 0x00067BFE File Offset: 0x00065DFE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint RotateLeft(uint value, int bits)
			{
				return (value << bits) | (value >> 32 - bits);
			}

			// Token: 0x06001DC9 RID: 7625 RVA: 0x00067C10 File Offset: 0x00065E10
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint RotateRight(uint value, int bits)
			{
				return (value >> bits) | (value << 32 - bits);
			}

			// Token: 0x06001DCA RID: 7626 RVA: 0x00067C22 File Offset: 0x00065E22
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ulong RotateRight(ulong value, int bits)
			{
				return (value >> bits) | (value << 64 - bits);
			}

			// Token: 0x06001DCB RID: 7627 RVA: 0x00067C34 File Offset: 0x00065E34
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static ulong PartialBytesToUInt64(byte* ptr, int leftBytes)
			{
				ulong num = 0UL;
				for (int i = 0; i < leftBytes; i++)
				{
					num |= (ulong)ptr[i] << (i << 3);
				}
				return num;
			}

			// Token: 0x06001DCC RID: 7628 RVA: 0x00067C60 File Offset: 0x00065E60
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ulong PartialBytesToUInt64(byte[] buffer, int leftBytes)
			{
				ulong num = 0UL;
				for (int i = 0; i < leftBytes; i++)
				{
					num |= (ulong)buffer[i] << (i << 3);
				}
				return num;
			}

			// Token: 0x06001DCD RID: 7629 RVA: 0x00067C8C File Offset: 0x00065E8C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static uint PartialBytesToUInt32(byte* ptr, int leftBytes)
			{
				if (leftBytes > 3)
				{
					return *(uint*)ptr;
				}
				uint num = (uint)(*ptr);
				if (leftBytes > 1)
				{
					num |= (uint)((uint)ptr[1] << 8);
				}
				if (leftBytes > 2)
				{
					num |= (uint)((uint)ptr[2] << 16);
				}
				return num;
			}

			// Token: 0x06001DCE RID: 7630 RVA: 0x00067CC0 File Offset: 0x00065EC0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint PartialBytesToUInt32(byte[] buffer, int leftBytes)
			{
				if (leftBytes > 3)
				{
					return XXHash.Bits.ToUInt32(buffer, 0);
				}
				uint num = (uint)buffer[0];
				if (leftBytes > 1)
				{
					num |= (uint)((uint)buffer[1] << 8);
				}
				if (leftBytes > 2)
				{
					num |= (uint)((uint)buffer[2] << 16);
				}
				return num;
			}

			// Token: 0x06001DCF RID: 7631 RVA: 0x00067CF7 File Offset: 0x00065EF7
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static uint SwapBytes32(uint num)
			{
				return (XXHash.Bits.RotateLeft(num, 8) & 16711935U) | (XXHash.Bits.RotateRight(num, 8) & 4278255360U);
			}

			// Token: 0x06001DD0 RID: 7632 RVA: 0x00067D14 File Offset: 0x00065F14
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ulong SwapBytes64(ulong num)
			{
				num = (XXHash.Bits.RotateLeft(num, 48) & 18446462603027742720UL) | (XXHash.Bits.RotateLeft(num, 16) & 281470681808895UL);
				return (XXHash.Bits.RotateLeft(num, 8) & 18374966859414961920UL) | (XXHash.Bits.RotateRight(num, 8) & 71777214294589695UL);
			}

			// Token: 0x06001DD1 RID: 7633 RVA: 0x00067D6B File Offset: 0x00065F6B
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint ToUInt32(byte[] value, int startIndex)
			{
				return (uint)XXHash.Bits.ToInt32(value, startIndex);
			}

			// Token: 0x06001DD2 RID: 7634 RVA: 0x00067D74 File Offset: 0x00065F74
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe static int ToInt32(byte[] value, int startIndex)
			{
				fixed (byte* ptr = &value[startIndex])
				{
					byte* ptr2 = ptr;
					if ((startIndex & 3) == 0)
					{
						return *(int*)ptr2;
					}
					return (int)(*ptr2) | ((int)ptr2[1] << 8) | ((int)ptr2[2] << 16) | ((int)ptr2[3] << 24);
				}
			}
		}
	}
}
