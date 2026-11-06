using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public class MMFeedbacksEvents
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002F4BD File Offset: 0x0002D6BD
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002F4C5 File Offset: 0x0002D6C5
		public virtual bool OnPlayIsNull { get; protected set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0002F4CE File Offset: 0x0002D6CE
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0002F4D6 File Offset: 0x0002D6D6
		public virtual bool OnPauseIsNull { get; protected set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002F4DF File Offset: 0x0002D6DF
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0002F4E7 File Offset: 0x0002D6E7
		public virtual bool OnResumeIsNull { get; protected set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0002F4F0 File Offset: 0x0002D6F0
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0002F4F8 File Offset: 0x0002D6F8
		public virtual bool OnRevertIsNull { get; protected set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0002F501 File Offset: 0x0002D701
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0002F509 File Offset: 0x0002D709
		public virtual bool OnCompleteIsNull { get; protected set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002F512 File Offset: 0x0002D712
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0002F51A File Offset: 0x0002D71A
		public virtual bool OnRestoreInitialValuesIsNull { get; protected set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0002F523 File Offset: 0x0002D723
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0002F52B File Offset: 0x0002D72B
		public virtual bool OnSkipToTheEndIsNull { get; protected set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0002F534 File Offset: 0x0002D734
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0002F53C File Offset: 0x0002D73C
		public virtual bool OnInitializationCompleteIsNull { get; protected set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0002F545 File Offset: 0x0002D745
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0002F54D File Offset: 0x0002D74D
		public virtual bool OnEnableIsNull { get; protected set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0002F556 File Offset: 0x0002D756
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0002F55E File Offset: 0x0002D75E
		public virtual bool OnDisableIsNull { get; protected set; }

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002F568 File Offset: 0x0002D768
		public virtual void Initialization()
		{
			this.OnPlayIsNull = this.OnPlay == null;
			this.OnPauseIsNull = this.OnPause == null;
			this.OnResumeIsNull = this.OnResume == null;
			this.OnRevertIsNull = this.OnRevert == null;
			this.OnCompleteIsNull = this.OnComplete == null;
			this.OnRestoreInitialValuesIsNull = this.OnRestoreInitialValues == null;
			this.OnSkipToTheEndIsNull = this.OnSkipToTheEnd == null;
			this.OnInitializationCompleteIsNull = this.OnInitializationComplete == null;
			this.OnEnableIsNull = this.OnEnable == null;
			this.OnDisableIsNull = this.OnDisable == null;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002F60B File Offset: 0x0002D80B
		public virtual void TriggerOnPlay(MMFeedbacks source)
		{
			if (!this.OnPlayIsNull && this.TriggerUnityEvents)
			{
				this.OnPlay.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Play);
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002F637 File Offset: 0x0002D837
		public virtual void TriggerOnPause(MMFeedbacks source)
		{
			if (!this.OnPauseIsNull && this.TriggerUnityEvents)
			{
				this.OnPause.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Pause);
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0002F663 File Offset: 0x0002D863
		public virtual void TriggerOnResume(MMFeedbacks source)
		{
			if (!this.OnResumeIsNull && this.TriggerUnityEvents)
			{
				this.OnResume.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Resume);
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0002F68F File Offset: 0x0002D88F
		public virtual void TriggerOnRevert(MMFeedbacks source)
		{
			if (!this.OnRevertIsNull && this.TriggerUnityEvents)
			{
				this.OnRevert.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Revert);
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002F6BB File Offset: 0x0002D8BB
		public virtual void TriggerOnComplete(MMFeedbacks source)
		{
			if (!this.OnCompleteIsNull && this.TriggerUnityEvents)
			{
				this.OnComplete.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Complete);
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002F6E7 File Offset: 0x0002D8E7
		public virtual void TriggerOnSkipToTheEnd(MMFeedbacks source)
		{
			if (!this.OnSkipToTheEndIsNull && this.TriggerUnityEvents)
			{
				this.OnSkipToTheEnd.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.SkipToTheEnd);
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002F713 File Offset: 0x0002D913
		public virtual void TriggerOnInitializationComplete(MMFeedbacks source)
		{
			if (!this.OnInitializationCompleteIsNull && this.TriggerUnityEvents)
			{
				this.OnInitializationComplete.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.InitializationComplete);
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002F740 File Offset: 0x0002D940
		public virtual void TriggerOnRestoreInitialValues(MMFeedbacks source)
		{
			if (!this.OnRestoreInitialValuesIsNull && this.TriggerUnityEvents)
			{
				this.OnRestoreInitialValues.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.RestoreInitialValues);
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0002F76C File Offset: 0x0002D96C
		public virtual void TriggerOnEnable(MMF_Player source)
		{
			if (!this.OnEnableIsNull && this.TriggerUnityEvents)
			{
				this.OnEnable.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Enable);
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0002F798 File Offset: 0x0002D998
		public virtual void TriggerOnDisable(MMF_Player source)
		{
			if (!this.OnDisableIsNull && this.TriggerUnityEvents)
			{
				this.OnDisable.Invoke();
			}
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Disable);
			}
		}

		// Token: 0x04000C70 RID: 3184
		[Tooltip("whether or not this MMFeedbacks should fire MMFeedbacksEvents")]
		public bool TriggerMMFeedbacksEvents;

		// Token: 0x04000C71 RID: 3185
		[Tooltip("whether or not this MMFeedbacks should fire Unity Events")]
		public bool TriggerUnityEvents = true;

		// Token: 0x04000C72 RID: 3186
		[Tooltip("This event will fire every time this MMFeedbacks gets played")]
		public UnityEvent OnPlay;

		// Token: 0x04000C73 RID: 3187
		[Tooltip("This event will fire every time this MMFeedbacks starts a holding pause")]
		public UnityEvent OnPause;

		// Token: 0x04000C74 RID: 3188
		[Tooltip("This event will fire every time this MMFeedbacks resumes after a holding pause")]
		public UnityEvent OnResume;

		// Token: 0x04000C75 RID: 3189
		[Tooltip("This event will fire every time this MMFeedbacks reverts its play direction")]
		public UnityEvent OnRevert;

		// Token: 0x04000C76 RID: 3190
		[Tooltip("This event will fire every time this MMFeedbacks plays its last MMFeedback")]
		public UnityEvent OnComplete;

		// Token: 0x04000C77 RID: 3191
		[Tooltip("This event will fire every time this MMFeedbacks gets restored to its initial values")]
		public UnityEvent OnRestoreInitialValues;

		// Token: 0x04000C78 RID: 3192
		[Tooltip("This event will fire every time this MMFeedbacks gets skipped to the end")]
		public UnityEvent OnSkipToTheEnd;

		// Token: 0x04000C79 RID: 3193
		[Tooltip("This event will fire after the MMF Player is done initializing")]
		public UnityEvent OnInitializationComplete;

		// Token: 0x04000C7A RID: 3194
		[Tooltip("This event will fire every time this MMFeedbacks' game object gets enabled")]
		public UnityEvent OnEnable;

		// Token: 0x04000C7B RID: 3195
		[Tooltip("This event will fire every time this MMFeedbacks' game object gets disabled")]
		public UnityEvent OnDisable;
	}
}
