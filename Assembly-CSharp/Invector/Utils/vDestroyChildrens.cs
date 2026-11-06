using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000607 RID: 1543
	public class vDestroyChildrens : MonoBehaviour
	{
		// Token: 0x06002397 RID: 9111 RVA: 0x000CA0CA File Offset: 0x000C82CA
		public virtual void DestroyChildrens()
		{
			this.DestroyChildrens(base.transform);
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000CA0D8 File Offset: 0x000C82D8
		public virtual void DestroyChildrensOfOther(Transform target)
		{
			this.DestroyChildrens(target);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000CA0E4 File Offset: 0x000C82E4
		protected virtual void DestroyChildrens(Transform target)
		{
			for (int i = target.childCount - 1; i >= 0; i--)
			{
				Object.Destroy(target.GetChild(i).gameObject);
			}
		}
	}
}
