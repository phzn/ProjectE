using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000284 RID: 644
	[AddComponentMenu("More Mountains/Springs/MMSpringScale")]
	public class MMSpringScale : MMSpringVector3Component<Transform>
	{
		// Token: 0x06001251 RID: 4689 RVA: 0x0004F73C File Offset: 0x0004D93C
		protected override void Initialization()
		{
			base.Initialization();
			this.SpringVector3.SpringX.ClampSettings.ClampMin = true;
			this.SpringVector3.SpringX.ClampSettings.ClampMinValue = 0f;
			this.SpringVector3.SpringX.ClampSettings.ClampMinBounce = true;
			this.SpringVector3.SpringY.ClampSettings.ClampMin = true;
			this.SpringVector3.SpringY.ClampSettings.ClampMinValue = 0f;
			this.SpringVector3.SpringY.ClampSettings.ClampMinBounce = true;
			this.SpringVector3.SpringZ.ClampSettings.ClampMin = true;
			this.SpringVector3.SpringZ.ClampSettings.ClampMinValue = 0f;
			this.SpringVector3.SpringZ.ClampSettings.ClampMinBounce = true;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x0004F821 File Offset: 0x0004DA21
		// (set) Token: 0x06001253 RID: 4691 RVA: 0x0004F82E File Offset: 0x0004DA2E
		public override Vector3 TargetVector3
		{
			get
			{
				return this.Target.localScale;
			}
			set
			{
				this.Target.localScale = value;
			}
		}
	}
}
