using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000451 RID: 1105
	internal class NativePagedList<T> : IDisposable where T : struct
	{
		// Token: 0x06002292 RID: 8850 RVA: 0x000852B0 File Offset: 0x000834B0
		public NativePagedList(int poolCapacity)
		{
			Debug.Assert(poolCapacity > 0);
			this.k_PoolCapacity = Mathf.NextPowerOfTwo(poolCapacity);
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000852E8 File Offset: 0x000834E8
		public void Add(ref T data)
		{
			bool flag = this.m_CurrentPageCount < this.m_CurrentPage.Length;
			if (flag)
			{
				int currentPageCount = this.m_CurrentPageCount;
				this.m_CurrentPageCount = currentPageCount + 1;
				this.m_CurrentPage[currentPageCount] = data;
			}
			else
			{
				int num = ((this.m_Pages.Count > 0) ? (this.m_CurrentPage.Length << 1) : this.k_PoolCapacity);
				this.m_CurrentPage = new NativeArray<T>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				this.m_Pages.Add(this.m_CurrentPage);
				this.m_CurrentPage[0] = data;
				this.m_CurrentPageCount = 1;
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00085391 File Offset: 0x00083591
		public void Add(T data)
		{
			this.Add(ref data);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000853A0 File Offset: 0x000835A0
		public List<NativeSlice<T>> GetPages()
		{
			this.m_Enumerator.Clear();
			bool flag = this.m_Pages.Count > 0;
			if (flag)
			{
				int num = this.m_Pages.Count - 1;
				for (int i = 0; i < num; i++)
				{
					this.m_Enumerator.Add(this.m_Pages[i]);
				}
				bool flag2 = this.m_CurrentPageCount > 0;
				if (flag2)
				{
					this.m_Enumerator.Add(this.m_CurrentPage.Slice<T>(0, this.m_CurrentPageCount));
				}
			}
			return this.m_Enumerator;
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x00085444 File Offset: 0x00083644
		public void Reset()
		{
			bool flag = this.m_Pages.Count > 1;
			if (flag)
			{
				this.m_CurrentPage = this.m_Pages[0];
				for (int i = 1; i < this.m_Pages.Count; i++)
				{
					this.m_Pages[i].Dispose();
				}
				this.m_Pages.Clear();
				this.m_Pages.Add(this.m_CurrentPage);
			}
			this.m_CurrentPageCount = 0;
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000854CC File Offset: 0x000836CC
		// (set) Token: 0x06002298 RID: 8856 RVA: 0x000854D4 File Offset: 0x000836D4
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002299 RID: 8857 RVA: 0x000854DD File Offset: 0x000836DD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000854F0 File Offset: 0x000836F0
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.m_Pages.Count; i++)
					{
						this.m_Pages[i].Dispose();
					}
					this.m_Pages.Clear();
					this.m_CurrentPageCount = 0;
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000F77 RID: 3959
		private readonly int k_PoolCapacity;

		// Token: 0x04000F78 RID: 3960
		private List<NativeArray<T>> m_Pages = new List<NativeArray<T>>(8);

		// Token: 0x04000F79 RID: 3961
		private NativeArray<T> m_CurrentPage;

		// Token: 0x04000F7A RID: 3962
		private int m_CurrentPageCount;

		// Token: 0x04000F7B RID: 3963
		private List<NativeSlice<T>> m_Enumerator = new List<NativeSlice<T>>(8);
	}
}
