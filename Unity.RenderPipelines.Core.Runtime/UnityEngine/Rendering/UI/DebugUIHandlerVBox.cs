using System;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200013A RID: 314
	public class DebugUIHandlerVBox : DebugUIHandlerWidget
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x0002B060 File Offset: 0x00029260
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002B078 File Offset: 0x00029278
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			if (!fromNext && !this.m_Container.IsDirectChild(previous))
			{
				DebugUIHandlerWidget lastItem = this.m_Container.GetLastItem();
				DebugManager.instance.ChangeSelection(lastItem, false);
				return true;
			}
			return false;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002B0B4 File Offset: 0x000292B4
		public override DebugUIHandlerWidget Next()
		{
			if (this.m_Container == null)
			{
				return base.Next();
			}
			DebugUIHandlerWidget firstItem = this.m_Container.GetFirstItem();
			if (firstItem == null)
			{
				return base.Next();
			}
			return firstItem;
		}

		// Token: 0x04000572 RID: 1394
		private DebugUIHandlerContainer m_Container;
	}
}
