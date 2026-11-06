using System;
using UnityEngine;

namespace Features.Inventory.Audio
{
	// Token: 0x02001160 RID: 4448
	public class DildoAnimationEventRelay : MonoBehaviour
	{
		// Token: 0x06008777 RID: 34679 RVA: 0x0037709E File Offset: 0x0037529E
		public void Bind(DildoAudioController controller)
		{
			this._boundController = controller;
			if (this.debugLogs)
			{
				Debug.Log("[DildoAnimationEventRelay] Bound to controller on " + controller.gameObject.name);
			}
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x003770C9 File Offset: 0x003752C9
		public void Unbind(DildoAudioController controller)
		{
			if (this._boundController == controller)
			{
				this._boundController = null;
				if (this.debugLogs)
				{
					Debug.Log("[DildoAnimationEventRelay] Unbound from controller");
				}
			}
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x003770F4 File Offset: 0x003752F4
		public void AE_DildoInteraction(AnimationEvent evt)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoAnimationEventRelay] AE_DildoInteraction called | Frame:{0} | Weight:{1:F2}", Time.frameCount, (evt != null) ? evt.animatorClipInfo.weight : 0f));
			}
			if (this._boundController == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[DildoAnimationEventRelay] No bound controller - interaction sound skipped. Check DildoActionExecutor.animationEventRelay assignment.");
				}
				return;
			}
			this._boundController.PlayInteractionFromAnimationEvent(evt);
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x0037716C File Offset: 0x0037536C
		public void AE_DildoInteractionWithIntensity(AnimationEvent evt)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoAnimationEventRelay] AE_DildoInteractionWithIntensity called | Frame:{0} | Intensity:{1:F2} | Weight:{2:F2}", Time.frameCount, (evt != null) ? evt.floatParameter : 0f, (evt != null) ? evt.animatorClipInfo.weight : 0f));
			}
			if (this._boundController == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[DildoAnimationEventRelay] No bound controller - interaction sound skipped. Check DildoActionExecutor.animationEventRelay assignment.");
				}
				return;
			}
			this._boundController.PlayInteractionWithIntensityFromAnimationEvent(evt);
		}

		// Token: 0x04008A15 RID: 35349
		private DildoAudioController _boundController;

		// Token: 0x04008A16 RID: 35350
		[SerializeField]
		private bool debugLogs;
	}
}
