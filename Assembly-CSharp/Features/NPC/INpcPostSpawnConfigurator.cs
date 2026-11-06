using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EC8 RID: 3784
	public interface INpcPostSpawnConfigurator
	{
		// Token: 0x06007143 RID: 28995
		void Configure(GameObject npc, CharacterObject characterData);
	}
}
