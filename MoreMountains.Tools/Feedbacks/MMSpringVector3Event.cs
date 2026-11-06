using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000261 RID: 609
	public struct MMSpringVector3Event
	{
		// Token: 0x0600110F RID: 4367 RVA: 0x0004BE40 File Offset: 0x0004A040
		public static void Trigger(SpringCommands command, MMSpringComponentBase targetSpring, MMChannelData channelData, Vector3 moveToValue = default(Vector3), Vector3 bumpAmount = default(Vector3), Vector3 moveToRandomValueMin = default(Vector3), Vector3 moveToRandomValueMax = default(Vector3), Vector3 bumpAmountRandomValueMin = default(Vector3), Vector3 bumpAmountRandomValueMax = default(Vector3), bool overrideDamping = false, Vector3 newDamping = default(Vector3), bool overrideFrequency = false, Vector3 newFrequency = default(Vector3))
		{
			MMSpringVector3Event.e.ChannelData = channelData;
			MMSpringVector3Event.e.TargetSpring = targetSpring;
			MMSpringVector3Event.e.Command = command;
			MMSpringVector3Event.e.MoveToValue = moveToValue;
			MMSpringVector3Event.e.BumpAmount = bumpAmount;
			MMSpringVector3Event.e.MoveToRandomValueMin = moveToRandomValueMin;
			MMSpringVector3Event.e.MoveToRandomValueMax = moveToRandomValueMax;
			MMSpringVector3Event.e.BumpAmountRandomValueMin = bumpAmountRandomValueMin;
			MMSpringVector3Event.e.BumpAmountRandomValueMax = bumpAmountRandomValueMax;
			MMSpringVector3Event.e.OverrideDamping = overrideDamping;
			MMSpringVector3Event.e.NewDamping = newDamping;
			MMSpringVector3Event.e.OverrideFrequency = overrideFrequency;
			MMSpringVector3Event.e.NewFrequency = newFrequency;
			MMEventManager.TriggerEvent<MMSpringVector3Event>(MMSpringVector3Event.e);
		}

		// Token: 0x04001412 RID: 5138
		private static MMSpringVector3Event e;

		// Token: 0x04001413 RID: 5139
		public MMChannelData ChannelData;

		// Token: 0x04001414 RID: 5140
		public MMSpringComponentBase TargetSpring;

		// Token: 0x04001415 RID: 5141
		public SpringCommands Command;

		// Token: 0x04001416 RID: 5142
		public Vector3 MoveToValue;

		// Token: 0x04001417 RID: 5143
		public Vector3 BumpAmount;

		// Token: 0x04001418 RID: 5144
		public Vector3 MoveToRandomValueMin;

		// Token: 0x04001419 RID: 5145
		public Vector3 MoveToRandomValueMax;

		// Token: 0x0400141A RID: 5146
		public Vector3 BumpAmountRandomValueMin;

		// Token: 0x0400141B RID: 5147
		public Vector3 BumpAmountRandomValueMax;

		// Token: 0x0400141C RID: 5148
		public bool OverrideDamping;

		// Token: 0x0400141D RID: 5149
		public Vector3 NewDamping;

		// Token: 0x0400141E RID: 5150
		public bool OverrideFrequency;

		// Token: 0x0400141F RID: 5151
		public Vector3 NewFrequency;
	}
}
