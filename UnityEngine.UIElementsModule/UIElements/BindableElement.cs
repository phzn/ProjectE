using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200002B RID: 43
	public class BindableElement : VisualElement, IBindable
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00004CD1 File Offset: 0x00002ED1
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00004CD9 File Offset: 0x00002ED9
		public IBinding binding { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00004CE2 File Offset: 0x00002EE2
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00004CEA File Offset: 0x00002EEA
		public string bindingPath { get; set; }

		// Token: 0x0200002C RID: 44
		public new class UxmlFactory : UxmlFactory<BindableElement, BindableElement.UxmlTraits>
		{
		}

		// Token: 0x0200002D RID: 45
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x060001B4 RID: 436 RVA: 0x00004D05 File Offset: 0x00002F05
			public UxmlTraits()
			{
				this.m_PropertyPath = new UxmlStringAttributeDescription
				{
					name = "binding-path"
				};
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x00004D28 File Offset: 0x00002F28
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				string valueFromBag = this.m_PropertyPath.GetValueFromBag(bag, cc);
				IBindable bindable = ve as IBindable;
				bool flag = bindable != null;
				if (flag)
				{
					bindable.bindingPath = (string.IsNullOrEmpty(valueFromBag) ? string.Empty : valueFromBag);
				}
			}

			// Token: 0x0400007D RID: 125
			private UxmlStringAttributeDescription m_PropertyPath;
		}
	}
}
