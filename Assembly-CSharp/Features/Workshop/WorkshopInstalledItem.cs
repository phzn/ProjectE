using System;

namespace Features.Workshop
{
	// Token: 0x02000B42 RID: 2882
	public sealed class WorkshopInstalledItem
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x060051D2 RID: 20946 RVA: 0x001FB465 File Offset: 0x001F9665
		public ulong Id { get; }

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x060051D3 RID: 20947 RVA: 0x001FB46D File Offset: 0x001F966D
		public string Folder { get; }

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x060051D4 RID: 20948 RVA: 0x001FB475 File Offset: 0x001F9675
		public ulong SizeBytes { get; }

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x060051D5 RID: 20949 RVA: 0x001FB47D File Offset: 0x001F967D
		public uint UpdatedAt { get; }

		// Token: 0x060051D6 RID: 20950 RVA: 0x001FB485 File Offset: 0x001F9685
		public WorkshopInstalledItem(ulong id, string folder, ulong sizeBytes, uint updatedAt)
		{
			this.Id = id;
			this.Folder = folder ?? string.Empty;
			this.SizeBytes = sizeBytes;
			this.UpdatedAt = updatedAt;
		}
	}
}
