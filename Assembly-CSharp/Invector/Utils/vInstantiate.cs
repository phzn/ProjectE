using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x0200060F RID: 1551
	[vClassHeader("v Instantiate", true, "icon_v2", false, "", openClose = false)]
	public class vInstantiate : vMonoBehaviour
	{
		// Token: 0x060023C1 RID: 9153 RVA: 0x000CA653 File Offset: 0x000C8853
		protected virtual void Start()
		{
			if (this.instantiateOnStart)
			{
				this.InstantiateObject();
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000CA664 File Offset: 0x000C8864
		public virtual void InstantiateObject()
		{
			if (this.prefab)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, base.transform.position, base.transform.rotation);
				gameObject.SetActive(true);
				if (this.setThisAsParent)
				{
					gameObject.transform.parent = base.transform;
				}
			}
		}

		// Token: 0x040023C3 RID: 9155
		public GameObject prefab;

		// Token: 0x040023C4 RID: 9156
		public bool instantiateOnStart;

		// Token: 0x040023C5 RID: 9157
		public bool setThisAsParent;
	}
}
