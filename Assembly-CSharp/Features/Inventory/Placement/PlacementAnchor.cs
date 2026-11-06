using System;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x02001146 RID: 4422
	public class PlacementAnchor : MonoBehaviour
	{
		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x06008658 RID: 34392 RVA: 0x0036D2F0 File Offset: 0x0036B4F0
		public Vector3 LocalPosition
		{
			get
			{
				if (!this.useTransformAsAnchor)
				{
					return this.anchorLocalPosition;
				}
				return base.transform.localPosition;
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06008659 RID: 34393 RVA: 0x0036D30C File Offset: 0x0036B50C
		public Quaternion LocalRotation
		{
			get
			{
				if (!this.useTransformAsAnchor)
				{
					return Quaternion.Euler(this.anchorLocalEuler);
				}
				return base.transform.localRotation;
			}
		}

		// Token: 0x0600865A RID: 34394 RVA: 0x0036D32D File Offset: 0x0036B52D
		private void OnValidate()
		{
			if (this.useTransformAsAnchor)
			{
				this.anchorLocalPosition = base.transform.localPosition;
				this.anchorLocalEuler = base.transform.localEulerAngles;
			}
		}

		// Token: 0x040088E6 RID: 35046
		[Tooltip("If true, uses the Transform local position/rotation as the anchor.")]
		[SerializeField]
		private bool useTransformAsAnchor = true;

		// Token: 0x040088E7 RID: 35047
		[Tooltip("Optional override for anchor local position (ignored if useTransformAsAnchor is true).")]
		[SerializeField]
		private Vector3 anchorLocalPosition = Vector3.zero;

		// Token: 0x040088E8 RID: 35048
		[Tooltip("Optional override for anchor local rotation (ignored if useTransformAsAnchor is true).")]
		[SerializeField]
		private Vector3 anchorLocalEuler = Vector3.zero;
	}
}
