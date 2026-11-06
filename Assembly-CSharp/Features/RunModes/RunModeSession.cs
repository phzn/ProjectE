using System;
using Features.NPC;

namespace Features.RunModes
{
	// Token: 0x02000D79 RID: 3449
	[Serializable]
	public sealed class RunModeSession
	{
		// Token: 0x040067F3 RID: 26611
		public int version = 7;

		// Token: 0x040067F4 RID: 26612
		public bool prepared;

		// Token: 0x040067F5 RID: 26613
		public RunModeKind mode = RunModeKind.Classic;

		// Token: 0x040067F6 RID: 26614
		public bool classicTutorial;

		// Token: 0x040067F7 RID: 26615
		public ClassicRunKind classicRunKind;

		// Token: 0x040067F8 RID: 26616
		public ClassicRunSettings classicSettings = new ClassicRunSettings();

		// Token: 0x040067F9 RID: 26617
		public FreeRunSettings settings = new FreeRunSettings();

		// Token: 0x040067FA RID: 26618
		public bool startingItemsGranted;

		// Token: 0x040067FB RID: 26619
		public bool mapsVisit;

		// Token: 0x040067FC RID: 26620
		public bool hasWorldSeed;

		// Token: 0x040067FD RID: 26621
		public int actualWorldSeed;

		// Token: 0x040067FE RID: 26622
		public LongWayHomeRunSettings longWayHomeSettings = new LongWayHomeRunSettings();

		// Token: 0x040067FF RID: 26623
		public NPCRosterSnapshot npcRoster;

		// Token: 0x04006800 RID: 26624
		public string startingRankId;

		// Token: 0x04006801 RID: 26625
		public bool startedAsAdmin;

		// Token: 0x04006802 RID: 26626
		public bool leaderboardDisqualified;

		// Token: 0x04006803 RID: 26627
		public string rankedAccountId;

		// Token: 0x04006804 RID: 26628
		public string rankedRunId;

		// Token: 0x04006805 RID: 26629
		public string rankedBuildIdentity;

		// Token: 0x04006806 RID: 26630
		public string rankedIntegritySeal;

		// Token: 0x04006807 RID: 26631
		public string leaderboardExclusionReason;

		// Token: 0x04006808 RID: 26632
		public bool collarHandoffPending;

		// Token: 0x04006809 RID: 26633
		public bool collarHandoffWorn;

		// Token: 0x0400680A RID: 26634
		public CollarRunHandoff collarHandoff;
	}
}
