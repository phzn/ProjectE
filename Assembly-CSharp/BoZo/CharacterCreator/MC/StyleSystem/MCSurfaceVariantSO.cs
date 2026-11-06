using System;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007FB RID: 2043
	[CreateAssetMenu(fileName = "MCSurfaceVariant", menuName = "Character Creator/MC/Style Surface Variant")]
	public class MCSurfaceVariantSO : ScriptableObject
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x00138D2C File Offset: 0x00136F2C
		public bool SupportsTextureOverrides
		{
			get
			{
				if (this.baseMap != null || this.normalMap != null || this.maskMap != null)
				{
					return true;
				}
				if (this.textureBindings == null)
				{
					return false;
				}
				for (int i = 0; i < this.textureBindings.Length; i++)
				{
					if (this.textureBindings[i] != null && this.textureBindings[i].HasAnyTexture)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003659 RID: 13913 RVA: 0x00138D9E File Offset: 0x00136F9E
		public bool SupportsColorCustomization
		{
			get
			{
				return this.surfaceType == MCSurfaceType.Naked && this.colorProfile != null;
			}
		}

		// Token: 0x04003686 RID: 13958
		public string variantId = "surface";

		// Token: 0x04003687 RID: 13959
		public string displayName = "Surface";

		// Token: 0x04003688 RID: 13960
		public Sprite previewIcon;

		// Token: 0x04003689 RID: 13961
		public MCSurfaceType surfaceType = MCSurfaceType.Baked;

		// Token: 0x0400368A RID: 13962
		[Header("Texture Overrides")]
		public Texture baseMap;

		// Token: 0x0400368B RID: 13963
		public Texture normalMap;

		// Token: 0x0400368C RID: 13964
		public Texture maskMap;

		// Token: 0x0400368D RID: 13965
		[Header("Target Shader Properties")]
		public string baseMapPropertyName = "_MainTex";

		// Token: 0x0400368E RID: 13966
		public string normalMapPropertyName = "_BumpMap";

		// Token: 0x0400368F RID: 13967
		public string maskMapPropertyName = "_MaskTex";

		// Token: 0x04003690 RID: 13968
		[Header("Per-Renderer Texture Overrides (Optional)")]
		[Tooltip("Advanced overrides for specific renderer/material slots. If empty, global fields above are used everywhere.")]
		public MCSurfaceVariantSO.TextureTargetBinding[] textureBindings;

		// Token: 0x04003691 RID: 13969
		[Header("Material Overrides (Optional)")]
		[Tooltip("Full material swaps applied BEFORE texture overrides. Use when the variant's look lives in material colors/floats/secondary layers/shader, which texture overrides cannot represent. Empty = no swap.")]
		public MCSurfaceVariantSO.MaterialTargetBinding[] materialBindings;

		// Token: 0x04003692 RID: 13970
		[Header("Color")]
		public MCColorProfileSO colorProfile;

		// Token: 0x04003693 RID: 13971
		[Header("Shader Parameter Overrides (Optional)")]
		[Tooltip("Optional shader property overrides applied after textures/colors. Use for Fur parameters, custom shader sections, etc.")]
		public MCShaderParameterOverrideSO shaderOverrides;

		// Token: 0x020007FC RID: 2044
		public enum RendererTargetType
		{
			// Token: 0x04003695 RID: 13973
			Any,
			// Token: 0x04003696 RID: 13974
			Head,
			// Token: 0x04003697 RID: 13975
			Body
		}

		// Token: 0x020007FD RID: 2045
		[Serializable]
		public class MaterialTargetBinding
		{
			// Token: 0x04003698 RID: 13976
			[Tooltip("Type-safe renderer target selector.")]
			public MCSurfaceVariantSO.RendererTargetType rendererTarget;

			// Token: 0x04003699 RID: 13977
			[Tooltip("Optional exact renderer name. Empty targets every renderer selected by rendererTarget. Use this when a composite outfit has different material-slot layouts per piece.")]
			public string rendererName;

			// Token: 0x0400369A RID: 13978
			[Tooltip("Target material slot. -1 = all material slots on matched renderer.")]
			[Min(-1f)]
			public int materialIndex = -1;

			// Token: 0x0400369B RID: 13979
			[Tooltip("Material that REPLACES the slot's material while this variant is selected. Full swap: use when the look lives in colors/floats/secondary layers/shader (e.g. the gold->black bikini), which texture overrides cannot represent.")]
			public Material material;
		}

		// Token: 0x020007FE RID: 2046
		[Serializable]
		public class TextureTargetBinding
		{
			// Token: 0x170009C7 RID: 2503
			// (get) Token: 0x0600365C RID: 13916 RVA: 0x00138E18 File Offset: 0x00137018
			public bool HasAnyTexture
			{
				get
				{
					return this.baseMap != null || this.normalMap != null || this.maskMap != null;
				}
			}

			// Token: 0x0400369C RID: 13980
			[Tooltip("Type-safe renderer target selector.")]
			public MCSurfaceVariantSO.RendererTargetType rendererTarget;

			// Token: 0x0400369D RID: 13981
			[Tooltip("Target material slot. -1 = all material slots on matched renderer.")]
			[Min(-1f)]
			public int materialIndex = -1;

			// Token: 0x0400369E RID: 13982
			[Header("Texture Overrides")]
			public Texture baseMap;

			// Token: 0x0400369F RID: 13983
			public Texture normalMap;

			// Token: 0x040036A0 RID: 13984
			public Texture maskMap;

			// Token: 0x040036A1 RID: 13985
			[Header("Optional Property Overrides")]
			public string baseMapPropertyName;

			// Token: 0x040036A2 RID: 13986
			public string normalMapPropertyName;

			// Token: 0x040036A3 RID: 13987
			public string maskMapPropertyName;
		}
	}
}
