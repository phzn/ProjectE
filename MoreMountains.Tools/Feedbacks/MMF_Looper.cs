using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F4 RID: 500
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will move the current 'head' of an MMFeedbacks sequence back to another feedback above in the list. What feedback the head lands on depends on your settings : you can decide to have it loop at last pause, or at the last LoopStart feedback in the list (or both). Furthermore, you can decide to have it loop multiple times and cause a pause when met.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Loop/Looper")]
	public class MMF_Looper : MMF_Pause
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00038B3B File Offset: 0x00036D3B
		public override bool LooperPause
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00038B3E File Offset: 0x00036D3E
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x00038B4C File Offset: 0x00036D4C
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.PauseDuration);
			}
			set
			{
				this.PauseDuration = value;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00038B55 File Offset: 0x00036D55
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.InInfiniteLoop = this.InfiniteLoop;
			this.NumberOfLoopsLeft = this.NumberOfLoops;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00038B78 File Offset: 0x00036D78
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active)
			{
				this.ProcessNewPauseDuration();
				this.InInfiniteLoop = this.InfiniteLoop;
				this.NumberOfLoopsLeft--;
				this.Owner.StartCoroutine(this.PlayPause());
				this.TriggerOnLoop(this.Owner);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00038BCB File Offset: 0x00036DCB
		public virtual void TriggerOnLoop(MMFeedbacks source)
		{
			this.OnLoop.Invoke();
			if (this.TriggerMMFeedbacksEvents)
			{
				MMFeedbacksEvent.Trigger(source, MMFeedbacksEvent.EventTypes.Loop);
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00038BE7 File Offset: 0x00036DE7
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.InInfiniteLoop = false;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00038BF8 File Offset: 0x00036DF8
		protected override void CustomReset()
		{
			base.CustomReset();
			this.InInfiniteLoop = this.InfiniteLoop;
			this.NumberOfLoopsLeft = this.NumberOfLoops;
		}

		// Token: 0x04000EF3 RID: 3827
		[MMFInspectorGroup("Loop", true, 34, false, false)]
		[Header("Loop conditions")]
		[Tooltip("if this is true, this feedback, when met, will cause the MMFeedbacks to reposition its 'head' to the first pause found above it (going from this feedback to the top), or to the start if none is found")]
		public bool LoopAtLastPause = true;

		// Token: 0x04000EF4 RID: 3828
		[Tooltip("if this is true, this feedback, when met, will cause the MMFeedbacks to reposition its 'head' to the first LoopStart feedback found above it (going from this feedback to the top), or to the start if none is found")]
		public bool LoopAtLastLoopStart = true;

		// Token: 0x04000EF5 RID: 3829
		[Header("Loop")]
		[Tooltip("if this is true, the looper will loop forever")]
		public bool InfiniteLoop;

		// Token: 0x04000EF6 RID: 3830
		[Tooltip("how many times this loop should run")]
		[MMCondition("InfiniteLoop", true, true)]
		public int NumberOfLoops = 2;

		// Token: 0x04000EF7 RID: 3831
		[Tooltip("the amount of loops left (updated at runtime)")]
		[MMFReadOnly]
		public int NumberOfLoopsLeft = 1;

		// Token: 0x04000EF8 RID: 3832
		[Tooltip("whether we are in an infinite loop at this time or not")]
		[MMFReadOnly]
		public bool InInfiniteLoop;

		// Token: 0x04000EF9 RID: 3833
		[Tooltip("whether or not to trigger a Loop MMFeedbacksEvent when this looper is reached")]
		public bool TriggerMMFeedbacksEvents = true;

		// Token: 0x04000EFA RID: 3834
		[Header("Events")]
		[Tooltip("a Unity Event to invoke when the looper is reached")]
		public UnityEvent OnLoop;
	}
}
