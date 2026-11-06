using System;
using System.Collections.Generic;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;

namespace Features.NPC
{
	// Token: 0x02000EE7 RID: 3815
	[Serializable]
	public sealed class NPCRosterSnapshot
	{
		// Token: 0x04007555 RID: 30037
		public string runId;

		// Token: 0x04007556 RID: 30038
		public NpcPopulationMode populationMode;

		// Token: 0x04007557 RID: 30039
		public List<NPCRosterEntry> entries = new List<NPCRosterEntry>();
	}
}
