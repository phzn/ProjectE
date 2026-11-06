using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001E RID: 30
	public class ScooterAnchorProvider : MonoBehaviour, IVehicleAnchorProvider, IVehicleSpeedProvider
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00008A7B File Offset: 0x00006C7B
		public Transform SeatPosition
		{
			get
			{
				return this.seatPosition;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00008A83 File Offset: 0x00006C83
		public Transform LeftHandleGrip
		{
			get
			{
				return this.leftHandleGrip;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00008A8B File Offset: 0x00006C8B
		public Transform RightHandleGrip
		{
			get
			{
				return this.rightHandleGrip;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00008A93 File Offset: 0x00006C93
		public Transform Footboard
		{
			get
			{
				return this.footboard;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00008A9B File Offset: 0x00006C9B
		public Transform LeftFootPosition
		{
			get
			{
				return this.leftFootPosition;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00008AA3 File Offset: 0x00006CA3
		public Transform RightFootPosition
		{
			get
			{
				return this.rightFootPosition;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00008AAB File Offset: 0x00006CAB
		public Transform SpineReference
		{
			get
			{
				return this.spineReference;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008AB3 File Offset: 0x00006CB3
		private void Awake()
		{
			this.speedProvider = base.GetComponent<IVehicleSpeedProvider>();
			this.ValidateSetup();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008AC8 File Offset: 0x00006CC8
		private void ValidateSetup()
		{
			if (this.leftHandleGrip != null && this.leftHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[ScooterAnchorProvider] leftHandleGrip is a direct child of the scooter root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.rightHandleGrip != null && this.rightHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[ScooterAnchorProvider] rightHandleGrip is a direct child of the scooter root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.footboard == null && this.leftFootPosition == null && this.rightFootPosition == null)
			{
				Debug.LogWarning("[ScooterAnchorProvider] No footboard or foot positions assigned! Please assign at least one foot position reference.");
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008B6C File Offset: 0x00006D6C
		public Transform GetAnchor(VehicleAnchorType type)
		{
			switch (type)
			{
			case VehicleAnchorType.Seat:
				return this.seatPosition;
			case VehicleAnchorType.LeftHandGrip:
				return this.leftHandleGrip;
			case VehicleAnchorType.RightHandGrip:
				return this.rightHandleGrip;
			case VehicleAnchorType.LeftFootRest:
				return this.leftFootPosition ?? this.footboard;
			case VehicleAnchorType.RightFootRest:
				return this.rightFootPosition ?? this.footboard;
			case VehicleAnchorType.SpineAimReference:
				return this.spineReference;
			case VehicleAnchorType.PelvisReference:
				return this.pelvisReference ?? this.seatPosition;
			case VehicleAnchorType.Footboard:
				return this.footboard;
			case VehicleAnchorType.LeftFootboard:
				return this.leftFootPosition ?? this.footboard;
			case VehicleAnchorType.RightFootboard:
				return this.rightFootPosition ?? this.footboard;
			}
			return null;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008C5A File Offset: 0x00006E5A
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00008C69 File Offset: 0x00006E69
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

		// Token: 0x0600012F RID: 303 RVA: 0x00008C8A File Offset: 0x00006E8A
		public float GetCurrentSpeed()
		{
			return this.CurrentSpeed;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008C92 File Offset: 0x00006E92
		public void SetSpeed(float speed)
		{
			this.manualSpeed = speed;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00008C9B File Offset: 0x00006E9B
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00008CA3 File Offset: 0x00006EA3
		public bool UsesInvertedHierarchy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008CA8 File Offset: 0x00006EA8
		public Vector3 GetHandlebarCenter()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return (this.leftHandleGrip.position + this.rightHandleGrip.position) * 0.5f;
			}
			return base.transform.position + base.transform.forward * 0.5f;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008D1C File Offset: 0x00006F1C
		public Quaternion GetHandlebarRotation()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return Quaternion.Slerp(this.leftHandleGrip.rotation, this.rightHandleGrip.rotation, 0.5f);
			}
			return base.transform.rotation;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008D74 File Offset: 0x00006F74
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
			Gizmos.color = Color.cyan;
			if (this.footboard != null)
			{
				Gizmos.DrawWireCube(this.footboard.position, new Vector3(0.3f, 0.05f, 0.4f));
			}
			Gizmos.color = Color.green;
			if (this.leftFootPosition != null)
			{
				Gizmos.DrawWireSphere(this.leftFootPosition.position, 0.05f);
			}
			if (this.rightFootPosition != null)
			{
				Gizmos.DrawWireSphere(this.rightFootPosition.position, 0.05f);
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
			}
		}

		// Token: 0x04000140 RID: 320
		[Header("Seat")]
		[SerializeField]
		private Transform seatPosition;

		// Token: 0x04000141 RID: 321
		[Header("Handlebars")]
		[Tooltip("IMPORTANT: These should be CHILDREN of the handlebar Transform so they rotate with steering!")]
		[SerializeField]
		private Transform leftHandleGrip;

		// Token: 0x04000142 RID: 322
		[SerializeField]
		private Transform rightHandleGrip;

		// Token: 0x04000143 RID: 323
		[Header("Footboard")]
		[Tooltip("Center of the footboard where rider stands/rests feet")]
		[SerializeField]
		private Transform footboard;

		// Token: 0x04000144 RID: 324
		[Tooltip("Left foot position on footboard")]
		[SerializeField]
		private Transform leftFootPosition;

		// Token: 0x04000145 RID: 325
		[Tooltip("Right foot position on footboard")]
		[SerializeField]
		private Transform rightFootPosition;

		// Token: 0x04000146 RID: 326
		[Header("Reference Points")]
		[Tooltip("Reference for spine rotation/aim. If null, uses handlebar center as fallback.")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x04000147 RID: 327
		[Tooltip("Optional: Reference for pelvis. If null, uses seat position.")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x04000148 RID: 328
		private IVehicleSpeedProvider speedProvider;

		// Token: 0x04000149 RID: 329
		private float manualSpeed;
	}
}
