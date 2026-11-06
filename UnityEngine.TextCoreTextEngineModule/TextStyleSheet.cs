using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200004F RID: 79
	[ExcludeFromPreset]
	[ExcludeFromObjectFactory]
	[Serializable]
	public class TextStyleSheet : ScriptableObject
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000248D4 File Offset: 0x00022AD4
		internal List<TextStyle> styles
		{
			get
			{
				return this.m_StyleList;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000248EC File Offset: 0x00022AEC
		private void Reset()
		{
			this.LoadStyleDictionaryInternal();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000248F8 File Offset: 0x00022AF8
		public TextStyle GetStyle(int hashCode)
		{
			bool flag = this.m_StyleLookupDictionary == null;
			if (flag)
			{
				this.LoadStyleDictionaryInternal();
			}
			TextStyle textStyle;
			bool flag2 = this.m_StyleLookupDictionary.TryGetValue(hashCode, out textStyle);
			TextStyle textStyle2;
			if (flag2)
			{
				textStyle2 = textStyle;
			}
			else
			{
				textStyle2 = null;
			}
			return textStyle2;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00024938 File Offset: 0x00022B38
		public TextStyle GetStyle(string name)
		{
			bool flag = this.m_StyleLookupDictionary == null;
			if (flag)
			{
				this.LoadStyleDictionaryInternal();
			}
			int hashCodeCaseInSensitive = TextUtilities.GetHashCodeCaseInSensitive(name);
			TextStyle textStyle;
			bool flag2 = this.m_StyleLookupDictionary.TryGetValue(hashCodeCaseInSensitive, out textStyle);
			TextStyle textStyle2;
			if (flag2)
			{
				textStyle2 = textStyle;
			}
			else
			{
				textStyle2 = null;
			}
			return textStyle2;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000248EC File Offset: 0x00022AEC
		public void RefreshStyles()
		{
			this.LoadStyleDictionaryInternal();
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00024980 File Offset: 0x00022B80
		private void LoadStyleDictionaryInternal()
		{
			bool flag = this.m_StyleLookupDictionary == null;
			if (flag)
			{
				this.m_StyleLookupDictionary = new Dictionary<int, TextStyle>();
			}
			else
			{
				this.m_StyleLookupDictionary.Clear();
			}
			for (int i = 0; i < this.m_StyleList.Count; i++)
			{
				this.m_StyleList[i].RefreshStyle();
				bool flag2 = !this.m_StyleLookupDictionary.ContainsKey(this.m_StyleList[i].hashCode);
				if (flag2)
				{
					this.m_StyleLookupDictionary.Add(this.m_StyleList[i].hashCode, this.m_StyleList[i]);
				}
			}
			int hashCodeCaseInSensitive = TextUtilities.GetHashCodeCaseInSensitive("Normal");
			bool flag3 = !this.m_StyleLookupDictionary.ContainsKey(hashCodeCaseInSensitive);
			if (flag3)
			{
				TextStyle textStyle = new TextStyle("Normal", string.Empty, string.Empty);
				this.m_StyleList.Add(textStyle);
				this.m_StyleLookupDictionary.Add(hashCodeCaseInSensitive, textStyle);
			}
		}

		// Token: 0x0400040D RID: 1037
		[SerializeField]
		private List<TextStyle> m_StyleList = new List<TextStyle>(1);

		// Token: 0x0400040E RID: 1038
		private Dictionary<int, TextStyle> m_StyleLookupDictionary;
	}
}
