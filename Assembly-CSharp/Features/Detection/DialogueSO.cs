using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012EA RID: 4842
	[CreateAssetMenu(fileName = "New Dialogue", menuName = "Detection/Dialogue")]
	public class DialogueSO : ScriptableObject
	{
		// Token: 0x04009B50 RID: 39760
		public DialogueLine[] lines;
	}
}
