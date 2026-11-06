using System;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E08 RID: 3592
	[Serializable]
	public class EmotionModifier
	{
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x06006B1B RID: 27419 RVA: 0x002AEDA6 File Offset: 0x002ACFA6
		public bool IsExpired
		{
			get
			{
				return this.Duration > 0f && Time.time - this.StartTime >= this.Duration;
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06006B1C RID: 27420 RVA: 0x002AEDCE File Offset: 0x002ACFCE
		public float RemainingDuration
		{
			get
			{
				if (this.Duration <= 0f)
				{
					return 0f;
				}
				return Mathf.Max(0f, this.Duration - (Time.time - this.StartTime));
			}
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x002AEE00 File Offset: 0x002AD000
		public EmotionModifier()
		{
			this.ModifierId = Guid.NewGuid().ToString();
			this.ImpactMultiplier = 1f;
			this.DecayMultiplier = 1f;
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x002AEE58 File Offset: 0x002AD058
		public EmotionModifier(string id, string displayName, float duration = 0f)
		{
			this.ModifierId = id;
			this.DisplayName = displayName;
			this.Duration = duration;
			this.StartTime = Time.time;
			this.ImpactMultiplier = 1f;
			this.DecayMultiplier = 1f;
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x002AEEB8 File Offset: 0x002AD0B8
		public EmotionImpactData ApplyToImpact(EmotionImpactData impact)
		{
			if (impact.ImpactType != EmotionImpactType.Delta)
			{
				return impact;
			}
			return new EmotionImpactData
			{
				Value = impact.Value * this.ImpactMultiplier,
				ImpactType = impact.ImpactType,
				ApplicationMode = impact.ApplicationMode,
				Duration = impact.Duration,
				NakedMultiplier = impact.NakedMultiplier,
				UseProximityMultiplier = impact.UseProximityMultiplier,
				CloseProximityDistance = impact.CloseProximityDistance,
				CloseProximityMultiplier = impact.CloseProximityMultiplier
			};
		}

		// Token: 0x04006DB3 RID: 28083
		[Tooltip("Unique identifier for this modifier")]
		public string ModifierId;

		// Token: 0x04006DB4 RID: 28084
		[Tooltip("Display name for debugging/UI")]
		public string DisplayName;

		// Token: 0x04006DB5 RID: 28085
		[Tooltip("Source that created this modifier")]
		public string SourceId;

		// Token: 0x04006DB6 RID: 28086
		[Header("Impact Multiplier")]
		[Tooltip("Multiplier for all emotion balance changes (1 = normal, 0 = immune, 2 = double)")]
		[Range(0f, 5f)]
		public float ImpactMultiplier = 1f;

		// Token: 0x04006DB7 RID: 28087
		[Header("Decay Multiplier")]
		[Tooltip("Multiplier for decay rate (1 = normal, 0 = no decay, 2 = double decay)")]
		[Range(0f, 5f)]
		public float DecayMultiplier = 1f;

		// Token: 0x04006DB8 RID: 28088
		[Header("Duration")]
		[Tooltip("Duration in seconds (0 or negative = permanent until removed)")]
		public float Duration;

		// Token: 0x04006DB9 RID: 28089
		[Tooltip("Time when this modifier was applied")]
		[HideInInspector]
		public float StartTime;

		// Token: 0x04006DBA RID: 28090
		[Header("Stacking")]
		[Tooltip("If true, multiple instances of this modifier can stack")]
		public bool CanStack;

		// Token: 0x04006DBB RID: 28091
		[Tooltip("Maximum number of stacks (0 = unlimited)")]
		public int MaxStacks;
	}
}
