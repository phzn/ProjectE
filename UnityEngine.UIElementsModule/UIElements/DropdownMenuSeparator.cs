using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200017B RID: 379
	public class DropdownMenuSeparator : DropdownMenuItem
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00030E63 File Offset: 0x0002F063
		public string subMenuPath { get; }

		// Token: 0x06000C27 RID: 3111 RVA: 0x00030E6B File Offset: 0x0002F06B
		public DropdownMenuSeparator(string subMenuPath)
		{
			this.subMenuPath = subMenuPath;
		}
	}
}
