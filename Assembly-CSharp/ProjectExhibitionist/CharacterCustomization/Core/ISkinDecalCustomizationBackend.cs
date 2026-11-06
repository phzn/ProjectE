using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD9 RID: 2777
	public interface ISkinDecalCustomizationBackend
	{
		// Token: 0x06004EB1 RID: 20145
		bool TrySetDecalState(string decalId, bool enabled, float opacity, Color tint);
	}
}
