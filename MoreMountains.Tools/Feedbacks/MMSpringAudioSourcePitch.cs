using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000274 RID: 628
	[AddComponentMenu("More Mountains/Springs/MMSpringAudioSourcePitch")]
	public class MMSpringAudioSourcePitch : MMSpringFloatComponent<AudioSource>
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x0004F342 File Offset: 0x0004D542
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x0004F34F File Offset: 0x0004D54F
		public override float TargetFloat
		{
			get
			{
				return this.Target.pitch;
			}
			set
			{
				this.Target.pitch = value;
			}
		}
	}
}
