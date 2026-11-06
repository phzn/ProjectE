using System;
using System.Diagnostics;
using UnityEngine.Pool;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000AB RID: 171
	[DebuggerDisplay("ReadOnly: {Text}")]
	public class ReadOnlyMessageFragment : MessageFragment
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000109EC File Offset: 0x0000EBEC
		public string Text
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x04000160 RID: 352
		internal static readonly ObjectPool<ReadOnlyMessageFragment> Pool = new ObjectPool<ReadOnlyMessageFragment>(() => new ReadOnlyMessageFragment(), null, null, null, false, 10, 10000);
	}
}
