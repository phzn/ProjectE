using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000036 RID: 54
	internal struct CharacterSubstitution
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0001D831 File Offset: 0x0001BA31
		public CharacterSubstitution(int index, uint unicode)
		{
			this.index = index;
			this.unicode = unicode;
		}

		// Token: 0x04000260 RID: 608
		public int index;

		// Token: 0x04000261 RID: 609
		public uint unicode;
	}
}
