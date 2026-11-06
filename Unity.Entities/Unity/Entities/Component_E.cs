using System;
using System.Diagnostics;

namespace Unity.Entities
{
	// Token: 0x02000048 RID: 72
	internal class Component_E
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0000A60E File Offset: 0x0000880E
		public Component_E(in object value, bool enabled)
		{
			this.Value = value;
			this.Enabled = enabled;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A628 File Offset: 0x00008828
		public override string ToString()
		{
			string text = (this.Enabled ? "Enabled" : "Disabled");
			string text2 = this.Value.ToString();
			string text3 = this.Value.GetType().ToString();
			if (text2 == text3)
			{
				return text3 + " (" + text + ")";
			}
			return string.Concat(new string[] { text3, " (", text, ") ", text2 });
		}

		// Token: 0x04000089 RID: 137
		public bool Enabled;

		// Token: 0x0400008A RID: 138
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object Value;
	}
}
