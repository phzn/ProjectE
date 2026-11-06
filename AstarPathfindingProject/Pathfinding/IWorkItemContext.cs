using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000095 RID: 149
	public interface IWorkItemContext : IGraphUpdateContext
	{
		// Token: 0x060004F3 RID: 1267
		[Obsolete("You no longer need to call this method. Connectivity data is automatically kept up-to-date.")]
		void QueueFloodFill();

		// Token: 0x060004F4 RID: 1268
		void EnsureValidFloodFill();

		// Token: 0x060004F5 RID: 1269
		void PreUpdate();

		// Token: 0x060004F6 RID: 1270
		void SetGraphDirty(NavGraph graph);

		// Token: 0x060004F7 RID: 1271
		void VisualizeOriginalGraphUpdateBounds(Bounds bounds);
	}
}
