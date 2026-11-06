using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001285 RID: 4741
	[CreateAssetMenu(fileName = "New Emote Wheel Theme", menuName = "Detection/Emote Wheel/Wheel Theme")]
	public class EmoteWheelThemeSO : ScriptableObject
	{
		// Token: 0x040097DF RID: 38879
		[Header("Arc Background")]
		[Tooltip("Background sprite (bake color+pattern+shadow into this sprite)")]
		public Sprite ArcSprite;

		// Token: 0x040097E0 RID: 38880
		[Tooltip("Tint color applied to the arc sprite")]
		public Color ArcTint = Color.white;

		// Token: 0x040097E1 RID: 38881
		[Header("Card Prefab")]
		[Tooltip("Prefab to instantiate into each slot. Contains all visual styling (font, gradient, outline, shadows, glow).")]
		public EmoteCardUI CardPrefab;

		// Token: 0x040097E2 RID: 38882
		[Header("Focus Highlight")]
		[Tooltip("Scale multiplier for the focused (center) card")]
		public float FocusedScale = 1.15f;

		// Token: 0x040097E3 RID: 38883
		[Tooltip("Alpha for non-focused side cards (0-1)")]
		[Range(0f, 1f)]
		public float SideAlpha = 0.55f;
	}
}
