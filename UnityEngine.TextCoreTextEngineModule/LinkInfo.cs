using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000013 RID: 19
	internal struct LinkInfo
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x0000685C File Offset: 0x00004A5C
		internal void SetLinkId(char[] text, int startIndex, int length)
		{
			bool flag = this.linkId == null || this.linkId.Length < length;
			if (flag)
			{
				this.linkId = new char[length];
			}
			for (int i = 0; i < length; i++)
			{
				this.linkId[i] = text[startIndex + i];
			}
			this.linkIdLength = length;
			this.m_LinkIdString = null;
			this.m_LinkTextString = null;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000068C4 File Offset: 0x00004AC4
		public string GetLinkText(TextInfo textInfo)
		{
			bool flag = string.IsNullOrEmpty(this.m_LinkTextString);
			if (flag)
			{
				for (int i = this.linkTextfirstCharacterIndex; i < this.linkTextfirstCharacterIndex + this.linkTextLength; i++)
				{
					this.m_LinkTextString += textInfo.textElementInfo[i].character.ToString();
				}
			}
			return this.m_LinkTextString;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006934 File Offset: 0x00004B34
		public string GetLinkId()
		{
			bool flag = string.IsNullOrEmpty(this.m_LinkIdString);
			if (flag)
			{
				this.m_LinkIdString = new string(this.linkId, 0, this.linkIdLength);
			}
			return this.m_LinkIdString;
		}

		// Token: 0x04000091 RID: 145
		public int hashCode;

		// Token: 0x04000092 RID: 146
		public int linkIdFirstCharacterIndex;

		// Token: 0x04000093 RID: 147
		public int linkIdLength;

		// Token: 0x04000094 RID: 148
		public int linkTextfirstCharacterIndex;

		// Token: 0x04000095 RID: 149
		public int linkTextLength;

		// Token: 0x04000096 RID: 150
		internal char[] linkId;

		// Token: 0x04000097 RID: 151
		private string m_LinkIdString;

		// Token: 0x04000098 RID: 152
		private string m_LinkTextString;
	}
}
