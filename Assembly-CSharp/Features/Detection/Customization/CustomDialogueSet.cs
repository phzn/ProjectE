using System;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x02001319 RID: 4889
	[Serializable]
	public class CustomDialogueSet
	{
		// Token: 0x04009CB6 RID: 40118
		public string id;

		// Token: 0x04009CB7 RID: 40119
		[Range(1f, 100f)]
		public int weight = 10;

		// Token: 0x04009CB8 RID: 40120
		public DialogueLine[] lines;
	}
}
