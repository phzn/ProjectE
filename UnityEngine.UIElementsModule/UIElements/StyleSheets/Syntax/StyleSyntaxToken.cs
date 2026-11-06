using System;

namespace UnityEngine.UIElements.StyleSheets.Syntax
{
	// Token: 0x020004B0 RID: 1200
	internal struct StyleSyntaxToken
	{
		// Token: 0x0600253E RID: 9534 RVA: 0x0009D9F1 File Offset: 0x0009BBF1
		public StyleSyntaxToken(StyleSyntaxTokenType t)
		{
			this.type = t;
			this.text = null;
			this.number = 0;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x0009DA09 File Offset: 0x0009BC09
		public StyleSyntaxToken(StyleSyntaxTokenType type, string text)
		{
			this.type = type;
			this.text = text;
			this.number = 0;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x0009DA21 File Offset: 0x0009BC21
		public StyleSyntaxToken(StyleSyntaxTokenType type, int number)
		{
			this.type = type;
			this.text = null;
			this.number = number;
		}

		// Token: 0x0400122C RID: 4652
		public StyleSyntaxTokenType type;

		// Token: 0x0400122D RID: 4653
		public string text;

		// Token: 0x0400122E RID: 4654
		public int number;
	}
}
