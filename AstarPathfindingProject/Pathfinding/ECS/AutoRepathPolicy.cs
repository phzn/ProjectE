using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x020002F0 RID: 752
	[Serializable]
	public struct AutoRepathPolicy : IComponentData, IQueryTypeParameter
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x00073104 File Offset: 0x00071304
		public static AutoRepathPolicy Default
		{
			get
			{
				return new AutoRepathPolicy
				{
					mode = AutoRepathPolicy.Mode.Dynamic,
					period = 2f,
					lastDestination = float.PositiveInfinity,
					lastRepathTime = float.NegativeInfinity
				};
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0007314C File Offset: 0x0007134C
		public AutoRepathPolicy(AutoRepathPolicy policy)
		{
			this.mode = policy.mode;
			this.period = ((policy.mode == AutoRepathPolicy.Mode.Dynamic) ? policy.maximumPeriod : policy.period);
			this.lastDestination = float.PositiveInfinity;
			this.lastRepathTime = float.NegativeInfinity;
			this.pathFailures = 0;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x000731A4 File Offset: 0x000713A4
		public bool ShouldRecalculatePath(float3 position, float radius, float3 destination, float time, bool isPathStale)
		{
			if (this.mode == AutoRepathPolicy.Mode.Never || !float.IsFinite(destination.x))
			{
				return false;
			}
			float num = time - this.lastRepathTime;
			float num2 = this.period;
			if (isPathStale)
			{
				if (this.pathFailures == 0)
				{
					return true;
				}
				num2 = this.period * math.min(1f, 0.125f * (float)(1 << (int)this.pathFailures));
			}
			if (this.mode == AutoRepathPolicy.Mode.EveryNSeconds)
			{
				return num >= num2;
			}
			float num3 = math.lengthsq(destination - this.lastDestination) / math.max(math.lengthsq(position - this.lastDestination), radius * radius) * 100f;
			if (float.IsNaN(num3))
			{
				num3 = 0f;
			}
			return num >= num2 * (1f - math.sqrt(num3));
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00073270 File Offset: 0x00071470
		public void OnPathCalculated(bool hadError)
		{
			if (hadError)
			{
				this.pathFailures = (byte)math.min(255, (int)(this.pathFailures + 1));
				return;
			}
			this.pathFailures = 0;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00073296 File Offset: 0x00071496
		public void Reset()
		{
			this.lastDestination = float.PositiveInfinity;
			this.lastRepathTime = float.NegativeInfinity;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000732B3 File Offset: 0x000714B3
		public void OnScheduledPathRecalculation(float3 destination, float time)
		{
			this.lastRepathTime = time;
			this.lastDestination = destination;
			this.lastRepathTime -= (global::UnityEngine.Random.value - 0.5f) * 0.3f * this.period;
		}

		// Token: 0x04000DBA RID: 3514
		public const float Sensitivity = 10f;

		// Token: 0x04000DBB RID: 3515
		public AutoRepathPolicy.Mode mode;

		// Token: 0x04000DBC RID: 3516
		private byte pathFailures;

		// Token: 0x04000DBD RID: 3517
		public float period;

		// Token: 0x04000DBE RID: 3518
		private float3 lastDestination;

		// Token: 0x04000DBF RID: 3519
		private float lastRepathTime;
	}
}
