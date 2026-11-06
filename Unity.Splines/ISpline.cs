using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000012 RID: 18
	public interface ISpline : IReadOnlyList<BezierKnot>, IEnumerable<BezierKnot>, IEnumerable, IReadOnlyCollection<BezierKnot>
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006B RID: 107
		bool Closed { get; }

		// Token: 0x0600006C RID: 108
		float GetLength();

		// Token: 0x0600006D RID: 109
		BezierCurve GetCurve(int index);

		// Token: 0x0600006E RID: 110
		float GetCurveLength(int index);

		// Token: 0x0600006F RID: 111
		float3 GetCurveUpVector(int index, float t);

		// Token: 0x06000070 RID: 112
		float GetCurveInterpolation(int curveIndex, float curveDistance);
	}
}
