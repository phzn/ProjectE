using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200025A RID: 602
	public struct MMSpringColorEvent
	{
		// Token: 0x060010AC RID: 4268 RVA: 0x0004AE0C File Offset: 0x0004900C
		public static void Trigger(SpringCommands command, MMSpringComponentBase targetSpring, MMChannelData channelData, Color moveToValue = default(Color), Color bumpAmount = default(Color), Color moveToRandomValueMin = default(Color), Color moveToRandomValueMax = default(Color), Color bumpAmountRandomValueMin = default(Color), Color bumpAmountRandomValueMax = default(Color), bool overrideDamping = false, float newDamping = 0f, bool overrideFrequency = false, float newFrequency = 0f)
		{
			MMSpringColorEvent.e.ChannelData = channelData;
			MMSpringColorEvent.e.TargetSpring = targetSpring;
			MMSpringColorEvent.e.Command = command;
			MMSpringColorEvent.e.MoveToValue = moveToValue;
			MMSpringColorEvent.e.BumpAmount = bumpAmount;
			MMSpringColorEvent.e.MoveToRandomValueMin = moveToRandomValueMin;
			MMSpringColorEvent.e.MoveToRandomValueMax = moveToRandomValueMax;
			MMSpringColorEvent.e.BumpAmountRandomValueMin = bumpAmountRandomValueMin;
			MMSpringColorEvent.e.BumpAmountRandomValueMax = bumpAmountRandomValueMax;
			MMSpringColorEvent.e.OverrideDamping = overrideDamping;
			MMSpringColorEvent.e.NewDamping = newDamping;
			MMSpringColorEvent.e.OverrideFrequency = overrideFrequency;
			MMSpringColorEvent.e.NewFrequency = newFrequency;
			MMEventManager.TriggerEvent<MMSpringColorEvent>(MMSpringColorEvent.e);
		}

		// Token: 0x040013AA RID: 5034
		private static MMSpringColorEvent e;

		// Token: 0x040013AB RID: 5035
		public MMChannelData ChannelData;

		// Token: 0x040013AC RID: 5036
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040013AD RID: 5037
		public SpringCommands Command;

		// Token: 0x040013AE RID: 5038
		public Color MoveToValue;

		// Token: 0x040013AF RID: 5039
		public Color BumpAmount;

		// Token: 0x040013B0 RID: 5040
		public Color MoveToRandomValueMin;

		// Token: 0x040013B1 RID: 5041
		public Color MoveToRandomValueMax;

		// Token: 0x040013B2 RID: 5042
		public Color BumpAmountRandomValueMin;

		// Token: 0x040013B3 RID: 5043
		public Color BumpAmountRandomValueMax;

		// Token: 0x040013B4 RID: 5044
		public bool OverrideDamping;

		// Token: 0x040013B5 RID: 5045
		public float NewDamping;

		// Token: 0x040013B6 RID: 5046
		public bool OverrideFrequency;

		// Token: 0x040013B7 RID: 5047
		public float NewFrequency;
	}
}
