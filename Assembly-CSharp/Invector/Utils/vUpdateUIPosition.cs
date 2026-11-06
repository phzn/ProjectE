using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000619 RID: 1561
	public class vUpdateUIPosition : MonoBehaviour
	{
		// Token: 0x060023ED RID: 9197 RVA: 0x000CABA2 File Offset: 0x000C8DA2
		public void UpdatePosition(GameObject target)
		{
			this.SetLocalPosition(target.transform.position);
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x000CABB5 File Offset: 0x000C8DB5
		public void UpdatePosition(Collider target)
		{
			this.SetLocalPosition(target.transform.position);
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000CABC8 File Offset: 0x000C8DC8
		public void UpdatePosition(Transform target)
		{
			this.SetLocalPosition(target.position);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000CABD8 File Offset: 0x000C8DD8
		private void SetLocalPosition(Vector3 position)
		{
			if (this.limitOnBox && this.box)
			{
				position = this.box.ClosestPointOnBounds(position);
			}
			Vector3 vector = this.referenceLocalParent.InverseTransformPoint(position);
			Vector3 localPosition = base.transform.localPosition;
			if (this.updateLocalX)
			{
				localPosition.x = vector.x;
			}
			if (this.updateLocalY)
			{
				localPosition.y = vector.y;
			}
			if (this.updateLocalZ)
			{
				localPosition.z = vector.z;
			}
			base.transform.localPosition = localPosition;
		}

		// Token: 0x040023E6 RID: 9190
		public Transform referenceLocalParent;

		// Token: 0x040023E7 RID: 9191
		public bool updateLocalX;

		// Token: 0x040023E8 RID: 9192
		public bool updateLocalY;

		// Token: 0x040023E9 RID: 9193
		public bool updateLocalZ;

		// Token: 0x040023EA RID: 9194
		public bool limitOnBox;

		// Token: 0x040023EB RID: 9195
		[vHideInInspector("limitOnBox", false)]
		public BoxCollider box;
	}
}
