using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200027E RID: 638
	[AddComponentMenu("More Mountains/Springs/MMSpringMMTimeScale")]
	public class MMSpringMMTimeScale : MMSpringFloatComponent<Transform>
	{
		// Token: 0x0600123D RID: 4669 RVA: 0x0004F4C7 File Offset: 0x0004D6C7
		protected override void Initialization()
		{
			base.Initialization();
			this.FloatSpring.ClampSettings.ClampMin = true;
			this.FloatSpring.ClampSettings.ClampMinValue = 0f;
			this.FloatSpring.ClampSettings.ClampMinBounce = true;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0004F506 File Offset: 0x0004D706
		// (set) Token: 0x0600123F RID: 4671 RVA: 0x0004F514 File Offset: 0x0004D714
		public override float TargetFloat
		{
			get
			{
				return MMSingleton<MMTimeManager>.Instance.CurrentTimeScale;
			}
			set
			{
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, value, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
			}
		}
	}
}
