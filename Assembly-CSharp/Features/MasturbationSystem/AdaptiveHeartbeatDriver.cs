using System;
using System.Collections.Generic;
using Dinopunch.LivingMud;
using Features.BodyPaintStudio;
using Features.BoobTouch;
using Features.CielVoice;
using Features.Detection;
using Features.Flash;
using Features.Gym;
using Features.InsertableToys;
using Features.MallHack;
using Features.PlayerEmotes;
using Features.PlayerEmotions;
using Features.TintBodyPaint;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FED RID: 4077
	[DefaultExecutionOrder(-100)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(HeartbeatController))]
	public sealed class AdaptiveHeartbeatDriver : MonoBehaviour
	{
		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06007B57 RID: 31575 RVA: 0x003260B8 File Offset: 0x003242B8
		// (set) Token: 0x06007B58 RID: 31576 RVA: 0x003260C0 File Offset: 0x003242C0
		public float CurrentDesiredBpm { get; private set; }

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06007B59 RID: 31577 RVA: 0x003260C9 File Offset: 0x003242C9
		// (set) Token: 0x06007B5A RID: 31578 RVA: 0x003260D1 File Offset: 0x003242D1
		public float CurrentCoverage01 { get; private set; } = 1f;

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x003260DA File Offset: 0x003242DA
		// (set) Token: 0x06007B5C RID: 31580 RVA: 0x003260E2 File Offset: 0x003242E2
		public float CurrentNpcAttention01 { get; private set; }

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06007B5D RID: 31581 RVA: 0x003260EB File Offset: 0x003242EB
		// (set) Token: 0x06007B5E RID: 31582 RVA: 0x003260F3 File Offset: 0x003242F3
		public float CurrentNpcProximity01 { get; private set; }

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06007B5F RID: 31583 RVA: 0x003260FC File Offset: 0x003242FC
		// (set) Token: 0x06007B60 RID: 31584 RVA: 0x00326104 File Offset: 0x00324304
		public bool IsBodyPaintActive { get; private set; }

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06007B61 RID: 31585 RVA: 0x0032610D File Offset: 0x0032430D
		// (set) Token: 0x06007B62 RID: 31586 RVA: 0x00326115 File Offset: 0x00324315
		public bool IsHidden { get; private set; }

		// Token: 0x06007B63 RID: 31587 RVA: 0x0032611E File Offset: 0x0032431E
		private void Awake()
		{
			if (this.heartbeat == null)
			{
				this.heartbeat = base.GetComponent<HeartbeatController>();
			}
			this._variationPhase = Mathf.Abs((float)base.GetInstanceID() * 0.0174533f) % 6.2831855f;
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x00326158 File Offset: 0x00324358
		private void OnEnable()
		{
			this.ResolveReferences(true);
			this.RefreshFlashGatesIfDirty();
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x00326167 File Offset: 0x00324367
		private void OnDisable()
		{
			this.BindEmoteController(null);
			this.BindClothingController(null);
			if (this.heartbeat != null)
			{
				this.heartbeat.ClearSource(this.EffectiveSourceId);
			}
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x00326198 File Offset: 0x00324398
		private void Update()
		{
			this.ResolveReferences(false);
			this.RefreshFlashGatesIfDirty();
			if (this.heartbeat == null)
			{
				return;
			}
			float clockTime = this.heartbeat.ClockTime;
			if (clockTime >= this._nextNpcSampleTime)
			{
				this._nextNpcSampleTime = clockTime + Mathf.Max(0.05f, this.npcSampleInterval);
				this.RefreshNpcContext();
			}
			bool flag = this.clothing != null;
			bool flag2 = flag && this.clothing.IsFullyClothed;
			bool flag3 = flag2 && PlayerExposureState.IsExposed;
			bool flag4 = flag2 && !flag3;
			this.CurrentCoverage01 = (flag ? this.clothing.NormalizedBodyCoverage : 1f);
			if (flag3)
			{
				this.CurrentCoverage01 = Mathf.Min(this.CurrentCoverage01, 0.35f);
			}
			this.IsHidden = PlayerStealthBridge.IsFullyHidden;
			this.IsBodyPaintActive = this.ResolveBodyPaintActive();
			this.UpdateAttentionSpike(clockTime, flag && !flag4 && !this.IsHidden);
			Vector2 vector = AdaptiveHeartbeatModel.ResolveBaseRange(flag, flag4, this.CurrentCoverage01, this.fullyClothedBpmRange, this.nakedBpmRange, this.partiallyCoveredFloorRange);
			if (!flag4 && this.IsHidden)
			{
				vector = this.hiddenExposedBpmRange;
			}
			else if (!flag4 && this.IsBodyPaintActive)
			{
				vector = this.bodyPaintBpmRange;
			}
			float num = AdaptiveHeartbeatModel.VariableBpm(vector, clockTime, this._variationPhase);
			float num2 = (flag4 ? 0f : (1f - this.CurrentCoverage01));
			bool flag5 = ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive;
			if (!flag4 && !this.IsHidden)
			{
				float num3 = Mathf.Lerp(0.3f, 1f, num2);
				bool flag6 = !this._attentionSpikeArmed;
				float num4 = (flag5 ? this.attentionSpikeRearmThreshold : (flag6 ? this.postSpikeAttentionCap : 1f));
				float num5 = AdaptiveHeartbeatModel.CappedNpcAttention01(this.CurrentNpcAttention01, num4);
				float num6 = (flag5 ? 0f : (this.CurrentNpcProximity01 * (flag6 ? this.postSpikeProximityMultiplier : 1f)));
				num += num6 * this.npcProximityBoostBpm * num3;
				num += num5 * this.npcAttentionBoostBpm * num3;
			}
			bool flag7;
			bool flag8;
			if (this.TryGetEmoteState(out flag7, out flag8) && flag7)
			{
				num = (flag8 ? Mathf.Max(num, this.masturbationEmoteFloorBpm) : (num + this.normalEmoteBoostBpm));
			}
			if (this.coverPose != null && this.coverPose.IsCovering)
			{
				CoverPoseHeartbeatEffect coverPoseHeartbeatEffect = AdaptiveHeartbeatModel.ClassifyCoverPose(this.coverPose.GetCurrentEmotionPoseLabel());
				if (coverPoseHeartbeatEffect != CoverPoseHeartbeatEffect.Exciting)
				{
					if (coverPoseHeartbeatEffect == CoverPoseHeartbeatEffect.Stimulation)
					{
						num = Mathf.Max(num, this.stimulationPoseFloorBpm);
					}
					else
					{
						num -= this.coverPoseReductionBpm;
					}
				}
				else
				{
					num += this.totalExposeBoostBpm;
				}
			}
			if ((this.AnyFlashEngaged() || PlayerExposureState.IsFlashActive || flag3) && !this.IsHidden)
			{
				num += this.flashBoostBpm;
			}
			if (flag5)
			{
				num += this.caughtEventBoostBpm;
			}
			EmotionManager instance = EmotionManager.Instance;
			if (instance != null)
			{
				float num7 = Mathf.InverseLerp(70f, 100f, Mathf.Abs(instance.EmotionBalance));
				num += num7 * this.supremeEmotionMaxBoostBpm * num2;
			}
			if (this.gymExercise != null && this.gymExercise.IsExerciseActive)
			{
				num = Mathf.Max(num, this.exerciseFloorBpm);
			}
			if (this.swimming != null && this.swimming.isSwimming)
			{
				num = Mathf.Max(num, this.swimmingFloorBpm);
			}
			if ((this.runningNude != null && this.runningNude.IsActive) || (this.movement != null && this.movement.isSprinting && this.movement.inputMagnitude > 0.1f))
			{
				num += this.sprintBoostBpm;
			}
			else if (this.movement != null && this.movement.inputMagnitude > 0.2f)
			{
				num += this.movementBoostBpm;
			}
			if (this.boobTouch != null && this.boobTouch.IsActive)
			{
				num += this.normalEmoteBoostBpm;
			}
			if (this.groping != null && this.groping.IsActive)
			{
				num = Mathf.Max(num, this.gropingFloorBpm);
			}
			if (this.groping != null && this.groping.IsOrgasmReactionActive)
			{
				num = Mathf.Max(num, this.adaptiveMaximumBpm);
			}
			if (this.insertableToy != null && this.insertableToy.IsInserted)
			{
				num = Mathf.Max(num, this.insertionFloorBpm);
			}
			if (HackMePhoneApp.IsAnyHackInProgress)
			{
				num = Mathf.Max(num, this.hackFloorBpm);
			}
			if (this._attentionSpikeActive)
			{
				float num8 = clockTime - this._attentionSpikeStartedAt;
				if (num8 >= this.attentionSpikeDuration)
				{
					this._attentionSpikeActive = false;
				}
				else
				{
					num = Mathf.Max(num, AdaptiveHeartbeatModel.SpikeFloorBpm(num8, this.attentionSpikeDuration, this.attentionSpikePeakBpm, this.attentionSpikeSettledBpm));
				}
			}
			float num9 = Mathf.Min(this.adaptiveMaximumBpm, this.heartbeat.PeakBpm - 0.5f);
			this.CurrentDesiredBpm = Mathf.Clamp(num, this.heartbeat.RestingBpm, num9);
			if (this.CurrentDesiredBpm >= this.heartbeat.RestingBpm + 8f)
			{
				TutorialService.TryShowFirstUse("heartbeat_orgasm");
			}
			this.heartbeat.SetSourceBpm(this.EffectiveSourceId, this.CurrentDesiredBpm);
			if (this.debugMode && Time.frameCount % 120 == 0)
			{
				Debug.Log(string.Format("[AdaptiveHeartbeat] {0:F0} BPM | coverage={1:F2} ", this.CurrentDesiredBpm, this.CurrentCoverage01) + string.Format("hidden={0} paint={1} npc={2:F2}/", this.IsHidden, this.IsBodyPaintActive, this.CurrentNpcAttention01) + string.Format("{0:F2} emote={1}", this.CurrentNpcProximity01, flag8), this);
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06007B67 RID: 31591 RVA: 0x0032678E File Offset: 0x0032498E
		private string EffectiveSourceId
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.sourceId))
				{
					return this.sourceId;
				}
				return "adaptive";
			}
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x003267AC File Offset: 0x003249AC
		private void ResolveReferences(bool force)
		{
			float unscaledTime = Time.unscaledTime;
			if (!force && unscaledTime < this._nextResolveTime)
			{
				return;
			}
			this._nextResolveTime = unscaledTime + 1f;
			if (this.heartbeat == null)
			{
				this.heartbeat = base.GetComponent<HeartbeatController>();
			}
			ClothingToggleController clothingToggleController = ((this.clothing != null) ? this.clothing : Object.FindObjectOfType<ClothingToggleController>());
			this.BindClothingController(clothingToggleController);
			if (this.playerEmotion == null)
			{
				this.playerEmotion = ((PlayerEmotionController.Instance != null) ? PlayerEmotionController.Instance : this.ResolvePlayerComponent<PlayerEmotionController>(false));
			}
			if (this.coverPose == null)
			{
				this.coverPose = ((CoverPoseController.Instance != null) ? CoverPoseController.Instance : this.ResolvePlayerComponent<CoverPoseController>(false));
			}
			if (this.insertableToy == null)
			{
				this.insertableToy = this.ResolvePlayerComponent<InsertableToyController>(true);
			}
			if (this.skinDecals == null)
			{
				this.skinDecals = this.ResolvePlayerComponent<SkinDecalController>(false);
			}
			if (this.bodyPaintCanvas == null)
			{
				this.bodyPaintCanvas = this.ResolvePlayerComponent<BodyPaintCanvasService>(true);
			}
			if (this.tintBodyPaint == null)
			{
				this.tintBodyPaint = this.ResolvePlayerComponent<TintBodyPaintRuntimeController>(true);
			}
			if (this.mudBodyPaint == null)
			{
				this.mudBodyPaint = this.ResolvePlayerComponent<MudBodyPaintRuntimeController>(true);
			}
			if (this.gymExercise == null)
			{
				this.gymExercise = this.ResolvePlayerComponent<GymExerciseSweatController>(false);
			}
			if (this.runningNude == null)
			{
				this.runningNude = this.ResolvePlayerComponent<CielRunningNude>(true);
			}
			if (this.movement == null)
			{
				this.movement = this.ResolvePlayerComponent<vThirdPersonController>(false);
			}
			if (this.swimming == null)
			{
				this.swimming = this.ResolvePlayerComponent<vSwimming>(false);
			}
			if (this.groping == null)
			{
				this.groping = this.ResolvePlayerComponent<GropingModeController>(true);
			}
			if (this.boobTouch == null)
			{
				this.boobTouch = this.ResolvePlayerComponent<BoobTouchModeController>(true);
			}
			GameplayEmoteController gameplayEmoteController = ((this.emotes != null) ? this.emotes : this.ResolvePlayerComponent<GameplayEmoteController>(false));
			this.BindEmoteController(gameplayEmoteController);
			if (this.clothing != null)
			{
				this._playerTransform = this.clothing.transform;
				return;
			}
			if (SensoryManager.Instance != null && SensoryManager.Instance.GetPlayer() != null)
			{
				this._playerTransform = SensoryManager.Instance.GetPlayer().transform;
			}
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x00326A16 File Offset: 0x00324C16
		private T ResolvePlayerComponent<T>(bool includeChildren) where T : Component
		{
			if (!(this.clothing != null))
			{
				return Object.FindObjectOfType<T>();
			}
			if (!includeChildren)
			{
				return this.clothing.GetComponent<T>();
			}
			return this.clothing.GetComponentInChildren<T>(true);
		}

		// Token: 0x06007B6A RID: 31594 RVA: 0x00326A48 File Offset: 0x00324C48
		private void BindEmoteController(GameplayEmoteController next)
		{
			if (this._subscribedEmotes == next)
			{
				return;
			}
			if (this._subscribedEmotes != null)
			{
				this._subscribedEmotes.OnEmoteStarted -= this.HandleEmoteStarted;
				this._subscribedEmotes.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._activeEventEmotes.Clear();
			this._subscribedEmotes = next;
			this.emotes = next;
			if (this._subscribedEmotes != null)
			{
				this._subscribedEmotes.OnEmoteStarted += this.HandleEmoteStarted;
				this._subscribedEmotes.OnEmoteEnded += this.HandleEmoteEnded;
			}
		}

		// Token: 0x06007B6B RID: 31595 RVA: 0x00326AF8 File Offset: 0x00324CF8
		private void BindClothingController(ClothingToggleController next)
		{
			if (this._subscribedClothing == next)
			{
				return;
			}
			if (this._subscribedClothing != null)
			{
				this._subscribedClothing.OnWardrobeHierarchyChanged -= this.HandleWardrobeHierarchyChanged;
			}
			this._subscribedClothing = next;
			this.clothing = next;
			if (this._subscribedClothing != null)
			{
				this._subscribedClothing.OnWardrobeHierarchyChanged += this.HandleWardrobeHierarchyChanged;
			}
			this._flashGatesDirty = true;
		}

		// Token: 0x06007B6C RID: 31596 RVA: 0x00326B73 File Offset: 0x00324D73
		private void HandleWardrobeHierarchyChanged()
		{
			this._flashGatesDirty = true;
		}

		// Token: 0x06007B6D RID: 31597 RVA: 0x00326B7C File Offset: 0x00324D7C
		private void RefreshFlashGatesIfDirty()
		{
			if (!this._flashGatesDirty)
			{
				return;
			}
			Transform transform = ((this._playerTransform != null) ? this._playerTransform.root : null);
			this._flashGates = ((transform != null) ? transform.GetComponentsInChildren<IFlashGate>(true) : Array.Empty<IFlashGate>());
			this._flashGatesDirty = false;
		}

		// Token: 0x06007B6E RID: 31598 RVA: 0x00326BD4 File Offset: 0x00324DD4
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return;
			}
			int num;
			this._activeEventEmotes.TryGetValue(emote, out num);
			this._activeEventEmotes[emote] = num + 1;
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x00326C0C File Offset: 0x00324E0C
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			int num;
			if (emote == null || !this._activeEventEmotes.TryGetValue(emote, out num))
			{
				return;
			}
			if (num <= 1)
			{
				this._activeEventEmotes.Remove(emote);
				return;
			}
			this._activeEventEmotes[emote] = num - 1;
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x00326C54 File Offset: 0x00324E54
		private bool TryGetEmoteState(out bool any, out bool strong)
		{
			any = false;
			strong = false;
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.IsResolutionActionPlaying)
			{
				any = true;
				strong |= AdaptiveHeartbeatDriver.IsStrongEmote(instance.ActiveResolutionEmote);
			}
			if (this.emotes != null && this.emotes.IsPlaying)
			{
				any = true;
				strong |= AdaptiveHeartbeatDriver.IsStrongEmote(this.emotes.PlayingEmote);
			}
			foreach (KeyValuePair<EmoteDefinitionSO, int> keyValuePair in this._activeEventEmotes)
			{
				if (!(keyValuePair.Key == null) && keyValuePair.Value > 0)
				{
					any = true;
					strong |= AdaptiveHeartbeatDriver.IsStrongEmote(keyValuePair.Key);
				}
			}
			return any;
		}

		// Token: 0x06007B71 RID: 31601 RVA: 0x00326D34 File Offset: 0x00324F34
		private static bool IsStrongEmote(EmoteDefinitionSO emote)
		{
			return !(emote == null) && (emote.IsMasturbationEmote || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.EmoteId, "stimulation") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.DisplayName, "stimulation") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.EmoteId, "masturb") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.DisplayName, "masturb") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.EmoteId, "grop") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.DisplayName, "grop") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.EmoteId, "jerk") || AdaptiveHeartbeatDriver.ContainsIgnoreCase(emote.DisplayName, "jerk"));
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		private static bool ContainsIgnoreCase(string value, string token)
		{
			return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06007B73 RID: 31603 RVA: 0x00326DE8 File Offset: 0x00324FE8
		private bool ResolveBodyPaintActive()
		{
			return (this.skinDecals != null && this.skinDecals.HasActiveDecalOfType(SkinDecalType.BodyPaint, 0.01f)) || (this.bodyPaintCanvas != null && this.bodyPaintCanvas.HasAnyPaint) || (this.tintBodyPaint != null && this.tintBodyPaint.IsTintBodyPaintActive()) || (this.includeMudAsBodyPaint && this.mudBodyPaint != null && this.mudBodyPaint.IsMudBodyPaintActive());
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x00326E78 File Offset: 0x00325078
		private bool AnyFlashEngaged()
		{
			for (int i = 0; i < this._flashGates.Length; i++)
			{
				if (FlashGateRegistry.IsFlashEngaged(this._flashGates[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x00326EAC File Offset: 0x003250AC
		private void RefreshNpcContext()
		{
			float num = ((this.playerEmotion != null) ? this.playerEmotion.HighestCurrentAttention : 0f);
			float num2 = float.PositiveInfinity;
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null)
			{
				if (this._playerTransform == null && instance.GetPlayer() != null)
				{
					this._playerTransform = instance.GetPlayer().transform;
				}
				IReadOnlyList<NPCSenses> registeredNPCs = instance.GetRegisteredNPCs();
				for (int i = 0; i < registeredNPCs.Count; i++)
				{
					NPCSenses npcsenses = registeredNPCs[i];
					if (!(npcsenses == null) && npcsenses.isActiveAndEnabled)
					{
						num = Mathf.Max(num, npcsenses.CurrentAttention);
						AttentionUIController attentionUI = npcsenses.AttentionUI;
						if (attentionUI != null && attentionUI.UISource != null)
						{
							num = Mathf.Max(num, attentionUI.UISource.DetectionLevel);
						}
						if (this._playerTransform != null)
						{
							float num3 = Vector3.Distance(this._playerTransform.position, npcsenses.EyePosition);
							if (num3 < num2)
							{
								num2 = num3;
							}
						}
					}
				}
			}
			this.CurrentNpcAttention01 = Mathf.Clamp01(num);
			this.CurrentNpcProximity01 = (float.IsPositiveInfinity(num2) ? 0f : Mathf.InverseLerp(Mathf.Max(this.npcFarDistance, this.npcNearDistance + 0.1f), this.npcNearDistance, num2));
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x00327010 File Offset: 0x00325210
		private void UpdateAttentionSpike(float now, bool eligible)
		{
			if (this.CurrentNpcAttention01 <= this.attentionSpikeRearmThreshold)
			{
				this._attentionSpikeArmed = true;
			}
			if (!eligible)
			{
				this._attentionSpikeActive = false;
				return;
			}
			if (!this._attentionSpikeArmed || this.CurrentNpcAttention01 < this.attentionSpikeThreshold)
			{
				return;
			}
			this._attentionSpikeArmed = false;
			this._attentionSpikeActive = true;
			this._attentionSpikeStartedAt = now;
		}

		// Token: 0x04007EB8 RID: 32440
		private const string DefaultSourceId = "adaptive";

		// Token: 0x04007EB9 RID: 32441
		[Header("References (auto-resolved)")]
		[SerializeField]
		private HeartbeatController heartbeat;

		// Token: 0x04007EBA RID: 32442
		[SerializeField]
		private ClothingToggleController clothing;

		// Token: 0x04007EBB RID: 32443
		[SerializeField]
		private PlayerEmotionController playerEmotion;

		// Token: 0x04007EBC RID: 32444
		[SerializeField]
		private GameplayEmoteController emotes;

		// Token: 0x04007EBD RID: 32445
		[SerializeField]
		private CoverPoseController coverPose;

		// Token: 0x04007EBE RID: 32446
		[SerializeField]
		private InsertableToyController insertableToy;

		// Token: 0x04007EBF RID: 32447
		[SerializeField]
		private SkinDecalController skinDecals;

		// Token: 0x04007EC0 RID: 32448
		[SerializeField]
		private BodyPaintCanvasService bodyPaintCanvas;

		// Token: 0x04007EC1 RID: 32449
		[SerializeField]
		private TintBodyPaintRuntimeController tintBodyPaint;

		// Token: 0x04007EC2 RID: 32450
		[SerializeField]
		private MudBodyPaintRuntimeController mudBodyPaint;

		// Token: 0x04007EC3 RID: 32451
		[SerializeField]
		private GymExerciseSweatController gymExercise;

		// Token: 0x04007EC4 RID: 32452
		[SerializeField]
		private CielRunningNude runningNude;

		// Token: 0x04007EC5 RID: 32453
		[SerializeField]
		private vThirdPersonController movement;

		// Token: 0x04007EC6 RID: 32454
		[SerializeField]
		private vSwimming swimming;

		// Token: 0x04007EC7 RID: 32455
		[SerializeField]
		private GropingModeController groping;

		// Token: 0x04007EC8 RID: 32456
		[SerializeField]
		private BoobTouchModeController boobTouch;

		// Token: 0x04007EC9 RID: 32457
		[Header("Baseline BPM ranges")]
		[SerializeField]
		private Vector2 fullyClothedBpmRange = new Vector2(72f, 78f);

		// Token: 0x04007ECA RID: 32458
		[SerializeField]
		private Vector2 nakedBpmRange = new Vector2(124f, 136f);

		// Token: 0x04007ECB RID: 32459
		[SerializeField]
		private Vector2 partiallyCoveredFloorRange = new Vector2(94f, 104f);

		// Token: 0x04007ECC RID: 32460
		[SerializeField]
		private Vector2 hiddenExposedBpmRange = new Vector2(94f, 103f);

		// Token: 0x04007ECD RID: 32461
		[SerializeField]
		private Vector2 bodyPaintBpmRange = new Vector2(116f, 124f);

		// Token: 0x04007ECE RID: 32462
		[Header("NPC reaction")]
		[Min(0.05f)]
		[SerializeField]
		private float npcSampleInterval = 0.2f;

		// Token: 0x04007ECF RID: 32463
		[Min(0f)]
		[SerializeField]
		private float npcNearDistance = 2.5f;

		// Token: 0x04007ED0 RID: 32464
		[Min(0.1f)]
		[SerializeField]
		private float npcFarDistance = 11f;

		// Token: 0x04007ED1 RID: 32465
		[Min(0f)]
		[SerializeField]
		private float npcProximityBoostBpm = 6f;

		// Token: 0x04007ED2 RID: 32466
		[Min(0f)]
		[SerializeField]
		private float npcAttentionBoostBpm = 12f;

		// Token: 0x04007ED3 RID: 32467
		[Range(0f, 1f)]
		[SerializeField]
		private float attentionSpikeThreshold = 0.5f;

		// Token: 0x04007ED4 RID: 32468
		[Range(0f, 1f)]
		[SerializeField]
		private float attentionSpikeRearmThreshold = 0.32f;

		// Token: 0x04007ED5 RID: 32469
		[Min(0.1f)]
		[SerializeField]
		private float attentionSpikeDuration = 2.2f;

		// Token: 0x04007ED6 RID: 32470
		[Min(20f)]
		[SerializeField]
		private float attentionSpikePeakBpm = 158f;

		// Token: 0x04007ED7 RID: 32471
		[Min(20f)]
		[SerializeField]
		private float attentionSpikeSettledBpm = 142f;

		// Token: 0x04007ED8 RID: 32472
		[Range(0f, 1f)]
		[SerializeField]
		private float postSpikeAttentionCap = 0.5f;

		// Token: 0x04007ED9 RID: 32473
		[Range(0f, 1f)]
		[SerializeField]
		private float postSpikeProximityMultiplier = 0.5f;

		// Token: 0x04007EDA RID: 32474
		[Header("Activity modifiers")]
		[SerializeField]
		private float normalEmoteBoostBpm = 9f;

		// Token: 0x04007EDB RID: 32475
		[SerializeField]
		private float coverPoseReductionBpm = 7f;

		// Token: 0x04007EDC RID: 32476
		[SerializeField]
		private float totalExposeBoostBpm = 12f;

		// Token: 0x04007EDD RID: 32477
		[SerializeField]
		private float flashBoostBpm = 9f;

		// Token: 0x04007EDE RID: 32478
		[SerializeField]
		private float caughtEventBoostBpm = 5f;

		// Token: 0x04007EDF RID: 32479
		[SerializeField]
		private float sprintBoostBpm = 8f;

		// Token: 0x04007EE0 RID: 32480
		[SerializeField]
		private float movementBoostBpm = 2f;

		// Token: 0x04007EE1 RID: 32481
		[SerializeField]
		private float supremeEmotionMaxBoostBpm = 5f;

		// Token: 0x04007EE2 RID: 32482
		[SerializeField]
		private float masturbationEmoteFloorBpm = 150f;

		// Token: 0x04007EE3 RID: 32483
		[SerializeField]
		private float stimulationPoseFloorBpm = 150f;

		// Token: 0x04007EE4 RID: 32484
		[SerializeField]
		private float exerciseFloorBpm = 138f;

		// Token: 0x04007EE5 RID: 32485
		[SerializeField]
		private float swimmingFloorBpm = 140f;

		// Token: 0x04007EE6 RID: 32486
		[SerializeField]
		private float gropingFloorBpm = 147f;

		// Token: 0x04007EE7 RID: 32487
		[SerializeField]
		private float insertionFloorBpm = 155f;

		// Token: 0x04007EE8 RID: 32488
		[SerializeField]
		private float hackFloorBpm = 156f;

		// Token: 0x04007EE9 RID: 32489
		[SerializeField]
		private float adaptiveMaximumBpm = 158f;

		// Token: 0x04007EEA RID: 32490
		[Header("Source")]
		[SerializeField]
		private string sourceId = "adaptive";

		// Token: 0x04007EEB RID: 32491
		[SerializeField]
		private bool includeMudAsBodyPaint = true;

		// Token: 0x04007EEC RID: 32492
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007EED RID: 32493
		private readonly Dictionary<EmoteDefinitionSO, int> _activeEventEmotes = new Dictionary<EmoteDefinitionSO, int>();

		// Token: 0x04007EEE RID: 32494
		private GameplayEmoteController _subscribedEmotes;

		// Token: 0x04007EEF RID: 32495
		private ClothingToggleController _subscribedClothing;

		// Token: 0x04007EF0 RID: 32496
		private IFlashGate[] _flashGates = Array.Empty<IFlashGate>();

		// Token: 0x04007EF1 RID: 32497
		private bool _flashGatesDirty = true;

		// Token: 0x04007EF2 RID: 32498
		private Transform _playerTransform;

		// Token: 0x04007EF3 RID: 32499
		private float _variationPhase;

		// Token: 0x04007EF4 RID: 32500
		private float _nextResolveTime;

		// Token: 0x04007EF5 RID: 32501
		private float _nextNpcSampleTime;

		// Token: 0x04007EF6 RID: 32502
		private bool _attentionSpikeArmed = true;

		// Token: 0x04007EF7 RID: 32503
		private bool _attentionSpikeActive;

		// Token: 0x04007EF8 RID: 32504
		private float _attentionSpikeStartedAt;
	}
}
