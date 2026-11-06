using System;

namespace UnityEngine.UI
{
	// Token: 0x02000006 RID: 6
	public interface ICanvasElement
	{
		// Token: 0x06000015 RID: 21
		void Rebuild(CanvasUpdate executing);

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22
		Transform transform { get; }

		// Token: 0x06000017 RID: 23
		void LayoutComplete();

		// Token: 0x06000018 RID: 24
		void GraphicUpdateComplete();

		// Token: 0x06000019 RID: 25
		bool IsDestroyed();
	}
}
