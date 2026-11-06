using System;
using Unity.Profiling;

namespace Pathfinding.Drawing
{
	// Token: 0x0200000A RID: 10
	internal static class CommandBuilderSamplers
	{
		// Token: 0x04000015 RID: 21
		internal static readonly ProfilerMarker MarkerConvert = new ProfilerMarker("Convert");

		// Token: 0x04000016 RID: 22
		internal static readonly ProfilerMarker MarkerSetLayout = new ProfilerMarker("SetLayout");

		// Token: 0x04000017 RID: 23
		internal static readonly ProfilerMarker MarkerUpdateVertices = new ProfilerMarker("UpdateVertices");

		// Token: 0x04000018 RID: 24
		internal static readonly ProfilerMarker MarkerUpdateIndices = new ProfilerMarker("UpdateIndices");

		// Token: 0x04000019 RID: 25
		internal static readonly ProfilerMarker MarkerSubmesh = new ProfilerMarker("Submesh");

		// Token: 0x0400001A RID: 26
		internal static readonly ProfilerMarker MarkerUpdateBuffer = new ProfilerMarker("UpdateComputeBuffer");

		// Token: 0x0400001B RID: 27
		internal static readonly ProfilerMarker MarkerProcessCommands = new ProfilerMarker("Commands");

		// Token: 0x0400001C RID: 28
		internal static readonly ProfilerMarker MarkerCreateTriangles = new ProfilerMarker("CreateTriangles");
	}
}
