using System;
using Features.DebugTools;
using Features.Inventory;
using Features.PhoneCompanion;
using Features.Rank;
using Features.Tutorials;
using Features.Vehicles.Spawning;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200070B RID: 1803
	[RequireComponent(typeof(Collider))]
	public class vVehicleInteractable : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x00106230 File Offset: 0x00104430
		private string RideGateReason
		{
			get
			{
				return "VehicleRide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002E7A RID: 11898 RVA: 0x00106255 File Offset: 0x00104455
		public InventoryItemSO SourceItem
		{
			get
			{
				return this.sourceItem;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x0010625D File Offset: 0x0010445D
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistToPlayer;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x00106265 File Offset: 0x00104465
		public bool IsCandidateEligible
		{
			get
			{
				return this.isPlayerInRange && !this.IsBeingUsed && this.playerTransform != null && this.CanInteract(this.playerTransform.gameObject);
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x00106298 File Offset: 0x00104498
		private void OnDisable()
		{
			this.HideRideHint();
			if (this.IsBeingUsed)
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interactable.disabled_while_ridden", this.vehicleController.CurrentRider, null);
			}
			if (this.IsBeingUsed)
			{
				VehicleDisableDismount.Request(this.vehicleController);
			}
			InteractionCandidateRegistry.Unregister(this);
			VehicleRideActivityContext.Release(ref this._rideLease);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x001062F4 File Offset: 0x001044F4
		private void ShowRideHint()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (!instance.IsShowing || instance.CurrentConfig != this.interactionConfig)
			{
				instance.Show(this.interactionConfig);
			}
			this._rideHintShown = true;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0010633F File Offset: 0x0010453F
		private void HideRideHint()
		{
			if (!this._rideHintShown)
			{
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this.interactionConfig);
			}
			this._rideHintShown = false;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x00106368 File Offset: 0x00104568
		private void SyncRideGate()
		{
			if (!this.IsBeingUsed || (this._rideLease != null && this._rideLease.IsActive))
			{
				if (!this.IsBeingUsed)
				{
					this.HideRideHint();
					if (this._rideLease != null)
					{
						BicycleRideDiagnostics.Record(this.vehicleController, "interaction.ride_lease.release", null, this.RideGateReason);
					}
					VehicleRideActivityContext.Release(ref this._rideLease);
				}
				return;
			}
			if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.ride_lease.lost", this.vehicleController.CurrentRider, null);
				Debug.LogError("[Vehicle] Ride context was lost and could not be restored; forcing a safe dismount.", this);
				this.vehicleController.ForceDismount();
				return;
			}
			BicycleRideDiagnostics.Record(this.vehicleController, "interaction.ride_lease.restored", this.vehicleController.CurrentRider, this.RideGateReason);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x00106433 File Offset: 0x00104633
		public void SetSourceItem(InventoryItemSO item)
		{
			this.sourceItem = item;
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x0010643C File Offset: 0x0010463C
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002E85 RID: 11909 RVA: 0x00106444 File Offset: 0x00104644
		public bool IsBeingUsed
		{
			get
			{
				return this.vehicleController != null && this.vehicleController.IsBeingRidden;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x00106461 File Offset: 0x00104661
		public vThirdPersonController CurrentRider
		{
			get
			{
				if (!(this.vehicleController != null))
				{
					return null;
				}
				return this.vehicleController.CurrentRider;
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x0010647E File Offset: 0x0010467E
		public void ReleaseDismountedRideContext()
		{
			if (!this.IsBeingUsed)
			{
				VehicleRideActivityContext.Release(ref this._rideLease);
			}
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x00106494 File Offset: 0x00104694
		public bool CanInteract(GameObject player)
		{
			if (this.IsBeingUsed || player == null || this.vehicleController == null || !VehicleRideActivityContext.IsCompatibleWithRider(player, this.vehicleController.Type))
			{
				return false;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			return (!(component != null) || !component.lockMovement) && VehicleRideActivityContext.CanAcquire;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x001064F4 File Offset: 0x001046F4
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.interactionConfig, "shift", "L3", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "space", "A", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "E", "B", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "B", "Y", false);
			this.vehicleController = base.GetComponentInParent<vVehicleController>();
			if (this.vehicleController == null)
			{
				Debug.LogError("[vVehicleInteractable] No vVehicleController found in parent hierarchy!");
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0010657D File Offset: 0x0010477D
		private void Start()
		{
			this.playerTransform = this._playerLookup.Resolve();
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00106590 File Offset: 0x00104790
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			this.SyncRideGate();
			if (this.IsBeingUsed && !SmartphoneController.IsPhonePointerActive && !SmartphoneController.IsAnySelfieActive && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				this.ShowRideHint();
			}
			if (this.IsBeingUsed)
			{
				if (ControllerContextBindings.ExitPressed(this.interactInput))
				{
					BicycleRideDiagnostics.Record(this.vehicleController, "interaction.dismount_input", this.vehicleController.CurrentRider, null);
					if (this.vehicleController.Dismount(false))
					{
						VehicleRideActivityContext.Release(ref this._rideLease);
						InteractionHUDController instance = InteractionHUDController.Instance;
						if (instance == null)
						{
							return;
						}
						instance.Hide();
					}
				}
				return;
			}
			bool flag = this.isPlayerInRange;
			this.isPlayerInRange = this.IsPlayerInRange();
			this._sqrDistToPlayer = ((this.playerTransform != null) ? (base.transform.position - this.playerTransform.position).sqrMagnitude : float.MaxValue);
			bool flag2 = this.isPlayerInRange && InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			this.isPlayerInRange = flag2;
			if (flag2 && !flag)
			{
				if (this.CanInteract(this.playerTransform.gameObject))
				{
					if (this.promptUI != null)
					{
						this.promptUI.Show(this.interactionConfig);
					}
					UnityEvent onPlayerEnterRange = this.OnPlayerEnterRange;
					if (onPlayerEnterRange != null)
					{
						onPlayerEnterRange.Invoke();
					}
					if (this.enableDebugLogs)
					{
						Debug.Log("[vVehicleInteractable] Player entered interaction range");
					}
				}
			}
			else if (!flag2 && flag)
			{
				if (this.promptUI != null)
				{
					this.promptUI.Hide();
				}
				UnityEvent onPlayerExitRange = this.OnPlayerExitRange;
				if (onPlayerExitRange != null)
				{
					onPlayerExitRange.Invoke();
				}
				if (this.enableDebugLogs)
				{
					Debug.Log("[vVehicleInteractable] Player exited interaction range");
				}
			}
			if (this.isPlayerInRange && this.sourceItem != null && !this.IsBeingUsed)
			{
				PlayerInventoryManager instance2 = PlayerInventoryManager.Instance;
				if (instance2 != null && instance2.pickupInput.GetButtonDown())
				{
					if (instance2.AddItem(this.sourceItem))
					{
						InteractionCandidateRegistry.MarkInteract();
						if (this.promptUI != null)
						{
							this.promptUI.Hide();
						}
						InteractionHUDController instance3 = InteractionHUDController.Instance;
						if (instance3 != null)
						{
							instance3.Hide();
						}
						Object.Destroy(base.gameObject);
					}
					return;
				}
			}
			if (this.isPlayerInRange && this.interactInput.GetButtonDown())
			{
				if (this.CanInteract(this.playerTransform.gameObject))
				{
					this.OnInteract(this.playerTransform.gameObject);
					return;
				}
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.input_rejected", null, "player=" + BicycleRideDiagnostics.Path(this.playerTransform) + "; rideContextCanAcquire=" + VehicleRideActivityContext.CanAcquire.ToString());
			}
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00106838 File Offset: 0x00104A38
		private bool IsPlayerInRange()
		{
			this.playerTransform = this._playerLookup.Resolve();
			return !(this.playerTransform == null) && Vector3.Distance(base.transform.position, this.playerTransform.position) <= this.interactionDistance;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x0010688C File Offset: 0x00104A8C
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			BicycleRideDiagnostics.Record(this.vehicleController, "interaction.request", (player != null) ? player.GetComponent<vThirdPersonController>() : null, null);
			if (TutorialService.TryShowFirstUse("vehicles"))
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.waiting_for_tutorial", null, null);
				return;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			if (component == null)
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.rejected.missing_player_controller", null, player.name);
				return;
			}
			if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.rejected.activity_context", component, this.RideGateReason);
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this.vehicleController.Mount(component);
			if (this.IsBeingUsed)
			{
				if (this.promptUI != null)
				{
					this.promptUI.Hide();
				}
				this.ShowRideHint();
			}
			else
			{
				BicycleRideDiagnostics.Record(this.vehicleController, "interaction.mount_not_started", component, null);
				VehicleRideActivityContext.Release(ref this._rideLease);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[vVehicleInteractable] Player mounted vehicle");
			}
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x001069B4 File Offset: 0x00104BB4
		public void OnActionExecuted(string actionName, GameObject player)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[vVehicleInteractable] Action executed: " + actionName);
			}
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x001069D0 File Offset: 0x00104BD0
		private void OnDrawGizmos()
		{
			Gizmos.color = (this.isPlayerInRange ? Color.green : Color.yellow);
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.5f, this.interactionDistance);
		}

		// Token: 0x04002EDA RID: 11994
		[Header("Configuration")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04002EDB RID: 11995
		[Header("Input")]
		[Tooltip("Input to mount/dismount the vehicle")]
		public GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04002EDC RID: 11996
		[Header("Detection")]
		[Tooltip("Maximum distance for interaction. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		public float interactionDistance = 1.1f;

		// Token: 0x04002EDD RID: 11997
		[Header("UI References")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04002EDE RID: 11998
		[Header("Inventory Integration")]
		[Tooltip("If assigned, pressing the inventory pickup key (PlayerInventoryManager.pickupInput) while in range and not riding returns this vehicle to inventory. Set on the prefab so any instance of this vehicle in the world behaves uniformly (pre-placed scene objects and inventory-spawned instances).")]
		[SerializeField]
		private InventoryItemSO sourceItem;

		// Token: 0x04002EDF RID: 11999
		[Header("Events")]
		public UnityEvent OnPlayerEnterRange;

		// Token: 0x04002EE0 RID: 12000
		public UnityEvent OnPlayerExitRange;

		// Token: 0x04002EE1 RID: 12001
		[Header("Debug")]
		public bool enableDebugLogs;

		// Token: 0x04002EE2 RID: 12002
		private vVehicleController vehicleController;

		// Token: 0x04002EE3 RID: 12003
		private Transform playerTransform;

		// Token: 0x04002EE4 RID: 12004
		private readonly VehiclePlayerLookup _playerLookup = new VehiclePlayerLookup();

		// Token: 0x04002EE5 RID: 12005
		private bool isPlayerInRange;

		// Token: 0x04002EE6 RID: 12006
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04002EE7 RID: 12007
		private PlayerActivityLease _rideLease;

		// Token: 0x04002EE8 RID: 12008
		private bool _rideHintShown;
	}
}
