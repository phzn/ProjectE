using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001224 RID: 4644
	[Serializable]
	public sealed class EggRotorClimaxSettings
	{
		// Token: 0x06008FC9 RID: 36809 RVA: 0x003ACF46 File Offset: 0x003AB146
		public static float[] CreateDefaultFillRates()
		{
			return new float[] { 0.012f, 0.022f, 0.04f };
		}

		// Token: 0x06008FCA RID: 36810 RVA: 0x003ACF5C File Offset: 0x003AB15C
		public void Validate()
		{
			this.fillPerSecond = EggRotorClimaxSettings.EnsureRates(this.fillPerSecond);
			for (int i = 0; i < this.fillPerSecond.Length; i++)
			{
				this.fillPerSecond[i] = EggRotorClimaxSettings.NonNegative(this.fillPerSecond[i]);
			}
			this.outsideFactor = EggRotorClimaxSettings.NonNegative(this.outsideFactor);
			this.glansFactor = EggRotorClimaxSettings.NonNegative(this.glansFactor);
			this.nippleComboPerLevel = EggRotorClimaxSettings.NonNegative(this.nippleComboPerLevel);
			this.refractorySeconds = EggRotorClimaxSettings.NonNegative(this.refractorySeconds);
			this.resumeBelowValue = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.resumeBelowValue, 0.95f), 0.05f, 0.99f);
			this.durationSeconds = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.durationSeconds, 4f), 0.5f, 20f);
			this.fadeInSeconds = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.fadeInSeconds), 0f, this.durationSeconds * 0.5f);
			this.fadeOutSeconds = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.fadeOutSeconds), 0f, this.durationSeconds * 0.5f);
			this.reducedUpperBodyWeight = Mathf.Clamp01(EggRotorClimaxSettings.Finite(this.reducedUpperBodyWeight, 0.5f));
			this.emoteTrembleWeight = Mathf.Clamp01(EggRotorClimaxSettings.Finite(this.emoteTrembleWeight, 1f));
			this.pulseCount = Mathf.Clamp(this.pulseCount, 0, 12);
			this.firstPulseSeconds = EggRotorClimaxSettings.NonNegative(this.firstPulseSeconds);
			this.pulseIntervalSeconds = Mathf.Max(0.05f, EggRotorClimaxSettings.Finite(this.pulseIntervalSeconds, 0.45f));
			this.pulseIntervalGrowth = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.pulseIntervalGrowth, 1.15f), 0.5f, 2f);
			this.pulseAmplitudeDecay = Mathf.Clamp01(EggRotorClimaxSettings.Finite(this.pulseAmplitudeDecay, 0.82f));
			this.pulseAttackSeconds = Mathf.Max(0.005f, EggRotorClimaxSettings.Finite(this.pulseAttackSeconds, 0.05f));
			this.pulseDecaySeconds = Mathf.Max(0.01f, EggRotorClimaxSettings.Finite(this.pulseDecaySeconds, 0.18f));
			this.pulseShake = Mathf.Clamp01(EggRotorClimaxSettings.Finite(this.pulseShake, 0.8f));
			this.kneeDipDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.kneeDipDegrees), 0f, 20f);
			this.pulseKneeDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.pulseKneeDegrees), 0f, 15f);
			this.kneesTogetherDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.kneesTogetherDegrees), 0f, 10f);
			this.pelvisPulseDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.pelvisPulseDegrees), 0f, 15f);
			this.torsoCurlDegrees = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.torsoCurlDegrees, 5f), -15f, 15f);
			this.headPulseDegrees = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.headPulseDegrees, -5f), -15f, 15f);
			this.tremorDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.tremorDegrees), 0f, 6f);
			this.tremorHz = Mathf.Clamp(EggRotorClimaxSettings.Finite(this.tremorHz, 10f), 0.5f, 30f);
			this.breathDegrees = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.breathDegrees), 0f, 6f);
			this.breathHz = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.breathHz), 0f, 4f);
			this.squirtDelaySeconds = EggRotorClimaxSettings.NonNegative(this.squirtDelaySeconds);
			if (!EggRotorPlacementMath.IsFinite(this.squirtPosition))
			{
				this.squirtPosition = EggRotorRigSettings.CanalOpening;
			}
			if (!EggRotorPlacementMath.IsFinite(this.squirtDirection) || this.squirtDirection.sqrMagnitude < 1E-06f)
			{
				this.squirtDirection = EggRotorClimaxSettings.DefaultSquirtDirection;
			}
			this.cumDelaySeconds = EggRotorClimaxSettings.NonNegative(this.cumDelaySeconds);
			this.cumSeconds = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.cumSeconds), 0f, 15f);
			this.penisPulseVelocity = Mathf.Clamp(EggRotorClimaxSettings.NonNegative(this.penisPulseVelocity), 0f, 5f);
			this.voiceHoldSeconds = EggRotorClimaxSettings.NonNegative(this.voiceHoldSeconds);
			this.noiseRadius = EggRotorClimaxSettings.NonNegative(this.noiseRadius);
			this.noiseSeconds = EggRotorClimaxSettings.NonNegative(this.noiseSeconds);
		}

		// Token: 0x06008FCB RID: 36811 RVA: 0x003AD3BC File Offset: 0x003AB5BC
		private static float[] EnsureRates(float[] rates)
		{
			int num = 3;
			if (rates != null && rates.Length >= num)
			{
				return rates;
			}
			float[] array = EggRotorClimaxSettings.CreateDefaultFillRates();
			int num2 = 0;
			while (rates != null && num2 < rates.Length)
			{
				array[num2] = rates[num2];
				num2++;
			}
			return array;
		}

		// Token: 0x06008FCC RID: 36812 RVA: 0x001B3A41 File Offset: 0x001B1C41
		private static float Finite(float value, float fallback)
		{
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				return value;
			}
			return fallback;
		}

		// Token: 0x06008FCD RID: 36813 RVA: 0x003AD3F4 File Offset: 0x003AB5F4
		private static float NonNegative(float value)
		{
			return Mathf.Max(0f, EggRotorClimaxSettings.Finite(value, 0f));
		}

		// Token: 0x0400938E RID: 37774
		public const string SquirtPrefabGuid = "51312a01c45036d448022c0296fdac1d";

		// Token: 0x0400938F RID: 37775
		public const string SquirtPropLabel = "Squirt";

		// Token: 0x04009390 RID: 37776
		public const string SquirtPrefabName = "SmallDrops";

		// Token: 0x04009391 RID: 37777
		public const int MaxPulses = 12;

		// Token: 0x04009392 RID: 37778
		public const string CumPrefabGuid = "c35fc612aef7e6a42a965beee70c15ab";

		// Token: 0x04009393 RID: 37779
		public const string CumPropLabel = "Cum";

		// Token: 0x04009394 RID: 37780
		public static readonly Vector3 DefaultSquirtDirection = new Vector3(0f, -0.55f, 0.84f);

		// Token: 0x04009395 RID: 37781
		[Header("Orgasm meter")]
		[Tooltip("The egg fills the orgasm meter (off = it never does).")]
		public bool fillsOrgasmMeter = true;

		// Token: 0x04009396 RID: 37782
		[Tooltip("Meter per second Inside at Low, Medium, High: 0.012 / 0.022 / 0.04 fill an empty bar in about 85 / 45 / 25 s. The Hitachi fills 0.02/s at its default speed.")]
		public float[] fillPerSecond = EggRotorClimaxSettings.CreateDefaultFillRates();

		// Token: 0x04009397 RID: 37783
		[Tooltip("Outside (taped on the vulva) fills at this share of the Inside rate. The Swing never fills.")]
		public float outsideFactor = 0.8f;

		// Token: 0x04009398 RID: 37784
		[Tooltip("Futanari Mode: taped under the glans fills at this share of the Inside rate.")]
		public float glansFactor = 1f;

		// Token: 0x04009399 RID: 37785
		[Tooltip("With the Nipple Rotor vibrating too, the rate is multiplied by 1 + this * its level (Low x1.3, Medium x1.6, High x1.9). The Nipple Rotor alone never fills the bar.")]
		public float nippleComboPerLevel = 0.3f;

		// Token: 0x0400939A RID: 37786
		[Tooltip("Seconds after a climax (the egg's or another system's) during which the egg does not fill the bar.")]
		public float refractorySeconds = 12f;

		// Token: 0x0400939B RID: 37787
		[Tooltip("After the refractory time the egg also waits until the bar has drained to this value: feeding a bar that is still full never fires a new climax (OrgasmMeterController only fires on the way up to 1).")]
		[Range(0.05f, 0.99f)]
		public float resumeBelowValue = 0.95f;

		// Token: 0x0400939C RID: 37788
		[Header("Climax sequence")]
		[Tooltip("Length of the climax: Ciel stands still and trembles, then the meter is released.")]
		public float durationSeconds = 4f;

		// Token: 0x0400939D RID: 37789
		public float fadeInSeconds = 0.25f;

		// Token: 0x0400939E RID: 37790
		public float fadeOutSeconds = 0.6f;

		// Token: 0x0400939F RID: 37791
		[Tooltip("Stops Ciel in place for the sequence (Invector input lock plus an activity lease) when she stands free. An emote is frozen instead (holdEmotes). Otherwise (in an action, another lock, an emote that cannot be frozen...) only the upper body trembles.")]
		public bool stopInPlace = true;

		// Token: 0x040093A0 RID: 37792
		[Tooltip("Upper-body tremble weight when she cannot be stopped in place (the legs and pelvis stay animated).")]
		[Range(0f, 1f)]
		public float reducedUpperBodyWeight = 0.5f;

		// Token: 0x040093A1 RID: 37793
		[Header("During an emote")]
		[Tooltip("An emote playing when the climax starts is frozen in its pose for the sequence (GameplayEmoteController playback hold) and resumes where it was afterwards; it is never ended. Off = the emote keeps playing and only the upper body trembles at reducedUpperBodyWeight.")]
		public bool holdEmotes = true;

		// Token: 0x040093A2 RID: 37794
		[Tooltip("Tremble weight on a frozen emote pose. The knee dip only runs on a standing pose; kneeling, squatting, sitting and lying poses keep their legs, a hand planted on the ground keeps the torso and arms still, and an inverted pose (handstand) only gets the breast and butt pulses.")]
		[Range(0f, 1f)]
		public float emoteTrembleWeight = 1f;

		// Token: 0x040093A3 RID: 37795
		[Header("Contractions")]
		[Range(0f, 12f)]
		public int pulseCount = 6;

		// Token: 0x040093A4 RID: 37796
		public float firstPulseSeconds = 0.15f;

		// Token: 0x040093A5 RID: 37797
		[Tooltip("Gap between the first two contractions; each next gap grows by pulseIntervalGrowth.")]
		public float pulseIntervalSeconds = 0.45f;

		// Token: 0x040093A6 RID: 37798
		public float pulseIntervalGrowth = 1.15f;

		// Token: 0x040093A7 RID: 37799
		[Tooltip("Each contraction is this share of the previous one.")]
		[Range(0f, 1f)]
		public float pulseAmplitudeDecay = 0.82f;

		// Token: 0x040093A8 RID: 37800
		public float pulseAttackSeconds = 0.05f;

		// Token: 0x040093A9 RID: 37801
		[Tooltip("Exponential decay time of one contraction.")]
		public float pulseDecaySeconds = 0.18f;

		// Token: 0x040093AA RID: 37802
		[Tooltip("AnimeBodyPhysics.TryShake strength01 on each contraction, scaled by its amplitude (0 = off).")]
		[Range(0f, 1f)]
		public float pulseShake = 0.8f;

		// Token: 0x040093AB RID: 37803
		[Header("Tremble (degrees)")]
		[Tooltip("Knees bend this much for the whole climax; the pelvis drops so the feet stay on the ground.")]
		public float kneeDipDegrees = 7f;

		// Token: 0x040093AC RID: 37804
		[Tooltip("Extra knee bend on each contraction (the knees buckle).")]
		public float pulseKneeDegrees = 4f;

		// Token: 0x040093AD RID: 37805
		[Tooltip("Thighs turn inward, knees together.")]
		public float kneesTogetherDegrees = 3f;

		// Token: 0x040093AE RID: 37806
		[Tooltip("Pelvic tilt on each contraction (pubis forward); the spine cancels it, the thighs keep the legs still.")]
		public float pelvisPulseDegrees = 6f;

		// Token: 0x040093AF RID: 37807
		[Tooltip("Upper body curls forward on each contraction (negative arches it back).")]
		public float torsoCurlDegrees = 5f;

		// Token: 0x040093B0 RID: 37808
		[Tooltip("Head on each contraction: negative tips it back, positive drops the chin.")]
		public float headPulseDegrees = -5f;

		// Token: 0x040093B1 RID: 37809
		[Tooltip("Shiver amplitude (knees, pelvis, spine, arms, hands); it decays over the second half.")]
		public float tremorDegrees = 1.2f;

		// Token: 0x040093B2 RID: 37810
		[Tooltip("Shiver frequency (Perlin noise); above about 14 Hz it reads as jitter.")]
		public float tremorHz = 10f;

		// Token: 0x040093B3 RID: 37811
		[Tooltip("Panting on the chest.")]
		public float breathDegrees = 1.5f;

		// Token: 0x040093B4 RID: 37812
		public float breathHz = 1.3f;

		// Token: 0x040093B5 RID: 37813
		[Header("Squirt")]
		public bool squirt = true;

		// Token: 0x040093B6 RID: 37814
		[Tooltip("Particle prefab. Empty = the squirt the toys already use (SmallDrops, found on the loaded masturbation proxies' \"Squirt\" binding). Skipped while a garment covers the crotch or in water.")]
		public GameObject squirtPrefab;

		// Token: 0x040093B7 RID: 37815
		[Tooltip("Seconds after the climax starts (the first contraction).")]
		public float squirtDelaySeconds = 0.15f;

		// Token: 0x040093B8 RID: 37816
		[Tooltip("Emitter position in the PussyAnchor's axes, Ciel body units: the measured vaginal opening.")]
		public Vector3 squirtPosition = EggRotorRigSettings.CanalOpening;

		// Token: 0x040093B9 RID: 37817
		[Tooltip("Emission direction in the PussyAnchor's axes (+Y up, +Z out of the body): forward and down out of the opening. The canal axis out is (0, -0.962, 0.272); the toys' Hips binding aims about (0, -0.873, 0.488).")]
		public Vector3 squirtDirection = EggRotorClimaxSettings.DefaultSquirtDirection;

		// Token: 0x040093BA RID: 37818
		[Header("Futanari Mode: ejaculation (the egg on the glans)")]
		[Tooltip("With the egg on the glans the climax ejaculates from the tip instead of squirting.")]
		public bool cum = true;

		// Token: 0x040093BB RID: 37819
		[Tooltip("Effect prefab. Empty = the futa Jerk Off emote's Cum binding (ThirdParty/Dick Cum.prefab), placed like the emote's binding and kept on the deformed tip by EggRotorCumFollower while it emits.")]
		public GameObject cumPrefab;

		// Token: 0x040093BC RID: 37820
		[Tooltip("Seconds after the climax starts (the first contraction).")]
		public float cumDelaySeconds = 0.15f;

		// Token: 0x040093BD RID: 37821
		[Tooltip("Seconds of emission (the Jerk Off emote keeps its Cum on for 5 s); the drops already out keep falling.")]
		public float cumSeconds = 5f;

		// Token: 0x040093BE RID: 37822
		[Tooltip("Upward velocity added to the penis physics on each contraction, scaled by its amplitude (0 = off).")]
		public float penisPulseVelocity = 0.6f;

		// Token: 0x040093BF RID: 37823
		[Tooltip("Futanari Mode: the egg vibrating on the glans raises the penis (PenisErectionController external request). It goes down during the refractory time after a climax and rises again afterwards.")]
		public bool erectWhileVibrating = true;

		// Token: 0x040093C0 RID: 37824
		[Header("Voice and NPC noise")]
		[Tooltip("Plays one of Ciel's orgasm lines through MasturbationVoiceController's external session.")]
		public bool playVoice = true;

		// Token: 0x040093C1 RID: 37825
		[Tooltip("Seconds the voice session stays open (the orgasm clips last 9-14 s; it fades out when it closes).")]
		public float voiceHoldSeconds = 10f;

		// Token: 0x040093C2 RID: 37826
		[Tooltip("NPC hearing radius of the climax (added to the buzz, the larger radius wins).")]
		public float noiseRadius = 16f;

		// Token: 0x040093C3 RID: 37827
		public float noiseSeconds = 3f;
	}
}
