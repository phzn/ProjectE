using System;
using System.IO;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Core.Output
{
	// Token: 0x02000089 RID: 137
	public class TextWriterOutput : IOutput
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0000E270 File Offset: 0x0000C470
		public TextWriterOutput(TextWriter output)
		{
			this.Output = output;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000E27F File Offset: 0x0000C47F
		public TextWriter Output { get; }

		// Token: 0x0600039F RID: 927 RVA: 0x0000E287 File Offset: 0x0000C487
		public void Write(string text, IFormattingInfo formattingInfo)
		{
			this.Output.Write(text);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000E295 File Offset: 0x0000C495
		public void Write(string text, int startIndex, int length, IFormattingInfo formattingInfo)
		{
			this.Output.Write(text.Substring(startIndex, length));
		}
	}
}
