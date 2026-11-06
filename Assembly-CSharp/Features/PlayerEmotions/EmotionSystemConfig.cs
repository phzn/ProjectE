using System;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E0A RID: 3594
	[CreateAssetMenu(fileName = "EmotionSystemConfig", menuName = "Player Emotions/System Config")]
	public class EmotionSystemConfig : ScriptableObject
	{
		// Token: 0x04006DC4 RID: 28100
		[Header("=== DETECTION IMPACTS ===")]
		[Tooltip("Impact when NPC fully detects player (typically negative = shame)")]
		public EmotionImpactSO FullDetection;

		// Token: 0x04006DC5 RID: 28101
		[Tooltip("Per-second impact during VISUAL pre-detection")]
		public EmotionImpactSO VisualPreDetection;

		// Token: 0x04006DC6 RID: 28102
		[Tooltip("Per-second impact during AUDIO pre-detection")]
		public EmotionImpactSO AudioPreDetection;

		// Token: 0x04006DC7 RID: 28103
		[Header("=== PLAYER CHOICE IMPACTS ===")]
		[Tooltip("Impact when player chooses SHAME action (negative value)")]
		public EmotionImpactSO ShameChoice;

		// Token: 0x04006DC8 RID: 28104
		[Tooltip("Impact when player chooses EXPOSE action (positive value)")]
		public EmotionImpactSO ExposeChoice;

		// Token: 0x04006DC9 RID: 28105
		[Tooltip("Impact when player chooses FLEE action")]
		public EmotionImpactSO FleeChoice;

		// Token: 0x04006DCA RID: 28106
		[Header("=== ESCAPE IMPACTS ===")]
		[Tooltip("Impact when player successfully escapes detection")]
		public EmotionImpactSO SuccessfulEscape;

		// Token: 0x04006DCB RID: 28107
		[Header("=== PRE-DETECTION CURVE ===")]
		[Tooltip("Curve for pre-detection scaling (X = attention 0-1, Y = multiplier)")]
		public AnimationCurve PreDetectionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04006DCC RID: 28108
		[Header("=== FLASH / HIDE-FLASH IMPACTS ===")]
		[Tooltip("Per-second impact while holding a HideFlash cover item (typically positive = exhibitionist thrill)")]
		public EmotionImpactSO FlashCoverActive;

		// Token: 0x04006DCD RID: 28109
		[Tooltip("Impact when executing a Flash action (typically positive = exhibitionist)")]
		public EmotionImpactSO FlashExecution;

		// Token: 0x04006DCE RID: 28110
		[Tooltip("Impact when unequipping HideFlash item without flashing (small positive or neutral)")]
		public EmotionImpactSO FlashCoverUnequip;

		// Token: 0x04006DCF RID: 28111
		[Tooltip("Durata (s) della finestra HornySupreme forzata dal flash col cover item: FlashExecution porta il balance al supreme, e allo scadere della finestra torna al valore PRE-flash. Ogni nuovo flash ri-arma il timer. Default 15.")]
		public float FlashSupremeDuration = 15f;
	}
}
