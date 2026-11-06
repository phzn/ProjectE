using System;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E10 RID: 3600
	[Serializable]
	public class EmotionImpactData
	{
		// Token: 0x06006B24 RID: 27428 RVA: 0x002AF042 File Offset: 0x002AD242
		public EmotionImpactData()
		{
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x002AF06C File Offset: 0x002AD26C
		public EmotionImpactData(float delta)
		{
			this.Value = delta;
			this.ImpactType = EmotionImpactType.Delta;
			this.ApplicationMode = EmotionApplicationMode.Instant;
			this.NakedMultiplier = 1f;
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x002AF0C0 File Offset: 0x002AD2C0
		public EmotionImpactData(float value, EmotionImpactType type)
		{
			this.Value = value;
			this.ImpactType = type;
			this.ApplicationMode = EmotionApplicationMode.Instant;
			this.NakedMultiplier = 1f;
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x002AF114 File Offset: 0x002AD314
		public EmotionImpactData ApplyModifiers(bool isNaked, float distance = 0f)
		{
			EmotionImpactData emotionImpactData = new EmotionImpactData
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
			if (this.ImpactType != EmotionImpactType.Delta)
			{
				return emotionImpactData;
			}
			if (isNaked && this.NakedMultiplier != 1f)
			{
				emotionImpactData.Value *= this.NakedMultiplier;
			}
			if (this.UseProximityMultiplier && distance > 0f && distance < this.CloseProximityDistance)
			{
				emotionImpactData.Value *= this.CloseProximityMultiplier;
			}
			return emotionImpactData;
		}

		// Token: 0x04006DE8 RID: 28136
		[Header("Impact Value")]
		[Tooltip("Value for the impact. Negative = towards Shame, Positive = towards Exhibition")]
		[Range(-100f, 100f)]
		public float Value;

		// Token: 0x04006DE9 RID: 28137
		[Tooltip("How to apply this value: Delta (add/subtract) or SetTo (absolute)")]
		public EmotionImpactType ImpactType;

		// Token: 0x04006DEA RID: 28138
		[Header("Application Settings")]
		[Tooltip("How to apply this impact over time")]
		public EmotionApplicationMode ApplicationMode;

		// Token: 0x04006DEB RID: 28139
		[Tooltip("Duration for OverDuration mode (seconds)")]
		public float Duration;

		// Token: 0x04006DEC RID: 28140
		[Header("Modifiers")]
		[Tooltip("Multiply impact when player is naked (only for Delta type)")]
		public float NakedMultiplier = 1f;

		// Token: 0x04006DED RID: 28141
		[Tooltip("Multiply impact based on proximity (if applicable)")]
		public bool UseProximityMultiplier;

		// Token: 0x04006DEE RID: 28142
		[Tooltip("Distance threshold for close proximity bonus")]
		public float CloseProximityDistance = 3f;

		// Token: 0x04006DEF RID: 28143
		[Tooltip("Multiplier when within close proximity")]
		public float CloseProximityMultiplier = 1.5f;
	}
}
