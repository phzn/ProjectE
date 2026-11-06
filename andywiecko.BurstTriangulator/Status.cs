using System;
using Unity.Collections;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000006 RID: 6
	public struct Status
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020C7 File Offset: 0x000002C7
		public bool IsError
		{
			get
			{
				return this.type > TriangulatorErrorType.Ok;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020D4 File Offset: 0x000002D4
		public static Status Ok
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.Ok
				};
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020F4 File Offset: 0x000002F4
		public static Status PositionsLengthLessThan3(int length)
		{
			return new Status
			{
				value1 = length,
				type = TriangulatorErrorType.PositionsLengthLessThan3
			};
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000211C File Offset: 0x0000031C
		public static Status PositionsMustBeFinite(int index)
		{
			return new Status
			{
				value1 = index,
				type = TriangulatorErrorType.PositionsMustBeFinite
			};
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002144 File Offset: 0x00000344
		public static Status ConstraintsLengthNotDivisibleBy2(int length)
		{
			return new Status
			{
				value1 = length,
				type = TriangulatorErrorType.ConstraintsLengthNotDivisibleBy2
			};
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000216C File Offset: 0x0000036C
		public static Status DuplicatePosition(int index)
		{
			return new Status
			{
				value1 = index,
				type = TriangulatorErrorType.DuplicatePosition
			};
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002194 File Offset: 0x00000394
		public static Status DuplicateConstraint(int index1, int index2)
		{
			return new Status
			{
				value1 = index1,
				value2 = index2,
				type = TriangulatorErrorType.DuplicateConstraint
			};
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021C4 File Offset: 0x000003C4
		public static Status ConstraintOutOfBounds(int index, int2 constraint, int positionLength)
		{
			return new Status
			{
				value1 = index,
				value2 = constraint.x,
				value3 = constraint.y,
				value4 = positionLength,
				type = TriangulatorErrorType.ConstraintOutOfBounds
			};
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000220C File Offset: 0x0000040C
		public static Status ConstraintSelfLoop(int index, int2 constraint)
		{
			return new Status
			{
				value1 = index,
				value2 = constraint.x,
				value3 = constraint.y,
				type = TriangulatorErrorType.ConstraintSelfLoop
			};
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000224C File Offset: 0x0000044C
		public static Status ConstraintIntersection(int index1, int index2)
		{
			return new Status
			{
				value1 = index1,
				value2 = index2,
				type = TriangulatorErrorType.ConstraintIntersection
			};
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000227C File Offset: 0x0000047C
		public static Status DegenerateInput
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.DegenerateInput
				};
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000229C File Offset: 0x0000049C
		public static Status SloanMaxItersExceeded
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.SloanMaxItersExceeded
				};
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022BC File Offset: 0x000004BC
		public static Status IntegersDoNotSupportMeshRefinement
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.IntegersDoNotSupportMeshRefinement
				};
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022DC File Offset: 0x000004DC
		public static Status ConstraintArrayLengthMismatch(int constraintLength, int constraintTypeLength)
		{
			return new Status
			{
				value1 = constraintLength,
				value2 = constraintTypeLength,
				type = TriangulatorErrorType.ConstraintArrayLengthMismatch
			};
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000230C File Offset: 0x0000050C
		public static Status HoleMustBeFinite(int index)
		{
			return new Status
			{
				value1 = index,
				type = TriangulatorErrorType.HoleMustBeFinite
			};
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002334 File Offset: 0x00000534
		public static Status RedudantHolesArray
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.RedudantHolesArray
				};
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002354 File Offset: 0x00000554
		public static Status ConstraintEdgesMissingForAutoHolesAndBoundary
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.ConstraintEdgesMissingForAutoHolesAndBoundary
				};
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002374 File Offset: 0x00000574
		public static Status ConstraintEdgesMissingForRestoreBoundary
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.ConstraintEdgesMissingForRestoreBoundary
				};
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002394 File Offset: 0x00000594
		public static Status RefinementNotSupportedForCoordinateType
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.RefinementNotSupportedForCoordinateType
				};
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023B4 File Offset: 0x000005B4
		public static Status SloanMaxItersMustBePositive(int sloanMaxIters)
		{
			return new Status
			{
				type = TriangulatorErrorType.SloanMaxItersMustBePositive,
				value1 = sloanMaxIters
			};
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000023DC File Offset: 0x000005DC
		public static Status RefinementThresholdAreaMustBePositive
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.RefinementThresholdAreaMustBePositive
				};
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023FC File Offset: 0x000005FC
		public static Status RefinementThresholdAngleOutOfRange
		{
			get
			{
				return new Status
				{
					type = TriangulatorErrorType.RefinementThresholdAngleOutOfRange
				};
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000241B File Offset: 0x0000061B
		internal FixedString64Bytes ToFixedString()
		{
			return "Triangulation error. Run in editor for more info.";
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002428 File Offset: 0x00000628
		public override string ToString()
		{
			return this.ToFixedString().ToString();
		}

		// Token: 0x04000006 RID: 6
		private int value1;

		// Token: 0x04000007 RID: 7
		private int value2;

		// Token: 0x04000008 RID: 8
		private int value3;

		// Token: 0x04000009 RID: 9
		private int value4;

		// Token: 0x0400000A RID: 10
		public TriangulatorErrorType type;
	}
}
