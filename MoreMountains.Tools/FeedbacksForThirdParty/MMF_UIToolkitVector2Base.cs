using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000335 RID: 821
	[AddComponentMenu("")]
	public class MMF_UIToolkitVector2Base : MMF_UIToolkit
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x0005FB2E File Offset: 0x0005DD2E
		// (set) Token: 0x060016A4 RID: 5796 RVA: 0x0005FB4A File Offset: 0x0005DD4A
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_UIToolkitVector2Base.Modes.Instant)
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

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060016A5 RID: 5797 RVA: 0x0005FB53 File Offset: 0x0005DD53
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x0005FB56 File Offset: 0x0005DD56
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this._visualElements == null || this._visualElements.Count == 0)
			{
				return;
			}
			this._initialValue = this.GetInitialValue();
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0005FB84 File Offset: 0x0005DD84
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
			if (this.RelativeValues)
			{
				this._initialValue = this.GetInitialValue();
			}
			switch (this.Mode)
			{
			case MMF_UIToolkitVector2Base.Modes.Instant:
			{
				Vector2 vector = (this.RelativeValues ? (this.InstantValue + this._initialValue) : this.InstantValue);
				this.SetValue(vector);
				return;
			}
			case MMF_UIToolkitVector2Base.Modes.Interpolate:
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
			case MMF_UIToolkitVector2Base.Modes.ToDestination:
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

		// Token: 0x060016A8 RID: 5800 RVA: 0x0005FC9E File Offset: 0x0005DE9E
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

		// Token: 0x060016A9 RID: 5801 RVA: 0x0005FCB0 File Offset: 0x0005DEB0
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

		// Token: 0x060016AA RID: 5802 RVA: 0x0005FCFC File Offset: 0x0005DEFC
		protected virtual void ApplyTime(float time)
		{
			this._newValue.x = this._initialValue.x;
			this._newValue.y = this._initialValue.y;
			if (this.Mode == MMF_UIToolkitVector2Base.Modes.Interpolate)
			{
				if (this.AnimateX)
				{
					float num = (this.RelativeValues ? (this.CurveRemapZeroX + this._initialValue.x) : this.CurveRemapZeroX);
					float num2 = (this.RelativeValues ? (this.CurveRemapOneX + this._initialValue.x) : this.CurveRemapOneX);
					this._newValue.x = MMTween.Tween(time, 0f, 1f, num, num2, this.CurveX);
				}
				if (this.AnimateY)
				{
					float num3 = (this.RelativeValues ? (this.CurveRemapZeroY + this._initialValue.y) : this.CurveRemapZeroY);
					float num4 = (this.RelativeValues ? (this.CurveRemapOneY + this._initialValue.y) : this.CurveRemapOneY);
					this._newValue.y = MMTween.Tween(time, 0f, 1f, num3, num4, this.CurveY);
				}
			}
			else if (this.Mode == MMF_UIToolkitVector2Base.Modes.ToDestination)
			{
				if (this.AnimateX)
				{
					this._newValue.x = MMTween.Tween(time, 0f, 1f, this._initialValue.x, this.DestinationValueX, this.CurveX);
				}
				if (this.AnimateY)
				{
					this._newValue.y = MMTween.Tween(time, 0f, 1f, this._initialValue.y, this.DestinationValueY, this.CurveY);
				}
			}
			this.SetValue(this._newValue);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0005FEB1 File Offset: 0x0005E0B1
		protected virtual void SetValue(Vector2 newValue)
		{
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0005FEB3 File Offset: 0x0005E0B3
		protected virtual Vector2 GetInitialValue()
		{
			return Vector2.zero;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0005FEBA File Offset: 0x0005E0BA
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SetValue(this._initialValue);
		}

		// Token: 0x04001A0C RID: 6668
		[MMFInspectorGroup("Value", true, 16, false, false)]
		[Tooltip("the selected mode :Instant : the value will change instantly to the target one,Curve : the value will be interpolated along the curve,interpolate : lerps from the current value to the destination one ")]
		public MMF_UIToolkitVector2Base.Modes Mode = MMF_UIToolkitVector2Base.Modes.Interpolate;

		// Token: 0x04001A0D RID: 6669
		[Tooltip("whether or not the value should be applied relatively to the initial value")]
		[MMFEnumCondition("Mode", new int[] { 1, 0 })]
		public bool RelativeValues;

		// Token: 0x04001A0E RID: 6670
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04001A0F RID: 6671
		[Tooltip("how long the color of the text should change over time")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public float Duration = 0.2f;

		// Token: 0x04001A10 RID: 6672
		[Tooltip("the value to apply when in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Vector2 InstantValue = new Vector2(1f, 1f);

		// Token: 0x04001A11 RID: 6673
		[Header("X")]
		[Tooltip("whether or not to animate the x value")]
		public bool AnimateX = true;

		// Token: 0x04001A12 RID: 6674
		[Tooltip("the curve to use when interpolating towards the destination value")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public MMTweenType CurveX = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04001A13 RID: 6675
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapZeroX;

		// Token: 0x04001A14 RID: 6676
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapOneX = 1f;

		// Token: 0x04001A15 RID: 6677
		[Tooltip("the value to aim towards when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationValueX = 1f;

		// Token: 0x04001A16 RID: 6678
		[Header("Y")]
		[Tooltip("whether or not to animate the y value")]
		public bool AnimateY = true;

		// Token: 0x04001A17 RID: 6679
		[Tooltip("the curve to use when interpolating towards the destination value")]
		[MMFEnumCondition("Mode", new int[] { 1, 2 })]
		public MMTweenType CurveY = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04001A18 RID: 6680
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapZeroY;

		// Token: 0x04001A19 RID: 6681
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float CurveRemapOneY = 1f;

		// Token: 0x04001A1A RID: 6682
		[Tooltip("the value to aim towards when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationValueY = 1f;

		// Token: 0x04001A1B RID: 6683
		protected Vector2 _initialValue;

		// Token: 0x04001A1C RID: 6684
		protected Coroutine _coroutine;

		// Token: 0x04001A1D RID: 6685
		protected Vector2 _newValue;

		// Token: 0x0200057E RID: 1406
		public enum Modes
		{
			// Token: 0x040024C3 RID: 9411
			Instant,
			// Token: 0x040024C4 RID: 9412
			Interpolate,
			// Token: 0x040024C5 RID: 9413
			ToDestination
		}
	}
}
