using System;
using Unity.Properties;
using Unity.Serialization.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Unity.Serialization
{
	// Token: 0x0200000D RID: 13
	[Preserve]
	internal class DefaultPropertyBagInitializer
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000022F0 File Offset: 0x000004F0
		[RuntimeInitializeOnLoadMethod]
		internal static void Initialize()
		{
			PropertyBag.Register<SerializedObjectView>(new SerializedObjectViewPropertyBag());
			PropertyBag.Register<SerializedArrayView>(new SerializedArrayViewPropertyBag());
			UnsafeSerializedObjectReader.CreateBurstDelegates();
		}
	}
}
