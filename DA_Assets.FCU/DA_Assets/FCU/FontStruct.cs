using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public struct FontStruct
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003C0B File Offset: 0x00001E0B
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00003C13 File Offset: 0x00001E13
		public FontSubset FontSubset
		{
			get
			{
				return this.fontSubset;
			}
			set
			{
				this.fontSubset = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00003C1C File Offset: 0x00001E1C
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00003C24 File Offset: 0x00001E24
		public FontMetadata FontMetadata
		{
			get
			{
				return this.fontMetadata;
			}
			set
			{
				this.fontMetadata = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00003C2D File Offset: 0x00001E2D
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00003C35 File Offset: 0x00001E35
		public byte[] Bytes
		{
			get
			{
				return this.bytes;
			}
			set
			{
				this.bytes = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00003C3E File Offset: 0x00001E3E
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00003C46 File Offset: 0x00001E46
		public Object Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private FontSubset fontSubset;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		private FontItem fontItem;

		// Token: 0x0400033B RID: 827
		[SerializeField]
		private FontMetadata fontMetadata;

		// Token: 0x0400033C RID: 828
		[SerializeField]
		private byte[] bytes;

		// Token: 0x0400033D RID: 829
		[SerializeField]
		private Object font;
	}
}
