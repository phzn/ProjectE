using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000019 RID: 25
	internal struct RenderGraphLogIndent : IDisposable
	{
		// Token: 0x06000110 RID: 272 RVA: 0x0000739A File Offset: 0x0000559A
		public RenderGraphLogIndent(RenderGraphLogger logger, int indentation = 1)
		{
			this.m_Disposed = false;
			this.m_Indentation = indentation;
			this.m_Logger = logger;
			this.m_Logger.IncrementIndentation(this.m_Indentation);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000073C2 File Offset: 0x000055C2
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000073CB File Offset: 0x000055CB
		private void Dispose(bool disposing)
		{
			if (this.m_Disposed)
			{
				return;
			}
			if (disposing && this.m_Logger != null)
			{
				this.m_Logger.DecrementIndentation(this.m_Indentation);
			}
			this.m_Disposed = true;
		}

		// Token: 0x040000A1 RID: 161
		private int m_Indentation;

		// Token: 0x040000A2 RID: 162
		private RenderGraphLogger m_Logger;

		// Token: 0x040000A3 RID: 163
		private bool m_Disposed;
	}
}
