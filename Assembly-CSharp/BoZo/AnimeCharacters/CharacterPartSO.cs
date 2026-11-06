using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A1 RID: 2209
	[CreateAssetMenu(fileName = "New Character Part", menuName = "Character Creator/Character Part")]
	public class CharacterPartSO : ScriptableObject
	{
		// Token: 0x04003B99 RID: 15257
		[Tooltip("Nome della parte")]
		public string partName;

		// Token: 0x04003B9A RID: 15258
		[Tooltip("Icona della parte (opzionale)")]
		public Sprite icon;

		// Token: 0x04003B9B RID: 15259
		[Tooltip("Descrizione della parte (opzionale)")]
		[TextArea(2, 5)]
		public string description;

		// Token: 0x04003B9C RID: 15260
		[Tooltip("Tab di configurazione disponibili per questa parte")]
		public List<ConfigTabSO> configTabs = new List<ConfigTabSO>();

		// Token: 0x04003B9D RID: 15261
		[Header("MC Style (optional)")]
		[Tooltip("If assigned, Style tab uses the MC style customization pipeline for this part.")]
		public MCStylePartDefinitionSO stylePartDefinition;
	}
}
