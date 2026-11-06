using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000076 RID: 118
	internal interface ITransform
	{
		// Token: 0x060001BB RID: 443
		void GetUsedTransform(HashSet<Transform> transformSet);

		// Token: 0x060001BC RID: 444
		void ReplaceTransform(Dictionary<int, Transform> replaceDict);
	}
}
