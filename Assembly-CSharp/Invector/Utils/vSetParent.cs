using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000613 RID: 1555
	public class vSetParent : MonoBehaviour
	{
		// Token: 0x060023D1 RID: 9169 RVA: 0x000B7624 File Offset: 0x000B5824
		public void RemoveParent()
		{
			base.transform.parent = null;
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000B7632 File Offset: 0x000B5832
		public void RemoveParent(Transform target)
		{
			target.parent = null;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000CA85F File Offset: 0x000C8A5F
		public void SetParent(Transform parent)
		{
			base.transform.parent = parent;
		}
	}
}
