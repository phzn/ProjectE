using System;
using System.Collections;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F2C RID: 3884
	[CreateAssetMenu(fileName = "WaitAction", menuName = "NPC/Scene System/Actions/Wait")]
	public class WaitAction : SceneActionSO
	{
		// Token: 0x06007498 RID: 29848 RVA: 0x002F4EE6 File Offset: 0x002F30E6
		public override IEnumerator Execute(SceneContext context, string targetSlotId)
		{
			float waitTime = (this.useRandomDuration ? Random.Range(this.minDuration, this.maxDuration) : this.duration);
			float elapsed = 0f;
			while (elapsed < waitTime && !context.IsInterrupted)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04007768 RID: 30568
		[Header("Wait Settings")]
		[Tooltip("Duration to wait in seconds")]
		public float duration = 1f;

		// Token: 0x04007769 RID: 30569
		[Tooltip("If true, uses a random duration between minDuration and maxDuration")]
		public bool useRandomDuration;

		// Token: 0x0400776A RID: 30570
		[Tooltip("Minimum duration when using random")]
		public float minDuration = 1f;

		// Token: 0x0400776B RID: 30571
		[Tooltip("Maximum duration when using random")]
		public float maxDuration = 3f;
	}
}
