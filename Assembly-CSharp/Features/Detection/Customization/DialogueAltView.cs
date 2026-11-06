using System;

namespace Features.Detection.Customization
{
	// Token: 0x02001321 RID: 4897
	public readonly struct DialogueAltView
	{
		// Token: 0x0600976E RID: 38766 RVA: 0x003E1526 File Offset: 0x003DF726
		public DialogueAltView(string id, bool isSystem, int effectiveWeight, int defaultWeight, DialogueLine[] lines)
		{
			this.id = id;
			this.isSystem = isSystem;
			this.effectiveWeight = effectiveWeight;
			this.defaultWeight = defaultWeight;
			this.lines = lines;
		}

		// Token: 0x04009CDA RID: 40154
		public readonly string id;

		// Token: 0x04009CDB RID: 40155
		public readonly bool isSystem;

		// Token: 0x04009CDC RID: 40156
		public readonly int effectiveWeight;

		// Token: 0x04009CDD RID: 40157
		public readonly int defaultWeight;

		// Token: 0x04009CDE RID: 40158
		public readonly DialogueLine[] lines;
	}
}
