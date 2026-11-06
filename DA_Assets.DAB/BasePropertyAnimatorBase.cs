using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000005 RID: 5
	public abstract class BasePropertyAnimatorBase : MonoBehaviour
	{
		// Token: 0x0600001C RID: 28
		public abstract void SendPointerEvent(DAPointerEvent pointerEvent);

		// Token: 0x0600001D RID: 29
		protected abstract void SetIdleValue();

		// Token: 0x0600001E RID: 30
		protected abstract void OnStartAnimation();
	}
}
