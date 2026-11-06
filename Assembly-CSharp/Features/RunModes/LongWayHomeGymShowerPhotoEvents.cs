using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CBD RID: 3261
	[DisallowMultipleComponent]
	public sealed class LongWayHomeGymShowerPhotoEvents : MonoBehaviour
	{
		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06005EA0 RID: 24224 RVA: 0x002571A8 File Offset: 0x002553A8
		// (remove) Token: 0x06005EA1 RID: 24225 RVA: 0x002571E0 File Offset: 0x002553E0
		public event Action Captured;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06005EA2 RID: 24226 RVA: 0x00257218 File Offset: 0x00255418
		// (remove) Token: 0x06005EA3 RID: 24227 RVA: 0x00257250 File Offset: 0x00255450
		public event Action Footstep;

		// Token: 0x06005EA4 RID: 24228 RVA: 0x00257285 File Offset: 0x00255485
		public void TriggerFlash()
		{
			Action captured = this.Captured;
			if (captured == null)
			{
				return;
			}
			captured();
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x00257285 File Offset: 0x00255485
		public void TriggerFlashWithMultiplier(float multiplier)
		{
			Action captured = this.Captured;
			if (captured == null)
			{
				return;
			}
			captured();
		}

		// Token: 0x06005EA6 RID: 24230 RVA: 0x00257297 File Offset: 0x00255497
		public void PlayFootstep(AnimationEvent animationEvent)
		{
			Action footstep = this.Footstep;
			if (footstep == null)
			{
				return;
			}
			footstep();
		}

		// Token: 0x06005EA7 RID: 24231 RVA: 0x00004F31 File Offset: 0x00003131
		public void StopFlash()
		{
		}

		// Token: 0x06005EA8 RID: 24232 RVA: 0x00004F31 File Offset: 0x00003131
		public void StartVideoLight()
		{
		}

		// Token: 0x06005EA9 RID: 24233 RVA: 0x00004F31 File Offset: 0x00003131
		public void StopVideoLight()
		{
		}
	}
}
