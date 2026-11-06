using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Features.Detection;
using Features.ExposeLingerie;
using Features.Flash;
using Features.Gym;
using Features.HideFlash;
using Features.InsertableToys.Vegetables;
using Features.InteractiveStand;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MallHack;
using Features.MasturbationSystem;
using Features.MatchRecap;
using Features.NPC;
using Features.OxGram;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Police;
using Features.Rank;
using Features.RapidActions;
using Features.RenderingPrecision;
using Features.Score;
using Features.Slip;
using Features.SpicyHatch;
using Features.Stadium;
using Features.TintBodyPaint;
using Features.TowelSystem;
using Features.TrashcanLoot;
using Features.WardrobeCombos;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.NPCs;
using ProjectExhibitionist.Subway;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace Features.Missions
{
	// Token: 0x02000FA6 RID: 4006
	[DefaultExecutionOrder(-8500)]
	public sealed class MissionRuntimeHost : MonoBehaviour
	{
		// Token: 0x0600789A RID: 30874 RVA: 0x003135CE File Offset: 0x003117CE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			if (MissionRuntimeHost._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Mission Runtime");
			Object.DontDestroyOnLoad(gameObject);
			MissionRuntimeHost._instance = gameObject.AddComponent<MissionRuntimeHost>();
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x003135F8 File Offset: 0x003117F8
		private void Awake()
		{
			if (MissionRuntimeHost._instance != null && MissionRuntimeHost._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			MissionRuntimeHost._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			MissionService.EnsureGlobalMissions();
			MissionGameplaySignals.ItemCollected += this.HandleItemCollected;
			MissionGameplaySignals.TrashcanLooted += this.HandleTrashcanLooted;
			MissionGameplaySignals.WearableActionCompleted += this.HandleWearableAction;
			MissionGameplaySignals.ClothingBroken += this.HandleClothingBroken;
			MissionGameplaySignals.PlayerSeen += this.HandlePlayerSeen;
			MissionGameplaySignals.FirstFlight += this.HandleFirstFlight;
			MissionGameplaySignals.NpcPhotosCommitted += this.HandleNpcPhotosCommitted;
			SmartphoneFlash.OnPhotoCaptured += this.HandleNpcPhotoTaken;
			SmartphoneFlash.OnVideoStarted += this.HandleNpcVideoStarted;
			RefereeChaser.ChaseEscaped += this.HandleRefereeEscaped;
			this.SubscribePlaceObjectives();
			WorldOrigin.Shifted += this.HandleWorldOriginShifted;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			SceneManager.sceneUnloaded += this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x00313735 File Offset: 0x00311935
		private void Start()
		{
			this.RequestLandmarkRefresh();
			base.StartCoroutine(this.InstallCurrentSceneNextFrame());
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x0031374C File Offset: 0x0031194C
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			MissionGameplaySignals.ItemCollected -= this.HandleItemCollected;
			MissionGameplaySignals.TrashcanLooted -= this.HandleTrashcanLooted;
			MissionGameplaySignals.WearableActionCompleted -= this.HandleWearableAction;
			MissionGameplaySignals.ClothingBroken -= this.HandleClothingBroken;
			MissionGameplaySignals.PlayerSeen -= this.HandlePlayerSeen;
			MissionGameplaySignals.FirstFlight -= this.HandleFirstFlight;
			MissionGameplaySignals.NpcPhotosCommitted -= this.HandleNpcPhotosCommitted;
			SmartphoneFlash.OnPhotoCaptured -= this.HandleNpcPhotoTaken;
			SmartphoneFlash.OnVideoStarted -= this.HandleNpcVideoStarted;
			RefereeChaser.ChaseEscaped -= this.HandleRefereeEscaped;
			this.UnsubscribePlaceObjectives();
			WorldOrigin.Shifted -= this.HandleWorldOriginShifted;
			this.UnbindStats();
			this.UnbindPoi();
			this.UnbindEmotes();
			this.UnbindEncounterDirector();
			this.UnbindOrgasmMeter();
			this.UnbindWearController();
			this.UnbindCoverController();
			this.UnbindExposureSources();
			this.BindChunks(null);
			if (MissionRuntimeHost._instance == this)
			{
				MissionRuntimeHost._instance = null;
			}
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x00313898 File Offset: 0x00311A98
		private void Update()
		{
			if (Time.unscaledTime >= this._nextBindingRefresh)
			{
				this._nextBindingRefresh = Time.unscaledTime + 0.5f;
				this.BindPoiIfNeeded();
				this.BindEmotesIfNeeded();
				this.BindEncounterDirectorIfNeeded();
				this.BindOrgasmMeterIfNeeded();
				this.BindWearControllerIfNeeded();
				this.BindCoverControllerIfNeeded();
				this.BindExposureSourcesIfNeeded();
				this.BindStatsIfNeeded();
				this.BindChunks(ChunkManager.Instance);
				if (this._stats != null && this._stats.IsRunTrackingActive && !MissionService.IsRunActive)
				{
					this.BeginRunForCurrentContext();
				}
			}
			if (Time.unscaledTime >= this._nextCatalogRefresh)
			{
				this._nextCatalogRefresh = Time.unscaledTime + 1f;
				this.RefreshWearableCatalog();
				this.RefreshEmoteCatalog();
				this.RefreshTrafficLandmarks();
				this.PublishLandmarkOptions();
				this.PublishRankCapabilities();
				this.PublishPlaceCapabilities();
				MissionService.SetWallCoverAvailable(this._coverController != null && DynamicWallCoverScanner.Enabled);
				MissionService.RefreshAvailableMissions();
			}
			this.TrackMovingEmote();
			if (Time.time >= this._nextEmoteSample)
			{
				this._nextEmoteSample = Time.time + 0.25f;
				this.SampleEmoteProgress();
			}
			this.SampleRankObjectives();
			this.FlushPhotoCredits();
			this.UpdateSecretObjectives();
			if (!MissionService.IsRunActive || this._stats == null || Time.unscaledTime < this._nextPoll)
			{
				return;
			}
			this._nextPoll = Time.unscaledTime + 1f;
			this.FlushVehicleDistance();
			this.FlushCoverTime(true);
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x00313A08 File Offset: 0x00311C08
		private void BindChunks(ChunkManager chunks)
		{
			if (this._chunks == chunks)
			{
				return;
			}
			if (this._chunks != null)
			{
				this._chunks.OnChunkZonesReady -= this.HandleChunkChanged;
				this._chunks.OnChunkActivated -= this.HandleChunkChanged;
				this._chunks.OnChunkDeactivated -= this.HandleChunkChanged;
				this._chunks.OnChunkRemoved -= this.HandleChunkChanged;
			}
			this._chunks = chunks;
			if (this._chunks == null)
			{
				return;
			}
			this._chunks.OnChunkZonesReady += this.HandleChunkChanged;
			this._chunks.OnChunkActivated += this.HandleChunkChanged;
			this._chunks.OnChunkDeactivated += this.HandleChunkChanged;
			this._chunks.OnChunkRemoved += this.HandleChunkChanged;
			this.RequestLandmarkRefresh();
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x00313B06 File Offset: 0x00311D06
		private void HandleChunkChanged(ChunkInstance chunk)
		{
			this.RequestLandmarkRefresh();
		}

		// Token: 0x060078A1 RID: 30881 RVA: 0x00313B10 File Offset: 0x00311D10
		private void FlushVehicleDistance()
		{
			if (!MissionService.IsRunActive || this._stats == null)
			{
				return;
			}
			float num = Mathf.Max(0f, this._stats.VehicleTravelMeters);
			float num2 = num - this._lastVehicleDistance;
			this._lastVehicleDistance = num;
			if (num2 > 0f)
			{
				MissionService.Report(MissionObjectiveKind.VehicleDistance, num2, null, null, null);
			}
		}

		// Token: 0x060078A2 RID: 30882 RVA: 0x00313B74 File Offset: 0x00311D74
		private void TrackMovingEmote()
		{
			if (!MissionService.IsRunActive || this._activeEmote == null || this._activeEmote.MovementMode != EmoteMovementMode.Moving || this._emoteController == null || Time.timeScale <= 0f)
			{
				return;
			}
			Vector3 position = this._emoteController.transform.position;
			float num = Vector3.Distance(position, this._lastEmotePosition);
			this._lastEmotePosition = position;
			if (num > 0.001f && num < 5f)
			{
				this._movingEmoteDistance += num;
			}
		}

		// Token: 0x060078A3 RID: 30883 RVA: 0x00313C02 File Offset: 0x00311E02
		private void HandleWorldOriginShifted(Vector3 delta)
		{
			this._lastEmotePosition += delta;
		}

		// Token: 0x060078A4 RID: 30884 RVA: 0x00313C18 File Offset: 0x00311E18
		private void BindEmotesIfNeeded()
		{
			if (this._emoteController != null && this._emoteController.gameObject.scene.IsValid())
			{
				return;
			}
			this.UnbindEmotes();
			this._emoteController = MissionRuntimeHost.FindPlayerComponent<GameplayEmoteController>();
			if (this._emoteController == null)
			{
				return;
			}
			this._emoteController.OnEmoteStarted += this.HandleEmoteStarted;
			this._emoteController.OnEmoteEnded += this.HandleEmoteEnded;
			this.RefreshEmoteCatalog();
		}

		// Token: 0x060078A5 RID: 30885 RVA: 0x00313CA4 File Offset: 0x00311EA4
		private void UnbindEmotes()
		{
			if (this._emoteController != null)
			{
				this._emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this._emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._emoteController = null;
			this._emoteStarts.Clear();
			this._activeEmote = null;
		}

		// Token: 0x060078A6 RID: 30886 RVA: 0x00313D08 File Offset: 0x00311F08
		private void BindEncounterDirectorIfNeeded()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance == this._encounterDirector)
			{
				return;
			}
			this.UnbindEncounterDirector();
			this._encounterDirector = instance;
			if (this._encounterDirector != null)
			{
				this._encounterDirector.OnEncounterEmoteStarted += this.HandleEncounterEmoteStarted;
			}
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x00313D5C File Offset: 0x00311F5C
		private void UnbindEncounterDirector()
		{
			if (this._encounterDirector != null)
			{
				this._encounterDirector.OnEncounterEmoteStarted -= this.HandleEncounterEmoteStarted;
			}
			this._encounterDirector = null;
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x00313D8C File Offset: 0x00311F8C
		private void HandleEncounterEmoteStarted(EmoteDefinitionSO emote)
		{
			if (!MissionService.IsRunActive || emote == null || this.IsStanceEmote(emote))
			{
				return;
			}
			string text = MissionRuntimeHost.EmoteKey(emote);
			MissionService.Report(MissionObjectiveKind.EmotesPerformed, 1f, text, null, null);
			MissionService.Report(MissionObjectiveKind.SpecificEmote, 1f, text, null, null);
			MissionService.Report(MissionObjectiveKind.SuccessfulExhibitions, 1f, null, null, null);
			MissionService.Report(MissionObjectiveKind.SpecificEmoteExhibition, 1f, text, null, null);
			foreach (KeyValuePair<string, string> keyValuePair in this._currentlyWorn)
			{
				MissionService.Report(MissionObjectiveKind.SpecificClothingEmote, 1f, keyValuePair.Key, null, null);
				MissionService.Report(MissionObjectiveKind.SpecificClothingExhibition, 1f, keyValuePair.Key, null, null);
			}
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x00313E8C File Offset: 0x0031208C
		private bool IsStanceEmote(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return false;
			}
			if (this._stanceSource == null)
			{
				this._stanceEmotes.Clear();
				this._stanceSource = Object.FindObjectOfType<RapidActionsHUDController>();
				if (this._stanceSource != null)
				{
					FieldInfo field = typeof(RapidActionsHUDController).GetField("crouchSlot", BindingFlags.Instance | BindingFlags.NonPublic);
					object obj = ((field != null) ? field.GetValue(this._stanceSource) : null);
					object obj2;
					if (obj == null)
					{
						obj2 = null;
					}
					else
					{
						FieldInfo field2 = obj.GetType().GetField("crouchEmotes");
						obj2 = ((field2 != null) ? field2.GetValue(obj) : null);
					}
					List<EmoteDefinitionSO> list = obj2 as List<EmoteDefinitionSO>;
					if (list != null)
					{
						foreach (EmoteDefinitionSO emoteDefinitionSO in list)
						{
							if (emoteDefinitionSO != null)
							{
								this._stanceEmotes.Add(emoteDefinitionSO);
							}
						}
					}
				}
			}
			return this._stanceEmotes.Contains(emote);
		}

		// Token: 0x060078AA RID: 30890 RVA: 0x00313F90 File Offset: 0x00312190
		private void BindOrgasmMeterIfNeeded()
		{
			OrgasmMeterController instance = OrgasmMeterController.Instance;
			if (instance == this._orgasmMeter)
			{
				return;
			}
			this.UnbindOrgasmMeter();
			this._orgasmMeter = instance;
			if (this._orgasmMeter != null)
			{
				this._orgasmMeter.OnMaxReached += this.HandleOrgasmReached;
			}
		}

		// Token: 0x060078AB RID: 30891 RVA: 0x00313FE4 File Offset: 0x003121E4
		private void UnbindOrgasmMeter()
		{
			if (this._orgasmMeter != null)
			{
				this._orgasmMeter.OnMaxReached -= this.HandleOrgasmReached;
			}
			this._orgasmMeter = null;
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x00314014 File Offset: 0x00312214
		private void BindWearControllerIfNeeded()
		{
			if (this._wearController != null && this._wearController.gameObject.scene.IsValid())
			{
				return;
			}
			this.UnbindWearController();
			this._wearController = MissionRuntimeHost.FindPlayerComponent<WearableUseController>();
			if (this._wearController == null)
			{
				return;
			}
			this._wearController.OnItemWorn += this.HandleItemWorn;
			this._wearController.OnItemUnworn += this.HandleItemUnworn;
			this.RefreshWearableCatalog();
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x003140A0 File Offset: 0x003122A0
		private void UnbindWearController()
		{
			if (this._wearController != null)
			{
				this._wearController.OnItemWorn -= this.HandleItemWorn;
				this._wearController.OnItemUnworn -= this.HandleItemUnworn;
			}
			this._wearController = null;
			this._currentlyWorn.Clear();
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x003140FC File Offset: 0x003122FC
		private void BindCoverControllerIfNeeded()
		{
			if (this._coverController != null && this._coverController.gameObject.scene.IsValid())
			{
				return;
			}
			this.UnbindCoverController();
			this._coverController = MissionRuntimeHost.FindPlayerComponent<vBasicCoverController>();
			if (this._coverController == null)
			{
				return;
			}
			UnityEvent onEnterCover = this._coverController.onEnterCover;
			if (onEnterCover != null)
			{
				onEnterCover.AddListener(new UnityAction(this.HandleCoverEntered));
			}
			UnityEvent onExitCover = this._coverController.onExitCover;
			if (onExitCover != null)
			{
				onExitCover.AddListener(new UnityAction(this.HandleCoverExited));
			}
			if (this._coverController.inCover)
			{
				this.HandleCoverEntered();
			}
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x003141AC File Offset: 0x003123AC
		private void UnbindCoverController()
		{
			if (this._coverController != null)
			{
				UnityEvent onEnterCover = this._coverController.onEnterCover;
				if (onEnterCover != null)
				{
					onEnterCover.RemoveListener(new UnityAction(this.HandleCoverEntered));
				}
				UnityEvent onExitCover = this._coverController.onExitCover;
				if (onExitCover != null)
				{
					onExitCover.RemoveListener(new UnityAction(this.HandleCoverExited));
				}
			}
			this.FlushCoverTime(false);
			this._coverController = null;
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x0031421C File Offset: 0x0031241C
		private void HandleCoverEntered()
		{
			if (this._coverEnteredAt >= 0f)
			{
				return;
			}
			this._coverEnteredAt = Time.time;
			if (MissionService.IsRunActive)
			{
				MissionService.Report(MissionObjectiveKind.WallCoverEntries, 1f, null, null, null);
			}
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x00314260 File Offset: 0x00312460
		private void HandleCoverExited()
		{
			this.FlushCoverTime(false);
		}

		// Token: 0x060078B2 RID: 30898 RVA: 0x0031426C File Offset: 0x0031246C
		private void FlushCoverTime(bool continueTracking = false)
		{
			if (this._coverEnteredAt < 0f)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._coverEnteredAt);
			this._coverEnteredAt = (continueTracking ? Time.time : (-1f));
			if (MissionService.IsRunActive && num > 0f)
			{
				MissionService.Report(MissionObjectiveKind.WallCoverTime, num, null, null, null);
			}
		}

		// Token: 0x060078B3 RID: 30899 RVA: 0x003142D8 File Offset: 0x003124D8
		private void RefreshWearableCatalog()
		{
			if (this._wearController == null || this._wearController.Inventory == null)
			{
				MissionService.SetAvailableWearables(null);
				MissionService.SetAvailableWearableActions(null);
				return;
			}
			PlayerInventoryManager inventory = this._wearController.Inventory;
			List<MissionWearableOption> list = new List<MissionWearableOption>();
			List<MissionWearableActionOption> list2 = new List<MissionWearableActionOption>();
			this._currentlyWorn.Clear();
			this._ownedWearables.Clear();
			for (int i = 0; i < inventory.MaxItemSlots; i++)
			{
				this.AddWearableCatalogEntry(inventory.GetItemAt(i) as WearableItemSO, list, list2);
			}
			for (int j = 0; j < inventory.MaxClothSlots; j++)
			{
				this.AddWearableCatalogEntry(inventory.GetClothAt(j) as WearableItemSO, list, list2);
			}
			MissionService.SetAvailableWearables(list);
			MissionService.SetAvailableWearableActions(list2);
		}

		// Token: 0x060078B4 RID: 30900 RVA: 0x0031439C File Offset: 0x0031259C
		private void AddWearableCatalogEntry(WearableItemSO item, List<MissionWearableOption> wearables, List<MissionWearableActionOption> actions)
		{
			if (item == null)
			{
				return;
			}
			string id = MissionRuntimeHost.ItemKey(item);
			string text = (string.IsNullOrWhiteSpace(item.itemName) ? item.name : item.itemName);
			bool flag = this._wearController.IsWorn(item);
			if (flag)
			{
				this._currentlyWorn[id] = text;
			}
			if (!this._ownedWearables.Contains(item))
			{
				this._ownedWearables.Add(item);
			}
			bool flag2 = (item.blockedActivitiesWhileWorn & PlayerActivity.Emote) > PlayerActivity.None;
			if (!wearables.Exists((MissionWearableOption e) => string.Equals(e.Id, id, StringComparison.OrdinalIgnoreCase)))
			{
				wearables.Add(new MissionWearableOption(id, text, item.hasWearDuration, flag, flag2));
			}
			if (item.wearActions != null)
			{
				for (int i = 0; i < item.wearActions.Count; i++)
				{
					WearableActionDefinition wearableActionDefinition = item.wearActions[i];
					if (!(wearableActionDefinition == null) && (wearableActionDefinition.ActionType == WearableActionType.SecondaryEffectToggle || wearableActionDefinition.ActionType == WearableActionType.TimeFreeze))
					{
						actions.Add(new MissionWearableActionOption(id, text, wearableActionDefinition.ActionId, wearableActionDefinition.DisplayName, wearableActionDefinition.ActionType == WearableActionType.TimeFreeze));
					}
				}
			}
			if (string.Equals(id, "cloth_cheerleader_suite", StringComparison.OrdinalIgnoreCase))
			{
				actions.Add(new MissionWearableActionOption(id, text, "cheerleader_skirt_breeze", "Skirt Breeze (hold)", false));
				actions.Add(new MissionWearableActionOption(id, text, "cheerleader_dance", "Dance", false));
				return;
			}
			if (string.Equals(id, "cloth_swimsuit", StringComparison.OrdinalIgnoreCase))
			{
				actions.Add(new MissionWearableActionOption(id, text, "swimsuit_slip", "Drop Swimsuit", true));
				return;
			}
			if (string.Equals(id, "cloth_towel", StringComparison.OrdinalIgnoreCase))
			{
				actions.Add(new MissionWearableActionOption(id, text, "towel_slip", "Drop Towel", true));
				return;
			}
			if (string.Equals(id, "cloth_bikini_bra", StringComparison.OrdinalIgnoreCase))
			{
				actions.Add(new MissionWearableActionOption(id, text, "bikini_slip", "Slip Top", true));
				return;
			}
			if (string.Equals(id, "cloth_bikini_panties", StringComparison.OrdinalIgnoreCase))
			{
				actions.Add(new MissionWearableActionOption(id, text, "bikini_slip", "Slip Bottom", true));
			}
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x003145E1 File Offset: 0x003127E1
		private IEnumerator ScanLandmarksBudgeted(int generation, Dictionary<string, MissionRuntimeHost.LandmarkRuntime> results)
		{
			Stack<Transform> pending = new Stack<Transform>(4096);
			List<GameObject> list = new List<GameObject>(32);
			this._scanContextualEmotes.Clear();
			this.ClearPlaceScan();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.IsValid() && sceneAt.isLoaded)
				{
					list.Clear();
					sceneAt.GetRootGameObjects(list);
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (list[j] != null)
						{
							pending.Push(list[j].transform);
						}
					}
				}
			}
			while (pending.Count > 0)
			{
				if (generation != this._landmarkRefreshGeneration)
				{
					yield break;
				}
				int num = 0;
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				using (MissionRuntimeHost.RefreshLandmarksMarker.Auto())
				{
					while (pending.Count > 0 && num < 2048)
					{
						Transform transform = pending.Pop();
						num++;
						if (transform != null && transform.gameObject.activeInHierarchy)
						{
							MissionRuntimeHost.InspectLandmarkCandidate(transform, results);
							this.InspectContextualEmoteProvider(transform);
							this.InspectPlaceProvider(transform);
							for (int k = transform.childCount - 1; k >= 0; k--)
							{
								pending.Push(transform.GetChild(k));
							}
						}
						if ((num & 63) == 0 && Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble >= 0.002)
						{
							break;
						}
					}
				}
				if (pending.Count > 0)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060078B6 RID: 30902 RVA: 0x00314600 File Offset: 0x00312800
		private static void InspectLandmarkCandidate(Transform candidate, Dictionary<string, MissionRuntimeHost.LandmarkRuntime> results)
		{
			MissionLandmark missionLandmark;
			if (candidate.TryGetComponent<MissionLandmark>(out missionLandmark))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, missionLandmark.LandmarkId, missionLandmark.DisplayName, missionLandmark.ActivationRange, candidate);
			}
			NPCSenses npcsenses;
			if (candidate.TryGetComponent<NPCSenses>(out npcsenses))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, "NPC", "an NPC", 6f, npcsenses.transform);
			}
			TrashcanLootInteractable trashcanLootInteractable;
			if (candidate.TryGetComponent<TrashcanLootInteractable>(out trashcanLootInteractable))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, "TRASHCAN", "a trashcan", 3.5f, trashcanLootInteractable.transform);
			}
			CoverWallSetup coverWallSetup;
			if (candidate.TryGetComponent<CoverWallSetup>(out coverWallSetup))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, "COVER_WALL", "a cover wall", 4f, coverWallSetup.transform);
			}
			vVehicleController vVehicleController;
			VehicleAI vehicleAI;
			SkateboardInteractable skateboardInteractable;
			SimpleRollerSkatesInteractable simpleRollerSkatesInteractable;
			if (candidate.TryGetComponent<vVehicleController>(out vVehicleController) || candidate.TryGetComponent<VehicleAI>(out vehicleAI) || candidate.TryGetComponent<SkateboardInteractable>(out skateboardInteractable) || candidate.TryGetComponent<SimpleRollerSkatesInteractable>(out simpleRollerSkatesInteractable))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, "VEHICLE", "a vehicle", 5f, candidate);
			}
			string text2;
			float num;
			bool flag;
			string text = MissionRuntimeHost.NamedLandmarkFamily(candidate.name, out text2, out num, out flag);
			if (text == null)
			{
				return;
			}
			Transform parent = candidate.parent;
			string text3;
			float num2;
			bool flag2;
			if (parent != null && !flag && MissionRuntimeHost.NamedLandmarkFamily(parent.name, out text3, out num2, out flag2) == text && !flag2)
			{
				return;
			}
			if (flag)
			{
				for (int i = 0; i < candidate.childCount; i++)
				{
					Transform child = candidate.GetChild(i);
					if (child.gameObject.activeInHierarchy && MissionRuntimeHost.HasPhysicalPresence(child))
					{
						MissionRuntimeHost.AddLandmarkTransform(results, text, text2, num, child);
					}
				}
				return;
			}
			if (MissionRuntimeHost.HasPhysicalPresence(candidate))
			{
				MissionRuntimeHost.AddLandmarkTransform(results, text, text2, num, candidate);
			}
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x0031478C File Offset: 0x0031298C
		private static string NamedLandmarkFamily(string objectName, out string displayName, out float range, out bool container)
		{
			displayName = null;
			range = 0f;
			container = false;
			if (string.IsNullOrEmpty(objectName))
			{
				return null;
			}
			if (objectName.IndexOf("statue", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				displayName = "a statue";
				range = 6f;
				container = string.Equals(objectName, "Statues", StringComparison.OrdinalIgnoreCase);
				return "STATUE";
			}
			if (objectName.IndexOf("bench", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				displayName = "a bench";
				range = 4f;
				container = string.Equals(objectName, "Benches", StringComparison.OrdinalIgnoreCase);
				return "BENCH";
			}
			if (objectName.IndexOf("fountain", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				displayName = "a fountain";
				range = 6f;
				container = string.Equals(objectName, "Fountains", StringComparison.OrdinalIgnoreCase);
				return "FOUNTAIN";
			}
			if (objectName.IndexOf("vending", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				displayName = "a vending machine";
				range = 4f;
				container = string.Equals(objectName, "Vending Machines", StringComparison.OrdinalIgnoreCase);
				return "VENDING";
			}
			return null;
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x00314875 File Offset: 0x00312A75
		private static bool HasPhysicalPresence(Transform prop)
		{
			return prop.GetComponentInChildren<Renderer>() != null || prop.GetComponentInChildren<Collider>() != null;
		}

		// Token: 0x060078B9 RID: 30905 RVA: 0x00314894 File Offset: 0x00312A94
		private void InspectContextualEmoteProvider(Transform candidate)
		{
			GymExerciseStation gymExerciseStation;
			if (candidate.TryGetComponent<GymExerciseStation>(out gymExerciseStation))
			{
				FieldInfo field = typeof(GymExerciseStation).GetField("exercises", BindingFlags.Instance | BindingFlags.NonPublic);
				List<EmoteDefinitionSO> list = ((field != null) ? field.GetValue(gymExerciseStation) : null) as List<EmoteDefinitionSO>;
				if (list != null)
				{
					foreach (EmoteDefinitionSO emoteDefinitionSO in list)
					{
						if (emoteDefinitionSO != null)
						{
							this._scanContextualEmotes.Add(emoteDefinitionSO);
						}
					}
				}
			}
			StandInteractable standInteractable;
			if (candidate.TryGetComponent<StandInteractable>(out standInteractable))
			{
				FieldInfo field2 = typeof(StandInteractable).GetField("profile", BindingFlags.Instance | BindingFlags.NonPublic);
				StandActionProfileSO standActionProfileSO = ((field2 != null) ? field2.GetValue(standInteractable) : null) as StandActionProfileSO;
				if (standActionProfileSO != null && standActionProfileSO.Actions != null)
				{
					foreach (StandActionProfileSO.StandAction standAction in standActionProfileSO.Actions)
					{
						if (standAction != null && standAction.Emote != null)
						{
							this._scanContextualEmotes.Add(standAction.Emote);
						}
					}
				}
			}
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x003149D8 File Offset: 0x00312BD8
		private static void AddLandmarkTransform(Dictionary<string, MissionRuntimeHost.LandmarkRuntime> results, string id, string displayName, float range, Transform target)
		{
			if (target == null || string.IsNullOrEmpty(id))
			{
				return;
			}
			MissionRuntimeHost.LandmarkRuntime landmarkRuntime;
			if (!results.TryGetValue(id, out landmarkRuntime))
			{
				landmarkRuntime = new MissionRuntimeHost.LandmarkRuntime
				{
					Id = id,
					Name = displayName,
					Range = range
				};
				results[id] = landmarkRuntime;
			}
			if (!landmarkRuntime.Transforms.Contains(target))
			{
				landmarkRuntime.Transforms.Add(target);
				if (string.Equals(id, "VEHICLE", StringComparison.OrdinalIgnoreCase))
				{
					landmarkRuntime.VehicleColliders[target] = target.GetComponentsInChildren<Collider>(true);
					return;
				}
				if (MissionRuntimeHost.IsNamedPropFamily(id))
				{
					landmarkRuntime.VehicleColliders[target] = target.GetComponentsInChildren<Collider>(true);
					landmarkRuntime.PropRenderers[target] = target.GetComponentsInChildren<Renderer>(true);
				}
			}
		}

		// Token: 0x060078BB RID: 30907 RVA: 0x00314A98 File Offset: 0x00312C98
		private static bool IsNamedPropFamily(string id)
		{
			return id == "BENCH" || id == "STATUE" || id == "FOUNTAIN" || id == "VENDING";
		}

		// Token: 0x060078BC RID: 30908 RVA: 0x00314AD0 File Offset: 0x00312CD0
		private void PublishLandmarks(Dictionary<string, MissionRuntimeHost.LandmarkRuntime> results)
		{
			this._landmarks.Clear();
			foreach (KeyValuePair<string, MissionRuntimeHost.LandmarkRuntime> keyValuePair in results)
			{
				this._landmarks[keyValuePair.Key] = keyValuePair.Value;
			}
			this._contextualEmotes.Clear();
			this._contextualEmotes.UnionWith(this._scanContextualEmotes);
			this.PublishPlaceScan();
			this.PublishRankCapabilities();
			this.PublishPlaceCapabilities();
			this.RefreshTrafficLandmarks();
			this.PublishLandmarkOptions();
			this.RefreshEmoteCatalog();
		}

		// Token: 0x060078BD RID: 30909 RVA: 0x00314B7C File Offset: 0x00312D7C
		private void RefreshTrafficLandmarks()
		{
			VehicleAI.CollectActiveVehicles(this._trafficLandmarks);
			foreach (VehicleAI vehicleAI in this._trafficLandmarks)
			{
				MissionRuntimeHost.AddLandmarkTransform(this._landmarks, "VEHICLE", "a vehicle", 5f, vehicleAI.transform);
			}
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x00314BF4 File Offset: 0x00312DF4
		private void PublishLandmarkOptions()
		{
			List<MissionOption> list = new List<MissionOption>(this._landmarks.Count);
			foreach (MissionRuntimeHost.LandmarkRuntime landmarkRuntime in this._landmarks.Values)
			{
				if (landmarkRuntime.Transforms.Exists((Transform target) => target != null))
				{
					list.Add(new MissionOption(landmarkRuntime.Id, landmarkRuntime.Name, "", "", false, false, false, true));
				}
			}
			MissionService.SetAvailableLandmarks(list);
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x00314CB0 File Offset: 0x00312EB0
		private bool IsNearLandmark(MissionRuntimeHost.LandmarkRuntime landmark, Vector3 position)
		{
			float num = landmark.Range * landmark.Range;
			for (int i = landmark.Transforms.Count - 1; i >= 0; i--)
			{
				Transform transform = landmark.Transforms[i];
				if (transform == null)
				{
					if (transform != null)
					{
						landmark.VehicleColliders.Remove(transform);
						landmark.PropRenderers.Remove(transform);
					}
					landmark.Transforms.RemoveAt(i);
				}
				else if (transform.gameObject.activeInHierarchy)
				{
					Collider[] array;
					if (landmark.VehicleColliders.TryGetValue(transform, out array))
					{
						bool flag = false;
						foreach (Collider collider in array)
						{
							if (!(collider == null) && collider.enabled && !collider.isTrigger && collider.gameObject.activeInHierarchy && collider.transform.IsChildOf(transform))
							{
								flag = true;
								if ((MissionRuntimeHost.ClosestLandmarkPoint(collider, position) - position).sqrMagnitude <= num)
								{
									return true;
								}
							}
						}
						if (flag)
						{
							goto IL_01C6;
						}
					}
					Renderer[] array3;
					if (landmark.PropRenderers.TryGetValue(transform, out array3))
					{
						bool flag2 = false;
						foreach (Renderer renderer in array3)
						{
							if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
							{
								flag2 = true;
								if ((renderer.bounds.ClosestPoint(position) - position).sqrMagnitude <= num)
								{
									return true;
								}
							}
						}
						if (flag2)
						{
							goto IL_01C6;
						}
					}
					Vector3 vector = transform.position;
					Collider collider2;
					if (transform.TryGetComponent<Collider>(out collider2) && collider2.enabled && !collider2.isTrigger)
					{
						vector = MissionRuntimeHost.ClosestLandmarkPoint(collider2, position);
					}
					if ((vector - position).sqrMagnitude <= num)
					{
						return true;
					}
				}
				IL_01C6:;
			}
			return false;
		}

		// Token: 0x060078C0 RID: 30912 RVA: 0x00314E90 File Offset: 0x00313090
		private static Vector3 ClosestLandmarkPoint(Collider collider, Vector3 position)
		{
			bool flag;
			if (!(collider is BoxCollider) && !(collider is SphereCollider) && !(collider is CapsuleCollider))
			{
				MeshCollider meshCollider = collider as MeshCollider;
				flag = meshCollider != null && meshCollider.convex;
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return collider.bounds.ClosestPoint(position);
			}
			return collider.ClosestPoint(position);
		}

		// Token: 0x060078C1 RID: 30913 RVA: 0x00314EE4 File Offset: 0x003130E4
		private void RefreshEmoteCatalog()
		{
			if (this._emoteController == null || this._emoteController.EmoteCollection == null)
			{
				MissionService.SetAvailableEmotes(null);
				return;
			}
			List<EmoteDefinitionSO> unlockedEmotes = this._emoteController.EmoteCollection.GetUnlockedEmotes();
			List<MissionOption> list = new List<MissionOption>(unlockedEmotes.Count + 8);
			this.AddEmoteOptions(list, unlockedEmotes, false);
			if (this._contextualEmotes.Count > 0)
			{
				this.AddEmoteOptions(list, new List<EmoteDefinitionSO>(this._contextualEmotes), true);
			}
			MissionService.SetAvailableEmotes(list);
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x00314F68 File Offset: 0x00313168
		private void AddEmoteOptions(List<MissionOption> options, IList<EmoteDefinitionSO> emotes, bool contextual)
		{
			if (emotes == null)
			{
				return;
			}
			for (int i = 0; i < emotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = emotes[i];
				if (!(emoteDefinitionSO == null) && !(emoteDefinitionSO.ResolveTriggerPrefab() == null) && emoteDefinitionSO.SpecialAction == EmoteSpecialAction.None && MissionRankPolicy.IsAvailable(MissionObjectiveKind.EmotesPerformed) && (!emoteDefinitionSO.IsMasturbationEmote || MissionRankPolicy.IsAvailable(MissionObjectiveKind.Orgasms)) && (contextual || this._emoteController.IsEmoteAllowedByRank(emoteDefinitionSO, true)) && this._emoteController.IsEmoteAllowedByFutanari(emoteDefinitionSO))
				{
					string id = MissionRuntimeHost.EmoteKey(emoteDefinitionSO);
					if (!options.Exists((MissionOption option) => string.Equals(option.Id, id, StringComparison.OrdinalIgnoreCase)))
					{
						string text = (string.IsNullOrWhiteSpace(emoteDefinitionSO.DisplayName) ? emoteDefinitionSO.name : emoteDefinitionSO.DisplayName);
						string text2 = (contextual ? string.Empty : (emoteDefinitionSO.RequiredPoiId ?? string.Empty));
						string text3;
						bool flag = !emoteDefinitionSO.IsBlockedByClothing && !GameplayEmoteController.IsMasturbationEmoteBlocked(emoteDefinitionSO, out text3);
						options.Add(new MissionOption(id, text, text2, MissionRuntimeHost.ResolvePoiDisplayName(text2), emoteDefinitionSO.MovementMode == EmoteMovementMode.Moving, emoteDefinitionSO.IsMasturbationEmote, contextual, flag));
					}
				}
			}
		}

		// Token: 0x060078C3 RID: 30915 RVA: 0x003150A8 File Offset: 0x003132A8
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (!MissionService.IsRunActive || emote == null || this.IsStanceEmote(emote))
			{
				return;
			}
			this._emoteStarts[MissionRuntimeHost.EmoteKey(emote)] = Time.time;
			this._activeEmote = emote;
			this._performanceReported = false;
			this._witnessReported = false;
			this._performedNear.Clear();
			this._performedWearing.Clear();
			this._lastEmotePosition = ((this._emoteController != null) ? this._emoteController.transform.position : Vector3.zero);
			this._movingEmoteDistance = 0f;
			this._reportedMovingEmoteDistance = 0f;
			this.BeginEmoteAttempt();
			this.SampleEmoteWitness();
		}

		// Token: 0x060078C4 RID: 30916 RVA: 0x0031515D File Offset: 0x0031335D
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (emote == null || emote != this._activeEmote)
			{
				return;
			}
			this.TrackMovingEmote();
			this.SampleEmoteProgress();
			this._emoteStarts.Remove(MissionRuntimeHost.EmoteKey(emote));
			this._activeEmote = null;
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x0031519C File Offset: 0x0031339C
		private void SampleEmoteProgress()
		{
			if (!MissionService.IsRunActive || this._activeEmote == null || Time.timeScale <= 0f)
			{
				return;
			}
			string text = MissionRuntimeHost.EmoteKey(this._activeEmote);
			float num;
			if (!this._emoteStarts.TryGetValue(text, out num))
			{
				return;
			}
			this.SampleEmoteWitness();
			float num2 = this._movingEmoteDistance - this._reportedMovingEmoteDistance;
			if (num2 > 0f)
			{
				this._reportedMovingEmoteDistance = this._movingEmoteDistance;
				MissionService.Report(MissionObjectiveKind.MovingEmoteDistance, num2, text, null, null);
			}
			float num3 = Mathf.Clamp(this._activeEmote.Duration * 0.2f, 0.25f, 0.75f);
			if (Time.time - num < num3)
			{
				return;
			}
			if (!this._performanceReported)
			{
				this._performanceReported = true;
				MissionService.Report(MissionObjectiveKind.EmotesPerformed, 1f, text, null, null);
				MissionService.Report(MissionObjectiveKind.SpecificEmote, 1f, text, null, null);
				this.NoteEmotePerformed();
			}
			this.SampleSeatedAudience();
			foreach (KeyValuePair<string, string> keyValuePair in this._currentlyWorn)
			{
				if (this._performedWearing.Add(keyValuePair.Key))
				{
					MissionService.Report(MissionObjectiveKind.SpecificClothingEmote, 1f, keyValuePair.Key, null, null);
				}
			}
			Vector3 vector = ((this._emoteController != null) ? this._emoteController.transform.position : this._lastEmotePosition);
			this.RefreshTrafficLandmarks();
			foreach (MissionRuntimeHost.LandmarkRuntime landmarkRuntime in this._landmarks.Values)
			{
				if (!this._performedNear.Contains(landmarkRuntime.Id) && this.IsNearLandmark(landmarkRuntime, vector))
				{
					this._performedNear.Add(landmarkRuntime.Id);
					MissionService.Report(MissionObjectiveKind.EmoteNearLandmark, 1f, landmarkRuntime.Id, null, null);
					MissionService.Report(MissionObjectiveKind.SpecificEmoteNearLandmark, 1f, text, landmarkRuntime.Id, null);
				}
			}
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x003153F0 File Offset: 0x003135F0
		private void SampleEmoteWitness()
		{
			if (!MissionService.IsRunActive || this._activeEmote == null || this._witnessReported || SensoryManager.Instance == null || Time.timeScale <= 0f)
			{
				return;
			}
			if (MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, false) > 0)
			{
				this.ReportEmoteWitness(MissionRuntimeHost.EmoteKey(this._activeEmote));
			}
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x00315454 File Offset: 0x00313654
		private static int CountLiveWitnesses(SensoryManager sensory, bool seatedOnly = false)
		{
			if (sensory == null || sensory.IsPaused || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return 0;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.IsEventActive)
			{
				return 0;
			}
			if (PlayerStealthBridge.IsFullyHidden)
			{
				return 0;
			}
			int num = 0;
			IReadOnlyList<NPCSenses> registeredNPCs = sensory.GetRegisteredNPCs();
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (npcsenses != null && npcsenses.isActiveAndEnabled && !npcsenses.IsInCooldown && npcsenses.HasLineOfSight && (!seatedOnly || NPCSeatedEncounter.IsSeated(npcsenses)))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x00315504 File Offset: 0x00313704
		private void ReportEmoteWitness(string id)
		{
			if (this._witnessReported)
			{
				return;
			}
			this._witnessReported = true;
			this.NoteStatuePoseWitnessed(id);
			MissionService.Report(MissionObjectiveKind.SuccessfulExhibitions, 1f, null, null, null);
			MissionService.Report(MissionObjectiveKind.SpecificEmoteExhibition, 1f, id, null, null);
			foreach (KeyValuePair<string, string> keyValuePair in this._currentlyWorn)
			{
				MissionService.Report(MissionObjectiveKind.SpecificClothingExhibition, 1f, keyValuePair.Key, null, null);
			}
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x003155B4 File Offset: 0x003137B4
		private void HandlePlayerSeen(NPCSenses npc)
		{
			if (!MissionService.IsRunActive || npc == null || !npc.isActiveAndEnabled || Time.timeScale <= 0f)
			{
				return;
			}
			int instanceID = npc.GetInstanceID();
			float time = Time.time;
			float num;
			if (!this._npcLastSeen.TryGetValue(instanceID, out num) || time - num > 2f)
			{
				this._npcLookStart[instanceID] = time;
			}
			this._npcLastSeen[instanceID] = time;
			this.NotePlaceWitness(instanceID, time);
			if (this._activeEmote != null)
			{
				bool flag = !this._witnessReported;
				this.ReportEmoteWitness(MissionRuntimeHost.EmoteKey(this._activeEmote));
				if (flag)
				{
					this._npcLastSighting[instanceID] = time;
				}
				return;
			}
			if (this._currentlyWorn.Count == 0)
			{
				return;
			}
			float num2;
			if (this._npcLastSighting.TryGetValue(instanceID, out num2) && (num2 >= this._npcLookStart[instanceID] || time - num2 < 10f))
			{
				return;
			}
			this._npcLastSighting[instanceID] = time;
			foreach (KeyValuePair<string, string> keyValuePair in this._currentlyWorn)
			{
				MissionService.Report(MissionObjectiveKind.SpecificClothingExhibition, 1f, keyValuePair.Key, null, null);
			}
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x0031570C File Offset: 0x0031390C
		private void HandleOrgasmReached()
		{
			if (MissionService.IsRunActive)
			{
				MissionService.Report(MissionObjectiveKind.Orgasms, 1f, null, null, null);
			}
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x00315738 File Offset: 0x00313938
		private void HandleTrashcanLooted(int itemCount)
		{
			if (!MissionService.IsRunActive || itemCount <= 0)
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.TrashcanSearches, 1f, null, null, null);
			MissionService.Report(MissionObjectiveKind.TrashcanLootItems, (float)itemCount, null, null, null);
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x0031577C File Offset: 0x0031397C
		private void HandleFirstFlight()
		{
			MissionService.Report(MissionObjectiveKind.FirstFlight, 1f, null, null, null);
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x003157A0 File Offset: 0x003139A0
		private void HandleNpcPhotosCommitted(int photoCount)
		{
			if (!MissionService.IsRunActive || photoCount <= 0)
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.NpcPhotos, (float)photoCount, null, null, new MissionScope?(MissionScope.Global));
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x003157BE File Offset: 0x003139BE
		private void HandleNpcPhotoTaken(SmartphoneFlash phone)
		{
			this.CreditNpcPhoto(phone, 4f);
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x003157CC File Offset: 0x003139CC
		private void HandleNpcVideoStarted(SmartphoneFlash phone)
		{
			this.CreditNpcPhoto(phone, 6f);
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x003157DC File Offset: 0x003139DC
		private void CreditNpcPhoto(SmartphoneFlash phone, float cooldown)
		{
			if (!MissionService.IsRunActive || phone == null)
			{
				return;
			}
			if (!NpcPhotoRewardController.IsEligibleCielPhoto(phone))
			{
				return;
			}
			NPCSenses componentInParent = phone.GetComponentInParent<NPCSenses>();
			int num = ((componentInParent != null) ? componentInParent.GetInstanceID() : phone.GetInstanceID());
			float time = Time.time;
			this.TrackGroupShot(num, time);
			float num2;
			if (this._photoCredits.TryGetValue(num, out num2) && time - num2 < cooldown)
			{
				return;
			}
			this._photoCredits[num] = time;
			this._pendingPhotoCredits++;
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x00315864 File Offset: 0x00313A64
		private void FlushPhotoCredits()
		{
			if (this._pendingPhotoCredits <= 0)
			{
				return;
			}
			int pendingPhotoCredits = this._pendingPhotoCredits;
			this._pendingPhotoCredits = 0;
			if (!MissionService.IsRunActive)
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.NpcPhotos, (float)pendingPhotoCredits, null, null, new MissionScope?(MissionScope.Run));
			if (this.IsTopless)
			{
				MissionService.Report(MissionObjectiveKind.NpcPhotosTopless, (float)pendingPhotoCredits, null, null, new MissionScope?(MissionScope.Run));
			}
			if (this.IsNaked)
			{
				MissionService.Report(MissionObjectiveKind.NpcPhotosNaked, (float)pendingPhotoCredits, null, null, new MissionScope?(MissionScope.Run));
			}
		}

		// Token: 0x060078D2 RID: 30930 RVA: 0x003158D4 File Offset: 0x00313AD4
		private void HandleRefereeEscaped(RefereeChaser referee)
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (!MissionService.IsRunActive || referee == null || (instance != null && instance.IsEventActive))
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.Escapes, 1f, null, null, null);
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x00315920 File Offset: 0x00313B20
		private void HandleItemCollected(InventoryItemSO item, ItemRarity rarity)
		{
			if (item == null)
			{
				return;
			}
			if (item is WearableItemSO)
			{
				this.RefreshWearableCatalog();
			}
			if (!MissionService.IsRunActive)
			{
				return;
			}
			string text = MissionRuntimeHost.ItemKey(item);
			if (item.category == InventoryCategory.Clothing)
			{
				MissionService.Report(MissionObjectiveKind.ClothesCollected, 1f, text, null, null);
			}
			MissionService.Report(MissionObjectiveKind.ItemsCollected, 1f, text, null, null);
			if (rarity >= ItemRarity.Rare)
			{
				MissionService.Report(MissionObjectiveKind.RareLootCollected, 1f, text, null, null);
			}
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x003159A8 File Offset: 0x00313BA8
		private void HandleItemWorn(WearableItemSO item)
		{
			if (item == null)
			{
				return;
			}
			string text = MissionRuntimeHost.ItemKey(item);
			this._currentlyWorn[text] = (string.IsNullOrWhiteSpace(item.itemName) ? item.name : item.itemName);
			this.RefreshWearableCatalog();
			if (!MissionService.IsRunActive)
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.ClothesWorn, 1f, text, null, null);
			MissionService.Report(MissionObjectiveKind.SpecificClothingWorn, 1f, text, null, null);
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x00315A29 File Offset: 0x00313C29
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (item == null)
			{
				return;
			}
			this._currentlyWorn.Remove(MissionRuntimeHost.ItemKey(item));
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x00315A48 File Offset: 0x00313C48
		private void HandleWearableAction(WearableItemSO item, string actionId, string actionName)
		{
			if (!MissionService.IsRunActive || item == null)
			{
				return;
			}
			if ((actionId == "towel_slip" || actionId == "bikini_slip" || actionId == "swimsuit_slip") && this._wearController != null && !this._wearController.IsWorn(item))
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.WearableSpecialAction, 1f, MissionRuntimeHost.ItemKey(item), actionId, null);
			if (actionId == "towel_slip")
			{
				this.HandleTowelDropped();
			}
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x00315ADC File Offset: 0x00313CDC
		private void HandleClothingBroken(WearableItemSO item)
		{
			if (item == null)
			{
				return;
			}
			string text = MissionRuntimeHost.ItemKey(item);
			this._currentlyWorn.Remove(text);
			if (MissionService.IsRunActive)
			{
				MissionService.Report(MissionObjectiveKind.ClothingBroken, 1f, text, null, null);
			}
			base.StartCoroutine(this.RefreshWearablesNextFrame());
		}

		// Token: 0x060078D8 RID: 30936 RVA: 0x00315B32 File Offset: 0x00313D32
		private IEnumerator RefreshWearablesNextFrame()
		{
			yield return null;
			this.RefreshWearableCatalog();
			yield break;
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x00315B41 File Offset: 0x00313D41
		private static string EmoteKey(EmoteDefinitionSO emote)
		{
			if (string.IsNullOrWhiteSpace(emote.EmoteId))
			{
				return emote.name;
			}
			return emote.EmoteId;
		}

		// Token: 0x060078DA RID: 30938 RVA: 0x00315B5D File Offset: 0x00313D5D
		private static string ItemKey(InventoryItemSO item)
		{
			if (string.IsNullOrWhiteSpace(item.itemId))
			{
				return item.name;
			}
			return item.itemId;
		}

		// Token: 0x060078DB RID: 30939 RVA: 0x00315B7C File Offset: 0x00313D7C
		private static string ResolvePoiDisplayName(string poiId)
		{
			if (string.IsNullOrEmpty(poiId))
			{
				return string.Empty;
			}
			if (poiId == "ArtGallery")
			{
				return "Art Gallery";
			}
			if (poiId == "Pool")
			{
				return "City Pool";
			}
			if (poiId == "University")
			{
				return "University";
			}
			if (poiId == "Park")
			{
				return "Park";
			}
			if (poiId == "Stadium")
			{
				return "Stadium";
			}
			if (!(poiId == "Gymnasium"))
			{
				return poiId;
			}
			return "School Gymnasium";
		}

		// Token: 0x060078DC RID: 30940 RVA: 0x00315C0C File Offset: 0x00313E0C
		private void ClearRunUniqueness()
		{
			this._emoteStarts.Clear();
			this._activeEmote = null;
			this._movingEmoteDistance = 0f;
			this._photoCredits.Clear();
			this._pendingPhotoCredits = 0;
			this._npcLastSeen.Clear();
			this._npcLookStart.Clear();
			this._npcLastSighting.Clear();
			this.ResetRankObjectiveSampling();
			this.ResetPlaceObjectives();
		}

		// Token: 0x060078DD RID: 30941 RVA: 0x00315C78 File Offset: 0x00313E78
		private void BindStatsIfNeeded()
		{
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance == this._stats)
			{
				return;
			}
			this.UnbindStats();
			this._stats = instance;
			if (this._stats == null)
			{
				return;
			}
			this._stats.ScoreAwarded += this.HandleScoreAwarded;
			this._stats.RunTrackingChanged += this.HandleRunTrackingChanged;
			this._stats.RunReset += this.HandleRunReset;
			this._lastVehicleDistance = Mathf.Max(0f, this._stats.VehicleTravelMeters);
			if (this._stats.IsRunTrackingActive && !MissionService.IsRunActive)
			{
				this.BeginRunForCurrentContext();
			}
		}

		// Token: 0x060078DE RID: 30942 RVA: 0x00315D30 File Offset: 0x00313F30
		private void UnbindStats()
		{
			if (this._stats != null)
			{
				this._stats.ScoreAwarded -= this.HandleScoreAwarded;
				this._stats.RunTrackingChanged -= this.HandleRunTrackingChanged;
				this._stats.RunReset -= this.HandleRunReset;
			}
			this._stats = null;
		}

		// Token: 0x060078DF RID: 30943 RVA: 0x00315D98 File Offset: 0x00313F98
		private void BindPoiIfNeeded()
		{
			if (this._poiContext != null && this._poiContext.gameObject.scene.IsValid())
			{
				return;
			}
			this.UnbindPoi();
			this._poiContext = MissionRuntimeHost.FindPlayerComponent<PlayerPoiEmoteContext>();
			if (this._poiContext == null)
			{
				return;
			}
			this._poiContext.OnProfileChanged += this.HandleProfileChanged;
			this.RefreshEmoteCatalog();
		}

		// Token: 0x060078E0 RID: 30944 RVA: 0x00315E0B File Offset: 0x0031400B
		private void UnbindPoi()
		{
			if (this._poiContext != null)
			{
				this._poiContext.OnProfileChanged -= this.HandleProfileChanged;
			}
			this._poiContext = null;
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x00315E3C File Offset: 0x0031403C
		private void HandleRunTrackingChanged(bool active)
		{
			if (active)
			{
				this.ClearRunUniqueness();
				this.BeginRunForCurrentContext();
			}
			else
			{
				this.FlushVehicleDistance();
				this.FlushCoverTime(false);
				this.FlushRankSeconds();
				this.FlushPhotoCredits();
				MissionService.EndRun();
				this.ResetRankObjectiveSampling();
			}
			this._lastVehicleDistance = ((this._stats != null) ? Mathf.Max(0f, this._stats.VehicleTravelMeters) : 0f);
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x00315EAE File Offset: 0x003140AE
		private void HandleRunReset()
		{
			this.ClearRunUniqueness();
			this._lastVehicleDistance = 0f;
			if (this._stats != null && this._stats.IsRunTrackingActive)
			{
				this.BeginRunForCurrentContext();
			}
		}

		// Token: 0x060078E3 RID: 30947 RVA: 0x00315EE4 File Offset: 0x003140E4
		private void HandleScoreAwarded(RunScoreAward award)
		{
			if (!MissionService.IsRunActive || award.Points <= 0)
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.ScoreEarned, (float)award.Points, null, null, null);
			if (MissionRuntimeHost.IsPrimaryEscapeAward(award.Reason))
			{
				MissionService.Report(MissionObjectiveKind.Escapes, 1f, null, null, null);
				return;
			}
			switch (award.Reason)
			{
			case RunScoreReason.PoiDiscovered:
				MissionService.Report(MissionObjectiveKind.PoiDiscoveries, 1f, award.Detail, null, null);
				return;
			case RunScoreReason.Exhibition:
			case RunScoreReason.ExhibitionCrowd:
			case RunScoreReason.ExhibitionInPoi:
				break;
			case RunScoreReason.CarSurf:
				MissionService.Report(MissionObjectiveKind.CarSurfs, 1f, null, null, null);
				break;
			default:
				return;
			}
		}

		// Token: 0x060078E4 RID: 30948 RVA: 0x002F3147 File Offset: 0x002F1347
		private static bool IsPrimaryEscapeAward(RunScoreReason reason)
		{
			return reason == RunScoreReason.NpcEscape || reason == RunScoreReason.ReporterEscape || reason == RunScoreReason.PoliceEscape;
		}

		// Token: 0x060078E5 RID: 30949 RVA: 0x00315F94 File Offset: 0x00314194
		private void HandleProfileChanged(PoiEmoteProfileSO previous, PoiEmoteProfileSO current)
		{
			if (!MissionService.IsRunActive)
			{
				return;
			}
			this.RefreshEmoteCatalog();
		}

		// Token: 0x060078E6 RID: 30950 RVA: 0x00315FA4 File Offset: 0x003141A4
		private void BeginRunForCurrentContext()
		{
			if (!MissionService.IsRunSceneContext)
			{
				return;
			}
			string text;
			string text2;
			this.ResolveContext(out text, out text2);
			MissionService.BeginRun(text, text2);
		}

		// Token: 0x060078E7 RID: 30951 RVA: 0x00315FCA File Offset: 0x003141CA
		private void ResolveContext(out string id, out string display)
		{
			MissionRuntimeHost.ResolveSceneContext(SceneManager.GetActiveScene(), out id, out display);
		}

		// Token: 0x060078E8 RID: 30952 RVA: 0x00315FD8 File Offset: 0x003141D8
		private static void ResolveSceneContext(Scene scene, out string id, out string display)
		{
			string text = (scene.IsValid() ? scene.name : string.Empty);
			if (text.StartsWith("POI - ", StringComparison.OrdinalIgnoreCase))
			{
				display = text.Substring(6).Trim();
				id = display.Replace(" ", string.Empty);
				display = MissionRuntimeHost.ResolvePoiDisplayName(id);
				return;
			}
			id = "CITY";
			display = "the city";
		}

		// Token: 0x060078E9 RID: 30953 RVA: 0x00316044 File Offset: 0x00314244
		private void ApplySceneContext(Scene scene)
		{
			string text;
			string text2;
			MissionRuntimeHost.ResolveSceneContext(scene, out text, out text2);
			if (!string.Equals(text, MissionService.RunContextId, StringComparison.Ordinal) && this._contextualEmotes.Count > 0)
			{
				this._contextualEmotes.Clear();
				this.RefreshEmoteCatalog();
			}
			MissionService.SetRunContext(text, text2);
		}

		// Token: 0x060078EA RID: 30954 RVA: 0x00316090 File Offset: 0x00314290
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.UnbindPoi();
			this.UnbindEmotes();
			this.UnbindOrgasmMeter();
			this.UnbindWearController();
			this.UnbindCoverController();
			this.UnbindExposureSources();
			this._nextBindingRefresh = 0f;
			this._landmarks.Clear();
			MissionService.SetAvailableLandmarks(null);
			MissionSurfaceInstaller.InstallCurrentScene();
			this.RequestLandmarkRefresh();
			base.StartCoroutine(this.InstallSceneNextFrame(scene));
			if (MissionService.IsRunActive && scene == SceneManager.GetActiveScene())
			{
				this.ApplySceneContext(scene);
			}
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x00313B06 File Offset: 0x00311D06
		private void HandleSceneUnloaded(Scene scene)
		{
			this.RequestLandmarkRefresh();
		}

		// Token: 0x060078EC RID: 30956 RVA: 0x00316111 File Offset: 0x00314311
		private void HandleActiveSceneChanged(Scene previous, Scene current)
		{
			this.RequestLandmarkRefresh();
			if (!MissionService.IsRunSceneContext)
			{
				MissionService.EndRun();
				return;
			}
			if (MissionService.IsRunActive)
			{
				this.ApplySceneContext(current);
			}
		}

		// Token: 0x060078ED RID: 30957 RVA: 0x00316134 File Offset: 0x00314334
		private void RequestLandmarkRefresh()
		{
			this._landmarksDirty = true;
			this._landmarkRefreshGeneration++;
			this._landmarkRefreshNotBefore = Time.unscaledTime + 0.15f;
			if (this._landmarkRefreshRoutine == null && base.isActiveAndEnabled)
			{
				this._landmarkRefreshRoutine = base.StartCoroutine(this.RefreshLandmarksWhenStable());
			}
		}

		// Token: 0x060078EE RID: 30958 RVA: 0x00316189 File Offset: 0x00314389
		private IEnumerator RefreshLandmarksWhenStable()
		{
			Dictionary<string, MissionRuntimeHost.LandmarkRuntime> results;
			for (;;)
			{
				if (Time.unscaledTime >= this._landmarkRefreshNotBefore)
				{
					int generation = this._landmarkRefreshGeneration;
					this._landmarksDirty = false;
					results = new Dictionary<string, MissionRuntimeHost.LandmarkRuntime>(StringComparer.OrdinalIgnoreCase);
					yield return this.ScanLandmarksBudgeted(generation, results);
					if (generation == this._landmarkRefreshGeneration && !this._landmarksDirty)
					{
						break;
					}
				}
				else
				{
					yield return null;
				}
			}
			this.PublishLandmarks(results);
			if (this._refreshRunMissionsAfterLandmarks)
			{
				this._refreshRunMissionsAfterLandmarks = false;
				MissionService.RefreshRunMissionsAfterWorldScan();
			}
			this._landmarkRefreshRoutine = null;
			yield break;
		}

		// Token: 0x060078EF RID: 30959 RVA: 0x00316198 File Offset: 0x00314398
		private static T FindPlayerComponent<T>() where T : Component
		{
			Transform player = PlayerRegistry.Player;
			if (!(player != null))
			{
				return default(T);
			}
			return player.GetComponentInChildren<T>();
		}

		// Token: 0x060078F0 RID: 30960 RVA: 0x003161C4 File Offset: 0x003143C4
		private IEnumerator InstallCurrentSceneNextFrame()
		{
			yield return null;
			MissionSurfaceInstaller.InstallCurrentScene();
			this.RefreshWearableCatalog();
			yield break;
		}

		// Token: 0x060078F1 RID: 30961 RVA: 0x003161D3 File Offset: 0x003143D3
		private IEnumerator InstallSceneNextFrame(Scene scene)
		{
			yield return null;
			MissionSurfaceInstaller.InstallCurrentScene();
			if (scene != SceneManager.GetActiveScene())
			{
				yield break;
			}
			this.RefreshWearableCatalog();
			this._refreshRunMissionsAfterLandmarks = true;
			this.RequestLandmarkRefresh();
			yield break;
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x003161EC File Offset: 0x003143EC
		private void SubscribePlaceObjectives()
		{
			HackMeStoreZone.BroadcastStarted += this.HandleLedWallBroadcast;
			SubwayTravelService.WorldRelocated += this.HandleSubwayDeparture;
			vVehicleController.OnAnyVehicleMounted += this.HandleVehicleMounted;
			vVehicleController.OnAnyVehicleDismounted += this.HandleVehicleDismounted;
			PoliceOfficerChaser.OnChaseStarted += this.HandleOfficerChaseStarted;
			PoliceOfficerChaser.OnChaseEscaped += this.HandleOfficerChaseEnded;
			PolicePatrol.OnChaseStarted += this.HandlePatrolChaseStarted;
			PolicePatrol.OnChaseEscaped += this.HandlePatrolChaseEnded;
			NPCSenses.OnPlayerSpotted += this.HandlePlayerSpotted;
			MissionService.ClassicHomeReturnReached += this.HandleClassicHomeReturn;
		}

		// Token: 0x060078F3 RID: 30963 RVA: 0x003162A4 File Offset: 0x003144A4
		private void UnsubscribePlaceObjectives()
		{
			HackMeStoreZone.BroadcastStarted -= this.HandleLedWallBroadcast;
			SubwayTravelService.WorldRelocated -= this.HandleSubwayDeparture;
			vVehicleController.OnAnyVehicleMounted -= this.HandleVehicleMounted;
			vVehicleController.OnAnyVehicleDismounted -= this.HandleVehicleDismounted;
			PoliceOfficerChaser.OnChaseStarted -= this.HandleOfficerChaseStarted;
			PoliceOfficerChaser.OnChaseEscaped -= this.HandleOfficerChaseEnded;
			PolicePatrol.OnChaseStarted -= this.HandlePatrolChaseStarted;
			PolicePatrol.OnChaseEscaped -= this.HandlePatrolChaseEnded;
			NPCSenses.OnPlayerSpotted -= this.HandlePlayerSpotted;
			MissionService.ClassicHomeReturnReached -= this.HandleClassicHomeReturn;
		}

		// Token: 0x060078F4 RID: 30964 RVA: 0x0031635B File Offset: 0x0031455B
		private void BindPlaceSources(Transform player)
		{
			this._tint = player.GetComponentInChildren<TintBodyPaintRuntimeController>(true);
			this._bubbles = player.GetComponentInChildren<ShowerBubbleSpawner>(true);
			this._swimming = player.GetComponentInChildren<vSwimming>(true);
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x00316384 File Offset: 0x00314584
		private void UnbindPlaceSources()
		{
			this._tint = null;
			this._bubbles = null;
			this._swimming = null;
			this._hasSwimPosition = false;
		}

		// Token: 0x060078F6 RID: 30966 RVA: 0x003163A4 File Offset: 0x003145A4
		private void ResetPlaceObjectives()
		{
			this._lastPlayerSeenTime = -100f;
			this._poseAudience.Clear();
			this._poseAudienceReported = 0;
			this._bestSeatedAudience = 0;
			this._poseCollectionMissionId = null;
			this._poseCollection.Clear();
			this._hasSwimPosition = false;
			this._subwayRidePending = false;
			this._groupShotHits.Clear();
			this._ghostScared.Clear();
			this._policeChasers.Clear();
			this._lastPoliceSightTime = -100f;
			this._unicycle = null;
			this._pendingUnicycleMeters = 0f;
		}

		// Token: 0x060078F7 RID: 30967 RVA: 0x00316434 File Offset: 0x00314634
		private void ClearPlaceScan()
		{
			this._scanStandPoses.Clear();
			this._scanVegetables.Clear();
			this._scanScreen = null;
			this._scanParachute = null;
			this._scanLedWall = (this._scanTintPuddle = (this._scanShowers = (this._scanGrates = (this._scanSubway = (this._scanTreadmill = false)))));
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x0031649C File Offset: 0x0031469C
		private void InspectPlaceProvider(Transform candidate)
		{
			StandInteractable standInteractable;
			if (candidate.TryGetComponent<StandInteractable>(out standInteractable))
			{
				FieldInfo field = typeof(StandInteractable).GetField("profile", BindingFlags.Instance | BindingFlags.NonPublic);
				StandActionProfileSO standActionProfileSO = ((field != null) ? field.GetValue(standInteractable) : null) as StandActionProfileSO;
				if (standActionProfileSO != null && standActionProfileSO.Actions != null)
				{
					foreach (StandActionProfileSO.StandAction standAction in standActionProfileSO.Actions)
					{
						if (standAction != null && standAction.Emote != null)
						{
							this._scanStandPoses.Add(MissionRuntimeHost.EmoteKey(standAction.Emote));
						}
					}
				}
			}
			GymExerciseStation gymExerciseStation;
			if (candidate.TryGetComponent<GymExerciseStation>(out gymExerciseStation))
			{
				FieldInfo field2 = typeof(GymExerciseStation).GetField("exercises", BindingFlags.Instance | BindingFlags.NonPublic);
				List<EmoteDefinitionSO> list = ((field2 != null) ? field2.GetValue(gymExerciseStation) : null) as List<EmoteDefinitionSO>;
				if (list != null)
				{
					if (list.Exists((EmoteDefinitionSO e) => e != null && string.Equals(MissionRuntimeHost.EmoteKey(e), "mall_gym_treadmill_run", StringComparison.OrdinalIgnoreCase)))
					{
						this._scanTreadmill = true;
					}
				}
			}
			VegetableTheftSource vegetableTheftSource;
			if (candidate.TryGetComponent<VegetableTheftSource>(out vegetableTheftSource))
			{
				this._scanVegetables.Add(vegetableTheftSource);
			}
			StadiumScreenController stadiumScreenController;
			if (this._scanScreen == null && candidate.TryGetComponent<StadiumScreenController>(out stadiumScreenController))
			{
				this._scanScreen = stadiumScreenController;
			}
			StadiumParachuteController stadiumParachuteController;
			if (this._scanParachute == null && candidate.TryGetComponent<StadiumParachuteController>(out stadiumParachuteController))
			{
				this._scanParachute = stadiumParachuteController;
			}
			HackMeStoreZone hackMeStoreZone;
			if (candidate.TryGetComponent<HackMeStoreZone>(out hackMeStoreZone) && hackMeStoreZone.IsSingleScreen)
			{
				this._scanLedWall = true;
			}
			TintPuddleInteractable tintPuddleInteractable;
			if (candidate.TryGetComponent<TintPuddleInteractable>(out tintPuddleInteractable))
			{
				this._scanTintPuddle = true;
			}
			ShowerInteraction showerInteraction;
			if (candidate.TryGetComponent<ShowerInteraction>(out showerInteraction))
			{
				this._scanShowers = true;
			}
			SpicyHatchWind spicyHatchWind;
			if (candidate.TryGetComponent<SpicyHatchWind>(out spicyHatchWind))
			{
				this._scanGrates = true;
			}
			SubwayRideDirector subwayRideDirector;
			if (candidate.TryGetComponent<SubwayRideDirector>(out subwayRideDirector))
			{
				this._scanSubway = true;
			}
		}

		// Token: 0x060078F9 RID: 30969 RVA: 0x0031667C File Offset: 0x0031487C
		private void PublishPlaceScan()
		{
			this._standPoses.Clear();
			this._standPoses.UnionWith(this._scanStandPoses);
			this._vegetables.Clear();
			this._vegetables.AddRange(this._scanVegetables);
			foreach (VegetableTheftSource vegetableTheftSource in this._vegetables)
			{
				if (vegetableTheftSource != null && vegetableTheftSource.HasBeenLooted)
				{
					this._lootedVegetables.Add(vegetableTheftSource);
				}
			}
			this._screen = this._scanScreen;
			this._parachute = this._scanParachute;
			this._ledWall = this._scanLedWall;
			this._tintPuddle = this._scanTintPuddle;
			this._showers = this._scanShowers;
			this._grates = this._scanGrates;
			this._subway = this._scanSubway;
			this._treadmill = this._scanTreadmill;
		}

		// Token: 0x060078FA RID: 30970 RVA: 0x00316780 File Offset: 0x00314980
		private void RescanIfPlaceLooksEmpty(string context)
		{
			if ((context == "Stadium" || context == "Pool") && this._screen == null)
			{
				this._screen = Object.FindObjectOfType<StadiumScreenController>();
				if (this._parachute == null)
				{
					this._parachute = Object.FindObjectOfType<StadiumParachuteController>();
				}
			}
			bool flag;
			if (!(context == "ArtGallery"))
			{
				if (!(context == "Stadium") && !(context == "Pool"))
				{
					if (!(context == "Mall"))
					{
						if (!(context == "University"))
						{
							return;
						}
						flag = !this._ledWall;
					}
					else
					{
						flag = !this._showers && !this._treadmill;
					}
				}
				else
				{
					flag = this._screen == null;
				}
			}
			else
			{
				flag = this._standPoses.Count == 0;
			}
			if (!string.Equals(context, this._placeRescanContext, StringComparison.Ordinal))
			{
				this._placeRescanContext = context;
				this._placeRescans = 0;
			}
			if (!flag || this._placeRescans >= 3 || Time.unscaledTime < this._nextPlaceRescan)
			{
				return;
			}
			this._placeRescans++;
			this._nextPlaceRescan = Time.unscaledTime + 10f;
			this.RequestLandmarkRefresh();
		}

		// Token: 0x060078FB RID: 30971 RVA: 0x003168B8 File Offset: 0x00314AB8
		private void PublishPlaceCapabilities()
		{
			List<MissionObjectiveKind> list = new List<MissionObjectiveKind>();
			this.RescanIfPlaceLooksEmpty(MissionService.RunContextId);
			string runContextId = MissionService.RunContextId;
			if (!(runContextId == "ArtGallery"))
			{
				if (!(runContextId == "Stadium"))
				{
					if (!(runContextId == "University"))
					{
						if (!(runContextId == "Mall"))
						{
							if (!(runContextId == "Pool"))
							{
								if (runContextId == "CITY")
								{
									if (this._canGoBraless && this._vegetables.Exists((VegetableTheftSource v) => v != null && !this._lootedVegetables.Contains(v)))
									{
										list.Add(MissionObjectiveKind.KombiniToplessTheft);
									}
									if (this._grates)
									{
										if (this._ownedWearables.Exists((WearableItemSO item) => item != null && item.containsSkirt))
										{
											list.Add(MissionObjectiveKind.GrateWindSeenTime);
										}
									}
									if (this._subway && this._canGoBraless)
									{
										list.Add(MissionObjectiveKind.SubwayToplessRides);
									}
								}
							}
							else
							{
								if (this._swimming != null)
								{
									list.Add(MissionObjectiveKind.NakedSwimDistance);
								}
								if (this._screen != null && this._canGoBraless)
								{
									list.Add(MissionObjectiveKind.PoolScreenToplessSeenTime);
								}
							}
						}
						else
						{
							if (this._treadmill && this._canGoBraless)
							{
								list.Add(MissionObjectiveKind.TreadmillToplessTime);
							}
							if (this._showers)
							{
								list.Add(MissionObjectiveKind.GymShowerTime);
								list.Add(MissionObjectiveKind.ShowerFoamSeenTime);
							}
						}
					}
					else
					{
						list.Add(MissionObjectiveKind.SeatedStudentsEmoteAudience);
						if (this._ledWall && PhoneAppAccessPolicy.GetAccess("HackMe") == PhoneAppAccess.Available)
						{
							list.Add(MissionObjectiveKind.LedWallFaceBroadcast);
						}
						if (this.Owns("cloth_university_uniform_skirt") || this.Owns("cloth_university_uniform_full"))
						{
							list.Add(MissionObjectiveKind.UniformSkirtSeenTime);
						}
					}
				}
				else if (this._screen != null)
				{
					list.Add(MissionObjectiveKind.StadiumScreenExposedTime);
					list.Add(MissionObjectiveKind.StadiumScreenEmotes);
				}
			}
			else if (this._standPoses.Count > 0)
			{
				list.Add(MissionObjectiveKind.StatuePoseSeenTime);
				list.Add(MissionObjectiveKind.StatuePoseAudience);
				list.Add(MissionObjectiveKind.StatuePoseCollection);
				if (this._tintPuddle && this._tint != null)
				{
					list.Add(MissionObjectiveKind.PaintedStatueSeenTime);
				}
			}
			MissionService.SetPlaceCapabilities(list);
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x060078FC RID: 30972 RVA: 0x00316AFB File Offset: 0x00314CFB
		private bool IsTopless
		{
			get
			{
				return this._clothing != null && this._clothing.UsingService && !this._clothing.HasVisibleUpperGarment;
			}
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x060078FD RID: 30973 RVA: 0x00316B28 File Offset: 0x00314D28
		private bool IsNaked
		{
			get
			{
				return this._clothing != null && this._clothing.UsingService && !this._clothing.IsWearingAnything(true);
			}
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x00316B56 File Offset: 0x00314D56
		private bool IsStandPose(EmoteDefinitionSO emote)
		{
			return emote != null && this._standPoses.Contains(MissionRuntimeHost.EmoteKey(emote));
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x00316B74 File Offset: 0x00314D74
		private void AccumulatePlaceProgress(float dt, float now)
		{
			MissionRuntimeHost.<>c__DisplayClass217_0 CS$<>8__locals1;
			CS$<>8__locals1.witnesses = -1;
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			bool flag = instance != null && instance.IsEventActive;
			string runContextId = MissionService.RunContextId;
			if (!(runContextId == "ArtGallery"))
			{
				if (runContextId == "Stadium")
				{
					bool flag2 = this._parachute != null && (this._parachute.IsFlying || this._parachute.IsLanding);
					this.AddSeconds(MissionObjectiveKind.StadiumScreenExposedTime, this._screen != null && this._screen.IsPlayerFocusActive && !flag2 && !flag && PlayerExposureState.IsExposed, dt);
					return;
				}
				if (!(runContextId == "University"))
				{
					if (!(runContextId == "Mall"))
					{
						if (!(runContextId == "Pool"))
						{
							if (!(runContextId == "CITY"))
							{
								return;
							}
							if (now - SpicyHatchWind.LastWindTime <= 0.2f && MissionService.HasActiveRunMission(MissionObjectiveKind.GrateWindSeenTime))
							{
								this.AddSeconds(MissionObjectiveKind.GrateWindSeenTime, !flag && (now - this._lastPlayerSeenTime <= 0.3f || MissionRuntimeHost.<AccumulatePlaceProgress>g__Witnesses|217_0(ref CS$<>8__locals1) > 0), dt);
							}
						}
						else
						{
							this.SampleNakedSwim();
							if (this._screen != null && this._screen.IsPlayerFocusActive && this.IsTopless && MissionService.HasActiveRunMission(MissionObjectiveKind.PoolScreenToplessSeenTime))
							{
								this.AddSeconds(MissionObjectiveKind.PoolScreenToplessSeenTime, MissionRuntimeHost.<AccumulatePlaceProgress>g__Witnesses|217_0(ref CS$<>8__locals1) > 0, dt);
								return;
							}
						}
					}
					else
					{
						this.AddSeconds(MissionObjectiveKind.TreadmillToplessTime, this.IsTopless && this._activeEmote != null && string.Equals(MissionRuntimeHost.EmoteKey(this._activeEmote), "mall_gym_treadmill_run", StringComparison.OrdinalIgnoreCase), dt);
						bool isAnyShowerActionActive = ShowerInteraction.IsAnyShowerActionActive;
						this.AddSeconds(MissionObjectiveKind.GymShowerTime, isAnyShowerActionActive, dt);
						if (!isAnyShowerActionActive && this._bubbles != null && this._bubbles.BubblesActive && MissionService.HasActiveRunMission(MissionObjectiveKind.ShowerFoamSeenTime))
						{
							this.AddSeconds(MissionObjectiveKind.ShowerFoamSeenTime, MissionRuntimeHost.<AccumulatePlaceProgress>g__Witnesses|217_0(ref CS$<>8__locals1) > 0, dt);
							return;
						}
					}
				}
				else if (this.IsUniformSkirtFlashOpen() && MissionService.HasActiveRunMission(MissionObjectiveKind.UniformSkirtSeenTime))
				{
					this.AddSeconds(MissionObjectiveKind.UniformSkirtSeenTime, MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, true) > 0, dt);
					return;
				}
			}
			else if (this.IsStandPose(this._activeEmote) && !flag && (now - this._lastPlayerSeenTime <= 0.3f || MissionRuntimeHost.<AccumulatePlaceProgress>g__Witnesses|217_0(ref CS$<>8__locals1) > 0))
			{
				this.AddSeconds(MissionObjectiveKind.StatuePoseSeenTime, true, dt);
				this.AddSeconds(MissionObjectiveKind.PaintedStatueSeenTime, this._tint != null && this._tint.IsTintBodyPaintActive(), dt);
				return;
			}
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x00316E1C File Offset: 0x0031501C
		private bool IsUniformSkirtFlashOpen()
		{
			for (int i = 0; i < this._garmentFlashes.Length; i++)
			{
				ArmsFlashController armsFlashController = this._garmentFlashes[i];
				if (armsFlashController != null && armsFlashController.isActiveAndEnabled && armsFlashController.IsOpen && armsFlashController.Config != null && string.Equals(armsFlashController.Config.flashLayerName, "UniversitySkirtFlash", StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x00316E84 File Offset: 0x00315084
		private void SampleNakedSwim()
		{
			Transform player = PlayerRegistry.Player;
			if (!(player != null) || !(this._swimming != null) || !this._swimming.isSwimming || !this.IsNaked || !MissionService.HasActiveRunMission(MissionObjectiveKind.NakedSwimDistance))
			{
				this._hasSwimPosition = false;
				return;
			}
			Vector3 position = player.position;
			if (this._hasSwimPosition)
			{
				Vector3 vector = position - this._lastSwimPosition;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude <= 2f)
				{
					this.AddSeconds(MissionObjectiveKind.NakedSwimDistance, true, magnitude);
				}
			}
			this._lastSwimPosition = position;
			this._hasSwimPosition = true;
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x00316F28 File Offset: 0x00315128
		private void PollPlaceEvents(float now)
		{
			for (int i = 0; i < this._vegetables.Count; i++)
			{
				VegetableTheftSource vegetableTheftSource = this._vegetables[i];
				if (!(vegetableTheftSource == null) && vegetableTheftSource.HasBeenLooted && this._lootedVegetables.Add(vegetableTheftSource) && this.IsTopless && string.Equals(MissionService.RunContextId, "CITY", StringComparison.Ordinal))
				{
					MissionService.Report(MissionObjectiveKind.KombiniToplessTheft, 1f, null, null, new MissionScope?(MissionScope.Run));
				}
			}
			if (!this._subwayRidePending)
			{
				return;
			}
			if (now - this._subwayRideStartedAt > 240f)
			{
				this._subwayRidePending = false;
				return;
			}
			if (this._subwayDirector == null || !this._subwayDirector.HasActiveResidencyRide)
			{
				this._subwayDirector = null;
				foreach (SubwayRideDirector subwayRideDirector in Object.FindObjectsOfType<SubwayRideDirector>())
				{
					if (subwayRideDirector.HasActiveResidencyRide)
					{
						this._subwayDirector = subwayRideDirector;
						break;
					}
				}
			}
			if (this._subwayDirector == null)
			{
				return;
			}
			if (!this._subwayDirector.DoorsOpen)
			{
				this._subwayDoorsClosedSinceDeparture = true;
				return;
			}
			if (!this._subwayDoorsClosedSinceDeparture)
			{
				return;
			}
			this._subwayRidePending = false;
			if (SubwayTravelService.LastTripMeters <= 0.01f)
			{
				return;
			}
			if (this.IsTopless)
			{
				MissionService.Report(MissionObjectiveKind.SubwayToplessRides, 1f, null, null, new MissionScope?(MissionScope.Run));
			}
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x00317070 File Offset: 0x00315270
		private void BeginEmoteAttempt()
		{
			this._poseAudience.Clear();
			this._poseAudienceReported = 0;
			this._bestSeatedAudience = 0;
		}

		// Token: 0x06007904 RID: 30980 RVA: 0x0031708C File Offset: 0x0031528C
		private void NotePlaceWitness(int npcId, float now)
		{
			this._lastPlayerSeenTime = now;
			if (!this.IsStandPose(this._activeEmote) || !string.Equals(MissionService.RunContextId, "ArtGallery", StringComparison.Ordinal))
			{
				return;
			}
			this._poseAudience.Add(npcId);
			if (this._poseAudience.Count <= this._poseAudienceReported || !MissionService.HasActiveRunMission(MissionObjectiveKind.StatuePoseAudience))
			{
				return;
			}
			this._poseAudienceReported = this._poseAudience.Count;
			MissionService.Report(MissionObjectiveKind.StatuePoseAudience, (float)this._poseAudience.Count, null, null, new MissionScope?(MissionScope.Run));
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x00317118 File Offset: 0x00315318
		private void NoteStatuePoseWitnessed(string emoteId)
		{
			if (!this.IsStandPose(this._activeEmote) || !string.Equals(MissionService.RunContextId, "ArtGallery", StringComparison.Ordinal))
			{
				return;
			}
			MissionState missionState = null;
			foreach (MissionState missionState2 in MissionService.RunMissions)
			{
				if (missionState2 != null && !missionState2.IsComplete && missionState2.kind == MissionObjectiveKind.StatuePoseCollection)
				{
					missionState = missionState2;
				}
			}
			if (missionState == null)
			{
				return;
			}
			if (!string.Equals(this._poseCollectionMissionId, missionState.id, StringComparison.Ordinal))
			{
				this._poseCollectionMissionId = missionState.id;
				this._poseCollection.Clear();
			}
			if (this._poseCollection.Add(emoteId))
			{
				MissionService.Report(MissionObjectiveKind.StatuePoseCollection, 1f, null, null, new MissionScope?(MissionScope.Run));
			}
		}

		// Token: 0x06007906 RID: 30982 RVA: 0x003171E8 File Offset: 0x003153E8
		private void NoteEmotePerformed()
		{
			if (string.Equals(MissionService.RunContextId, "Stadium", StringComparison.Ordinal) && this._screen != null && this._screen.IsPlayerFocusActive)
			{
				MissionService.Report(MissionObjectiveKind.StadiumScreenEmotes, 1f, null, null, new MissionScope?(MissionScope.Run));
			}
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x00317238 File Offset: 0x00315438
		private void SampleSeatedAudience()
		{
			if (!string.Equals(MissionService.RunContextId, "University", StringComparison.Ordinal) || !MissionService.HasActiveRunMission(MissionObjectiveKind.SeatedStudentsEmoteAudience))
			{
				return;
			}
			int num = MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, true);
			if (num <= this._bestSeatedAudience)
			{
				return;
			}
			this._bestSeatedAudience = num;
			MissionService.Report(MissionObjectiveKind.SeatedStudentsEmoteAudience, (float)num, null, null, new MissionScope?(MissionScope.Run));
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x00317290 File Offset: 0x00315490
		private void HandleLedWallBroadcast(HackMeStoreZone zone, string sourcePath, bool video)
		{
			if (!MissionService.IsRunActive || video || zone == null || !zone.IsSingleScreen || !string.Equals(MissionService.RunContextId, "University", StringComparison.Ordinal) || !OxGramPhotoTag.ShowsFace(sourcePath))
			{
				return;
			}
			MissionService.Report(MissionObjectiveKind.LedWallFaceBroadcast, 1f, null, null, new MissionScope?(MissionScope.Run));
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x003172EC File Offset: 0x003154EC
		private void HandleSubwayDeparture(Transform train)
		{
			if (!MissionService.IsRunActive || !this.IsTopless || !MissionService.HasActiveRunMission(MissionObjectiveKind.SubwayToplessRides))
			{
				return;
			}
			this._subwayRidePending = true;
			this._subwayDoorsClosedSinceDeparture = false;
			this._subwayRideStartedAt = Time.time;
			this._subwayDirector = ((train != null) ? train.GetComponent<SubwayRideDirector>() : null);
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x00317344 File Offset: 0x00315544
		private void TrackGroupShot(int photographerId, float now)
		{
			if (!MissionService.IsRunActive)
			{
				return;
			}
			this._groupShotHits.RemoveAll((KeyValuePair<int, float> hit) => now - hit.Value > 5f);
			this._groupShotHits.RemoveAll((KeyValuePair<int, float> hit) => hit.Key == photographerId);
			this._groupShotHits.Add(new KeyValuePair<int, float>(photographerId, now));
			if (this._groupShotHits.Count >= 5)
			{
				MissionService.Report(MissionObjectiveKind.GroupShot, 1f, null, null, new MissionScope?(MissionScope.Global));
			}
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x003173DC File Offset: 0x003155DC
		private void HandlePlayerSpotted(NPCSenses npc)
		{
			if (!MissionService.IsRunActive || npc == null || (WardrobeComboRuntime.ActiveCombos & WardrobeCombo.Ghost) == WardrobeCombo.None)
			{
				return;
			}
			if (this._ghostScared.Add(npc.GetInstanceID()) && this._ghostScared.Count >= 5)
			{
				MissionService.Report(MissionObjectiveKind.GhostScare, 1f, null, null, new MissionScope?(MissionScope.Global));
			}
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x00317439 File Offset: 0x00315639
		private void HandleOfficerChaseStarted(PoliceOfficerChaser chaser)
		{
			if (chaser != null)
			{
				this._policeChasers.Add(chaser);
			}
		}

		// Token: 0x0600790D RID: 30989 RVA: 0x00317451 File Offset: 0x00315651
		private void HandleOfficerChaseEnded(PoliceOfficerChaser chaser)
		{
			this._policeChasers.Remove(chaser);
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x00317439 File Offset: 0x00315639
		private void HandlePatrolChaseStarted(PolicePatrol patrol)
		{
			if (patrol != null)
			{
				this._policeChasers.Add(patrol);
			}
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x00317451 File Offset: 0x00315651
		private void HandlePatrolChaseEnded(PolicePatrol patrol)
		{
			this._policeChasers.Remove(patrol);
		}

		// Token: 0x06007910 RID: 30992 RVA: 0x00317460 File Offset: 0x00315660
		private void HandleVehicleMounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			if (vehicle == null || vehicle.Type != vVehicleController.VehicleType.Unicycle)
			{
				return;
			}
			this._unicycle = vehicle.transform;
			this._lastUnicyclePosition = this._unicycle.position;
			this._lastUnicycleTime = Time.time;
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x0031749D File Offset: 0x0031569D
		private void HandleVehicleDismounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			if (vehicle == null || vehicle.transform != this._unicycle)
			{
				return;
			}
			this.SampleUnicycle(Time.time);
			this.FlushUnicycleMeters();
			this._unicycle = null;
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x003174D4 File Offset: 0x003156D4
		private void UpdateSecretObjectives()
		{
			float time = Time.time;
			if (this._unicycle != null)
			{
				this.SampleUnicycle(time);
			}
			if (time < this._nextSecretSample)
			{
				return;
			}
			this._nextSecretSample = time + 0.1f;
			if (this._policeChasers.Count > 0)
			{
				this._policeChasers.RemoveWhere((Object chaser) => chaser == null);
				PoliceManager instance = PoliceManager.Instance;
				if (instance != null && instance.AnyOfficerHasLineOfSight)
				{
					this._lastPoliceSightTime = time;
				}
			}
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x0031756C File Offset: 0x0031576C
		private void SampleUnicycle(float now)
		{
			Vector3 position = this._unicycle.position;
			Vector3 vector = position - this._lastUnicyclePosition;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float num = Mathf.Max(0.0001f, now - this._lastUnicycleTime);
			this._lastUnicyclePosition = position;
			this._lastUnicycleTime = now;
			if (magnitude > Mathf.Max(25f, num * 80f) || !MissionService.IsRunActive)
			{
				return;
			}
			this._pendingUnicycleMeters += magnitude;
			if (this._pendingUnicycleMeters >= 10f)
			{
				this.FlushUnicycleMeters();
			}
		}

		// Token: 0x06007914 RID: 30996 RVA: 0x00317604 File Offset: 0x00315804
		private void FlushUnicycleMeters()
		{
			if (this._pendingUnicycleMeters <= 0f)
			{
				return;
			}
			float pendingUnicycleMeters = this._pendingUnicycleMeters;
			this._pendingUnicycleMeters = 0f;
			if (MissionService.IsRunActive)
			{
				MissionService.Report(MissionObjectiveKind.UnicycleDistance, pendingUnicycleMeters, null, null, new MissionScope?(MissionScope.Global));
			}
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x00317648 File Offset: 0x00315848
		private void HandleClassicHomeReturn(float peakMeters)
		{
			if (peakMeters >= 1000f && this._stats != null && this._stats.GetSnapshot().TimesSeen == 0)
			{
				MissionService.Report(MissionObjectiveKind.GhostProtocol, 1f, null, null, new MissionScope?(MissionScope.Global));
			}
			this._policeChasers.RemoveWhere((Object chaser) => chaser == null);
			if (this._policeChasers.Count > 0 && Time.time - this._lastPoliceSightTime <= 5f)
			{
				MissionService.Report(MissionObjectiveKind.MostWantedHome, 1f, null, null, new MissionScope?(MissionScope.Global));
			}
		}

		// Token: 0x06007916 RID: 30998 RVA: 0x003176F4 File Offset: 0x003158F4
		private void BindExposureSourcesIfNeeded()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null || player == this._exposureOwner)
			{
				return;
			}
			this.UnbindExposureSources();
			this._exposureOwner = player;
			this._clothing = player.GetComponentInChildren<ClothingToggleController>(true);
			this._lingerie = player.GetComponentInChildren<ExposeLingerieController>(true);
			TowelClothBridge componentInChildren = player.GetComponentInChildren<TowelClothBridge>(true);
			this._towelFlash = ((componentInChildren != null) ? componentInChildren.GetComponent<ClothFullFlashController>() : null);
			this._towelSlip = ((componentInChildren != null) ? componentInChildren.GetComponent<ClothSlipController>() : null);
			this._garmentFlashes = player.GetComponentsInChildren<ArmsFlashController>(true);
			this._hideFlash = player.GetComponentInChildren<HideFlashController>(true);
			this.BindPlaceSources(player);
		}

		// Token: 0x06007917 RID: 30999 RVA: 0x0031779F File Offset: 0x0031599F
		private void UnbindExposureSources()
		{
			this._exposureOwner = null;
			this._clothing = null;
			this._lingerie = null;
			this._towelFlash = null;
			this._towelSlip = null;
			this._garmentFlashes = Array.Empty<ArmsFlashController>();
			this._hideFlash = null;
			this.UnbindPlaceSources();
		}

		// Token: 0x06007918 RID: 31000 RVA: 0x003177DC File Offset: 0x003159DC
		private void ResetRankObjectiveSampling()
		{
			this._rankSeconds.Clear();
			this._lastRankSampleTime = -1f;
			this._lastWitnessTime = -100f;
		}

		// Token: 0x06007919 RID: 31001 RVA: 0x00317800 File Offset: 0x00315A00
		private void PublishRankCapabilities()
		{
			List<MissionObjectiveKind> list = new List<MissionObjectiveKind>();
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			bool isAdminRankActive = RankProgressionService.IsAdminRankActive;
			this._canGoBraless = false;
			if (this._wearController == null || (currentRank == null && !isAdminRankActive))
			{
				MissionService.SetRankCapabilities(list);
				return;
			}
			bool flag = isAdminRankActive || currentRank.allowLingerieOpenAnimations;
			bool flag2 = isAdminRankActive || currentRank.allowFlash;
			bool fullUndress = isAdminRankActive || currentRank.allowFullUndress;
			bool flag3 = fullUndress || currentRank.allowBraRemoval;
			bool flag4 = isAdminRankActive || currentRank.allowCoverPose;
			bool flag5 = !isAdminRankActive && string.Equals(currentRank.rankId, "curious", StringComparison.Ordinal);
			CoverPoseController instance = CoverPoseController.Instance;
			bool flag6 = instance != null && instance.IsFutanariLocked;
			bool flag7 = this.Owns("cloth_expose_bra");
			bool flag8 = this.Owns("cloth_expose_panties");
			bool flag9 = this.Owns("cloth_towel") || (this._towelSlip != null && this._towelSlip.HasSlippedPiece("Bath_Towel"));
			object obj = (flag && flag7) || (flag2 && (flag9 || this.OwnsAny(MissionRuntimeHost.TopFlashGarments)));
			bool flag10 = (flag && flag8) || (flag2 && (flag9 || this.OwnsAny(MissionRuntimeHost.BottomFlashGarments)));
			bool flag11 = (flag && flag7 && flag8) || (flag2 && (flag9 || this.Owns("cloth_raincoat")));
			object obj2 = obj;
			if (obj2 != null)
			{
				list.Add(MissionObjectiveKind.FlashTopSeenTime);
			}
			if (flag10)
			{
				list.Add(MissionObjectiveKind.FlashBottomSeenTime);
			}
			if ((obj2 | flag10) != null)
			{
				list.Add(MissionObjectiveKind.FlashAnySeenTime);
			}
			if (flag11)
			{
				list.Add(MissionObjectiveKind.FlashBothSeenTime);
			}
			bool flag12 = this._clothing != null && this._clothing.UsingService && !this._clothing.HasVisibleUpperGarment;
			bool flag13 = flag3 && (flag12 || this._ownedWearables.Exists((WearableItemSO item) => item != null && (string.Equals(item.mcStylePartId, "Outfit.Top", StringComparison.Ordinal) || (fullUndress && item.physicallyCoversTop))));
			this._canGoBraless = flag13;
			if (flag13)
			{
				list.Add(MissionObjectiveKind.NpcPhotosTopless);
			}
			if (fullUndress)
			{
				list.Add(MissionObjectiveKind.NpcPhotosNaked);
			}
			if (flag13)
			{
				list.Add(MissionObjectiveKind.BralessSeenTime);
			}
			if (flag13 && flag4)
			{
				list.Add(MissionObjectiveKind.HoldBoobsSeenTime);
			}
			bool flag14 = flag4 && flag && (flag7 || (!flag5 && flag13));
			bool flag15 = flag14 && !flag6 && ((flag && flag8) | fullUndress);
			if (flag14)
			{
				list.Add(MissionObjectiveKind.StimulationTopSeenTime);
				list.Add(MissionObjectiveKind.StimulationAnySeenTime);
			}
			if (flag15)
			{
				list.Add(MissionObjectiveKind.StimulationBottomSeenTime);
			}
			if (flag2 && flag9)
			{
				list.Add(MissionObjectiveKind.TowelFlashSeenTime);
			}
			if (fullUndress && flag9)
			{
				list.Add(MissionObjectiveKind.TowelDropSeen);
			}
			MissionService.SetRankCapabilities(list);
		}

		// Token: 0x0600791A RID: 31002 RVA: 0x00317AC8 File Offset: 0x00315CC8
		private bool Owns(string itemId)
		{
			for (int i = 0; i < this._ownedWearables.Count; i++)
			{
				if (this._ownedWearables[i] != null && string.Equals(this._ownedWearables[i].itemId, itemId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600791B RID: 31003 RVA: 0x00317B1C File Offset: 0x00315D1C
		private bool OwnsAny(string[] itemIds)
		{
			for (int i = 0; i < itemIds.Length; i++)
			{
				if (this.Owns(itemIds[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600791C RID: 31004 RVA: 0x00317B48 File Offset: 0x00315D48
		private void SampleRankObjectives()
		{
			if (!MissionService.AreRunMissionsEnabled)
			{
				this._lastRankSampleTime = -1f;
				return;
			}
			float time = Time.time;
			if (time < this._nextRankSample)
			{
				return;
			}
			this._nextRankSample = time + 0.1f;
			float num = ((this._lastRankSampleTime < 0f) ? 0f : Mathf.Min(time - this._lastRankSampleTime, 0.25f));
			this._lastRankSampleTime = time;
			bool flag = MissionRuntimeHost.AnyRankTimerActive();
			bool flag2 = MissionService.HasActiveRunMission(MissionObjectiveKind.TowelDropSeen);
			if ((flag || flag2) && Time.timeScale > 0f && MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, false) > 0)
			{
				this._lastWitnessTime = time;
				if (flag && num > 0f)
				{
					this.AccumulateExposureSeconds(num);
				}
			}
			if (num > 0f && Time.timeScale > 0f)
			{
				this.AccumulatePlaceProgress(num, time);
			}
			this.PollPlaceEvents(time);
			if (time >= this._nextRankFlush)
			{
				this._nextRankFlush = time + 0.5f;
				this.FlushRankSeconds();
			}
		}

		// Token: 0x0600791D RID: 31005 RVA: 0x00317C38 File Offset: 0x00315E38
		private static bool AnyRankTimerActive()
		{
			for (int i = 0; i < MissionRuntimeHost.RankTimedKinds.Length; i++)
			{
				if (MissionService.HasActiveRunMission(MissionRuntimeHost.RankTimedKinds[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600791E RID: 31006 RVA: 0x00317C68 File Offset: 0x00315E68
		private void AccumulateExposureSeconds(float dt)
		{
			bool flag = this._clothing != null && this._clothing.IsWornBraOpen;
			bool flag2 = this._lingerie != null && this._lingerie.isActiveAndEnabled && this._lingerie.IsPantiesOpen && this._clothing != null && this._clothing.IsWearingVisiblePreset("Outfit.Underwear", "expose_panties");
			bool flag3 = this._towelFlash != null && this._towelFlash.isActiveAndEnabled && this._towelFlash.IsOpen;
			bool flag4;
			bool flag5;
			bool flag6;
			this.GarmentFlashZones(out flag4, out flag5, out flag6);
			bool flag7 = this._hideFlash != null && this._hideFlash.IsExposureActive;
			bool flag8 = flag || flag3 || flag4;
			bool flag9 = flag2 || flag3 || flag5;
			CoverPoseController instance = CoverPoseController.Instance;
			bool flag10 = instance != null && instance.isActiveAndEnabled && instance.IsCovering;
			string text = (flag10 ? instance.GetCurrentEmotionPoseLabel() : null);
			bool flag11 = flag10 && string.Equals(text, "Stimulation", StringComparison.OrdinalIgnoreCase);
			bool flag12 = flag11 && instance.CurrentCoverType == CoverType.FullCover;
			bool flag13 = flag10 && string.Equals(text, "Hold Boobs", StringComparison.OrdinalIgnoreCase);
			bool flag14 = this._clothing != null && this._clothing.UsingService && !this._clothing.HasVisibleUpperGarment;
			this.AddSeconds(MissionObjectiveKind.FlashTopSeenTime, flag8, dt);
			this.AddSeconds(MissionObjectiveKind.FlashBottomSeenTime, flag9, dt);
			this.AddSeconds(MissionObjectiveKind.FlashAnySeenTime, flag8 || flag9 || flag6 || flag7, dt);
			this.AddSeconds(MissionObjectiveKind.FlashBothSeenTime, flag8 && flag9, dt);
			this.AddSeconds(MissionObjectiveKind.StimulationTopSeenTime, flag11 && !flag12, dt);
			this.AddSeconds(MissionObjectiveKind.StimulationBottomSeenTime, flag12, dt);
			this.AddSeconds(MissionObjectiveKind.StimulationAnySeenTime, flag11, dt);
			this.AddSeconds(MissionObjectiveKind.BralessSeenTime, flag14, dt);
			this.AddSeconds(MissionObjectiveKind.HoldBoobsSeenTime, flag13, dt);
			this.AddSeconds(MissionObjectiveKind.TowelFlashSeenTime, flag3, dt);
		}

		// Token: 0x0600791F RID: 31007 RVA: 0x00317E64 File Offset: 0x00316064
		private void GarmentFlashZones(out bool top, out bool bottom, out bool other)
		{
			top = (bottom = (other = false));
			for (int i = 0; i < this._garmentFlashes.Length; i++)
			{
				ArmsFlashController armsFlashController = this._garmentFlashes[i];
				if (!(armsFlashController == null) && armsFlashController.isActiveAndEnabled && armsFlashController.IsOpen && armsFlashController.Config != null && armsFlashController.Config.gateAsFlash)
				{
					string flashLayerName = armsFlashController.Config.flashLayerName;
					if (!(flashLayerName == "BikiniBraFlash") && !(flashLayerName == "SwimsuitFlash"))
					{
						if (!(flashLayerName == "BikiniPantiesFlash") && !(flashLayerName == "UniversitySkirtFlash"))
						{
							if (!(flashLayerName == "RainCoatFlash"))
							{
								other = true;
							}
							else
							{
								top = true;
								bottom = true;
							}
						}
						else
						{
							bottom = true;
						}
					}
					else
					{
						top = true;
					}
				}
			}
		}

		// Token: 0x06007920 RID: 31008 RVA: 0x00317F38 File Offset: 0x00316138
		private void AddSeconds(MissionObjectiveKind kind, bool active, float dt)
		{
			if (!active || !MissionService.HasActiveRunMission(kind))
			{
				return;
			}
			float num;
			this._rankSeconds.TryGetValue(kind, out num);
			this._rankSeconds[kind] = num + dt;
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x00317F70 File Offset: 0x00316170
		private void FlushRankSeconds()
		{
			if (this._rankSeconds.Count == 0)
			{
				return;
			}
			List<KeyValuePair<MissionObjectiveKind, float>> list = new List<KeyValuePair<MissionObjectiveKind, float>>(this._rankSeconds);
			this._rankSeconds.Clear();
			if (!MissionService.IsRunActive)
			{
				return;
			}
			foreach (KeyValuePair<MissionObjectiveKind, float> keyValuePair in list)
			{
				if (keyValuePair.Value > 0f)
				{
					MissionService.Report(keyValuePair.Key, keyValuePair.Value, null, null, new MissionScope?(MissionScope.Run));
				}
			}
		}

		// Token: 0x06007922 RID: 31010 RVA: 0x00318010 File Offset: 0x00316210
		private void HandleTowelDropped()
		{
			if (!MissionService.HasActiveRunMission(MissionObjectiveKind.TowelDropSeen))
			{
				return;
			}
			if (Time.time - this._lastWitnessTime <= 2.5f || MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, false) > 0)
			{
				MissionService.Report(MissionObjectiveKind.TowelDropSeen, 1f, null, null, new MissionScope?(MissionScope.Run));
			}
		}

		// Token: 0x06007926 RID: 31014 RVA: 0x0031829C File Offset: 0x0031649C
		[CompilerGenerated]
		internal static int <AccumulatePlaceProgress>g__Witnesses|217_0(ref MissionRuntimeHost.<>c__DisplayClass217_0 A_0)
		{
			if (A_0.witnesses < 0)
			{
				return A_0.witnesses = MissionRuntimeHost.CountLiveWitnesses(SensoryManager.Instance, false);
			}
			return A_0.witnesses;
		}

		// Token: 0x04007BF9 RID: 31737
		private const float BindingRetryInterval = 0.5f;

		// Token: 0x04007BFA RID: 31738
		private const float LandmarkRefreshDebounceSeconds = 0.15f;

		// Token: 0x04007BFB RID: 31739
		private const int LandmarkTransformsPerFrame = 2048;

		// Token: 0x04007BFC RID: 31740
		private const double LandmarkFrameBudgetSeconds = 0.002;

		// Token: 0x04007BFD RID: 31741
		private static readonly ProfilerMarker RefreshLandmarksMarker = new ProfilerMarker("MissionRuntimeHost.RefreshLandmarks");

		// Token: 0x04007BFE RID: 31742
		private static MissionRuntimeHost _instance;

		// Token: 0x04007BFF RID: 31743
		private MatchStatsService _stats;

		// Token: 0x04007C00 RID: 31744
		private PlayerPoiEmoteContext _poiContext;

		// Token: 0x04007C01 RID: 31745
		private GameplayEmoteController _emoteController;

		// Token: 0x04007C02 RID: 31746
		private OrgasmMeterController _orgasmMeter;

		// Token: 0x04007C03 RID: 31747
		private WearableUseController _wearController;

		// Token: 0x04007C04 RID: 31748
		private vBasicCoverController _coverController;

		// Token: 0x04007C05 RID: 31749
		private readonly Dictionary<string, string> _currentlyWorn = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007C06 RID: 31750
		private readonly Dictionary<string, MissionRuntimeHost.LandmarkRuntime> _landmarks = new Dictionary<string, MissionRuntimeHost.LandmarkRuntime>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007C07 RID: 31751
		private readonly List<VehicleAI> _trafficLandmarks = new List<VehicleAI>();

		// Token: 0x04007C08 RID: 31752
		private readonly Dictionary<string, float> _emoteStarts = new Dictionary<string, float>();

		// Token: 0x04007C09 RID: 31753
		private float _lastVehicleDistance;

		// Token: 0x04007C0A RID: 31754
		private float _nextPoll;

		// Token: 0x04007C0B RID: 31755
		private float _nextCatalogRefresh;

		// Token: 0x04007C0C RID: 31756
		private float _nextBindingRefresh;

		// Token: 0x04007C0D RID: 31757
		private bool _landmarksDirty = true;

		// Token: 0x04007C0E RID: 31758
		private float _landmarkRefreshNotBefore;

		// Token: 0x04007C0F RID: 31759
		private Coroutine _landmarkRefreshRoutine;

		// Token: 0x04007C10 RID: 31760
		private int _landmarkRefreshGeneration;

		// Token: 0x04007C11 RID: 31761
		private bool _refreshRunMissionsAfterLandmarks;

		// Token: 0x04007C12 RID: 31762
		private EmoteDefinitionSO _activeEmote;

		// Token: 0x04007C13 RID: 31763
		private Vector3 _lastEmotePosition;

		// Token: 0x04007C14 RID: 31764
		private float _movingEmoteDistance;

		// Token: 0x04007C15 RID: 31765
		private float _reportedMovingEmoteDistance;

		// Token: 0x04007C16 RID: 31766
		private float _coverEnteredAt = -1f;

		// Token: 0x04007C17 RID: 31767
		private ChunkManager _chunks;

		// Token: 0x04007C18 RID: 31768
		private bool _performanceReported;

		// Token: 0x04007C19 RID: 31769
		private bool _witnessReported;

		// Token: 0x04007C1A RID: 31770
		private readonly HashSet<string> _performedNear = new HashSet<string>();

		// Token: 0x04007C1B RID: 31771
		private readonly HashSet<string> _performedWearing = new HashSet<string>();

		// Token: 0x04007C1C RID: 31772
		private float _nextEmoteSample;

		// Token: 0x04007C1D RID: 31773
		private ExhibitionEventDirector _encounterDirector;

		// Token: 0x04007C1E RID: 31774
		private readonly HashSet<EmoteDefinitionSO> _stanceEmotes = new HashSet<EmoteDefinitionSO>();

		// Token: 0x04007C1F RID: 31775
		private RapidActionsHUDController _stanceSource;

		// Token: 0x04007C20 RID: 31776
		private readonly HashSet<EmoteDefinitionSO> _contextualEmotes = new HashSet<EmoteDefinitionSO>();

		// Token: 0x04007C21 RID: 31777
		private readonly HashSet<EmoteDefinitionSO> _scanContextualEmotes = new HashSet<EmoteDefinitionSO>();

		// Token: 0x04007C22 RID: 31778
		private readonly Dictionary<int, float> _photoCredits = new Dictionary<int, float>();

		// Token: 0x04007C23 RID: 31779
		private int _pendingPhotoCredits;

		// Token: 0x04007C24 RID: 31780
		private readonly Dictionary<int, float> _npcLastSeen = new Dictionary<int, float>();

		// Token: 0x04007C25 RID: 31781
		private readonly Dictionary<int, float> _npcLookStart = new Dictionary<int, float>();

		// Token: 0x04007C26 RID: 31782
		private readonly Dictionary<int, float> _npcLastSighting = new Dictionary<int, float>();

		// Token: 0x04007C27 RID: 31783
		private const float PhotoCreditCooldownSeconds = 4f;

		// Token: 0x04007C28 RID: 31784
		private const float VideoCreditCooldownSeconds = 6f;

		// Token: 0x04007C29 RID: 31785
		private const float SightingGapSeconds = 2f;

		// Token: 0x04007C2A RID: 31786
		private const float SightingCooldownSeconds = 10f;

		// Token: 0x04007C2B RID: 31787
		private const string TreadmillEmoteId = "mall_gym_treadmill_run";

		// Token: 0x04007C2C RID: 31788
		private const string UniversitySkirtFlashLayer = "UniversitySkirtFlash";

		// Token: 0x04007C2D RID: 31789
		private const float RecentSightSeconds = 0.3f;

		// Token: 0x04007C2E RID: 31790
		private const float GrateWindGraceSeconds = 0.2f;

		// Token: 0x04007C2F RID: 31791
		private const float MaxSwimStepMeters = 2f;

		// Token: 0x04007C30 RID: 31792
		private const float GroupShotWindowSeconds = 5f;

		// Token: 0x04007C31 RID: 31793
		private const int GroupShotPhotographers = 5;

		// Token: 0x04007C32 RID: 31794
		private const int GhostScareTarget = 5;

		// Token: 0x04007C33 RID: 31795
		private const float GhostProtocolMeters = 1000f;

		// Token: 0x04007C34 RID: 31796
		private const float PoliceSightWindowSeconds = 5f;

		// Token: 0x04007C35 RID: 31797
		private const float UnicycleReportMeters = 10f;

		// Token: 0x04007C36 RID: 31798
		private const float SubwayRideTimeoutSeconds = 240f;

		// Token: 0x04007C37 RID: 31799
		private const float PlaceRescanSeconds = 10f;

		// Token: 0x04007C38 RID: 31800
		private readonly HashSet<string> _scanStandPoses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007C39 RID: 31801
		private readonly HashSet<string> _standPoses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007C3A RID: 31802
		private readonly List<VegetableTheftSource> _scanVegetables = new List<VegetableTheftSource>();

		// Token: 0x04007C3B RID: 31803
		private readonly List<VegetableTheftSource> _vegetables = new List<VegetableTheftSource>();

		// Token: 0x04007C3C RID: 31804
		private readonly HashSet<VegetableTheftSource> _lootedVegetables = new HashSet<VegetableTheftSource>();

		// Token: 0x04007C3D RID: 31805
		private StadiumScreenController _scanScreen;

		// Token: 0x04007C3E RID: 31806
		private StadiumScreenController _screen;

		// Token: 0x04007C3F RID: 31807
		private StadiumParachuteController _scanParachute;

		// Token: 0x04007C40 RID: 31808
		private StadiumParachuteController _parachute;

		// Token: 0x04007C41 RID: 31809
		private bool _scanLedWall;

		// Token: 0x04007C42 RID: 31810
		private bool _ledWall;

		// Token: 0x04007C43 RID: 31811
		private bool _scanTintPuddle;

		// Token: 0x04007C44 RID: 31812
		private bool _tintPuddle;

		// Token: 0x04007C45 RID: 31813
		private bool _scanShowers;

		// Token: 0x04007C46 RID: 31814
		private bool _showers;

		// Token: 0x04007C47 RID: 31815
		private bool _scanGrates;

		// Token: 0x04007C48 RID: 31816
		private bool _grates;

		// Token: 0x04007C49 RID: 31817
		private bool _scanSubway;

		// Token: 0x04007C4A RID: 31818
		private bool _subway;

		// Token: 0x04007C4B RID: 31819
		private bool _scanTreadmill;

		// Token: 0x04007C4C RID: 31820
		private bool _treadmill;

		// Token: 0x04007C4D RID: 31821
		private float _nextPlaceRescan;

		// Token: 0x04007C4E RID: 31822
		private string _placeRescanContext;

		// Token: 0x04007C4F RID: 31823
		private int _placeRescans;

		// Token: 0x04007C50 RID: 31824
		private const int MaxPlaceRescans = 3;

		// Token: 0x04007C51 RID: 31825
		private TintBodyPaintRuntimeController _tint;

		// Token: 0x04007C52 RID: 31826
		private ShowerBubbleSpawner _bubbles;

		// Token: 0x04007C53 RID: 31827
		private vSwimming _swimming;

		// Token: 0x04007C54 RID: 31828
		private bool _canGoBraless;

		// Token: 0x04007C55 RID: 31829
		private float _lastPlayerSeenTime = -100f;

		// Token: 0x04007C56 RID: 31830
		private readonly HashSet<int> _poseAudience = new HashSet<int>();

		// Token: 0x04007C57 RID: 31831
		private int _poseAudienceReported;

		// Token: 0x04007C58 RID: 31832
		private int _bestSeatedAudience;

		// Token: 0x04007C59 RID: 31833
		private string _poseCollectionMissionId;

		// Token: 0x04007C5A RID: 31834
		private readonly HashSet<string> _poseCollection = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007C5B RID: 31835
		private Vector3 _lastSwimPosition;

		// Token: 0x04007C5C RID: 31836
		private bool _hasSwimPosition;

		// Token: 0x04007C5D RID: 31837
		private bool _subwayRidePending;

		// Token: 0x04007C5E RID: 31838
		private bool _subwayDoorsClosedSinceDeparture;

		// Token: 0x04007C5F RID: 31839
		private float _subwayRideStartedAt;

		// Token: 0x04007C60 RID: 31840
		private SubwayRideDirector _subwayDirector;

		// Token: 0x04007C61 RID: 31841
		private readonly List<KeyValuePair<int, float>> _groupShotHits = new List<KeyValuePair<int, float>>();

		// Token: 0x04007C62 RID: 31842
		private readonly HashSet<int> _ghostScared = new HashSet<int>();

		// Token: 0x04007C63 RID: 31843
		private readonly HashSet<Object> _policeChasers = new HashSet<Object>();

		// Token: 0x04007C64 RID: 31844
		private float _lastPoliceSightTime = -100f;

		// Token: 0x04007C65 RID: 31845
		private float _nextSecretSample;

		// Token: 0x04007C66 RID: 31846
		private Transform _unicycle;

		// Token: 0x04007C67 RID: 31847
		private Vector3 _lastUnicyclePosition;

		// Token: 0x04007C68 RID: 31848
		private float _lastUnicycleTime;

		// Token: 0x04007C69 RID: 31849
		private float _pendingUnicycleMeters;

		// Token: 0x04007C6A RID: 31850
		private const string TowelItemId = "cloth_towel";

		// Token: 0x04007C6B RID: 31851
		private const string TowelSlipActionId = "towel_slip";

		// Token: 0x04007C6C RID: 31852
		private const string BikiniSlipActionId = "bikini_slip";

		// Token: 0x04007C6D RID: 31853
		private const string SwimsuitSlipActionId = "swimsuit_slip";

		// Token: 0x04007C6E RID: 31854
		private const string TowelSlipPiece = "Bath_Towel";

		// Token: 0x04007C6F RID: 31855
		private const string ExposeBraId = "cloth_expose_bra";

		// Token: 0x04007C70 RID: 31856
		private const string ExposePantiesId = "cloth_expose_panties";

		// Token: 0x04007C71 RID: 31857
		private const string StimulationPoseLabel = "Stimulation";

		// Token: 0x04007C72 RID: 31858
		private const string HoldBoobsPoseLabel = "Hold Boobs";

		// Token: 0x04007C73 RID: 31859
		private const float RankSampleInterval = 0.1f;

		// Token: 0x04007C74 RID: 31860
		private const float RankFlushInterval = 0.5f;

		// Token: 0x04007C75 RID: 31861
		private const float DropWitnessWindowSeconds = 2.5f;

		// Token: 0x04007C76 RID: 31862
		private static readonly string[] TopFlashGarments = new string[] { "cloth_towel", "cloth_bikini_bra", "cloth_swimsuit", "cloth_raincoat" };

		// Token: 0x04007C77 RID: 31863
		private static readonly string[] BottomFlashGarments = new string[] { "cloth_towel", "cloth_bikini_panties", "cloth_university_uniform_skirt", "cloth_university_uniform_full", "cloth_raincoat" };

		// Token: 0x04007C78 RID: 31864
		private static readonly MissionObjectiveKind[] RankTimedKinds = new MissionObjectiveKind[]
		{
			MissionObjectiveKind.FlashTopSeenTime,
			MissionObjectiveKind.FlashBottomSeenTime,
			MissionObjectiveKind.FlashAnySeenTime,
			MissionObjectiveKind.FlashBothSeenTime,
			MissionObjectiveKind.StimulationTopSeenTime,
			MissionObjectiveKind.StimulationBottomSeenTime,
			MissionObjectiveKind.StimulationAnySeenTime,
			MissionObjectiveKind.BralessSeenTime,
			MissionObjectiveKind.HoldBoobsSeenTime,
			MissionObjectiveKind.TowelFlashSeenTime
		};

		// Token: 0x04007C79 RID: 31865
		private readonly List<WearableItemSO> _ownedWearables = new List<WearableItemSO>();

		// Token: 0x04007C7A RID: 31866
		private readonly Dictionary<MissionObjectiveKind, float> _rankSeconds = new Dictionary<MissionObjectiveKind, float>();

		// Token: 0x04007C7B RID: 31867
		private Transform _exposureOwner;

		// Token: 0x04007C7C RID: 31868
		private ClothingToggleController _clothing;

		// Token: 0x04007C7D RID: 31869
		private ExposeLingerieController _lingerie;

		// Token: 0x04007C7E RID: 31870
		private ClothFullFlashController _towelFlash;

		// Token: 0x04007C7F RID: 31871
		private ClothSlipController _towelSlip;

		// Token: 0x04007C80 RID: 31872
		private ArmsFlashController[] _garmentFlashes = Array.Empty<ArmsFlashController>();

		// Token: 0x04007C81 RID: 31873
		private HideFlashController _hideFlash;

		// Token: 0x04007C82 RID: 31874
		private float _nextRankSample;

		// Token: 0x04007C83 RID: 31875
		private float _lastRankSampleTime = -1f;

		// Token: 0x04007C84 RID: 31876
		private float _nextRankFlush;

		// Token: 0x04007C85 RID: 31877
		private float _lastWitnessTime = -100f;

		// Token: 0x02000FA7 RID: 4007
		private sealed class LandmarkRuntime
		{
			// Token: 0x04007C86 RID: 31878
			public string Id;

			// Token: 0x04007C87 RID: 31879
			public string Name;

			// Token: 0x04007C88 RID: 31880
			public float Range;

			// Token: 0x04007C89 RID: 31881
			public readonly List<Transform> Transforms = new List<Transform>();

			// Token: 0x04007C8A RID: 31882
			public readonly Dictionary<Transform, Collider[]> VehicleColliders = new Dictionary<Transform, Collider[]>();

			// Token: 0x04007C8B RID: 31883
			public readonly Dictionary<Transform, Renderer[]> PropRenderers = new Dictionary<Transform, Renderer[]>();
		}
	}
}
