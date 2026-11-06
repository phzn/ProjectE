using System;

namespace DA_Assets.SVGMeshUnity.Internals
{
	// Token: 0x02000008 RID: 8
	public struct Int3 : IComparable<Int3>
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000039A1 File Offset: 0x00001BA1
		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000039B8 File Offset: 0x00001BB8
		public int CompareTo(Int3 b)
		{
			int num = this.x - b.x;
			if (num != 0)
			{
				return num;
			}
			num = this.y - b.y;
			if (num != 0)
			{
				return num;
			}
			return this.z - b.z;
		}

		// Token: 0x04000021 RID: 33
		public int x;

		// Token: 0x04000022 RID: 34
		public int y;

		// Token: 0x04000023 RID: 35
		public int z;
	}
}
