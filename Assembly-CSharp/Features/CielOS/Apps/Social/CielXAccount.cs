using System;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001452 RID: 5202
	[Serializable]
	public class CielXAccount
	{
		// Token: 0x0600A0A4 RID: 41124 RVA: 0x00004D1F File Offset: 0x00002F1F
		public CielXAccount()
		{
		}

		// Token: 0x0600A0A5 RID: 41125 RVA: 0x00427E8C File Offset: 0x0042608C
		public CielXAccount(string handle, string displayName, bool verified = false, bool isMe = false)
		{
			this.handle = handle;
			this.displayName = displayName;
			this.verified = verified;
			this.isMe = isMe;
		}

		// Token: 0x0400A455 RID: 42069
		public string handle;

		// Token: 0x0400A456 RID: 42070
		public string displayName;

		// Token: 0x0400A457 RID: 42071
		public bool verified;

		// Token: 0x0400A458 RID: 42072
		public bool isMe;
	}
}
