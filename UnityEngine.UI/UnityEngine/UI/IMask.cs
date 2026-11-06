using System;
using System.ComponentModel;

namespace UnityEngine.UI
{
	// Token: 0x02000017 RID: 23
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Not supported anymore.", true)]
	public interface IMask
	{
		// Token: 0x0600015B RID: 347
		bool Enabled();

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600015C RID: 348
		RectTransform rectTransform { get; }
	}
}
