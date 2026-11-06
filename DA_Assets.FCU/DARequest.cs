using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public struct DARequest
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000004 RID: 4 RVA: 0x0000207F File Offset: 0x0000027F
	// (set) Token: 0x06000005 RID: 5 RVA: 0x00002087 File Offset: 0x00000287
	public RequestName Name { readonly get; set; }

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000006 RID: 6 RVA: 0x00002090 File Offset: 0x00000290
	// (set) Token: 0x06000007 RID: 7 RVA: 0x00002098 File Offset: 0x00000298
	public string Query { readonly get; set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000020A1 File Offset: 0x000002A1
	// (set) Token: 0x06000009 RID: 9 RVA: 0x000020A9 File Offset: 0x000002A9
	public RequestType RequestType { readonly get; set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000020B2 File Offset: 0x000002B2
	// (set) Token: 0x0600000B RID: 11 RVA: 0x000020BA File Offset: 0x000002BA
	public RequestHeader RequestHeader { readonly get; set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000C RID: 12 RVA: 0x000020C3 File Offset: 0x000002C3
	// (set) Token: 0x0600000D RID: 13 RVA: 0x000020CB File Offset: 0x000002CB
	public WWWForm WWWForm { readonly get; set; }
}
