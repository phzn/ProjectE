using System;
using System.Collections;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012CC RID: 4812
	public class PlayerActionAnimator : MonoBehaviour
	{
		// Token: 0x0600950A RID: 38154 RVA: 0x003D2E18 File Offset: 0x003D1018
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x0600950B RID: 38155 RVA: 0x003D2E20 File Offset: 0x003D1020
		private void Initialize()
		{
			if (this._isInitialized)
			{
				return;
			}
			if (this.GenericAction == null)
			{
				this.GenericAction = base.GetComponent<vGenericAction>();
			}
			if (this.GenericAction == null)
			{
				Debug.LogError("[PlayerActionAnimator] vGenericAction component not found! Please add it to the player.");
				return;
			}
			this._isInitialized = true;
			if (this.DebugMode)
			{
				Debug.Log("[PlayerActionAnimator] Initialized.");
			}
		}

		// Token: 0x0600950C RID: 38156 RVA: 0x003D2E82 File Offset: 0x003D1082
		public float TriggerAction(PlayerActionEntry entry)
		{
			if (!this._isInitialized)
			{
				this.Initialize();
			}
			if (entry == null)
			{
				return 0f;
			}
			this.TriggerAction(entry.TriggerPrefab, entry.ActionId);
			return entry.Duration;
		}

		// Token: 0x0600950D RID: 38157 RVA: 0x003D2EB4 File Offset: 0x003D10B4
		private void TriggerAction(vTriggerGenericAction actionTrigger, string actionName)
		{
			if (!this._isInitialized)
			{
				this.Initialize();
			}
			if (this.GenericAction == null)
			{
				Debug.LogError("[PlayerActionAnimator] Cannot trigger action - vGenericAction not found!");
				return;
			}
			if (actionTrigger == null)
			{
				Debug.LogWarning("[PlayerActionAnimator] Cannot trigger " + actionName + " - trigger prefab not assigned in profile!");
				return;
			}
			if (!this.GenericAction.actionConditions)
			{
				if (this.DebugMode)
				{
					vThirdPersonController component = this.GenericAction.GetComponent<vThirdPersonController>();
					if (component != null)
					{
						Debug.Log("[PlayerActionAnimator] WARNING: actionConditions=false. " + string.Format("isGrounded={0}, customAction={1}, ", component.isGrounded, component.customAction) + string.Format("isJumping={0}, isRolling={1}, ragdolled={2}", component.isJumping, component.isRolling, component.ragdolled));
					}
					else
					{
						Debug.Log("[PlayerActionAnimator] WARNING: actionConditions=false (cc is null)");
					}
				}
				if (this.DebugMode)
				{
					Debug.Log("[PlayerActionAnimator] Attempting to force trigger despite conditions...");
				}
			}
			this.DisableEmotionLayers();
			if (this.DebugMode)
			{
				Debug.Log("[PlayerActionAnimator] Triggering " + actionName + " action...");
			}
			this.ValidateActionState(actionTrigger, actionName);
			if (this.GenericAction.doingAction)
			{
				this.GenericAction.FinishAction();
			}
			this.GenericAction.triggerAction = actionTrigger;
			this.GenericAction.TriggerActionEvents();
			this.GenericAction.TriggerAnimation();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[PlayerActionAnimator] TriggerAnimation called for {0}. DoingAction: {1}", actionName, this.GenericAction.doingAction));
			}
		}

		// Token: 0x0600950E RID: 38158 RVA: 0x003D303C File Offset: 0x003D123C
		private void ValidateActionState(vTriggerGenericAction actionTrigger, string actionName)
		{
			if (actionTrigger == null || string.IsNullOrEmpty(actionTrigger.playAnimation) || this.GenericAction == null)
			{
				return;
			}
			Animator componentInChildren = this.GenericAction.GetComponentInChildren<Animator>();
			if (componentInChildren == null)
			{
				return;
			}
			int num = Mathf.Clamp(actionTrigger.animatorLayer, 0, componentInChildren.layerCount - 1);
			int num2 = Animator.StringToHash(actionTrigger.playAnimation);
			int num3 = Animator.StringToHash("Base Layer." + actionTrigger.playAnimation);
			if (!componentInChildren.HasState(num, num2) && !componentInChildren.HasState(num, num3))
			{
				Debug.LogWarning(string.Format("[PlayerActionAnimator] Trigger '{0}' references state '{1}' on layer {2}, but Animator.HasState returned false. Verify state name/path in Animator Controller.", actionName, actionTrigger.playAnimation, num));
			}
		}

		// Token: 0x0600950F RID: 38159 RVA: 0x003D30EE File Offset: 0x003D12EE
		public void DisableEmotionLayers()
		{
			if (!this.DisableEmotionLayersDuringAction)
			{
				return;
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.DisableEmotionLayersForAction();
				if (this.DebugMode)
				{
					Debug.Log("[PlayerActionAnimator] Disabled emotion layers via PlayerEmotionController");
					return;
				}
			}
			else
			{
				Debug.LogWarning("[PlayerActionAnimator] DisableEmotionLayers: PlayerEmotionController.Instance NULL — layer emotion NON spenti!");
			}
		}

		// Token: 0x06009510 RID: 38160 RVA: 0x003D3130 File Offset: 0x003D1330
		public void EnableRootMotionForAction()
		{
			if (!this._isInitialized)
			{
				this.Initialize();
			}
			if (this._didEnableRootMotion)
			{
				return;
			}
			if (this.GenericAction == null)
			{
				return;
			}
			vThirdPersonInput component = this.GenericAction.GetComponent<vThirdPersonInput>();
			if (component == null || component.cc == null)
			{
				return;
			}
			if (component.cc.useRootMotion)
			{
				return;
			}
			this._savedUseRootMotion = component.cc.useRootMotion;
			component.cc.useRootMotion = true;
			Animator animator = component.cc.animator;
			if (animator != null)
			{
				this._savedAnimatorApplyRootMotion = animator.applyRootMotion;
				animator.applyRootMotion = true;
			}
			this._didEnableRootMotion = true;
			if (this.DebugMode)
			{
				Debug.Log("[PlayerActionAnimator] Root motion forced ON for action");
			}
		}

		// Token: 0x06009511 RID: 38161 RVA: 0x003D31F4 File Offset: 0x003D13F4
		public void RestoreRootMotionForAction()
		{
			if (!this._didEnableRootMotion)
			{
				return;
			}
			if (this.GenericAction == null)
			{
				this._didEnableRootMotion = false;
				return;
			}
			vThirdPersonInput component = this.GenericAction.GetComponent<vThirdPersonInput>();
			if (component != null && component.cc != null)
			{
				component.cc.useRootMotion = this._savedUseRootMotion;
				Animator animator = component.cc.animator;
				if (animator != null)
				{
					animator.applyRootMotion = this._savedAnimatorApplyRootMotion;
				}
			}
			this._didEnableRootMotion = false;
			if (this.DebugMode)
			{
				Debug.Log("[PlayerActionAnimator] Root motion restored after action");
			}
		}

		// Token: 0x06009512 RID: 38162 RVA: 0x003D328E File Offset: 0x003D148E
		public void RestoreOverrideLayer()
		{
			if (!this.DisableEmotionLayersDuringAction)
			{
				return;
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.RestoreEmotionLayers();
				if (this.DebugMode)
				{
					Debug.Log("[PlayerActionAnimator] Restored emotion layers via PlayerEmotionController");
				}
			}
		}

		// Token: 0x06009513 RID: 38163 RVA: 0x003D32C4 File Offset: 0x003D14C4
		public void CancelCurrentAction()
		{
			if (this.GenericAction == null)
			{
				return;
			}
			int currentActionAnimatorLayer = this.CurrentActionAnimatorLayer;
			if (this.SmoothFinishAction && !this.TrySmoothCrossfadeToLocomotion(currentActionAnimatorLayer))
			{
				this.ForceRemoveCustomActionTag(currentActionAnimatorLayer);
			}
			this.GenericAction.CancelAction();
			if (this.GenericAction.doingAction)
			{
				this.GenericAction.ResetTriggerSettings(true);
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[PlayerActionAnimator] Action cancelled and locomotion restored (layer {0}).", currentActionAnimatorLayer));
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06009514 RID: 38164 RVA: 0x003D3340 File Offset: 0x003D1540
		public int CurrentActionAnimatorLayer
		{
			get
			{
				if (this.GenericAction != null && this.GenericAction.triggerAction != null)
				{
					return this.GenericAction.triggerAction.animatorLayer;
				}
				return 0;
			}
		}

		// Token: 0x06009515 RID: 38165 RVA: 0x003D3378 File Offset: 0x003D1578
		public void FinishCurrentAction()
		{
			if (this.GenericAction == null)
			{
				return;
			}
			int currentActionAnimatorLayer = this.CurrentActionAnimatorLayer;
			if (this.SmoothFinishAction && !this.TrySmoothCrossfadeToLocomotion(currentActionAnimatorLayer))
			{
				this.ForceRemoveCustomActionTag(currentActionAnimatorLayer);
			}
			this.GenericAction.FinishAction();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[PlayerActionAnimator] Action finished manually (layer {0}).", currentActionAnimatorLayer));
			}
		}

		// Token: 0x06009516 RID: 38166 RVA: 0x003D33DC File Offset: 0x003D15DC
		public void FinishCurrentActionWaitForExitState(Action onFullyExited, float timeoutSeconds = 5f)
		{
			if (this.GenericAction == null)
			{
				if (onFullyExited != null)
				{
					onFullyExited();
				}
				return;
			}
			int currentActionAnimatorLayer = this.CurrentActionAnimatorLayer;
			this.GenericAction.FinishAction();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[PlayerActionAnimator] Wait-for-exit-state finish on layer {0}; letting graph drive the wind-down.", currentActionAnimatorLayer));
			}
			base.StartCoroutine(this.WaitForCustomActionToClear(currentActionAnimatorLayer, timeoutSeconds, onFullyExited));
		}

		// Token: 0x06009517 RID: 38167 RVA: 0x003D3440 File Offset: 0x003D1640
		private IEnumerator WaitForCustomActionToClear(int actionLayer, float timeoutSeconds, Action onFullyExited)
		{
			vThirdPersonInput vThirdPersonInput = ((this.GenericAction != null) ? this.GenericAction.GetComponent<vThirdPersonInput>() : null);
			vThirdPersonController cc = ((vThirdPersonInput != null) ? vThirdPersonInput.cc : null);
			if (cc == null)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[PlayerActionAnimator] WaitForCustomActionToClear: controller not found, skipping wait.");
				}
				if (onFullyExited != null)
				{
					onFullyExited();
				}
				yield break;
			}
			float elapsed = 0f;
			yield return null;
			while (cc.customAction && elapsed < timeoutSeconds)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (cc.customAction)
			{
				Debug.LogWarning(string.Format("[PlayerActionAnimator] WaitForCustomActionToClear timed out after {0:0.00}s on layer {1}. Force-clearing tag.", timeoutSeconds, actionLayer));
				this.ForceRemoveCustomActionTag(actionLayer);
			}
			else if (this.DebugMode)
			{
				Debug.Log(string.Format("[PlayerActionAnimator] CustomAction tag cleared naturally after {0:0.00}s (layer {1}).", elapsed, actionLayer));
			}
			if (onFullyExited != null)
			{
				onFullyExited();
			}
			yield break;
		}

		// Token: 0x06009518 RID: 38168 RVA: 0x003D3464 File Offset: 0x003D1664
		private bool TrySmoothCrossfadeToLocomotion(int layer)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			Animator animator = ((this.GenericAction != null) ? this.GenericAction.GetComponentInChildren<Animator>() : base.GetComponentInChildren<Animator>());
			if (animator == null || !animator.isActiveAndEnabled || !animator.gameObject.activeInHierarchy || !animator.isInitialized)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[PlayerActionAnimator] Smooth finish skipped: Animator missing or inactive.");
				}
				return false;
			}
			string[] array = new string[] { "Locomotion", "Idle", "Grounded", "Free Movement", "Base Layer.Locomotion", "Base Layer.Idle", "Base Layer.Grounded", "Base Layer.Locomotion.Free Movement", "Base Layer.Free Movement" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (animator.HasState(layer, num))
				{
					animator.CrossFadeInFixedTime(num, this.SmoothFinishDuration, layer);
					if (this.DebugMode)
					{
						Debug.Log(string.Format("[PlayerActionAnimator] Smooth crossfade -> {0} on layer {1} ({2:0.00}s)", array[i], layer, this.SmoothFinishDuration));
					}
					return true;
				}
			}
			if (layer != 0)
			{
				for (int j = 0; j < array.Length; j++)
				{
					int num2 = Animator.StringToHash(array[j]);
					if (animator.HasState(0, num2))
					{
						animator.CrossFadeInFixedTime(num2, this.SmoothFinishDuration, 0);
						if (this.DebugMode)
						{
							Debug.Log(string.Format("[PlayerActionAnimator] Smooth crossfade -> {0} on base layer (fallback) ({1:0.00}s)", array[j], this.SmoothFinishDuration));
						}
						return true;
					}
				}
			}
			if (this.DebugMode)
			{
				Debug.LogWarning(string.Format("[PlayerActionAnimator] Smooth finish: no candidate state found on layer {0} or base layer.", layer));
			}
			return false;
		}

		// Token: 0x06009519 RID: 38169 RVA: 0x003D3610 File Offset: 0x003D1810
		private void ForceRemoveCustomActionTag(int actionLayer)
		{
			vThirdPersonInput vThirdPersonInput = ((this.GenericAction != null) ? this.GenericAction.GetComponent<vThirdPersonInput>() : base.GetComponent<vThirdPersonInput>());
			if (vThirdPersonInput == null || vThirdPersonInput.cc == null)
			{
				return;
			}
			if (vThirdPersonInput.cc.animatorStateInfos != null)
			{
				vThirdPersonInput.cc.animatorStateInfos.RemoveStateInfo("CustomAction", actionLayer);
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[PlayerActionAnimator] Force-removed CustomAction tag from animatorStateInfos layer {0}", actionLayer));
				}
			}
			vThirdPersonInput.cc.customAction = false;
			Animator animator = vThirdPersonInput.cc.animator;
			if (animator != null && animator.isActiveAndEnabled && animator.gameObject.activeInHierarchy && animator.isInitialized && actionLayer >= 0 && actionLayer < animator.layerCount)
			{
				animator.Play(0, actionLayer, 0f);
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[PlayerActionAnimator] Force-played default state on layer {0} to flush stale pose", actionLayer));
				}
			}
			if (this.DebugMode)
			{
				Debug.Log("[PlayerActionAnimator] Force-cleared customAction flag on controller");
			}
		}

		// Token: 0x04009A27 RID: 39463
		[Header("Invector Integration")]
		[Tooltip("Reference to the player's vGenericAction component")]
		public vGenericAction GenericAction;

		// Token: 0x04009A28 RID: 39464
		[Header("Layer Override Fix")]
		[Tooltip("If true, temporarily disable emotion layers (Confident/Embarrassed) during actions")]
		public bool DisableEmotionLayersDuringAction = true;

		// Token: 0x04009A29 RID: 39465
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x04009A2A RID: 39466
		[Header("Exit Smoothing")]
		[Tooltip("If true, attempts a short crossfade to locomotion/idle when ending the action manually.")]
		public bool SmoothFinishAction = true;

		// Token: 0x04009A2B RID: 39467
		[Tooltip("Crossfade duration used when SmoothFinishAction is enabled.")]
		[Range(0f, 1f)]
		public float SmoothFinishDuration = 0.15f;

		// Token: 0x04009A2C RID: 39468
		private bool _isInitialized;

		// Token: 0x04009A2D RID: 39469
		private bool _didEnableRootMotion;

		// Token: 0x04009A2E RID: 39470
		private bool _savedUseRootMotion;

		// Token: 0x04009A2F RID: 39471
		private bool _savedAnimatorApplyRootMotion;
	}
}
