using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000146 RID: 326
	public struct ReadOnlyArray<TValue> : IReadOnlyList<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x000558B3 File Offset: 0x00053AB3
		public ReadOnlyArray(TValue[] array)
		{
			this.m_Array = array;
			this.m_StartIndex = 0;
			this.m_Length = ((array != null) ? array.Length : 0);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x000558D2 File Offset: 0x00053AD2
		public ReadOnlyArray(TValue[] array, int index, int length)
		{
			this.m_Array = array;
			this.m_StartIndex = index;
			this.m_Length = length;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000558EC File Offset: 0x00053AEC
		public TValue[] ToArray()
		{
			TValue[] array = new TValue[this.m_Length];
			if (this.m_Length > 0)
			{
				Array.Copy(this.m_Array, this.m_StartIndex, array, 0, this.m_Length);
			}
			return array;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00055928 File Offset: 0x00053B28
		public int IndexOf(Predicate<TValue> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			for (int i = 0; i < this.m_Length; i++)
			{
				if (predicate(this.m_Array[this.m_StartIndex + i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00055972 File Offset: 0x00053B72
		public ReadOnlyArray<TValue>.Enumerator GetEnumerator()
		{
			return new ReadOnlyArray<TValue>.Enumerator(this.m_Array, this.m_StartIndex, this.m_Length);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0005598B File Offset: 0x00053B8B
		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00055998 File Offset: 0x00053B98
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000559A5 File Offset: 0x00053BA5
		public static implicit operator ReadOnlyArray<TValue>(TValue[] array)
		{
			return new ReadOnlyArray<TValue>(array);
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x000559AD File Offset: 0x00053BAD
		public int Count
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x170004E4 RID: 1252
		public TValue this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (this.m_Array == null)
				{
					throw new InvalidOperationException();
				}
				return this.m_Array[this.m_StartIndex + index];
			}
		}

		// Token: 0x04000736 RID: 1846
		internal TValue[] m_Array;

		// Token: 0x04000737 RID: 1847
		internal int m_StartIndex;

		// Token: 0x04000738 RID: 1848
		internal int m_Length;

		// Token: 0x02000252 RID: 594
		public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x06001661 RID: 5729 RVA: 0x00066C1A File Offset: 0x00064E1A
			internal Enumerator(TValue[] array, int index, int length)
			{
				this.m_Array = array;
				this.m_IndexStart = index - 1;
				this.m_IndexEnd = index + length;
				this.m_Index = this.m_IndexStart;
			}

			// Token: 0x06001662 RID: 5730 RVA: 0x00066C41 File Offset: 0x00064E41
			public void Dispose()
			{
			}

			// Token: 0x06001663 RID: 5731 RVA: 0x00066C43 File Offset: 0x00064E43
			public bool MoveNext()
			{
				if (this.m_Index < this.m_IndexEnd)
				{
					this.m_Index++;
				}
				return this.m_Index != this.m_IndexEnd;
			}

			// Token: 0x06001664 RID: 5732 RVA: 0x00066C72 File Offset: 0x00064E72
			public void Reset()
			{
				this.m_Index = this.m_IndexStart;
			}

			// Token: 0x17000604 RID: 1540
			// (get) Token: 0x06001665 RID: 5733 RVA: 0x00066C80 File Offset: 0x00064E80
			public TValue Current
			{
				get
				{
					if (this.m_Index == this.m_IndexEnd)
					{
						throw new InvalidOperationException("Iterated beyond end");
					}
					return this.m_Array[this.m_Index];
				}
			}

			// Token: 0x17000605 RID: 1541
			// (get) Token: 0x06001666 RID: 5734 RVA: 0x00066CAC File Offset: 0x00064EAC
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000C9E RID: 3230
			private readonly TValue[] m_Array;

			// Token: 0x04000C9F RID: 3231
			private readonly int m_IndexStart;

			// Token: 0x04000CA0 RID: 3232
			private readonly int m_IndexEnd;

			// Token: 0x04000CA1 RID: 3233
			private int m_Index;
		}
	}
}
