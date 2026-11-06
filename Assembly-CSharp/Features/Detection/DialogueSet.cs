using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012BF RID: 4799
	[Serializable]
	public class DialogueSet
	{
		// Token: 0x040099CF RID: 39375
		[Tooltip("Stable identity for this set. Used by the dialogue customization layer for per-set weight overrides. Empty = the runtime falls back to a deterministic id 'sys-<profile>-<category>-<index>'.")]
		public string setId;

		// Token: 0x040099D0 RID: 39376
		[Range(1f, 100f)]
		public int weight = 10;

		// Token: 0x040099D1 RID: 39377
		public DialogueLine[] lines;
	}
}
