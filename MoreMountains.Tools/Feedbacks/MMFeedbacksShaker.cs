using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000246 RID: 582
	[RequireComponent(typeof(MMFeedbacks))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Feedbacks/MMFeedbacksShaker")]
	public class MMFeedbacksShaker : MMShaker
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x000473D7 File Offset: 0x000455D7
		protected override void Initialization()
		{
			base.Initialization();
			this._mmFeedbacks = base.gameObject.GetComponent<MMFeedbacks>();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x000473F0 File Offset: 0x000455F0
		public virtual void OnMMFeedbacksShakeEvent(MMChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			if (!this.CheckEventAllowed(channelData, useRange, eventRange, eventOriginPosition) || (!this.Interruptible && this.Shaking))
			{
				return;
			}
			this.Play();
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00047416 File Offset: 0x00045616
		protected override void ShakeStarts()
		{
			this._mmFeedbacks.PlayFeedbacks();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00047423 File Offset: 0x00045623
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.01f;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00047430 File Offset: 0x00045630
		public override void StartListening()
		{
			base.StartListening();
			MMFeedbacksShakeEvent.Register(new MMFeedbacksShakeEvent.Delegate(this.OnMMFeedbacksShakeEvent));
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0004744A File Offset: 0x0004564A
		public override void StopListening()
		{
			base.StopListening();
			MMFeedbacksShakeEvent.Unregister(new MMFeedbacksShakeEvent.Delegate(this.OnMMFeedbacksShakeEvent));
		}

		// Token: 0x0400127E RID: 4734
		protected MMFeedbacks _mmFeedbacks;
	}
}
