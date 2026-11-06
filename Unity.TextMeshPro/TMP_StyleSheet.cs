using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000058 RID: 88
	[ExcludeFromPreset]
	[Serializable]
	public class TMP_StyleSheet : ScriptableObject
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00026A82 File Offset: 0x00024C82
		internal List<TMP_Style> styles
		{
			get
			{
				return this.m_StyleList;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00026A8A File Offset: 0x00024C8A
		private void Reset()
		{
			this.LoadStyleDictionaryInternal();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00026A94 File Offset: 0x00024C94
		public TMP_Style GetStyle(int hashCode)
		{
			if (this.m_StyleLookupDictionary == null)
			{
				this.LoadStyleDictionaryInternal();
			}
			TMP_Style tmp_Style;
			if (this.m_StyleLookupDictionary.TryGetValue(hashCode, out tmp_Style))
			{
				return tmp_Style;
			}
			return null;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00026AC4 File Offset: 0x00024CC4
		public TMP_Style GetStyle(string name)
		{
			if (this.m_StyleLookupDictionary == null)
			{
				this.LoadStyleDictionaryInternal();
			}
			int hashCode = TMP_TextParsingUtilities.GetHashCode(name);
			TMP_Style tmp_Style;
			if (this.m_StyleLookupDictionary.TryGetValue(hashCode, out tmp_Style))
			{
				return tmp_Style;
			}
			return null;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00026AF9 File Offset: 0x00024CF9
		public void RefreshStyles()
		{
			this.LoadStyleDictionaryInternal();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00026B04 File Offset: 0x00024D04
		private void LoadStyleDictionaryInternal()
		{
			if (this.m_StyleLookupDictionary == null)
			{
				this.m_StyleLookupDictionary = new Dictionary<int, TMP_Style>();
			}
			else
			{
				this.m_StyleLookupDictionary.Clear();
			}
			for (int i = 0; i < this.m_StyleList.Count; i++)
			{
				this.m_StyleList[i].RefreshStyle();
				if (!this.m_StyleLookupDictionary.ContainsKey(this.m_StyleList[i].hashCode))
				{
					this.m_StyleLookupDictionary.Add(this.m_StyleList[i].hashCode, this.m_StyleList[i]);
				}
			}
			int hashCode = TMP_TextParsingUtilities.GetHashCode("Normal");
			if (!this.m_StyleLookupDictionary.ContainsKey(hashCode))
			{
				TMP_Style tmp_Style = new TMP_Style("Normal", string.Empty, string.Empty);
				this.m_StyleList.Add(tmp_Style);
				this.m_StyleLookupDictionary.Add(hashCode, tmp_Style);
			}
		}

		// Token: 0x040003B3 RID: 947
		[SerializeField]
		private List<TMP_Style> m_StyleList = new List<TMP_Style>(1);

		// Token: 0x040003B4 RID: 948
		private Dictionary<int, TMP_Style> m_StyleLookupDictionary;
	}
}
