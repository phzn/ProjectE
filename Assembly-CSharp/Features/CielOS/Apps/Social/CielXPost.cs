using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001454 RID: 5204
	[Serializable]
	public class CielXPost
	{
		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600A0A7 RID: 41127 RVA: 0x00427EB1 File Offset: 0x004260B1
		public bool HasPhoto
		{
			get
			{
				return !string.IsNullOrEmpty(this.photoName);
			}
		}

		// Token: 0x0600A0A8 RID: 41128 RVA: 0x00427EC4 File Offset: 0x004260C4
		public static CielXPost Text(CielXAccount author, string body, long ageMinutes)
		{
			return new CielXPost
			{
				authorHandle = author.handle,
				authorName = author.displayName,
				authorVerified = author.verified,
				byMe = author.isMe,
				text = body,
				ageMinutes = ageMinutes
			};
		}

		// Token: 0x0400A461 RID: 42081
		[Tooltip("Stable id, used to delete the player's own posts. Empty for seed/bot posts.")]
		public string id;

		// Token: 0x0400A462 RID: 42082
		public string authorHandle;

		// Token: 0x0400A463 RID: 42083
		public string authorName;

		// Token: 0x0400A464 RID: 42084
		public bool authorVerified;

		// Token: 0x0400A465 RID: 42085
		public bool byMe;

		// Token: 0x0400A466 RID: 42086
		[TextArea]
		public string text;

		// Token: 0x0400A467 RID: 42087
		[Tooltip("On-disk, tag-bearing base name of the photo (no extension). Empty for text-only posts. The stored faceVisible choice determines which image version the feed shows.")]
		public string photoName;

		// Token: 0x0400A468 RID: 42088
		[Tooltip("True only when a Certified player explicitly publishes a photo showing their face. False protects the face, including for legacy saves that omit this field.")]
		public bool faceVisible;

		// Token: 0x0400A469 RID: 42089
		public int likes;

		// Token: 0x0400A46A RID: 42090
		public int reposts;

		// Token: 0x0400A46B RID: 42091
		public int replyCount;

		// Token: 0x0400A46C RID: 42092
		[Tooltip("Unix-ish minutes since the app's epoch; used only to render a relative '2h' label.")]
		public long ageMinutes;

		// Token: 0x0400A46D RID: 42093
		[Tooltip("DateTime.UtcNow.Ticks when the player published this post. Used to grow likes/reposts over real time, persistently across sessions. 0 for seed/bot posts.")]
		public long createdAtTicks;

		// Token: 0x0400A46E RID: 42094
		[Tooltip("Likes this post gains per minute (decaying). Higher for racier tags. Drives the time-based growth so a post keeps gaining likes for as long as it exists.")]
		public float likeRatePerMinute;

		// Token: 0x0400A46F RID: 42095
		[Tooltip("Baseline likes at publish time (before time-based growth is added on top).")]
		public int baseLikes;

		// Token: 0x0400A470 RID: 42096
		[Tooltip("Baseline reposts at publish time.")]
		public int baseReposts;

		// Token: 0x0400A471 RID: 42097
		[Tooltip("Baseline comments at publish time.")]
		public int baseReplies;

		// Token: 0x0400A472 RID: 42098
		public bool likedByMe;

		// Token: 0x0400A473 RID: 42099
		public bool repostedByMe;

		// Token: 0x0400A474 RID: 42100
		public List<CielXReply> replies = new List<CielXReply>();

		// Token: 0x0400A475 RID: 42101
		[NonSerialized]
		public Sprite cachedPhoto;

		// Token: 0x0400A476 RID: 42102
		[NonSerialized]
		public bool photoResolved;
	}
}
