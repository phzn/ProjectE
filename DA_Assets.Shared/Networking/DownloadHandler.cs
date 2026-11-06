using System;

namespace DA_Assets.Networking
{
	// Token: 0x02000007 RID: 7
	public class DownloadHandler
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000025A8 File Offset: 0x000007A8
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000025B0 File Offset: 0x000007B0
		public byte[] data { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000025B9 File Offset: 0x000007B9
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000025C1 File Offset: 0x000007C1
		public string text { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000025CA File Offset: 0x000007CA
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000025D2 File Offset: 0x000007D2
		public string error { get; private set; }

		// Token: 0x06000028 RID: 40 RVA: 0x000025DB File Offset: 0x000007DB
		public void SetData(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000025E4 File Offset: 0x000007E4
		public void SetText(string text)
		{
			this.text = text;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025ED File Offset: 0x000007ED
		public void SetError(string error)
		{
			this.error = error;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000025F6 File Offset: 0x000007F6
		public bool HasError
		{
			get
			{
				return !string.IsNullOrEmpty(this.error);
			}
		}
	}
}
