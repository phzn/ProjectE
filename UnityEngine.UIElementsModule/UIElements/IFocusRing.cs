using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000247 RID: 583
	public interface IFocusRing
	{
		// Token: 0x0600109B RID: 4251
		FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e);

		// Token: 0x0600109C RID: 4252
		Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction);
	}
}
