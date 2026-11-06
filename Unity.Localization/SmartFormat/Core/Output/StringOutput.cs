using System;
using System.Text;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Core.Output
{
	// Token: 0x02000088 RID: 136
	public class StringOutput : IOutput
	{
		// Token: 0x06000395 RID: 917 RVA: 0x0000E1E3 File Offset: 0x0000C3E3
		public StringOutput()
		{
			this.output = new StringBuilder();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E1F6 File Offset: 0x0000C3F6
		public StringOutput(int capacity)
		{
			this.output = new StringBuilder(capacity);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E20A File Offset: 0x0000C40A
		public StringOutput(StringBuilder output)
		{
			this.output = output;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000E219 File Offset: 0x0000C419
		public void SetCapacity(int capacity)
		{
			if (this.output.Capacity < capacity)
			{
				this.output.Capacity = capacity;
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000E235 File Offset: 0x0000C435
		public void Write(string text, IFormattingInfo formattingInfo)
		{
			this.output.Append(text);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E244 File Offset: 0x0000C444
		public void Write(string text, int startIndex, int length, IFormattingInfo formattingInfo)
		{
			this.output.Append(text, startIndex, length);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E255 File Offset: 0x0000C455
		public void Clear()
		{
			this.output.Clear();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000E263 File Offset: 0x0000C463
		public override string ToString()
		{
			return this.output.ToString();
		}

		// Token: 0x04000104 RID: 260
		private readonly StringBuilder output;
	}
}
