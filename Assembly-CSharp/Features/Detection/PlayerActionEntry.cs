using System;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012CE RID: 4814
	[Serializable]
	public class PlayerActionEntry
	{
		// Token: 0x04009A38 RID: 39480
		[Tooltip("Unique identifier for this action (e.g. 'Shame', 'Exhibit', 'Wave')")]
		public string ActionId;

		// Token: 0x04009A39 RID: 39481
		[Tooltip("Display name shown in UI")]
		public string DisplayName;

		// Token: 0x04009A3A RID: 39482
		[Tooltip("How long the action sequence lasts before returning to gameplay")]
		public float Duration = 3f;

		// Token: 0x04009A3B RID: 39483
		[Header("Invector Configuration")]
		[Tooltip("Prefab containing the vTriggerGenericAction configuration for this action")]
		public vTriggerGenericAction TriggerPrefab;

		// Token: 0x04009A3C RID: 39484
		[Header("Exit Flow")]
		[Tooltip("If true, on action end the system does NOT crossfade to Locomotion. It lets the Animator graph follow its natural transitions (e.g. PoliceTaser1 → PoliceTaser1Ending → Exit) and waits until the 'CustomAction' animator tag clears before unlocking input and restoring layers. Use for actions whose graph already has a baked wind-down state (rise-up, recovery, etc.).")]
		public bool WaitForExitState;
	}
}
