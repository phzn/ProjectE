using System;
using TMPro;
using UnityEngine;

namespace Features.Localization
{
	// Token: 0x020010C2 RID: 4290
	public sealed class GameLocalizationConfig : ScriptableObject
	{
		// Token: 0x04008567 RID: 34151
		public string tableName = "Game";

		// Token: 0x04008568 RID: 34152
		public GameLocalizationConfig.Language[] languages = Array.Empty<GameLocalizationConfig.Language>();

		// Token: 0x04008569 RID: 34153
		public GameLocalizationConfig.SourceEntry[] sources = Array.Empty<GameLocalizationConfig.SourceEntry>();

		// Token: 0x0400856A RID: 34154
		public GameLocalizationConfig.SceneText[] sceneTexts = Array.Empty<GameLocalizationConfig.SceneText>();

		// Token: 0x020010C3 RID: 4291
		[Serializable]
		public sealed class SourceEntry
		{
			// Token: 0x0400856B RID: 34155
			public string scope;

			// Token: 0x0400856C RID: 34156
			public string english;

			// Token: 0x0400856D RID: 34157
			public string key;
		}

		// Token: 0x020010C4 RID: 4292
		[Serializable]
		public sealed class Language
		{
			// Token: 0x0400856E RID: 34158
			public string code;

			// Token: 0x0400856F RID: 34159
			public string nativeName;

			// Token: 0x04008570 RID: 34160
			public TMP_FontAsset font;
		}

		// Token: 0x020010C5 RID: 4293
		[Serializable]
		public sealed class SceneText
		{
			// Token: 0x04008571 RID: 34161
			public string scene;

			// Token: 0x04008572 RID: 34162
			public string rootName;

			// Token: 0x04008573 RID: 34163
			public int[] childIndices;

			// Token: 0x04008574 RID: 34164
			public string[] childNames;

			// Token: 0x04008575 RID: 34165
			public string objectName;

			// Token: 0x04008576 RID: 34166
			public string english;

			// Token: 0x04008577 RID: 34167
			public string key;

			// Token: 0x04008578 RID: 34168
			public bool fitToRectangle;

			// Token: 0x04008579 RID: 34169
			public bool singleLine;
		}
	}
}
