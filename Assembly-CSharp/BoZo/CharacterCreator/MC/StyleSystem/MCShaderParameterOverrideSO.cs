using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007DD RID: 2013
	[CreateAssetMenu(fileName = "MCShaderParamOverride", menuName = "Character Creator/MC/Shader Parameter Override")]
	public class MCShaderParameterOverrideSO : ScriptableObject
	{
		// Token: 0x06003552 RID: 13650 RVA: 0x0012FAE4 File Offset: 0x0012DCE4
		public void ApplyToMaterial(Material material)
		{
			if (material == null || this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = this.entries[i];
				if (shaderOverrideEntry != null)
				{
					MCShaderParameterOverrideSO.ApplyEntry(material, shaderOverrideEntry, shaderOverrideEntry.floatValue);
				}
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x0012FB38 File Offset: 0x0012DD38
		public void ApplyToMaterial(Material material, IReadOnlyDictionary<string, float> runtimeFloats)
		{
			if (material == null || this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = this.entries[i];
				if (shaderOverrideEntry != null)
				{
					float num = shaderOverrideEntry.floatValue;
					float num2;
					if (shaderOverrideEntry.type == MCShaderParameterOverrideSO.OverrideType.Float && shaderOverrideEntry.exposeAsSlider && runtimeFloats != null && runtimeFloats.TryGetValue(shaderOverrideEntry.propertyName, out num2))
					{
						num = Mathf.Clamp(num2, shaderOverrideEntry.sliderMin, shaderOverrideEntry.sliderMax);
					}
					MCShaderParameterOverrideSO.ApplyEntry(material, shaderOverrideEntry, num);
				}
			}
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x0012FBC4 File Offset: 0x0012DDC4
		public List<MCShaderParameterOverrideSO.ShaderOverrideEntry> GetExposedSliders()
		{
			List<MCShaderParameterOverrideSO.ShaderOverrideEntry> list = new List<MCShaderParameterOverrideSO.ShaderOverrideEntry>();
			if (this.entries == null)
			{
				return list;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = this.entries[i];
				if (shaderOverrideEntry != null && shaderOverrideEntry.type == MCShaderParameterOverrideSO.OverrideType.Float && shaderOverrideEntry.exposeAsSlider)
				{
					list.Add(shaderOverrideEntry);
				}
			}
			return list;
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x0012FC20 File Offset: 0x0012DE20
		public bool HasExposedSliders
		{
			get
			{
				if (this.entries == null)
				{
					return false;
				}
				for (int i = 0; i < this.entries.Count; i++)
				{
					if (this.entries[i] != null && this.entries[i].type == MCShaderParameterOverrideSO.OverrideType.Float && this.entries[i].exposeAsSlider)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0012FC84 File Offset: 0x0012DE84
		private static void ApplyEntry(Material material, MCShaderParameterOverrideSO.ShaderOverrideEntry entry, float resolvedFloat)
		{
			if (material == null || entry == null)
			{
				return;
			}
			switch (entry.type)
			{
			case MCShaderParameterOverrideSO.OverrideType.Float:
				if (!string.IsNullOrEmpty(entry.propertyName))
				{
					if (material.HasFloat(entry.propertyName))
					{
						material.SetFloat(entry.propertyName, resolvedFloat);
						return;
					}
					if (material.HasVector(entry.propertyName))
					{
						Vector4 vector = material.GetVector(entry.propertyName);
						vector.w = resolvedFloat;
						material.SetVector(entry.propertyName, vector);
						return;
					}
				}
				break;
			case MCShaderParameterOverrideSO.OverrideType.Texture:
				if (!string.IsNullOrEmpty(entry.propertyName) && entry.textureValue != null && material.HasTexture(entry.propertyName))
				{
					material.SetTexture(entry.propertyName, entry.textureValue);
					return;
				}
				break;
			case MCShaderParameterOverrideSO.OverrideType.Color:
				if (!string.IsNullOrEmpty(entry.propertyName) && material.HasColor(entry.propertyName))
				{
					material.SetColor(entry.propertyName, entry.colorValue);
					return;
				}
				break;
			case MCShaderParameterOverrideSO.OverrideType.Vector:
				if (!string.IsNullOrEmpty(entry.propertyName) && material.HasVector(entry.propertyName))
				{
					material.SetVector(entry.propertyName, entry.vectorValue);
					return;
				}
				break;
			case MCShaderParameterOverrideSO.OverrideType.Keyword:
				if (!string.IsNullOrEmpty(entry.keyword))
				{
					if (entry.keywordEnabled)
					{
						material.EnableKeyword(entry.keyword);
						return;
					}
					material.DisableKeyword(entry.keyword);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0400359B RID: 13723
		[Tooltip("List of shader property overrides applied to all materials on the part.")]
		public List<MCShaderParameterOverrideSO.ShaderOverrideEntry> entries = new List<MCShaderParameterOverrideSO.ShaderOverrideEntry>();

		// Token: 0x020007DE RID: 2014
		public enum OverrideType
		{
			// Token: 0x0400359D RID: 13725
			Float,
			// Token: 0x0400359E RID: 13726
			Texture,
			// Token: 0x0400359F RID: 13727
			Color,
			// Token: 0x040035A0 RID: 13728
			Vector,
			// Token: 0x040035A1 RID: 13729
			Keyword
		}

		// Token: 0x020007DF RID: 2015
		[Serializable]
		public class ShaderOverrideEntry
		{
			// Token: 0x040035A2 RID: 13730
			[Tooltip("Shader property name (e.g. '_FurLength', '_FurAlphaMask').")]
			public string propertyName;

			// Token: 0x040035A3 RID: 13731
			public MCShaderParameterOverrideSO.OverrideType type;

			// Token: 0x040035A4 RID: 13732
			[Header("Float")]
			[Tooltip("Default float value applied when this override is active.")]
			public float floatValue;

			// Token: 0x040035A5 RID: 13733
			[Tooltip("If true, this float is exposed to the player as a slider.")]
			public bool exposeAsSlider;

			// Token: 0x040035A6 RID: 13734
			[Tooltip("Slider display name shown in UI.")]
			public string sliderDisplayName;

			// Token: 0x040035A7 RID: 13735
			[Tooltip("Minimum slider value.")]
			public float sliderMin;

			// Token: 0x040035A8 RID: 13736
			[Tooltip("Maximum slider value.")]
			public float sliderMax = 1f;

			// Token: 0x040035A9 RID: 13737
			[Tooltip("Step increment for the slider. 0 = continuous.")]
			public float sliderStep;

			// Token: 0x040035AA RID: 13738
			[Header("Texture")]
			public Texture textureValue;

			// Token: 0x040035AB RID: 13739
			[Header("Color")]
			public Color colorValue = Color.white;

			// Token: 0x040035AC RID: 13740
			[Header("Vector")]
			public Vector4 vectorValue;

			// Token: 0x040035AD RID: 13741
			[Header("Keyword")]
			[Tooltip("Shader keyword to enable/disable (e.g. '_FUR'). Works with keyword toggle.")]
			public string keyword;

			// Token: 0x040035AE RID: 13742
			[Tooltip("Enable or disable the keyword.")]
			public bool keywordEnabled = true;
		}
	}
}
