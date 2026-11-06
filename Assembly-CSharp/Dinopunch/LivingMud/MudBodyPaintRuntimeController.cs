using System;
using System.Collections;
using Features.Detection;
using Features.PlayerEmotes;
using Features.RunModes;
using Features.UI.TimedEffects;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200073D RID: 1853
	public class MudBodyPaintRuntimeController : MonoBehaviour
	{
		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x0010EC0C File Offset: 0x0010CE0C
		private SkinDecal ActiveDecal
		{
			get
			{
				if (!(this._activeDecal != null))
				{
					return this.mudBodyPaintDecal;
				}
				return this._activeDecal;
			}
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x0010EC29 File Offset: 0x0010CE29
		public bool IsMudBodyPaintActive()
		{
			return !(this.skinDecalController == null) && !(this.ActiveDecal == null) && this.skinDecalController.IsDecalActive(this.ActiveDecal);
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x0010EC5A File Offset: 0x0010CE5A
		public void ApplyTimedBodyPaint()
		{
			this.ApplyTimedBodyPaint(null);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x0010EC64 File Offset: 0x0010CE64
		public void ApplyTimedBodyPaint(SkinDecal decal)
		{
			this._activeTemporaryDurationSeconds = this.temporaryDurationSeconds;
			SkinDecal skinDecal = ((decal != null) ? decal : this.mudBodyPaintDecal);
			if (skinDecal != null && this._activeDecal != null && this._activeDecal != skinDecal && this.skinDecalController != null)
			{
				if (this.opacityRampRoutine != null)
				{
					base.StopCoroutine(this.opacityRampRoutine);
					this.opacityRampRoutine = null;
				}
				if (this.fadeAndRemoveRoutine != null)
				{
					base.StopCoroutine(this.fadeAndRemoveRoutine);
					this.fadeAndRemoveRoutine = null;
				}
				this._managedPaintExpected = false;
				this.UnregisterHUDTimer();
				this.skinDecalController.RemoveDecal(this._activeDecal);
			}
			bool flag = this.skinDecalController != null && skinDecal != null && this.skinDecalController.IsDecalActive(skinDecal);
			float num = (flag ? Mathf.Clamp01(Mathf.Max(this.currentMudOpacity, this.startOpacity)) : Mathf.Clamp01(this.startOpacity));
			this._activeDecal = skinDecal;
			this._managedPaintExpected = false;
			if (this.skinDecalController == null || this.ActiveDecal == null)
			{
				return;
			}
			if (!flag)
			{
				this.skinDecalController.AddDecal(this.ActiveDecal, null, new float?(num));
				this.skinDecalController.SetDecalActive(this.ActiveDecal, true);
			}
			this.skinDecalController.SetDecalOpacity(this.ActiveDecal, num);
			this.currentMudOpacity = num;
			this._managedPaintExpected = this.skinDecalController.HasDecal(this.ActiveDecal);
			if (!this._managedPaintExpected)
			{
				this.currentMudOpacity = 0f;
				return;
			}
			if (this.opacityRampRoutine != null)
			{
				base.StopCoroutine(this.opacityRampRoutine);
			}
			if (this.rampOpacityDuringAction)
			{
				float num2 = Mathf.Max(0f, this.fallbackRampDurationSeconds);
				if (num2 > 0f && num < Mathf.Clamp01(this.targetOpacity))
				{
					this.opacityRampRoutine = base.StartCoroutine(this.RampMudOpacityRoutine(num, this.targetOpacity, num2));
				}
				else
				{
					this.skinDecalController.SetDecalOpacity(this.ActiveDecal, this.targetOpacity);
					this.currentMudOpacity = Mathf.Clamp01(this.targetOpacity);
				}
			}
			this.StartTimedCleanup();
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x0010EE99 File Offset: 0x0010D099
		public void TriggerMudBodyPaint()
		{
			this.TryTriggerMudBodyPaint(0f);
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x0010EEA8 File Offset: 0x0010D0A8
		public bool TryTriggerMudBodyPaint(float durationOverrideSeconds = 0f)
		{
			if (this.actionFlowRoutine != null)
			{
				base.StopCoroutine(this.actionFlowRoutine);
				this.actionFlowRoutine = null;
				this.ForceEndMudAction();
			}
			if (!this.TryAcquireActionContext())
			{
				return false;
			}
			if (!base.isActiveAndEnabled || this._actionLease == null || !this._actionLease.IsActive)
			{
				this.ReleaseActionContext();
				return false;
			}
			this._activeDecal = this.mudBodyPaintDecal;
			this._managedPaintExpected = false;
			this._activeTemporaryDurationSeconds = ((durationOverrideSeconds > 0f) ? durationOverrideSeconds : this.temporaryDurationSeconds);
			float num = this.ResolveRampDurationSeconds();
			bool flag = false;
			bool flag2;
			try
			{
				if (this.mudActionEntry != null && (this.actionEmoteRuntimeService != null || this.playerActionAnimator != null))
				{
					this.StartMudActionFlow(num);
					flag = this.actionFlowRoutine != null;
				}
				if (this.skinDecalController == null || this.ActiveDecal == null)
				{
					flag2 = true;
				}
				else
				{
					this.skinDecalController.AddDecal(this.ActiveDecal, null, new float?(this.startOpacity));
					this.skinDecalController.SetDecalActive(this.ActiveDecal, true);
					this.skinDecalController.SetDecalOpacity(this.ActiveDecal, this.startOpacity);
					this.currentMudOpacity = Mathf.Clamp01(this.startOpacity);
					this._managedPaintExpected = this.skinDecalController.HasDecal(this.ActiveDecal);
					if (!this._managedPaintExpected)
					{
						this.currentMudOpacity = 0f;
						flag2 = true;
					}
					else
					{
						if (this.opacityRampRoutine != null)
						{
							base.StopCoroutine(this.opacityRampRoutine);
						}
						if (this.rampOpacityDuringAction)
						{
							if (num > 0f)
							{
								this.opacityRampRoutine = base.StartCoroutine(this.RampMudOpacityRoutine(this.startOpacity, this.targetOpacity, num));
							}
							else
							{
								this.skinDecalController.SetDecalOpacity(this.ActiveDecal, this.targetOpacity);
								this.currentMudOpacity = Mathf.Clamp01(this.targetOpacity);
							}
						}
						if (!flag)
						{
							this.StartTimedCleanup();
						}
						flag2 = true;
					}
				}
			}
			catch
			{
				if (this.actionFlowRoutine != null)
				{
					base.StopCoroutine(this.actionFlowRoutine);
					this.actionFlowRoutine = null;
				}
				this.ForceEndMudAction();
				throw;
			}
			finally
			{
				if (!flag)
				{
					this.ReleaseActionContext();
				}
			}
			return flag2;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x0010F100 File Offset: 0x0010D300
		private void StartTimedCleanup()
		{
			if (!this._managedPaintExpected || this.skinDecalController == null || this.ActiveDecal == null || this.ActiveDecal.isPermanent || !this.skinDecalController.HasDecal(this.ActiveDecal))
			{
				return;
			}
			if (this.fadeAndRemoveRoutine != null)
			{
				base.StopCoroutine(this.fadeAndRemoveRoutine);
			}
			this._activeFadeDurationSeconds = ((this.fadeDurationSeconds > 0f) ? this.fadeDurationSeconds : 5f);
			this._activeTotalDurationSeconds = RunModeService.ResolveBodyPaintDuration(Mathf.Max(0f, this._activeTemporaryDurationSeconds) + this._activeFadeDurationSeconds);
			this._activeFadeDurationSeconds = Mathf.Min(this._activeFadeDurationSeconds, this._activeTotalDurationSeconds);
			this.fadeAndRemoveRoutine = (float.IsPositiveInfinity(this._activeTotalDurationSeconds) ? null : base.StartCoroutine(this.FadeAndRemoveRoutine()));
			this.RegisterHUDTimer();
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x0010F1E8 File Offset: 0x0010D3E8
		public void RemoveBodyPaintImmediate()
		{
			if (this.fadeAndRemoveRoutine != null)
			{
				base.StopCoroutine(this.fadeAndRemoveRoutine);
				this.fadeAndRemoveRoutine = null;
			}
			if (this.opacityRampRoutine != null)
			{
				base.StopCoroutine(this.opacityRampRoutine);
				this.opacityRampRoutine = null;
			}
			this._managedPaintExpected = false;
			if (this.skinDecalController != null && this.ActiveDecal != null)
			{
				this.skinDecalController.RemoveDecal(this.ActiveDecal);
			}
			this.currentMudOpacity = 0f;
			this.UnregisterHUDTimer();
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0010F270 File Offset: 0x0010D470
		private IEnumerator FadeAndRemoveRoutine()
		{
			SkinDecal decal = this.ActiveDecal;
			float num = Mathf.Max(0f, this._activeTotalDurationSeconds - this._activeFadeDurationSeconds);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			if (this.skinDecalController == null || decal == null || !this._managedPaintExpected || !this.skinDecalController.HasDecal(decal))
			{
				this._managedPaintExpected = false;
				this.currentMudOpacity = 0f;
				if (this.opacityRampRoutine != null)
				{
					base.StopCoroutine(this.opacityRampRoutine);
					this.opacityRampRoutine = null;
				}
				this.UnregisterHUDTimer();
				this.fadeAndRemoveRoutine = null;
				yield break;
			}
			float fadeTime = this._activeFadeDurationSeconds;
			if (this.fadeDurationSeconds <= 0f)
			{
				Debug.LogWarning(string.Format("[MudBodyPaintRuntimeController] fadeDurationSeconds={0} a runtime: uso fallback 5s.", this.fadeDurationSeconds), this);
			}
			float elapsed = 0f;
			float from = ((this.currentMudOpacity > 0.01f) ? Mathf.Clamp01(this.currentMudOpacity) : 1f);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[MudBodyPaintRuntimeController] Fade-out '{0}': {1:F2} -> 0 in {2:F1}s.", decal.name, from, fadeTime), this);
			}
			while (elapsed < fadeTime)
			{
				elapsed += Time.deltaTime;
				float num2 = Mathf.Clamp01(elapsed / fadeTime);
				float num3 = Mathf.Lerp(from, 0f, num2);
				this.skinDecalController.SetDecalOpacity(decal, num3);
				this.currentMudOpacity = num3;
				yield return null;
			}
			this._managedPaintExpected = false;
			this.currentMudOpacity = 0f;
			this.UnregisterHUDTimer();
			this.fadeAndRemoveRoutine = null;
			this.skinDecalController.RemoveDecal(decal);
			yield break;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x0010F27F File Offset: 0x0010D47F
		private IEnumerator RampMudOpacityRoutine(float fromOpacity, float toOpacity, float durationSeconds)
		{
			SkinDecal decal = this.ActiveDecal;
			if (this.skinDecalController == null || decal == null || !this._managedPaintExpected || !this.skinDecalController.HasDecal(decal))
			{
				this.currentMudOpacity = 0f;
				this.opacityRampRoutine = null;
				yield break;
			}
			float duration = Mathf.Max(0f, durationSeconds);
			if (duration <= 0f)
			{
				this.skinDecalController.SetDecalOpacity(decal, Mathf.Clamp01(toOpacity));
				this.currentMudOpacity = Mathf.Clamp01(toOpacity);
				this.opacityRampRoutine = null;
				yield break;
			}
			float elapsed = 0f;
			float from = Mathf.Clamp01(fromOpacity);
			float to = Mathf.Clamp01(toOpacity);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = Mathf.Lerp(from, to, num);
				this.skinDecalController.SetDecalOpacity(decal, num2);
				this.currentMudOpacity = num2;
				yield return null;
			}
			this.skinDecalController.SetDecalOpacity(decal, to);
			this.currentMudOpacity = to;
			this.opacityRampRoutine = null;
			yield break;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x0010F2A4 File Offset: 0x0010D4A4
		private void HandleDecalsChanged()
		{
			if (!this._managedPaintExpected || this.skinDecalController == null || this.ActiveDecal == null || this.skinDecalController.HasDecal(this.ActiveDecal))
			{
				return;
			}
			if (this.fadeAndRemoveRoutine != null)
			{
				base.StopCoroutine(this.fadeAndRemoveRoutine);
				this.fadeAndRemoveRoutine = null;
			}
			if (this.opacityRampRoutine != null)
			{
				base.StopCoroutine(this.opacityRampRoutine);
				this.opacityRampRoutine = null;
			}
			this._managedPaintExpected = false;
			this.currentMudOpacity = 0f;
			this.UnregisterHUDTimer();
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x0010F338 File Offset: 0x0010D538
		private void ObserveSkinDecalController(SkinDecalController controller)
		{
			if (this._observedSkinDecalController == controller)
			{
				return;
			}
			if (this._observedSkinDecalController != null)
			{
				this._observedSkinDecalController.OnDecalsChanged -= this.HandleDecalsChanged;
			}
			this._observedSkinDecalController = controller;
			if (this._observedSkinDecalController != null)
			{
				this._observedSkinDecalController.OnDecalsChanged += this.HandleDecalsChanged;
			}
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x0010F3A8 File Offset: 0x0010D5A8
		private void StartMudActionFlow(float expectedDurationSeconds)
		{
			if (this.actionFlowRoutine != null)
			{
				base.StopCoroutine(this.actionFlowRoutine);
				this.actionFlowRoutine = null;
				this.ForceEndMudAction();
			}
			if (this.mudActionEntry != null && this.mudActionEntry.TriggerPrefab != null && !this.mudActionEntry.TriggerPrefab.endActionManualy)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[MudBodyPaintRuntimeController] TriggerPrefab.endActionManualy was false — forcing to true so the action respects configured Duration.");
				}
				this.mudActionEntry.TriggerPrefab.endActionManualy = true;
			}
			this.actionFlowRoutine = base.StartCoroutine(this.MudActionFlowRoutine(expectedDurationSeconds));
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x0010F439 File Offset: 0x0010D639
		private IEnumerator MudActionFlowRoutine(float expectedDurationSeconds)
		{
			this.SetMovementLocked(true);
			float num = Mathf.Max(0f, expectedDurationSeconds);
			if (this.actionEmoteRuntimeService != null && this.mudActionEntry != null)
			{
				float num2 = this.actionEmoteRuntimeService.TriggerAction(this.mudActionEntry, this.mudPropEmoteDefinition, this.useAutoPropTimers);
				if (num2 > 0f)
				{
					num = num2;
				}
			}
			else if (this.playerActionAnimator != null && this.mudActionEntry != null)
			{
				float num3 = this.playerActionAnimator.TriggerAction(this.mudActionEntry);
				if (num3 > 0f)
				{
					num = num3;
				}
			}
			else if (this.mudPropEmoteDefinition != null && this.debugLogs)
			{
				Debug.LogWarning("[MudBodyPaintRuntimeController] mudPropEmoteDefinition configured but PlayerActionEmoteRuntimeService is missing. AE_EmoteProp events will not find mud props.");
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[MudBodyPaintRuntimeController] MudActionFlow started — effectiveDuration={0:F2}s (expected={1:F2}s)", num, expectedDurationSeconds));
			}
			float startTime = Time.realtimeSinceStartup;
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			else
			{
				yield return null;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[MudBodyPaintRuntimeController] MudActionFlow finished — actual elapsed={0:F2}s", Time.realtimeSinceStartup - startTime));
			}
			this.ForceEndMudAction();
			this.actionFlowRoutine = null;
			if (base.isActiveAndEnabled)
			{
				this.StartTimedCleanup();
			}
			yield break;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x0010F450 File Offset: 0x0010D650
		private void ForceEndMudAction()
		{
			if (this.actionEmoteRuntimeService != null)
			{
				this.actionEmoteRuntimeService.FinishAction(this.cleanupMudPropsOnActionEnd);
			}
			else if (this.playerActionAnimator != null)
			{
				this.playerActionAnimator.FinishCurrentAction();
				this.playerActionAnimator.RestoreOverrideLayer();
			}
			this.SetMovementLocked(false);
			this.ReleaseActionContext();
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0010F4AF File Offset: 0x0010D6AF
		private float ResolveRampDurationSeconds()
		{
			if (this.mudActionEntry != null && this.mudActionEntry.Duration > 0f)
			{
				return this.mudActionEntry.Duration;
			}
			return Mathf.Max(0f, this.fallbackRampDurationSeconds);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0010F4E8 File Offset: 0x0010D6E8
		private void SetMovementLocked(bool shouldLock)
		{
			if (this.playerInput == null)
			{
				return;
			}
			if (shouldLock)
			{
				if (this.movementLockedByThisController)
				{
					return;
				}
				this.previousInputLock = this.playerInput.lockInput;
				this.playerInput.SetLockAllInput(true);
				if (this.playerInput.cc != null)
				{
					this.playerInput.cc.input = Vector2.zero;
					this.playerInput.cc.isSprinting = false;
				}
				this.movementLockedByThisController = true;
				return;
			}
			else
			{
				if (!this.movementLockedByThisController)
				{
					return;
				}
				this.playerInput.SetLockAllInput(this.previousInputLock);
				this.movementLockedByThisController = false;
				return;
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x0010F594 File Offset: 0x0010D794
		private bool TryAcquireActionContext()
		{
			if (this._actionLease != null)
			{
				if (this._actionLease.IsActive)
				{
					return true;
				}
				this._actionLease = null;
			}
			return PlayerActivityGate.TryAcquire("MudBodyPaint#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._actionLease, false, null);
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x0010F5EE File Offset: 0x0010D7EE
		private void ReleaseActionContext()
		{
			PlayerActivityLease actionLease = this._actionLease;
			this._actionLease = null;
			if (actionLease == null)
			{
				return;
			}
			actionLease.Dispose();
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0010F608 File Offset: 0x0010D808
		private void Awake()
		{
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponent<PlayerActionAnimator>();
			}
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInParent<PlayerActionAnimator>();
			}
			if (this.actionEmoteRuntimeService == null)
			{
				this.actionEmoteRuntimeService = base.GetComponent<PlayerActionEmoteRuntimeService>();
			}
			if (this.actionEmoteRuntimeService == null)
			{
				this.actionEmoteRuntimeService = base.GetComponentInChildren<PlayerActionEmoteRuntimeService>();
			}
			if (this.actionEmoteRuntimeService == null)
			{
				this.actionEmoteRuntimeService = base.GetComponentInParent<PlayerActionEmoteRuntimeService>();
			}
			if (this.skinDecalController == null)
			{
				this.skinDecalController = base.GetComponent<SkinDecalController>();
			}
			this.ObserveSkinDecalController(this.skinDecalController);
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponent<vThirdPersonInput>();
			}
			this._activeDecal = this.mudBodyPaintDecal;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x0010F6FD File Offset: 0x0010D8FD
		private void OnDisable()
		{
			this.ForceEndMudAction();
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x0010F705 File Offset: 0x0010D905
		private void OnDestroy()
		{
			this.ObserveSkinDecalController(null);
			this.ForceEndMudAction();
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x0010F714 File Offset: 0x0010D914
		private void RegisterHUDTimer()
		{
			if (!this.showHUDTimer || TimedEffectHUDService.Instance == null)
			{
				return;
			}
			float activeTotalDurationSeconds = this._activeTotalDurationSeconds;
			this.hudTimerData.TotalDuration = activeTotalDurationSeconds;
			TimedEffectHUDService.Instance.RegisterEffect(this.hudTimerData);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[MudBodyPaintRuntimeController] HUD timer registered — {0:F1}s", activeTotalDurationSeconds));
			}
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x0010F777 File Offset: 0x0010D977
		private void UnregisterHUDTimer()
		{
			if (!this.showHUDTimer || TimedEffectHUDService.Instance == null)
			{
				return;
			}
			TimedEffectHUDService.Instance.UnregisterEffect(this.hudTimerData.EffectId);
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x0010F7A4 File Offset: 0x0010D9A4
		private void OnValidate()
		{
			this.startOpacity = Mathf.Clamp01(this.startOpacity);
			this.targetOpacity = Mathf.Clamp01(this.targetOpacity);
			this.fallbackRampDurationSeconds = Mathf.Max(0f, this.fallbackRampDurationSeconds);
			this.temporaryDurationSeconds = Mathf.Max(0f, this.temporaryDurationSeconds);
			this.fadeDurationSeconds = Mathf.Max(0f, this.fadeDurationSeconds);
		}

		// Token: 0x0400305A RID: 12378
		private const PlayerActivity RequiredActivity = PlayerActivity.Interact;

		// Token: 0x0400305B RID: 12379
		private const PlayerContextResource ActionResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x0400305C RID: 12380
		[Header("References")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x0400305D RID: 12381
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400305E RID: 12382
		[SerializeField]
		private SkinDecal mudBodyPaintDecal;

		// Token: 0x0400305F RID: 12383
		[Header("Action Animation")]
		[SerializeField]
		private PlayerActionEntry mudActionEntry;

		// Token: 0x04003060 RID: 12384
		[Header("Emote Props Bridge")]
		[SerializeField]
		private PlayerActionEmoteRuntimeService actionEmoteRuntimeService;

		// Token: 0x04003061 RID: 12385
		[SerializeField]
		private EmoteDefinitionSO mudPropEmoteDefinition;

		// Token: 0x04003062 RID: 12386
		[SerializeField]
		private bool useAutoPropTimers = true;

		// Token: 0x04003063 RID: 12387
		[SerializeField]
		private bool cleanupMudPropsOnActionEnd = true;

		// Token: 0x04003064 RID: 12388
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04003065 RID: 12389
		[Header("Body Paint Runtime")]
		[SerializeField]
		[Range(0f, 1f)]
		private float startOpacity = 1f;

		// Token: 0x04003066 RID: 12390
		[SerializeField]
		[Range(0f, 1f)]
		private float targetOpacity = 1f;

		// Token: 0x04003067 RID: 12391
		[SerializeField]
		private bool rampOpacityDuringAction = true;

		// Token: 0x04003068 RID: 12392
		[SerializeField]
		private float fallbackRampDurationSeconds = 2f;

		// Token: 0x04003069 RID: 12393
		[SerializeField]
		private float temporaryDurationSeconds = 30f;

		// Token: 0x0400306A RID: 12394
		private float _activeTemporaryDurationSeconds;

		// Token: 0x0400306B RID: 12395
		private float _activeTotalDurationSeconds;

		// Token: 0x0400306C RID: 12396
		private float _activeFadeDurationSeconds;

		// Token: 0x0400306D RID: 12397
		[SerializeField]
		private float fadeDurationSeconds = 5f;

		// Token: 0x0400306E RID: 12398
		[SerializeField]
		private bool restartTimerIfAlreadyActive = true;

		// Token: 0x0400306F RID: 12399
		[Header("HUD Timer")]
		[Tooltip("If true, registers the body paint duration on the shared HUD timer service")]
		[SerializeField]
		private bool showHUDTimer = true;

		// Token: 0x04003070 RID: 12400
		[SerializeField]
		private TimedEffectData hudTimerData = new TimedEffectData
		{
			EffectId = "MudBodyPaint",
			DisplayName = "Mud Paint",
			AccentColor = new Color(0.55f, 0.35f, 0.15f, 1f)
		};

		// Token: 0x04003071 RID: 12401
		private Coroutine fadeAndRemoveRoutine;

		// Token: 0x04003072 RID: 12402
		private Coroutine actionFlowRoutine;

		// Token: 0x04003073 RID: 12403
		private Coroutine opacityRampRoutine;

		// Token: 0x04003074 RID: 12404
		private vThirdPersonInput playerInput;

		// Token: 0x04003075 RID: 12405
		private bool movementLockedByThisController;

		// Token: 0x04003076 RID: 12406
		private bool previousInputLock;

		// Token: 0x04003077 RID: 12407
		private float currentMudOpacity;

		// Token: 0x04003078 RID: 12408
		private bool _managedPaintExpected;

		// Token: 0x04003079 RID: 12409
		private SkinDecalController _observedSkinDecalController;

		// Token: 0x0400307A RID: 12410
		private PlayerActivityLease _actionLease;

		// Token: 0x0400307B RID: 12411
		private SkinDecal _activeDecal;
	}
}
