using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EAC RID: 3756
	[Serializable]
	public class OxGramChat
	{
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x060070BD RID: 28861 RVA: 0x002DC242 File Offset: 0x002DA442
		public OxGramMessage LastMessage
		{
			get
			{
				if (this.messages == null || this.messages.Count <= 0)
				{
					return null;
				}
				return this.messages[this.messages.Count - 1];
			}
		}

		// Token: 0x0400737F RID: 29567
		public string id;

		// Token: 0x04007380 RID: 29568
		public string contactId;

		// Token: 0x04007381 RID: 29569
		public string title;

		// Token: 0x04007382 RID: 29570
		public bool isGroup;

		// Token: 0x04007383 RID: 29571
		[Tooltip("Subtitle under the title in the chat list (e.g. members count or last seen).")]
		public string subtitle;

		// Token: 0x04007384 RID: 29572
		[Tooltip("Names of NPC members that can talk in this chat (used by the 'alive' simulator).")]
		public List<string> members = new List<string>();

		// Token: 0x04007385 RID: 29573
		[Tooltip("Lines NPCs may randomly post to keep the chat feeling alive.")]
		public List<string> ambientLines = new List<string>();

		// Token: 0x04007386 RID: 29574
		public List<OxGramMessage> messages = new List<OxGramMessage>();
	}
}
