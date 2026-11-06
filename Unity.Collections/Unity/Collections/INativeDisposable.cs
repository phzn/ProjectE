using System;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x0200003C RID: 60
	public interface INativeDisposable : IDisposable
	{
		// Token: 0x06000145 RID: 325
		JobHandle Dispose(JobHandle inputDeps);
	}
}
