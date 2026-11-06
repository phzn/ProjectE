using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000210 RID: 528
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Squash and Stretch Spring")]
	[FeedbackHelp("This feedback will let you animate the scale of the target object over time, with a spring + squash and stretch effect")]
	public class MMF_SquashAndStretchSpring : MMF_Feedback
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x000401A9 File Offset: 0x0003E3A9
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x000401AC File Offset: 0x0003E3AC
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000401AF File Offset: 0x0003E3AF
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateScaleTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x000401BD File Offset: 0x0003E3BD
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x000401CB File Offset: 0x0003E3CB
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

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x000401D4 File Offset: 0x0003E3D4
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x000401D7 File Offset: 0x0003E3D7
		protected virtual bool LowVelocity
		{
			get
			{
				return Mathf.Abs(this._velocity) < this._velocityLowThreshold;
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000401EC File Offset: 0x0003E3EC
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateScaleTarget != null)
			{
				this.GetInitialValues();
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00040211 File Offset: 0x0003E411
		protected virtual void GetInitialValues()
		{
			this._initialScale = this.AnimateScaleTarget.localScale;
			this._currentValue = this.AnimateScaleTarget.localScale.x;
			this._targetValue = this._currentValue;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00040248 File Offset: 0x0003E448
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SquashAndStretchSpring.FeedbackTypeAuthorized || this.AnimateScaleTarget == null)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			switch (this.Mode)
			{
			case MMF_SquashAndStretchSpring.Modes.MoveTo:
				this._targetValue = Random.Range(this.MoveToMin, this.MoveToMax);
				break;
			case MMF_SquashAndStretchSpring.Modes.MoveToAdditive:
				this._targetValue += Random.Range(this.MoveToMin, this.MoveToMax);
				break;
			case MMF_SquashAndStretchSpring.Modes.Bump:
				this._velocity = Random.Range(this.BumpScaleMin, this.BumpScaleMax);
				break;
			}
			this._coroutine = this.Owner.StartCoroutine(this.Spring());
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004030E File Offset: 0x0003E50E
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
			this._velocity = 0f;
			this._currentValue = this._targetValue;
			this.ApplyValue();
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004031D File Offset: 0x0003E51D
		protected virtual void UpdateSpring()
		{
			MMMaths.Spring(ref this._currentValue, this._targetValue, ref this._velocity, this.Damping, this.Frequency, this.FeedbackDeltaTime);
			this.ApplyValue();
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00040350 File Offset: 0x0003E550
		protected virtual void ApplyValue()
		{
			float currentValue = this._currentValue;
			float num = 1f / Mathf.Sqrt(currentValue);
			switch (this.Axis)
			{
			case MMF_SquashAndStretchSpring.PossibleAxis.XtoYZ:
				this._newScale.x = currentValue;
				this._newScale.y = num;
				this._newScale.z = num;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.XtoY:
				this._newScale.x = currentValue;
				this._newScale.y = num;
				this._newScale.z = this._initialScale.z;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.XtoZ:
				this._newScale.x = currentValue;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.YtoXZ:
				this._newScale.x = num;
				this._newScale.y = currentValue;
				this._newScale.z = num;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.YtoX:
				this._newScale.x = num;
				this._newScale.y = currentValue;
				this._newScale.z = this._initialScale.z;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.YtoZ:
				this._newScale.x = currentValue;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.ZtoXZ:
				this._newScale.x = num;
				this._newScale.y = num;
				this._newScale.z = currentValue;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.ZtoX:
				this._newScale.x = num;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = currentValue;
				break;
			case MMF_SquashAndStretchSpring.PossibleAxis.ZtoY:
				this._newScale.x = this._initialScale.x;
				this._newScale.y = num;
				this._newScale.z = currentValue;
				break;
			}
			this._newScale.x = Mathf.Abs(this._newScale.x);
			this._newScale.y = Mathf.Abs(this._newScale.y);
			this._newScale.z = Mathf.Abs(this._newScale.z);
			this.AnimateScaleTarget.localScale = this._newScale;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000405AC File Offset: 0x0003E7AC
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SquashAndStretchSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this.IsPlaying = false;
			this._velocity = 0f;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00040608 File Offset: 0x0003E808
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_SquashAndStretchSpring.FeedbackTypeAuthorized && this.AnimateScaleTarget != null)
			{
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._currentValue = this._targetValue;
				this.IsPlaying = false;
				this._velocity = 0f;
				this.ApplyValue();
			}
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0004066F File Offset: 0x0003E86F
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SquashAndStretchSpring.FeedbackTypeAuthorized)
			{
				return;
			}
			this._currentValue = this._initialScale.x;
			this._targetValue = this._currentValue;
			this.ApplyValue();
		}

		// Token: 0x040010AB RID: 4267
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010AC RID: 4268
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the object to animate")]
		public Transform AnimateScaleTarget;

		// Token: 0x040010AD RID: 4269
		[Tooltip("spring duration is determined by the spring (and could be impacted real time), so it's up to you to determine how long this feedback should last, from the point of view of its parent MMF Player")]
		public float DeclaredDuration;

		// Token: 0x040010AE RID: 4270
		[Tooltip("the axis on which to operate squashing and stretching")]
		public MMF_SquashAndStretchSpring.PossibleAxis Axis;

		// Token: 0x040010AF RID: 4271
		[MMFInspectorGroup("Spring Settings", true, 18, false, false)]
		[Tooltip("the dumping ratio determines how fast the spring will evolve after a disturbance. At a low value, it'll oscillate for a long time, while closer to 1 it'll stop oscillating quickly")]
		[Range(0.01f, 1f)]
		public float Damping = 0.4f;

		// Token: 0x040010B0 RID: 4272
		[Tooltip("the frequency determines how fast the spring will oscillate when disturbed, low frequency means less oscillations per second, high frequency means more oscillations per second")]
		public float Frequency = 6f;

		// Token: 0x040010B1 RID: 4273
		[MMFInspectorGroup("Spring Mode", true, 19, false, false)]
		[Tooltip("the chosen mode for this spring. MoveTo will move the target the specified scale (randomized between min and max). MoveToAdditive will add the specified scale (randomized between min and max) to the target's current scale. Bump will bump the target's scale by the specified power (randomized between min and max)")]
		public MMF_SquashAndStretchSpring.Modes Mode = MMF_SquashAndStretchSpring.Modes.Bump;

		// Token: 0x040010B2 RID: 4274
		[Tooltip("the min value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public float MoveToMin = 1f;

		// Token: 0x040010B3 RID: 4275
		[Tooltip("the max value from which to pick a random target value when in MoveTo or MoveToAdditive modes")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public float MoveToMax = 2f;

		// Token: 0x040010B4 RID: 4276
		[Tooltip("the min value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float BumpScaleMin = 20f;

		// Token: 0x040010B5 RID: 4277
		[Tooltip("the max value from which to pick a random bump amount when in Bump mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float BumpScaleMax = 30f;

		// Token: 0x040010B6 RID: 4278
		protected float _currentValue;

		// Token: 0x040010B7 RID: 4279
		protected float _targetValue;

		// Token: 0x040010B8 RID: 4280
		protected float _velocity;

		// Token: 0x040010B9 RID: 4281
		protected Coroutine _coroutine;

		// Token: 0x040010BA RID: 4282
		protected float _velocityLowThreshold = 0.001f;

		// Token: 0x040010BB RID: 4283
		protected Vector3 _newScale;

		// Token: 0x040010BC RID: 4284
		protected Vector3 _initialScale;

		// Token: 0x020004F8 RID: 1272
		public enum Modes
		{
			// Token: 0x0400233E RID: 9022
			MoveTo,
			// Token: 0x0400233F RID: 9023
			MoveToAdditive,
			// Token: 0x04002340 RID: 9024
			Bump
		}

		// Token: 0x020004F9 RID: 1273
		public enum PossibleAxis
		{
			// Token: 0x04002342 RID: 9026
			XtoYZ,
			// Token: 0x04002343 RID: 9027
			XtoY,
			// Token: 0x04002344 RID: 9028
			XtoZ,
			// Token: 0x04002345 RID: 9029
			YtoXZ,
			// Token: 0x04002346 RID: 9030
			YtoX,
			// Token: 0x04002347 RID: 9031
			YtoZ,
			// Token: 0x04002348 RID: 9032
			ZtoXZ,
			// Token: 0x04002349 RID: 9033
			ZtoX,
			// Token: 0x0400234A RID: 9034
			ZtoY
		}
	}
}
