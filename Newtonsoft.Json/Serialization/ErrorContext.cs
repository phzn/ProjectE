using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorContext
	{
		// Token: 0x06000653 RID: 1619 RVA: 0x0001AEC0 File Offset: 0x000190C0
		internal ErrorContext([Nullable(2)] object originalObject, [Nullable(2)] object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001AEE5 File Offset: 0x000190E5
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0001AEED File Offset: 0x000190ED
		internal bool Traced { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001AEF6 File Offset: 0x000190F6
		public Exception Error { get; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001AEFE File Offset: 0x000190FE
		[Nullable(2)]
		public object OriginalObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001AF06 File Offset: 0x00019106
		[Nullable(2)]
		public object Member
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001AF0E File Offset: 0x0001910E
		public string Path { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001AF16 File Offset: 0x00019116
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0001AF1E File Offset: 0x0001911E
		public bool Handled { get; set; }
	}
}
