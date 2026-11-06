using System;
using Features.Detection;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.JumpOver
{
	// Token: 0x020010E2 RID: 4322
	[DisallowMultipleComponent]
	public class JumpOverInteractable : MonoBehaviour
	{
		// Token: 0x06008312 RID: 33554 RVA: 0x00357ABC File Offset: 0x00355CBC
		private void Awake()
		{
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Transform transform = ((base.transform.root != null) ? base.transform.root : base.transform);
				this.triggers = transform.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[JumpOverInteractable] No vTriggerGenericAction found in hierarchy of '" + base.name + "'.", this);
			}
			if (this.promptUI != null && this.interactionConfig != null)
			{
				this.promptUI.Show(this.interactionConfig);
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008313 RID: 33555 RVA: 0x00357B94 File Offset: 0x00355D94
		private void OnEnable()
		{
			if (this.triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleValidate));
					vTriggerGenericAction.OnInvalidate.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnPlayerExit.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnStartAnimation.AddListener(new UnityAction(this.HandleAnimationStart));
					vTriggerGenericAction.OnEndAnimation.AddListener(new UnityAction(this.HandleAnimationEnd));
					vTriggerGenericAction.OnCancelAction.AddListener(new UnityAction(this.HandleAnimationEnd));
				}
			}
		}

		// Token: 0x06008314 RID: 33556 RVA: 0x00357C60 File Offset: 0x00355E60
		private void OnDisable()
		{
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnStartAnimation.RemoveListener(new UnityAction(this.HandleAnimationStart));
						vTriggerGenericAction.OnEndAnimation.RemoveListener(new UnityAction(this.HandleAnimationEnd));
						vTriggerGenericAction.OnCancelAction.RemoveListener(new UnityAction(this.HandleAnimationEnd));
					}
				}
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this.ForceRestoreRootMotion();
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008315 RID: 33557 RVA: 0x00357DA0 File Offset: 0x00355FA0
		private void HandleValidate(GameObject player)
		{
			this._activePlayer = player;
			this._activeActionAnimator = ((player != null) ? player.GetComponentInChildren<PlayerActionAnimator>() : null);
			vGenericAction vGenericAction = ((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			this.EnsureGenericActionSubscription(vGenericAction);
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[JumpOverInteractable] Validate: player=",
					(player != null) ? player.name : null,
					" actionAnimator=",
					(this._activeActionAnimator != null) ? "OK" : "NULL",
					" playerGenericAction=",
					(vGenericAction != null) ? "OK" : "NULL"
				}), this);
			}
		}

		// Token: 0x06008316 RID: 33558 RVA: 0x00357E7C File Offset: 0x0035607C
		private void EnsureGenericActionSubscription(vGenericAction playerGenericAction)
		{
			if (playerGenericAction == this._activeGenericAction && this._genericActionSubscribed)
			{
				return;
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this._activeGenericAction = playerGenericAction;
			if (this._activeGenericAction != null && this._activeGenericAction.OnStartAction != null)
			{
				this._activeGenericAction.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = true;
			}
		}

		// Token: 0x06008317 RID: 33559 RVA: 0x00357F1E File Offset: 0x0035611E
		private void HandlePlayerActionStart(vTriggerGenericAction firingTrigger)
		{
			if (!this.IsOwnedTrigger(firingTrigger))
			{
				return;
			}
			this.EnableRootMotion();
		}

		// Token: 0x06008318 RID: 33560 RVA: 0x00357F30 File Offset: 0x00356130
		private bool IsOwnedTrigger(vTriggerGenericAction t)
		{
			if (t == null || this.triggers == null)
			{
				return false;
			}
			for (int i = 0; i < this.triggers.Length; i++)
			{
				if (this.triggers[i] == t)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008319 RID: 33561 RVA: 0x00357F78 File Offset: 0x00356178
		private void EnableRootMotion()
		{
			if (this._rootMotionPushed)
			{
				return;
			}
			if (this._activeActionAnimator == null)
			{
				this._activeActionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._activeActionAnimator == null)
			{
				Debug.LogError("[JumpOverInteractable] OnStartAction fired but no PlayerActionAnimator found in scene. JumpOver will play without root motion.", this);
				return;
			}
			this._activeActionAnimator.DisableEmotionLayers();
			this._activeActionAnimator.EnableRootMotionForAction();
			this._rootMotionPushed = true;
			if (this.debugLogs)
			{
				Debug.Log("[JumpOverInteractable] Root motion enabled + emotion layers disabled for JumpOver", this);
			}
		}

		// Token: 0x0600831A RID: 33562 RVA: 0x00357FF4 File Offset: 0x003561F4
		private void HandleInvalidate(GameObject player)
		{
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			if (this.debugLogs)
			{
				Debug.Log("[JumpOverInteractable] Invalidate (player=" + ((player != null) ? player.name : null) + ")", this);
			}
		}

		// Token: 0x0600831B RID: 33563 RVA: 0x00358049 File Offset: 0x00356249
		private void HandleAnimationStart()
		{
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this.EnableRootMotion();
		}

		// Token: 0x0600831C RID: 33564 RVA: 0x00358082 File Offset: 0x00356282
		private void HandleAnimationEnd()
		{
			this.ForceRestoreRootMotion();
		}

		// Token: 0x0600831D RID: 33565 RVA: 0x0035808C File Offset: 0x0035628C
		private void ForceRestoreRootMotion()
		{
			if (!this._rootMotionPushed)
			{
				return;
			}
			if (this._activeActionAnimator != null)
			{
				this._activeActionAnimator.RestoreRootMotionForAction();
				this._activeActionAnimator.RestoreOverrideLayer();
			}
			this._rootMotionPushed = false;
			if (this.debugLogs)
			{
				Debug.Log("[JumpOverInteractable] Root motion + emotion layers restored", this);
			}
		}

		// Token: 0x04008631 RID: 34353
		[Header("References")]
		[Tooltip("All vTriggerGenericAction siblings that drive the JumpOver animation for this prefab. The default JumpOver prefab ships with TWO triggers (one per side of the obstacle) so the player can jump it from either direction — both need to be wired here, otherwise root motion only kicks in when crossing from the wired side. Auto-resolved at Awake if empty.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x04008632 RID: 34354
		[Tooltip("World-space prompt shown while the player is in range. Auto-resolved from child components if left empty.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04008633 RID: 34355
		[Header("UI Config")]
		[Tooltip("Text + key labels shown by the prompt UI. Required.")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04008634 RID: 34356
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008635 RID: 34357
		private GameObject _activePlayer;

		// Token: 0x04008636 RID: 34358
		private PlayerActionAnimator _activeActionAnimator;

		// Token: 0x04008637 RID: 34359
		private vGenericAction _activeGenericAction;

		// Token: 0x04008638 RID: 34360
		private bool _genericActionSubscribed;

		// Token: 0x04008639 RID: 34361
		private bool _rootMotionPushed;
	}
}
