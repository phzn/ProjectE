using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200025D RID: 605
	public struct MMSpringFloatEvent
	{
		// Token: 0x060010CD RID: 4301 RVA: 0x0004B42C File Offset: 0x0004962C
		public static void Trigger(SpringCommands command, MMSpringComponentBase targetSpring, MMChannelData channelData, float moveToValue = 1f, float bumpAmount = 1f, Vector2 moveToRandomValue = default(Vector2), Vector2 bumpAmountRandomValue = default(Vector2), bool overrideDamping = false, float newDamping = 0.8f, bool overrideFrequency = false, float newFrequency = 5f)
		{
			MMSpringFloatEvent.e.ChannelData = channelData;
			MMSpringFloatEvent.e.TargetSpring = targetSpring;
			MMSpringFloatEvent.e.Command = command;
			MMSpringFloatEvent.e.MoveToValue = moveToValue;
			MMSpringFloatEvent.e.BumpAmount = bumpAmount;
			MMSpringFloatEvent.e.MoveToRandomValue = moveToRandomValue;
			MMSpringFloatEvent.e.BumpAmountRandomValue = bumpAmountRandomValue;
			MMSpringFloatEvent.e.OverrideDamping = overrideDamping;
			MMSpringFloatEvent.e.NewDamping = newDamping;
			MMSpringFloatEvent.e.OverrideFrequency = overrideFrequency;
			MMSpringFloatEvent.e.NewFrequency = newFrequency;
			MMEventManager.TriggerEvent<MMSpringFloatEvent>(MMSpringFloatEvent.e);
		}

		// Token: 0x040013DA RID: 5082
		private static MMSpringFloatEvent e;

		// Token: 0x040013DB RID: 5083
		public MMChannelData ChannelData;

		// Token: 0x040013DC RID: 5084
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040013DD RID: 5085
		public SpringCommands Command;

		// Token: 0x040013DE RID: 5086
		public float MoveToValue;

		// Token: 0x040013DF RID: 5087
		public float BumpAmount;

		// Token: 0x040013E0 RID: 5088
		public Vector2 MoveToRandomValue;

		// Token: 0x040013E1 RID: 5089
		public Vector2 BumpAmountRandomValue;

		// Token: 0x040013E2 RID: 5090
		public bool OverrideDamping;

		// Token: 0x040013E3 RID: 5091
		public float NewDamping;

		// Token: 0x040013E4 RID: 5092
		public bool OverrideFrequency;

		// Token: 0x040013E5 RID: 5093
		public float NewFrequency;
	}
}
