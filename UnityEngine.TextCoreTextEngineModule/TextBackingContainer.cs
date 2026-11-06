using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000035 RID: 53
	internal struct TextBackingContainer
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0001D75C File Offset: 0x0001B95C
		public uint[] Text
		{
			get
			{
				return this.m_Array;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0001D774 File Offset: 0x0001B974
		public int Capacity
		{
			get
			{
				return this.m_Array.Length;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0001D790 File Offset: 0x0001B990
		// (set) Token: 0x06000166 RID: 358 RVA: 0x0001D7A8 File Offset: 0x0001B9A8
		public int Count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = value;
			}
		}

		// Token: 0x1700003B RID: 59
		public uint this[int index]
		{
			get
			{
				return this.m_Array[index];
			}
			set
			{
				bool flag = index >= this.m_Array.Length;
				if (flag)
				{
					this.Resize(index);
				}
				this.m_Array[index] = value;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001D801 File Offset: 0x0001BA01
		public TextBackingContainer(int size)
		{
			this.m_Array = new uint[size];
			this.m_Count = 0;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0001D817 File Offset: 0x0001BA17
		public void Resize(int size)
		{
			size = Mathf.NextPowerOfTwo(size + 1);
			Array.Resize<uint>(ref this.m_Array, size);
		}

		// Token: 0x0400025E RID: 606
		private uint[] m_Array;

		// Token: 0x0400025F RID: 607
		private int m_Count;
	}
}
