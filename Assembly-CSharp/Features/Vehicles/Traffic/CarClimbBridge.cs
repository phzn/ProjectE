using System;
using System.Collections;
using System.Collections.Generic;
using Features.Rank;
using Features.RunModes;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B8F RID: 2959
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-200)]
	public class CarClimbBridge : MonoBehaviour
	{
		// Token: 0x0600549F RID: 21663 RVA: 0x0020DC70 File Offset: 0x0020BE70
		public static void ApplyWorldOriginShiftToAll(Vector3 delta)
		{
			foreach (CarClimbBridge carClimbBridge in CarClimbBridge.OriginInstances)
			{
				if (carClimbBridge != null)
				{
					carClimbBridge.ApplyWorldOriginShift(delta);
				}
			}
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x0020DCCC File Offset: 0x0020BECC
		private void OnDestroy()
		{
			CarClimbBridge.OriginInstances.Remove(this);
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x0020DCDA File Offset: 0x0020BEDA
		public void ApplyWorldOriginShift(Vector3 delta)
		{
			this._preClimbPosition += delta;
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x0020DCEE File Offset: 0x0020BEEE
		public bool InterruptForEncounter()
		{
			if (this._climbingPlayer == null)
			{
				return false;
			}
			this.HandleActionCancelled();
			return true;
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x0020DD08 File Offset: 0x0020BF08
		private void Awake()
		{
			CarClimbBridge.OriginInstances.Add(this);
			if (this.triggers == null || this.triggers.Length == 0)
			{
				this.triggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			this._vehicle = base.GetComponentInParent<VehicleAI>();
			this._roofCarrier = base.GetComponentInChildren<CarRoofCarrier>(true);
			this._promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			if (this._promptUI == null && base.transform.parent != null)
			{
				this._promptUI = base.transform.parent.GetComponentInChildren<InteractablePromptUI>(true);
			}
			this.RefreshClimbAvailability();
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x0020DDA4 File Offset: 0x0020BFA4
		private void OnEnable()
		{
			RankProgressionService.Changed += this.RefreshClimbAvailability;
			RunModeService.Changed += this.RefreshClimbAvailability;
			this.RefreshClimbAvailability();
			if (this.triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleTriggerValidate));
					vTriggerGenericAction.OnEndAnimation.AddListener(new UnityAction(this.HandleActionEnded));
					vTriggerGenericAction.OnCancelAction.AddListener(new UnityAction(this.HandleActionCancelled));
				}
			}
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x0020DE4C File Offset: 0x0020C04C
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.RefreshClimbAvailability;
			RunModeService.Changed -= this.RefreshClimbAvailability;
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleTriggerValidate));
						vTriggerGenericAction.OnEndAnimation.RemoveListener(new UnityAction(this.HandleActionEnded));
						vTriggerGenericAction.OnCancelAction.RemoveListener(new UnityAction(this.HandleActionCancelled));
					}
				}
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
				this._restartRoutine = null;
			}
			if (this._blockedByUs && this._vehicle != null)
			{
				this._vehicle.SetBlocked(false);
				this._vehicle.HasPassenger = this._roofCarrier != null && this._roofCarrier.HasRider;
			}
			this._blockedByUs = false;
			this._climbingPlayer = null;
			CarInteractionActivityContext.Release(ref this._climbLease);
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x0020DFA0 File Offset: 0x0020C1A0
		private void Update()
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked)
			{
				this.ApplyClimbAvailability(true);
				return;
			}
			if (VehicleSimulationPause.IsPaused)
			{
				return;
			}
			this.ApplyClimbAvailability(false);
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x0020DFC0 File Offset: 0x0020C1C0
		private void RefreshClimbAvailability()
		{
			this.ApplyClimbAvailability(GuidedRankGameplayPolicy.CarInteractionsBlocked);
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x0020DFD0 File Offset: 0x0020C1D0
		private void ApplyClimbAvailability(bool rankBlocked)
		{
			if (this.triggers == null)
			{
				return;
			}
			bool flag = !rankBlocked && !this._blockedByUs && CarInteractionActivityContext.CanEnter && (this._roofCarrier == null || this._roofCarrier.CanStartNewRide) && (this._vehicle == null || this._vehicle.CurrentSpeed < this.maxClimbSpeed);
			for (int i = 0; i < this.triggers.Length; i++)
			{
				vTriggerGenericAction vTriggerGenericAction = this.triggers[i];
				if (!(vTriggerGenericAction == null) && vTriggerGenericAction.CanDoAction != flag)
				{
					vTriggerGenericAction.CanDoAction = flag;
				}
			}
			if (!flag && this._promptUI != null)
			{
				this._promptUI.Hide();
			}
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x0020E08C File Offset: 0x0020C28C
		private void HandleTriggerValidate(GameObject player)
		{
			vGenericAction vGenericAction = ((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			if (vGenericAction == this._activeGenericAction && this._genericActionSubscribed)
			{
				return;
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this._activeGenericAction = vGenericAction;
			if (this._activeGenericAction != null && this._activeGenericAction.OnStartAction != null)
			{
				this._activeGenericAction.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = true;
			}
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x0020E144 File Offset: 0x0020C344
		private void HandlePlayerActionStart(vTriggerGenericAction firingTrigger)
		{
			if (!this.IsOwnedTrigger(firingTrigger))
			{
				return;
			}
			if (!this.TryAcquireClimbContext())
			{
				return;
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
				this._restartRoutine = null;
			}
			this._climbingPlayer = ((this._activeGenericAction != null) ? this._activeGenericAction.transform : null);
			if (this._climbingPlayer != null)
			{
				this._preClimbPosition = this._climbingPlayer.position;
				this._preClimbRotation = this._climbingPlayer.rotation;
			}
			if (this._vehicle != null)
			{
				this._vehicle.SetBlocked(true);
				this._blockedByUs = true;
				this._vehicle.HasPassenger = true;
				if (VspDiag.Carry)
				{
					Debug.Log(string.Format("[CarryDiag] CarClimbBridge: HasPassenger=TRUE at climb START on '{0}' frame={1}", this._vehicle.name, Time.frameCount), this);
				}
			}
			if (this.debugLogs)
			{
				Debug.Log("[CarClimbBridge] Climb started ('" + ((firingTrigger != null) ? firingTrigger.name : null) + "') — car blocked.", this);
			}
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x0020E254 File Offset: 0x0020C454
		private void HandleActionEnded()
		{
			CarInteractionActivityContext.Release(ref this._climbLease);
			if (!this._blockedByUs)
			{
				return;
			}
			if (this._roofCarrier != null && this._climbingPlayer != null)
			{
				this._roofCarrier.Mount(this._climbingPlayer, true);
			}
			this._climbingPlayer = null;
			if (!base.isActiveAndEnabled)
			{
				if (this._vehicle != null)
				{
					this._vehicle.SetBlocked(false);
				}
				this._blockedByUs = false;
				return;
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
			}
			this._restartRoutine = base.StartCoroutine(this.RestartAfterDelay());
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x0020E2FC File Offset: 0x0020C4FC
		private void HandleActionCancelled()
		{
			CarInteractionActivityContext.Release(ref this._climbLease);
			if (this._climbingPlayer == null)
			{
				return;
			}
			Transform climbingPlayer = this._climbingPlayer;
			this._climbingPlayer = null;
			climbingPlayer.SetPositionAndRotation(this._preClimbPosition, this._preClimbRotation);
			Rigidbody component = climbingPlayer.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = this._preClimbPosition;
				component.rotation = this._preClimbRotation;
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
				}
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
			}
			if (base.isActiveAndEnabled)
			{
				this._restartRoutine = base.StartCoroutine(this.RestartAfterDelay());
				return;
			}
			if (this._vehicle != null)
			{
				this._vehicle.SetBlocked(false);
				this._vehicle.HasPassenger = this._roofCarrier != null && this._roofCarrier.HasRider;
			}
			this._blockedByUs = false;
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x0020E3F3 File Offset: 0x0020C5F3
		private IEnumerator RestartAfterDelay()
		{
			float remaining = Mathf.Max(0f, this.restartDelay);
			while (remaining > 0f || VehicleSimulationPause.IsPaused)
			{
				yield return null;
				if (!VehicleSimulationPause.IsPaused)
				{
					remaining -= Time.deltaTime;
				}
			}
			if (this._vehicle != null)
			{
				this._vehicle.SetBlocked(false);
			}
			this._blockedByUs = false;
			this._restartRoutine = null;
			if (this._vehicle != null && this._vehicle.HasPassenger && (this._roofCarrier == null || !this._roofCarrier.HasRider))
			{
				if (VspDiag.Carry)
				{
					Debug.Log(string.Format("[CarryDiag] CarClimbBridge: climb ended with NO rider aboard → releasing HasPassenger on '{0}' frame={1}", this._vehicle.name, Time.frameCount), this);
				}
				this._vehicle.HasPassenger = false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[CarClimbBridge] Restart delay elapsed — car driving off.", this);
			}
			yield break;
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x0020E404 File Offset: 0x0020C604
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

		// Token: 0x060054AF RID: 21679 RVA: 0x0020E44C File Offset: 0x0020C64C
		public bool TryAcquireClimbContext()
		{
			return (!(this._roofCarrier != null) || this._roofCarrier.CanStartNewRide) && ((this._climbLease != null && this._climbLease.IsActive) || (!GuidedRankGameplayPolicy.CarInteractionsBlocked && !this._blockedByUs && (!(this._vehicle != null) || this._vehicle.CurrentSpeed < this.maxClimbSpeed) && CarInteractionActivityContext.TryAcquire("CarClimb#" + base.GetInstanceID().ToString(), this, ref this._climbLease)));
		}

		// Token: 0x040054DA RID: 21722
		private static readonly HashSet<CarClimbBridge> OriginInstances = new HashSet<CarClimbBridge>();

		// Token: 0x040054DB RID: 21723
		[Header("Gating")]
		[Tooltip("Max VehicleAI speed (m/s) at which the climb prompt is offered. Above this the car is moving and the 4 triggers are disabled so no prompt shows.")]
		[SerializeField]
		private float maxClimbSpeed = 1.5f;

		// Token: 0x040054DC RID: 21724
		[Tooltip("Seconds the car stays blocked after the climb action ends before driving off again with the player on the roof (D4).")]
		[SerializeField]
		private float restartDelay = 2f;

		// Token: 0x040054DD RID: 21725
		[Header("References (auto-resolved)")]
		[Tooltip("The climb triggers on this ClimbUpCar. Auto-resolved from children at Awake.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x040054DE RID: 21726
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040054DF RID: 21727
		private VehicleAI _vehicle;

		// Token: 0x040054E0 RID: 21728
		private CarRoofCarrier _roofCarrier;

		// Token: 0x040054E1 RID: 21729
		private vGenericAction _activeGenericAction;

		// Token: 0x040054E2 RID: 21730
		private bool _genericActionSubscribed;

		// Token: 0x040054E3 RID: 21731
		private bool _blockedByUs;

		// Token: 0x040054E4 RID: 21732
		private Coroutine _restartRoutine;

		// Token: 0x040054E5 RID: 21733
		private PlayerActivityLease _climbLease;

		// Token: 0x040054E6 RID: 21734
		private InteractablePromptUI _promptUI;

		// Token: 0x040054E7 RID: 21735
		private Transform _climbingPlayer;

		// Token: 0x040054E8 RID: 21736
		private Vector3 _preClimbPosition;

		// Token: 0x040054E9 RID: 21737
		private Quaternion _preClimbRotation;
	}
}
