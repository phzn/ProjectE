using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B06 RID: 2822
	public interface ICustomizationSpawnProvider
	{
		// Token: 0x06004FDA RID: 20442
		bool CanSpawn(CustomizationSessionContext context);

		// Token: 0x06004FDB RID: 20443
		GameObject Spawn(CustomizationSessionContext context, Transform fallbackSpawnPoint);

		// Token: 0x06004FDC RID: 20444
		void Despawn(GameObject runtimeInstance);
	}
}
