using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004D6 RID: 1238
	[Serializable]
	public class vHandler
	{
		// Token: 0x06001DDF RID: 7647 RVA: 0x000B6B52 File Offset: 0x000B4D52
		public vHandler()
		{
			this.customHandlers = new List<Transform>();
		}

		// Token: 0x04001EEE RID: 7918
		public Transform defaultHandler;

		// Token: 0x04001EEF RID: 7919
		public List<Transform> customHandlers;
	}
}
