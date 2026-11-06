using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000045 RID: 69
	internal struct MarkupAttribute
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00021EA0 File Offset: 0x000200A0
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00021EB8 File Offset: 0x000200B8
		public int NameHashCode
		{
			get
			{
				return this.m_NameHashCode;
			}
			set
			{
				this.m_NameHashCode = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00021EC4 File Offset: 0x000200C4
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00021EDC File Offset: 0x000200DC
		public int ValueHashCode
		{
			get
			{
				return this.m_ValueHashCode;
			}
			set
			{
				this.m_ValueHashCode = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00021EE8 File Offset: 0x000200E8
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00021F00 File Offset: 0x00020100
		public int ValueStartIndex
		{
			get
			{
				return this.m_ValueStartIndex;
			}
			set
			{
				this.m_ValueStartIndex = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00021F0C File Offset: 0x0002010C
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00021F24 File Offset: 0x00020124
		public int ValueLength
		{
			get
			{
				return this.m_ValueLength;
			}
			set
			{
				this.m_ValueLength = value;
			}
		}

		// Token: 0x04000384 RID: 900
		private int m_NameHashCode;

		// Token: 0x04000385 RID: 901
		private int m_ValueHashCode;

		// Token: 0x04000386 RID: 902
		private int m_ValueStartIndex;

		// Token: 0x04000387 RID: 903
		private int m_ValueLength;
	}
}
