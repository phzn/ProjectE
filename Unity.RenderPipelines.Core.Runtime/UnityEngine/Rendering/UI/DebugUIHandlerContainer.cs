using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000122 RID: 290
	public class DebugUIHandlerContainer : MonoBehaviour
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x00028F18 File Offset: 0x00027118
		internal DebugUIHandlerWidget GetFirstItem()
		{
			if (this.contentHolder.childCount == 0)
			{
				return null;
			}
			List<DebugUIHandlerWidget> activeChildren = this.GetActiveChildren();
			if (activeChildren.Count == 0)
			{
				return null;
			}
			return activeChildren[0];
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00028F4C File Offset: 0x0002714C
		internal DebugUIHandlerWidget GetLastItem()
		{
			if (this.contentHolder.childCount == 0)
			{
				return null;
			}
			List<DebugUIHandlerWidget> activeChildren = this.GetActiveChildren();
			if (activeChildren.Count == 0)
			{
				return null;
			}
			return activeChildren[activeChildren.Count - 1];
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00028F88 File Offset: 0x00027188
		internal bool IsDirectChild(DebugUIHandlerWidget widget)
		{
			return this.contentHolder.childCount != 0 && this.GetActiveChildren().Count<DebugUIHandlerWidget>((DebugUIHandlerWidget x) => x == widget) > 0;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00028FCC File Offset: 0x000271CC
		private List<DebugUIHandlerWidget> GetActiveChildren()
		{
			List<DebugUIHandlerWidget> list = new List<DebugUIHandlerWidget>();
			foreach (object obj in this.contentHolder)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeInHierarchy)
				{
					DebugUIHandlerWidget component = transform.GetComponent<DebugUIHandlerWidget>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			return list;
		}

		// Token: 0x0400051A RID: 1306
		[SerializeField]
		public RectTransform contentHolder;
	}
}
