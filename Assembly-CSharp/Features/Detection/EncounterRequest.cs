using System;
using System.Collections.Generic;

namespace Features.Detection
{
	// Token: 0x0200128F RID: 4751
	public class EncounterRequest
	{
		// Token: 0x0400985D RID: 39005
		public NPCSenses initiator;

		// Token: 0x0400985E RID: 39006
		public List<NPCSenses> additionalActors;

		// Token: 0x0400985F RID: 39007
		public EncounterProfileSO profile;

		// Token: 0x04009860 RID: 39008
		public Action onSceneEnded;

		// Token: 0x04009861 RID: 39009
		public bool completeUnderFade;

		// Token: 0x04009862 RID: 39010
		public IEncounterPreReactionPhase preReactionPhase;

		// Token: 0x04009863 RID: 39011
		public IEncounterMode postEmoteMode;
	}
}
