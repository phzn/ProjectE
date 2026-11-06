using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200049F RID: 1183
	internal struct MatchResult
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x0009B640 File Offset: 0x00099840
		public bool success
		{
			get
			{
				return this.errorCode == MatchResultErrorCode.None;
			}
		}

		// Token: 0x040011C3 RID: 4547
		public MatchResultErrorCode errorCode;

		// Token: 0x040011C4 RID: 4548
		public string errorValue;
	}
}
