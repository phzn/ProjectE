using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A2 RID: 418
	[AddComponentMenu("")]
	public class DemoGhost : MonoBehaviour
	{
		// Token: 0x060009DB RID: 2523 RVA: 0x0002DF38 File Offset: 0x0002C138
		public virtual void OnAnimationEnd()
		{
			base.gameObject.SetActive(false);
		}
	}
}
