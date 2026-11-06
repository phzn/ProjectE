using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001F RID: 31
	public class SkateboardAnchorProvider : MonoBehaviour, IVehicleAnchorProvider, IVehicleSpeedProvider
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00008F84 File Offset: 0x00007184
		public Transform LeftFootRest
		{
			get
			{
				return this.leftFootRest;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00008F8C File Offset: 0x0000718C
		public Transform RightFootRest
		{
			get
			{
				return this.rightFootRest;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00008F94 File Offset: 0x00007194
		public Transform PelvisReference
		{
			get
			{
				return this.pelvisReference;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008F9C File Offset: 0x0000719C
		public Transform GetAnchor(VehicleAnchorType type)
		{
			switch (type)
			{
			case VehicleAnchorType.Seat:
				return this.pelvisReference;
			case VehicleAnchorType.LeftHandGrip:
			case VehicleAnchorType.RightHandGrip:
				break;
			case VehicleAnchorType.LeftFootRest:
				return this.leftFootRest;
			case VehicleAnchorType.RightFootRest:
				return this.rightFootRest;
			case VehicleAnchorType.SpineAimReference:
				return this.spineReference;
			case VehicleAnchorType.PelvisReference:
				return this.pelvisReference;
			default:
				if (type == VehicleAnchorType.Footboard)
				{
					return base.transform;
				}
				break;
			}
			return null;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000900B File Offset: 0x0000720B
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000901A File Offset: 0x0000721A
		public float CurrentSpeed
		{
			get
			{
				return this.manualSpeed;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009022 File Offset: 0x00007222
		public float GetCurrentSpeed()
		{
			return this.manualSpeed;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000902A File Offset: 0x0000722A
		public void SetSpeed(float speed)
		{
			this.manualSpeed = Mathf.Abs(speed);
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00009038 File Offset: 0x00007238
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00009040 File Offset: 0x00007240
		public bool UsesInvertedHierarchy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009044 File Offset: 0x00007244
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			if (this.leftFootRest != null)
			{
				Gizmos.DrawWireSphere(this.leftFootRest.position, 0.05f);
				Gizmos.DrawLine(this.leftFootRest.position, this.leftFootRest.position + this.leftFootRest.forward * 0.15f);
			}
			Gizmos.color = Color.yellow;
			if (this.rightFootRest != null)
			{
				Gizmos.DrawWireSphere(this.rightFootRest.position, 0.05f);
				Gizmos.DrawLine(this.rightFootRest.position, this.rightFootRest.position + this.rightFootRest.forward * 0.15f);
			}
			Gizmos.color = Color.magenta;
			if (this.pelvisReference != null)
			{
				Gizmos.DrawWireSphere(this.pelvisReference.position, 0.1f);
			}
		}

		// Token: 0x0400014A RID: 330
		[Header("Deck")]
		[Tooltip("Position for the LEFT foot on the deck (front foot)")]
		[SerializeField]
		private Transform leftFootRest;

		// Token: 0x0400014B RID: 331
		[Tooltip("Position for the RIGHT foot on the deck (back foot, stays planted while pushing)")]
		[SerializeField]
		private Transform rightFootRest;

		// Token: 0x0400014C RID: 332
		[Header("Reference Points")]
		[Tooltip("Rider root/pelvis reference (used as Seat + PelvisReference; rider stands)")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x0400014D RID: 333
		[Tooltip("Optional reference for spine rotation/aim")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x0400014E RID: 334
		private float manualSpeed;
	}
}
