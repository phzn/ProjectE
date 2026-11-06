using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000359 RID: 857
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	internal readonly struct Marker
	{
		// Token: 0x06001A13 RID: 6675 RVA: 0x000573E0 File Offset: 0x000555E0
		public Marker(int count, int index)
		{
			this.Count = count;
			this.Index = index;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000573F0 File Offset: 0x000555F0
		public int Count { get; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000573F8 File Offset: 0x000555F8
		public int Index { get; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x00057400 File Offset: 0x00055600
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{0}: {1}, {2}: {3}", new object[] { "Index", this.Index, "Count", this.Count });
			}
		}
	}
}
