using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x0200003D RID: 61
	[GenerateTestsForBurstCompatibility]
	public static class CollectionHelper
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00004DE2 File Offset: 0x00002FE2
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckAllocator(AllocatorManager.AllocatorHandle allocator)
		{
			if (!CollectionHelper.ShouldDeallocate(allocator))
			{
				throw new ArgumentException(string.Format("Allocator {0} must not be None or Invalid", allocator));
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004E02 File Offset: 0x00003002
		public static int Log2Floor(int value)
		{
			return 31 - math.lzcnt((uint)value);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00004E0D File Offset: 0x0000300D
		public static int Log2Ceil(int value)
		{
			return 32 - math.lzcnt((uint)(value - 1));
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004E1A File Offset: 0x0000301A
		public static int Align(int size, int alignmentPowerOfTwo)
		{
			if (alignmentPowerOfTwo == 0)
			{
				return size;
			}
			return (size + alignmentPowerOfTwo - 1) & ~(alignmentPowerOfTwo - 1);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00004E2B File Offset: 0x0000302B
		public static ulong Align(ulong size, ulong alignmentPowerOfTwo)
		{
			if (alignmentPowerOfTwo == 0UL)
			{
				return size;
			}
			return (size + alignmentPowerOfTwo - 1UL) & ~(alignmentPowerOfTwo - 1UL);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004E40 File Offset: 0x00003040
		internal unsafe static void* AlignPointer(void* ptr, int alignmentPowerOfTwo)
		{
			if (alignmentPowerOfTwo == 0)
			{
				return ptr;
			}
			UIntPtr uintPtr = (UIntPtr)((IntPtr)alignmentPowerOfTwo);
			return (void*)(((byte*)((byte*)ptr + uintPtr) - 1) & ~(uintPtr - 1));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004E61 File Offset: 0x00003061
		public unsafe static bool IsAligned(void* p, int alignmentPowerOfTwo)
		{
			return ((byte*)p & ((byte*)((long)alignmentPowerOfTwo) - 1L)) == null;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004E6F File Offset: 0x0000306F
		public static bool IsAligned(ulong offset, int alignmentPowerOfTwo)
		{
			return (offset & (ulong)((long)alignmentPowerOfTwo - 1L)) == 0UL;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004E7C File Offset: 0x0000307C
		public static bool IsPowerOfTwo(int value)
		{
			return (value & (value - 1)) == 0;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004E88 File Offset: 0x00003088
		public unsafe static uint Hash(void* ptr, int bytes)
		{
			ulong num = 5381UL;
			while (bytes > 0)
			{
				int num2 = --bytes;
				ulong num3 = (ulong)((byte*)ptr)[num2];
				num = (num << 5) + num + num3;
			}
			return (uint)num;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00004EBC File Offset: 0x000030BC
		[ExcludeFromBurstCompatTesting("Used only for debugging, and uses managed strings")]
		internal static void WriteLayout(Type type)
		{
			Console.WriteLine(string.Format("   Offset | Bytes  | Name     Layout: {0}", 0), type.Name);
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Console.WriteLine("   {0, 6} | {1, 6} | {2}", Marshal.OffsetOf(type, fieldInfo.Name), Marshal.SizeOf(fieldInfo.FieldType), fieldInfo.Name);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004F30 File Offset: 0x00003130
		internal static bool ShouldDeallocate(AllocatorManager.AllocatorHandle allocator)
		{
			return allocator.ToAllocator > Allocator.None;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004F3C File Offset: 0x0000313C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: AssumeRange(0L, 2147483647L)]
		internal static int AssumePositive(int value)
		{
			return value;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00004F3F File Offset: 0x0000313F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(NativeArray<int>) })]
		internal static void CheckIsUnmanaged<T>()
		{
			if (!UnsafeUtility.IsUnmanaged<T>())
			{
				throw new ArgumentException(string.Format("{0} used in native collection is not blittable or not primitive", typeof(T)));
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00004F62 File Offset: 0x00003162
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckIntPositivePowerOfTwo(int value)
		{
			if (value <= 0 || (value & (value - 1)) != 0)
			{
				throw new ArgumentException(string.Format("Alignment requested: {0} is not a non-zero, positive power of two.", value));
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004F8B File Offset: 0x0000318B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckUlongPositivePowerOfTwo(ulong value)
		{
			if (value <= 0UL || (value & (value - 1UL)) != 0UL)
			{
				throw new ArgumentException(string.Format("Alignment requested: {0} is not a non-zero, positive power of two.", value));
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004FB7 File Offset: 0x000031B7
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CheckIndexInRange(int index, int length)
		{
			if (index >= length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in container of '{1}' Length.", index, length));
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00004FD9 File Offset: 0x000031D9
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckCapacityInRange(int capacity, int length)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be positive.", capacity));
			}
			if (capacity < length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} is out of range in container of '{1}' Length.", capacity, length));
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005018 File Offset: 0x00003218
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(AllocatorManager.AllocatorHandle)
		})]
		public static NativeArray<T> CreateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(int length, ref U allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType where U : struct, ValueType, AllocatorManager.IAllocator
		{
			NativeArray<T> nativeArray;
			if (!allocator.IsCustomAllocator)
			{
				nativeArray = new NativeArray<T>(length, allocator.ToAllocator, options);
			}
			else
			{
				nativeArray = default(NativeArray<T>);
				(ref nativeArray).Initialize<T, U>(length, ref allocator, options);
			}
			return nativeArray;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005060 File Offset: 0x00003260
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static NativeArray<T> CreateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int length, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			NativeArray<T> nativeArray;
			if (!AllocatorManager.IsCustomAllocator(allocator))
			{
				nativeArray = new NativeArray<T>(length, allocator.ToAllocator, options);
			}
			else
			{
				nativeArray = default(NativeArray<T>);
				(ref nativeArray).Initialize<T>(length, allocator, options);
			}
			return nativeArray;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000509C File Offset: 0x0000329C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static NativeArray<T> CreateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> array, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeArray<T> nativeArray;
			if (!AllocatorManager.IsCustomAllocator(allocator))
			{
				nativeArray = new NativeArray<T>(array, allocator.ToAllocator);
			}
			else
			{
				nativeArray = default(NativeArray<T>);
				(ref nativeArray).Initialize<T>(array.Length, allocator, NativeArrayOptions.ClearMemory);
				nativeArray.CopyFrom(array);
			}
			return nativeArray;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000050E4 File Offset: 0x000032E4
		[ExcludeFromBurstCompatTesting("Managed array")]
		public static NativeArray<T> CreateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T[] array, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			NativeArray<T> nativeArray;
			if (!AllocatorManager.IsCustomAllocator(allocator))
			{
				nativeArray = new NativeArray<T>(array, allocator.ToAllocator);
			}
			else
			{
				nativeArray = default(NativeArray<T>);
				(ref nativeArray).Initialize<T>(array.Length, allocator, NativeArrayOptions.ClearMemory);
				nativeArray.CopyFrom(array);
			}
			return nativeArray;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005128 File Offset: 0x00003328
		[ExcludeFromBurstCompatTesting("Managed array")]
		public static NativeArray<T> CreateNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(T[] array, ref U allocator) where T : struct, ValueType where U : struct, ValueType, AllocatorManager.IAllocator
		{
			NativeArray<T> nativeArray;
			if (!allocator.IsCustomAllocator)
			{
				nativeArray = new NativeArray<T>(array, allocator.ToAllocator);
			}
			else
			{
				nativeArray = default(NativeArray<T>);
				(ref nativeArray).Initialize<T, U>(array.Length, ref allocator, NativeArrayOptions.ClearMemory);
				nativeArray.CopyFrom(array);
			}
			return nativeArray;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005176 File Offset: 0x00003376
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void DisposeNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> nativeArray, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			(ref nativeArray).DisposeCheckAllocator<T>();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005176 File Offset: 0x00003376
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void Dispose<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> nativeArray) where T : struct, ValueType
		{
			(ref nativeArray).DisposeCheckAllocator<T>();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005180 File Offset: 0x00003380
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckConvertArguments<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int length) where T : struct, ValueType
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
			}
			if (!UnsafeUtility.IsUnmanaged<T>())
			{
				throw new InvalidOperationException(string.Format("{0} used in NativeArray<{1}> must be unmanaged (contain no managed types).", typeof(T), typeof(T)));
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000051CC File Offset: 0x000033CC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static NativeArray<T> ConvertExistingDataToNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(void* dataPointer, int length, AllocatorManager.AllocatorHandle allocator, bool setTempMemoryHandle = false) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = default(NativeArray<T>);
			nativeArray.m_Buffer = dataPointer;
			nativeArray.m_Length = length;
			if (!allocator.IsCustomAllocator)
			{
				nativeArray.m_AllocatorLabel = allocator.ToAllocator;
			}
			else
			{
				nativeArray.m_AllocatorLabel = Allocator.None;
			}
			return nativeArray;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005213 File Offset: 0x00003413
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe static NativeArray<T> ConvertExistingNativeListToNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ref NativeList<T> nativeList, int length, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			return CollectionHelper.ConvertExistingDataToNativeArray<T>((void*)nativeList.GetUnsafePtr<T>(), length, allocator, false);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005228 File Offset: 0x00003428
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int),
			typeof(AllocatorManager.AllocatorHandle)
		})]
		public static NativeParallelMultiHashMap<TKey, TValue> CreateNativeParallelMultiHashMap<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(int length, ref U allocator) where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType where U : struct, ValueType, AllocatorManager.IAllocator
		{
			NativeParallelMultiHashMap<TKey, TValue> nativeParallelMultiHashMap = default(NativeParallelMultiHashMap<TKey, TValue>);
			nativeParallelMultiHashMap.Initialize<U>(length, ref allocator);
			return nativeParallelMultiHashMap;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00002C47 File Offset: 0x00000E47
		[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(CollectionHelper.DummyJob) })]
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public static void CheckReflectionDataCorrect<T>(IntPtr reflectionData)
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005247 File Offset: 0x00003447
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[BurstDiscard]
		private static void CheckReflectionDataCorrectInternal<T>(IntPtr reflectionData, ref bool burstCompiled)
		{
			if (reflectionData == IntPtr.Zero)
			{
				throw new InvalidOperationException(string.Format("Reflection data was not set up by an Initialize() call. For generic job types, please include [assembly: RegisterGenericJobType(typeof({0}))] in your source file.", typeof(T)));
			}
			burstCompiled = false;
		}

		// Token: 0x04000089 RID: 137
		public const int CacheLineSize = 64;

		// Token: 0x0200003E RID: 62
		[StructLayout(LayoutKind.Explicit)]
		internal struct LongDoubleUnion
		{
			// Token: 0x0400008A RID: 138
			[FieldOffset(0)]
			internal long longValue;

			// Token: 0x0400008B RID: 139
			[FieldOffset(0)]
			internal double doubleValue;
		}

		// Token: 0x0200003F RID: 63
		[BurstCompile]
		public struct DummyJob : IJob
		{
			// Token: 0x06000165 RID: 357 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Execute()
			{
			}
		}
	}
}
