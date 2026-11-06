using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.DebugTools;
using Features.Detection;
using Features.MainMenu;
using Features.PhoneCompanion;
using Features.Rank;
using Features.TimeFreeze;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Futanari;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E1F RID: 3615
	public class GameplayEmoteController : MonoBehaviour
	{
		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x002B0E8A File Offset: 0x002AF08A
		// (set) Token: 0x06006B93 RID: 27539 RVA: 0x002B0E92 File Offset: 0x002AF092
		public GameplayEmoteController.EmoteState CurrentState { get; private set; }

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x002B0E9B File Offset: 0x002AF09B
		public bool IsWheelOpen
		{
			get
			{
				return this.CurrentState == GameplayEmoteController.EmoteState.WheelOpen;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x002B0EA6 File Offset: 0x002AF0A6
		public bool IsPlaying
		{
			get
			{
				return this.CurrentState == GameplayEmoteController.EmoteState.Playing;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06006B96 RID: 27542 RVA: 0x002B0EB1 File Offset: 0x002AF0B1
		public bool IsEnding
		{
			get
			{
				return this.IsPlaying && this._endingRequested;
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06006B97 RID: 27543 RVA: 0x002B0EC3 File Offset: 0x002AF0C3
		public EmoteDefinitionSO PlayingEmote
		{
			get
			{
				return this._playingEmote;
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06006B98 RID: 27544 RVA: 0x002B0ECC File Offset: 0x002AF0CC
		// (remove) Token: 0x06006B99 RID: 27545 RVA: 0x002B0F00 File Offset: 0x002AF100
		public static event Action MasturbationEmoteBlockChanged;

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x002B0F33 File Offset: 0x002AF133
		// (set) Token: 0x06006B9B RID: 27547 RVA: 0x002B0F3A File Offset: 0x002AF13A
		public static int MasturbationBlockVersion { get; private set; }

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x06006B9C RID: 27548 RVA: 0x002B0F42 File Offset: 0x002AF142
		public static bool AreMasturbationEmotesBlocked
		{
			get
			{
				GameplayEmoteController.PruneMasturbationBlockOwners();
				return GameplayEmoteController.MasturbationBlockOwners.Count > 0;
			}
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x002B0F58 File Offset: 0x002AF158
		public static void SetMasturbationEmotesBlocked(Object owner, bool blocked, string reason = null)
		{
			if (owner == null)
			{
				return;
			}
			int num = GameplayEmoteController.IndexOfMasturbationBlockOwner(owner);
			if (blocked)
			{
				if (owner == null)
				{
					return;
				}
				if (num >= 0)
				{
					GameplayEmoteController.MasturbationBlockReasons[num] = reason;
					return;
				}
				GameplayEmoteController.MasturbationBlockOwners.Add(owner);
				GameplayEmoteController.MasturbationBlockReasons.Add(reason);
			}
			else
			{
				if (num < 0)
				{
					return;
				}
				GameplayEmoteController.MasturbationBlockOwners.RemoveAt(num);
				GameplayEmoteController.MasturbationBlockReasons.RemoveAt(num);
			}
			GameplayEmoteController.RaiseMasturbationBlockChanged();
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x002B0FC6 File Offset: 0x002AF1C6
		public static bool IsCoveredByMasturbationBlock(EmoteDefinitionSO emote)
		{
			return emote != null && (emote.IsMasturbationEmote || (emote.FutanariRequirement == FutanariRequirement.FutanariOnly && !emote.UseMagicaClothDuringEmote));
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x002B0FF1 File Offset: 0x002AF1F1
		public static bool IsMasturbationEmoteBlocked(EmoteDefinitionSO emote, out string reason)
		{
			reason = null;
			if (!GameplayEmoteController.IsCoveredByMasturbationBlock(emote) || !GameplayEmoteController.AreMasturbationEmotesBlocked)
			{
				return false;
			}
			reason = GameplayEmoteController.MasturbationBlockReasons[GameplayEmoteController.MasturbationBlockReasons.Count - 1];
			return true;
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x002B1020 File Offset: 0x002AF220
		private static int IndexOfMasturbationBlockOwner(Object owner)
		{
			for (int i = 0; i < GameplayEmoteController.MasturbationBlockOwners.Count; i++)
			{
				if (GameplayEmoteController.MasturbationBlockOwners[i] == owner)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x002B1054 File Offset: 0x002AF254
		private static void PruneMasturbationBlockOwners()
		{
			bool flag = false;
			for (int i = GameplayEmoteController.MasturbationBlockOwners.Count - 1; i >= 0; i--)
			{
				if (!(GameplayEmoteController.MasturbationBlockOwners[i] != null))
				{
					GameplayEmoteController.MasturbationBlockOwners.RemoveAt(i);
					GameplayEmoteController.MasturbationBlockReasons.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				GameplayEmoteController.RaiseMasturbationBlockChanged();
			}
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x002B10AD File Offset: 0x002AF2AD
		private static void RaiseMasturbationBlockChanged()
		{
			GameplayEmoteController.MasturbationBlockVersion++;
			Action masturbationEmoteBlockChanged = GameplayEmoteController.MasturbationEmoteBlockChanged;
			if (masturbationEmoteBlockChanged == null)
			{
				return;
			}
			masturbationEmoteBlockChanged();
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x002B10CA File Offset: 0x002AF2CA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetMasturbationEmoteBlocks()
		{
			GameplayEmoteController.MasturbationBlockOwners.Clear();
			GameplayEmoteController.MasturbationBlockReasons.Clear();
			GameplayEmoteController.MasturbationBlockVersion = 0;
			GameplayEmoteController.MasturbationEmoteBlockChanged = null;
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x002B10EC File Offset: 0x002AF2EC
		private void HandleMasturbationEmoteBlockChanged()
		{
			string text;
			if (this.CurrentState == GameplayEmoteController.EmoteState.Playing && !this._endingRequested && GameplayEmoteController.IsMasturbationEmoteBlocked(this._playingEmote, out text))
			{
				this.EndEmote();
			}
		}

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06006BA5 RID: 27557 RVA: 0x002B1120 File Offset: 0x002AF320
		// (remove) Token: 0x06006BA6 RID: 27558 RVA: 0x002B1158 File Offset: 0x002AF358
		public event Action<EmoteDefinitionSO> OnEmoteStarted;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06006BA7 RID: 27559 RVA: 0x002B1190 File Offset: 0x002AF390
		// (remove) Token: 0x06006BA8 RID: 27560 RVA: 0x002B11C8 File Offset: 0x002AF3C8
		public event Action<EmoteDefinitionSO> OnEmoteEnded;

		// Token: 0x06006BA9 RID: 27561 RVA: 0x002B11FD File Offset: 0x002AF3FD
		public void NotifyExternalMasturbationStart(EmoteDefinitionSO proxyEmote)
		{
			if (proxyEmote != null)
			{
				Action<EmoteDefinitionSO> onEmoteStarted = this.OnEmoteStarted;
				if (onEmoteStarted == null)
				{
					return;
				}
				onEmoteStarted(proxyEmote);
			}
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x002B1219 File Offset: 0x002AF419
		public void NotifyExternalMasturbationEnd(EmoteDefinitionSO proxyEmote)
		{
			if (proxyEmote != null)
			{
				Action<EmoteDefinitionSO> onEmoteEnded = this.OnEmoteEnded;
				if (onEmoteEnded == null)
				{
					return;
				}
				onEmoteEnded(proxyEmote);
			}
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x002B1238 File Offset: 0x002AF438
		private void Awake()
		{
			if (base.GetComponent<HandstandBreastGravity>() == null)
			{
				base.gameObject.AddComponent<HandstandBreastGravity>();
			}
			if (this.ToggleInput != null)
			{
				this.ToggleInput.useInput = false;
			}
			this.OnEmoteStarted += GameplayEmoteController.RegisterCollectionUnlock;
			if (this.ActionAnimator == null)
			{
				this.ActionAnimator = base.GetComponent<PlayerActionAnimator>();
			}
			if (this.PropHandler == null)
			{
				this.PropHandler = base.GetComponent<EmotePropHandler>();
			}
			if (this.EmoteAudioSource == null)
			{
				this.EmoteAudioSource = base.GetComponent<AudioSource>();
			}
			if (this.PoiContext == null)
			{
				this.PoiContext = base.GetComponent<PlayerPoiEmoteContext>();
			}
			if (this.FutanariMode == null)
			{
				this.FutanariMode = base.GetComponentInChildren<FutanariModeController>(true);
			}
			if (this.FutanariErection == null)
			{
				this.FutanariErection = base.GetComponentInChildren<PenisErectionController>(true);
			}
			this._playerInput = base.GetComponent<vThirdPersonInput>();
			if (this.ActionAnimator != null && this.ActionAnimator.GenericAction != null)
			{
				this.ActionAnimator.GenericAction.OnEndAction.AddListener(new UnityAction<vTriggerGenericAction>(this.OnGenericActionEnded));
			}
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x002B1371 File Offset: 0x002AF571
		private void OnEnable()
		{
			this.SubscribeToPoiContext();
			GameplayEmoteController.MasturbationEmoteBlockChanged -= this.HandleMasturbationEmoteBlockChanged;
			GameplayEmoteController.MasturbationEmoteBlockChanged += this.HandleMasturbationEmoteBlockChanged;
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x002B139C File Offset: 0x002AF59C
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			bool isAnySelfieActive = SmartphoneController.IsAnySelfieActive;
			bool isAnyTripodActive = SmartphoneController.IsAnyTripodActive;
			if (isAnySelfieActive && (!isAnyTripodActive || this.CurrentState != GameplayEmoteController.EmoteState.Playing))
			{
				return;
			}
			GameplayEmoteController.EmoteState currentState = this.CurrentState;
			if (currentState == GameplayEmoteController.EmoteState.WheelOpen)
			{
				this.CloseWheel();
				return;
			}
			if (currentState != GameplayEmoteController.EmoteState.Playing)
			{
				return;
			}
			this.HandlePlayingState();
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x002B13E8 File Offset: 0x002AF5E8
		private void OnDestroy()
		{
			this.ReleasePlaybackHolds(true);
			GameplayEmoteController.MasturbationEmoteBlockChanged -= this.HandleMasturbationEmoteBlockChanged;
			this.ReleasePoseBlendShape();
			this.UnsubscribeFromPoiContext();
			this.OnEmoteStarted -= GameplayEmoteController.RegisterCollectionUnlock;
			if (this.CurrentState == GameplayEmoteController.EmoteState.WheelOpen)
			{
				this.RestoreTimeScale();
			}
			this.RestoreWheelCamera();
			this.RestoreRootMotionFromEmote();
			this.ReleaseAutomaticFutanariErectionRequest();
			this.ReleasePlaybackActivityContext();
			if (this.ActionAnimator != null && this.ActionAnimator.GenericAction != null)
			{
				this.ActionAnimator.GenericAction.OnEndAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.OnGenericActionEnded));
			}
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x002B1494 File Offset: 0x002AF694
		private void OnDisable()
		{
			this.UnsubscribeFromPoiContext();
			GameplayEmoteController.MasturbationEmoteBlockChanged -= this.HandleMasturbationEmoteBlockChanged;
			if (this.CurrentState != GameplayEmoteController.EmoteState.Idle)
			{
				this.ForceEndEmoteImmediate();
			}
			this.ReleasePlaybackHolds(true);
			this.ReleasePoseBlendShape();
			this.ReleaseAutomaticFutanariErectionRequest();
			this.ReleasePlaybackActivityContext();
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x002B14D4 File Offset: 0x002AF6D4
		private static void RegisterCollectionUnlock(EmoteDefinitionSO emote)
		{
			CollectionUnlockRegistry.UnlockEmote(emote);
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x002B14DC File Offset: 0x002AF6DC
		private void LateUpdate()
		{
			this.TickPlaybackHold();
			EmoteDefinitionSO emoteDefinitionSO = ((this.CurrentState == GameplayEmoteController.EmoteState.Playing) ? this._playingEmote : null);
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.IsResolutionActionPlaying && instance.PlayerTransform == base.transform)
			{
				emoteDefinitionSO = instance.ActiveResolutionEmote;
			}
			if (!(emoteDefinitionSO != null) || (!(emoteDefinitionSO.EmoteId == "down_back") && !(emoteDefinitionSO.EmoteId == "down_back_pee")))
			{
				this.ReleasePoseBlendShape();
				return;
			}
			if (this._poseBlendShapeManager == null)
			{
				this._poseBlendShapeManager = BlendShapeManager.ResolveForRig(this);
			}
			if (this._poseBlendShapeManager != null)
			{
				this._poseBlendShapeManager.SetOverride("Kupa_V_LR", 50f, "GameplayEmote.DownBack", BlendShapePriority.Pose, false);
			}
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x002B15B2 File Offset: 0x002AF7B2
		private void ReleasePoseBlendShape()
		{
			if (this._poseBlendShapeManager != null)
			{
				this._poseBlendShapeManager.RemoveOverride("Kupa_V_LR", "GameplayEmote.DownBack", true, false, 0f);
			}
			this._poseBlendShapeManager = null;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x002B15E8 File Offset: 0x002AF7E8
		private bool EndEmotePressed()
		{
			if (this.EndEmoteInput == null || !this.EndEmoteInput.useInput)
			{
				return false;
			}
			if (GameplayInputGate.BlocksGameplayInput || TextInputFocus.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return false;
			}
			bool isActive = ControllerInput.IsActive;
			return (isActive ? ControllerGameplayBridge.Read(KeyCode.Space, 1) : this.EndEmoteInput.GetButtonDown()) && InteractionCandidateRegistry.TryConsumeContextInput(isActive ? this._controllerEndInput : this.EndEmoteInput);
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x002B1664 File Offset: 0x002AF864
		private void HandlePlayingState()
		{
			if (this._playingEmote == null)
			{
				return;
			}
			if (this._playingEmote.EndBehavior == EmoteEndBehavior.ManualEnd && !this._suppressEndHud && this.EndEmotePressed())
			{
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				this.RequestEndEmote();
			}
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x002B16B0 File Offset: 0x002AF8B0
		private EmoteWheelThemeSO ResolveWheelTheme()
		{
			PoiEmoteProfileSO poiEmoteProfileSO = ((this.PoiContext != null) ? this.PoiContext.CurrentProfile : null);
			if (poiEmoteProfileSO != null && poiEmoteProfileSO.WheelThemeOverride != null)
			{
				return poiEmoteProfileSO.WheelThemeOverride;
			}
			return this.WheelTheme;
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x002B1700 File Offset: 0x002AF900
		private List<EmoteDefinitionSO> BuildWheelEmotes()
		{
			List<EmoteDefinitionSO> list = ((this.EmoteCollection != null) ? this.EmoteCollection.GetUnlockedEmotes() : new List<EmoteDefinitionSO>());
			List<EmoteDefinitionSO> list2 = new List<EmoteDefinitionSO>(list.Count + 8);
			HashSet<string> hashSet = new HashSet<string>();
			PoiEmoteProfileSO poiEmoteProfileSO = ((this.PoiContext != null) ? this.PoiContext.CurrentProfile : null);
			if (poiEmoteProfileSO != null && poiEmoteProfileSO.PriorityEmotes != null)
			{
				for (int i = 0; i < poiEmoteProfileSO.PriorityEmotes.Count; i++)
				{
					EmoteDefinitionSO emoteDefinitionSO = poiEmoteProfileSO.PriorityEmotes[i];
					if (!(emoteDefinitionSO == null) && CollectionUnlockRegistry.IsEmoteUnlocked(emoteDefinitionSO) && this.IsEmoteAllowedByFutanari(emoteDefinitionSO) && this.IsEmoteAllowedByRank(emoteDefinitionSO, true) && this.IsEmoteAllowedWhileMounted(emoteDefinitionSO) && this.IsEmoteAllowedByPoi(emoteDefinitionSO))
					{
						string text = (string.IsNullOrEmpty(emoteDefinitionSO.EmoteId) ? emoteDefinitionSO.name : emoteDefinitionSO.EmoteId);
						if (hashSet.Add(text))
						{
							list2.Add(emoteDefinitionSO);
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				EmoteDefinitionSO emoteDefinitionSO2 = list[j];
				if (!(emoteDefinitionSO2 == null) && this.IsEmoteAllowedByFutanari(emoteDefinitionSO2) && this.IsEmoteAllowedByRank(emoteDefinitionSO2, true) && this.IsEmoteAllowedWhileMounted(emoteDefinitionSO2) && this.IsEmoteAllowedByPoi(emoteDefinitionSO2))
				{
					string text2 = (string.IsNullOrEmpty(emoteDefinitionSO2.EmoteId) ? emoteDefinitionSO2.name : emoteDefinitionSO2.EmoteId);
					if (hashSet.Add(text2))
					{
						list2.Add(emoteDefinitionSO2);
					}
				}
			}
			return list2;
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x002B1898 File Offset: 0x002AFA98
		private bool IsEmoteBlockedByClothing(EmoteDefinitionSO emote)
		{
			return emote != null && emote.IsBlockedByClothing;
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x002B18AB File Offset: 0x002AFAAB
		private string ClothingBlockedMessage()
		{
			return UncoveredBottomRequirement.BlockedMessage;
		}

		// Token: 0x06006BB9 RID: 27577 RVA: 0x002B18B4 File Offset: 0x002AFAB4
		private void BreakCoverPoseForEmote()
		{
			if (this._coverPoseController == null)
			{
				this._coverPoseController = base.GetComponentInParent<CoverPoseController>() ?? Object.FindObjectOfType<CoverPoseController>();
			}
			if (this._coverPoseController != null && this._coverPoseController.IsCovering)
			{
				this._coverPoseController.DeactivateCoverPose();
			}
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x002B190C File Offset: 0x002AFB0C
		public bool IsEmoteAllowedByRank(EmoteDefinitionSO emote, bool includePageRestrictions = true)
		{
			if (emote == null)
			{
				return true;
			}
			if (includePageRestrictions && !RankEmotePagePolicy.IsEmoteAvailable(RankProgressionService.CurrentRank, this.EmoteCollection, emote, this.FutanariMode != null && this.FutanariMode.FutanariMode))
			{
				return false;
			}
			if (!emote.IsMasturbationEmote)
			{
				return true;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			return rankRuntimeController == null || rankRuntimeController.AllowsMasturbationEmotes;
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x002B1979 File Offset: 0x002AFB79
		public bool IsEmoteAllowedWhileMounted(EmoteDefinitionSO emote)
		{
			return !(emote == null) && (!CarRoofCarrier.IsPlayerMounted || emote.MovementMode != EmoteMovementMode.Moving);
		}

		// Token: 0x06006BBC RID: 27580 RVA: 0x002B199B File Offset: 0x002AFB9B
		private bool IsEmoteAllowedByPoi(EmoteDefinitionSO emote)
		{
			return !(emote == null) && (string.IsNullOrWhiteSpace(emote.RequiredPoiId) || (this.PoiContext != null && this.PoiContext.IsInPoi(emote.RequiredPoiId)));
		}

		// Token: 0x06006BBD RID: 27581 RVA: 0x002B19D8 File Offset: 0x002AFBD8
		private void SubscribeToPoiContext()
		{
			if (this._subscribedPoiContext == this.PoiContext)
			{
				return;
			}
			this.UnsubscribeFromPoiContext();
			if (this.PoiContext == null)
			{
				return;
			}
			this.PoiContext.OnProfileChanged += this.HandlePoiProfileChanged;
			this._subscribedPoiContext = this.PoiContext;
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x002B1A31 File Offset: 0x002AFC31
		private void UnsubscribeFromPoiContext()
		{
			if (this._subscribedPoiContext == null)
			{
				return;
			}
			this._subscribedPoiContext.OnProfileChanged -= this.HandlePoiProfileChanged;
			this._subscribedPoiContext = null;
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x002B1A60 File Offset: 0x002AFC60
		private void HandlePoiProfileChanged(PoiEmoteProfileSO previousProfile, PoiEmoteProfileSO currentProfile)
		{
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing || this._playingEmote == null || this.IsEmoteAllowedByPoi(this._playingEmote))
			{
				return;
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[GameplayEmote] Ending '",
					this._playingEmote.name,
					"': required POI '",
					this._playingEmote.RequiredPoiId,
					"' was left."
				}));
			}
			this.ForceEndEmoteImmediate();
		}

		// Token: 0x06006BC0 RID: 27584 RVA: 0x002B1AE8 File Offset: 0x002AFCE8
		private void ApplyFutanariClothForEmote(EmoteDefinitionSO emote, bool starting)
		{
			if (this.FutanariMode == null)
			{
				return;
			}
			if (!starting)
			{
				this.FutanariMode.SetClothSuspended(this, false);
				return;
			}
			if (emote == null || emote.UseMagicaClothDuringEmote)
			{
				return;
			}
			this.FutanariMode.SetClothSuspended(this, true);
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x002B1B34 File Offset: 0x002AFD34
		public static bool ShouldRequestAutomaticFutanariErection(EmoteDefinitionSO emote)
		{
			return emote != null && !emote.IsMasturbationEmote && emote.UseMagicaClothDuringEmote;
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x002B1B50 File Offset: 0x002AFD50
		private void SetAutomaticFutanariErectionForPlayback(EmoteDefinitionSO emote, bool requestedByRapidEmoteSlot)
		{
			bool flag = requestedByRapidEmoteSlot && this.FutanariMode != null && this.FutanariMode.FutanariMode && GameplayEmoteController.ShouldRequestAutomaticFutanariErection(emote);
			this._playingEmoteRequestsFutanariErection = flag;
			if (this.FutanariErection != null)
			{
				this.FutanariErection.SetRapidEmoteErectionRequested(flag);
			}
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x002B1BA6 File Offset: 0x002AFDA6
		private void ReleaseAutomaticFutanariErectionRequest()
		{
			if (!this._playingEmoteRequestsFutanariErection)
			{
				return;
			}
			this._playingEmoteRequestsFutanariErection = false;
			if (this.FutanariErection != null)
			{
				this.FutanariErection.SetRapidEmoteErectionRequested(false);
			}
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x002B1BD4 File Offset: 0x002AFDD4
		public bool IsEmoteAllowedByFutanari(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return false;
			}
			switch (emote.FutanariRequirement)
			{
			case FutanariRequirement.Always:
				return true;
			case FutanariRequirement.FutanariOnly:
				return this.FutanariMode != null && this.FutanariMode.FutanariMode;
			case FutanariRequirement.NoFutanari:
				return this.FutanariMode == null || !this.FutanariMode.FutanariMode;
			default:
				return true;
			}
		}

		// Token: 0x06006BC5 RID: 27589 RVA: 0x002B1C48 File Offset: 0x002AFE48
		private Dictionary<string, EmoteCardUI> BuildCardOverridesByEmoteId()
		{
			PoiEmoteProfileSO poiEmoteProfileSO = ((this.PoiContext != null) ? this.PoiContext.CurrentProfile : null);
			if (poiEmoteProfileSO == null || poiEmoteProfileSO.PriorityEmotes == null || poiEmoteProfileSO.PriorityEmotes.Count == 0)
			{
				return null;
			}
			EmoteCardUI emoteCardUI = null;
			if (poiEmoteProfileSO.WheelThemeOverride != null && poiEmoteProfileSO.WheelThemeOverride.CardPrefab != null)
			{
				emoteCardUI = poiEmoteProfileSO.WheelThemeOverride.CardPrefab;
			}
			else if (poiEmoteProfileSO.PriorityEmoteCardPrefab != null)
			{
				emoteCardUI = poiEmoteProfileSO.PriorityEmoteCardPrefab;
			}
			if (emoteCardUI == null)
			{
				return null;
			}
			Dictionary<string, EmoteCardUI> dictionary = new Dictionary<string, EmoteCardUI>();
			for (int i = 0; i < poiEmoteProfileSO.PriorityEmotes.Count; i++)
			{
				EmoteDefinitionSO emoteDefinitionSO = poiEmoteProfileSO.PriorityEmotes[i];
				if (!(emoteDefinitionSO == null) && CollectionUnlockRegistry.IsEmoteUnlocked(emoteDefinitionSO))
				{
					string text = (string.IsNullOrEmpty(emoteDefinitionSO.EmoteId) ? emoteDefinitionSO.name : emoteDefinitionSO.EmoteId);
					if (!string.IsNullOrEmpty(text) && !dictionary.ContainsKey(text))
					{
						dictionary.Add(text, emoteCardUI);
					}
				}
			}
			if (dictionary.Count <= 0)
			{
				return null;
			}
			return dictionary;
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x002B1D68 File Offset: 0x002AFF68
		private EmoteCardUI ResolveDefaultCardFallback(EmoteWheelThemeSO runtimeTheme)
		{
			if (this.WheelTheme == null || this.WheelTheme.CardPrefab == null)
			{
				return null;
			}
			if (runtimeTheme == null)
			{
				return null;
			}
			if (!(runtimeTheme != this.WheelTheme))
			{
				return null;
			}
			return this.WheelTheme.CardPrefab;
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x002B1DBE File Offset: 0x002AFFBE
		private void RestoreWheelCamera()
		{
			if (!this._wheelCameraCaptured)
			{
				return;
			}
			this._wheelCameraCaptured = false;
			vThirdPersonInput.ForceCameraLock = this._savedWheelForceCameraLock;
			if (this._playerInput != null)
			{
				this._playerInput.SetLockCameraInput(this._savedWheelCameraLock);
			}
		}

		// Token: 0x06006BC8 RID: 27592 RVA: 0x002B1DFC File Offset: 0x002AFFFC
		private void CloseWheel()
		{
			if (this.EmoteWheel != null)
			{
				this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteConfirmed;
				this.EmoteWheel.Close();
			}
			this.RestoreTimeScale();
			this.RestoreWheelCamera();
			this.CurrentState = GameplayEmoteController.EmoteState.Idle;
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] Wheel closed (cancelled)");
			}
		}

		// Token: 0x06006BC9 RID: 27593 RVA: 0x002B1E60 File Offset: 0x002B0060
		private void OnEmoteConfirmed(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return;
			}
			this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteConfirmed;
			this.EmoteWheel.Close();
			this.RestoreTimeScale();
			this.RestoreWheelCamera();
			if (!this.IsEmoteAllowedByRank(emote, true) || !this.IsEmoteAllowedByFutanari(emote))
			{
				BlockedActionFeedback.Show(null);
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			if (!this.IsEmoteAllowedByPoi(emote))
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Emote rejected outside required POI '" + emote.RequiredPoiId + "': " + emote.name);
				}
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			string text;
			if (GameplayEmoteController.IsMasturbationEmoteBlocked(emote, out text))
			{
				BlockedActionFeedback.Show(text);
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			if (this.IsEmoteBlockedByClothing(emote))
			{
				BlockedActionFeedback.Show(this.ClothingBlockedMessage());
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			if (!this.IsEmoteAllowedWhileMounted(emote))
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Emote rejected on car roof (moving emote): " + emote.name);
				}
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			if (!this.TryAcquirePlaybackActivityContext(false))
			{
				BlockedActionFeedback.Show(null);
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
				return;
			}
			this.BreakCoverPoseForEmote();
			this._suppressEndHud = false;
			this._playingEmote = emote;
			this.CurrentState = GameplayEmoteController.EmoteState.Playing;
			int num = this.BeginPlaybackSession();
			this.SetAutomaticFutanariErectionForPlayback(emote, false);
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[GameplayEmote] Emote confirmed: {0} (Movement={1}, End={2})", emote.DisplayName, emote.MovementMode, emote.EndBehavior));
			}
			Action<EmoteDefinitionSO> onEmoteStarted = this.OnEmoteStarted;
			if (onEmoteStarted != null)
			{
				onEmoteStarted(emote);
			}
			if (this.IsCurrentPlayback(num) && !this._endingRequested)
			{
				this._emotePlaybackCoroutine = base.StartCoroutine(this.EmotePlaybackRoutine(emote, num));
			}
		}

		// Token: 0x06006BCA RID: 27594 RVA: 0x002B200C File Offset: 0x002B020C
		private IEnumerator EmotePlaybackRoutine(EmoteDefinitionSO emote, int playbackSessionId)
		{
			this.ApplyFutanariClothForEmote(emote, true);
			if (this.PropHandler != null)
			{
				this.PropHandler.SetupProps(emote);
			}
			if (emote.MovementMode == EmoteMovementMode.Static)
			{
				if (this._playerInput != null)
				{
					this._playerInput.SetLockAllInput(true);
					this._playerInput.cc.input = Vector2.zero;
					this._playerInput.cc.isSprinting = false;
					this._playerInput.cc.animator.SetFloat("InputHorizontal", 0f);
					this._playerInput.cc.animator.SetFloat("InputVertical", 0f);
					this._playerInput.cc.animator.SetFloat("InputMagnitude", 0f);
				}
				if (emote.MovingUseRootMotion)
				{
					this.EnableRootMotionForEmote();
				}
			}
			else
			{
				if (this._playerInput != null)
				{
					this._playerInput.SetLockAllInput(false);
				}
				this.EnableLocomotionForEmote(emote);
			}
			this.PlayEmoteAudio(emote);
			float num = 0f;
			if (this.ActionAnimator != null)
			{
				num = this.ActionAnimator.TriggerAction(new PlayerActionEntry
				{
					ActionId = emote.EmoteId,
					DisplayName = emote.DisplayName,
					Duration = emote.Duration,
					TriggerPrefab = emote.ResolveTriggerPrefab()
				});
			}
			if (this.PropHandler != null)
			{
				this.PropHandler.StartAutoTimers();
			}
			if (emote.EndBehavior == EmoteEndBehavior.ManualEnd)
			{
				if (!this._suppressEndHud)
				{
					this.ShowEndEmoteHUD();
				}
				while (this.IsCurrentPlayback(playbackSessionId))
				{
					yield return null;
				}
			}
			else
			{
				float num2 = ((num > 0f) ? num : emote.Duration);
				yield return new WaitForSeconds(num2);
				while (this.IsCurrentPlayback(playbackSessionId) && (this.IsPlaybackHeld || this._holdOwedSeconds > 0f))
				{
					if (!this.IsPlaybackHeld)
					{
						this._holdOwedSeconds -= Time.deltaTime;
					}
					yield return null;
				}
				if (this.IsCurrentPlayback(playbackSessionId))
				{
					this.EndEmote();
				}
			}
			yield break;
		}

		// Token: 0x06006BCB RID: 27595 RVA: 0x002B2029 File Offset: 0x002B0229
		public bool TryPlayRapidEmote(EmoteDefinitionSO emote, bool suppressEndHud = false, bool bypassActivityGate = false, bool requestFutanariErection = false)
		{
			return this.TryPlayEmote(emote, suppressEndHud, bypassActivityGate, requestFutanariErection, false);
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06006BCC RID: 27596 RVA: 0x002B2037 File Offset: 0x002B0237
		public static bool CanStartContextualEmote
		{
			get
			{
				return !TextInputFocus.BlocksGameplayInput && PlayerActivityGate.AllowsIgnoringLegacyReason(PlayerActivity.Emote | PlayerActivity.Interact, "Rank");
			}
		}

		// Token: 0x06006BCD RID: 27597 RVA: 0x002B204E File Offset: 0x002B024E
		public bool TryPlayContextualEmote(EmoteDefinitionSO emote, bool suppressEndHud = false)
		{
			return GameplayEmoteController.CanStartContextualEmote && this.TryPlayEmote(emote, suppressEndHud, false, false, true);
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x002B2064 File Offset: 0x002B0264
		private bool TryPlayEmote(EmoteDefinitionSO emote, bool suppressEndHud, bool bypassActivityGate, bool requestFutanariErection, bool contextualAction)
		{
			if (emote == null)
			{
				return false;
			}
			if (this.CurrentState == GameplayEmoteController.EmoteState.Playing)
			{
				return false;
			}
			if (!this.IsEmoteAllowedByPoi(emote))
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked outside required POI '" + emote.RequiredPoiId + "': " + emote.name);
				}
				return false;
			}
			if (!bypassActivityGate && !PlayerActivityGate.AllowsIgnoringLegacyReason(PlayerActivity.Emote, contextualAction ? "Rank" : null))
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked: PlayerActivityGate (Emote)");
				}
				return false;
			}
			if (!TimeFreezeController.IsAnyActive && ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked: detection event active");
				}
				return false;
			}
			if (this.ActionAnimator != null && this.ActionAnimator.GenericAction != null && this.ActionAnimator.GenericAction.doingAction)
			{
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked: player already doing action");
				}
				return false;
			}
			if (!this.IsEmoteAllowedByFutanari(emote))
			{
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[GameplayEmote] Rapid emote blocked by Futanari gating: {0} ({1})", emote.name, emote.FutanariRequirement));
				}
				return false;
			}
			if (!contextualAction && !this.IsEmoteAllowedByRank(emote, !bypassActivityGate))
			{
				BlockedActionFeedback.Show(null);
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked by Rank gating: " + emote.name);
				}
				return false;
			}
			string text;
			if (GameplayEmoteController.IsMasturbationEmoteBlocked(emote, out text))
			{
				BlockedActionFeedback.Show(text);
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked by the masturbation emote block: " + emote.name);
				}
				return false;
			}
			if (this.IsEmoteBlockedByClothing(emote))
			{
				BlockedActionFeedback.Show(this.ClothingBlockedMessage());
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked: bottom physically covered (" + emote.name + ")");
				}
				return false;
			}
			if (!this.TryAcquirePlaybackContext(bypassActivityGate, contextualAction))
			{
				if (!bypassActivityGate)
				{
					BlockedActionFeedback.Show(null);
				}
				if (this.DebugMode)
				{
					Debug.Log("[GameplayEmote] Rapid emote blocked: playback resources unavailable");
				}
				return false;
			}
			if (!bypassActivityGate)
			{
				this.BreakCoverPoseForEmote();
			}
			if (this.CurrentState == GameplayEmoteController.EmoteState.WheelOpen)
			{
				this.EmoteWheel.OnEmoteConfirmed -= this.OnEmoteConfirmed;
				if (this.EmoteWheel != null)
				{
					this.EmoteWheel.Close();
				}
				this.RestoreTimeScale();
				this.RestoreWheelCamera();
				this.CurrentState = GameplayEmoteController.EmoteState.Idle;
			}
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] Rapid emote: " + emote.DisplayName + (suppressEndHud ? " (silent end)" : ""));
			}
			this._suppressEndHud = suppressEndHud;
			this._playingEmote = emote;
			this.CurrentState = GameplayEmoteController.EmoteState.Playing;
			int num = this.BeginPlaybackSession();
			this.SetAutomaticFutanariErectionForPlayback(emote, requestFutanariErection);
			Action<EmoteDefinitionSO> onEmoteStarted = this.OnEmoteStarted;
			if (onEmoteStarted != null)
			{
				onEmoteStarted(emote);
			}
			if (this.IsCurrentPlayback(num) && !this._endingRequested)
			{
				this._emotePlaybackCoroutine = base.StartCoroutine(this.EmotePlaybackRoutine(emote, num));
			}
			return true;
		}

		// Token: 0x06006BCF RID: 27599 RVA: 0x002B2336 File Offset: 0x002B0536
		private int BeginPlaybackSession()
		{
			this.ReleasePlaybackHolds(false);
			this._endRequestedAfterHold = false;
			this._holdOwedSeconds = 0f;
			this._playbackSessionId++;
			this._endingRequested = false;
			this.ResetEmoteEndingTrigger();
			return this._playbackSessionId;
		}

		// Token: 0x06006BD0 RID: 27600 RVA: 0x002B2372 File Offset: 0x002B0572
		private bool IsCurrentPlayback(int playbackSessionId)
		{
			return this.CurrentState == GameplayEmoteController.EmoteState.Playing && this._playbackSessionId == playbackSessionId;
		}

		// Token: 0x06006BD1 RID: 27601 RVA: 0x002B2388 File Offset: 0x002B0588
		private void ResetEmoteEndingTrigger()
		{
			Animator animator = ((this._playerInput != null && this._playerInput.cc != null) ? this._playerInput.cc.animator : null);
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				animator.ResetTrigger("EmoteEnding");
			}
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x002B23EC File Offset: 0x002B05EC
		public void EndEmote()
		{
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing || this._endingRequested)
			{
				return;
			}
			this.ReleasePlaybackHolds(false);
			this._endingRequested = true;
			if (this.DebugMode)
			{
				string text = "[GameplayEmote] Ending emote: ";
				EmoteDefinitionSO playingEmote = this._playingEmote;
				Debug.Log(text + ((playingEmote != null) ? playingEmote.DisplayName : null));
			}
			if (!this._suppressEndHud)
			{
				this.HideEndEmoteHUD();
			}
			if (this._playingEmote != null && this._playingEmote.EndBehavior == EmoteEndBehavior.TimedEnd)
			{
				this.StopEmoteAudio();
			}
			if (!(this._playingEmote != null) || this._playingEmote.MovementMode != EmoteMovementMode.Moving)
			{
				if (this._emotePlaybackCoroutine != null)
				{
					base.StopCoroutine(this._emotePlaybackCoroutine);
					this._emotePlaybackCoroutine = null;
				}
				this.FinalizeEmoteCleanup();
				return;
			}
			if (this._emotePlaybackCoroutine != null)
			{
				base.StopCoroutine(this._emotePlaybackCoroutine);
				this._emotePlaybackCoroutine = null;
			}
			Animator animator = ((this._playerInput != null && this._playerInput.cc != null) ? this._playerInput.cc.animator : null);
			if (animator == null)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[GameplayEmote] Cannot fire EmoteEnding: animator not found. Falling back to immediate cleanup.");
				}
				this.FinalizeEmoteCleanup();
				return;
			}
			animator.ResetTrigger("EmoteEnding");
			animator.SetTrigger("EmoteEnding");
			this._movingEndCoroutine = base.StartCoroutine(this.MovingEmoteEndRoutine(this._playbackSessionId));
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x002B2558 File Offset: 0x002B0758
		public void ForceEndEmoteImmediate()
		{
			if (this.CurrentState == GameplayEmoteController.EmoteState.WheelOpen)
			{
				this.CloseWheel();
				return;
			}
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing)
			{
				return;
			}
			int playbackSessionId = this._playbackSessionId;
			this.ReleasePlaybackHolds(false);
			if (this._emotePlaybackCoroutine != null)
			{
				base.StopCoroutine(this._emotePlaybackCoroutine);
				this._emotePlaybackCoroutine = null;
			}
			if (this._movingEndCoroutine != null)
			{
				base.StopCoroutine(this._movingEndCoroutine);
				this._movingEndCoroutine = null;
			}
			if (!this._suppressEndHud)
			{
				this.HideEndEmoteHUD();
			}
			this._suppressEndHud = false;
			this.StopEmoteAudio();
			vGenericAction vGenericAction = ((this.ActionAnimator != null) ? this.ActionAnimator.GenericAction : null);
			vTriggerGenericAction vTriggerGenericAction = ((vGenericAction != null) ? vGenericAction.triggerAction : null);
			if (this.ActionAnimator != null && this.ActionAnimator.gameObject.activeInHierarchy && this.ActionAnimator.GenericAction != null)
			{
				this.ActionAnimator.FinishCurrentAction();
			}
			else if (this.ActionAnimator != null && this.ActionAnimator.GenericAction != null && this.ActionAnimator.GenericAction.doingAction)
			{
				this.ActionAnimator.GenericAction.ResetTriggerSettings(true);
			}
			if (this.IsCurrentPlayback(playbackSessionId) && vGenericAction != null && vGenericAction.doingAction && vGenericAction.triggerAction == vTriggerGenericAction)
			{
				vGenericAction.ResetTriggerSettings(true);
			}
			if (this.IsCurrentPlayback(playbackSessionId))
			{
				this.FinalizeEmoteCleanup();
			}
		}

		// Token: 0x06006BD4 RID: 27604 RVA: 0x002B26D4 File Offset: 0x002B08D4
		private void FinalizeEmoteCleanup()
		{
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing)
			{
				return;
			}
			this.ReleasePlaybackHolds(false);
			this._endRequestedAfterHold = false;
			if (this._emotePlaybackCoroutine != null)
			{
				base.StopCoroutine(this._emotePlaybackCoroutine);
				this._emotePlaybackCoroutine = null;
			}
			if (this._movingEndCoroutine != null)
			{
				base.StopCoroutine(this._movingEndCoroutine);
				this._movingEndCoroutine = null;
			}
			bool flag = this._playingEmote != null && this._playingEmote.MovementMode == EmoteMovementMode.Moving;
			if (this.PropHandler != null)
			{
				this.PropHandler.CleanupAllProps();
			}
			this.ApplyFutanariClothForEmote(this._playingEmote, false);
			this.ReleaseAutomaticFutanariErectionRequest();
			if (this.ActionAnimator != null)
			{
				if (!flag)
				{
					this.ActionAnimator.FinishCurrentAction();
				}
				this.ActionAnimator.RestoreOverrideLayer();
			}
			if (this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(false);
			}
			this.RestoreRootMotionFromEmote();
			this.ResetEmoteEndingTrigger();
			EmoteDefinitionSO playingEmote = this._playingEmote;
			this._playingEmote = null;
			this.ReleasePoseBlendShape();
			this._endingRequested = false;
			this._suppressEndHud = false;
			this.CurrentState = GameplayEmoteController.EmoteState.Idle;
			this.ReleasePlaybackActivityContext();
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] Emote ended, back to Idle");
			}
			Action<EmoteDefinitionSO> onEmoteEnded = this.OnEmoteEnded;
			if (onEmoteEnded == null)
			{
				return;
			}
			onEmoteEnded(playingEmote);
		}

		// Token: 0x06006BD5 RID: 27605 RVA: 0x002B281A File Offset: 0x002B0A1A
		private bool TryAcquirePlaybackActivityContext(bool bypassActivityGate)
		{
			return this.TryAcquirePlaybackContext(bypassActivityGate, false);
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x002B2824 File Offset: 0x002B0A24
		private bool TryAcquirePlaybackContext(bool bypassActivityGate, bool contextualAction)
		{
			if (this._playbackLease != null)
			{
				if (this._playbackLease.IsActive)
				{
					return true;
				}
				this._playbackLease = null;
			}
			if (bypassActivityGate)
			{
				return PlayerActivityGate.TryAcquire("Crouch.Playback#" + base.GetInstanceID().ToString(), this, PlayerActivity.Crouch, PlayerContextResource.FullBodyAnimation, PlayerActivity.Emote, out this._playbackLease, false, null);
			}
			return PlayerActivityGate.TryAcquire("Emote.Playback#" + base.GetInstanceID().ToString(), this, PlayerActivity.Emote, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, out this._playbackLease, false, contextualAction ? "Rank" : null);
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x002B28B8 File Offset: 0x002B0AB8
		private void ReleasePlaybackActivityContext()
		{
			PlayerActivityLease playbackLease = this._playbackLease;
			this._playbackLease = null;
			if (playbackLease == null)
			{
				return;
			}
			playbackLease.Dispose();
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x002B28D1 File Offset: 0x002B0AD1
		private IEnumerator MovingEmoteEndRoutine(int playbackSessionId)
		{
			float deadline = Time.unscaledTime + 3f;
			vThirdPersonController cc = ((this._playerInput != null) ? this._playerInput.cc : null);
			Animator animator = ((cc != null) ? cc.animator : null);
			int actionLayer = ((this.ActionAnimator != null) ? this.ActionAnimator.CurrentActionAnimatorLayer : 0);
			yield return null;
			if (!this.IsCurrentPlayback(playbackSessionId))
			{
				yield break;
			}
			bool sawEndStateActive = false;
			bool timedOut = false;
			while (Time.unscaledTime < deadline)
			{
				if (!this.IsCurrentPlayback(playbackSessionId))
				{
					yield break;
				}
				if (cc != null && cc.customAction)
				{
					sawEndStateActive = true;
					break;
				}
				yield return null;
			}
			if (!sawEndStateActive)
			{
				timedOut = true;
			}
			else
			{
				while (Time.unscaledTime < deadline)
				{
					if (!this.IsCurrentPlayback(playbackSessionId))
					{
						yield break;
					}
					if ((cc == null || !cc.customAction) && (animator == null || !animator.IsInTransition(actionLayer)))
					{
						break;
					}
					yield return null;
				}
				if (Time.unscaledTime >= deadline)
				{
					timedOut = true;
				}
			}
			if (!this.IsCurrentPlayback(playbackSessionId))
			{
				yield break;
			}
			if (this.DebugMode)
			{
				Debug.Log(timedOut ? string.Format("[GameplayEmote] Moving emote end TIMEOUT after {0:0.0}s — forcing cleanup", 3f) : "[GameplayEmote] Moving emote: End state finished, finishing action");
			}
			this._movingEndCoroutine = null;
			if (timedOut)
			{
				this.ForceEndEmoteImmediate();
				yield break;
			}
			if (this.ActionAnimator != null && this.ActionAnimator.GenericAction != null && this.ActionAnimator.GenericAction.doingAction)
			{
				this.ActionAnimator.GenericAction.FinishAction();
			}
			if (this.IsCurrentPlayback(playbackSessionId))
			{
				this.FinalizeEmoteCleanup();
			}
			yield break;
		}

		// Token: 0x06006BD9 RID: 27609 RVA: 0x002B28E8 File Offset: 0x002B0AE8
		private void OnGenericActionEnded(vTriggerGenericAction trigger)
		{
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing)
			{
				return;
			}
			if (this._playingEmote == null || this._playingEmote.MovementMode != EmoteMovementMode.Moving)
			{
				return;
			}
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] OnEndAction received -> finalizing moving emote (safety path)");
			}
			this.FinalizeEmoteCleanup();
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x06006BDA RID: 27610 RVA: 0x002B2934 File Offset: 0x002B0B34
		public bool IsPlaybackHeld
		{
			get
			{
				return this._holdOwners.Count > 0;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06006BDB RID: 27611 RVA: 0x002B2944 File Offset: 0x002B0B44
		public bool IsEndDeferredByHold
		{
			get
			{
				return this._endRequestedAfterHold;
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06006BDC RID: 27612 RVA: 0x002B294C File Offset: 0x002B0B4C
		public EmotePlaybackHoldSpeed PlaybackHoldSpeed
		{
			get
			{
				return this._holdSpeed;
			}
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x002B2954 File Offset: 0x002B0B54
		public bool IsPlaybackHeldBy(Object owner)
		{
			return this.IndexOfHoldOwner(owner) >= 0;
		}

		// Token: 0x06006BDE RID: 27614 RVA: 0x002B2964 File Offset: 0x002B0B64
		public bool TryHoldPlayback(Object owner)
		{
			if (owner == null || this.CurrentState != GameplayEmoteController.EmoteState.Playing || this._endingRequested || this._playingEmote == null)
			{
				return false;
			}
			if (this._playingEmote.MovementMode == EmoteMovementMode.Moving)
			{
				return false;
			}
			if (this.IndexOfHoldOwner(owner) >= 0)
			{
				return true;
			}
			if (this._holdOwners.Count == 0)
			{
				if (SmartphoneController.IsAnySelfieActive || SmartphoneController.IsAnyTripodActive)
				{
					return false;
				}
				this.BeginPlaybackHold();
			}
			this._holdOwners.Add(owner);
			return true;
		}

		// Token: 0x06006BDF RID: 27615 RVA: 0x002B29E8 File Offset: 0x002B0BE8
		public void ReleasePlaybackHold(Object owner)
		{
			int num = this.IndexOfHoldOwner(owner);
			if (num < 0)
			{
				return;
			}
			this._holdOwners.RemoveAt(num);
			if (this._holdOwners.Count == 0)
			{
				this.EndPlaybackHold(false, false);
			}
		}

		// Token: 0x06006BE0 RID: 27616 RVA: 0x002B2A23 File Offset: 0x002B0C23
		public void RequestEndEmote()
		{
			if (this.CurrentState != GameplayEmoteController.EmoteState.Playing || this._endingRequested)
			{
				return;
			}
			if (this.IsPlaybackHeld)
			{
				this._endRequestedAfterHold = true;
				return;
			}
			this.EndEmote();
		}

		// Token: 0x06006BE1 RID: 27617 RVA: 0x002B2A50 File Offset: 0x002B0C50
		private int IndexOfHoldOwner(Object owner)
		{
			if (owner == null)
			{
				return -1;
			}
			for (int i = 0; i < this._holdOwners.Count; i++)
			{
				if (this._holdOwners[i] == owner)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x002B2A8C File Offset: 0x002B0C8C
		private void BeginPlaybackHold()
		{
			this._holdSessionId = this._playbackSessionId;
			this._holdSpeed.Begin(this.ResolveHoldAnimator());
			if (this.EmoteAudioSource != null && this._playingEmote.EmoteAudioClip != null && this.EmoteAudioSource.clip == this._playingEmote.EmoteAudioClip && this.EmoteAudioSource.isPlaying)
			{
				this.EmoteAudioSource.Pause();
				this._holdPausedAudio = true;
			}
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x002B2B14 File Offset: 0x002B0D14
		private void EndPlaybackHold(bool snap, bool final)
		{
			this._holdOwners.Clear();
			if (this._holdPausedAudio)
			{
				this._holdPausedAudio = false;
				if (this.EmoteAudioSource != null && this._playingEmote != null && this.EmoteAudioSource.clip == this._playingEmote.EmoteAudioClip)
				{
					this.EmoteAudioSource.UnPause();
				}
			}
			this._holdSpeed.Step(false, 0f, 0.12f, 0.25f, snap, final);
		}

		// Token: 0x06006BE4 RID: 27620 RVA: 0x002B2B9C File Offset: 0x002B0D9C
		private void ReleasePlaybackHolds(bool final = false)
		{
			if (this._holdOwners.Count > 0 || this._holdPausedAudio)
			{
				this.EndPlaybackHold(true, final);
				return;
			}
			this._holdSpeed.Step(false, 0f, 0.12f, 0.25f, true, final);
		}

		// Token: 0x06006BE5 RID: 27621 RVA: 0x002B2BDC File Offset: 0x002B0DDC
		private void TickPlaybackHold()
		{
			if (this._holdOwners.Count > 0)
			{
				for (int i = this._holdOwners.Count - 1; i >= 0; i--)
				{
					if (this._holdOwners[i] == null)
					{
						this._holdOwners.RemoveAt(i);
					}
				}
				if (!this.IsCurrentPlayback(this._holdSessionId) || this._endingRequested)
				{
					this.ReleasePlaybackHolds(false);
				}
				else if (this._holdOwners.Count == 0)
				{
					this.EndPlaybackHold(false, false);
				}
			}
			bool flag = this._holdOwners.Count > 0;
			if (this.CurrentState == GameplayEmoteController.EmoteState.Playing)
			{
				float num = (this._holdSpeed.IsManaging ? this._holdSpeed.FrozenFraction : (flag ? 1f : 0f));
				if (num > 0f)
				{
					this._holdOwedSeconds += Time.deltaTime * num;
				}
			}
			this._holdSpeed.Step(flag, Time.unscaledDeltaTime, 0.12f, 0.25f, false, false);
			if (this._endRequestedAfterHold && !flag)
			{
				this._endRequestedAfterHold = false;
				if (this.CurrentState == GameplayEmoteController.EmoteState.Playing && !this._endingRequested)
				{
					this.EndEmote();
				}
			}
		}

		// Token: 0x06006BE6 RID: 27622 RVA: 0x002B2D08 File Offset: 0x002B0F08
		private Animator ResolveHoldAnimator()
		{
			if (!(this._playerInput != null) || !(this._playerInput.cc != null) || !(this._playerInput.cc.animator != null))
			{
				return base.GetComponent<Animator>();
			}
			return this._playerInput.cc.animator;
		}

		// Token: 0x06006BE7 RID: 27623 RVA: 0x002B2D68 File Offset: 0x002B0F68
		private void ShowEndEmoteHUD()
		{
			if (InteractionHUDController.Instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = ((this._playingEmote != null && this._playingEmote.IsMasturbationEmote && this.MasturbationEmoteHUDConfig != null) ? this.MasturbationEmoteHUDConfig : this.EndEmoteHUDConfig);
			if (interactionConfigSO != null)
			{
				if (this.EndEmoteInput != null)
				{
					ControllerContextBindings.SetLabel(interactionConfigSO, this.EndEmoteInput.keyboard, "A", false);
				}
				ControllerContextBindings.SetLabel(interactionConfigSO, "CTRL + MW", "LT + RS", false);
				InteractionHUDController.Instance.Show(interactionConfigSO);
			}
		}

		// Token: 0x06006BE8 RID: 27624 RVA: 0x00170130 File Offset: 0x0016E330
		private void HideEndEmoteHUD()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
		}

		// Token: 0x06006BE9 RID: 27625 RVA: 0x002B2E00 File Offset: 0x002B1000
		private void PlayEmoteAudio(EmoteDefinitionSO emote)
		{
			if (((emote != null) ? emote.EmoteAudioClip : null) == null || this.EmoteAudioSource == null)
			{
				return;
			}
			this.EmoteAudioSource.clip = emote.EmoteAudioClip;
			this.EmoteAudioSource.volume = emote.AudioVolume;
			this.EmoteAudioSource.loop = false;
			this.EmoteAudioSource.Play();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[GameplayEmote] Playing audio for emote '{0}' (volume: {1})", emote.DisplayName, emote.AudioVolume));
			}
		}

		// Token: 0x06006BEA RID: 27626 RVA: 0x002B2E91 File Offset: 0x002B1091
		private void StopEmoteAudio()
		{
			if (this.EmoteAudioSource != null && this.EmoteAudioSource.isPlaying)
			{
				this.EmoteAudioSource.Stop();
			}
		}

		// Token: 0x06006BEB RID: 27627 RVA: 0x002B2EBC File Offset: 0x002B10BC
		private void EnableLocomotionForEmote(EmoteDefinitionSO emote)
		{
			if (this._playerInput == null || this._playerInput.cc == null)
			{
				return;
			}
			if (emote == null)
			{
				return;
			}
			if (emote.MovingUseRootMotion)
			{
				this.EnableRootMotionForEmote();
				return;
			}
			this.EnableNoRootMotionLocomotion(emote.MovingNoRootMotionSpeed);
		}

		// Token: 0x06006BEC RID: 27628 RVA: 0x002B2F10 File Offset: 0x002B1110
		private void EnableRootMotionForEmote()
		{
			if (this._rootMotionOverridden)
			{
				return;
			}
			if (this._playerInput == null || this._playerInput.cc == null)
			{
				return;
			}
			this._savedUseRootMotion = this._playerInput.cc.useRootMotion;
			this._playerInput.cc.useRootMotion = true;
			Animator animator = this._playerInput.cc.animator;
			if (animator != null)
			{
				this._savedAnimatorApplyRootMotion = animator.applyRootMotion;
				animator.applyRootMotion = true;
			}
			this._rootMotionOverridden = true;
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] Root motion enabled for moving emote");
			}
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x002B2FB8 File Offset: 0x002B11B8
		private void EnableNoRootMotionLocomotion(float speed)
		{
			if (this._playerInput == null || this._playerInput.cc == null)
			{
				return;
			}
			this._savedUseRootMotion = this._playerInput.cc.useRootMotion;
			this._playerInput.cc.useRootMotion = false;
			Animator animator = this._playerInput.cc.animator;
			if (animator != null)
			{
				this._savedAnimatorApplyRootMotion = animator.applyRootMotion;
				animator.applyRootMotion = false;
			}
			this._rootMotionOverridden = true;
			vThirdPersonMotor.vMovementSpeed freeSpeed = this._playerInput.cc.freeSpeed;
			if (freeSpeed != null)
			{
				this._savedFreeWalkSpeed = freeSpeed.walkSpeed;
				this._savedFreeRunningSpeed = freeSpeed.runningSpeed;
				this._savedFreeSprintSpeed = freeSpeed.sprintSpeed;
				freeSpeed.walkSpeed = speed;
				freeSpeed.runningSpeed = speed;
				freeSpeed.sprintSpeed = speed;
				this._freeSpeedOverridden = true;
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[GameplayEmote] No-root-motion locomotion enabled (speed={0:0.00})", speed));
			}
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x002B30B4 File Offset: 0x002B12B4
		private void RestoreRootMotionFromEmote()
		{
			if (!this._rootMotionOverridden && !this._freeSpeedOverridden)
			{
				return;
			}
			if (this._playerInput != null && this._playerInput.cc != null)
			{
				if (this._rootMotionOverridden)
				{
					this._playerInput.cc.useRootMotion = this._savedUseRootMotion;
					Animator animator = this._playerInput.cc.animator;
					if (animator != null)
					{
						animator.applyRootMotion = this._savedAnimatorApplyRootMotion;
					}
				}
				if (this._freeSpeedOverridden)
				{
					vThirdPersonMotor.vMovementSpeed freeSpeed = this._playerInput.cc.freeSpeed;
					if (freeSpeed != null)
					{
						freeSpeed.walkSpeed = this._savedFreeWalkSpeed;
						freeSpeed.runningSpeed = this._savedFreeRunningSpeed;
						freeSpeed.sprintSpeed = this._savedFreeSprintSpeed;
					}
				}
			}
			this._rootMotionOverridden = false;
			this._freeSpeedOverridden = false;
			if (this.DebugMode)
			{
				Debug.Log("[GameplayEmote] Locomotion settings restored");
			}
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x002B319D File Offset: 0x002B139D
		private void ApplyWheelTimeScale()
		{
			this._savedTimeScale = Time.timeScale;
			this._wheelTimeScaleApplied = !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldSimulation);
			if (this._wheelTimeScaleApplied)
			{
				Time.timeScale = this.WheelOpenTimeScale;
			}
		}

		// Token: 0x06006BF0 RID: 27632 RVA: 0x002B31D0 File Offset: 0x002B13D0
		private void RestoreTimeScale()
		{
			if (this._wheelTimeScaleApplied)
			{
				Time.timeScale = this._savedTimeScale;
			}
			this._wheelTimeScaleApplied = false;
		}

		// Token: 0x04006E33 RID: 28211
		[Header("Emote Data")]
		[Tooltip("Collection of emotes available in the gameplay wheel")]
		public GameplayEmoteCollectionSO EmoteCollection;

		// Token: 0x04006E34 RID: 28212
		[Header("Emote Wheel")]
		[Tooltip("Reference to the EmoteWheelUI in the scene")]
		public EmoteWheelUI EmoteWheel;

		// Token: 0x04006E35 RID: 28213
		[Tooltip("Theme used for the gameplay emote wheel (card prefab + background)")]
		public EmoteWheelThemeSO WheelTheme;

		// Token: 0x04006E36 RID: 28214
		[Header("POI Context (Optional)")]
		[Tooltip("If assigned, POI profile can override theme and inject priority emotes at top of list")]
		public PlayerPoiEmoteContext PoiContext;

		// Token: 0x04006E37 RID: 28215
		[Header("Futanari (Optional)")]
		[Tooltip("Riferimento al FutanariModeController per filtrare gli emote in base alla modalita'. Se null, viene cercato in modo automatico nei figli del player.")]
		public FutanariModeController FutanariMode;

		// Token: 0x04006E38 RID: 28216
		[Tooltip("Controller fisico del pene. Riceve una richiesta temporanea soltanto dagli emote avviati tramite gli slot Q/Z; se null viene cercato nei figli del player.")]
		public PenisErectionController FutanariErection;

		// Token: 0x04006E39 RID: 28217
		[Header("Input")]
		[HideInInspector]
		public GenericInput ToggleInput = new GenericInput("None", "None", "None")
		{
			useInput = false
		};

		// Token: 0x04006E3A RID: 28218
		[Tooltip("Input to end a playing emote (for ManualEnd emotes)")]
		public GenericInput EndEmoteInput = new GenericInput("Space", "A", "A");

		// Token: 0x04006E3B RID: 28219
		[Header("Hysteresis")]
		[Tooltip("Minimum seconds between toggle presses to avoid accidental close/reopen")]
		[Range(0.1f, 1f)]
		public float ToggleCooldown = 0.25f;

		// Token: 0x04006E3C RID: 28220
		[Header("Player Action")]
		[Tooltip("Reference to the PlayerActionAnimator on the player")]
		public PlayerActionAnimator ActionAnimator;

		// Token: 0x04006E3D RID: 28221
		[Header("Props")]
		[Tooltip("Reference to the EmotePropHandler on the player")]
		public EmotePropHandler PropHandler;

		// Token: 0x04006E3E RID: 28222
		[Header("Audio")]
		[Tooltip("AudioSource for playing emote sounds (optional, auto-found if null)")]
		public AudioSource EmoteAudioSource;

		// Token: 0x04006E3F RID: 28223
		[Header("Time Scale")]
		[Tooltip("Time scale applied while the emote wheel is open (0 = full pause)")]
		[Range(0f, 1f)]
		public float WheelOpenTimeScale;

		// Token: 0x04006E40 RID: 28224
		[Header("Interaction HUD")]
		[Tooltip("InteractionConfigSO used to show 'end emote' hint via the shared InteractionHUDController")]
		public InteractionConfigSO EndEmoteHUDConfig;

		// Token: 0x04006E41 RID: 28225
		[Tooltip("Optional override for masturbation emotes: shows both 'end emote' AND the speed control hint (Ctrl + Mouse Wheel). If null, falls back to EndEmoteHUDConfig.")]
		public InteractionConfigSO MasturbationEmoteHUDConfig;

		// Token: 0x04006E42 RID: 28226
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x04006E44 RID: 28228
		private float _savedTimeScale = 1f;

		// Token: 0x04006E45 RID: 28229
		private bool _wheelTimeScaleApplied;

		// Token: 0x04006E46 RID: 28230
		private EmoteDefinitionSO _playingEmote;

		// Token: 0x04006E47 RID: 28231
		private BlendShapeManager _poseBlendShapeManager;

		// Token: 0x04006E48 RID: 28232
		private const string PoseBlendShapeSource = "GameplayEmote.DownBack";

		// Token: 0x04006E49 RID: 28233
		private const string PoseBlendShape = "Kupa_V_LR";

		// Token: 0x04006E4A RID: 28234
		private Coroutine _emotePlaybackCoroutine;

		// Token: 0x04006E4B RID: 28235
		private PlayerActivityLease _playbackLease;

		// Token: 0x04006E4C RID: 28236
		private bool _suppressEndHud;

		// Token: 0x04006E4D RID: 28237
		private bool _playingEmoteRequestsFutanariErection;

		// Token: 0x04006E4E RID: 28238
		private vThirdPersonInput _playerInput;

		// Token: 0x04006E4F RID: 28239
		private bool _wheelCameraCaptured;

		// Token: 0x04006E50 RID: 28240
		private bool _savedWheelCameraLock;

		// Token: 0x04006E51 RID: 28241
		private bool _savedWheelForceCameraLock;

		// Token: 0x04006E52 RID: 28242
		private PlayerPoiEmoteContext _subscribedPoiContext;

		// Token: 0x04006E53 RID: 28243
		private bool _rootMotionOverridden;

		// Token: 0x04006E54 RID: 28244
		private bool _savedUseRootMotion;

		// Token: 0x04006E55 RID: 28245
		private bool _savedAnimatorApplyRootMotion;

		// Token: 0x04006E56 RID: 28246
		private bool _freeSpeedOverridden;

		// Token: 0x04006E57 RID: 28247
		private float _savedFreeWalkSpeed;

		// Token: 0x04006E58 RID: 28248
		private float _savedFreeRunningSpeed;

		// Token: 0x04006E59 RID: 28249
		private float _savedFreeSprintSpeed;

		// Token: 0x04006E5A RID: 28250
		private Coroutine _movingEndCoroutine;

		// Token: 0x04006E5B RID: 28251
		private bool _endingRequested;

		// Token: 0x04006E5C RID: 28252
		private int _playbackSessionId;

		// Token: 0x04006E5D RID: 28253
		private const string EmoteEndingTrigger = "EmoteEnding";

		// Token: 0x04006E5E RID: 28254
		private const string PlaybackGateReason = "Emote.Playback";

		// Token: 0x04006E5F RID: 28255
		private const PlayerContextResource PlaybackResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04006E60 RID: 28256
		private const PlayerActivity PlaybackBlockedActivities = PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose;

		// Token: 0x04006E61 RID: 28257
		private const string CrouchGateReason = "Crouch.Playback";

		// Token: 0x04006E62 RID: 28258
		private const PlayerContextResource CrouchResources = PlayerContextResource.FullBodyAnimation;

		// Token: 0x04006E63 RID: 28259
		private const PlayerActivity CrouchBlockedActivities = PlayerActivity.Emote;

		// Token: 0x04006E64 RID: 28260
		private const float MovingEndTimeoutSeconds = 3f;

		// Token: 0x04006E65 RID: 28261
		private static readonly List<Object> MasturbationBlockOwners = new List<Object>(2);

		// Token: 0x04006E66 RID: 28262
		private static readonly List<string> MasturbationBlockReasons = new List<string>(2);

		// Token: 0x04006E6B RID: 28267
		private readonly GenericInput _controllerEndInput = new GenericInput("Space", "A", "A");

		// Token: 0x04006E6C RID: 28268
		private CoverPoseController _coverPoseController;

		// Token: 0x04006E6D RID: 28269
		private const float HoldFreezeSeconds = 0.12f;

		// Token: 0x04006E6E RID: 28270
		private const float HoldResumeSeconds = 0.25f;

		// Token: 0x04006E6F RID: 28271
		private readonly List<Object> _holdOwners = new List<Object>(2);

		// Token: 0x04006E70 RID: 28272
		private readonly EmotePlaybackHoldSpeed _holdSpeed = new EmotePlaybackHoldSpeed();

		// Token: 0x04006E71 RID: 28273
		private int _holdSessionId;

		// Token: 0x04006E72 RID: 28274
		private bool _holdPausedAudio;

		// Token: 0x04006E73 RID: 28275
		private bool _endRequestedAfterHold;

		// Token: 0x04006E74 RID: 28276
		private float _holdOwedSeconds;

		// Token: 0x02000E20 RID: 3616
		public enum EmoteState
		{
			// Token: 0x04006E76 RID: 28278
			Idle,
			// Token: 0x04006E77 RID: 28279
			WheelOpen,
			// Token: 0x04006E78 RID: 28280
			Playing
		}
	}
}
