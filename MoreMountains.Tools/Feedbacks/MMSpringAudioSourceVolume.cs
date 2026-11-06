using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000275 RID: 629
	[AddComponentMenu("More Mountains/Springs/MMSpringAudioSourceVolume")]
	public class MMSpringAudioSourceVolume : MMSpringFloatComponent<AudioSource>
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x0004F365 File Offset: 0x0004D565
		// (set) Token: 0x06001223 RID: 4643 RVA: 0x0004F372 File Offset: 0x0004D572
		public override float TargetFloat
		{
			get
			{
				return this.Target.volume;
			}
			set
			{
				this.Target.volume = value;
			}
		}
	}
}
