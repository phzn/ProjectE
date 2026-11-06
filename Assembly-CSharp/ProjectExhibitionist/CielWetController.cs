using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008EA RID: 2282
	public class CielWetController : MonoBehaviour
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x0016E1FF File Offset: 0x0016C3FF
		public bool IsWetRequested
		{
			get
			{
				return this._legacyWetRequested || this._wetOwners.Count > 0;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x0016E219 File Offset: 0x0016C419
		public float WetAmount
		{
			get
			{
				return this._wet01;
			}
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0016E224 File Offset: 0x0016C424
		public void ResetForHomeReturn()
		{
			this._wetOwners.Clear();
			this._legacyWetRequested = false;
			this._target01 = (this._wet01 = 0f);
			this._running = false;
			this._resolved = false;
			this.ResolveMaterials();
			this.ApplyLookEnabled(false);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0016E272 File Offset: 0x0016C472
		public void SetWet(bool wet)
		{
			this._legacyWetRequested = wet;
			this.SetWetTarget(this.IsWetRequested);
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0016E287 File Offset: 0x0016C487
		public void AcquireWet(string owner)
		{
			if (string.IsNullOrEmpty(owner))
			{
				Debug.LogWarning("[CielWet] AcquireWet richiede un owner non vuoto.", this);
				return;
			}
			this._wetOwners.Add(owner);
			this.SetWetTarget(true);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x0016E2B1 File Offset: 0x0016C4B1
		public void ReleaseWet(string owner)
		{
			if (string.IsNullOrEmpty(owner) || !this._wetOwners.Remove(owner))
			{
				return;
			}
			this.SetWetTarget(this.IsWetRequested);
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0016E2D8 File Offset: 0x0016C4D8
		private void SetWetTarget(bool wet)
		{
			this._resolved = false;
			this.ResolveMaterials();
			if (this._materials.Count == 0)
			{
				return;
			}
			this._target01 = (wet ? 1f : 0f);
			if (wet)
			{
				this.ApplyLookEnabled(true);
			}
			this._running = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[CielWet] Target = {0} (from {1:0.00}), ", this._target01, this._wet01) + string.Format("owners={0}, legacy={1}, mats={2}.", this._wetOwners.Count, this._legacyWetRequested, this._materials.Count), this);
			}
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0016E390 File Offset: 0x0016C590
		private void Update()
		{
			if (!this._running)
			{
				return;
			}
			float num;
			if (this._target01 > this._wet01)
			{
				num = ((this.fadeInDuration > 0f) ? (1f / this.fadeInDuration) : 1000f);
			}
			else
			{
				num = ((this.fadeOutDuration > 0f) ? (1f / this.fadeOutDuration) : 1000f);
			}
			this._wet01 = Mathf.MoveTowards(this._wet01, this._target01, num * Time.unscaledDeltaTime);
			this.ApplyIntensity(this._wet01);
			if (Mathf.Approximately(this._wet01, this._target01))
			{
				this._running = false;
				if (this._target01 <= 0f)
				{
					this.ApplyLookEnabled(false);
				}
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[CielWet] Reached {0:0.00} (GSAA lerp done).", this._wet01), this);
				}
			}
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0016E474 File Offset: 0x0016C674
		private void ApplyIntensity(float t)
		{
			for (int i = 0; i < this._materials.Count; i++)
			{
				Material material = this._materials[i];
				if (!(material == null))
				{
					float num = ((i < this._intensityScale.Count) ? this._intensityScale[i] : 1f);
					float num2 = Mathf.Lerp(0f, this.wetGSAA * num, t);
					CielWetController.SetFloat(material, CielWetController.GSAAId, num2);
				}
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0016E4F0 File Offset: 0x0016C6F0
		private void ApplyLookEnabled(bool on)
		{
			foreach (Material material in this._materials)
			{
				if (!(material == null))
				{
					if (on)
					{
						CielWetController.SetFloat(material, CielWetController.UseReflectionId, 1f);
						CielWetController.SetFloat(material, CielWetController.ReflectanceId, this.wetReflectance);
						CielWetController.SetFloat(material, CielWetController.SpecularBorderId, this.wetSpecularBorder);
						CielWetController.SetFloat(material, CielWetController.SpecularBlurId, this.wetSpecularBlur);
						material.EnableKeyword("_USEREFLECTION");
					}
					else
					{
						CielWetController.SetFloat(material, CielWetController.UseReflectionId, 0f);
						CielWetController.SetFloat(material, CielWetController.ReflectanceId, this.dryReflectance);
						CielWetController.SetFloat(material, CielWetController.SpecularBorderId, this.drySpecularBorder);
						CielWetController.SetFloat(material, CielWetController.SpecularBlurId, this.drySpecularBlur);
						CielWetController.SetFloat(material, CielWetController.GSAAId, this.dryGSAA);
						material.DisableKeyword("_USEREFLECTION");
					}
				}
			}
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x0016E600 File Offset: 0x0016C800
		private static void SetFloat(Material m, int id, float value)
		{
			if (m.HasProperty(id))
			{
				m.SetFloat(id, value);
			}
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0016E614 File Offset: 0x0016C814
		private void ResolveMaterials()
		{
			if (this._resolved && this._materials.Count > 0)
			{
				return;
			}
			Transform transform = this.ResolveCharacterRoot();
			this.BindAppearanceEvents(transform);
			this._materials.Clear();
			this._intensityScale.Clear();
			HashSet<Material> hashSet = new HashSet<Material>();
			foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>(true))
			{
				SkinnedMeshRenderer skinnedMeshRenderer = null;
				bool flag = this._skinDecals != null && this._skinDecals.TryGetDecalSource(renderer, out skinnedMeshRenderer);
				bool flag2 = flag && this.SourceUsesTargetMaterial(skinnedMeshRenderer);
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!(material == null) && !hashSet.Contains(material) && (flag2 || (!flag && this.IsTargetMaterial(material.name))))
					{
						hashSet.Add(material);
						this._materials.Add(material);
						bool flag3 = (flag2 ? (skinnedMeshRenderer == this._skinDecals.headRenderer) : (!string.IsNullOrEmpty(this.faceMaterialNameContains) && material.name.ToLowerInvariant().Contains(this.faceMaterialNameContains.ToLowerInvariant())));
						this._intensityScale.Add(flag3 ? this.faceWetMultiplier : 1f);
					}
				}
			}
			this._resolved = true;
			if (this.debugLogs)
			{
				List<string> list = new List<string>();
				foreach (Material material2 in this._materials)
				{
					list.Add(material2.name);
				}
				Debug.Log(string.Format("[CielWet] Resolved {0} wet material(s): {1}", this._materials.Count, string.Join(", ", list)), this);
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0016E820 File Offset: 0x0016CA20
		private Transform ResolveCharacterRoot()
		{
			Transform transform = base.transform;
			while (transform != null)
			{
				if (transform.CompareTag("Player"))
				{
					return transform;
				}
				transform = transform.parent;
			}
			return base.transform.root;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0016E860 File Offset: 0x0016CA60
		private bool IsTargetMaterial(string matName)
		{
			if (string.IsNullOrEmpty(matName) || this.targetMaterialNameContains == null)
			{
				return false;
			}
			string text = matName.ToLowerInvariant();
			foreach (string text2 in this.targetMaterialNameContains)
			{
				if (!string.IsNullOrEmpty(text2) && text.Contains(text2.ToLowerInvariant()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0016E8B8 File Offset: 0x0016CAB8
		private bool SourceUsesTargetMaterial(Renderer source)
		{
			if (source == null)
			{
				return false;
			}
			foreach (Material material in source.sharedMaterials)
			{
				if (material != null && this.IsTargetMaterial(material.name))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x0016E904 File Offset: 0x0016CB04
		private void BindAppearanceEvents(Transform root)
		{
			if (this._skinDecals == null)
			{
				this._skinDecals = root.GetComponentInChildren<SkinDecalController>(true);
				if (this._skinDecals != null)
				{
					this._skinDecals.OnDecalsChanged += this.HandleAppearanceChanged;
				}
			}
			if (this._styles == null)
			{
				this._styles = root.GetComponentInChildren<MCStyleCustomizationService>(true);
				if (this._styles != null)
				{
					this._styles.OnActiveStateChanged += this.HandleAppearanceChanged;
				}
			}
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0016E994 File Offset: 0x0016CB94
		private void HandleAppearanceChanged()
		{
			if (this._wet01 > 0f || this._running)
			{
				this.ApplyLookEnabled(false);
			}
			this._resolved = false;
			if (this._wet01 <= 0f && !this._running)
			{
				return;
			}
			this.ResolveMaterials();
			this.ApplyLookEnabled(true);
			this.ApplyIntensity(this._wet01);
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0016E9F4 File Offset: 0x0016CBF4
		private void UnbindAppearanceEvents()
		{
			if (this._skinDecals != null)
			{
				this._skinDecals.OnDecalsChanged -= this.HandleAppearanceChanged;
			}
			if (this._styles != null)
			{
				this._styles.OnActiveStateChanged -= this.HandleAppearanceChanged;
			}
			this._skinDecals = null;
			this._styles = null;
			this._resolved = false;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x0016EA60 File Offset: 0x0016CC60
		private void ForceDryImmediate()
		{
			this._legacyWetRequested = false;
			this._wetOwners.Clear();
			this.ResolveMaterials();
			this._wet01 = 0f;
			this._target01 = 0f;
			this._running = false;
			this.ApplyLookEnabled(false);
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x0016EA9E File Offset: 0x0016CC9E
		private void OnDisable()
		{
			if (this._wet01 > 0f || this._running)
			{
				this.ForceDryImmediate();
			}
			this.UnbindAppearanceEvents();
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x0016EA9E File Offset: 0x0016CC9E
		private void OnDestroy()
		{
			if (this._wet01 > 0f || this._running)
			{
				this.ForceDryImmediate();
			}
			this.UnbindAppearanceEvents();
		}

		// Token: 0x04003DAC RID: 15788
		[Header("Target materials (by name substring, case-insensitive)")]
		[Tooltip("Shared materials whose name CONTAINS one of these get the wet look. Covers Ciel's body, face and hair.")]
		[SerializeField]
		private string[] targetMaterialNameContains = new string[] { "body", "face", "hair" };

		// Token: 0x04003DAD RID: 15789
		[Header("Wet look (held at peak while wet)")]
		[SerializeField]
		private float wetReflectance = 1f;

		// Token: 0x04003DAE RID: 15790
		[SerializeField]
		private float wetSpecularBorder = 0.149f;

		// Token: 0x04003DAF RID: 15791
		[SerializeField]
		private float wetSpecularBlur = 1f;

		// Token: 0x04003DB0 RID: 15792
		[Header("Wet intensity via _GSAAStrength (this is what fades)")]
		[Tooltip("_GSAAStrength at full wet (the peak after fade-in).")]
		[SerializeField]
		private float wetGSAA = 1f;

		// Token: 0x04003DB1 RID: 15793
		[Tooltip("_GSAAStrength when fully dry (the material default).")]
		[SerializeField]
		private float dryGSAA = 0.549f;

		// Token: 0x04003DB2 RID: 15794
		[Header("Dry restore (defaults del materiale a cui tornare quando asciutto)")]
		[Tooltip("Valori riscritti sui materiali quando il wet si spegne. I default corrispondono ai materiali body/face/hair di Ciel; un'istanza che targetta ALTRI materiali (es. il raincoat PVC, che ha SpecularBlur=1 da asciutto) li imposta sui SUOI default.")]
		[SerializeField]
		private float dryReflectance = 0.04f;

		// Token: 0x04003DB3 RID: 15795
		[SerializeField]
		private float drySpecularBorder = 0.5f;

		// Token: 0x04003DB4 RID: 15796
		[SerializeField]
		private float drySpecularBlur;

		// Token: 0x04003DB5 RID: 15797
		[Header("Per-zone intensity scale")]
		[Tooltip("Name substring identifying the FACE material(s), which get a reduced wet intensity (the face should look less wet than body/hair).")]
		[SerializeField]
		private string faceMaterialNameContains = "face";

		// Token: 0x04003DB6 RID: 15798
		[Tooltip("Multiplier applied to the FACE's wet intensity. 0.6 = 40% less wet than body/hair.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float faceWetMultiplier = 0.6f;

		// Token: 0x04003DB7 RID: 15799
		[Header("Gradual transition")]
		[Tooltip("Seconds for the wet look to ramp UP from dry to its peak.")]
		[SerializeField]
		private float fadeInDuration = 5f;

		// Token: 0x04003DB8 RID: 15800
		[Tooltip("Seconds for the wet look to ramp DOWN from peak back to dry.")]
		[SerializeField]
		private float fadeOutDuration = 5f;

		// Token: 0x04003DB9 RID: 15801
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04003DBA RID: 15802
		private static readonly int UseReflectionId = Shader.PropertyToID("_UseReflection");

		// Token: 0x04003DBB RID: 15803
		private static readonly int ReflectanceId = Shader.PropertyToID("_Reflectance");

		// Token: 0x04003DBC RID: 15804
		private static readonly int SpecularBorderId = Shader.PropertyToID("_SpecularBorder");

		// Token: 0x04003DBD RID: 15805
		private static readonly int SpecularBlurId = Shader.PropertyToID("_SpecularBlur");

		// Token: 0x04003DBE RID: 15806
		private static readonly int GSAAId = Shader.PropertyToID("_GSAAStrength");

		// Token: 0x04003DBF RID: 15807
		private readonly List<Material> _materials = new List<Material>();

		// Token: 0x04003DC0 RID: 15808
		private readonly List<float> _intensityScale = new List<float>();

		// Token: 0x04003DC1 RID: 15809
		private readonly HashSet<string> _wetOwners = new HashSet<string>();

		// Token: 0x04003DC2 RID: 15810
		private bool _legacyWetRequested;

		// Token: 0x04003DC3 RID: 15811
		private float _wet01;

		// Token: 0x04003DC4 RID: 15812
		private float _target01;

		// Token: 0x04003DC5 RID: 15813
		private bool _running;

		// Token: 0x04003DC6 RID: 15814
		private bool _resolved;

		// Token: 0x04003DC7 RID: 15815
		private SkinDecalController _skinDecals;

		// Token: 0x04003DC8 RID: 15816
		private MCStyleCustomizationService _styles;
	}
}
