using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x0200026B RID: 619
	public struct NativeCircularBuffer<[IsUnmanaged] T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : struct, ValueType
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0005EDF9 File Offset: 0x0005CFF9
		public readonly int Length
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0005EE01 File Offset: 0x0005D001
		public readonly int AbsoluteStartIndex
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0005EE09 File Offset: 0x0005D009
		public readonly int AbsoluteEndIndex
		{
			get
			{
				return this.head + this.length - 1;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0005EE1A File Offset: 0x0005D01A
		public unsafe readonly ref T First
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[(IntPtr)(this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0005EE38 File Offset: 0x0005D038
		public unsafe readonly ref T Last
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[(IntPtr)((this.head + this.length - 1) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0005EE5F File Offset: 0x0005D05F
		int IReadOnlyCollection<T>.Count
		{
			get
			{
				return this.Length;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0005EE67 File Offset: 0x0005D067
		public readonly bool IsCreated
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0005EE76 File Offset: 0x0005D076
		public NativeCircularBuffer(AllocatorManager.AllocatorHandle allocator)
		{
			this.data = null;
			this.Allocator = allocator;
			this.capacityMask = -1;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0005EE9C File Offset: 0x0005D09C
		public NativeCircularBuffer(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			initialCapacity = math.ceilpow2(initialCapacity);
			this.data = AllocatorManager.Allocate<T>(allocator, initialCapacity);
			this.capacityMask = initialCapacity - 1;
			this.Allocator = allocator;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0005EED1 File Offset: 0x0005D0D1
		public NativeCircularBuffer(CircularBuffer<T> buffer, out ulong gcHandle)
		{
			this = new NativeCircularBuffer<T>(buffer.data, buffer.head, buffer.Length, out gcHandle);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0005EEED File Offset: 0x0005D0ED
		public unsafe NativeCircularBuffer(T[] data, int head, int length, out ulong gcHandle)
		{
			this.data = (T*)UnsafeUtility.PinGCArrayAndGetDataAddress(data, out gcHandle);
			this.capacityMask = data.Length - 1;
			this.head = head;
			this.length = length;
			this.Allocator = Unity.Collections.Allocator.None;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0005EF22 File Offset: 0x0005D122
		public void Clear()
		{
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0005EF34 File Offset: 0x0005D134
		public void AddRange(List<T> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				this.PushEnd(items[i]);
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0005EF5F File Offset: 0x0005D15F
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushStart(T item)
		{
			if (this.length > this.capacityMask)
			{
				this.Grow();
			}
			this.length++;
			this.head--;
			this[0] = item;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0005EF99 File Offset: 0x0005D199
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushEnd(T item)
		{
			if (this.length > this.capacityMask)
			{
				this.Grow();
			}
			this.length++;
			this[this.length - 1] = item;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0005EFCC File Offset: 0x0005D1CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Push(bool toStart, T item)
		{
			if (toStart)
			{
				this.PushStart(item);
				return;
			}
			this.PushEnd(item);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0005EFE0 File Offset: 0x0005D1E0
		[IgnoredByDeepProfiler]
		public T PopStart()
		{
			T t = this[0];
			this.head++;
			this.length--;
			return t;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0005F005 File Offset: 0x0005D205
		[IgnoredByDeepProfiler]
		public T PopEnd()
		{
			T t = this[this.length - 1];
			this.length--;
			return t;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0005F023 File Offset: 0x0005D223
		public T Pop(bool fromStart)
		{
			if (fromStart)
			{
				return this.PopStart();
			}
			return this.PopEnd();
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0005F035 File Offset: 0x0005D235
		public readonly T GetBoundaryValue(bool start)
		{
			if (!start)
			{
				return this.GetAbsolute(this.AbsoluteEndIndex);
			}
			return this.GetAbsolute(this.AbsoluteStartIndex);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0005F053 File Offset: 0x0005D253
		public void TrimTo(int length)
		{
			this.length = math.min(this.length, length);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0005F067 File Offset: 0x0005D267
		public void Splice(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0005F07C File Offset: 0x0005D27C
		public unsafe void SpliceAbsolute(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex, toRemove, toInsert.Count);
			for (int i = 0; i < toInsert.Count; i++)
			{
				this.data[(IntPtr)((startIndex + i) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = toInsert[i];
			}
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0005F0CD File Offset: 0x0005D2CD
		public void SpliceUninitialized(int startIndex, int toRemove, int toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0005F0E0 File Offset: 0x0005D2E0
		public void SpliceUninitializedAbsolute(int startIndex, int toRemove, int toInsert)
		{
			int num = toInsert - toRemove;
			while (this.length + num > this.capacityMask + 1)
			{
				this.Grow();
			}
			this.MoveAbsolute(startIndex + toRemove, this.AbsoluteEndIndex, num);
			this.length += num;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0005F12C File Offset: 0x0005D32C
		private unsafe void MoveAbsolute(int startIndex, int endIndex, int deltaIndex)
		{
			if (deltaIndex > 0)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					this.data[(IntPtr)((i + deltaIndex) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = this.data[(IntPtr)(i & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
				return;
			}
			if (deltaIndex < 0)
			{
				for (int j = startIndex; j <= endIndex; j++)
				{
					this.data[(IntPtr)((j + deltaIndex) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = this.data[(IntPtr)(j & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
			}
		}

		// Token: 0x17000246 RID: 582
		public unsafe T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.data[(IntPtr)((index + this.head) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.data[(IntPtr)((index + this.head) & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			}
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005F219 File Offset: 0x0005D419
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly T GetAbsolute(int index)
		{
			return this.data[(IntPtr)(index & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0005F238 File Offset: 0x0005D438
		[MethodImpl(MethodImplOptions.NoInlining)]
		private unsafe void Grow()
		{
			int num = this.capacityMask + 1;
			int num2 = math.max(4, num * 2);
			T* ptr = AllocatorManager.Allocate<T>(this.Allocator, num2);
			if (this.data != null)
			{
				int num3 = num - (this.head & this.capacityMask);
				UnsafeUtility.MemCpy((void*)(ptr + (IntPtr)(this.head & (num2 - 1)) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)(this.data + (IntPtr)(this.head & this.capacityMask) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num3 * sizeof(T)));
				int num4 = this.length - num3;
				if (num4 > 0)
				{
					UnsafeUtility.MemCpy((void*)(ptr + (IntPtr)((this.head + num3) & (num2 - 1)) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.data, (long)(num4 * sizeof(T)));
				}
				AllocatorManager.Free<T>(this.Allocator, this.data, 1);
			}
			this.capacityMask = num2 - 1;
			this.data = ptr;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005F31C File Offset: 0x0005D51C
		public void Dispose()
		{
			this.capacityMask = -1;
			this.length = 0;
			this.head = 0;
			AllocatorManager.Free<T>(this.Allocator, this.data, 1);
			this.data = null;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0005F34D File Offset: 0x0005D54D
		public IEnumerator<T> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.length; i = num + 1)
			{
				yield return this[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0005F361 File Offset: 0x0005D561
		IEnumerator IEnumerable.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.length; i = num + 1)
			{
				yield return this[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0005F378 File Offset: 0x0005D578
		public unsafe NativeCircularBuffer<T> Clone()
		{
			if (!this.IsCreated)
			{
				return default(NativeCircularBuffer<T>);
			}
			T* ptr = AllocatorManager.Allocate<T>(this.Allocator, this.capacityMask + 1);
			UnsafeUtility.MemCpy((void*)ptr, (void*)this.data, (long)(this.length * sizeof(T)));
			return new NativeCircularBuffer<T>
			{
				data = ptr,
				head = this.head,
				length = this.length,
				capacityMask = this.capacityMask,
				Allocator = this.Allocator
			};
		}

		// Token: 0x04000B7D RID: 2941
		[NativeDisableUnsafePtrRestriction]
		internal unsafe T* data;

		// Token: 0x04000B7E RID: 2942
		internal int head;

		// Token: 0x04000B7F RID: 2943
		private int length;

		// Token: 0x04000B80 RID: 2944
		private int capacityMask;

		// Token: 0x04000B81 RID: 2945
		public AllocatorManager.AllocatorHandle Allocator;
	}
}
