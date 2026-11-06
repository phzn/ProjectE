using System;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200006F RID: 111
	[ExecuteAlways]
	[AddComponentMenu("")]
	internal class DefaultWorldInitializationProxy : MonoBehaviour
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000C211 File Offset: 0x0000A411
		public void OnEnable()
		{
			if (!this.IsActive)
			{
				return;
			}
			this.IsActive = false;
			Object.DestroyImmediate(base.gameObject);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000C22E File Offset: 0x0000A42E
		public void OnDisable()
		{
			if (this.IsActive)
			{
				DefaultWorldInitialization.DomainUnloadOrPlayModeChangeShutdown();
			}
		}

		// Token: 0x040000B2 RID: 178
		public bool IsActive;
	}
}
