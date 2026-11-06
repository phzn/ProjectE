using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x020000F4 RID: 244
	[GenerateTestsForBurstCompatibility]
	[BurstCompile]
	[GenerateTestsForBurstCompatibility]
	[GenerateTestsForBurstCompatibility]
	public static class xxHash3
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x0001F628 File Offset: 0x0001D828
		internal unsafe static void Avx2HashLongInternalLoop(ulong* acc, byte* input, byte* dest, long length, byte* secret, int isHash64)
		{
			if (X86.Avx2.IsAvx2Supported)
			{
				long num = (length - 1L) / 1024L;
				int num2 = 0;
				while ((long)num2 < num)
				{
					xxHash3.Avx2Accumulate(acc, input + num2 * 1024, (dest == null) ? null : (dest + num2 * 1024), secret, 16L, isHash64);
					xxHash3.Avx2ScrambleAcc(acc, secret + 192 - 64);
					num2++;
				}
				long num3 = (length - 1L - 1024L * num) / 64L;
				xxHash3.Avx2Accumulate(acc, input + num * 1024L, (dest == null) ? null : (dest + num * 1024L), secret, num3, isHash64);
				byte* ptr = input + length - 64;
				xxHash3.Avx2Accumulate512(acc, ptr, null, secret + 192 - 64 - 7);
				if (dest != null)
				{
					long num4 = length % 64L;
					if (num4 != 0L)
					{
						UnsafeUtility.MemCpy((void*)(dest + length - num4), (void*)(input + length - num4), num4);
					}
				}
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0001F714 File Offset: 0x0001D914
		internal unsafe static void Avx2ScrambleAcc(ulong* acc, byte* secret)
		{
			if (X86.Avx2.IsAvx2Supported)
			{
				v256 v = X86.Avx.mm256_set1_epi32(-1640531535);
				v256 v2 = *(v256*)acc;
				v256 v3 = X86.Avx2.mm256_srli_epi64(v2, 47);
				v256 v4 = X86.Avx2.mm256_xor_si256(v2, v3);
				v256 v5 = X86.Avx.mm256_loadu_si256((void*)secret);
				v256 v6 = X86.Avx2.mm256_xor_si256(v4, v5);
				v256 v7 = X86.Avx2.mm256_shuffle_epi32(v6, X86.Sse.SHUFFLE(0, 3, 0, 1));
				v256 v8 = X86.Avx2.mm256_mul_epu32(v6, v);
				v256 v9 = X86.Avx2.mm256_mul_epu32(v7, v);
				*(v256*)acc = X86.Avx2.mm256_add_epi64(v8, X86.Avx2.mm256_slli_epi64(v9, 32));
				v256 v10 = *(v256*)(acc + sizeof(v256) / 8);
				v3 = X86.Avx2.mm256_srli_epi64(v10, 47);
				v256 v11 = X86.Avx2.mm256_xor_si256(v10, v3);
				v5 = X86.Avx.mm256_loadu_si256((void*)(secret + sizeof(v256)));
				v256 v12 = X86.Avx2.mm256_xor_si256(v11, v5);
				v7 = X86.Avx2.mm256_shuffle_epi32(v12, X86.Sse.SHUFFLE(0, 3, 0, 1));
				v8 = X86.Avx2.mm256_mul_epu32(v12, v);
				v9 = X86.Avx2.mm256_mul_epu32(v7, v);
				*(v256*)(acc + sizeof(v256) / 8) = X86.Avx2.mm256_add_epi64(v8, X86.Avx2.mm256_slli_epi64(v9, 32));
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001F80C File Offset: 0x0001DA0C
		internal unsafe static void Avx2Accumulate(ulong* acc, byte* input, byte* dest, byte* secret, long nbStripes, int isHash64)
		{
			if (X86.Avx2.IsAvx2Supported)
			{
				int num = 0;
				while ((long)num < nbStripes)
				{
					byte* ptr = input + num * 64;
					xxHash3.Avx2Accumulate512(acc, ptr, (dest == null) ? null : (dest + num * 64), secret + num * 8);
					num++;
				}
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0001F850 File Offset: 0x0001DA50
		internal unsafe static void Avx2Accumulate512(ulong* acc, byte* input, byte* dest, byte* secret)
		{
			if (X86.Avx2.IsAvx2Supported)
			{
				v256 v = X86.Avx.mm256_loadu_si256((void*)input);
				v256 v2 = X86.Avx.mm256_loadu_si256((void*)secret);
				v256 v3 = X86.Avx2.mm256_xor_si256(v, v2);
				if (dest != null)
				{
					X86.Avx.mm256_storeu_si256((void*)dest, v);
				}
				v256 v4 = X86.Avx2.mm256_shuffle_epi32(v3, X86.Sse.SHUFFLE(0, 3, 0, 1));
				v256 v5 = X86.Avx2.mm256_mul_epu32(v3, v4);
				v256 v6 = X86.Avx2.mm256_shuffle_epi32(v, X86.Sse.SHUFFLE(1, 0, 3, 2));
				v256 v7 = X86.Avx2.mm256_add_epi64(*(v256*)acc, v6);
				*(v256*)acc = X86.Avx2.mm256_add_epi64(v5, v7);
				v = X86.Avx.mm256_loadu_si256((void*)(input + sizeof(v256)));
				v2 = X86.Avx.mm256_loadu_si256((void*)(secret + sizeof(v256)));
				v256 v8 = X86.Avx2.mm256_xor_si256(v, v2);
				if (dest != null)
				{
					X86.Avx.mm256_storeu_si256((void*)(dest + 32), v);
				}
				v4 = X86.Avx2.mm256_shuffle_epi32(v8, X86.Sse.SHUFFLE(0, 3, 0, 1));
				v5 = X86.Avx2.mm256_mul_epu32(v8, v4);
				v6 = X86.Avx2.mm256_shuffle_epi32(v, X86.Sse.SHUFFLE(1, 0, 3, 2));
				v7 = X86.Avx2.mm256_add_epi64(*(v256*)(acc + sizeof(v256) / 8), v6);
				*(v256*)(acc + sizeof(v256) / 8) = X86.Avx2.mm256_add_epi64(v5, v7);
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0001F964 File Offset: 0x0001DB64
		public unsafe static uint2 Hash64(void* input, long length)
		{
			byte[] kSecret;
			void* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = (void*)(&kSecret[0]);
			}
			return xxHash3.ToUint2(xxHash3.Hash64Internal((byte*)input, null, length, (byte*)ptr, 0UL));
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0001F99E File Offset: 0x0001DB9E
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static uint2 Hash64<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in T input) where T : struct, ValueType
		{
			return xxHash3.Hash64(UnsafeUtilityExtensions.AddressOf<T>(in input), (long)UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0001F9B4 File Offset: 0x0001DBB4
		public unsafe static uint2 Hash64(void* input, long length, ulong seed)
		{
			byte[] kSecret;
			byte* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &kSecret[0];
			}
			return xxHash3.ToUint2(xxHash3.Hash64Internal((byte*)input, null, length, ptr, seed));
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0001F9F0 File Offset: 0x0001DBF0
		public unsafe static uint4 Hash128(void* input, long length)
		{
			byte[] kSecret;
			void* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = (void*)(&kSecret[0]);
			}
			uint4 @uint;
			xxHash3.Hash128Internal((byte*)input, null, length, (byte*)ptr, 0UL, out @uint);
			return @uint;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0001FA28 File Offset: 0x0001DC28
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static uint4 Hash128<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in T input) where T : struct, ValueType
		{
			return xxHash3.Hash128(UnsafeUtilityExtensions.AddressOf<T>(in input), (long)UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001FA3C File Offset: 0x0001DC3C
		public unsafe static uint4 Hash128(void* input, void* destination, long length)
		{
			byte[] kSecret;
			byte* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &kSecret[0];
			}
			uint4 @uint;
			xxHash3.Hash128Internal((byte*)input, (byte*)destination, length, ptr, 0UL, out @uint);
			return @uint;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001FA74 File Offset: 0x0001DC74
		public unsafe static uint4 Hash128(void* input, long length, ulong seed)
		{
			byte[] kSecret;
			byte* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &kSecret[0];
			}
			uint4 @uint;
			xxHash3.Hash128Internal((byte*)input, null, length, ptr, seed, out @uint);
			return @uint;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0001FAAC File Offset: 0x0001DCAC
		public unsafe static uint4 Hash128(void* input, void* destination, long length, ulong seed)
		{
			byte[] kSecret;
			byte* ptr;
			if ((kSecret = xxHashDefaultKey.kSecret) == null || kSecret.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &kSecret[0];
			}
			uint4 @uint;
			xxHash3.Hash128Internal((byte*)input, (byte*)destination, length, ptr, seed, out @uint);
			return @uint;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
		internal unsafe static ulong Hash64Internal(byte* input, byte* dest, long length, byte* secret, ulong seed)
		{
			if (dest != null && length < 240L)
			{
				UnsafeUtility.MemCpy((void*)dest, (void*)input, length);
			}
			if (length <= 16L)
			{
				return xxHash3.Hash64Len0To16(input, length, secret, seed);
			}
			if (length <= 128L)
			{
				return xxHash3.Hash64Len17To128(input, length, secret, seed);
			}
			if (length <= 240L)
			{
				return xxHash3.Hash64Len129To240(input, length, secret, seed);
			}
			if (seed != 0UL)
			{
				byte* ptr = (stackalloc byte[(UIntPtr)223] + 31L) & -32L;
				xxHash3.EncodeSecretKey(ptr, secret, seed);
				return xxHash3.Hash64Long(input, dest, length, ptr);
			}
			return xxHash3.Hash64Long(input, dest, length, secret);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0001FB74 File Offset: 0x0001DD74
		internal unsafe static void Hash128Internal(byte* input, byte* dest, long length, byte* secret, ulong seed, out uint4 result)
		{
			if (dest != null && length < 240L)
			{
				UnsafeUtility.MemCpy((void*)dest, (void*)input, length);
			}
			if (length <= 16L)
			{
				xxHash3.Hash128Len0To16(input, length, secret, seed, out result);
				return;
			}
			if (length <= 128L)
			{
				xxHash3.Hash128Len17To128(input, length, secret, seed, out result);
				return;
			}
			if (length <= 240L)
			{
				xxHash3.Hash128Len129To240(input, length, secret, seed, out result);
				return;
			}
			if (seed != 0UL)
			{
				byte* ptr = (stackalloc byte[(UIntPtr)223] + 31L) & -32L;
				xxHash3.EncodeSecretKey(ptr, secret, seed);
				xxHash3.Hash128Long(input, dest, length, ptr, out result);
				return;
			}
			xxHash3.Hash128Long(input, dest, length, secret, out result);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001FC10 File Offset: 0x0001DE10
		private unsafe static ulong Hash64Len1To3(byte* input, long len, byte* secret, ulong seed)
		{
			ulong num = (ulong)(*input);
			byte b = input[len >> 1];
			byte b2 = input[len - 1L];
			ulong num2 = (num << 16) | (ulong)((ulong)b << 24) | (ulong)b2 | (ulong)((ulong)((uint)len) << 8);
			ulong num3 = (ulong)(xxHash3.Read32LE((void*)secret) ^ xxHash3.Read32LE((void*)(secret + 4))) + seed;
			return xxHash3.AvalancheH64(num2 ^ num3);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001FC5C File Offset: 0x0001DE5C
		private unsafe static ulong Hash64Len4To8(byte* input, long length, byte* secret, ulong seed)
		{
			seed ^= (ulong)xxHash3.Swap32((uint)seed) << 32;
			uint num = xxHash3.Read32LE((void*)input);
			ulong num2 = (ulong)xxHash3.Read32LE((void*)(input + length - 4));
			ulong num3 = (xxHash3.Read64LE((void*)(secret + 8)) ^ xxHash3.Read64LE((void*)(secret + 16))) - seed;
			return xxHash3.rrmxmx((num2 + ((ulong)num << 32)) ^ num3, (ulong)length);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001FCB0 File Offset: 0x0001DEB0
		private unsafe static ulong Hash64Len9To16(byte* input, long length, byte* secret, ulong seed)
		{
			ulong num = (xxHash3.Read64LE((void*)(secret + 24)) ^ xxHash3.Read64LE((void*)(secret + 32))) + seed;
			ulong num2 = (xxHash3.Read64LE((void*)(secret + 40)) ^ xxHash3.Read64LE((void*)(secret + 48))) - seed;
			ulong num3 = xxHash3.Read64LE((void*)input) ^ num;
			ulong num4 = xxHash3.Read64LE((void*)(input + length - 8)) ^ num2;
			return xxHash3.Avalanche((ulong)(length + (long)xxHash3.Swap64(num3) + (long)num4 + (long)xxHash3.Mul128Fold64(num3, num4)));
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001FD18 File Offset: 0x0001DF18
		private unsafe static ulong Hash64Len0To16(byte* input, long length, byte* secret, ulong seed)
		{
			if (length > 8L)
			{
				return xxHash3.Hash64Len9To16(input, length, secret, seed);
			}
			if (length >= 4L)
			{
				return xxHash3.Hash64Len4To8(input, length, secret, seed);
			}
			if (length > 0L)
			{
				return xxHash3.Hash64Len1To3(input, length, secret, seed);
			}
			return xxHash3.AvalancheH64(seed ^ (xxHash3.Read64LE((void*)(secret + 56)) ^ xxHash3.Read64LE((void*)(secret + 64))));
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001FD6C File Offset: 0x0001DF6C
		private unsafe static ulong Hash64Len17To128(byte* input, long length, byte* secret, ulong seed)
		{
			ulong num = (ulong)(length * -7046029288634856825L);
			if (length > 32L)
			{
				if (length > 64L)
				{
					if (length > 96L)
					{
						num += xxHash3.Mix16(input + 48, secret + 96, seed);
						num += xxHash3.Mix16(input + length - 64, secret + 112, seed);
					}
					num += xxHash3.Mix16(input + 32, secret + 64, seed);
					num += xxHash3.Mix16(input + length - 48, secret + 80, seed);
				}
				num += xxHash3.Mix16(input + 16, secret + 32, seed);
				num += xxHash3.Mix16(input + length - 32, secret + 48, seed);
			}
			num += xxHash3.Mix16(input, secret, seed);
			num += xxHash3.Mix16(input + length - 16, secret + 16, seed);
			return xxHash3.Avalanche(num);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001FE2C File Offset: 0x0001E02C
		private unsafe static ulong Hash64Len129To240(byte* input, long length, byte* secret, ulong seed)
		{
			ulong num = (ulong)(length * -7046029288634856825L);
			int num2 = (int)length / 16;
			for (int i = 0; i < 8; i++)
			{
				num += xxHash3.Mix16(input + 16 * i, secret + 16 * i, seed);
			}
			num = xxHash3.Avalanche(num);
			for (int j = 8; j < num2; j++)
			{
				num += xxHash3.Mix16(input + 16 * j, secret + 16 * (j - 8) + 3, seed);
			}
			num += xxHash3.Mix16(input + length - 16, secret + 136 - 17, seed);
			return xxHash3.Avalanche(num);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		[BurstCompile]
		private unsafe static ulong Hash64Long(byte* input, byte* dest, long length, byte* secret)
		{
			return xxHash3.Hash64Long_00000A6B$BurstDirectCall.Invoke(input, dest, length, secret);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001FEC4 File Offset: 0x0001E0C4
		private unsafe static void Hash128Len1To3(byte* input, long length, byte* secret, ulong seed, out uint4 result)
		{
			int num = (int)(*input);
			byte b = input[length >> 1];
			byte b2 = input[length - 1L];
			int num2 = (num << 16) + ((int)b << 24) + (int)b2 + (int)((int)((uint)length) << 8);
			uint num3 = xxHash3.RotL32(xxHash3.Swap32((uint)num2), 13);
			ulong num4 = (ulong)(xxHash3.Read32LE((void*)secret) ^ xxHash3.Read32LE((void*)(secret + 4))) + seed;
			ulong num5 = (ulong)(xxHash3.Read32LE((void*)(secret + 8)) ^ xxHash3.Read32LE((void*)(secret + 12))) - seed;
			ulong num6 = (ulong)num2 ^ num4;
			ulong num7 = (ulong)num3 ^ num5;
			result = xxHash3.ToUint4(xxHash3.AvalancheH64(num6), xxHash3.AvalancheH64(num7));
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001FF54 File Offset: 0x0001E154
		private unsafe static void Hash128Len4To8(byte* input, long len, byte* secret, ulong seed, out uint4 result)
		{
			seed ^= (ulong)xxHash3.Swap32((uint)seed) << 32;
			ulong num = (ulong)xxHash3.Read32LE((void*)input);
			uint num2 = xxHash3.Read32LE((void*)(input + len - 4));
			ulong num3 = num + ((ulong)num2 << 32);
			ulong num4 = (xxHash3.Read64LE((void*)(secret + 16)) ^ xxHash3.Read64LE((void*)(secret + 24))) + seed;
			ulong num6;
			ulong num5 = Common.umul128(num3 ^ num4, (ulong)(-7046029288634856825L + (len << 2)), out num6);
			num6 += num5 << 1;
			num5 ^= num6 >> 3;
			num5 = xxHash3.XorShift64(num5, 35);
			num5 *= 11507291218515648293UL;
			num5 = xxHash3.XorShift64(num5, 28);
			num6 = xxHash3.Avalanche(num6);
			result = xxHash3.ToUint4(num5, num6);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001FFF8 File Offset: 0x0001E1F8
		private unsafe static void Hash128Len9To16(byte* input, long len, byte* secret, ulong seed, out uint4 result)
		{
			ulong num = (xxHash3.Read64LE((void*)(secret + 32)) ^ xxHash3.Read64LE((void*)(secret + 40))) - seed;
			ulong num2 = (xxHash3.Read64LE((void*)(secret + 48)) ^ xxHash3.Read64LE((void*)(secret + 56))) + seed;
			ulong num3 = xxHash3.Read64LE((void*)input);
			ulong num4 = xxHash3.Read64LE((void*)(input + len - 8));
			ulong num6;
			ulong num5 = Common.umul128(num3 ^ num4 ^ num, 11400714785074694791UL, out num6) + (ulong)((ulong)(len - 1L) << 54);
			num4 ^= num2;
			num6 += num4 + xxHash3.Mul32To64((uint)num4, 2246822518U);
			ulong num8;
			ulong num7 = Common.umul128(num5 ^ xxHash3.Swap64(num6), 14029467366897019727UL, out num8);
			num8 += num6 * 14029467366897019727UL;
			result = xxHash3.ToUint4(xxHash3.Avalanche(num7), xxHash3.Avalanche(num8));
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000200B8 File Offset: 0x0001E2B8
		private unsafe static void Hash128Len0To16(byte* input, long length, byte* secret, ulong seed, out uint4 result)
		{
			if (length > 8L)
			{
				xxHash3.Hash128Len9To16(input, length, secret, seed, out result);
				return;
			}
			if (length >= 4L)
			{
				xxHash3.Hash128Len4To8(input, length, secret, seed, out result);
				return;
			}
			if (length > 0L)
			{
				xxHash3.Hash128Len1To3(input, length, secret, seed, out result);
				return;
			}
			ulong num = xxHash3.Read64LE((void*)(secret + 64)) ^ xxHash3.Read64LE((void*)(secret + 72));
			ulong num2 = xxHash3.Read64LE((void*)(secret + 80)) ^ xxHash3.Read64LE((void*)(secret + 88));
			ulong num3 = xxHash3.AvalancheH64(seed ^ num);
			ulong num4 = xxHash3.AvalancheH64(seed ^ num2);
			result = xxHash3.ToUint4(num3, num4);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00020140 File Offset: 0x0001E340
		private unsafe static void Hash128Len17To128(byte* input, long length, byte* secret, ulong seed, out uint4 result)
		{
			xxHash3.ulong2 @ulong = new xxHash3.ulong2((ulong)(length * -7046029288634856825L), 0UL);
			if (length > 32L)
			{
				if (length > 64L)
				{
					if (length > 96L)
					{
						@ulong = xxHash3.Mix32(@ulong, input + 48, input + length - 64, secret + 96, seed);
					}
					@ulong = xxHash3.Mix32(@ulong, input + 32, input + length - 48, secret + 64, seed);
				}
				@ulong = xxHash3.Mix32(@ulong, input + 16, input + length - 32, secret + 32, seed);
			}
			@ulong = xxHash3.Mix32(@ulong, input, input + length - 16, secret, seed);
			ulong num = @ulong.x + @ulong.y;
			ulong num2 = @ulong.x * 11400714785074694791UL + @ulong.y * 9650029242287828579UL + (ulong)((length - (long)seed) * -4417276706812531889L);
			result = xxHash3.ToUint4(xxHash3.Avalanche(num), 0UL - xxHash3.Avalanche(num2));
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00020224 File Offset: 0x0001E424
		private unsafe static void Hash128Len129To240(byte* input, long length, byte* secret, ulong seed, out uint4 result)
		{
			xxHash3.ulong2 @ulong = new xxHash3.ulong2((ulong)(length * -7046029288634856825L), 0UL);
			long num = length / 32L;
			int i;
			for (i = 0; i < 4; i++)
			{
				@ulong = xxHash3.Mix32(@ulong, input + 32 * i, input + 32 * i + 16, secret + 32 * i, seed);
			}
			@ulong.x = xxHash3.Avalanche(@ulong.x);
			@ulong.y = xxHash3.Avalanche(@ulong.y);
			i = 4;
			while ((long)i < num)
			{
				@ulong = xxHash3.Mix32(@ulong, input + 32 * i, input + 32 * i + 16, secret + 3 + 32 * (i - 4), seed);
				i++;
			}
			@ulong = xxHash3.Mix32(@ulong, input + length - 16, input + length - 32, secret + 136 - 17 - 16, 0UL - seed);
			ulong num2 = @ulong.x + @ulong.y;
			ulong num3 = @ulong.x * 11400714785074694791UL + @ulong.y * 9650029242287828579UL + (ulong)((length - (long)seed) * -4417276706812531889L);
			result = xxHash3.ToUint4(xxHash3.Avalanche(num2), 0UL - xxHash3.Avalanche(num3));
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00020347 File Offset: 0x0001E547
		[BurstCompile]
		private unsafe static void Hash128Long(byte* input, byte* dest, long length, byte* secret, out uint4 result)
		{
			xxHash3.Hash128Long_00000A72$BurstDirectCall.Invoke(input, dest, length, secret, out result);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00020354 File Offset: 0x0001E554
		internal static uint2 ToUint2(ulong u)
		{
			return new uint2((uint)(u & (ulong)(-1)), (uint)(u >> 32));
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00020365 File Offset: 0x0001E565
		internal static uint4 ToUint4(ulong ul0, ulong ul1)
		{
			return new uint4((uint)(ul0 & (ulong)(-1)), (uint)(ul0 >> 32), (uint)(ul1 & (ulong)(-1)), (uint)(ul1 >> 32));
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00020380 File Offset: 0x0001E580
		internal unsafe static void EncodeSecretKey(byte* dst, byte* secret, ulong seed)
		{
			int num = 12;
			for (int i = 0; i < num; i++)
			{
				xxHash3.Write64LE((void*)(dst + 16 * i), xxHash3.Read64LE((void*)(secret + 16 * i)) + seed);
				xxHash3.Write64LE((void*)(dst + 16 * i + 8), xxHash3.Read64LE((void*)(secret + 16 * i + 8)) - seed);
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000203D0 File Offset: 0x0001E5D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static ulong Read64LE(void* addr)
		{
			return (ulong)(*(long*)addr);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000203D4 File Offset: 0x0001E5D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static uint Read32LE(void* addr)
		{
			return *(uint*)addr;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000203D8 File Offset: 0x0001E5D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void Write64LE(void* addr, ulong value)
		{
			*(long*)addr = (long)value;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000203DD File Offset: 0x0001E5DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void Read32LE(void* addr, uint value)
		{
			*(int*)addr = (int)value;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000203E2 File Offset: 0x0001E5E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Mul32To64(uint x, uint y)
		{
			return (ulong)x * (ulong)y;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000203EC File Offset: 0x0001E5EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Swap64(ulong x)
		{
			return ((x << 56) & 18374686479671623680UL) | ((x << 40) & 71776119061217280UL) | ((x << 24) & 280375465082880UL) | ((x << 8) & 1095216660480UL) | ((x >> 8) & (ulong)(-16777216)) | ((x >> 24) & 16711680UL) | ((x >> 40) & 65280UL) | ((x >> 56) & 255UL);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00020462 File Offset: 0x0001E662
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Swap32(uint x)
		{
			return ((x << 24) & 4278190080U) | ((x << 8) & 16711680U) | ((x >> 8) & 65280U) | ((x >> 24) & 255U);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002048D File Offset: 0x0001E68D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint RotL32(uint x, int r)
		{
			return (x << r) | (x >> 32 - r);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002049F File Offset: 0x0001E69F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong RotL64(ulong x, int r)
		{
			return (x << r) | (x >> 64 - r);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000204B1 File Offset: 0x0001E6B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong XorShift64(ulong v64, int shift)
		{
			return v64 ^ (v64 >> shift);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000204BC File Offset: 0x0001E6BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Mul128Fold64(ulong lhs, ulong rhs)
		{
			ulong num;
			return Common.umul128(lhs, rhs, out num) ^ num;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000204D4 File Offset: 0x0001E6D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static ulong Mix16(byte* input, byte* secret, ulong seed)
		{
			ulong num = xxHash3.Read64LE((void*)input);
			ulong num2 = xxHash3.Read64LE((void*)(input + 8));
			return xxHash3.Mul128Fold64(num ^ (xxHash3.Read64LE((void*)secret) + seed), num2 ^ (xxHash3.Read64LE((void*)(secret + 8)) - seed));
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002050C File Offset: 0x0001E70C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static xxHash3.ulong2 Mix32(xxHash3.ulong2 acc, byte* input_1, byte* input_2, byte* secret, ulong seed)
		{
			ulong num = (acc.x + xxHash3.Mix16(input_1, secret, seed)) ^ (xxHash3.Read64LE((void*)input_2) + xxHash3.Read64LE((void*)(input_2 + 8)));
			ulong num2 = acc.y + xxHash3.Mix16(input_2, secret + 16, seed);
			num2 ^= xxHash3.Read64LE((void*)input_1) + xxHash3.Read64LE((void*)(input_1 + 8));
			return new xxHash3.ulong2(num, num2);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00020565 File Offset: 0x0001E765
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong Avalanche(ulong h64)
		{
			h64 = xxHash3.XorShift64(h64, 37);
			h64 *= 1609587791953885689UL;
			h64 = xxHash3.XorShift64(h64, 32);
			return h64;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00020589 File Offset: 0x0001E789
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong AvalancheH64(ulong h64)
		{
			h64 ^= h64 >> 33;
			h64 *= 14029467366897019727UL;
			h64 ^= h64 >> 29;
			h64 *= 1609587929392839161UL;
			h64 ^= h64 >> 32;
			return h64;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000205C0 File Offset: 0x0001E7C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong rrmxmx(ulong h64, ulong length)
		{
			h64 ^= xxHash3.RotL64(h64, 49) ^ xxHash3.RotL64(h64, 24);
			h64 *= 11507291218515648293UL;
			h64 ^= (h64 >> 35) + length;
			h64 *= 11507291218515648293UL;
			return xxHash3.XorShift64(h64, 28);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002060E File Offset: 0x0001E80E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static ulong Mix2Acc(ulong acc0, ulong acc1, byte* secret)
		{
			return xxHash3.Mul128Fold64(acc0 ^ xxHash3.Read64LE((void*)secret), acc1 ^ xxHash3.Read64LE((void*)(secret + 8)));
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00020628 File Offset: 0x0001E828
		internal unsafe static ulong MergeAcc(ulong* acc, byte* secret, ulong start)
		{
			return xxHash3.Avalanche(start + xxHash3.Mix2Acc(*acc, acc[1], secret) + xxHash3.Mix2Acc(acc[2], acc[3], secret + 16) + xxHash3.Mix2Acc(acc[4], acc[5], secret + 32) + xxHash3.Mix2Acc(acc[6], acc[7], secret + 48));
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00020690 File Offset: 0x0001E890
		private unsafe static void DefaultHashLongInternalLoop(ulong* acc, byte* input, byte* dest, long length, byte* secret, int isHash64)
		{
			long num = (length - 1L) / 1024L;
			int num2 = 0;
			while ((long)num2 < num)
			{
				xxHash3.DefaultAccumulate(acc, input + num2 * 1024, (dest == null) ? null : (dest + num2 * 1024), secret, 16L, isHash64);
				xxHash3.DefaultScrambleAcc(acc, secret + 192 - 64);
				num2++;
			}
			long num3 = (length - 1L - 1024L * num) / 64L;
			xxHash3.DefaultAccumulate(acc, input + num * 1024L, (dest == null) ? null : (dest + num * 1024L), secret, num3, isHash64);
			byte* ptr = input + length - 64;
			xxHash3.DefaultAccumulate512(acc, ptr, null, secret + 192 - 64 - 7, isHash64);
			if (dest != null)
			{
				long num4 = length % 64L;
				if (num4 != 0L)
				{
					UnsafeUtility.MemCpy((void*)(dest + length - num4), (void*)(input + length - num4), num4);
				}
			}
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00020774 File Offset: 0x0001E974
		internal unsafe static void DefaultAccumulate(ulong* acc, byte* input, byte* dest, byte* secret, long nbStripes, int isHash64)
		{
			int num = 0;
			while ((long)num < nbStripes)
			{
				xxHash3.DefaultAccumulate512(acc, input + num * 64, (dest == null) ? null : (dest + num * 64), secret + num * 8, isHash64);
				num++;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000207B4 File Offset: 0x0001E9B4
		internal unsafe static void DefaultAccumulate512(ulong* acc, byte* input, byte* dest, byte* secret, int isHash64)
		{
			int num = 8;
			for (int i = 0; i < num; i++)
			{
				ulong num2 = xxHash3.Read64LE((void*)(input + 8 * i));
				ulong num3 = num2 ^ xxHash3.Read64LE((void*)(secret + i * 8));
				if (dest != null)
				{
					xxHash3.Write64LE((void*)(dest + 8 * i), num2);
				}
				acc[i ^ 1] += num2;
				acc[i] += xxHash3.Mul32To64((uint)(num3 & (ulong)(-1)), (uint)(num3 >> 32));
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00020820 File Offset: 0x0001EA20
		internal unsafe static void DefaultScrambleAcc(ulong* acc, byte* secret)
		{
			for (int i = 0; i < 8; i++)
			{
				ulong num = xxHash3.Read64LE((void*)(secret + 8 * i));
				ulong num2 = acc[i];
				num2 = xxHash3.XorShift64(num2, 47);
				num2 ^= num;
				num2 *= (ulong)(-1640531535);
				acc[i] = num2;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002086C File Offset: 0x0001EA6C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static ulong Hash64Long$BurstManaged(byte* input, byte* dest, long length, byte* secret)
		{
			ulong* ptr = (stackalloc ulong[(UIntPtr)95] + 31L / 8L) & -32L;
			*ptr = (ulong)(-1028477379);
			ptr[1] = 11400714785074694791UL;
			ptr[2] = 14029467366897019727UL;
			ptr[3] = 1609587929392839161UL;
			ptr[4] = 9650029242287828579UL;
			ptr[5] = (ulong)(-2048144777);
			ptr[6] = 2870177450012600261UL;
			ptr[7] = (ulong)(-1640531535);
			if (X86.Avx2.IsAvx2Supported)
			{
				xxHash3.Avx2HashLongInternalLoop(ptr, input, dest, length, secret, 1);
			}
			else
			{
				xxHash3.DefaultHashLongInternalLoop(ptr, input, dest, length, secret, 1);
			}
			return xxHash3.MergeAcc(ptr, secret + 11, (ulong)(length * -7046029288634856825L));
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002092C File Offset: 0x0001EB2C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void Hash128Long$BurstManaged(byte* input, byte* dest, long length, byte* secret, out uint4 result)
		{
			ulong* ptr = (stackalloc ulong[(UIntPtr)95] + 31L / 8L) & -32L;
			*ptr = (ulong)(-1028477379);
			ptr[1] = 11400714785074694791UL;
			ptr[2] = 14029467366897019727UL;
			ptr[3] = 1609587929392839161UL;
			ptr[4] = 9650029242287828579UL;
			ptr[5] = (ulong)(-2048144777);
			ptr[6] = 2870177450012600261UL;
			ptr[7] = (ulong)(-1640531535);
			if (X86.Avx2.IsAvx2Supported)
			{
				xxHash3.Avx2HashLongInternalLoop(ptr, input, dest, length, secret, 0);
			}
			else
			{
				xxHash3.DefaultHashLongInternalLoop(ptr, input, dest, length, secret, 0);
			}
			ulong num = xxHash3.MergeAcc(ptr, secret + 11, (ulong)(length * -7046029288634856825L));
			ulong num2 = xxHash3.MergeAcc(ptr, secret + 192 - 64 - 11, (ulong)(~(ulong)(length * -4417276706812531889L)));
			result = xxHash3.ToUint4(num, num2);
		}

		// Token: 0x04000472 RID: 1138
		private const int STRIPE_LEN = 64;

		// Token: 0x04000473 RID: 1139
		private const int ACC_NB = 8;

		// Token: 0x04000474 RID: 1140
		private const int SECRET_CONSUME_RATE = 8;

		// Token: 0x04000475 RID: 1141
		private const int SECRET_KEY_SIZE = 192;

		// Token: 0x04000476 RID: 1142
		private const int SECRET_KEY_MIN_SIZE = 136;

		// Token: 0x04000477 RID: 1143
		private const int SECRET_LASTACC_START = 7;

		// Token: 0x04000478 RID: 1144
		private const int NB_ROUNDS = 16;

		// Token: 0x04000479 RID: 1145
		private const int BLOCK_LEN = 1024;

		// Token: 0x0400047A RID: 1146
		private const uint PRIME32_1 = 2654435761U;

		// Token: 0x0400047B RID: 1147
		private const uint PRIME32_2 = 2246822519U;

		// Token: 0x0400047C RID: 1148
		private const uint PRIME32_3 = 3266489917U;

		// Token: 0x0400047D RID: 1149
		private const uint PRIME32_5 = 374761393U;

		// Token: 0x0400047E RID: 1150
		private const ulong PRIME64_1 = 11400714785074694791UL;

		// Token: 0x0400047F RID: 1151
		private const ulong PRIME64_2 = 14029467366897019727UL;

		// Token: 0x04000480 RID: 1152
		private const ulong PRIME64_3 = 1609587929392839161UL;

		// Token: 0x04000481 RID: 1153
		private const ulong PRIME64_4 = 9650029242287828579UL;

		// Token: 0x04000482 RID: 1154
		private const ulong PRIME64_5 = 2870177450012600261UL;

		// Token: 0x04000483 RID: 1155
		private const int MIDSIZE_MAX = 240;

		// Token: 0x04000484 RID: 1156
		private const int MIDSIZE_STARTOFFSET = 3;

		// Token: 0x04000485 RID: 1157
		private const int MIDSIZE_LASTOFFSET = 17;

		// Token: 0x04000486 RID: 1158
		private const int SECRET_MERGEACCS_START = 11;

		// Token: 0x020000F5 RID: 245
		private struct ulong2
		{
			// Token: 0x06000ABC RID: 2748 RVA: 0x00020A1B File Offset: 0x0001EC1B
			public ulong2(ulong x, ulong y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04000487 RID: 1159
			public ulong x;

			// Token: 0x04000488 RID: 1160
			public ulong y;
		}

		// Token: 0x020000F6 RID: 246
		[GenerateTestsForBurstCompatibility]
		public struct StreamingState
		{
			// Token: 0x06000ABD RID: 2749 RVA: 0x00020A2B File Offset: 0x0001EC2B
			public StreamingState(bool isHash64, ulong seed = 0UL)
			{
				this.State = default(xxHash3.StreamingState.StreamingStateData);
				this.Reset(isHash64, seed);
			}

			// Token: 0x06000ABE RID: 2750 RVA: 0x00020A44 File Offset: 0x0001EC44
			public unsafe void Reset(bool isHash64, ulong seed = 0UL)
			{
				int num = UnsafeUtility.SizeOf<xxHash3.StreamingState.StreamingStateData>();
				UnsafeUtility.MemClear(UnsafeUtility.AddressOf<xxHash3.StreamingState.StreamingStateData>(ref this.State), (long)num);
				this.State.IsHash64 = (isHash64 ? 1 : 0);
				ulong* acc = this.Acc;
				*acc = (ulong)(-1028477379);
				acc[1] = 11400714785074694791UL;
				acc[2] = 14029467366897019727UL;
				acc[3] = 1609587929392839161UL;
				acc[4] = 9650029242287828579UL;
				acc[5] = (ulong)(-2048144777);
				acc[6] = 2870177450012600261UL;
				acc[7] = (ulong)(-1640531535);
				this.State.Seed = seed;
				byte[] array;
				byte* ptr;
				if ((array = xxHashDefaultKey.kSecret) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				if (seed != 0UL)
				{
					xxHash3.EncodeSecretKey(this.SecretKey, ptr, seed);
				}
				else
				{
					UnsafeUtility.MemCpy((void*)this.SecretKey, (void*)ptr, 192L);
				}
				array = null;
			}

			// Token: 0x06000ABF RID: 2751 RVA: 0x00020B40 File Offset: 0x0001ED40
			public unsafe void Update(void* input, int length)
			{
				byte* ptr = (byte*)input;
				byte* ptr2 = ptr + length;
				int isHash = this.State.IsHash64;
				byte* secretKey = this.SecretKey;
				this.State.TotalLength = this.State.TotalLength + (long)length;
				if (this.State.BufferedSize + length <= xxHash3.StreamingState.INTERNAL_BUFFER_SIZE)
				{
					UnsafeUtility.MemCpy((void*)(this.Buffer + this.State.BufferedSize), (void*)ptr, (long)length);
					this.State.BufferedSize = this.State.BufferedSize + length;
					return;
				}
				if (this.State.BufferedSize != 0)
				{
					int num = xxHash3.StreamingState.INTERNAL_BUFFER_SIZE - this.State.BufferedSize;
					UnsafeUtility.MemCpy((void*)(this.Buffer + this.State.BufferedSize), (void*)ptr, (long)num);
					ptr += num;
					this.ConsumeStripes(this.Acc, ref this.State.NbStripesSoFar, this.Buffer, (long)xxHash3.StreamingState.INTERNAL_BUFFER_STRIPES, secretKey, isHash);
					this.State.BufferedSize = 0;
				}
				if (ptr + xxHash3.StreamingState.INTERNAL_BUFFER_SIZE < ptr2)
				{
					byte* ptr3 = ptr2 - xxHash3.StreamingState.INTERNAL_BUFFER_SIZE;
					do
					{
						this.ConsumeStripes(this.Acc, ref this.State.NbStripesSoFar, ptr, (long)xxHash3.StreamingState.INTERNAL_BUFFER_STRIPES, secretKey, isHash);
						ptr += xxHash3.StreamingState.INTERNAL_BUFFER_SIZE;
					}
					while (ptr < ptr3);
					UnsafeUtility.MemCpy((void*)(this.Buffer + xxHash3.StreamingState.INTERNAL_BUFFER_SIZE - 64), (void*)(ptr - 64), 64L);
				}
				if (ptr < ptr2)
				{
					long num2 = (long)(ptr2 - ptr);
					UnsafeUtility.MemCpy((void*)this.Buffer, (void*)ptr, num2);
					this.State.BufferedSize = (int)num2;
				}
			}

			// Token: 0x06000AC0 RID: 2752 RVA: 0x00020CAA File Offset: 0x0001EEAA
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public void Update<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(in T input) where T : struct, ValueType
			{
				this.Update(UnsafeUtilityExtensions.AddressOf<T>(in input), UnsafeUtility.SizeOf<T>());
			}

			// Token: 0x06000AC1 RID: 2753 RVA: 0x00020CC0 File Offset: 0x0001EEC0
			public unsafe uint4 DigestHash128()
			{
				byte* secretKey = this.SecretKey;
				uint4 @uint;
				if (this.State.TotalLength > 240L)
				{
					ulong* ptr = stackalloc ulong[(UIntPtr)64];
					this.DigestLong(ptr, secretKey, 0);
					ulong num = xxHash3.MergeAcc(ptr, secretKey + 11, (ulong)(this.State.TotalLength * -7046029288634856825L));
					ulong num2 = xxHash3.MergeAcc(ptr, secretKey + xxHash3.StreamingState.SECRET_LIMIT - 11, (ulong)(~(ulong)(this.State.TotalLength * -4417276706812531889L)));
					@uint = xxHash3.ToUint4(num, num2);
				}
				else
				{
					@uint = xxHash3.Hash128((void*)this.Buffer, this.State.TotalLength, this.State.Seed);
				}
				this.Reset(this.State.IsHash64 == 1, this.State.Seed);
				return @uint;
			}

			// Token: 0x06000AC2 RID: 2754 RVA: 0x00020D88 File Offset: 0x0001EF88
			public unsafe uint2 DigestHash64()
			{
				byte* secretKey = this.SecretKey;
				uint2 @uint;
				if (this.State.TotalLength > 240L)
				{
					ulong* ptr = stackalloc ulong[(UIntPtr)64];
					this.DigestLong(ptr, secretKey, 1);
					@uint = xxHash3.ToUint2(xxHash3.MergeAcc(ptr, secretKey + 11, (ulong)(this.State.TotalLength * -7046029288634856825L)));
				}
				else
				{
					@uint = xxHash3.Hash64((void*)this.Buffer, this.State.TotalLength, this.State.Seed);
				}
				this.Reset(this.State.IsHash64 == 1, this.State.Seed);
				return @uint;
			}

			// Token: 0x17000136 RID: 310
			// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00020E27 File Offset: 0x0001F027
			private unsafe ulong* Acc
			{
				[DebuggerStepThrough]
				get
				{
					return (ulong*)UnsafeUtility.AddressOf<ulong>(ref this.State.Acc);
				}
			}

			// Token: 0x17000137 RID: 311
			// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00020E39 File Offset: 0x0001F039
			private unsafe byte* Buffer
			{
				[DebuggerStepThrough]
				get
				{
					return (byte*)UnsafeUtility.AddressOf<byte>(ref this.State.Buffer);
				}
			}

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00020E4B File Offset: 0x0001F04B
			private unsafe byte* SecretKey
			{
				[DebuggerStepThrough]
				get
				{
					return (byte*)UnsafeUtility.AddressOf<byte>(ref this.State.SecretKey);
				}
			}

			// Token: 0x06000AC6 RID: 2758 RVA: 0x00020E60 File Offset: 0x0001F060
			private unsafe void DigestLong(ulong* acc, byte* secret, int isHash64)
			{
				UnsafeUtility.MemCpy((void*)acc, (void*)this.Acc, 64L);
				if (this.State.BufferedSize >= 64)
				{
					int num = (this.State.BufferedSize - 1) / 64;
					this.ConsumeStripes(acc, ref this.State.NbStripesSoFar, this.Buffer, (long)num, secret, isHash64);
					if (X86.Avx2.IsAvx2Supported)
					{
						xxHash3.Avx2Accumulate512(acc, this.Buffer + this.State.BufferedSize - 64, null, secret + xxHash3.StreamingState.SECRET_LIMIT - 7);
						return;
					}
					xxHash3.DefaultAccumulate512(acc, this.Buffer + this.State.BufferedSize - 64, null, secret + xxHash3.StreamingState.SECRET_LIMIT - 7, isHash64);
					return;
				}
				else
				{
					byte* ptr = stackalloc byte[(UIntPtr)64];
					int num2 = 64 - this.State.BufferedSize;
					UnsafeUtility.MemCpy((void*)ptr, (void*)(this.Buffer + xxHash3.StreamingState.INTERNAL_BUFFER_SIZE - num2), (long)num2);
					UnsafeUtility.MemCpy((void*)(ptr + num2), (void*)this.Buffer, (long)this.State.BufferedSize);
					if (X86.Avx2.IsAvx2Supported)
					{
						xxHash3.Avx2Accumulate512(acc, ptr, null, secret + xxHash3.StreamingState.SECRET_LIMIT - 7);
						return;
					}
					xxHash3.DefaultAccumulate512(acc, ptr, null, secret + xxHash3.StreamingState.SECRET_LIMIT - 7, isHash64);
					return;
				}
			}

			// Token: 0x06000AC7 RID: 2759 RVA: 0x00020F84 File Offset: 0x0001F184
			private unsafe void ConsumeStripes(ulong* acc, ref int nbStripesSoFar, byte* input, long totalStripes, byte* secret, int isHash64)
			{
				if ((long)(xxHash3.StreamingState.NB_STRIPES_PER_BLOCK - nbStripesSoFar) <= totalStripes)
				{
					int num = xxHash3.StreamingState.NB_STRIPES_PER_BLOCK - nbStripesSoFar;
					if (X86.Avx2.IsAvx2Supported)
					{
						xxHash3.Avx2Accumulate(acc, input, null, secret + nbStripesSoFar * 8, (long)num, isHash64);
						xxHash3.Avx2ScrambleAcc(acc, secret + xxHash3.StreamingState.SECRET_LIMIT);
						xxHash3.Avx2Accumulate(acc, input + num * 64, null, secret, totalStripes - (long)num, isHash64);
					}
					else
					{
						xxHash3.DefaultAccumulate(acc, input, null, secret + nbStripesSoFar * 8, (long)num, isHash64);
						xxHash3.DefaultScrambleAcc(acc, secret + xxHash3.StreamingState.SECRET_LIMIT);
						xxHash3.DefaultAccumulate(acc, input + num * 64, null, secret, totalStripes - (long)num, isHash64);
					}
					nbStripesSoFar = (int)totalStripes - num;
					return;
				}
				if (X86.Avx2.IsAvx2Supported)
				{
					xxHash3.Avx2Accumulate(acc, input, null, secret + nbStripesSoFar * 8, totalStripes, isHash64);
				}
				else
				{
					xxHash3.DefaultAccumulate(acc, input, null, secret + nbStripesSoFar * 8, totalStripes, isHash64);
				}
				nbStripesSoFar += (int)totalStripes;
			}

			// Token: 0x06000AC8 RID: 2760 RVA: 0x00021068 File Offset: 0x0001F268
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckKeySize(int isHash64)
			{
				if (this.State.IsHash64 != isHash64)
				{
					string text = ((this.State.IsHash64 != 0) ? "64" : "128");
					throw new InvalidOperationException("The streaming state was create for " + text + " bits hash key, the calling method doesn't support this key size, please use the appropriate API");
				}
			}

			// Token: 0x04000489 RID: 1161
			private static readonly int SECRET_LIMIT = 128;

			// Token: 0x0400048A RID: 1162
			private static readonly int NB_STRIPES_PER_BLOCK = xxHash3.StreamingState.SECRET_LIMIT / 8;

			// Token: 0x0400048B RID: 1163
			private static readonly int INTERNAL_BUFFER_SIZE = 256;

			// Token: 0x0400048C RID: 1164
			private static readonly int INTERNAL_BUFFER_STRIPES = xxHash3.StreamingState.INTERNAL_BUFFER_SIZE / 64;

			// Token: 0x0400048D RID: 1165
			private xxHash3.StreamingState.StreamingStateData State;

			// Token: 0x020000F7 RID: 247
			[StructLayout(LayoutKind.Explicit)]
			private struct StreamingStateData
			{
				// Token: 0x0400048E RID: 1166
				[FieldOffset(0)]
				public ulong Acc;

				// Token: 0x0400048F RID: 1167
				[FieldOffset(64)]
				public byte Buffer;

				// Token: 0x04000490 RID: 1168
				[FieldOffset(320)]
				public int IsHash64;

				// Token: 0x04000491 RID: 1169
				[FieldOffset(324)]
				public int BufferedSize;

				// Token: 0x04000492 RID: 1170
				[FieldOffset(328)]
				public int NbStripesSoFar;

				// Token: 0x04000493 RID: 1171
				[FieldOffset(336)]
				public long TotalLength;

				// Token: 0x04000494 RID: 1172
				[FieldOffset(344)]
				public ulong Seed;

				// Token: 0x04000495 RID: 1173
				[FieldOffset(352)]
				public byte SecretKey;

				// Token: 0x04000496 RID: 1174
				[FieldOffset(540)]
				public byte _PadEnd;
			}
		}

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x06000ACB RID: 2763
		internal unsafe delegate ulong Hash64Long_00000A6B$PostfixBurstDelegate(byte* input, byte* dest, long length, byte* secret);

		// Token: 0x020000F9 RID: 249
		internal static class Hash64Long_00000A6B$BurstDirectCall
		{
			// Token: 0x06000ACE RID: 2766 RVA: 0x000210E2 File Offset: 0x0001F2E2
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (xxHash3.Hash64Long_00000A6B$BurstDirectCall.Pointer == 0)
				{
					xxHash3.Hash64Long_00000A6B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(xxHash3.Hash64Long_00000A6B$BurstDirectCall.DeferredCompilation, methodof(xxHash3.Hash64Long$BurstManaged(byte*, byte*, long, byte*)).MethodHandle, typeof(xxHash3.Hash64Long_00000A6B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = xxHash3.Hash64Long_00000A6B$BurstDirectCall.Pointer;
			}

			// Token: 0x06000ACF RID: 2767 RVA: 0x00021110 File Offset: 0x0001F310
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				xxHash3.Hash64Long_00000A6B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000AD0 RID: 2768 RVA: 0x00021128 File Offset: 0x0001F328
			public unsafe static void Constructor()
			{
				xxHash3.Hash64Long_00000A6B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(xxHash3.Hash64Long(byte*, byte*, long, byte*)).MethodHandle);
			}

			// Token: 0x06000AD1 RID: 2769 RVA: 0x00002C47 File Offset: 0x00000E47
			public static void Initialize()
			{
			}

			// Token: 0x06000AD2 RID: 2770 RVA: 0x00021139 File Offset: 0x0001F339
			// Note: this type is marked as 'beforefieldinit'.
			static Hash64Long_00000A6B$BurstDirectCall()
			{
				xxHash3.Hash64Long_00000A6B$BurstDirectCall.Constructor();
			}

			// Token: 0x06000AD3 RID: 2771 RVA: 0x00021140 File Offset: 0x0001F340
			public unsafe static ulong Invoke(byte* input, byte* dest, long length, byte* secret)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = xxHash3.Hash64Long_00000A6B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.UInt64(System.Byte*,System.Byte*,System.Int64,System.Byte*), input, dest, length, secret, functionPointer);
					}
				}
				return xxHash3.Hash64Long$BurstManaged(input, dest, length, secret);
			}

			// Token: 0x04000497 RID: 1175
			private static IntPtr Pointer;

			// Token: 0x04000498 RID: 1176
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x06000AD5 RID: 2773
		internal unsafe delegate void Hash128Long_00000A72$PostfixBurstDelegate(byte* input, byte* dest, long length, byte* secret, out uint4 result);

		// Token: 0x020000FB RID: 251
		internal static class Hash128Long_00000A72$BurstDirectCall
		{
			// Token: 0x06000AD8 RID: 2776 RVA: 0x00021177 File Offset: 0x0001F377
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (xxHash3.Hash128Long_00000A72$BurstDirectCall.Pointer == 0)
				{
					xxHash3.Hash128Long_00000A72$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(xxHash3.Hash128Long_00000A72$BurstDirectCall.DeferredCompilation, methodof(xxHash3.Hash128Long$BurstManaged(byte*, byte*, long, byte*, ref uint4)).MethodHandle, typeof(xxHash3.Hash128Long_00000A72$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = xxHash3.Hash128Long_00000A72$BurstDirectCall.Pointer;
			}

			// Token: 0x06000AD9 RID: 2777 RVA: 0x000211A4 File Offset: 0x0001F3A4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				xxHash3.Hash128Long_00000A72$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000ADA RID: 2778 RVA: 0x000211BC File Offset: 0x0001F3BC
			public unsafe static void Constructor()
			{
				xxHash3.Hash128Long_00000A72$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(xxHash3.Hash128Long(byte*, byte*, long, byte*, ref uint4)).MethodHandle);
			}

			// Token: 0x06000ADB RID: 2779 RVA: 0x00002C47 File Offset: 0x00000E47
			public static void Initialize()
			{
			}

			// Token: 0x06000ADC RID: 2780 RVA: 0x000211CD File Offset: 0x0001F3CD
			// Note: this type is marked as 'beforefieldinit'.
			static Hash128Long_00000A72$BurstDirectCall()
			{
				xxHash3.Hash128Long_00000A72$BurstDirectCall.Constructor();
			}

			// Token: 0x06000ADD RID: 2781 RVA: 0x000211D4 File Offset: 0x0001F3D4
			public unsafe static void Invoke(byte* input, byte* dest, long length, byte* secret, out uint4 result)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = xxHash3.Hash128Long_00000A72$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Byte*,System.Byte*,System.Int64,System.Byte*,Unity.Mathematics.uint4&), input, dest, length, secret, ref result, functionPointer);
						return;
					}
				}
				xxHash3.Hash128Long$BurstManaged(input, dest, length, secret, out result);
			}

			// Token: 0x04000499 RID: 1177
			private static IntPtr Pointer;

			// Token: 0x0400049A RID: 1178
			private static IntPtr DeferredCompilation;
		}
	}
}
