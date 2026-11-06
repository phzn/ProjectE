using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TintBodyPaint
{
	// Token: 0x02000C0E RID: 3086
	[RequireComponent(typeof(Collider))]
	public class TintPuddleInteractable : MonoBehaviour, IInteractable
	{
		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x0600593B RID: 22843 RVA: 0x0022E5C5 File Offset: 0x0022C7C5
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x0600593C RID: 22844 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x0022E5D0 File Offset: 0x0022C7D0
		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (!component.isTrigger)
			{
				component.isTrigger = true;
				Debug.LogWarning("[TintPuddleInteractable] Collider forced to isTrigger=true for detection.", this);
			}
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x0022E5FE File Offset: 0x0022C7FE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			TintPuddleInteractable.cachedPlayerController = null;
			TintPuddleInteractable.cachedTintController = null;
			TintPuddleInteractable.activePromptOwner = null;
			TintPuddleInteractable.PlayerOverlappingInteractables.Clear();
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x0022E61C File Offset: 0x0022C81C
		private void Update()
		{
			if (!this.isPlayerInRange)
			{
				return;
			}
			if (TintPuddleInteractable.activePromptOwner == this && !this.CanInteract(null))
			{
				TintPuddleInteractable.ReevaluateActivePromptOwner();
				return;
			}
			if (TintPuddleInteractable.activePromptOwner == this && this.CanInteract(null) && this.interactInput.GetButtonDown())
			{
				this.OnInteract(null);
			}
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x0022E678 File Offset: 0x0022C878
		private void OnTriggerEnter(Collider other)
		{
			this.RegisterPlayerCollider(other);
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x0022E681 File Offset: 0x0022C881
		private void OnTriggerStay(Collider other)
		{
			if (!this.overlappingPlayerColliders.Contains(other))
			{
				this.RegisterPlayerCollider(other);
			}
		}

		// Token: 0x06005942 RID: 22850 RVA: 0x0022E698 File Offset: 0x0022C898
		private void OnTriggerExit(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>(true);
			if (componentInParent == null || (TintPuddleInteractable.cachedPlayerController != null && componentInParent != TintPuddleInteractable.cachedPlayerController))
			{
				return;
			}
			if (!this.overlappingPlayerColliders.Remove(other) || this.overlappingPlayerColliders.Count > 0)
			{
				return;
			}
			this.isPlayerInRange = false;
			TintPuddleInteractable.PlayerOverlappingInteractables.Remove(this);
			this.HidePrompt();
			TintPuddleInteractable.ReevaluateActivePromptOwner();
			UnityEvent unityEvent = this.onPlayerExitRange;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[TintPuddleInteractable] Main Player exited tint trigger.", this);
			}
		}

		// Token: 0x06005943 RID: 22851 RVA: 0x0022E730 File Offset: 0x0022C930
		private void RegisterPlayerCollider(Collider other)
		{
			if (!TintPuddleInteractable.TryCachePlayer(other))
			{
				return;
			}
			bool flag = this.overlappingPlayerColliders.Count == 0;
			if (!this.overlappingPlayerColliders.Add(other))
			{
				return;
			}
			this.isPlayerInRange = true;
			TintPuddleInteractable.PlayerOverlappingInteractables.Add(this);
			TintPuddleInteractable.ReevaluateActivePromptOwner();
			if (flag)
			{
				UnityEvent unityEvent = this.onPlayerEnterRange;
				if (unityEvent != null)
				{
					unityEvent.Invoke();
				}
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[TintPuddleInteractable] Player collider entered tint trigger: " + other.name, this);
			}
		}

		// Token: 0x06005944 RID: 22852 RVA: 0x0022E7B0 File Offset: 0x0022C9B0
		private static bool TryCachePlayer(Collider other)
		{
			if (other == null)
			{
				return false;
			}
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>(true);
			if (componentInParent == null)
			{
				return false;
			}
			if (TintPuddleInteractable.cachedPlayerController != null && componentInParent != TintPuddleInteractable.cachedPlayerController)
			{
				return false;
			}
			TintPuddleInteractable.cachedPlayerController = componentInParent;
			if (TintPuddleInteractable.cachedTintController == null)
			{
				TintPuddleInteractable.cachedTintController = componentInParent.GetComponentInChildren<TintBodyPaintRuntimeController>(true);
			}
			return TintPuddleInteractable.cachedTintController != null;
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x0022E824 File Offset: 0x0022CA24
		private TintBodyPaintRuntimeController GetRuntimeController(GameObject player)
		{
			if (this.targetBodyPaintController != null)
			{
				return this.targetBodyPaintController;
			}
			if (TintPuddleInteractable.cachedTintController != null)
			{
				return TintPuddleInteractable.cachedTintController;
			}
			if (player != null)
			{
				vThirdPersonController vThirdPersonController = player.GetComponentInParent<vThirdPersonController>(true) ?? player.GetComponentInChildren<vThirdPersonController>(true);
				if (vThirdPersonController != null)
				{
					TintPuddleInteractable.cachedPlayerController = vThirdPersonController;
					TintPuddleInteractable.cachedTintController = vThirdPersonController.GetComponentInChildren<TintBodyPaintRuntimeController>(true);
				}
			}
			return TintPuddleInteractable.cachedTintController;
		}

		// Token: 0x06005946 RID: 22854 RVA: 0x0022E898 File Offset: 0x0022CA98
		public bool CanInteract(GameObject player)
		{
			if (!PlayerActivityGate.CanStartWorldInteraction)
			{
				return false;
			}
			TintBodyPaintRuntimeController runtimeController = this.GetRuntimeController(player);
			return !(TintPuddleInteractable.cachedPlayerController == null) && !TintPuddleInteractable.cachedPlayerController.lockMovement && !(runtimeController == null) && !runtimeController.IsTintBodyPaintActive();
		}

		// Token: 0x06005947 RID: 22855 RVA: 0x0022E8E8 File Offset: 0x0022CAE8
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			TintBodyPaintRuntimeController runtimeController = this.GetRuntimeController(player);
			if (runtimeController == null)
			{
				if (this.enableDebugLogs)
				{
					Debug.LogWarning("[TintPuddleInteractable] No TintBodyPaintRuntimeController found on interacting player.", this);
				}
				return;
			}
			if (!runtimeController.TryTriggerTintBodyPaint())
			{
				return;
			}
			UnityEvent unityEvent = this.onInteract;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			TintPuddleInteractable.ReevaluateActivePromptOwner();
			if (this.enableDebugLogs)
			{
				Debug.Log("[TintPuddleInteractable] Tint interaction executed.", this);
			}
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x0022E958 File Offset: 0x0022CB58
		private static void ReevaluateActivePromptOwner()
		{
			if (TintPuddleInteractable.cachedPlayerController == null)
			{
				return;
			}
			Vector3 position = TintPuddleInteractable.cachedPlayerController.transform.position;
			TintPuddleInteractable tintPuddleInteractable = null;
			float num = float.MaxValue;
			List<TintPuddleInteractable> list = new List<TintPuddleInteractable>();
			foreach (TintPuddleInteractable tintPuddleInteractable2 in TintPuddleInteractable.PlayerOverlappingInteractables)
			{
				if (tintPuddleInteractable2 == null || !tintPuddleInteractable2.isActiveAndEnabled || !tintPuddleInteractable2.isPlayerInRange)
				{
					list.Add(tintPuddleInteractable2);
				}
				else if (tintPuddleInteractable2.CanInteract(null))
				{
					float sqrMagnitude = (tintPuddleInteractable2.transform.position - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						tintPuddleInteractable = tintPuddleInteractable2;
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				TintPuddleInteractable.PlayerOverlappingInteractables.Remove(list[i]);
			}
			if (TintPuddleInteractable.activePromptOwner == tintPuddleInteractable)
			{
				return;
			}
			if (TintPuddleInteractable.activePromptOwner != null)
			{
				TintPuddleInteractable.activePromptOwner.HidePrompt();
			}
			TintPuddleInteractable.activePromptOwner = tintPuddleInteractable;
			if (TintPuddleInteractable.activePromptOwner != null)
			{
				TintPuddleInteractable.activePromptOwner.ShowPrompt();
			}
		}

		// Token: 0x06005949 RID: 22857 RVA: 0x0022EA94 File Offset: 0x0022CC94
		private void ShowPrompt()
		{
			if (this.promptVisible)
			{
				return;
			}
			if (this.promptUI == null || this.interactionConfig == null)
			{
				return;
			}
			this.promptUI.Show(this.interactionConfig);
			this.promptVisible = true;
		}

		// Token: 0x0600594A RID: 22858 RVA: 0x0022EAD4 File Offset: 0x0022CCD4
		private void HidePrompt()
		{
			if (!this.promptVisible)
			{
				return;
			}
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			this.promptVisible = false;
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnActionExecuted(string actionName, GameObject player)
		{
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x0022EAFF File Offset: 0x0022CCFF
		private void OnDisable()
		{
			this.isPlayerInRange = false;
			this.overlappingPlayerColliders.Clear();
			TintPuddleInteractable.PlayerOverlappingInteractables.Remove(this);
			this.HidePrompt();
			if (TintPuddleInteractable.activePromptOwner == this)
			{
				TintPuddleInteractable.activePromptOwner = null;
				TintPuddleInteractable.ReevaluateActivePromptOwner();
			}
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x0022EB3D File Offset: 0x0022CD3D
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.5f, this.interactionDistance);
		}

		// Token: 0x040059E4 RID: 23012
		[Header("Configuration")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x040059E5 RID: 23013
		[Header("Input")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x040059E6 RID: 23014
		[Header("Detection")]
		[SerializeField]
		private float interactionDistance = 2f;

		// Token: 0x040059E7 RID: 23015
		[Header("UI")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040059E8 RID: 23016
		[Header("Action (Optional Override)")]
		[Tooltip("Direct reference to the player's controller. If null, will be auto-detected on the interacting player.")]
		[SerializeField]
		private TintBodyPaintRuntimeController targetBodyPaintController;

		// Token: 0x040059E9 RID: 23017
		[Header("Events")]
		[SerializeField]
		private UnityEvent onPlayerEnterRange;

		// Token: 0x040059EA RID: 23018
		[SerializeField]
		private UnityEvent onPlayerExitRange;

		// Token: 0x040059EB RID: 23019
		[SerializeField]
		private UnityEvent onInteract;

		// Token: 0x040059EC RID: 23020
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x040059ED RID: 23021
		private static readonly HashSet<TintPuddleInteractable> PlayerOverlappingInteractables = new HashSet<TintPuddleInteractable>();

		// Token: 0x040059EE RID: 23022
		private static TintPuddleInteractable activePromptOwner;

		// Token: 0x040059EF RID: 23023
		private bool isPlayerInRange;

		// Token: 0x040059F0 RID: 23024
		private bool promptVisible;

		// Token: 0x040059F1 RID: 23025
		private readonly HashSet<Collider> overlappingPlayerColliders = new HashSet<Collider>();

		// Token: 0x040059F2 RID: 23026
		private static vThirdPersonController cachedPlayerController;

		// Token: 0x040059F3 RID: 23027
		private static TintBodyPaintRuntimeController cachedTintController;
	}
}
