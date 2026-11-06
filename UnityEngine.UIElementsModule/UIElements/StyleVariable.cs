using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200035E RID: 862
	internal struct StyleVariable
	{
		// Token: 0x06001CC4 RID: 7364 RVA: 0x0006FAA5 File Offset: 0x0006DCA5
		public StyleVariable(string name, StyleSheet sheet, StyleValueHandle[] handles)
		{
			this.name = name;
			this.sheet = sheet;
			this.handles = handles;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0006FAC0 File Offset: 0x0006DCC0
		public override int GetHashCode()
		{
			int num = this.name.GetHashCode();
			num = (num * 397) ^ this.sheet.GetHashCode();
			return (num * 397) ^ this.handles.GetHashCode();
		}

		// Token: 0x04000C15 RID: 3093
		public readonly string name;

		// Token: 0x04000C16 RID: 3094
		public readonly StyleSheet sheet;

		// Token: 0x04000C17 RID: 3095
		public readonly StyleValueHandle[] handles;
	}
}
