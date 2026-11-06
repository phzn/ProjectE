using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000DE RID: 222
	public class Label : TextElement
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x0001D7AA File Offset: 0x0001B9AA
		public Label()
			: this(string.Empty)
		{
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001D7B9 File Offset: 0x0001B9B9
		public Label(string text)
		{
			base.AddToClassList(Label.ussClassName);
			this.text = text;
		}

		// Token: 0x04000352 RID: 850
		public new static readonly string ussClassName = "unity-label";

		// Token: 0x020000DF RID: 223
		public new class UxmlFactory : UxmlFactory<Label, Label.UxmlTraits>
		{
		}

		// Token: 0x020000E0 RID: 224
		public new class UxmlTraits : TextElement.UxmlTraits
		{
		}
	}
}
