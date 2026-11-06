using System;

namespace MagicaCloth2
{
	// Token: 0x0200010A RID: 266
	public struct DataChunk
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0002D863 File Offset: 0x0002BA63
		public bool IsValid
		{
			get
			{
				return this.dataLength > 0;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0002D870 File Offset: 0x0002BA70
		public static DataChunk Empty
		{
			get
			{
				return default(DataChunk);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0002D886 File Offset: 0x0002BA86
		public DataChunk(int sindex, int length)
		{
			this.startIndex = sindex;
			this.dataLength = length;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0002D896 File Offset: 0x0002BA96
		public DataChunk(int sindex)
		{
			this.startIndex = sindex;
			this.dataLength = 1;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002D8A6 File Offset: 0x0002BAA6
		public void Clear()
		{
			this.startIndex = 0;
			this.dataLength = 0;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0002D8B6 File Offset: 0x0002BAB6
		public override string ToString()
		{
			return string.Format("[startIndex={0}, dataLength={1}]", this.startIndex, this.dataLength);
		}

		// Token: 0x0400089B RID: 2203
		public int startIndex;

		// Token: 0x0400089C RID: 2204
		public int dataLength;
	}
}
