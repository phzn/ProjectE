using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000128 RID: 296
	public class DebugUIHandlerGroup : DebugUIHandlerWidget
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x0002990C File Offset: 0x00027B0C
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Container>();
			this.m_Container = base.GetComponent<DebugUIHandlerContainer>();
			if (this.m_Field.hideDisplayName)
			{
				this.header.gameObject.SetActive(false);
				return;
			}
			this.nameLabel.text = this.m_Field.displayName;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00029970 File Offset: 0x00027B70
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

		// Token: 0x060008E0 RID: 2272 RVA: 0x000299AC File Offset: 0x00027BAC
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

		// Token: 0x0400052B RID: 1323
		public Text nameLabel;

		// Token: 0x0400052C RID: 1324
		public Transform header;

		// Token: 0x0400052D RID: 1325
		private DebugUI.Container m_Field;

		// Token: 0x0400052E RID: 1326
		private DebugUIHandlerContainer m_Container;
	}
}
