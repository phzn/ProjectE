using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Sync;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

namespace Pathfinding
{
	// Token: 0x020000C0 RID: 192
	public class PathProcessor
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000668 RID: 1640 RVA: 0x0001DFE4 File Offset: 0x0001C1E4
		// (remove) Token: 0x06000669 RID: 1641 RVA: 0x0001E01C File Offset: 0x0001C21C
		public event Action<Path> OnPathPreSearch;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600066A RID: 1642 RVA: 0x0001E054 File Offset: 0x0001C254
		// (remove) Token: 0x0600066B RID: 1643 RVA: 0x0001E08C File Offset: 0x0001C28C
		public event Action<PathHandler, Path> OnPathPostSearch;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600066C RID: 1644 RVA: 0x0001E0C4 File Offset: 0x0001C2C4
		// (remove) Token: 0x0600066D RID: 1645 RVA: 0x0001E0FC File Offset: 0x0001C2FC
		public event Action OnQueueUnblocked;

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001E131 File Offset: 0x0001C331
		public int NumThreads
		{
			get
			{
				return this.pathHandlers.Length;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001E13B File Offset: 0x0001C33B
		public bool IsUsingMultithreading
		{
			get
			{
				return this.multithreaded;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001E144 File Offset: 0x0001C344
		internal PathProcessor(AstarPath astar, PathReturnQueue returnQueue, int processors, bool multithreaded)
		{
			this.astar = astar;
			this.returnQueue = returnQueue;
			this.queue = new BlockableChannel<Path>();
			this.threads = null;
			this.threadCoroutine = null;
			this.pathHandlers = new PathHandler[0];
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001E198 File Offset: 0x0001C398
		public void SetThreadCount(int processors, bool multithreaded)
		{
			if (this.threads != null || this.threadCoroutine != null || this.pathHandlers.Length != 0)
			{
				throw new Exception("Call StopThreads before setting the thread count");
			}
			if (processors < 1)
			{
				throw new ArgumentOutOfRangeException("processors");
			}
			if (!multithreaded && processors != 1)
			{
				throw new Exception("Only a single non-multithreaded processor is allowed");
			}
			this.pathHandlers = new PathHandler[processors];
			this.multithreaded = multithreaded;
			for (int i = 0; i < processors; i++)
			{
				this.pathHandlers[i] = new PathHandler(this.astar.nodeStorage, i, processors);
			}
			this.astar.nodeStorage.SetThreadCount(processors);
			this.StartThreads();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001E23C File Offset: 0x0001C43C
		private void StartThreads()
		{
			if (this.threads != null || this.threadCoroutine != null)
			{
				throw new Exception("Call StopThreads before starting threads");
			}
			this.queue.Reopen();
			this.astar.nodeStorage.SetThreadCount(this.pathHandlers.Length);
			if (this.multithreaded)
			{
				this.threads = new Thread[this.pathHandlers.Length];
				for (int i = 0; i < this.pathHandlers.Length; i++)
				{
					PathHandler pathHandler = this.pathHandlers[i];
					BlockableChannel<Path>.Receiver receiver = this.queue.AddReceiver();
					this.threads[i] = new Thread(delegate
					{
						this.CalculatePathsThreaded(pathHandler, receiver);
					});
					this.threads[i].Name = "Pathfinding Thread " + i.ToString();
					this.threads[i].IsBackground = true;
					this.threads[i].Start();
				}
				return;
			}
			this.coroutineReceiver = this.queue.AddReceiver();
			this.threadCoroutine = this.CalculatePaths(this.pathHandlers[0]);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001E364 File Offset: 0x0001C564
		private int Lock(bool block)
		{
			this.queue.isBlocked = true;
			if (block)
			{
				while (!this.queue.allReceiversBlocked)
				{
					if (this.IsUsingMultithreading)
					{
						Thread.Sleep(1);
					}
					else
					{
						this.TickNonMultithreaded();
					}
				}
			}
			this.nextLockID++;
			this.locks.Add(this.nextLockID);
			return this.nextLockID;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001E3CC File Offset: 0x0001C5CC
		private void Unlock(int id)
		{
			if (!this.locks.Remove(id))
			{
				throw new ArgumentException("This lock has already been released");
			}
			if (this.locks.Count == 0)
			{
				if (this.OnQueueUnblocked != null)
				{
					this.OnQueueUnblocked();
				}
				this.queue.isBlocked = false;
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001E41E File Offset: 0x0001C61E
		public PathProcessor.GraphUpdateLock PausePathfinding(bool block)
		{
			return new PathProcessor.GraphUpdateLock(this, block);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001E428 File Offset: 0x0001C628
		public void TickNonMultithreaded()
		{
			if (this.threadCoroutine == null)
			{
				throw new InvalidOperationException("Cannot tick non-multithreaded pathfinding when no coroutine has been started");
			}
			try
			{
				if (!this.threadCoroutine.MoveNext())
				{
					this.threadCoroutine = null;
					this.coroutineReceiver.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.Close();
				this.threadCoroutine = null;
				this.coroutineReceiver.Close();
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001E4A8 File Offset: 0x0001C6A8
		public void StopThreads()
		{
			this.queue.Close();
			if (this.threads != null)
			{
				for (int i = 0; i < this.threads.Length; i++)
				{
					if (!this.threads[i].Join(200))
					{
						Debug.LogError("Could not terminate pathfinding thread[" + i.ToString() + "] in 200ms, trying Thread.Abort");
						this.threads[i].Abort();
					}
				}
				this.threads = null;
			}
			if (this.threadCoroutine != null)
			{
				while (this.queue.numReceivers > 0)
				{
					this.TickNonMultithreaded();
				}
			}
			for (int j = 0; j < this.pathHandlers.Length; j++)
			{
				this.pathHandlers[j].Dispose();
			}
			this.pathHandlers = new PathHandler[0];
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001E568 File Offset: 0x0001C768
		public void Dispose()
		{
			this.StopThreads();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E570 File Offset: 0x0001C770
		private void CalculatePathsThreaded(PathHandler pathHandler, BlockableChannel<Path>.Receiver receiver)
		{
			try
			{
				long num = 100000L;
				long num2 = DateTime.UtcNow.Ticks + num;
				Path path;
				while (receiver.Receive(out path) != BlockableChannel<Path>.PopState.Closed)
				{
					IPathInternals pathInternals = path;
					pathInternals.PrepareBase(pathHandler);
					pathInternals.AdvanceState(PathState.Processing);
					if (this.OnPathPreSearch != null)
					{
						this.OnPathPreSearch(path);
					}
					long ticks = DateTime.UtcNow.Ticks;
					Path.SearchContext searchContext = new Path.SearchContext
					{
						pathHandler = pathHandler,
						path = path,
						pathID = path.pathID
					};
					pathInternals.Prepare(ref searchContext);
					pathHandler.heap.tieBreaking = (searchContext.heuristicObjective.hasHeuristic ? BinaryHeap.TieBreaking.HScore : BinaryHeap.TieBreaking.InsertionOrder);
					if (path.CompleteState == PathCompleteState.NotCalculated)
					{
						this.astar.debugPathData = pathHandler;
						this.astar.debugPathID = searchContext.pathID;
						while (path.CompleteState == PathCompleteState.NotCalculated)
						{
							pathInternals.CalculateStep(ref searchContext, num2);
							num2 = DateTime.UtcNow.Ticks + num;
							if (this.queue.isClosed)
							{
								path.FailWithError("AstarPath object destroyed");
							}
						}
						path.duration = (float)(DateTime.UtcNow.Ticks - ticks) * 0.0001f;
					}
					pathInternals.Cleanup(ref searchContext);
					pathHandler.heap.Clear(pathHandler.pathNodes);
					if (path.immediateCallback != null)
					{
						path.immediateCallback(path);
					}
					if (this.OnPathPostSearch != null)
					{
						this.OnPathPostSearch(pathHandler, path);
					}
					this.returnQueue.Enqueue(path);
					pathInternals.AdvanceState(PathState.ReturnQueue);
				}
				if (this.astar.logPathResults == PathLog.Heavy)
				{
					Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID.ToString());
				}
				receiver.Close();
				return;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
				{
					if (this.astar.logPathResults == PathLog.Heavy)
					{
						Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID.ToString());
					}
					receiver.Close();
					return;
				}
				Debug.LogException(ex);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.Close();
			}
			finally
			{
				Profiler.EndThreadProfiling();
			}
			Debug.LogError("Error : This part should never be reached.");
			receiver.Close();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001E7DC File Offset: 0x0001C9DC
		private IEnumerator CalculatePaths(PathHandler pathHandler)
		{
			long maxTicks = (long)(this.astar.maxFrameTime * 10000f);
			long targetTick = DateTime.UtcNow.Ticks + maxTicks;
			for (;;)
			{
				Path p = null;
				bool blockedBefore = false;
				while (p == null)
				{
					switch (this.coroutineReceiver.ReceiveNoBlock(blockedBefore, out p))
					{
					case BlockableChannel<Path>.PopState.Wait:
						blockedBefore = true;
						yield return null;
						break;
					case BlockableChannel<Path>.PopState.Closed:
						goto IL_00BD;
					}
				}
				IPathInternals ip = p;
				maxTicks = (long)(this.astar.maxFrameTime * 10000f);
				ip.PrepareBase(pathHandler);
				ip.AdvanceState(PathState.Processing);
				Action<Path> onPathPreSearch = this.OnPathPreSearch;
				if (onPathPreSearch != null)
				{
					onPathPreSearch(p);
				}
				long num = DateTime.UtcNow.Ticks;
				long totalTicks = 0L;
				Path.SearchContext searchContext = new Path.SearchContext
				{
					pathHandler = pathHandler,
					path = p,
					pathID = p.pathID
				};
				ip.Prepare(ref searchContext);
				pathHandler.heap.tieBreaking = (searchContext.heuristicObjective.hasHeuristic ? BinaryHeap.TieBreaking.HScore : BinaryHeap.TieBreaking.InsertionOrder);
				if (p.CompleteState == PathCompleteState.NotCalculated)
				{
					this.astar.debugPathData = searchContext.pathHandler;
					this.astar.debugPathID = searchContext.pathID;
					while (p.CompleteState == PathCompleteState.NotCalculated)
					{
						ip.CalculateStep(ref searchContext, targetTick);
						if (p.CompleteState != PathCompleteState.NotCalculated)
						{
							break;
						}
						totalTicks += DateTime.UtcNow.Ticks - num;
						yield return null;
						num = DateTime.UtcNow.Ticks;
						if (this.queue.isClosed)
						{
							p.FailWithError("AstarPath object destroyed");
						}
						targetTick = DateTime.UtcNow.Ticks + maxTicks;
					}
					totalTicks += DateTime.UtcNow.Ticks - num;
					p.duration = (float)totalTicks * 0.0001f;
				}
				ip.Cleanup(ref searchContext);
				pathHandler.heap.Clear(pathHandler.pathNodes);
				OnPathDelegate immediateCallback = p.immediateCallback;
				if (immediateCallback != null)
				{
					immediateCallback(p);
				}
				Action<PathHandler, Path> onPathPostSearch = this.OnPathPostSearch;
				if (onPathPostSearch != null)
				{
					onPathPostSearch(pathHandler, p);
				}
				this.returnQueue.Enqueue(p);
				ip.AdvanceState(PathState.ReturnQueue);
				if (DateTime.UtcNow.Ticks > targetTick)
				{
					yield return null;
					targetTick = DateTime.UtcNow.Ticks + maxTicks;
				}
				p = null;
				ip = null;
				searchContext = default(Path.SearchContext);
			}
			IL_00BD:
			yield break;
			yield break;
		}

		// Token: 0x04000419 RID: 1049
		internal BlockableChannel<Path> queue;

		// Token: 0x0400041A RID: 1050
		private readonly AstarPath astar;

		// Token: 0x0400041B RID: 1051
		private readonly PathReturnQueue returnQueue;

		// Token: 0x0400041C RID: 1052
		private PathHandler[] pathHandlers;

		// Token: 0x0400041D RID: 1053
		private Thread[] threads;

		// Token: 0x0400041E RID: 1054
		private bool multithreaded;

		// Token: 0x0400041F RID: 1055
		private IEnumerator threadCoroutine;

		// Token: 0x04000420 RID: 1056
		private BlockableChannel<Path>.Receiver coroutineReceiver;

		// Token: 0x04000421 RID: 1057
		private readonly List<int> locks = new List<int>();

		// Token: 0x04000422 RID: 1058
		private int nextLockID;

		// Token: 0x04000423 RID: 1059
		private static readonly ProfilerMarker MarkerCalculatePath = new ProfilerMarker("Calculating Path");

		// Token: 0x04000424 RID: 1060
		private static readonly ProfilerMarker MarkerPreparePath = new ProfilerMarker("Prepare Path");

		// Token: 0x020000C1 RID: 193
		public struct GraphUpdateLock : IDisposable
		{
			// Token: 0x0600067C RID: 1660 RVA: 0x0001E812 File Offset: 0x0001CA12
			public GraphUpdateLock(PathProcessor pathProcessor, bool block)
			{
				this.pathProcessor = pathProcessor;
				this.id = pathProcessor.Lock(block);
			}

			// Token: 0x17000144 RID: 324
			// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001E828 File Offset: 0x0001CA28
			public bool Held
			{
				get
				{
					return this.pathProcessor != null && this.pathProcessor.locks.Contains(this.id);
				}
			}

			// Token: 0x0600067E RID: 1662 RVA: 0x0001E84A File Offset: 0x0001CA4A
			public void Release()
			{
				this.pathProcessor.Unlock(this.id);
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x0001E85D File Offset: 0x0001CA5D
			void IDisposable.Dispose()
			{
				this.Release();
			}

			// Token: 0x04000425 RID: 1061
			private PathProcessor pathProcessor;

			// Token: 0x04000426 RID: 1062
			private int id;
		}
	}
}
