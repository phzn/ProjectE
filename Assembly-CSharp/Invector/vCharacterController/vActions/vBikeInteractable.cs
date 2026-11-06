using System;
using Features.DebugTools;
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
	// Token: 0x02000704 RID: 1796
	[RequireComponent(typeof(vBikeController))]
	public class vBikeInteractable : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x00103434 File Offset: 0x00101634
		private string RideGateReason
		{
			get
			{
				return "VehicleRide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x00103459 File Offset: 0x00101659
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistToPlayer;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002E1C RID: 11804 RVA: 0x00103461 File Offset: 0x00101661
		public bool IsCandidateEligible
		{
			get
			{
				return this.isPlayerInRange && !this.IsBeingUsed && this.playerTransform != null && this.CanInteract(this.playerTransform.gameObject);
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x00103494 File Offset: 0x00101694
		private void OnDisable()
		{
			this.HideRideHint();
			if (this.IsBeingUsed)
			{
				BicycleRideDiagnostics.Record(this.bikeController, "interactable.disabled_while_ridden", this.bikeController.CurrentRider, null);
			}
			if (this.IsBeingUsed)
			{
				this.bikeController.Dismount();
			}
			InteractionCandidateRegistry.Unregister(this);
			VehicleRideActivityContext.Release(ref this._rideLease);
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x001034F0 File Offset: 0x001016F0
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

		// Token: 0x06002E21 RID: 11809 RVA: 0x0010353B File Offset: 0x0010173B
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

		// Token: 0x06002E22 RID: 11810 RVA: 0x00103564 File Offset: 0x00101764
		private void SyncRideGate()
		{
			if (!this.IsBeingUsed || (this._rideLease != null && this._rideLease.IsActive))
			{
				if (!this.IsBeingUsed)
				{
					this.HideRideHint();
					if (this._rideLease != null)
					{
						BicycleRideDiagnostics.Record(this.bikeController, "interaction.ride_lease.release", null, this.RideGateReason);
					}
					VehicleRideActivityContext.Release(ref this._rideLease);
				}
				return;
			}
			if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
			{
				BicycleRideDiagnostics.Record(this.bikeController, "interaction.ride_lease.lost", this.bikeController.CurrentRider, null);
				Debug.LogError("[Bike] Ride context was lost and could not be restored; forcing a safe dismount.", this);
				this.bikeController.Dismount();
				return;
			}
			BicycleRideDiagnostics.Record(this.bikeController, "interaction.ride_lease.restored", this.bikeController.CurrentRider, this.RideGateReason);
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x0010362F File Offset: 0x0010182F
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x00103637 File Offset: 0x00101837
		public bool IsBeingUsed
		{
			get
			{
				return this.bikeController != null && this.bikeController.IsBeingRidden;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x00103654 File Offset: 0x00101854
		public vThirdPersonController CurrentRider
		{
			get
			{
				if (!(this.bikeController != null))
				{
					return null;
				}
				return this.bikeController.CurrentRider;
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x00103671 File Offset: 0x00101871
		public void ReleaseDismountedRideContext()
		{
			if (!this.IsBeingUsed)
			{
				VehicleRideActivityContext.Release(ref this._rideLease);
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x00103688 File Offset: 0x00101888
		public bool CanInteract(GameObject player)
		{
			if (this.IsBeingUsed || player == null || !VehicleRideActivityContext.IsCompatibleWithRider(player, vVehicleController.VehicleType.Bicycle))
			{
				return false;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			return (!(component != null) || !component.lockMovement) && VehicleRideActivityContext.CanAcquire;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x001036D0 File Offset: 0x001018D0
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.interactionConfig, "shift", "L3", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "space", "A", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "E", "B", false);
			ControllerContextBindings.SetLabel(this.interactionConfig, "B", "Y", false);
			this.bikeController = base.GetComponent<vBikeController>();
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00103741 File Offset: 0x00101941
		private void Start()
		{
			this.playerTransform = this._playerLookup.Resolve();
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x00103754 File Offset: 0x00101954
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
					BicycleRideDiagnostics.Record(this.bikeController, "interaction.dismount_input", this.bikeController.CurrentRider, null);
					this.bikeController.Dismount();
					VehicleRideActivityContext.Release(ref this._rideLease);
					InteractionHUDController instance = InteractionHUDController.Instance;
					if (instance == null)
					{
						return;
					}
					instance.Hide();
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
			}
			if (this.isPlayerInRange && this.interactInput.GetButtonDown())
			{
				if (this.CanInteract(this.playerTransform.gameObject))
				{
					this.OnInteract(this.playerTransform.gameObject);
					return;
				}
				BicycleRideDiagnostics.Record(this.bikeController, "interaction.input_rejected", null, "player=" + BicycleRideDiagnostics.Path(this.playerTransform) + "; rideContextCanAcquire=" + VehicleRideActivityContext.CanAcquire.ToString());
			}
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x00103950 File Offset: 0x00101B50
		private bool IsPlayerInRange()
		{
			this.playerTransform = this._playerLookup.Resolve();
			return !(this.playerTransform == null) && Vector3.Distance(base.transform.position, this.playerTransform.position) <= this.interactionDistance;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x001039A4 File Offset: 0x00101BA4
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
			BicycleRideDiagnostics.Record(this.bikeController, "interaction.request", (player != null) ? player.GetComponent<vThirdPersonController>() : null, null);
			if (TutorialService.TryShowFirstUse("vehicles"))
			{
				BicycleRideDiagnostics.Record(this.bikeController, "interaction.waiting_for_tutorial", null, null);
				return;
			}
			vThirdPersonController component = player.GetComponent<vThirdPersonController>();
			if (component == null)
			{
				BicycleRideDiagnostics.Record(this.bikeController, "interaction.rejected.missing_player_controller", null, player.name);
				return;
			}
			if (!VehicleRideActivityContext.TryAcquire(this.RideGateReason, this, ref this._rideLease))
			{
				BicycleRideDiagnostics.Record(this.bikeController, "interaction.rejected.activity_context", component, this.RideGateReason);
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this.bikeController.Mount(component);
			if (this.IsBeingUsed)
			{
				if (this.promptUI != null)
				{
					this.promptUI.Hide();
				}
				this.ShowRideHint();
				return;
			}
			BicycleRideDiagnostics.Record(this.bikeController, "interaction.mount_not_started", component, null);
			VehicleRideActivityContext.Release(ref this._rideLease);
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x00103AB9 File Offset: 0x00101CB9
		public void OnActionExecuted(string actionName, GameObject player)
		{
			Debug.Log("[Bike] Action executed: " + actionName);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x00103ACC File Offset: 0x00101CCC
		private void OnDrawGizmos()
		{
			Gizmos.color = (this.isPlayerInRange ? Color.green : Color.yellow);
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.5f, this.interactionDistance);
		}

		// Token: 0x04002E70 RID: 11888
		[Header("Configuration")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04002E71 RID: 11889
		[Header("Input")]
		[Tooltip("Input to mount/dismount the bike")]
		public GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04002E72 RID: 11890
		[Header("Detection")]
		[Tooltip("Maximum distance for interaction. Aligned with the clothes pickup radius (1.1) so every interactable engages at the same distance.")]
		public float interactionDistance = 1.1f;

		// Token: 0x04002E73 RID: 11891
		[Header("UI References")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04002E74 RID: 11892
		[Header("Events")]
		public UnityEvent OnPlayerEnterRange;

		// Token: 0x04002E75 RID: 11893
		public UnityEvent OnPlayerExitRange;

		// Token: 0x04002E76 RID: 11894
		private vBikeController bikeController;

		// Token: 0x04002E77 RID: 11895
		private Transform playerTransform;

		// Token: 0x04002E78 RID: 11896
		private readonly VehiclePlayerLookup _playerLookup = new VehiclePlayerLookup();

		// Token: 0x04002E79 RID: 11897
		private bool isPlayerInRange;

		// Token: 0x04002E7A RID: 11898
		private float _sqrDistToPlayer = float.MaxValue;

		// Token: 0x04002E7B RID: 11899
		private PlayerActivityLease _rideLease;

		// Token: 0x04002E7C RID: 11900
		private bool _rideHintShown;
	}
}
