using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000067 RID: 103
	[Serializable]
	public struct WeightedTransformArray : IList<WeightedTransform>, ICollection<WeightedTransform>, IEnumerable<WeightedTransform>, IEnumerable, IList, ICollection
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x000090F0 File Offset: 0x000072F0
		public WeightedTransformArray(int size)
		{
			this.m_Length = WeightedTransformArray.ClampSize(size);
			this.m_Item0 = default(WeightedTransform);
			this.m_Item1 = default(WeightedTransform);
			this.m_Item2 = default(WeightedTransform);
			this.m_Item3 = default(WeightedTransform);
			this.m_Item4 = default(WeightedTransform);
			this.m_Item5 = default(WeightedTransform);
			this.m_Item6 = default(WeightedTransform);
			this.m_Item7 = default(WeightedTransform);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00009169 File Offset: 0x00007369
		public IEnumerator<WeightedTransform> GetEnumerator()
		{
			return new WeightedTransformArray.Enumerator(ref this);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009176 File Offset: 0x00007376
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new WeightedTransformArray.Enumerator(ref this);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00009183 File Offset: 0x00007383
		int IList.Add(object value)
		{
			this.Add((WeightedTransform)value);
			return this.m_Length - 1;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000919C File Offset: 0x0000739C
		public void Add(WeightedTransform value)
		{
			if (this.m_Length >= WeightedTransformArray.k_MaxLength)
			{
				throw new ArgumentException(string.Format("This array cannot have more than '{0}' items.", WeightedTransformArray.k_MaxLength));
			}
			this.Set(this.m_Length, value);
			this.m_Length++;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000091EB File Offset: 0x000073EB
		public void Clear()
		{
			this.m_Length = 0;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000091F4 File Offset: 0x000073F4
		int IList.IndexOf(object value)
		{
			return this.IndexOf((WeightedTransform)value);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00009204 File Offset: 0x00007404
		public int IndexOf(WeightedTransform value)
		{
			for (int i = 0; i < this.m_Length; i++)
			{
				if (this.Get(i).Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00009237 File Offset: 0x00007437
		bool IList.Contains(object value)
		{
			return this.Contains((WeightedTransform)value);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00009248 File Offset: 0x00007448
		public bool Contains(WeightedTransform value)
		{
			for (int i = 0; i < this.m_Length; i++)
			{
				if (this.Get(i).Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000927C File Offset: 0x0000747C
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("The array cannot be null.");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
			}
			if (this.Count > array.Length - arrayIndex + 1)
			{
				throw new ArgumentException("The destination array has fewer elements than the collection.");
			}
			for (int i = 0; i < this.m_Length; i++)
			{
				array.SetValue(this.Get(i), i + arrayIndex);
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000092EC File Offset: 0x000074EC
		public void CopyTo(WeightedTransform[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("The array cannot be null.");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
			}
			if (this.Count > array.Length - arrayIndex + 1)
			{
				throw new ArgumentException("The destination array has fewer elements than the collection.");
			}
			for (int i = 0; i < this.m_Length; i++)
			{
				array[i + arrayIndex] = this.Get(i);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00009351 File Offset: 0x00007551
		void IList.Remove(object value)
		{
			this.Remove((WeightedTransform)value);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00009360 File Offset: 0x00007560
		public bool Remove(WeightedTransform value)
		{
			for (int i = 0; i < this.m_Length; i++)
			{
				if (this.Get(i).Equals(value))
				{
					while (i < this.m_Length - 1)
					{
						this.Set(i, this.Get(i + 1));
						i++;
					}
					this.m_Length--;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000093C4 File Offset: 0x000075C4
		public void RemoveAt(int index)
		{
			this.CheckOutOfRangeIndex(index);
			for (int i = index; i < this.m_Length - 1; i++)
			{
				this.Set(i, this.Get(i + 1));
			}
			this.m_Length--;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00009409 File Offset: 0x00007609
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (WeightedTransform)value);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00009418 File Offset: 0x00007618
		public void Insert(int index, WeightedTransform value)
		{
			if (this.m_Length >= WeightedTransformArray.k_MaxLength)
			{
				throw new ArgumentException(string.Format("This array cannot have more than '{0}' items.", WeightedTransformArray.k_MaxLength));
			}
			this.CheckOutOfRangeIndex(index);
			if (index >= this.m_Length)
			{
				this.Add(value);
				return;
			}
			for (int i = this.m_Length; i > index; i--)
			{
				this.Set(i, this.Get(i - 1));
			}
			this.Set(index, value);
			this.m_Length++;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000949B File Offset: 0x0000769B
		private static int ClampSize(int size)
		{
			return Mathf.Clamp(size, 0, WeightedTransformArray.k_MaxLength);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000094A9 File Offset: 0x000076A9
		private void CheckOutOfRangeIndex(int index)
		{
			if (index < 0 || index >= WeightedTransformArray.k_MaxLength)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.m_Length));
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000094D8 File Offset: 0x000076D8
		private WeightedTransform Get(int index)
		{
			this.CheckOutOfRangeIndex(index);
			switch (index)
			{
			case 0:
				return this.m_Item0;
			case 1:
				return this.m_Item1;
			case 2:
				return this.m_Item2;
			case 3:
				return this.m_Item3;
			case 4:
				return this.m_Item4;
			case 5:
				return this.m_Item5;
			case 6:
				return this.m_Item6;
			case 7:
				return this.m_Item7;
			default:
				return this.m_Item0;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00009554 File Offset: 0x00007754
		private void Set(int index, WeightedTransform value)
		{
			this.CheckOutOfRangeIndex(index);
			switch (index)
			{
			case 0:
				this.m_Item0 = value;
				return;
			case 1:
				this.m_Item1 = value;
				return;
			case 2:
				this.m_Item2 = value;
				return;
			case 3:
				this.m_Item3 = value;
				return;
			case 4:
				this.m_Item4 = value;
				return;
			case 5:
				this.m_Item5 = value;
				return;
			case 6:
				this.m_Item6 = value;
				return;
			case 7:
				this.m_Item7 = value;
				return;
			default:
				return;
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000095D0 File Offset: 0x000077D0
		public void SetWeight(int index, float weight)
		{
			WeightedTransform weightedTransform = this.Get(index);
			weightedTransform.weight = weight;
			this.Set(index, weightedTransform);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000095F5 File Offset: 0x000077F5
		public float GetWeight(int index)
		{
			return this.Get(index).weight;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009604 File Offset: 0x00007804
		public void SetTransform(int index, Transform transform)
		{
			WeightedTransform weightedTransform = this.Get(index);
			weightedTransform.transform = transform;
			this.Set(index, weightedTransform);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009629 File Offset: 0x00007829
		public Transform GetTransform(int index)
		{
			return this.Get(index).transform;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009638 File Offset: 0x00007838
		public static void OnValidate(ref WeightedTransformArray array, float min = 0f, float max = 1f)
		{
			for (int i = 0; i < WeightedTransformArray.k_MaxLength; i++)
			{
				array.SetWeight(i, Mathf.Clamp(array.GetWeight(i), min, max));
			}
		}

		// Token: 0x17000111 RID: 273
		object IList.this[int index]
		{
			get
			{
				return this.Get(index);
			}
			set
			{
				this.Set(index, (WeightedTransform)value);
			}
		}

		// Token: 0x17000112 RID: 274
		public WeightedTransform this[int index]
		{
			get
			{
				return this.Get(index);
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000969A File Offset: 0x0000789A
		public int Count
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000096A2 File Offset: 0x000078A2
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x000096A5 File Offset: 0x000078A5
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x000096A8 File Offset: 0x000078A8
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x000096AB File Offset: 0x000078AB
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000118 RID: 280
		public static readonly int k_MaxLength = 8;

		// Token: 0x04000119 RID: 281
		[SerializeField]
		[NotKeyable]
		private int m_Length;

		// Token: 0x0400011A RID: 282
		[SerializeField]
		private WeightedTransform m_Item0;

		// Token: 0x0400011B RID: 283
		[SerializeField]
		private WeightedTransform m_Item1;

		// Token: 0x0400011C RID: 284
		[SerializeField]
		private WeightedTransform m_Item2;

		// Token: 0x0400011D RID: 285
		[SerializeField]
		private WeightedTransform m_Item3;

		// Token: 0x0400011E RID: 286
		[SerializeField]
		private WeightedTransform m_Item4;

		// Token: 0x0400011F RID: 287
		[SerializeField]
		private WeightedTransform m_Item5;

		// Token: 0x04000120 RID: 288
		[SerializeField]
		private WeightedTransform m_Item6;

		// Token: 0x04000121 RID: 289
		[SerializeField]
		private WeightedTransform m_Item7;

		// Token: 0x02000088 RID: 136
		[Serializable]
		private struct Enumerator : IEnumerator<WeightedTransform>, IEnumerator, IDisposable
		{
			// Token: 0x0600032A RID: 810 RVA: 0x0000A7D5 File Offset: 0x000089D5
			public Enumerator(ref WeightedTransformArray array)
			{
				this.m_Array = array;
				this.m_Index = -1;
			}

			// Token: 0x0600032B RID: 811 RVA: 0x0000A7EA File Offset: 0x000089EA
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_Array.Count;
			}

			// Token: 0x0600032C RID: 812 RVA: 0x0000A80D File Offset: 0x00008A0D
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x0600032D RID: 813 RVA: 0x0000A816 File Offset: 0x00008A16
			void IDisposable.Dispose()
			{
			}

			// Token: 0x17000120 RID: 288
			// (get) Token: 0x0600032E RID: 814 RVA: 0x0000A818 File Offset: 0x00008A18
			public WeightedTransform Current
			{
				get
				{
					return this.m_Array.Get(this.m_Index);
				}
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x0600032F RID: 815 RVA: 0x0000A82B File Offset: 0x00008A2B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000177 RID: 375
			private WeightedTransformArray m_Array;

			// Token: 0x04000178 RID: 376
			private int m_Index;
		}
	}
}
