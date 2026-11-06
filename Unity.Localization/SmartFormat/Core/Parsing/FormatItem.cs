using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Settings;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000080 RID: 128
	public abstract class FormatItem
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000CF12 File Offset: 0x0000B112
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000CF1A File Offset: 0x0000B11A
		public FormatItem Parent { get; private set; }

		// Token: 0x0600035A RID: 858 RVA: 0x0000CF23 File Offset: 0x0000B123
		public void Init(SmartSettings smartSettings, FormatItem parent, int startIndex)
		{
			this.Init(smartSettings, parent, parent.baseString, startIndex, parent.baseString.Length);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000CF3F File Offset: 0x0000B13F
		public void Init(SmartSettings smartSettings, FormatItem parent, int startIndex, int endIndex)
		{
			this.Init(smartSettings, parent, parent.baseString, startIndex, endIndex);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000CF52 File Offset: 0x0000B152
		public void Init(SmartSettings smartSettings, FormatItem parent, string baseString, int startIndex, int endIndex)
		{
			this.Parent = parent;
			this.SmartSettings = smartSettings;
			this.baseString = baseString;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000CF79 File Offset: 0x0000B179
		public virtual void Clear()
		{
			this.baseString = null;
			this.endIndex = 0;
			this.startIndex = 0;
			this.SmartSettings = null;
			this.m_RawText = null;
			this.Parent = null;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		public string RawText
		{
			get
			{
				if (this.m_RawText == null)
				{
					this.m_RawText = this.baseString.Substring(this.startIndex, this.endIndex - this.startIndex);
				}
				return this.m_RawText;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000CFD9 File Offset: 0x0000B1D9
		public IEnumerable<char> ToEnumerable()
		{
			return new FormatItem.PartialCharEnumerator(this.baseString, this.startIndex, this.endIndex);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		public override string ToString()
		{
			string text;
			if (this.endIndex > this.startIndex)
			{
				if ((text = this.RawText) == null)
				{
					return "";
				}
			}
			else
			{
				text = "Empty (" + this.baseString.Substring(this.startIndex) + ")";
			}
			return text;
		}

		// Token: 0x040000E6 RID: 230
		public string baseString;

		// Token: 0x040000E7 RID: 231
		public int endIndex;

		// Token: 0x040000E8 RID: 232
		protected SmartSettings SmartSettings;

		// Token: 0x040000E9 RID: 233
		public int startIndex;

		// Token: 0x040000EA RID: 234
		protected string m_RawText;

		// Token: 0x0200013F RID: 319
		private struct PartialCharEnumerator : IEnumerable<char>, IEnumerable
		{
			// Token: 0x060007BB RID: 1979 RVA: 0x0001915D File Offset: 0x0001735D
			public PartialCharEnumerator(string s, int from, int to)
			{
				this.m_BaseString = s;
				this.m_From = from;
				this.m_To = to;
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x00019174 File Offset: 0x00017374
			public IEnumerator<char> GetEnumerator()
			{
				int num;
				for (int i = this.m_From; i < this.m_To; i = num)
				{
					yield return this.m_BaseString[i];
					num = i + 1;
				}
				yield break;
			}

			// Token: 0x060007BD RID: 1981 RVA: 0x00019188 File Offset: 0x00017388
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040002D6 RID: 726
			private string m_BaseString;

			// Token: 0x040002D7 RID: 727
			private int m_From;

			// Token: 0x040002D8 RID: 728
			private int m_To;
		}
	}
}
