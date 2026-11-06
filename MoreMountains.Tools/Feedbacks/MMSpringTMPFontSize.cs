using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028D RID: 653
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPFontSize")]
	public class MMSpringTMPFontSize : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x0004FBF9 File Offset: 0x0004DDF9
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x0004FC06 File Offset: 0x0004DE06
		public override float TargetFloat
		{
			get
			{
				return this.Target.fontSize;
			}
			set
			{
				this.Target.fontSize = (float)((int)value);
			}
		}
	}
}
