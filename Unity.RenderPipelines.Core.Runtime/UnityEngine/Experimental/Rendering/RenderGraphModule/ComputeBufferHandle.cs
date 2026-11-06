using System;
using System.Diagnostics;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200001F RID: 31
	[DebuggerDisplay("ComputeBuffer ({handle.index})")]
	public struct ComputeBufferHandle
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007B08 File Offset: 0x00005D08
		public static ComputeBufferHandle nullHandle
		{
			get
			{
				return ComputeBufferHandle.s_NullHandle;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007B0F File Offset: 0x00005D0F
		internal ComputeBufferHandle(int handle, bool shared = false)
		{
			this.handle = new ResourceHandle(handle, RenderGraphResourceType.ComputeBuffer, shared);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007B1F File Offset: 0x00005D1F
		public static implicit operator ComputeBuffer(ComputeBufferHandle buffer)
		{
			if (!buffer.IsValid())
			{
				return null;
			}
			return RenderGraphResourceRegistry.current.GetComputeBuffer(in buffer);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007B38 File Offset: 0x00005D38
		public bool IsValid()
		{
			return this.handle.IsValid();
		}

		// Token: 0x040000C0 RID: 192
		private static ComputeBufferHandle s_NullHandle;

		// Token: 0x040000C1 RID: 193
		internal ResourceHandle handle;
	}
}
