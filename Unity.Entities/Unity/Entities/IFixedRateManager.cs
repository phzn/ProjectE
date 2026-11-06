using System;

namespace Unity.Entities
{
	// Token: 0x02000261 RID: 609
	[Obsolete("This interface has been renamed to IRateManager (RemovedAFter Entities 1.0)", true)]
	public interface IFixedRateManager
	{
		// Token: 0x06001093 RID: 4243
		bool ShouldGroupUpdate(ComponentSystemGroup group);

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06001094 RID: 4244
		// (set) Token: 0x06001095 RID: 4245
		float Timestep { get; set; }
	}
}
