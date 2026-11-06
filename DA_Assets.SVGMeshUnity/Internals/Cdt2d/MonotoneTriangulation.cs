using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x02000010 RID: 16
	public class MonotoneTriangulation
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004C30 File Offset: 0x00002E30
		public void BuildTriangles(MeshData data)
		{
			int count = data.Vertices.Count;
			int count2 = data.Edges.Count;
			data.Triangles.Capacity = count * 3;
			WorkBuffer<MonotoneTriangulation.Event> workBuffer = this.WorkBufferPool.Get<MonotoneTriangulation.Event>();
			if (workBuffer.NewForClass == null)
			{
				workBuffer.NewForClass = () => new MonotoneTriangulation.Event();
			}
			for (int i = 0; i < count; i++)
			{
				MonotoneTriangulation.Event @event = workBuffer.Push();
				@event.A = data.Vertices[i];
				@event.B = Vector2.zero;
				@event.Type = MonotoneTriangulation.EventType.Point;
				@event.Index = i;
			}
			for (int j = 0; j < count2; j++)
			{
				Int2 @int = data.Edges[j];
				Vector3 vector = data.Vertices[@int.x];
				Vector3 vector2 = data.Vertices[@int.y];
				if (vector.x < vector2.x)
				{
					MonotoneTriangulation.Event event2 = workBuffer.Push();
					event2.A = vector;
					event2.B = vector2;
					event2.Type = MonotoneTriangulation.EventType.Start;
					event2.Index = j;
					MonotoneTriangulation.Event event3 = workBuffer.Push();
					event3.A = vector2;
					event3.B = vector;
					event3.Type = MonotoneTriangulation.EventType.End;
					event3.Index = j;
				}
				else if (vector.x > vector2.x)
				{
					MonotoneTriangulation.Event event4 = workBuffer.Push();
					event4.A = vector2;
					event4.B = vector;
					event4.Type = MonotoneTriangulation.EventType.Start;
					event4.Index = j;
					MonotoneTriangulation.Event event5 = workBuffer.Push();
					event5.A = vector;
					event5.B = vector2;
					event5.Type = MonotoneTriangulation.EventType.End;
					event5.Index = j;
				}
			}
			WorkBuffer<MonotoneTriangulation.Event>.Sort<MonotoneTriangulation.Event>(workBuffer);
			if (MonotoneTriangulation.Verbose)
			{
				this.DumpEvents(workBuffer);
			}
			float num = workBuffer.Data[0].A.x - 1f;
			WorkBuffer<MonotoneTriangulation.PartialHull> workBuffer2 = this.WorkBufferPool.Get<MonotoneTriangulation.PartialHull>();
			if (workBuffer2.NewForClass == null)
			{
				workBuffer2.NewForClass = () => new MonotoneTriangulation.PartialHull();
			}
			MonotoneTriangulation.PartialHull partialHull = workBuffer2.Push();
			partialHull.A = new Vector2(num, 1f);
			partialHull.B = new Vector2(num, 0f);
			partialHull.Index = -1;
			partialHull.LowerIds.Clear();
			partialHull.UpperIds.Clear();
			int usedSize = workBuffer.UsedSize;
			for (int k = 0; k < usedSize; k++)
			{
				MonotoneTriangulation.Event event6 = workBuffer.Data[k];
				if (MonotoneTriangulation.Verbose)
				{
					Debug.Log("");
					Debug.Log(k);
					this.DumpEvent(event6);
				}
				switch (event6.Type)
				{
				case MonotoneTriangulation.EventType.Point:
					this.AddPoint(data.Triangles, workBuffer2, data.Vertices, event6.A, event6.Index);
					break;
				case MonotoneTriangulation.EventType.End:
					this.MergeHulls(workBuffer2, event6);
					break;
				case MonotoneTriangulation.EventType.Start:
					this.SplitHulls(workBuffer2, event6);
					break;
				}
				if (MonotoneTriangulation.Verbose)
				{
					Debug.Log("");
				}
			}
			this.WorkBufferPool.Release<MonotoneTriangulation.PartialHull>(ref workBuffer2);
			this.WorkBufferPool.Release<MonotoneTriangulation.Event>(ref workBuffer);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004F8A File Offset: 0x0000318A
		private static int Sign(float n)
		{
			if (n < 0f)
			{
				return -1;
			}
			if (n > 0f)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004FA4 File Offset: 0x000031A4
		private void AddPoint(List<int> cells, WorkBuffer<MonotoneTriangulation.PartialHull> hulls, List<Vector3> points, Vector2 p, int idx)
		{
			int num = BinarySearch.LT<MonotoneTriangulation.PartialHull, Vector2>(hulls.Data, p, MonotoneTriangulation.TestPoint.Default, 0, hulls.UsedSize - 1);
			int num2 = BinarySearch.GT<MonotoneTriangulation.PartialHull, Vector2>(hulls.Data, p, MonotoneTriangulation.TestPoint.Default, 0, hulls.UsedSize - 1);
			for (int i = num; i < num2; i++)
			{
				MonotoneTriangulation.PartialHull partialHull = hulls.Data[i];
				WorkBuffer<int> lowerIds = partialHull.LowerIds;
				int num3 = lowerIds.UsedSize;
				int[] data = lowerIds.Data;
				while (num3 > 1 && Robust.Orientation(points[data[num3 - 2]], points[data[num3 - 1]], p) > 0f)
				{
					cells.Add(data[num3 - 1]);
					cells.Add(data[num3 - 2]);
					cells.Add(idx);
					num3--;
				}
				if (num3 < lowerIds.UsedSize)
				{
					lowerIds.RemoveLast(lowerIds.UsedSize - num3);
				}
				lowerIds.Push(ref idx);
				WorkBuffer<int> upperIds = partialHull.UpperIds;
				int num4 = upperIds.UsedSize;
				int[] data2 = upperIds.Data;
				while (num4 > 1 && Robust.Orientation(points[data2[num4 - 2]], points[data2[num4 - 1]], p) < 0f)
				{
					cells.Add(data2[num4 - 2]);
					cells.Add(data2[num4 - 1]);
					cells.Add(idx);
					num4--;
				}
				if (num4 < upperIds.UsedSize)
				{
					upperIds.RemoveLast(upperIds.UsedSize - num4);
				}
				upperIds.Push(ref idx);
			}
			if (MonotoneTriangulation.Verbose)
			{
				Debug.Log("Add");
				this.DumpHulls(hulls);
				hulls.Dump();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000515C File Offset: 0x0000335C
		private void SplitHulls(WorkBuffer<MonotoneTriangulation.PartialHull> hulls, MonotoneTriangulation.Event e)
		{
			int num = BinarySearch.LE<MonotoneTriangulation.PartialHull, MonotoneTriangulation.Event>(hulls.Data, e, MonotoneTriangulation.FindSplit.Default, 0, hulls.UsedSize - 1);
			MonotoneTriangulation.PartialHull partialHull = hulls.Data[num];
			WorkBuffer<int> upperIds = partialHull.UpperIds;
			int num2 = upperIds.Data[upperIds.UsedSize - 1];
			partialHull.UpperIds = new WorkBuffer<int>(8);
			partialHull.UpperIds.Push(ref num2);
			MonotoneTriangulation.PartialHull partialHull2 = hulls.Insert(num + 1);
			partialHull2.A = e.A;
			partialHull2.B = e.B;
			partialHull2.Index = e.Index;
			partialHull2.LowerIds.Clear();
			partialHull2.LowerIds.Push(ref num2);
			partialHull2.UpperIds = upperIds;
			if (MonotoneTriangulation.Verbose)
			{
				Debug.Log("Split: " + num.ToString());
				this.DumpHulls(hulls);
				hulls.Dump();
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005230 File Offset: 0x00003430
		private void MergeHulls(WorkBuffer<MonotoneTriangulation.PartialHull> hulls, MonotoneTriangulation.Event e)
		{
			Vector2 a = e.A;
			e.A = e.B;
			e.B = a;
			int num = BinarySearch.EQ<MonotoneTriangulation.PartialHull, MonotoneTriangulation.Event>(hulls.Data, e, MonotoneTriangulation.FindSplit.Default, 0, hulls.UsedSize - 1);
			MonotoneTriangulation.PartialHull partialHull = hulls.Data[num];
			hulls.Data[num - 1].UpperIds = partialHull.UpperIds;
			hulls.RemoveAt(num);
			if (MonotoneTriangulation.Verbose)
			{
				Debug.Log("Merge: " + num.ToString());
				this.DumpHulls(hulls);
				hulls.Dump();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000052C4 File Offset: 0x000034C4
		private void DumpEvent(MonotoneTriangulation.Event _)
		{
			Debug.Log(string.Format("{{ a: {0}, b: {1}, type: {2}, idx: {3} }}", new object[] { _.A, _.B, _.Type, _.Index }));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005320 File Offset: 0x00003520
		private void DumpEvents(WorkBuffer<MonotoneTriangulation.Event> events)
		{
			Debug.Log((from _ in events.Data.Take<MonotoneTriangulation.Event>(events.UsedSize)
				select string.Format("{{ a: {0}, b: {1}, type: {2}, idx: {3} }}", new object[] { _.A, _.B, _.Type, _.Index })).Aggregate("", (string _, string s) => _ + s + "\n"));
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005390 File Offset: 0x00003590
		private void DumpHulls(WorkBuffer<MonotoneTriangulation.PartialHull> hulls)
		{
			Debug.Log((from _ in hulls.Data.Take<MonotoneTriangulation.PartialHull>(hulls.UsedSize)
				select string.Format("{{ a: {0}, b: {1}, idx: {2}, lowerIds: [ {3} ], upperIds: [ {4} ] }}", new object[]
				{
					_.A,
					_.B,
					_.Index,
					this.ToString(_.LowerIds),
					this.ToString(_.UpperIds)
				})).Aggregate("", (string _, string s) => _ + s + "\n"));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000053F0 File Offset: 0x000035F0
		private string ToString(WorkBuffer<int> list)
		{
			return string.Join(", ", (from _ in list.Data.Take<int>(list.UsedSize)
				select _.ToString()).ToArray<string>());
		}

		// Token: 0x04000031 RID: 49
		private static readonly bool Verbose;

		// Token: 0x04000032 RID: 50
		public WorkBufferPool WorkBufferPool;

		// Token: 0x0200001B RID: 27
		private enum EventType
		{
			// Token: 0x0400005D RID: 93
			Point,
			// Token: 0x0400005E RID: 94
			End,
			// Token: 0x0400005F RID: 95
			Start
		}

		// Token: 0x0200001C RID: 28
		private class Event : IComparable<MonotoneTriangulation.Event>
		{
			// Token: 0x060000AA RID: 170 RVA: 0x0000707C File Offset: 0x0000527C
			public int CompareTo(MonotoneTriangulation.Event b)
			{
				int num = MonotoneTriangulation.Sign(this.A.x - b.A.x);
				if (num != 0)
				{
					return num;
				}
				num = MonotoneTriangulation.Sign(this.A.y - b.A.y);
				if (num != 0)
				{
					return num;
				}
				num = this.Type - b.Type;
				if (num != 0)
				{
					return num;
				}
				if (this.Type != MonotoneTriangulation.EventType.Point)
				{
					num = MonotoneTriangulation.Sign(Robust.Orientation(this.A, this.B, b.B));
					if (num != 0)
					{
						return num;
					}
				}
				return this.Index - b.Index;
			}

			// Token: 0x04000060 RID: 96
			public Vector2 A;

			// Token: 0x04000061 RID: 97
			public Vector2 B;

			// Token: 0x04000062 RID: 98
			public MonotoneTriangulation.EventType Type;

			// Token: 0x04000063 RID: 99
			public int Index;
		}

		// Token: 0x0200001D RID: 29
		private class PartialHull
		{
			// Token: 0x04000064 RID: 100
			public Vector2 A;

			// Token: 0x04000065 RID: 101
			public Vector2 B;

			// Token: 0x04000066 RID: 102
			public int Index;

			// Token: 0x04000067 RID: 103
			public WorkBuffer<int> LowerIds = new WorkBuffer<int>(8);

			// Token: 0x04000068 RID: 104
			public WorkBuffer<int> UpperIds = new WorkBuffer<int>(8);
		}

		// Token: 0x0200001E RID: 30
		private class TestPoint : BinarySearch.IComparer<MonotoneTriangulation.PartialHull, Vector2>
		{
			// Token: 0x060000AD RID: 173 RVA: 0x00007141 File Offset: 0x00005341
			public int Compare(MonotoneTriangulation.PartialHull hull, Vector2 p)
			{
				return MonotoneTriangulation.Sign(Robust.Orientation(hull.A, hull.B, p));
			}

			// Token: 0x04000069 RID: 105
			public static readonly MonotoneTriangulation.TestPoint Default = new MonotoneTriangulation.TestPoint();
		}

		// Token: 0x0200001F RID: 31
		private class FindSplit : BinarySearch.IComparer<MonotoneTriangulation.PartialHull, MonotoneTriangulation.Event>
		{
			// Token: 0x060000B0 RID: 176 RVA: 0x00007170 File Offset: 0x00005370
			public int Compare(MonotoneTriangulation.PartialHull hull, MonotoneTriangulation.Event edge)
			{
				int num;
				if (hull.A.x < edge.A.x)
				{
					num = MonotoneTriangulation.Sign(Robust.Orientation(hull.A, hull.B, edge.A));
				}
				else
				{
					num = MonotoneTriangulation.Sign(Robust.Orientation(edge.B, edge.A, hull.A));
				}
				if (num != 0)
				{
					return num;
				}
				if (edge.B.x < hull.B.x)
				{
					num = MonotoneTriangulation.Sign(Robust.Orientation(hull.A, hull.B, edge.B));
				}
				else
				{
					num = MonotoneTriangulation.Sign(Robust.Orientation(edge.B, edge.A, hull.B));
				}
				if (num != 0)
				{
					return num;
				}
				return hull.Index - edge.Index;
			}

			// Token: 0x0400006A RID: 106
			public static readonly MonotoneTriangulation.FindSplit Default = new MonotoneTriangulation.FindSplit();
		}
	}
}
