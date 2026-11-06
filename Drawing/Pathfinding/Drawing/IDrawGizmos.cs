using System;

namespace Pathfinding.Drawing
{
	// Token: 0x02000046 RID: 70
	public interface IDrawGizmos
	{
		// Token: 0x06000257 RID: 599
		void DrawGizmos();

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000AE2B File Offset: 0x0000902B
		bool Exists
		{
			get
			{
				throw new NotImplementedException("This method should be overridden in the implementing class, unless it inherits from MonoBehaviour");
			}
		}
	}
}
