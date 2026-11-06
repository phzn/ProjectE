using System;
using System.Collections.Generic;
using Features.Detection;
using Features.MainMenu;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E1E RID: 3614
	[CreateAssetMenu(fileName = "New Gameplay Emote Collection", menuName = "Player Emotes/Gameplay Emote Collection")]
	public class GameplayEmoteCollectionSO : ScriptableObject
	{
		// Token: 0x06006B90 RID: 27536 RVA: 0x002B0E14 File Offset: 0x002AF014
		public List<EmoteDefinitionSO> GetUnlockedEmotes()
		{
			List<EmoteDefinitionSO> list = new List<EmoteDefinitionSO>();
			for (int i = 0; i < this.Emotes.Count; i++)
			{
				if (this.Emotes[i] != null && CollectionUnlockRegistry.IsEmoteUnlocked(this.Emotes[i]))
				{
					list.Add(this.Emotes[i]);
				}
			}
			return list;
		}

		// Token: 0x04006E32 RID: 28210
		[Tooltip("Flat list of all emotes available in the gameplay emote wheel")]
		public List<EmoteDefinitionSO> Emotes = new List<EmoteDefinitionSO>();
	}
}
