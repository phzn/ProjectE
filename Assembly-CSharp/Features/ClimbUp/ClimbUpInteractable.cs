using System;
using Features.Detection;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using VehicleSpawnPlugin;

namespace Features.ClimbUp
{
	// Token: 0x02001351 RID: 4945
	[DisallowMultipleComponent]
	public class ClimbUpInteractable : MonoBehaviour
	{
		// Token: 0x060098F6 RID: 39158 RVA: 0x003EB6A4 File Offset: 0x003E98A4
		private void Awake()
		{
			this.CacheEventCallbacks();
			if (this.triggers == null || this.triggers.Length == 0)
			{
				this.triggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.promptUI == null)
			{
				VehicleAI componentInParent = base.GetComponentInParent<VehicleAI>();
				if (componentInParent != null)
				{
					this.promptUI = componentInParent.GetComponentInChildren<InteractablePromptUI>(true);
				}
			}
			this.NormalizePromptScale();
			this.CacheTriggerColliders();
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[ClimbUpInteractable] No vTriggerGenericAction found in hierarchy of '" + base.name + "'.", this);
			}
			if (this.promptUI != null && this.interactionConfig != null)
			{
				this.promptUI.Hide();
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x060098F7 RID: 39159 RVA: 0x003EB78C File Offset: 0x003E998C
		private void CacheTriggerColliders()
		{
			if (this.triggers == null)
			{
				this._triggerColliders = null;
				return;
			}
			this._triggerColliders = new Collider[this.triggers.Length];
			for (int i = 0; i < this.triggers.Length; i++)
			{
				this._triggerColliders[i] = ((this.triggers[i] != null) ? this.triggers[i].GetComponent<Collider>() : null);
			}
		}

		// Token: 0x060098F8 RID: 39160 RVA: 0x003EB7F8 File Offset: 0x003E99F8
		private void LateUpdate()
		{
			this.RefreshPrompt(false);
			if (!this.followPlayerAlongTrigger || this.promptUI == null)
			{
				return;
			}
			if (!this.promptUI.gameObject.activeSelf)
			{
				return;
			}
			if (this._activePlayer == null)
			{
				return;
			}
			Vector3 position = this._activePlayer.transform.position;
			float num = position.y;
			float num2 = float.PositiveInfinity;
			int num3 = 0;
			while (this._triggerColliders != null && num3 < this._triggerColliders.Length)
			{
				Collider collider = this._triggerColliders[num3];
				if (!(collider == null) && collider.enabled)
				{
					Vector3 vector = collider.ClosestPoint(position);
					float sqrMagnitude = (vector - position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						num = vector.y;
					}
				}
				num3++;
			}
			this.promptUI.transform.position = new Vector3(position.x, num + this.promptHeightOffset, position.z);
		}

		// Token: 0x060098F9 RID: 39161 RVA: 0x003EB8F4 File Offset: 0x003E9AF4
		private void RefreshPrompt(bool explicitlySelected = false)
		{
			if (!this._acceptTriggerEvents || this.promptUI == null || this.interactionConfig == null)
			{
				return;
			}
			vTriggerGenericAction vTriggerGenericAction = ((this._activeGenericAction != null) ? this._activeGenericAction.triggerAction : null);
			if (!(this._activeGenericAction != null) || this._activeGenericAction.doingAction || !(vTriggerGenericAction != null) || !vTriggerGenericAction.isActiveAndEnabled || !vTriggerGenericAction.CanDoAction || !this.IsOwnedTrigger(vTriggerGenericAction) || !PlayerActivityGate.CanStartWorldInteraction)
			{
				if (this.promptUI.IsVisible)
				{
					this.promptUI.Hide();
				}
				return;
			}
			if (!this.promptUI.IsVisible)
			{
				if (!explicitlySelected && InteractablePromptUI.HasVisiblePromptOtherThan(this.promptUI))
				{
					return;
				}
				this.NormalizePromptScale();
				this.promptUI.gameObject.SetActive(true);
				this.promptUI.Show(this.interactionConfig);
			}
		}

		// Token: 0x060098FA RID: 39162 RVA: 0x003EB9E9 File Offset: 0x003E9BE9
		private void OnEnable()
		{
			this.SubscribeTriggerEvents();
			this._acceptTriggerEvents = true;
			this.NormalizePromptScale();
		}

		// Token: 0x060098FB RID: 39163 RVA: 0x003EB9FE File Offset: 0x003E9BFE
		private void NormalizePromptScale()
		{
			if (this.promptUI == null)
			{
				return;
			}
			ClimbUpInteractable.SetUniformWorldScale(this.promptUI.transform, Mathf.Max(0.0001f, this.promptWorldScale));
		}

		// Token: 0x060098FC RID: 39164 RVA: 0x003EBA30 File Offset: 0x003E9C30
		private static void SetUniformWorldScale(Transform target, float worldScale)
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(ClimbUpInteractable.DivideBySignedScale(worldScale, vector.x), ClimbUpInteractable.DivideBySignedScale(worldScale, vector.y), ClimbUpInteractable.DivideBySignedScale(worldScale, vector.z));
		}

		// Token: 0x060098FD RID: 39165 RVA: 0x003EBA98 File Offset: 0x003E9C98
		private static float DivideBySignedScale(float worldScale, float parentScale)
		{
			float num = ((Mathf.Abs(parentScale) >= 0.0001f) ? parentScale : ((parentScale < 0f) ? (-0.0001f) : 0.0001f));
			return worldScale / num;
		}

		// Token: 0x060098FE RID: 39166 RVA: 0x003EBAD0 File Offset: 0x003E9CD0
		private void CacheEventCallbacks()
		{
			if (this._validateCallback == null)
			{
				this._validateCallback = new UnityAction<GameObject>(this.HandleValidate);
			}
			if (this._invalidateCallback == null)
			{
				this._invalidateCallback = new UnityAction<GameObject>(this.HandleInvalidate);
			}
			if (this._animationStartCallback == null)
			{
				this._animationStartCallback = new UnityAction(this.HandleAnimationStart);
			}
			if (this._animationEndCallback == null)
			{
				this._animationEndCallback = new UnityAction(this.HandleAnimationEnd);
			}
			if (this._playerActionStartCallback == null)
			{
				this._playerActionStartCallback = new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart);
			}
		}

		// Token: 0x060098FF RID: 39167 RVA: 0x003EBB60 File Offset: 0x003E9D60
		private void SubscribeTriggerEvents()
		{
			if (this._triggerEventsSubscribed || this.triggers == null)
			{
				return;
			}
			this.CacheEventCallbacks();
			for (int i = 0; i < this.triggers.Length; i++)
			{
				vTriggerGenericAction vTriggerGenericAction = this.triggers[i];
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(this._validateCallback);
					vTriggerGenericAction.OnInvalidate.AddListener(this._invalidateCallback);
					vTriggerGenericAction.OnPlayerExit.AddListener(this._invalidateCallback);
					vTriggerGenericAction.OnStartAnimation.AddListener(this._animationStartCallback);
					vTriggerGenericAction.OnEndAnimation.AddListener(this._animationEndCallback);
					vTriggerGenericAction.OnCancelAction.AddListener(this._animationEndCallback);
				}
			}
			this._triggerEventsSubscribed = true;
		}

		// Token: 0x06009900 RID: 39168 RVA: 0x003EBC1C File Offset: 0x003E9E1C
		private void OnDisable()
		{
			this._acceptTriggerEvents = false;
			this.UnsubscribeGenericAction();
			this.ForceRestoreEmotionLayers();
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009901 RID: 39169 RVA: 0x003EBC6D File Offset: 0x003E9E6D
		private void OnDestroy()
		{
			this._acceptTriggerEvents = false;
			this.UnsubscribeTriggerEvents();
			this.UnsubscribeGenericAction();
		}

		// Token: 0x06009902 RID: 39170 RVA: 0x003EBC84 File Offset: 0x003E9E84
		private void UnsubscribeTriggerEvents()
		{
			if (!this._triggerEventsSubscribed)
			{
				return;
			}
			this._triggerEventsSubscribed = false;
			if (this.triggers == null)
			{
				return;
			}
			this.CacheEventCallbacks();
			for (int i = 0; i < this.triggers.Length; i++)
			{
				vTriggerGenericAction vTriggerGenericAction = this.triggers[i];
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.RemoveListener(this._validateCallback);
					vTriggerGenericAction.OnInvalidate.RemoveListener(this._invalidateCallback);
					vTriggerGenericAction.OnPlayerExit.RemoveListener(this._invalidateCallback);
					vTriggerGenericAction.OnStartAnimation.RemoveListener(this._animationStartCallback);
					vTriggerGenericAction.OnEndAnimation.RemoveListener(this._animationEndCallback);
					vTriggerGenericAction.OnCancelAction.RemoveListener(this._animationEndCallback);
				}
			}
		}

		// Token: 0x06009903 RID: 39171 RVA: 0x003EBD40 File Offset: 0x003E9F40
		private void UnsubscribeGenericAction()
		{
			if (!this._genericActionSubscribed)
			{
				return;
			}
			this._genericActionSubscribed = false;
			this.CacheEventCallbacks();
			if (this._activeGenericAction != null && this._activeGenericAction.OnStartAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(this._playerActionStartCallback);
			}
		}

		// Token: 0x06009904 RID: 39172 RVA: 0x003EBD94 File Offset: 0x003E9F94
		private void HandleValidate(GameObject player)
		{
			if (!this._acceptTriggerEvents)
			{
				return;
			}
			this._activePlayer = player;
			this._activeActionAnimator = ((player != null) ? player.GetComponentInChildren<PlayerActionAnimator>() : null);
			vGenericAction vGenericAction = ((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			this.EnsureGenericActionSubscription(vGenericAction);
			if (this.promptUI != null)
			{
				this.NormalizePromptScale();
				if (this.interactionConfig != null)
				{
					this.RefreshPrompt(true);
				}
				else
				{
					this.promptUI.gameObject.SetActive(true);
				}
			}
			this.LogPlayerState(player, "Validate");
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ClimbUpInteractable] Validate: player=",
					(player != null) ? player.name : null,
					" actionAnimator=",
					(this._activeActionAnimator != null) ? "OK" : "NULL",
					" playerGenericAction=",
					(vGenericAction != null) ? "OK" : "NULL"
				}), this);
			}
		}

		// Token: 0x06009905 RID: 39173 RVA: 0x003EBEA0 File Offset: 0x003EA0A0
		private void LogPlayerState(GameObject player, string when)
		{
			if (player == null)
			{
				return;
			}
			vThirdPersonController componentInChildren = player.GetComponentInChildren<vThirdPersonController>();
			vSwimming componentInChildren2 = player.GetComponentInChildren<vSwimming>();
			vGenericAction componentInChildren3 = player.GetComponentInChildren<vGenericAction>();
			Rigidbody rigidbody = ((componentInChildren != null) ? componentInChildren._rigidbody : null);
			Debug.Log(string.Concat(new string[]
			{
				"[ClimbUpDIAG] ",
				when,
				": customAction=",
				(componentInChildren != null) ? componentInChildren.customAction.ToString() : "n/a",
				" kinematic=",
				(rigidbody != null) ? rigidbody.isKinematic.ToString() : "n/a",
				" useGravity=",
				(rigidbody != null) ? rigidbody.useGravity.ToString() : "n/a",
				" disableCheckGround=",
				(componentInChildren != null) ? componentInChildren.disableCheckGround.ToString() : "n/a",
				" disableSnapToGround=",
				(componentInChildren != null) ? componentInChildren.disableSnapToGround.ToString() : "n/a",
				" lockSetMoveSpeed=",
				(componentInChildren != null) ? componentInChildren.lockSetMoveSpeed.ToString() : "n/a",
				" isGrounded=",
				(componentInChildren != null) ? componentInChildren.isGrounded.ToString() : "n/a",
				" inTheWater=",
				(componentInChildren2 != null) ? componentInChildren2.inTheWater.ToString() : "n/a",
				" isSwimming=",
				(componentInChildren2 != null) ? componentInChildren2.isSwimming.ToString() : "n/a",
				" doingAction=",
				(componentInChildren3 != null) ? componentInChildren3.doingAction.ToString() : "n/a"
			}), this);
		}

		// Token: 0x06009906 RID: 39174 RVA: 0x003EC0A8 File Offset: 0x003EA2A8
		private void EnsureGenericActionSubscription(vGenericAction playerGenericAction)
		{
			if (playerGenericAction == this._activeGenericAction && this._genericActionSubscribed)
			{
				return;
			}
			this.UnsubscribeGenericAction();
			this._activeGenericAction = playerGenericAction;
			if (this._activeGenericAction != null && this._activeGenericAction.OnStartAction != null)
			{
				this.CacheEventCallbacks();
				this._activeGenericAction.OnStartAction.AddListener(this._playerActionStartCallback);
				this._genericActionSubscribed = true;
			}
		}

		// Token: 0x06009907 RID: 39175 RVA: 0x003EC118 File Offset: 0x003EA318
		private void HandlePlayerActionStart(vTriggerGenericAction firingTrigger)
		{
			if (!this._acceptTriggerEvents)
			{
				return;
			}
			if (!this.IsOwnedTrigger(firingTrigger))
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[ClimbUpInteractable] ClimbUp action STARTED (trigger='" + ((firingTrigger != null) ? firingTrigger.name : null) + "').", this);
			}
			this.DisableEmotionLayers();
		}

		// Token: 0x06009908 RID: 39176 RVA: 0x003EC168 File Offset: 0x003EA368
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

		// Token: 0x06009909 RID: 39177 RVA: 0x003EC1B0 File Offset: 0x003EA3B0
		private void DisableEmotionLayers()
		{
			if (this._emotionLayersPushed)
			{
				return;
			}
			if (this._activeActionAnimator == null)
			{
				this._activeActionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._activeActionAnimator == null)
			{
				Debug.LogError("[ClimbUpInteractable] OnStartAction fired but no PlayerActionAnimator found in scene. ClimbUp will play with the emotion layers overriding the pose.", this);
				return;
			}
			this._activeActionAnimator.DisableEmotionLayers();
			this._emotionLayersPushed = true;
			if (this.debugLogs)
			{
				Debug.Log("[ClimbUpInteractable] Emotion layers disabled for ClimbUp", this);
			}
		}

		// Token: 0x0600990A RID: 39178 RVA: 0x003EC220 File Offset: 0x003EA420
		private void HandleInvalidate(GameObject player)
		{
			if (!this._acceptTriggerEvents)
			{
				return;
			}
			if (this.promptUI != null && this.interactionConfig == null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			if (this.debugLogs)
			{
				Debug.Log("[ClimbUpInteractable] Invalidate (player=" + ((player != null) ? player.name : null) + ")", this);
			}
		}

		// Token: 0x0600990B RID: 39179 RVA: 0x003EC28C File Offset: 0x003EA48C
		private void HandleAnimationStart()
		{
			if (!this._acceptTriggerEvents)
			{
				return;
			}
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this.DisableEmotionLayers();
		}

		// Token: 0x0600990C RID: 39180 RVA: 0x003EC2D9 File Offset: 0x003EA4D9
		private void HandleAnimationEnd()
		{
			if (!this._acceptTriggerEvents)
			{
				return;
			}
			this.ForceRestoreEmotionLayers();
		}

		// Token: 0x0600990D RID: 39181 RVA: 0x003EC2EA File Offset: 0x003EA4EA
		private void ForceRestoreEmotionLayers()
		{
			if (!this._emotionLayersPushed)
			{
				return;
			}
			if (this._activeActionAnimator != null)
			{
				this._activeActionAnimator.RestoreOverrideLayer();
			}
			this._emotionLayersPushed = false;
			if (this.debugLogs)
			{
				Debug.Log("[ClimbUpInteractable] Emotion layers restored", this);
			}
		}

		// Token: 0x04009E07 RID: 40455
		[Header("References")]
		[Tooltip("All vTriggerGenericAction siblings that drive the ClimbUp animation for this prefab. The default ClimbUp prefab ships with FOUR triggers (one per climb height); they need to all be wired here, otherwise the emotion-layer suppression only kicks in for the heights you listed. Auto-resolved at Awake if empty.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x04009E08 RID: 40456
		[Tooltip("World-space prompt shown while the player is in range. Auto-resolved from child components if left empty. Leave both this and interactionConfig null to keep the Invector vanilla vActionText canvases instead.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04009E09 RID: 40457
		[Header("UI Config")]
		[Tooltip("Text + key labels shown by the prompt UI. Optional — leave null to keep the Invector built-in prompt.")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04009E0A RID: 40458
		[Header("Prompt Follow")]
		[Tooltip("The climb triggers here are LONG box colliders running along the pool edge — the player swims along them and climbs out at whatever point they're nearest. So the prompt can't sit at a fixed trigger centre (it would float far away); instead it tracks the point on the active trigger collider nearest the player each frame.")]
		[SerializeField]
		private bool followPlayerAlongTrigger = true;

		// Token: 0x04009E0B RID: 40459
		[Tooltip("World-space vertical offset added to the climb point so the prompt floats above the pool edge / the player's head instead of at water level.")]
		[SerializeField]
		private float promptHeightOffset = 1.8f;

		// Token: 0x04009E0C RID: 40460
		[Tooltip("Final world-space scale of the shared interaction card. Normalized at runtime so the strongly stretched ClimbUp parent cannot squash or offset the UI.")]
		[SerializeField]
		private float promptWorldScale = 0.0055f;

		// Token: 0x04009E0D RID: 40461
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04009E0E RID: 40462
		private GameObject _activePlayer;

		// Token: 0x04009E0F RID: 40463
		private PlayerActionAnimator _activeActionAnimator;

		// Token: 0x04009E10 RID: 40464
		private vGenericAction _activeGenericAction;

		// Token: 0x04009E11 RID: 40465
		private bool _genericActionSubscribed;

		// Token: 0x04009E12 RID: 40466
		private bool _emotionLayersPushed;

		// Token: 0x04009E13 RID: 40467
		private Collider[] _triggerColliders;

		// Token: 0x04009E14 RID: 40468
		private UnityAction<GameObject> _validateCallback;

		// Token: 0x04009E15 RID: 40469
		private UnityAction<GameObject> _invalidateCallback;

		// Token: 0x04009E16 RID: 40470
		private UnityAction _animationStartCallback;

		// Token: 0x04009E17 RID: 40471
		private UnityAction _animationEndCallback;

		// Token: 0x04009E18 RID: 40472
		private UnityAction<vTriggerGenericAction> _playerActionStartCallback;

		// Token: 0x04009E19 RID: 40473
		private bool _triggerEventsSubscribed;

		// Token: 0x04009E1A RID: 40474
		private bool _acceptTriggerEvents;
	}
}
