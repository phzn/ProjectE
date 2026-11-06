using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005D2 RID: 1490
	[vClassHeader("Master Window", true, "icon_v2", false, "", openClose = false)]
	public class vMasterWindow : vMonoBehaviour
	{
		// Token: 0x060022B1 RID: 8881 RVA: 0x000C74A1 File Offset: 0x000C56A1
		protected virtual void OnDisable()
		{
			this.GetParentWindow();
			this.parentWindow;
			this.CloseAllMasterWindows();
			this.onDisable.Invoke();
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000C74C6 File Offset: 0x000C56C6
		protected virtual void GetParentWindow()
		{
			if (!this.parentWindow)
			{
				this.parentWindow = base.transform.parent.GetComponentInParent<vMasterWindow>();
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000C74EC File Offset: 0x000C56EC
		protected virtual void OnEnable()
		{
			this.GetParentWindow();
			if (this.parentWindow)
			{
				this.parentWindow.SetCurrentWindow(base.gameObject);
			}
			if (this.windows.Count == 0 && this.mainWindow)
			{
				this.SetCurrentWindow(this.mainWindow);
			}
			this.onEnable.Invoke();
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000C7550 File Offset: 0x000C5750
		public virtual void RemoveWindow(GameObject window)
		{
			if (!this.windows.Contains(window) || window == this.mainWindow)
			{
				return;
			}
			if (!this.sequenceWindows || this.currentWindow == window)
			{
				this.windows.Remove(window);
			}
			this.currentWindow = null;
			if (this.sequenceWindows && this.windows.Count > 0)
			{
				this.currentWindow = this.windows[this.windows.Count - 1];
				if (!this.currentWindow.activeSelf)
				{
					this.currentWindow.SetActive(true);
				}
			}
			if (this.windows.Count == 0 && this.mainWindow)
			{
				this.SetCurrentWindow(this.mainWindow);
			}
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000C7618 File Offset: 0x000C5818
		public virtual void SetCurrentWindow(GameObject window)
		{
			if (this.currentWindow == window)
			{
				if (!this.currentWindow.activeSelf)
				{
					this.currentWindow.SetActive(true);
				}
				return;
			}
			if (!this.windows.Contains(window))
			{
				this.windows.Add(window);
			}
			if (!this.sequenceWindows && this.currentWindow)
			{
				this.windows.Remove(this.currentWindow);
				if (this.currentWindow.activeSelf)
				{
					this.currentWindow.SetActive(false);
				}
			}
			this.currentWindow = window;
			if (!this.currentWindow.activeSelf)
			{
				this.currentWindow.SetActive(true);
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000C76C8 File Offset: 0x000C58C8
		public virtual void CloseAllMasterWindows()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].SetActive(false);
			}
			this.windows.Clear();
			if (this.mainWindow)
			{
				this.mainWindow.SetActive(true);
			}
		}

		// Token: 0x040022F9 RID: 8953
		[vHelpBox("Window that always opens when this window is activated", vHelpBoxAttribute.MessageType.None)]
		public GameObject mainWindow;

		// Token: 0x040022FA RID: 8954
		public bool sequenceWindows;

		// Token: 0x040022FB RID: 8955
		[vReadOnly(true)]
		public vMasterWindow parentWindow;

		// Token: 0x040022FC RID: 8956
		[vReadOnly(true)]
		public GameObject currentWindow;

		// Token: 0x040022FD RID: 8957
		[vReadOnly(true)]
		public List<GameObject> windows;

		// Token: 0x040022FE RID: 8958
		public UnityEvent onEnable;

		// Token: 0x040022FF RID: 8959
		public UnityEvent onDisable;
	}
}
