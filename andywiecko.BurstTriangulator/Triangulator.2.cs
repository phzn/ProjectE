using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000012 RID: 18
	public class Triangulator<[IsUnmanaged] T2> : IDisposable where T2 : struct, ValueType
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000271A File Offset: 0x0000091A
		public TriangulationSettings Settings { get; } = new TriangulationSettings();

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002722 File Offset: 0x00000922
		// (set) Token: 0x06000056 RID: 86 RVA: 0x0000272A File Offset: 0x0000092A
		public InputData<T2> Input { get; set; } = new InputData<T2>();

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002733 File Offset: 0x00000933
		public OutputData<T2> Output { get; }

		// Token: 0x06000058 RID: 88 RVA: 0x0000273C File Offset: 0x0000093C
		public Triangulator(int capacity, Allocator allocator)
		{
			this.outputPositions = new NativeList<T2>(capacity, allocator);
			this.triangles = new NativeList<int>(6 * capacity, allocator);
			this.status = new NativeReference<Status>(Status.Ok, allocator);
			this.halfedges = new NativeList<int>(6 * capacity, allocator);
			this.constrainedHalfedges = new NativeList<HalfedgeState>(6 * capacity, allocator);
			this.Output = new OutputData<T2>(this);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000027D5 File Offset: 0x000009D5
		public Triangulator(Allocator allocator)
			: this(16384, allocator)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000027E3 File Offset: 0x000009E3
		public void Dispose()
		{
			this.outputPositions.Dispose();
			this.triangles.Dispose();
			this.status.Dispose();
			this.halfedges.Dispose();
			this.constrainedHalfedges.Dispose();
		}

		// Token: 0x04000043 RID: 67
		internal NativeList<T2> outputPositions;

		// Token: 0x04000044 RID: 68
		internal NativeList<int> triangles;

		// Token: 0x04000045 RID: 69
		internal NativeList<int> halfedges;

		// Token: 0x04000046 RID: 70
		internal NativeList<HalfedgeState> constrainedHalfedges;

		// Token: 0x04000047 RID: 71
		internal NativeReference<Status> status;
	}
}
