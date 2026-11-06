using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x0200000F RID: 15
	public class Filter
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00004710 File Offset: 0x00002910
		public void Do(Triangles triangles, List<int> result)
		{
			using (Filter.FaceIndex faceIndex = this.IndexCells(triangles))
			{
				int usedSize = faceIndex.Cells.UsedSize;
				if (result.Capacity < usedSize * 3)
				{
					result.Capacity = usedSize * 3;
				}
				result.Clear();
				if (this.Target == 0)
				{
					if (this.Infinity)
					{
						this.FillTriangles(faceIndex.Cells, result);
						this.FillTriangles(faceIndex.Boundary, result);
					}
					else
					{
						this.FillTriangles(faceIndex.Cells, result);
					}
				}
				else
				{
					int num = 1;
					WorkBuffer<int> workBuffer = faceIndex.Active;
					WorkBuffer<int> workBuffer2 = faceIndex.Next;
					WorkBuffer<int> flags = faceIndex.Flags;
					int[] data = flags.Data;
					WorkBuffer<Int3> cells = faceIndex.Cells;
					bool[] data2 = faceIndex.Constraint.Data;
					int[] data3 = faceIndex.Neighbor.Data;
					while (workBuffer.UsedSize > 0 || workBuffer2.UsedSize > 0)
					{
						while (workBuffer.UsedSize > 0)
						{
							int num2 = workBuffer.Pop();
							if (data[num2] != -num)
							{
								data[num2] = num;
								for (int i = 0; i < 3; i++)
								{
									int num3 = data3[3 * num2 + i];
									if (num3 >= 0 && data[num3] == 0)
									{
										if (data2[3 * num2 + i])
										{
											workBuffer2.Push(ref num3);
										}
										else
										{
											workBuffer.Push(ref num3);
											data[num3] = num;
										}
									}
								}
							}
						}
						WorkBuffer<int> workBuffer3 = workBuffer2;
						workBuffer2 = workBuffer;
						workBuffer = workBuffer3;
						workBuffer2.Clear();
						num = -num;
					}
					this.FilterCells(cells, flags);
					this.FillTriangles(cells, result);
					if (this.Infinity)
					{
						this.FillTriangles(faceIndex.Boundary, result);
					}
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000048B4 File Offset: 0x00002AB4
		private Filter.FaceIndex IndexCells(Triangles triangles)
		{
			int num = 0;
			bool flag = false;
			Filter.FaceIndex faceIndex = new Filter.FaceIndex(this.WorkBufferPool);
			triangles.Fill(faceIndex.Cells);
			WorkBuffer<Int3> cells = faceIndex.Cells;
			int usedSize = cells.UsedSize;
			Int3[] data = cells.Data;
			for (int i = 0; i < usedSize; i++)
			{
				Int3 @int = data[i];
				int x = @int.x;
				int y = @int.y;
				int z = @int.z;
				if (y < z)
				{
					if (y < x)
					{
						@int.x = y;
						@int.y = z;
						@int.z = x;
						data[i] = @int;
					}
				}
				else if (z < x)
				{
					@int.x = z;
					@int.y = x;
					@int.z = y;
					data[i] = @int;
				}
			}
			WorkBuffer<Int3>.Sort<Int3>(cells);
			WorkBuffer<int> flags = faceIndex.Flags;
			flags.Fill(ref num, usedSize);
			WorkBuffer<int> active = faceIndex.Active;
			WorkBuffer<int> next = faceIndex.Next;
			WorkBuffer<int> neighbor = faceIndex.Neighbor;
			WorkBuffer<bool> constraint = faceIndex.Constraint;
			WorkBuffer<Int3> boundary = faceIndex.Boundary;
			neighbor.Fill(ref num, usedSize * 3);
			constraint.Fill(ref flag, usedSize * 3);
			int[] data2 = flags.Data;
			int[] data3 = neighbor.Data;
			bool[] data4 = constraint.Data;
			for (int j = 0; j < usedSize; j++)
			{
				Int3 int2 = data[j];
				for (int k = 0; k < 3; k++)
				{
					int num2 = 0;
					int num3 = 0;
					switch (k)
					{
					case 0:
						num2 = int2.x;
						num3 = int2.y;
						break;
					case 1:
						num2 = int2.y;
						num3 = int2.z;
						break;
					case 2:
						num2 = int2.z;
						num3 = int2.x;
						break;
					}
					int num4 = (data3[3 * j + k] = this.Locate(cells, num3, num2, triangles.Opposite(num3, num2)));
					bool flag2 = (data4[3 * j + k] = triangles.IsConstraint(num2, num3));
					if (num4 < 0)
					{
						if (flag2)
						{
							next.Push(ref j);
						}
						else
						{
							active.Push(ref j);
							data2[j] = 1;
						}
						if (this.Infinity)
						{
							Int3 int3 = new Int3(num3, num2, -1);
							boundary.Push(ref int3);
						}
					}
				}
			}
			return faceIndex;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004B10 File Offset: 0x00002D10
		private int Locate(WorkBuffer<Int3> cells, int a, int b, int c)
		{
			int num = a;
			int num2 = b;
			int num3 = c;
			if (b < c)
			{
				if (b < a)
				{
					num = b;
					num2 = c;
					num3 = a;
				}
			}
			else if (c < a)
			{
				num = c;
				num2 = a;
				num3 = b;
			}
			if (num < 0)
			{
				return -1;
			}
			return BinarySearch.EQ<Int3>(cells.Data, new Int3(num, num2, num3), 0, cells.UsedSize - 1);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004B64 File Offset: 0x00002D64
		private void FilterCells(WorkBuffer<Int3> cells, WorkBuffer<int> flags)
		{
			int num = 0;
			int usedSize = cells.UsedSize;
			Int3[] data = cells.Data;
			int[] data2 = flags.Data;
			for (int i = 0; i < usedSize; i++)
			{
				if (data2[i] == this.Target)
				{
					data[num++] = data[i];
				}
			}
			cells.RemoveLast(usedSize - num);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004BC0 File Offset: 0x00002DC0
		private void FillTriangles(WorkBuffer<Int3> from, List<int> to)
		{
			int usedSize = from.UsedSize;
			if (to.Capacity < usedSize * 3)
			{
				to.Capacity = usedSize * 3;
			}
			Int3[] data = from.Data;
			for (int i = 0; i < usedSize; i++)
			{
				Int3 @int = data[i];
				to.Add(@int.x);
				to.Add(@int.y);
				to.Add(@int.z);
			}
		}

		// Token: 0x0400002E RID: 46
		public int Target;

		// Token: 0x0400002F RID: 47
		public bool Infinity;

		// Token: 0x04000030 RID: 48
		public WorkBufferPool WorkBufferPool;

		// Token: 0x0200001A RID: 26
		private class FaceIndex : IDisposable
		{
			// Token: 0x060000A5 RID: 165 RVA: 0x00006E48 File Offset: 0x00005048
			public FaceIndex(WorkBufferPool pool)
			{
				this.WorkBufferPool = pool;
				pool.Get<Int3>(ref this.Cells);
				pool.Get<int>(ref this.Neighbor);
				pool.Get<bool>(ref this.Constraint);
				pool.Get<int>(ref this.Flags);
				pool.Get<int>(ref this.Active);
				pool.Get<int>(ref this.Next);
				pool.Get<Int3>(ref this.Boundary);
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00006EB6 File Offset: 0x000050B6
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x00006EC0 File Offset: 0x000050C0
			private void Dispose(bool disposing)
			{
				if (this.Disposed)
				{
					return;
				}
				if (disposing)
				{
					this.WorkBufferPool.Release<Int3>(ref this.Boundary);
					this.WorkBufferPool.Release<int>(ref this.Next);
					this.WorkBufferPool.Release<int>(ref this.Active);
					this.WorkBufferPool.Release<int>(ref this.Flags);
					this.WorkBufferPool.Release<bool>(ref this.Constraint);
					this.WorkBufferPool.Release<int>(ref this.Neighbor);
					this.WorkBufferPool.Release<Int3>(ref this.Cells);
				}
				this.Disposed = true;
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00006F58 File Offset: 0x00005158
			public void Dump()
			{
				Debug.LogFormat("Cells:\n{0}\n", new object[] { this.Dump<Int3>(this.Cells) });
				Debug.LogFormat("Neighbor:\n{0}\n", new object[] { this.Dump<int>(this.Neighbor) });
				Debug.LogFormat("Flags:\n{0}\n", new object[] { this.Dump<int>(this.Flags) });
				Debug.LogFormat("Constraint:\n{0}\n", new object[] { this.Dump<bool>(this.Constraint) });
				Debug.LogFormat("Active:\n{0}\n", new object[] { this.Dump<int>(this.Active) });
				Debug.LogFormat("Next:\n{0}\n", new object[] { this.Dump<int>(this.Next) });
				Debug.LogFormat("Boundary:\n{0}\n", new object[] { this.Dump<Int3>(this.Boundary) });
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x0000703E File Offset: 0x0000523E
			private string Dump<T>(WorkBuffer<T> buf)
			{
				return buf.Data.Take<T>(buf.UsedSize).Aggregate("", (string _, T s) => _ + " " + s.ToString() + ",\n");
			}

			// Token: 0x04000053 RID: 83
			public WorkBuffer<Int3> Cells;

			// Token: 0x04000054 RID: 84
			public WorkBuffer<int> Neighbor;

			// Token: 0x04000055 RID: 85
			public WorkBuffer<bool> Constraint;

			// Token: 0x04000056 RID: 86
			public WorkBuffer<int> Flags;

			// Token: 0x04000057 RID: 87
			public WorkBuffer<int> Active;

			// Token: 0x04000058 RID: 88
			public WorkBuffer<int> Next;

			// Token: 0x04000059 RID: 89
			public WorkBuffer<Int3> Boundary;

			// Token: 0x0400005A RID: 90
			private WorkBufferPool WorkBufferPool;

			// Token: 0x0400005B RID: 91
			private bool Disposed;
		}
	}
}
