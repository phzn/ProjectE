using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028B RID: 651
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPCharacterSpacing")]
	public class MMSpringTMPCharacterSpacing : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0004FB8D File Offset: 0x0004DD8D
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x0004FB9A File Offset: 0x0004DD9A
		public override float TargetFloat
		{
			get
			{
				return this.Target.characterSpacing;
			}
			set
			{
				this.Target.characterSpacing = value;
			}
		}
	}
}
