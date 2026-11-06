using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000007 RID: 7
	public class BikeAnchorProvider : MonoBehaviour, IVehicleAnchorProvider, IVehicleSpeedProvider
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003210 File Offset: 0x00001410
		public float GetCurrentSpeed()
		{
			if (this.speedProvider == null)
			{
				return this.manualSpeed;
			}
			return this.speedProvider.GetCurrentSpeed();
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000322C File Offset: 0x0000142C
		public Transform SeatPosition
		{
			get
			{
				return this.seatPosition;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003234 File Offset: 0x00001434
		public Transform LeftHandleGrip
		{
			get
			{
				return this.leftHandleGrip;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000323C File Offset: 0x0000143C
		public Transform RightHandleGrip
		{
			get
			{
				return this.rightHandleGrip;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003244 File Offset: 0x00001444
		public Transform LeftPedal
		{
			get
			{
				return this.leftPedal;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000324C File Offset: 0x0000144C
		public Transform RightPedal
		{
			get
			{
				return this.rightPedal;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00003254 File Offset: 0x00001454
		public Transform SpineReference
		{
			get
			{
				return this.spineReference;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000325C File Offset: 0x0000145C
		private void Awake()
		{
			this.speedProvider = base.GetComponent<IVehicleSpeedProvider>();
			this.ValidateHierarchy();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003270 File Offset: 0x00001470
		private void ValidateHierarchy()
		{
			if (this.leftHandleGrip != null && this.leftHandleGrip.parent != null && this.leftHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[BikeAnchorProvider] leftHandleGrip is a direct child of the bike root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.rightHandleGrip != null && this.rightHandleGrip.parent != null && this.rightHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[BikeAnchorProvider] rightHandleGrip is a direct child of the bike root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.leftPedal != null && this.leftPedal.parent != null && this.leftPedal.parent == base.transform)
			{
				Debug.LogWarning("[BikeAnchorProvider] leftPedal is a direct child of the bike root. For feet to follow pedaling, pedals should be children of the crankset/pedal arm!");
			}
			if (this.rightPedal != null && this.rightPedal.parent != null && this.rightPedal.parent == base.transform)
			{
				Debug.LogWarning("[BikeAnchorProvider] rightPedal is a direct child of the bike root. For feet to follow pedaling, pedals should be children of the crankset/pedal arm!");
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000338C File Offset: 0x0000158C
		public Transform GetAnchor(VehicleAnchorType type)
		{
			Transform transform;
			switch (type)
			{
			case VehicleAnchorType.Seat:
				transform = this.seatPosition;
				break;
			case VehicleAnchorType.LeftHandGrip:
				transform = this.leftHandleGrip;
				break;
			case VehicleAnchorType.RightHandGrip:
				transform = this.rightHandleGrip;
				break;
			case VehicleAnchorType.LeftFootRest:
				transform = this.leftPedal;
				break;
			case VehicleAnchorType.RightFootRest:
				transform = this.rightPedal;
				break;
			case VehicleAnchorType.SpineAimReference:
				transform = this.spineReference;
				break;
			case VehicleAnchorType.PelvisReference:
				transform = this.pelvisReference ?? this.seatPosition;
				break;
			default:
				transform = null;
				break;
			}
			return transform;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003409 File Offset: 0x00001609
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003418 File Offset: 0x00001618
		public float CurrentSpeed
		{
			get
			{
				if (this.speedProvider == null)
				{
					return this.manualSpeed;
				}
				return Mathf.Abs(this.speedProvider.GetCurrentSpeed());
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003439 File Offset: 0x00001639
		public void SetSpeed(float speed)
		{
			this.manualSpeed = speed;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003442 File Offset: 0x00001642
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000344A File Offset: 0x0000164A
		public bool UsesInvertedHierarchy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003450 File Offset: 0x00001650
		public Vector3 GetHandlebarCenter()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return (this.leftHandleGrip.position + this.rightHandleGrip.position) * 0.5f;
			}
			return base.transform.position + base.transform.forward * 0.5f;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000034C4 File Offset: 0x000016C4
		public Quaternion GetHandlebarRotation()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return Quaternion.Slerp(this.leftHandleGrip.rotation, this.rightHandleGrip.rotation, 0.5f);
			}
			return base.transform.rotation;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000351C File Offset: 0x0000171C
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			if (this.seatPosition != null)
			{
				Gizmos.DrawWireSphere(this.seatPosition.position, 0.1f);
				Gizmos.DrawLine(this.seatPosition.position, this.seatPosition.position + this.seatPosition.up * 0.2f);
			}
			Gizmos.color = Color.blue;
			if (this.leftHandleGrip != null)
			{
				Gizmos.DrawWireSphere(this.leftHandleGrip.position, 0.05f);
			}
			if (this.rightHandleGrip != null)
			{
				Gizmos.DrawWireSphere(this.rightHandleGrip.position, 0.05f);
			}
			Gizmos.color = Color.green;
			if (this.leftPedal != null)
			{
				Gizmos.DrawWireSphere(this.leftPedal.position, 0.05f);
			}
			if (this.rightPedal != null)
			{
				Gizmos.DrawWireSphere(this.rightPedal.position, 0.05f);
			}
			Gizmos.color = Color.magenta;
			if (this.spineReference != null)
			{
				Gizmos.DrawWireSphere(this.spineReference.position, 0.08f);
				Gizmos.color = Color.red;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.right * 0.15f);
				Gizmos.color = Color.green;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.up * 0.15f);
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.forward * 0.15f);
				return;
			}
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				Gizmos.DrawWireSphere(this.GetHandlebarCenter(), 0.08f);
			}
		}

		// Token: 0x04000035 RID: 53
		[Header("Seat")]
		[SerializeField]
		private Transform seatPosition;

		// Token: 0x04000036 RID: 54
		[Header("Handlebars")]
		[Tooltip("IMPORTANT: These should be CHILDREN of the handlebar Transform so they rotate with steering!")]
		[SerializeField]
		private Transform leftHandleGrip;

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private Transform rightHandleGrip;

		// Token: 0x04000038 RID: 56
		[Header("Pedals")]
		[Tooltip("IMPORTANT: These should be CHILDREN of the crankset/pedal arm so they rotate with pedaling!")]
		[SerializeField]
		private Transform leftPedal;

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private Transform rightPedal;

		// Token: 0x0400003A RID: 58
		[Header("Reference Points")]
		[Tooltip("Reference for spine rotation/aim. For MultiRotationConstraint, the rotation of this transform is used. For MultiAimConstraint, the position is used as aim target. If null, uses handlebar center/rotation as fallback.")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x0400003B RID: 59
		[Tooltip("Optional: Reference for pelvis. If null, uses seat position.")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x0400003C RID: 60
		private IVehicleSpeedProvider speedProvider;

		// Token: 0x0400003D RID: 61
		private float manualSpeed;
	}
}
