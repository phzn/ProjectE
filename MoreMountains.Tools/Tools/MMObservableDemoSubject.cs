using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200016A RID: 362
	[AddComponentMenu("")]
	public class MMObservableDemoSubject : MonoBehaviour
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00028921 File Offset: 0x00026B21
		protected virtual void Update()
		{
			this.PositionX.Value = base.transform.position.x;
		}

		// Token: 0x04000A35 RID: 2613
		public MMObservable<float> PositionX;
	}
}
