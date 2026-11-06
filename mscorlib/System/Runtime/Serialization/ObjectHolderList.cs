using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000666 RID: 1638
	internal class ObjectHolderList
	{
		// Token: 0x06003D48 RID: 15688 RVA: 0x000D4572 File Offset: 0x000D2772
		internal ObjectHolderList()
			: this(8)
		{
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x000D457B File Offset: 0x000D277B
		internal ObjectHolderList(int startingSize)
		{
			this.m_count = 0;
			this.m_values = new ObjectHolder[startingSize];
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x000D4598 File Offset: 0x000D2798
		internal virtual void Add(ObjectHolder value)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			ObjectHolder[] values = this.m_values;
			int count = this.m_count;
			this.m_count = count + 1;
			values[count] = value;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x000D45D4 File Offset: 0x000D27D4
		internal ObjectHolderListEnumerator GetFixupEnumerator()
		{
			return new ObjectHolderListEnumerator(this, true);
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x000D45E0 File Offset: 0x000D27E0
		private void EnlargeArray()
		{
			int num = this.m_values.Length * 2;
			if (num < 0)
			{
				if (num == 2147483647)
				{
					throw new SerializationException(Environment.GetResourceString("The internal array cannot expand to greater than Int32.MaxValue elements."));
				}
				num = int.MaxValue;
			}
			ObjectHolder[] array = new ObjectHolder[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x000D463A File Offset: 0x000D283A
		internal int Version
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x000D463A File Offset: 0x000D283A
		internal int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x04002771 RID: 10097
		internal const int DefaultInitialSize = 8;

		// Token: 0x04002772 RID: 10098
		internal ObjectHolder[] m_values;

		// Token: 0x04002773 RID: 10099
		internal int m_count;
	}
}
