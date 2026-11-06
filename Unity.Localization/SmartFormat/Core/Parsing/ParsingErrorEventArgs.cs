using System;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000083 RID: 131
	public class ParsingErrorEventArgs : EventArgs
	{
		// Token: 0x06000373 RID: 883 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		internal ParsingErrorEventArgs(ParsingErrors errors, bool throwsException)
		{
			this.Errors = errors;
			this.ThrowsException = throwsException;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000DCD2 File Offset: 0x0000BED2
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000DCDA File Offset: 0x0000BEDA
		public ParsingErrors Errors { get; internal set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000DCE3 File Offset: 0x0000BEE3
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000DCEB File Offset: 0x0000BEEB
		public bool ThrowsException { get; internal set; }
	}
}
