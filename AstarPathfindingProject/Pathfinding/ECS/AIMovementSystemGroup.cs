using System;
using System.Diagnostics;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

namespace Pathfinding.ECS
{
	// Token: 0x02000359 RID: 857
	[UpdateAfter(typeof(TransformSystemGroup))]
	public class AIMovementSystemGroup : ComponentSystemGroup
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x0007A20D File Offset: 0x0007840D
		// (set) Token: 0x0600156F RID: 5487 RVA: 0x0007A21F File Offset: 0x0007841F
		public double CustomTimeScale
		{
			get
			{
				return (base.RateManager as AIMovementSystemGroup.TimeScaledRateManager).CustomTimeScale;
			}
			set
			{
				(base.RateManager as AIMovementSystemGroup.TimeScaledRateManager).CustomTimeScale = value;
			}
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0007A234 File Offset: 0x00078434
		[Preserve]
		protected override void OnUpdate()
		{
			if (AstarPath.active != null && AstarPath.active.IsNavigationSuspended)
			{
				return;
			}
			RWLock.ReadLockAsync readLockAsync = ((AstarPath.active != null) ? AstarPath.active.LockGraphDataForReading() : default(RWLock.ReadLockAsync));
			NativeList<SystemHandle> unmanagedSystems = base.GetUnmanagedSystems(Allocator.Temp);
			for (int i = 0; i < unmanagedSystems.Length; i++)
			{
				ref SystemState ptr = ref base.World.Unmanaged.ResolveSystemStateRef(unmanagedSystems[i]);
				ptr.Dependency = JobHandle.CombineDependencies(ptr.Dependency, readLockAsync.dependency);
			}
			long timestamp = Stopwatch.GetTimestamp();
			base.OnUpdate();
			long timestamp2 = Stopwatch.GetTimestamp();
			AIMovementSystemGroup.TimeScaledRateManager timeScaledRateManager = base.RateManager as AIMovementSystemGroup.TimeScaledRateManager;
			double num = (double)(timestamp2 - timestamp) / ((double)timeScaledRateManager.NumUpdatesThisFrame * (double)Stopwatch.Frequency);
			timeScaledRateManager.OnSimulationStepsFinished((float)((double)(timestamp2 - timestamp) / (double)Stopwatch.Frequency));
			JobHandle jobHandle = default(JobHandle);
			for (int j = 0; j < unmanagedSystems.Length; j++)
			{
				ref SystemState ptr2 = ref base.World.Unmanaged.ResolveSystemStateRef(unmanagedSystems[j]);
				jobHandle = JobHandle.CombineDependencies(jobHandle, ptr2.Dependency);
			}
			unmanagedSystems.Dispose();
			readLockAsync.UnlockAfter(jobHandle);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0007A374 File Offset: 0x00078574
		[Preserve]
		protected override void OnDestroy()
		{
			base.OnDestroy();
			(base.RateManager as AIMovementSystemGroup.TimeScaledRateManager).Dispose();
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0007A38C File Offset: 0x0007858C
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			base.RateManager = new AIMovementSystemGroup.TimeScaledRateManager();
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0007A39F File Offset: 0x0007859F
		[Preserve]
		public AIMovementSystemGroup()
		{
		}

		// Token: 0x0200035A RID: 858
		public class TimeScaledRateManager : IRateManager, IDisposable
		{
			// Token: 0x17000320 RID: 800
			// (get) Token: 0x06001574 RID: 5492 RVA: 0x0007A3A7 File Offset: 0x000785A7
			public static bool CheapSimulationOnly
			{
				get
				{
					if (!AIMovementSystemGroup.TimeScaledRateManager.inGroup)
					{
						throw new InvalidOperationException("Cannot call this method outside of a simulation group using TimeScaledRateManager");
					}
					return AIMovementSystemGroup.TimeScaledRateManager.cheapSimulationOnly;
				}
			}

			// Token: 0x17000321 RID: 801
			// (get) Token: 0x06001575 RID: 5493 RVA: 0x0007A3C0 File Offset: 0x000785C0
			public static float CheapStepDeltaTime
			{
				get
				{
					if (!AIMovementSystemGroup.TimeScaledRateManager.inGroup)
					{
						throw new InvalidOperationException("Cannot call this method outside of a simulation group using TimeScaledRateManager");
					}
					return AIMovementSystemGroup.TimeScaledRateManager.cheapTimeData.DeltaTime;
				}
			}

			// Token: 0x17000322 RID: 802
			// (get) Token: 0x06001576 RID: 5494 RVA: 0x0007A3DE File Offset: 0x000785DE
			public static bool IsLastSubstep
			{
				get
				{
					if (!AIMovementSystemGroup.TimeScaledRateManager.inGroup)
					{
						throw new InvalidOperationException("Cannot call this method outside of a simulation group using TimeScaledRateManager");
					}
					return AIMovementSystemGroup.TimeScaledRateManager.isLastSubstep;
				}
			}

			// Token: 0x17000323 RID: 803
			// (get) Token: 0x06001577 RID: 5495 RVA: 0x0007A3F7 File Offset: 0x000785F7
			public static bool IsFirstSubstep
			{
				get
				{
					if (!AIMovementSystemGroup.TimeScaledRateManager.inGroup)
					{
						throw new InvalidOperationException("Cannot call this method outside of a simulation group using TimeScaledRateManager");
					}
					return AIMovementSystemGroup.TimeScaledRateManager.isFirstSubstep;
				}
			}

			// Token: 0x17000324 RID: 804
			// (get) Token: 0x06001578 RID: 5496 RVA: 0x0007A410 File Offset: 0x00078610
			public int NumUpdatesThisFrame
			{
				get
				{
					return this.numUpdatesThisFrame;
				}
			}

			// Token: 0x06001579 RID: 5497 RVA: 0x0007A418 File Offset: 0x00078618
			public TimeScaledRateManager()
			{
				this.cheapTimeDataQueue = new NativeList<TimeData>(Allocator.Persistent);
				this.timeDataQueue = new NativeList<TimeData>(Allocator.Persistent);
			}

			// Token: 0x0600157A RID: 5498 RVA: 0x0007A467 File Offset: 0x00078667
			public void Dispose()
			{
				this.cheapTimeDataQueue.Dispose();
				this.timeDataQueue.Dispose();
			}

			// Token: 0x0600157B RID: 5499 RVA: 0x0007A47F File Offset: 0x0007867F
			public void OnSimulationStepsFinished(float totalSimulationProcessingTime)
			{
				if (AIMovementSystemGroup.TimeScaledRateManager.cheapSimulationOnly)
				{
					return;
				}
				this.ownProcessingTimePerIteration = totalSimulationProcessingTime / (float)this.numUpdatesThisFrame;
			}

			// Token: 0x0600157C RID: 5500 RVA: 0x0007A498 File Offset: 0x00078698
			public bool ShouldGroupUpdate(ComponentSystemGroup group)
			{
				if (AIMovementSystemGroup.TimeScaledRateManager.inGroup)
				{
					group.World.PopTime();
					this.updateIndex++;
					if (this.updateIndex >= this.numUpdatesThisFrame)
					{
						AIMovementSystemGroup.TimeScaledRateManager.inGroup = false;
						return false;
					}
				}
				else
				{
					this.cheapTimeDataQueue.Clear();
					this.timeDataQueue.Clear();
					if (AIMovementSystemGroup.TimeScaledRateManager.inGroup)
					{
						throw new InvalidOperationException("Cannot nest simulation groups using TimeScaledRateManager");
					}
					double num = group.World.Time.ElapsedTime * this.CustomTimeScale;
					float num2 = (float)(num - this.lastFullSimulation);
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					this.numUpdatesThisFrame = Mathf.FloorToInt(Mathf.Pow(num2 / (this.maximumDt + this.ownProcessingTimePerIteration), 0.8f));
					AIMovementSystemGroup.TimeScaledRateManager.cheapSimulationOnly = this.numUpdatesThisFrame == 0;
					if (AIMovementSystemGroup.TimeScaledRateManager.cheapSimulationOnly)
					{
						TimeData timeData = new TimeData(this.lastFullSimulation, 0f);
						this.timeDataQueue.Add(in timeData);
						timeData = new TimeData(num, (float)(num - this.lastCheapSimulation));
						this.cheapTimeDataQueue.Add(in timeData);
						this.lastCheapSimulation = num;
					}
					else
					{
						this.stepDt = num2 / (float)this.numUpdatesThisFrame;
						for (int i = 0; i < this.numUpdatesThisFrame; i++)
						{
							double num3 = this.lastFullSimulation + (double)((float)(i + 1) * this.stepDt);
							TimeData timeData = new TimeData(num3, this.stepDt);
							this.timeDataQueue.Add(in timeData);
							timeData = new TimeData(num3, (float)(num3 - this.lastCheapSimulation));
							this.cheapTimeDataQueue.Add(in timeData);
							this.lastCheapSimulation = num3;
						}
						this.lastFullSimulation = num;
					}
					this.numUpdatesThisFrame = Mathf.Max(1, this.numUpdatesThisFrame);
					AIMovementSystemGroup.TimeScaledRateManager.inGroup = true;
					this.updateIndex = 0;
				}
				group.World.PushTime(this.timeDataQueue[this.updateIndex]);
				AIMovementSystemGroup.TimeScaledRateManager.cheapTimeData = this.cheapTimeDataQueue[this.updateIndex];
				AIMovementSystemGroup.TimeScaledRateManager.isLastSubstep = this.updateIndex + 1 >= this.numUpdatesThisFrame;
				AIMovementSystemGroup.TimeScaledRateManager.isFirstSubstep = this.updateIndex == 0;
				return true;
			}

			// Token: 0x17000325 RID: 805
			// (get) Token: 0x0600157D RID: 5501 RVA: 0x0007A6A9 File Offset: 0x000788A9
			// (set) Token: 0x0600157E RID: 5502 RVA: 0x0007A6B1 File Offset: 0x000788B1
			public float Timestep
			{
				get
				{
					return this.maximumDt;
				}
				set
				{
					this.maximumDt = value;
				}
			}

			// Token: 0x04000EEC RID: 3820
			private int numUpdatesThisFrame;

			// Token: 0x04000EED RID: 3821
			private int updateIndex;

			// Token: 0x04000EEE RID: 3822
			private float stepDt;

			// Token: 0x04000EEF RID: 3823
			private float maximumDt = 0.033333335f;

			// Token: 0x04000EF0 RID: 3824
			private float ownProcessingTimePerIteration;

			// Token: 0x04000EF1 RID: 3825
			private NativeList<TimeData> cheapTimeDataQueue;

			// Token: 0x04000EF2 RID: 3826
			private NativeList<TimeData> timeDataQueue;

			// Token: 0x04000EF3 RID: 3827
			private double lastFullSimulation;

			// Token: 0x04000EF4 RID: 3828
			private double lastCheapSimulation;

			// Token: 0x04000EF5 RID: 3829
			private static bool cheapSimulationOnly;

			// Token: 0x04000EF6 RID: 3830
			private static bool isLastSubstep;

			// Token: 0x04000EF7 RID: 3831
			private static bool isFirstSubstep;

			// Token: 0x04000EF8 RID: 3832
			private static bool inGroup;

			// Token: 0x04000EF9 RID: 3833
			private static TimeData cheapTimeData;

			// Token: 0x04000EFA RID: 3834
			public double CustomTimeScale = 1.0;
		}
	}
}
