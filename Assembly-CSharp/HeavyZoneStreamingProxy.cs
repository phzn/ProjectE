using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Features.RenderingPrecision;
using Pathfinding;
using Unity.Profiling;
using UnityEngine;

// Token: 0x0200006A RID: 106
public sealed class HeavyZoneStreamingProxy : MonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000525 RID: 1317 RVA: 0x00026B78 File Offset: 0x00024D78
	// (remove) Token: 0x06000526 RID: 1318 RVA: 0x00026BAC File Offset: 0x00024DAC
	public static event Action<HeavyZoneStreamingProxy.ColdPayloadSetupSample> ColdPayloadSetupMeasured;

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000527 RID: 1319 RVA: 0x00026BDF File Offset: 0x00024DDF
	// (set) Token: 0x06000528 RID: 1320 RVA: 0x00026BE6 File Offset: 0x00024DE6
	public static long CompletedColdPayloadSetupCount { get; private set; }

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000529 RID: 1321 RVA: 0x00026BF0 File Offset: 0x00024DF0
	// (remove) Token: 0x0600052A RID: 1322 RVA: 0x00026C28 File Offset: 0x00024E28
	public event Action<HeavyZoneStreamingProxy> PayloadReady;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600052B RID: 1323 RVA: 0x00026C60 File Offset: 0x00024E60
	// (remove) Token: 0x0600052C RID: 1324 RVA: 0x00026C98 File Offset: 0x00024E98
	public event Action<HeavyZoneStreamingProxy> OperationalReady;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x0600052D RID: 1325 RVA: 0x00026CD0 File Offset: 0x00024ED0
	// (remove) Token: 0x0600052E RID: 1326 RVA: 0x00026D08 File Offset: 0x00024F08
	public event Action<HeavyZoneStreamingProxy, string> PayloadFailed;

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x00026D3D File Offset: 0x00024F3D
	// (set) Token: 0x06000530 RID: 1328 RVA: 0x00026D45 File Offset: 0x00024F45
	public bool IsReady { get; private set; }

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000531 RID: 1329 RVA: 0x00026D4E File Offset: 0x00024F4E
	// (set) Token: 0x06000532 RID: 1330 RVA: 0x00026D56 File Offset: 0x00024F56
	public bool LoadFailed { get; private set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x00026D60 File Offset: 0x00024F60
	public bool NeverPool
	{
		get
		{
			PoiResidencyCatalog.Entry entry;
			return this.neverPool || (PoiResidencyCatalog.TryGetByResourcePath(this.resourcesPrefabPath, out entry) && MapResidencyPilotSelection.IsFamilyEnabled(entry.family));
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x00026D93 File Offset: 0x00024F93
	public bool UsesStagedPooledActivation
	{
		get
		{
			return this.useStagedPooledActivation;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000535 RID: 1333 RVA: 0x00026D9B File Offset: 0x00024F9B
	// (set) Token: 0x06000536 RID: 1334 RVA: 0x00026DA3 File Offset: 0x00024FA3
	public bool IsOperationalReady { get; private set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x00026DAC File Offset: 0x00024FAC
	public int StagedSleepWorkRemaining
	{
		get
		{
			return Mathf.Max(0, this._stagedSleepIndex + 1);
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x00026DBC File Offset: 0x00024FBC
	public bool HasCompleteColliderSnapshot
	{
		get
		{
			return !this.useStagedPooledActivation || this._stagedSnapshotReady;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00026DCE File Offset: 0x00024FCE
	public int OwnedColliderCount
	{
		get
		{
			return this._colliders.Count;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x00026DDB File Offset: 0x00024FDB
	public string ResourcesPrefabPath
	{
		get
		{
			return this.resourcesPrefabPath;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x00026DE3 File Offset: 0x00024FE3
	public Vector3 PayloadLocalOffset
	{
		get
		{
			return this.payloadLocalOffset;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x00026DEB File Offset: 0x00024FEB
	public Vector3 PayloadLocalScale
	{
		get
		{
			return this.payloadLocalScale;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x00026DF3 File Offset: 0x00024FF3
	public bool UsesTerrainAutoAlignment
	{
		get
		{
			return this.alignTerrainSurfaceToAnchor;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x00026DFB File Offset: 0x00024FFB
	public GameObject PayloadInstance
	{
		get
		{
			return this._payloadInstance;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x00026E03 File Offset: 0x00025003
	public bool IsStreamingUrgent
	{
		get
		{
			return this._urgentStreamingWork;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000540 RID: 1344 RVA: 0x00026E0B File Offset: 0x0002500B
	public bool UsesAddressablePayload
	{
		get
		{
			return this._addressableLease != null;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x00026E16 File Offset: 0x00025016
	public GameObject NativeAddressablePrefab
	{
		get
		{
			HeavyZoneAddressableResidency.Lease addressableLease = this._addressableLease;
			if (addressableLease == null)
			{
				return null;
			}
			return addressableLease.Asset;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x00026E29 File Offset: 0x00025029
	public bool HasCanonicalGameplayReferences
	{
		get
		{
			return this._canonicalGameplayBound;
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00026E31 File Offset: 0x00025031
	public HeavyZoneAddressableResidency.Lease RetainAddressablePayload()
	{
		HeavyZoneAddressableResidency.Lease addressableLease = this._addressableLease;
		if (addressableLease == null)
		{
			return null;
		}
		return addressableLease.Retain();
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00026E44 File Offset: 0x00025044
	public bool IsLoadPending
	{
		get
		{
			return !string.IsNullOrWhiteSpace(this.resourcesPrefabPath) && !this.IsReady && !this.LoadFailed;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00026E66 File Offset: 0x00025066
	public bool IsWakePending
	{
		get
		{
			return this.IsReady && !this.IsOperationalReady && !this.LoadFailed;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00026E83 File Offset: 0x00025083
	public static int ActivePayloadAssetLoadCount
	{
		get
		{
			return HeavyZoneStreamingProxy._activeSharedPayloadLoads;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000547 RID: 1351 RVA: 0x00026E8C File Offset: 0x0002508C
	public static int CompletedPayloadCacheEntryCount
	{
		get
		{
			int num = 0;
			foreach (HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad in HeavyZoneStreamingProxy.SharedPayloadLoads.Values)
			{
				if (sharedPayloadLoad != null && sharedPayloadLoad.completed)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00026EF0 File Offset: 0x000250F0
	public static void ReleaseCachedPayloadAssets(bool releasePendingConsumers = false)
	{
		HeavyZoneAddressableResidency.TrimUnused(true);
		string[] array = new string[HeavyZoneStreamingProxy.SharedPayloadLoads.Count];
		HeavyZoneStreamingProxy.SharedPayloadLoads.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad;
			if (HeavyZoneStreamingProxy.SharedPayloadLoads.TryGetValue(array[i], out sharedPayloadLoad))
			{
				HeavyZoneStreamingProxy.PruneDeadConsumers(sharedPayloadLoad);
				if (sharedPayloadLoad.completed)
				{
					HeavyZoneStreamingProxy.SharedPayloadLoads.Remove(array[i]);
				}
				else if (releasePendingConsumers || sharedPayloadLoad.consumers.Count == 0)
				{
					sharedPayloadLoad.releaseWhenCompleted = true;
				}
			}
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00026F78 File Offset: 0x00025178
	public static bool PrefetchPayloadAsset(string resourcePath)
	{
		if (string.IsNullOrWhiteSpace(resourcePath))
		{
			return false;
		}
		string text;
		if (MapResidencyPilotSelection.TryResolveAddress(resourcePath, out text))
		{
			if (HeavyZoneStreamingProxy._activeSharedPayloadLoads > 0)
			{
				return false;
			}
			QualitySettings.streamingMipmapsActive = true;
			MapResidencyPilotSelection.IGameplayManifestRequest gameplayManifestRequest;
			try
			{
				gameplayManifestRequest = MapResidencyPilotSelection.EnsureGameplayManifest(text);
			}
			catch (Exception)
			{
				return false;
			}
			return gameplayManifestRequest.IsDone && !(gameplayManifestRequest.Asset == null) && HeavyZoneAddressableResidency.Prefetch(text);
		}
		else
		{
			bool flag = !HeavyZoneStreamingProxy.SharedPayloadLoads.ContainsKey(resourcePath);
			if (!HeavyZoneStreamingProxy.CanStartNewPrefetchAssetLoad(HeavyZoneStreamingProxy._activeSharedPayloadLoads, !flag))
			{
				return false;
			}
			QualitySettings.streamingMipmapsActive = true;
			HeavyZoneStreamingProxy.GetOrStartSharedPayloadLoad(resourcePath, null);
			return flag;
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002701C File Offset: 0x0002521C
	private static bool CanStartNewPrefetchAssetLoad(int activeSharedPayloadLoads, bool alreadyTracked)
	{
		return alreadyTracked || activeSharedPayloadLoads < 1;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00027028 File Offset: 0x00025228
	public void SetStreamingUrgency(bool urgent)
	{
		bool flag = this._urgentStreamingWork != urgent;
		this._urgentStreamingWork = urgent;
		this._requestedLoadPriority = (urgent ? 100 : (-1));
		HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad;
		if (!string.IsNullOrWhiteSpace(this.resourcesPrefabPath) && HeavyZoneStreamingProxy.SharedPayloadLoads.TryGetValue(this.resourcesPrefabPath, out sharedPayloadLoad) && !sharedPayloadLoad.completed)
		{
			bool flag2 = sharedPayloadLoad.consumers.Add(this);
			if (flag || flag2)
			{
				HeavyZoneStreamingProxy.RefreshSharedPayloadLoadPriority(sharedPayloadLoad);
			}
		}
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00027098 File Offset: 0x00025298
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetSharedPayloadLoads()
	{
		if (HeavyZoneStreamingProxy._activeSharedPayloadLoads > 0)
		{
			Application.backgroundLoadingPriority = HeavyZoneStreamingProxy._backgroundPriorityBeforeSharedLoads;
		}
		HeavyZoneStreamingProxy.SharedPayloadLoads.Clear();
		HeavyZoneStreamingProxy._activeSharedPayloadLoads = 0;
		HeavyZoneStreamingProxy._sharedPayloadAccessSequence = 0UL;
		HeavyZoneStreamingProxy._nextCameraMaskUpdate = 0f;
		HeavyZoneStreamingProxy.CompletedColdPayloadSetupCount = 0L;
		HeavyZoneStreamingProxy.ColdPayloadSetupMeasured = null;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x000270E5 File Offset: 0x000252E5
	private void Awake()
	{
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x000270F8 File Offset: 0x000252F8
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		this.ReleaseAddressablePayloadAfterDestruction(new Object[] { base.gameObject, this._payloadInstance, this._payloadLoadingGate });
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00027134 File Offset: 0x00025334
	private void OnWorldOriginShifted(Vector3 delta)
	{
		this._lastCullPosition += delta;
		this._runtimeColliderUpdatePosition += delta;
		for (int i = 0; i < this._colliders.Count; i++)
		{
			HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[i];
			colliderState.worldBounds.center = colliderState.worldBounds.center + delta;
			this._colliders[i] = colliderState;
		}
		if (this._navigationColliderBounds != null)
		{
			Bounds value = this._navigationColliderBounds.Value;
			value.center += delta;
			this._navigationColliderBounds = new Bounds?(value);
		}
		if (this._restoredNavigationColliderBounds != null)
		{
			Bounds value2 = this._restoredNavigationColliderBounds.Value;
			value2.center += delta;
			this._restoredNavigationColliderBounds = new Bounds?(value2);
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00027220 File Offset: 0x00025420
	private void OnEnable()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.IsReady)
		{
			if (this.useStagedPooledActivation && this._stagedDormant)
			{
				this.IsOperationalReady = false;
				if (this._stagedWakeArmed && this._stagedWakeRoutine == null)
				{
					this._stagedWakeRoutine = base.StartCoroutine(this.StagedWakeRoutine());
				}
				return;
			}
			this.RefreshColliderBounds();
			this.UpdateColliderStates(true);
			return;
		}
		else
		{
			if (this.LoadFailed || this._loadRoutine != null)
			{
				return;
			}
			Coroutine coroutine = base.StartCoroutine(this.LoadPayloadRoutine());
			if (!base.isActiveAndEnabled && coroutine != null)
			{
				base.StopCoroutine(coroutine);
			}
			this._loadRoutine = ((!this.LoadFailed && !this.IsReady && base.isActiveAndEnabled) ? coroutine : null);
			if (!this.IsReady && !base.isActiveAndEnabled)
			{
				this.DiscardIncompletePayload();
			}
			return;
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x000272F0 File Offset: 0x000254F0
	private void OnDisable()
	{
		if (!this.IsReady)
		{
			if (this._loadRoutine != null)
			{
				base.StopCoroutine(this._loadRoutine);
			}
			this._loadRoutine = null;
			this.DiscardIncompletePayload();
		}
		this.UnregisterSharedPayloadLoadConsumer();
		if (this._stagedWakeRoutine != null)
		{
			base.StopCoroutine(this._stagedWakeRoutine);
			this._stagedWakeRoutine = null;
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00027347 File Offset: 0x00025547
	private IEnumerator LoadPayloadRoutine()
	{
		if (string.IsNullOrWhiteSpace(this.resourcesPrefabPath))
		{
			this.Fail("Resources prefab path is empty.");
			yield break;
		}
		QualitySettings.streamingMipmapsActive = true;
		HeavyZoneStreamingProxy.SharedPayloadLoad sharedLoad = null;
		string loadError = null;
		string address;
		if (MapResidencyPilotSelection.TryResolveAddress(this.resourcesPrefabPath, out address))
		{
			MapResidencyPilotSelection.IGameplayManifestRequest sharedGameplay = null;
			if (!this.useStagedPooledActivation)
			{
				loadError = "POI gameplay binding requires an inactive staged loading gate.";
			}
			else
			{
				try
				{
					sharedGameplay = MapResidencyPilotSelection.EnsureGameplayManifest(address);
				}
				catch (Exception ex)
				{
					loadError = ex.Message;
				}
			}
			if (sharedGameplay != null)
			{
				while (!sharedGameplay.IsDone)
				{
					yield return null;
				}
				PoiResidencyCatalog.Entry entry;
				if (sharedGameplay.Asset == null)
				{
					loadError = "Shared gameplay definitions could not load: " + sharedGameplay.Error;
				}
				else if (!PoiResidencyCatalog.TryGetByAddress(address, out entry) || sharedGameplay.Asset.family != entry.family || sharedGameplay.Asset.variant != entry.variant)
				{
					loadError = "POI gameplay manifest does not match its selected family and variant.";
				}
				else
				{
					this._gameplayManifest = sharedGameplay.Asset;
					try
					{
						this._addressableLease = HeavyZoneAddressableResidency.Acquire(address);
					}
					catch (Exception ex2)
					{
						loadError = ex2.Message;
					}
				}
			}
			if (this._addressableLease != null)
			{
				while (!this._addressableLease.IsDone)
				{
					yield return null;
				}
				this._loadedPrefab = this._addressableLease.Asset;
				loadError = this._addressableLease.Error;
			}
			sharedGameplay = null;
		}
		else
		{
			sharedLoad = HeavyZoneStreamingProxy.GetOrStartSharedPayloadLoad(this.resourcesPrefabPath, this);
			while (!sharedLoad.completed)
			{
				yield return null;
			}
			this._loadedPrefab = sharedLoad.asset;
		}
		if (this._loadedPrefab == null)
		{
			this.ReleaseAddressablePayloadAfterDestruction(Array.Empty<Object>());
			this.Fail("Payload not found: '" + this.resourcesPrefabPath + "'. " + loadError);
			yield break;
		}
		if (!this.TryCloneAndConfigurePayload(new Action<GameObject>(this.ConfigureClonedPayload)))
		{
			yield break;
		}
		sharedLoad = null;
		if (this.useStagedPooledActivation)
		{
			yield return this.BuildStagedSnapshotRoutine();
			yield return this.PrepareInitialStagedDormancyRoutine();
		}
		this.PublishPayloadReady();
		if (!this.useStagedPooledActivation)
		{
			yield return null;
			this.UpdateColliderStates(true);
		}
		yield break;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00027358 File Offset: 0x00025558
	private bool TryCloneAndConfigurePayload(Action<GameObject> configure)
	{
		bool flag = HeavyZoneStreamingProxy.ColdPayloadSetupMeasured != null;
		double num = (flag ? Time.realtimeSinceStartupAsDouble : 0.0);
		long num2 = (flag ? Stopwatch.GetTimestamp() : 0L);
		long num3;
		try
		{
			if (this._gameplayManifest != null && !this.useStagedPooledActivation)
			{
				throw new InvalidOperationException("Gameplay binding must run before payload activation.");
			}
			using (HeavyZoneStreamingProxy.AtomicInstantiateMarker.Auto())
			{
				this._payloadInstance = (this.useStagedPooledActivation ? this.InstantiatePayloadBehindInactiveGate(this._loadedPrefab) : Object.Instantiate<GameObject>(this._loadedPrefab, base.transform, false));
			}
			num3 = (flag ? Stopwatch.GetTimestamp() : 0L);
			this._payloadInstance.name = this._loadedPrefab.name + " [Payload]";
			this._loadedPrefab = null;
			if (this._gameplayManifest != null)
			{
				UniversityGameplayBinder.ValidateAndApply(this._payloadInstance, this._gameplayManifest);
				this._canonicalGameplayBound = true;
			}
			PoiResidencyPlacement component = base.GetComponent<PoiResidencyPlacement>();
			if (component != null)
			{
				component.BindPayload(this._payloadInstance);
			}
			configure(this._payloadInstance);
			if (component != null)
			{
				component.PrepareForActivation(this._payloadInstance, (this._payloadLoadingGate != null) ? this._payloadLoadingGate.transform : null);
			}
		}
		catch (Exception ex)
		{
			this.DiscardIncompletePayload();
			this.Fail("Payload setup failed: '" + this.resourcesPrefabPath + "'. " + ex.Message);
			return false;
		}
		HeavyZoneStreamingProxy.CompletedColdPayloadSetupCount += 1L;
		if (flag)
		{
			long timestamp = Stopwatch.GetTimestamp();
			HeavyZoneStreamingProxy.ColdPayloadSetupSample coldPayloadSetupSample = new HeavyZoneStreamingProxy.ColdPayloadSetupSample(HeavyZoneStreamingProxy.CompletedColdPayloadSetupCount, Time.frameCount, base.GetInstanceID(), num, (double)(num3 - num2) * 1000.0 / (double)Stopwatch.Frequency, (double)(timestamp - num3) * 1000.0 / (double)Stopwatch.Frequency, this.resourcesPrefabPath);
			try
			{
				Action<HeavyZoneStreamingProxy.ColdPayloadSetupSample> coldPayloadSetupMeasured = HeavyZoneStreamingProxy.ColdPayloadSetupMeasured;
				if (coldPayloadSetupMeasured != null)
				{
					coldPayloadSetupMeasured(coldPayloadSetupSample);
				}
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
			}
		}
		return true;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000275B4 File Offset: 0x000257B4
	private void ConfigureClonedPayload(GameObject payload)
	{
		Transform transform = payload.transform;
		transform.localPosition = this.payloadLocalOffset;
		transform.localRotation = Quaternion.identity;
		transform.localScale = this.payloadLocalScale;
		if (this.preserveAuthoredPayloadSettings)
		{
			return;
		}
		using (HeavyZoneStreamingProxy.TerrainAlignMarker.Auto())
		{
			this.AlignTerrainSurfaceToAnchor(payload);
		}
		using (HeavyZoneStreamingProxy.RendererLayersMarker.Auto())
		{
			HeavyZoneStreamingProxy.AssignStreamedVisualLayer(payload);
			HeavyZoneStreamingProxy.ApplyCameraCullingMasks();
		}
		using (HeavyZoneStreamingProxy.LodTuningMarker.Auto())
		{
			this.ApplyConservativeLodTuning(payload);
		}
		using (HeavyZoneStreamingProxy.TerrainClampMarker.Auto())
		{
			HeavyZoneStreamingProxy.ClampTerrainDistances(payload);
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000276C0 File Offset: 0x000258C0
	private void PublishPayloadReady()
	{
		if (!this.useStagedPooledActivation)
		{
			this.CacheColliders(this._payloadInstance);
		}
		this.IsReady = true;
		this.IsOperationalReady = !this.useStagedPooledActivation;
		this._loadRoutine = null;
		Action<HeavyZoneStreamingProxy> payloadReady = this.PayloadReady;
		if (payloadReady != null)
		{
			payloadReady(this);
		}
		if (this.useStagedPooledActivation)
		{
			this.ArmStagedWake();
			return;
		}
		Action<HeavyZoneStreamingProxy> operationalReady = this.OperationalReady;
		if (operationalReady == null)
		{
			return;
		}
		operationalReady(this);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00027730 File Offset: 0x00025930
	private static HeavyZoneStreamingProxy.SharedPayloadLoad GetOrStartSharedPayloadLoad(string resourcePath, HeavyZoneStreamingProxy consumer)
	{
		HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad;
		if (HeavyZoneStreamingProxy.SharedPayloadLoads.TryGetValue(resourcePath, out sharedPayloadLoad))
		{
			if (!sharedPayloadLoad.completed && consumer != null)
			{
				sharedPayloadLoad.consumers.Add(consumer);
				sharedPayloadLoad.releaseWhenCompleted = false;
			}
			sharedPayloadLoad.lastAccessSequence = (HeavyZoneStreamingProxy._sharedPayloadAccessSequence += 1UL);
			HeavyZoneStreamingProxy.RefreshSharedPayloadLoadPriority(sharedPayloadLoad);
			return sharedPayloadLoad;
		}
		HeavyZoneStreamingProxy.SharedPayloadLoad sharedLoad = new HeavyZoneStreamingProxy.SharedPayloadLoad
		{
			resourcePath = resourcePath,
			lastAccessSequence = (HeavyZoneStreamingProxy._sharedPayloadAccessSequence += 1UL)
		};
		if (consumer != null)
		{
			sharedLoad.consumers.Add(consumer);
		}
		HeavyZoneStreamingProxy.SharedPayloadLoads.Add(resourcePath, sharedLoad);
		HeavyZoneStreamingProxy.BeginSharedPayloadLoad();
		try
		{
			ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(resourcePath);
			sharedLoad.request = resourceRequest;
			HeavyZoneStreamingProxy.RefreshSharedPayloadLoadPriority(sharedLoad);
			resourceRequest.completed += delegate(AsyncOperation _)
			{
				HeavyZoneStreamingProxy.CompleteSharedPayloadLoad(sharedLoad);
			};
			if (resourceRequest.isDone)
			{
				HeavyZoneStreamingProxy.CompleteSharedPayloadLoad(sharedLoad);
			}
		}
		catch
		{
			sharedLoad.completed = true;
			HeavyZoneStreamingProxy.EndSharedPayloadLoad();
			throw;
		}
		return sharedLoad;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0002785C File Offset: 0x00025A5C
	private static void RefreshSharedPayloadLoadPriority(HeavyZoneStreamingProxy.SharedPayloadLoad sharedLoad)
	{
		if (((sharedLoad != null) ? sharedLoad.request : null) == null || sharedLoad.completed)
		{
			return;
		}
		int num = -1;
		foreach (HeavyZoneStreamingProxy heavyZoneStreamingProxy in sharedLoad.consumers)
		{
			if (heavyZoneStreamingProxy != null)
			{
				num = Mathf.Max(num, heavyZoneStreamingProxy._requestedLoadPriority);
			}
		}
		sharedLoad.request.priority = num;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000278E4 File Offset: 0x00025AE4
	private void UnregisterSharedPayloadLoadConsumer()
	{
		HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad;
		if (string.IsNullOrWhiteSpace(this.resourcesPrefabPath) || !HeavyZoneStreamingProxy.SharedPayloadLoads.TryGetValue(this.resourcesPrefabPath, out sharedPayloadLoad) || sharedPayloadLoad.completed)
		{
			return;
		}
		sharedPayloadLoad.consumers.Remove(this);
		HeavyZoneStreamingProxy.RefreshSharedPayloadLoadPriority(sharedPayloadLoad);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00027930 File Offset: 0x00025B30
	private static void CompleteSharedPayloadLoad(HeavyZoneStreamingProxy.SharedPayloadLoad sharedLoad)
	{
		if (sharedLoad == null || sharedLoad.completed)
		{
			return;
		}
		sharedLoad.asset = ((sharedLoad.request != null) ? (sharedLoad.request.asset as GameObject) : null);
		sharedLoad.request = null;
		sharedLoad.completed = true;
		foreach (HeavyZoneStreamingProxy heavyZoneStreamingProxy in sharedLoad.consumers)
		{
			if (heavyZoneStreamingProxy != null && heavyZoneStreamingProxy._loadRoutine != null && !heavyZoneStreamingProxy.IsReady)
			{
				heavyZoneStreamingProxy._loadedPrefab = sharedLoad.asset;
			}
		}
		sharedLoad.consumers.Clear();
		HeavyZoneStreamingProxy.EndSharedPayloadLoad();
		if (sharedLoad.releaseWhenCompleted)
		{
			HeavyZoneStreamingProxy.SharedPayloadLoad sharedPayloadLoad;
			if (!string.IsNullOrEmpty(sharedLoad.resourcePath) && HeavyZoneStreamingProxy.SharedPayloadLoads.TryGetValue(sharedLoad.resourcePath, out sharedPayloadLoad) && sharedPayloadLoad == sharedLoad)
			{
				HeavyZoneStreamingProxy.SharedPayloadLoads.Remove(sharedLoad.resourcePath);
				return;
			}
		}
		else
		{
			HeavyZoneStreamingProxy.TrimCompletedPayloadCache();
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00027A30 File Offset: 0x00025C30
	private static void TrimCompletedPayloadCache()
	{
		for (;;)
		{
			int num = 0;
			string text = null;
			ulong num2 = ulong.MaxValue;
			foreach (KeyValuePair<string, HeavyZoneStreamingProxy.SharedPayloadLoad> keyValuePair in HeavyZoneStreamingProxy.SharedPayloadLoads)
			{
				HeavyZoneStreamingProxy.SharedPayloadLoad value = keyValuePair.Value;
				if (value != null && value.completed)
				{
					num++;
					if (value.lastAccessSequence < num2)
					{
						num2 = value.lastAccessSequence;
						text = keyValuePair.Key;
					}
				}
			}
			if (num <= 2 || text == null)
			{
				break;
			}
			HeavyZoneStreamingProxy.SharedPayloadLoads.Remove(text);
		}
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00027ACC File Offset: 0x00025CCC
	private static void PruneDeadConsumers(HeavyZoneStreamingProxy.SharedPayloadLoad load)
	{
		if (load == null || load.consumers.Count == 0)
		{
			return;
		}
		load.consumers.RemoveWhere((HeavyZoneStreamingProxy consumer) => consumer == null);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00027B0A File Offset: 0x00025D0A
	internal static void BeginSharedPayloadLoad()
	{
		if (HeavyZoneStreamingProxy._activeSharedPayloadLoads == 0)
		{
			HeavyZoneStreamingProxy._backgroundPriorityBeforeSharedLoads = Application.backgroundLoadingPriority;
			Application.backgroundLoadingPriority = ThreadPriority.Low;
		}
		HeavyZoneStreamingProxy._activeSharedPayloadLoads++;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00027B2F File Offset: 0x00025D2F
	internal static void EndSharedPayloadLoad()
	{
		HeavyZoneStreamingProxy._activeSharedPayloadLoads = Mathf.Max(0, HeavyZoneStreamingProxy._activeSharedPayloadLoads - 1);
		if (HeavyZoneStreamingProxy._activeSharedPayloadLoads == 0)
		{
			Application.backgroundLoadingPriority = HeavyZoneStreamingProxy._backgroundPriorityBeforeSharedLoads;
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00027B54 File Offset: 0x00025D54
	public IEnumerator PrepareStagedDormancyAfterWarmup()
	{
		if (!this.useStagedPooledActivation || !this.IsReady || this._payloadInstance == null)
		{
			yield break;
		}
		if (!this._stagedSnapshotReady)
		{
			yield return this.BuildStagedSnapshotRoutine();
		}
		this.BeginStagedSleep();
		int operations = 0;
		long num = Stopwatch.GetTimestamp();
		while (this.ProcessOneStagedSleepStep())
		{
			int num2 = operations;
			operations = num2 + 1;
			if (this.StagedBudgetExpired(operations, num))
			{
				operations = 0;
				yield return null;
				num = Stopwatch.GetTimestamp();
			}
		}
		yield break;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00027B63 File Offset: 0x00025D63
	public void ArmStagedWake()
	{
		if (!this.useStagedPooledActivation)
		{
			return;
		}
		this._stagedWakeArmed = true;
		if (!this.IsReady)
		{
			this.IsOperationalReady = false;
			return;
		}
		this.IsOperationalReady = !this._stagedDormant;
		this.TryStartArmedStagedWake();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00027B9A File Offset: 0x00025D9A
	public void SetChunkCollidersEnabled(bool enabled)
	{
		if (!this.useStagedPooledActivation)
		{
			return;
		}
		bool flag = this._chunkCollidersEnabled != enabled;
		this._chunkCollidersEnabled = enabled;
		if (!flag || !this.IsOperationalReady || !this._stagedSnapshotReady)
		{
			return;
		}
		this.ScheduleRuntimeColliderStateUpdate();
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00027BD4 File Offset: 0x00025DD4
	public void BeginStagedSleep()
	{
		if (!this.useStagedPooledActivation || this._stagedObjects.Count == 0)
		{
			return;
		}
		this._stagedCollidersSuppressed = false;
		this.HideStagedVisuals();
		if (this._stagedWakeRoutine != null)
		{
			base.StopCoroutine(this._stagedWakeRoutine);
			this._stagedWakeRoutine = null;
		}
		this._stagedWakeArmed = false;
		this.IsOperationalReady = false;
		this._runtimeColliderUpdateIndex = -1;
		this._stagedSleepIndex = (this._stagedDormant ? Mathf.Min(this._stagedWakeIndex - 1, this._stagedObjects.Count - 1) : (this._stagedObjects.Count - 1));
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00027C6C File Offset: 0x00025E6C
	public bool ProcessOneStagedSleepStep()
	{
		if (this._stagedSleepIndex < 0)
		{
			this.SuppressStagedColliderActivation();
			this._stagedDormant = true;
			this._stagedWakeIndex = 0;
			return false;
		}
		using (HeavyZoneStreamingProxy.StagedSleepMarker.Auto())
		{
			List<GameObject> stagedObjects = this._stagedObjects;
			int stagedSleepIndex = this._stagedSleepIndex;
			this._stagedSleepIndex = stagedSleepIndex - 1;
			GameObject gameObject = stagedObjects[stagedSleepIndex];
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		if (this._stagedSleepIndex < 0)
		{
			this.SuppressStagedColliderActivation();
			this._stagedDormant = true;
			this._stagedWakeIndex = 0;
		}
		return true;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00027D1C File Offset: 0x00025F1C
	private IEnumerator BuildStagedSnapshotRoutine()
	{
		if (this._stagedSnapshotReady || this._payloadInstance == null)
		{
			yield break;
		}
		this._stagedObjects.Clear();
		this._stagedActiveDefaults.Clear();
		this._colliders.Clear();
		this._renderers.Clear();
		this._stagedObjects.Add(this._payloadInstance);
		int readIndex = 0;
		int operations = 0;
		long num = Stopwatch.GetTimestamp();
		Matrix4x4 matrix4x = base.transform.worldToLocalMatrix;
		while (readIndex < this._stagedObjects.Count)
		{
			List<GameObject> stagedObjects = this._stagedObjects;
			int num2 = readIndex;
			readIndex = num2 + 1;
			GameObject gameObject = stagedObjects[num2];
			if (gameObject != null)
			{
				this._stagedActiveDefaults.Add(gameObject.activeSelf);
				Transform transform = gameObject.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					this._stagedObjects.Add(transform.GetChild(i).gameObject);
				}
				this._colliderComponentScratch.Clear();
				gameObject.GetComponents<Collider>(this._colliderComponentScratch);
				for (int j = 0; j < this._colliderComponentScratch.Count; j++)
				{
					Collider collider = this._colliderComponentScratch[j];
					if (!(collider == null))
					{
						Bounds bounds = collider.bounds;
						HeavyZoneStreamingProxy.ColliderState colliderState = new HeavyZoneStreamingProxy.ColliderState
						{
							component = collider,
							authoredEnabled = collider.enabled,
							isTrigger = collider.isTrigger,
							worldBounds = bounds
						};
						HeavyZoneStreamingProxy.CaptureProxyLocalBounds(ref colliderState, matrix4x);
						HeavyZoneStreamingProxy.CaptureProxyLocalBoundsFromGeometry(ref colliderState, matrix4x);
						this._colliders.Add(colliderState);
					}
				}
				if (this.atomicVisualReveal)
				{
					this._rendererComponentScratch.Clear();
					gameObject.GetComponents<Renderer>(this._rendererComponentScratch);
					for (int k = 0; k < this._rendererComponentScratch.Count; k++)
					{
						Renderer renderer = this._rendererComponentScratch[k];
						if (!(renderer == null))
						{
							this._renderers.Add(new HeavyZoneStreamingProxy.RendererState
							{
								component = renderer,
								authoredForceRenderingOff = renderer.forceRenderingOff
							});
							renderer.forceRenderingOff = true;
						}
					}
				}
				operations += 1 + this._colliderComponentScratch.Count + (this.atomicVisualReveal ? this._rendererComponentScratch.Count : 0);
			}
			else
			{
				this._stagedActiveDefaults.Add(false);
				num2 = operations;
				operations = num2 + 1;
			}
			if (this.StagedBudgetExpired(operations, num))
			{
				operations = 0;
				yield return null;
				num = Stopwatch.GetTimestamp();
				matrix4x = base.transform.worldToLocalMatrix;
			}
		}
		this._colliderComponentScratch.Clear();
		this._rendererComponentScratch.Clear();
		this._stagedSnapshotReady = true;
		yield break;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00027D2C File Offset: 0x00025F2C
	private GameObject InstantiatePayloadBehindInactiveGate(GameObject prefab)
	{
		this._payloadLoadingGate = new GameObject(base.name + " [Payload Loading Gate]");
		this._payloadLoadingGate.SetActive(false);
		Transform transform = this._payloadLoadingGate.transform;
		transform.SetParent(base.transform, false);
		return Object.Instantiate<GameObject>(prefab, transform, false);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00027D81 File Offset: 0x00025F81
	private IEnumerator PrepareInitialStagedDormancyRoutine()
	{
		this.BeginStagedSleep();
		int operations = 0;
		long num = Stopwatch.GetTimestamp();
		while (this.ProcessOneStagedSleepStep())
		{
			int num2 = operations;
			operations = num2 + 1;
			if (this.StagedBudgetExpired(operations, num))
			{
				operations = 0;
				yield return null;
				num = Stopwatch.GetTimestamp();
			}
		}
		this.ReleasePayloadFromInactiveLoadingGate();
		yield break;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00027D90 File Offset: 0x00025F90
	private void ReleasePayloadFromInactiveLoadingGate()
	{
		if (this._payloadInstance != null)
		{
			this._payloadInstance.transform.SetParent(base.transform, true);
		}
		GameObject payloadLoadingGate = this._payloadLoadingGate;
		this._payloadLoadingGate = null;
		if (payloadLoadingGate == null)
		{
			return;
		}
		if (Application.isPlaying)
		{
			Object.Destroy(payloadLoadingGate);
			return;
		}
		Object.DestroyImmediate(payloadLoadingGate);
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00027DF0 File Offset: 0x00025FF0
	private void DiscardIncompletePayload()
	{
		GameObject payloadInstance = this._payloadInstance;
		GameObject payloadLoadingGate = this._payloadLoadingGate;
		this._payloadInstance = null;
		this._payloadLoadingGate = null;
		this._loadedPrefab = null;
		this._stagedObjects.Clear();
		this._stagedActiveDefaults.Clear();
		this._colliders.Clear();
		this._renderers.Clear();
		this._rendererComponentScratch.Clear();
		this._stagedSnapshotReady = false;
		this._stagedDormant = false;
		this._stagedCollidersSuppressed = false;
		this._stagedWakeArmed = false;
		this._stagedWakeIndex = 0;
		this._stagedSleepIndex = -1;
		this.IsReady = false;
		this.IsOperationalReady = false;
		if (payloadInstance != null && (payloadLoadingGate == null || payloadInstance.transform.parent != payloadLoadingGate.transform))
		{
			HeavyZoneStreamingProxy.DestroyIncompletePayloadObject(payloadInstance);
		}
		if (payloadLoadingGate != null)
		{
			HeavyZoneStreamingProxy.DestroyIncompletePayloadObject(payloadLoadingGate);
		}
		this.ReleaseAddressablePayloadAfterDestruction(new Object[] { payloadInstance, payloadLoadingGate });
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00027EE2 File Offset: 0x000260E2
	private static void DestroyIncompletePayloadObject(GameObject target)
	{
		if (Application.isPlaying)
		{
			Object.Destroy(target);
			return;
		}
		Object.DestroyImmediate(target);
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00027EF8 File Offset: 0x000260F8
	private void ReleaseAddressablePayloadAfterDestruction(params Object[] guards)
	{
		HeavyZoneAddressableResidency.Lease addressableLease = this._addressableLease;
		this._addressableLease = null;
		this._gameplayManifest = null;
		this._canonicalGameplayBound = false;
		if (addressableLease != null)
		{
			HeavyZoneAddressableResidency.ReleaseAfterDestroyed(addressableLease, guards);
		}
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00027F2C File Offset: 0x0002612C
	private void HideStagedVisuals()
	{
		if (!this.atomicVisualReveal)
		{
			return;
		}
		for (int i = 0; i < this._renderers.Count; i++)
		{
			Renderer component = this._renderers[i].component;
			if (component != null)
			{
				component.forceRenderingOff = true;
			}
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00027F7C File Offset: 0x0002617C
	private void RevealStagedVisuals()
	{
		if (!this.atomicVisualReveal)
		{
			return;
		}
		for (int i = 0; i < this._renderers.Count; i++)
		{
			HeavyZoneStreamingProxy.RendererState rendererState = this._renderers[i];
			if (rendererState.component != null)
			{
				rendererState.component.forceRenderingOff = rendererState.authoredForceRenderingOff;
			}
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00027FD4 File Offset: 0x000261D4
	private IEnumerator FinalizeStagedColliderStatesRoutine()
	{
		this.RefreshNavigationColliderBounds();
		this._stagedColliderIndex = 0;
		Camera wakeCamera = Camera.main;
		bool flag = wakeCamera != null;
		Vector3 vector = (flag ? wakeCamera.transform.position : Vector3.zero);
		Matrix4x4 matrix4x = base.transform.worldToLocalMatrix;
		Matrix4x4 matrix4x2 = base.transform.localToWorldMatrix;
		int operations = 0;
		long num = Stopwatch.GetTimestamp();
		while (this._stagedColliderIndex < this._colliders.Count)
		{
			int num2;
			using (HeavyZoneStreamingProxy.StagedColliderMarker.Auto())
			{
				num2 = this._stagedColliderIndex;
				this._stagedColliderIndex = num2 + 1;
				int num3 = num2;
				this.CaptureColliderBoundsAfterWakeIfNeeded(num3, matrix4x);
				this.RefreshColliderWorldBoundsFromSnapshot(num3, matrix4x2);
				this.ApplyColliderState(num3, flag, vector);
			}
			num2 = operations;
			operations = num2 + 1;
			if (this.StagedBudgetExpired(operations, num))
			{
				operations = 0;
				yield return null;
				num = Stopwatch.GetTimestamp();
				matrix4x = base.transform.worldToLocalMatrix;
				matrix4x2 = base.transform.localToWorldMatrix;
				flag = wakeCamera != null;
				vector = (flag ? wakeCamera.transform.position : Vector3.zero);
			}
		}
		this._hasCullPosition = flag;
		this._lastCullPosition = vector;
		this.FlushRestoredNavigationColliders();
		yield break;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00027FE4 File Offset: 0x000261E4
	private void CaptureColliderBoundsAfterWakeIfNeeded(int index, Matrix4x4 proxyWorldToLocal)
	{
		HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[index];
		Collider component = colliderState.component;
		if (colliderState.hasProxyLocalBounds || component == null || !component.enabled || !component.gameObject.activeInHierarchy)
		{
			return;
		}
		colliderState.worldBounds = component.bounds;
		HeavyZoneStreamingProxy.CaptureProxyLocalBounds(ref colliderState, proxyWorldToLocal);
		this._colliders[index] = colliderState;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0002804E File Offset: 0x0002624E
	private IEnumerator StagedWakeRoutine()
	{
		yield return null;
		int operations = 0;
		long num = Stopwatch.GetTimestamp();
		while (this._stagedWakeIndex < this._stagedObjects.Count)
		{
			int num2;
			using (HeavyZoneStreamingProxy.StagedWakeMarker.Auto())
			{
				num2 = this._stagedWakeIndex;
				this._stagedWakeIndex = num2 + 1;
				int num3 = num2;
				GameObject gameObject = this._stagedObjects[num3];
				if (gameObject != null && gameObject.activeSelf != this._stagedActiveDefaults[num3])
				{
					gameObject.SetActive(this._stagedActiveDefaults[num3]);
				}
			}
			num2 = operations;
			operations = num2 + 1;
			if (this.StagedBudgetExpired(operations, num))
			{
				operations = 0;
				yield return null;
				num = Stopwatch.GetTimestamp();
			}
		}
		yield return this.FinalizeStagedColliderStatesRoutine();
		this._stagedDormant = false;
		this._stagedWakeArmed = false;
		this._stagedWakeRoutine = null;
		this.RevealStagedVisuals();
		this.IsOperationalReady = true;
		Action<HeavyZoneStreamingProxy> operationalReady = this.OperationalReady;
		if (operationalReady != null)
		{
			operationalReady(this);
		}
		yield break;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00028060 File Offset: 0x00026260
	private void TryStartArmedStagedWake()
	{
		if (!Application.isPlaying || !base.isActiveAndEnabled || !this.IsReady || !this._stagedDormant || !this._stagedWakeArmed || this._stagedWakeRoutine != null)
		{
			return;
		}
		this._stagedWakeRoutine = base.StartCoroutine(this.StagedWakeRoutine());
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000280B0 File Offset: 0x000262B0
	private bool StagedBudgetExpired(int operations, long frameStart)
	{
		int num = (this._urgentStreamingWork ? Mathf.Max(this.stagedMaxOperationsPerFrame, this.stagedMaxOperationsPerFrame * 4) : Mathf.Max(1, this.stagedMaxOperationsPerFrame));
		float num2 = (this._urgentStreamingWork ? Mathf.Max(this.stagedFrameBudgetMs, 2f) : this.stagedFrameBudgetMs);
		long num3 = Math.Max(1L, (long)Math.Ceiling((double)((float)Stopwatch.Frequency * num2) / 1000.0));
		return operations >= num || Stopwatch.GetTimestamp() - frameStart >= num3;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0002813C File Offset: 0x0002633C
	private void AlignTerrainSurfaceToAnchor(GameObject payload)
	{
		if (!this.alignTerrainSurfaceToAnchor || payload == null)
		{
			return;
		}
		Terrain[] componentsInChildren = payload.GetComponentsInChildren<Terrain>(true);
		if (componentsInChildren.Length == 0)
		{
			return;
		}
		List<float> list = new List<float>(componentsInChildren.Length * 5);
		foreach (Terrain terrain in componentsInChildren)
		{
			TerrainData terrainData = ((terrain != null) ? terrain.terrainData : null);
			if (!(terrainData == null))
			{
				float y = terrain.transform.position.y;
				float y2 = terrain.transform.lossyScale.y;
				list.Add(y + terrainData.GetInterpolatedHeight(0.5f, 0.5f) * y2);
				list.Add(y + terrainData.GetInterpolatedHeight(0.1f, 0.1f) * y2);
				list.Add(y + terrainData.GetInterpolatedHeight(0.9f, 0.1f) * y2);
				list.Add(y + terrainData.GetInterpolatedHeight(0.1f, 0.9f) * y2);
				list.Add(y + terrainData.GetInterpolatedHeight(0.9f, 0.9f) * y2);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		list.Sort();
		float num = list[list.Count / 2];
		float num2 = base.transform.position.y + this.terrainSurfaceYOffset - num;
		payload.transform.position += Vector3.up * num2;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000282C4 File Offset: 0x000264C4
	private void Update()
	{
		if (!this.preserveAuthoredPayloadSettings && Time.unscaledTime >= HeavyZoneStreamingProxy._nextCameraMaskUpdate)
		{
			HeavyZoneStreamingProxy._nextCameraMaskUpdate = Time.unscaledTime + 1f;
			HeavyZoneStreamingProxy.ApplyCameraCullingMasks();
		}
		if (!this.IsOperationalReady || this._colliders.Count == 0)
		{
			return;
		}
		if (this.useStagedPooledActivation)
		{
			if (this._runtimeColliderUpdateIndex < 0 && Time.unscaledTime >= this._nextColliderUpdate)
			{
				this._nextColliderUpdate = Time.unscaledTime + this.colliderUpdateInterval;
				Camera main = Camera.main;
				Vector3 vector = ((main != null) ? main.transform.position : Vector3.zero);
				if (this.RefreshNavigationColliderBounds() || !this._hasCullPosition || main == null || (vector - this._lastCullPosition).sqrMagnitude >= this.colliderMoveThreshold * this.colliderMoveThreshold)
				{
					this.ScheduleRuntimeColliderStateUpdate();
				}
			}
			this.ProcessRuntimeColliderStateBudget();
			return;
		}
		if (Time.unscaledTime < this._nextColliderUpdate)
		{
			return;
		}
		this._nextColliderUpdate = Time.unscaledTime + this.colliderUpdateInterval;
		this.UpdateColliderStates(false);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000283D8 File Offset: 0x000265D8
	private static void AssignStreamedVisualLayer(GameObject payload)
	{
		Transform transform = HeavyZoneStreamingProxy.FindDescendant(payload.transform, "CombinedRender");
		if (transform != null)
		{
			HeavyZoneStreamingProxy.SetLayerRecursively(transform, 3);
		}
		Renderer[] componentsInChildren = payload.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			GameObject gameObject = componentsInChildren[i].gameObject;
			if (gameObject.GetComponent<Collider>() == null && gameObject.GetComponents<MonoBehaviour>().Length == 0)
			{
				gameObject.layer = 3;
			}
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00028444 File Offset: 0x00026644
	private static void ApplyCameraCullingMasks()
	{
		int num = 8;
		Camera[] array = Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		Camera main = Camera.main;
		foreach (Camera camera in array)
		{
			if (!(camera == null) && camera.gameObject.activeInHierarchy)
			{
				if (camera == main || camera.CompareTag("MainCamera"))
				{
					camera.cullingMask |= num;
					float[] layerCullDistances = camera.layerCullDistances;
					layerCullDistances[3] = 0f;
					camera.layerCullDistances = layerCullDistances;
					camera.layerCullSpherical = true;
				}
				else if (camera.name == "TVCam" || camera.name == "PlayerFaceCam")
				{
					camera.cullingMask &= ~num;
				}
			}
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0002851C File Offset: 0x0002671C
	private static Transform FindDescendant(Transform root, string targetName)
	{
		if (root.name == targetName)
		{
			return root;
		}
		for (int i = 0; i < root.childCount; i++)
		{
			Transform transform = HeavyZoneStreamingProxy.FindDescendant(root.GetChild(i), targetName);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00028564 File Offset: 0x00026764
	private static void SetLayerRecursively(Transform root, int layer)
	{
		root.gameObject.layer = layer;
		for (int i = 0; i < root.childCount; i++)
		{
			HeavyZoneStreamingProxy.SetLayerRecursively(root.GetChild(i), layer);
		}
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0002859C File Offset: 0x0002679C
	private void ApplyConservativeLodTuning(GameObject payload)
	{
		if (this.lodTransitionMultiplier <= 1.001f)
		{
			return;
		}
		foreach (LODGroup lodgroup in payload.GetComponentsInChildren<LODGroup>(true))
		{
			if (!(lodgroup == null) && lodgroup.enabled)
			{
				LOD[] lods = lodgroup.GetLODs();
				float num = 0.999f;
				bool flag = false;
				for (int j = 0; j < lods.Length; j++)
				{
					float screenRelativeTransitionHeight = lods[j].screenRelativeTransitionHeight;
					float num2 = Mathf.Clamp(screenRelativeTransitionHeight * this.lodTransitionMultiplier, 0.0001f, 0.999f);
					if (num2 >= num)
					{
						num2 = Mathf.Max(0.0001f, num * 0.75f);
					}
					flag |= !Mathf.Approximately(screenRelativeTransitionHeight, num2);
					lods[j].screenRelativeTransitionHeight = num2;
					num = num2;
				}
				if (flag)
				{
					lodgroup.SetLODs(lods);
				}
			}
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00028684 File Offset: 0x00026884
	private static void ClampTerrainDistances(GameObject payload)
	{
		foreach (Terrain terrain in payload.GetComponentsInChildren<Terrain>(true))
		{
			if (!(terrain == null))
			{
				terrain.detailObjectDistance = Mathf.Min(terrain.detailObjectDistance, 45f);
				terrain.treeDistance = Mathf.Min(terrain.treeDistance, 150f);
				terrain.treeBillboardDistance = Mathf.Min(terrain.treeBillboardDistance, 80f);
				terrain.basemapDistance = Mathf.Min(terrain.basemapDistance, 75f);
				terrain.heightmapPixelError = Mathf.Max(terrain.heightmapPixelError, 12f);
			}
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00028728 File Offset: 0x00026928
	private void CacheColliders(GameObject payload)
	{
		this._colliders.Clear();
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		foreach (Collider collider in payload.GetComponentsInChildren<Collider>(true))
		{
			if (!(collider == null))
			{
				HeavyZoneStreamingProxy.ColliderState colliderState = new HeavyZoneStreamingProxy.ColliderState
				{
					component = collider,
					authoredEnabled = collider.enabled,
					isTrigger = collider.isTrigger,
					worldBounds = collider.bounds
				};
				HeavyZoneStreamingProxy.CaptureProxyLocalBoundsFromGeometry(ref colliderState, worldToLocalMatrix);
				this._colliders.Add(colliderState);
				if (colliderState.hasProxyLocalBounds)
				{
					this.RefreshColliderWorldBoundsFromSnapshot(this._colliders.Count - 1, base.transform.localToWorldMatrix);
				}
			}
		}
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x000287EC File Offset: 0x000269EC
	private void RefreshColliderBounds()
	{
		bool flag = false;
		for (int i = 0; i < this._colliders.Count; i++)
		{
			HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[i];
			if (!(colliderState.component == null) && colliderState.authoredEnabled && !colliderState.component.enabled)
			{
				colliderState.component.enabled = true;
				flag = true;
			}
		}
		if (flag)
		{
			Physics.SyncTransforms();
		}
		for (int j = 0; j < this._colliders.Count; j++)
		{
			HeavyZoneStreamingProxy.ColliderState colliderState2 = this._colliders[j];
			if (!(colliderState2.component == null))
			{
				colliderState2.worldBounds = colliderState2.component.bounds;
				this._colliders[j] = colliderState2;
			}
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x000288AC File Offset: 0x00026AAC
	private void UpdateColliderStates(bool force)
	{
		bool flag = this.RefreshNavigationColliderBounds();
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		Vector3 position = main.transform.position;
		if (!force && !flag && this._hasCullPosition && (position - this._lastCullPosition).sqrMagnitude < this.colliderMoveThreshold * this.colliderMoveThreshold)
		{
			return;
		}
		this._lastCullPosition = position;
		this._hasCullPosition = true;
		float num = this.colliderDistance * this.colliderDistance;
		for (int i = 0; i < this._colliders.Count; i++)
		{
			HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[i];
			Collider component = colliderState.component;
			if (!(component == null))
			{
				bool flag2 = colliderState.authoredEnabled && (this.preserveAuthoredPayloadSettings || colliderState.isTrigger || colliderState.worldBounds.SqrDistance(position) <= num || this.IntersectsNavigationColliderBounds(colliderState.worldBounds));
				if (component.enabled != flag2)
				{
					component.enabled = flag2;
					this.TrackRestoredNavigationCollider(colliderState, flag2);
				}
			}
		}
		this.FlushRestoredNavigationColliders();
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x000289D0 File Offset: 0x00026BD0
	private void ScheduleRuntimeColliderStateUpdate()
	{
		this.RefreshNavigationColliderBounds();
		Camera main = Camera.main;
		bool flag;
		if (main != null)
		{
			PoiResidencyPlacement component = base.GetComponent<PoiResidencyPlacement>();
			flag = component == null || !component.HasActiveGameplayUse;
		}
		else
		{
			flag = false;
		}
		this._runtimeColliderUpdateHasCamera = flag;
		this._runtimeColliderUpdatePosition = ((main != null) ? main.transform.position : Vector3.zero);
		this._runtimeColliderUpdateIndex = 0;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00028A3C File Offset: 0x00026C3C
	private void ProcessRuntimeColliderStateBudget()
	{
		if (this._runtimeColliderUpdateIndex < 0)
		{
			return;
		}
		int num = 0;
		long timestamp = Stopwatch.GetTimestamp();
		while (this._runtimeColliderUpdateIndex < this._colliders.Count)
		{
			int runtimeColliderUpdateIndex = this._runtimeColliderUpdateIndex;
			this._runtimeColliderUpdateIndex = runtimeColliderUpdateIndex + 1;
			this.ApplyColliderState(runtimeColliderUpdateIndex, this._runtimeColliderUpdateHasCamera, this._runtimeColliderUpdatePosition);
			num++;
			if (this.StagedBudgetExpired(num, timestamp))
			{
				return;
			}
		}
		this._runtimeColliderUpdateIndex = -1;
		this._hasCullPosition = this._runtimeColliderUpdateHasCamera;
		this._lastCullPosition = this._runtimeColliderUpdatePosition;
		this.FlushRestoredNavigationColliders();
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00028AC8 File Offset: 0x00026CC8
	private void ApplyColliderState(int index, bool hasCamera, Vector3 cameraPosition)
	{
		HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[index];
		Collider component = colliderState.component;
		if (component == null)
		{
			return;
		}
		bool flag = colliderState.authoredEnabled && this._chunkCollidersEnabled && (this.preserveAuthoredPayloadSettings || colliderState.isTrigger || !hasCamera || !colliderState.hasProxyLocalBounds || colliderState.worldBounds.SqrDistance(cameraPosition) <= this.colliderDistance * this.colliderDistance || this.IntersectsNavigationColliderBounds(colliderState.worldBounds));
		if (component.enabled != flag)
		{
			component.enabled = flag;
			this.TrackRestoredNavigationCollider(colliderState, flag);
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00028B68 File Offset: 0x00026D68
	private bool RefreshNavigationColliderBounds()
	{
		Bounds? bounds = null;
		NavGraph[] array;
		if (!(AstarPath.active != null))
		{
			array = null;
		}
		else
		{
			AstarData data = AstarPath.active.data;
			array = ((data != null) ? data.graphs : null);
		}
		NavGraph[] array2 = array;
		if (array2 != null)
		{
			for (int i = 0; i < array2.Length; i++)
			{
				RecastGraph recastGraph = array2[i] as RecastGraph;
				if (recastGraph != null && recastGraph.rasterizeColliders)
				{
					Bounds bounds2 = new Bounds(recastGraph.forcedBoundsCenter, recastGraph.forcedBoundsSize);
					float num = (float)recastGraph.editorTileSize * recastGraph.cellSize + this.colliderMoveThreshold;
					bounds2.Expand(new Vector3(num * 2f, 0f, num * 2f));
					if (bounds != null)
					{
						Bounds value = bounds.Value;
						value.Encapsulate(bounds2);
						bounds = new Bounds?(value);
					}
					else
					{
						bounds = new Bounds?(bounds2);
					}
				}
			}
		}
		bool flag = !Nullable.Equals<Bounds>(this._navigationColliderBounds, bounds);
		this._navigationColliderBounds = bounds;
		return flag;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00028C68 File Offset: 0x00026E68
	private bool IntersectsNavigationColliderBounds(Bounds bounds)
	{
		if (this._navigationColliderBounds == null)
		{
			return false;
		}
		Bounds value = this._navigationColliderBounds.Value;
		return bounds.max.x >= value.min.x && bounds.min.x <= value.max.x && bounds.max.z >= value.min.z && bounds.min.z <= value.max.z;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00028D00 File Offset: 0x00026F00
	private void TrackRestoredNavigationCollider(HeavyZoneStreamingProxy.ColliderState state, bool enabled)
	{
		if (!enabled || state.isTrigger || !this.IntersectsNavigationColliderBounds(state.worldBounds))
		{
			return;
		}
		Bounds bounds = this._restoredNavigationColliderBounds ?? state.worldBounds;
		bounds.Encapsulate(state.worldBounds);
		this._restoredNavigationColliderBounds = new Bounds?(bounds);
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00028D60 File Offset: 0x00026F60
	private void FlushRestoredNavigationColliders()
	{
		if (this._restoredNavigationColliderBounds == null)
		{
			return;
		}
		Bounds value = this._restoredNavigationColliderBounds.Value;
		this._restoredNavigationColliderBounds = null;
		AStarGraphManager instance = AStarGraphManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.OnChunkZonesReady(new Vector3(value.center.x, value.min.y, value.center.z), value.size);
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00028DD4 File Offset: 0x00026FD4
	private void SuppressStagedColliderActivation()
	{
		if (this._stagedCollidersSuppressed)
		{
			return;
		}
		for (int i = 0; i < this._colliders.Count; i++)
		{
			Collider component = this._colliders[i].component;
			if (component != null && component.enabled)
			{
				component.enabled = false;
			}
		}
		this._stagedCollidersSuppressed = true;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00028E34 File Offset: 0x00027034
	private static void CaptureProxyLocalBoundsFromGeometry(ref HeavyZoneStreamingProxy.ColliderState state, Matrix4x4 proxyWorldToLocal)
	{
		Bounds bounds;
		if (state.hasProxyLocalBounds || state.component == null || !HeavyZoneStreamingProxy.TryGetColliderLocalBounds(state.component, out bounds))
		{
			return;
		}
		Matrix4x4 matrix4x = proxyWorldToLocal * state.component.transform.localToWorldMatrix;
		Vector3 extents = bounds.extents;
		state.proxyLocalBoundsCenter = matrix4x.MultiplyPoint3x4(bounds.center);
		state.proxyLocalBoundsAxisX = matrix4x.MultiplyVector(new Vector3(extents.x, 0f, 0f));
		state.proxyLocalBoundsAxisY = matrix4x.MultiplyVector(new Vector3(0f, extents.y, 0f));
		state.proxyLocalBoundsAxisZ = matrix4x.MultiplyVector(new Vector3(0f, 0f, extents.z));
		state.hasProxyLocalBounds = true;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00028F08 File Offset: 0x00027108
	private static bool TryGetColliderLocalBounds(Collider component, out Bounds localBounds)
	{
		BoxCollider boxCollider = component as BoxCollider;
		if (boxCollider != null)
		{
			localBounds = new Bounds(boxCollider.center, boxCollider.size);
			return HeavyZoneStreamingProxy.IsFinite(localBounds);
		}
		MeshCollider meshCollider = component as MeshCollider;
		if (meshCollider != null && meshCollider.sharedMesh != null)
		{
			localBounds = meshCollider.sharedMesh.bounds;
			return HeavyZoneStreamingProxy.IsFinite(localBounds);
		}
		TerrainCollider terrainCollider = component as TerrainCollider;
		if (terrainCollider != null && terrainCollider.terrainData != null)
		{
			Vector3 size = terrainCollider.terrainData.size;
			localBounds = new Bounds(size * 0.5f, size);
			return HeavyZoneStreamingProxy.IsFinite(localBounds);
		}
		localBounds = default(Bounds);
		return false;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00028FC8 File Offset: 0x000271C8
	private static void CaptureProxyLocalBounds(ref HeavyZoneStreamingProxy.ColliderState state, Matrix4x4 proxyWorldToLocal)
	{
		Collider component = state.component;
		if (component == null || !component.enabled || !component.gameObject.activeInHierarchy || !HeavyZoneStreamingProxy.IsFinite(state.worldBounds))
		{
			return;
		}
		Vector3 extents = state.worldBounds.extents;
		state.proxyLocalBoundsCenter = proxyWorldToLocal.MultiplyPoint3x4(state.worldBounds.center);
		state.proxyLocalBoundsAxisX = proxyWorldToLocal.MultiplyVector(new Vector3(extents.x, 0f, 0f));
		state.proxyLocalBoundsAxisY = proxyWorldToLocal.MultiplyVector(new Vector3(0f, extents.y, 0f));
		state.proxyLocalBoundsAxisZ = proxyWorldToLocal.MultiplyVector(new Vector3(0f, 0f, extents.z));
		state.hasProxyLocalBounds = true;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0002909C File Offset: 0x0002729C
	private void RefreshColliderWorldBoundsFromSnapshot(int index, Matrix4x4 proxyLocalToWorld)
	{
		HeavyZoneStreamingProxy.ColliderState colliderState = this._colliders[index];
		if (!colliderState.hasProxyLocalBounds)
		{
			return;
		}
		Vector3 vector = proxyLocalToWorld.MultiplyVector(colliderState.proxyLocalBoundsAxisX);
		Vector3 vector2 = proxyLocalToWorld.MultiplyVector(colliderState.proxyLocalBoundsAxisY);
		Vector3 vector3 = proxyLocalToWorld.MultiplyVector(colliderState.proxyLocalBoundsAxisZ);
		Vector3 vector4 = new Vector3(Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x), Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y), Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z));
		colliderState.worldBounds = new Bounds(proxyLocalToWorld.MultiplyPoint3x4(colliderState.proxyLocalBoundsCenter), vector4 * 2f);
		this._colliders[index] = colliderState;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0002918C File Offset: 0x0002738C
	private static bool IsFinite(Bounds bounds)
	{
		return HeavyZoneStreamingProxy.IsFinite(bounds.center.x) && HeavyZoneStreamingProxy.IsFinite(bounds.center.y) && HeavyZoneStreamingProxy.IsFinite(bounds.center.z) && HeavyZoneStreamingProxy.IsFinite(bounds.extents.x) && HeavyZoneStreamingProxy.IsFinite(bounds.extents.y) && HeavyZoneStreamingProxy.IsFinite(bounds.extents.z);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0002920B File Offset: 0x0002740B
	private static bool IsFinite(float value)
	{
		return !float.IsNaN(value) && !float.IsInfinity(value);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00029220 File Offset: 0x00027420
	private void Fail(string reason)
	{
		this.LoadFailed = true;
		this._loadRoutine = null;
		Debug.LogError("[HeavyZoneStreamingProxy] " + reason, this);
		Action<HeavyZoneStreamingProxy, string> payloadFailed = this.PayloadFailed;
		if (payloadFailed == null)
		{
			return;
		}
		payloadFailed(this, reason);
	}

	// Token: 0x040003D2 RID: 978
	[Header("Lazy payload")]
	[SerializeField]
	private string resourcesPrefabPath;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	private bool neverPool = true;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private bool useStagedPooledActivation;

	// Token: 0x040003D5 RID: 981
	[Tooltip("Keep every payload renderer hidden while the hierarchy wakes, then reveal the completed POI in one frame.")]
	[SerializeField]
	private bool atomicVisualReveal;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	[Min(1f)]
	private int stagedMaxOperationsPerFrame = 128;

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	[Min(0.1f)]
	private float stagedFrameBudgetMs = 1f;

	// Token: 0x040003D8 RID: 984
	[Tooltip("Optional fine alignment applied before automatic ground matching.")]
	[SerializeField]
	private Vector3 payloadLocalOffset = Vector3.zero;

	// Token: 0x040003D9 RID: 985
	[Tooltip("Uniform footprint fit for the authored payload. Park 3x3 uses the same 0.955 XZ scale as NormalSquareChunk Base - 3x3.")]
	[SerializeField]
	private Vector3 payloadLocalScale = Vector3.one;

	// Token: 0x040003DA RID: 986
	[Tooltip("Aligns the actual median Terrain surface (not merely each Terrain transform) to the procedural chunk plane.")]
	[SerializeField]
	private bool alignTerrainSurfaceToAnchor = true;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	private float terrainSurfaceYOffset;

	// Token: 0x040003DC RID: 988
	[Tooltip("Direct-prefab POIs keep their authored rendering, LOD, terrain and collider policy when moved behind a loading boundary.")]
	[SerializeField]
	private bool preserveAuthoredPayloadSettings;

	// Token: 0x040003DD RID: 989
	[Header("Conservative runtime tuning")]
	[SerializeField]
	[Range(1f, 4f)]
	private float lodTransitionMultiplier = 2.25f;

	// Token: 0x040003DE RID: 990
	[SerializeField]
	[Min(40f)]
	private float colliderDistance = 105f;

	// Token: 0x040003DF RID: 991
	[SerializeField]
	[Min(0.05f)]
	private float colliderUpdateInterval = 0.2f;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	[Min(0f)]
	private float colliderMoveThreshold = 8f;

	// Token: 0x040003E1 RID: 993
	private static readonly Dictionary<string, HeavyZoneStreamingProxy.SharedPayloadLoad> SharedPayloadLoads = new Dictionary<string, HeavyZoneStreamingProxy.SharedPayloadLoad>(StringComparer.Ordinal);

	// Token: 0x040003E2 RID: 994
	private static int _activeSharedPayloadLoads;

	// Token: 0x040003E3 RID: 995
	private static ThreadPriority _backgroundPriorityBeforeSharedLoads;

	// Token: 0x040003E4 RID: 996
	private static ulong _sharedPayloadAccessSequence;

	// Token: 0x040003E5 RID: 997
	private const int MaxCompletedSharedPayloadCacheEntries = 2;

	// Token: 0x040003E6 RID: 998
	private readonly List<HeavyZoneStreamingProxy.ColliderState> _colliders = new List<HeavyZoneStreamingProxy.ColliderState>(1024);

	// Token: 0x040003E7 RID: 999
	private readonly List<Collider> _colliderComponentScratch = new List<Collider>(4);

	// Token: 0x040003E8 RID: 1000
	private readonly List<HeavyZoneStreamingProxy.RendererState> _renderers = new List<HeavyZoneStreamingProxy.RendererState>(512);

	// Token: 0x040003E9 RID: 1001
	private readonly List<Renderer> _rendererComponentScratch = new List<Renderer>(4);

	// Token: 0x040003EA RID: 1002
	private GameObject _payloadInstance;

	// Token: 0x040003EB RID: 1003
	private GameObject _payloadLoadingGate;

	// Token: 0x040003EC RID: 1004
	private GameObject _loadedPrefab;

	// Token: 0x040003ED RID: 1005
	private HeavyZoneAddressableResidency.Lease _addressableLease;

	// Token: 0x040003EE RID: 1006
	private UniversityGameplayManifest _gameplayManifest;

	// Token: 0x040003EF RID: 1007
	private bool _canonicalGameplayBound;

	// Token: 0x040003F0 RID: 1008
	private Coroutine _loadRoutine;

	// Token: 0x040003F1 RID: 1009
	private float _nextColliderUpdate;

	// Token: 0x040003F2 RID: 1010
	private Vector3 _lastCullPosition;

	// Token: 0x040003F3 RID: 1011
	private bool _hasCullPosition;

	// Token: 0x040003F4 RID: 1012
	private readonly List<GameObject> _stagedObjects = new List<GameObject>(8192);

	// Token: 0x040003F5 RID: 1013
	private readonly List<bool> _stagedActiveDefaults = new List<bool>(8192);

	// Token: 0x040003F6 RID: 1014
	private Coroutine _stagedWakeRoutine;

	// Token: 0x040003F7 RID: 1015
	private int _stagedWakeIndex;

	// Token: 0x040003F8 RID: 1016
	private int _stagedSleepIndex = -1;

	// Token: 0x040003F9 RID: 1017
	private int _stagedColliderIndex;

	// Token: 0x040003FA RID: 1018
	private bool _stagedDormant;

	// Token: 0x040003FB RID: 1019
	private bool _stagedCollidersSuppressed;

	// Token: 0x040003FC RID: 1020
	private bool _stagedWakeArmed;

	// Token: 0x040003FD RID: 1021
	private bool _stagedSnapshotReady;

	// Token: 0x040003FE RID: 1022
	private bool _urgentStreamingWork;

	// Token: 0x040003FF RID: 1023
	private int _requestedLoadPriority = -1;

	// Token: 0x04000400 RID: 1024
	private bool _chunkCollidersEnabled = true;

	// Token: 0x04000401 RID: 1025
	private int _runtimeColliderUpdateIndex = -1;

	// Token: 0x04000402 RID: 1026
	private Vector3 _runtimeColliderUpdatePosition;

	// Token: 0x04000403 RID: 1027
	private bool _runtimeColliderUpdateHasCamera;

	// Token: 0x04000404 RID: 1028
	private Bounds? _navigationColliderBounds;

	// Token: 0x04000405 RID: 1029
	private Bounds? _restoredNavigationColliderBounds;

	// Token: 0x04000406 RID: 1030
	private static float _nextCameraMaskUpdate;

	// Token: 0x04000407 RID: 1031
	private const int StreamedVisualLayer = 3;

	// Token: 0x04000408 RID: 1032
	private static readonly ProfilerMarker StagedSleepMarker = new ProfilerMarker("HeavyZoneStreamingProxy.StagedSleep");

	// Token: 0x04000409 RID: 1033
	private static readonly ProfilerMarker StagedWakeMarker = new ProfilerMarker("HeavyZoneStreamingProxy.StagedWake");

	// Token: 0x0400040A RID: 1034
	private static readonly ProfilerMarker StagedColliderMarker = new ProfilerMarker("HeavyZoneStreamingProxy.StagedColliderFinalize");

	// Token: 0x0400040B RID: 1035
	private static readonly ProfilerMarker AtomicInstantiateMarker = new ProfilerMarker("HeavyZoneStreamingProxy.AtomicInstantiate");

	// Token: 0x0400040C RID: 1036
	private static readonly ProfilerMarker TerrainAlignMarker = new ProfilerMarker("HeavyZoneStreamingProxy.TerrainAlign");

	// Token: 0x0400040D RID: 1037
	private static readonly ProfilerMarker RendererLayersMarker = new ProfilerMarker("HeavyZoneStreamingProxy.RendererLayers");

	// Token: 0x0400040E RID: 1038
	private static readonly ProfilerMarker LodTuningMarker = new ProfilerMarker("HeavyZoneStreamingProxy.LODTuning");

	// Token: 0x0400040F RID: 1039
	private static readonly ProfilerMarker TerrainClampMarker = new ProfilerMarker("HeavyZoneStreamingProxy.TerrainClamp");

	// Token: 0x0200006B RID: 107
	private struct ColliderState
	{
		// Token: 0x04000418 RID: 1048
		public Collider component;

		// Token: 0x04000419 RID: 1049
		public bool authoredEnabled;

		// Token: 0x0400041A RID: 1050
		public bool isTrigger;

		// Token: 0x0400041B RID: 1051
		public Bounds worldBounds;

		// Token: 0x0400041C RID: 1052
		public bool hasProxyLocalBounds;

		// Token: 0x0400041D RID: 1053
		public Vector3 proxyLocalBoundsCenter;

		// Token: 0x0400041E RID: 1054
		public Vector3 proxyLocalBoundsAxisX;

		// Token: 0x0400041F RID: 1055
		public Vector3 proxyLocalBoundsAxisY;

		// Token: 0x04000420 RID: 1056
		public Vector3 proxyLocalBoundsAxisZ;
	}

	// Token: 0x0200006C RID: 108
	private struct RendererState
	{
		// Token: 0x04000421 RID: 1057
		public Renderer component;

		// Token: 0x04000422 RID: 1058
		public bool authoredForceRenderingOff;
	}

	// Token: 0x0200006D RID: 109
	private sealed class SharedPayloadLoad
	{
		// Token: 0x04000423 RID: 1059
		public string resourcePath;

		// Token: 0x04000424 RID: 1060
		public ResourceRequest request;

		// Token: 0x04000425 RID: 1061
		public GameObject asset;

		// Token: 0x04000426 RID: 1062
		public bool completed;

		// Token: 0x04000427 RID: 1063
		public bool releaseWhenCompleted;

		// Token: 0x04000428 RID: 1064
		public ulong lastAccessSequence;

		// Token: 0x04000429 RID: 1065
		public readonly HashSet<HeavyZoneStreamingProxy> consumers = new HashSet<HeavyZoneStreamingProxy>();
	}

	// Token: 0x0200006E RID: 110
	public readonly struct ColdPayloadSetupSample
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x000293EB File Offset: 0x000275EB
		public ColdPayloadSetupSample(long sequence, int frame, int proxyInstanceId, double startedRealtime, double cloneMs, double setupMs, string resourcePath)
		{
			this.sequence = sequence;
			this.frame = frame;
			this.proxyInstanceId = proxyInstanceId;
			this.startedRealtime = startedRealtime;
			this.cloneMs = cloneMs;
			this.setupMs = setupMs;
			this.resourcePath = resourcePath;
		}

		// Token: 0x0400042A RID: 1066
		public readonly long sequence;

		// Token: 0x0400042B RID: 1067
		public readonly int frame;

		// Token: 0x0400042C RID: 1068
		public readonly int proxyInstanceId;

		// Token: 0x0400042D RID: 1069
		public readonly double startedRealtime;

		// Token: 0x0400042E RID: 1070
		public readonly double cloneMs;

		// Token: 0x0400042F RID: 1071
		public readonly double setupMs;

		// Token: 0x04000430 RID: 1072
		public readonly string resourcePath;
	}
}
