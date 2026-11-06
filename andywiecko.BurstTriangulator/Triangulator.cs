using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000011 RID: 17
	public class Triangulator : IDisposable
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002694 File Offset: 0x00000894
		public TriangulationSettings Settings
		{
			get
			{
				return this.impl.Settings;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000026A1 File Offset: 0x000008A1
		// (set) Token: 0x0600004D RID: 77 RVA: 0x000026AE File Offset: 0x000008AE
		public InputData<double2> Input
		{
			get
			{
				return this.impl.Input;
			}
			set
			{
				this.impl.Input = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000026BC File Offset: 0x000008BC
		public OutputData<double2> Output
		{
			get
			{
				return this.impl.Output;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000026C9 File Offset: 0x000008C9
		public Triangulator(int capacity, Allocator allocator)
		{
			this.impl = new Triangulator<double2>(capacity, allocator);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000026DE File Offset: 0x000008DE
		public Triangulator(Allocator allocator)
		{
			this.impl = new Triangulator<double2>(allocator);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000026F2 File Offset: 0x000008F2
		public void Dispose()
		{
			this.impl.Dispose();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000026FF File Offset: 0x000008FF
		public void Run()
		{
			this.impl.Run();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000270C File Offset: 0x0000090C
		public JobHandle Schedule(JobHandle dependencies = default(JobHandle))
		{
			return this.impl.Schedule(dependencies);
		}

		// Token: 0x0400003F RID: 63
		private readonly Triangulator<double2> impl;
	}
}
