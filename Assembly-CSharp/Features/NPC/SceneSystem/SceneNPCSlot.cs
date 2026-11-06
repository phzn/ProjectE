using System;
using Bozo.AnimeCharacters;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F3B RID: 3899
	[Serializable]
	public class SceneNPCSlot
	{
		// Token: 0x040077D2 RID: 30674
		[Header("Identity")]
		[Tooltip("Unique identifier for this NPC within the scene (e.g. 'teacher', 'student_1')")]
		public string slotId;

		// Token: 0x040077D3 RID: 30675
		[Tooltip("Character data defining appearance and outfit")]
		public CharacterObject characterData;

		// Token: 0x040077D4 RID: 30676
		[Header("Spawn")]
		[Tooltip("Index into SceneTrigger.spawnPoints array for initial position")]
		public int spawnPointIndex;

		// Token: 0x040077D5 RID: 30677
		[Tooltip("If set, overrides spawnPointIndex with a named Transform from the trigger")]
		public string spawnPointName;

		// Token: 0x040077D6 RID: 30678
		[Header("Behavior")]
		[Tooltip("If false, NPCSenses is disabled - NPC won't detect the player")]
		public bool enableDetection;

		// Token: 0x040077D7 RID: 30679
		[Tooltip("Initial posture when spawned")]
		public NPCInterestPoint.PostureType initialPosture;

		// Token: 0x040077D8 RID: 30680
		[Tooltip("Initial action when spawned")]
		public NPCInterestPoint.ActionType initialAction;

		// Token: 0x040077D9 RID: 30681
		[Tooltip("Personality profile for expressions and reactions (optional)")]
		public NPCPersonalityProfileSO personalityProfile;

		// Token: 0x040077DA RID: 30682
		[Header("End Behavior")]
		[Tooltip("What happens to this NPC when the scene ends")]
		public SceneEndNPCBehavior endBehavior;
	}
}
