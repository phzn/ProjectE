using System;

namespace System.Linq
{
	// Token: 0x020000CC RID: 204
	internal sealed class SystemCore_EnumerableDebugViewEmptyException : Exception
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001A976 File Offset: 0x00018B76
		public string Empty
		{
			get
			{
				return "Enumeration yielded no results";
			}
		}
	}
}
