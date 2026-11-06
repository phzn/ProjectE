using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002EC RID: 748
	[DisableGeneration]
	internal readonly struct BurstCompatibleAspect : IAspect, IQueryTypeParameter, IAspectCreate<BurstCompatibleAspect>
	{
		// Token: 0x0600142D RID: 5165 RVA: 0x00002287 File Offset: 0x00000487
		public void AddComponentRequirementsTo(ref UnsafeList<ComponentType> all)
		{
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00002287 File Offset: 0x00000487
		public void CompleteDependencyBeforeRO(ref SystemState state)
		{
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00002287 File Offset: 0x00000487
		public void CompleteDependencyBeforeRW(ref SystemState state)
		{
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00038687 File Offset: 0x00036887
		BurstCompatibleAspect IAspectCreate<BurstCompatibleAspect>.CreateAspect(Entity entity, ref SystemState system)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000769 RID: 1897
		public readonly int UnusedField;
	}
}
