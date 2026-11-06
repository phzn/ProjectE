using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Transforms
{
	// Token: 0x0200001A RID: 26
	public struct PostTransformMatrix : IComponentData, IQueryTypeParameter
	{
		// Token: 0x0400004E RID: 78
		public float4x4 Value;
	}
}
