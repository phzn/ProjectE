using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Pool;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x0200007F RID: 127
	public class Format : FormatItem
	{
		// Token: 0x06000349 RID: 841 RVA: 0x0000CA50 File Offset: 0x0000AC50
		public void ReleaseToPool()
		{
			this.Clear();
			foreach (FormatItem formatItem in this.Items)
			{
				if (this == formatItem.Parent)
				{
					FormatItemPool.Release(formatItem);
				}
			}
			foreach (Format.SplitList splitList in this.m_Splits)
			{
				SplitListPool.Release(splitList);
			}
			this.parent = null;
			this.Items.Clear();
			this.HasNested = false;
			this.splitCache = null;
			this.m_Splits.Clear();
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		public List<FormatItem> Items { get; } = new List<FormatItem>();

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000CB24 File Offset: 0x0000AD24
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		public bool HasNested { get; set; }

		// Token: 0x0600034D RID: 845 RVA: 0x0000CB35 File Offset: 0x0000AD35
		public Format Substring(int startIndex)
		{
			return this.Substring(startIndex, this.endIndex - this.startIndex - startIndex);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000CB50 File Offset: 0x0000AD50
		public Format Substring(int startIndex, int length)
		{
			startIndex = this.startIndex + startIndex;
			int num = startIndex + length;
			if (startIndex < this.startIndex || startIndex > this.endIndex)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (num > this.endIndex)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (startIndex == this.startIndex && num == this.endIndex)
			{
				return this;
			}
			Format format = FormatItemPool.GetFormat(this.SmartSettings, this.baseString, startIndex, num);
			foreach (FormatItem formatItem in this.Items)
			{
				if (formatItem.endIndex > startIndex)
				{
					if (num <= formatItem.startIndex)
					{
						break;
					}
					FormatItem formatItem2 = formatItem;
					if (formatItem is LiteralText)
					{
						if (startIndex > formatItem.startIndex || formatItem.endIndex > num)
						{
							formatItem2 = FormatItemPool.GetLiteralText(this.SmartSettings, format, Math.Max(startIndex, formatItem.startIndex), Math.Min(num, formatItem.endIndex));
						}
					}
					else
					{
						format.HasNested = true;
					}
					format.Items.Add(formatItem2);
				}
			}
			return format;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000CC74 File Offset: 0x0000AE74
		public int IndexOf(char search)
		{
			return this.IndexOf(search, 0);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000CC80 File Offset: 0x0000AE80
		public int IndexOf(char search, int startIndex)
		{
			startIndex = this.startIndex + startIndex;
			foreach (FormatItem formatItem in this.Items)
			{
				if (formatItem.endIndex >= startIndex)
				{
					LiteralText literalText = formatItem as LiteralText;
					if (literalText != null)
					{
						if (startIndex < literalText.startIndex)
						{
							startIndex = literalText.startIndex;
						}
						int num = literalText.baseString.IndexOf(search, startIndex, literalText.endIndex - startIndex);
						if (num != -1)
						{
							return num - this.startIndex;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000CD24 File Offset: 0x0000AF24
		private List<int> FindAll(char search)
		{
			return this.FindAll(search, -1);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000CD30 File Offset: 0x0000AF30
		private List<int> FindAll(char search, int maxCount)
		{
			List<int> list = CollectionPool<List<int>, int>.Get();
			int num = 0;
			while (maxCount != 0)
			{
				num = this.IndexOf(search, num);
				if (num == -1)
				{
					break;
				}
				list.Add(num);
				num++;
				maxCount--;
			}
			return list;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000CD68 File Offset: 0x0000AF68
		public IList<Format> Split(char search)
		{
			if (this.splitCache == null || this.splitCacheChar != search)
			{
				this.splitCacheChar = search;
				this.splitCache = this.Split(search, -1);
			}
			return this.splitCache;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000CD98 File Offset: 0x0000AF98
		public IList<Format> Split(char search, int maxCount)
		{
			List<int> list = this.FindAll(search, maxCount);
			Format.SplitList splitList = SplitListPool.Get(this, list);
			this.m_Splits.Add(splitList);
			return splitList;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		public string GetLiteralText()
		{
			StringBuilder stringBuilder;
			string text;
			using (StringBuilderPool.Get(out stringBuilder))
			{
				foreach (FormatItem formatItem in this.Items)
				{
					LiteralText literalText = formatItem as LiteralText;
					if (literalText != null)
					{
						stringBuilder.Append(literalText);
					}
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000CE50 File Offset: 0x0000B050
		public override string ToString()
		{
			StringBuilder stringBuilder;
			string text;
			using (StringBuilderPool.Get(out stringBuilder))
			{
				int num = this.endIndex - this.startIndex;
				if (stringBuilder.Capacity < num)
				{
					stringBuilder.Capacity = num;
				}
				foreach (FormatItem formatItem in this.Items)
				{
					stringBuilder.Append(formatItem);
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x040000E0 RID: 224
		public Placeholder parent;

		// Token: 0x040000E3 RID: 227
		private List<Format.SplitList> m_Splits = new List<Format.SplitList>();

		// Token: 0x040000E4 RID: 228
		private char splitCacheChar;

		// Token: 0x040000E5 RID: 229
		private IList<Format> splitCache;

		// Token: 0x0200013E RID: 318
		internal class SplitList : IList<Format>, ICollection<Format>, IEnumerable<Format>, IEnumerable
		{
			// Token: 0x060007AB RID: 1963 RVA: 0x00018F30 File Offset: 0x00017130
			public void Init(Format format, List<int> splits)
			{
				this.m_Format = format;
				this.m_Splits = splits;
				for (int i = 0; i < this.Count; i++)
				{
					this.m_FormatCache.Add(null);
				}
			}

			// Token: 0x17000186 RID: 390
			public Format this[int index]
			{
				get
				{
					if (index > this.m_Splits.Count)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					if (this.m_Splits.Count == 0)
					{
						return this.m_Format;
					}
					if (this.m_FormatCache[index] != null)
					{
						return this.m_FormatCache[index];
					}
					if (index == 0)
					{
						Format format = this.m_Format.Substring(0, this.m_Splits[0]);
						this.m_FormatCache[index] = format;
						return format;
					}
					if (index == this.m_Splits.Count)
					{
						Format format2 = this.m_Format.Substring(this.m_Splits[index - 1] + 1);
						this.m_FormatCache[index] = format2;
						return format2;
					}
					int num = this.m_Splits[index - 1] + 1;
					Format format3 = this.m_Format.Substring(num, this.m_Splits[index] - num);
					this.m_FormatCache[index] = format3;
					return format3;
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x060007AE RID: 1966 RVA: 0x00019064 File Offset: 0x00017264
			public void CopyTo(Format[] array, int arrayIndex)
			{
				int num = this.m_Splits.Count + 1;
				for (int i = 0; i < num; i++)
				{
					array[arrayIndex + i] = this[i];
				}
			}

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x060007AF RID: 1967 RVA: 0x00019097 File Offset: 0x00017297
			public int Count
			{
				get
				{
					return this.m_Splits.Count + 1;
				}
			}

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000190A6 File Offset: 0x000172A6
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060007B1 RID: 1969 RVA: 0x000190A9 File Offset: 0x000172A9
			public int IndexOf(Format item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B2 RID: 1970 RVA: 0x000190B0 File Offset: 0x000172B0
			public void Insert(int index, Format item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B3 RID: 1971 RVA: 0x000190B7 File Offset: 0x000172B7
			public void RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B4 RID: 1972 RVA: 0x000190BE File Offset: 0x000172BE
			public void Add(Format item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B5 RID: 1973 RVA: 0x000190C8 File Offset: 0x000172C8
			public void Clear()
			{
				this.m_Format = null;
				CollectionPool<List<int>, int>.Release(this.m_Splits);
				this.m_Splits = null;
				for (int i = 0; i < this.m_FormatCache.Count; i++)
				{
					if (this.m_FormatCache[i] != null)
					{
						FormatItemPool.ReleaseFormat(this.m_FormatCache[i]);
					}
				}
				this.m_FormatCache.Clear();
			}

			// Token: 0x060007B6 RID: 1974 RVA: 0x0001912E File Offset: 0x0001732E
			public bool Contains(Format item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B7 RID: 1975 RVA: 0x00019135 File Offset: 0x00017335
			public bool Remove(Format item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x0001913C File Offset: 0x0001733C
			public IEnumerator<Format> GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060007B9 RID: 1977 RVA: 0x00019143 File Offset: 0x00017343
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040002D3 RID: 723
			private Format m_Format;

			// Token: 0x040002D4 RID: 724
			private List<int> m_Splits;

			// Token: 0x040002D5 RID: 725
			private List<Format> m_FormatCache = new List<Format>();
		}
	}
}
