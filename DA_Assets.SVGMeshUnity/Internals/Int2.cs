using System;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x02000007 RID: 7
	public struct Int2 : IComparable<Int2>
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003963 File Offset: 0x00001B63
		public Int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003974 File Offset: 0x00001B74
		public int CompareTo(Int2 b)
		{
			int num = this.x - b.x;
			if (num != 0)
			{
				return num;
			}
			return this.y - b.y;
		}

		// Token: 0x0400001F RID: 31
		public int x;

		// Token: 0x04000020 RID: 32
		public int y;
	}
}
