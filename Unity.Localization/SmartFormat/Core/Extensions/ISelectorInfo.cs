using System;
using System.ComponentModel;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Core.Extensions
{
	// Token: 0x02000093 RID: 147
	public interface ISelectorInfo
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003F9 RID: 1017
		object CurrentValue { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003FA RID: 1018
		string SelectorText { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003FB RID: 1019
		int SelectorIndex { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003FC RID: 1020
		string SelectorOperator { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003FD RID: 1021
		// (set) Token: 0x060003FE RID: 1022
		object Result { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003FF RID: 1023
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Placeholder Placeholder { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000400 RID: 1024
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		FormatDetails FormatDetails { get; }
	}
}
