using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000280 RID: 640
	[AddComponentMenu("More Mountains/Springs/MMSpringRectTransformPosition")]
	public class MMSpringRectTransformPosition : MMSpringVector3Component<RectTransform>
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0004F59D File Offset: 0x0004D79D
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0004F5AA File Offset: 0x0004D7AA
		public override Vector3 TargetVector3
		{
			get
			{
				return this.Target.anchoredPosition3D;
			}
			set
			{
				this.Target.anchoredPosition3D = value;
			}
		}
	}
}
