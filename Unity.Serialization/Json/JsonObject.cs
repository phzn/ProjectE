using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x02000027 RID: 39
	public class JsonObject : Dictionary<string, object>
	{
		// Token: 0x06000099 RID: 153 RVA: 0x0000340B File Offset: 0x0000160B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RegisterPropertyBag()
		{
			if (JsonObject.k_Registered)
			{
				return;
			}
			JsonObject.k_Registered = true;
			PropertyBag.Register<JsonObject>(new KeyValueCollectionPropertyBag<JsonObject, string, object>());
		}

		// Token: 0x04000034 RID: 52
		private static bool k_Registered;
	}
}
