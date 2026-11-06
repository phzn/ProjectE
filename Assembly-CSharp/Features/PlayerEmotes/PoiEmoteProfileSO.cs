using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E28 RID: 3624
	[CreateAssetMenu(fileName = "PoiEmoteProfile", menuName = "Player Emotes/POI Emote Profile")]
	public class PoiEmoteProfileSO : ScriptableObject
	{
		// Token: 0x04006E9B RID: 28315
		[Header("Identity")]
		[Tooltip("Deterministic POI id (for debug/telemetry)")]
		public string PoiId = "POI_Default";

		// Token: 0x04006E9C RID: 28316
		[Tooltip("Player-facing location name used by the Collection.")]
		public string DisplayName;

		// Token: 0x04006E9D RID: 28317
		[TextArea]
		[Tooltip("Short player-facing description used by the Collection.")]
		public string Description;

		// Token: 0x04006E9E RID: 28318
		[Tooltip("Optional 2D location preview. The Collection uses a styled fallback when empty.")]
		public Sprite PreviewImage;

		// Token: 0x04006E9F RID: 28319
		[Header("Wheel Theme Override")]
		[Tooltip("If assigned, overrides GameplayEmoteController.WheelTheme while player is in this POI")]
		public EmoteWheelThemeSO WheelThemeOverride;

		// Token: 0x04006EA0 RID: 28320
		[Header("Priority Emotes")]
		[Tooltip("Emotes injected at the top of the wheel list while in this POI")]
		public List<EmoteDefinitionSO> PriorityEmotes = new List<EmoteDefinitionSO>();

		// Token: 0x04006EA1 RID: 28321
		[Tooltip("Optional card prefab applied only to PriorityEmotes in this POI. Default emotes keep the base theme card.")]
		public EmoteCardUI PriorityEmoteCardPrefab;

		// Token: 0x04006EA2 RID: 28322
		[Header("Zone Priority")]
		[Tooltip("Higher value wins when multiple POI zones overlap")]
		public int Priority;
	}
}
