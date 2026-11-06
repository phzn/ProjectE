using System;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x020004ED RID: 1261
	public class vThrowAnimatorEvent : StateMachineBehaviour
	{
		// Token: 0x06001E6D RID: 7789 RVA: 0x000B796B File Offset: 0x000B5B6B
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.manager = animator.GetComponentInChildren<vThrowManagerBase>();
			this.isTrigger = false;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000B7980 File Offset: 0x000B5B80
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.normalizedTime >= this.time && !this.isTrigger)
			{
				this.OnTrigger();
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000B799F File Offset: 0x000B5B9F
		protected virtual void OnTrigger()
		{
			this.isTrigger = true;
			this.manager.TriggerAnimatorEvent(this.eventType);
		}

		// Token: 0x04001F46 RID: 8006
		public vThrowAnimatorEvent.ThrowEventType eventType;

		// Token: 0x04001F47 RID: 8007
		[Range(0f, 1f)]
		public float time;

		// Token: 0x04001F48 RID: 8008
		private bool isTrigger;

		// Token: 0x04001F49 RID: 8009
		private vThrowManagerBase manager;

		// Token: 0x020004EE RID: 1262
		public enum ThrowEventType
		{
			// Token: 0x04001F4B RID: 8011
			EquipThrowable,
			// Token: 0x04001F4C RID: 8012
			EnableAiming,
			// Token: 0x04001F4D RID: 8013
			CancelAiming,
			// Token: 0x04001F4E RID: 8014
			StartLaunch,
			// Token: 0x04001F4F RID: 8015
			FinishLaunch
		}
	}
}
