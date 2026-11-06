using System;
using UnityEngine;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BAB RID: 2987
	public static class TrafficLaneConvention
	{
		// Token: 0x060055AD RID: 21933 RVA: 0x002141D8 File Offset: 0x002123D8
		public static Vector3 DirVector(TrafficLaneConvention.Side s)
		{
			switch (s)
			{
			case TrafficLaneConvention.Side.North:
				return TrafficLaneConvention.North;
			case TrafficLaneConvention.Side.South:
				return TrafficLaneConvention.South;
			case TrafficLaneConvention.Side.East:
				return TrafficLaneConvention.East;
			case TrafficLaneConvention.Side.West:
				return TrafficLaneConvention.West;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x00214210 File Offset: 0x00212410
		public static Vector3 EdgePoint(TrafficLaneConvention.Side edge, Vector3 travelDir, float halfExtent)
		{
			Vector3 vector = TrafficLaneConvention.LeftOf(travelDir);
			return TrafficLaneConvention.DirVector(edge) * halfExtent + vector * (halfExtent - 1.0499992f);
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x00214244 File Offset: 0x00212444
		public static Vector3 LeftOf(Vector3 travelDir)
		{
			return Vector3.Cross(travelDir.normalized, Vector3.up).normalized;
		}

		// Token: 0x040055F6 RID: 22006
		public const float HalfCell = 30f;

		// Token: 0x040055F7 RID: 22007
		public const float CellSize = 60f;

		// Token: 0x040055F8 RID: 22008
		public const float LaneOffset = 28.95f;

		// Token: 0x040055F9 RID: 22009
		public const float LaneInset = 1.0499992f;

		// Token: 0x040055FA RID: 22010
		public const float LaneY = 0f;

		// Token: 0x040055FB RID: 22011
		public const float SpawnInset = 8f;

		// Token: 0x040055FC RID: 22012
		public const float SpeedResidential = 14f;

		// Token: 0x040055FD RID: 22013
		public const float SpeedArterial = 20f;

		// Token: 0x040055FE RID: 22014
		public const float SpeedStart = 9f;

		// Token: 0x040055FF RID: 22015
		public const int MaxVehiclesPerLane = 5;

		// Token: 0x04005600 RID: 22016
		public const int MaxVehiclesPerIntersectionLane = 1;

		// Token: 0x04005601 RID: 22017
		public const float WeightStraight = 4f;

		// Token: 0x04005602 RID: 22018
		public const float WeightRight = 2f;

		// Token: 0x04005603 RID: 22019
		public const float WeightLeft = 1f;

		// Token: 0x04005604 RID: 22020
		public const float TurnRadius = 28.95f;

		// Token: 0x04005605 RID: 22021
		public const float ArcHandleFactor = 0.5522848f;

		// Token: 0x04005606 RID: 22022
		public static readonly Vector3 North = new Vector3(0f, 0f, 1f);

		// Token: 0x04005607 RID: 22023
		public static readonly Vector3 South = new Vector3(0f, 0f, -1f);

		// Token: 0x04005608 RID: 22024
		public static readonly Vector3 East = new Vector3(1f, 0f, 0f);

		// Token: 0x04005609 RID: 22025
		public static readonly Vector3 West = new Vector3(-1f, 0f, 0f);

		// Token: 0x02000BAC RID: 2988
		public enum Side
		{
			// Token: 0x0400560B RID: 22027
			North,
			// Token: 0x0400560C RID: 22028
			South,
			// Token: 0x0400560D RID: 22029
			East,
			// Token: 0x0400560E RID: 22030
			West
		}
	}
}
