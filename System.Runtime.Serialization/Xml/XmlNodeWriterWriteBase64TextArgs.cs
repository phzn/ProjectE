using System;

namespace System.Xml
{
	// Token: 0x02000090 RID: 144
	internal class XmlNodeWriterWriteBase64TextArgs
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001FA9F File Offset: 0x0001DC9F
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0001FAA7 File Offset: 0x0001DCA7
		internal byte[] TrailBuffer { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001FAB0 File Offset: 0x0001DCB0
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0001FAB8 File Offset: 0x0001DCB8
		internal int TrailCount { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001FAC1 File Offset: 0x0001DCC1
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0001FAC9 File Offset: 0x0001DCC9
		internal byte[] Buffer { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001FAD2 File Offset: 0x0001DCD2
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0001FADA File Offset: 0x0001DCDA
		internal int Offset { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001FAE3 File Offset: 0x0001DCE3
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0001FAEB File Offset: 0x0001DCEB
		internal int Count { get; set; }
	}
}
