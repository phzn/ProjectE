using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist
{
	// Token: 0x020008DB RID: 2267
	public class EyeEffectController : MonoBehaviour
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x0016C77B File Offset: 0x0016A97B
		private void Start()
		{
			if (this._invisibleMaterial == null && this.invisibleShader != null)
			{
				this._invisibleMaterial = new Material(this.invisibleShader);
			}
			this.CreateContainer();
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x0016C7B0 File Offset: 0x0016A9B0
		private void OnDestroy()
		{
			this.ClearAllEffects();
			if (this._effectsContainer != null)
			{
				Object.Destroy(this._effectsContainer.gameObject);
			}
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x0016C7D6 File Offset: 0x0016A9D6
		private void LateUpdate()
		{
			this.ApplyActiveBlendShapeOverrides();
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0016C7E0 File Offset: 0x0016A9E0
		private void InitializeBlendShapeCache()
		{
			if (this._blendShapeCacheInitialized || this.headRenderer == null)
			{
				return;
			}
			Mesh sharedMesh = this.headRenderer.sharedMesh;
			if (sharedMesh == null)
			{
				return;
			}
			this._blendShapeIndexCache.Clear();
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				string blendShapeName = sharedMesh.GetBlendShapeName(i);
				this._blendShapeIndexCache[blendShapeName] = i;
			}
			this._blendShapeCacheInitialized = true;
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x0016C854 File Offset: 0x0016AA54
		private void ApplyActiveBlendShapeOverrides()
		{
			if (this.headRenderer == null)
			{
				return;
			}
			foreach (EyeEffectController.EyeEffectPreset eyeEffectPreset in this.presets)
			{
				if (eyeEffectPreset.isActive && eyeEffectPreset.blendShapeOverrides != null)
				{
					foreach (EyeEffectController.BlendShapeOverride blendShapeOverride in eyeEffectPreset.blendShapeOverrides)
					{
						if (!string.IsNullOrEmpty(blendShapeOverride.blendShapeName))
						{
							int blendShapeIndex = this.GetBlendShapeIndex(blendShapeOverride.blendShapeName);
							if (blendShapeIndex >= 0)
							{
								this.headRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeOverride.value);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x0016C930 File Offset: 0x0016AB30
		private int GetBlendShapeIndex(string blendShapeName)
		{
			this.InitializeBlendShapeCache();
			int blendShapeIndex;
			if (this._blendShapeIndexCache.TryGetValue(blendShapeName, out blendShapeIndex))
			{
				return blendShapeIndex;
			}
			Mesh sharedMesh = this.headRenderer.sharedMesh;
			if (sharedMesh != null)
			{
				blendShapeIndex = sharedMesh.GetBlendShapeIndex(blendShapeName);
				if (blendShapeIndex >= 0)
				{
					this._blendShapeIndexCache[blendShapeName] = blendShapeIndex;
				}
			}
			return blendShapeIndex;
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x0016C984 File Offset: 0x0016AB84
		private void CreateContainer()
		{
			if (this._effectsContainer == null)
			{
				this._effectsContainer = new GameObject("EyeEffectsContainer").transform;
				this._effectsContainer.SetParent(base.transform, false);
				this._effectsContainer.localPosition = Vector3.zero;
				this._effectsContainer.localRotation = Quaternion.identity;
				this._effectsContainer.localScale = Vector3.one;
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x0016C9F8 File Offset: 0x0016ABF8
		public void AddEffect(EyeEffectController.EyeEffectData data)
		{
			if (this.headRenderer == null || this.eyeEffectShader == null)
			{
				return;
			}
			this.CreateContainer();
			GameObject gameObject = new GameObject("EyeEffect_" + data.name);
			gameObject.transform.SetParent(this._effectsContainer, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.sharedMesh = this.headRenderer.sharedMesh;
			skinnedMeshRenderer.bones = this.headRenderer.bones;
			skinnedMeshRenderer.rootBone = this.headRenderer.rootBone;
			skinnedMeshRenderer.localBounds = this.headRenderer.localBounds;
			Material[] array = new Material[this.headRenderer.sharedMesh.subMeshCount];
			Material material = this.CreateEffectMaterial(data, this._activeEffects.Count);
			for (int i = 0; i < array.Length; i++)
			{
				if (i == this.eyeMaterialIndex)
				{
					array[i] = material;
				}
				else
				{
					array[i] = this._invisibleMaterial;
				}
			}
			skinnedMeshRenderer.materials = array;
			gameObject.AddComponent<BlendShapeSynchronizer>().SetSourceRenderer(this.headRenderer);
			this._activeEffects.Add(new EyeEffectController.EyeEffectInstance
			{
				gameObject = gameObject,
				renderer = skinnedMeshRenderer,
				material = material
			});
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0016CB5E File Offset: 0x0016AD5E
		public void RemoveEffect(int index)
		{
			if (index >= 0 && index < this._activeEffects.Count)
			{
				Object.Destroy(this._activeEffects[index].gameObject);
				this._activeEffects.RemoveAt(index);
			}
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x0016CB94 File Offset: 0x0016AD94
		public void ClearAllEffects()
		{
			foreach (EyeEffectController.EyeEffectInstance eyeEffectInstance in this._activeEffects)
			{
				if (eyeEffectInstance.gameObject != null)
				{
					Object.Destroy(eyeEffectInstance.gameObject);
				}
			}
			this._activeEffects.Clear();
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x0016CC04 File Offset: 0x0016AE04
		private Material CreateEffectMaterial(EyeEffectController.EyeEffectData data, int layerIndex)
		{
			Material material = new Material(this.eyeEffectShader);
			material.name = "Mat_EyeEffect_" + data.name;
			if (data.mainTexture != null)
			{
				material.SetTexture("_MainTex", data.mainTexture);
			}
			if (data.maskTexture != null)
			{
				material.SetTexture("_MaskTex", data.maskTexture);
			}
			material.SetColor("_Color", data.color);
			material.SetFloat("_RotationSpeed", data.rotationSpeed);
			material.SetFloat("_ScrollX", data.scrollSpeed.x);
			material.SetFloat("_ScrollY", data.scrollSpeed.y);
			material.SetFloat("_UsePrePositioned", data.usePrePositioned ? 1f : 0f);
			material.SetVector("_LeftEyeCenter", new Vector4(data.leftEyeUVCenter.x, data.leftEyeUVCenter.y, 0f, 0f));
			material.SetVector("_RightEyeCenter", new Vector4(data.rightEyeUVCenter.x, data.rightEyeUVCenter.y, 0f, 0f));
			material.SetFloat("_EyeRadius", data.eyeRadius);
			material.SetFloat("_UVScale", data.uvScale);
			material.SetFloat("_SrcBlend", (float)data.srcBlend);
			material.SetFloat("_DstBlend", (float)data.dstBlend);
			material.SetFloat("_BaseMode", (float)data.baseMode);
			material.SetColor("_BaseColor", data.baseColor);
			material.renderQueue = this.baseRenderQueue + layerIndex;
			return material;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x0016CDB8 File Offset: 0x0016AFB8
		public void ActivatePreset(string presetName)
		{
			EyeEffectController.EyeEffectPreset eyeEffectPreset = this.presets.Find((EyeEffectController.EyeEffectPreset p) => p.presetName == presetName);
			if (eyeEffectPreset == null)
			{
				Debug.LogWarning("[EyeEffectController] Preset '" + presetName + "' not found!");
				return;
			}
			this.ActivatePreset(eyeEffectPreset);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0016CE0F File Offset: 0x0016B00F
		public void ActivatePresetByIndex(int index)
		{
			if (index < 0 || index >= this.presets.Count)
			{
				Debug.LogWarning(string.Format("[EyeEffectController] Preset index {0} out of range!", index));
				return;
			}
			this.ActivatePreset(this.presets[index]);
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0016CE4C File Offset: 0x0016B04C
		private void ActivatePreset(EyeEffectController.EyeEffectPreset preset)
		{
			if (preset.isActive)
			{
				Debug.Log("[EyeEffectController] Preset '" + preset.presetName + "' is already active.");
				return;
			}
			EyeEffectController.EyeEffectData eyeEffectData = this.PresetToData(preset);
			this.AddEffect(eyeEffectData);
			preset.isActive = true;
			this._activePresetIndices[preset.presetName] = this._activeEffects.Count - 1;
			Debug.Log("[EyeEffectController] Activated preset '" + preset.presetName + "'");
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0016CECC File Offset: 0x0016B0CC
		public void DeactivatePreset(string presetName)
		{
			EyeEffectController.EyeEffectPreset eyeEffectPreset = this.presets.Find((EyeEffectController.EyeEffectPreset p) => p.presetName == presetName);
			if (eyeEffectPreset == null || !eyeEffectPreset.isActive)
			{
				return;
			}
			int num;
			if (this._activePresetIndices.TryGetValue(presetName, out num))
			{
				for (int i = 0; i < this._activeEffects.Count; i++)
				{
					if (this._activeEffects[i].gameObject.name == "EyeEffect_" + presetName)
					{
						this.RemoveEffect(i);
						break;
					}
				}
				this._activePresetIndices.Remove(presetName);
			}
			eyeEffectPreset.isActive = false;
			Debug.Log("[EyeEffectController] Deactivated preset '" + eyeEffectPreset.presetName + "'");
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x0016CFA0 File Offset: 0x0016B1A0
		public void TogglePreset(string presetName)
		{
			EyeEffectController.EyeEffectPreset eyeEffectPreset = this.presets.Find((EyeEffectController.EyeEffectPreset p) => p.presetName == presetName);
			if (eyeEffectPreset == null)
			{
				return;
			}
			if (eyeEffectPreset.isActive)
			{
				this.DeactivatePreset(presetName);
				return;
			}
			this.ActivatePreset(presetName);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x0016CFF7 File Offset: 0x0016B1F7
		public void TogglePresetByIndex(int index)
		{
			if (index < 0 || index >= this.presets.Count)
			{
				return;
			}
			this.TogglePreset(this.presets[index].presetName);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x0016D024 File Offset: 0x0016B224
		private EyeEffectController.EyeEffectData PresetToData(EyeEffectController.EyeEffectPreset preset)
		{
			return new EyeEffectController.EyeEffectData
			{
				name = preset.presetName,
				mainTexture = preset.effectTexture,
				maskTexture = ((preset.customMask != null) ? preset.customMask : this.sharedMaskTexture),
				color = preset.color,
				rotationSpeed = preset.rotationSpeed,
				scrollSpeed = preset.scrollSpeed,
				usePrePositioned = preset.usePrePositioned,
				leftEyeUVCenter = preset.leftEyeUVCenter,
				rightEyeUVCenter = preset.rightEyeUVCenter,
				eyeRadius = preset.eyeRadius,
				uvScale = preset.uvScale,
				baseMode = preset.baseMode,
				baseColor = preset.baseColor,
				srcBlend = preset.srcBlend,
				dstBlend = preset.dstBlend
			};
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x0016D100 File Offset: 0x0016B300
		[ContextMenu("Activate First Preset")]
		private void DebugActivateFirst()
		{
			if (this.presets.Count > 0)
			{
				this.ActivatePresetByIndex(0);
			}
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0016D118 File Offset: 0x0016B318
		[ContextMenu("Deactivate All")]
		private void DebugDeactivateAll()
		{
			this.ClearAllEffects();
			foreach (EyeEffectController.EyeEffectPreset eyeEffectPreset in this.presets)
			{
				eyeEffectPreset.isActive = false;
			}
			this._activePresetIndices.Clear();
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0016D17C File Offset: 0x0016B37C
		[ContextMenu("Toggle First Preset")]
		private void DebugToggleFirst()
		{
			if (this.presets.Count > 0)
			{
				this.TogglePresetByIndex(0);
			}
		}

		// Token: 0x04003D3A RID: 15674
		[Header("Target")]
		[Tooltip("SkinnedMeshRenderer della testa che contiene gli occhi")]
		public SkinnedMeshRenderer headRenderer;

		// Token: 0x04003D3B RID: 15675
		[Tooltip("Indice del materiale degli occhi nella lista dei materiali della testa")]
		public int eyeMaterialIndex = 2;

		// Token: 0x04003D3C RID: 15676
		[Header("Resources")]
		[Tooltip("Shader ProjectExhibitionist/AnimeEyeEffect")]
		public Shader eyeEffectShader;

		// Token: 0x04003D3D RID: 15677
		[Tooltip("Shader ProjectExhibitionist/InvisibleMask per nascondere le altre parti")]
		public Shader invisibleShader;

		// Token: 0x04003D3E RID: 15678
		[Header("Settings")]
		public int baseRenderQueue = 3100;

		// Token: 0x04003D3F RID: 15679
		[Header("Shared Textures")]
		[Tooltip("Maschera condivisa per tutti gli effetti (iridi)")]
		public Texture2D sharedMaskTexture;

		// Token: 0x04003D40 RID: 15680
		[Header("Effect Presets (Test)")]
		public List<EyeEffectController.EyeEffectPreset> presets = new List<EyeEffectController.EyeEffectPreset>();

		// Token: 0x04003D41 RID: 15681
		private Transform _effectsContainer;

		// Token: 0x04003D42 RID: 15682
		private Material _invisibleMaterial;

		// Token: 0x04003D43 RID: 15683
		private List<EyeEffectController.EyeEffectInstance> _activeEffects = new List<EyeEffectController.EyeEffectInstance>();

		// Token: 0x04003D44 RID: 15684
		private Dictionary<string, int> _activePresetIndices = new Dictionary<string, int>();

		// Token: 0x04003D45 RID: 15685
		private Dictionary<string, int> _blendShapeIndexCache = new Dictionary<string, int>();

		// Token: 0x04003D46 RID: 15686
		private bool _blendShapeCacheInitialized;

		// Token: 0x020008DC RID: 2268
		public enum BaseMode
		{
			// Token: 0x04003D48 RID: 15688
			None,
			// Token: 0x04003D49 RID: 15689
			SolidColor
		}

		// Token: 0x020008DD RID: 2269
		[Serializable]
		public class BlendShapeOverride
		{
			// Token: 0x04003D4A RID: 15690
			[Tooltip("Nome del BlendShape da sovrascrivere")]
			public string blendShapeName;

			// Token: 0x04003D4B RID: 15691
			[Range(0f, 100f)]
			[Tooltip("Valore del BlendShape (0-100)")]
			public float value = 100f;
		}

		// Token: 0x020008DE RID: 2270
		[Serializable]
		public class EyeEffectPreset
		{
			// Token: 0x04003D4C RID: 15692
			public string presetName = "New Effect";

			// Token: 0x04003D4D RID: 15693
			[Header("Textures")]
			[Tooltip("Texture dell'effetto (es. spirale, cuore).")]
			public Texture2D effectTexture;

			// Token: 0x04003D4E RID: 15694
			[Tooltip("Maschera custom (opzionale, usa sharedMaskTexture se null)")]
			public Texture2D customMask;

			// Token: 0x04003D4F RID: 15695
			[ColorUsage(true, true)]
			public Color color = Color.white;

			// Token: 0x04003D50 RID: 15696
			[Header("Animation")]
			[Tooltip("Velocità di rotazione (radianti/sec). Positivo = antiorario, Negativo = orario.")]
			public float rotationSpeed = 2f;

			// Token: 0x04003D51 RID: 15697
			[Tooltip("Velocità di scroll UV (unità/sec).")]
			public Vector2 scrollSpeed = Vector2.zero;

			// Token: 0x04003D52 RID: 15698
			[Header("UV Mode")]
			[Tooltip("Se attivo, usa la texture già posizionata (effetti disegnati nelle posizioni degli occhi). Se disattivo, centra la texture singola su ogni occhio.")]
			public bool usePrePositioned;

			// Token: 0x04003D53 RID: 15699
			[Header("UV Transform")]
			public Vector2 leftEyeUVCenter = new Vector2(0.12f, 0.85f);

			// Token: 0x04003D54 RID: 15700
			public Vector2 rightEyeUVCenter = new Vector2(0.88f, 0.85f);

			// Token: 0x04003D55 RID: 15701
			[Tooltip("Raggio dell'occhio nello spazio UV. Regola per far combaciare la spirale con l'iride.")]
			public float eyeRadius = 0.12f;

			// Token: 0x04003D56 RID: 15702
			public float uvScale = 1f;

			// Token: 0x04003D57 RID: 15703
			[Header("Base Cover")]
			[Tooltip("None = overlay (occhio originale visibile), SolidColor = copre con colore + effetto")]
			public EyeEffectController.BaseMode baseMode;

			// Token: 0x04003D58 RID: 15704
			public Color baseColor = Color.white;

			// Token: 0x04003D59 RID: 15705
			[Header("Blending")]
			public BlendMode srcBlend = BlendMode.SrcAlpha;

			// Token: 0x04003D5A RID: 15706
			public BlendMode dstBlend = BlendMode.OneMinusSrcAlpha;

			// Token: 0x04003D5B RID: 15707
			[Header("BlendShape Overrides")]
			[Tooltip("Lista di BlendShape da applicare quando questo preset è attivo (es. eye_>< per chiudere gli occhi)")]
			public List<EyeEffectController.BlendShapeOverride> blendShapeOverrides = new List<EyeEffectController.BlendShapeOverride>();

			// Token: 0x04003D5C RID: 15708
			[Header("Runtime State")]
			[HideInInspector]
			public bool isActive;
		}

		// Token: 0x020008DF RID: 2271
		[Serializable]
		public class EyeEffectData
		{
			// Token: 0x04003D5D RID: 15709
			public string name;

			// Token: 0x04003D5E RID: 15710
			[Header("Textures")]
			[Tooltip("Texture dell'effetto (es. spirale, cuore). Deve essere seamless/tileable per rotazione.")]
			public Texture2D mainTexture;

			// Token: 0x04003D5F RID: 15711
			[Tooltip("Maschera per limitare l'effetto all'iride. Bianco = visibile, Nero = invisibile.")]
			public Texture2D maskTexture;

			// Token: 0x04003D60 RID: 15712
			[ColorUsage(true, true)]
			public Color color = Color.white;

			// Token: 0x04003D61 RID: 15713
			[Header("Animation")]
			[Tooltip("Velocità di rotazione (radianti/sec). Positivo = antiorario, Negativo = orario.")]
			public float rotationSpeed;

			// Token: 0x04003D62 RID: 15714
			[Tooltip("Velocità di scroll UV (unità/sec).")]
			public Vector2 scrollSpeed = Vector2.zero;

			// Token: 0x04003D63 RID: 15715
			[Header("UV Mode")]
			public bool usePrePositioned;

			// Token: 0x04003D64 RID: 15716
			[Header("UV Transform")]
			[Tooltip("Centro UV dell'occhio sinistro nella texture map (basato su BoZo face texture).")]
			public Vector2 leftEyeUVCenter = new Vector2(0.12f, 0.85f);

			// Token: 0x04003D65 RID: 15717
			[Tooltip("Centro UV dell'occhio destro nella texture map (basato su BoZo face texture).")]
			public Vector2 rightEyeUVCenter = new Vector2(0.88f, 0.85f);

			// Token: 0x04003D66 RID: 15718
			[Tooltip("Raggio dell'occhio nello spazio UV.")]
			public float eyeRadius = 0.12f;

			// Token: 0x04003D67 RID: 15719
			[Tooltip("Scala dell'effetto. >1 = più grande, <1 = più piccolo.")]
			public float uvScale = 1f;

			// Token: 0x04003D68 RID: 15720
			[Header("Base Cover")]
			public EyeEffectController.BaseMode baseMode;

			// Token: 0x04003D69 RID: 15721
			public Color baseColor = Color.white;

			// Token: 0x04003D6A RID: 15722
			[Header("Blending")]
			[Tooltip("SrcAlpha + OneMinusSrcAlpha = Normal, One + One = Additive (glow)")]
			public BlendMode srcBlend = BlendMode.SrcAlpha;

			// Token: 0x04003D6B RID: 15723
			public BlendMode dstBlend = BlendMode.OneMinusSrcAlpha;
		}

		// Token: 0x020008E0 RID: 2272
		private class EyeEffectInstance
		{
			// Token: 0x04003D6C RID: 15724
			public GameObject gameObject;

			// Token: 0x04003D6D RID: 15725
			public SkinnedMeshRenderer renderer;

			// Token: 0x04003D6E RID: 15726
			public Material material;
		}
	}
}
