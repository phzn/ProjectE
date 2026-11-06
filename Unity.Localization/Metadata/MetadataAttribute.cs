using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000D8 RID: 216
	[AttributeUsage(AttributeTargets.Class)]
	public class MetadataAttribute : Attribute
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00014C75 File Offset: 0x00012E75
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00014C7D File Offset: 0x00012E7D
		public string MenuItem { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00014C86 File Offset: 0x00012E86
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00014C8E File Offset: 0x00012E8E
		public bool AllowMultiple { get; set; } = true;

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00014C97 File Offset: 0x00012E97
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00014C9F File Offset: 0x00012E9F
		public MetadataType AllowedTypes { get; set; } = MetadataType.All;
	}
}
