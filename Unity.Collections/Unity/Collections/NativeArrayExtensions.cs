using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000087 RID: 135
	[GenerateTestsForBurstCompatibility]
	public static class NativeArrayExtensions
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x00016EF6 File Offset: 0x000150F6
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T> array, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>(array.GetUnsafeReadOnlyPtr<T>(), array.Length, value) != -1;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00016F11 File Offset: 0x00015111
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T> array, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>(array.GetUnsafeReadOnlyPtr<T>(), array.Length, value);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00016F26 File Offset: 0x00015126
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T>.ReadOnly array, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>(array.GetUnsafeReadOnlyPtr<T>(), array.m_Length, value) != -1;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00016F40 File Offset: 0x00015140
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(this NativeArray<T>.ReadOnly array, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>(array.GetUnsafeReadOnlyPtr<T>(), array.m_Length, value);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00016F54 File Offset: 0x00015154
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static bool Contains<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* ptr, int length, U value) where T : struct, ValueType, IEquatable<U>
		{
			return NativeArrayExtensions.IndexOf<T, U>(ptr, length, value) != -1;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00016F64 File Offset: 0x00015164
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public unsafe static int IndexOf<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U>(void* ptr, int length, U value) where T : struct, ValueType, IEquatable<U>
		{
			for (int num = 0; num != length; num++)
			{
				T t = UnsafeUtility.ReadArrayElement<T>(ptr, num);
				if (t.Equals(value))
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00016F98 File Offset: 0x00015198
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void CopyFrom<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			container.CopyFrom(other.AsArray());
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00016FA8 File Offset: 0x000151A8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void CopyFrom<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> container, in NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			NativeHashSet<T> nativeHashSet = other;
			using (NativeArray<T> nativeArray = nativeHashSet.ToNativeArray(Allocator.TempJob))
			{
				container.CopyFrom(nativeArray);
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00016FF4 File Offset: 0x000151F4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static void CopyFrom<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> container, in UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeHashSet<T> unsafeHashSet = other;
			using (NativeArray<T> nativeArray = unsafeHashSet.ToNativeArray(Allocator.TempJob))
			{
				container.CopyFrom(nativeArray);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00017040 File Offset: 0x00015240
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(int)
		})]
		public static NativeArray<U> Reinterpret<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(this NativeArray<T> array) where T : struct, ValueType where U : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			long num3 = (long)array.Length * (long)num / (long)num2;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<U>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(array), (int)num3, Allocator.None);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00017078 File Offset: 0x00015278
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public static bool ArraysEqual<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			if (container.Length != other.Length)
			{
				return false;
			}
			for (int num = 0; num != container.Length; num++)
			{
				T t = container[num];
				if (!t.Equals(other[num]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000170CC File Offset: 0x000152CC
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckReinterpretSize<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref NativeArray<T> array) where T : struct, ValueType where U : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			long num3 = (long)array.Length * (long)num;
			if (num3 / (long)num2 * (long)num2 != num3)
			{
				throw new InvalidOperationException(string.Format("Types {0} (array length {1}) and {2} cannot be aliased due to size constraints. The size of the types and lengths involved must line up.", typeof(T), array.Length, typeof(U)));
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001712C File Offset: 0x0001532C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal static void Initialize<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array, int length, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			AllocatorManager.AllocatorHandle allocatorHandle = allocator;
			array = default(NativeArray<T>);
			array.m_Buffer = (ref allocatorHandle).AllocateStruct(default(T), length);
			array.m_Length = length;
			array.m_AllocatorLabel = (allocator.IsAutoDispose ? Allocator.None : allocator.ToAllocator);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear(array.m_Buffer, (long)(array.m_Length * UnsafeUtility.SizeOf<T>()));
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00017198 File Offset: 0x00015398
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(int),
			typeof(AllocatorManager.AllocatorHandle)
		})]
		internal static void Initialize<[global::System.Runtime.CompilerServices.IsUnmanaged] T, [global::System.Runtime.CompilerServices.IsUnmanaged] U>(this NativeArray<T> array, int length, ref U allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType where U : struct, ValueType, AllocatorManager.IAllocator
		{
			array = default(NativeArray<T>);
			array.m_Buffer = (ref allocator).AllocateStruct(default(T), length);
			array.m_Length = length;
			array.m_AllocatorLabel = (allocator.IsAutoDispose ? Allocator.None : allocator.ToAllocator);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear(array.m_Buffer, (long)(array.m_Length * UnsafeUtility.SizeOf<T>()));
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00017208 File Offset: 0x00015408
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		internal static void DisposeCheckAllocator<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			if (array.m_Buffer == null)
			{
				throw new ObjectDisposedException("The NativeArray is already disposed.");
			}
			if (!AllocatorManager.IsCustomAllocator(array.m_AllocatorLabel))
			{
				array.Dispose();
				return;
			}
			AllocatorManager.Free(array.m_AllocatorLabel, array.m_Buffer);
			array.m_AllocatorLabel = Allocator.Invalid;
			array.m_Buffer = null;
		}

		// Token: 0x02000088 RID: 136
		public struct NativeArrayStaticId<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
		{
			// Token: 0x040003AE RID: 942
			internal static readonly SharedStatic<int> s_staticSafetyId = SharedStatic<int>.GetOrCreate<NativeArray<T>>(0U);
		}
	}
}
