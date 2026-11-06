using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200017E RID: 382
	public class DropdownMenu
	{
		// Token: 0x06000C34 RID: 3124 RVA: 0x00030F44 File Offset: 0x0002F144
		public List<DropdownMenuItem> MenuItems()
		{
			return this.m_MenuItems;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00030F5C File Offset: 0x0002F15C
		public void AppendAction(string actionName, Action<DropdownMenuAction> action, Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback, object userData = null)
		{
			DropdownMenuAction dropdownMenuAction = new DropdownMenuAction(actionName, action, actionStatusCallback, userData);
			this.m_MenuItems.Add(dropdownMenuAction);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00030F84 File Offset: 0x0002F184
		public void AppendAction(string actionName, Action<DropdownMenuAction> action, DropdownMenuAction.Status status = DropdownMenuAction.Status.Normal)
		{
			bool flag = status == DropdownMenuAction.Status.Normal;
			if (flag)
			{
				this.AppendAction(actionName, action, new Func<DropdownMenuAction, DropdownMenuAction.Status>(DropdownMenuAction.AlwaysEnabled), null);
			}
			else
			{
				bool flag2 = status == DropdownMenuAction.Status.Disabled;
				if (flag2)
				{
					this.AppendAction(actionName, action, new Func<DropdownMenuAction, DropdownMenuAction.Status>(DropdownMenuAction.AlwaysDisabled), null);
				}
				else
				{
					this.AppendAction(actionName, action, (DropdownMenuAction e) => status, null);
				}
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00031008 File Offset: 0x0002F208
		public void InsertAction(int atIndex, string actionName, Action<DropdownMenuAction> action, Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback, object userData = null)
		{
			DropdownMenuAction dropdownMenuAction = new DropdownMenuAction(actionName, action, actionStatusCallback, userData);
			this.m_MenuItems.Insert(atIndex, dropdownMenuAction);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00031030 File Offset: 0x0002F230
		public void InsertAction(int atIndex, string actionName, Action<DropdownMenuAction> action, DropdownMenuAction.Status status = DropdownMenuAction.Status.Normal)
		{
			bool flag = status == DropdownMenuAction.Status.Normal;
			if (flag)
			{
				this.InsertAction(atIndex, actionName, action, new Func<DropdownMenuAction, DropdownMenuAction.Status>(DropdownMenuAction.AlwaysEnabled), null);
			}
			else
			{
				bool flag2 = status == DropdownMenuAction.Status.Disabled;
				if (flag2)
				{
					this.InsertAction(atIndex, actionName, action, new Func<DropdownMenuAction, DropdownMenuAction.Status>(DropdownMenuAction.AlwaysDisabled), null);
				}
				else
				{
					this.InsertAction(atIndex, actionName, action, (DropdownMenuAction e) => status, null);
				}
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000310B8 File Offset: 0x0002F2B8
		public void AppendSeparator(string subMenuPath = null)
		{
			bool flag = this.m_MenuItems.Count > 0 && !(this.m_MenuItems[this.m_MenuItems.Count - 1] is DropdownMenuSeparator);
			if (flag)
			{
				DropdownMenuSeparator dropdownMenuSeparator = new DropdownMenuSeparator(subMenuPath ?? string.Empty);
				this.m_MenuItems.Add(dropdownMenuSeparator);
			}
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00031120 File Offset: 0x0002F320
		public void InsertSeparator(string subMenuPath, int atIndex)
		{
			bool flag = atIndex > 0 && atIndex <= this.m_MenuItems.Count && !(this.m_MenuItems[atIndex - 1] is DropdownMenuSeparator);
			if (flag)
			{
				DropdownMenuSeparator dropdownMenuSeparator = new DropdownMenuSeparator(subMenuPath ?? string.Empty);
				this.m_MenuItems.Insert(atIndex, dropdownMenuSeparator);
			}
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00031180 File Offset: 0x0002F380
		public void RemoveItemAt(int index)
		{
			this.m_MenuItems.RemoveAt(index);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00031190 File Offset: 0x0002F390
		public void ClearItems()
		{
			this.m_MenuItems.Clear();
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000311A0 File Offset: 0x0002F3A0
		public void PrepareForDisplay(EventBase e)
		{
			this.m_DropdownMenuEventInfo = ((e != null) ? new DropdownMenuEventInfo(e) : null);
			bool flag = this.m_MenuItems.Count == 0;
			if (!flag)
			{
				foreach (DropdownMenuItem dropdownMenuItem in this.m_MenuItems)
				{
					DropdownMenuAction dropdownMenuAction = dropdownMenuItem as DropdownMenuAction;
					bool flag2 = dropdownMenuAction != null;
					if (flag2)
					{
						dropdownMenuAction.UpdateActionStatus(this.m_DropdownMenuEventInfo);
					}
				}
				bool flag3 = this.m_MenuItems[this.m_MenuItems.Count - 1] is DropdownMenuSeparator;
				if (flag3)
				{
					this.m_MenuItems.RemoveAt(this.m_MenuItems.Count - 1);
				}
			}
		}

		// Token: 0x040005D3 RID: 1491
		private List<DropdownMenuItem> m_MenuItems = new List<DropdownMenuItem>();

		// Token: 0x040005D4 RID: 1492
		private DropdownMenuEventInfo m_DropdownMenuEventInfo;
	}
}
