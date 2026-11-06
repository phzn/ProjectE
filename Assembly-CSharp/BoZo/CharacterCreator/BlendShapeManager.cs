using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x0200079B RID: 1947
	public class BlendShapeManager : MonoBehaviour
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06003361 RID: 13153 RVA: 0x00123FBF File Offset: 0x001221BF
		// (set) Token: 0x06003362 RID: 13154 RVA: 0x00123FC6 File Offset: 0x001221C6
		public static BlendShapeManager Instance { get; private set; }

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06003363 RID: 13155 RVA: 0x00123FD0 File Offset: 0x001221D0
		// (remove) Token: 0x06003364 RID: 13156 RVA: 0x00124008 File Offset: 0x00122208
		public event Action OnBlendShapesApplied;

		// Token: 0x06003365 RID: 13157 RVA: 0x0012403D File Offset: 0x0012223D
		private void Awake()
		{
			BlendShapeManager.Instance = this;
			this.Initialize();
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0012404B File Offset: 0x0012224B
		private void OnEnable()
		{
			BlendShapeManager.Instance = this;
			this.SubscribePhysicalStyles();
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x00124059 File Offset: 0x00122259
		private void Start()
		{
			this.SubscribePhysicalStyles();
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x00124064 File Offset: 0x00122264
		private void OnDisable()
		{
			if (this.styleService != null && this.styleSubscribed)
			{
				this.styleService.OnPhysicalSwapCompleted -= this.HandlePhysicalStyleAdded;
				this.styleService.OnPhysicalRemoved -= this.HandlePhysicalStyleRemoved;
			}
			this.styleSubscribed = false;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001240BC File Offset: 0x001222BC
		private void SubscribePhysicalStyles()
		{
			if (!this.autoFindRenderers || this.styleSubscribed)
			{
				return;
			}
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInParent<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null)
			{
				return;
			}
			this.styleService.OnPhysicalSwapCompleted += this.HandlePhysicalStyleAdded;
			this.styleService.OnPhysicalRemoved += this.HandlePhysicalStyleRemoved;
			this.styleSubscribed = true;
			this.Rebuild();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x00124140 File Offset: 0x00122340
		private void HandlePhysicalStyleAdded(MCStylePartDefinitionSO definition, GameObject root)
		{
			if (root == null || !root.transform.IsChildOf(base.transform))
			{
				return;
			}
			this.changedHierarchyRenderers.Clear();
			root.GetComponentsInChildren<SkinnedMeshRenderer>(true, this.changedHierarchyRenderers);
			this.targetRenderers.RemoveAll((SkinnedMeshRenderer renderer) => renderer == null);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.changedHierarchyRenderers)
			{
				if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0 && !this.targetRenderers.Contains(skinnedMeshRenderer))
				{
					this.targetRenderers.Add(skinnedMeshRenderer);
				}
			}
			this.RefreshPhysicalStyleTargets();
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x00124228 File Offset: 0x00122428
		private void HandlePhysicalStyleRemoved(MCStylePartDefinitionSO definition, GameObject root)
		{
			if (root == null)
			{
				return;
			}
			this.targetRenderers.RemoveAll((SkinnedMeshRenderer renderer) => renderer == null || renderer.transform.IsChildOf(root.transform));
			this.RefreshPhysicalStyleTargets();
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x00124270 File Offset: 0x00122470
		private void RefreshPhysicalStyleTargets()
		{
			this.BuildBlendShapeCache();
			foreach (string text in this.overrideRequests.Keys)
			{
				this.CalculateFinalValue(text);
			}
			this.ApplyAllFinalValues();
			this.ForceRefreshAll();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001242DC File Offset: 0x001224DC
		private void OnDestroy()
		{
			if (BlendShapeManager.Instance == this)
			{
				BlendShapeManager.Instance = null;
			}
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001242F1 File Offset: 0x001224F1
		public static BlendShapeManager ResolveForRig(Component context)
		{
			if (context == null)
			{
				return BlendShapeManager.Instance;
			}
			return context.GetComponentInParent<BlendShapeManager>(true);
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x0012430C File Offset: 0x0012250C
		public void Initialize()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (this.autoFindRenderers)
			{
				this.FindAllRenderers();
			}
			this.BuildBlendShapeCache();
			this.isInitialized = true;
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[BlendShapeManager] Initialized with {0} renderers, {1} unique blend shapes", this.targetRenderers.Count, this.blendShapeCache.Count));
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x00124374 File Offset: 0x00122574
		public void Rebuild()
		{
			if (this.autoFindRenderers)
			{
				this.FindAllRenderers();
			}
			this.BuildBlendShapeCache();
			this.isInitialized = true;
			this.ForceRefreshAll();
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x00124398 File Offset: 0x00122598
		public void FindAllRenderers()
		{
			this.targetRenderers.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
				{
					this.targetRenderers.Add(skinnedMeshRenderer);
				}
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[BlendShapeManager] Found {0} SkinnedMeshRenderers with blend shapes", this.targetRenderers.Count));
			}
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0012441C File Offset: 0x0012261C
		private void BuildBlendShapeCache()
		{
			this.blendShapeCache.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.targetRenderers)
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					for (int i = 0; i < sharedMesh.blendShapeCount; i++)
					{
						string text = sharedMesh.GetBlendShapeName(i);
						if (text.Contains(":"))
						{
							text = text.Substring(text.LastIndexOf(':') + 1);
						}
						if (!this.blendShapeCache.ContainsKey(text))
						{
							this.blendShapeCache[text] = new List<ValueTuple<SkinnedMeshRenderer, int>>();
						}
						this.blendShapeCache[text].Add(new ValueTuple<SkinnedMeshRenderer, int>(skinnedMeshRenderer, i));
					}
				}
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x00124510 File Offset: 0x00122710
		public void SetOverride(string blendShapeName, float value, string sourceId, BlendShapePriority priority, bool applyImmediately = false)
		{
			if (string.IsNullOrEmpty(blendShapeName))
			{
				return;
			}
			if (!this.overrideRequests.ContainsKey(blendShapeName))
			{
				this.overrideRequests[blendShapeName] = new List<BlendShapeOverrideRequest>();
			}
			List<BlendShapeOverrideRequest> list = this.overrideRequests[blendShapeName];
			BlendShapeOverrideRequest blendShapeOverrideRequest = list.Find((BlendShapeOverrideRequest r) => r.SourceId == sourceId);
			bool flag = blendShapeOverrideRequest == null || !blendShapeOverrideRequest.IsActive || blendShapeOverrideRequest.Value != value || blendShapeOverrideRequest.Priority != priority;
			if (blendShapeOverrideRequest != null)
			{
				blendShapeOverrideRequest.Value = value;
				blendShapeOverrideRequest.Priority = priority;
				blendShapeOverrideRequest.IsActive = true;
			}
			else
			{
				list.Add(new BlendShapeOverrideRequest(sourceId, priority, value));
			}
			list.Sort((BlendShapeOverrideRequest a, BlendShapeOverrideRequest b) => b.Priority.CompareTo(a.Priority));
			if (flag || !applyImmediately)
			{
				this.dirtyBlendShapes.Add(blendShapeName);
			}
			if (applyImmediately)
			{
				this.CalculateFinalValue(blendShapeName);
				this.ApplyFinalValue(blendShapeName, this.finalValues[blendShapeName]);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[BlendShapeManager] SetOverride: {0} = {1} from {2} (priority: {3})", new object[] { blendShapeName, value, sourceId, priority }));
			}
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00124658 File Offset: 0x00122858
		public void RemoveOverride(string blendShapeName, string sourceId, bool applyImmediately = false, bool releaseToAnimationWhenUnowned = false, float unownedValue = 0f)
		{
			if (string.IsNullOrEmpty(blendShapeName))
			{
				return;
			}
			List<BlendShapeOverrideRequest> list;
			if (this.overrideRequests.TryGetValue(blendShapeName, out list))
			{
				BlendShapeOverrideRequest blendShapeOverrideRequest = list.Find((BlendShapeOverrideRequest r) => r.SourceId == sourceId);
				if (blendShapeOverrideRequest != null)
				{
					blendShapeOverrideRequest.IsActive = false;
					if (releaseToAnimationWhenUnowned)
					{
						if (!list.Exists((BlendShapeOverrideRequest r) => r.IsActive))
						{
							this.ApplyFinalValue(blendShapeName, unownedValue);
							this.overrideRequests.Remove(blendShapeName);
							this.finalValues.Remove(blendShapeName);
							this.dirtyBlendShapes.Remove(blendShapeName);
							return;
						}
					}
					this.dirtyBlendShapes.Add(blendShapeName);
					if (applyImmediately)
					{
						this.CalculateFinalValue(blendShapeName);
						this.ApplyFinalValue(blendShapeName, this.finalValues[blendShapeName]);
					}
					if (this.enableDebugLogs)
					{
						Debug.Log("[BlendShapeManager] RemoveOverride: " + blendShapeName + " from " + sourceId);
					}
				}
			}
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00124757 File Offset: 0x00122957
		public void ClearOverridesForHomeReturn()
		{
			this.overrideRequests.Clear();
			this.finalValues.Clear();
			this.dirtyBlendShapes.Clear();
			this.ClearRendererOverrides();
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00124780 File Offset: 0x00122980
		public void RemoveAllOverridesFromSource(string sourceId)
		{
			this.RemoveRendererOverridesFromSource(sourceId);
			Predicate<BlendShapeOverrideRequest> <>9__0;
			foreach (KeyValuePair<string, List<BlendShapeOverrideRequest>> keyValuePair in this.overrideRequests)
			{
				List<BlendShapeOverrideRequest> value = keyValuePair.Value;
				Predicate<BlendShapeOverrideRequest> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (BlendShapeOverrideRequest r) => r.SourceId == sourceId);
				}
				BlendShapeOverrideRequest blendShapeOverrideRequest = value.Find(predicate);
				if (blendShapeOverrideRequest != null)
				{
					blendShapeOverrideRequest.IsActive = false;
					this.dirtyBlendShapes.Add(keyValuePair.Key);
				}
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[BlendShapeManager] RemoveAllOverridesFromSource: " + sourceId);
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x00124850 File Offset: 0x00122A50
		private void LateUpdate()
		{
			if (!this.isInitialized)
			{
				return;
			}
			bool flag = this.dirtyBlendShapes.Count > 0 || this.rendererOverridesDirty;
			this.rendererOverridesDirty = false;
			foreach (string text in this.dirtyBlendShapes)
			{
				this.CalculateFinalValue(text);
			}
			this.dirtyBlendShapes.Clear();
			this.ApplyAllFinalValues();
			if (flag)
			{
				Action onBlendShapesApplied = this.OnBlendShapesApplied;
				if (onBlendShapesApplied == null)
				{
					return;
				}
				onBlendShapesApplied();
			}
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001248F0 File Offset: 0x00122AF0
		private void CalculateFinalValue(string blendShapeName)
		{
			float num = 0f;
			List<BlendShapeOverrideRequest> list;
			if (this.overrideRequests.TryGetValue(blendShapeName, out list))
			{
				foreach (BlendShapeOverrideRequest blendShapeOverrideRequest in list)
				{
					if (blendShapeOverrideRequest.IsActive)
					{
						num = blendShapeOverrideRequest.Value;
						break;
					}
				}
			}
			this.finalValues[blendShapeName] = num;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0012496C File Offset: 0x00122B6C
		private void ApplyAllFinalValues()
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.finalValues)
			{
				string key = keyValuePair.Key;
				float value = keyValuePair.Value;
				List<ValueTuple<SkinnedMeshRenderer, int>> list;
				if (this.blendShapeCache.TryGetValue(key, out list))
				{
					foreach (ValueTuple<SkinnedMeshRenderer, int> valueTuple in list)
					{
						SkinnedMeshRenderer item = valueTuple.Item1;
						int item2 = valueTuple.Item2;
						if (item != null)
						{
							item.SetBlendShapeWeight(item2, value);
						}
					}
					if (this.logAppliedValues && this.enableDebugLogs)
					{
						Debug.Log(string.Format("[BlendShapeManager] Applied: {0} = {1}", key, value));
					}
				}
			}
			this.ApplyRendererOverrides(null);
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x00124A68 File Offset: 0x00122C68
		private void ApplyFinalValue(string blendShapeName, float value)
		{
			List<ValueTuple<SkinnedMeshRenderer, int>> list;
			if (this.blendShapeCache.TryGetValue(blendShapeName, out list))
			{
				foreach (ValueTuple<SkinnedMeshRenderer, int> valueTuple in list)
				{
					SkinnedMeshRenderer item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					if (item != null)
					{
						item.SetBlendShapeWeight(item2, value);
					}
				}
			}
			this.ApplyRendererOverrides(blendShapeName);
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x00124AE4 File Offset: 0x00122CE4
		public float GetEffectiveBlendShapeValue(string blendShapeName, string excludedSource = null, bool readRendererFallback = true)
		{
			if (string.IsNullOrEmpty(blendShapeName))
			{
				return 0f;
			}
			List<BlendShapeOverrideRequest> list;
			if (this.overrideRequests.TryGetValue(blendShapeName, out list))
			{
				foreach (BlendShapeOverrideRequest blendShapeOverrideRequest in list)
				{
					if (blendShapeOverrideRequest.IsActive && blendShapeOverrideRequest.SourceId != excludedSource)
					{
						return blendShapeOverrideRequest.Value;
					}
				}
				return 0f;
			}
			if (!readRendererFallback)
			{
				return 0f;
			}
			return this.GetBlendShapeValue(blendShapeName);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x00124B80 File Offset: 0x00122D80
		public bool TryGetEffectiveOverride(string blendShapeName, out float value, string excludedSource = null)
		{
			value = 0f;
			List<BlendShapeOverrideRequest> list;
			if (string.IsNullOrEmpty(blendShapeName) || !this.overrideRequests.TryGetValue(blendShapeName, out list))
			{
				return false;
			}
			foreach (BlendShapeOverrideRequest blendShapeOverrideRequest in list)
			{
				if (blendShapeOverrideRequest.IsActive && !(blendShapeOverrideRequest.SourceId == excludedSource))
				{
					value = blendShapeOverrideRequest.Value;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x00124C10 File Offset: 0x00122E10
		public bool TryGetAuthoredBlendShapeValue(string blendShapeName, out float value)
		{
			value = 0f;
			List<BlendShapeOverrideRequest> list;
			if (string.IsNullOrEmpty(blendShapeName) || !this.overrideRequests.TryGetValue(blendShapeName, out list))
			{
				return false;
			}
			foreach (BlendShapeOverrideRequest blendShapeOverrideRequest in list)
			{
				if (blendShapeOverrideRequest.IsActive && blendShapeOverrideRequest.Priority <= BlendShapePriority.Customization)
				{
					value = blendShapeOverrideRequest.Value;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x00124C9C File Offset: 0x00122E9C
		public float GetBlendShapeValue(string blendShapeName)
		{
			float num;
			if (this.finalValues.TryGetValue(blendShapeName, out num))
			{
				return num;
			}
			List<ValueTuple<SkinnedMeshRenderer, int>> list;
			if (this.blendShapeCache.TryGetValue(blendShapeName, out list) && list.Count > 0)
			{
				ValueTuple<SkinnedMeshRenderer, int> valueTuple = list[0];
				SkinnedMeshRenderer item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (item != null)
				{
					return item.GetBlendShapeWeight(item2);
				}
			}
			return 0f;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x00124CFE File Offset: 0x00122EFE
		public bool HasBlendShape(string blendShapeName)
		{
			return this.blendShapeCache.ContainsKey(blendShapeName);
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x00124D0C File Offset: 0x00122F0C
		public List<string> GetAllBlendShapeNames()
		{
			return new List<string>(this.blendShapeCache.Keys);
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x00124D20 File Offset: 0x00122F20
		[ContextMenu("Force Refresh All")]
		public void ForceRefreshAll()
		{
			foreach (string text in this.overrideRequests.Keys)
			{
				this.dirtyBlendShapes.Add(text);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[BlendShapeManager] Force refresh all blend shapes");
			}
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x00124D90 File Offset: 0x00122F90
		[ContextMenu("Log Active Overrides")]
		public void LogActiveOverrides()
		{
			string text = "[BlendShapeManager] Active Overrides:\n";
			foreach (KeyValuePair<string, List<BlendShapeOverrideRequest>> keyValuePair in this.overrideRequests)
			{
				string key = keyValuePair.Key;
				foreach (BlendShapeOverrideRequest blendShapeOverrideRequest in keyValuePair.Value)
				{
					if (blendShapeOverrideRequest.IsActive)
					{
						text += string.Format("  {0}: {1} from {2} (priority: {3})\n", new object[] { key, blendShapeOverrideRequest.Value, blendShapeOverrideRequest.SourceId, blendShapeOverrideRequest.Priority });
					}
				}
			}
			Debug.Log(text);
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x00124E80 File Offset: 0x00123080
		public bool HasRendererOverride(SkinnedMeshRenderer renderer, string blendShapeName, string sourceId)
		{
			foreach (BlendShapeManager.RendererShape rendererShape in this.rendererShapes)
			{
				if (!(rendererShape.renderer != renderer) && !(rendererShape.name != blendShapeName))
				{
					using (List<BlendShapeManager.RendererShapeRequest>.Enumerator enumerator2 = rendererShape.requests.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.source == sourceId)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x00124F38 File Offset: 0x00123138
		public bool SetRendererOverride(SkinnedMeshRenderer renderer, string blendShapeName, float value, string sourceId, BlendShapePriority priority, float? nativeValue = null, string[] suppressWhenShapesActive = null, bool applyImmediately = false)
		{
			if (renderer == null || renderer.sharedMesh == null || string.IsNullOrEmpty(blendShapeName) || string.IsNullOrEmpty(sourceId) || !BlendShapeManager.FiniteRendererValue(value) || (nativeValue != null && !BlendShapeManager.FiniteRendererValue(nativeValue.Value)) || !renderer.transform.IsChildOf(base.transform))
			{
				return false;
			}
			int blendShapeIndex = renderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
			if (blendShapeIndex < 0)
			{
				return false;
			}
			BlendShapeManager.RendererShape rendererShape = this.rendererShapes.Find((BlendShapeManager.RendererShape entry) => entry.renderer == renderer && entry.name == blendShapeName);
			if (rendererShape == null)
			{
				rendererShape = new BlendShapeManager.RendererShape
				{
					renderer = renderer,
					mesh = renderer.sharedMesh,
					name = blendShapeName,
					index = blendShapeIndex,
					nativeValue = (nativeValue ?? renderer.GetBlendShapeWeight(blendShapeIndex))
				};
				this.rendererShapes.Add(rendererShape);
			}
			BlendShapeManager.RendererShapeRequest rendererShapeRequest = rendererShape.requests.Find((BlendShapeManager.RendererShapeRequest entry) => entry.source == sourceId);
			if (rendererShapeRequest == null)
			{
				rendererShapeRequest = new BlendShapeManager.RendererShapeRequest
				{
					source = sourceId
				};
				rendererShape.requests.Add(rendererShapeRequest);
			}
			rendererShapeRequest.value = value;
			rendererShapeRequest.priority = priority;
			rendererShapeRequest.suppressWhenShapesActive = suppressWhenShapesActive;
			this.rendererOverridesDirty = true;
			if (applyImmediately)
			{
				this.ApplyRendererShape(rendererShape);
			}
			return true;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x001250DC File Offset: 0x001232DC
		public void RemoveRendererOverridesFromSource(string sourceId)
		{
			Predicate<BlendShapeManager.RendererShapeRequest> <>9__0;
			for (int i = this.rendererShapes.Count - 1; i >= 0; i--)
			{
				BlendShapeManager.RendererShape rendererShape = this.rendererShapes[i];
				List<BlendShapeManager.RendererShapeRequest> requests = rendererShape.requests;
				Predicate<BlendShapeManager.RendererShapeRequest> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (BlendShapeManager.RendererShapeRequest request) => request.source == sourceId);
				}
				if (requests.RemoveAll(predicate) != 0)
				{
					this.rendererOverridesDirty = true;
					if (BlendShapeManager.RefreshRendererShape(rendererShape))
					{
						this.ApplyRendererShape(rendererShape);
					}
					if (rendererShape.requests.Count == 0)
					{
						this.rendererShapes.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x00125178 File Offset: 0x00123378
		private void ClearRendererOverrides()
		{
			foreach (BlendShapeManager.RendererShape rendererShape in this.rendererShapes)
			{
				rendererShape.requests.Clear();
				if (BlendShapeManager.RefreshRendererShape(rendererShape))
				{
					this.ApplyRendererShape(rendererShape);
				}
			}
			this.rendererShapes.Clear();
			this.rendererOverridesDirty = false;
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001251F0 File Offset: 0x001233F0
		private void ApplyRendererOverrides(string changedName = null)
		{
			for (int i = this.rendererShapes.Count - 1; i >= 0; i--)
			{
				BlendShapeManager.RendererShape rendererShape = this.rendererShapes[i];
				if (!BlendShapeManager.RefreshRendererShape(rendererShape))
				{
					this.rendererShapes.RemoveAt(i);
				}
				else
				{
					this.ApplyRendererShape(rendererShape);
				}
			}
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x00125240 File Offset: 0x00123440
		private static bool RefreshRendererShape(BlendShapeManager.RendererShape target)
		{
			if (target.renderer == null || target.renderer.sharedMesh == null)
			{
				return false;
			}
			if (target.mesh != target.renderer.sharedMesh)
			{
				target.mesh = target.renderer.sharedMesh;
				target.index = target.mesh.GetBlendShapeIndex(target.name);
			}
			return target.index >= 0 && target.index < target.mesh.blendShapeCount;
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x001252D0 File Offset: 0x001234D0
		private void ApplyRendererShape(BlendShapeManager.RendererShape target)
		{
			BlendShapeManager.RendererShapeRequest rendererShapeRequest = null;
			foreach (BlendShapeManager.RendererShapeRequest rendererShapeRequest2 in target.requests)
			{
				if (rendererShapeRequest == null || rendererShapeRequest2.priority > rendererShapeRequest.priority)
				{
					rendererShapeRequest = rendererShapeRequest2;
				}
			}
			BlendShapeOverrideRequest blendShapeOverrideRequest = null;
			string text = BlendShapeManager.NormalizeRendererShapeName(target.name);
			List<BlendShapeOverrideRequest> list;
			if (this.overrideRequests.TryGetValue(text, out list))
			{
				foreach (BlendShapeOverrideRequest blendShapeOverrideRequest2 in list)
				{
					if (blendShapeOverrideRequest2.IsActive)
					{
						blendShapeOverrideRequest = blendShapeOverrideRequest2;
						break;
					}
				}
			}
			float num = target.nativeValue;
			if (blendShapeOverrideRequest != null && (rendererShapeRequest == null || blendShapeOverrideRequest.Priority >= rendererShapeRequest.priority))
			{
				num = blendShapeOverrideRequest.Value;
			}
			else if (rendererShapeRequest != null)
			{
				num = Mathf.Lerp(rendererShapeRequest.value, target.nativeValue, this.RendererShapeCoverWeight(target, rendererShapeRequest.suppressWhenShapesActive));
			}
			target.renderer.SetBlendShapeWeight(target.index, num);
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x001253F8 File Offset: 0x001235F8
		private float RendererShapeCoverWeight(BlendShapeManager.RendererShape target, string[] covers)
		{
			float num = 0f;
			if (covers == null)
			{
				return num;
			}
			float num2 = 0f;
			float num3 = 0f;
			foreach (string text in covers)
			{
				float num4 = 0f;
				float num5;
				if (this.TryGetEffectiveOverride(BlendShapeManager.NormalizeRendererShapeName(text), out num5, null))
				{
					num4 = Mathf.Clamp01(num5 / 100f);
				}
				else
				{
					int blendShapeIndex = target.mesh.GetBlendShapeIndex(text);
					if (blendShapeIndex >= 0)
					{
						num4 = Mathf.Clamp01(target.renderer.GetBlendShapeWeight(blendShapeIndex) / 100f);
					}
				}
				if (text == "RainCoat_HideHair")
				{
					num2 = Mathf.Max(num2, num4);
				}
				else if (text == "RainCoat_HoodDown_HairFit")
				{
					num3 = Mathf.Max(num3, num4);
				}
				else
				{
					num = Mathf.Max(num, num4);
				}
			}
			return Mathf.Max(num, Mathf.Clamp01(num2 + num3));
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x001254E0 File Offset: 0x001236E0
		private static string NormalizeRendererShapeName(string value)
		{
			int num = value.LastIndexOf(':');
			if (num < 0)
			{
				return value;
			}
			return value.Substring(num + 1);
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool FiniteRendererValue(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x04003412 RID: 13330
		[Header("Configuration")]
		[Tooltip("Trova automaticamente tutti i SkinnedMeshRenderer")]
		[SerializeField]
		private bool autoFindRenderers = true;

		// Token: 0x04003413 RID: 13331
		[Tooltip("Lista manuale di SkinnedMeshRenderer da gestire")]
		[SerializeField]
		private List<SkinnedMeshRenderer> targetRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x04003414 RID: 13332
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x04003415 RID: 13333
		[SerializeField]
		private bool logAppliedValues;

		// Token: 0x04003416 RID: 13334
		[TupleElementNames(new string[] { "renderer", "index" })]
		private Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int>>> blendShapeCache = new Dictionary<string, List<ValueTuple<SkinnedMeshRenderer, int>>>();

		// Token: 0x04003417 RID: 13335
		private Dictionary<string, List<BlendShapeOverrideRequest>> overrideRequests = new Dictionary<string, List<BlendShapeOverrideRequest>>();

		// Token: 0x04003418 RID: 13336
		private Dictionary<string, float> finalValues = new Dictionary<string, float>();

		// Token: 0x04003419 RID: 13337
		private HashSet<string> dirtyBlendShapes = new HashSet<string>();

		// Token: 0x0400341A RID: 13338
		private bool isInitialized;

		// Token: 0x0400341B RID: 13339
		private MCStyleCustomizationService styleService;

		// Token: 0x0400341C RID: 13340
		private bool styleSubscribed;

		// Token: 0x0400341D RID: 13341
		private readonly List<SkinnedMeshRenderer> changedHierarchyRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x0400341E RID: 13342
		private readonly List<BlendShapeManager.RendererShape> rendererShapes = new List<BlendShapeManager.RendererShape>();

		// Token: 0x0400341F RID: 13343
		private bool rendererOverridesDirty;

		// Token: 0x0200079C RID: 1948
		private sealed class RendererShapeRequest
		{
			// Token: 0x04003420 RID: 13344
			public string source;

			// Token: 0x04003421 RID: 13345
			public float value;

			// Token: 0x04003422 RID: 13346
			public BlendShapePriority priority;

			// Token: 0x04003423 RID: 13347
			public string[] suppressWhenShapesActive;
		}

		// Token: 0x0200079D RID: 1949
		private sealed class RendererShape
		{
			// Token: 0x04003424 RID: 13348
			public SkinnedMeshRenderer renderer;

			// Token: 0x04003425 RID: 13349
			public Mesh mesh;

			// Token: 0x04003426 RID: 13350
			public string name;

			// Token: 0x04003427 RID: 13351
			public int index;

			// Token: 0x04003428 RID: 13352
			public float nativeValue;

			// Token: 0x04003429 RID: 13353
			public readonly List<BlendShapeManager.RendererShapeRequest> requests = new List<BlendShapeManager.RendererShapeRequest>();
		}
	}
}
