using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C2 RID: 450
	[Serializable]
	public class MMFeedbackTiming
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x000300F2 File Offset: 0x0002E2F2
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x000300FA File Offset: 0x0002E2FA
		public virtual bool UseScriptDrivenTimescale { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00030103 File Offset: 0x0002E303
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0003010B File Offset: 0x0002E30B
		public virtual float ScriptDrivenDeltaTime { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00030114 File Offset: 0x0002E314
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0003011C File Offset: 0x0002E31C
		public virtual float ScriptDrivenTime { get; set; }

		// Token: 0x04000CB6 RID: 3254
		[Header("Timescale")]
		[Tooltip("whether we're working on scaled or unscaled time")]
		public TimescaleModes TimescaleMode;

		// Token: 0x04000CB7 RID: 3255
		[Header("Exceptions")]
		[Tooltip("if this is true, holding pauses won't wait for this feedback to finish")]
		public bool ExcludeFromHoldingPauses;

		// Token: 0x04000CB8 RID: 3256
		[Tooltip("whether to count this feedback in the parent MMFeedbacks(Player) total duration or not")]
		public bool ContributeToTotalDuration = true;

		// Token: 0x04000CB9 RID: 3257
		[Header("Delays")]
		[Tooltip("the initial delay to apply before playing the delay (in seconds)")]
		public float InitialDelay;

		// Token: 0x04000CBA RID: 3258
		[Tooltip("the cooldown duration mandatory between two plays")]
		public float CooldownDuration;

		// Token: 0x04000CBB RID: 3259
		[Header("Stop")]
		[Tooltip("if this is true, this feedback will interrupt itself when Stop is called on its parent MMFeedbacks, otherwise it'll keep running")]
		public bool InterruptsOnStop = true;

		// Token: 0x04000CBC RID: 3260
		[Header("Repeat")]
		[Tooltip("the repeat mode, whether the feedback should be played once, multiple times, or forever")]
		public int NumberOfRepeats;

		// Token: 0x04000CBD RID: 3261
		[Tooltip("if this is true, the feedback will be repeated forever")]
		public bool RepeatForever;

		// Token: 0x04000CBE RID: 3262
		[Tooltip("the delay (in seconds) between two firings of this feedback. This doesn't include the duration of the feedback.")]
		public float DelayBetweenRepeats = 1f;

		// Token: 0x04000CBF RID: 3263
		[Header("PlayCount")]
		[Tooltip("the number of times this feedback's been played since its initialization (or last reset if SetPlayCountToZeroOnReset is true)")]
		[MMFReadOnly]
		public int PlayCount;

		// Token: 0x04000CC0 RID: 3264
		[Tooltip("whether or not to limit the amount of times this feedback can be played. beyond that amount, it won't play anymore")]
		public bool LimitPlayCount;

		// Token: 0x04000CC1 RID: 3265
		[Tooltip("if LimitPlayCount is true, the maximum amount of times this feedback can be played")]
		[MMFCondition("LimitPlayCount", true)]
		public int MaxPlayCount = 3;

		// Token: 0x04000CC2 RID: 3266
		[Tooltip("if LimitPlayCount is true, whether or not to reset the play count to zero when the feedback is reset")]
		[MMFCondition("LimitPlayCount", true)]
		public bool SetPlayCountToZeroOnReset;

		// Token: 0x04000CC3 RID: 3267
		[Header("Play Direction")]
		[Tooltip("this defines how this feedback should play when the host MMFeedbacks is played :- always (default) : this feedback will always play- OnlyWhenForwards : this feedback will only play if the host MMFeedbacks is played in the top to bottom direction (forwards)- OnlyWhenBackwards : this feedback will only play if the host MMFeedbacks is played in the bottom to top direction (backwards)")]
		public MMFeedbackTiming.MMFeedbacksDirectionConditions MMFeedbacksDirectionCondition;

		// Token: 0x04000CC4 RID: 3268
		[Tooltip("this defines the way this feedback will play. It can play in its normal direction, or in rewind (a sound will play backwards, an object normally scaling up will scale down, a curve will be evaluated from right to left, etc)- BasedOnMMFeedbacksDirection : will play normally when the host MMFeedbacks is played forwards, in rewind when it's played backwards- OppositeMMFeedbacksDirection : will play in rewind when the host MMFeedbacks is played forwards, and normally when played backwards- Always Normal : will always play normally, regardless of the direction of the host MMFeedbacks- Always Rewind : will always play in rewind, regardless of the direction of the host MMFeedbacks")]
		public MMFeedbackTiming.PlayDirections PlayDirection;

		// Token: 0x04000CC5 RID: 3269
		[Header("Intensity")]
		[Tooltip("if this is true, intensity will be constant, even if the parent MMFeedbacks is played at a lower intensity")]
		public bool ConstantIntensity;

		// Token: 0x04000CC6 RID: 3270
		[Tooltip("if this is true, this feedback will only play if its intensity is higher or equal to IntensityIntervalMin and lower than IntensityIntervalMax")]
		public bool UseIntensityInterval;

		// Token: 0x04000CC7 RID: 3271
		[Tooltip("the minimum intensity required for this feedback to play")]
		[MMFCondition("UseIntensityInterval", true)]
		public float IntensityIntervalMin;

		// Token: 0x04000CC8 RID: 3272
		[Tooltip("the maximum intensity required for this feedback to play")]
		[MMFCondition("UseIntensityInterval", true)]
		public float IntensityIntervalMax;

		// Token: 0x04000CC9 RID: 3273
		[Header("Sequence")]
		[Tooltip("A MMSequence to use to play these feedbacks on")]
		public MMSequence Sequence;

		// Token: 0x04000CCA RID: 3274
		[Tooltip("The MMSequence's TrackID to consider")]
		public int TrackID;

		// Token: 0x04000CCB RID: 3275
		[Tooltip("whether or not to use the quantized version of the target sequence")]
		public bool Quantized;

		// Token: 0x04000CCC RID: 3276
		[Tooltip("if using the quantized version of the target sequence, the BPM to apply to the sequence when playing it")]
		[MMFCondition("Quantized", true)]
		public int TargetBPM = 120;

		// Token: 0x02000492 RID: 1170
		public enum MMFeedbacksDirectionConditions
		{
			// Token: 0x04002152 RID: 8530
			Always,
			// Token: 0x04002153 RID: 8531
			OnlyWhenForwards,
			// Token: 0x04002154 RID: 8532
			OnlyWhenBackwards
		}

		// Token: 0x02000493 RID: 1171
		public enum PlayDirections
		{
			// Token: 0x04002156 RID: 8534
			FollowMMFeedbacksDirection,
			// Token: 0x04002157 RID: 8535
			OppositeMMFeedbacksDirection,
			// Token: 0x04002158 RID: 8536
			AlwaysNormal,
			// Token: 0x04002159 RID: 8537
			AlwaysRewind
		}
	}
}
