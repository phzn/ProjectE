using System;

namespace Unity.Entities
{
	// Token: 0x02000262 RID: 610
	public interface IRateManager
	{
		// Token: 0x06001096 RID: 4246
		bool ShouldGroupUpdate(ComponentSystemGroup group);

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06001097 RID: 4247
		// (set) Token: 0x06001098 RID: 4248
		float Timestep { get; set; }
	}
}
