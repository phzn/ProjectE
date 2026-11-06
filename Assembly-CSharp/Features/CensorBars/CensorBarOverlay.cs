using System;
using Features.MainMenu;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014B7 RID: 5303
	public class CensorBarOverlay : MonoBehaviour
	{
		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x0600A32D RID: 41773 RVA: 0x0043CA45 File Offset: 0x0043AC45
		// (set) Token: 0x0600A32E RID: 41774 RVA: 0x0043CA4C File Offset: 0x0043AC4C
		public static bool Enabled
		{
			get
			{
				return GameSettingsService.StreamerMode;
			}
			set
			{
				GameSettingsService.SetStreamerMode(value);
			}
		}

		// Token: 0x0600A32F RID: 41775 RVA: 0x0043CA54 File Offset: 0x0043AC54
		private void Awake()
		{
			this.BuildOverlay();
		}

		// Token: 0x0600A330 RID: 41776 RVA: 0x0043CA5C File Offset: 0x0043AC5C
		private void BuildOverlay()
		{
			Canvas canvas = base.GetComponent<Canvas>();
			if (canvas == null)
			{
				canvas = base.gameObject.AddComponent<Canvas>();
			}
			canvas.enabled = false;
		}
	}
}
