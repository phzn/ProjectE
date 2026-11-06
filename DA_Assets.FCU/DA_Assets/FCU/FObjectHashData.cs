using System;
using System.Collections.Generic;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x02000036 RID: 54
	public struct FObjectHashData
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00003B59 File Offset: 0x00001D59
		public FObjectHashData(FObject fobject, List<FieldHashData> hashes, List<EffectHashData> effectDatas, int indent)
		{
			this.fobject = fobject;
			this.hashes = hashes;
			this.indent = indent;
			this.effectDatas = effectDatas;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00003B78 File Offset: 0x00001D78
		public int Indent
		{
			get
			{
				return this.indent;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003B80 File Offset: 0x00001D80
		public FObject FObject
		{
			get
			{
				return this.fobject;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00003B88 File Offset: 0x00001D88
		public List<FieldHashData> FieldHashes
		{
			get
			{
				return this.hashes;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003B90 File Offset: 0x00001D90
		public List<EffectHashData> EffectDatas
		{
			get
			{
				return this.effectDatas;
			}
		}

		// Token: 0x0400032E RID: 814
		private int indent;

		// Token: 0x0400032F RID: 815
		private FObject fobject;

		// Token: 0x04000330 RID: 816
		private List<FieldHashData> hashes;

		// Token: 0x04000331 RID: 817
		public List<EffectHashData> effectDatas;
	}
}
