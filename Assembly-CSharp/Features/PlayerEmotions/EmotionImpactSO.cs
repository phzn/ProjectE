using System;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E03 RID: 3587
	[CreateAssetMenu(fileName = "NewEmotionImpact", menuName = "Player Emotions/Emotion Impact")]
	public class EmotionImpactSO : ScriptableObject
	{
		// Token: 0x06006AD4 RID: 27348 RVA: 0x002ADC18 File Offset: 0x002ABE18
		public EmotionImpactData ToImpactData()
		{
			return new EmotionImpactData
			{
				Value = this.Value,
				ImpactType = this.ImpactType,
				ApplicationMode = this.ApplicationMode,
				Duration = this.Duration,
				NakedMultiplier = this.NakedMultiplier,
				UseProximityMultiplier = this.UseProximityMultiplier,
				CloseProximityDistance = this.CloseProximityDistance,
				CloseProximityMultiplier = this.CloseProximityMultiplier
			};
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x002ADC8C File Offset: 0x002ABE8C
		public EmotionImpactData CalculateImpact(bool isNaked = false, float distance = 0f, float curveInput = 1f)
		{
			float num = this.Value;
			if (this.ImpactType == EmotionImpactType.Delta)
			{
				if (this.UseCurveModifier)
				{
					float num2 = this.ImpactCurve.Evaluate(Mathf.Clamp01(curveInput));
					num *= num2;
				}
				if (isNaked && this.NakedMultiplier != 1f)
				{
					num *= this.NakedMultiplier;
				}
				if (this.UseProximityMultiplier && distance > 0f && distance < this.CloseProximityDistance)
				{
					num *= this.CloseProximityMultiplier;
				}
			}
			return new EmotionImpactData
			{
				Value = num,
				ImpactType = this.ImpactType,
				ApplicationMode = this.ApplicationMode,
				Duration = this.Duration
			};
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x002ADD31 File Offset: 0x002ABF31
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.ImpactId))
			{
				this.ImpactId = base.name;
			}
			if (string.IsNullOrEmpty(this.DisplayName))
			{
				this.DisplayName = base.name;
			}
		}

		// Token: 0x04006D89 RID: 28041
		[Header("Identification")]
		[Tooltip("Unique identifier for this impact")]
		public string ImpactId;

		// Token: 0x04006D8A RID: 28042
		[Tooltip("Display name for debugging/UI")]
		public string DisplayName;

		// Token: 0x04006D8B RID: 28043
		[TextArea(2, 4)]
		[Tooltip("Description of when this impact is applied")]
		public string Description;

		// Token: 0x04006D8C RID: 28044
		[Header("Source Settings")]
		[Tooltip("Category of the source that triggers this impact")]
		public EmotionSourceCategory Category = EmotionSourceCategory.System;

		// Token: 0x04006D8D RID: 28045
		[Tooltip("Priority level for conflict resolution")]
		public EmotionPriority Priority = EmotionPriority.Normal;

		// Token: 0x04006D8E RID: 28046
		[Header("Impact Value")]
		[Tooltip("Value for the impact. Negative = towards Shame (-100), Positive = towards Exhibition (+100)")]
		[Range(-100f, 100f)]
		public float Value;

		// Token: 0x04006D8F RID: 28047
		[Tooltip("Delta: Add/subtract from current. SetTo: Set to absolute value")]
		public EmotionImpactType ImpactType;

		// Token: 0x04006D90 RID: 28048
		[Header("Application Mode")]
		[Tooltip("How to apply this impact over time")]
		public EmotionApplicationMode ApplicationMode;

		// Token: 0x04006D91 RID: 28049
		[Tooltip("Duration for OverDuration mode (seconds)")]
		[Range(0f, 60f)]
		public float Duration;

		// Token: 0x04006D92 RID: 28050
		[Header("Contextual Modifiers (Delta only)")]
		[Tooltip("Multiply impact when player is naked")]
		[Range(1f, 5f)]
		public float NakedMultiplier = 1f;

		// Token: 0x04006D93 RID: 28051
		[Tooltip("Enable proximity-based multiplier")]
		public bool UseProximityMultiplier;

		// Token: 0x04006D94 RID: 28052
		[Tooltip("Distance threshold for close proximity (meters)")]
		[Range(1f, 20f)]
		public float CloseProximityDistance = 3f;

		// Token: 0x04006D95 RID: 28053
		[Tooltip("Multiplier when within close proximity")]
		[Range(1f, 5f)]
		public float CloseProximityMultiplier = 1.5f;

		// Token: 0x04006D96 RID: 28054
		[Header("Curve Modifiers (Optional, Delta only)")]
		[Tooltip("If set, multiplies impact based on an input value (0-1). Useful for attention-based scaling.")]
		public bool UseCurveModifier;

		// Token: 0x04006D97 RID: 28055
		[Tooltip("Curve for scaling impact (X = input 0-1, Y = multiplier)")]
		public AnimationCurve ImpactCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
