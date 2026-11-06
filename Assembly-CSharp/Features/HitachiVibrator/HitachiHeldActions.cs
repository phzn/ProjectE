using System;
using System.Collections;
using Features.Detection;
using Features.Inventory;
using Features.MasturbationSystem;
using Features.PlayerEmotes;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.HitachiVibrator
{
	// Token: 0x0200119D RID: 4509
	public class HitachiHeldActions : MonoBehaviour
	{
		// Token: 0x06008A06 RID: 35334 RVA: 0x00386930 File Offset: 0x00384B30
		private void Start()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (this.actionAnimator == null && instance != null)
			{
				this.actionAnimator = instance.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this.actionAnimator == null)
			{
				this.actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this.playerInput == null && this.actionAnimator != null)
			{
				this.playerInput = this.actionAnimator.GetComponentInParent<vThirdPersonInput>();
			}
			if (this.playerInput == null)
			{
				this.playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this.masturbationSpeed == null)
			{
				this.masturbationSpeed = Object.FindObjectOfType<MasturbationSpeedController>();
			}
			if (this.emoteController == null)
			{
				this.emoteController = Object.FindObjectOfType<GameplayEmoteController>();
			}
			if (this.propHandler == null)
			{
				this.propHandler = Object.FindObjectOfType<EmotePropHandler>();
			}
		}

		// Token: 0x06008A07 RID: 35335 RVA: 0x00386A10 File Offset: 0x00384C10
		private bool IsHitachiActive()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				return false;
			}
			InventoryItemSO activeItem = instance.GetActiveItem();
			return !(activeItem == null) && (this.hitachiItem == null || activeItem == this.hitachiItem);
		}

		// Token: 0x06008A08 RID: 35336 RVA: 0x00386A5C File Offset: 0x00384C5C
		private void Update()
		{
			ItemUsageSignal.Set(this, this._phase > HitachiHeldActions.Phase.Idle);
			if (!this.IsHitachiActive())
			{
				if (this._phase != HitachiHeldActions.Phase.Idle)
				{
					this.AbortToIdle();
				}
				return;
			}
			if (this._phase == HitachiHeldActions.Phase.Idle)
			{
				if (this.useInput.GetButtonDown())
				{
					this.StartAction();
					return;
				}
			}
			else if (this._phase == HitachiHeldActions.Phase.Playing)
			{
				if (this.stopInput.GetButtonDown())
				{
					this.StopAction();
					return;
				}
				this.TickSpeedAndOrgasm();
			}
		}

		// Token: 0x06008A09 RID: 35337 RVA: 0x00386AD0 File Offset: 0x00384CD0
		private void TickSpeedAndOrgasm()
		{
			bool flag = InventoryControllerInput.SpeedModifierHeld(this.speedModifierInput);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = flag;
			}
			int num = InventoryControllerInput.SpeedStep(flag, ref this._controllerSpeedDirection, ref this._controllerSpeedRepeat);
			if (num != 0)
			{
				float targetSpeed = this._targetSpeed;
				this._targetSpeed = Mathf.Clamp(this._targetSpeed + (float)num * this.speedStepPerNotch, this.minSpeed, this.maxSpeed);
				if (!Mathf.Approximately(targetSpeed, this._targetSpeed))
				{
					if (this.masturbationSpeed != null)
					{
						this.masturbationSpeed.ShowExternalSpeed(this._targetSpeed);
					}
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[HitachiHeldActions] target speed -> {0:F2}", this._targetSpeed));
					}
				}
			}
			if (this.speedSmoothing >= 1f)
			{
				this._currentSpeed = this._targetSpeed;
			}
			else
			{
				float num2 = 1f - Mathf.Pow(1f - this.speedSmoothing, Time.deltaTime * 60f);
				this._currentSpeed = Mathf.Lerp(this._currentSpeed, this._targetSpeed, num2);
			}
			this.ApplySpeedToAnimator(this._currentSpeed);
			if (this.orgasmMeter != null && !this._maxReached)
			{
				float num3 = ((this.proxyEmote != null) ? this.proxyEmote.OrgasmProgressPerSecondAtBaseSpeed : 0.05f) * this._currentSpeed * Time.deltaTime;
				if (num3 > 0f)
				{
					this.orgasmMeter.AddProgress(num3);
				}
			}
		}

		// Token: 0x06008A0A RID: 35338 RVA: 0x00386C50 File Offset: 0x00384E50
		private void ApplySpeedToAnimator(float s)
		{
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				return;
			}
			float num2;
			float num3;
			if (s <= this.slowToMediumSpeed)
			{
				float num = Mathf.InverseLerp(this.minSpeed, this.slowToMediumSpeed, s);
				num2 = Mathf.Lerp(0f, 1f, num);
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num);
			}
			else if (s <= this.mediumToFastSpeed)
			{
				float num4 = Mathf.InverseLerp(this.slowToMediumSpeed, this.mediumToFastSpeed, s);
				num2 = 1f;
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num4);
			}
			else
			{
				float num5 = Mathf.InverseLerp(this.mediumToFastSpeed, this.maxSpeed, s);
				num2 = Mathf.Lerp(1f, 2f, num5);
				num3 = Mathf.Lerp(this.clipMultMin, this.clipMultMax, num5);
			}
			if (!string.IsNullOrEmpty(this.blendParam))
			{
				animator.SetFloat(this.blendParam, num2);
			}
			if (!string.IsNullOrEmpty(this.speedMultParam))
			{
				animator.SetFloat(this.speedMultParam, num3);
			}
		}

		// Token: 0x06008A0B RID: 35339 RVA: 0x00386D58 File Offset: 0x00384F58
		private void StartAction()
		{
			if (this.actionAnimator == null || this.actionEntry == null || this.actionEntry.TriggerPrefab == null)
			{
				Debug.LogWarning("[HitachiHeldActions] Cannot start: missing actionAnimator or actionEntry/TriggerPrefab.");
				return;
			}
			this.actionAnimator.TriggerAction(this.actionEntry);
			this.SuppressHoldingLayer();
			this.ApplyActionHandOffset();
			this._targetSpeed = Mathf.Clamp(this.defaultSpeed, this.minSpeed, this.maxSpeed);
			this._currentSpeed = this._targetSpeed;
			this._maxReached = false;
			this._cumPropFired = false;
			this.HookMeter();
			this.ApplySpeedToAnimator(this._currentSpeed);
			if (this.masturbationSpeed != null)
			{
				this.masturbationSpeed.ExternalDriverActive = true;
			}
			this.StartVibratorLoop();
			if (this.propHandler != null && this.proxyEmote != null)
			{
				this.propHandler.SetupProps(this.proxyEmote);
			}
			if (this.emoteController != null && this.proxyEmote != null)
			{
				this.emoteController.NotifyExternalMasturbationStart(this.proxyEmote);
				this._masturbationStarted = true;
			}
			this._phase = HitachiHeldActions.Phase.Playing;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Started Hitachi action (Start -> Loop).");
			}
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x00386E9C File Offset: 0x0038509C
		private void StartVibratorLoop()
		{
			if (this.vibratorLoopSound == null)
			{
				return;
			}
			if (this._vibratorAudio == null)
			{
				GameObject gameObject = ((this.actionAnimator != null) ? this.actionAnimator.gameObject : base.gameObject);
				this._vibratorAudio = gameObject.AddComponent<AudioSource>();
				this._vibratorAudio.playOnAwake = false;
				this._vibratorAudio.spatialBlend = 0f;
			}
			this._vibratorAudio.clip = this.vibratorLoopSound;
			this._vibratorAudio.loop = true;
			this._vibratorAudio.volume = this.vibratorVolume;
			this._vibratorAudio.Play();
		}

		// Token: 0x06008A0D RID: 35341 RVA: 0x00386F49 File Offset: 0x00385149
		private void StopVibratorLoop()
		{
			if (this._vibratorAudio != null && this._vibratorAudio.isPlaying)
			{
				this._vibratorAudio.Stop();
			}
		}

		// Token: 0x06008A0E RID: 35342 RVA: 0x00386F74 File Offset: 0x00385174
		private void EndMasturbationDrive()
		{
			if (this._masturbationStarted && this.emoteController != null && this.proxyEmote != null)
			{
				this.emoteController.NotifyExternalMasturbationEnd(this.proxyEmote);
			}
			this._masturbationStarted = false;
			if (this.propHandler != null)
			{
				this.propHandler.CleanupAllProps();
			}
			if (this.masturbationSpeed != null)
			{
				this.masturbationSpeed.ExternalDriverActive = false;
			}
		}

		// Token: 0x06008A0F RID: 35343 RVA: 0x00386FF0 File Offset: 0x003851F0
		private void HookMeter()
		{
			if (this.orgasmMeter == null)
			{
				this.orgasmMeter = OrgasmMeterController.Instance;
			}
			if (this.orgasmMeter != null && !this._meterHooked)
			{
				this.orgasmMeter.OnMaxReached += this.HandleOrgasmMaxReached;
				this._meterHooked = true;
			}
		}

		// Token: 0x06008A10 RID: 35344 RVA: 0x0038704A File Offset: 0x0038524A
		private void UnhookMeter()
		{
			if (this.orgasmMeter != null && this._meterHooked)
			{
				this.orgasmMeter.OnMaxReached -= this.HandleOrgasmMaxReached;
			}
			this._meterHooked = false;
		}

		// Token: 0x06008A11 RID: 35345 RVA: 0x00387080 File Offset: 0x00385280
		private void HandleOrgasmMaxReached()
		{
			if (this._maxReached)
			{
				return;
			}
			this._maxReached = true;
			if (this._phase != HitachiHeldActions.Phase.Playing)
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Orgasm max reached -> Cum -> End.");
			}
			this._phase = HitachiHeldActions.Phase.Stopping;
			if (this._stopRoutine != null)
			{
				base.StopCoroutine(this._stopRoutine);
			}
			this._stopRoutine = base.StartCoroutine(this.OrgasmRoutine());
		}

		// Token: 0x06008A12 RID: 35346 RVA: 0x003870E8 File Offset: 0x003852E8
		private Animator ResolveAnimator()
		{
			Animator animator = ((this.actionAnimator != null) ? this.actionAnimator.GetComponentInChildren<Animator>() : null);
			if (animator == null && PlayerInventoryManager.Instance != null)
			{
				animator = PlayerInventoryManager.Instance.PlayerAnimator;
			}
			return animator;
		}

		// Token: 0x06008A13 RID: 35347 RVA: 0x00387134 File Offset: 0x00385334
		private void SuppressHoldingLayer()
		{
			if (this._holdSuppressed)
			{
				return;
			}
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				return;
			}
			if (this._holdLayerIndex < 0)
			{
				this._holdLayerIndex = animator.GetLayerIndex(this.holdingLayerName);
			}
			if (this._holdLayerIndex < 0)
			{
				return;
			}
			this._savedHoldWeight = animator.GetLayerWeight(this._holdLayerIndex);
			animator.SetLayerWeight(this._holdLayerIndex, 0f);
			this._holdSuppressed = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HitachiHeldActions] HoldingItem layer disabled (saved weight {0}).", this._savedHoldWeight));
			}
		}

		// Token: 0x06008A14 RID: 35348 RVA: 0x003871CC File Offset: 0x003853CC
		private void RestoreHoldingLayer()
		{
			if (!this._holdSuppressed)
			{
				return;
			}
			Animator animator = this.ResolveAnimator();
			if (animator != null && this._holdLayerIndex >= 0)
			{
				animator.SetLayerWeight(this._holdLayerIndex, this._savedHoldWeight);
			}
			this._holdSuppressed = false;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] HoldingItem layer restored.");
			}
		}

		// Token: 0x06008A15 RID: 35349 RVA: 0x00387228 File Offset: 0x00385428
		private void ApplyActionHandOffset()
		{
			if (this._propOffsetApplied)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			this._handProp = ((instance != null && instance.GetSpawnedHandObject() != null) ? instance.GetSpawnedHandObject().transform : null);
			if (this._handProp == null)
			{
				return;
			}
			this._savedPropLocalPos = this._handProp.localPosition;
			this._savedPropLocalRot = this._handProp.localRotation;
			this._handProp.localPosition = this.actionHandPositionOffset;
			this._handProp.localRotation = Quaternion.Euler(this.actionHandRotationOffset);
			this._propOffsetApplied = true;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Hand prop offset -> action offset.");
			}
		}

		// Token: 0x06008A16 RID: 35350 RVA: 0x003872E0 File Offset: 0x003854E0
		private void RestoreHandOffset()
		{
			if (!this._propOffsetApplied)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			Transform transform = ((instance != null && instance.GetSpawnedHandObject() != null) ? instance.GetSpawnedHandObject().transform : this._handProp);
			if (transform != null)
			{
				if (this.hitachiItem != null)
				{
					transform.localPosition = this.hitachiItem.handPositionOffset;
					transform.localRotation = Quaternion.Euler(this.hitachiItem.handRotationOffset);
				}
				else
				{
					transform.localPosition = this._savedPropLocalPos;
					transform.localRotation = this._savedPropLocalRot;
				}
			}
			this._handProp = null;
			this._propOffsetApplied = false;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Hand prop offset -> classic holding offset.");
			}
		}

		// Token: 0x06008A17 RID: 35351 RVA: 0x0038739F File Offset: 0x0038559F
		private void StopAction()
		{
			if (this._phase != HitachiHeldActions.Phase.Playing)
			{
				return;
			}
			this._phase = HitachiHeldActions.Phase.Stopping;
			if (this._stopRoutine != null)
			{
				base.StopCoroutine(this._stopRoutine);
			}
			this._stopRoutine = base.StartCoroutine(this.StopRoutine());
		}

		// Token: 0x06008A18 RID: 35352 RVA: 0x003873D8 File Offset: 0x003855D8
		private IEnumerator StopRoutine()
		{
			yield return this.PlayEndAndFinish();
			this._stopRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Stopped (End -> hold).");
			}
			yield break;
		}

		// Token: 0x06008A19 RID: 35353 RVA: 0x003873E7 File Offset: 0x003855E7
		private IEnumerator OrgasmRoutine()
		{
			Animator animator = this.ResolveAnimator();
			if (animator != null && animator.HasState(this.actionLayer, Animator.StringToHash(this.cumState)))
			{
				animator.CrossFadeInFixedTime(this.cumState, this.cumCrossfade, this.actionLayer);
				if (this.playerInput != null)
				{
					this.playerInput.suppressCameraZoom = false;
				}
				if (this.masturbationSpeed != null)
				{
					this.masturbationSpeed.HideExternalSpeed();
				}
				float fxDelay = Mathf.Clamp(this.cumEffectDelay, 0f, this.cumDuration);
				if (fxDelay > 0f)
				{
					yield return new WaitForSeconds(fxDelay);
				}
				this.FireCumProp();
				float num = this.cumDuration - fxDelay;
				if (num > 0f)
				{
					yield return new WaitForSeconds(num);
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning(string.Format("[HitachiHeldActions] Cum state '{0}' not found on layer {1}; going straight to End.", this.cumState, this.actionLayer));
			}
			yield return this.PlayEndAndFinish();
			this._stopRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Orgasm finished (Cum -> End -> hold).");
			}
			yield break;
		}

		// Token: 0x06008A1A RID: 35354 RVA: 0x003873F8 File Offset: 0x003855F8
		private void FireCumProp()
		{
			if (this._cumPropFired)
			{
				return;
			}
			this._cumPropFired = true;
			if (this.propHandler != null)
			{
				this.propHandler.ActivateProp(this.cumPropLabel);
			}
			if (this.debugLogs)
			{
				Debug.Log("[HitachiHeldActions] Cum prop '" + this.cumPropLabel + "' activated (squirt + vocal).");
			}
		}

		// Token: 0x06008A1B RID: 35355 RVA: 0x00387456 File Offset: 0x00385656
		private IEnumerator PlayEndAndFinish()
		{
			Animator anim = this.ResolveAnimator();
			if (anim != null && anim.HasState(this.actionLayer, Animator.StringToHash(this.endState)))
			{
				anim.CrossFadeInFixedTime(this.endState, this.endCrossfade, this.actionLayer);
				float guard = 0f;
				while (guard < 1f)
				{
					if (this.IsInState(anim, this.endState))
					{
						break;
					}
					guard += Time.deltaTime;
					yield return null;
				}
				while (this.IsInState(anim, this.endState))
				{
					if (anim.GetCurrentAnimatorStateInfo(this.actionLayer).normalizedTime >= 0.98f)
					{
						break;
					}
					yield return null;
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning(string.Format("[HitachiHeldActions] End state '{0}' not found on layer {1}; skipping wind-down.", this.endState, this.actionLayer));
			}
			this.RestoreHoldingLayer();
			this.RestoreHandOffset();
			this.EndMasturbationDrive();
			this.CleanupSpeedAndMeter();
			if (this.actionAnimator != null)
			{
				this.actionAnimator.FinishCurrentAction();
				this.actionAnimator.RestoreOverrideLayer();
			}
			this._phase = HitachiHeldActions.Phase.Idle;
			yield break;
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x00387468 File Offset: 0x00385668
		private void AbortToIdle()
		{
			if (this._stopRoutine != null)
			{
				base.StopCoroutine(this._stopRoutine);
				this._stopRoutine = null;
			}
			this.RestoreHoldingLayer();
			this.RestoreHandOffset();
			this.EndMasturbationDrive();
			this.CleanupSpeedAndMeter();
			if (this.actionAnimator != null && this.actionAnimator.GenericAction != null && this.actionAnimator.GenericAction.doingAction)
			{
				this.actionAnimator.FinishCurrentAction();
			}
			if (this.actionAnimator != null)
			{
				this.actionAnimator.RestoreOverrideLayer();
			}
			this._phase = HitachiHeldActions.Phase.Idle;
		}

		// Token: 0x06008A1D RID: 35357 RVA: 0x00387508 File Offset: 0x00385708
		private void CleanupSpeedAndMeter()
		{
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
			if (this.masturbationSpeed != null)
			{
				this.masturbationSpeed.HideExternalSpeed();
			}
			this.StopVibratorLoop();
			Animator animator = this.ResolveAnimator();
			if (animator != null)
			{
				if (!string.IsNullOrEmpty(this.blendParam))
				{
					animator.SetFloat(this.blendParam, 1f);
				}
				if (!string.IsNullOrEmpty(this.speedMultParam))
				{
					animator.SetFloat(this.speedMultParam, 1f);
				}
			}
			if (this.orgasmMeter != null)
			{
				if (this._maxReached)
				{
					this.orgasmMeter.UnlockAndCooldown();
				}
				else
				{
					this.orgasmMeter.ForceCooldown();
				}
			}
			this.UnhookMeter();
			this._targetSpeed = this.defaultSpeed;
			this._currentSpeed = this.defaultSpeed;
		}

		// Token: 0x06008A1E RID: 35358 RVA: 0x003875E6 File Offset: 0x003857E6
		private void OnDisable()
		{
			this.UnhookMeter();
			this.EndMasturbationDrive();
			this.StopVibratorLoop();
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
		}

		// Token: 0x06008A1F RID: 35359 RVA: 0x00387614 File Offset: 0x00385814
		private bool IsInState(Animator anim, string stateName)
		{
			return !anim.IsInTransition(this.actionLayer) && anim.GetCurrentAnimatorStateInfo(this.actionLayer).IsName(stateName);
		}

		// Token: 0x06008A20 RID: 35360 RVA: 0x00387648 File Offset: 0x00385848
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = null;
			if (this.IsHitachiActive())
			{
				interactionConfigSO = ((this._phase == HitachiHeldActions.Phase.Idle) ? this.idleConfig : this.loopConfig);
			}
			if (interactionConfigSO == this._shown)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shown != null && instance.IsShowing)
				{
					instance.Hide();
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shown = interactionConfigSO;
		}

		// Token: 0x04008CB2 RID: 36018
		[Header("Item")]
		[Tooltip("The Hitachi item: HUD/actions are active only while this is the active held item.")]
		[SerializeField]
		private InventoryItemSO hitachiItem;

		// Token: 0x04008CB3 RID: 36019
		[Header("HUD Configs")]
		[Tooltip("Shown while the Hitachi is in hand and idle (F : Use).")]
		[SerializeField]
		private InteractionConfigSO idleConfig;

		// Token: 0x04008CB4 RID: 36020
		[Tooltip("Shown while the looping action is playing (X : Stop).")]
		[SerializeField]
		private InteractionConfigSO loopConfig;

		// Token: 0x04008CB5 RID: 36021
		[Header("Action")]
		[Tooltip("PlayerActionEntry referencing the vTriggerGenericAction prefab whose playAnimation is 'Ciel_Hitachi_Start' (the Hitachi_LoopEmote prefab).")]
		[SerializeField]
		private PlayerActionEntry actionEntry;

		// Token: 0x04008CB6 RID: 36022
		[Header("Animator State Names (Base Layer / Actions/Ciel_Hitachi)")]
		[SerializeField]
		private string endState = "Ciel_Hitachi_End";

		// Token: 0x04008CB7 RID: 36023
		[Tooltip("Orgasm climax state played when the orgasm meter fills.")]
		[SerializeField]
		private string cumState = "Ciel_Hitachi_Cum";

		// Token: 0x04008CB8 RID: 36024
		[Tooltip("Base Layer index the Hitachi states live on (animatorLayer:0).")]
		[SerializeField]
		private int actionLayer;

		// Token: 0x04008CB9 RID: 36025
		[Tooltip("Crossfade duration into the End state when stopping.")]
		[SerializeField]
		private float endCrossfade = 0.2f;

		// Token: 0x04008CBA RID: 36026
		[Tooltip("Crossfade duration into the Cum state on orgasm.")]
		[SerializeField]
		private float cumCrossfade = 0.2f;

		// Token: 0x04008CBB RID: 36027
		[Tooltip("How long the cum animation plays before transitioning to the End wind-down (seconds).")]
		[SerializeField]
		private float cumDuration = 12.25f;

		// Token: 0x04008CBC RID: 36028
		[Header("Masturbation System Integration")]
		[Tooltip("Proxy EmoteDefinitionSO (IsMasturbationEmote=1) used to drive the SHARED masturbation controllers (face, voice/breathing, heartbeat) and to carry the 'Cum' squirt PropBinding. Its OrgasmProgressPerSecondAtBaseSpeed sets the bar speed — same as the masturbation emotes.")]
		[SerializeField]
		private EmoteDefinitionSO proxyEmote;

		// Token: 0x04008CBD RID: 36029
		[Tooltip("Shared GameplayEmoteController on the player. Auto-found.")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04008CBE RID: 36030
		[Tooltip("Shared EmotePropHandler on the player (spawns/activates the squirt prop). Auto-found.")]
		[SerializeField]
		private EmotePropHandler propHandler;

		// Token: 0x04008CBF RID: 36031
		[Tooltip("Shared MasturbationSpeedController. We flag ExternalDriverActive so it does NOT grab the scroll wheel — the Hitachi drives its own 3-clip blend speed. Auto-found.")]
		[SerializeField]
		private MasturbationSpeedController masturbationSpeed;

		// Token: 0x04008CC0 RID: 36032
		[Tooltip("Seconds into the Cum animation at which the squirt + cum vocal fire (NOT at the start).")]
		[SerializeField]
		private float cumEffectDelay = 3.24f;

		// Token: 0x04008CC1 RID: 36033
		[Tooltip("PropBinding label (on the proxy emote) for the squirt/cum effect.")]
		[SerializeField]
		private string cumPropLabel = "Cum";

		// Token: 0x04008CC2 RID: 36034
		[Header("Vibrator Loop Sound")]
		[Tooltip("The vibrator buzz, played LOOPED for the whole action (Start->Loop->Cum) and stopped at End.")]
		[SerializeField]
		private AudioClip vibratorLoopSound;

		// Token: 0x04008CC3 RID: 36035
		[Range(0f, 1f)]
		[SerializeField]
		private float vibratorVolume = 0.7f;

		// Token: 0x04008CC4 RID: 36036
		[Header("Holding Layer")]
		[Tooltip("The HoldingItem layer is disabled while the action plays so the in-hand pose (right arm) doesn't fight the full-body Hitachi clips, then restored when it ends.")]
		[SerializeField]
		private string holdingLayerName = "HoldingItem";

		// Token: 0x04008CC5 RID: 36037
		[Header("Hand Prop Offset During Action")]
		[Tooltip("The in-hand prop uses a DIFFERENT local offset while the action plays (the clips pose the hand differently). Restored to the item's classic holding offset when it ends.")]
		[SerializeField]
		private Vector3 actionHandPositionOffset = new Vector3(-0.2120626f, 0.5796965f, 0.9105911f);

		// Token: 0x04008CC6 RID: 36038
		[SerializeField]
		private Vector3 actionHandRotationOffset = new Vector3(-5.996f, 81.206f, -93.149f);

		// Token: 0x04008CC7 RID: 36039
		[Header("Speed & Loop Blend (Ctrl + Scroll)")]
		[Tooltip("Animator float that selects the loop clip: 0=Slow, 1=Medium, 2=Fast (HitachiBlend).")]
		[SerializeField]
		private string blendParam = "HitachiBlend";

		// Token: 0x04008CC8 RID: 36040
		[Tooltip("Animator float that scales the loop clip playback speed (HitachiSpeedMult).")]
		[SerializeField]
		private string speedMultParam = "HitachiSpeedMult";

		// Token: 0x04008CC9 RID: 36041
		[Tooltip("Modifier held to grab the scroll wheel for speed (Ctrl). Without it, the wheel zooms the camera.")]
		[SerializeField]
		private GenericInput speedModifierInput = new GenericInput("LeftControl", "LeftBumper", "LeftBumper");

		// Token: 0x04008CCA RID: 36042
		[Tooltip("Continuous speed range. 1 = default (Medium @ 1x).")]
		[SerializeField]
		private float minSpeed = 0.5f;

		// Token: 0x04008CCB RID: 36043
		[SerializeField]
		private float maxSpeed = 2.5f;

		// Token: 0x04008CCC RID: 36044
		[SerializeField]
		private float defaultSpeed = 1f;

		// Token: 0x04008CCD RID: 36045
		[Tooltip("Speed added/removed per scroll notch.")]
		[Min(0.01f)]
		[SerializeField]
		private float speedStepPerNotch = 0.15f;

		// Token: 0x04008CCE RID: 36046
		[Range(0f, 1f)]
		[SerializeField]
		private float speedSmoothing = 0.2f;

		// Token: 0x04008CCF RID: 36047
		[Header("Speed -> Clip Mapping")]
		[Tooltip("At/below this speed the loop is fully Slow. Between here and the Fast threshold it rides Medium.")]
		[SerializeField]
		private float slowToMediumSpeed = 1f;

		// Token: 0x04008CD0 RID: 36048
		[Tooltip("At/above this speed the loop is fully Fast.")]
		[SerializeField]
		private float mediumToFastSpeed = 1.6f;

		// Token: 0x04008CD1 RID: 36049
		[Tooltip("Playback multiplier applied to the CURRENT clip as speed climbs within its band (1 = clip's authored speed).")]
		[SerializeField]
		private float clipMultMin = 0.85f;

		// Token: 0x04008CD2 RID: 36050
		[SerializeField]
		private float clipMultMax = 1.4f;

		// Token: 0x04008CD3 RID: 36051
		[Header("Orgasm Meter")]
		[SerializeField]
		private OrgasmMeterController orgasmMeter;

		// Token: 0x04008CD4 RID: 36052
		[Header("Input")]
		[SerializeField]
		private GenericInput useInput = new GenericInput("F", "X", "X");

		// Token: 0x04008CD5 RID: 36053
		[SerializeField]
		private GenericInput stopInput = new GenericInput("X", "B", "B");

		// Token: 0x04008CD6 RID: 36054
		[Header("Refs (auto-found if left empty)")]
		[SerializeField]
		private PlayerActionAnimator actionAnimator;

		// Token: 0x04008CD7 RID: 36055
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04008CD8 RID: 36056
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008CD9 RID: 36057
		private HitachiHeldActions.Phase _phase;

		// Token: 0x04008CDA RID: 36058
		private InteractionConfigSO _shown;

		// Token: 0x04008CDB RID: 36059
		private Coroutine _stopRoutine;

		// Token: 0x04008CDC RID: 36060
		private int _holdLayerIndex = -1;

		// Token: 0x04008CDD RID: 36061
		private float _savedHoldWeight;

		// Token: 0x04008CDE RID: 36062
		private bool _holdSuppressed;

		// Token: 0x04008CDF RID: 36063
		private Transform _handProp;

		// Token: 0x04008CE0 RID: 36064
		private Vector3 _savedPropLocalPos;

		// Token: 0x04008CE1 RID: 36065
		private Quaternion _savedPropLocalRot;

		// Token: 0x04008CE2 RID: 36066
		private bool _propOffsetApplied;

		// Token: 0x04008CE3 RID: 36067
		private float _targetSpeed = 1f;

		// Token: 0x04008CE4 RID: 36068
		private float _currentSpeed = 1f;

		// Token: 0x04008CE5 RID: 36069
		private bool _maxReached;

		// Token: 0x04008CE6 RID: 36070
		private bool _meterHooked;

		// Token: 0x04008CE7 RID: 36071
		private bool _masturbationStarted;

		// Token: 0x04008CE8 RID: 36072
		private bool _cumPropFired;

		// Token: 0x04008CE9 RID: 36073
		private AudioSource _vibratorAudio;

		// Token: 0x04008CEA RID: 36074
		private int _controllerSpeedDirection;

		// Token: 0x04008CEB RID: 36075
		private float _controllerSpeedRepeat;

		// Token: 0x0200119E RID: 4510
		private enum Phase
		{
			// Token: 0x04008CED RID: 36077
			Idle,
			// Token: 0x04008CEE RID: 36078
			Playing,
			// Token: 0x04008CEF RID: 36079
			Stopping
		}
	}
}
