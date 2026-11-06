using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200010B RID: 267
	public class PopupWindow : TextElement
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x000239C8 File Offset: 0x00021BC8
		public PopupWindow()
		{
			base.AddToClassList(PopupWindow.ussClassName);
			this.m_ContentContainer = new VisualElement
			{
				name = "unity-content-container"
			};
			this.m_ContentContainer.AddToClassList(PopupWindow.contentUssClassName);
			base.hierarchy.Add(this.m_ContentContainer);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00023A28 File Offset: 0x00021C28
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_ContentContainer;
			}
		}

		// Token: 0x0400041C RID: 1052
		private VisualElement m_ContentContainer;

		// Token: 0x0400041D RID: 1053
		public new static readonly string ussClassName = "unity-popup-window";

		// Token: 0x0400041E RID: 1054
		public static readonly string contentUssClassName = PopupWindow.ussClassName + "__content-container";

		// Token: 0x0200010C RID: 268
		public new class UxmlFactory : UxmlFactory<PopupWindow, PopupWindow.UxmlTraits>
		{
		}

		// Token: 0x0200010D RID: 269
		public new class UxmlTraits : TextElement.UxmlTraits
		{
			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x0600092F RID: 2351 RVA: 0x00023A6C File Offset: 0x00021C6C
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield return new UxmlChildElementDescription(typeof(VisualElement));
					yield break;
				}
			}
		}
	}
}
