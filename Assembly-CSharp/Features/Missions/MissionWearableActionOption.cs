using System;

namespace Features.Missions
{
	// Token: 0x02000F9C RID: 3996
	public readonly struct MissionWearableActionOption
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x003123B0 File Offset: 0x003105B0
		public MissionWearableActionOption(string wearableId, string wearableName, string actionId, string actionName, bool singleUse = false)
		{
			this.WearableId = wearableId ?? string.Empty;
			this.WearableName = wearableName ?? string.Empty;
			this.ActionId = actionId ?? string.Empty;
			this.ActionName = actionName ?? string.Empty;
			this.SingleUse = singleUse;
		}

		// Token: 0x04007B6D RID: 31597
		public readonly string WearableId;

		// Token: 0x04007B6E RID: 31598
		public readonly string WearableName;

		// Token: 0x04007B6F RID: 31599
		public readonly string ActionId;

		// Token: 0x04007B70 RID: 31600
		public readonly string ActionName;

		// Token: 0x04007B71 RID: 31601
		public readonly bool SingleUse;
	}
}
