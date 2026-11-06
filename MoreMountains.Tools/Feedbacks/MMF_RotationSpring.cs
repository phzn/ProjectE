using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000206 RID: 518
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Rotation Spring")]
	[FeedbackHelp("This feedback will let you animate the rotation of the target object over time, with a spring effect.")]
	public class MMF_RotationSpring : MMF_Feedback
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0003D930 File Offset: 0x0003BB30
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0003D933 File Offset: 0x0003BB33
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003D936 File Offset: 0x0003BB36
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateRotationTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0003D944 File Offset: 0x0003BB44
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0003D952 File Offset: 0x0003BB52
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0003D95B File Offset: 0x0003BB5B
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0003D95E File Offset: 0x0003BB5E
		protected virtual bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this._velocity.x) + Mathf.Abs(this._velocity.y) + Mathf.Abs(this._velocity.z) < this._velocityLowThreshold;
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003D99A File Offset: 0x0003BB9A
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateRotationTarget != null)
			{
				this.GetInitialValues();
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
		protected virtual void GetInitialValues()
		{
			if (this.RotationSpace == Space.Self)
			{
				this._initialRotation = this.AnimateRotationTarget.localRotation.eulerAngles;
			}
			else
			{
				this._initialRotation = this.AnimateRotationTarget.rotation.eulerAngles;
			}
			this._currentValue = this._initialRotation;
			this._targetValue = this._currentValue;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0003DA24 File Offset: 0x0003BC24
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_RotationSpring.FeedbackTypeAuthorized || this.AnimateRotationTarget == null)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			switch (this.Mode)
			{
			case MMF_RotationSpring.Modes.MoveTo:
				this._targetValue.x = Random.Range(this.MoveToRotationMin.x, this.MoveToRotationMax.x);
				this._targetValue.y = Random.Range(this.MoveToRotationMin.y, this.MoveToRotationMax.y);
				this._targetValue.z = Random.Range(this.MoveToRotationMin.z, this.MoveToRotationMax.z);
				break;
			case MMF_RotationSpring.Modes.MoveToAdditive:
				this._targetValue.x = this._targetValue.x + Random.Range(this.MoveToRotationMin.x, this.MoveToRotationMax.x);
				this._targetValue.y = this._targetValue.y + Random.Range(this.MoveToRotationMin.y, this.MoveToRotationMax.y);
				this._targetValue.z = this._targetValue.z + Random.Range(this.MoveToRotationMin.z, this.MoveToRotationMax.z);
				break;
			case MMF_RotationSpring.Modes.Bump:
			{
				this._velocity.x = Random.Range(this.BumpRotationMin.x, this.BumpRotationMax.x);
				this._velocity.y = Random.Range(this.BumpRotationMin.y, this.BumpRotationMax.y);
				this._velocity.z = Random.Range(this.BumpRotationMin.z, this.BumpRotationMax.z);
				float num = this.ComputeIntensity(feedbacksIntensity, position);
				this._velocity.x = this._velocity.x * num;
				break;
			}
			}
			this._coroutine = this.Owner.StartCoroutine(this.Spring());
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0003DC22 File Offset: 0x0003BE22
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

		// Token: 0x06000DEF RID: 3567 RVA: 0x0003DC34 File Offset: 0x0003BE34
		protected virtual void UpdateSpring()
		{
			MMMaths.Spring(ref this._currentValue.x, this._targetValue.x, ref this._velocity.x, this.DampingX, this.FrequencyX, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.y, this._targetValue.y, ref this._velocity.y, this.DampingY, this.FrequencyY, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.z, this._targetValue.z, ref this._velocity.z, this.DampingZ, this.FrequencyZ, this.FeedbackDeltaTime);
			this.ApplyValue();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003DCEF File Offset: 0x0003BEEF
		protected virtual void ApplyValue()
		{
			if (this.RotationSpace == Space.World)
			{
				this.AnimateRotationTarget.rotation = Quaternion.Euler(this._currentValue);
				return;
			}
			this.AnimateRotationTarget.localRotation = Quaternion.Euler(this._currentValue);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0003DD28 File Offset: 0x0003BF28
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_RotationSpring.FeedbackTypeAuthorized)
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

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003DDA8 File Offset: 0x0003BFA8
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_RotationSpring.FeedbackTypeAuthorized && this.AnimateRotationTarget != null)
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

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0003DE34 File Offset: 0x0003C034
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_RotationSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			this._currentValue = this._initialRotation;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x04001006 RID: 4102
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001007 RID: 4103
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the object to animate")]
		public Transform AnimateRotationTarget;

		// Token: 0x04001008 RID: 4104
		[Tooltip("spring duration is determined by the spring (and could be impacted real time), so it's up to you to determine how long this feedback should last, from the point of view of its parent MMF Player")]
		public float DeclaredDuration;

		// Token: 0x04001009 RID: 4105
		[Tooltip("whether this feedback should play on local or world rotation")]
		public Space RotationSpace;

		// Token: 0x0400100A RID: 4106
		[MMFInspectorGroup("Spring Settings", true, 18, false, false)]
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingX = 0.4f;

		// Token: 0x0400100B RID: 4107
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyX = 6f;

		// Token: 0x0400100C RID: 4108
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingY = 0.4f;

		// Token: 0x0400100D RID: 4109
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyY = 6f;

		// Token: 0x0400100E RID: 4110
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingZ = 0.4f;

		// Token: 0x0400100F RID: 4111
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyZ = 6f;

		// Token: 0x04001010 RID: 4112
		[MMFInspectorGroup("Spring Mode", true, 19, false, false)]
		[Tooltip("the chosen mode for this spring. MoveTo will move the target the specified rotation (randomized between min and max). MoveToAdditive will add the specified rotation (randomized between min and max) to the target's current rotation. Bump will bump the target's rotation by the specified power (randomized between min and max)")]
		public MMF_RotationSpring.Modes Mode = MMF_RotationSpring.Modes.Bump;

		// Token: 0x04001011 RID: 4113
		[Tooltip("the min value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToRotationMin = new Vector3(45f, 0f, 0f);

		// Token: 0x04001012 RID: 4114
		[Tooltip("the max value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToRotationMax = new Vector3(90f, 0f, 0f);

		// Token: 0x04001013 RID: 4115
		[Tooltip("the min value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpRotationMin = new Vector3(2000f, 2000f, 0f);

		// Token: 0x04001014 RID: 4116
		[Tooltip("the max value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpRotationMax = new Vector3(3000f, 3000f, 0f);

		// Token: 0x04001015 RID: 4117
		protected Vector3 _currentValue = Vector3.zero;

		// Token: 0x04001016 RID: 4118
		protected Vector3 _targetValue = Vector3.zero;

		// Token: 0x04001017 RID: 4119
		protected Vector3 _velocity = Vector3.zero;

		// Token: 0x04001018 RID: 4120
		protected Vector3 _initialRotation;

		// Token: 0x04001019 RID: 4121
		protected Coroutine _coroutine;

		// Token: 0x0400101A RID: 4122
		protected float _velocityLowThreshold = 0.001f;

		// Token: 0x020004E4 RID: 1252
		public enum Modes
		{
			// Token: 0x040022D8 RID: 8920
			MoveTo,
			// Token: 0x040022D9 RID: 8921
			MoveToAdditive,
			// Token: 0x040022DA RID: 8922
			Bump
		}
	}
}
