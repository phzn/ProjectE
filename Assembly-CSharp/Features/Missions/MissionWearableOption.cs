using System;

namespace Features.Missions
{
	// Token: 0x02000F9B RID: 3995
	public readonly struct MissionWearableOption
	{
		// Token: 0x06007863 RID: 30819 RVA: 0x00312376 File Offset: 0x00310576
		public MissionWearableOption(string id, string name, bool breakable, bool isWorn = false, bool blocksEmotes = false)
		{
			this.Id = id ?? string.Empty;
			this.Name = name ?? string.Empty;
			this.Breakable = breakable;
			this.IsWorn = isWorn;
			this.BlocksEmotes = blocksEmotes;
		}

		// Token: 0x04007B68 RID: 31592
		public readonly string Id;

		// Token: 0x04007B69 RID: 31593
		public readonly string Name;

		// Token: 0x04007B6A RID: 31594
		public readonly bool Breakable;

		// Token: 0x04007B6B RID: 31595
		public readonly bool IsWorn;

		// Token: 0x04007B6C RID: 31596
		public readonly bool BlocksEmotes;
	}
}
