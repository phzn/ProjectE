using System;

namespace Unity.Entities
{
	// Token: 0x02000039 RID: 57
	internal struct UpdateIndex
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00008035 File Offset: 0x00006235
		public bool IsManaged
		{
			get
			{
				return (this.Data & 32768) > 0;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008046 File Offset: 0x00006246
		public int Index
		{
			get
			{
				return (int)(this.Data & 32767);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008054 File Offset: 0x00006254
		public UpdateIndex(int index, bool managed)
		{
			this.Data = (ushort)index;
			this.Data |= (ushort)((managed ? 1 : 0) << 15);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008078 File Offset: 0x00006278
		public override string ToString()
		{
			if (!this.IsManaged)
			{
				return "UnManaged: Index " + this.Index.ToString();
			}
			return "Managed: Index " + this.Index.ToString();
		}

		// Token: 0x04000065 RID: 101
		private ushort Data;
	}
}
