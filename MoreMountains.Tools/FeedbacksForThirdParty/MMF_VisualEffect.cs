using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.VFX;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200036D RID: 877
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you apply basic controls to a target VisualEffect")]
	[FeedbackPath("Particles/VisualEffect")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.VisualEffectGraph", null)]
	public class MMF_VisualEffect : MMF_Feedback
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x00066808 File Offset: 0x00064A08
		// (set) Token: 0x06001822 RID: 6178 RVA: 0x00066816 File Offset: 0x00064A16
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.DeclaredDuration);
			}
			set
			{
				this.DeclaredDuration = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x0006681F File Offset: 0x00064A1F
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x00066822 File Offset: 0x00064A22
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00066825 File Offset: 0x00064A25
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.StopVisualEffectOnInit)
			{
				this.StopVisualEffect();
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0006683C File Offset: 0x00064A3C
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1f)
		{
			if (!this.Active || !MMF_VisualEffect.FeedbackTypeAuthorized || this.TargetVisualEffect == null)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_VisualEffect.Modes.Play:
				this.TargetVisualEffect.Play();
				return;
			case MMF_VisualEffect.Modes.Stop:
				this.StopVisualEffect();
				return;
			case MMF_VisualEffect.Modes.Pause:
				this.TargetVisualEffect.pause = true;
				return;
			case MMF_VisualEffect.Modes.Unpause:
				this.TargetVisualEffect.pause = false;
				return;
			case MMF_VisualEffect.Modes.AdvanceOneFrame:
				this.TargetVisualEffect.AdvanceOneFrame();
				return;
			case MMF_VisualEffect.Modes.Reinit:
				this.TargetVisualEffect.Reinit();
				return;
			case MMF_VisualEffect.Modes.SetPlayRate:
				this.TargetVisualEffect.playRate = this.NewPlayRate;
				return;
			case MMF_VisualEffect.Modes.Simulate:
				this.TargetVisualEffect.Simulate(this.StepDeltaTime, this.StepCount);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00066903 File Offset: 0x00064B03
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_VisualEffect.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.StopVisualEffectOnStopFeedback)
			{
				this.StopVisualEffect();
			}
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0006692B File Offset: 0x00064B2B
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.InCooldown)
			{
				return;
			}
			if (this.StopVisualEffectOnReset)
			{
				this.StopVisualEffect();
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0006694A File Offset: 0x00064B4A
		protected virtual void StopVisualEffect()
		{
			if (this.TargetVisualEffect == null)
			{
				return;
			}
			this.TargetVisualEffect.Stop();
		}

		// Token: 0x04001BE8 RID: 7144
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001BE9 RID: 7145
		[MMFInspectorGroup("Visual Effect", true, 41, false, false)]
		[Tooltip("the duration for the player to consider. This won't impact your visual effect, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual visual effect, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04001BEA RID: 7146
		[Tooltip("the visual effect to control when playing this feedback")]
		public VisualEffect TargetVisualEffect;

		// Token: 0x04001BEB RID: 7147
		[Tooltip("the selected mode, the instruction to send to the target visual effect when playing this feedback")]
		public MMF_VisualEffect.Modes Mode;

		// Token: 0x04001BEC RID: 7148
		[Tooltip("when in SetPlayRate mode, the new play rate to apply")]
		[MMFEnumCondition("Mode", new int[] { 6 })]
		public float NewPlayRate = 1f;

		// Token: 0x04001BED RID: 7149
		[Tooltip("when in Simulate mode, the delta time to use")]
		[MMFEnumCondition("Mode", new int[] { 7 })]
		public float StepDeltaTime = 1f;

		// Token: 0x04001BEE RID: 7150
		[Tooltip("when in Simulate mode, the number of steps to simulate")]
		[MMFEnumCondition("Mode", new int[] { 7 })]
		public uint StepCount = 5U;

		// Token: 0x04001BEF RID: 7151
		[Tooltip("whether or not to stop the visual effect when stopping this feedback")]
		public bool StopVisualEffectOnStopFeedback;

		// Token: 0x04001BF0 RID: 7152
		[Tooltip("whether or not to stop the visual effect when resetting this feedback")]
		public bool StopVisualEffectOnReset;

		// Token: 0x04001BF1 RID: 7153
		[Tooltip("whether or not to stop the visual effect when initializing this feedback")]
		public bool StopVisualEffectOnInit;

		// Token: 0x04001BF2 RID: 7154
		protected VFXEventAttribute _eventAttribute;

		// Token: 0x02000594 RID: 1428
		public enum Modes
		{
			// Token: 0x040024EE RID: 9454
			Play,
			// Token: 0x040024EF RID: 9455
			Stop,
			// Token: 0x040024F0 RID: 9456
			Pause,
			// Token: 0x040024F1 RID: 9457
			Unpause,
			// Token: 0x040024F2 RID: 9458
			AdvanceOneFrame,
			// Token: 0x040024F3 RID: 9459
			Reinit,
			// Token: 0x040024F4 RID: 9460
			SetPlayRate,
			// Token: 0x040024F5 RID: 9461
			Simulate
		}
	}
}
