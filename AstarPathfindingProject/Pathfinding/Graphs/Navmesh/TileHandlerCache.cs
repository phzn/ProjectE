using System;
using Pathfinding.Clipper2Lib;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C3 RID: 451
	internal static class TileHandlerCache
	{
		// Token: 0x0400089B RID: 2203
		internal static Clipper64[] cachedClippers = new Clipper64[JobsUtility.ThreadIndexCount];
	}
}
