using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200000E RID: 14
	public interface IAspectCreate<T> : IQueryTypeParameter where T : IAspect
	{
		// Token: 0x06000033 RID: 51
		T CreateAspect(Entity entity, ref SystemState system);

		// Token: 0x06000034 RID: 52
		void AddComponentRequirementsTo(ref UnsafeList<ComponentType> all);

		// Token: 0x06000035 RID: 53
		void CompleteDependencyBeforeRO(ref SystemState state);

		// Token: 0x06000036 RID: 54
		void CompleteDependencyBeforeRW(ref SystemState state);
	}
}
