using System;
using System.Collections.Generic;
using System.Linq;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist
{
	// Token: 0x020008F3 RID: 2291
	public class SkinDecalController : MonoBehaviour
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06003E17 RID: 15895 RVA: 0x0017125C File Offset: 0x0016F45C
		// (remove) Token: 0x06003E18 RID: 15896 RVA: 0x00171294 File Offset: 0x0016F494
		public event Action OnDecalsChanged;

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x001712CC File Offset: 0x0016F4CC
		public float DetectionCoverage
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.appliedDecals.Count; i++)
				{
					AppliedDecal appliedDecal = this.appliedDecals[i];
					if (appliedDecal != null && appliedDecal.isActive && !(appliedDecal.decal == null))
					{
						SkinDecal decal = appliedDecal.decal;
						if (decal.HasBodyTexture || decal.HasHeadTexture)
						{
							num += decal.detectionCoverage * Mathf.Clamp01(this.GetDisplayedDecalColor(appliedDecal).a);
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x00171350 File Offset: 0x0016F550
		public float DetectionAttentionMultiplier
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.appliedDecals.Count; i++)
				{
					AppliedDecal appliedDecal = this.appliedDecals[i];
					if (appliedDecal != null && appliedDecal.isActive && !(appliedDecal.decal == null))
					{
						SkinDecal decal = appliedDecal.decal;
						if (decal.HasBodyTexture || decal.HasHeadTexture)
						{
							num += Mathf.Max(0f, decal.detectionAttentionBonus) * Mathf.Clamp01(this.GetDisplayedDecalColor(appliedDecal).a);
						}
					}
				}
				return 1f + num;
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x001713E2 File Offset: 0x0016F5E2
		private void Awake()
		{
			this.CaptureStartupDecalState();
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x001713EA File Offset: 0x0016F5EA
		private void Start()
		{
			this.Initialize();
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x001713F2 File Offset: 0x0016F5F2
		private void OnDestroy()
		{
			this._isDestroying = true;
			this.Cleanup();
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00171401 File Offset: 0x0016F601
		public void Initialize()
		{
			if (this._isDestroying)
			{
				return;
			}
			this.CaptureStartupDecalState();
			this.NormalizeActiveTans(null);
			this.CreateContainers();
			if (this.appliedDecals.Count > 0)
			{
				this.RebuildDecalMeshes();
			}
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00171434 File Offset: 0x0016F634
		private void CreateContainers()
		{
			this._bodyDecalContainer = new GameObject("BodyDecals").transform;
			this._bodyDecalContainer.SetParent(base.transform, false);
			this._bodyDecalContainer.localPosition = Vector3.zero;
			this._bodyDecalContainer.localRotation = Quaternion.identity;
			this._bodyDecalContainer.localScale = Vector3.one;
			this._headDecalContainer = new GameObject("HeadDecals").transform;
			this._headDecalContainer.SetParent(base.transform, false);
			this._headDecalContainer.localPosition = Vector3.zero;
			this._headDecalContainer.localRotation = Quaternion.identity;
			this._headDecalContainer.localScale = Vector3.one;
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x001714F0 File Offset: 0x0016F6F0
		private void Cleanup()
		{
			this.ClearAllDecalMeshes();
			this.ReleaseSeamPaddingResources();
			SkinDecalController.DestroyObjectSafe((this._bodyDecalContainer != null) ? this._bodyDecalContainer.gameObject : null);
			SkinDecalController.DestroyObjectSafe((this._headDecalContainer != null) ? this._headDecalContainer.gameObject : null);
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x0017154C File Offset: 0x0016F74C
		public void AddDecal(SkinDecal decal, Color? tint = null, float? opacity = null)
		{
			if (decal == null)
			{
				return;
			}
			if (!decal.HasBodyTexture && !decal.HasHeadTexture)
			{
				return;
			}
			this.appliedDecals.RemoveAll((AppliedDecal d) => d.decal == decal);
			Color color = (decal.matchSkinTone ? this._currentSkinTone : (tint ?? decal.defaultTint));
			this.appliedDecals.Add(new AppliedDecal
			{
				decal = decal,
				tint = color,
				opacity = (opacity ?? decal.defaultOpacity),
				isActive = true
			});
			if (decal.decalType == SkinDecalType.Tan)
			{
				this.NormalizeActiveTans(null);
			}
			this.RebuildDecalMeshes();
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00171648 File Offset: 0x0016F848
		public void RemoveDecal(SkinDecal decal)
		{
			if (decal == null)
			{
				return;
			}
			if (this.appliedDecals.RemoveAll((AppliedDecal d) => d.decal == decal) > 0)
			{
				this.RebuildDecalMeshes();
			}
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x00171694 File Offset: 0x0016F894
		public void SetDecalActive(SkinDecal decal, bool active)
		{
			AppliedDecal applied = this.appliedDecals.Find((AppliedDecal d) => d.decal == decal);
			if (applied != null && (applied.isActive != active || (active && decal.decalType == SkinDecalType.Tan && this.appliedDecals.Any<AppliedDecal>((AppliedDecal d) => d != applied && d.isActive && d.decal != null && d.decal.decalType == SkinDecalType.Tan))))
			{
				applied.isActive = active;
				if (active && decal.decalType == SkinDecalType.Tan)
				{
					this.NormalizeActiveTans(applied);
				}
				this.RebuildDecalMeshes();
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x00171738 File Offset: 0x0016F938
		private void NormalizeActiveTans(AppliedDecal preferred = null)
		{
			AppliedDecal appliedDecal = preferred;
			if (appliedDecal == null)
			{
				for (int i = 0; i < this.appliedDecals.Count; i++)
				{
					if (this.appliedDecals[i] != null && this.appliedDecals[i].isActive && this.appliedDecals[i].decal != null && this.appliedDecals[i].decal.decalType == SkinDecalType.Tan)
					{
						appliedDecal = this.appliedDecals[i];
					}
				}
			}
			if (appliedDecal == null)
			{
				return;
			}
			for (int j = 0; j < this.appliedDecals.Count; j++)
			{
				AppliedDecal appliedDecal2 = this.appliedDecals[j];
				if (appliedDecal2 != null && appliedDecal2 != appliedDecal && appliedDecal2.isActive && appliedDecal2.decal != null && appliedDecal2.decal.decalType == SkinDecalType.Tan)
				{
					appliedDecal2.isActive = false;
				}
			}
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x00171818 File Offset: 0x0016FA18
		public void SetDecalOpacity(SkinDecal decal, float opacity)
		{
			AppliedDecal appliedDecal = this.appliedDecals.Find((AppliedDecal d) => d.decal == decal);
			if (appliedDecal != null)
			{
				appliedDecal.opacity = Mathf.Clamp01(opacity);
				this.UpdateDecalMaterial(appliedDecal);
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x00171860 File Offset: 0x0016FA60
		public void SetDecalTint(SkinDecal decal, Color tint)
		{
			AppliedDecal appliedDecal = this.appliedDecals.Find((AppliedDecal d) => d.decal == decal);
			if (appliedDecal != null)
			{
				appliedDecal.tint = tint;
				this.UpdateDecalMaterial(appliedDecal);
			}
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x001718A4 File Offset: 0x0016FAA4
		public void SetSkinTone(Color skinColor)
		{
			this._currentSkinTone = skinColor;
			bool flag = false;
			for (int i = 0; i < this.appliedDecals.Count; i++)
			{
				AppliedDecal appliedDecal = this.appliedDecals[i];
				if (appliedDecal.decal != null && appliedDecal.decal.matchSkinTone)
				{
					appliedDecal.tint = skinColor;
					this.UpdateDecalMaterial(appliedDecal);
					flag = true;
				}
			}
			if (flag)
			{
				Action onDecalsChanged = this.OnDecalsChanged;
				if (onDecalsChanged == null)
				{
					return;
				}
				onDecalsChanged();
			}
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0017191B File Offset: 0x0016FB1B
		public void ClearTemporaryDecals()
		{
			if (this.appliedDecals.RemoveAll((AppliedDecal d) => !d.decal.isPermanent) > 0)
			{
				this.RebuildDecalMeshes();
			}
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x00171950 File Offset: 0x0016FB50
		public void ClearAllDecals()
		{
			this.appliedDecals.Clear();
			this.RebuildDecalMeshes();
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x00171964 File Offset: 0x0016FB64
		public void DeactivateAllDecals()
		{
			bool flag = false;
			for (int i = 0; i < this.appliedDecals.Count; i++)
			{
				if (this.appliedDecals[i].isActive)
				{
					this.appliedDecals[i].isActive = false;
					flag = true;
				}
			}
			if (flag)
			{
				this.RebuildDecalMeshes();
			}
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x001719BC File Offset: 0x0016FBBC
		public void ResetToStartupDefaults()
		{
			this.CaptureStartupDecalState();
			HashSet<SkinDecal> hashSet = new HashSet<SkinDecal>();
			for (int i = 0; i < this._startupDecalStates.Count; i++)
			{
				SkinDecal decal = this._startupDecalStates[i].decal;
				if (decal != null)
				{
					hashSet.Add(decal);
				}
			}
			List<AppliedDecal> list = new List<AppliedDecal>();
			for (int j = 0; j < this.appliedDecals.Count; j++)
			{
				AppliedDecal appliedDecal = this.appliedDecals[j];
				if (appliedDecal != null && !(appliedDecal.decal == null) && !hashSet.Contains(appliedDecal.decal))
				{
					appliedDecal.isActive = false;
					list.Add(appliedDecal);
				}
			}
			this.appliedDecals.Clear();
			for (int k = 0; k < this._startupDecalStates.Count; k++)
			{
				SkinDecalController.StartupDecalState startupDecalState = this._startupDecalStates[k];
				if (!(startupDecalState.decal == null))
				{
					this.appliedDecals.Add(new AppliedDecal
					{
						decal = startupDecalState.decal,
						tint = startupDecalState.tint,
						opacity = startupDecalState.opacity,
						isActive = startupDecalState.isActive
					});
				}
			}
			this.appliedDecals.AddRange(list);
			this.NormalizeActiveTans(null);
			this.RebuildDecalMeshes();
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x00171B10 File Offset: 0x0016FD10
		public bool HasDecal(SkinDecal decal)
		{
			return this.appliedDecals.Any<AppliedDecal>((AppliedDecal d) => d.decal == decal);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x00171B44 File Offset: 0x0016FD44
		public bool IsDecalActive(SkinDecal decal)
		{
			AppliedDecal appliedDecal = this.appliedDecals.Find((AppliedDecal d) => d.decal == decal);
			return appliedDecal != null && appliedDecal.isActive;
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x00171B80 File Offset: 0x0016FD80
		public bool HasActiveDecalOfType(SkinDecalType type, float minimumOpacity = 0.01f)
		{
			float num = Mathf.Clamp01(minimumOpacity);
			for (int i = 0; i < this.appliedDecals.Count; i++)
			{
				AppliedDecal appliedDecal = this.appliedDecals[i];
				if (appliedDecal != null && !(appliedDecal.decal == null) && appliedDecal.isActive && appliedDecal.opacity > num && appliedDecal.decal.decalType == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00171BEC File Offset: 0x0016FDEC
		public bool TryGetDecalState(SkinDecal decal, out bool active, out float opacity, out Color tint)
		{
			AppliedDecal appliedDecal = this.appliedDecals.Find((AppliedDecal d) => d.decal == decal);
			if (appliedDecal == null)
			{
				active = false;
				opacity = 0f;
				tint = Color.white;
				return false;
			}
			active = appliedDecal.isActive;
			opacity = appliedDecal.opacity;
			tint = appliedDecal.tint;
			return true;
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x00171C58 File Offset: 0x0016FE58
		public bool TrySetDecalStateByName(string decalName, bool active, float opacity, Color tint)
		{
			if (string.IsNullOrWhiteSpace(decalName))
			{
				return false;
			}
			AppliedDecal applied = this.appliedDecals.Find((AppliedDecal d) => d != null && d.decal != null && string.Equals(d.decal.name, decalName, StringComparison.OrdinalIgnoreCase));
			if (applied == null)
			{
				return false;
			}
			active &= !applied.decal.gameplayOnly;
			applied.tint = tint;
			applied.opacity = Mathf.Clamp01(opacity);
			bool flag = applied.isActive != active;
			applied.isActive = active;
			bool flag2 = false;
			if (active && applied.decal.decalType == SkinDecalType.Tan)
			{
				flag2 = this.appliedDecals.Any<AppliedDecal>((AppliedDecal d) => d != applied && d != null && d.isActive && d.decal != null && d.decal.decalType == SkinDecalType.Tan);
				this.NormalizeActiveTans(applied);
			}
			if (flag || flag2)
			{
				this.RebuildDecalMeshes();
			}
			else
			{
				this.UpdateDecalMaterial(applied);
			}
			return true;
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x00171D4E File Offset: 0x0016FF4E
		public List<SkinDecal> GetAppliedDecals()
		{
			return this.appliedDecals.Select<AppliedDecal, SkinDecal>((AppliedDecal d) => d.decal).ToList<SkinDecal>();
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x00171D80 File Offset: 0x0016FF80
		public List<SkinDecal> GetDecalsByType(SkinDecalType type)
		{
			return (from d in this.appliedDecals
				where d.decal.decalType == type
				select d.decal).ToList<SkinDecal>();
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00171DDA File Offset: 0x0016FFDA
		public int GetDecalCount()
		{
			return this.appliedDecals.Count;
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x00171DE8 File Offset: 0x0016FFE8
		public bool HasRainWashableDecals()
		{
			for (int i = 0; i < this.appliedDecals.Count; i++)
			{
				AppliedDecal appliedDecal = this.appliedDecals[i];
				if (appliedDecal != null && !(appliedDecal.decal == null) && appliedDecal.isActive && appliedDecal.opacity > 0.001f && appliedDecal.decal.IsWashableByRain)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x00171E4E File Offset: 0x0017004E
		public void SetRainFade(float amount)
		{
			this._rainFadeAmount = Mathf.Clamp01(amount);
			this.ApplyRainFadeToInstances(this._activeBodyMeshes);
			this.ApplyRainFadeToInstances(this._activeHeadMeshes);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x00171E74 File Offset: 0x00170074
		public int RemoveRainWashableDecals()
		{
			int num = this.appliedDecals.RemoveAll((AppliedDecal applied) => applied != null && applied.isActive && applied.decal != null && applied.decal.IsWashableByRain);
			this._rainFadeAmount = 0f;
			if (num > 0)
			{
				this.RebuildDecalMeshes();
			}
			return num;
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00171EC0 File Offset: 0x001700C0
		[ContextMenu("Rebuild Decal Meshes")]
		public void RebuildDecalMeshes()
		{
			if (this._isDestroying)
			{
				return;
			}
			this.ClearAllDecalMeshes();
			List<AppliedDecal> list = (from d in this.appliedDecals
				where d.isActive
				orderby d.decal.GetSortOrder(), this.appliedDecals.IndexOf(d)
				select d).ToList<AppliedDecal>();
			int num = 0;
			int num2 = 0;
			foreach (AppliedDecal appliedDecal in list)
			{
				if (appliedDecal.decal.HasBodyTexture && this.bodyRenderer != null)
				{
					this.CreateDecalMesh(appliedDecal, this.bodyRenderer, this._bodyDecalContainer, this._activeBodyMeshes, num++, true);
				}
				if (appliedDecal.decal.HasHeadTexture && this.headRenderer != null)
				{
					this.CreateDecalMesh(appliedDecal, this.headRenderer, this._headDecalContainer, this._activeHeadMeshes, num2++, false);
				}
			}
			Action onDecalsChanged = this.OnDecalsChanged;
			if (onDecalsChanged == null)
			{
				return;
			}
			onDecalsChanged();
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00172004 File Offset: 0x00170204
		private void CreateDecalMesh(AppliedDecal applied, SkinnedMeshRenderer sourceRenderer, Transform container, List<DecalMeshInstance> meshList, int index, bool isBody)
		{
			if (container == null || sourceRenderer == null || sourceRenderer.sharedMesh == null)
			{
				return;
			}
			GameObject gameObject = new GameObject(string.Format("Decal_{0}_{1}", applied.decal.name, index));
			gameObject.transform.SetParent(container, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			if (isBody && this.bodyDecalLayer.value != 0)
			{
				gameObject.layer = SkinDecalController.GetLayerFromMask(this.bodyDecalLayer);
			}
			else if (!isBody && this.headDecalLayer.value != 0)
			{
				gameObject.layer = SkinDecalController.GetLayerFromMask(this.headDecalLayer);
			}
			SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.sharedMesh = sourceRenderer.sharedMesh;
			skinnedMeshRenderer.bones = sourceRenderer.bones;
			skinnedMeshRenderer.rootBone = sourceRenderer.rootBone;
			skinnedMeshRenderer.localBounds = sourceRenderer.localBounds;
			skinnedMeshRenderer.updateWhenOffscreen = sourceRenderer.updateWhenOffscreen;
			skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			int num = 2001 + index;
			Material material = this.CreateDecalMaterial(applied, isBody, num, sourceRenderer.sharedMesh);
			skinnedMeshRenderer.material = material;
			gameObject.AddComponent<BlendShapeSynchronizer>().SetSourceRenderer(sourceRenderer);
			meshList.Add(new DecalMeshInstance
			{
				applied = applied,
				gameObject = gameObject,
				renderer = skinnedMeshRenderer,
				material = material
			});
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00172178 File Offset: 0x00170378
		public bool TryGetDecalSource(Renderer renderer, out SkinnedMeshRenderer source)
		{
			for (int i = 0; i < this._activeBodyMeshes.Count; i++)
			{
				if (this._activeBodyMeshes[i].renderer == renderer)
				{
					source = this.bodyRenderer;
					return true;
				}
			}
			for (int j = 0; j < this._activeHeadMeshes.Count; j++)
			{
				if (this._activeHeadMeshes[j].renderer == renderer)
				{
					source = this.headRenderer;
					return true;
				}
			}
			source = null;
			return false;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x001721FC File Offset: 0x001703FC
		public void SynchronizeMeshesForCapture()
		{
			foreach (DecalMeshInstance decalMeshInstance in this._activeBodyMeshes)
			{
				if (decalMeshInstance.gameObject != null)
				{
					BlendShapeSynchronizer component = decalMeshInstance.gameObject.GetComponent<BlendShapeSynchronizer>();
					if (component != null)
					{
						component.SynchronizeNow();
					}
				}
			}
			foreach (DecalMeshInstance decalMeshInstance2 in this._activeHeadMeshes)
			{
				if (decalMeshInstance2.gameObject != null)
				{
					BlendShapeSynchronizer component2 = decalMeshInstance2.gameObject.GetComponent<BlendShapeSynchronizer>();
					if (component2 != null)
					{
						component2.SynchronizeNow();
					}
				}
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x001722CC File Offset: 0x001704CC
		private Material CreateDecalMaterial(AppliedDecal applied, bool isBody, int renderQueue, Mesh sourceMesh)
		{
			if (this.decalBaseMaterial == null)
			{
				Debug.LogError("[SkinDecalController] decalBaseMaterial non assegnato!");
				return null;
			}
			Material material = new Material(this.decalBaseMaterial);
			material.name = "DecalMat_" + applied.decal.name;
			material.renderQueue = renderQueue;
			if (material.HasProperty("_ZWrite"))
			{
				material.SetFloat("_ZWrite", 0f);
			}
			if (material.HasProperty("_OffsetFactor"))
			{
				material.SetFloat("_OffsetFactor", 0f);
			}
			if (material.HasProperty("_OffsetUnits"))
			{
				material.SetFloat("_OffsetUnits", -1f);
			}
			if (applied.decal.minimumLight > 0f && material.HasProperty("_LightMinLimit"))
			{
				float num = Mathf.Max(material.GetFloat("_LightMinLimit"), Mathf.Clamp01(applied.decal.minimumLight));
				material.SetFloat("_LightMinLimit", num);
				if (material.HasProperty("_LightMaxLimit"))
				{
					material.SetFloat("_LightMaxLimit", Mathf.Max(material.GetFloat("_LightMaxLimit"), num));
				}
			}
			Texture2D texture2D = (isBody ? applied.decal.bodyTexture : applied.decal.headTexture);
			int num2 = (isBody ? applied.decal.bodyUvSeamPaddingPixels : applied.decal.headUvSeamPaddingPixels);
			Texture texture = this.GetOrCreateSeamSafeTexture(texture2D, sourceMesh, num2, isBody && applied.decal.bodyUvSeamGutterOnly);
			if (!isBody && applied.decal.headLipToneReference != null)
			{
				texture = this.GetOrCreateLipToneTexture(texture, applied.decal.headLipToneReference);
			}
			material.SetTexture("_MainTex", texture);
			if (material.HasProperty("_Color"))
			{
				Color displayedDecalColor = this.GetDisplayedDecalColor(applied);
				material.SetColor("_Color", displayedDecalColor);
			}
			if (material.HasProperty("_TransparentMode"))
			{
				material.SetFloat("_TransparentMode", 1f);
			}
			if (material.HasProperty("_Cutoff"))
			{
				material.SetFloat("_Cutoff", 0.001f);
			}
			return material;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x001724D8 File Offset: 0x001706D8
		private Texture GetOrCreateSeamSafeTexture(Texture2D sourceTexture, Mesh sourceMesh, int requestedPaddingPixels, bool gutterOnly = false)
		{
			int num = Mathf.Clamp(requestedPaddingPixels, 0, 16);
			if (sourceTexture == null || sourceMesh == null || num == 0)
			{
				return sourceTexture;
			}
			SkinDecalController.SeamTextureCacheKey seamTextureCacheKey = new SkinDecalController.SeamTextureCacheKey(sourceTexture.GetInstanceID(), sourceMesh.GetInstanceID(), gutterOnly ? (-num) : num);
			RenderTexture renderTexture;
			if (this._seamTextureCache.TryGetValue(seamTextureCacheKey, out renderTexture) && renderTexture != null)
			{
				return renderTexture;
			}
			if (!this.EnsureSeamPaddingMaterials())
			{
				return sourceTexture;
			}
			RenderTextureFormat renderTextureFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
			RenderTexture temporary = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height, 0, renderTextureFormat, RenderTextureReadWrite.Linear);
			temporary.name = "SkinDecalUvMask_" + sourceMesh.name;
			temporary.filterMode = FilterMode.Point;
			temporary.wrapMode = TextureWrapMode.Clamp;
			RenderTexture renderTexture2 = null;
			RenderTexture renderTexture3 = null;
			CommandBuffer commandBuffer = new CommandBuffer
			{
				name = "SkinDecal UV seam mask"
			};
			Texture texture;
			try
			{
				commandBuffer.SetRenderTarget(temporary);
				commandBuffer.ClearRenderTarget(false, true, Color.clear);
				for (int i = 0; i < sourceMesh.subMeshCount; i++)
				{
					commandBuffer.DrawMesh(sourceMesh, Matrix4x4.identity, this._uvMaskMaterial, i, 0);
				}
				Graphics.ExecuteCommandBuffer(commandBuffer);
				renderTexture3 = new RenderTexture(sourceTexture.width, sourceTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
				{
					name = string.Format("SeamSafe_{0}_{1}px", sourceTexture.name, num),
					useMipMap = false,
					autoGenerateMips = false,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					hideFlags = HideFlags.HideAndDontSave
				};
				if (!renderTexture3.Create())
				{
					SkinDecalController.DestroyObjectSafe(renderTexture3);
					texture = sourceTexture;
				}
				else
				{
					this._seamPaddingMaterial.SetTexture("_MaskTex", temporary);
					this._seamPaddingMaterial.SetFloat("_PaddingPixels", (float)num);
					if (gutterOnly)
					{
						Graphics.Blit(sourceTexture, renderTexture3, this._seamPaddingMaterial, 1);
						this._seamTextureCache.Add(seamTextureCacheKey, renderTexture3);
						texture = renderTexture3;
					}
					else
					{
						renderTexture2 = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
						renderTexture2.name = "SkinDecalInteriorRepair_" + sourceTexture.name;
						renderTexture2.filterMode = FilterMode.Bilinear;
						renderTexture2.wrapMode = TextureWrapMode.Clamp;
						Graphics.Blit(sourceTexture, renderTexture2, this._seamPaddingMaterial, 0);
						Graphics.Blit(renderTexture2, renderTexture3, this._seamPaddingMaterial, 1);
						this._seamTextureCache.Add(seamTextureCacheKey, renderTexture3);
						texture = renderTexture3;
					}
				}
			}
			catch (Exception ex)
			{
				if (renderTexture3 != null)
				{
					renderTexture3.Release();
					SkinDecalController.DestroyObjectSafe(renderTexture3);
				}
				Debug.LogWarning("[SkinDecalController] Impossibile generare il padding UV per '" + sourceTexture.name + "'. Uso la texture originale.\n" + ex.Message, this);
				texture = sourceTexture;
			}
			finally
			{
				commandBuffer.Release();
				if (renderTexture2 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture2);
				}
				RenderTexture.ReleaseTemporary(temporary);
			}
			return texture;
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x001727BC File Offset: 0x001709BC
		private Texture GetOrCreateLipToneTexture(Texture source, Texture2D reference)
		{
			if (source == null || reference == null)
			{
				return source;
			}
			SkinDecalController.SeamTextureCacheKey seamTextureCacheKey = new SkinDecalController.SeamTextureCacheKey(source.GetInstanceID(), reference.GetInstanceID(), -1);
			RenderTexture renderTexture;
			if (this._seamTextureCache.TryGetValue(seamTextureCacheKey, out renderTexture) && renderTexture != null)
			{
				return renderTexture;
			}
			if (!this.EnsureSeamPaddingMaterials())
			{
				return source;
			}
			RenderTexture renderTexture2 = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
			{
				name = "LipToneSafe_" + source.name,
				useMipMap = true,
				autoGenerateMips = false,
				filterMode = FilterMode.Trilinear,
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.HideAndDontSave
			};
			if (!renderTexture2.Create())
			{
				SkinDecalController.DestroyObjectSafe(renderTexture2);
				return source;
			}
			this._seamPaddingMaterial.SetTexture("_ReferenceTex", reference);
			Graphics.Blit(source, renderTexture2, this._seamPaddingMaterial, 2);
			renderTexture2.GenerateMips();
			this._seamTextureCache.Add(seamTextureCacheKey, renderTexture2);
			return renderTexture2;
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x001728A8 File Offset: 0x00170AA8
		private bool EnsureSeamPaddingMaterials()
		{
			if (!this.EnsureUvMaskMaterial())
			{
				return false;
			}
			if (this._seamPaddingMaterial != null)
			{
				return true;
			}
			Shader shader = Shader.Find("Hidden/ProjectExhibitionist/SkinDecalSeamPadding");
			if (shader == null || !shader.isSupported)
			{
				if (!this._seamShaderWarningLogged)
				{
					Debug.LogWarning("[SkinDecalController] Shader di padding UV non disponibili o non supportati. I decal continueranno a usare le texture originali.", this);
					this._seamShaderWarningLogged = true;
				}
				return false;
			}
			this._seamPaddingMaterial = new Material(shader)
			{
				name = "SkinDecalSeamPadding (Runtime)",
				hideFlags = HideFlags.HideAndDontSave
			};
			return true;
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x00172928 File Offset: 0x00170B28
		private bool EnsureUvMaskMaterial()
		{
			if (this._uvMaskMaterial != null)
			{
				return true;
			}
			Shader shader = Shader.Find("Hidden/ProjectExhibitionist/SkinDecalUvMask");
			if (shader == null || !shader.isSupported)
			{
				if (!this._seamShaderWarningLogged)
				{
					Debug.LogWarning("[SkinDecalController] Shader 'Hidden/ProjectExhibitionist/SkinDecalUvMask' non disponibile. Il padding UV sara' disabilitato.", this);
					this._seamShaderWarningLogged = true;
				}
				return false;
			}
			this._uvMaskMaterial = new Material(shader)
			{
				name = "SkinDecalUvMask (Runtime)",
				hideFlags = HideFlags.HideAndDontSave
			};
			return true;
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x001729A0 File Offset: 0x00170BA0
		private void ReleaseSeamPaddingResources()
		{
			foreach (RenderTexture renderTexture in this._seamTextureCache.Values)
			{
				if (!(renderTexture == null))
				{
					renderTexture.Release();
					SkinDecalController.DestroyObjectSafe(renderTexture);
				}
			}
			this._seamTextureCache.Clear();
			SkinDecalController.DestroyObjectSafe(this._uvMaskMaterial);
			SkinDecalController.DestroyObjectSafe(this._seamPaddingMaterial);
			this._uvMaskMaterial = null;
			this._seamPaddingMaterial = null;
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x00172A38 File Offset: 0x00170C38
		private void UpdateDecalMaterial(AppliedDecal applied)
		{
			Color displayedDecalColor = this.GetDisplayedDecalColor(applied);
			DecalMeshInstance decalMeshInstance = this._activeBodyMeshes.Find((DecalMeshInstance m) => m.applied == applied);
			if (decalMeshInstance != null)
			{
				SkinDecalController.ApplyColorToInstance(decalMeshInstance, displayedDecalColor);
			}
			DecalMeshInstance decalMeshInstance2 = this._activeHeadMeshes.Find((DecalMeshInstance m) => m.applied == applied);
			if (decalMeshInstance2 != null)
			{
				SkinDecalController.ApplyColorToInstance(decalMeshInstance2, displayedDecalColor);
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00172AA4 File Offset: 0x00170CA4
		private static void ApplyColorToInstance(DecalMeshInstance instance, Color color)
		{
			Material material = ((instance.renderer != null) ? instance.renderer.sharedMaterial : null);
			if (material == null)
			{
				material = instance.material;
			}
			if (material == null)
			{
				return;
			}
			SkinDecalController.SetMaterialColor(material, color);
			if (instance.material != null && instance.material != material)
			{
				SkinDecalController.SetMaterialColor(instance.material, color);
			}
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x00172B17 File Offset: 0x00170D17
		private static void SetMaterialColor(Material material, Color color)
		{
			if (material.HasProperty("_Color"))
			{
				material.SetColor("_Color", color);
				return;
			}
			material.color = color;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x00172B3C File Offset: 0x00170D3C
		private Color GetDisplayedDecalColor(AppliedDecal applied)
		{
			Color tint = applied.tint;
			float num = ((applied.decal != null && applied.decal.IsWashableByRain) ? (1f - this._rainFadeAmount) : 1f);
			tint.a = applied.opacity * num;
			return tint;
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x00172B90 File Offset: 0x00170D90
		private void ApplyRainFadeToInstances(List<DecalMeshInstance> instances)
		{
			for (int i = 0; i < instances.Count; i++)
			{
				DecalMeshInstance decalMeshInstance = instances[i];
				if (decalMeshInstance != null && decalMeshInstance.applied != null && !(decalMeshInstance.applied.decal == null) && decalMeshInstance.applied.decal.IsWashableByRain)
				{
					SkinDecalController.ApplyColorToInstance(decalMeshInstance, this.GetDisplayedDecalColor(decalMeshInstance.applied));
				}
			}
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00172BF8 File Offset: 0x00170DF8
		private void ClearAllDecalMeshes()
		{
			foreach (DecalMeshInstance decalMeshInstance in this._activeBodyMeshes)
			{
				this.DestroyDecalMeshInstance(decalMeshInstance);
			}
			this._activeBodyMeshes.Clear();
			foreach (DecalMeshInstance decalMeshInstance2 in this._activeHeadMeshes)
			{
				this.DestroyDecalMeshInstance(decalMeshInstance2);
			}
			this._activeHeadMeshes.Clear();
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00172CA4 File Offset: 0x00170EA4
		private void DestroyDecalMeshInstance(DecalMeshInstance instance)
		{
			if (instance.renderer != null)
			{
				instance.renderer.enabled = false;
			}
			if (instance.gameObject != null)
			{
				instance.gameObject.SetActive(false);
			}
			Material material = ((instance.renderer != null) ? instance.renderer.sharedMaterial : null);
			if (material != null && material != instance.material)
			{
				SkinDecalController.DestroyObjectSafe(material);
			}
			if (instance.material != null)
			{
				SkinDecalController.DestroyObjectSafe(instance.material);
			}
			if (instance.gameObject != null)
			{
				SkinDecalController.DestroyObjectSafe(instance.gameObject);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyObjectSafe(Object target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00172D54 File Offset: 0x00170F54
		public SkinDecalSaveData GetSaveData(bool forCustomization = false)
		{
			SkinDecalSaveData skinDecalSaveData = new SkinDecalSaveData();
			foreach (AppliedDecal appliedDecal in this.appliedDecals)
			{
				if (appliedDecal != null && !(appliedDecal.decal == null) && (!forCustomization || !appliedDecal.decal.gameplayOnly))
				{
					skinDecalSaveData.entries.Add(new SkinDecalSaveData.Entry
					{
						decalName = appliedDecal.decal.name,
						tint = appliedDecal.tint,
						opacity = appliedDecal.opacity,
						isActive = appliedDecal.isActive
					});
				}
			}
			return skinDecalSaveData;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00172E10 File Offset: 0x00171010
		public void LoadSaveData(SkinDecalSaveData data, List<SkinDecal> availableDecals)
		{
			if (data == null)
			{
				return;
			}
			this.appliedDecals.Clear();
			using (List<SkinDecalSaveData.Entry>.Enumerator enumerator = data.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkinDecalSaveData.Entry entry = enumerator.Current;
					SkinDecal skinDecal = availableDecals.Find((SkinDecal d) => d.name == entry.decalName);
					if (skinDecal != null)
					{
						this.appliedDecals.Add(new AppliedDecal
						{
							decal = skinDecal,
							tint = entry.tint,
							opacity = entry.opacity,
							isActive = entry.isActive
						});
					}
				}
			}
			this.NormalizeActiveTans(null);
			this.RebuildDecalMeshes();
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00172EEC File Offset: 0x001710EC
		private void CaptureStartupDecalState()
		{
			if (this._startupDecalStateCaptured)
			{
				return;
			}
			this._startupDecalStates.Clear();
			for (int i = 0; i < this.appliedDecals.Count; i++)
			{
				AppliedDecal appliedDecal = this.appliedDecals[i];
				if (appliedDecal != null && !(appliedDecal.decal == null))
				{
					this._startupDecalStates.Add(new SkinDecalController.StartupDecalState
					{
						decal = appliedDecal.decal,
						tint = appliedDecal.tint,
						opacity = appliedDecal.opacity,
						isActive = appliedDecal.isActive
					});
				}
			}
			this._startupDecalStateCaptured = true;
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00172F90 File Offset: 0x00171190
		private static int GetLayerFromMask(LayerMask mask)
		{
			int value = mask.value;
			if (value == 0)
			{
				return 0;
			}
			int num = 0;
			while (value >> num != 1)
			{
				num++;
			}
			return num;
		}

		// Token: 0x04003E55 RID: 15957
		[Header("Mesh Targets")]
		[Tooltip("SkinnedMeshRenderer del corpo")]
		public SkinnedMeshRenderer bodyRenderer;

		// Token: 0x04003E56 RID: 15958
		[Tooltip("SkinnedMeshRenderer della testa")]
		public SkinnedMeshRenderer headRenderer;

		// Token: 0x04003E57 RID: 15959
		[Header("Decal Material")]
		[Tooltip("Materiale base per i decal (deve essere trasparente)")]
		public Material decalBaseMaterial;

		// Token: 0x04003E58 RID: 15960
		[Header("Rendering")]
		[Tooltip("Offset Z tra layer per evitare z-fighting (in unità locali)")]
		public float zOffset = 0.0001f;

		// Token: 0x04003E59 RID: 15961
		[Tooltip("Layer da assegnare ai GameObject dei decal del corpo")]
		public LayerMask bodyDecalLayer;

		// Token: 0x04003E5A RID: 15962
		[Tooltip("Layer da assegnare ai GameObject dei decal della testa (es. PlayerHead)")]
		public LayerMask headDecalLayer;

		// Token: 0x04003E5B RID: 15963
		[Header("Applied Decals")]
		[SerializeField]
		private List<AppliedDecal> appliedDecals = new List<AppliedDecal>();

		// Token: 0x04003E5C RID: 15964
		private Transform _bodyDecalContainer;

		// Token: 0x04003E5D RID: 15965
		private Transform _headDecalContainer;

		// Token: 0x04003E5E RID: 15966
		private List<DecalMeshInstance> _activeBodyMeshes = new List<DecalMeshInstance>();

		// Token: 0x04003E5F RID: 15967
		private List<DecalMeshInstance> _activeHeadMeshes = new List<DecalMeshInstance>();

		// Token: 0x04003E60 RID: 15968
		private readonly List<SkinDecalController.StartupDecalState> _startupDecalStates = new List<SkinDecalController.StartupDecalState>();

		// Token: 0x04003E61 RID: 15969
		private bool _startupDecalStateCaptured;

		// Token: 0x04003E62 RID: 15970
		private Color _currentSkinTone = Color.white;

		// Token: 0x04003E63 RID: 15971
		private const int MaximumUvSeamPaddingPixels = 16;

		// Token: 0x04003E64 RID: 15972
		private const string UvMaskShaderName = "Hidden/ProjectExhibitionist/SkinDecalUvMask";

		// Token: 0x04003E65 RID: 15973
		private const string SeamPaddingShaderName = "Hidden/ProjectExhibitionist/SkinDecalSeamPadding";

		// Token: 0x04003E66 RID: 15974
		private readonly Dictionary<SkinDecalController.SeamTextureCacheKey, RenderTexture> _seamTextureCache = new Dictionary<SkinDecalController.SeamTextureCacheKey, RenderTexture>();

		// Token: 0x04003E67 RID: 15975
		private Material _uvMaskMaterial;

		// Token: 0x04003E68 RID: 15976
		private Material _seamPaddingMaterial;

		// Token: 0x04003E69 RID: 15977
		private bool _seamShaderWarningLogged;

		// Token: 0x04003E6A RID: 15978
		private float _rainFadeAmount;

		// Token: 0x04003E6B RID: 15979
		private bool _isDestroying;

		// Token: 0x020008F4 RID: 2292
		private struct StartupDecalState
		{
			// Token: 0x04003E6D RID: 15981
			public SkinDecal decal;

			// Token: 0x04003E6E RID: 15982
			public Color tint;

			// Token: 0x04003E6F RID: 15983
			public float opacity;

			// Token: 0x04003E70 RID: 15984
			public bool isActive;
		}

		// Token: 0x020008F5 RID: 2293
		private readonly struct SeamTextureCacheKey : IEquatable<SkinDecalController.SeamTextureCacheKey>
		{
			// Token: 0x06003E4F RID: 15951 RVA: 0x0017302A File Offset: 0x0017122A
			public SeamTextureCacheKey(int textureId, int meshId, int paddingPixels)
			{
				this._textureId = textureId;
				this._meshId = meshId;
				this._paddingPixels = paddingPixels;
			}

			// Token: 0x06003E50 RID: 15952 RVA: 0x00173041 File Offset: 0x00171241
			public bool Equals(SkinDecalController.SeamTextureCacheKey other)
			{
				return this._textureId == other._textureId && this._meshId == other._meshId && this._paddingPixels == other._paddingPixels;
			}

			// Token: 0x06003E51 RID: 15953 RVA: 0x00173070 File Offset: 0x00171270
			public override bool Equals(object obj)
			{
				if (obj is SkinDecalController.SeamTextureCacheKey)
				{
					SkinDecalController.SeamTextureCacheKey seamTextureCacheKey = (SkinDecalController.SeamTextureCacheKey)obj;
					return this.Equals(seamTextureCacheKey);
				}
				return false;
			}

			// Token: 0x06003E52 RID: 15954 RVA: 0x00173095 File Offset: 0x00171295
			public override int GetHashCode()
			{
				return (((this._textureId * 397) ^ this._meshId) * 397) ^ this._paddingPixels;
			}

			// Token: 0x04003E71 RID: 15985
			private readonly int _textureId;

			// Token: 0x04003E72 RID: 15986
			private readonly int _meshId;

			// Token: 0x04003E73 RID: 15987
			private readonly int _paddingPixels;
		}
	}
}
