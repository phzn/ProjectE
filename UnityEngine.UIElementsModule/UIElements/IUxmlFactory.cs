using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003CD RID: 973
	public interface IUxmlFactory : IBaseUxmlFactory
	{
		// Token: 0x0600200E RID: 8206
		VisualElement Create(IUxmlAttributes bag, CreationContext cc);
	}
}
