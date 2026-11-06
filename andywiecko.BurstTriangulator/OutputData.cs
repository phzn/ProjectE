using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x0200000F RID: 15
	public class OutputData<[IsUnmanaged] T2> where T2 : struct, ValueType
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000262E File Offset: 0x0000082E
		public NativeList<T2> Positions
		{
			get
			{
				return this.owner.outputPositions;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000263B File Offset: 0x0000083B
		public NativeList<int> Triangles
		{
			get
			{
				return this.owner.triangles;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002648 File Offset: 0x00000848
		public NativeReference<Status> Status
		{
			get
			{
				return this.owner.status;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002655 File Offset: 0x00000855
		public NativeList<int> Halfedges
		{
			get
			{
				return this.owner.halfedges;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002662 File Offset: 0x00000862
		public NativeList<HalfedgeState> ConstrainedHalfedges
		{
			get
			{
				return this.owner.constrainedHalfedges;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000266F File Offset: 0x0000086F
		[Obsolete("This will be converted into internal ctor.")]
		public OutputData(Triangulator<T2> owner)
		{
			this.owner = owner;
		}

		// Token: 0x0400003D RID: 61
		private readonly Triangulator<T2> owner;
	}
}
