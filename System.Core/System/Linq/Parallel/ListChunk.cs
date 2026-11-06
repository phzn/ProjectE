using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	// Token: 0x020001F2 RID: 498
	internal class ListChunk<TInputOutput> : IEnumerable<TInputOutput>, IEnumerable
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x0002ADE2 File Offset: 0x00028FE2
		internal ListChunk(int size)
		{
			this._chunk = new TInputOutput[size];
			this._chunkCount = 0;
			this._tailChunk = this;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002AE04 File Offset: 0x00029004
		internal void Add(TInputOutput e)
		{
			ListChunk<TInputOutput> listChunk = this._tailChunk;
			if (listChunk._chunkCount == listChunk._chunk.Length)
			{
				this._tailChunk = new ListChunk<TInputOutput>(listChunk._chunkCount * 2);
				listChunk = (listChunk._nextChunk = this._tailChunk);
			}
			TInputOutput[] chunk = listChunk._chunk;
			ListChunk<TInputOutput> listChunk2 = listChunk;
			int chunkCount = listChunk2._chunkCount;
			listChunk2._chunkCount = chunkCount + 1;
			chunk[chunkCount] = e;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0002AE68 File Offset: 0x00029068
		internal ListChunk<TInputOutput> Next
		{
			get
			{
				return this._nextChunk;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0002AE70 File Offset: 0x00029070
		internal int Count
		{
			get
			{
				return this._chunkCount;
			}
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0002AE78 File Offset: 0x00029078
		public IEnumerator<TInputOutput> GetEnumerator()
		{
			for (ListChunk<TInputOutput> curr = this; curr != null; curr = curr._nextChunk)
			{
				int num;
				for (int i = 0; i < curr._chunkCount; i = num + 1)
				{
					yield return curr._chunk[i];
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0000817A File Offset: 0x0000637A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TInputOutput>)this).GetEnumerator();
		}

		// Token: 0x040008A3 RID: 2211
		internal TInputOutput[] _chunk;

		// Token: 0x040008A4 RID: 2212
		private int _chunkCount;

		// Token: 0x040008A5 RID: 2213
		private ListChunk<TInputOutput> _nextChunk;

		// Token: 0x040008A6 RID: 2214
		private ListChunk<TInputOutput> _tailChunk;
	}
}
