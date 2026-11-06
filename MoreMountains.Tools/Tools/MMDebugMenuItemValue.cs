using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000041 RID: 65
	public class MMDebugMenuItemValue : MonoBehaviour
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000706C File Offset: 0x0000526C
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00007074 File Offset: 0x00005274
		public virtual float Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
				this.ValueText.text = value.ToString("F2");
			}
		}

		// Token: 0x040001BA RID: 442
		[Header("Bindings")]
		public Text LabelText;

		// Token: 0x040001BB RID: 443
		public Text ValueText;

		// Token: 0x040001BC RID: 444
		public MMRadioReceiver RadioReceiver;

		// Token: 0x040001BD RID: 445
		protected float _level;
	}
}
