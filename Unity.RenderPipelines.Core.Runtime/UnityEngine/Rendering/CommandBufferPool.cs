using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000034 RID: 52
	public static class CommandBufferPool
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000A50E File Offset: 0x0000870E
		public static CommandBuffer Get()
		{
			CommandBuffer commandBuffer = CommandBufferPool.s_BufferPool.Get();
			commandBuffer.name = "";
			return commandBuffer;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A525 File Offset: 0x00008725
		public static CommandBuffer Get(string name)
		{
			CommandBuffer commandBuffer = CommandBufferPool.s_BufferPool.Get();
			commandBuffer.name = name;
			return commandBuffer;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A538 File Offset: 0x00008738
		public static void Release(CommandBuffer buffer)
		{
			CommandBufferPool.s_BufferPool.Release(buffer);
		}

		// Token: 0x04000133 RID: 307
		private static ObjectPool<CommandBuffer> s_BufferPool = new ObjectPool<CommandBuffer>(null, delegate(CommandBuffer x)
		{
			x.Clear();
		}, true);
	}
}
