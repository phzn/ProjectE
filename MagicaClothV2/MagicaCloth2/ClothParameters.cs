using System;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000010 RID: 16
	public struct ClothParameters
	{
		// Token: 0x04000026 RID: 38
		public float gravity;

		// Token: 0x04000027 RID: 39
		public float3 worldGravityDirection;

		// Token: 0x04000028 RID: 40
		public float gravityFalloff;

		// Token: 0x04000029 RID: 41
		public float stablizationTimeAfterReset;

		// Token: 0x0400002A RID: 42
		public float blendWeight;

		// Token: 0x0400002B RID: 43
		public float4x4 dampingCurveData;

		// Token: 0x0400002C RID: 44
		public float4x4 radiusCurveData;

		// Token: 0x0400002D RID: 45
		public ClothNormalAxis normalAxis;

		// Token: 0x0400002E RID: 46
		public float rotationalInterpolation;

		// Token: 0x0400002F RID: 47
		public float rootRotation;

		// Token: 0x04000030 RID: 48
		public CullingSettings.CullingParams culling;

		// Token: 0x04000031 RID: 49
		public InertiaConstraint.InertiaConstraintParams inertiaConstraint;

		// Token: 0x04000032 RID: 50
		public TetherConstraint.TetherConstraintParams tetherConstraint;

		// Token: 0x04000033 RID: 51
		public DistanceConstraint.DistanceConstraintParams distanceConstraint;

		// Token: 0x04000034 RID: 52
		public TriangleBendingConstraint.TriangleBendingConstraintParams triangleBendingConstraint;

		// Token: 0x04000035 RID: 53
		public AngleConstraint.AngleConstraintParams angleConstraint;

		// Token: 0x04000036 RID: 54
		public MotionConstraint.MotionConstraintParams motionConstraint;

		// Token: 0x04000037 RID: 55
		public ColliderCollisionConstraint.ColliderCollisionConstraintParams colliderCollisionConstraint;

		// Token: 0x04000038 RID: 56
		public SelfCollisionConstraint.SelfCollisionConstraintParams selfCollisionConstraint;

		// Token: 0x04000039 RID: 57
		public WindParams wind;

		// Token: 0x0400003A RID: 58
		public SpringConstraint.SpringConstraintParams springConstraint;
	}
}
