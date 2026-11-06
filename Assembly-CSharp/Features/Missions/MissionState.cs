using System;

namespace Features.Missions
{
	// Token: 0x02000F9E RID: 3998
	[Serializable]
	public sealed class MissionState
	{
		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06007865 RID: 30821 RVA: 0x00312406 File Offset: 0x00310606
		public float NormalizedProgress
		{
			get
			{
				if (this.target > 0f)
				{
					return Math.Min(1f, this.progress / this.target);
				}
				return 0f;
			}
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06007866 RID: 30822 RVA: 0x00312432 File Offset: 0x00310632
		public bool IsComplete
		{
			get
			{
				return this.target > 0f && this.progress >= this.target;
			}
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x00312454 File Offset: 0x00310654
		public MissionState Clone()
		{
			return (MissionState)base.MemberwiseClone();
		}

		// Token: 0x04007BB4 RID: 31668
		public string id;

		// Token: 0x04007BB5 RID: 31669
		public MissionScope scope;

		// Token: 0x04007BB6 RID: 31670
		public MissionObjectiveKind kind;

		// Token: 0x04007BB7 RID: 31671
		public string contextId;

		// Token: 0x04007BB8 RID: 31672
		public string contextName;

		// Token: 0x04007BB9 RID: 31673
		public string qualifierId;

		// Token: 0x04007BBA RID: 31674
		public string qualifierName;

		// Token: 0x04007BBB RID: 31675
		public string secondaryQualifierId;

		// Token: 0x04007BBC RID: 31676
		public string secondaryQualifierName;

		// Token: 0x04007BBD RID: 31677
		public string description;

		// Token: 0x04007BBE RID: 31678
		public float target;

		// Token: 0x04007BBF RID: 31679
		public float progress;

		// Token: 0x04007BC0 RID: 31680
		public int xpReward;

		// Token: 0x04007BC1 RID: 31681
		public int generation;

		// Token: 0x04007BC2 RID: 31682
		public MissionCategory category;

		// Token: 0x04007BC3 RID: 31683
		public int tier = 1;

		// Token: 0x04007BC4 RID: 31684
		public int minimumRankIndex;

		// Token: 0x04007BC5 RID: 31685
		public bool rankSlot;

		// Token: 0x04007BC6 RID: 31686
		public bool secret;
	}
}
