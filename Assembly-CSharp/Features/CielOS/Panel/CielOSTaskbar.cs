using System;
using System.Collections.Generic;
using Features.CielOS.WindowManager;
using UnityEngine;

namespace Features.CielOS.Panel
{
	// Token: 0x020013E2 RID: 5090
	public class CielOSTaskbar : MonoBehaviour
	{
		// Token: 0x06009D21 RID: 40225 RVA: 0x00408A8B File Offset: 0x00406C8B
		private void OnEnable()
		{
			if (this.windowManager == null)
			{
				return;
			}
			this.windowManager.WindowOpened += this.OnWindowOpened;
			this.windowManager.WindowClosed += this.OnWindowClosed;
		}

		// Token: 0x06009D22 RID: 40226 RVA: 0x00408ACA File Offset: 0x00406CCA
		private void OnDisable()
		{
			if (this.windowManager == null)
			{
				return;
			}
			this.windowManager.WindowOpened -= this.OnWindowOpened;
			this.windowManager.WindowClosed -= this.OnWindowClosed;
		}

		// Token: 0x06009D23 RID: 40227 RVA: 0x00408B0C File Offset: 0x00406D0C
		private void OnWindowOpened(CielOSWindow window)
		{
			if (window == null || this.entryParent == null || this.entryPrefab == null)
			{
				return;
			}
			if (this._entries.ContainsKey(window))
			{
				return;
			}
			CielOSTaskbarEntry cielOSTaskbarEntry = Object.Instantiate<CielOSTaskbarEntry>(this.entryPrefab, this.entryParent);
			cielOSTaskbarEntry.Bind(window);
			this._entries[window] = cielOSTaskbarEntry;
		}

		// Token: 0x06009D24 RID: 40228 RVA: 0x00408B74 File Offset: 0x00406D74
		private void OnWindowClosed(CielOSWindow window)
		{
			if (window == null)
			{
				return;
			}
			CielOSTaskbarEntry cielOSTaskbarEntry;
			if (this._entries.TryGetValue(window, out cielOSTaskbarEntry))
			{
				if (cielOSTaskbarEntry != null)
				{
					Object.Destroy(cielOSTaskbarEntry.gameObject);
				}
				this._entries.Remove(window);
			}
		}

		// Token: 0x0400A1A4 RID: 41380
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x0400A1A5 RID: 41381
		[Tooltip("Parent (with a HorizontalLayoutGroup) for taskbar entries.")]
		[SerializeField]
		private RectTransform entryParent;

		// Token: 0x0400A1A6 RID: 41382
		[SerializeField]
		private CielOSTaskbarEntry entryPrefab;

		// Token: 0x0400A1A7 RID: 41383
		private readonly Dictionary<CielOSWindow, CielOSTaskbarEntry> _entries = new Dictionary<CielOSWindow, CielOSTaskbarEntry>();
	}
}
