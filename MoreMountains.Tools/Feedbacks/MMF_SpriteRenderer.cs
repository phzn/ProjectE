using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020E RID: 526
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the color of a target sprite renderer over time, and flip it on X or Y. You can also use it to command one or many MMSpriteRendererShakers.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/SpriteRenderer")]
	public class MMF_SpriteRenderer : MMF_Feedback
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0003F6F0 File Offset: 0x0003D8F0
		// (set) Token: 0x06000E40 RID: 3648 RVA: 0x0003F70D File Offset: 0x0003D90D
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_SpriteRenderer.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x0003F716 File Offset: 0x0003D916
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003F719 File Offset: 0x0003D919
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0003F71C File Offset: 0x0003D91C
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0003F71F File Offset: 0x0003D91F
		protected override void AutomateTargetAcquisition()
		{
			this.BoundSpriteRenderer = this.FindAutomatedTarget<SpriteRenderer>();
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0003F730 File Offset: 0x0003D930
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.EventOriginTransform == null)
			{
				this.EventOriginTransform = this.Owner.transform;
			}
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
			if (this.BoundSpriteRenderer != null && this.InitialColorMode == MMF_SpriteRenderer.InitialColorModes.InitialColorOnInit)
			{
				this._initialColor = this.BoundSpriteRenderer.color;
				this._initialFlipX = this.BoundSpriteRenderer.flipX;
				this._initialFlipY = this.BoundSpriteRenderer.flipY;
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0003F7C4 File Offset: 0x0003D9C4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpriteRenderer.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.BoundSpriteRenderer != null && this.InitialColorMode == MMF_SpriteRenderer.InitialColorModes.InitialColorOnPlay)
			{
				this._initialColor = this.BoundSpriteRenderer.color;
				this._initialFlipX = this.BoundSpriteRenderer.flipX;
				this._initialFlipY = this.BoundSpriteRenderer.flipY;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.Turn(true);
			switch (this.Mode)
			{
			case MMF_SpriteRenderer.Modes.OverTime:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.SpriteRendererSequence());
				return;
			case MMF_SpriteRenderer.Modes.Instant:
				if (this.ModifyColor)
				{
					this.BoundSpriteRenderer.color = this.InstantColor;
				}
				this.Flip();
				return;
			case MMF_SpriteRenderer.Modes.ShakerEvent:
				MMSpriteRendererShakeEvent.Trigger(this.FeedbackDuration, this.ModifyColor, this.ColorOverTime, this.FlipX, this.FlipY, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.OnlyBroadcastInRange, this.EventRange, this.EventOriginTransform.position);
				return;
			case MMF_SpriteRenderer.Modes.ToDestinationColor:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.SpriteRendererToDestinationSequence(false));
				return;
			case MMF_SpriteRenderer.Modes.ToDestinationColorAndBack:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.SpriteRendererToDestinationSequence(true));
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0003F992 File Offset: 0x0003DB92
		protected virtual IEnumerator SpriteRendererSequence()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			this.Flip();
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetSpriteRendererValues(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetSpriteRendererValues(this.FinalNormalizedTime);
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0003F9A1 File Offset: 0x0003DBA1
		protected virtual IEnumerator SpriteRendererToDestinationSequence(bool andBack)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			this.Flip();
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				if (andBack)
				{
					num = ((num < 0.5f) ? MMFeedbacksHelpers.Remap(num, 0f, 0.5f, 0f, 1f) : MMFeedbacksHelpers.Remap(num, 0.5f, 1f, 1f, 0f));
				}
				float num2 = this.ToDestinationColorCurve.Evaluate(num);
				if (this.ModifyColor)
				{
					this.BoundSpriteRenderer.color = Color.LerpUnclamped(this._initialColor, this.ToDestinationColor, num2);
				}
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			if (this.ModifyColor)
			{
				this.BoundSpriteRenderer.color = (andBack ? this._initialColor : this.ToDestinationColor);
			}
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0003F9B8 File Offset: 0x0003DBB8
		protected virtual void Flip()
		{
			if (this.FlipX)
			{
				this.BoundSpriteRenderer.flipX = !this.BoundSpriteRenderer.flipX;
			}
			if (this.FlipY)
			{
				this.BoundSpriteRenderer.flipY = !this.BoundSpriteRenderer.flipY;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0003FA07 File Offset: 0x0003DC07
		protected virtual void SetSpriteRendererValues(float time)
		{
			if (this.ModifyColor)
			{
				this.BoundSpriteRenderer.color = this.ColorOverTime.Evaluate(time);
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0003FA28 File Offset: 0x0003DC28
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SpriteRenderer.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.Owner.StopCoroutine(this._coroutine);
			this.IsPlaying = false;
			this._coroutine = null;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0003FA74 File Offset: 0x0003DC74
		protected virtual void Turn(bool status)
		{
			this.BoundSpriteRenderer.gameObject.SetActive(status);
			this.BoundSpriteRenderer.enabled = status;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0003FA94 File Offset: 0x0003DC94
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SpriteRenderer.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.BoundSpriteRenderer != null)
			{
				this.BoundSpriteRenderer.color = this._initialColor;
				this.BoundSpriteRenderer.flipX = this._initialFlipX;
				this.BoundSpriteRenderer.flipY = this._initialFlipY;
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0003FAF2 File Offset: 0x0003DCF2
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x04001084 RID: 4228
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001085 RID: 4229
		[MMFInspectorGroup("Sprite Renderer", true, 51, true, false)]
		[Tooltip("the SpriteRenderer to affect when playing the feedback")]
		public SpriteRenderer BoundSpriteRenderer;

		// Token: 0x04001086 RID: 4230
		[Tooltip("whether the feedback should affect the sprite renderer instantly or over a period of time")]
		public MMF_SpriteRenderer.Modes Mode;

		// Token: 0x04001087 RID: 4231
		[Tooltip("how long the sprite renderer should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2, 3, 4 })]
		public float Duration = 0.2f;

		// Token: 0x04001088 RID: 4232
		[Tooltip("whether or not that sprite renderer should be turned off on start")]
		public bool StartsOff;

		// Token: 0x04001089 RID: 4233
		[Tooltip("whether or not to reset shaker values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x0400108A RID: 4234
		[Tooltip("whether or not to reset the target's values after shake")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x0400108B RID: 4235
		[Tooltip("whether or not to broadcast a range to only affect certain shakers")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool OnlyBroadcastInRange;

		// Token: 0x0400108C RID: 4236
		[Tooltip("the range of the event, in units")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float EventRange = 100f;

		// Token: 0x0400108D RID: 4237
		[Tooltip("the transform to use to broadcast the event as origin point")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Transform EventOriginTransform;

		// Token: 0x0400108E RID: 4238
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x0400108F RID: 4239
		[Tooltip("whether to grab the initial color to (potentially) go back to at init or when the feedback plays")]
		public MMF_SpriteRenderer.InitialColorModes InitialColorMode = MMF_SpriteRenderer.InitialColorModes.InitialColorOnPlay;

		// Token: 0x04001090 RID: 4240
		[MMFInspectorGroup("Color", true, 52, false, false)]
		[Tooltip("whether or not to modify the color of the sprite renderer")]
		public bool ModifyColor = true;

		// Token: 0x04001091 RID: 4241
		[Tooltip("the colors to apply to the sprite renderer over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public Gradient ColorOverTime;

		// Token: 0x04001092 RID: 4242
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public Color InstantColor;

		// Token: 0x04001093 RID: 4243
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1, 3, 4 })]
		public Color ToDestinationColor = Color.red;

		// Token: 0x04001094 RID: 4244
		[Tooltip("the color to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1, 3, 4 })]
		public AnimationCurve ToDestinationColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04001095 RID: 4245
		[MMFInspectorGroup("Flip", true, 53, false, false)]
		[Tooltip("whether or not to flip the sprite on X")]
		public bool FlipX;

		// Token: 0x04001096 RID: 4246
		[Tooltip("whether or not to flip the sprite on Y")]
		public bool FlipY;

		// Token: 0x04001097 RID: 4247
		protected Coroutine _coroutine;

		// Token: 0x04001098 RID: 4248
		protected Color _initialColor;

		// Token: 0x04001099 RID: 4249
		protected bool _initialFlipX;

		// Token: 0x0400109A RID: 4250
		protected bool _initialFlipY;

		// Token: 0x020004EF RID: 1263
		public enum Modes
		{
			// Token: 0x0400230E RID: 8974
			OverTime,
			// Token: 0x0400230F RID: 8975
			Instant,
			// Token: 0x04002310 RID: 8976
			ShakerEvent,
			// Token: 0x04002311 RID: 8977
			ToDestinationColor,
			// Token: 0x04002312 RID: 8978
			ToDestinationColorAndBack
		}

		// Token: 0x020004F0 RID: 1264
		public enum InitialColorModes
		{
			// Token: 0x04002314 RID: 8980
			InitialColorOnInit,
			// Token: 0x04002315 RID: 8981
			InitialColorOnPlay
		}
	}
}
