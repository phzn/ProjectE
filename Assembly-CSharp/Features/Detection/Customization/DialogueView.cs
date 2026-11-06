using System;
using Bozo.AnimeCharacters;

namespace Features.Detection.Customization
{
	// Token: 0x02001320 RID: 4896
	public readonly struct DialogueView
	{
		// Token: 0x0600976B RID: 38763 RVA: 0x003E143C File Offset: 0x003DF63C
		public DialogueView(string id, bool isSystem, PersonalityType personality, DialogueGender gender, DialogueCategory category, int effectiveWeight, int defaultWeight, DialogueLine[] lines, bool excludeFromPool = false, DialogueActorGroup actorGroup = DialogueActorGroup.Civilians, string displayName = null)
		{
			this.id = id;
			this.isSystem = isSystem;
			this.personality = personality;
			this.gender = gender;
			this.category = category;
			this.effectiveWeight = effectiveWeight;
			this.defaultWeight = defaultWeight;
			this.lines = lines;
			this.excludeFromPool = excludeFromPool;
			this.actorGroup = actorGroup;
			this.displayName = displayName ?? "";
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x0600976C RID: 38764 RVA: 0x003E14A7 File Offset: 0x003DF6A7
		public string DisplayLabel
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.displayName))
				{
					return DialogueView.FallbackName(this.lines);
				}
				return this.displayName;
			}
		}

		// Token: 0x0600976D RID: 38765 RVA: 0x003E14C8 File Offset: 0x003DF6C8
		private static string FallbackName(DialogueLine[] lines)
		{
			string text = ((lines != null && lines.Length != 0 && lines[0] != null) ? lines[0].text : null);
			if (string.IsNullOrWhiteSpace(text))
			{
				return "(unnamed)";
			}
			text = text.Trim();
			if (text.Length > 40)
			{
				return text.Substring(0, 40).TrimEnd() + "…";
			}
			return text;
		}

		// Token: 0x04009CCF RID: 40143
		public readonly string id;

		// Token: 0x04009CD0 RID: 40144
		public readonly bool isSystem;

		// Token: 0x04009CD1 RID: 40145
		public readonly PersonalityType personality;

		// Token: 0x04009CD2 RID: 40146
		public readonly DialogueGender gender;

		// Token: 0x04009CD3 RID: 40147
		public readonly DialogueCategory category;

		// Token: 0x04009CD4 RID: 40148
		public readonly int effectiveWeight;

		// Token: 0x04009CD5 RID: 40149
		public readonly int defaultWeight;

		// Token: 0x04009CD6 RID: 40150
		public readonly DialogueLine[] lines;

		// Token: 0x04009CD7 RID: 40151
		public readonly bool excludeFromPool;

		// Token: 0x04009CD8 RID: 40152
		public readonly DialogueActorGroup actorGroup;

		// Token: 0x04009CD9 RID: 40153
		public readonly string displayName;
	}
}
