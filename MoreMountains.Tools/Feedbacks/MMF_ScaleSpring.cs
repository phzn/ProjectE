using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000209 RID: 521
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Scale Spring")]
	[FeedbackHelp("This feedback will let you animate the scale of the target object over time, with a spring effect.")]
	public class MMF_ScaleSpring : MMF_Feedback
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0003E8C8 File Offset: 0x0003CAC8
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0003E8CB File Offset: 0x0003CACB
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0003E8CE File Offset: 0x0003CACE
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateScaleTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0003E8DC File Offset: 0x0003CADC
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x0003E8EA File Offset: 0x0003CAEA
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0003E8F3 File Offset: 0x0003CAF3
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0003E8F6 File Offset: 0x0003CAF6
		protected virtual bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this._velocity.x) + Mathf.Abs(this._velocity.y) + Mathf.Abs(this._velocity.z) < this._velocityLowThreshold;
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0003E932 File Offset: 0x0003CB32
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateScaleTarget != null)
			{
				this.GetInitialValues();
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0003E957 File Offset: 0x0003CB57
		protected virtual void GetInitialValues()
		{
			this._initialScale = this.AnimateScaleTarget.localScale;
			this._currentValue = this.AnimateScaleTarget.localScale;
			this._targetValue = this._currentValue;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003E988 File Offset: 0x0003CB88
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ScaleSpring.FeedbackTypeAuthorized || this.AnimateScaleTarget == null)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			switch (this.Mode)
			{
			case MMF_ScaleSpring.Modes.MoveTo:
				this._targetValue.x = Random.Range(this.MoveToScaleMin.x, this.MoveToScaleMax.x);
				this._targetValue.y = Random.Range(this.MoveToScaleMin.y, this.MoveToScaleMax.y);
				this._targetValue.z = Random.Range(this.MoveToScaleMin.z, this.MoveToScaleMax.z);
				break;
			case MMF_ScaleSpring.Modes.MoveToAdditive:
				this._targetValue.x = this._targetValue.x + Random.Range(this.MoveToScaleMin.x, this.MoveToScaleMax.x);
				this._targetValue.y = this._targetValue.y + Random.Range(this.MoveToScaleMin.y, this.MoveToScaleMax.y);
				this._targetValue.z = this._targetValue.z + Random.Range(this.MoveToScaleMin.z, this.MoveToScaleMax.z);
				break;
			case MMF_ScaleSpring.Modes.Bump:
			{
				this._velocity.x = Random.Range(this.BumpScaleMin.x, this.BumpScaleMax.x);
				this._velocity.y = Random.Range(this.BumpScaleMin.y, this.BumpScaleMax.y);
				this._velocity.z = Random.Range(this.BumpScaleMin.z, this.BumpScaleMax.z);
				float num = this.ComputeIntensity(feedbacksIntensity, position);
				this._velocity.x = this._velocity.x * num;
				break;
			}
			}
			this._coroutine = this.Owner.StartCoroutine(this.Spring());
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003EB86 File Offset: 0x0003CD86
		protected virtual IEnumerator Spring()
		{
			this.IsPlaying = true;
			this.UpdateSpring();
			while (!this.LowVelocity)
			{
				yield return null;
				this.UpdateSpring();
				this.ApplyValue();
			}
			this._velocity.x = 0f;
			this._velocity.y = 0f;
			this._velocity.z = 0f;
			this._currentValue = this._targetValue;
			this.ApplyValue();
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003EB98 File Offset: 0x0003CD98
		protected virtual void UpdateSpring()
		{
			MMMaths.Spring(ref this._currentValue.x, this._targetValue.x, ref this._velocity.x, this.DampingX, this.FrequencyX, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.y, this._targetValue.y, ref this._velocity.y, this.DampingY, this.FrequencyY, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.z, this._targetValue.z, ref this._velocity.z, this.DampingZ, this.FrequencyZ, this.FeedbackDeltaTime);
			this.ApplyValue();
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0003EC54 File Offset: 0x0003CE54
		protected virtual void ApplyValue()
		{
			this._newScale.x = Mathf.Abs(this._currentValue.x);
			this._newScale.y = Mathf.Abs(this._currentValue.y);
			this._newScale.z = Mathf.Abs(this._currentValue.z);
			this.AnimateScaleTarget.localScale = this._currentValue;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0003ECC4 File Offset: 0x0003CEC4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ScaleSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this.IsPlaying = false;
			this._velocity.x = 0f;
			this._velocity.y = 0f;
			this._velocity.z = 0f;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0003ED44 File Offset: 0x0003CF44
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_ScaleSpring.FeedbackTypeAuthorized && this.AnimateScaleTarget != null)
			{
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._currentValue = this._targetValue;
				this.IsPlaying = false;
				this._velocity.x = 0f;
				this._velocity.y = 0f;
				this._velocity.z = 0f;
				this.ApplyValue();
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0003EDD0 File Offset: 0x0003CFD0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ScaleSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			this._currentValue = this._initialScale;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x04001044 RID: 4164
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001045 RID: 4165
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the object to animate")]
		public Transform AnimateScaleTarget;

		// Token: 0x04001046 RID: 4166
		[Tooltip("spring duration is determined by the spring (and could be impacted real time), so it's up to you to determine how long this feedback should last, from the point of view of its parent MMF Player")]
		public float DeclaredDuration;

		// Token: 0x04001047 RID: 4167
		[MMFInspectorGroup("Spring Settings", true, 18, false, false)]
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingX = 0.4f;

		// Token: 0x04001048 RID: 4168
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyX = 6f;

		// Token: 0x04001049 RID: 4169
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingY = 0.4f;

		// Token: 0x0400104A RID: 4170
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyY = 6f;

		// Token: 0x0400104B RID: 4171
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingZ = 0.4f;

		// Token: 0x0400104C RID: 4172
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyZ = 6f;

		// Token: 0x0400104D RID: 4173
		[MMFInspectorGroup("Spring Mode", true, 19, false, false)]
		[Tooltip("the chosen mode for this spring. MoveTo will move the target the specified scale (randomized between min and max). MoveToAdditive will add the specified scale (randomized between min and max) to the target's current scale. Bump will bump the target's scale by the specified power (randomized between min and max)")]
		public MMF_ScaleSpring.Modes Mode = MMF_ScaleSpring.Modes.Bump;

		// Token: 0x0400104E RID: 4174
		[Tooltip("the min value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToScaleMin = new Vector3(1f, 1f, 1f);

		// Token: 0x0400104F RID: 4175
		[Tooltip("the max value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToScaleMax = new Vector3(2f, 2f, 2f);

		// Token: 0x04001050 RID: 4176
		[Tooltip("the min value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpScaleMin = new Vector3(20f, 20f, 20f);

		// Token: 0x04001051 RID: 4177
		[Tooltip("the max value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpScaleMax = new Vector3(30f, 30f, 30f);

		// Token: 0x04001052 RID: 4178
		protected Vector3 _currentValue = Vector3.zero;

		// Token: 0x04001053 RID: 4179
		protected Vector3 _targetValue = Vector3.zero;

		// Token: 0x04001054 RID: 4180
		protected Vector3 _velocity = Vector3.zero;

		// Token: 0x04001055 RID: 4181
		protected Vector3 _initialScale;

		// Token: 0x04001056 RID: 4182
		protected Coroutine _coroutine;

		// Token: 0x04001057 RID: 4183
		protected float _velocityLowThreshold = 0.001f;

		// Token: 0x04001058 RID: 4184
		protected Vector3 _newScale;

		// Token: 0x020004EA RID: 1258
		public enum Modes
		{
			// Token: 0x040022F6 RID: 8950
			MoveTo,
			// Token: 0x040022F7 RID: 8951
			MoveToAdditive,
			// Token: 0x040022F8 RID: 8952
			Bump
		}
	}
}
