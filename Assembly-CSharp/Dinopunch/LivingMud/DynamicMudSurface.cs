using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200074C RID: 1868
	public class DynamicMudSurface : MonoBehaviour
	{
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003066 RID: 12390 RVA: 0x00111F1E File Offset: 0x0011011E
		public MudType Thickness
		{
			get
			{
				return this.thickness;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x00111F26 File Offset: 0x00110126
		public float BoundingBoxSizeInUnits
		{
			get
			{
				return this.boundingBoxSizeInUnits;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x00111F2E File Offset: 0x0011012E
		public Material Material
		{
			get
			{
				return this.material;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x00111F36 File Offset: 0x00110136
		public Shader Shader
		{
			get
			{
				return this.shader;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600306A RID: 12394 RVA: 0x00111F3E File Offset: 0x0011013E
		public CustomRenderTexture StampRenderTexture
		{
			get
			{
				return this.stampRenderTexture;
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x00111F48 File Offset: 0x00110148
		public Texture2D GetColorMap()
		{
			if (this.material == null)
			{
				return null;
			}
			if (this.material.HasProperty("_Albedo"))
			{
				return this.material.GetTexture("_Albedo") as Texture2D;
			}
			if (this.material.HasProperty("_BaseMap"))
			{
				return this.material.GetTexture("_BaseMap") as Texture2D;
			}
			return null;
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x00111FB8 File Offset: 0x001101B8
		public Color GetColor()
		{
			if (this.material == null)
			{
				return Color.white;
			}
			if (this.material.HasProperty("_Tint"))
			{
				return this.material.GetColor("_Tint");
			}
			if (this.material.HasProperty("_BaseColor"))
			{
				return this.material.GetColor("_BaseColor");
			}
			return Color.white;
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x00112024 File Offset: 0x00110224
		private void OnValidate()
		{
			this.SetReferences();
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x00112024 File Offset: 0x00110224
		private void Awake()
		{
			this.SetReferences();
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x0011202C File Offset: 0x0011022C
		public void ClearStampTexture()
		{
			if (this.stampRenderTexture == null)
			{
				return;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.stampRenderTexture;
			Graphics.ClearRandomWriteTargets();
			Graphics.SetRenderTarget(this.stampRenderTexture);
			GL.Clear(true, true, Color.clear);
			RenderTexture renderTexture = (RenderTexture.active = this.stampRenderTexture.GetDoubleBufferRenderTexture());
			Graphics.ClearRandomWriteTargets();
			Graphics.SetRenderTarget(renderTexture);
			GL.Clear(true, true, Color.clear);
			RenderTexture.active = active;
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x001120A0 File Offset: 0x001102A0
		private void SetReferences()
		{
			Renderer renderer;
			if (!base.TryGetComponent<Renderer>(out renderer))
			{
				Debug.LogError("[DynamicMudSurface] '" + base.name + "' has no Renderer component", this);
				return;
			}
			this.material = renderer.sharedMaterial;
			if (this.material == null)
			{
				Debug.LogError("[DynamicMudSurface] '" + base.name + "' renderer has no material assigned", this);
				return;
			}
			this.shader = this.material.shader;
			if (this.material.HasProperty("_Stamp_Render_Texture_1"))
			{
				this.stampRenderTexture = this.material.GetTexture("_Stamp_Render_Texture_1") as CustomRenderTexture;
			}
			else
			{
				this.stampRenderTexture = null;
			}
			if (this.OverrideMaterialStampTexture && this.customRenderTexture == null)
			{
				if (this.stampRenderTexture != null)
				{
					this.customRenderTexture = this.stampRenderTexture;
					Debug.LogWarning("[DynamicMudSurface] '" + DynamicMudSurface.GetHierarchyPath(base.transform) + "': Override is ON but Custom Render Texture field was empty. Recovered reference from material.", this);
				}
				else
				{
					Debug.LogError("[DynamicMudSurface] '" + DynamicMudSurface.GetHierarchyPath(base.transform) + "': Override Material Stamp Texture is ON but Custom Render Texture is not assigned", this);
				}
			}
			else if (this.OverrideMaterialStampTexture)
			{
				this.stampRenderTexture = this.customRenderTexture;
				if (this.material.HasProperty("_Stamp_Render_Texture_1"))
				{
					this.material.SetTexture("_Stamp_Render_Texture_1", this.stampRenderTexture);
				}
				else if (DynamicMudSurface.s_warnedMaterials.Add(this.material.GetInstanceID()))
				{
					Debug.LogWarning("[DynamicMudSurface] material '" + this.material.name + "' does not expose _Stamp_Render_Texture_1. Stamps may not be visible on this shader.", this);
				}
			}
			if (!(this.stampRenderTexture == null))
			{
				return;
			}
			if (this.OverrideMaterialStampTexture)
			{
				Debug.LogError("[DynamicMudSurface] '" + DynamicMudSurface.GetHierarchyPath(base.transform) + "': No Render Texture Assigned for Dynamic Mud Stamps", this);
				return;
			}
			Debug.LogWarning(string.Concat(new string[]
			{
				"[DynamicMudSurface] '",
				DynamicMudSurface.GetHierarchyPath(base.transform),
				"': No stamp render texture found on material '",
				this.material.name,
				"'. Assign one via shader property or enable Override Material Stamp Texture."
			}), this);
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x001122BC File Offset: 0x001104BC
		private static string GetHierarchyPath(Transform t)
		{
			if (t == null)
			{
				return "<null>";
			}
			string text = t.name;
			while (t.parent != null)
			{
				t = t.parent;
				text = t.name + "/" + text;
			}
			return text;
		}

		// Token: 0x040030FA RID: 12538
		private static readonly HashSet<int> s_warnedMaterials = new HashSet<int>();

		// Token: 0x040030FB RID: 12539
		[Tooltip("What type of mud is this surface")]
		[SerializeField]
		private MudType thickness;

		// Token: 0x040030FC RID: 12540
		[Tooltip("Maximum size in units of the surface")]
		[SerializeField]
		private float boundingBoxSizeInUnits = 10f;

		// Token: 0x040030FD RID: 12541
		[Tooltip("Use the CustomRenderTexture assigned here")]
		[SerializeField]
		private bool OverrideMaterialStampTexture;

		// Token: 0x040030FE RID: 12542
		[Tooltip("Reference to CustomRenderTexture stamps use on this surface")]
		[SerializeField]
		private CustomRenderTexture customRenderTexture;

		// Token: 0x040030FF RID: 12543
		private Material material;

		// Token: 0x04003100 RID: 12544
		private Shader shader;

		// Token: 0x04003101 RID: 12545
		private CustomRenderTexture stampRenderTexture;
	}
}
