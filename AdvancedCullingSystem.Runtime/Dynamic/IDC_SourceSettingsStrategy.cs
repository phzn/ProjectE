using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000051 RID: 81
	public interface IDC_SourceSettingsStrategy
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000202 RID: 514
		bool ReadyForCulling { get; }

		// Token: 0x06000203 RID: 515
		bool CheckCompatibilityAndGetComponents(out string incompatibilityReason);

		// Token: 0x06000204 RID: 516
		void PrepareForCulling();

		// Token: 0x06000205 RID: 517
		void ClearData();

		// Token: 0x06000206 RID: 518
		bool TryGetBounds(ref Bounds bounds);

		// Token: 0x06000207 RID: 519
		ICullingTarget CreateCullingTarget();

		// Token: 0x06000208 RID: 520
		IEnumerable<Collider> GetColliders();
	}
}
