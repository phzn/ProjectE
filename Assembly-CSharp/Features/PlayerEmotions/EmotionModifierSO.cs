using System;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E09 RID: 3593
	[CreateAssetMenu(fileName = "NewEmotionModifier", menuName = "Player Emotions/Emotion Modifier")]
	public class EmotionModifierSO : ScriptableObject
	{
		// Token: 0x06006B20 RID: 27424 RVA: 0x002AEF3C File Offset: 0x002AD13C
		public EmotionModifier CreateInstance(string sourceId = null)
		{
			return new EmotionModifier
			{
				ModifierId = this.ModifierId,
				DisplayName = this.DisplayName,
				SourceId = (sourceId ?? this.ModifierId),
				ImpactMultiplier = this.ImpactMultiplier,
				DecayMultiplier = this.DecayMultiplier,
				Duration = this.Duration,
				StartTime = Time.time,
				CanStack = this.CanStack,
				MaxStacks = this.MaxStacks
			};
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x002AEFBE File Offset: 0x002AD1BE
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.ModifierId))
			{
				this.ModifierId = base.name;
			}
			if (string.IsNullOrEmpty(this.DisplayName))
			{
				this.DisplayName = base.name;
			}
		}

		// Token: 0x04006DBC RID: 28092
		[Header("Identification")]
		public string ModifierId;

		// Token: 0x04006DBD RID: 28093
		public string DisplayName;

		// Token: 0x04006DBE RID: 28094
		[TextArea(2, 4)]
		public string Description;

		// Token: 0x04006DBF RID: 28095
		[Header("Impact Multiplier")]
		[Tooltip("Multiplier for all emotion balance changes (1 = normal)")]
		[Range(0f, 5f)]
		public float ImpactMultiplier = 1f;

		// Token: 0x04006DC0 RID: 28096
		[Header("Decay Multiplier")]
		[Tooltip("Multiplier for decay rate (1 = normal, 0 = no decay)")]
		[Range(0f, 5f)]
		public float DecayMultiplier = 1f;

		// Token: 0x04006DC1 RID: 28097
		[Header("Duration")]
		[Tooltip("Duration in seconds (0 = permanent until removed)")]
		public float Duration;

		// Token: 0x04006DC2 RID: 28098
		[Header("Stacking")]
		public bool CanStack;

		// Token: 0x04006DC3 RID: 28099
		public int MaxStacks;
	}
}
