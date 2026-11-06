using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Core;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000263 RID: 611
	public static class RateUtils
	{
		// Token: 0x04000648 RID: 1608
		internal const float MinFixedDeltaTime = 0.0001f;

		// Token: 0x04000649 RID: 1609
		internal const float MaxFixedDeltaTime = 10f;

		// Token: 0x02000264 RID: 612
		public class FixedRateSimpleManager : IRateManager
		{
			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06001099 RID: 4249 RVA: 0x00046CFA File Offset: 0x00044EFA
			// (set) Token: 0x0600109A RID: 4250 RVA: 0x00046D02 File Offset: 0x00044F02
			public float Timestep
			{
				get
				{
					return this.m_FixedTimestep;
				}
				set
				{
					this.m_FixedTimestep = math.clamp(value, 0.0001f, 10f);
				}
			}

			// Token: 0x0600109B RID: 4251 RVA: 0x00046D1A File Offset: 0x00044F1A
			public FixedRateSimpleManager(float fixedDeltaTime)
			{
				this.Timestep = fixedDeltaTime;
			}

			// Token: 0x0600109C RID: 4252 RVA: 0x00046D34 File Offset: 0x00044F34
			public bool ShouldGroupUpdate(ComponentSystemGroup group)
			{
				if (this.m_DidPushTime)
				{
					group.World.PopTime();
					this.m_DidPushTime = false;
					group.World.RestoreGroupAllocator(this.m_OldGroupAllocators);
					return false;
				}
				group.World.PushTime(new TimeData(this.m_LastFixedUpdateTime, this.m_FixedTimestep));
				this.m_LastFixedUpdateTime += (double)this.m_FixedTimestep;
				this.m_DidPushTime = true;
				this.m_OldGroupAllocators = group.World.CurrentGroupAllocators;
				group.World.SetGroupAllocator(group.RateGroupAllocators);
				return true;
			}

			// Token: 0x0400064A RID: 1610
			private float m_FixedTimestep;

			// Token: 0x0400064B RID: 1611
			private double m_LastFixedUpdateTime;

			// Token: 0x0400064C RID: 1612
			private bool m_DidPushTime;

			// Token: 0x0400064D RID: 1613
			private unsafe DoubleRewindableAllocators* m_OldGroupAllocators = null;
		}

		// Token: 0x02000265 RID: 613
		public class FixedRateCatchUpManager : IRateManager
		{
			// Token: 0x17000114 RID: 276
			// (get) Token: 0x0600109D RID: 4253 RVA: 0x00046DC8 File Offset: 0x00044FC8
			// (set) Token: 0x0600109E RID: 4254 RVA: 0x00046DD0 File Offset: 0x00044FD0
			public float Timestep
			{
				get
				{
					return this.m_FixedTimestep;
				}
				set
				{
					this.m_FixedTimestep = math.clamp(value, 0.0001f, 10f);
				}
			}

			// Token: 0x0600109F RID: 4255 RVA: 0x00046DE8 File Offset: 0x00044FE8
			public FixedRateCatchUpManager(float fixedDeltaTime)
			{
				this.Timestep = fixedDeltaTime;
			}

			// Token: 0x060010A0 RID: 4256 RVA: 0x00046E00 File Offset: 0x00045000
			public bool ShouldGroupUpdate(ComponentSystemGroup group)
			{
				float num = math.max(group.World.MaximumDeltaTime, this.m_FixedTimestep);
				if (this.m_DidPushTime)
				{
					group.World.PopTime();
				}
				else
				{
					this.m_MaxFinalElapsedTime = this.m_LastFixedUpdateTime + (double)num;
				}
				double num2 = math.min(this.m_MaxFinalElapsedTime, group.World.Time.ElapsedTime);
				if (this.m_FixedUpdateCount != 0L)
				{
					if (num2 - this.m_LastFixedUpdateTime < (double)this.m_FixedTimestep)
					{
						if (this.m_DidPushTime)
						{
							group.World.RestoreGroupAllocator(this.m_OldGroupAllocators);
						}
						this.m_DidPushTime = false;
						return false;
					}
					this.m_LastFixedUpdateTime += (double)this.m_FixedTimestep;
				}
				this.m_FixedUpdateCount += 1L;
				group.World.PushTime(new TimeData(this.m_LastFixedUpdateTime, this.m_FixedTimestep));
				if (!this.m_DidPushTime)
				{
					this.m_DidPushTime = true;
					this.m_OldGroupAllocators = group.World.CurrentGroupAllocators;
					group.World.SetGroupAllocator(group.RateGroupAllocators);
				}
				return true;
			}

			// Token: 0x0400064E RID: 1614
			private float m_FixedTimestep;

			// Token: 0x0400064F RID: 1615
			private double m_LastFixedUpdateTime;

			// Token: 0x04000650 RID: 1616
			private long m_FixedUpdateCount;

			// Token: 0x04000651 RID: 1617
			private bool m_DidPushTime;

			// Token: 0x04000652 RID: 1618
			private double m_MaxFinalElapsedTime;

			// Token: 0x04000653 RID: 1619
			private unsafe DoubleRewindableAllocators* m_OldGroupAllocators = null;
		}

		// Token: 0x02000266 RID: 614
		public class VariableRateManager : IRateManager
		{
			// Token: 0x060010A1 RID: 4257 RVA: 0x00046F14 File Offset: 0x00045114
			public VariableRateManager(uint updateRateInMS = 66U, bool pushToWorld = true)
			{
				if (updateRateInMS < 1U)
				{
					updateRateInMS = 1U;
				}
				this.m_UpdateRate = (long)(updateRateInMS * ((float)Stopwatch.Frequency / 1000f));
				this.m_ShouldPushToWorld = pushToWorld;
				this.m_CurrentTimestamp = Stopwatch.GetTimestamp();
				this.m_PreviousTimestamp = this.m_CurrentTimestamp;
				this.m_TickCount = this.m_UpdateRate;
			}

			// Token: 0x060010A2 RID: 4258 RVA: 0x00046F84 File Offset: 0x00045184
			public bool ShouldGroupUpdate(ComponentSystemGroup group)
			{
				this.m_CurrentTimestamp = Stopwatch.GetTimestamp();
				long num;
				if (this.m_CurrentTimestamp < this.m_PreviousTimestamp)
				{
					num = long.MaxValue - this.m_PreviousTimestamp + this.m_CurrentTimestamp;
				}
				else
				{
					num = this.m_CurrentTimestamp - this.m_PreviousTimestamp;
				}
				this.m_PreviousTimestamp = this.m_CurrentTimestamp;
				this.m_TickCount += num;
				if (this.m_ShouldPushToWorld && this.m_DidPushTime)
				{
					group.World.PopTime();
					this.m_DidPushTime = false;
					group.World.RestoreGroupAllocator(this.m_OldGroupAllocators);
				}
				if (this.m_TickCount < this.m_UpdateRate)
				{
					return false;
				}
				this.m_Timestep = (float)this.m_TickCount / this.m_TicksPerSecond;
				if (this.m_ShouldPushToWorld)
				{
					this.m_ElapsedTime += (double)this.m_Timestep;
					group.World.PushTime(new TimeData(this.m_ElapsedTime, this.m_Timestep));
					this.m_DidPushTime = true;
					this.m_OldGroupAllocators = group.World.CurrentGroupAllocators;
					group.World.SetGroupAllocator(group.RateGroupAllocators);
				}
				this.m_TickCount = 0L;
				return true;
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x060010A3 RID: 4259 RVA: 0x000470AD File Offset: 0x000452AD
			// (set) Token: 0x060010A4 RID: 4260 RVA: 0x000470B5 File Offset: 0x000452B5
			public float Timestep
			{
				get
				{
					return this.m_Timestep;
				}
				set
				{
					this.m_Timestep = value;
				}
			}

			// Token: 0x04000654 RID: 1620
			private const uint MinUpdateRateMS = 1U;

			// Token: 0x04000655 RID: 1621
			private readonly bool m_ShouldPushToWorld;

			// Token: 0x04000656 RID: 1622
			private readonly float m_TicksPerSecond = (float)Stopwatch.Frequency;

			// Token: 0x04000657 RID: 1623
			private readonly long m_UpdateRate;

			// Token: 0x04000658 RID: 1624
			private long m_CurrentTimestamp;

			// Token: 0x04000659 RID: 1625
			private double m_ElapsedTime;

			// Token: 0x0400065A RID: 1626
			private long m_PreviousTimestamp;

			// Token: 0x0400065B RID: 1627
			private bool m_DidPushTime;

			// Token: 0x0400065C RID: 1628
			private long m_TickCount;

			// Token: 0x0400065D RID: 1629
			private float m_Timestep;

			// Token: 0x0400065E RID: 1630
			private unsafe DoubleRewindableAllocators* m_OldGroupAllocators = null;
		}
	}
}
