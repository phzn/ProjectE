using System;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001B6 RID: 438
	public class PegasusHide : MonoBehaviour
	{
		// Token: 0x06000CE9 RID: 3305 RVA: 0x00058AA8 File Offset: 0x00056CA8
		private void Start()
		{
			if (this.m_hideAtRuntime)
			{
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
			}
		}

		// Token: 0x04000D4B RID: 3403
		public bool m_hideAtRuntime = true;
	}
}
