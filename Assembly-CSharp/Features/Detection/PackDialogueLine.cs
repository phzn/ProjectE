using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200127B RID: 4731
	[Serializable]
	public struct PackDialogueLine
	{
		// Token: 0x04009791 RID: 38801
		[Tooltip("Chi parla questa battuta (risolto a runtime dal director)")]
		public SpeakerRole role;

		// Token: 0x04009792 RID: 38802
		[TextArea(2, 4)]
		[Tooltip("Testo della battuta")]
		public string text;

		// Token: 0x04009793 RID: 38803
		[Tooltip("Animator trigger opzionale da lanciare sull'attore che parla (vuoto = nessuno)")]
		public string animationTrigger;
	}
}
