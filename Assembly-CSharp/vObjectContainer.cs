using System;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class vObjectContainer : MonoBehaviour
{
	// Token: 0x17000213 RID: 531
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00044F16 File Offset: 0x00043116
	public static Transform root
	{
		get
		{
			if (!vObjectContainer.instance)
			{
				vObjectContainer.instance = new GameObject("Object Container", new Type[] { typeof(vObjectContainer) }).GetComponent<vObjectContainer>();
			}
			return vObjectContainer.instance.transform;
		}
	}

	// Token: 0x0400091A RID: 2330
	private static vObjectContainer instance;
}
