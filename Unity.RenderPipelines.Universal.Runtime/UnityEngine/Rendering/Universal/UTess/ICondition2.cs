using System;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000137 RID: 311
	internal interface ICondition2<in T, in U>
	{
		// Token: 0x06000A14 RID: 2580
		bool Test(T x, U y, ref float t);
	}
}
