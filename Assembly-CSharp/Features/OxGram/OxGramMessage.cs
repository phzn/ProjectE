using System;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EAB RID: 3755
	[Serializable]
	public class OxGramMessage
	{
		// Token: 0x060070BB RID: 28859 RVA: 0x002DC1EE File Offset: 0x002DA3EE
		public static OxGramMessage Text(OxGramSender sender, string author, string body, string time)
		{
			return new OxGramMessage
			{
				sender = sender,
				kind = OxGramMessageKind.Text,
				authorName = author,
				text = body,
				time = time
			};
		}

		// Token: 0x060070BC RID: 28860 RVA: 0x002DC218 File Offset: 0x002DA418
		public static OxGramMessage Photo(OxGramSender sender, string author, Sprite photo, string time)
		{
			return new OxGramMessage
			{
				sender = sender,
				kind = OxGramMessageKind.Photo,
				authorName = author,
				photo = photo,
				time = time
			};
		}

		// Token: 0x04007376 RID: 29558
		public OxGramSender sender = OxGramSender.Other;

		// Token: 0x04007377 RID: 29559
		public OxGramMessageKind kind;

		// Token: 0x04007378 RID: 29560
		[Tooltip("Display name of the author (NPC name, or the player's handle).")]
		public string authorName;

		// Token: 0x04007379 RID: 29561
		[Tooltip("Message body for text messages.")]
		[TextArea]
		public string text;

		// Token: 0x0400737A RID: 29562
		[Tooltip("Time label shown next to the bubble, e.g. \"14:32\".")]
		public string time;

		// Token: 0x0400737B RID: 29563
		[Tooltip("Photo sprite for Photo messages (optional).")]
		public Sprite photo;

		// Token: 0x0400737C RID: 29564
		[NonSerialized]
		public string photoFileName;

		// Token: 0x0400737D RID: 29565
		[NonSerialized]
		public string reactionTemplate;

		// Token: 0x0400737E RID: 29566
		[NonSerialized]
		public string reactionAlias;
	}
}
