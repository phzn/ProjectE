using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000288 RID: 648
	internal class ObjectPool<T> where T : new()
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x00041548 File Offset: 0x0003F748
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x00041560 File Offset: 0x0003F760
		public int maxSize
		{
			get
			{
				return this.m_MaxSize;
			}
			set
			{
				this.m_MaxSize = Math.Max(0, value);
				while (this.Size() > this.m_MaxSize)
				{
					this.Get();
				}
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00041598 File Offset: 0x0003F798
		public ObjectPool(Func<T> CreateFunc, int maxSize = 100)
		{
			this.maxSize = maxSize;
			bool flag = CreateFunc == null;
			if (flag)
			{
				this.CreateFunc = () => new T();
			}
			else
			{
				this.CreateFunc = CreateFunc;
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000415FC File Offset: 0x0003F7FC
		public int Size()
		{
			return this.m_Stack.Count;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00041619 File Offset: 0x0003F819
		public void Clear()
		{
			this.m_Stack.Clear();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00041628 File Offset: 0x0003F828
		public T Get()
		{
			return (this.m_Stack.Count == 0) ? this.CreateFunc() : this.m_Stack.Pop();
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00041664 File Offset: 0x0003F864
		public void Release(T element)
		{
			bool flag = this.m_Stack.Count > 0 && this.m_Stack.Peek() == element;
			if (flag)
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			bool flag2 = this.m_Stack.Count < this.maxSize;
			if (flag2)
			{
				this.m_Stack.Push(element);
			}
		}

		// Token: 0x0400083B RID: 2107
		private readonly Stack<T> m_Stack = new Stack<T>();

		// Token: 0x0400083C RID: 2108
		private int m_MaxSize;

		// Token: 0x0400083D RID: 2109
		internal Func<T> CreateFunc;
	}
}
