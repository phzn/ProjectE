using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000131 RID: 305
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class DisplayStringFormatAttribute : Attribute
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x00051AA4 File Offset: 0x0004FCA4
		// (set) Token: 0x0600110B RID: 4363 RVA: 0x00051AAC File Offset: 0x0004FCAC
		public string formatString { get; set; }

		// Token: 0x0600110C RID: 4364 RVA: 0x00051AB5 File Offset: 0x0004FCB5
		public DisplayStringFormatAttribute(string formatString)
		{
			this.formatString = formatString;
		}
	}
}
