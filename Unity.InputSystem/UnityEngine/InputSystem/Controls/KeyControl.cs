using System;
using System.Globalization;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011C RID: 284
	public class KeyControl : ButtonControl
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0004EE37 File Offset: 0x0004D037
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x0004EE3F File Offset: 0x0004D03F
		public Key keyCode { get; set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x0004EE48 File Offset: 0x0004D048
		public int scanCode
		{
			get
			{
				base.RefreshConfigurationIfNeeded();
				return this.m_ScanCode;
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004EE58 File Offset: 0x0004D058
		protected override void RefreshConfiguration()
		{
			base.displayName = null;
			this.m_ScanCode = 0;
			QueryKeyNameCommand queryKeyNameCommand = QueryKeyNameCommand.Create(this.keyCode);
			if (base.device.ExecuteCommand<QueryKeyNameCommand>(ref queryKeyNameCommand) > 0L)
			{
				this.m_ScanCode = queryKeyNameCommand.scanOrKeyCode;
				string text = queryKeyNameCommand.ReadKeyName();
				if (string.IsNullOrEmpty(text))
				{
					base.displayName = text;
					return;
				}
				string text2 = text.ToLowerInvariant();
				if (string.IsNullOrEmpty(text2))
				{
					base.displayName = text;
					return;
				}
				TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
				base.displayName = textInfo.ToTitleCase(text2);
			}
		}

		// Token: 0x040006C8 RID: 1736
		private int m_ScanCode;
	}
}
