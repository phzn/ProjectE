using System;
using UnityEngine;
using UnityEngine.Events;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000008 RID: 8
	public class BikeRiderController : MonoBehaviour
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000371D File Offset: 0x0000191D
		public bool HasRider
		{
			get
			{
				return this.hasRider;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003725 File Offset: 0x00001925
		public CharacterRigController CurrentRider
		{
			get
			{
				return this.currentRider;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000372D File Offset: 0x0000192D
		public VehiclePoseState CurrentPoseState
		{
			get
			{
				if (!(this.poseDriver != null))
				{
					return VehiclePoseState.Idle;
				}
				return this.poseDriver.CurrentState;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000374A File Offset: 0x0000194A
		private void Awake()
		{
			this.anchorProvider = base.GetComponent<BikeAnchorProvider>();
			this.poseDriver = base.GetComponent<VehiclePoseDriver>();
			if (this.posePreset != null)
			{
				this.poseDriver.SetPosePreset(this.posePreset);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003784 File Offset: 0x00001984
		private void Update()
		{
			if (!this.hasRider)
			{
				return;
			}
			this.poseDriver.UpdateFromSpeed(this.anchorProvider.CurrentSpeed);
			if (this.poseDriver.CurrentState != this.lastNotifiedState)
			{
				this.lastNotifiedState = this.poseDriver.CurrentState;
				UnityEvent<VehiclePoseState> onPoseStateChanged = this.OnPoseStateChanged;
				if (onPoseStateChanged == null)
				{
					return;
				}
				onPoseStateChanged.Invoke(this.lastNotifiedState);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000037EC File Offset: 0x000019EC
		public void SetupRider(GameObject riderObject)
		{
			if (riderObject == null)
			{
				Debug.LogError("[BikeRiderController] Cannot setup rider - riderObject is null");
				return;
			}
			this.currentRider = riderObject.GetComponent<CharacterRigController>();
			if (this.currentRider == null)
			{
				this.currentRider = riderObject.AddComponent<CharacterRigController>();
			}
			if (!this.currentRider.Initialize())
			{
				Debug.LogError("[BikeRiderController] Failed to initialize CharacterRigController");
				this.currentRider = null;
				return;
			}
			this.riderTransform = riderObject.transform;
			this.originalRiderParent = this.riderTransform.parent;
			this.originalRiderLocalPosition = this.riderTransform.localPosition;
			this.originalRiderLocalRotation = this.riderTransform.localRotation;
			this.originalRiderLocalScale = this.riderTransform.localScale;
			if (this.parentRiderToSeat && this.anchorProvider.SeatPosition != null)
			{
				Vector3 lossyScale = this.riderTransform.lossyScale;
				this.riderTransform.SetParent(this.anchorProvider.SeatPosition, false);
				Vector3 lossyScale2 = this.anchorProvider.SeatPosition.lossyScale;
				this.riderTransform.localScale = new Vector3((lossyScale2.x != 0f) ? (lossyScale.x / lossyScale2.x) : this.riderTransform.localScale.x, (lossyScale2.y != 0f) ? (lossyScale.y / lossyScale2.y) : this.riderTransform.localScale.y, (lossyScale2.z != 0f) ? (lossyScale.z / lossyScale2.z) : this.riderTransform.localScale.z);
			}
			this.poseDriver.Initialize(this.anchorProvider, this.currentRider, this.riderTransform);
			this.poseDriver.SetPoseImmediate(VehiclePoseState.Idle);
			this.hasRider = true;
			this.lastNotifiedState = VehiclePoseState.Idle;
			Debug.Log("[BikeRiderController] Rider setup complete: " + riderObject.name);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000039D8 File Offset: 0x00001BD8
		public void RemoveRider()
		{
			if (!this.hasRider)
			{
				return;
			}
			this.poseDriver.Shutdown();
			if (this.riderTransform != null)
			{
				this.riderTransform.SetParent(this.originalRiderParent, false);
				this.riderTransform.localPosition = this.originalRiderLocalPosition;
				this.riderTransform.localRotation = this.originalRiderLocalRotation;
				this.riderTransform.localScale = this.originalRiderLocalScale;
			}
			this.currentRider = null;
			this.riderTransform = null;
			this.hasRider = false;
			Debug.Log("[BikeRiderController] Rider removed");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003A6B File Offset: 0x00001C6B
		public void SetPoseState(VehiclePoseState state, float? duration = null)
		{
			if (!this.hasRider)
			{
				return;
			}
			this.poseDriver.TransitionTo(state, duration);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003A83 File Offset: 0x00001C83
		public void SetPosePreset(VehiclePosePreset preset)
		{
			this.posePreset = preset;
			this.poseDriver.SetPosePreset(preset);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A98 File Offset: 0x00001C98
		public RiderPoseData GetCurrentPose()
		{
			VehiclePoseDriver vehiclePoseDriver = this.poseDriver;
			if (vehiclePoseDriver == null)
			{
				return null;
			}
			return vehiclePoseDriver.CurrentPose;
		}

		// Token: 0x0400003E RID: 62
		[Header("Pose Configuration")]
		[SerializeField]
		private VehiclePosePreset posePreset;

		// Token: 0x0400003F RID: 63
		[Header("Rider Settings")]
		[Tooltip("If true, parent the rider to the seat while riding")]
		[SerializeField]
		private bool parentRiderToSeat = true;

		// Token: 0x04000040 RID: 64
		[Header("Events")]
		public UnityEvent<VehiclePoseState> OnPoseStateChanged;

		// Token: 0x04000041 RID: 65
		private BikeAnchorProvider anchorProvider;

		// Token: 0x04000042 RID: 66
		private VehiclePoseDriver poseDriver;

		// Token: 0x04000043 RID: 67
		private CharacterRigController currentRider;

		// Token: 0x04000044 RID: 68
		private Transform riderTransform;

		// Token: 0x04000045 RID: 69
		private Transform originalRiderParent;

		// Token: 0x04000046 RID: 70
		private Vector3 originalRiderLocalPosition;

		// Token: 0x04000047 RID: 71
		private Quaternion originalRiderLocalRotation;

		// Token: 0x04000048 RID: 72
		private Vector3 originalRiderLocalScale;

		// Token: 0x04000049 RID: 73
		private VehiclePoseState lastNotifiedState;

		// Token: 0x0400004A RID: 74
		private bool hasRider;
	}
}
