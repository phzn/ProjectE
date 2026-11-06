using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E7 RID: 487
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you flicker the color of a specified renderer (sprite, mesh, etc) for a certain duration, at the specified octave, and with the specified color. Useful when a character gets hit, for example (but so much more!).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Flicker")]
	public class MMF_Flicker : MMF_Feedback
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000362FB File Offset: 0x000344FB
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000362FE File Offset: 0x000344FE
		protected override void AutomateTargetAcquisition()
		{
			this.BoundRenderer = this.FindAutomatedTarget<Renderer>();
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0003630C File Offset: 0x0003450C
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x0003631A File Offset: 0x0003451A
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.FlickerDuration);
			}
			set
			{
				this.FlickerDuration = value;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00036324 File Offset: 0x00034524
		protected override void CustomInitialization(MMF_Player owner)
		{
			if (this.MaterialIndexes.Length == 0)
			{
				this.MaterialIndexes = new int[1];
				this.MaterialIndexes[0] = 0;
			}
			this._coroutines = new Coroutine[this.MaterialIndexes.Length];
			this._initialFlickerColors = new Color[this.MaterialIndexes.Length];
			this._extraCoroutines = new List<Coroutine[]>();
			this._extraInitialFlickerColors = new List<Color[]>();
			foreach (Renderer renderer in this.ExtraBoundRenderers)
			{
				this._extraCoroutines.Add(new Coroutine[this.MaterialIndexes.Length]);
				this._extraInitialFlickerColors.Add(new Color[this.MaterialIndexes.Length]);
			}
			this._propertyIDs = new int[this.MaterialIndexes.Length];
			this._propertiesFound = new bool[this.MaterialIndexes.Length];
			this._propertyBlock = new MaterialPropertyBlock();
			this.AcquireRenderers(owner);
			this.StoreSpriteRendererTexture();
			for (int i = 0; i < this.MaterialIndexes.Length; i++)
			{
				this._propertiesFound[i] = false;
				int num = this.MaterialIndexes[i];
				if (this.Active && this.BoundRenderer != null)
				{
					if (this.Mode == MMF_Flicker.Modes.Color)
					{
						this._propertiesFound[i] = (this.UseMaterialPropertyBlocks ? this.BoundRenderer.sharedMaterials[num].HasProperty("_Color") : this.BoundRenderer.materials[num].HasProperty("_Color"));
						if (!this._propertiesFound[i])
						{
							goto IL_0361;
						}
						this._initialFlickerColors[i] = (this.UseMaterialPropertyBlocks ? this.BoundRenderer.sharedMaterials[num].color : this.BoundRenderer.materials[num].color);
						using (List<Renderer>.Enumerator enumerator = this.ExtraBoundRenderers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Renderer renderer2 = enumerator.Current;
								this._extraInitialFlickerColors[this.ExtraBoundRenderers.IndexOf(renderer2)][i] = (this.UseMaterialPropertyBlocks ? renderer2.sharedMaterials[num].color : renderer2.materials[num].color);
							}
							goto IL_0361;
						}
					}
					this._propertiesFound[i] = (this.UseMaterialPropertyBlocks ? this.BoundRenderer.sharedMaterials[num].HasProperty(this.PropertyName) : this.BoundRenderer.materials[num].HasProperty(this.PropertyName));
					if (this._propertiesFound[i])
					{
						this._propertyIDs[i] = Shader.PropertyToID(this.PropertyName);
						this._initialFlickerColors[i] = (this.UseMaterialPropertyBlocks ? this.BoundRenderer.sharedMaterials[num].GetColor(this._propertyIDs[i]) : this.BoundRenderer.materials[num].GetColor(this._propertyIDs[i]));
						foreach (Renderer renderer3 in this.ExtraBoundRenderers)
						{
							this._extraInitialFlickerColors[this.ExtraBoundRenderers.IndexOf(renderer3)][i] = (this.UseMaterialPropertyBlocks ? renderer3.sharedMaterials[num].GetColor(this._propertyIDs[i]) : renderer3.materials[num].GetColor(this._propertyIDs[i]));
						}
					}
				}
				IL_0361:;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000366CC File Offset: 0x000348CC
		protected virtual void AcquireRenderers(MMF_Player owner)
		{
			if (this.Active && this.BoundRenderer == null && owner != null)
			{
				if (this.Owner.gameObject.MMFGetComponentNoAlloc<Renderer>() != null)
				{
					this.BoundRenderer = owner.GetComponent<Renderer>();
				}
				if (this.BoundRenderer == null)
				{
					this.BoundRenderer = owner.GetComponentInChildren<Renderer>();
				}
			}
			if (this.BoundRenderer == null)
			{
				Debug.LogWarning("[MMFeedbackFlicker] The flicker feedback on " + this.Owner.name + " doesn't have a bound renderer, it won't work. You need to specify a renderer to flicker in its inspector.");
			}
			this._spriteRenderer = this.BoundRenderer.GetComponent<SpriteRenderer>();
			this._spriteRenderers = new List<SpriteRenderer>();
			foreach (Renderer renderer in this.ExtraBoundRenderers)
			{
				if (renderer.GetComponent<SpriteRenderer>() != null)
				{
					this._spriteRenderers.Add(renderer.GetComponent<SpriteRenderer>());
				}
			}
			this._spriteRendererIsNull = this._spriteRenderer == null;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000367F0 File Offset: 0x000349F0
		protected virtual void StoreSpriteRendererTexture()
		{
			if (this._spriteRendererIsNull)
			{
				return;
			}
			this._spriteRendererTexture = this._spriteRenderer.sprite.texture;
			this._spriteRendererTextures = new List<Texture2D>();
			for (int i = 0; i < this.ExtraBoundRenderers.Count; i++)
			{
				this._spriteRendererTextures.Add(this._spriteRenderers[i].sprite.texture);
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00036860 File Offset: 0x00034A60
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Flicker.FeedbackTypeAuthorized || this.BoundRenderer == null)
			{
				return;
			}
			for (int i = 0; i < this.MaterialIndexes.Length; i++)
			{
				if (this._coroutines[i] != null)
				{
					this.Owner.StopCoroutine(this._coroutines[i]);
				}
				this._coroutines[i] = this.Owner.StartCoroutine(this.Flicker(this.BoundRenderer, i, this._initialFlickerColors[i], this.FlickerColor, this.FlickerPeriod, this.FeedbackDuration));
				for (int j = 0; j < this.ExtraBoundRenderers.Count; j++)
				{
					this._extraCoroutines[j][i] = this.Owner.StartCoroutine(this.Flicker(this.ExtraBoundRenderers[j], i, this._extraInitialFlickerColors[j][i], this.FlickerColor, this.FlickerPeriod, this.FeedbackDuration));
				}
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00036968 File Offset: 0x00034B68
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.InCooldown)
			{
				return;
			}
			if (this.Active && MMF_Flicker.FeedbackTypeAuthorized && this.BoundRenderer != null)
			{
				for (int i = 0; i < this.MaterialIndexes.Length; i++)
				{
					this.SetColor(this.BoundRenderer, i, this._initialFlickerColors[i]);
				}
			}
			foreach (Renderer renderer in this.ExtraBoundRenderers)
			{
				for (int j = 0; j < this.MaterialIndexes.Length; j++)
				{
					this.SetColor(renderer, j, this._extraInitialFlickerColors[this.ExtraBoundRenderers.IndexOf(renderer)][j]);
				}
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00036A44 File Offset: 0x00034C44
		protected virtual void SetStoredSpriteRendererTexture(Renderer renderer, MaterialPropertyBlock block)
		{
			if (this._spriteRendererIsNull)
			{
				return;
			}
			if (renderer == this.BoundRenderer)
			{
				block.SetTexture(this.SpriteRendererTextureProperty, this._spriteRendererTexture);
				return;
			}
			block.SetTexture(this.SpriteRendererTextureProperty, this._spriteRendererTextures[this.ExtraBoundRenderers.IndexOf(renderer)]);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00036A9E File Offset: 0x00034C9E
		public virtual IEnumerator Flicker(Renderer renderer, int materialIndex, Color initialColor, Color flickerColor, float flickerSpeed, float flickerDuration)
		{
			if (renderer == null)
			{
				yield break;
			}
			if (!this._propertiesFound[materialIndex])
			{
				yield break;
			}
			if (initialColor == flickerColor)
			{
				yield break;
			}
			float flickerStop = this.FeedbackTime + flickerDuration;
			this.IsPlaying = true;
			this.StoreSpriteRendererTexture();
			while (this.FeedbackTime < flickerStop)
			{
				this.SetColor(renderer, materialIndex, flickerColor);
				yield return this.WaitFor(flickerSpeed);
				this.SetColor(renderer, materialIndex, initialColor);
				yield return this.WaitFor(flickerSpeed);
			}
			this.SetColor(renderer, materialIndex, initialColor);
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00036ADC File Offset: 0x00034CDC
		protected virtual void SetColor(Renderer renderer, int materialIndex, Color color)
		{
			if (!this._propertiesFound[materialIndex])
			{
				return;
			}
			if (this.Mode == MMF_Flicker.Modes.Color)
			{
				if (this.UseMaterialPropertyBlocks)
				{
					renderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndexes[materialIndex]);
					this._propertyBlock.SetColor("_Color", color);
					this.SetStoredSpriteRendererTexture(renderer, this._propertyBlock);
					renderer.SetPropertyBlock(this._propertyBlock, this.MaterialIndexes[materialIndex]);
					return;
				}
				renderer.materials[this.MaterialIndexes[materialIndex]].color = color;
				return;
			}
			else
			{
				if (this.UseMaterialPropertyBlocks)
				{
					renderer.GetPropertyBlock(this._propertyBlock, this.MaterialIndexes[materialIndex]);
					this._propertyBlock.SetColor(this._propertyIDs[materialIndex], color);
					this.SetStoredSpriteRendererTexture(renderer, this._propertyBlock);
					renderer.SetPropertyBlock(this._propertyBlock, this.MaterialIndexes[materialIndex]);
					return;
				}
				renderer.materials[this.MaterialIndexes[materialIndex]].SetColor(this._propertyIDs[materialIndex], color);
				return;
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00036BD0 File Offset: 0x00034DD0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Flicker.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			for (int i = 0; i < this._coroutines.Length; i++)
			{
				if (this._coroutines[i] != null)
				{
					this.Owner.StopCoroutine(this._coroutines[i]);
				}
				this._coroutines[i] = null;
			}
			foreach (Renderer renderer in this.ExtraBoundRenderers)
			{
				for (int j = 0; j < this.MaterialIndexes.Length; j++)
				{
					if (this._extraCoroutines[this.ExtraBoundRenderers.IndexOf(renderer)][j] != null)
					{
						this.Owner.StopCoroutine(this._extraCoroutines[this.ExtraBoundRenderers.IndexOf(renderer)][j]);
					}
					this._extraCoroutines[this.ExtraBoundRenderers.IndexOf(renderer)][j] = null;
				}
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00036CE0 File Offset: 0x00034EE0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Flicker.FeedbackTypeAuthorized)
			{
				return;
			}
			this.CustomReset();
		}

		// Token: 0x04000E38 RID: 3640
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E39 RID: 3641
		[MMFInspectorGroup("Flicker", true, 61, true, false)]
		[Tooltip("the renderer to flicker when played")]
		public Renderer BoundRenderer;

		// Token: 0x04000E3A RID: 3642
		[Tooltip("more renderers to flicker when played")]
		public List<Renderer> ExtraBoundRenderers;

		// Token: 0x04000E3B RID: 3643
		[Tooltip("the selected mode to flicker the renderer")]
		public MMF_Flicker.Modes Mode;

		// Token: 0x04000E3C RID: 3644
		[MMFEnumCondition("Mode", new int[] { 1 })]
		[Tooltip("the name of the property to target")]
		public string PropertyName = "_Tint";

		// Token: 0x04000E3D RID: 3645
		[Tooltip("the duration of the flicker when getting damage")]
		public float FlickerDuration = 0.2f;

		// Token: 0x04000E3E RID: 3646
		[Tooltip("the duration of the period for the flicker")]
		[FormerlySerializedAs("FlickerOctave")]
		public float FlickerPeriod = 0.04f;

		// Token: 0x04000E3F RID: 3647
		[Tooltip("the color we should flicker the sprite to")]
		[ColorUsage(true, true)]
		public Color FlickerColor = new Color32(byte.MaxValue, 20, 20, byte.MaxValue);

		// Token: 0x04000E40 RID: 3648
		[Tooltip("the list of material indexes we want to flicker on the target renderer. If left empty, will only target the material at index 0")]
		public int[] MaterialIndexes;

		// Token: 0x04000E41 RID: 3649
		[Tooltip("if this is true, this component will use material property blocks instead of working on an instance of the material.")]
		public bool UseMaterialPropertyBlocks;

		// Token: 0x04000E42 RID: 3650
		[Tooltip("if using material property blocks on a sprite renderer, you'll want to make sure the sprite texture gets passed to the block when updating it. For that, you need to specify your sprite's material's shader's texture property name. If you're not working with a sprite renderer, you can safely ignore this.")]
		[MMCondition("UseMaterialPropertyBlocks", true)]
		public string SpriteRendererTextureProperty = "_MainTex";

		// Token: 0x04000E43 RID: 3651
		protected const string _colorPropertyName = "_Color";

		// Token: 0x04000E44 RID: 3652
		protected int[] _propertyIDs;

		// Token: 0x04000E45 RID: 3653
		protected bool[] _propertiesFound;

		// Token: 0x04000E46 RID: 3654
		protected bool _spriteRendererIsNull;

		// Token: 0x04000E47 RID: 3655
		protected Coroutine[] _coroutines;

		// Token: 0x04000E48 RID: 3656
		protected List<Coroutine[]> _extraCoroutines;

		// Token: 0x04000E49 RID: 3657
		protected Color[] _initialFlickerColors;

		// Token: 0x04000E4A RID: 3658
		protected List<Color[]> _extraInitialFlickerColors;

		// Token: 0x04000E4B RID: 3659
		protected MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000E4C RID: 3660
		protected List<MaterialPropertyBlock> _extraPropertyBlocks;

		// Token: 0x04000E4D RID: 3661
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x04000E4E RID: 3662
		protected List<SpriteRenderer> _spriteRenderers;

		// Token: 0x04000E4F RID: 3663
		protected Texture2D _spriteRendererTexture;

		// Token: 0x04000E50 RID: 3664
		protected List<Texture2D> _spriteRendererTextures;

		// Token: 0x020004B4 RID: 1204
		public enum Modes
		{
			// Token: 0x040021EC RID: 8684
			Color,
			// Token: 0x040021ED RID: 8685
			PropertyName
		}
	}
}
