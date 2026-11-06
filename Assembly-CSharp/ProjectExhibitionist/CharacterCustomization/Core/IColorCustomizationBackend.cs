using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD7 RID: 2775
	public interface IColorCustomizationBackend
	{
		// Token: 0x06004EAD RID: 20141
		bool TrySetColor(string category, string propertyName, Color color);

		// Token: 0x06004EAE RID: 20142
		bool TryGetColor(string category, string propertyName, out Color color);
	}
}
