using System;
using System.Runtime.InteropServices;

namespace Unity.Serialization.Json
{
	// Token: 0x02000075 RID: 117
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct JsonValidationResult
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000BC4D File Offset: 0x00009E4D
		public bool IsValid()
		{
			return (this.ActualType & this.ExpectedType) == this.ActualType;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BC64 File Offset: 0x00009E64
		public override string ToString()
		{
			string text = ((this.Char == '\0') ? "\\0" : this.Char.ToString());
			string text2 = (this.IsValid() ? "valid" : "invalid");
			return string.Format("Input json was {0}. {1}=[{2}] {3}=[{4}] ActualChar=['{5}'] at Line=[{6}] at Character=[{7}]", new object[] { text2, "ExpectedType", this.ExpectedType, "ActualType", this.ActualType, text, this.LineCount, this.CharCount });
		}

		// Token: 0x04000171 RID: 369
		public JsonValidationType ValidationType;

		// Token: 0x04000172 RID: 370
		public JsonType ExpectedType;

		// Token: 0x04000173 RID: 371
		public JsonType ActualType;

		// Token: 0x04000174 RID: 372
		public char Char;

		// Token: 0x04000175 RID: 373
		public int LineCount;

		// Token: 0x04000176 RID: 374
		public int CharCount;
	}
}
