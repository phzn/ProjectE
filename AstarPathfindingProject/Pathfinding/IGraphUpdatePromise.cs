using System;
using System.Collections.Generic;
using Unity.Jobs;

namespace Pathfinding
{
	// Token: 0x020000A4 RID: 164
	public interface IGraphUpdatePromise
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00005C0D File Offset: 0x00003E0D
		float Progress
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00003792 File Offset: 0x00001992
		IEnumerator<JobHandle> Prepare()
		{
			return null;
		}

		// Token: 0x06000584 RID: 1412
		void Apply(IGraphUpdateContext context);
	}
}
