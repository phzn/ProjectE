using System;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000007 RID: 7
	public enum TriangulatorErrorType : byte
	{
		// Token: 0x0400000C RID: 12
		Ok,
		// Token: 0x0400000D RID: 13
		PositionsLengthLessThan3,
		// Token: 0x0400000E RID: 14
		PositionsMustBeFinite,
		// Token: 0x0400000F RID: 15
		ConstraintsLengthNotDivisibleBy2,
		// Token: 0x04000010 RID: 16
		DuplicatePosition,
		// Token: 0x04000011 RID: 17
		DuplicateConstraint,
		// Token: 0x04000012 RID: 18
		ConstraintOutOfBounds,
		// Token: 0x04000013 RID: 19
		ConstraintSelfLoop,
		// Token: 0x04000014 RID: 20
		ConstraintIntersection,
		// Token: 0x04000015 RID: 21
		DegenerateInput,
		// Token: 0x04000016 RID: 22
		SloanMaxItersExceeded,
		// Token: 0x04000017 RID: 23
		IntegersDoNotSupportMeshRefinement,
		// Token: 0x04000018 RID: 24
		ConstraintArrayLengthMismatch,
		// Token: 0x04000019 RID: 25
		HoleMustBeFinite,
		// Token: 0x0400001A RID: 26
		RedudantHolesArray,
		// Token: 0x0400001B RID: 27
		ConstraintEdgesMissingForAutoHolesAndBoundary,
		// Token: 0x0400001C RID: 28
		ConstraintEdgesMissingForRestoreBoundary,
		// Token: 0x0400001D RID: 29
		RefinementNotSupportedForCoordinateType,
		// Token: 0x0400001E RID: 30
		SloanMaxItersMustBePositive,
		// Token: 0x0400001F RID: 31
		RefinementThresholdAreaMustBePositive,
		// Token: 0x04000020 RID: 32
		RefinementThresholdAngleOutOfRange
	}
}
