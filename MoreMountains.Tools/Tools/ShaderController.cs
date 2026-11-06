using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000116 RID: 278
	[MMRequiresConstantRepaint]
	[AddComponentMenu("More Mountains/Tools/Property Controllers/ShaderController")]
	public class ShaderController : MMMonoBehaviour
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
		public virtual bool FindShaderProperty(string propertyName)
		{
			if (this.TargetType == ShaderController.TargetTypes.Renderer)
			{
				if (this.CreateMaterialInstance)
				{
					this.TargetRenderer.materials[this.TargetMaterialID] = new Material(this.TargetRenderer.materials[this.TargetMaterialID]);
				}
				this.TargetMaterial = (this.UseMaterialPropertyBlocks ? this.TargetRenderer.sharedMaterials[this.TargetMaterialID] : this.TargetRenderer.materials[this.TargetMaterialID]);
			}
			else if (this.TargetType == ShaderController.TargetTypes.Image)
			{
				if (this.CreateMaterialInstance)
				{
					this.TargetImage.material = new Material(this.TargetImage.material);
				}
				this.TargetMaterial = this.TargetImage.material;
			}
			else if (this.TargetType == ShaderController.TargetTypes.RawImage)
			{
				if (this.CreateMaterialInstance)
				{
					this.TargetRawImage.material = new Material(this.TargetRawImage.material);
				}
				this.TargetMaterial = this.TargetRawImage.material;
			}
			else if (this.TargetType == ShaderController.TargetTypes.Text)
			{
				if (this.CreateMaterialInstance)
				{
					this.TargetText.material = new Material(this.TargetText.material);
				}
				this.TargetMaterial = this.TargetText.material;
			}
			if (this.PropertyType == ShaderController.PropertyTypes.Keyword)
			{
				this.PropertyFound = true;
				return true;
			}
			if (this.TargetMaterial.HasProperty(propertyName))
			{
				this.PropertyID = Shader.PropertyToID(propertyName);
				this.PropertyFound = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001E750 File Offset: 0x0001C950
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001E758 File Offset: 0x0001C958
		protected virtual void OnEnable()
		{
			this.InitialValue = this.GetInitialValue();
			if (this.PropertyType == ShaderController.PropertyTypes.Color)
			{
				this.InitialColor = this.TargetMaterial.GetColor(this.PropertyID);
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001E788 File Offset: 0x0001C988
		protected virtual bool RendererIsNull()
		{
			return (this.TargetType == ShaderController.TargetTypes.Renderer && this.TargetRenderer == null) || (this.TargetType == ShaderController.TargetTypes.Image && this.TargetImage == null) || (this.TargetType == ShaderController.TargetTypes.RawImage && this.TargetRawImage == null) || (this.TargetType == ShaderController.TargetTypes.Text && this.TargetText == null);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public virtual void Initialization()
		{
			if (this.RendererIsNull() || string.IsNullOrEmpty(this.TargetPropertyName))
			{
				return;
			}
			if (this.TargetType != ShaderController.TargetTypes.Renderer)
			{
				this.UseMaterialPropertyBlocks = false;
			}
			this.StoreSpriteRenderer();
			this.PropertyFound = this.FindShaderProperty(this.TargetPropertyName);
			if (!this.PropertyFound)
			{
				return;
			}
			this._elapsedTime = 0f;
			this._randomAmplitude = Random.Range(this.Amplitude.x, this.Amplitude.y);
			this._randomFrequency = Random.Range(this.Frequency.x, this.Frequency.y);
			this._randomShift = Random.Range(this.Shift.x, this.Shift.y);
			if (this.TargetType == ShaderController.TargetTypes.Renderer && this.UseMaterialPropertyBlocks)
			{
				this._propertyBlock = new MaterialPropertyBlock();
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
			}
			this.InitialValue = this.GetInitialValue();
			if (this.PropertyType == ShaderController.PropertyTypes.Color)
			{
				this.InitialColor = this.TargetMaterial.GetColor(this.PropertyID);
			}
			this._shaking = false;
			if (this.ControlMode == ShaderController.ControlModes.OneTime)
			{
				base.enabled = false;
			}
			this.StoreSpriteRendererTexture();
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001E938 File Offset: 0x0001CB38
		public virtual void StoreSpriteRenderer()
		{
			this._spriteRenderer = ((this.TargetRenderer != null) ? this.TargetRenderer.GetComponent<SpriteRenderer>() : null);
			this.SpriteRendererIsNull = this._spriteRenderer == null;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001E96E File Offset: 0x0001CB6E
		public virtual void StoreSpriteRendererTexture()
		{
			if (this.SpriteRendererIsNull)
			{
				return;
			}
			this._spriteRendererTexture = this._spriteRenderer.sprite.texture;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001E98F File Offset: 0x0001CB8F
		protected virtual void SetStoredSpriteRendererTexture(MaterialPropertyBlock block)
		{
			if (this.SpriteRendererIsNull)
			{
				return;
			}
			block.SetTexture(this.SpriteRendererTextureProperty, this._spriteRendererTexture);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001E9AC File Offset: 0x0001CBAC
		public virtual void SetDrivenLevelAbsolute(float level)
		{
			this.DrivenLevel = level;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001E9B5 File Offset: 0x0001CBB5
		public virtual void SetDrivenLevelNormalized(float normalizedLevel, float remapZero, float remapOne)
		{
			this.DrivenLevel = MMMaths.Remap(normalizedLevel, 0f, 1f, remapZero, remapOne);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001E9D0 File Offset: 0x0001CBD0
		public virtual void OneTime()
		{
			if (!this.CacheMaterial)
			{
				this.Initialization();
			}
			if (this.GetInitialValueOnOneTime)
			{
				this.InitialValue = this.GetInitialValue();
			}
			if (this.RendererIsNull() || !this.PropertyFound)
			{
				return;
			}
			base.gameObject.SetActive(true);
			base.enabled = true;
			this.ControlMode = ShaderController.ControlModes.OneTime;
			this._startedTimestamp = this.GetTime();
			this._shaking = true;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001EA40 File Offset: 0x0001CC40
		public virtual void ToDestination()
		{
			if (!this.CacheMaterial)
			{
				this.Initialization();
			}
			if (this.RendererIsNull() || !this.PropertyFound)
			{
				return;
			}
			base.enabled = true;
			if (this.PropertyType == ShaderController.PropertyTypes.Color)
			{
				this._fromColorStorage = this.FromColor;
				this.FromColor = this.TargetMaterial.GetColor(this.PropertyID);
			}
			this.ControlMode = ShaderController.ControlModes.ToDestination;
			this._startedTimestamp = this.GetTime();
			this._shaking = true;
			this._initialValue = this.GetInitialValue();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001EAC5 File Offset: 0x0001CCC5
		public void SetFromColor(Color newColor)
		{
			this.FromColor = newColor;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001EACE File Offset: 0x0001CCCE
		public void SetToColor(Color newColor)
		{
			this.ToColor = newColor;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001EAD7 File Offset: 0x0001CCD7
		public virtual void SetRemapOneTimeMin(float newValue)
		{
			this.OneTimeRemapMin = newValue;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
		public virtual void SetRemapOneTimeMax(float newValue)
		{
			this.OneTimeRemapMax = newValue;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001EAE9 File Offset: 0x0001CCE9
		public virtual void SetToDestinationValue(float newValue)
		{
			this.ToDestinationValue = newValue;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001EAF2 File Offset: 0x0001CCF2
		protected float GetDeltaTime()
		{
			if (!this.UseUnscaledTime)
			{
				return Time.deltaTime;
			}
			return Time.unscaledDeltaTime;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001EB07 File Offset: 0x0001CD07
		protected float GetTime()
		{
			if (!this.UseUnscaledTime)
			{
				return Time.time;
			}
			return Time.unscaledTime;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001EB1C File Offset: 0x0001CD1C
		protected virtual void Update()
		{
			this.UpdateValue();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001EB24 File Offset: 0x0001CD24
		protected virtual void OnDisable()
		{
			if (this.RevertToInitialValueAfterEnd)
			{
				this.CurrentValue = this.InitialValue;
				this._currentColor = this.InitialColor;
				this.SetValue(this.CurrentValue);
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001EB54 File Offset: 0x0001CD54
		protected virtual void UpdateValue()
		{
			if (this.SafeMode && (this.RendererIsNull() || !this.PropertyFound))
			{
				return;
			}
			switch (this.ControlMode)
			{
			case ShaderController.ControlModes.PingPong:
				if (this.GetTime() - this._lastPingPongPauseAt < this.PingPongPauseDuration)
				{
					return;
				}
				this.PingPong += this.GetDeltaTime() * this._pingPongDirection;
				if (this.PingPong < 0f)
				{
					this.PingPong = 0f;
					this._pingPongDirection = -this._pingPongDirection;
					this._lastPingPongPauseAt = this.GetTime();
				}
				if (this.PingPong > this.Duration)
				{
					this.PingPong = this.Duration;
					this._pingPongDirection = -this._pingPongDirection;
					this._lastPingPongPauseAt = this.GetTime();
				}
				this.CurrentValue = MMTween.Tween(this.PingPong, 0f, this.Duration, this.MinValue, this.MaxValue, this.Curve);
				this.CurrentValueNormalized = MMMaths.Remap(this.CurrentValue, this.MinValue, this.MaxValue, 0f, 1f);
				break;
			case ShaderController.ControlModes.Random:
				this._elapsedTime += this.GetDeltaTime();
				this.CurrentValueNormalized = Mathf.PerlinNoise(this._randomFrequency * this._elapsedTime, this._randomShift);
				if (this.RemapNoiseValues)
				{
					this.CurrentValue = this.CurrentValueNormalized;
					this.CurrentValue = MMMaths.Remap(this.CurrentValue, 0f, 1f, this.RemapNoiseZero, this.RemapNoiseOne);
				}
				else
				{
					this.CurrentValue = (this.CurrentValueNormalized * 2f - 1f) * this._randomAmplitude;
				}
				break;
			case ShaderController.ControlModes.OneTime:
				if (!this._shaking)
				{
					return;
				}
				this._remappedTimeSinceStart = MMMaths.Remap(this.GetTime() - this._startedTimestamp, 0f, this.OneTimeDuration, 0f, 1f);
				this.CurrentValueNormalized = this.OneTimeCurve.Evaluate(this._remappedTimeSinceStart);
				this.CurrentValue = MMMaths.Remap(this.CurrentValueNormalized, 0f, 1f, this.OneTimeRemapMin, this.OneTimeRemapMax);
				this.CurrentValue *= this.OneTimeAmplitude;
				break;
			case ShaderController.ControlModes.AudioAnalyzer:
				this.CurrentValue = Mathf.Lerp(this.CurrentValue, this.AudioAnalyzer.Beats[this.BeatID].CurrentValue * this.AudioAnalyzerMultiplier + this.AudioAnalyzerOffset, this.AudioAnalyzerLerp * this.GetDeltaTime());
				this.CurrentValueNormalized = Mathf.Clamp(this.AudioAnalyzer.Beats[this.BeatID].CurrentValue, 0f, 1f);
				break;
			case ShaderController.ControlModes.ToDestination:
			{
				if (!this._shaking)
				{
					return;
				}
				this._remappedTimeSinceStart = MMMaths.Remap(this.GetTime() - this._startedTimestamp, 0f, this.ToDestinationDuration, 0f, 1f);
				float num = this.ToDestinationCurve.Evaluate(this._remappedTimeSinceStart);
				this.CurrentValue = Mathf.LerpUnclamped(this._initialValue, this.ToDestinationValue, num);
				this.CurrentValueNormalized = MMMaths.Remap(this.CurrentValue, this._initialValue, this.ToDestinationValue, 0f, 1f);
				break;
			}
			case ShaderController.ControlModes.Driven:
				this.CurrentValue = this.DrivenLevel;
				this.CurrentValueNormalized = Mathf.Clamp(this.CurrentValue, 0f, 1f);
				break;
			case ShaderController.ControlModes.Loop:
				if (this.GetTime() - this._lastLoopPauseAt < this.LoopPauseDuration)
				{
					return;
				}
				this.LoopTime += this.GetDeltaTime();
				if (this.LoopTime > this.LoopDuration)
				{
					this.LoopTime = 0f;
					this._lastLoopPauseAt = this.GetTime();
				}
				this.CurrentValue = MMTween.Tween(this.LoopTime, 0f, this.LoopDuration, this.LoopStartValue, this.LoopEndValue, this.LoopCurve);
				this.CurrentValueNormalized = MMMaths.Remap(this.CurrentValue, this.LoopStartValue, this.LoopEndValue, 0f, 1f);
				break;
			}
			if (this.PropertyType == ShaderController.PropertyTypes.Color)
			{
				if (this.ColorMode == ShaderController.ColorModes.TwoColors)
				{
					this._currentColor = Color.Lerp(this.FromColor, this.ToColor, this.CurrentValue);
				}
				else
				{
					this._currentColor = this.ColorRamp.Evaluate(this.CurrentValue);
				}
			}
			if (this.AddToInitialValue)
			{
				this.CurrentValue += this.InitialValue;
			}
			if (this.ControlMode == ShaderController.ControlModes.OneTime && this._shaking && this.GetTime() - this._startedTimestamp > this.OneTimeDuration)
			{
				this._shaking = false;
				if (this.RevertToInitialValueAfterEnd)
				{
					this.CurrentValue = this.InitialValue;
					if (this.PropertyType == ShaderController.PropertyTypes.Color)
					{
						this._currentColor = this.InitialColor;
					}
				}
				else
				{
					this.CurrentValue = this.OneTimeCurve.Evaluate(1f);
					this.CurrentValue = MMMaths.Remap(this.CurrentValue, 0f, 1f, this.OneTimeRemapMin, this.OneTimeRemapMax);
					this.CurrentValue *= this.OneTimeAmplitude;
					if (this.AddToInitialValue)
					{
						this.CurrentValue += this.InitialValue;
					}
				}
				this.SetValue(this.CurrentValue);
				if (this.DisableAfterOneTime)
				{
					base.enabled = false;
				}
				if (this.DisableGameObjectAfterOneTime)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			if (this.ControlMode == ShaderController.ControlModes.ToDestination && this._shaking && this.GetTime() - this._startedTimestamp > this.ToDestinationDuration)
			{
				this._shaking = false;
				this.FromColor = this._fromColorStorage;
				if (this.RevertToInitialValueAfterEnd)
				{
					this.CurrentValue = this.InitialValue;
					if (this.PropertyType == ShaderController.PropertyTypes.Color)
					{
						this._currentColor = this.InitialColor;
					}
				}
				else
				{
					this.CurrentValue = this.ToDestinationValue;
				}
				this.SetValue(this.CurrentValue);
				if (this.DisableAfterToDestination)
				{
					base.enabled = false;
				}
				return;
			}
			this.SetValue(this.CurrentValue);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001F180 File Offset: 0x0001D380
		protected virtual float GetInitialValue()
		{
			if (this.TargetMaterial == null)
			{
				Debug.LogWarning("Material is null", this);
				return 0f;
			}
			switch (this.PropertyType)
			{
			case ShaderController.PropertyTypes.Bool:
				return (float)this.TargetMaterial.GetInt(this.PropertyID);
			case ShaderController.PropertyTypes.Float:
				return this.TargetMaterial.GetFloat(this.PropertyID);
			case ShaderController.PropertyTypes.Int:
				return (float)this.TargetMaterial.GetInt(this.PropertyID);
			case ShaderController.PropertyTypes.Vector:
				return this.TargetMaterial.GetVector(this.PropertyID).x;
			case ShaderController.PropertyTypes.Keyword:
				if (!this.TargetMaterial.IsKeywordEnabled(this.TargetPropertyName))
				{
					return 0f;
				}
				return 1f;
			case ShaderController.PropertyTypes.Color:
				if (this.ControlMode != ShaderController.ControlModes.ToDestination)
				{
					this.InitialColor = this.TargetMaterial.GetColor(this.PropertyID);
				}
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001F270 File Offset: 0x0001D470
		protected virtual void SetValue(float newValue)
		{
			if (this.TargetType == ShaderController.TargetTypes.Image && this.UseMaterialForRendering)
			{
				if (this.SafeMode && this.TargetImage == null)
				{
					return;
				}
				this.TargetMaterial = this.TargetImage.materialForRendering;
			}
			switch (this.PropertyType)
			{
			case ShaderController.PropertyTypes.Bool:
			{
				newValue = ((newValue > 0f) ? 1f : 0f);
				int num = Mathf.RoundToInt(newValue);
				if (!this.UseMaterialPropertyBlocks)
				{
					this.TargetMaterial.SetInt(this.PropertyID, num);
					return;
				}
				if (this.TargetRenderer == null)
				{
					return;
				}
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				this.StoreSpriteRendererTexture();
				this._propertyBlock.SetInt(this.PropertyID, num);
				this.SetStoredSpriteRendererTexture(this._propertyBlock);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				return;
			}
			case ShaderController.PropertyTypes.Float:
				if (!this.UseMaterialPropertyBlocks)
				{
					this.TargetMaterial.SetFloat(this.PropertyID, newValue);
					return;
				}
				if (this.TargetRenderer == null)
				{
					return;
				}
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				this.StoreSpriteRendererTexture();
				this._propertyBlock.SetFloat(this.PropertyID, newValue);
				this.SetStoredSpriteRendererTexture(this._propertyBlock);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				return;
			case ShaderController.PropertyTypes.Int:
			{
				int num2 = Mathf.RoundToInt(newValue);
				if (!this.UseMaterialPropertyBlocks)
				{
					this.TargetMaterial.SetInt(this.PropertyID, num2);
					return;
				}
				if (this.TargetRenderer == null)
				{
					return;
				}
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				this.StoreSpriteRendererTexture();
				this._propertyBlock.SetInt(this.PropertyID, num2);
				this.SetStoredSpriteRendererTexture(this._propertyBlock);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				return;
			}
			case ShaderController.PropertyTypes.Vector:
				this._vectorValue = this.TargetMaterial.GetVector(this.PropertyID);
				if (this.X)
				{
					this._vectorValue.x = newValue;
				}
				if (this.Y)
				{
					this._vectorValue.y = newValue;
				}
				if (this.Z)
				{
					this._vectorValue.z = newValue;
				}
				if (this.W)
				{
					this._vectorValue.w = newValue;
				}
				if (!this.UseMaterialPropertyBlocks)
				{
					this.TargetMaterial.SetVector(this.PropertyID, this._vectorValue);
					return;
				}
				if (this.TargetRenderer == null)
				{
					return;
				}
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				this._propertyBlock.SetVector(this.PropertyID, this._vectorValue);
				this.SetStoredSpriteRendererTexture(this._propertyBlock);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				return;
			case ShaderController.PropertyTypes.Keyword:
				newValue = ((newValue > 0f) ? 1f : 0f);
				if (newValue == 0f)
				{
					this.TargetMaterial.DisableKeyword(this.TargetPropertyName);
					return;
				}
				this.TargetMaterial.EnableKeyword(this.TargetPropertyName);
				return;
			case ShaderController.PropertyTypes.Color:
				if (!this.UseMaterialPropertyBlocks)
				{
					this.TargetMaterial.SetColor(this.PropertyID, this._currentColor);
					return;
				}
				if (this.TargetRenderer == null)
				{
					return;
				}
				this.TargetRenderer.GetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				this.StoreSpriteRendererTexture();
				this._propertyBlock.SetColor(this.PropertyID, this._currentColor);
				this.SetStoredSpriteRendererTexture(this._propertyBlock);
				this.TargetRenderer.SetPropertyBlock(this._propertyBlock, this.TargetMaterialID);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001F627 File Offset: 0x0001D827
		public virtual void Stop()
		{
			this._shaking = false;
			base.enabled = false;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001F637 File Offset: 0x0001D837
		public virtual void RestoreInitialValues()
		{
			this._currentColor = this.InitialColor;
			this.SetValue(this.InitialValue);
		}

		// Token: 0x0400076D RID: 1901
		[Header("Target")]
		[Tooltip("the type of renderer to pilot")]
		public ShaderController.TargetTypes TargetType;

		// Token: 0x0400076E RID: 1902
		[Tooltip("the renderer with the shader you want to control")]
		[MMEnumCondition("TargetType", new int[] { 0 })]
		public Renderer TargetRenderer;

		// Token: 0x0400076F RID: 1903
		[Tooltip("the ID of the material in the Materials array on the target renderer (usually 0)")]
		[MMEnumCondition("TargetType", new int[] { 0 })]
		public int TargetMaterialID;

		// Token: 0x04000770 RID: 1904
		[Tooltip("the Image with the shader you want to control")]
		[MMEnumCondition("TargetType", new int[] { 1 })]
		public Image TargetImage;

		// Token: 0x04000771 RID: 1905
		[Tooltip("if this is true, the 'materialForRendering' for this Image will be used, instead of the regular material")]
		[MMEnumCondition("TargetType", new int[] { 1 })]
		public bool UseMaterialForRendering;

		// Token: 0x04000772 RID: 1906
		[Tooltip("the RawImage with the shader you want to control")]
		[MMEnumCondition("TargetType", new int[] { 2 })]
		public RawImage TargetRawImage;

		// Token: 0x04000773 RID: 1907
		[Tooltip("the Text with the shader you want to control")]
		[MMEnumCondition("TargetType", new int[] { 3 })]
		public Text TargetText;

		// Token: 0x04000774 RID: 1908
		[Tooltip("if this is true, material will be cached on Start")]
		public bool CacheMaterial = true;

		// Token: 0x04000775 RID: 1909
		[Tooltip("if this is true, an instance of the material will be created on start so that this controller only affects its target")]
		public bool CreateMaterialInstance;

		// Token: 0x04000776 RID: 1910
		[Tooltip("the EXACT name of the property to affect")]
		public string TargetPropertyName;

		// Token: 0x04000777 RID: 1911
		[Tooltip("the type of the property to affect")]
		public ShaderController.PropertyTypes PropertyType = ShaderController.PropertyTypes.Float;

		// Token: 0x04000778 RID: 1912
		[Tooltip("whether or not to affect its x component")]
		[MMEnumCondition("PropertyType", new int[] { 3 })]
		public bool X;

		// Token: 0x04000779 RID: 1913
		[Tooltip("whether or not to affect its y component")]
		[MMEnumCondition("PropertyType", new int[] { 3 })]
		public bool Y;

		// Token: 0x0400077A RID: 1914
		[Tooltip("whether or not to affect its z component")]
		[MMEnumCondition("PropertyType", new int[] { 3 })]
		public bool Z;

		// Token: 0x0400077B RID: 1915
		[Tooltip("whether or not to affect its w component")]
		[MMEnumCondition("PropertyType", new int[] { 3 })]
		public bool W;

		// Token: 0x0400077C RID: 1916
		[Header("Color")]
		[Tooltip("whether to move from a color to another, or to evalute colors on a ramp")]
		public ShaderController.ColorModes ColorMode;

		// Token: 0x0400077D RID: 1917
		[Tooltip("the ramp along which to lerp when in ramp color mode")]
		[GradientUsage(true)]
		public Gradient ColorRamp;

		// Token: 0x0400077E RID: 1918
		[Tooltip("the color to lerp from")]
		[ColorUsage(true, true)]
		public Color FromColor = Color.black;

		// Token: 0x0400077F RID: 1919
		[Tooltip("the color to lerp to")]
		[ColorUsage(true, true)]
		public Color ToColor = Color.white;

		// Token: 0x04000780 RID: 1920
		[Header("Global Settings")]
		[Tooltip("the control mode (ping pong or random)")]
		public ShaderController.ControlModes ControlMode;

		// Token: 0x04000781 RID: 1921
		[Tooltip("whether or not the updated value should be added to the initial one")]
		public bool AddToInitialValue;

		// Token: 0x04000782 RID: 1922
		[Tooltip("whether or not to use unscaled time")]
		public bool UseUnscaledTime = true;

		// Token: 0x04000783 RID: 1923
		[Tooltip("whether or not you want to revert to the InitialValue after the control ends")]
		public bool RevertToInitialValueAfterEnd = true;

		// Token: 0x04000784 RID: 1924
		[Tooltip("if this is true, this component will use material property blocks instead of working on an instance of the material.")]
		[MMEnumCondition("TargetType", new int[] { 0 })]
		public bool UseMaterialPropertyBlocks;

		// Token: 0x04000785 RID: 1925
		[Tooltip("if using material property blocks on a sprite renderer, you'll want to make sure the sprite texture gets passed to the block when updating it. For that, you need to specify your sprite's material's shader's texture property name. If you're not working with a sprite renderer, you can safely ignore this.")]
		[MMCondition("UseMaterialPropertyBlocks", true)]
		public string SpriteRendererTextureProperty = "_MainTex";

		// Token: 0x04000786 RID: 1926
		[Tooltip("whether or not to perform extra safety checks (safer, more costly)")]
		public bool SafeMode;

		// Token: 0x04000787 RID: 1927
		[Header("Ping Pong")]
		[Tooltip("the curve to apply to the tween")]
		public MMTweenType Curve;

		// Token: 0x04000788 RID: 1928
		[Tooltip("the minimum value for the ping pong")]
		public float MinValue;

		// Token: 0x04000789 RID: 1929
		[Tooltip("the maximum value for the ping pong")]
		public float MaxValue = 5f;

		// Token: 0x0400078A RID: 1930
		[Tooltip("the duration of one ping (or pong)")]
		public float Duration = 1f;

		// Token: 0x0400078B RID: 1931
		[Tooltip("the duration of the pause between two ping (or pongs) (in seconds)")]
		public float PingPongPauseDuration = 1f;

		// Token: 0x0400078C RID: 1932
		[Header("Loop")]
		[Tooltip("the curve to apply to the tween")]
		public MMTweenType LoopCurve;

		// Token: 0x0400078D RID: 1933
		[Tooltip("the start value for the loop tween")]
		public float LoopStartValue;

		// Token: 0x0400078E RID: 1934
		[Tooltip("the end value for the loop tween")]
		public float LoopEndValue = 5f;

		// Token: 0x0400078F RID: 1935
		[Tooltip("the duration of one loop")]
		public float LoopDuration = 1f;

		// Token: 0x04000790 RID: 1936
		[Tooltip("the duration of the pause between two loops (in seconds)")]
		public float LoopPauseDuration = 1f;

		// Token: 0x04000791 RID: 1937
		[Header("Driven")]
		[Tooltip("the value that will be applied to the controlled float in driven mode")]
		public float DrivenLevel;

		// Token: 0x04000792 RID: 1938
		[Header("Random")]
		[Tooltip("the noise amplitude")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Amplitude = new Vector2(0f, 5f);

		// Token: 0x04000793 RID: 1939
		[Tooltip("the noise frequency")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Frequency = new Vector2(1f, 1f);

		// Token: 0x04000794 RID: 1940
		[Tooltip("the noise shift")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Shift = new Vector2(0f, 1f);

		// Token: 0x04000795 RID: 1941
		[Tooltip("if this is true, will let you remap the noise value (without amplitude) to the bounds you've specified")]
		public bool RemapNoiseValues;

		// Token: 0x04000796 RID: 1942
		[Tooltip("the value to which to remap the random's zero bound")]
		[MMCondition("RemapNoiseValues", true)]
		public float RemapNoiseZero;

		// Token: 0x04000797 RID: 1943
		[Tooltip("the value to which to remap the random's one bound")]
		[MMCondition("RemapNoiseValues", true)]
		public float RemapNoiseOne = 1f;

		// Token: 0x04000798 RID: 1944
		[Header("OneTime")]
		[Tooltip("the duration of the One Time shake")]
		public float OneTimeDuration = 1f;

		// Token: 0x04000799 RID: 1945
		[Tooltip("the amplitude of the One Time shake (this will be multiplied by the curve's height)")]
		public float OneTimeAmplitude = 1f;

		// Token: 0x0400079A RID: 1946
		[Tooltip("the low value to remap the normalized curve value to")]
		public float OneTimeRemapMin;

		// Token: 0x0400079B RID: 1947
		[Tooltip("the high value to remap the normalized curve value to")]
		public float OneTimeRemapMax = 1f;

		// Token: 0x0400079C RID: 1948
		[Tooltip("the curve to apply to the one time shake")]
		public AnimationCurve OneTimeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400079D RID: 1949
		[MMInspectorButton("OneTime")]
		[Tooltip("a test button for the one time shake")]
		public bool OneTimeButton;

		// Token: 0x0400079E RID: 1950
		[Tooltip("whether or not this controller should go back to sleep after a OneTime")]
		public bool DisableAfterOneTime;

		// Token: 0x0400079F RID: 1951
		[Tooltip("whether or not this controller should go back to sleep after a OneTime")]
		public bool DisableGameObjectAfterOneTime;

		// Token: 0x040007A0 RID: 1952
		[Tooltip("whether or not to initialize the initial value to the current value on a OneTime play")]
		public bool GetInitialValueOnOneTime;

		// Token: 0x040007A1 RID: 1953
		[Header("AudioAnalyzer")]
		[Tooltip("the bound audio analyzer used to drive this controller")]
		public MMAudioAnalyzer AudioAnalyzer;

		// Token: 0x040007A2 RID: 1954
		[Tooltip("the ID of the selected beat on the analyzer")]
		public int BeatID;

		// Token: 0x040007A3 RID: 1955
		[Tooltip("the multiplier to apply to the value out of the analyzer")]
		public float AudioAnalyzerMultiplier = 1f;

		// Token: 0x040007A4 RID: 1956
		[Tooltip("the offset to apply to the value out of the analyzer")]
		public float AudioAnalyzerOffset;

		// Token: 0x040007A5 RID: 1957
		[Tooltip("the speed at which to lerp the value")]
		public float AudioAnalyzerLerp = 60f;

		// Token: 0x040007A6 RID: 1958
		[Header("ToDestination")]
		[Tooltip("the value to go to when in ToDestination mode")]
		public float ToDestinationValue = 1f;

		// Token: 0x040007A7 RID: 1959
		[Tooltip("the duration of the ToDestination tween")]
		public float ToDestinationDuration = 1f;

		// Token: 0x040007A8 RID: 1960
		[Tooltip("the curve to use to tween to the ToDestination value")]
		public AnimationCurve ToDestinationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 0.6f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040007A9 RID: 1961
		[Tooltip("a test button for the one time shake")]
		[MMInspectorButton("ToDestination")]
		public bool ToDestinationButton;

		// Token: 0x040007AA RID: 1962
		[Tooltip("whether or not this controller should go back to sleep after a OneTime")]
		public bool DisableAfterToDestination;

		// Token: 0x040007AB RID: 1963
		[Header("Debug")]
		[Tooltip("the initial value of the controlled float")]
		[MMReadOnly]
		public float InitialValue;

		// Token: 0x040007AC RID: 1964
		[Tooltip("the current value of the controlled float")]
		[MMReadOnly]
		public float CurrentValue;

		// Token: 0x040007AD RID: 1965
		[Tooltip("the current value of the controlled float, normalized")]
		[MMReadOnly]
		public float CurrentValueNormalized;

		// Token: 0x040007AE RID: 1966
		[Tooltip("the current value of the controlled float")]
		[MMReadOnly]
		public Color InitialColor;

		// Token: 0x040007AF RID: 1967
		[Tooltip("the ID of the property")]
		[MMReadOnly]
		public int PropertyID;

		// Token: 0x040007B0 RID: 1968
		[Tooltip("whether or not the property got found")]
		[MMReadOnly]
		public bool PropertyFound;

		// Token: 0x040007B1 RID: 1969
		[Tooltip("the target material")]
		[MMReadOnly]
		public Material TargetMaterial;

		// Token: 0x040007B2 RID: 1970
		[HideInInspector]
		public float PingPong;

		// Token: 0x040007B3 RID: 1971
		[HideInInspector]
		public float LoopTime;

		// Token: 0x040007B4 RID: 1972
		protected float _randomAmplitude;

		// Token: 0x040007B5 RID: 1973
		protected float _randomFrequency;

		// Token: 0x040007B6 RID: 1974
		protected float _randomShift;

		// Token: 0x040007B7 RID: 1975
		protected float _elapsedTime;

		// Token: 0x040007B8 RID: 1976
		protected bool _shaking;

		// Token: 0x040007B9 RID: 1977
		protected float _startedTimestamp;

		// Token: 0x040007BA RID: 1978
		protected float _remappedTimeSinceStart;

		// Token: 0x040007BB RID: 1979
		protected Color _currentColor;

		// Token: 0x040007BC RID: 1980
		protected Vector4 _vectorValue;

		// Token: 0x040007BD RID: 1981
		protected float _pingPongDirection = 1f;

		// Token: 0x040007BE RID: 1982
		protected float _lastPingPongPauseAt;

		// Token: 0x040007BF RID: 1983
		protected float _lastLoopPauseAt;

		// Token: 0x040007C0 RID: 1984
		protected float _initialValue;

		// Token: 0x040007C1 RID: 1985
		protected Color _fromColorStorage;

		// Token: 0x040007C2 RID: 1986
		protected bool _activeLastFrame;

		// Token: 0x040007C3 RID: 1987
		protected MaterialPropertyBlock _propertyBlock;

		// Token: 0x040007C4 RID: 1988
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x040007C5 RID: 1989
		protected Texture2D _spriteRendererTexture;

		// Token: 0x040007C6 RID: 1990
		protected bool SpriteRendererIsNull;

		// Token: 0x02000437 RID: 1079
		public enum TargetTypes
		{
			// Token: 0x04001FA9 RID: 8105
			Renderer,
			// Token: 0x04001FAA RID: 8106
			Image,
			// Token: 0x04001FAB RID: 8107
			RawImage,
			// Token: 0x04001FAC RID: 8108
			Text
		}

		// Token: 0x02000438 RID: 1080
		public enum PropertyTypes
		{
			// Token: 0x04001FAE RID: 8110
			Bool,
			// Token: 0x04001FAF RID: 8111
			Float,
			// Token: 0x04001FB0 RID: 8112
			Int,
			// Token: 0x04001FB1 RID: 8113
			Vector,
			// Token: 0x04001FB2 RID: 8114
			Keyword,
			// Token: 0x04001FB3 RID: 8115
			Color
		}

		// Token: 0x02000439 RID: 1081
		public enum ControlModes
		{
			// Token: 0x04001FB5 RID: 8117
			PingPong,
			// Token: 0x04001FB6 RID: 8118
			Random,
			// Token: 0x04001FB7 RID: 8119
			OneTime,
			// Token: 0x04001FB8 RID: 8120
			AudioAnalyzer,
			// Token: 0x04001FB9 RID: 8121
			ToDestination,
			// Token: 0x04001FBA RID: 8122
			Driven,
			// Token: 0x04001FBB RID: 8123
			Loop
		}

		// Token: 0x0200043A RID: 1082
		public enum ColorModes
		{
			// Token: 0x04001FBD RID: 8125
			TwoColors,
			// Token: 0x04001FBE RID: 8126
			ColorRamp
		}
	}
}
