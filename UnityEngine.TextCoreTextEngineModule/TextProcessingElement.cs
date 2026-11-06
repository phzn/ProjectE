using System;
using System.Diagnostics;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000034 RID: 52
	[DebuggerDisplay("Unicode ({unicode})  '{(char)unicode}'")]
	internal struct TextProcessingElement
	{
		// Token: 0x0400025A RID: 602
		public TextProcessingElementType elementType;

		// Token: 0x0400025B RID: 603
		public uint unicode;

		// Token: 0x0400025C RID: 604
		public int stringIndex;

		// Token: 0x0400025D RID: 605
		public int length;
	}
}
