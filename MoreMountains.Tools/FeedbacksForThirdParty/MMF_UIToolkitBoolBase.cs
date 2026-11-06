using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000332 RID: 818
	[AddComponentMenu("")]
	public class MMF_UIToolkitBoolBase : MMF_UIToolkit
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0005F4A5 File Offset: 0x0005D6A5
		public override float FeedbackDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x0005F4AC File Offset: 0x0005D6AC
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x0005F4AF File Offset: 0x0005D6AF
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this._visualElements == null || this._visualElements.Count == 0)
			{
				return;
			}
			this._initialValue = this.GetInitialValue();
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0005F4DA File Offset: 0x0005D6DA
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._visualElements == null || this._visualElements.Count == 0)
			{
				return;
			}
			this.SetValue();
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x0005F508 File Offset: 0x0005D708
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x0005F529 File Offset: 0x0005D729
		protected virtual void SetValue()
		{
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x0005F52B File Offset: 0x0005D72B
		protected virtual void SetValue(bool newValue)
		{
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x0005F52D File Offset: 0x0005D72D
		protected virtual bool GetInitialValue()
		{
			return false;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0005F530 File Offset: 0x0005D730
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SetValue(this._initialValue);
		}

		// Token: 0x040019F5 RID: 6645
		protected bool _initialValue;
	}
}
