using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Core.Output
{
	// Token: 0x02000087 RID: 135
	public interface IOutput
	{
		// Token: 0x06000393 RID: 915
		void Write(string text, IFormattingInfo formattingInfo);

		// Token: 0x06000394 RID: 916
		void Write(string text, int startIndex, int length, IFormattingInfo formattingInfo);
	}
}
