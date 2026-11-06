using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200077B RID: 1915
	public class UnicycleAnchorProvider : MonoBehaviour, IVehicleAnchorProvider, IVehicleSpeedProvider
	{
		// Token: 0x06003223 RID: 12835 RVA: 0x0011C7E4 File Offset: 0x0011A9E4
		public float GetCurrentSpeed()
		{
			if (this.speedProvider == null)
			{
				return this.manualSpeed;
			}
			return this.speedProvider.GetCurrentSpeed();
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x0011C800 File Offset: 0x0011AA00
		public Transform SeatPosition
		{
			get
			{
				return this.seatPosition;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x0011C808 File Offset: 0x0011AA08
		public Transform LeftHandGrip
		{
			get
			{
				return this.leftHandGrip;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x0011C810 File Offset: 0x0011AA10
		public Transform RightHandGrip
		{
			get
			{
				return this.rightHandGrip;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x0011C818 File Offset: 0x0011AA18
		public Transform LeftPedal
		{
			get
			{
				return this.leftPedal;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x0011C820 File Offset: 0x0011AA20
		public Transform RightPedal
		{
			get
			{
				return this.rightPedal;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x0011C828 File Offset: 0x0011AA28
		public Transform SpineReference
		{
			get
			{
				return this.spineReference;
			}
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x0011C830 File Offset: 0x0011AA30
		private void Awake()
		{
			this.speedProvider = base.GetComponent<IVehicleSpeedProvider>();
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x0011C840 File Offset: 0x0011AA40
		public Transform GetAnchor(VehicleAnchorType type)
		{
			Transform transform;
			switch (type)
			{
			case VehicleAnchorType.Seat:
				transform = this.seatPosition;
				break;
			case VehicleAnchorType.LeftHandGrip:
				transform = this.leftHandGrip;
				break;
			case VehicleAnchorType.RightHandGrip:
				transform = this.rightHandGrip;
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

		// Token: 0x0600322C RID: 12844 RVA: 0x0011C8BD File Offset: 0x0011AABD
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x0011C8CC File Offset: 0x0011AACC
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

		// Token: 0x0600322E RID: 12846 RVA: 0x0011C8ED File Offset: 0x0011AAED
		public void SetSpeed(float speed)
		{
			this.manualSpeed = speed;
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000AD00A File Offset: 0x000AB20A
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool UsesInvertedHierarchy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0011C8F8 File Offset: 0x0011AAF8
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			if (this.seatPosition != null)
			{
				Gizmos.DrawWireSphere(this.seatPosition.position, 0.1f);
			}
			Gizmos.color = Color.blue;
			if (this.leftHandGrip != null)
			{
				Gizmos.DrawWireSphere(this.leftHandGrip.position, 0.05f);
			}
			if (this.rightHandGrip != null)
			{
				Gizmos.DrawWireSphere(this.rightHandGrip.position, 0.05f);
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
		}

		// Token: 0x040032EB RID: 13035
		[Header("Seat")]
		[SerializeField]
		private Transform seatPosition;

		// Token: 0x040032EC RID: 13036
		[Header("Handle/Grips")]
		[Tooltip("For unicycle, these might be attached to the frame or seat")]
		[SerializeField]
		private Transform leftHandGrip;

		// Token: 0x040032ED RID: 13037
		[SerializeField]
		private Transform rightHandGrip;

		// Token: 0x040032EE RID: 13038
		[Header("Pedals")]
		[Tooltip("IMPORTANT: These should be CHILDREN of the crankset/pedal arm so they rotate with pedaling!")]
		[SerializeField]
		private Transform leftPedal;

		// Token: 0x040032EF RID: 13039
		[SerializeField]
		private Transform rightPedal;

		// Token: 0x040032F0 RID: 13040
		[Header("Reference Points")]
		[Tooltip("Reference for spine rotation/aim.")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x040032F1 RID: 13041
		[Tooltip("Optional: Reference for pelvis. If null, uses seat position.")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x040032F2 RID: 13042
		private IVehicleSpeedProvider speedProvider;

		// Token: 0x040032F3 RID: 13043
		private float manualSpeed;
	}
}
