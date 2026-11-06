using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E50 RID: 3664
	[DisallowMultipleComponent]
	public sealed class PhoneStableUiMaterials : MonoBehaviour
	{
		// Token: 0x06006D75 RID: 28021 RVA: 0x002BF4B0 File Offset: 0x002BD6B0
		public static PhoneStableUiMaterials Ensure(GameObject phoneRoot)
		{
			if (phoneRoot == null)
			{
				return null;
			}
			PhoneStableUiMaterials phoneStableUiMaterials = phoneRoot.GetComponent<PhoneStableUiMaterials>();
			if (phoneStableUiMaterials == null)
			{
				phoneStableUiMaterials = phoneRoot.AddComponent<PhoneStableUiMaterials>();
			}
			phoneStableUiMaterials.RefreshMaterials();
			return phoneStableUiMaterials;
		}

		// Token: 0x06006D76 RID: 28022 RVA: 0x002BF4E6 File Offset: 0x002BD6E6
		private void OnEnable()
		{
			Canvas.preWillRenderCanvases += this.RefreshMaterials;
			this.RefreshMaterials();
		}

		// Token: 0x06006D77 RID: 28023 RVA: 0x002BF500 File Offset: 0x002BD700
		public void RefreshMaterials()
		{
			if (this._shader == null)
			{
				Material material = Resources.Load<Material>("PhoneStableDefault");
				if (material != null)
				{
					this._shader = material.shader;
				}
			}
			if (this._tmpShader == null)
			{
				Material material2 = Resources.Load<Material>("PhoneStableTmpDistanceField");
				if (material2 != null)
				{
					this._tmpShader = material2.shader;
				}
			}
			if (this._tmpMobileShader == null)
			{
				Material material3 = Resources.Load<Material>("PhoneStableTmpMobileDistanceField");
				if (material3 != null)
				{
					this._tmpMobileShader = material3.shader;
				}
			}
			if (this._shader == null && this._tmpShader == null && this._tmpMobileShader == null)
			{
				return;
			}
			this._retiredModifiers.Clear();
			foreach (PhoneStableUiGraphic phoneStableUiGraphic in this._modifiers)
			{
				if (!(phoneStableUiGraphic != null) || !phoneStableUiGraphic.transform.IsChildOf(base.transform))
				{
					if (phoneStableUiGraphic != null)
					{
						phoneStableUiGraphic.Bind(null);
						PhoneStableUiMaterials.DestroyOwned(phoneStableUiGraphic);
					}
					this._retiredModifiers.Add(phoneStableUiGraphic);
				}
			}
			foreach (PhoneStableUiGraphic phoneStableUiGraphic2 in this._retiredModifiers)
			{
				this._modifiers.Remove(phoneStableUiGraphic2);
			}
			base.GetComponentsInChildren<Graphic>(true, this._graphics);
			foreach (Graphic graphic in this._graphics)
			{
				if (!(graphic == null))
				{
					PhoneStableUiGraphic phoneStableUiGraphic3 = graphic.GetComponent<PhoneStableUiGraphic>();
					if (phoneStableUiGraphic3 == null)
					{
						phoneStableUiGraphic3 = graphic.gameObject.AddComponent<PhoneStableUiGraphic>();
						phoneStableUiGraphic3.hideFlags = HideFlags.DontSave;
					}
					phoneStableUiGraphic3.Bind(this);
					this._modifiers.Add(phoneStableUiGraphic3);
				}
			}
			this._deadMaterials.Clear();
			foreach (KeyValuePair<Material, Material> keyValuePair in this._materials)
			{
				if (keyValuePair.Key == null)
				{
					PhoneStableUiMaterials.DestroyOwned(keyValuePair.Value);
					this._deadMaterials.Add(keyValuePair.Key);
				}
				else
				{
					keyValuePair.Value.shader = this.GetStableShader(keyValuePair.Key) ?? keyValuePair.Key.shader;
					keyValuePair.Value.CopyPropertiesFromMaterial(keyValuePair.Key);
					keyValuePair.Value.renderQueue = keyValuePair.Key.renderQueue;
				}
			}
			foreach (Material material4 in this._deadMaterials)
			{
				this._materials.Remove(material4);
			}
		}

		// Token: 0x06006D78 RID: 28024 RVA: 0x002BF84C File Offset: 0x002BDA4C
		internal Material GetStableMaterial(Material source)
		{
			if (!base.isActiveAndEnabled)
			{
				return source;
			}
			Shader stableShader = this.GetStableShader(source);
			if (stableShader == null)
			{
				return source;
			}
			Material material;
			if (!this._materials.TryGetValue(source, out material) || material == null)
			{
				material = new Material(source)
				{
					shader = stableShader,
					name = source.name + " (Phone Stable UI)",
					hideFlags = HideFlags.HideAndDontSave
				};
				material.CopyPropertiesFromMaterial(source);
				material.renderQueue = source.renderQueue;
				this._materials[source] = material;
			}
			return material;
		}

		// Token: 0x06006D79 RID: 28025 RVA: 0x002BF8DC File Offset: 0x002BDADC
		private Shader GetStableShader(Material source)
		{
			if (source == null || source.shader == null)
			{
				return null;
			}
			string name = source.shader.name;
			if (name == "UI/Default")
			{
				return this._shader;
			}
			if (name == "TextMeshPro/Distance Field")
			{
				return this._tmpShader;
			}
			if (!(name == "TextMeshPro/Mobile/Distance Field"))
			{
				return null;
			}
			return this._tmpMobileShader;
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x002BF950 File Offset: 0x002BDB50
		private void OnDisable()
		{
			Canvas.preWillRenderCanvases -= this.RefreshMaterials;
			foreach (PhoneStableUiGraphic phoneStableUiGraphic in this._modifiers)
			{
				if (phoneStableUiGraphic != null)
				{
					phoneStableUiGraphic.Bind(null);
				}
			}
			foreach (Material material in this._materials.Values)
			{
				PhoneStableUiMaterials.DestroyOwned(material);
			}
			this._materials.Clear();
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x002BFA0C File Offset: 0x002BDC0C
		private void OnDestroy()
		{
			this.OnDisable();
			foreach (PhoneStableUiGraphic phoneStableUiGraphic in this._modifiers)
			{
				if (phoneStableUiGraphic != null)
				{
					PhoneStableUiMaterials.DestroyOwned(phoneStableUiGraphic);
				}
			}
			this._modifiers.Clear();
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyOwned(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x04007012 RID: 28690
		public const string MaterialResourcePath = "PhoneStableDefault";

		// Token: 0x04007013 RID: 28691
		public const string TmpMaterialResourcePath = "PhoneStableTmpDistanceField";

		// Token: 0x04007014 RID: 28692
		public const string TmpMobileMaterialResourcePath = "PhoneStableTmpMobileDistanceField";

		// Token: 0x04007015 RID: 28693
		private readonly List<Graphic> _graphics = new List<Graphic>();

		// Token: 0x04007016 RID: 28694
		private readonly HashSet<PhoneStableUiGraphic> _modifiers = new HashSet<PhoneStableUiGraphic>();

		// Token: 0x04007017 RID: 28695
		private readonly List<PhoneStableUiGraphic> _retiredModifiers = new List<PhoneStableUiGraphic>();

		// Token: 0x04007018 RID: 28696
		private readonly Dictionary<Material, Material> _materials = new Dictionary<Material, Material>();

		// Token: 0x04007019 RID: 28697
		private readonly List<Material> _deadMaterials = new List<Material>();

		// Token: 0x0400701A RID: 28698
		private Shader _shader;

		// Token: 0x0400701B RID: 28699
		private Shader _tmpShader;

		// Token: 0x0400701C RID: 28700
		private Shader _tmpMobileShader;
	}
}
