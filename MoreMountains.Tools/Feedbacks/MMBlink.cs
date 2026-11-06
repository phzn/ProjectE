using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000238 RID: 568
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Various/MMBlink")]
	public class MMBlink : MMMonoBehaviour
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x00045B21 File Offset: 0x00043D21
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00045B36 File Offset: 0x00043D36
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00045B4B File Offset: 0x00043D4B
		public virtual void ToggleBlinking()
		{
			this.Blinking = !this.Blinking;
			this.ResetBlinkProperties();
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00045B62 File Offset: 0x00043D62
		public virtual void StartBlinking()
		{
			base.enabled = true;
			this.Blinking = true;
			this.ResetBlinkProperties();
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00045B78 File Offset: 0x00043D78
		public virtual void StopBlinking()
		{
			this.Blinking = false;
			this.ResetBlinkProperties();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00045B87 File Offset: 0x00043D87
		protected virtual void Update()
		{
			this.DetermineState();
			if (!this.Blinking)
			{
				return;
			}
			this.Blink();
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00045BA0 File Offset: 0x00043DA0
		protected virtual void DetermineState()
		{
			this.DetermineCurrentPhase();
			if (!this.Blinking)
			{
				return;
			}
			if (this.Active)
			{
				if (this.GetTime() - this._lastBlinkAt > this.Phases[this.CurrentPhaseIndex].OnDuration)
				{
					this.Active = false;
					this._lastBlinkAt = this.GetTime();
				}
			}
			else if (this.GetTime() - this._lastBlinkAt > this.Phases[this.CurrentPhaseIndex].OffDuration)
			{
				this.Active = true;
				this._lastBlinkAt = this.GetTime();
			}
			this._currentBlinkDuration = (this.Active ? this.Phases[this.CurrentPhaseIndex].OnDuration : this.Phases[this.CurrentPhaseIndex].OffDuration);
			this._currentLerpDuration = (this.Active ? this.Phases[this.CurrentPhaseIndex].OnLerpDuration : this.Phases[this.CurrentPhaseIndex].OffLerpDuration);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00045CB0 File Offset: 0x00043EB0
		protected virtual void Blink()
		{
			Color currentColor = this._currentColor;
			float num = (this.Active ? this.OffValue : this.OnValue);
			float num2 = (this.Active ? this.OnValue : this.OffValue);
			float num4;
			if (this.LerpValue && this.GetTime() - this._lastBlinkAt < this._currentLerpDuration)
			{
				float num3 = MMFeedbacksHelpers.Remap(this.GetTime() - this._lastBlinkAt, 0f, this._currentLerpDuration, 0f, 1f);
				num4 = this.Curve.Evaluate(num3);
				num4 = MMFeedbacksHelpers.Remap(num4, 0f, 1f, num, num2);
			}
			else
			{
				num4 = num2;
			}
			this.ApplyBlink(this.Active, num4);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00045D6C File Offset: 0x00043F6C
		public virtual float Duration
		{
			get
			{
				if (this.RepeatCount < 0 || this.Phases.Count == 0)
				{
					return 0f;
				}
				float num = 0f;
				foreach (BlinkPhase blinkPhase in this.Phases)
				{
					num += blinkPhase.PhaseDuration;
				}
				return num + num * (float)this.RepeatCount;
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00045DF0 File Offset: 0x00043FF0
		protected virtual void ApplyBlink(bool active, float value)
		{
			switch (this.Method)
			{
			case MMBlink.Methods.SetGameObjectActive:
			{
				this.TargetGameObject.SetActive(active);
				using (List<GameObject>.Enumerator enumerator = this.ExtraGameObjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						gameObject.SetActive(active);
					}
					return;
				}
				break;
			}
			case MMBlink.Methods.MaterialAlpha:
				break;
			case MMBlink.Methods.MaterialEmissionIntensity:
			{
				this._currentColor = this._initialColor * value;
				this.ApplyCurrentColor(this.TargetRenderer);
				for (int i = 0; i < this.ExtraRenderers.Count; i++)
				{
					BlinkTargetRenderer blinkTargetRenderer = this.ExtraRenderers[i];
					this.ApplyCurrentColor(blinkTargetRenderer.TargetRenderer);
				}
				return;
			}
			case MMBlink.Methods.ShaderFloatValue:
			{
				this.ApplyFloatValue(this.TargetRenderer, value);
				for (int j = 0; j < this.ExtraRenderers.Count; j++)
				{
					BlinkTargetRenderer blinkTargetRenderer2 = this.ExtraRenderers[j];
					this.ApplyFloatValue(blinkTargetRenderer2.TargetRenderer, value);
				}
				return;
			}
			default:
				return;
			}
			this._currentColor.a = value;
			this.ApplyCurrentColor(this.TargetRenderer);
			for (int k = 0; k < this.ExtraRenderers.Count; k++)
			{
				BlinkTargetRenderer blinkTargetRenderer3 = this.ExtraRenderers[k];
				this.ApplyCurrentColor(blinkTargetRenderer3.TargetRenderer);
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00045F50 File Offset: 0x00044150
		protected virtual void ApplyFloatValue(Renderer targetRenderer, float value)
		{
			if (this.UseMaterialPropertyBlocks)
			{
				targetRenderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndex);
				this._propertyBlock.SetFloat(this._propertyID, value);
				targetRenderer.SetPropertyBlock(this._propertyBlock);
				return;
			}
			targetRenderer.materials[this.MaterialIndex].SetFloat(this._propertyID, value);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00045FB0 File Offset: 0x000441B0
		protected virtual void ApplyCurrentColor(Renderer targetRenderer)
		{
			if (this.UseMaterialPropertyBlocks)
			{
				targetRenderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndex);
				this._propertyBlock.SetColor(this._propertyID, this._currentColor);
				targetRenderer.SetPropertyBlock(this._propertyBlock);
				return;
			}
			targetRenderer.materials[this.MaterialIndex].SetColor(this._propertyID, this._currentColor);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0004601C File Offset: 0x0004421C
		protected virtual void DetermineCurrentPhase()
		{
			if (this.Phases[this.CurrentPhaseIndex].PhaseDuration <= 0f)
			{
				return;
			}
			if (this.GetTime() - this._currentPhaseStartedAt > this.Phases[this.CurrentPhaseIndex].PhaseDuration)
			{
				this.CurrentPhaseIndex++;
				this._currentPhaseStartedAt = this.GetTime();
			}
			if (this.CurrentPhaseIndex > this.Phases.Count - 1)
			{
				this.CurrentPhaseIndex = 0;
				if (this.RepeatCount != -1)
				{
					this._repeatCount--;
					if (this._repeatCount < 0)
					{
						this.ResetBlinkProperties();
						if (this.ForceStateOnExit)
						{
							if (this.StateOnExit == MMBlink.States.Off)
							{
								this.ApplyBlink(false, 0f);
							}
							else
							{
								this.ApplyBlink(true, 1f);
							}
						}
						this.Blinking = false;
					}
				}
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000460FA File Offset: 0x000442FA
		protected virtual void OnEnable()
		{
			this.InitializeBlinkProperties();
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00046104 File Offset: 0x00044304
		protected virtual void InitializeBlinkProperties()
		{
			if (this.Phases.Count == 0)
			{
				Debug.LogError("MMBlink : You need to define at least one phase for this component to work.");
				base.enabled = false;
				return;
			}
			this._currentPhaseStartedAt = this.GetTime();
			this.CurrentPhaseIndex = 0;
			this._repeatCount = this.RepeatCount;
			this._propertyBlock = new MaterialPropertyBlock();
			switch (this.Method)
			{
			case MMBlink.Methods.MaterialAlpha:
				this.GetInitialColor();
				return;
			case MMBlink.Methods.MaterialEmissionIntensity:
				this.GetInitialColor();
				return;
			case MMBlink.Methods.ShaderFloatValue:
				this.GetInitialFloatValue();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0004618C File Offset: 0x0004438C
		protected virtual void GetInitialColor()
		{
			this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndex);
			this._propertyID = Shader.PropertyToID(this.ShaderPropertyName);
			this._initialColor = (this.UseMaterialPropertyBlocks ? this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetColor(this._propertyID) : this.TargetRenderer.materials[this.MaterialIndex].GetColor(this._propertyID));
			this._currentColor = this._initialColor;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00046218 File Offset: 0x00044418
		protected virtual void GetInitialFloatValue()
		{
			this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndex);
			this._propertyID = Shader.PropertyToID(this.ShaderPropertyName);
			this._initialShaderFloatValue = (this.UseMaterialPropertyBlocks ? this.TargetRenderer.sharedMaterials[this.MaterialIndex].GetFloat(this._propertyID) : this.TargetRenderer.materials[this.MaterialIndex].GetFloat(this._propertyID));
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00046298 File Offset: 0x00044498
		protected virtual void ResetBlinkProperties()
		{
			this._currentPhaseStartedAt = this.GetTime();
			this.CurrentPhaseIndex = 0;
			this._repeatCount = this.RepeatCount;
			float num = 1f;
			if (this.Method == MMBlink.Methods.ShaderFloatValue)
			{
				num = this._initialShaderFloatValue;
			}
			this.ApplyBlink(false, num);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x000462E2 File Offset: 0x000444E2
		protected void OnDisable()
		{
			if (this.ForceStateOnExit)
			{
				if (this.StateOnExit == MMBlink.States.Off)
				{
					this.ApplyBlink(false, 0f);
					return;
				}
				this.ApplyBlink(true, 1f);
			}
		}

		// Token: 0x0400121F RID: 4639
		[MMInspectorGroup("Blink Method", true, 17, false)]
		[Tooltip("the selected method to blink the target object")]
		public MMBlink.Methods Method;

		// Token: 0x04001220 RID: 4640
		[Tooltip("the object to set active/inactive if that method was chosen")]
		[MMFEnumCondition("Method", new int[] { 0 })]
		public GameObject TargetGameObject;

		// Token: 0x04001221 RID: 4641
		[Tooltip("the target renderer to work with")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public Renderer TargetRenderer;

		// Token: 0x04001222 RID: 4642
		[Tooltip("the material index to target")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public int MaterialIndex;

		// Token: 0x04001223 RID: 4643
		[Tooltip("the shader property to alter a float on")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public string ShaderPropertyName = "_Color";

		// Token: 0x04001224 RID: 4644
		[Tooltip("the value to apply when blinking is off")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public float OffValue;

		// Token: 0x04001225 RID: 4645
		[Tooltip("the value to apply when blinking is on")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public float OnValue = 1f;

		// Token: 0x04001226 RID: 4646
		[Tooltip("whether to lerp these values or not")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public bool LerpValue = true;

		// Token: 0x04001227 RID: 4647
		[Tooltip("the curve to apply to the lerping")]
		[MMFEnumCondition("Method", new int[] { 1, 2, 3 })]
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1.05f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001228 RID: 4648
		[Tooltip("if this is true, this component will use material property blocks instead of working on an instance of the material.")]
		public bool UseMaterialPropertyBlocks;

		// Token: 0x04001229 RID: 4649
		[MMInspectorGroup("Extra Targets", true, 12, false)]
		[Tooltip("a list of optional extra renderers and their material index to target")]
		public List<BlinkTargetRenderer> ExtraRenderers;

		// Token: 0x0400122A RID: 4650
		[Tooltip("a list of optional extra game objects to target")]
		public List<GameObject> ExtraGameObjects;

		// Token: 0x0400122B RID: 4651
		[MMInspectorGroup("State", true, 18, false)]
		[Tooltip("whether the object should blink or not")]
		public bool Blinking = true;

		// Token: 0x0400122C RID: 4652
		[Tooltip("whether or not to force a certain state on exit")]
		public bool ForceStateOnExit;

		// Token: 0x0400122D RID: 4653
		[Tooltip("the state to apply on exit")]
		[MMFCondition("ForceStateOnExit", true)]
		public MMBlink.States StateOnExit;

		// Token: 0x0400122E RID: 4654
		[MMInspectorGroup("TimeScale", true, 120, false)]
		[Tooltip("whether or not this MMBlink should operate on unscaled time")]
		public TimescaleModes TimescaleMode;

		// Token: 0x0400122F RID: 4655
		[MMInspectorGroup("Sequence", true, 121, false)]
		[Tooltip("how many times the sequence should repeat (-1 : infinite)")]
		public int RepeatCount;

		// Token: 0x04001230 RID: 4656
		[Tooltip("The list of phases to apply blinking with")]
		public List<BlinkPhase> Phases;

		// Token: 0x04001231 RID: 4657
		[MMInspectorGroup("Debug", true, 122, false)]
		[MMInspectorButtonBar(new string[] { "ToggleBlinking", "StartBlinking", "StopBlinking" }, new string[] { "ToggleBlinking", "StartBlinking", "StopBlinking" }, new bool[] { true, true, true }, new string[] { "main-call-to-action", "", "" })]
		public bool DebugToolbar;

		// Token: 0x04001232 RID: 4658
		[Tooltip("is the blinking object in an active state right now?")]
		[MMFReadOnly]
		public bool Active;

		// Token: 0x04001233 RID: 4659
		[Tooltip("the index of the phase we're currently in")]
		[MMFReadOnly]
		public int CurrentPhaseIndex;

		// Token: 0x04001234 RID: 4660
		protected float _lastBlinkAt;

		// Token: 0x04001235 RID: 4661
		protected float _currentPhaseStartedAt;

		// Token: 0x04001236 RID: 4662
		protected float _currentBlinkDuration;

		// Token: 0x04001237 RID: 4663
		protected float _currentLerpDuration;

		// Token: 0x04001238 RID: 4664
		protected int _propertyID;

		// Token: 0x04001239 RID: 4665
		protected float _initialShaderFloatValue;

		// Token: 0x0400123A RID: 4666
		protected Color _initialColor;

		// Token: 0x0400123B RID: 4667
		protected Color _currentColor;

		// Token: 0x0400123C RID: 4668
		protected int _repeatCount;

		// Token: 0x0400123D RID: 4669
		protected MaterialPropertyBlock _propertyBlock;

		// Token: 0x0400123E RID: 4670
		protected List<MaterialPropertyBlock> _extraPropertyBlocks;

		// Token: 0x0400123F RID: 4671
		protected List<Color> _extraInitialColors;

		// Token: 0x02000513 RID: 1299
		public enum States
		{
			// Token: 0x04002393 RID: 9107
			On,
			// Token: 0x04002394 RID: 9108
			Off
		}

		// Token: 0x02000514 RID: 1300
		public enum Methods
		{
			// Token: 0x04002396 RID: 9110
			SetGameObjectActive,
			// Token: 0x04002397 RID: 9111
			MaterialAlpha,
			// Token: 0x04002398 RID: 9112
			MaterialEmissionIntensity,
			// Token: 0x04002399 RID: 9113
			ShaderFloatValue
		}
	}
}
