using System;
using System.Threading;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200031A RID: 794
	public class PostLoadCommandBuffer : IComponentData, IQueryTypeParameter, IDisposable, ICloneable
	{
		// Token: 0x06001546 RID: 5446 RVA: 0x00050965 File Offset: 0x0004EB65
		[Preserve]
		public PostLoadCommandBuffer()
		{
			this.RefCount = 1;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00050974 File Offset: 0x0004EB74
		public void Dispose()
		{
			if (Interlocked.Decrement(ref this.RefCount) == 0)
			{
				this.CommandBuffer.Dispose();
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0005098E File Offset: 0x0004EB8E
		public object Clone()
		{
			Interlocked.Increment(ref this.RefCount);
			return this;
		}

		// Token: 0x040007BB RID: 1979
		public EntityCommandBuffer CommandBuffer;

		// Token: 0x040007BC RID: 1980
		private int RefCount;
	}
}
