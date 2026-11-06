using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000023 RID: 35
	public interface IStaticCullingSourceStrategy
	{
		// Token: 0x060000C9 RID: 201
		bool Validate(out string errorMessage);

		// Token: 0x060000CA RID: 202
		bool TryGetBounds(out Bounds bounds);

		// Token: 0x060000CB RID: 203
		CullingTarget CreateCullingTarget();

		// Token: 0x060000CC RID: 204
		void PrepareForBaking();

		// Token: 0x060000CD RID: 205
		void ClearAfterBaking();
	}
}
