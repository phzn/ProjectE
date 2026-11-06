using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Pathfinding.Collections
{
	// Token: 0x02000276 RID: 630
	public static class SpanExtensions
	{
		// Token: 0x06000FCB RID: 4043 RVA: 0x0006000B File Offset: 0x0005E20B
		public unsafe static void FillZeros<[IsUnmanaged] T>(this UnsafeSpan<T> span) where T : struct, ValueType
		{
			if (span.length > 0U)
			{
				UnsafeUtility.MemSet((void*)span.ptr, 0, (long)sizeof(T) * (long)((ulong)span.length));
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00060034 File Offset: 0x0005E234
		public unsafe static void Fill<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType
		{
			if (span.length > 0U)
			{
				if ((long)sizeof(T) * (long)((ulong)span.length) > 2147483647L)
				{
					throw new ArgumentException("Span is too large to fill");
				}
				UnsafeUtility.MemCpyReplicate((void*)span.ptr, (void*)(&value), sizeof(T), (int)span.length);
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00060088 File Offset: 0x0005E288
		public static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, NativeArray<T> array) where T : struct, ValueType
		{
			array.AsUnsafeReadOnlySpan<T>().CopyTo(span);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000600A4 File Offset: 0x0005E2A4
		public static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, UnsafeSpan<T> other) where T : struct, ValueType
		{
			other.CopyTo(span);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x000600B0 File Offset: 0x0005E2B0
		public unsafe static void CopyFrom<[IsUnmanaged] T>(this UnsafeSpan<T> span, T[] array) where T : struct, ValueType
		{
			if (array.Length > span.Length)
			{
				throw new InvalidOperationException();
			}
			if (array.Length == 0)
			{
				return;
			}
			ulong num;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num);
			UnsafeUtility.MemCpy((void*)span.ptr, ptr, (long)sizeof(T) * (long)array.Length);
			UnsafeUtility.ReleaseGCObject(num);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x000600FC File Offset: 0x0005E2FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this UnsafeAppendBuffer buffer) where T : struct, ValueType
		{
			int num = buffer.Length / UnsafeUtility.SizeOf<T>();
			if (num * UnsafeUtility.SizeOf<T>() != buffer.Length)
			{
				throw new ArgumentException("Buffer length is not a multiple of the element size");
			}
			return new UnsafeSpan<T>((void*)buffer.Ptr, num, Allocator.None);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0006013D File Offset: 0x0005E33D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			return new UnsafeSpan<T>((void*)list.GetUnsafePtr<T>(), list.Length, Allocator.None);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00060152 File Offset: 0x0005E352
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(arr.GetUnsafePtr<T>(), arr.Length, Allocator.None);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00060167 File Offset: 0x0005E367
		public static UnsafeSpan<T> MoveToUnsafeSpan<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(arr.GetUnsafePtr<T>(), arr.Length, arr.GetAllocator<T>());
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00060181 File Offset: 0x0005E381
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpanNoChecks<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(arr), arr.Length, Allocator.None);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00060196 File Offset: 0x0005E396
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeReadOnlySpan<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(arr.GetUnsafeReadOnlyPtr<T>(), arr.Length, Allocator.None);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000601AC File Offset: 0x0005E3AC
		public unsafe static Allocator GetAllocator<[IsUnmanaged] T>(this NativeArray<T> arr) where T : struct, ValueType
		{
			byte* ptr = (byte*)UnsafeUtility.AddressOf<NativeArray<T>>(ref arr);
			return (Allocator)(*(int*)(ptr + SpanExtensions.AllocatorOffset));
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000601C9 File Offset: 0x0005E3C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this UnsafeList<T> arr) where T : struct, ValueType
		{
			return new UnsafeSpan<T>((void*)arr.Ptr, arr.Length, Allocator.None);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000601DE File Offset: 0x0005E3DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UnsafeSpan<T> AsUnsafeSpan<[IsUnmanaged] T>(this NativeSlice<T> slice) where T : struct, ValueType
		{
			return new UnsafeSpan<T>(slice.GetUnsafePtr<T>(), slice.Length, Allocator.None);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x000601F3 File Offset: 0x0005E3F3
		public static bool Contains<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType, IEquatable<T>
		{
			return span.IndexOf(value) != -1;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00060202 File Offset: 0x0005E402
		public unsafe static int IndexOf<[IsUnmanaged] T>(this UnsafeSpan<T> span, T value) where T : struct, ValueType, IEquatable<T>
		{
			return new ReadOnlySpan<T>((void*)span.ptr, (int)span.length).IndexOf(value);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0006021B File Offset: 0x0005E41B
		public static void Sort<[IsUnmanaged] T>(this UnsafeSpan<T> span) where T : struct, ValueType, IComparable<T>
		{
			NativeSortExtension.Sort<T>(span.ptr, span.Length);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0006022F File Offset: 0x0005E42F
		public static void Sort<[IsUnmanaged] T, U>(this UnsafeSpan<T> span, U comp) where T : struct, ValueType where U : IComparer<T>
		{
			NativeSortExtension.Sort<T, U>(span.ptr, span.Length, comp);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00060244 File Offset: 0x0005E444
		public static void InsertRange<[IsUnmanaged] T>(this NativeList<T> list, int index, int count) where T : struct, ValueType
		{
			list.ResizeUninitialized(list.Length + count);
			list.AsUnsafeSpan<T>().Move(index, index + count, list.Length - (index + count));
		}

		// Token: 0x04000B9D RID: 2973
		private static readonly int AllocatorOffset = UnsafeUtility.GetFieldOffset(typeof(NativeArray<int>).GetField("m_AllocatorLabel", BindingFlags.Instance | BindingFlags.NonPublic));
	}
}
