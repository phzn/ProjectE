using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200000E RID: 14
	public interface IExtrudeShape
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000037C4 File Offset: 0x000019C4
		void Setup(ISpline path, int segmentCount)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000037C6 File Offset: 0x000019C6
		void SetSegment(int index, float t, float3 position, float3 tangent, float3 up)
		{
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000059 RID: 89
		int SideCount { get; }

		// Token: 0x0600005A RID: 90
		float2 GetPosition(float t, int index);
	}
}
