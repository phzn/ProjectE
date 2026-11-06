using System;
using Unity.Profiling;
using UnityEngine.Profiling;

namespace UnityEngine.Rendering
{
	// Token: 0x0200007A RID: 122
	[Obsolete("Please use ProfilingScope")]
	[IgnoredByDeepProfiler]
	public struct ProfilingSample : IDisposable
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00010751 File Offset: 0x0000E951
		public ProfilingSample(CommandBuffer cmd, string name, CustomSampler sampler = null)
		{
			this.m_Cmd = cmd;
			this.m_Name = name;
			this.m_Disposed = false;
			if (cmd != null && name != "")
			{
				cmd.BeginSample(name);
			}
			this.m_Sampler = sampler;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00010786 File Offset: 0x0000E986
		public ProfilingSample(CommandBuffer cmd, string format, object arg)
		{
			this = new ProfilingSample(cmd, string.Format(format, arg), null);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00010797 File Offset: 0x0000E997
		public ProfilingSample(CommandBuffer cmd, string format, params object[] args)
		{
			this = new ProfilingSample(cmd, string.Format(format, args), null);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000107A8 File Offset: 0x0000E9A8
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000107B1 File Offset: 0x0000E9B1
		private void Dispose(bool disposing)
		{
			if (this.m_Disposed)
			{
				return;
			}
			if (disposing && this.m_Cmd != null && this.m_Name != "")
			{
				this.m_Cmd.EndSample(this.m_Name);
			}
			this.m_Disposed = true;
		}

		// Token: 0x04000215 RID: 533
		private readonly CommandBuffer m_Cmd;

		// Token: 0x04000216 RID: 534
		private readonly string m_Name;

		// Token: 0x04000217 RID: 535
		private bool m_Disposed;

		// Token: 0x04000218 RID: 536
		private CustomSampler m_Sampler;
	}
}
