using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E5 RID: 229
	public interface IVolume
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000791 RID: 1937
		// (set) Token: 0x06000792 RID: 1938
		bool isGlobal { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000793 RID: 1939
		List<Collider> colliders { get; }
	}
}
