using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001D RID: 29
	public class KickScooterAnchorProvider : MonoBehaviour, IVehicleAnchorProvider, IVehicleSpeedProvider
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00008523 File Offset: 0x00006723
		public Transform LeftHandleGrip
		{
			get
			{
				return this.leftHandleGrip;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000852B File Offset: 0x0000672B
		public Transform RightHandleGrip
		{
			get
			{
				return this.rightHandleGrip;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00008533 File Offset: 0x00006733
		public Transform Footboard
		{
			get
			{
				return this.footboard;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000853B File Offset: 0x0000673B
		public Transform StandingFootPosition
		{
			get
			{
				return this.standingFootPosition;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00008543 File Offset: 0x00006743
		public Transform KickingFootRestPosition
		{
			get
			{
				return this.kickingFootRestPosition;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000854B File Offset: 0x0000674B
		public Transform SpineReference
		{
			get
			{
				return this.spineReference;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00008553 File Offset: 0x00006753
		public KickScooterAnchorProvider.StandingFoot StandingFootSide
		{
			get
			{
				return this.standingFootSide;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000855B File Offset: 0x0000675B
		private void Awake()
		{
			this.speedProvider = base.GetComponent<IVehicleSpeedProvider>();
			this.ValidateSetup();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00008570 File Offset: 0x00006770
		private void ValidateSetup()
		{
			if (this.leftHandleGrip != null && this.leftHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[KickScooterAnchorProvider] leftHandleGrip is a direct child of the scooter root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.rightHandleGrip != null && this.rightHandleGrip.parent == base.transform)
			{
				Debug.LogWarning("[KickScooterAnchorProvider] rightHandleGrip is a direct child of the scooter root. For hands to follow steering, grips should be children of the handlebar Transform!");
			}
			if (this.footboard == null && this.standingFootPosition == null)
			{
				Debug.LogWarning("[KickScooterAnchorProvider] No footboard or standing foot position assigned!");
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008604 File Offset: 0x00006804
		public Transform GetAnchor(VehicleAnchorType type)
		{
			bool flag = this.standingFootSide == KickScooterAnchorProvider.StandingFoot.Right;
			switch (type)
			{
			case VehicleAnchorType.Seat:
				return this.pelvisReference;
			case VehicleAnchorType.LeftHandGrip:
				return this.leftHandleGrip;
			case VehicleAnchorType.RightHandGrip:
				return this.rightHandleGrip;
			case VehicleAnchorType.LeftFootRest:
				return flag ? (this.kickingFootRestPosition ?? this.footboard) : (this.standingFootPosition ?? this.footboard);
			case VehicleAnchorType.RightFootRest:
				return flag ? (this.standingFootPosition ?? this.footboard) : (this.kickingFootRestPosition ?? this.footboard);
			case VehicleAnchorType.SpineAimReference:
				return this.spineReference;
			case VehicleAnchorType.PelvisReference:
				return this.pelvisReference;
			case VehicleAnchorType.Footboard:
				return this.footboard;
			}
			return null;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000086EB File Offset: 0x000068EB
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000086FA File Offset: 0x000068FA
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

		// Token: 0x0600011A RID: 282 RVA: 0x0000871B File Offset: 0x0000691B
		public float GetCurrentSpeed()
		{
			return this.CurrentSpeed;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008723 File Offset: 0x00006923
		public void SetSpeed(float speed)
		{
			this.manualSpeed = speed;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000872C File Offset: 0x0000692C
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00008734 File Offset: 0x00006934
		public bool UsesInvertedHierarchy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008738 File Offset: 0x00006938
		public Vector3 GetHandlebarCenter()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return (this.leftHandleGrip.position + this.rightHandleGrip.position) * 0.5f;
			}
			return base.transform.position + base.transform.forward * 0.5f + base.transform.up * 1f;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000087C8 File Offset: 0x000069C8
		public Quaternion GetHandlebarRotation()
		{
			if (this.leftHandleGrip != null && this.rightHandleGrip != null)
			{
				return Quaternion.Slerp(this.leftHandleGrip.rotation, this.rightHandleGrip.rotation, 0.5f);
			}
			return base.transform.rotation;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000881D File Offset: 0x00006A1D
		public GroundedFoot GetGroundedFoot()
		{
			if (this.standingFootSide != KickScooterAnchorProvider.StandingFoot.Right)
			{
				return GroundedFoot.Right;
			}
			return GroundedFoot.Left;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000882C File Offset: 0x00006A2C
		private void OnDrawGizmosSelected()
		{
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
				Gizmos.DrawWireCube(this.footboard.position, new Vector3(0.15f, 0.03f, 0.5f));
			}
			Gizmos.color = Color.green;
			if (this.standingFootPosition != null)
			{
				Gizmos.DrawWireSphere(this.standingFootPosition.position, 0.05f);
				Gizmos.DrawLine(this.standingFootPosition.position, this.standingFootPosition.position + this.standingFootPosition.forward * 0.15f);
			}
			Gizmos.color = Color.yellow;
			if (this.kickingFootRestPosition != null)
			{
				Gizmos.DrawWireSphere(this.kickingFootRestPosition.position, 0.05f);
				Gizmos.DrawLine(this.kickingFootRestPosition.position, this.kickingFootRestPosition.position + this.kickingFootRestPosition.forward * 0.1f);
			}
			Gizmos.color = Color.magenta;
			if (this.pelvisReference != null)
			{
				Gizmos.DrawWireSphere(this.pelvisReference.position, 0.1f);
			}
			if (this.spineReference != null)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.spineReference.position, 0.08f);
				Gizmos.color = Color.red;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.right * 0.15f);
				Gizmos.color = Color.green;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.up * 0.15f);
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.forward * 0.15f);
			}
		}

		// Token: 0x04000136 RID: 310
		[Header("Handlebars")]
		[Tooltip("IMPORTANT: These should be CHILDREN of the handlebar Transform so they rotate with steering!")]
		[SerializeField]
		private Transform leftHandleGrip;

		// Token: 0x04000137 RID: 311
		[SerializeField]
		private Transform rightHandleGrip;

		// Token: 0x04000138 RID: 312
		[Header("Footboard")]
		[Tooltip("Main footboard/deck of the kick scooter")]
		[SerializeField]
		private Transform footboard;

		// Token: 0x04000139 RID: 313
		[Tooltip("Position for the STANDING foot (the one that stays on the board)")]
		[SerializeField]
		private Transform standingFootPosition;

		// Token: 0x0400013A RID: 314
		[Tooltip("Position for the KICKING foot when resting on the board (during Moving state). Ground position during Idle is handled by VehiclePoseDriver raycast + groundedFootOffset.")]
		[SerializeField]
		private Transform kickingFootRestPosition;

		// Token: 0x0400013B RID: 315
		[Header("Reference Points")]
		[Tooltip("Reference for spine rotation/aim. If null, uses handlebar center as fallback.")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x0400013C RID: 316
		[Tooltip("Reference for pelvis positioning (rider stands, so this is higher than seated vehicles)")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x0400013D RID: 317
		[Header("Riding Style")]
		[Tooltip("Which foot is the standing foot")]
		[SerializeField]
		private KickScooterAnchorProvider.StandingFoot standingFootSide;

		// Token: 0x0400013E RID: 318
		private IVehicleSpeedProvider speedProvider;

		// Token: 0x0400013F RID: 319
		private float manualSpeed;

		// Token: 0x02000026 RID: 38
		public enum StandingFoot
		{
			// Token: 0x04000168 RID: 360
			Left,
			// Token: 0x04000169 RID: 361
			Right
		}
	}
}
