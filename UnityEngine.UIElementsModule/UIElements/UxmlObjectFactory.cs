using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D2 RID: 978
	internal class UxmlObjectFactory<TCreatedType, TTraits> : BaseUxmlFactory<TCreatedType, TTraits>, IUxmlObjectFactory<TCreatedType>, IBaseUxmlObjectFactory, IBaseUxmlFactory where TCreatedType : new() where TTraits : UxmlObjectTraits<TCreatedType>, new()
	{
		// Token: 0x0600201E RID: 8222 RVA: 0x000797F8 File Offset: 0x000779F8
		public virtual TCreatedType CreateObject(IUxmlAttributes bag, CreationContext cc)
		{
			TCreatedType tcreatedType = new TCreatedType();
			this.m_Traits.Init(ref tcreatedType, bag, cc);
			return tcreatedType;
		}
	}
}
