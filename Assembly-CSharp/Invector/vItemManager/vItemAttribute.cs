using System;

namespace Invector.vItemManager
{
	// Token: 0x02000561 RID: 1377
	[Serializable]
	public class vItemAttribute
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x000C13C4 File Offset: 0x000BF5C4
		public string displayFormat
		{
			get
			{
				return this.name.DisplayFormat();
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000C13D4 File Offset: 0x000BF5D4
		public string GetDisplayText(string format = null)
		{
			string text = (string.IsNullOrEmpty(format) ? this.displayFormat : format);
			if (string.IsNullOrEmpty(text))
			{
				text = this.name.ToString().InsertSpaceBeforeUpperCase().RemoveUnderline();
				text = text + " : " + this.value.ToString();
			}
			else
			{
				if (text.Contains("(NAME)"))
				{
					text = text.Replace("(NAME)", this.name.ToString().InsertSpaceBeforeUpperCase().RemoveUnderline());
				}
				if (text.Contains("(VALUE)"))
				{
					text = text.Replace("(VALUE)", this.value.ToString());
				}
			}
			return text;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000C1489 File Offset: 0x000BF689
		public vItemAttribute(vItemAttributes name, int value)
		{
			this.name = name;
			this.value = value;
		}

		// Token: 0x040021A1 RID: 8609
		public vItemAttributes name;

		// Token: 0x040021A2 RID: 8610
		public int value;

		// Token: 0x040021A3 RID: 8611
		public bool isOpen;

		// Token: 0x040021A4 RID: 8612
		public bool isBool;
	}
}
