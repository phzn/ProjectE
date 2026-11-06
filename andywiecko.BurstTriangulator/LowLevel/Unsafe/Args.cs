using System;
using System.Runtime.InteropServices;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000017 RID: 23
	public readonly struct Args
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public Args(Preprocessor preprocessor, int sloanMaxIters, bool autoHolesAndBoundary, bool refineMesh, bool restoreBoundary, bool validateInput, bool verbose, float refinementThresholdAngle, float refinementThresholdArea)
		{
			this.AutoHolesAndBoundary = autoHolesAndBoundary;
			this.Preprocessor = preprocessor;
			this.RefineMesh = refineMesh;
			this.RestoreBoundary = restoreBoundary;
			this.SloanMaxIters = sloanMaxIters;
			this.ValidateInput = validateInput;
			this.Verbose = verbose;
			this.RefinementThresholdAngle = refinementThresholdAngle;
			this.RefinementThresholdArea = refinementThresholdArea;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002C3C File Offset: 0x00000E3C
		public static Args Default(Preprocessor preprocessor = Preprocessor.None, int sloanMaxIters = 1000000, bool autoHolesAndBoundary = false, bool refineMesh = false, bool restoreBoundary = false, bool validateInput = true, bool verbose = true, float refinementThresholdAngle = 0.08726646f, float refinementThresholdArea = 1f)
		{
			return new Args(preprocessor, sloanMaxIters, autoHolesAndBoundary, refineMesh, restoreBoundary, validateInput, verbose, refinementThresholdAngle, refinementThresholdArea);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static implicit operator Args(TriangulationSettings settings)
		{
			bool autoHolesAndBoundary = settings.AutoHolesAndBoundary;
			Preprocessor preprocessor = settings.Preprocessor;
			bool refineMesh = settings.RefineMesh;
			bool restoreBoundary = settings.RestoreBoundary;
			return new Args(preprocessor, settings.SloanMaxIters, autoHolesAndBoundary, refineMesh, restoreBoundary, settings.ValidateInput, settings.Verbose, settings.RefinementThresholds.Angle, settings.RefinementThresholds.Area);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public Args With(Preprocessor? preprocessor = null, int? sloanMaxIters = null, bool? autoHolesAndBoundary = null, bool? refineMesh = null, bool? restoreBoundary = null, bool? validateInput = null, bool? verbose = null, float? refinementThresholdAngle = null, float? refinementThresholdArea = null)
		{
			return new Args(preprocessor ?? this.Preprocessor, sloanMaxIters ?? this.SloanMaxIters, autoHolesAndBoundary ?? this.AutoHolesAndBoundary, refineMesh ?? this.RefineMesh, restoreBoundary ?? this.RestoreBoundary, validateInput ?? this.ValidateInput, verbose ?? this.Verbose, refinementThresholdAngle ?? this.RefinementThresholdAngle, refinementThresholdArea ?? this.RefinementThresholdArea);
		}

		// Token: 0x04000053 RID: 83
		public readonly Preprocessor Preprocessor;

		// Token: 0x04000054 RID: 84
		public readonly int SloanMaxIters;

		// Token: 0x04000055 RID: 85
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool AutoHolesAndBoundary;

		// Token: 0x04000056 RID: 86
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool RefineMesh;

		// Token: 0x04000057 RID: 87
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool RestoreBoundary;

		// Token: 0x04000058 RID: 88
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool ValidateInput;

		// Token: 0x04000059 RID: 89
		[MarshalAs(UnmanagedType.U1)]
		public readonly bool Verbose;

		// Token: 0x0400005A RID: 90
		public readonly float RefinementThresholdAngle;

		// Token: 0x0400005B RID: 91
		public readonly float RefinementThresholdArea;
	}
}
