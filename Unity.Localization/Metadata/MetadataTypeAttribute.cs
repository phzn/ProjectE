using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000D7 RID: 215
	[AttributeUsage(AttributeTargets.Field)]
	internal class MetadataTypeAttribute : PropertyAttribute
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00014C55 File Offset: 0x00012E55
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00014C5D File Offset: 0x00012E5D
		public MetadataType Type { get; set; }

		// Token: 0x060005E6 RID: 1510 RVA: 0x00014C66 File Offset: 0x00012E66
		public MetadataTypeAttribute(MetadataType type)
		{
			this.Type = type;
		}
	}
}
