using System;

namespace System.Configuration
{
	// Token: 0x0200002D RID: 45
	internal class ConfigurationSaveEventArgs : EventArgs
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006737 File Offset: 0x00004937
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000673F File Offset: 0x0000493F
		public string StreamPath { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006748 File Offset: 0x00004948
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00006750 File Offset: 0x00004950
		public bool Start { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006759 File Offset: 0x00004959
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00006761 File Offset: 0x00004961
		public object Context { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000676A File Offset: 0x0000496A
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00006772 File Offset: 0x00004972
		public bool Failed { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000677B File Offset: 0x0000497B
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00006783 File Offset: 0x00004983
		public Exception Exception { get; private set; }

		// Token: 0x0600019E RID: 414 RVA: 0x0000678C File Offset: 0x0000498C
		public ConfigurationSaveEventArgs(string streamPath, bool start, Exception ex, object context)
		{
			this.StreamPath = streamPath;
			this.Start = start;
			this.Failed = ex != null;
			this.Exception = ex;
			this.Context = context;
		}
	}
}
