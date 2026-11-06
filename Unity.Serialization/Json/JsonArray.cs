using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x02000028 RID: 40
	public class JsonArray : List<object>
	{
		// Token: 0x0600009B RID: 155 RVA: 0x0000342D File Offset: 0x0000162D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RegisterPropertyBag()
		{
			if (JsonArray.k_Registered)
			{
				return;
			}
			JsonArray.k_Registered = true;
			PropertyBag.Register<JsonArray>(new IndexedCollectionPropertyBag<JsonArray, object>());
		}

		// Token: 0x04000035 RID: 53
		private static bool k_Registered;
	}
}
