using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200044B RID: 1099
	internal class LinkedPool<T> where T : LinkedPoolItem<T>
	{
		// Token: 0x0600227C RID: 8828 RVA: 0x00084D03 File Offset: 0x00082F03
		public LinkedPool(Func<T> createFunc, Action<T> resetAction, int limit = 10000)
		{
			Debug.Assert(createFunc != null);
			this.m_CreateFunc = createFunc;
			Debug.Assert(resetAction != null);
			this.m_ResetAction = resetAction;
			Debug.Assert(limit > 0);
			this.m_Limit = limit;
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x00084D40 File Offset: 0x00082F40
		// (set) Token: 0x0600227E RID: 8830 RVA: 0x00084D48 File Offset: 0x00082F48
		public int Count { get; private set; }

		// Token: 0x0600227F RID: 8831 RVA: 0x00084D51 File Offset: 0x00082F51
		public void Clear()
		{
			this.m_PoolFirst = default(T);
			this.Count = 0;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x00084D68 File Offset: 0x00082F68
		public T Get()
		{
			T t = this.m_PoolFirst;
			bool flag = this.m_PoolFirst != null;
			if (flag)
			{
				int num = this.Count - 1;
				this.Count = num;
				this.m_PoolFirst = t.poolNext;
				this.m_ResetAction(t);
			}
			else
			{
				t = this.m_CreateFunc();
			}
			return t;
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00084DD4 File Offset: 0x00082FD4
		public void Return(T item)
		{
			bool flag = this.Count < this.m_Limit;
			if (flag)
			{
				item.poolNext = this.m_PoolFirst;
				this.m_PoolFirst = item;
				int num = this.Count + 1;
				this.Count = num;
			}
		}

		// Token: 0x04000F65 RID: 3941
		private readonly Func<T> m_CreateFunc;

		// Token: 0x04000F66 RID: 3942
		private readonly Action<T> m_ResetAction;

		// Token: 0x04000F67 RID: 3943
		private readonly int m_Limit;

		// Token: 0x04000F68 RID: 3944
		private T m_PoolFirst;
	}
}
