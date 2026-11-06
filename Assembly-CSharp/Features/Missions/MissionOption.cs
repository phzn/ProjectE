using System;

namespace Features.Missions
{
	// Token: 0x02000F9A RID: 3994
	public readonly struct MissionOption
	{
		// Token: 0x06007862 RID: 30818 RVA: 0x00312308 File Offset: 0x00310508
		public MissionOption(string id, string name, string contextId = "", string contextName = "", bool isMoving = false, bool isMasturbation = false, bool isContextual = false, bool playableNow = true)
		{
			this.Id = id ?? string.Empty;
			this.Name = name ?? string.Empty;
			this.ContextId = contextId ?? string.Empty;
			this.ContextName = contextName ?? string.Empty;
			this.IsMoving = isMoving;
			this.IsMasturbation = isMasturbation;
			this.IsContextual = isContextual;
			this.PlayableNow = playableNow;
		}

		// Token: 0x04007B60 RID: 31584
		public readonly string Id;

		// Token: 0x04007B61 RID: 31585
		public readonly string Name;

		// Token: 0x04007B62 RID: 31586
		public readonly string ContextId;

		// Token: 0x04007B63 RID: 31587
		public readonly string ContextName;

		// Token: 0x04007B64 RID: 31588
		public readonly bool IsMoving;

		// Token: 0x04007B65 RID: 31589
		public readonly bool IsMasturbation;

		// Token: 0x04007B66 RID: 31590
		public readonly bool IsContextual;

		// Token: 0x04007B67 RID: 31591
		public readonly bool PlayableNow;
	}
}
