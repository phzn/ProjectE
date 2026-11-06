using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000491 RID: 1169
	internal struct MatchResultInfo
	{
		// Token: 0x060024A5 RID: 9381 RVA: 0x000998FA File Offset: 0x00097AFA
		public MatchResultInfo(bool success, PseudoStates triggerPseudoMask, PseudoStates dependencyPseudoMask)
		{
			this.success = success;
			this.triggerPseudoMask = triggerPseudoMask;
			this.dependencyPseudoMask = dependencyPseudoMask;
		}

		// Token: 0x04001192 RID: 4498
		public readonly bool success;

		// Token: 0x04001193 RID: 4499
		public readonly PseudoStates triggerPseudoMask;

		// Token: 0x04001194 RID: 4500
		public readonly PseudoStates dependencyPseudoMask;
	}
}
