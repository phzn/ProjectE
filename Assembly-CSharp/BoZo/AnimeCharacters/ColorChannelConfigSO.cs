using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A3 RID: 2211
	[CreateAssetMenu(fileName = "ColorChannelConfig", menuName = "Character Creator/Color Channel Config")]
	public class ColorChannelConfigSO : ScriptableObject
	{
		// Token: 0x06003B72 RID: 15218 RVA: 0x001617B0 File Offset: 0x0015F9B0
		public ColorChannelConfigSO.ColorChannelInfo GetChannelByPropertyName(string propertyName)
		{
			return this.colorChannels.Find((ColorChannelConfigSO.ColorChannelInfo c) => c.channelPropertyName == propertyName);
		}

		// Token: 0x04003BA8 RID: 15272
		[Tooltip("Nome della categoria di colori (es. 'Skin', 'Hair', 'Outfit')")]
		public string categoryName;

		// Token: 0x04003BA9 RID: 15273
		[Tooltip("Elenco dei canali di colore disponibili per questa categoria")]
		public List<ColorChannelConfigSO.ColorChannelInfo> colorChannels = new List<ColorChannelConfigSO.ColorChannelInfo>();

		// Token: 0x020008A4 RID: 2212
		[Serializable]
		public class ColorChannelInfo
		{
			// Token: 0x04003BAA RID: 15274
			public string channelPropertyName;

			// Token: 0x04003BAB RID: 15275
			public string displayLabel;

			// Token: 0x04003BAC RID: 15276
			public Color defaultColor = Color.white;

			// Token: 0x04003BAD RID: 15277
			public bool isEnabled = true;

			// Token: 0x04003BAE RID: 15278
			public int sortOrder;
		}
	}
}
