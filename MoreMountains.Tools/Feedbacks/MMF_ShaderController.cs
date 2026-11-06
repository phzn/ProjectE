using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B1 RID: 689
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you trigger a one time play on a target ShaderController.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Renderer/ShaderController")]
	public class MMF_ShaderController : MMF_Feedback
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0005369B File Offset: 0x0005189B
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x0005369E File Offset: 0x0005189E
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000536A1 File Offset: 0x000518A1
		protected override void AutomateTargetAcquisition()
		{
			this.TargetShaderController = this.FindAutomatedTarget<ShaderController>();
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x000536AF File Offset: 0x000518AF
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x000536D2 File Offset: 0x000518D2
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_ShaderController.Modes.OneTime)
				{
					return this.ApplyTimeMultiplier(this.ToDestinationDuration);
				}
				return this.ApplyTimeMultiplier(this.OneTimeDuration);
			}
			set
			{
				this.OneTimeDuration = value;
				this.ToDestinationDuration = value;
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000536E4 File Offset: 0x000518E4
		protected override void CustomInitialization(MMF_Player owner)
		{
			if (this.Active && this.TargetShaderController != null)
			{
				this._oneTimeDurationStorage = this.TargetShaderController.OneTimeDuration;
				this._oneTimeAmplitudeStorage = this.TargetShaderController.OneTimeAmplitude;
				this._oneTimeCurveStorage = this.TargetShaderController.OneTimeCurve;
				this._oneTimeRemapMinStorage = this.TargetShaderController.OneTimeRemapMin;
				this._oneTimeRemapMaxStorage = this.TargetShaderController.OneTimeRemapMax;
				this._toDestinationCurveStorage = this.TargetShaderController.ToDestinationCurve;
				this._toDestinationDurationStorage = this.TargetShaderController.ToDestinationDuration;
				this._toDestinationValueStorage = this.TargetShaderController.ToDestinationValue;
				this._revertToInitialValueAfterEndStorage = this.TargetShaderController.RevertToInitialValueAfterEnd;
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000537A8 File Offset: 0x000519A8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ShaderController.FeedbackTypeAuthorized || this.TargetShaderController == null)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.PerformPlay(this.TargetShaderController, num);
			foreach (ShaderController shaderController in this.TargetShaderControllerList)
			{
				this.PerformPlay(shaderController, num);
			}
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00053830 File Offset: 0x00051A30
		protected virtual void PerformPlay(ShaderController shaderController, float intensityMultiplier)
		{
			shaderController.RevertToInitialValueAfterEnd = this.RevertToInitialValueAfterEnd;
			if (this.Mode == MMF_ShaderController.Modes.OneTime)
			{
				shaderController.OneTimeDuration = this.FeedbackDuration;
				shaderController.GetInitialValueOnOneTime = this.GetInitialValueOnOneTime;
				shaderController.OneTimeAmplitude = this.OneTimeAmplitude;
				shaderController.OneTimeCurve = this.OneTimeCurve;
				if (this.NormalPlayDirection)
				{
					shaderController.OneTimeRemapMin = this.OneTimeRemapMin * intensityMultiplier;
					shaderController.OneTimeRemapMax = this.OneTimeRemapMax * intensityMultiplier;
				}
				else
				{
					shaderController.OneTimeRemapMin = this.OneTimeRemapMax * intensityMultiplier;
					shaderController.OneTimeRemapMax = this.OneTimeRemapMin * intensityMultiplier;
				}
				shaderController.OneTime();
			}
			if (this.Mode == MMF_ShaderController.Modes.ToDestination)
			{
				shaderController.ToColor = this.ToDestinationColor;
				shaderController.ToDestinationCurve = this.ToDestinationCurve;
				shaderController.ToDestinationDuration = this.FeedbackDuration;
				shaderController.ToDestinationValue = this.ToDestinationValue;
				shaderController.ToDestination();
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00053908 File Offset: 0x00051B08
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ShaderController.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.TargetShaderController != null)
			{
				this.TargetShaderController.Stop();
			}
			foreach (ShaderController shaderController in this.TargetShaderControllerList)
			{
				shaderController.Stop();
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005398C File Offset: 0x00051B8C
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.Active && MMF_ShaderController.FeedbackTypeAuthorized && this.TargetShaderController != null)
			{
				this.PerformReset(this.TargetShaderController);
			}
			foreach (ShaderController shaderController in this.TargetShaderControllerList)
			{
				this.PerformReset(shaderController);
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00053A10 File Offset: 0x00051C10
		protected virtual void PerformReset(ShaderController shaderController)
		{
			shaderController.OneTimeDuration = this._oneTimeDurationStorage;
			shaderController.OneTimeAmplitude = this._oneTimeAmplitudeStorage;
			shaderController.OneTimeCurve = this._oneTimeCurveStorage;
			shaderController.OneTimeRemapMin = this._oneTimeRemapMinStorage;
			shaderController.OneTimeRemapMax = this._oneTimeRemapMaxStorage;
			shaderController.ToDestinationCurve = this._toDestinationCurveStorage;
			shaderController.ToDestinationDuration = this._toDestinationDurationStorage;
			shaderController.ToDestinationValue = this._toDestinationValueStorage;
			shaderController.RevertToInitialValueAfterEnd = this._revertToInitialValueAfterEndStorage;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00053A8C File Offset: 0x00051C8C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ShaderController.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetShaderController.RestoreInitialValues();
			foreach (ShaderController shaderController in this.TargetShaderControllerList)
			{
				shaderController.RestoreInitialValues();
			}
		}

		// Token: 0x04001645 RID: 5701
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001646 RID: 5702
		[MMFInspectorGroup("Shader Controller", true, 37, true, false)]
		[Tooltip("the mode this controller is in")]
		public MMF_ShaderController.Modes Mode;

		// Token: 0x04001647 RID: 5703
		[Tooltip("the float controller to trigger a one time play on")]
		public ShaderController TargetShaderController;

		// Token: 0x04001648 RID: 5704
		[Tooltip("an optional list of float controllers to trigger a one time play on")]
		public List<ShaderController> TargetShaderControllerList;

		// Token: 0x04001649 RID: 5705
		[Tooltip("whether this should revert to original at the end")]
		public bool RevertToInitialValueAfterEnd;

		// Token: 0x0400164A RID: 5706
		[Tooltip("whether or not to initialize the initial value to the current value on a OneTime play")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public bool GetInitialValueOnOneTime;

		// Token: 0x0400164B RID: 5707
		[Tooltip("the duration of the One Time shake")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeDuration = 1f;

		// Token: 0x0400164C RID: 5708
		[Tooltip("the amplitude of the One Time shake (this will be multiplied by the curve's height)")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeAmplitude = 1f;

		// Token: 0x0400164D RID: 5709
		[Tooltip("the low value to remap the normalized curve value to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeRemapMin;

		// Token: 0x0400164E RID: 5710
		[Tooltip("the high value to remap the normalized curve value to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeRemapMax = 1f;

		// Token: 0x0400164F RID: 5711
		[Tooltip("the curve to apply to the one time shake")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve OneTimeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001650 RID: 5712
		[Tooltip("the new value towards which to move the current value")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float ToDestinationValue = 1f;

		// Token: 0x04001651 RID: 5713
		[Tooltip("the duration over which to interpolate the target value")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float ToDestinationDuration = 1f;

		// Token: 0x04001652 RID: 5714
		[Tooltip("the color to aim for (when targetting a Color property")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public Color ToDestinationColor = Color.red;

		// Token: 0x04001653 RID: 5715
		[Tooltip("the curve over which to interpolate the value")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public AnimationCurve ToDestinationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001654 RID: 5716
		protected float _oneTimeDurationStorage;

		// Token: 0x04001655 RID: 5717
		protected float _oneTimeAmplitudeStorage;

		// Token: 0x04001656 RID: 5718
		protected float _oneTimeRemapMinStorage;

		// Token: 0x04001657 RID: 5719
		protected float _oneTimeRemapMaxStorage;

		// Token: 0x04001658 RID: 5720
		protected AnimationCurve _oneTimeCurveStorage;

		// Token: 0x04001659 RID: 5721
		protected float _toDestinationValueStorage;

		// Token: 0x0400165A RID: 5722
		protected float _toDestinationDurationStorage;

		// Token: 0x0400165B RID: 5723
		protected AnimationCurve _toDestinationCurveStorage;

		// Token: 0x0400165C RID: 5724
		protected bool _revertToInitialValueAfterEndStorage;

		// Token: 0x02000541 RID: 1345
		public enum Modes
		{
			// Token: 0x04002426 RID: 9254
			OneTime,
			// Token: 0x04002427 RID: 9255
			ToDestination
		}
	}
}
