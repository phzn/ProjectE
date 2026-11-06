using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000AA8 RID: 2728
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	internal readonly struct CopyPosition
	{
		// Token: 0x060061A9 RID: 25001 RVA: 0x001467F0 File Offset: 0x001449F0
		internal CopyPosition(int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x00146800 File Offset: 0x00144A00
		public static CopyPosition Start
		{
			get
			{
				return default(CopyPosition);
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x060061AB RID: 25003 RVA: 0x00146816 File Offset: 0x00144A16
		internal int Row { get; }

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x060061AC RID: 25004 RVA: 0x0014681E File Offset: 0x00144A1E
		internal int Column { get; }

		// Token: 0x060061AD RID: 25005 RVA: 0x00146826 File Offset: 0x00144A26
		public CopyPosition Normalize(int endColumn)
		{
			if (this.Column != endColumn)
			{
				return this;
			}
			return new CopyPosition(this.Row + 1, 0);
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x060061AE RID: 25006 RVA: 0x00146846 File Offset: 0x00144A46
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("[{0}, {1}]", this.Row, this.Column);
			}
		}
	}
}
