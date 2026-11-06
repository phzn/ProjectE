using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000DB RID: 219
	internal struct InclusiveRange
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x000292CE File Offset: 0x000274CE
		public InclusiveRange(short startEnd)
		{
			this.start = startEnd;
			this.end = startEnd;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000292DE File Offset: 0x000274DE
		public InclusiveRange(short start, short end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000292EE File Offset: 0x000274EE
		public void Expand(short index)
		{
			this.start = Math.Min(this.start, index);
			this.end = Math.Max(this.end, index);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00029314 File Offset: 0x00027514
		public void Clamp(short min, short max)
		{
			this.start = Math.Max(min, this.start);
			this.end = Math.Min(max, this.end);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0002933A File Offset: 0x0002753A
		public bool isEmpty
		{
			get
			{
				return this.end < this.start;
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002934A File Offset: 0x0002754A
		public bool Contains(short index)
		{
			return index >= this.start && index <= this.end;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00029363 File Offset: 0x00027563
		public static InclusiveRange Merge(InclusiveRange a, InclusiveRange b)
		{
			return new InclusiveRange(Math.Min(a.start, b.start), Math.Max(a.end, b.end));
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0002938C File Offset: 0x0002758C
		public static InclusiveRange empty
		{
			get
			{
				return new InclusiveRange(short.MaxValue, short.MinValue);
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002939D File Offset: 0x0002759D
		public override string ToString()
		{
			return string.Format("[{0}, {1}]", this.start, this.end);
		}

		// Token: 0x0400056E RID: 1390
		public short start;

		// Token: 0x0400056F RID: 1391
		public short end;
	}
}
