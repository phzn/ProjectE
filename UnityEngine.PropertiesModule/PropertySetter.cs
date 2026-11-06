using System;

namespace Unity.Properties
{
	// Token: 0x0200001C RID: 28
	// (Invoke) Token: 0x06000067 RID: 103
	public delegate void PropertySetter<TContainer, in TValue>(ref TContainer container, TValue value);
}
