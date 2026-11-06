using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Jobs
{
	// Token: 0x02000181 RID: 385
	[BurstCompile]
	public struct JobRotate3DArray<[IsUnmanaged] T> : IJob where T : struct, ValueType
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x0004142C File Offset: 0x0003F62C
		public void Execute()
		{
			int x = this.size.x;
			int y = this.size.y;
			int z = this.size.z;
			UnsafeSpan<T> unsafeSpan = this.arr.AsUnsafeSpan<T>();
			this.dx %= x;
			this.dz %= z;
			if (this.dx != 0)
			{
				if (this.dx < 0)
				{
					this.dx = x + this.dx;
				}
				UnsafeSpan<T> unsafeSpan2 = new NativeArray<T>(this.dx, Allocator.Temp, NativeArrayOptions.ClearMemory).AsUnsafeSpan<T>();
				for (int i = 0; i < y; i++)
				{
					int num = i * x * z;
					for (int j = 0; j < z; j++)
					{
						unsafeSpan.Slice(num + j * x + x - this.dx, this.dx).CopyTo(unsafeSpan2);
						unsafeSpan.Move(num + j * x, num + j * x + this.dx, x - this.dx);
						unsafeSpan2.CopyTo(unsafeSpan.Slice(num + j * x, this.dx));
					}
				}
			}
			if (this.dz != 0)
			{
				if (this.dz < 0)
				{
					this.dz = z + this.dz;
				}
				UnsafeSpan<T> unsafeSpan3 = new NativeArray<T>(this.dz * x, Allocator.Temp, NativeArrayOptions.ClearMemory).AsUnsafeSpan<T>();
				for (int k = 0; k < y; k++)
				{
					int num2 = k * x * z;
					unsafeSpan.Slice(num2 + (z - this.dz) * x, this.dz * x).CopyTo(unsafeSpan3);
					unsafeSpan.Move(num2, num2 + this.dz * x, (z - this.dz) * x);
					unsafeSpan3.CopyTo(unsafeSpan.Slice(num2, this.dz * x));
				}
			}
		}

		// Token: 0x040007BA RID: 1978
		public NativeArray<T> arr;

		// Token: 0x040007BB RID: 1979
		public int3 size;

		// Token: 0x040007BC RID: 1980
		public int dx;

		// Token: 0x040007BD RID: 1981
		public int dz;
	}
}
