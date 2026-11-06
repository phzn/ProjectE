using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000A6 RID: 166
	public class GraphUpdateProcessor
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001AB5C File Offset: 0x00018D5C
		public bool IsAnyGraphUpdateQueued
		{
			get
			{
				return this.graphUpdateQueue.Count > 0;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001AB6C File Offset: 0x00018D6C
		public bool IsAnyGraphUpdateInProgress
		{
			get
			{
				return this.anyGraphUpdateInProgress;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001AB74 File Offset: 0x00018D74
		public GraphUpdateProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		public AstarWorkItem GetWorkItem()
		{
			return new AstarWorkItem(new Action<IWorkItemContext>(this.QueueGraphUpdatesInternal), new Func<IWorkItemContext, bool, bool>(this.ProcessGraphUpdates));
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001ABC3 File Offset: 0x00018DC3
		public void AddToQueue(GraphUpdateObject ob)
		{
			this.graphUpdateQueue.Enqueue(ob);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001ABD1 File Offset: 0x00018DD1
		public void DiscardQueued()
		{
			while (this.graphUpdateQueue.Count > 0)
			{
				this.graphUpdateQueue.Dequeue().internalStage = -3;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001ABF8 File Offset: 0x00018DF8
		private void QueueGraphUpdatesInternal(IWorkItemContext context)
		{
			while (this.graphUpdateQueue.Count > 0)
			{
				GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
				this.pendingGraphUpdates.Add(graphUpdateObject);
				context.VisualizeOriginalGraphUpdateBounds(graphUpdateObject.bounds);
				if (graphUpdateObject.internalStage != -2)
				{
					Debug.LogError("Expected remaining graph update to be pending");
				}
			}
			foreach (object obj in this.astar.data.GetUpdateableGraphs())
			{
				IUpdatableGraph updatableGraph = (IUpdatableGraph)obj;
				NavGraph navGraph = updatableGraph as NavGraph;
				List<GraphUpdateObject> list = ListPool<GraphUpdateObject>.Claim();
				for (int i = 0; i < this.pendingGraphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject2 = this.pendingGraphUpdates[i];
					if (graphUpdateObject2.graphMask.Contains(navGraph))
					{
						list.Add(graphUpdateObject2);
					}
				}
				if (list.Count > 0)
				{
					IGraphUpdatePromise graphUpdatePromise = updatableGraph.ScheduleGraphUpdates(list);
					if (graphUpdatePromise != null)
					{
						IEnumerator<JobHandle> enumerator2 = graphUpdatePromise.Prepare();
						this.pendingPromises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, enumerator2));
					}
					else
					{
						ListPool<GraphUpdateObject>.Release(ref list);
					}
				}
				else
				{
					ListPool<GraphUpdateObject>.Release(ref list);
				}
			}
			context.PreUpdate();
			this.anyGraphUpdateInProgress = true;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001AD48 File Offset: 0x00018F48
		private bool ProcessGraphUpdates(IWorkItemContext context, bool force)
		{
			if (this.pendingPromises.Count > 0)
			{
				try
				{
					if (GraphUpdateProcessor.ProcessGraphUpdatePromises(this.pendingPromises, context, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) != -1)
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(new Exception("Error while updating graphs", ex));
					return false;
				}
				this.pendingPromises.Clear();
			}
			this.anyGraphUpdateInProgress = false;
			for (int i = 0; i < this.pendingGraphUpdates.Count; i++)
			{
				this.pendingGraphUpdates[i].internalStage = 0;
			}
			this.pendingGraphUpdates.Clear();
			return true;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001ADFC File Offset: 0x00018FFC
		public static int ProcessGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, IGraphUpdateContext context, TimeSlice timeSlice)
		{
			int num = GraphUpdateProcessor.PrepareGraphUpdatePromises(promises, timeSlice);
			if (num == -1)
			{
				GraphUpdateProcessor.ApplyGraphUpdatePromises(promises, context);
			}
			return num;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001AE10 File Offset: 0x00019010
		public static int PrepareGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, TimeSlice timeSlice)
		{
			TimeSlice timeSlice2 = default(TimeSlice);
			bool flag = !timeSlice.isInfinite;
			int num;
			for (;;)
			{
				num = -1;
				bool flag2 = false;
				for (int i = 0; i < promises.Count; i++)
				{
					ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>> valueTuple = promises[i];
					IGraphUpdatePromise item = valueTuple.Item1;
					IEnumerator<JobHandle> item2 = valueTuple.Item2;
					if (item2 != null)
					{
						if (flag)
						{
							JobHandle jobHandle = item2.Current;
							if (jobHandle.IsCompleted)
							{
								flag2 = true;
								jobHandle = item2.Current;
								jobHandle.Complete();
							}
							else
							{
								if (num == -1)
								{
									num = i;
									goto IL_00B3;
								}
								goto IL_00B3;
							}
						}
						else
						{
							JobHandle jobHandle = item2.Current;
							jobHandle.Complete();
						}
						try
						{
							if (item2.MoveNext())
							{
								if (num == -1)
								{
									num = i;
								}
							}
							else
							{
								promises[i] = new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(item, null);
							}
						}
						catch
						{
							promises[i] = new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(null, null);
							throw;
						}
					}
					IL_00B3:;
				}
				if (num == -1)
				{
					return -1;
				}
				if (flag)
				{
					if (timeSlice.expired)
					{
						break;
					}
					if (flag2)
					{
						timeSlice2 = TimeSlice.MillisFromNow(0.1f);
					}
					else
					{
						if (timeSlice2.expired)
						{
							return num;
						}
						if (!flag2)
						{
							Thread.Yield();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001AF38 File Offset: 0x00019138
		public static void ApplyGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, IGraphUpdateContext context)
		{
			for (int i = 0; i < promises.Count; i++)
			{
				IGraphUpdatePromise item = promises[i].Item1;
				if (item != null)
				{
					try
					{
						item.Apply(context);
					}
					finally
					{
					}
				}
			}
		}

		// Token: 0x0400037D RID: 893
		private readonly AstarPath astar;

		// Token: 0x0400037E RID: 894
		private bool anyGraphUpdateInProgress;

		// Token: 0x0400037F RID: 895
		private readonly Queue<GraphUpdateObject> graphUpdateQueue = new Queue<GraphUpdateObject>();

		// Token: 0x04000380 RID: 896
		private readonly List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> pendingPromises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();

		// Token: 0x04000381 RID: 897
		private readonly List<GraphUpdateObject> pendingGraphUpdates = new List<GraphUpdateObject>();

		// Token: 0x04000382 RID: 898
		private static readonly ProfilerMarker MarkerSleep = new ProfilerMarker(ProfilerCategory.Loading, "Sleep");

		// Token: 0x04000383 RID: 899
		private static readonly ProfilerMarker MarkerCalculate = new ProfilerMarker("Calculating Graph Update");

		// Token: 0x04000384 RID: 900
		private static readonly ProfilerMarker MarkerApply = new ProfilerMarker("Applying Graph Update");
	}
}
