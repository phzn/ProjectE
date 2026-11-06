using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000263 RID: 611
	public struct MMSpringVector4Event
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x0004C3B0 File Offset: 0x0004A5B0
		public static void Trigger(SpringCommands command, MMSpringComponentBase targetSpring, MMChannelData channelData, Vector4 moveToValue = default(Vector4), Vector4 bumpAmount = default(Vector4), Vector4 moveToRandomValueMin = default(Vector4), Vector4 moveToRandomValueMax = default(Vector4), Vector4 bumpAmountRandomValueMin = default(Vector4), Vector4 bumpAmountRandomValueMax = default(Vector4), bool overrideDamping = false, Vector4 newDamping = default(Vector4), bool overrideFrequency = false, Vector4 newFrequency = default(Vector4))
		{
			MMSpringVector4Event.e.ChannelData = channelData;
			MMSpringVector4Event.e.TargetSpring = targetSpring;
			MMSpringVector4Event.e.Command = command;
			MMSpringVector4Event.e.MoveToValue = moveToValue;
			MMSpringVector4Event.e.BumpAmount = bumpAmount;
			MMSpringVector4Event.e.MoveToRandomValueMin = moveToRandomValueMin;
			MMSpringVector4Event.e.MoveToRandomValueMax = moveToRandomValueMax;
			MMSpringVector4Event.e.BumpAmountRandomValueMin = bumpAmountRandomValueMin;
			MMSpringVector4Event.e.BumpAmountRandomValueMax = bumpAmountRandomValueMax;
			MMSpringVector4Event.e.OverrideDamping = overrideDamping;
			MMSpringVector4Event.e.NewDamping = newDamping;
			MMSpringVector4Event.e.OverrideFrequency = overrideFrequency;
			MMSpringVector4Event.e.NewFrequency = newFrequency;
			MMEventManager.TriggerEvent<MMSpringVector4Event>(MMSpringVector4Event.e);
		}

		// Token: 0x04001430 RID: 5168
		private static MMSpringVector4Event e;

		// Token: 0x04001431 RID: 5169
		public MMChannelData ChannelData;

		// Token: 0x04001432 RID: 5170
		public MMSpringComponentBase TargetSpring;

		// Token: 0x04001433 RID: 5171
		public SpringCommands Command;

		// Token: 0x04001434 RID: 5172
		public Vector4 MoveToValue;

		// Token: 0x04001435 RID: 5173
		public Vector4 BumpAmount;

		// Token: 0x04001436 RID: 5174
		public Vector4 MoveToRandomValueMin;

		// Token: 0x04001437 RID: 5175
		public Vector4 MoveToRandomValueMax;

		// Token: 0x04001438 RID: 5176
		public Vector4 BumpAmountRandomValueMin;

		// Token: 0x04001439 RID: 5177
		public Vector4 BumpAmountRandomValueMax;

		// Token: 0x0400143A RID: 5178
		public bool OverrideDamping;

		// Token: 0x0400143B RID: 5179
		public Vector4 NewDamping;

		// Token: 0x0400143C RID: 5180
		public bool OverrideFrequency;

		// Token: 0x0400143D RID: 5181
		public Vector4 NewFrequency;
	}
}
