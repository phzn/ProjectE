using System;

namespace UnityEngine.Localization.SmartFormat.Core.Extensions
{
	// Token: 0x02000090 RID: 144
	public interface IFormatter
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003EB RID: 1003
		// (set) Token: 0x060003EC RID: 1004
		string[] Names { get; set; }

		// Token: 0x060003ED RID: 1005
		bool TryEvaluateFormat(IFormattingInfo formattingInfo);
	}
}
