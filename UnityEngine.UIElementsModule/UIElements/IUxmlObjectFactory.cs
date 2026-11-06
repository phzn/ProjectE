using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003CF RID: 975
	internal interface IUxmlObjectFactory<out T> : IBaseUxmlObjectFactory, IBaseUxmlFactory where T : new()
	{
		// Token: 0x0600200F RID: 8207
		T CreateObject(IUxmlAttributes bag, CreationContext cc);
	}
}
