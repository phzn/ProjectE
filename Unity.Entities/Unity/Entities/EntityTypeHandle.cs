using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001CD RID: 461
	[NativeContainer]
	[NativeContainerIsReadOnly]
	public struct EntityTypeHandle
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x00002287 File Offset: 0x00000487
		internal EntityTypeHandle(bool unused)
		{
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00034127 File Offset: 0x00032327
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00002287 File Offset: 0x00000487
		public void Update(ref SystemState state)
		{
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00034135 File Offset: 0x00032335
		public FixedString128Bytes ToFixedString()
		{
			return "EntityTypeHandle";
		}
	}
}
