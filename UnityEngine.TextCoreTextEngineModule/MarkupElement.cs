using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000046 RID: 70
	internal struct MarkupElement
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00021F30 File Offset: 0x00020130
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00021F60 File Offset: 0x00020160
		public int NameHashCode
		{
			get
			{
				return (this.m_Attributes == null) ? 0 : this.m_Attributes[0].NameHashCode;
			}
			set
			{
				bool flag = this.m_Attributes == null;
				if (flag)
				{
					this.m_Attributes = new MarkupAttribute[8];
				}
				this.m_Attributes[0].NameHashCode = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00021F9C File Offset: 0x0002019C
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00021FCA File Offset: 0x000201CA
		public int ValueHashCode
		{
			get
			{
				return (this.m_Attributes == null) ? 0 : this.m_Attributes[0].ValueHashCode;
			}
			set
			{
				this.m_Attributes[0].ValueHashCode = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00021FE0 File Offset: 0x000201E0
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0002200E File Offset: 0x0002020E
		public int ValueStartIndex
		{
			get
			{
				return (this.m_Attributes == null) ? 0 : this.m_Attributes[0].ValueStartIndex;
			}
			set
			{
				this.m_Attributes[0].ValueStartIndex = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00022024 File Offset: 0x00020224
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00022052 File Offset: 0x00020252
		public int ValueLength
		{
			get
			{
				return (this.m_Attributes == null) ? 0 : this.m_Attributes[0].ValueLength;
			}
			set
			{
				this.m_Attributes[0].ValueLength = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00022068 File Offset: 0x00020268
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00022080 File Offset: 0x00020280
		public MarkupAttribute[] Attributes
		{
			get
			{
				return this.m_Attributes;
			}
			set
			{
				this.m_Attributes = value;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0002208C File Offset: 0x0002028C
		public MarkupElement(int nameHashCode, int startIndex, int length)
		{
			this.m_Attributes = new MarkupAttribute[8];
			this.m_Attributes[0].NameHashCode = nameHashCode;
			this.m_Attributes[0].ValueStartIndex = startIndex;
			this.m_Attributes[0].ValueLength = length;
		}

		// Token: 0x04000388 RID: 904
		private MarkupAttribute[] m_Attributes;
	}
}
