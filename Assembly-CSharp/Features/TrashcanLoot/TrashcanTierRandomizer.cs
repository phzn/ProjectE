using System;
using System.Collections.Generic;
using Features.Rank;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BF7 RID: 3063
	public sealed class TrashcanTierRandomizer : MonoBehaviour
	{
		// Token: 0x0600582A RID: 22570 RVA: 0x0022774D File Offset: 0x0022594D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			TrashcanTierRandomizer._instance = null;
			TrashcanTierRandomizer.PendingRegistrations.Clear();
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x0022775F File Offset: 0x0022595F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (TrashcanTierRandomizer._instance != null)
			{
				return;
			}
			TrashcanTierRandomizer._instance = new GameObject("TrashcanTierRandomizer").AddComponent<TrashcanTierRandomizer>();
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x00227784 File Offset: 0x00225984
		public static void Register(TrashcanLootInteractable can)
		{
			if (can != null && can.GetComponent<StreamedWorldPropView>() != null)
			{
				return;
			}
			if (Application.isPlaying && TrashcanTierRandomizer._instance != null && TrashcanTierRandomizer._instance.isActiveAndEnabled && TrashcanTierRandomizer._instance.TryProcessScopedCan(can, null, null))
			{
				return;
			}
			if (GuidedRankGameplayPolicy.AdvancedMechanicsBlocked)
			{
				return;
			}
			if (!Application.isPlaying || can == null)
			{
				return;
			}
			if (TrashcanTierRandomizer._instance != null && TrashcanTierRandomizer._instance.isActiveAndEnabled)
			{
				TrashcanTierRandomizer._instance.TryProcessBeforeGameplay(can);
				return;
			}
			TrashcanTierRandomizer.PendingRegistrations.Add(can);
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x00227824 File Offset: 0x00225A24
		public static GameObject SelectStreamedPrefab(Transform placement, GameObject bakedCommon)
		{
			if (GuidedRankGameplayPolicy.AdvancedMechanicsBlocked)
			{
				return bakedCommon;
			}
			TrashcanTierSpawnConfigSO trashcanTierSpawnConfigSO = ((TrashcanTierRandomizer._instance != null) ? TrashcanTierRandomizer._instance._config : Resources.Load<TrashcanTierSpawnConfigSO>("TrashcanTierSpawnConfig"));
			if (trashcanTierSpawnConfigSO == null)
			{
				return bakedCommon;
			}
			int num = ((TrashcanTierRandomizer._instance != null) ? TrashcanTierRandomizer._instance._fallbackSessionSeed : 0);
			int num2;
			if (ChunkManager.Instance != null && ChunkManager.Instance.TryGetSessionSeed(out num2))
			{
				num = num2;
			}
			GameObject gameObject = TrashcanTierRandomizer.SelectTier(trashcanTierSpawnConfigSO, TrashcanTierRandomizer.StableSampleForSeed(placement, num));
			if (!(gameObject == null) && !(gameObject == trashcanTierSpawnConfigSO.commonPrefab))
			{
				return gameObject;
			}
			return bakedCommon;
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x002278C9 File Offset: 0x00225AC9
		internal static void ForgetDestroyedScopedCan(TrashcanLootInteractable can)
		{
			TrashcanTierRandomizer.PendingRegistrations.Remove(can);
			if (TrashcanTierRandomizer._instance != null)
			{
				TrashcanTierRandomizer._instance._processed.Remove(can);
			}
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x002278F8 File Offset: 0x00225AF8
		internal static void PrepareScopedPayload(GameObject payload, Transform loadingGate = null)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			TrashcanLootInteractable[] componentsInChildren = payload.GetComponentsInChildren<TrashcanLootInteractable>(true);
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			if (TrashcanTierRandomizer._instance == null || TrashcanTierRandomizer._instance._config == null)
			{
				throw new InvalidOperationException("POI trashcan tiers require the gameplay tier service before activation.");
			}
			foreach (TrashcanLootInteractable trashcanLootInteractable in componentsInChildren)
			{
				TrashcanTierRandomizer._instance.TryProcessScopedCan(trashcanLootInteractable, payload.transform, loadingGate);
			}
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x0022796C File Offset: 0x00225B6C
		public static GameObject CanonicalTierForStreamedChoice(GameObject selected, GameObject bakedCommon)
		{
			TrashcanTierSpawnConfigSO trashcanTierSpawnConfigSO = ((TrashcanTierRandomizer._instance != null) ? TrashcanTierRandomizer._instance._config : Resources.Load<TrashcanTierSpawnConfigSO>("TrashcanTierSpawnConfig"));
			if (!(selected == bakedCommon) || !(trashcanTierSpawnConfigSO != null) || !(trashcanTierSpawnConfigSO.commonPrefab != null))
			{
				return selected;
			}
			return trashcanTierSpawnConfigSO.commonPrefab;
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x002279C8 File Offset: 0x00225BC8
		public static GameObject StreamedViewForCanonicalTier(GameObject canonicalTier, GameObject bakedCommon)
		{
			TrashcanTierSpawnConfigSO trashcanTierSpawnConfigSO = ((TrashcanTierRandomizer._instance != null) ? TrashcanTierRandomizer._instance._config : Resources.Load<TrashcanTierSpawnConfigSO>("TrashcanTierSpawnConfig"));
			if (!(trashcanTierSpawnConfigSO != null) || !(canonicalTier == trashcanTierSpawnConfigSO.commonPrefab))
			{
				return canonicalTier;
			}
			return bakedCommon;
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x00227A14 File Offset: 0x00225C14
		private void Awake()
		{
			if (TrashcanTierRandomizer._instance != null && TrashcanTierRandomizer._instance != this)
			{
				base.enabled = false;
				Object.Destroy(base.gameObject);
				return;
			}
			TrashcanTierRandomizer._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this._fallbackSessionSeed = Environment.TickCount ^ (base.GetInstanceID() * 397);
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x00227A78 File Offset: 0x00225C78
		private void OnEnable()
		{
			this._config = Resources.Load<TrashcanTierSpawnConfigSO>("TrashcanTierSpawnConfig");
			if (this._config == null)
			{
				Debug.LogWarning("[TrashcanTier] Config 'TrashcanTierSpawnConfig' non trovata in Resources - i bidoni di mappa restano tutti Common. Lancia Tools/Trashcan Loot/NEW - Build Tier Randomizer Config.");
				return;
			}
			this.CacheCommonCompatibilityRequirements();
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			SceneManager.sceneUnloaded += this.HandleSceneUnloaded;
			GameSystemsBootstrapper.OnGameplayReady += this.HandleGameplayReady;
			this.TryBindChunkManager();
			this.DrainEarlyRegistrations();
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x00227AF3 File Offset: 0x00225CF3
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			GameSystemsBootstrapper.OnGameplayReady -= this.HandleGameplayReady;
			this.UnbindChunkManager();
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x00227B2E File Offset: 0x00225D2E
		private void OnDestroy()
		{
			if (TrashcanTierRandomizer._instance == this)
			{
				TrashcanTierRandomizer._instance = null;
			}
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x00227B43 File Offset: 0x00225D43
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.TryBindChunkManager();
			this.DrainEarlyRegistrations();
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x00227B54 File Offset: 0x00225D54
		private void HandleSceneUnloaded(Scene scene)
		{
			this._processed.RemoveWhere((TrashcanLootInteractable can) => can == null || can.gameObject.scene == scene);
			TrashcanTierRandomizer.PendingRegistrations.RemoveWhere((TrashcanLootInteractable can) => can == null || can.gameObject.scene == scene);
		}

		// Token: 0x06005838 RID: 22584 RVA: 0x00227BA0 File Offset: 0x00225DA0
		private void HandleGameplayReady()
		{
			TrashcanTierRandomizer.PendingRegistrations.Clear();
			if (this._config != null && this._config.logProcessing && this._seen > this._lastLoggedSeen)
			{
				Debug.Log(string.Format("[TrashcanTier] Preprocessati {0} bidoni durante il loading ", this._seen) + string.Format("(common {0}, di cui {1} baked; ", this._seen - this._rare - this._legendary, this._keptCommon) + string.Format("rare {0}, legendary {1}).", this._rare, this._legendary));
				this._lastLoggedSeen = this._seen;
			}
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x00227C60 File Offset: 0x00225E60
		private void TryBindChunkManager()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == this._chunkManager && instance != null)
			{
				return;
			}
			this.UnbindChunkManager();
			if (instance == null)
			{
				return;
			}
			this._chunkManager = instance;
			this._chunkManager.OnContentRootReady += this.HandleContentRootReady;
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				ChunkInstance value = keyValuePair.Value;
				if (((value != null) ? value.chunkRoot : null) != null)
				{
					this.ProcessRootBeforeGameplay(value.chunkRoot);
				}
			}
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x00227D20 File Offset: 0x00225F20
		private void UnbindChunkManager()
		{
			if (this._chunkManager != null)
			{
				this._chunkManager.OnContentRootReady -= this.HandleContentRootReady;
			}
			this._chunkManager = null;
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x00227D4E File Offset: 0x00225F4E
		private void HandleContentRootReady(GameObject root)
		{
			this.ProcessRootBeforeGameplay(root);
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x00227D58 File Offset: 0x00225F58
		private void ProcessRootBeforeGameplay(GameObject root)
		{
			if (this._config == null || root == null || !TrashcanTierRandomizer.CanCreateReplacement(GameSystemsBootstrapper.GameplayReady))
			{
				return;
			}
			using (TrashcanTierRandomizer.DiscoverRootMarker.Auto())
			{
				this._rootScratch.Clear();
				root.GetComponentsInChildren<TrashcanLootInteractable>(true, this._rootScratch);
				for (int i = 0; i < this._rootScratch.Count; i++)
				{
					this.TryProcessBeforeGameplay(this._rootScratch[i]);
				}
				this._rootScratch.Clear();
			}
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x00227E04 File Offset: 0x00226004
		private void DrainEarlyRegistrations()
		{
			if (TrashcanTierRandomizer.PendingRegistrations.Count == 0)
			{
				return;
			}
			if (!TrashcanTierRandomizer.CanCreateReplacement(GameSystemsBootstrapper.GameplayReady))
			{
				TrashcanTierRandomizer.PendingRegistrations.Clear();
				return;
			}
			foreach (TrashcanLootInteractable trashcanLootInteractable in TrashcanTierRandomizer.PendingRegistrations)
			{
				this.TryProcessBeforeGameplay(trashcanLootInteractable);
			}
			TrashcanTierRandomizer.PendingRegistrations.Clear();
		}

		// Token: 0x0600583E RID: 22590 RVA: 0x00227E84 File Offset: 0x00226084
		private void TryProcessBeforeGameplay(TrashcanLootInteractable can)
		{
			if (this.TryProcessScopedCan(can, null, null))
			{
				return;
			}
			if (GuidedRankGameplayPolicy.AdvancedMechanicsBlocked)
			{
				return;
			}
			bool gameplayReady = GameSystemsBootstrapper.GameplayReady;
			if (!TrashcanTierRandomizer.CanCreateReplacement(gameplayReady) || this._config == null || can == null || this._isCreatingReplacement || !TrashcanTierRandomizer.IsEligibleMapSpawn(can.transform, gameplayReady) || this._processed.Contains(can))
			{
				return;
			}
			this.MarkHierarchyProcessed(can.gameObject);
			using (TrashcanTierRandomizer.PrewarmProcessMarker.Auto())
			{
				this.ProcessCan(can);
			}
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x00227F30 File Offset: 0x00226130
		private bool TryProcessScopedCan(TrashcanLootInteractable can, Transform payloadRoot = null, Transform loadingGate = null)
		{
			PoiResidencyStateKey poiResidencyStateKey;
			if (can == null || !PoiResidencyPlacement.TryGetWorldPropTierKey(can, out poiResidencyStateKey))
			{
				return false;
			}
			if (this._config == null || this._isCreatingReplacement || can.PoiResidencyTierPrepared)
			{
				return true;
			}
			can.PoiResidencyTierPrepared = true;
			GameObject gameObject;
			if (!PoiResidencyLedger.TryGetTier(poiResidencyStateKey, out gameObject))
			{
				gameObject = ((!GuidedRankGameplayPolicy.AdvancedMechanicsBlocked && TrashcanTierRandomizer.CanCreateReplacement(GameSystemsBootstrapper.GameplayReady) && TrashcanTierRandomizer.IsEligibleMapSpawn(can.transform, GameSystemsBootstrapper.GameplayReady)) ? ((loadingGate != null) ? TrashcanTierRandomizer.SelectTier(this._config, TrashcanTierRandomizer.StableSampleAfterPayloadRelease(can.transform, this.SessionSeed(), payloadRoot, loadingGate)) : this.RollTier(can.transform)) : this._config.commonPrefab);
				if (gameObject == null)
				{
					gameObject = this._config.commonPrefab;
				}
				PoiResidencyLedger.RememberCanonicalTier(poiResidencyStateKey, gameObject);
			}
			if (gameObject != null)
			{
				this.ProcessChosenCan(can, gameObject, true);
			}
			return true;
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x0022801D File Offset: 0x0022621D
		private void ProcessCan(TrashcanLootInteractable can)
		{
			this.ProcessChosenCan(can, null, false);
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x00228028 File Offset: 0x00226228
		private void ProcessChosenCan(TrashcanLootInteractable can, GameObject selectedTier, bool restoreScopedTier)
		{
			if (GuidedRankGameplayPolicy.AdvancedMechanicsBlocked)
			{
				return;
			}
			if ((!restoreScopedTier && !TrashcanTierRandomizer.CanCreateReplacement(GameSystemsBootstrapper.GameplayReady)) || can == null)
			{
				return;
			}
			this._seen++;
			GameObject gameObject = ((selectedTier != null) ? selectedTier : this.RollTier(can.transform));
			if (gameObject == null)
			{
				return;
			}
			if (gameObject == this._config.commonPrefab && this._config.keepCompatibleBakedCommon && this.CanKeepBakedCommon(can.gameObject))
			{
				this._keptCommon++;
				return;
			}
			if (gameObject == this._config.rarePrefab)
			{
				this._rare++;
			}
			else if (gameObject == this._config.legendaryPrefab)
			{
				this._legendary++;
			}
			Transform transform = can.transform;
			int siblingIndex = transform.GetSiblingIndex();
			this._isCreatingReplacement = true;
			GameObject gameObject2 = null;
			try
			{
				if (restoreScopedTier)
				{
					gameObject2 = new GameObject("Inactive POI tier restore");
					gameObject2.SetActive(false);
					gameObject2.transform.SetParent(transform.parent, false);
				}
				GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, (gameObject2 != null) ? gameObject2.transform : transform.parent);
				gameObject3.name = can.name + " [" + gameObject.name + "]";
				gameObject3.transform.localPosition = transform.localPosition;
				gameObject3.transform.localRotation = transform.localRotation;
				gameObject3.transform.localScale = transform.localScale;
				if (restoreScopedTier)
				{
					PoiResidencyPlacement.ReplaceWorldProp(can.gameObject, gameObject3);
					gameObject3.transform.SetParent(transform.parent, false);
				}
				gameObject3.transform.SetSiblingIndex(siblingIndex);
				if (restoreScopedTier)
				{
					TrashcanLootInteractable[] componentsInChildren = gameObject3.GetComponentsInChildren<TrashcanLootInteractable>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].PoiResidencyTierPrepared = true;
					}
					can.gameObject.SetActive(false);
				}
				else
				{
					this.MarkHierarchyProcessed(gameObject3);
				}
			}
			finally
			{
				this._isCreatingReplacement = false;
				if (gameObject2 != null)
				{
					Object.Destroy(gameObject2);
				}
			}
			if (restoreScopedTier)
			{
				this._processed.Remove(can);
			}
			Object.Destroy(can.gameObject);
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x00228280 File Offset: 0x00226480
		private void MarkHierarchyProcessed(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			this._hierarchyScratch.Clear();
			root.GetComponentsInChildren<TrashcanLootInteractable>(true, this._hierarchyScratch);
			for (int i = 0; i < this._hierarchyScratch.Count; i++)
			{
				TrashcanLootInteractable trashcanLootInteractable = this._hierarchyScratch[i];
				if (trashcanLootInteractable != null)
				{
					this._processed.Add(trashcanLootInteractable);
				}
			}
			this._hierarchyScratch.Clear();
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x002282F3 File Offset: 0x002264F3
		private GameObject RollTier(Transform can)
		{
			return TrashcanTierRandomizer.SelectTier(this._config, this.StableSample(can));
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x00228308 File Offset: 0x00226508
		private static GameObject SelectTier(TrashcanTierSpawnConfigSO config, float sample)
		{
			float num = Mathf.Max(0f, config.commonWeight);
			float num2 = ((config.rarePrefab != null) ? Mathf.Max(0f, config.rareWeight) : 0f);
			float num3 = ((config.legendaryPrefab != null) ? Mathf.Max(0f, config.legendaryWeight) : 0f);
			float num4 = num + num2 + num3;
			if (num4 <= 0f)
			{
				return config.commonPrefab;
			}
			float num5 = Mathf.Clamp01(sample) * num4;
			if (num5 < num)
			{
				return config.commonPrefab;
			}
			if (num5 < num + num2)
			{
				return config.rarePrefab;
			}
			return config.legendaryPrefab;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x002283B1 File Offset: 0x002265B1
		private float StableSample(Transform can)
		{
			return TrashcanTierRandomizer.StableSampleForSeed(can, this.SessionSeed());
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x002283C0 File Offset: 0x002265C0
		private int SessionSeed()
		{
			int num = this._fallbackSessionSeed;
			int num2;
			if (this._chunkManager != null && this._chunkManager.TryGetSessionSeed(out num2))
			{
				num = num2;
			}
			return num;
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x002283F4 File Offset: 0x002265F4
		private static float StableSampleForSeed(Transform can, int sessionSeed)
		{
			return TrashcanTierRandomizer.StableSampleHierarchy(can, sessionSeed, null, null, 0, default(Vector3));
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x00228414 File Offset: 0x00226614
		internal static float StableSampleAfterPayloadRelease(Transform can, int sessionSeed, Transform payloadRoot, Transform loadingGate)
		{
			if (can == null || payloadRoot == null || loadingGate == null || payloadRoot.parent != loadingGate || loadingGate.parent == null || !can.IsChildOf(payloadRoot))
			{
				throw new InvalidOperationException("Tier sampling requires the actual inactive payload loading gate.");
			}
			Transform parent = loadingGate.parent;
			return TrashcanTierRandomizer.StableSampleHierarchy(can, sessionSeed, payloadRoot, parent, parent.childCount - 1, parent.InverseTransformPoint(payloadRoot.position));
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x00228490 File Offset: 0x00226690
		private static float StableSampleHierarchy(Transform can, int sessionSeed, Transform projectedRoot, Transform projectedParent, int projectedSiblingIndex, Vector3 projectedLocalPosition)
		{
			uint num = (uint)(sessionSeed ^ -1525552717);
			Transform transform = can;
			while (transform != null)
			{
				num = TrashcanTierRandomizer.HashString(num, transform.name);
				num = TrashcanTierRandomizer.Mix(num, (uint)((transform == projectedRoot) ? projectedSiblingIndex : transform.GetSiblingIndex()));
				Vector3 vector = ((transform == projectedRoot) ? projectedLocalPosition : transform.localPosition);
				num = TrashcanTierRandomizer.Mix(num, (uint)Mathf.RoundToInt(vector.x * 1000f));
				num = TrashcanTierRandomizer.Mix(num, (uint)Mathf.RoundToInt(vector.y * 1000f));
				num = TrashcanTierRandomizer.Mix(num, (uint)Mathf.RoundToInt(vector.z * 1000f));
				transform = ((transform == projectedRoot) ? projectedParent : transform.parent);
			}
			num ^= num >> 16;
			num *= 2146121005U;
			num ^= num >> 15;
			num *= 2221713035U;
			num ^= num >> 16;
			return (num & 16777215U) / 16777216f;
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x00228584 File Offset: 0x00226784
		private static uint HashString(uint hash, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return TrashcanTierRandomizer.Mix(hash, 0U);
			}
			for (int i = 0; i < value.Length; i++)
			{
				hash = TrashcanTierRandomizer.Mix(hash, (uint)value[i]);
			}
			return hash;
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x002285C2 File Offset: 0x002267C2
		private static uint Mix(uint hash, uint value)
		{
			hash ^= value;
			hash *= 16777619U;
			return hash;
		}

		// Token: 0x0600584C RID: 22604 RVA: 0x002285D4 File Offset: 0x002267D4
		private void CacheCommonCompatibilityRequirements()
		{
			this._commonRootRequirements.Clear();
			if (this._config.commonPrefab == null)
			{
				return;
			}
			this._componentScratch.Clear();
			this._config.commonPrefab.GetComponents<Component>(this._componentScratch);
			for (int i = 0; i < this._componentScratch.Count; i++)
			{
				Component component = this._componentScratch[i];
				if (!(component == null) && !(component is Transform))
				{
					Type type = component.GetType();
					int num;
					this._commonRootRequirements.TryGetValue(type, out num);
					this._commonRootRequirements[type] = num + 1;
				}
			}
			this._componentScratch.Clear();
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x00228688 File Offset: 0x00226888
		private bool CanKeepBakedCommon(GameObject candidate)
		{
			if (candidate == null || this._config.commonPrefab == null || this._commonRootRequirements.Count == 0)
			{
				return false;
			}
			this._candidateRootCounts.Clear();
			this._componentScratch.Clear();
			candidate.GetComponents<Component>(this._componentScratch);
			for (int i = 0; i < this._componentScratch.Count; i++)
			{
				Component component = this._componentScratch[i];
				if (!(component == null) && !(component is Transform))
				{
					Type type = component.GetType();
					int num;
					this._candidateRootCounts.TryGetValue(type, out num);
					this._candidateRootCounts[type] = num + 1;
				}
			}
			this._componentScratch.Clear();
			foreach (KeyValuePair<Type, int> keyValuePair in this._commonRootRequirements)
			{
				int num2;
				if (!this._candidateRootCounts.TryGetValue(keyValuePair.Key, out num2) || num2 < keyValuePair.Value)
				{
					return false;
				}
			}
			for (int j = 0; j < TrashcanTierRandomizer.RequiredCommonChildNames.Length; j++)
			{
				string text = TrashcanTierRandomizer.RequiredCommonChildNames[j];
				if (TrashcanTierRandomizer.FindDescendant(this._config.commonPrefab.transform, text) != null && TrashcanTierRandomizer.FindDescendant(candidate.transform, text) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x00228808 File Offset: 0x00226A08
		private static Transform FindDescendant(Transform root, string targetName)
		{
			if (root == null)
			{
				return null;
			}
			if (root.name == targetName)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = TrashcanTierRandomizer.FindDescendant(root.GetChild(i), targetName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x0022885C File Offset: 0x00226A5C
		private static bool IsEligibleMapSpawn(Transform transform, bool gameplayReady)
		{
			bool flag = false;
			bool flag2 = false;
			Transform transform2 = transform;
			while (transform2 != null)
			{
				string name = transform2.name;
				if (name == "ChunkPool" || name == "ZonePool")
				{
					flag2 = true;
				}
				if (name == "TrashcanSpawnPoints")
				{
					flag = true;
				}
				transform2 = transform2.parent;
			}
			return flag && (!flag2 || !gameplayReady);
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x002288C1 File Offset: 0x00226AC1
		private static bool CanCreateReplacement(bool gameplayReady)
		{
			return !gameplayReady;
		}

		// Token: 0x040058B2 RID: 22706
		private const string ContainerName = "TrashcanSpawnPoints";

		// Token: 0x040058B3 RID: 22707
		private const string ChunkPoolName = "ChunkPool";

		// Token: 0x040058B4 RID: 22708
		private const string ZonePoolName = "ZonePool";

		// Token: 0x040058B5 RID: 22709
		private const string ConfigResourceName = "TrashcanTierSpawnConfig";

		// Token: 0x040058B6 RID: 22710
		private const uint TierHashSalt = 2769414579U;

		// Token: 0x040058B7 RID: 22711
		private static readonly ProfilerMarker PrewarmProcessMarker = new ProfilerMarker("TrashcanTierRandomizer.PreGameplayProcess");

		// Token: 0x040058B8 RID: 22712
		private static readonly ProfilerMarker DiscoverRootMarker = new ProfilerMarker("TrashcanTierRandomizer.DiscoverRoot");

		// Token: 0x040058B9 RID: 22713
		private static TrashcanTierRandomizer _instance;

		// Token: 0x040058BA RID: 22714
		private static readonly HashSet<TrashcanLootInteractable> PendingRegistrations = new HashSet<TrashcanLootInteractable>();

		// Token: 0x040058BB RID: 22715
		private readonly HashSet<TrashcanLootInteractable> _processed = new HashSet<TrashcanLootInteractable>();

		// Token: 0x040058BC RID: 22716
		private readonly List<TrashcanLootInteractable> _rootScratch = new List<TrashcanLootInteractable>(16);

		// Token: 0x040058BD RID: 22717
		private readonly List<TrashcanLootInteractable> _hierarchyScratch = new List<TrashcanLootInteractable>(2);

		// Token: 0x040058BE RID: 22718
		private readonly Dictionary<Type, int> _commonRootRequirements = new Dictionary<Type, int>();

		// Token: 0x040058BF RID: 22719
		private readonly List<Component> _componentScratch = new List<Component>(8);

		// Token: 0x040058C0 RID: 22720
		private readonly Dictionary<Type, int> _candidateRootCounts = new Dictionary<Type, int>();

		// Token: 0x040058C1 RID: 22721
		private static readonly string[] RequiredCommonChildNames = new string[] { "RummageAnchor", "WasteCanRig" };

		// Token: 0x040058C2 RID: 22722
		private TrashcanTierSpawnConfigSO _config;

		// Token: 0x040058C3 RID: 22723
		private ChunkManager _chunkManager;

		// Token: 0x040058C4 RID: 22724
		private int _fallbackSessionSeed;

		// Token: 0x040058C5 RID: 22725
		private int _seen;

		// Token: 0x040058C6 RID: 22726
		private int _rare;

		// Token: 0x040058C7 RID: 22727
		private int _legendary;

		// Token: 0x040058C8 RID: 22728
		private int _keptCommon;

		// Token: 0x040058C9 RID: 22729
		private int _lastLoggedSeen;

		// Token: 0x040058CA RID: 22730
		private bool _isCreatingReplacement;
	}
}
