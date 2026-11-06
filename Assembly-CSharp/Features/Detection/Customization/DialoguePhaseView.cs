using System;

namespace Features.Detection.Customization
{
	// Token: 0x02001322 RID: 4898
	public readonly struct DialoguePhaseView
	{
		// Token: 0x0600976F RID: 38767 RVA: 0x003E154D File Offset: 0x003DF74D
		public DialoguePhaseView(DialogueCategory category, DialogueAltView[] alternatives)
		{
			this.category = category;
			this.alternatives = alternatives ?? Array.Empty<DialogueAltView>();
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06009770 RID: 38768 RVA: 0x003E1568 File Offset: 0x003DF768
		public int LineCount
		{
			get
			{
				int num = 0;
				foreach (DialogueAltView dialogueAltView in this.alternatives)
				{
					num += ((dialogueAltView.lines != null) ? dialogueAltView.lines.Length : 0);
				}
				return num;
			}
		}

		// Token: 0x04009CDF RID: 40159
		public readonly DialogueCategory category;

		// Token: 0x04009CE0 RID: 40160
		public readonly DialogueAltView[] alternatives;
	}
}
