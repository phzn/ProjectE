using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001CC RID: 460
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will allow you to send to an animator (bound in its inspector) a bool, int, float or trigger parameter, allowing you to trigger an animation, with or without randomness.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Animation/Animation Parameter")]
	public class MMF_Animation : MMF_Feedback
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00032F4C File Offset: 0x0003114C
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x00032F5A File Offset: 0x0003115A
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00032F63 File Offset: 0x00031163
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00032F66 File Offset: 0x00031166
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00032F69 File Offset: 0x00031169
		protected override void AutomateTargetAcquisition()
		{
			this.BoundAnimator = this.FindAutomatedTarget<Animator>();
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00032F78 File Offset: 0x00031178
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._triggerParameter = Animator.StringToHash(this.TriggerParameterName);
			this._boolParameter = Animator.StringToHash(this.BoolParameterName);
			this._intParameter = Animator.StringToHash(this.IntParameterName);
			this._floatParameter = Animator.StringToHash(this.FloatParameterName);
			this._randomTriggerParameters = new List<int>();
			foreach (string text in this.RandomTriggerParameterNames)
			{
				this._randomTriggerParameters.Add(Animator.StringToHash(text));
			}
			this._randomBoolParameters = new List<int>();
			foreach (string text2 in this.RandomBoolParameterNames)
			{
				this._randomBoolParameters.Add(Animator.StringToHash(text2));
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00033084 File Offset: 0x00031284
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Animation.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.BoundAnimator == null)
			{
				Debug.LogWarning("No animator was set for " + this.Owner.name);
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.ApplyValue(this.BoundAnimator, num);
			foreach (Animator animator in this.ExtraBoundAnimators)
			{
				this.ApplyValue(animator, num);
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00033128 File Offset: 0x00031328
		protected virtual void ApplyValue(Animator targetAnimator, float intensityMultiplier)
		{
			if (this.UpdateTrigger)
			{
				if (this.TriggerMode == MMF_Animation.TriggerModes.SetTrigger)
				{
					targetAnimator.SetTrigger(this._triggerParameter);
				}
				if (this.TriggerMode == MMF_Animation.TriggerModes.ResetTrigger)
				{
					targetAnimator.ResetTrigger(this._triggerParameter);
				}
			}
			if (this.UpdateRandomTrigger)
			{
				int num = this._randomTriggerParameters[Random.Range(0, this._randomTriggerParameters.Count)];
				if (this.RandomTriggerMode == MMF_Animation.TriggerModes.SetTrigger)
				{
					targetAnimator.SetTrigger(num);
				}
				if (this.RandomTriggerMode == MMF_Animation.TriggerModes.ResetTrigger)
				{
					targetAnimator.ResetTrigger(num);
				}
			}
			if (this.UpdateBool)
			{
				targetAnimator.SetBool(this._boolParameter, this.BoolParameterValue);
			}
			if (this.UpdateRandomBool)
			{
				int num2 = this._randomBoolParameters[Random.Range(0, this._randomBoolParameters.Count)];
				targetAnimator.SetBool(num2, this.RandomBoolParameterValue);
			}
			switch (this.IntValueMode)
			{
			case MMF_Animation.ValueModes.Constant:
				targetAnimator.SetInteger(this._intParameter, this.IntValue);
				break;
			case MMF_Animation.ValueModes.Random:
			{
				int num3 = Random.Range(this.IntValueMin, this.IntValueMax);
				targetAnimator.SetInteger(this._intParameter, num3);
				break;
			}
			case MMF_Animation.ValueModes.Incremental:
			{
				int num4 = targetAnimator.GetInteger(this._intParameter) + this.IntIncrement;
				targetAnimator.SetInteger(this._intParameter, num4);
				break;
			}
			}
			switch (this.FloatValueMode)
			{
			case MMF_Animation.ValueModes.Constant:
				targetAnimator.SetFloat(this._floatParameter, this.FloatValue * intensityMultiplier);
				break;
			case MMF_Animation.ValueModes.Random:
			{
				float num5 = Random.Range(this.FloatValueMin, this.FloatValueMax) * intensityMultiplier;
				targetAnimator.SetFloat(this._floatParameter, num5);
				break;
			}
			case MMF_Animation.ValueModes.Incremental:
			{
				float num6 = targetAnimator.GetFloat(this._floatParameter) + this.FloatIncrement * intensityMultiplier;
				targetAnimator.SetFloat(this._floatParameter, num6);
				break;
			}
			}
			if (this.SetLayerWeight)
			{
				targetAnimator.SetLayerWeight(this.TargetLayerIndex, this.NewWeight);
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00033304 File Offset: 0x00031504
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !this.UpdateBool || !MMF_Animation.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundAnimator.SetBool(this._boolParameter, false);
			foreach (Animator animator in this.ExtraBoundAnimators)
			{
				animator.SetBool(this._boolParameter, false);
			}
		}

		// Token: 0x04000D31 RID: 3377
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D32 RID: 3378
		[MMFInspectorGroup("Animation", true, 12, true, false)]
		[Tooltip("the animator whose parameters you want to update")]
		public Animator BoundAnimator;

		// Token: 0x04000D33 RID: 3379
		[Tooltip("the list of extra animators whose parameters you want to update")]
		public List<Animator> ExtraBoundAnimators;

		// Token: 0x04000D34 RID: 3380
		[Tooltip("the duration for the player to consider. This won't impact your animation, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual animation, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04000D35 RID: 3381
		[MMFInspectorGroup("Trigger", true, 16, false, false)]
		[Tooltip("if this is true, will update the specified trigger parameter")]
		public bool UpdateTrigger;

		// Token: 0x04000D36 RID: 3382
		[Tooltip("the selected mode to interact with this trigger")]
		[MMFCondition("UpdateTrigger", true)]
		public MMF_Animation.TriggerModes TriggerMode;

		// Token: 0x04000D37 RID: 3383
		[Tooltip("the trigger animator parameter to, well, trigger when the feedback is played")]
		[MMFCondition("UpdateTrigger", true)]
		public string TriggerParameterName;

		// Token: 0x04000D38 RID: 3384
		[MMFInspectorGroup("Random Trigger", true, 20, false, false)]
		[Tooltip("if this is true, will update a random trigger parameter, picked from the list below")]
		public bool UpdateRandomTrigger;

		// Token: 0x04000D39 RID: 3385
		[Tooltip("the selected mode to interact with this trigger")]
		[MMFCondition("UpdateRandomTrigger", true)]
		public MMF_Animation.TriggerModes RandomTriggerMode;

		// Token: 0x04000D3A RID: 3386
		[Tooltip("the trigger animator parameters to trigger at random when the feedback is played")]
		public List<string> RandomTriggerParameterNames;

		// Token: 0x04000D3B RID: 3387
		[MMFInspectorGroup("Bool", true, 17, false, false)]
		[Tooltip("if this is true, will update the specified bool parameter")]
		public bool UpdateBool;

		// Token: 0x04000D3C RID: 3388
		[Tooltip("the bool parameter to turn true when the feedback gets played")]
		[MMFCondition("UpdateBool", true)]
		public string BoolParameterName;

		// Token: 0x04000D3D RID: 3389
		[Tooltip("when in bool mode, whether to set the bool parameter to true or false")]
		[MMFCondition("UpdateBool", true)]
		public bool BoolParameterValue = true;

		// Token: 0x04000D3E RID: 3390
		[MMFInspectorGroup("Random Bool", true, 19, false, false)]
		[Tooltip("if this is true, will update a random bool parameter picked from the list below")]
		public bool UpdateRandomBool;

		// Token: 0x04000D3F RID: 3391
		[Tooltip("when in bool mode, whether to set the bool parameter to true or false")]
		[MMFCondition("UpdateRandomBool", true)]
		public bool RandomBoolParameterValue = true;

		// Token: 0x04000D40 RID: 3392
		[Tooltip("the bool parameter to turn true when the feedback gets played")]
		public List<string> RandomBoolParameterNames;

		// Token: 0x04000D41 RID: 3393
		[MMFInspectorGroup("Int", true, 24, false, false)]
		[Tooltip("the int parameter to turn true when the feedback gets played")]
		public MMF_Animation.ValueModes IntValueMode;

		// Token: 0x04000D42 RID: 3394
		[Tooltip("the int parameter to turn true when the feedback gets played")]
		[MMFEnumCondition("IntValueMode", new int[] { 1, 2, 3 })]
		public string IntParameterName;

		// Token: 0x04000D43 RID: 3395
		[Tooltip("the value to set to that int parameter")]
		[MMFEnumCondition("IntValueMode", new int[] { 1 })]
		public int IntValue;

		// Token: 0x04000D44 RID: 3396
		[Tooltip("the min value (inclusive) to set at random to that int parameter")]
		[MMFEnumCondition("IntValueMode", new int[] { 2 })]
		public int IntValueMin;

		// Token: 0x04000D45 RID: 3397
		[Tooltip("the max value (exclusive) to set at random to that int parameter")]
		[MMFEnumCondition("IntValueMode", new int[] { 2 })]
		public int IntValueMax = 5;

		// Token: 0x04000D46 RID: 3398
		[Tooltip("the value to increment that int parameter by")]
		[MMFEnumCondition("IntValueMode", new int[] { 3 })]
		public int IntIncrement = 1;

		// Token: 0x04000D47 RID: 3399
		[MMFInspectorGroup("Float", true, 22, false, false)]
		[Tooltip("the Float parameter to turn true when the feedback gets played")]
		public MMF_Animation.ValueModes FloatValueMode;

		// Token: 0x04000D48 RID: 3400
		[Tooltip("the float parameter to turn true when the feedback gets played")]
		[MMFEnumCondition("FloatValueMode", new int[] { 1, 2, 3 })]
		public string FloatParameterName;

		// Token: 0x04000D49 RID: 3401
		[Tooltip("the value to set to that float parameter")]
		[MMFEnumCondition("FloatValueMode", new int[] { 1 })]
		public float FloatValue;

		// Token: 0x04000D4A RID: 3402
		[Tooltip("the min value (inclusive) to set at random to that float parameter")]
		[MMFEnumCondition("FloatValueMode", new int[] { 2 })]
		public float FloatValueMin;

		// Token: 0x04000D4B RID: 3403
		[Tooltip("the max value (exclusive) to set at random to that float parameter")]
		[MMFEnumCondition("FloatValueMode", new int[] { 2 })]
		public float FloatValueMax = 5f;

		// Token: 0x04000D4C RID: 3404
		[Tooltip("the value to increment that float parameter by")]
		[MMFEnumCondition("FloatValueMode", new int[] { 3 })]
		public float FloatIncrement = 1f;

		// Token: 0x04000D4D RID: 3405
		[MMFInspectorGroup("Layer Weights", true, 22, false, false)]
		[Tooltip("whether or not to set layer weights on the specified layer when playing this feedback")]
		public bool SetLayerWeight;

		// Token: 0x04000D4E RID: 3406
		[Tooltip("the index of the layer to target when changing layer weights")]
		[MMFCondition("SetLayerWeight", true)]
		public int TargetLayerIndex = 1;

		// Token: 0x04000D4F RID: 3407
		[Tooltip("the new weight to set on the target animator layer")]
		[MMFCondition("SetLayerWeight", true)]
		public float NewWeight = 0.5f;

		// Token: 0x04000D50 RID: 3408
		protected int _triggerParameter;

		// Token: 0x04000D51 RID: 3409
		protected int _boolParameter;

		// Token: 0x04000D52 RID: 3410
		protected int _intParameter;

		// Token: 0x04000D53 RID: 3411
		protected int _floatParameter;

		// Token: 0x04000D54 RID: 3412
		protected List<int> _randomTriggerParameters;

		// Token: 0x04000D55 RID: 3413
		protected List<int> _randomBoolParameters;

		// Token: 0x020004A5 RID: 1189
		public enum TriggerModes
		{
			// Token: 0x040021AC RID: 8620
			SetTrigger,
			// Token: 0x040021AD RID: 8621
			ResetTrigger
		}

		// Token: 0x020004A6 RID: 1190
		public enum ValueModes
		{
			// Token: 0x040021AF RID: 8623
			None,
			// Token: 0x040021B0 RID: 8624
			Constant,
			// Token: 0x040021B1 RID: 8625
			Random,
			// Token: 0x040021B2 RID: 8626
			Incremental
		}
	}
}
