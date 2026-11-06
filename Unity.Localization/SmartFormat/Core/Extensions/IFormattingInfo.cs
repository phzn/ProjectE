using System;
using System.ComponentModel;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace UnityEngine.Localization.SmartFormat.Core.Extensions
{
	// Token: 0x02000092 RID: 146
	public interface IFormattingInfo
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003EF RID: 1007
		object CurrentValue { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003F0 RID: 1008
		Format Format { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003F1 RID: 1009
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Placeholder Placeholder { get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003F2 RID: 1010
		int Alignment { get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003F3 RID: 1011
		string FormatterOptions { get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003F4 RID: 1012
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		FormatDetails FormatDetails { get; }

		// Token: 0x060003F5 RID: 1013
		void Write(string text);

		// Token: 0x060003F6 RID: 1014
		void Write(string text, int startIndex, int length);

		// Token: 0x060003F7 RID: 1015
		void Write(Format format, object value);

		// Token: 0x060003F8 RID: 1016
		FormattingException FormattingException(string issue, FormatItem problemItem = null, int startIndex = -1);
	}
}
