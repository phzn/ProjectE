using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace MagicaCloth2
{
	// Token: 0x02000110 RID: 272
	public class ExProcessingList<T> : IDisposable, IValid where T : struct
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x0002E829 File Offset: 0x0002CA29
		public void Dispose()
		{
			if (this.Counter.IsCreated)
			{
				this.Counter.Dispose();
			}
			if (this.Buffer.IsCreated)
			{
				this.Buffer.Dispose();
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0002E85B File Offset: 0x0002CA5B
		public bool IsValid()
		{
			return this.Counter.IsCreated;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0002E868 File Offset: 0x0002CA68
		public ExProcessingList()
		{
			this.Counter = new NativeReference<int>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0002E884 File Offset: 0x0002CA84
		public void UpdateBuffer(int capacity)
		{
			if (!this.Buffer.IsCreated || this.Buffer.Length < capacity)
			{
				if (this.Buffer.IsCreated)
				{
					this.Buffer.Dispose();
				}
				this.Buffer = new NativeArray<T>(capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0002E8D2 File Offset: 0x0002CAD2
		public unsafe int* GetJobSchedulePtr()
		{
			return this.Counter.GetUnsafePtrWithoutChecks<int>();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0002E8E0 File Offset: 0x0002CAE0
		public override string ToString()
		{
			int num = (this.Counter.IsCreated ? this.Counter.Value : 0);
			int num2 = (this.Buffer.IsCreated ? this.Buffer.Length : 0);
			return string.Format("ExProcessingList BufferLength:{0} Counter:{1}", num2, num);
		}

		// Token: 0x040008A6 RID: 2214
		public NativeReference<int> Counter;

		// Token: 0x040008A7 RID: 2215
		public NativeArray<T> Buffer;
	}
}
