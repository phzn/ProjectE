using System;

// Token: 0x02000005 RID: 5
public struct RequestHeader
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000020D4 File Offset: 0x000002D4
	// (set) Token: 0x0600000F RID: 15 RVA: 0x000020DC File Offset: 0x000002DC
	public string Name { readonly get; set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000010 RID: 16 RVA: 0x000020E5 File Offset: 0x000002E5
	// (set) Token: 0x06000011 RID: 17 RVA: 0x000020ED File Offset: 0x000002ED
	public string Value { readonly get; set; }
}
