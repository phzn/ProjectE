using System;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x02000351 RID: 849
	[Serializable]
	internal class StyleSelector
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0006ECC0 File Offset: 0x0006CEC0
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x0006ECD8 File Offset: 0x0006CED8
		public StyleSelectorPart[] parts
		{
			get
			{
				return this.m_Parts;
			}
			internal set
			{
				this.m_Parts = value;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x0006ECE4 File Offset: 0x0006CEE4
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x0006ECFC File Offset: 0x0006CEFC
		public StyleSelectorRelationship previousRelationship
		{
			get
			{
				return this.m_PreviousRelationship;
			}
			internal set
			{
				this.m_PreviousRelationship = value;
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0006ED08 File Offset: 0x0006CF08
		public override string ToString()
		{
			return string.Join(", ", this.parts.Select<StyleSelectorPart, string>((StyleSelectorPart p) => p.ToString()).ToArray<string>());
		}

		// Token: 0x04000BCA RID: 3018
		[SerializeField]
		private StyleSelectorPart[] m_Parts;

		// Token: 0x04000BCB RID: 3019
		[SerializeField]
		private StyleSelectorRelationship m_PreviousRelationship;

		// Token: 0x04000BCC RID: 3020
		internal int pseudoStateMask = -1;

		// Token: 0x04000BCD RID: 3021
		internal int negatedPseudoStateMask = -1;
	}
}
