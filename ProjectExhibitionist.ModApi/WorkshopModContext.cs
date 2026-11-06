using System;

namespace ProjectExhibitionist.ModApi
{
	// Token: 0x02000004 RID: 4
	public sealed class WorkshopModContext
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000002B8
		public ulong WorkshopId { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020C0 File Offset: 0x000002C0
		public string ContentDirectory { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		public string DataDirectory { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020D0 File Offset: 0x000002D0
		public string GameVersion { get; }

		// Token: 0x06000008 RID: 8 RVA: 0x000020D8 File Offset: 0x000002D8
		public WorkshopModContext(ulong workshopId, string contentDirectory, string dataDirectory, string gameVersion, Action<string> log)
		{
			this.WorkshopId = workshopId;
			this.ContentDirectory = contentDirectory;
			this.DataDirectory = dataDirectory;
			this.GameVersion = gameVersion;
			this._log = log;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002105 File Offset: 0x00000305
		public void Log(string message)
		{
			Action<string> log = this._log;
			if (log == null)
			{
				return;
			}
			log(message ?? string.Empty);
		}

		// Token: 0x04000001 RID: 1
		public const int CurrentApiVersion = 1;

		// Token: 0x04000006 RID: 6
		private readonly Action<string> _log;
	}
}
