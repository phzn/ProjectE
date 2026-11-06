using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000243 RID: 579
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public abstract class BaseFieldMouseDragger
	{
		// Token: 0x0600106E RID: 4206 RVA: 0x0003B9DA File Offset: 0x00039BDA
		public void SetDragZone(VisualElement dragElement)
		{
			this.SetDragZone(dragElement, new Rect(0f, 0f, -1f, -1f));
		}

		// Token: 0x0600106F RID: 4207
		public abstract void SetDragZone(VisualElement dragElement, Rect hotZone);
	}
}
