using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D1 RID: 209
	internal class TextEditorEventHandler
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x0001B17F File Offset: 0x0001937F
		protected TextEditorEventHandler(TextElement textElement, TextEditingUtilities editingUtilities)
		{
			this.textElement = textElement;
			this.editingUtilities = editingUtilities;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void ExecuteDefaultActionAtTarget(EventBase evt)
		{
		}

		// Token: 0x0400031C RID: 796
		protected TextElement textElement;

		// Token: 0x0400031D RID: 797
		protected TextEditingUtilities editingUtilities;
	}
}
