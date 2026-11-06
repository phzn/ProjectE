using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200025F RID: 607
	public struct MMSpringVector2Event
	{
		// Token: 0x060010EE RID: 4334 RVA: 0x0004B914 File Offset: 0x00049B14
		public static void Trigger(SpringCommands command, MMSpringComponentBase targetSpring, MMChannelData channelData, Vector2 moveToValue = default(Vector2), Vector2 bumpAmount = default(Vector2), Vector2 moveToRandomValueMin = default(Vector2), Vector2 moveToRandomValueMax = default(Vector2), Vector2 bumpAmountRandomValueMin = default(Vector2), Vector2 bumpAmountRandomValueMax = default(Vector2), bool overrideDamping = false, Vector2 newDamping = default(Vector2), bool overrideFrequency = false, Vector2 newFrequency = default(Vector2))
		{
			MMSpringVector2Event.e.ChannelData = channelData;
			MMSpringVector2Event.e.TargetSpring = targetSpring;
			MMSpringVector2Event.e.Command = command;
			MMSpringVector2Event.e.MoveToValue = moveToValue;
			MMSpringVector2Event.e.BumpAmount = bumpAmount;
			MMSpringVector2Event.e.MoveToRandomValueMin = moveToRandomValueMin;
			MMSpringVector2Event.e.MoveToRandomValueMax = moveToRandomValueMax;
			MMSpringVector2Event.e.BumpAmountRandomValueMin = bumpAmountRandomValueMin;
			MMSpringVector2Event.e.BumpAmountRandomValueMax = bumpAmountRandomValueMax;
			MMSpringVector2Event.e.OverrideDamping = overrideDamping;
			MMSpringVector2Event.e.NewDamping = newDamping;
			MMSpringVector2Event.e.OverrideFrequency = overrideFrequency;
			MMSpringVector2Event.e.NewFrequency = newFrequency;
			MMEventManager.TriggerEvent<MMSpringVector2Event>(MMSpringVector2Event.e);
		}

		// Token: 0x040013F4 RID: 5108
		private static MMSpringVector2Event e;

		// Token: 0x040013F5 RID: 5109
		public MMChannelData ChannelData;

		// Token: 0x040013F6 RID: 5110
		public MMSpringComponentBase TargetSpring;

		// Token: 0x040013F7 RID: 5111
		public SpringCommands Command;

		// Token: 0x040013F8 RID: 5112
		public Vector2 MoveToValue;

		// Token: 0x040013F9 RID: 5113
		public Vector2 BumpAmount;

		// Token: 0x040013FA RID: 5114
		public Vector2 MoveToRandomValueMin;

		// Token: 0x040013FB RID: 5115
		public Vector2 MoveToRandomValueMax;

		// Token: 0x040013FC RID: 5116
		public Vector2 BumpAmountRandomValueMin;

		// Token: 0x040013FD RID: 5117
		public Vector2 BumpAmountRandomValueMax;

		// Token: 0x040013FE RID: 5118
		public bool OverrideDamping;

		// Token: 0x040013FF RID: 5119
		public Vector2 NewDamping;

		// Token: 0x04001400 RID: 5120
		public bool OverrideFrequency;

		// Token: 0x04001401 RID: 5121
		public Vector2 NewFrequency;
	}
}
