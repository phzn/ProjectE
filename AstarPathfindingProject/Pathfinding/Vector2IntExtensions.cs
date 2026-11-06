using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000063 RID: 99
	public static class Vector2IntExtensions
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x00011CFD File Offset: 0x0000FEFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ToInt2(this Vector2Int v)
		{
			return new int2(v.x, v.y);
		}
	}
}
