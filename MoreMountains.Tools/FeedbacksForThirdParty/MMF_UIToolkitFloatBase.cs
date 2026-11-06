using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000334 RID: 820
	[AddComponentMenu("")]
	public class MMF_UIToolkitFloatBase : MMF_UIToolkit
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x0005F845 File Offset: 0x0005DA45
		// (set) Token: 0x06001698 RID: 5784 RVA: 0x0005F861 File Offset: 0x0005DA61
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_UIToolkitFloatBase.Modes.Instant)
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

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x0005F86A File Offset: 0x0005DA6A
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x0005F86D File Offset: 0x0005DA6D
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this._visualElements == null || this._visualElements.Count == 0)
			{
				return;
			}
			this._initialValue = this.GetInitialValue();
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0005F898 File Offset: 0x0005DA98
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
			if (this.RelativeValue)
			{
				this._initialValue = this.GetInitialValue();
			}
			switch (this.Mode)
			{
			case MMF_UIToolkitFloatBase.Modes.Instant:
			{
				float num = (this.RelativeValue ? (this.InstantValue + this._initialValue) : this.InstantValue);
				this.SetValue(num);
				return;
			}
			case MMF_UIToolkitFloatBase.Modes.Interpolate:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeValue());
				return;
			case MMF_UIToolkitFloatBase.Modes.ToDestination:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				this._initialValue = this.GetInitialValue();
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ChangeValue());
				return;
			default:
				return;
			}
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0005F9AE File Offset: 0x0005DBAE
		protected virtual IEnumerator ChangeValue()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.ApplyTime(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ApplyTime(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0005F9C0 File Offset: 0x0005DBC0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0005FA0C File Offset: 0x0005DC0C
		protected virtual void ApplyTime(float time)
		{
			float num = 0f;
			if (this.Mode == MMF_UIToolkitFloatBase.Modes.Interpolate)
			{
				float num2 = (this.RelativeValue ? (this.CurveRemapZero + this._initialValue) : this.CurveRemapZero);
				float num3 = (this.RelativeValue ? (this.CurveRemapOne + this._initialValue) : this.CurveRemapOne);
				num = MMTween.Tween(time, 0f, 1f, num2, num3, this.Curve);
			}
			else if (this.Mode == MMF_UIToolkitFloatBase.Modes.ToDestination)
			{
				num = MMTween.Tween(time, 0f, 1f, this._initialValue, this.DestinationValue, this.Curve);
			}
			this.SetValue(num);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0005FAB2 File Offset: 0x0005DCB2
		protected virtual void SetValue(float newValue)
		{
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0005FAB4 File Offset: 0x0005DCB4
		protected virtual float GetInitialValue()
		{
			return 0f;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0005FABB File Offset: 0x0005DCBB
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SetValue(this._initialValue);
		}

		// Token: 0x04001A01 RID: 6657
		[MMFInspectorGroup("Value", true, 16, false, false)]
		[Tooltip("the selected mode :Instant : the value will change instantly to the target one,Curve : the value will be interpolated along the curve,interpolate : lerps from the current value to the destination one ")]
		public MMF_UIToolkitFloatBase.Modes Mode = MMF_UIToolkitFloatBase.Modes.Interpolate;

		// Token: 0x04001A02 RID: 6658
		[Tooltip("whether or not the value should be applied relatively to the initial value")]
		[MMFEnumCondition("Mode", new int[] { 1, 0 })]
		public bool RelativeValue;

		// Token: 0x04001A03 RID: 6659
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001A04 RID: 6660
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public float Duration = 0.2f;

		// Token: 0x04001A05 RID: 6661
		[Tooltip("the value to apply when in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float InstantValue = 1f;

		// Token: 0x04001A06 RID: 6662
		[Tooltip("the curve to use when interpolating towards the destination value")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04001A07 RID: 6663
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapZero;

		// Token: 0x04001A08 RID: 6664
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapOne = 1f;

		// Token: 0x04001A09 RID: 6665
		[Tooltip("the value to aim towards when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationValue = 1f;

		// Token: 0x04001A0A RID: 6666
		protected float _initialValue;

		// Token: 0x04001A0B RID: 6667
		protected Coroutine _coroutine;

		// Token: 0x0200057C RID: 1404
		public enum Modes
		{
			// Token: 0x040024BB RID: 9403
			Instant,
			// Token: 0x040024BC RID: 9404
			Interpolate,
			// Token: 0x040024BD RID: 9405
			ToDestination
		}
	}
}
