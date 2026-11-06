using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000745 RID: 1861
	[RequireComponent(typeof(Collider))]
	public class MudPuddleInteractable : MonoBehaviour, IInteractable
	{
		// Token: 0x06003027 RID: 12327 RVA: 0x00110EFB File Offset: 0x0010F0FB
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.MudPuddleInteractable_interactionConfig)
			{
				return this.interactionConfig;
			}
			throw new InvalidOperationException("Unsupported MudPuddleInteractable POI binding: " + field.ToString());
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x00110F25 File Offset: 0x0010F125
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.MudPuddleInteractable_interactionConfig)
			{
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported MudPuddleInteractable POI binding: " + field.ToString());
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x00110F55 File Offset: 0x0010F155
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x00110F5D File Offset: 0x0010F15D
		private void Awake()
		{
			this.interactionCollider = base.GetComponent<Collider>();
			if (!this.interactionCollider.isTrigger)
			{
				this.interactionCollider.isTrigger = true;
				Debug.LogWarning("[MudPuddleInteractable] Collider forced to isTrigger=true for detection.", this);
			}
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x00110F8F File Offset: 0x0010F18F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			MudPuddleInteractable.cachedPlayerController = null;
			MudPuddleInteractable.cachedMudController = null;
			MudPuddleInteractable.activePromptOwner = null;
			MudPuddleInteractable.PlayerOverlappingInteractables.Clear();
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x00110FB0 File Offset: 0x0010F1B0
		private void Update()
		{
			if (!this.isPlayerInRange)
			{
				return;
			}
			if (MudPuddleInteractable.activePromptOwner == null || (MudPuddleInteractable.activePromptOwner == this && !this.CanInteract(null)))
			{
				MudPuddleInteractable.ReevaluateActivePromptOwner();
			}
			if (MudPuddleInteractable.activePromptOwner == this && this.CanInteract(null))
			{
				if (this.promptUI != null && !this.promptUI.IsVisible && !InteractablePromptUI.HasVisiblePromptOtherThan(this.promptUI))
				{
					this.ShowPrompt();
				}
				if (this.interactInput.GetButtonDown())
				{
					this.OnInteract(null);
				}
			}
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00111046 File Offset: 0x0010F246
		private void OnTriggerEnter(Collider other)
		{
			this.RegisterPlayerCollider(other);
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x0011104F File Offset: 0x0010F24F
		private void OnTriggerStay(Collider other)
		{
			if (!this.overlappingPlayerColliders.Contains(other))
			{
				this.RegisterPlayerCollider(other);
			}
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x00111068 File Offset: 0x0010F268
		private void OnTriggerExit(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>(true);
			if (componentInParent == null || (MudPuddleInteractable.cachedPlayerController != null && componentInParent != MudPuddleInteractable.cachedPlayerController))
			{
				return;
			}
			if (!this.overlappingPlayerColliders.Remove(other) || this.overlappingPlayerColliders.Count > 0)
			{
				return;
			}
			this.isPlayerInRange = false;
			MudPuddleInteractable.PlayerOverlappingInteractables.Remove(this);
			this.HidePrompt();
			MudPuddleInteractable.ReevaluateActivePromptOwner();
			UnityEvent unityEvent = this.onPlayerExitRange;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[MudPuddleInteractable] Main Player exited puddle trigger.", this);
			}
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x00111100 File Offset: 0x0010F300
		private void RegisterPlayerCollider(Collider other)
		{
			if (!MudPuddleInteractable.TryCachePlayer(other))
			{
				return;
			}
			bool flag = this.overlappingPlayerColliders.Count == 0;
			if (!this.overlappingPlayerColliders.Add(other))
			{
				return;
			}
			this.isPlayerInRange = true;
			MudPuddleInteractable.PlayerOverlappingInteractables.Add(this);
			MudPuddleInteractable.ReevaluateActivePromptOwner();
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
				Debug.Log("[MudPuddleInteractable] Player collider entered puddle trigger: " + other.name, this);
			}
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x00111180 File Offset: 0x0010F380
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
			if (MudPuddleInteractable.cachedPlayerController != null && componentInParent != MudPuddleInteractable.cachedPlayerController)
			{
				return false;
			}
			MudPuddleInteractable.cachedPlayerController = componentInParent;
			if (MudPuddleInteractable.cachedMudController == null)
			{
				MudPuddleInteractable.cachedMudController = componentInParent.GetComponentInChildren<MudBodyPaintRuntimeController>(true);
			}
			return MudPuddleInteractable.cachedMudController != null;
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x001111F4 File Offset: 0x0010F3F4
		private MudBodyPaintRuntimeController GetRuntimeController(GameObject player)
		{
			if (this.targetBodyPaintController != null)
			{
				return this.targetBodyPaintController;
			}
			if (MudPuddleInteractable.cachedMudController != null)
			{
				return MudPuddleInteractable.cachedMudController;
			}
			if (player != null)
			{
				vThirdPersonController vThirdPersonController = player.GetComponentInParent<vThirdPersonController>(true) ?? player.GetComponentInChildren<vThirdPersonController>(true);
				if (vThirdPersonController != null)
				{
					MudPuddleInteractable.cachedPlayerController = vThirdPersonController;
					MudPuddleInteractable.cachedMudController = vThirdPersonController.GetComponentInChildren<MudBodyPaintRuntimeController>(true);
				}
			}
			return MudPuddleInteractable.cachedMudController;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x00111268 File Offset: 0x0010F468
		public bool CanInteract(GameObject player)
		{
			if (!this.isPlayerInRange || !PlayerActivityGate.CanStartWorldInteraction)
			{
				return false;
			}
			MudBodyPaintRuntimeController runtimeController = this.GetRuntimeController(player);
			return !(MudPuddleInteractable.cachedPlayerController == null) && !MudPuddleInteractable.cachedPlayerController.lockMovement && !(runtimeController == null) && this.IsPlayerWithinReach() && !runtimeController.IsMudBodyPaintActive();
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x001112CC File Offset: 0x0010F4CC
		private bool IsPlayerWithinReach()
		{
			Vector3 position = MudPuddleInteractable.cachedPlayerController.transform.position;
			float num = ((this.interactionDistance > 0f) ? this.interactionDistance : 2f);
			if ((position - base.transform.position).sqrMagnitude > num * num)
			{
				return false;
			}
			if (this.interactionCollider == null)
			{
				this.interactionCollider = base.GetComponent<Collider>();
			}
			if (this.interactionCollider == null || !this.interactionCollider.enabled)
			{
				return false;
			}
			Vector3 vector = position + Vector3.up * 0.5f;
			BoxCollider boxCollider = this.interactionCollider as BoxCollider;
			if (boxCollider != null)
			{
				Vector3 vector2 = boxCollider.transform.InverseTransformPoint(vector) - boxCollider.center;
				Vector3 vector3 = boxCollider.size * 0.5f;
				if (Mathf.Abs(vector2.x) > vector3.x || Mathf.Abs(vector2.y) > vector3.y || Mathf.Abs(vector2.z) > vector3.z)
				{
					return false;
				}
			}
			else if ((this.interactionCollider.ClosestPoint(vector) - vector).sqrMagnitude > 0.0025f)
			{
				return false;
			}
			Vector3 vector4 = base.transform.position + Vector3.up * 0.5f - vector;
			float magnitude = vector4.magnitude;
			if (magnitude <= 0.01f)
			{
				return true;
			}
			int num2 = Physics.RaycastNonAlloc(vector, vector4 / magnitude, MudPuddleInteractable.ReachHits, magnitude, -5, QueryTriggerInteraction.Ignore);
			if (num2 == MudPuddleInteractable.ReachHits.Length)
			{
				return false;
			}
			for (int i = 0; i < num2; i++)
			{
				Transform transform = MudPuddleInteractable.ReachHits[i].transform;
				if (transform != null && !transform.IsChildOf(MudPuddleInteractable.cachedPlayerController.transform) && !transform.IsChildOf(base.transform))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x001114C8 File Offset: 0x0010F6C8
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			MudBodyPaintRuntimeController runtimeController = this.GetRuntimeController(player);
			if (runtimeController == null)
			{
				if (this.enableDebugLogs)
				{
					Debug.LogWarning("[MudPuddleInteractable] No MudBodyPaintRuntimeController found on interacting player.", this);
				}
				return;
			}
			if (!runtimeController.TryTriggerMudBodyPaint(this.mudDurationOverrideSeconds))
			{
				return;
			}
			UnityEvent unityEvent = this.onInteract;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			MudPuddleInteractable.ReevaluateActivePromptOwner();
			if (this.enableDebugLogs)
			{
				Debug.Log("[MudPuddleInteractable] Mud interaction executed.", this);
			}
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x0011153C File Offset: 0x0010F73C
		private static void ReevaluateActivePromptOwner()
		{
			if (MudPuddleInteractable.cachedPlayerController == null)
			{
				return;
			}
			Vector3 position = MudPuddleInteractable.cachedPlayerController.transform.position;
			MudPuddleInteractable mudPuddleInteractable = null;
			float num = float.MaxValue;
			List<MudPuddleInteractable> list = new List<MudPuddleInteractable>();
			foreach (MudPuddleInteractable mudPuddleInteractable2 in MudPuddleInteractable.PlayerOverlappingInteractables)
			{
				if (mudPuddleInteractable2 == null || !mudPuddleInteractable2.isActiveAndEnabled || !mudPuddleInteractable2.isPlayerInRange)
				{
					list.Add(mudPuddleInteractable2);
				}
				else if (mudPuddleInteractable2.CanInteract(null))
				{
					float sqrMagnitude = (mudPuddleInteractable2.transform.position - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						mudPuddleInteractable = mudPuddleInteractable2;
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				MudPuddleInteractable.PlayerOverlappingInteractables.Remove(list[i]);
			}
			if (MudPuddleInteractable.activePromptOwner == mudPuddleInteractable)
			{
				return;
			}
			if (MudPuddleInteractable.activePromptOwner != null)
			{
				MudPuddleInteractable.activePromptOwner.HidePrompt();
			}
			MudPuddleInteractable.activePromptOwner = mudPuddleInteractable;
			if (MudPuddleInteractable.activePromptOwner != null && MudPuddleInteractable.activePromptOwner.promptUI != null && !InteractablePromptUI.HasVisiblePromptOtherThan(MudPuddleInteractable.activePromptOwner.promptUI))
			{
				MudPuddleInteractable.activePromptOwner.ShowPrompt();
			}
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x0011169C File Offset: 0x0010F89C
		private void ShowPrompt()
		{
			if (this.promptVisible && this.promptUI != null && this.promptUI.IsVisible)
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

		// Token: 0x06003039 RID: 12345 RVA: 0x00111702 File Offset: 0x0010F902
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

		// Token: 0x0600303A RID: 12346 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnActionExecuted(string actionName, GameObject player)
		{
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x0011172D File Offset: 0x0010F92D
		private void OnDisable()
		{
			this.isPlayerInRange = false;
			this.overlappingPlayerColliders.Clear();
			MudPuddleInteractable.PlayerOverlappingInteractables.Remove(this);
			this.HidePrompt();
			if (MudPuddleInteractable.activePromptOwner == this)
			{
				MudPuddleInteractable.activePromptOwner = null;
				MudPuddleInteractable.ReevaluateActivePromptOwner();
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x0011176B File Offset: 0x0010F96B
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, (this.interactionDistance > 0f) ? this.interactionDistance : 2f);
		}

		// Token: 0x040030C3 RID: 12483
		[Header("Configuration")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x040030C4 RID: 12484
		[Header("Input")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x040030C5 RID: 12485
		[Header("Detection")]
		[Tooltip("Maximum reach in world metres. Legacy puddles with zero use 2 metres.")]
		[SerializeField]
		[Min(0f)]
		private float interactionDistance = 2f;

		// Token: 0x040030C6 RID: 12486
		[Tooltip("Optional duration for mud from this puddle only. Zero uses the player's normal mud duration.")]
		[SerializeField]
		[Min(0f)]
		private float mudDurationOverrideSeconds;

		// Token: 0x040030C7 RID: 12487
		[Header("UI")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040030C8 RID: 12488
		[Header("Action (Optional Override)")]
		[Tooltip("Direct reference to the player's controller. If null, will be auto-detected on the interacting player.")]
		[SerializeField]
		private MudBodyPaintRuntimeController targetBodyPaintController;

		// Token: 0x040030C9 RID: 12489
		[Header("Events")]
		[SerializeField]
		private UnityEvent onPlayerEnterRange;

		// Token: 0x040030CA RID: 12490
		[SerializeField]
		private UnityEvent onPlayerExitRange;

		// Token: 0x040030CB RID: 12491
		[SerializeField]
		private UnityEvent onInteract;

		// Token: 0x040030CC RID: 12492
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x040030CD RID: 12493
		private static readonly HashSet<MudPuddleInteractable> PlayerOverlappingInteractables = new HashSet<MudPuddleInteractable>();

		// Token: 0x040030CE RID: 12494
		private static MudPuddleInteractable activePromptOwner;

		// Token: 0x040030CF RID: 12495
		private static readonly RaycastHit[] ReachHits = new RaycastHit[32];

		// Token: 0x040030D0 RID: 12496
		private bool isPlayerInRange;

		// Token: 0x040030D1 RID: 12497
		private bool promptVisible;

		// Token: 0x040030D2 RID: 12498
		private Collider interactionCollider;

		// Token: 0x040030D3 RID: 12499
		private readonly HashSet<Collider> overlappingPlayerColliders = new HashSet<Collider>();

		// Token: 0x040030D4 RID: 12500
		private static vThirdPersonController cachedPlayerController;

		// Token: 0x040030D5 RID: 12501
		private static MudBodyPaintRuntimeController cachedMudController;
	}
}
