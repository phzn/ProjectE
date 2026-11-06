using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004C4 RID: 1220
	public class vSpikeControl : MonoBehaviour
	{
		// Token: 0x06001D84 RID: 7556 RVA: 0x000B575C File Offset: 0x000B395C
		private void Start()
		{
			this.attachColliders = new List<Transform>();
			vSpike[] componentsInChildren = base.GetComponentsInChildren<vSpike>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].control = this;
			}
		}

		// Token: 0x04001EB0 RID: 7856
		[HideInInspector]
		public List<Transform> attachColliders;
	}
}
