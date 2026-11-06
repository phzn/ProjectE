using System;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FEC RID: 4076
	public static class AdaptiveHeartbeatModel
	{
		// Token: 0x06007B50 RID: 31568 RVA: 0x00325ECC File Offset: 0x003240CC
		public static float CalculateCoverage01(bool fullyClothed, bool topPhysicallyCovered, bool bottomPhysicallyCovered, bool topDecentlyCovered, bool bottomDecentlyCovered)
		{
			if (fullyClothed)
			{
				return 1f;
			}
			float num = (topPhysicallyCovered ? 0.5f : 0f) + (bottomPhysicallyCovered ? 0.5f : 0f);
			float num2 = (topDecentlyCovered ? 0.5f : 0f) + (bottomDecentlyCovered ? 0.5f : 0f);
			return Mathf.Clamp01(num * 0.7f + num2 * 0.3f);
		}

		// Token: 0x06007B51 RID: 31569 RVA: 0x00325F38 File Offset: 0x00324138
		public static Vector2 ResolveBaseRange(bool coverageAvailable, bool fullyClothed, float coverage01, Vector2 fullyClothedRange, Vector2 nakedRange, Vector2 partiallyCoveredFloorRange)
		{
			if (!coverageAvailable || fullyClothed)
			{
				return AdaptiveHeartbeatModel.Ordered(fullyClothedRange);
			}
			Vector2 vector = AdaptiveHeartbeatModel.Ordered(nakedRange);
			Vector2 vector2 = AdaptiveHeartbeatModel.Ordered(partiallyCoveredFloorRange);
			float num = 1f - Mathf.Clamp01(coverage01);
			return new Vector2(Mathf.Lerp(vector2.x, vector.x, num), Mathf.Lerp(vector2.y, vector.y, num));
		}

		// Token: 0x06007B52 RID: 31570 RVA: 0x00325F9C File Offset: 0x0032419C
		public static float VariableBpm(Vector2 range, float time, float phase)
		{
			range = AdaptiveHeartbeatModel.Ordered(range);
			float num = 0.5f + 0.33f * Mathf.Sin(time * 0.91f + phase) + 0.17f * Mathf.Sin(time * 0.37f + phase * 1.73f + 1.1f);
			return Mathf.Lerp(range.x, range.y, Mathf.Clamp01(num));
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x00326004 File Offset: 0x00324204
		public static CoverPoseHeartbeatEffect ClassifyCoverPose(string label)
		{
			if (string.IsNullOrWhiteSpace(label))
			{
				return CoverPoseHeartbeatEffect.Calming;
			}
			string text = label.Trim().ToLowerInvariant();
			if (text.Contains("stimulation"))
			{
				return CoverPoseHeartbeatEffect.Stimulation;
			}
			if (text.Contains("expose"))
			{
				return CoverPoseHeartbeatEffect.Exciting;
			}
			return CoverPoseHeartbeatEffect.Calming;
		}

		// Token: 0x06007B54 RID: 31572 RVA: 0x00326048 File Offset: 0x00324248
		public static float SpikeFloorBpm(float elapsed, float duration, float peakBpm, float settledBpm)
		{
			float num = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, duration));
			num = num * num * (3f - 2f * num);
			return Mathf.Lerp(peakBpm, settledBpm, num);
		}

		// Token: 0x06007B55 RID: 31573 RVA: 0x00326082 File Offset: 0x00324282
		public static float CappedNpcAttention01(float attention01, float cap01)
		{
			return Mathf.Min(Mathf.Clamp01(attention01), Mathf.Clamp01(cap01));
		}

		// Token: 0x06007B56 RID: 31574 RVA: 0x00326095 File Offset: 0x00324295
		private static Vector2 Ordered(Vector2 range)
		{
			if (range.x > range.y)
			{
				return new Vector2(range.y, range.x);
			}
			return range;
		}
	}
}
