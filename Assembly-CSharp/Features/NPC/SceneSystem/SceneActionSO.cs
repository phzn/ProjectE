using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F26 RID: 3878
	public abstract class SceneActionSO : ScriptableObject
	{
		// Token: 0x06007485 RID: 29829
		public abstract IEnumerator Execute(SceneContext context, string targetSlotId);

		// Token: 0x06007486 RID: 29830 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnInterrupt(SceneContext context, string targetSlotId)
		{
		}

		// Token: 0x04007750 RID: 30544
		[Header("Base Settings")]
		[Tooltip("Display name for this action (for editor/debugging)")]
		public string displayName;
	}
}
