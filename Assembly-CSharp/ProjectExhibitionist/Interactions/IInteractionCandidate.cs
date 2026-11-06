using System;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x0200090C RID: 2316
	public interface IInteractionCandidate
	{
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003ECD RID: 16077
		float CandidateSqrDistance { get; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003ECE RID: 16078
		bool IsCandidateEligible { get; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003ECF RID: 16079
		int CandidateInstanceId { get; }
	}
}
