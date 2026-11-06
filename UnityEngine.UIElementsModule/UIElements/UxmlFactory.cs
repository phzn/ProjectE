using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D1 RID: 977
	public class UxmlFactory<TCreatedType, TTraits> : BaseUxmlFactory<TCreatedType, TTraits>, IUxmlFactory, IBaseUxmlFactory where TCreatedType : VisualElement, new() where TTraits : UxmlTraits, new()
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x000797B8 File Offset: 0x000779B8
		public virtual VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			TCreatedType tcreatedType = new TCreatedType();
			this.m_Traits.Init(tcreatedType, bag, cc);
			return tcreatedType;
		}
	}
}
