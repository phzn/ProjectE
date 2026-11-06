using System;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000014 RID: 20
	public interface IKeyEvaluator
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000158 RID: 344
		object RuntimeKey { get; }

		// Token: 0x06000159 RID: 345
		bool RuntimeKeyIsValid();
	}
}
