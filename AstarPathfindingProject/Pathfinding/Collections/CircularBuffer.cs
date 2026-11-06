using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Pooling;
using Unity.Profiling;

namespace Pathfinding.Collections
{
	// Token: 0x02000264 RID: 612
	public struct CircularBuffer<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0005E14A File Offset: 0x0005C34A
		public readonly int Length
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0005E152 File Offset: 0x0005C352
		public readonly int AbsoluteStartIndex
		{
			get
			{
				return this.head;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0005E15A File Offset: 0x0005C35A
		public readonly int AbsoluteEndIndex
		{
			get
			{
				return this.head + this.length - 1;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0005E16B File Offset: 0x0005C36B
		public readonly ref T First
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[this.head & (this.data.Length - 1)];
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0005E189 File Offset: 0x0005C389
		public readonly ref T Last
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref this.data[(this.head + this.length - 1) & (this.data.Length - 1)];
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0005E14A File Offset: 0x0005C34A
		int IReadOnlyCollection<T>.Count
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.length;
			}
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0005E1B0 File Offset: 0x0005C3B0
		public CircularBuffer(int initialCapacity)
		{
			this.data = ArrayPool<T>.Claim(initialCapacity);
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0005E1CC File Offset: 0x0005C3CC
		public CircularBuffer(T[] backingArray)
		{
			this.data = backingArray;
			this.head = 0;
			this.length = 0;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0005E1E3 File Offset: 0x0005C3E3
		public void Clear()
		{
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0005E1F4 File Offset: 0x0005C3F4
		public void AddRange(List<T> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				this.PushEnd(items[i]);
			}
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0005E220 File Offset: 0x0005C420
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushStart(T item)
		{
			if (this.data == null || this.length >= this.data.Length)
			{
				this.Grow();
			}
			this.length++;
			this.head--;
			this[0] = item;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0005E26F File Offset: 0x0005C46F
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushEnd(T item)
		{
			if (this.data == null || this.length >= this.data.Length)
			{
				this.Grow();
			}
			this.length++;
			this[this.length - 1] = item;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0005E2AC File Offset: 0x0005C4AC
		[IgnoredByDeepProfiler]
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

		// Token: 0x06000F2A RID: 3882 RVA: 0x0005E2C0 File Offset: 0x0005C4C0
		[IgnoredByDeepProfiler]
		public T PopStart()
		{
			if (this.length == 0)
			{
				throw new InvalidOperationException();
			}
			T t = this[0];
			this.head++;
			this.length--;
			return t;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0005E2F3 File Offset: 0x0005C4F3
		[IgnoredByDeepProfiler]
		public T PopEnd()
		{
			if (this.length == 0)
			{
				throw new InvalidOperationException();
			}
			T t = this[this.length - 1];
			this.length--;
			return t;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0005E31F File Offset: 0x0005C51F
		[IgnoredByDeepProfiler]
		public T Pop(bool fromStart)
		{
			if (fromStart)
			{
				return this.PopStart();
			}
			return this.PopEnd();
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0005E331 File Offset: 0x0005C531
		public readonly T GetBoundaryValue(bool start)
		{
			return this.GetAbsolute(start ? this.AbsoluteStartIndex : this.AbsoluteEndIndex);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0005E34A File Offset: 0x0005C54A
		public void InsertAbsolute(int index, T item)
		{
			this.SpliceUninitializedAbsolute(index, 0, 1);
			this.data[index & (this.data.Length - 1)] = item;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0005E36D File Offset: 0x0005C56D
		public void Splice(int startIndex, int toRemove, List<T> toInsert)
		{
			this.SpliceAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0005E380 File Offset: 0x0005C580
		public void SpliceAbsolute(int startIndex, int toRemove, List<T> toInsert)
		{
			if (toInsert == null)
			{
				this.SpliceUninitializedAbsolute(startIndex, toRemove, 0);
				return;
			}
			this.SpliceUninitializedAbsolute(startIndex, toRemove, toInsert.Count);
			for (int i = 0; i < toInsert.Count; i++)
			{
				this.data[(startIndex + i) & (this.data.Length - 1)] = toInsert[i];
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0005E3D9 File Offset: 0x0005C5D9
		public void SpliceUninitialized(int startIndex, int toRemove, int toInsert)
		{
			this.SpliceUninitializedAbsolute(startIndex + this.head, toRemove, toInsert);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0005E3EC File Offset: 0x0005C5EC
		public void SpliceUninitializedAbsolute(int startIndex, int toRemove, int toInsert)
		{
			int num = toInsert - toRemove;
			while (this.length + num > this.data.Length)
			{
				this.Grow();
			}
			this.MoveAbsolute(startIndex + toRemove, this.AbsoluteEndIndex, num);
			this.length += num;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0005E438 File Offset: 0x0005C638
		private void MoveAbsolute(int startIndex, int endIndex, int deltaIndex)
		{
			if (deltaIndex > 0)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					this.data[(i + deltaIndex) & (this.data.Length - 1)] = this.data[i & (this.data.Length - 1)];
				}
				return;
			}
			if (deltaIndex < 0)
			{
				for (int j = startIndex; j <= endIndex; j++)
				{
					this.data[(j + deltaIndex) & (this.data.Length - 1)] = this.data[j & (this.data.Length - 1)];
				}
			}
		}

		// Token: 0x17000235 RID: 565
		public T this[int index]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.data[(index + this.head) & (this.data.Length - 1)];
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.data[(index + this.head) & (this.data.Length - 1)] = value;
			}
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0005E507 File Offset: 0x0005C707
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly T GetAbsolute(int index)
		{
			return this.data[index & (this.data.Length - 1)];
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0005E520 File Offset: 0x0005C720
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void SetAbsolute(int index, T value)
		{
			this.data[index & (this.data.Length - 1)] = value;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0005E53C File Offset: 0x0005C73C
		private void Grow()
		{
			T[] array = ArrayPool<T>.Claim(Math.Max(4, (this.data != null) ? (this.data.Length * 2) : 0));
			if (this.data != null)
			{
				int num = this.data.Length - (this.head & (this.data.Length - 1));
				Array.Copy(this.data, this.head & (this.data.Length - 1), array, this.head & (array.Length - 1), num);
				int num2 = this.length - num;
				if (num2 > 0)
				{
					Array.Copy(this.data, 0, array, (this.head + num) & (array.Length - 1), num2);
				}
				ArrayPool<T>.Release(ref this.data, false);
			}
			this.data = array;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0005E5F3 File Offset: 0x0005C7F3
		public void Pool()
		{
			ArrayPool<T>.Release(ref this.data, false);
			this.length = 0;
			this.head = 0;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0005E60F File Offset: 0x0005C80F
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

		// Token: 0x06000F3B RID: 3899 RVA: 0x0005E623 File Offset: 0x0005C823
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

		// Token: 0x06000F3C RID: 3900 RVA: 0x0005E638 File Offset: 0x0005C838
		public CircularBuffer<T> Clone()
		{
			return new CircularBuffer<T>
			{
				data = ((this.data != null) ? ((T[])this.data.Clone()) : null),
				length = this.length,
				head = this.head
			};
		}

		// Token: 0x04000B65 RID: 2917
		internal T[] data;

		// Token: 0x04000B66 RID: 2918
		internal int head;

		// Token: 0x04000B67 RID: 2919
		private int length;
	}
}
