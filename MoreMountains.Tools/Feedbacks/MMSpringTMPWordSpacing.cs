using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000291 RID: 657
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPWordSpacing")]
	public class MMSpringTMPWordSpacing : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0004FCAD File Offset: 0x0004DEAD
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x0004FCBA File Offset: 0x0004DEBA
		public override float TargetFloat
		{
			get
			{
				return this.Target.wordSpacing;
			}
			set
			{
				this.Target.wordSpacing = value;
			}
		}
	}
}
