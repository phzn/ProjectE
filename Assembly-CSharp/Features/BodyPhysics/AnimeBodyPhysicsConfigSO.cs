using System;
using UnityEngine;

namespace Features.BodyPhysics
{
	// Token: 0x020014FC RID: 5372
	[CreateAssetMenu(fileName = "AnimeBodyPhysicsConfig", menuName = "Project Exhibitionist/Body Physics/Anime Body Physics")]
	public sealed class AnimeBodyPhysicsConfigSO : ScriptableObject
	{
		// Token: 0x0600A612 RID: 42514 RVA: 0x004567A1 File Offset: 0x004549A1
		public static AnimeBodyPhysicsConfigSO Load()
		{
			return Resources.Load<AnimeBodyPhysicsConfigSO>("AnimeBodyPhysicsConfig");
		}

		// Token: 0x0600A613 RID: 42515 RVA: 0x004567B0 File Offset: 0x004549B0
		public float BreastScaleFor(int boobsType)
		{
			float[] sizeScale = this.breasts.sizeScale;
			if (sizeScale == null || sizeScale.Length == 0)
			{
				return 1f;
			}
			return Mathf.Max(0f, sizeScale[Mathf.Clamp(boobsType, 0, sizeScale.Length - 1)]);
		}

		// Token: 0x0400AAA9 RID: 43689
		public const string ResourceName = "AnimeBodyPhysicsConfig";

		// Token: 0x0400AAAA RID: 43690
		[Header("Breasts (c_breast_01 swinging round c_breast_02)")]
		public AnimeBodyPhysicsConfigSO.BreastSettings breasts = new AnimeBodyPhysicsConfigSO.BreastSettings();

		// Token: 0x0400AAAB RID: 43691
		[Header("Butt (Butt_L / Butt_R)")]
		public AnimeBodyPhysicsConfigSO.ButtSettings butt = new AnimeBodyPhysicsConfigSO.ButtSettings();

		// Token: 0x0400AAAC RID: 43692
		[Header("Steps (from her feet as they show)")]
		[Tooltip("A foot counts as lifted once this far above the ground of its last stance, in metres, and a strike must come down at least this far from the top of its swing.")]
		[Range(0.01f, 0.2f)]
		public float footLiftHeight = 0.06f;

		// Token: 0x0400AAAD RID: 43693
		[Tooltip("A lifted foot strikes where its fall slows to this speed (m/s): it has met the ground.")]
		[Range(0f, 1f)]
		public float footStopSpeed = 0.1f;

		// Token: 0x0400AAAE RID: 43694
		[Tooltip("Below this speed on the ground (m/s) her steps give no jolt.")]
		[Min(0f)]
		public float minStepSpeed = 0.35f;

		// Token: 0x0400AAAF RID: 43695
		[Tooltip("At this speed (m/s) the jolts are whole; walking gives a part of them.")]
		[Min(0.5f)]
		public float fullStepSpeed = 3f;

		// Token: 0x0400AAB0 RID: 43696
		[Tooltip("How the jolt grows with her speed: 1 in proportion, lower keeps more of it when walking.")]
		[Range(0.2f, 2f)]
		public float stepSpeedCurve = 0.75f;

		// Token: 0x0400AAB1 RID: 43697
		[Header("Gait (the breasts' springs from her walk to her run)")]
		[Tooltip("At this speed (m/s) and slower, moving, the breasts use the walk springs (her walk is 1)...")]
		[Min(0f)]
		public float walkSpeed = 1.3f;

		// Token: 0x0400AAB2 RID: 43698
		[Tooltip("...and at this speed and faster the run's, the bounce approved in the stadium (her run is 3). In between they blend. Braking turns them to the settle springs whatever her pace.")]
		[Min(0.1f)]
		public float runSpeed = 2.5f;

		// Token: 0x0400AAB3 RID: 43699
		[Header("Settling (the springs from her run to standing still)")]
		[Tooltip("At this speed (m/s) and slower the springs are the settle ones: softer, both sides more together...")]
		[Min(0f)]
		public float settleSpeed = 0.3f;

		// Token: 0x0400AAB4 RID: 43700
		[Tooltip("...and at this speed and faster (a walk already) the run's: the bounce approved in the stadium. In between they blend; the moment the player lets go of the movement they go to the settle at once.")]
		[Min(0.1f)]
		public float bounceSpeed = 0.9f;

		// Token: 0x0400AAB5 RID: 43701
		[Tooltip("Seconds the springs take to turn soft once she stops (the moment the player lets go) or lively again as she speeds up.")]
		[Range(0.02f, 1f)]
		public float settleBlendSeconds = 0.15f;

		// Token: 0x0400AAB6 RID: 43702
		[Tooltip("Seconds the soft bounces of coming to rest last once she slows down, before the calm rest springs.")]
		[Range(0f, 5f)]
		public float settleSeconds = 1.2f;

		// Token: 0x0400AAB7 RID: 43703
		[Tooltip("Seconds from the settle springs to the rest ones.")]
		[Range(0.05f, 3f)]
		public float restBlendSeconds = 0.6f;

		// Token: 0x0400AAB8 RID: 43704
		[Tooltip("Share of its jolt each braking step gives once the player lets go of the movement.")]
		[Range(0f, 1f)]
		public float brakingStepKick = 0.4f;

		// Token: 0x0400AAB9 RID: 43705
		[Header("Clothes (WardrobeBodyPhysicsController's weights)")]
		[Tooltip("How much firmer a garment holding the body makes the springs: their frequency grows by this much per unit of hold (a sports top's weight 0.7 holds 0.3). The bounce keeps the garment's weight as its size, as the Magica springs' blend does. Running, firmer springs leave the steps' rhythm, so the bounce shrinks far more than the weight: 0.8 here left a weight of 0.7 only 43% of the naked bounce, hence 0.")]
		[Range(0f, 3f)]
		public float clothesFirmness;

		// Token: 0x0400AABA RID: 43706
		[Tooltip("Damping a garment adds per unit of hold, so a held bounce settles sooner. 0.1 keeps the running and walking bounce at about its weight (0.7 leaves 67%).")]
		[Range(0f, 1f)]
		public float clothesDamping = 0.1f;

		// Token: 0x0400AABB RID: 43707
		[Header("Hands")]
		[Tooltip("A hand (its wrist, palm or knuckles) this close to the core of a breast (c_breast_02 to c_breast_01) or to a cheek's bone holds it, in metres...")]
		[Range(0.01f, 0.3f)]
		public float handHoldRadius = 0.055f;

		// Token: 0x0400AABC RID: 43708
		[Tooltip("...and lets go of it from this far.")]
		[Range(0.02f, 0.4f)]
		public float handReleaseRadius = 0.08f;

		// Token: 0x0400AABD RID: 43709
		[Tooltip("How much of the bounce a holding hand stops.")]
		[Range(0f, 1f)]
		public float handHoldStrength = 0.85f;

		// Token: 0x0400AABE RID: 43710
		[Tooltip("Damping a holding hand adds, so the bounce does not come back at once when it lets go.")]
		[Range(0f, 1f)]
		public float handHoldDamping = 0.5f;

		// Token: 0x0400AABF RID: 43711
		[Tooltip("How much of the bounce hands holding the breasts (Hold Boobs) stop while she walks or runs. Measured in Play: 0.2 leaves about 75% of the free walking bounce and 70% of the run's, with no arm or hand going through them; 0.6 (30%) and the standing hold above (5-8%) looked still. Standing they hold them as above.")]
		[Range(0f, 1f)]
		public float handHoldMovingStrength = 0.2f;

		// Token: 0x0400AAC0 RID: 43712
		[Tooltip("Damping hands holding the breasts add while she walks or runs.")]
		[Range(0f, 1f)]
		public float handHoldMovingDamping = 0.05f;

		// Token: 0x0400AAC1 RID: 43713
		[Header("Blending")]
		[Tooltip("Seconds for the clothes' and the game's caps on the physics to take effect, as the Magica springs blend (WardrobeBodyPhysicsController).")]
		[Min(0.01f)]
		public float intensityBlendSeconds = 0.3f;

		// Token: 0x020014FD RID: 5373
		[Serializable]
		public sealed class BreastSettings
		{
			// Token: 0x0400AAC2 RID: 43714
			[Tooltip("Swings a second of the breast springs: lower is softer and slower to come back.")]
			[Range(1f, 10f)]
			public float frequency = 3.7f;

			// Token: 0x0400AAC3 RID: 43715
			[Tooltip("How far apart the two springs are tuned, as a share of the frequency: they drift in and out of step, so one often rises while the other drops.")]
			[Range(0f, 0.5f)]
			public float asymmetry = 0.14f;

			// Token: 0x0400AAC4 RID: 43716
			[Tooltip("0 bounces for ever, 1 settles without bouncing.")]
			[Range(0.02f, 1f)]
			public float damping = 0.12f;

			// Token: 0x0400AAC5 RID: 43717
			[Tooltip("Speed of the jolt a running foot strike gives the breast on its side, in metres a second.")]
			[Range(0f, 3f)]
			public float kick = 0.6f;

			// Token: 0x0400AAC6 RID: 43718
			[Tooltip("Random variation of each jolt, as a share of Kick.")]
			[Range(0f, 1f)]
			public float kickRandom = 0.3f;

			// Token: 0x0400AAC7 RID: 43719
			[Tooltip("The other breast is flung up by this share of the jolt, so they bounce in turn.")]
			[Range(0f, 1.5f)]
			public float counterKick = 0.75f;

			// Token: 0x0400AAC8 RID: 43720
			[Tooltip("How much of the simulated swing shows: above 1 is the anime exaggeration.")]
			[Range(0f, 4f)]
			public float gain = 1.5f;

			// Token: 0x0400AAC9 RID: 43721
			[Tooltip("How much of the bounce both breasts share: lower makes one rise while the other drops.")]
			[Range(0f, 1f)]
			public float unison = 0.45f;

			// Token: 0x0400AACA RID: 43722
			[Tooltip("The farthest a breast swings around its base, in degrees.")]
			[Range(5f, 60f)]
			public float maxAngle = 32f;

			// Token: 0x0400AACB RID: 43723
			[Tooltip("The farthest the simulated mass may swing from its rest, in metres (before the gain).")]
			[Range(0.01f, 0.2f)]
			public float maxSwing = 0.06f;

			// Token: 0x0400AACC RID: 43724
			[Tooltip("Share of the sideways swing that shows; up and down always shows whole.")]
			[Range(0f, 1f)]
			public float sway = 0.5f;

			// Token: 0x0400AACD RID: 43725
			[Tooltip("How much of the swing the top of the breast (c_breast_02, the bone it swings around) follows.")]
			[Range(0f, 1f)]
			public float baseFollow = 0.3f;

			// Token: 0x0400AACE RID: 43726
			[Tooltip("Scale of the swing per breast size (BoobsType 0 Flat .. 5 Balloon).")]
			public float[] sizeScale = new float[] { 0f, 0.55f, 0.75f, 1f, 1.1f, 1.25f };

			// Token: 0x0400AACF RID: 43727
			[Tooltip("Swings a second while she walks (Walk Speed and slower). Her steps come too far apart for the run's springs: between two of them those rang on by themselves like a rubber ball (half the walking swing was that ringing). Softer and slower, each step gives one heavy bounce.")]
			[Range(1f, 10f)]
			public float walkFrequency = 2.8f;

			// Token: 0x0400AAD0 RID: 43728
			[Tooltip("Damping while she walks: a step's bounce is over before the next foot lands.")]
			[Range(0.02f, 1f)]
			public float walkDamping = 0.35f;

			// Token: 0x0400AAD1 RID: 43729
			[Tooltip("Speed of the jolt a walking foot strike gives (Kick at a run), before her pace scales it down. 0.45 keeps the walking swing about as big as it was with the run's springs, now without the ringing; more carries more swing into a stop from a walk.")]
			[Range(0f, 3f)]
			public float walkKick = 0.45f;

			// Token: 0x0400AAD2 RID: 43730
			[Tooltip("How much of the bounce both breasts share while she walks: more together than at a run.")]
			[Range(0f, 1f)]
			public float walkUnison = 0.6f;

			// Token: 0x0400AAD3 RID: 43731
			[Tooltip("Swings a second while they come to rest after she moved (the first Settle Seconds once she slows down or the player lets go): slower than the run's, so the bounces are soft.")]
			[Range(1f, 10f)]
			public float settleFrequency = 2.2f;

			// Token: 0x0400AAD4 RID: 43732
			[Tooltip("Damping while they come to rest: a few soft bounces fading out, neither a rubber ball nor still at once.")]
			[Range(0.02f, 1f)]
			public float settleDamping = 0.3f;

			// Token: 0x0400AAD5 RID: 43733
			[Tooltip("How much of the swing both breasts share once she slows down: they settle mostly together, not in turn.")]
			[Range(0f, 1f)]
			public float settleUnison = 0.75f;

			// Token: 0x0400AAD6 RID: 43734
			[Tooltip("Share of their swing forward and back that shows once she slows down: braking they carry on forward, lift and swing back; starting off they press back against her.")]
			[Range(0f, 1.5f)]
			public float settleForward = 0.4f;

			// Token: 0x0400AAD7 RID: 43735
			[Tooltip("Swings a second once she has been still a while (idle, emotes, dancing): calm, so no rhythm of her body builds the swing up.")]
			[Range(1f, 10f)]
			public float restFrequency = 2.8f;

			// Token: 0x0400AAD8 RID: 43736
			[Tooltip("Damping once she has been still a while.")]
			[Range(0.02f, 1f)]
			public float restDamping = 0.6f;
		}

		// Token: 0x020014FE RID: 5374
		[Serializable]
		public sealed class ButtSettings
		{
			// Token: 0x0400AAD9 RID: 43737
			[Tooltip("Swings a second of each cheek: firmer and quicker than the breasts.")]
			[Range(1f, 10f)]
			public float frequency = 4.4f;

			// Token: 0x0400AADA RID: 43738
			[Tooltip("How far apart the two cheeks are tuned, as a share of the frequency.")]
			[Range(0f, 0.5f)]
			public float asymmetry = 0.05f;

			// Token: 0x0400AADB RID: 43739
			[Tooltip("0 jiggles for ever, 1 settles without jiggling.")]
			[Range(0.02f, 1f)]
			public float damping = 0.18f;

			// Token: 0x0400AADC RID: 43740
			[Tooltip("Speed of the jolt a running foot strike gives the cheek above that leg, in metres a second.")]
			[Range(0f, 3f)]
			public float kick = 0.85f;

			// Token: 0x0400AADD RID: 43741
			[Tooltip("Random variation of each jolt, as a share of Kick.")]
			[Range(0f, 1f)]
			public float kickRandom = 0.25f;

			// Token: 0x0400AADE RID: 43742
			[Tooltip("The other cheek is flung up by this share of the jolt.")]
			[Range(0f, 1.5f)]
			public float counterKick = 0.35f;

			// Token: 0x0400AADF RID: 43743
			[Tooltip("How much a jolt also throws the cheek back, out of her body, as well as down (0 straight down).")]
			[Range(0f, 1f)]
			public float kickBack = 0.3f;

			// Token: 0x0400AAE0 RID: 43744
			[Tooltip("How much of the simulated swing shows.")]
			[Range(0f, 4f)]
			public float gain = 1.8f;

			// Token: 0x0400AAE1 RID: 43745
			[Tooltip("How much of the jiggle both cheeks share.")]
			[Range(0f, 1f)]
			public float unison = 0.5f;

			// Token: 0x0400AAE2 RID: 43746
			[Tooltip("The farthest a cheek moves from its rest, in metres (after the gain).")]
			[Range(0.005f, 0.1f)]
			public float maxOffset = 0.06f;

			// Token: 0x0400AAE3 RID: 43747
			[Tooltip("The farthest the simulated mass may swing from its rest, in metres (before the gain).")]
			[Range(0.01f, 0.2f)]
			public float maxSwing = 0.05f;

			// Token: 0x0400AAE4 RID: 43748
			[Tooltip("Share of the sideways and back-and-forth jiggle that shows; up and down always shows whole.")]
			[Range(0f, 1f)]
			public float sway = 0.5f;

			// Token: 0x0400AAE5 RID: 43749
			[Tooltip("How much a cheek stretches as it drops and squashes as it rises, per metre it moves up or down (keeping its volume): 4 stretches it 8% at 2 cm.")]
			[Range(0f, 12f)]
			public float squash = 4f;

			// Token: 0x0400AAE6 RID: 43750
			[Tooltip("The most a cheek stretches or squashes, as a share of its size.")]
			[Range(0f, 0.3f)]
			public float maxSquash = 0.14f;

			// Token: 0x0400AAE7 RID: 43751
			[Tooltip("Swings a second while the cheeks come to rest after she moved (the first Settle Seconds).")]
			[Range(1f, 10f)]
			public float settleFrequency = 3f;

			// Token: 0x0400AAE8 RID: 43752
			[Tooltip("Damping while they come to rest: a couple of soft wobbles fading out.")]
			[Range(0.02f, 1f)]
			public float settleDamping = 0.32f;

			// Token: 0x0400AAE9 RID: 43753
			[Tooltip("How much of the jiggle both cheeks share once she slows down.")]
			[Range(0f, 1f)]
			public float settleUnison = 0.5f;

			// Token: 0x0400AAEA RID: 43754
			[Tooltip("Swings a second once she has been still a while.")]
			[Range(1f, 10f)]
			public float restFrequency = 3.6f;

			// Token: 0x0400AAEB RID: 43755
			[Tooltip("Damping once she has been still a while.")]
			[Range(0.02f, 1f)]
			public float restDamping = 0.5f;
		}
	}
}
