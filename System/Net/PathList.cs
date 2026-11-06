using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000654 RID: 1620
	[Serializable]
	internal class PathList
	{
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x000B23B1 File Offset: 0x000B05B1
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000B23C0 File Offset: 0x000B05C0
		public int GetCookiesCount()
		{
			int num = 0;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				foreach (object obj in this.m_list.Values)
				{
					CookieCollection cookieCollection = (CookieCollection)obj;
					num += cookieCollection.Count;
				}
			}
			return num;
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x000B2450 File Offset: 0x000B0650
		public ICollection Values
		{
			get
			{
				return this.m_list.Values;
			}
		}

		// Token: 0x17000A4F RID: 2639
		public object this[string s]
		{
			get
			{
				return this.m_list[s];
			}
			set
			{
				object syncRoot = this.SyncRoot;
				lock (syncRoot)
				{
					this.m_list[s] = value;
				}
			}
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000B24B4 File Offset: 0x000B06B4
		public IEnumerator GetEnumerator()
		{
			return this.m_list.GetEnumerator();
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x000B24C1 File Offset: 0x000B06C1
		public object SyncRoot
		{
			get
			{
				return this.m_list.SyncRoot;
			}
		}

		// Token: 0x04001DF9 RID: 7673
		private SortedList m_list = SortedList.Synchronized(new SortedList(PathList.PathListComparer.StaticInstance));

		// Token: 0x02000655 RID: 1621
		[Serializable]
		private class PathListComparer : IComparer
		{
			// Token: 0x06003315 RID: 13077 RVA: 0x000B24D0 File Offset: 0x000B06D0
			int IComparer.Compare(object ol, object or)
			{
				string text = CookieParser.CheckQuoted((string)ol);
				string text2 = CookieParser.CheckQuoted((string)or);
				int length = text.Length;
				int length2 = text2.Length;
				int num = Math.Min(length, length2);
				for (int i = 0; i < num; i++)
				{
					if (text[i] != text2[i])
					{
						return (int)(text[i] - text2[i]);
					}
				}
				return length2 - length;
			}

			// Token: 0x04001DFA RID: 7674
			internal static readonly PathList.PathListComparer StaticInstance = new PathList.PathListComparer();
		}
	}
}
