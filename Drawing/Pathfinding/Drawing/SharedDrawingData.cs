using System;
using Unity.Burst;

namespace Pathfinding.Drawing
{
	// Token: 0x02000029 RID: 41
	public static class SharedDrawingData
	{
		// Token: 0x04000083 RID: 131
		public static readonly SharedStatic<float> BurstTime = SharedStatic<float>.GetOrCreateUnsafe(4U, 4667476456522965744L, -7737948255972676495L);

		// Token: 0x0200002A RID: 42
		private class BurstTimeKey
		{
		}
	}
}
