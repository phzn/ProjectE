using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class HamburgerItem
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006072 File Offset: 0x00004272
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000607A File Offset: 0x0000427A
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006083 File Offset: 0x00004283
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000608B File Offset: 0x0000428B
		public UpdateBool CheckBoxValue
		{
			get
			{
				return this.checkBoxValue;
			}
			set
			{
				this.checkBoxValue = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006094 File Offset: 0x00004294
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000609C File Offset: 0x0000429C
		public GUIContent GUIContent
		{
			get
			{
				return this.guiContent;
			}
			set
			{
				this.guiContent = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000060A5 File Offset: 0x000042A5
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000060AD File Offset: 0x000042AD
		public Action Body
		{
			get
			{
				return this.body;
			}
			set
			{
				this.body = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000060B6 File Offset: 0x000042B6
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000060BE File Offset: 0x000042BE
		public Action<string, bool> CheckBoxValueChanged
		{
			get
			{
				return this.checkBoxValueChanged;
			}
			set
			{
				this.checkBoxValueChanged = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000060C7 File Offset: 0x000042C7
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000060CF File Offset: 0x000042CF
		public Action OnButtonClick
		{
			get
			{
				return this.onButtonClick;
			}
			set
			{
				this.onButtonClick = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000060D8 File Offset: 0x000042D8
		// (set) Token: 0x06000124 RID: 292 RVA: 0x000060E0 File Offset: 0x000042E0
		public GUIContent ButtonGuiContent
		{
			get
			{
				return this.buttonGuiContent;
			}
			set
			{
				this.buttonGuiContent = value;
			}
		}

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private string id;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private UpdateBool checkBoxValue = new UpdateBool(false, false);

		// Token: 0x0400006C RID: 108
		[SerializeField]
		private GUIContent guiContent;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private Action body;

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private Action<string, bool> checkBoxValueChanged;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		private Action onButtonClick;

		// Token: 0x04000070 RID: 112
		[SerializeField]
		private GUIContent buttonGuiContent;
	}
}
