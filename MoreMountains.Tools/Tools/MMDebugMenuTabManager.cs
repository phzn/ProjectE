using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000046 RID: 70
	public class MMDebugMenuTabManager : MonoBehaviour
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00007414 File Offset: 0x00005614
		public virtual void Select(int selected)
		{
			foreach (MMDebugMenuTab mmdebugMenuTab in this.Tabs)
			{
				if (mmdebugMenuTab.Index != selected)
				{
					mmdebugMenuTab.Deselect();
				}
			}
			foreach (MMDebugMenuTabContents mmdebugMenuTabContents in this.TabsContents)
			{
				if (mmdebugMenuTabContents.Index == selected)
				{
					mmdebugMenuTabContents.gameObject.SetActive(true);
				}
				else
				{
					mmdebugMenuTabContents.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x040001D7 RID: 471
		public List<MMDebugMenuTab> Tabs;

		// Token: 0x040001D8 RID: 472
		public List<MMDebugMenuTabContents> TabsContents;
	}
}
