using System;

namespace Unity.Properties
{
	// Token: 0x0200008E RID: 142
	internal interface IConstructor<out T> : IConstructor
	{
		// Token: 0x06000308 RID: 776
		T Instantiate();
	}
}
