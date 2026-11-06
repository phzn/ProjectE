using System;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000129 RID: 297
	public class DebugUIHandlerHBox : DebugUIHandlerWidget
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x000299F3 File Offset: 0x00027BF3
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00029A08 File Offset: 0x00027C08
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

		// Token: 0x060008E4 RID: 2276 RVA: 0x00029A44 File Offset: 0x00027C44
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

		// Token: 0x0400052F RID: 1327
		private DebugUIHandlerContainer m_Container;
	}
}
