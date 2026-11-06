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

namespace Features.TintBodyPaint
{
	// Token: 0x02000C0A RID: 3082
	public class TintBodyPaintRuntimeController : MonoBehaviour
	{
		// Token: 0x06005912 RID: 22802 RVA: 0x0022D623 File Offset: 0x0022B823
		public bool IsTintBodyPaintActive()
		{
			return !(this.skinDecalController == null) && !(this.tintBodyPaintDecal == null) && this.skinDecalController.IsDecalActive(this.tintBodyPaintDecal);
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x0022D654 File Offset: 0x0022B854
		public void TriggerTintBodyPaint()
		{
			this.TryTriggerTintBodyPaint();
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x0022D660 File Offset: 0x0022B860
		public bool TryTriggerTintBodyPaint()
		{
			if (this.actionFlowRoutine != null)
			{
				base.StopCoroutine(this.actionFlowRoutine);
				this.actionFlowRoutine = null;
				this.ForceEndTintAction();
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
			float num = this.ResolveRampDurationSeconds();
			bool flag = false;
			this._managedPaintExpected = false;
			bool flag2;
			try
			{
				if (this.tintActionEntry != null && (this.actionEmoteRuntimeService != null || this.playerActionAnimator != null))
				{
					this.StartTintActionFlow(num);
					flag = this.actionFlowRoutine != null;
				}
				if (this.skinDecalController == null || this.tintBodyPaintDecal == null)
				{
					flag2 = true;
				}
				else
				{
					this.skinDecalController.AddDecal(this.tintBodyPaintDecal, null, new float?(this.startOpacity));
					this.skinDecalController.SetDecalActive(this.tintBodyPaintDecal, true);
					this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, this.startOpacity);
					this.currentTintOpacity = Mathf.Clamp01(this.startOpacity);
					this._managedPaintExpected = this.skinDecalController.HasDecal(this.tintBodyPaintDecal);
					if (!this._managedPaintExpected)
					{
						this.currentTintOpacity = 0f;
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
								this.opacityRampRoutine = base.StartCoroutine(this.RampTintOpacityRoutine(this.startOpacity, this.targetOpacity, num));
							}
							else
							{
								this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, this.targetOpacity);
								this.currentTintOpacity = Mathf.Clamp01(this.targetOpacity);
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
				this.ForceEndTintAction();
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

		// Token: 0x06005915 RID: 22805 RVA: 0x0022D894 File Offset: 0x0022BA94
		private void StartTimedCleanup()
		{
			if (!this._managedPaintExpected || this.skinDecalController == null || this.tintBodyPaintDecal == null || this.tintBodyPaintDecal.isPermanent || !this.skinDecalController.HasDecal(this.tintBodyPaintDecal))
			{
				return;
			}
			if (this.fadeAndRemoveRoutine != null)
			{
				base.StopCoroutine(this.fadeAndRemoveRoutine);
			}
			this._activeFadeDurationSeconds = ((this.fadeDurationSeconds > 0f) ? this.fadeDurationSeconds : 5f);
			this._activeTotalDurationSeconds = RunModeService.ResolveBodyPaintDuration(Mathf.Max(0f, this.temporaryDurationSeconds) + this._activeFadeDurationSeconds);
			this._activeFadeDurationSeconds = Mathf.Min(this._activeFadeDurationSeconds, this._activeTotalDurationSeconds);
			this.fadeAndRemoveRoutine = (float.IsPositiveInfinity(this._activeTotalDurationSeconds) ? null : base.StartCoroutine(this.FadeAndRemoveRoutine()));
			this.RegisterHUDTimer();
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x0022D97C File Offset: 0x0022BB7C
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
			if (this.skinDecalController != null && this.tintBodyPaintDecal != null)
			{
				this.skinDecalController.RemoveDecal(this.tintBodyPaintDecal);
			}
			this.currentTintOpacity = 0f;
			this.UnregisterHUDTimer();
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x0022DA04 File Offset: 0x0022BC04
		private IEnumerator FadeAndRemoveRoutine()
		{
			float num = Mathf.Max(0f, this._activeTotalDurationSeconds - this._activeFadeDurationSeconds);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			if (this.skinDecalController == null || this.tintBodyPaintDecal == null || !this._managedPaintExpected || !this.skinDecalController.HasDecal(this.tintBodyPaintDecal))
			{
				this._managedPaintExpected = false;
				this.currentTintOpacity = 0f;
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
				Debug.LogWarning(string.Format("[TintBodyPaintRuntimeController] fadeDurationSeconds={0} a runtime: uso fallback 5s.", this.fadeDurationSeconds), this);
			}
			float elapsed = 0f;
			float from = ((this.currentTintOpacity > 0.01f) ? Mathf.Clamp01(this.currentTintOpacity) : 1f);
			while (elapsed < fadeTime)
			{
				elapsed += Time.deltaTime;
				float num2 = Mathf.Clamp01(elapsed / fadeTime);
				float num3 = Mathf.Lerp(from, 0f, num2);
				this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, num3);
				this.currentTintOpacity = num3;
				yield return null;
			}
			this._managedPaintExpected = false;
			this.currentTintOpacity = 0f;
			this.UnregisterHUDTimer();
			this.fadeAndRemoveRoutine = null;
			this.skinDecalController.RemoveDecal(this.tintBodyPaintDecal);
			yield break;
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x0022DA13 File Offset: 0x0022BC13
		private IEnumerator RampTintOpacityRoutine(float fromOpacity, float toOpacity, float durationSeconds)
		{
			if (this.skinDecalController == null || this.tintBodyPaintDecal == null || !this._managedPaintExpected || !this.skinDecalController.HasDecal(this.tintBodyPaintDecal))
			{
				this.currentTintOpacity = 0f;
				this.opacityRampRoutine = null;
				yield break;
			}
			float duration = Mathf.Max(0f, durationSeconds);
			if (duration <= 0f)
			{
				this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, Mathf.Clamp01(toOpacity));
				this.currentTintOpacity = Mathf.Clamp01(toOpacity);
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
				this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, num2);
				this.currentTintOpacity = num2;
				yield return null;
			}
			this.skinDecalController.SetDecalOpacity(this.tintBodyPaintDecal, to);
			this.currentTintOpacity = to;
			this.opacityRampRoutine = null;
			yield break;
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x0022DA38 File Offset: 0x0022BC38
		private void HandleDecalsChanged()
		{
			if (!this._managedPaintExpected || this.skinDecalController == null || this.tintBodyPaintDecal == null || this.skinDecalController.HasDecal(this.tintBodyPaintDecal))
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
			this.currentTintOpacity = 0f;
			this.UnregisterHUDTimer();
		}

		// Token: 0x0600591A RID: 22810 RVA: 0x0022DACC File Offset: 0x0022BCCC
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

		// Token: 0x0600591B RID: 22811 RVA: 0x0022DB3C File Offset: 0x0022BD3C
		private void StartTintActionFlow(float expectedDurationSeconds)
		{
			if (this.actionFlowRoutine != null)
			{
				base.StopCoroutine(this.actionFlowRoutine);
				this.actionFlowRoutine = null;
				this.ForceEndTintAction();
			}
			if (this.tintActionEntry != null && this.tintActionEntry.TriggerPrefab != null && !this.tintActionEntry.TriggerPrefab.endActionManualy)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[TintBodyPaintRuntimeController] TriggerPrefab.endActionManualy was false - forcing to true so the action respects configured Duration.");
				}
				this.tintActionEntry.TriggerPrefab.endActionManualy = true;
			}
			this.actionFlowRoutine = base.StartCoroutine(this.TintActionFlowRoutine(expectedDurationSeconds));
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x0022DBCD File Offset: 0x0022BDCD
		private IEnumerator TintActionFlowRoutine(float expectedDurationSeconds)
		{
			this.SetMovementLocked(true);
			float num = Mathf.Max(0f, expectedDurationSeconds);
			if (this.actionEmoteRuntimeService != null && this.tintActionEntry != null)
			{
				float num2 = this.actionEmoteRuntimeService.TriggerAction(this.tintActionEntry, this.tintPropEmoteDefinition, this.useAutoPropTimers);
				if (num2 > 0f)
				{
					num = num2;
				}
			}
			else if (this.playerActionAnimator != null && this.tintActionEntry != null)
			{
				float num3 = this.playerActionAnimator.TriggerAction(this.tintActionEntry);
				if (num3 > 0f)
				{
					num = num3;
				}
			}
			else if (this.tintPropEmoteDefinition != null && this.debugLogs)
			{
				Debug.LogWarning("[TintBodyPaintRuntimeController] tintPropEmoteDefinition configured but PlayerActionEmoteRuntimeService is missing. AE_EmoteProp events will not find tint props.");
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[TintBodyPaintRuntimeController] TintActionFlow started - effectiveDuration={0:F2}s (expected={1:F2}s)", num, expectedDurationSeconds));
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
				Debug.Log(string.Format("[TintBodyPaintRuntimeController] TintActionFlow finished - actual elapsed={0:F2}s", Time.realtimeSinceStartup - startTime));
			}
			this.ForceEndTintAction();
			this.actionFlowRoutine = null;
			if (base.isActiveAndEnabled)
			{
				this.StartTimedCleanup();
			}
			yield break;
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x0022DBE4 File Offset: 0x0022BDE4
		private void ForceEndTintAction()
		{
			if (this.actionEmoteRuntimeService != null)
			{
				this.actionEmoteRuntimeService.FinishAction(this.cleanupTintPropsOnActionEnd);
			}
			else if (this.playerActionAnimator != null)
			{
				this.playerActionAnimator.FinishCurrentAction();
				this.playerActionAnimator.RestoreOverrideLayer();
			}
			this.SetMovementLocked(false);
			this.ReleaseActionContext();
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x0022DC43 File Offset: 0x0022BE43
		private float ResolveRampDurationSeconds()
		{
			if (this.tintActionEntry != null && this.tintActionEntry.Duration > 0f)
			{
				return this.tintActionEntry.Duration;
			}
			return Mathf.Max(0f, this.fallbackRampDurationSeconds);
		}

		// Token: 0x0600591F RID: 22815 RVA: 0x0022DC7C File Offset: 0x0022BE7C
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

		// Token: 0x06005920 RID: 22816 RVA: 0x0022DD28 File Offset: 0x0022BF28
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
			return PlayerActivityGate.TryAcquire("TintBodyPaint#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._actionLease, false, null);
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x0022DD82 File Offset: 0x0022BF82
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

		// Token: 0x06005922 RID: 22818 RVA: 0x0022DD9C File Offset: 0x0022BF9C
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
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x0022DE85 File Offset: 0x0022C085
		private void OnDisable()
		{
			this.ForceEndTintAction();
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x0022DE8D File Offset: 0x0022C08D
		private void OnDestroy()
		{
			this.ObserveSkinDecalController(null);
			this.ForceEndTintAction();
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x0022DE9C File Offset: 0x0022C09C
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
				Debug.Log(string.Format("[TintBodyPaintRuntimeController] HUD timer registered - {0:F1}s", activeTotalDurationSeconds));
			}
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x0022DEFF File Offset: 0x0022C0FF
		private void UnregisterHUDTimer()
		{
			if (!this.showHUDTimer || TimedEffectHUDService.Instance == null)
			{
				return;
			}
			TimedEffectHUDService.Instance.UnregisterEffect(this.hudTimerData.EffectId);
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x0022DF2C File Offset: 0x0022C12C
		private void OnValidate()
		{
			this.startOpacity = Mathf.Clamp01(this.startOpacity);
			this.targetOpacity = Mathf.Clamp01(this.targetOpacity);
			this.fallbackRampDurationSeconds = Mathf.Max(0f, this.fallbackRampDurationSeconds);
			this.temporaryDurationSeconds = Mathf.Max(0f, this.temporaryDurationSeconds);
			this.fadeDurationSeconds = Mathf.Max(0f, this.fadeDurationSeconds);
		}

		// Token: 0x040059AF RID: 22959
		private const PlayerActivity RequiredActivity = PlayerActivity.Interact;

		// Token: 0x040059B0 RID: 22960
		private const PlayerContextResource ActionResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x040059B1 RID: 22961
		[Header("References")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x040059B2 RID: 22962
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x040059B3 RID: 22963
		[SerializeField]
		private SkinDecal tintBodyPaintDecal;

		// Token: 0x040059B4 RID: 22964
		[Header("Action Animation")]
		[SerializeField]
		private PlayerActionEntry tintActionEntry;

		// Token: 0x040059B5 RID: 22965
		[Header("Emote Props Bridge")]
		[SerializeField]
		private PlayerActionEmoteRuntimeService actionEmoteRuntimeService;

		// Token: 0x040059B6 RID: 22966
		[SerializeField]
		private EmoteDefinitionSO tintPropEmoteDefinition;

		// Token: 0x040059B7 RID: 22967
		[SerializeField]
		private bool useAutoPropTimers = true;

		// Token: 0x040059B8 RID: 22968
		[SerializeField]
		private bool cleanupTintPropsOnActionEnd = true;

		// Token: 0x040059B9 RID: 22969
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040059BA RID: 22970
		[Header("Body Paint Runtime")]
		[SerializeField]
		[Range(0f, 1f)]
		private float startOpacity = 1f;

		// Token: 0x040059BB RID: 22971
		[SerializeField]
		[Range(0f, 1f)]
		private float targetOpacity = 1f;

		// Token: 0x040059BC RID: 22972
		[SerializeField]
		private bool rampOpacityDuringAction = true;

		// Token: 0x040059BD RID: 22973
		[SerializeField]
		private float fallbackRampDurationSeconds = 2f;

		// Token: 0x040059BE RID: 22974
		[SerializeField]
		private float temporaryDurationSeconds = 30f;

		// Token: 0x040059BF RID: 22975
		private float _activeTotalDurationSeconds;

		// Token: 0x040059C0 RID: 22976
		private float _activeFadeDurationSeconds;

		// Token: 0x040059C1 RID: 22977
		[SerializeField]
		private float fadeDurationSeconds = 5f;

		// Token: 0x040059C2 RID: 22978
		[SerializeField]
		private bool restartTimerIfAlreadyActive = true;

		// Token: 0x040059C3 RID: 22979
		[Header("HUD Timer")]
		[Tooltip("If true, registers the body paint duration on the shared HUD timer service")]
		[SerializeField]
		private bool showHUDTimer = true;

		// Token: 0x040059C4 RID: 22980
		[SerializeField]
		private TimedEffectData hudTimerData = new TimedEffectData
		{
			EffectId = "TintBodyPaint",
			DisplayName = "Tint Paint",
			AccentColor = new Color(0.9f, 0.2f, 0.7f, 1f)
		};

		// Token: 0x040059C5 RID: 22981
		private Coroutine fadeAndRemoveRoutine;

		// Token: 0x040059C6 RID: 22982
		private Coroutine actionFlowRoutine;

		// Token: 0x040059C7 RID: 22983
		private Coroutine opacityRampRoutine;

		// Token: 0x040059C8 RID: 22984
		private vThirdPersonInput playerInput;

		// Token: 0x040059C9 RID: 22985
		private bool movementLockedByThisController;

		// Token: 0x040059CA RID: 22986
		private bool previousInputLock;

		// Token: 0x040059CB RID: 22987
		private float currentTintOpacity;

		// Token: 0x040059CC RID: 22988
		private bool _managedPaintExpected;

		// Token: 0x040059CD RID: 22989
		private SkinDecalController _observedSkinDecalController;

		// Token: 0x040059CE RID: 22990
		private PlayerActivityLease _actionLease;
	}
}
