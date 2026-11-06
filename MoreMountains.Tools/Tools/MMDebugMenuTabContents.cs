using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000045 RID: 69
	public class MMDebugMenuTabContents : MonoBehaviour
	{
		// Token: 0x06000160 RID: 352 RVA: 0x000073DC File Offset: 0x000055DC
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000073E4 File Offset: 0x000055E4
		protected virtual void Initialization()
		{
			if (this.ForceScaleOne)
			{
				base.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
			}
		}

		// Token: 0x040001D4 RID: 468
		public int Index;

		// Token: 0x040001D5 RID: 469
		public Transform Parent;

		// Token: 0x040001D6 RID: 470
		public bool ForceScaleOne = true;
	}
}
