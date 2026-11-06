using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200029A RID: 666
	internal static class MathExtensions
	{
		// Token: 0x0600109D RID: 4253 RVA: 0x00064948 File Offset: 0x00062B48
		public static Bounds BoundsOfTransformedBounds(float4x4 matrix, Bounds bounds)
		{
			return new Bounds
			{
				center = math.transform(matrix, bounds.center),
				extents = math.mul(new float3x3(math.abs(matrix.c0.xyz), math.abs(matrix.c1.xyz), math.abs(matrix.c2.xyz)), bounds.extents)
			};
		}
	}
}
