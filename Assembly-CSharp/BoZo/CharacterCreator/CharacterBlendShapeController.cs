using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using ProjectExhibitionist.Futanari;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007AB RID: 1963
	public class CharacterBlendShapeController : MonoBehaviour
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x00125B28 File Offset: 0x00123D28
		public BlendShapeMapSO BlendShapeMap
		{
			get
			{
				return this.blendShapeMap;
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060033B9 RID: 13241 RVA: 0x00125B30 File Offset: 0x00123D30
		// (remove) Token: 0x060033BA RID: 13242 RVA: 0x00125B68 File Offset: 0x00123D68
		public event Action AuthoredValuesChanged;

		// Token: 0x060033BB RID: 13243 RVA: 0x00125BA0 File Offset: 0x00123DA0
		public void ResetMappedValuesToDefaults()
		{
			BlendShapeMapSO blendShapeMapSO = this.blendShapeMap;
			if (((blendShapeMapSO != null) ? blendShapeMapSO.blendShapeMappings : null) == null)
			{
				return;
			}
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeMap.blendShapeMappings)
			{
				if (blendShapeEntry != null)
				{
					this.ApplyEntryValue(blendShapeEntry, blendShapeEntry.GetUiDefaultValue());
				}
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x00125C18 File Offset: 0x00123E18
		private BlendShapeManager Manager
		{
			get
			{
				if (this._manager == null)
				{
					this._manager = BlendShapeManager.ResolveForRig(this);
				}
				return this._manager;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x00125C3A File Offset: 0x00123E3A
		private OutfitSystem OutfitSystem
		{
			get
			{
				if (!this._outfitSystemResolved)
				{
					this._outfitSystem = base.GetComponentInParent<OutfitSystem>(true);
					if (this._outfitSystem == null)
					{
						this._outfitSystem = base.GetComponentInChildren<OutfitSystem>(true);
					}
					this._outfitSystemResolved = true;
				}
				return this._outfitSystem;
			}
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x00125C79 File Offset: 0x00123E79
		private void Awake()
		{
			this._manager = BlendShapeManager.ResolveForRig(this);
			this.ValidateSetup();
			this.InitializeBlendShapeCache();
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x00125C93 File Offset: 0x00123E93
		private void LateUpdate()
		{
			if (!this.overrideAnimatorBlendShapes)
			{
				return;
			}
			if (this._suppressOverrides)
			{
				return;
			}
			this.ApplyOverriddenBlendShapes();
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00125CAD File Offset: 0x00123EAD
		public void SuppressBlendShapeOverrides()
		{
			this._suppressOverrides = true;
			if (this.Manager != null)
			{
				this.Manager.RemoveAllOverridesFromSource("CharacterBlendShape");
			}
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x00125CD4 File Offset: 0x00123ED4
		public void RestoreBlendShapeOverrides()
		{
			this._suppressOverrides = false;
			this.ApplyOverriddenBlendShapes();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x00125CE4 File Offset: 0x00123EE4
		private void ApplyOverriddenBlendShapes()
		{
			foreach (string text in this.overriddenBlendShapes)
			{
				float num;
				CharacterBlendShapeController.BlendShapeInfo blendShapeInfo;
				if (this.currentBlendShapeValues.TryGetValue(text, out num) && this.blendShapeCache.TryGetValue(text, out blendShapeInfo))
				{
					if (this.Manager != null)
					{
						this.Manager.SetOverride(text, num, "CharacterBlendShape", BlendShapePriority.Customization, false);
					}
					else if (blendShapeInfo.renderer != null)
					{
						blendShapeInfo.renderer.SetBlendShapeWeight(blendShapeInfo.index, num);
					}
				}
			}
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x00125D98 File Offset: 0x00123F98
		private void ValidateSetup()
		{
			if (this.blendShapeMap == null)
			{
				Debug.LogError("No BlendShapeMapSO assigned to CharacterBlendShapeController!");
				return;
			}
			if (this.headRenderer == null)
			{
				Debug.LogError("No head renderer assigned to CharacterBlendShapeController!");
			}
			if (this.bodyRenderer == null)
			{
				Debug.LogError("No body renderer assigned to CharacterBlendShapeController!");
			}
			if (this.blendShapeMap.blendShapeMappings == null || this.blendShapeMap.blendShapeMappings.Count == 0)
			{
				Debug.LogError("BlendShapeMapSO has no blend shape mappings!");
				return;
			}
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x00125E18 File Offset: 0x00124018
		private void InitializeBlendShapeCache()
		{
			this.blendShapeCache.Clear();
			this.currentBlendShapeValues.Clear();
			this.missingRendererWarnings.Clear();
			this.missingCacheWarnings.Clear();
			if (this.blendShapeMap == null)
			{
				Debug.LogError("No blend shape map assigned to CharacterBlendShapeController!");
				return;
			}
			if (this.headRenderer != null)
			{
				this.CacheBlendShapesForRenderer(this.headRenderer);
			}
			if (this.bodyRenderer != null)
			{
				this.CacheBlendShapesForRenderer(this.bodyRenderer);
			}
			this.AutoBindMissingBlendShapesFromChildren();
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeMap.blendShapeMappings)
			{
				this.ApplyEntryValue(blendShapeEntry, blendShapeEntry.GetUiDefaultValue());
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x00125EF8 File Offset: 0x001240F8
		private HashSet<string> BuildRequiredBlendShapeNameSet()
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (this.blendShapeMap == null || this.blendShapeMap.blendShapeMappings == null)
			{
				return hashSet;
			}
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in this.blendShapeMap.blendShapeMappings)
			{
				if (blendShapeEntry != null)
				{
					foreach (string text in blendShapeEntry.GetAllBlendShapeNames())
					{
						if (!string.IsNullOrWhiteSpace(text))
						{
							hashSet.Add(text);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x00125FC0 File Offset: 0x001241C0
		private void AutoBindMissingBlendShapesFromChildren()
		{
			HashSet<string> hashSet = this.BuildRequiredBlendShapeNameSet();
			if (hashSet.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (string text in hashSet)
			{
				CharacterBlendShapeController.BlendShapeInfo blendShapeInfo;
				if (!this.blendShapeCache.TryGetValue(text, out blendShapeInfo) || blendShapeInfo.renderer == null)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			int num = 0;
			int num2 = 0;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				if (!(skinnedMeshRenderer == null))
				{
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					if (!(sharedMesh == null) && sharedMesh.blendShapeCount != 0)
					{
						num2++;
						for (int j = 0; j < sharedMesh.blendShapeCount; j++)
						{
							string text2 = sharedMesh.GetBlendShapeName(j);
							if (text2.Contains(":"))
							{
								text2 = text2.Substring(text2.LastIndexOf(':') + 1);
							}
							CharacterBlendShapeController.BlendShapeInfo blendShapeInfo2;
							if (hashSet.Contains(text2) && (!this.blendShapeCache.TryGetValue(text2, out blendShapeInfo2) || !(blendShapeInfo2.renderer != null)))
							{
								this.blendShapeCache[text2] = new CharacterBlendShapeController.BlendShapeInfo
								{
									renderer = skinnedMeshRenderer,
									index = j
								};
								num++;
								if (!this.currentBlendShapeValues.ContainsKey(text2))
								{
									this.currentBlendShapeValues[text2] = 0f;
								}
							}
						}
					}
				}
			}
			if (num > 0 && this.Manager != null)
			{
				this.Manager.Rebuild();
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x00126184 File Offset: 0x00124384
		public void RebindRenderersAndRecache()
		{
			this.InitializeBlendShapeCache();
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0012618C File Offset: 0x0012438C
		private void EnsureBlendShapesCached(List<BlendShapeMapSO.BlendShapeEntry> entries)
		{
			if (entries == null || entries.Count == 0)
			{
				return;
			}
			bool flag = false;
			int num = 0;
			while (num < entries.Count && !flag)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = entries[num];
				if (blendShapeEntry != null)
				{
					foreach (string text in blendShapeEntry.GetAllBlendShapeNames())
					{
						CharacterBlendShapeController.BlendShapeInfo blendShapeInfo;
						if (!string.IsNullOrWhiteSpace(text) && (!this.blendShapeCache.TryGetValue(text, out blendShapeInfo) || blendShapeInfo.renderer == null))
						{
							flag = true;
							break;
						}
					}
				}
				num++;
			}
			if (flag)
			{
				this.AutoBindMissingBlendShapesFromChildren();
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0012623C File Offset: 0x0012443C
		private void CacheBlendShapesForRenderer(SkinnedMeshRenderer renderer)
		{
			Mesh sharedMesh = renderer.sharedMesh;
			if (sharedMesh == null)
			{
				Debug.LogError("No mesh found on " + renderer.name);
				return;
			}
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				string text = sharedMesh.GetBlendShapeName(i);
				if (text.Contains(":"))
				{
					text = text.Substring(text.LastIndexOf(':') + 1);
				}
				CharacterBlendShapeController.BlendShapeInfo blendShapeInfo = new CharacterBlendShapeController.BlendShapeInfo
				{
					renderer = renderer,
					index = i
				};
				this.blendShapeCache[text] = blendShapeInfo;
				this.currentBlendShapeValues[text] = 0f;
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x001262E0 File Offset: 0x001244E0
		public void SetCombinedBlendShapeValue(List<string> blendShapeNames, float value)
		{
			if (blendShapeNames == null || blendShapeNames.Count == 0)
			{
				Debug.LogWarning("No blend shape names provided for combined blend shape!");
				return;
			}
			foreach (string text in blendShapeNames)
			{
				if (!string.IsNullOrEmpty(text))
				{
					this.SetBlendShapeValue(text, value);
				}
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x00126350 File Offset: 0x00124550
		public void SetBlendShapeValue(string blendShapeName, float value)
		{
			CharacterBlendShapeController.BlendShapeInfo blendShapeInfo;
			if (this.blendShapeCache.TryGetValue(blendShapeName, out blendShapeInfo))
			{
				if (blendShapeInfo.renderer != null)
				{
					this.currentBlendShapeValues[blendShapeName] = value;
					if (blendShapeName.Contains("Expression"))
					{
						this.overriddenBlendShapes.Add(blendShapeName);
					}
					if (this.Manager != null)
					{
						this.Manager.SetOverride(blendShapeName, value, "CharacterBlendShape", BlendShapePriority.Customization, false);
					}
					else
					{
						blendShapeInfo.renderer.SetBlendShapeWeight(blendShapeInfo.index, value);
						if (Mathf.Abs(blendShapeInfo.renderer.GetBlendShapeWeight(blendShapeInfo.index) - value) > 0.01f)
						{
							Debug.LogWarning("Blend shape value mismatch! Forcing update...");
							blendShapeInfo.renderer.SetBlendShapeWeight(blendShapeInfo.index, value);
							blendShapeInfo.renderer.forceMatrixRecalculationPerRender = true;
						}
					}
					this.PropagateToOutfits(blendShapeName, value);
					return;
				}
				if (this.missingRendererWarnings.Add(blendShapeName))
				{
					Debug.LogWarning("Renderer for blend shape '" + blendShapeName + "' is null!");
					return;
				}
			}
			else if (this.missingCacheWarnings.Add(blendShapeName))
			{
				Debug.LogWarning("Blend shape '" + blendShapeName + "' not found in cache!");
			}
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x00126478 File Offset: 0x00124678
		private void PropagateToOutfits(string blendShapeName, float value)
		{
			OutfitSystem outfitSystem = this.OutfitSystem;
			if (outfitSystem == null || outfitSystem.Outfits == null || outfitSystem.Outfits.Count == 0)
			{
				return;
			}
			string text = CharacterBlendShapeController.CanonicalShapeKey(blendShapeName);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			foreach (KeyValuePair<OutfitType, Outfit> keyValuePair in outfitSystem.Outfits)
			{
				Outfit value2 = keyValuePair.Value;
				if (value2 != null)
				{
					value2.SetShape(text, value);
				}
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x00126518 File Offset: 0x00124718
		private static string CanonicalShapeKey(string rawName)
		{
			if (string.IsNullOrWhiteSpace(rawName))
			{
				return null;
			}
			string text = rawName;
			int num = text.LastIndexOfAny(new char[] { '.', ':' });
			if (num >= 0 && num < text.Length - 1)
			{
				text = text.Substring(num + 1);
			}
			int num2 = text.IndexOf('_');
			if (num2 >= 0 && num2 < text.Length - 1)
			{
				return text.Substring(num2 + 1);
			}
			return text;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x00126584 File Offset: 0x00124784
		public void VerifyBlendShapeValues()
		{
			foreach (KeyValuePair<string, CharacterBlendShapeController.BlendShapeInfo> keyValuePair in this.blendShapeCache)
			{
				string key = keyValuePair.Key;
				CharacterBlendShapeController.BlendShapeInfo value = keyValuePair.Value;
				if (value.renderer != null)
				{
					float num = 0f;
					this.currentBlendShapeValues.TryGetValue(key, out num);
					float blendShapeWeight = value.renderer.GetBlendShapeWeight(value.index);
					if (Mathf.Abs(blendShapeWeight - num) > 0.01f)
					{
						Debug.LogWarning(string.Format("Blend shape value mismatch for '{0}': Expected {1}, Actual {2}", key, num, blendShapeWeight));
					}
				}
			}
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0012664C File Offset: 0x0012484C
		public float GetBlendShapeValue(string blendShapeName)
		{
			float num;
			if (this.currentBlendShapeValues.TryGetValue(blendShapeName, out num))
			{
				return num;
			}
			return 0f;
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x00126670 File Offset: 0x00124870
		public float GetCombinedBlendShapeValue(List<string> blendShapeNames)
		{
			if (blendShapeNames == null || blendShapeNames.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			foreach (string text in blendShapeNames)
			{
				float num3;
				if (!string.IsNullOrEmpty(text) && this.currentBlendShapeValues.TryGetValue(text, out num3))
				{
					num += num3;
					num2++;
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00126700 File Offset: 0x00124900
		public void ApplyBlendShapesForSubcategory(string category, string subcategory, BlendShapeMapSO.BlendShapeType type, Dictionary<string, float> values)
		{
			if (this.blendShapeMap == null)
			{
				Debug.LogError("BlendShapeMap is not assigned!");
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapesForSubcategoryByType = this.blendShapeMap.GetBlendShapesForSubcategoryByType(category, subcategory, type);
			if (blendShapesForSubcategoryByType.Count == 0)
			{
				Debug.LogWarning(string.Format("No blend shapes found for {0}/{1} of type {2}", category, subcategory, type));
				return;
			}
			this.EnsureBlendShapesCached(blendShapesForSubcategoryByType);
			bool flag = false;
			foreach (BlendShapeMapSO.BlendShapeEntry blendShapeEntry in blendShapesForSubcategoryByType)
			{
				float num;
				if (values.TryGetValue(blendShapeEntry.displayName, out num))
				{
					this.ApplyEntryValue(blendShapeEntry, num);
					flag = true;
				}
			}
			if (flag)
			{
				if (this.headRenderer != null)
				{
					this.headRenderer.forceMatrixRecalculationPerRender = true;
					this.headRenderer.sharedMesh = this.headRenderer.sharedMesh;
				}
				if (this.bodyRenderer != null)
				{
					this.bodyRenderer.forceMatrixRecalculationPerRender = true;
					this.bodyRenderer.sharedMesh = this.bodyRenderer.sharedMesh;
				}
				this.VerifyBlendShapeValues();
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x00126820 File Offset: 0x00124A20
		public void ApplyEntryValue(BlendShapeMapSO.BlendShapeEntry blendShape, float uiValue)
		{
			this.ApplyEntryValueInternal(blendShape, uiValue);
			Action authoredValuesChanged = this.AuthoredValuesChanged;
			if (authoredValuesChanged == null)
			{
				return;
			}
			authoredValuesChanged();
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0012683C File Offset: 0x00124A3C
		private void ApplyEntryValueInternal(BlendShapeMapSO.BlendShapeEntry blendShape, float uiValue)
		{
			if (blendShape == null)
			{
				return;
			}
			if (blendShape.isFutanariToggle)
			{
				FutanariModeController componentInChildren = base.GetComponentInChildren<FutanariModeController>(true);
				if (componentInChildren != null)
				{
					componentInChildren.SetFutanariMode(uiValue >= 0.5f);
				}
				return;
			}
			if (blendShape.IsPairControl())
			{
				float num = Mathf.Clamp(uiValue, -1f, 1f);
				float num2 = ((num < 0f) ? (Mathf.Abs(num) * 100f) : 0f);
				float num3 = ((num > 0f) ? (num * 100f) : 0f);
				if (!string.IsNullOrWhiteSpace(blendShape.pairInBlendShapeName))
				{
					this.SetBlendShapeValue(blendShape.pairInBlendShapeName, num2);
				}
				if (!string.IsNullOrWhiteSpace(blendShape.pairOutBlendShapeName))
				{
					this.SetBlendShapeValue(blendShape.pairOutBlendShapeName, num3);
				}
				return;
			}
			if (blendShape.IsSelectControl())
			{
				this.ApplySelectValue(blendShape, uiValue);
				return;
			}
			float num4;
			if (blendShape.IsBooleanControl())
			{
				num4 = (((blendShape.invertBoolean ? ((uiValue >= 0.5f) ? 0f : 1f) : uiValue) >= 0.5f) ? blendShape.maxValue : blendShape.minValue);
			}
			else
			{
				float num5 = Mathf.InverseLerp(blendShape.minValue, blendShape.maxValue, uiValue);
				num4 = Mathf.Lerp(0f, 100f, num5);
			}
			if (blendShape.IsCombined())
			{
				this.SetCombinedBlendShapeValue(blendShape.combinedBlendShapeNames, num4);
				return;
			}
			this.SetBlendShapeValue(blendShape.blendShapeName, num4);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x00126994 File Offset: 0x00124B94
		private void ApplySelectValue(BlendShapeMapSO.BlendShapeEntry blendShape, float uiValue)
		{
			int num = Mathf.RoundToInt(uiValue);
			List<string> combinedBlendShapeNames = blendShape.combinedBlendShapeNames;
			if (combinedBlendShapeNames == null || combinedBlendShapeNames.Count == 0)
			{
				if (!string.IsNullOrWhiteSpace(blendShape.blendShapeName))
				{
					this.SetBlendShapeValue(blendShape.blendShapeName, (num == 0) ? 100f : 0f);
				}
				return;
			}
			num = Mathf.Clamp(num, 0, combinedBlendShapeNames.Count - 1);
			for (int i = 0; i < combinedBlendShapeNames.Count; i++)
			{
				if (!string.IsNullOrWhiteSpace(combinedBlendShapeNames[i]))
				{
					float num2 = ((i == num) ? 100f : 0f);
					this.SetBlendShapeValue(combinedBlendShapeNames[i], num2);
				}
			}
			this.ForceGeometryRefreshForSelect(blendShape);
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x00126A38 File Offset: 0x00124C38
		private void ForceGeometryRefreshForSelect(BlendShapeMapSO.BlendShapeEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			if (this.headRenderer != null)
			{
				this.headRenderer.forceMatrixRecalculationPerRender = true;
			}
			if (this.bodyRenderer != null)
			{
				this.bodyRenderer.forceMatrixRecalculationPerRender = true;
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x00126A72 File Offset: 0x00124C72
		public List<BlendShapeMapSO.BlendShapeEntry> GetAvailableBlendShapes(string category, string subcategory, BlendShapeMapSO.BlendShapeType type)
		{
			return this.blendShapeMap.GetBlendShapesForSubcategoryByType(category, subcategory, type);
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x00126A82 File Offset: 0x00124C82
		public void SetBlendShapeMap(BlendShapeMapSO newMap)
		{
			this.blendShapeMap = newMap;
			this.InitializeBlendShapeCache();
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x00126A94 File Offset: 0x00124C94
		private bool TryGetLiveWeight(string blendShapeName, out float weight)
		{
			weight = 0f;
			if (string.IsNullOrWhiteSpace(blendShapeName))
			{
				return false;
			}
			CharacterBlendShapeController.BlendShapeInfo blendShapeInfo;
			if (this.blendShapeCache.TryGetValue(blendShapeName, out blendShapeInfo) && blendShapeInfo.renderer != null && blendShapeInfo.index >= 0 && blendShapeInfo.renderer.sharedMesh != null && blendShapeInfo.index < blendShapeInfo.renderer.sharedMesh.blendShapeCount)
			{
				weight = blendShapeInfo.renderer.GetBlendShapeWeight(blendShapeInfo.index);
				return true;
			}
			return this.currentBlendShapeValues.TryGetValue(blendShapeName, out weight);
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x00126B26 File Offset: 0x00124D26
		public float GetEntryUiValue(BlendShapeMapSO.BlendShapeEntry entry)
		{
			return this.ReadEntryUiValue(entry, false);
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00126B30 File Offset: 0x00124D30
		public float GetEntryAuthoredUiValue(BlendShapeMapSO.BlendShapeEntry entry)
		{
			return this.ReadEntryUiValue(entry, true);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x00126B3A File Offset: 0x00124D3A
		private bool TryGetEntryWeight(string name, bool authored, out float value)
		{
			return (authored && this.Manager != null && this.Manager.TryGetAuthoredBlendShapeValue(name, out value)) || this.TryGetLiveWeight(name, out value);
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x00126B68 File Offset: 0x00124D68
		private float ReadEntryUiValue(BlendShapeMapSO.BlendShapeEntry entry, bool authored)
		{
			if (entry == null)
			{
				return 0f;
			}
			if (entry.isFutanariToggle)
			{
				FutanariModeController componentInChildren = base.GetComponentInChildren<FutanariModeController>(true);
				if (!(componentInChildren != null) || !componentInChildren.FutanariMode)
				{
					return 0f;
				}
				return 1f;
			}
			else
			{
				this.EnsureBlendShapesCached(new List<BlendShapeMapSO.BlendShapeEntry> { entry });
				if (entry.IsPairControl())
				{
					float num = 0f;
					float num2 = 0f;
					if (!string.IsNullOrWhiteSpace(entry.pairInBlendShapeName))
					{
						this.TryGetEntryWeight(entry.pairInBlendShapeName, authored, out num);
					}
					if (!string.IsNullOrWhiteSpace(entry.pairOutBlendShapeName))
					{
						this.TryGetEntryWeight(entry.pairOutBlendShapeName, authored, out num2);
					}
					if (num > 0f)
					{
						return -(num / 100f);
					}
					if (num2 > 0f)
					{
						return num2 / 100f;
					}
					return 0f;
				}
				else
				{
					if (entry.IsSelectControl())
					{
						List<string> combinedBlendShapeNames = entry.combinedBlendShapeNames;
						if (combinedBlendShapeNames != null)
						{
							for (int i = 0; i < combinedBlendShapeNames.Count; i++)
							{
								float num3;
								if (!string.IsNullOrWhiteSpace(combinedBlendShapeNames[i]) && this.TryGetEntryWeight(combinedBlendShapeNames[i], authored, out num3) && num3 > 50f)
								{
									return (float)i;
								}
							}
						}
						return entry.GetUiDefaultValue();
					}
					if (entry.IsBooleanControl())
					{
						string text = ((entry.IsCombined() && entry.combinedBlendShapeNames.Count > 0) ? entry.combinedBlendShapeNames[0] : entry.blendShapeName);
						float num4;
						if (string.IsNullOrWhiteSpace(text) || !this.TryGetEntryWeight(text, authored, out num4))
						{
							return entry.GetUiDefaultValue();
						}
						float num5 = ((num4 > 50f) ? 1f : 0f);
						if (!entry.invertBoolean)
						{
							return num5;
						}
						return 1f - num5;
					}
					else
					{
						string text2 = ((entry.IsCombined() && entry.combinedBlendShapeNames.Count > 0) ? entry.combinedBlendShapeNames[0] : entry.blendShapeName);
						float num6;
						if (!string.IsNullOrWhiteSpace(text2) && this.TryGetEntryWeight(text2, authored, out num6))
						{
							float num7 = Mathf.InverseLerp(0f, 100f, num6);
							return Mathf.Lerp(entry.minValue, entry.maxValue, num7);
						}
						return entry.GetUiDefaultValue();
					}
				}
			}
		}

		// Token: 0x04003459 RID: 13401
		[Tooltip("Reference to the blend shape mapping for this character")]
		[SerializeField]
		private BlendShapeMapSO blendShapeMap;

		// Token: 0x0400345B RID: 13403
		[Tooltip("The SkinnedMeshRenderer for the character's head")]
		[SerializeField]
		private SkinnedMeshRenderer headRenderer;

		// Token: 0x0400345C RID: 13404
		[Tooltip("The SkinnedMeshRenderer for the character's body")]
		[SerializeField]
		private SkinnedMeshRenderer bodyRenderer;

		// Token: 0x0400345D RID: 13405
		private Dictionary<string, CharacterBlendShapeController.BlendShapeInfo> blendShapeCache = new Dictionary<string, CharacterBlendShapeController.BlendShapeInfo>();

		// Token: 0x0400345E RID: 13406
		private Dictionary<string, float> currentBlendShapeValues = new Dictionary<string, float>();

		// Token: 0x0400345F RID: 13407
		private HashSet<string> overriddenBlendShapes = new HashSet<string>();

		// Token: 0x04003460 RID: 13408
		private readonly HashSet<string> missingRendererWarnings = new HashSet<string>();

		// Token: 0x04003461 RID: 13409
		private readonly HashSet<string> missingCacheWarnings = new HashSet<string>();

		// Token: 0x04003462 RID: 13410
		private bool _suppressOverrides;

		// Token: 0x04003463 RID: 13411
		private BlendShapeManager _manager;

		// Token: 0x04003464 RID: 13412
		private OutfitSystem _outfitSystem;

		// Token: 0x04003465 RID: 13413
		private bool _outfitSystemResolved;

		// Token: 0x04003466 RID: 13414
		[Tooltip("If true, will override any blend shapes controlled by Animator in LateUpdate")]
		[SerializeField]
		private bool overrideAnimatorBlendShapes = true;

		// Token: 0x020007AC RID: 1964
		private struct BlendShapeInfo
		{
			// Token: 0x04003467 RID: 13415
			public SkinnedMeshRenderer renderer;

			// Token: 0x04003468 RID: 13416
			public int index;
		}
	}
}
