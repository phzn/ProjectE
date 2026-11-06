using System;

namespace TMPro
{
	// Token: 0x02000018 RID: 24
	public struct TMP_WordInfo
	{
		// Token: 0x0600010D RID: 269 RVA: 0x000171D8 File Offset: 0x000153D8
		public string GetWord()
		{
			string text = string.Empty;
			TMP_CharacterInfo[] characterInfo = this.textComponent.textInfo.characterInfo;
			for (int i = this.firstCharacterIndex; i < this.lastCharacterIndex + 1; i++)
			{
				text += characterInfo[i].character.ToString();
			}
			return text;
		}

		// Token: 0x040000B0 RID: 176
		public TMP_Text textComponent;

		// Token: 0x040000B1 RID: 177
		public int firstCharacterIndex;

		// Token: 0x040000B2 RID: 178
		public int lastCharacterIndex;

		// Token: 0x040000B3 RID: 179
		public int characterCount;
	}
}
