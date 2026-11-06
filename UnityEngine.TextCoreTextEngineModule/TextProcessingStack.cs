using System;
using System.Diagnostics;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000048 RID: 72
	[DebuggerDisplay("Item count = {m_Count}")]
	internal struct TextProcessingStack<T>
	{
		// Token: 0x060001FD RID: 509 RVA: 0x000224EB File Offset: 0x000206EB
		public TextProcessingStack(T[] stack)
		{
			this.itemStack = stack;
			this.m_Capacity = stack.Length;
			this.index = 0;
			this.m_RolloverSize = 0;
			this.m_DefaultItem = default(T);
			this.m_Count = 0;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0002251F File Offset: 0x0002071F
		public TextProcessingStack(int capacity)
		{
			this.itemStack = new T[capacity];
			this.m_Capacity = capacity;
			this.index = 0;
			this.m_RolloverSize = 0;
			this.m_DefaultItem = default(T);
			this.m_Count = 0;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00022556 File Offset: 0x00020756
		public TextProcessingStack(int capacity, int rolloverSize)
		{
			this.itemStack = new T[capacity];
			this.m_Capacity = capacity;
			this.index = 0;
			this.m_RolloverSize = rolloverSize;
			this.m_DefaultItem = default(T);
			this.m_Count = 0;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00022590 File Offset: 0x00020790
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000225A8 File Offset: 0x000207A8
		public T current
		{
			get
			{
				bool flag = this.index > 0;
				T t;
				if (flag)
				{
					t = this.itemStack[this.index - 1];
				}
				else
				{
					t = this.itemStack[0];
				}
				return t;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000225EC File Offset: 0x000207EC
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00022604 File Offset: 0x00020804
		public int rolloverSize
		{
			get
			{
				return this.m_RolloverSize;
			}
			set
			{
				this.m_RolloverSize = value;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00022610 File Offset: 0x00020810
		internal static void SetDefault(TextProcessingStack<T>[] stack, T item)
		{
			for (int i = 0; i < stack.Length; i++)
			{
				stack[i].SetDefault(item);
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0002263E File Offset: 0x0002083E
		public void Clear()
		{
			this.index = 0;
			this.m_Count = 0;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00022650 File Offset: 0x00020850
		public void SetDefault(T item)
		{
			bool flag = this.itemStack == null;
			if (flag)
			{
				this.m_Capacity = 4;
				this.itemStack = new T[this.m_Capacity];
				this.m_DefaultItem = default(T);
			}
			this.itemStack[0] = item;
			this.index = 1;
			this.m_Count = 1;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000226AC File Offset: 0x000208AC
		public void Add(T item)
		{
			bool flag = this.index < this.itemStack.Length;
			if (flag)
			{
				this.itemStack[this.index] = item;
				this.index++;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000226F0 File Offset: 0x000208F0
		public T Remove()
		{
			this.index--;
			this.m_Count--;
			bool flag = this.index <= 0;
			T t;
			if (flag)
			{
				this.m_Count = 0;
				this.index = 1;
				t = this.itemStack[0];
			}
			else
			{
				t = this.itemStack[this.index - 1];
			}
			return t;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00022760 File Offset: 0x00020960
		public void Push(T item)
		{
			bool flag = this.index == this.m_Capacity;
			if (flag)
			{
				this.m_Capacity *= 2;
				bool flag2 = this.m_Capacity == 0;
				if (flag2)
				{
					this.m_Capacity = 4;
				}
				Array.Resize<T>(ref this.itemStack, this.m_Capacity);
			}
			this.itemStack[this.index] = item;
			bool flag3 = this.m_RolloverSize == 0;
			if (flag3)
			{
				this.index++;
				this.m_Count++;
			}
			else
			{
				this.index = (this.index + 1) % this.m_RolloverSize;
				this.m_Count = ((this.m_Count < this.m_RolloverSize) ? (this.m_Count + 1) : this.m_RolloverSize);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00022830 File Offset: 0x00020A30
		public T Pop()
		{
			bool flag = this.index == 0 && this.m_RolloverSize == 0;
			T t;
			if (flag)
			{
				t = default(T);
			}
			else
			{
				bool flag2 = this.m_RolloverSize == 0;
				if (flag2)
				{
					this.index--;
				}
				else
				{
					this.index = (this.index - 1) % this.m_RolloverSize;
					this.index = ((this.index < 0) ? (this.index + this.m_RolloverSize) : this.index);
				}
				T t2 = this.itemStack[this.index];
				this.itemStack[this.index] = this.m_DefaultItem;
				this.m_Count = ((this.m_Count > 0) ? (this.m_Count - 1) : 0);
				t = t2;
			}
			return t;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00022908 File Offset: 0x00020B08
		public T Peek()
		{
			bool flag = this.index == 0;
			T t;
			if (flag)
			{
				t = this.m_DefaultItem;
			}
			else
			{
				t = this.itemStack[this.index - 1];
			}
			return t;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00022944 File Offset: 0x00020B44
		public T CurrentItem()
		{
			bool flag = this.index > 0;
			T t;
			if (flag)
			{
				t = this.itemStack[this.index - 1];
			}
			else
			{
				t = this.itemStack[0];
			}
			return t;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00022988 File Offset: 0x00020B88
		public T PreviousItem()
		{
			bool flag = this.index > 1;
			T t;
			if (flag)
			{
				t = this.itemStack[this.index - 2];
			}
			else
			{
				t = this.itemStack[0];
			}
			return t;
		}

		// Token: 0x04000393 RID: 915
		public T[] itemStack;

		// Token: 0x04000394 RID: 916
		public int index;

		// Token: 0x04000395 RID: 917
		private T m_DefaultItem;

		// Token: 0x04000396 RID: 918
		private int m_Capacity;

		// Token: 0x04000397 RID: 919
		private int m_RolloverSize;

		// Token: 0x04000398 RID: 920
		private int m_Count;

		// Token: 0x04000399 RID: 921
		private const int k_DefaultCapacity = 4;
	}
}
