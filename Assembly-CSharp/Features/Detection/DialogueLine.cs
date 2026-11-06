using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012E9 RID: 4841
	[Serializable]
	public class DialogueLine
	{
		// Token: 0x06009634 RID: 38452 RVA: 0x003DA850 File Offset: 0x003D8A50
		public static DialogueLine[] CloneForPresentation(DialogueLine[] source, string scope, string speakerScope = null)
		{
			if (source == null)
			{
				return null;
			}
			DialogueLine[] array = new DialogueLine[source.Length];
			for (int i = 0; i < source.Length; i++)
			{
				DialogueLine[] array2 = array;
				int num = i;
				DialogueLine dialogueLine2;
				if (source[i] != null)
				{
					DialogueLine dialogueLine = new DialogueLine();
					dialogueLine.speakerName = source[i].speakerName;
					dialogueLine.text = source[i].text;
					dialogueLine.localizationScope = scope;
					dialogueLine2 = dialogueLine;
					dialogueLine.speakerLocalizationScope = speakerScope ?? ((scope != null) ? source[i].speakerLocalizationScope : null);
				}
				else
				{
					dialogueLine2 = null;
				}
				array2[num] = dialogueLine2;
			}
			return array;
		}

		// Token: 0x04009B4C RID: 39756
		public string speakerName;

		// Token: 0x04009B4D RID: 39757
		[TextArea(2, 5)]
		public string text;

		// Token: 0x04009B4E RID: 39758
		[NonSerialized]
		public string localizationScope;

		// Token: 0x04009B4F RID: 39759
		[NonSerialized]
		public string speakerLocalizationScope;
	}
}
