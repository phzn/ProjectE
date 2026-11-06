using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x0200000E RID: 14
	public class DelaunayRefine
	{
		// Token: 0x06000068 RID: 104 RVA: 0x0000441C File Offset: 0x0000261C
		public void RefineTriangles(Triangles triangles)
		{
			WorkBuffer<Int2> workBuffer = this.WorkBufferPool.Get<Int2>();
			List<Vector3> vertices = triangles.Vertices;
			int count = vertices.Count;
			WorkBuffer<Int2>[] stars = triangles.Stars;
			for (int i = 0; i < count; i++)
			{
				WorkBuffer<Int2> workBuffer2 = stars[i];
				Int2[] data = workBuffer2.Data;
				int usedSize = workBuffer2.UsedSize;
				for (int j = 0; j < usedSize; j++)
				{
					Int2 @int = data[j];
					int y = @int.y;
					if (y >= i && !triangles.IsConstraint(i, y))
					{
						int x = @int.x;
						int num = -1;
						for (int k = 0; k < usedSize; k++)
						{
							if (data[k].x == y)
							{
								num = data[k].y;
								break;
							}
						}
						if (num >= 0 && Robust.InSphere(vertices[i], vertices[y], vertices[x], vertices[num]) < 0f)
						{
							Int2 int2 = new Int2(i, y);
							workBuffer.Push(ref int2);
						}
					}
				}
			}
			while (workBuffer.UsedSize > 0)
			{
				Int2 int3 = workBuffer.Pop();
				int x2 = int3.x;
				int y2 = int3.y;
				int num2 = -1;
				int num3 = -1;
				WorkBuffer<Int2> workBuffer3 = stars[x2];
				Int2[] data2 = workBuffer3.Data;
				int usedSize2 = workBuffer3.UsedSize;
				for (int l = 0; l < usedSize2; l++)
				{
					int x3 = data2[l].x;
					int y3 = data2[l].y;
					if (x3 == y2)
					{
						num3 = y3;
					}
					else if (y3 == y2)
					{
						num2 = x3;
					}
				}
				if (num2 >= 0 && num3 >= 0 && Robust.InSphere(vertices[x2], vertices[y2], vertices[num2], vertices[num3]) < 0f)
				{
					triangles.Flip(x2, y2);
					this.TestFlip(vertices, triangles, workBuffer, num2, x2, num3);
					this.TestFlip(vertices, triangles, workBuffer, x2, num3, num2);
					this.TestFlip(vertices, triangles, workBuffer, num3, y2, num2);
					this.TestFlip(vertices, triangles, workBuffer, y2, num2, num3);
				}
			}
			this.WorkBufferPool.Release<Int2>(ref workBuffer);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004678 File Offset: 0x00002878
		private void TestFlip(List<Vector3> points, Triangles triangles, WorkBuffer<Int2> stack, int a, int b, int x)
		{
			int num = triangles.Opposite(a, b);
			if (num < 0)
			{
				return;
			}
			if (b < a)
			{
				int num2 = a;
				a = b;
				b = num2;
				int num3 = x;
				x = num;
				num = num3;
			}
			if (triangles.IsConstraint(a, b))
			{
				return;
			}
			if (Robust.InSphere(points[a], points[b], points[x], points[num]) < 0f)
			{
				Int2 @int = new Int2(a, b);
				stack.Push(ref @int);
			}
		}

		// Token: 0x0400002D RID: 45
		public WorkBufferPool WorkBufferPool;
	}
}
