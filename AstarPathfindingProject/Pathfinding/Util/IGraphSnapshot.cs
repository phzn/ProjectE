using System;

namespace Pathfinding.Util
{
	// Token: 0x02000290 RID: 656
	public interface IGraphSnapshot : IDisposable
	{
		// Token: 0x06001054 RID: 4180
		void Restore(IGraphUpdateContext ctx);
	}
}
