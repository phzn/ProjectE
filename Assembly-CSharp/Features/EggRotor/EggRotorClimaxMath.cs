using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200121A RID: 4634
	public static class EggRotorClimaxMath
	{
		// Token: 0x06008F3A RID: 36666 RVA: 0x003A9324 File Offset: 0x003A7524
		public static float FillRate(EggRotorClimaxSettings settings, int eggLevel, EggRotorPlacement placement, bool nippleVibrating, int nippleLevel)
		{
			if (settings == null || eggLevel <= 0 || !EggRotorIds.Stimulates(placement))
			{
				return 0f;
			}
			float[] fillPerSecond = settings.fillPerSecond;
			if (fillPerSecond == null || fillPerSecond.Length == 0)
			{
				return 0f;
			}
			int num = Mathf.Min(EggRotorIds.ClampLevel(eggLevel), fillPerSecond.Length) - 1;
			float num2 = EggRotorClimaxMath.NonNegative(fillPerSecond[num]);
			if (placement == EggRotorPlacement.Outside)
			{
				num2 *= EggRotorClimaxMath.NonNegative(settings.outsideFactor);
			}
			else if (placement == EggRotorPlacement.Glans)
			{
				num2 *= EggRotorClimaxMath.NonNegative(settings.glansFactor);
			}
			return num2 * EggRotorClimaxMath.ComboMultiplier(settings, nippleVibrating, nippleLevel);
		}

		// Token: 0x06008F3B RID: 36667 RVA: 0x003A93A5 File Offset: 0x003A75A5
		public static float ComboMultiplier(EggRotorClimaxSettings settings, bool nippleVibrating, int nippleLevel)
		{
			if (settings == null || !nippleVibrating || nippleLevel <= 0)
			{
				return 1f;
			}
			return 1f + EggRotorClimaxMath.NonNegative(settings.nippleComboPerLevel) * (float)EggRotorIds.ClampLevel(nippleLevel);
		}

		// Token: 0x06008F3C RID: 36668 RVA: 0x003A93D0 File Offset: 0x003A75D0
		public static float SecondsToFill(float ratePerSecond)
		{
			if (ratePerSecond <= 0f || float.IsInfinity(ratePerSecond))
			{
				return float.PositiveInfinity;
			}
			return 1f / ratePerSecond;
		}

		// Token: 0x06008F3D RID: 36669 RVA: 0x003A93EF File Offset: 0x003A75EF
		public static bool ShouldFeed(bool stimulating, bool meterLocked, bool foreignDriver, bool refractory, float meterValue)
		{
			return stimulating && !meterLocked && !foreignDriver && !refractory && meterValue < 0.9999f;
		}

		// Token: 0x06008F3E RID: 36670 RVA: 0x003A9408 File Offset: 0x003A7608
		public static Quaternion SquirtRotation(Vector3 direction)
		{
			Vector3 vector = ((EggRotorPlacementMath.IsFinite(direction) && direction.sqrMagnitude > 1E-08f) ? direction.normalized : EggRotorClimaxSettings.DefaultSquirtDirection.normalized);
			Vector3 vector2 = ((Mathf.Abs(vector.y) > 0.99f) ? Vector3.forward : Vector3.up);
			return Quaternion.LookRotation(vector, vector2);
		}

		// Token: 0x06008F3F RID: 36671 RVA: 0x003A9466 File Offset: 0x003A7666
		public static float Duration(EggRotorClimaxSettings settings)
		{
			if (settings == null)
			{
				return 4f;
			}
			return Mathf.Max(0.5f, EggRotorClimaxMath.Finite(settings.durationSeconds, 4f));
		}

		// Token: 0x06008F40 RID: 36672 RVA: 0x003A948C File Offset: 0x003A768C
		public static int FillPulseTimes(EggRotorClimaxSettings settings, float[] times)
		{
			if (settings == null || times == null)
			{
				return 0;
			}
			int num = Mathf.Min(EggRotorClimaxMath.PulseCount(settings), times.Length);
			float num2 = EggRotorClimaxMath.NonNegative(settings.firstPulseSeconds);
			float num3 = EggRotorClimaxMath.PulseInterval(settings);
			float num4 = EggRotorClimaxMath.PulseGrowth(settings);
			for (int i = 0; i < num; i++)
			{
				times[i] = num2;
				num2 += num3;
				num3 *= num4;
			}
			return num;
		}

		// Token: 0x06008F41 RID: 36673 RVA: 0x003A94E8 File Offset: 0x003A76E8
		public static float PulseAmplitude(EggRotorClimaxSettings settings, int index)
		{
			if (settings == null || index < 0)
			{
				return 0f;
			}
			return Mathf.Pow(Mathf.Clamp01(EggRotorClimaxMath.Finite(settings.pulseAmplitudeDecay, 0.82f)), (float)index);
		}

		// Token: 0x06008F42 RID: 36674 RVA: 0x003A9514 File Offset: 0x003A7714
		public static float Pulse(float t, EggRotorClimaxSettings settings)
		{
			if (settings == null || t < 0f || float.IsInfinity(t))
			{
				return 0f;
			}
			int num = EggRotorClimaxMath.PulseCount(settings);
			float num2 = EggRotorClimaxMath.NonNegative(settings.firstPulseSeconds);
			float num3 = EggRotorClimaxMath.PulseInterval(settings);
			float num4 = EggRotorClimaxMath.PulseGrowth(settings);
			float num5 = Mathf.Clamp01(EggRotorClimaxMath.Finite(settings.pulseAmplitudeDecay, 0.82f));
			float num6 = Mathf.Max(0.005f, EggRotorClimaxMath.Finite(settings.pulseAttackSeconds, 0.05f));
			float num7 = Mathf.Max(0.01f, EggRotorClimaxMath.Finite(settings.pulseDecaySeconds, 0.18f));
			float num8 = 1f;
			float num9 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num10 = t - num2;
				if (num10 < 0f)
				{
					break;
				}
				num9 += num8 * ((num10 < num6) ? EggRotorClimaxMath.Smooth01(num10 / num6) : Mathf.Exp(-(num10 - num6) / num7));
				num2 += num3;
				num3 *= num4;
				num8 *= num5;
			}
			return Mathf.Min(num9, 1.5f);
		}

		// Token: 0x06008F43 RID: 36675 RVA: 0x003A961C File Offset: 0x003A781C
		public static float Envelope(float t, EggRotorClimaxSettings settings)
		{
			if (settings == null)
			{
				return 0f;
			}
			float num = EggRotorClimaxMath.Duration(settings);
			if (t <= 0f || t >= num)
			{
				return 0f;
			}
			float num2 = num * 0.5f;
			float num3 = Mathf.Clamp(EggRotorClimaxMath.NonNegative(settings.fadeInSeconds), 0f, num2);
			float num4 = Mathf.Clamp(EggRotorClimaxMath.NonNegative(settings.fadeOutSeconds), 0f, num2);
			float num5 = 1f;
			if (num3 > 0f && t < num3)
			{
				num5 = EggRotorClimaxMath.Smooth01(t / num3);
			}
			float num6 = num - t;
			if (num4 > 0f && num6 < num4)
			{
				num5 = Mathf.Min(num5, EggRotorClimaxMath.Smooth01(num6 / num4));
			}
			return num5;
		}

		// Token: 0x06008F44 RID: 36676 RVA: 0x003A96C4 File Offset: 0x003A78C4
		public static float TremorEnvelope(float t, EggRotorClimaxSettings settings)
		{
			float num = EggRotorClimaxMath.Duration(settings);
			return EggRotorClimaxMath.Envelope(t, settings) * Mathf.Lerp(1f, 0.3f, EggRotorClimaxMath.Smooth01((t - 0.4f * num) / (0.5f * num)));
		}

		// Token: 0x06008F45 RID: 36677 RVA: 0x003A9705 File Offset: 0x003A7905
		public static EggRotorTremblePose EvaluatePose(float t, EggRotorClimaxSettings settings, float lowerWeight, float upperWeight, float seed)
		{
			return EggRotorClimaxMath.EvaluatePose(t, settings, EggRotorTrembleWeights.Split(EggRotorClimaxMath.Finite(lowerWeight, 0f), EggRotorClimaxMath.Finite(upperWeight, 0f)), seed);
		}

		// Token: 0x06008F46 RID: 36678 RVA: 0x003A972C File Offset: 0x003A792C
		public static EggRotorTremblePose EvaluatePose(float t, EggRotorClimaxSettings settings, EggRotorTrembleWeights weights, float seed)
		{
			EggRotorTremblePose eggRotorTremblePose = default(EggRotorTremblePose);
			if (settings == null)
			{
				return eggRotorTremblePose;
			}
			weights = weights.Clamped();
			eggRotorTremblePose.Weights = weights;
			float legs = weights.Legs;
			float pelvis = weights.Pelvis;
			float torso = weights.Torso;
			float head = weights.Head;
			float arms = weights.Arms;
			float num = EggRotorClimaxMath.Envelope(t, settings);
			eggRotorTremblePose.Envelope = num;
			if (num <= 0f || !weights.Any)
			{
				return eggRotorTremblePose;
			}
			float num2 = EggRotorClimaxMath.Pulse(t, settings) * num;
			eggRotorTremblePose.Pulse = num2;
			float num3 = EggRotorClimaxMath.NonNegative(settings.tremorDegrees) * EggRotorClimaxMath.TremorEnvelope(t, settings);
			float num4 = Mathf.Clamp(EggRotorClimaxMath.Finite(settings.tremorHz, 10f), 0.5f, 30f);
			float num5 = EggRotorClimaxMath.NonNegative(settings.breathDegrees) * num * Mathf.Sin(6.2831855f * EggRotorClimaxMath.NonNegative(settings.breathHz) * t);
			seed = EggRotorClimaxMath.Finite(seed, 0f);
			float num6 = EggRotorClimaxMath.NonNegative(settings.kneeDipDegrees) * num + EggRotorClimaxMath.NonNegative(settings.pulseKneeDegrees) * num2;
			eggRotorTremblePose.LeftKneeFlex = legs * Mathf.Max(0f, num6 + num3 * 1f * EggRotorClimaxMath.Noise(seed + 1.3f, t, num4));
			eggRotorTremblePose.RightKneeFlex = legs * Mathf.Max(0f, num6 + num3 * 1f * EggRotorClimaxMath.Noise(seed + 2.7f, t, num4 * 1.13f));
			eggRotorTremblePose.KneesTogether = legs * EggRotorClimaxMath.NonNegative(settings.kneesTogetherDegrees) * num;
			float num7 = pelvis * EggRotorClimaxMath.NonNegative(settings.pelvisPulseDegrees) * num2;
			eggRotorTremblePose.HipsPitch = -num7 + pelvis * num3 * 0.5f * EggRotorClimaxMath.Noise(seed + 4.1f, t, num4 * 0.91f);
			eggRotorTremblePose.HipsRoll = pelvis * num3 * 0.5f * EggRotorClimaxMath.Noise(seed + 5.9f, t, num4 * 1.07f);
			float num8 = EggRotorClimaxMath.Finite(settings.torsoCurlDegrees, 5f) * num2;
			float num9 = EggRotorClimaxMath.Finite(settings.headPulseDegrees, -5f) * num2;
			eggRotorTremblePose.SpinePitch = 0.5f * num7 + torso * (0.5f * num8 + num3 * 0.6f * EggRotorClimaxMath.Noise(seed + 7.7f, t, num4 * 0.87f));
			eggRotorTremblePose.SpineRoll = torso * num3 * 0.6f * EggRotorClimaxMath.Noise(seed + 9.1f, t, num4 * 0.95f);
			eggRotorTremblePose.ChestPitch = 0.5f * num7 + torso * (0.5f * num8 + num5 + num3 * 0.6f * EggRotorClimaxMath.Noise(seed + 10.3f, t, num4 * 1.03f));
			eggRotorTremblePose.ChestRoll = torso * num3 * 0.6f * EggRotorClimaxMath.Noise(seed + 11.9f, t, num4 * 0.83f);
			eggRotorTremblePose.NeckPitch = head * (0.5f * num9 + num3 * 0.5f * EggRotorClimaxMath.Noise(seed + 13.1f, t, num4 * 0.79f));
			eggRotorTremblePose.HeadPitch = head * (0.5f * num9 + num3 * 0.5f * EggRotorClimaxMath.Noise(seed + 14.7f, t, num4 * 0.97f));
			eggRotorTremblePose.HeadRoll = head * num3 * 0.5f * EggRotorClimaxMath.Noise(seed + 16.3f, t, num4 * 1.11f);
			eggRotorTremblePose.LeftArmPitch = arms * num3 * 1.2f * EggRotorClimaxMath.Noise(seed + 17.9f, t, num4 * 1.05f);
			eggRotorTremblePose.LeftArmRoll = arms * num3 * 1.2f * EggRotorClimaxMath.Noise(seed + 19.3f, t, num4 * 0.89f);
			eggRotorTremblePose.RightArmPitch = arms * num3 * 1.2f * EggRotorClimaxMath.Noise(seed + 20.9f, t, num4 * 1.09f);
			eggRotorTremblePose.RightArmRoll = arms * num3 * 1.2f * EggRotorClimaxMath.Noise(seed + 22.1f, t, num4 * 0.93f);
			eggRotorTremblePose.LeftHandPitch = arms * num3 * 1.8f * EggRotorClimaxMath.Noise(seed + 23.7f, t, num4 * 1.21f);
			eggRotorTremblePose.LeftHandRoll = arms * num3 * 1.8f * EggRotorClimaxMath.Noise(seed + 25.3f, t, num4 * 1.17f);
			eggRotorTremblePose.RightHandPitch = arms * num3 * 1.8f * EggRotorClimaxMath.Noise(seed + 26.9f, t, num4 * 1.25f);
			eggRotorTremblePose.RightHandRoll = arms * num3 * 1.8f * EggRotorClimaxMath.Noise(seed + 28.1f, t, num4 * 1.19f);
			return eggRotorTremblePose;
		}

		// Token: 0x06008F47 RID: 36679 RVA: 0x003A9BD8 File Offset: 0x003A7DD8
		public static EggRotorTrembleWeights EmoteWeights(in EggRotorPoseSample pose, float weight)
		{
			weight = Mathf.Clamp01(EggRotorClimaxMath.Finite(weight, 0f));
			Vector3 up = pose.Up;
			EggRotorTrembleWeights eggRotorTrembleWeights;
			if (weight <= 0f || !EggRotorPlacementMath.IsFinite(up) || up.sqrMagnitude < 1E-06f)
			{
				eggRotorTrembleWeights = default(EggRotorTrembleWeights);
				return eggRotorTrembleWeights;
			}
			up.Normalize();
			Vector3 ground = pose.Ground;
			float num = EggRotorClimaxMath.Height(pose.Hips, ground, up);
			float num2 = (pose.HasLegs ? (0.5f * (Vector3.Distance(pose.LeftThigh, pose.LeftKnee) + Vector3.Distance(pose.LeftKnee, pose.LeftFoot) + Vector3.Distance(pose.RightThigh, pose.RightKnee) + Vector3.Distance(pose.RightKnee, pose.RightFoot))) : 0f);
			if (num2 <= 0.001f || float.IsInfinity(num2) || float.IsNaN(num) || float.IsInfinity(num))
			{
				return EggRotorTrembleWeights.Split(0f, 0.5f * weight);
			}
			float num3 = 0.15f * num2;
			bool flag = pose.HasHead && EggRotorClimaxMath.Height(pose.Head, ground, up) < num - num3;
			bool flag2 = EggRotorClimaxMath.Height(pose.LeftFoot, ground, up) > num + num3 && EggRotorClimaxMath.Height(pose.RightFoot, ground, up) > num + num3;
			if (flag && flag2)
			{
				eggRotorTrembleWeights = default(EggRotorTrembleWeights);
				return eggRotorTrembleWeights;
			}
			bool flag3 = num > 0.8f * num2 && EggRotorClimaxMath.IsStandingLeg(pose.LeftThigh, pose.LeftKnee, pose.LeftFoot, ground, up, num2) && EggRotorClimaxMath.IsStandingLeg(pose.RightThigh, pose.RightKnee, pose.RightFoot, ground, up, num2);
			float num4 = 0.3f * num2;
			bool flag4 = (pose.HasLeftHand && EggRotorClimaxMath.Height(pose.LeftHand, ground, up) < num4) || (pose.HasRightHand && EggRotorClimaxMath.Height(pose.RightHand, ground, up) < num4);
			float num5 = 1f;
			if (pose.HasHead)
			{
				Vector3 vector = pose.Head - pose.Hips;
				if (vector.sqrMagnitude > 1E-08f)
				{
					num5 = Vector3.Dot(vector.normalized, up);
				}
			}
			float num6 = (flag4 ? 0f : ((num5 >= 0.7f) ? 1f : ((num5 >= 0.3f) ? 0.5f : 0f)));
			eggRotorTrembleWeights = new EggRotorTrembleWeights
			{
				Legs = ((flag3 && !flag4) ? weight : 0f),
				Pelvis = ((flag4 || num5 < 0.3f) ? 0f : (flag3 ? weight : (weight * 0.5f))),
				Torso = num6 * weight,
				Head = weight,
				Arms = (flag4 ? 0f : weight)
			};
			return eggRotorTrembleWeights;
		}

		// Token: 0x06008F48 RID: 36680 RVA: 0x003A9EA0 File Offset: 0x003A80A0
		private static bool IsStandingLeg(Vector3 thigh, Vector3 knee, Vector3 foot, Vector3 ground, Vector3 up, float legLength)
		{
			if (EggRotorClimaxMath.Height(foot, ground, up) >= 0.3f * legLength)
			{
				return false;
			}
			Vector3 vector = knee - thigh;
			Vector3 vector2 = foot - knee;
			return vector.sqrMagnitude >= 1E-08f && vector2.sqrMagnitude >= 1E-08f && Vector3.Dot(vector.normalized, -up) > 0.8f && Vector3.Dot(vector2.normalized, -up) > 0.8f;
		}

		// Token: 0x06008F49 RID: 36681 RVA: 0x003A9F26 File Offset: 0x003A8126
		private static float Height(Vector3 point, Vector3 ground, Vector3 up)
		{
			return Vector3.Dot(point - ground, up);
		}

		// Token: 0x06008F4A RID: 36682 RVA: 0x003A9F38 File Offset: 0x003A8138
		public static float LegRise(Vector3 hipsPivot, Vector3 thigh, Vector3 knee, Vector3 ankle, Quaternion hipsDelta, Quaternion thighNet, Quaternion shinNet, Vector3 up)
		{
			return Vector3.Dot(hipsPivot + hipsDelta * (thigh - hipsPivot) + thighNet * (knee - thigh) + shinNet * (ankle - knee) - ankle, up);
		}

		// Token: 0x06008F4B RID: 36683 RVA: 0x003A9F8C File Offset: 0x003A818C
		public static float Smooth01(float x)
		{
			x = EggRotorClimaxMath.Finite(x, 0f);
			x = Mathf.Clamp01(x);
			return x * x * (3f - 2f * x);
		}

		// Token: 0x06008F4C RID: 36684 RVA: 0x003A9FB4 File Offset: 0x003A81B4
		private static int PulseCount(EggRotorClimaxSettings settings)
		{
			return Mathf.Clamp(settings.pulseCount, 0, 12);
		}

		// Token: 0x06008F4D RID: 36685 RVA: 0x003A9FC4 File Offset: 0x003A81C4
		private static float PulseInterval(EggRotorClimaxSettings settings)
		{
			return Mathf.Max(0.05f, EggRotorClimaxMath.Finite(settings.pulseIntervalSeconds, 0.45f));
		}

		// Token: 0x06008F4E RID: 36686 RVA: 0x003A9FE0 File Offset: 0x003A81E0
		private static float PulseGrowth(EggRotorClimaxSettings settings)
		{
			return Mathf.Clamp(EggRotorClimaxMath.Finite(settings.pulseIntervalGrowth, 1.15f), 0.5f, 2f);
		}

		// Token: 0x06008F4F RID: 36687 RVA: 0x003AA001 File Offset: 0x003A8201
		private static float Noise(float seed, float t, float hz)
		{
			return Mathf.Clamp((Mathf.PerlinNoise(seed, t * hz) - 0.5f) * 2f, -1f, 1f);
		}

		// Token: 0x06008F50 RID: 36688 RVA: 0x001B3A41 File Offset: 0x001B1C41
		private static float Finite(float value, float fallback)
		{
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				return value;
			}
			return fallback;
		}

		// Token: 0x06008F51 RID: 36689 RVA: 0x003AA027 File Offset: 0x003A8227
		private static float NonNegative(float value)
		{
			return Mathf.Max(0f, EggRotorClimaxMath.Finite(value, 0f));
		}

		// Token: 0x040092F2 RID: 37618
		public const float FullValue = 0.9999f;

		// Token: 0x040092F3 RID: 37619
		private const float KneeTremor = 1f;

		// Token: 0x040092F4 RID: 37620
		private const float HipsTremor = 0.5f;

		// Token: 0x040092F5 RID: 37621
		private const float SpineTremor = 0.6f;

		// Token: 0x040092F6 RID: 37622
		private const float HeadTremor = 0.5f;

		// Token: 0x040092F7 RID: 37623
		private const float ArmTremor = 1.2f;

		// Token: 0x040092F8 RID: 37624
		private const float HandTremor = 1.8f;

		// Token: 0x040092F9 RID: 37625
		private const float MaxPulseSum = 1.5f;

		// Token: 0x040092FA RID: 37626
		private const float StandingFootHeight = 0.3f;

		// Token: 0x040092FB RID: 37627
		private const float StandingHipsHeight = 0.8f;

		// Token: 0x040092FC RID: 37628
		private const float StandingLegDot = 0.8f;

		// Token: 0x040092FD RID: 37629
		private const float PlantedHandHeight = 0.3f;

		// Token: 0x040092FE RID: 37630
		private const float InvertedMargin = 0.15f;

		// Token: 0x040092FF RID: 37631
		private const float UprightTorsoDot = 0.7f;

		// Token: 0x04009300 RID: 37632
		private const float BentTorsoDot = 0.3f;
	}
}
