using System;
using System.Collections.Generic;
using ExhibitionistDemo.Movement;
using Features.BodyPhysics;
using Features.Detection;
using Features.Inventory.Actions;
using Features.MainMenu;
using Features.MasturbationSystem;
using Features.NippleRotor;
using Features.PlayerEmotes;
using Features.Police;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001221 RID: 4641
	[DefaultExecutionOrder(20500)]
	[DisallowMultipleComponent]
	public sealed class EggRotorClimax : MonoBehaviour
	{
		// Token: 0x140000FA RID: 250
		// (add) Token: 0x06008F7E RID: 36734 RVA: 0x003AAD24 File Offset: 0x003A8F24
		// (remove) Token: 0x06008F7F RID: 36735 RVA: 0x003AAD58 File Offset: 0x003A8F58
		public static event Action ClimaxStarted;

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x06008F80 RID: 36736 RVA: 0x003AAD8C File Offset: 0x003A8F8C
		// (remove) Token: 0x06008F81 RID: 36737 RVA: 0x003AADC0 File Offset: 0x003A8FC0
		public static event Action ClimaxEnded;

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x06008F82 RID: 36738 RVA: 0x003AADF3 File Offset: 0x003A8FF3
		public bool IsClimaxRunning
		{
			get
			{
				return this._running;
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x06008F83 RID: 36739 RVA: 0x003AADFB File Offset: 0x003A8FFB
		public float ClimaxTime
		{
			get
			{
				if (!this._running)
				{
					return 0f;
				}
				return this._time;
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06008F84 RID: 36740 RVA: 0x003AAE11 File Offset: 0x003A9011
		public bool IsRefractory
		{
			get
			{
				return this._refractory.Active;
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06008F85 RID: 36741 RVA: 0x003AAE1E File Offset: 0x003A901E
		public float RefractoryRemaining
		{
			get
			{
				return this._refractory.Remaining;
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06008F86 RID: 36742 RVA: 0x003AAE2B File Offset: 0x003A902B
		public bool HoldsPlayerInPlace
		{
			get
			{
				return this._inputLock.Owned;
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06008F87 RID: 36743 RVA: 0x003AAE38 File Offset: 0x003A9038
		public bool HoldsEmote
		{
			get
			{
				return this._emoteHeld;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06008F88 RID: 36744 RVA: 0x003AAE40 File Offset: 0x003A9040
		public EggRotorTrembleWeights TrembleWeights
		{
			get
			{
				return this._weights;
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x06008F89 RID: 36745 RVA: 0x003AAE48 File Offset: 0x003A9048
		public GameObject Squirt
		{
			get
			{
				return this._squirt;
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x06008F8A RID: 36746 RVA: 0x003AAE50 File Offset: 0x003A9050
		public GameObject Cum
		{
			get
			{
				return this._cum;
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x06008F8B RID: 36747 RVA: 0x003AAE58 File Offset: 0x003A9058
		public bool IsFutanariClimax
		{
			get
			{
				return this._running && this._futaClimax;
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06008F8C RID: 36748 RVA: 0x003AAE6C File Offset: 0x003A906C
		private EggRotorClimaxSettings Settings
		{
			get
			{
				EggRotorConfigSO eggRotorConfigSO = ((this._controller != null) ? this._controller.Config : null);
				if (eggRotorConfigSO != null && eggRotorConfigSO.climax != null)
				{
					return eggRotorConfigSO.climax;
				}
				EggRotorClimaxSettings eggRotorClimaxSettings;
				if ((eggRotorClimaxSettings = this._fallbackSettings) == null)
				{
					eggRotorClimaxSettings = (this._fallbackSettings = new EggRotorClimaxSettings());
				}
				return eggRotorClimaxSettings;
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06008F8D RID: 36749 RVA: 0x003AAEC8 File Offset: 0x003A90C8
		private string LeaseReason
		{
			get
			{
				string text;
				if ((text = this._leaseReason) == null)
				{
					text = (this._leaseReason = "EggRotor.Climax#" + base.GetInstanceID().ToString());
				}
				return text;
			}
		}

		// Token: 0x06008F8E RID: 36750 RVA: 0x003AAF00 File Offset: 0x003A9100
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			EggRotorClimax._foundSquirtPrefab = null;
			EggRotorClimax._nextSquirtSearch = 0f;
			EggRotorClimax._warnedSquirt = false;
			EggRotorClimax._foundCumPrefab = null;
			EggRotorClimax._nextCumSearch = 0f;
			EggRotorClimax._warnedCum = false;
			EggRotorClimax.ClimaxStarted = null;
			EggRotorClimax.ClimaxEnded = null;
		}

		// Token: 0x06008F8F RID: 36751 RVA: 0x003AAF3C File Offset: 0x003A913C
		[ContextMenu("Test climax now")]
		public void TestClimaxNow()
		{
			if (!Application.isPlaying || this._running)
			{
				return;
			}
			if (this._controller == null)
			{
				this._controller = base.GetComponent<EggRotorController>();
			}
			if (this._controller == null || this._controller.PlayerRoot == null)
			{
				return;
			}
			this.ResolvePlayerSystems(true);
			this.BindMeter(OrgasmMeterController.Instance);
			this.StartClimax(this.Settings);
		}

		// Token: 0x06008F90 RID: 36752 RVA: 0x003AAFB3 File Offset: 0x003A91B3
		[ContextMenu("Clear refractory")]
		public void ClearRefractory()
		{
			this._refractory.Clear();
		}

		// Token: 0x06008F91 RID: 36753 RVA: 0x003AAFC0 File Offset: 0x003A91C0
		private void Awake()
		{
			this._maxReachedHandler = new Action(this.HandleMaxReached);
			this._gateChangedHandler = new Action<PlayerActivity>(this.HandleActivityGateChanged);
			this._controller = base.GetComponent<EggRotorController>();
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x003AAFF2 File Offset: 0x003A91F2
		private void OnEnable()
		{
			this._resolveTimer = 0f;
		}

		// Token: 0x06008F93 RID: 36755 RVA: 0x003AAFFF File Offset: 0x003A91FF
		private void OnDisable()
		{
			this.Shutdown();
		}

		// Token: 0x06008F94 RID: 36756 RVA: 0x003AAFFF File Offset: 0x003A91FF
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x06008F95 RID: 36757 RVA: 0x003AB008 File Offset: 0x003A9208
		private void Update()
		{
			if (this._controller == null)
			{
				this._controller = base.GetComponent<EggRotorController>();
			}
			if (this._controller == null || this._controller.Config == null || this._controller.PlayerRoot == null)
			{
				if (this._running)
				{
					this.EndClimax(EggRotorClimax.EndReason.Cancelled);
				}
				return;
			}
			EggRotorClimaxSettings settings = this.Settings;
			float deltaTime = Time.deltaTime;
			this.ResolvePlayerSystems(false);
			this.BindMeter(OrgasmMeterController.Instance);
			this.TickInputRepair(Time.unscaledDeltaTime);
			if (this._running)
			{
				this.TickClimax(settings, deltaTime);
			}
			else
			{
				bool flag = this._meter != null && (this._meter.IsLocked || this.IsForeignDriverActive());
				this._refractory.Tick(flag ? 0f : deltaTime, (this._meter != null) ? this._meter.Value : 0f, settings.resumeBelowValue);
				this.Feed(settings, deltaTime);
			}
			this.TickVoice(deltaTime);
			this.TickNoise(deltaTime);
			this.TickCum(deltaTime);
			this._controller.HoldsClimaxFace = this._running;
		}

		// Token: 0x06008F96 RID: 36758 RVA: 0x003AB144 File Offset: 0x003A9344
		private void LateUpdate()
		{
			if (!this._running || !this._tremble.IsBound || !this._weights.Any)
			{
				return;
			}
			EggRotorTremblePose eggRotorTremblePose = EggRotorClimaxMath.EvaluatePose(this._time, this.Settings, this._weights, this._seed);
			this._tremble.Apply(in eggRotorTremblePose);
		}

		// Token: 0x06008F97 RID: 36759 RVA: 0x003AB1A0 File Offset: 0x003A93A0
		private void Shutdown()
		{
			if (this._running)
			{
				this.EndClimax((this._controller != null && this._controller.IsInterrupted) ? EggRotorClimax.EndReason.Abandoned : EggRotorClimax.EndReason.Cancelled);
			}
			this.ReleaseEmoteHold();
			this._inputLock.Forget();
			this.HookGate(false);
			this.DisposeLease();
			this.CloseVoice();
			this.StopNoise();
			this.StopSquirt();
			this.StopCum();
			this.BindMeter(null);
			this._tremble.Unbind();
			if (this._controller != null)
			{
				this._controller.HoldsClimaxFace = false;
			}
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x003AB23C File Offset: 0x003A943C
		private void BindMeter(OrgasmMeterController meter)
		{
			if (meter != null && !base.isActiveAndEnabled)
			{
				meter = null;
			}
			if (meter == this._meter)
			{
				return;
			}
			if (this._maxReachedHandler == null)
			{
				this._maxReachedHandler = new Action(this.HandleMaxReached);
			}
			if (this._meter != null)
			{
				this._meter.OnMaxReached -= this._maxReachedHandler;
			}
			this._meter = meter;
			if (this._meter != null)
			{
				this._meter.OnMaxReached += this._maxReachedHandler;
			}
		}

		// Token: 0x06008F99 RID: 36761 RVA: 0x003AB2C0 File Offset: 0x003A94C0
		private void Feed(EggRotorClimaxSettings settings, float deltaTime)
		{
			if (!settings.fillsOrgasmMeter || this._meter == null || !this._meter.isActiveAndEnabled || deltaTime <= 0f)
			{
				return;
			}
			if (!EggRotorClimaxMath.ShouldFeed(this._controller.IsStimulating, this._meter.IsLocked, this.IsForeignDriverActive(), this._refractory.Active, this._meter.Value))
			{
				return;
			}
			NippleRotorController nippleRotorController = NippleRotorController.Current;
			bool flag = nippleRotorController != null && nippleRotorController.IsVibrating;
			float num = EggRotorClimaxMath.FillRate(settings, this._controller.Level, this._controller.PhysicalPlacement, flag, flag ? nippleRotorController.Level : 0);
			if (num <= 0f)
			{
				return;
			}
			if (this._controller.IsOnGlans)
			{
				if (settings.cum)
				{
					Vector3 vector;
					Vector3 vector2;
					Vector3 vector3;
					EggRotorClimax.ResolveCumPrefab(settings, out vector, out vector2, out vector3);
				}
			}
			else if (settings.squirt)
			{
				EggRotorClimax.ResolveSquirtPrefab(settings);
			}
			if (this._feed.Feed(this._meter, num * deltaTime))
			{
				this.StartClimax(settings);
			}
		}

		// Token: 0x06008F9A RID: 36762 RVA: 0x003AB3CF File Offset: 0x003A95CF
		private void HandleMaxReached()
		{
			if (this._feed.NotifyMaxReached() || this._running)
			{
				return;
			}
			this._refractory.Begin(this.Settings.refractorySeconds);
		}

		// Token: 0x06008F9B RID: 36763 RVA: 0x003AB400 File Offset: 0x003A9600
		private bool IsForeignDriverActive()
		{
			if (this._speed != null && (this._speed.IsActive || this._speed.ExternalDriverActive))
			{
				return true;
			}
			if (this._dildo != null && this._dildo.IsExecuting)
			{
				return true;
			}
			if (this._emotes != null && this._emotes.IsPlaying)
			{
				EmoteDefinitionSO playingEmote = this._emotes.PlayingEmote;
				if (playingEmote != null && playingEmote.IsMasturbationEmote)
				{
					return true;
				}
			}
			return !this._voiceOpen && MasturbationVoiceController.IsMasturbationVoiceActive;
		}

		// Token: 0x06008F9C RID: 36764 RVA: 0x003AB4A0 File Offset: 0x003A96A0
		private void StartClimax(EggRotorClimaxSettings settings)
		{
			if (this._running)
			{
				return;
			}
			this.ResolvePlayerSystems(true);
			this._running = true;
			this._abortRequested = false;
			this._gateBaseline = PlayerActivityGate.BlockedMask & ~(PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose);
			this._time = 0f;
			this._nextPulse = 0;
			this._pulseCount = EggRotorClimaxMath.FillPulseTimes(settings, this._pulseTimes);
			this._squirted = false;
			this._futaClimax = this._controller.IsOnGlans;
			this._seed = Random.Range(0f, 100f);
			bool flag = this.CanTremble();
			this._emoteHeld = flag && settings.holdEmotes && this.TryHoldEmote();
			if (this._emoteHeld)
			{
				this._tremble.Bind(this._controller.PlayerAnimator);
				EggRotorPoseSample eggRotorPoseSample;
				this._weights = (this._tremble.TrySample(out eggRotorPoseSample) ? EggRotorClimaxMath.EmoteWeights(in eggRotorPoseSample, settings.emoteTrembleWeight) : default(EggRotorTrembleWeights));
			}
			else
			{
				bool flag2 = flag && settings.stopInPlace && this.TryStopInPlace();
				float num = (flag2 ? 1f : 0f);
				float num2 = ((!flag) ? 0f : (flag2 ? 1f : Mathf.Clamp01(settings.reducedUpperBodyWeight)));
				this._weights = EggRotorTrembleWeights.Split(num, num2);
				if (this._weights.Any)
				{
					this._tremble.Bind(this._controller.PlayerAnimator);
				}
			}
			this.OpenVoice(settings);
			this.StartNoise(settings);
			this._controller.HoldsClimaxFace = true;
			Action climaxStarted = EggRotorClimax.ClimaxStarted;
			if (climaxStarted == null)
			{
				return;
			}
			climaxStarted();
		}

		// Token: 0x06008F9D RID: 36765 RVA: 0x003AB640 File Offset: 0x003A9840
		private bool CanTremble()
		{
			Animator playerAnimator = this._controller.PlayerAnimator;
			return !(playerAnimator == null) && playerAnimator.isHuman && (!(this._input != null) || !(this._input.cc != null) || !this._input.cc.ragdolled) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider | PlayerContextResource.WaterMovement);
		}

		// Token: 0x06008F9E RID: 36766 RVA: 0x003AB6B0 File Offset: 0x003A98B0
		private bool TryStopInPlace()
		{
			if (this._input == null || this._input.cc == null)
			{
				return false;
			}
			vThirdPersonController cc = this._input.cc;
			if (this._input.lockInput || cc.ragdolled || !cc.isGrounded || cc.isJumping || cc.isRolling || cc.customAction || cc.isCrouching)
			{
				return false;
			}
			if (this._genericAction != null && this._genericAction.doingAction)
			{
				return false;
			}
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			if (instance != null && instance.IsStunned)
			{
				return false;
			}
			if (this._emotes != null && this._emotes.CurrentState != GameplayEmoteController.EmoteState.Idle)
			{
				return false;
			}
			if (!PlayerActivityGate.TryAcquire(this.LeaseReason, this, PlayerActivity.None, PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, out this._lease, false, null))
			{
				this._lease = null;
				return false;
			}
			if (!this._inputLock.TryTake(this._input.lockInput))
			{
				this.DisposeLease();
				return false;
			}
			AutoLocomotionController component = this._input.GetComponent<AutoLocomotionController>();
			if (component != null)
			{
				component.CancelAutoLocomotion();
			}
			this._input.SetLockAllInput(true);
			this.KeepStill();
			this.HookGate(true);
			return true;
		}

		// Token: 0x06008F9F RID: 36767 RVA: 0x003AB7FC File Offset: 0x003A99FC
		private bool TryHoldEmote()
		{
			GameplayEmoteController emotes = this._emotes;
			if (emotes == null || !emotes.isActiveAndEnabled || !emotes.IsPlaying || emotes.IsEnding)
			{
				return false;
			}
			EmoteDefinitionSO playingEmote = emotes.PlayingEmote;
			if (playingEmote == null || playingEmote.IsMasturbationEmote)
			{
				return false;
			}
			if (!emotes.TryHoldPlayback(this))
			{
				return false;
			}
			this._heldEmotes = emotes;
			this.HookGate(true);
			return true;
		}

		// Token: 0x06008FA0 RID: 36768 RVA: 0x003AB868 File Offset: 0x003A9A68
		private void ReleaseEmoteHold()
		{
			GameplayEmoteController heldEmotes = this._heldEmotes;
			this._heldEmotes = null;
			this._emoteHeld = false;
			if (heldEmotes != null)
			{
				heldEmotes.ReleasePlaybackHold(this);
			}
		}

		// Token: 0x06008FA1 RID: 36769 RVA: 0x003AB89C File Offset: 0x003A9A9C
		private void KeepStill()
		{
			if (this._input == null || this._input.cc == null)
			{
				return;
			}
			this._input.cc.input = Vector3.zero;
			this._input.cc.isSprinting = false;
		}

		// Token: 0x06008FA2 RID: 36770 RVA: 0x003AB8F4 File Offset: 0x003A9AF4
		private void TickClimax(EggRotorClimaxSettings settings, float deltaTime)
		{
			EggRotorClimax.EndReason endReason;
			if (this.TryGetAbortReason(out endReason))
			{
				this.EndClimax(endReason);
				return;
			}
			if (this._inputLock.Owned)
			{
				this._inputLock.Observe(this._input != null && this._input.lockInput);
				if (this._inputLock.Owned)
				{
					this.KeepStill();
				}
				else
				{
					this.HookGate(false);
				}
			}
			if (this._emoteHeld && (this._heldEmotes == null || !this._heldEmotes.IsPlaybackHeldBy(this)))
			{
				this._heldEmotes = null;
				this._emoteHeld = false;
				if (!this._inputLock.Owned)
				{
					this.HookGate(false);
				}
			}
			if (!this._inputLock.Owned && !this._emoteHeld && (this._weights.Legs > 0f || this._weights.Pelvis > 0f) && deltaTime > 0f)
			{
				this._weights.ReleaseLowerBody(4f * deltaTime);
			}
			if (deltaTime > 0f)
			{
				this._time += deltaTime;
			}
			while (this._nextPulse < this._pulseCount && this._time >= this._pulseTimes[this._nextPulse])
			{
				this.Shake(settings, this._nextPulse);
				this._nextPulse++;
			}
			if (!this._squirted && this._time >= (this._futaClimax ? settings.cumDelaySeconds : settings.squirtDelaySeconds))
			{
				this._squirted = true;
				if (this._futaClimax)
				{
					this.FireCum(settings);
				}
				else
				{
					this.FireSquirt(settings);
				}
			}
			if (this._time >= EggRotorClimaxMath.Duration(settings))
			{
				this.EndClimax(EggRotorClimax.EndReason.Completed);
			}
		}

		// Token: 0x06008FA3 RID: 36771 RVA: 0x003ABAAC File Offset: 0x003A9CAC
		private bool TryGetAbortReason(out EggRotorClimax.EndReason reason)
		{
			reason = EggRotorClimax.EndReason.Cancelled;
			if (DetectionInteractionGate.IsGameOver)
			{
				reason = EggRotorClimax.EndReason.Abandoned;
				return true;
			}
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			if (this._abortRequested || this._controller.IsInterrupted || (instance != null && instance.IsStunned) || this.IsForeignDriverActive() || (this._input != null && this._input.cc != null && this._input.cc.ragdolled))
			{
				reason = EggRotorClimax.EndReason.Interrupted;
				return true;
			}
			return !this._controller.isActiveAndEnabled || !this._controller.IsWorn;
		}

		// Token: 0x06008FA4 RID: 36772 RVA: 0x003ABB54 File Offset: 0x003A9D54
		private void EndClimax(EggRotorClimax.EndReason reason)
		{
			if (!this._running)
			{
				return;
			}
			this._running = false;
			this._abortRequested = false;
			this._tremble.Restore();
			this.ReleaseEmoteHold();
			this.ReleaseInputLock(reason);
			this.DisposeLease();
			if (this._meter != null && this._meter.IsLocked)
			{
				if (this.IsForeignDriverActive())
				{
					this._meter.Reset();
				}
				else
				{
					this._meter.UnlockAndCooldown();
				}
			}
			this._refractory.Begin(this.Settings.refractorySeconds);
			this.CloseVoice();
			this.StopSquirt();
			if (reason != EggRotorClimax.EndReason.Completed)
			{
				this.StopCum();
			}
			if (reason != EggRotorClimax.EndReason.Completed)
			{
				this.StopNoise();
			}
			if (this._controller != null)
			{
				this._controller.HoldsClimaxFace = false;
			}
			Action climaxEnded = EggRotorClimax.ClimaxEnded;
			if (climaxEnded == null)
			{
				return;
			}
			climaxEnded();
		}

		// Token: 0x06008FA5 RID: 36773 RVA: 0x003ABC30 File Offset: 0x003A9E30
		private void Shake(EggRotorClimaxSettings settings, int index)
		{
			float num = EggRotorClimaxMath.PulseAmplitude(settings, index);
			if (this._futaClimax)
			{
				this._controller.TryPulsePenis(settings.penisPulseVelocity * num);
			}
			float num2 = Mathf.Clamp01(settings.pulseShake * num);
			Transform playerRoot = this._controller.PlayerRoot;
			if (num2 <= 0f || playerRoot == null)
			{
				return;
			}
			AnimeBodyPhysics.TryShake(playerRoot, Vector3.up, index % 2 == 1, num2);
		}

		// Token: 0x06008FA6 RID: 36774 RVA: 0x003ABCA0 File Offset: 0x003A9EA0
		private void ReleaseInputLock(EggRotorClimax.EndReason reason)
		{
			this.HookGate(false);
			bool flag = this._input != null && this._input.lockInput;
			if (reason > EggRotorClimax.EndReason.Cancelled)
			{
				if (reason == EggRotorClimax.EndReason.Interrupted)
				{
					this._inputLock.Orphan(flag);
					return;
				}
				this._inputLock.Forget();
			}
			else if (this._inputLock.Release(flag))
			{
				this._input.SetLockAllInput(false);
				return;
			}
		}

		// Token: 0x06008FA7 RID: 36775 RVA: 0x003ABD10 File Offset: 0x003A9F10
		private void HookGate(bool hooked)
		{
			if (hooked == this._gateHooked)
			{
				return;
			}
			if (this._gateChangedHandler == null)
			{
				this._gateChangedHandler = new Action<PlayerActivity>(this.HandleActivityGateChanged);
			}
			if (hooked)
			{
				PlayerActivityGate.Changed += this._gateChangedHandler;
			}
			else
			{
				PlayerActivityGate.Changed -= this._gateChangedHandler;
			}
			this._gateHooked = hooked;
		}

		// Token: 0x06008FA8 RID: 36776 RVA: 0x003ABD64 File Offset: 0x003A9F64
		private void HandleActivityGateChanged(PlayerActivity blocked)
		{
			bool flag = this._inputLock.Owned && this._input != null;
			if (!this._running || (!flag && !this._emoteHeld))
			{
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			bool flag2 = instance != null && instance.isActiveAndEnabled && instance.IsEventActive;
			bool flag3 = (blocked & ~(PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose) & ~this._gateBaseline) > PlayerActivity.None;
			if (!flag2 && !flag3 && !GameplayInputGate.IsLoading)
			{
				return;
			}
			if (flag && this._inputLock.Release(this._input.lockInput))
			{
				this._input.SetLockAllInput(false);
			}
			this.ReleaseEmoteHold();
			this._abortRequested = true;
		}

		// Token: 0x06008FA9 RID: 36777 RVA: 0x003ABE18 File Offset: 0x003AA018
		private void TickInputRepair(float unscaledDeltaTime)
		{
			if (!this._inputLock.RepairPending)
			{
				return;
			}
			if (this._input == null || DetectionInteractionGate.IsGameOver)
			{
				this._inputLock.Forget();
				return;
			}
			if (!this._inputLock.TickRepair(unscaledDeltaTime, this._input.lockInput, this.OthersMayOwnInput(), 1f, 180f))
			{
				return;
			}
			this._input.SetLockAllInput(false);
			Debug.Log("[EggRotor] Released the input lock an interrupted climax left behind.", this);
		}

		// Token: 0x06008FAA RID: 36778 RVA: 0x003ABE98 File Offset: 0x003AA098
		private bool OthersMayOwnInput()
		{
			if (Time.timeScale <= 0f || this._controller.IsInterrupted)
			{
				return true;
			}
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			return (instance != null && instance.IsStunned) || DetectionRecoveryGate.IsPlayerRecovering || (this._genericAction != null && this._genericAction.doingAction) || (this._emotes != null && this._emotes.CurrentState != GameplayEmoteController.EmoteState.Idle) || PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.Rider);
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x003ABF23 File Offset: 0x003AA123
		private void DisposeLease()
		{
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
		}

		// Token: 0x06008FAC RID: 36780 RVA: 0x003ABF40 File Offset: 0x003AA140
		private void OpenVoice(EggRotorClimaxSettings settings)
		{
			if (!settings.playVoice || this._voiceOpen || this._voice == null || settings.voiceHoldSeconds <= 0f)
			{
				return;
			}
			if (!this._voice.TryBeginExternalSession(this))
			{
				return;
			}
			this._voiceOpen = true;
			this._voiceTimer = settings.voiceHoldSeconds;
			this._voice.PlayExternalEnding(this);
		}

		// Token: 0x06008FAD RID: 36781 RVA: 0x003ABFA8 File Offset: 0x003AA1A8
		private void TickVoice(float deltaTime)
		{
			if (!this._voiceOpen)
			{
				return;
			}
			if (this._voice == null)
			{
				this._voiceOpen = false;
				return;
			}
			if (deltaTime > 0f)
			{
				this._voiceTimer -= deltaTime;
			}
			if (this._voiceTimer <= 0f || (!this._running && this.IsForeignDriverActive()))
			{
				this.CloseVoice();
			}
		}

		// Token: 0x06008FAE RID: 36782 RVA: 0x003AC00D File Offset: 0x003AA20D
		private void CloseVoice()
		{
			if (this._voiceOpen && this._voice != null)
			{
				this._voice.EndExternalSession(this);
			}
			this._voiceOpen = false;
		}

		// Token: 0x06008FAF RID: 36783 RVA: 0x003AC038 File Offset: 0x003AA238
		private void StartNoise(EggRotorClimaxSettings settings)
		{
			if (this._actor == null || settings.noiseRadius <= 0f || settings.noiseSeconds <= 0f)
			{
				return;
			}
			this._actor.SetExternalNoise(this, settings.noiseRadius);
			this._noiseActive = true;
			this._noiseTimer = settings.noiseSeconds;
		}

		// Token: 0x06008FB0 RID: 36784 RVA: 0x003AC093 File Offset: 0x003AA293
		private void TickNoise(float deltaTime)
		{
			if (!this._noiseActive)
			{
				return;
			}
			if (deltaTime > 0f)
			{
				this._noiseTimer -= deltaTime;
			}
			if (this._noiseTimer <= 0f)
			{
				this.StopNoise();
			}
		}

		// Token: 0x06008FB1 RID: 36785 RVA: 0x003AC0C7 File Offset: 0x003AA2C7
		private void StopNoise()
		{
			if (this._noiseActive && this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
			this._noiseActive = false;
		}

		// Token: 0x06008FB2 RID: 36786 RVA: 0x003AC0F8 File Offset: 0x003AA2F8
		private void FireSquirt(EggRotorClimaxSettings settings)
		{
			if (!settings.squirt || this._controller.IsCovered)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.WaterMovement))
			{
				return;
			}
			Transform anchor = this._controller.Anchor;
			GameObject gameObject = EggRotorClimax.ResolveSquirtPrefab(settings);
			if (anchor == null || gameObject == null)
			{
				return;
			}
			this.StopSquirt();
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, anchor, false);
			gameObject2.name = "EggRotorSquirt";
			Transform transform = gameObject2.transform;
			transform.localPosition = EggRotorPlacementMath.BodyToLocalPosition(anchor, settings.squirtPosition, this._controller.BodyScale);
			transform.localRotation = EggRotorClimaxMath.SquirtRotation(settings.squirtDirection);
			if (!gameObject2.activeSelf)
			{
				gameObject2.SetActive(true);
			}
			gameObject2.GetComponentsInChildren<ParticleSystem>(true, this._particles);
			for (int i = 0; i < this._particles.Count; i++)
			{
				ParticleSystem particleSystem = this._particles[i];
				if (!(particleSystem == null))
				{
					particleSystem.main.stopAction = ParticleSystemStopAction.Destroy;
					particleSystem.Clear(false);
					particleSystem.Play(false);
				}
			}
			this._particles.Clear();
			Object.Destroy(gameObject2, 15f);
			this._squirt = gameObject2;
		}

		// Token: 0x06008FB3 RID: 36787 RVA: 0x003AC224 File Offset: 0x003AA424
		private void StopSquirt()
		{
			if (this._squirt != null)
			{
				this._squirt.GetComponentsInChildren<ParticleSystem>(true, this._particles);
				for (int i = 0; i < this._particles.Count; i++)
				{
					if (this._particles[i] != null)
					{
						this._particles[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
					}
				}
				this._particles.Clear();
			}
			this._squirt = null;
		}

		// Token: 0x06008FB4 RID: 36788 RVA: 0x003AC2A0 File Offset: 0x003AA4A0
		private static GameObject ResolveSquirtPrefab(EggRotorClimaxSettings settings)
		{
			if (settings.squirtPrefab != null)
			{
				return settings.squirtPrefab;
			}
			if (EggRotorClimax._foundSquirtPrefab != null)
			{
				return EggRotorClimax._foundSquirtPrefab;
			}
			if (Time.unscaledTime < EggRotorClimax._nextSquirtSearch)
			{
				return null;
			}
			EggRotorClimax._nextSquirtSearch = Time.unscaledTime + 10f;
			EggRotorClimax._foundSquirtPrefab = EggRotorClimax.FindToySquirtPrefab();
			if (EggRotorClimax._foundSquirtPrefab == null && !EggRotorClimax._warnedSquirt)
			{
				EggRotorClimax._warnedSquirt = true;
				Debug.LogWarning("[EggRotor] No \"Squirt\" effect found on the loaded masturbation proxies; the climax plays without the squirt. Assign climax.squirtPrefab on the config (SmallDrops).");
			}
			return EggRotorClimax._foundSquirtPrefab;
		}

		// Token: 0x06008FB5 RID: 36789 RVA: 0x003AC328 File Offset: 0x003AA528
		public static GameObject FindToySquirtPrefab()
		{
			GameObject gameObject = null;
			EmoteDefinitionSO[] array = Resources.FindObjectsOfTypeAll<EmoteDefinitionSO>();
			for (int i = 0; i < array.Length; i++)
			{
				List<EmotePropBinding> list = ((array[i] != null) ? array[i].PropBindings : null);
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						EmotePropBinding emotePropBinding = list[j];
						if (emotePropBinding != null && !(emotePropBinding.EffectPrefab == null) && emotePropBinding.Label != null && string.Equals(emotePropBinding.Label.Trim(), "Squirt", StringComparison.OrdinalIgnoreCase))
						{
							if (emotePropBinding.EffectPrefab.name == "SmallDrops")
							{
								return emotePropBinding.EffectPrefab;
							}
							if (gameObject == null)
							{
								gameObject = emotePropBinding.EffectPrefab;
							}
						}
					}
				}
			}
			return gameObject;
		}

		// Token: 0x06008FB6 RID: 36790 RVA: 0x003AC3F4 File Offset: 0x003AA5F4
		private void FireCum(EggRotorClimaxSettings settings)
		{
			if (!settings.cum || this._controller.IsCovered || !this._controller.IsPenisVisible)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.WaterMovement))
			{
				return;
			}
			Transform penisTip = this._controller.PenisTip;
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			GameObject gameObject = EggRotorClimax.ResolveCumPrefab(settings, out vector, out vector2, out vector3);
			if (penisTip == null || gameObject == null)
			{
				return;
			}
			this.StopCum();
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, penisTip, false);
			gameObject2.name = "EggRotorCum";
			Transform transform = gameObject2.transform;
			transform.localPosition = vector;
			transform.localRotation = Quaternion.Euler(vector2);
			if (vector3.sqrMagnitude >= 1E-06f)
			{
				transform.localScale = vector3;
			}
			if (!gameObject2.activeSelf)
			{
				gameObject2.SetActive(true);
			}
			EggRotorCumFollower eggRotorCumFollower = gameObject2.AddComponent<EggRotorCumFollower>();
			if (!eggRotorCumFollower.Bind(this._controller))
			{
				Object.Destroy(eggRotorCumFollower);
			}
			gameObject2.GetComponentsInChildren<ParticleSystem>(true, this._particles);
			for (int i = 0; i < this._particles.Count; i++)
			{
				if (this._particles[i] != null)
				{
					this._particles[i].Play(true);
				}
			}
			this._particles.Clear();
			foreach (AudioSource audioSource in gameObject2.GetComponentsInChildren<AudioSource>(true))
			{
				if (!(audioSource == null) && !(audioSource.clip == null))
				{
					GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
					audioSource.enabled = true;
					audioSource.Stop();
					audioSource.Play();
				}
			}
			Object.Destroy(gameObject2, 20f);
			this._cum = gameObject2;
			this._cumTimer = Mathf.Max(0f, settings.cumSeconds);
		}

		// Token: 0x06008FB7 RID: 36791 RVA: 0x003AC5BE File Offset: 0x003AA7BE
		private void TickCum(float deltaTime)
		{
			if (this._cum == null)
			{
				return;
			}
			if (deltaTime > 0f)
			{
				this._cumTimer -= deltaTime;
			}
			if (this._cumTimer <= 0f)
			{
				this.StopCum();
			}
		}

		// Token: 0x06008FB8 RID: 36792 RVA: 0x003AC5F8 File Offset: 0x003AA7F8
		private void StopCum()
		{
			if (this._cum != null)
			{
				this._cum.GetComponentsInChildren<ParticleSystem>(true, this._particles);
				for (int i = 0; i < this._particles.Count; i++)
				{
					if (this._particles[i] != null)
					{
						this._particles[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
					}
				}
				this._particles.Clear();
				EggRotorCumFollower eggRotorCumFollower;
				if (this._cum.TryGetComponent<EggRotorCumFollower>(out eggRotorCumFollower))
				{
					eggRotorCumFollower.enabled = false;
				}
			}
			this._cum = null;
			this._cumTimer = 0f;
		}

		// Token: 0x06008FB9 RID: 36793 RVA: 0x003AC698 File Offset: 0x003AA898
		private static GameObject ResolveCumPrefab(EggRotorClimaxSettings settings, out Vector3 position, out Vector3 euler, out Vector3 scale)
		{
			if (EggRotorClimax._foundCumPrefab == null && Time.unscaledTime >= EggRotorClimax._nextCumSearch)
			{
				EggRotorClimax._nextCumSearch = Time.unscaledTime + 10f;
				EggRotorClimax._foundCumPrefab = EggRotorClimax.FindFutaCumPrefab(out EggRotorClimax._foundCumPosition, out EggRotorClimax._foundCumEuler, out EggRotorClimax._foundCumScale);
				if (EggRotorClimax._foundCumPrefab == null && settings.cumPrefab == null && !EggRotorClimax._warnedCum)
				{
					EggRotorClimax._warnedCum = true;
					Debug.LogWarning("[EggRotor] No \"Cum\" effect found on the loaded futa emotes; the penis climax plays without it. Assign climax.cumPrefab.");
				}
			}
			bool flag = EggRotorClimax._foundCumPrefab != null;
			position = (flag ? EggRotorClimax._foundCumPosition : EggRotorClimax.DefaultCumPosition);
			euler = (flag ? EggRotorClimax._foundCumEuler : EggRotorClimax.DefaultCumEuler);
			scale = (flag ? EggRotorClimax._foundCumScale : Vector3.zero);
			if (!(settings.cumPrefab != null))
			{
				return EggRotorClimax._foundCumPrefab;
			}
			return settings.cumPrefab;
		}

		// Token: 0x06008FBA RID: 36794 RVA: 0x003AC780 File Offset: 0x003AA980
		public static GameObject FindFutaCumPrefab(out Vector3 position, out Vector3 euler, out Vector3 scale)
		{
			position = EggRotorClimax.DefaultCumPosition;
			euler = EggRotorClimax.DefaultCumEuler;
			scale = Vector3.zero;
			foreach (EmoteDefinitionSO emoteDefinitionSO in Resources.FindObjectsOfTypeAll<EmoteDefinitionSO>())
			{
				if (!(emoteDefinitionSO == null) && emoteDefinitionSO.FutanariRequirement == FutanariRequirement.FutanariOnly && emoteDefinitionSO.PropBindings != null)
				{
					for (int j = 0; j < emoteDefinitionSO.PropBindings.Count; j++)
					{
						EmotePropBinding emotePropBinding = emoteDefinitionSO.PropBindings[j];
						if (emotePropBinding != null && !(emotePropBinding.EffectPrefab == null) && emotePropBinding.Label != null && string.Equals(emotePropBinding.Label.Trim(), "Cum", StringComparison.OrdinalIgnoreCase))
						{
							position = emotePropBinding.LocalPositionOffset;
							euler = emotePropBinding.LocalRotationOffset;
							scale = emotePropBinding.LocalScaleOffset;
							return emotePropBinding.EffectPrefab;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06008FBB RID: 36795 RVA: 0x003AC87C File Offset: 0x003AAA7C
		private void ResolvePlayerSystems(bool force)
		{
			Transform transform = ((this._controller != null) ? this._controller.PlayerRoot : null);
			if (transform != this._resolvedRoot)
			{
				if (this._running)
				{
					this.EndClimax(EggRotorClimax.EndReason.Cancelled);
				}
				this.StopCum();
				this._inputLock.Forget();
				this.CloseVoice();
				this.StopNoise();
				this._speed = null;
				this._voice = null;
				this._dildo = null;
				this._emotes = null;
				this._input = null;
				this._genericAction = null;
				this._actor = null;
				this._globalSearchDone = false;
				this._resolvedRoot = transform;
				force = true;
			}
			if (!force)
			{
				this._resolveTimer -= Time.unscaledDeltaTime;
				if (this._resolveTimer > 0f)
				{
					return;
				}
			}
			this._resolveTimer = 2f;
			if (transform == null)
			{
				return;
			}
			if (this._speed == null)
			{
				this._speed = EggRotorClimax.FindOnPlayer<MasturbationSpeedController>(transform);
			}
			if (this._voice == null)
			{
				this._voice = EggRotorClimax.FindOnPlayer<MasturbationVoiceController>(transform);
			}
			if (this._dildo == null)
			{
				this._dildo = EggRotorClimax.FindOnPlayer<DildoActionExecutor>(transform);
			}
			if (this._emotes == null)
			{
				this._emotes = EggRotorClimax.FindOnPlayer<GameplayEmoteController>(transform);
			}
			if (this._input == null)
			{
				this._input = EggRotorClimax.FindOnPlayer<vThirdPersonInput>(transform);
				this._genericAction = null;
			}
			if (this._genericAction == null && this._input != null)
			{
				this._genericAction = this._input.GetComponent<vGenericAction>();
			}
			if (this._actor == null)
			{
				this.ResolveActor(transform);
			}
			if (!this._globalSearchDone && (this._speed == null || this._voice == null))
			{
				this._globalSearchDone = true;
				if (this._speed == null)
				{
					this._speed = Object.FindObjectOfType<MasturbationSpeedController>();
				}
				if (this._voice == null)
				{
					this._voice = Object.FindObjectOfType<MasturbationVoiceController>();
				}
			}
		}

		// Token: 0x06008FBC RID: 36796 RVA: 0x003ACA84 File Offset: 0x003AAC84
		private void ResolveActor(Transform root)
		{
			StealthActor stealthActor = root.GetComponentInChildren<StealthActor>(true);
			if (stealthActor == null && SensoryManager.Instance != null)
			{
				stealthActor = SensoryManager.Instance.GetPlayer();
			}
			if (stealthActor == this._actor)
			{
				return;
			}
			if (this._noiseActive && this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
			this._actor = stealthActor;
			if (this._noiseActive && this._actor != null)
			{
				this._actor.SetExternalNoise(this, this.Settings.noiseRadius);
			}
		}

		// Token: 0x06008FBD RID: 36797 RVA: 0x003ACB24 File Offset: 0x003AAD24
		private static T FindOnPlayer<T>(Transform root) where T : Component
		{
			T t = root.GetComponentInChildren<T>(true);
			if (t == null)
			{
				t = root.GetComponentInParent<T>();
			}
			return t;
		}

		// Token: 0x04009328 RID: 37672
		public const string SquirtObjectName = "EggRotorSquirt";

		// Token: 0x04009329 RID: 37673
		public const string CumObjectName = "EggRotorCum";

		// Token: 0x0400932A RID: 37674
		public static readonly Vector3 DefaultCumPosition = new Vector3(-0.0001f, 0.0018f, -0.0025f);

		// Token: 0x0400932B RID: 37675
		public static readonly Vector3 DefaultCumEuler = new Vector3(-90f, 0f, 0f);

		// Token: 0x0400932C RID: 37676
		private const float CumSafetyLifetime = 20f;

		// Token: 0x0400932D RID: 37677
		private const float ResolveInterval = 2f;

		// Token: 0x0400932E RID: 37678
		private const float SquirtSearchInterval = 10f;

		// Token: 0x0400932F RID: 37679
		private const float SquirtSafetyLifetime = 15f;

		// Token: 0x04009330 RID: 37680
		private const float RepairStableSeconds = 1f;

		// Token: 0x04009331 RID: 37681
		private const float RepairTimeoutSeconds = 180f;

		// Token: 0x04009332 RID: 37682
		private const float LowerBodyReleasePerSecond = 4f;

		// Token: 0x04009333 RID: 37683
		private const PlayerContextResource LeaseResources = PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation;

		// Token: 0x04009334 RID: 37684
		private PlayerActivity _gateBaseline;

		// Token: 0x04009335 RID: 37685
		private const PlayerActivity LeaseBlocked = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose;

		// Token: 0x04009336 RID: 37686
		private static GameObject _foundSquirtPrefab;

		// Token: 0x04009337 RID: 37687
		private static float _nextSquirtSearch;

		// Token: 0x04009338 RID: 37688
		private static bool _warnedSquirt;

		// Token: 0x04009339 RID: 37689
		private static GameObject _foundCumPrefab;

		// Token: 0x0400933A RID: 37690
		private static Vector3 _foundCumPosition;

		// Token: 0x0400933B RID: 37691
		private static Vector3 _foundCumEuler;

		// Token: 0x0400933C RID: 37692
		private static Vector3 _foundCumScale;

		// Token: 0x0400933D RID: 37693
		private static float _nextCumSearch;

		// Token: 0x0400933E RID: 37694
		private static bool _warnedCum;

		// Token: 0x04009341 RID: 37697
		private EggRotorController _controller;

		// Token: 0x04009342 RID: 37698
		private OrgasmMeterController _meter;

		// Token: 0x04009343 RID: 37699
		private Action _maxReachedHandler;

		// Token: 0x04009344 RID: 37700
		private Action<PlayerActivity> _gateChangedHandler;

		// Token: 0x04009345 RID: 37701
		private bool _gateHooked;

		// Token: 0x04009346 RID: 37702
		private Transform _resolvedRoot;

		// Token: 0x04009347 RID: 37703
		private float _resolveTimer;

		// Token: 0x04009348 RID: 37704
		private bool _globalSearchDone;

		// Token: 0x04009349 RID: 37705
		private MasturbationSpeedController _speed;

		// Token: 0x0400934A RID: 37706
		private MasturbationVoiceController _voice;

		// Token: 0x0400934B RID: 37707
		private DildoActionExecutor _dildo;

		// Token: 0x0400934C RID: 37708
		private GameplayEmoteController _emotes;

		// Token: 0x0400934D RID: 37709
		private vThirdPersonInput _input;

		// Token: 0x0400934E RID: 37710
		private vGenericAction _genericAction;

		// Token: 0x0400934F RID: 37711
		private StealthActor _actor;

		// Token: 0x04009350 RID: 37712
		private readonly EggRotorMeterFeed _feed = new EggRotorMeterFeed();

		// Token: 0x04009351 RID: 37713
		private readonly EggRotorRefractory _refractory = new EggRotorRefractory();

		// Token: 0x04009352 RID: 37714
		private readonly EggRotorInputLock _inputLock = new EggRotorInputLock();

		// Token: 0x04009353 RID: 37715
		private readonly EggRotorTremble _tremble = new EggRotorTremble();

		// Token: 0x04009354 RID: 37716
		private readonly float[] _pulseTimes = new float[12];

		// Token: 0x04009355 RID: 37717
		private readonly List<ParticleSystem> _particles = new List<ParticleSystem>(2);

		// Token: 0x04009356 RID: 37718
		private EggRotorClimaxSettings _fallbackSettings;

		// Token: 0x04009357 RID: 37719
		private bool _running;

		// Token: 0x04009358 RID: 37720
		private bool _abortRequested;

		// Token: 0x04009359 RID: 37721
		private float _time;

		// Token: 0x0400935A RID: 37722
		private int _pulseCount;

		// Token: 0x0400935B RID: 37723
		private int _nextPulse;

		// Token: 0x0400935C RID: 37724
		private bool _squirted;

		// Token: 0x0400935D RID: 37725
		private EggRotorTrembleWeights _weights;

		// Token: 0x0400935E RID: 37726
		private bool _emoteHeld;

		// Token: 0x0400935F RID: 37727
		private GameplayEmoteController _heldEmotes;

		// Token: 0x04009360 RID: 37728
		private float _seed;

		// Token: 0x04009361 RID: 37729
		private PlayerActivityLease _lease;

		// Token: 0x04009362 RID: 37730
		private string _leaseReason;

		// Token: 0x04009363 RID: 37731
		private bool _voiceOpen;

		// Token: 0x04009364 RID: 37732
		private float _voiceTimer;

		// Token: 0x04009365 RID: 37733
		private bool _noiseActive;

		// Token: 0x04009366 RID: 37734
		private float _noiseTimer;

		// Token: 0x04009367 RID: 37735
		private GameObject _squirt;

		// Token: 0x04009368 RID: 37736
		private bool _futaClimax;

		// Token: 0x04009369 RID: 37737
		private GameObject _cum;

		// Token: 0x0400936A RID: 37738
		private float _cumTimer;

		// Token: 0x02001222 RID: 4642
		private enum EndReason
		{
			// Token: 0x0400936C RID: 37740
			Completed,
			// Token: 0x0400936D RID: 37741
			Cancelled,
			// Token: 0x0400936E RID: 37742
			Interrupted,
			// Token: 0x0400936F RID: 37743
			Abandoned
		}
	}
}
