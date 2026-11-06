using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002EB RID: 747
	internal interface IStyleDataGroup<T>
	{
		// Token: 0x06001954 RID: 6484
		T Copy();

		// Token: 0x06001955 RID: 6485
		void CopyFrom(ref T other);
	}
}
