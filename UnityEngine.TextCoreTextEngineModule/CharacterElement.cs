using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000044 RID: 68
	internal struct CharacterElement
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00021E68 File Offset: 0x00020068
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00021E80 File Offset: 0x00020080
		public uint Unicode
		{
			get
			{
				return this.m_Unicode;
			}
			set
			{
				this.m_Unicode = value;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00021E8A File Offset: 0x0002008A
		public CharacterElement(TextElement textElement)
		{
			this.m_Unicode = textElement.unicode;
			this.m_TextElement = textElement;
		}

		// Token: 0x04000382 RID: 898
		private uint m_Unicode;

		// Token: 0x04000383 RID: 899
		private TextElement m_TextElement;
	}
}
