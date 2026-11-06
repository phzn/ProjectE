using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000029 RID: 41
	public struct DistanceMetric
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000B026 File Offset: 0x00009226
		public bool isProjectedDistance
		{
			get
			{
				return this.projectionAxis != Vector3.zero;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B038 File Offset: 0x00009238
		public static DistanceMetric ClosestAsSeenFromAboveSoft()
		{
			return new DistanceMetric
			{
				projectionAxis = Vector3.positiveInfinity,
				distanceScaleAlongProjectionDirection = 0.2f
			};
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B068 File Offset: 0x00009268
		public static DistanceMetric ClosestAsSeenFromAboveSoft(Vector3 up)
		{
			return new DistanceMetric
			{
				projectionAxis = up,
				distanceScaleAlongProjectionDirection = 0.2f
			};
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000B094 File Offset: 0x00009294
		public static DistanceMetric ClosestAsSeenFromAbove()
		{
			return new DistanceMetric
			{
				projectionAxis = Vector3.positiveInfinity,
				distanceScaleAlongProjectionDirection = 0f
			};
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000B0C4 File Offset: 0x000092C4
		public static DistanceMetric ClosestAsSeenFromAbove(Vector3 up)
		{
			return new DistanceMetric
			{
				projectionAxis = up,
				distanceScaleAlongProjectionDirection = 0f
			};
		}

		// Token: 0x04000150 RID: 336
		public Vector3 projectionAxis;

		// Token: 0x04000151 RID: 337
		public float distanceScaleAlongProjectionDirection;

		// Token: 0x04000152 RID: 338
		public static readonly DistanceMetric Euclidean = new DistanceMetric
		{
			projectionAxis = Vector3.zero,
			distanceScaleAlongProjectionDirection = 0f
		};
	}
}
