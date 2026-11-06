using System;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200110F RID: 4367
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public sealed class CollarIdCard : MonoBehaviour
	{
		// Token: 0x06008490 RID: 33936 RVA: 0x003603B0 File Offset: 0x0035E5B0
		private void OnEnable()
		{
			CollarIdPortraitService.Register(this);
		}

		// Token: 0x06008491 RID: 33937 RVA: 0x003603B8 File Offset: 0x0035E5B8
		private void OnDisable()
		{
			CollarIdPortraitService.Unregister(this);
			this.RestoreBinding();
			this.ReleaseComposite();
		}

		// Token: 0x06008492 RID: 33938 RVA: 0x003603CC File Offset: 0x0035E5CC
		public void ApplyPortrait(Texture portrait)
		{
			if (portrait == null || this.cardTemplate == null || !this.HasTarget())
			{
				this.RestoreBinding();
				return;
			}
			if (portrait == this._composite)
			{
				return;
			}
			RectInt rectInt = this.portraitPixelsTopLeft;
			if (rectInt.width <= 0 || rectInt.height <= 0 || rectInt.xMin < 0 || rectInt.yMin < 0 || rectInt.xMax > this.cardTemplate.width || rectInt.yMax > this.cardTemplate.height)
			{
				this.RestoreBinding();
				return;
			}
			this.EnsureComposite();
			if (this._composite == null || !this._composite.IsCreated())
			{
				return;
			}
			this.CompositePortrait(portrait, rectInt);
			this.ApplyBinding();
		}

		// Token: 0x06008493 RID: 33939 RVA: 0x0036049C File Offset: 0x0035E69C
		private bool HasTarget()
		{
			return this.cardRenderer != null && this.materialIndex >= 0 && this.materialIndex < this.cardRenderer.sharedMaterials.Length && this.cardRenderer.sharedMaterials[this.materialIndex] != null;
		}

		// Token: 0x06008494 RID: 33940 RVA: 0x003604F0 File Offset: 0x0035E6F0
		private void EnsureComposite()
		{
			if (this._composite != null && (this._composite.width != this.cardTemplate.width || this._composite.height != this.cardTemplate.height))
			{
				this.RestoreBinding();
				this.ReleaseComposite();
			}
			if (this._composite == null)
			{
				this._composite = new RenderTexture(this.cardTemplate.width, this.cardTemplate.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
				{
					name = "Collar ID card (instance)",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					useMipMap = false,
					autoGenerateMips = false
				};
			}
			if (!this._composite.IsCreated())
			{
				this._composite.Create();
			}
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x003605C4 File Offset: 0x0035E7C4
		private void CompositePortrait(Texture portrait, RectInt pixels)
		{
			RenderTexture active = RenderTexture.active;
			bool sRGBWrite = GL.sRGBWrite;
			bool flag = false;
			try
			{
				GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
				Graphics.Blit(this.cardTemplate, this._composite);
				RenderTexture.active = this._composite;
				GL.PushMatrix();
				flag = true;
				GL.LoadPixelMatrix(0f, (float)this._composite.width, (float)this._composite.height, 0f);
				Rect rect = new Rect((float)pixels.x, (float)pixels.y, (float)pixels.width, (float)pixels.height);
				Color linear = this.portraitBackground;
				linear.a = 1f;
				if (QualitySettings.activeColorSpace == ColorSpace.Linear)
				{
					linear = linear.linear;
				}
				Graphics.DrawTexture(rect, Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, linear * 0.5f);
				Graphics.DrawTexture(rect, portrait);
			}
			finally
			{
				if (flag)
				{
					GL.PopMatrix();
				}
				RenderTexture.active = active;
				GL.sRGBWrite = sRGBWrite;
			}
		}

		// Token: 0x06008496 RID: 33942 RVA: 0x003606E0 File Offset: 0x0035E8E0
		private void ApplyBinding()
		{
			if (this._boundRenderer != null && (this._boundRenderer != this.cardRenderer || this._boundIndex != this.materialIndex))
			{
				this.RestoreBinding();
			}
			if (this._properties == null)
			{
				this._properties = new MaterialPropertyBlock();
			}
			this.cardRenderer.GetPropertyBlock(this._properties, this.materialIndex);
			if (this._boundRenderer == null)
			{
				this._boundRenderer = this.cardRenderer;
				this._boundIndex = this.materialIndex;
				for (int i = 0; i < CollarIdCard.TextureProperties.Length; i++)
				{
					this._hadTextureOverride[i] = this._properties.HasTexture(CollarIdCard.TextureProperties[i]);
					this._previousTextures[i] = (this._hadTextureOverride[i] ? this._properties.GetTexture(CollarIdCard.TextureProperties[i]) : null);
				}
			}
			foreach (int num in CollarIdCard.TextureProperties)
			{
				this._properties.SetTexture(num, this._composite);
			}
			this.cardRenderer.SetPropertyBlock(this._properties, this.materialIndex);
		}

		// Token: 0x06008497 RID: 33943 RVA: 0x00360808 File Offset: 0x0035EA08
		private void RestoreBinding()
		{
			if (this._boundRenderer != null && this._boundIndex >= 0 && this._boundIndex < this._boundRenderer.sharedMaterials.Length)
			{
				Material material = this._boundRenderer.sharedMaterials[this._boundIndex];
				if (this._properties == null)
				{
					this._properties = new MaterialPropertyBlock();
				}
				this._boundRenderer.GetPropertyBlock(this._properties, this._boundIndex);
				for (int i = 0; i < CollarIdCard.TextureProperties.Length; i++)
				{
					int num = CollarIdCard.TextureProperties[i];
					if (!(this._properties.GetTexture(num) != this._composite))
					{
						Texture texture = (this._hadTextureOverride[i] ? this._previousTextures[i] : ((material != null && material.HasProperty(num)) ? material.GetTexture(num) : this.cardTemplate));
						this._properties.SetTexture(num, (texture != null) ? texture : this.cardTemplate);
					}
				}
				this._boundRenderer.SetPropertyBlock(this._properties, this._boundIndex);
			}
			this._boundRenderer = null;
			for (int j = 0; j < this._previousTextures.Length; j++)
			{
				this._previousTextures[j] = null;
				this._hadTextureOverride[j] = false;
			}
		}

		// Token: 0x06008498 RID: 33944 RVA: 0x003603B8 File Offset: 0x0035E5B8
		private void OnDestroy()
		{
			CollarIdPortraitService.Unregister(this);
			this.RestoreBinding();
			this.ReleaseComposite();
		}

		// Token: 0x06008499 RID: 33945 RVA: 0x0036095C File Offset: 0x0035EB5C
		private void ReleaseComposite()
		{
			if (this._composite == null)
			{
				return;
			}
			this._composite.Release();
			if (Application.IsPlaying(base.gameObject))
			{
				Object.Destroy(this._composite);
			}
			else
			{
				Object.DestroyImmediate(this._composite);
			}
			this._composite = null;
		}

		// Token: 0x04008756 RID: 34646
		[SerializeField]
		private Renderer cardRenderer;

		// Token: 0x04008757 RID: 34647
		[SerializeField]
		[Min(0f)]
		private int materialIndex;

		// Token: 0x04008758 RID: 34648
		[SerializeField]
		private Texture2D cardTemplate;

		// Token: 0x04008759 RID: 34649
		[Tooltip("Photo interior in template pixels, measured from its top-left corner.")]
		[SerializeField]
		private RectInt portraitPixelsTopLeft = new RectInt(166, 766, 750, 848);

		// Token: 0x0400875A RID: 34650
		[SerializeField]
		private Color portraitBackground = new Color32(156, 133, 181, byte.MaxValue);

		// Token: 0x0400875B RID: 34651
		private static readonly int[] TextureProperties = new int[]
		{
			Shader.PropertyToID("_MainTex"),
			Shader.PropertyToID("_BaseMap"),
			Shader.PropertyToID("_BaseColorMap")
		};

		// Token: 0x0400875C RID: 34652
		private RenderTexture _composite;

		// Token: 0x0400875D RID: 34653
		private MaterialPropertyBlock _properties;

		// Token: 0x0400875E RID: 34654
		private Renderer _boundRenderer;

		// Token: 0x0400875F RID: 34655
		private int _boundIndex;

		// Token: 0x04008760 RID: 34656
		private readonly Texture[] _previousTextures = new Texture[3];

		// Token: 0x04008761 RID: 34657
		private readonly bool[] _hadTextureOverride = new bool[3];
	}
}
