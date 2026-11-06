using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FF RID: 511
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Position Spring")]
	[FeedbackHelp("This feedback will let you animate the position of the target object over time, with a spring effect.")]
	public class MMF_PositionSpring : MMF_Feedback
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0003BE35 File Offset: 0x0003A035
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0003BE38 File Offset: 0x0003A038
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0003BE3B File Offset: 0x0003A03B
		protected override void AutomateTargetAcquisition()
		{
			this.AnimatePositionTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003BE49 File Offset: 0x0003A049
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0003BE57 File Offset: 0x0003A057
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0003BE60 File Offset: 0x0003A060
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0003BE63 File Offset: 0x0003A063
		protected virtual bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this._velocity.x) + Mathf.Abs(this._velocity.y) + Mathf.Abs(this._velocity.z) < this._velocityLowThreshold;
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0003BE9F File Offset: 0x0003A09F
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Space == MMF_PositionSpring.Spaces.RectTransform)
			{
				this._rectTransform = this.AnimatePositionTarget.GetComponent<RectTransform>();
			}
			if (this.Active && this.AnimatePositionTarget != null)
			{
				this.GetInitialValues();
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0003BEE0 File Offset: 0x0003A0E0
		protected virtual void GetInitialValues()
		{
			switch (this.Space)
			{
			case MMF_PositionSpring.Spaces.World:
				this._initialPosition = this.AnimatePositionTarget.position;
				break;
			case MMF_PositionSpring.Spaces.Local:
				this._initialPosition = this.AnimatePositionTarget.localPosition;
				break;
			case MMF_PositionSpring.Spaces.RectTransform:
				this._initialPosition = this._rectTransform.anchoredPosition3D;
				break;
			}
			this._currentValue = this._initialPosition;
			this._targetValue = this._currentValue;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0003BF58 File Offset: 0x0003A158
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PositionSpring.FeedbackTypeAuthorized || this.AnimatePositionTarget == null)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			switch (this.Mode)
			{
			case MMF_PositionSpring.Modes.MoveTo:
				this._targetValue.x = Random.Range(this.MoveToPositionMin.x, this.MoveToPositionMax.x);
				this._targetValue.y = Random.Range(this.MoveToPositionMin.y, this.MoveToPositionMax.y);
				this._targetValue.z = Random.Range(this.MoveToPositionMin.z, this.MoveToPositionMax.z);
				break;
			case MMF_PositionSpring.Modes.MoveToAdditive:
				this._targetValue.x = this._targetValue.x + Random.Range(this.MoveToPositionMin.x, this.MoveToPositionMax.x);
				this._targetValue.y = this._targetValue.y + Random.Range(this.MoveToPositionMin.y, this.MoveToPositionMax.y);
				this._targetValue.z = this._targetValue.z + Random.Range(this.MoveToPositionMin.z, this.MoveToPositionMax.z);
				break;
			case MMF_PositionSpring.Modes.Bump:
			{
				this._velocity.x = Random.Range(this.BumpPositionMin.x, this.BumpPositionMax.x);
				this._velocity.y = Random.Range(this.BumpPositionMin.y, this.BumpPositionMax.y);
				this._velocity.z = Random.Range(this.BumpPositionMin.z, this.BumpPositionMax.z);
				float num = this.ComputeIntensity(feedbacksIntensity, position);
				this._velocity.x = this._velocity.x * num;
				break;
			}
			}
			this._coroutine = this.Owner.StartCoroutine(this.Spring());
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0003C156 File Offset: 0x0003A356
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

		// Token: 0x06000D9C RID: 3484 RVA: 0x0003C168 File Offset: 0x0003A368
		protected virtual void UpdateSpring()
		{
			MMMaths.Spring(ref this._currentValue.x, this._targetValue.x, ref this._velocity.x, this.DampingX, this.FrequencyX, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.y, this._targetValue.y, ref this._velocity.y, this.DampingY, this.FrequencyY, this.FeedbackDeltaTime);
			MMMaths.Spring(ref this._currentValue.z, this._targetValue.z, ref this._velocity.z, this.DampingZ, this.FrequencyZ, this.FeedbackDeltaTime);
			this.ApplyValue();
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003C224 File Offset: 0x0003A424
		protected virtual void ApplyValue()
		{
			this._appliedPosition = this._currentValue;
			if (this.ForceAbsolute)
			{
				this._appliedPosition.x = Mathf.Abs(this._appliedPosition.x - this._initialPosition.x) + this._initialPosition.x;
				this._appliedPosition.y = Mathf.Abs(this._appliedPosition.y - this._initialPosition.y) + this._initialPosition.y;
				this._appliedPosition.z = Mathf.Abs(this._appliedPosition.z - this._initialPosition.z) + this._initialPosition.z;
			}
			if (this.Space == MMF_PositionSpring.Spaces.World)
			{
				this.AnimatePositionTarget.position = this._appliedPosition;
				return;
			}
			if (this.Space == MMF_PositionSpring.Spaces.RectTransform)
			{
				this._rectTransform.anchoredPosition3D = this._appliedPosition;
				return;
			}
			if (this.Space == MMF_PositionSpring.Spaces.Local)
			{
				this.AnimatePositionTarget.localPosition = this._appliedPosition;
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003C330 File Offset: 0x0003A530
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PositionSpring.FeedbackTypeAuthorized)
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

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003C3B0 File Offset: 0x0003A5B0
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_PositionSpring.FeedbackTypeAuthorized && this.AnimatePositionTarget != null)
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

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0003C43C File Offset: 0x0003A63C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PositionSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			this._currentValue = this._initialPosition;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x04000F98 RID: 3992
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F99 RID: 3993
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the object to animate")]
		public Transform AnimatePositionTarget;

		// Token: 0x04000F9A RID: 3994
		[Tooltip("spring duration is determined by the spring (and could be impacted real time), so it's up to you to determine how long this feedback should last, from the point of view of its parent MMF Player")]
		public float DeclaredDuration;

		// Token: 0x04000F9B RID: 3995
		[Tooltip("the space in which to move the position in")]
		public MMF_PositionSpring.Spaces Space;

		// Token: 0x04000F9C RID: 3996
		[MMFInspectorGroup("Spring Settings", true, 18, false, false)]
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingX = 0.4f;

		// Token: 0x04000F9D RID: 3997
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyX = 6f;

		// Token: 0x04000F9E RID: 3998
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingY = 0.4f;

		// Token: 0x04000F9F RID: 3999
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyY = 6f;

		// Token: 0x04000FA0 RID: 4000
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float DampingZ = 0.4f;

		// Token: 0x04000FA1 RID: 4001
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float FrequencyZ = 6f;

		// Token: 0x04000FA2 RID: 4002
		[MMFInspectorGroup("Spring Mode", true, 19, false, false)]
		[Tooltip("the chosen mode for this spring. MoveTo will move the target the specified position (randomized between min and max). MoveToAdditive will add the specified position (randomized between min and max) to the target's current position. Bump will bump the target's position by the specified power (randomized between min and max)")]
		public MMF_PositionSpring.Modes Mode = MMF_PositionSpring.Modes.Bump;

		// Token: 0x04000FA3 RID: 4003
		[Tooltip("the min value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToPositionMin = new Vector3(1f, 1f, 1f);

		// Token: 0x04000FA4 RID: 4004
		[Tooltip("the max value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 MoveToPositionMax = new Vector3(2f, 2f, 2f);

		// Token: 0x04000FA5 RID: 4005
		[Tooltip("the min value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpPositionMin = new Vector3(0f, 20f, 0f);

		// Token: 0x04000FA6 RID: 4006
		[Tooltip("the max value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 BumpPositionMax = new Vector3(0f, 30f, 0f);

		// Token: 0x04000FA7 RID: 4007
		public bool ForceAbsolute;

		// Token: 0x04000FA8 RID: 4008
		protected Vector3 _currentValue = Vector3.zero;

		// Token: 0x04000FA9 RID: 4009
		protected Vector3 _targetValue = Vector3.zero;

		// Token: 0x04000FAA RID: 4010
		protected Vector3 _velocity = Vector3.zero;

		// Token: 0x04000FAB RID: 4011
		protected Vector3 _initialPosition;

		// Token: 0x04000FAC RID: 4012
		protected Coroutine _coroutine;

		// Token: 0x04000FAD RID: 4013
		protected float _velocityLowThreshold = 0.001f;

		// Token: 0x04000FAE RID: 4014
		protected RectTransform _rectTransform;

		// Token: 0x04000FAF RID: 4015
		protected Vector3 _appliedPosition;

		// Token: 0x020004D9 RID: 1241
		public enum Modes
		{
			// Token: 0x0400229F RID: 8863
			MoveTo,
			// Token: 0x040022A0 RID: 8864
			MoveToAdditive,
			// Token: 0x040022A1 RID: 8865
			Bump
		}

		// Token: 0x020004DA RID: 1242
		public enum Spaces
		{
			// Token: 0x040022A3 RID: 8867
			World,
			// Token: 0x040022A4 RID: 8868
			Local,
			// Token: 0x040022A5 RID: 8869
			RectTransform
		}
	}
}
