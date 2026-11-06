using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000292 RID: 658
	public interface IPanel : IDisposable
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001251 RID: 4689
		VisualElement visualTree { get; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001252 RID: 4690
		EventDispatcher dispatcher { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001253 RID: 4691
		ContextType contextType { get; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001254 RID: 4692
		FocusController focusController { get; }

		// Token: 0x06001255 RID: 4693
		VisualElement Pick(Vector2 point);

		// Token: 0x06001256 RID: 4694
		VisualElement PickAll(Vector2 point, List<VisualElement> picked);

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001257 RID: 4695
		ContextualMenuManager contextualMenuManager { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001258 RID: 4696
		bool isDirty { get; }
	}
}
