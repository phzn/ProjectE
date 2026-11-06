using System;
using System.Diagnostics;
using UnityEngine.Pool;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000AA RID: 170
	[DebuggerDisplay("Writable: {Text}")]
	public class WritableMessageFragment : MessageFragment
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x000109A6 File Offset: 0x0000EBA6
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x000109AE File Offset: 0x0000EBAE
		public string Text
		{
			get
			{
				return this.ToString();
			}
			set
			{
				base.Initialize(base.Message, value);
			}
		}

		// Token: 0x0400015F RID: 351
		internal static readonly ObjectPool<WritableMessageFragment> Pool = new ObjectPool<WritableMessageFragment>(() => new WritableMessageFragment(), null, null, null, false, 10, 10000);
	}
}
