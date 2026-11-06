using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A6 RID: 2214
	[CreateAssetMenu(fileName = "ColorGroupConfig", menuName = "Character Creator/Color Group Config")]
	public class ColorGroupConfigSO : ScriptableObject
	{
		// Token: 0x04003BB0 RID: 15280
		[Tooltip("Nome del gruppo di colori")]
		public string groupName;

		// Token: 0x04003BB1 RID: 15281
		[Tooltip("Etichetta da mostrare nella UI")]
		public string displayLabel;

		// Token: 0x04003BB2 RID: 15282
		[Tooltip("Colore predefinito per questo gruppo")]
		public Color defaultColor = Color.white;

		// Token: 0x04003BB3 RID: 15283
		[Tooltip("Priorità di visualizzazione (valori più bassi vengono mostrati prima)")]
		public int sortOrder;

		// Token: 0x04003BB4 RID: 15284
		[Tooltip("Canali di colore collegati che verranno modificati insieme")]
		public List<ColorGroupConfigSO.LinkedColorChannel> linkedChannels = new List<ColorGroupConfigSO.LinkedColorChannel>();

		// Token: 0x020008A7 RID: 2215
		[Serializable]
		public class LinkedColorChannel
		{
			// Token: 0x04003BB5 RID: 15285
			[Tooltip("Configurazione del canale di colore")]
			public ColorChannelConfigSO channelConfig;

			// Token: 0x04003BB6 RID: 15286
			[Tooltip("Nome della proprietà del canale di colore")]
			public string propertyName;
		}
	}
}
