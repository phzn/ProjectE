using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C3E RID: 3134
	[CreateAssetMenu(fileName = "StadiumCommentaryLines", menuName = "Stadium/Commentary Lines")]
	public class StadiumCommentaryLinesSO : ScriptableObject
	{
		// Token: 0x04005B53 RID: 23379
		[TextArea(2, 4)]
		public string[] lines;
	}
}
