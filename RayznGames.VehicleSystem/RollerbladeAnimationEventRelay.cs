using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000003 RID: 3
	public class RollerbladeAnimationEventRelay : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public void Bind(RollerbladeAudioController controller)
		{
			this.boundController = controller;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C9 File Offset: 0x000002C9
		public void Unbind(RollerbladeAudioController controller)
		{
			if (this.boundController == controller)
			{
				this.boundController = null;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E0 File Offset: 0x000002E0
		public void AE_RollerbladeStep(AnimationEvent evt)
		{
			RollerbladeAudioController rollerbladeAudioController = this.boundController;
			if (rollerbladeAudioController == null)
			{
				return;
			}
			rollerbladeAudioController.PlayStepFromAnimationEvent(evt);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F3 File Offset: 0x000002F3
		public void AE_RollerbladeStepWithIntensity(AnimationEvent evt)
		{
			RollerbladeAudioController rollerbladeAudioController = this.boundController;
			if (rollerbladeAudioController == null)
			{
				return;
			}
			rollerbladeAudioController.PlayStepWithIntensityFromAnimationEvent(evt);
		}

		// Token: 0x04000001 RID: 1
		private RollerbladeAudioController boundController;
	}
}
