using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AB RID: 683
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you target (almost) any property, on any object in your scene. It also works on scriptable objects. Drag an object, select a property, and setup your feedback to update that property over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("GameObject/Property")]
	public class MMF_Property : MMF_Feedback
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x00052B43 File Offset: 0x00050D43
		// (set) Token: 0x06001326 RID: 4902 RVA: 0x00052B60 File Offset: 0x00050D60
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_Property.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				if (this.Mode != MMF_Property.Modes.Instant)
				{
					this.Duration = value;
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x00052B72 File Offset: 0x00050D72
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00052B75 File Offset: 0x00050D75
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x00052B78 File Offset: 0x00050D78
		public override bool ForceInitialValueDelayed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x00052B7B File Offset: 0x00050D7B
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00052B7E File Offset: 0x00050D7E
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.Target.Initialization(this.Owner.gameObject);
			this.GetInitialIntensity();
			if (this.Active && this.StartsOff)
			{
				this.Turn(false);
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00052BBA File Offset: 0x00050DBA
		protected virtual void GetInitialIntensity()
		{
			this._initialIntensity = this.Target.Level;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00052BD0 File Offset: 0x00050DD0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Property.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.DetermineInitialValueOnPlay)
			{
				this.GetInitialIntensity();
			}
			this.Turn(true);
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			switch (this.Mode)
			{
			case MMF_Property.Modes.OverTime:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.UpdateValueSequence(num));
				return;
			case MMF_Property.Modes.Instant:
				this.Target.SetLevel(this.InstantLevel);
				return;
			case MMF_Property.Modes.ToDestination:
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ToDestinationSequence(num));
				return;
			default:
				return;
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00052CAB File Offset: 0x00050EAB
		protected virtual IEnumerator ToDestinationSequence(float intensityMultiplier)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			float initialValue = this.Target.Level;
			float destinationValue = this.ToDestinationLevel;
			if (this.RelativeValues)
			{
				destinationValue += this._initialIntensity;
			}
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValues(num, intensityMultiplier, initialValue, destinationValue, false);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValues(this.FinalNormalizedTime, intensityMultiplier, initialValue, destinationValue, false);
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00052CC1 File Offset: 0x00050EC1
		protected virtual IEnumerator UpdateValueSequence(float intensityMultiplier)
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetValues(num, intensityMultiplier, this.RemapLevelZero, this.RemapLevelOne, true);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetValues(this.FinalNormalizedTime, intensityMultiplier, this.RemapLevelZero, this.RemapLevelOne, true);
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00052CD8 File Offset: 0x00050ED8
		protected virtual void SetValues(float time, float intensityMultiplier, float remapZero, float remapOne, bool applyRelative)
		{
			float num = MMTween.Tween(time, 0f, 1f, remapZero, remapOne, this.LevelCurve);
			num *= intensityMultiplier;
			if (applyRelative && this.RelativeValues)
			{
				num += this._initialIntensity;
			}
			this.Target.SetLevel(num);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00052D24 File Offset: 0x00050F24
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active)
			{
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
					this._coroutine = null;
					this.Target.SetLevel(this._initialIntensity);
				}
				if (this.StartsOff)
				{
					this.Turn(false);
				}
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00052D81 File Offset: 0x00050F81
		protected virtual void Turn(bool status)
		{
			if (this.Target.TargetComponent.gameObject != null)
			{
				this.Target.TargetComponent.gameObject.SetActive(status);
			}
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00052DB1 File Offset: 0x00050FB1
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Property.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.StartsOff)
			{
				this.Turn(false);
			}
			this.Target.SetLevel(this._initialIntensity);
		}

		// Token: 0x04001617 RID: 5655
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001618 RID: 5656
		[MMFInspectorGroup("Target Property", true, 12, false, false)]
		[Tooltip("the receiver to write the level to")]
		public MMPropertyReceiver Target;

		// Token: 0x04001619 RID: 5657
		[MMFInspectorGroup("Mode", true, 29, false, false)]
		[Tooltip("whether the feedback should affect the target property instantly or over a period of time")]
		public MMF_Property.Modes Mode;

		// Token: 0x0400161A RID: 5658
		[Tooltip("how long the target property should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float Duration = 0.2f;

		// Token: 0x0400161B RID: 5659
		[Tooltip("whether or not that target property should be turned off on start")]
		public bool StartsOff;

		// Token: 0x0400161C RID: 5660
		[Tooltip("whether or not the values should be relative or not")]
		public bool RelativeValues = true;

		// Token: 0x0400161D RID: 5661
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x0400161E RID: 5662
		[Tooltip("if this is true, initial value will be computed for every play, otherwise only once, on initialization")]
		public bool DetermineInitialValueOnPlay;

		// Token: 0x0400161F RID: 5663
		[MMFInspectorGroup("Level", true, 30, false, false)]
		[Tooltip("the curve to tween the intensity on")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public MMTweenType LevelCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001620 RID: 5664
		[Tooltip("the value to remap the intensity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapLevelZero;

		// Token: 0x04001621 RID: 5665
		[Tooltip("the value to remap the intensity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapLevelOne = 1f;

		// Token: 0x04001622 RID: 5666
		[Tooltip("the value to move the intensity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantLevel;

		// Token: 0x04001623 RID: 5667
		[Tooltip("the value towards which to animate when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float ToDestinationLevel = 5f;

		// Token: 0x04001624 RID: 5668
		protected float _initialIntensity;

		// Token: 0x04001625 RID: 5669
		protected Coroutine _coroutine;

		// Token: 0x0200053E RID: 1342
		public enum Modes
		{
			// Token: 0x04002416 RID: 9238
			OverTime,
			// Token: 0x04002417 RID: 9239
			Instant,
			// Token: 0x04002418 RID: 9240
			ToDestination
		}
	}
}
