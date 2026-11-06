using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000281 RID: 641
	[AddComponentMenu("More Mountains/Springs/MMSpringRectTransformSizeDelta")]
	public class MMSpringRectTransformSizeDelta : MMSpringVector2Component<RectTransform>
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x0004F5C0 File Offset: 0x0004D7C0
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x0004F5CD File Offset: 0x0004D7CD
		public override Vector2 TargetVector2
		{
			get
			{
				return this.Target.sizeDelta;
			}
			set
			{
				this.Target.sizeDelta = value;
			}
		}
	}
}
