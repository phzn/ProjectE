using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000231 RID: 561
	[EventCategory(EventCategory.Style)]
	public class CustomStyleResolvedEvent : EventBase<CustomStyleResolvedEvent>
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x0003B56C File Offset: 0x0003976C
		static CustomStyleResolvedEvent()
		{
			EventBase<CustomStyleResolvedEvent>.SetCreateFunction(() => new CustomStyleResolvedEvent());
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0003B588 File Offset: 0x00039788
		public ICustomStyle customStyle
		{
			get
			{
				VisualElement visualElement = base.target as VisualElement;
				return (visualElement != null) ? visualElement.customStyle : null;
			}
		}
	}
}
