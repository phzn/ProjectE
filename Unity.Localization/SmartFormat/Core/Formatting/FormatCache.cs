using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization.SmartFormat.Core.Formatting
{
	// Token: 0x0200008B RID: 139
	public class FormatCache
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000E2D2 File Offset: 0x0000C4D2
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		public Format Format { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000E2E3 File Offset: 0x0000C4E3
		public Dictionary<string, object> CachedObjects { get; } = new Dictionary<string, object>();

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000E2EB File Offset: 0x0000C4EB
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000E2F3 File Offset: 0x0000C4F3
		public IVariableGroup LocalVariables { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		public List<IVariableValueChanged> VariableTriggers { get; } = new List<IVariableValueChanged>();

		// Token: 0x04000109 RID: 265
		public LocalizationTable Table;
	}
}
