using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029B RID: 667
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will request the spawn of a floating text, usually to signify damage, but not necessarily. This requires that a MMFloatingTextSpawner be correctly setup in the scene, otherwise nothing will happen. To do so, create a new empty object, add a MMFloatingTextSpawner to it. Drag (at least) one MMFloatingText prefab into its PooledSimpleMMFloatingText slot. You'll find such prefabs already made in the MMTools/Tools/MMFloatingText/Prefabs folder, but feel free to create your own. Using that feedback will always spawn the same text. While this may be what you want, if you're using the Corgi Engine or TopDown Engine, you'll find dedicated versions directly hooked to the Health component, letting you display damage taken.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/Floating Text")]
	public class MMF_FloatingText : MMF_Feedback
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x00050B60 File Offset: 0x0004ED60
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x00050B6E File Offset: 0x0004ED6E
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Lifetime);
			}
			set
			{
				this.Lifetime = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x00050B77 File Offset: 0x0004ED77
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x00050B7A File Offset: 0x0004ED7A
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00050B80 File Offset: 0x0004ED80
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FloatingText.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			switch (this.PositionMode)
			{
			case MMF_FloatingText.PositionModes.TargetTransform:
				this._playPosition = this.TargetTransform.position;
				break;
			case MMF_FloatingText.PositionModes.FeedbackPosition:
				this._playPosition = this.Owner.transform.position;
				break;
			case MMF_FloatingText.PositionModes.PlayPosition:
				this._playPosition = position;
				break;
			}
			if (this.RoundingMethod != MMF_FloatingText.RoundingMethods.NoRounding)
			{
				MMF_FloatingText.RoundingMethods roundingMethod = this.RoundingMethod;
			}
			feedbacksIntensity = this.ApplyRounding(feedbacksIntensity);
			this._value = (this.UseIntensityAsValue ? feedbacksIntensity.ToString() : this.Value);
			MMFloatingTextSpawnEvent.Trigger(this.ChannelData, this._playPosition, this._value, this.Direction, this.Intensity * num, this.ForceLifetime, this.Lifetime, this.ForceColor, this.AnimateColorGradient, this.ComputedTimescaleMode == TimescaleModes.Unscaled);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00050C70 File Offset: 0x0004EE70
		protected virtual float ApplyRounding(float value)
		{
			if (this.RoundingMethod == MMF_FloatingText.RoundingMethods.NoRounding)
			{
				return value;
			}
			switch (this.RoundingMethod)
			{
			case MMF_FloatingText.RoundingMethods.Round:
				return Mathf.Round(value);
			case MMF_FloatingText.RoundingMethods.Ceil:
				return Mathf.Ceil(value);
			case MMF_FloatingText.RoundingMethods.Floor:
				return Mathf.Floor(value);
			default:
				return value;
			}
		}

		// Token: 0x0400151F RID: 5407
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001520 RID: 5408
		[MMFInspectorGroup("Floating Text", true, 64, false, false)]
		[Tooltip("the Intensity to spawn this text with, will act as a lifetime/movement/scale multiplier based on the spawner's settings")]
		public float Intensity = 1f;

		// Token: 0x04001521 RID: 5409
		[Tooltip("the value to display when spawning this text")]
		public string Value = "100";

		// Token: 0x04001522 RID: 5410
		[Tooltip("if this is true, the intensity passed to this feedback will be the value displayed")]
		public bool UseIntensityAsValue;

		// Token: 0x04001523 RID: 5411
		[Tooltip("the rounding methods to apply to the output value (when using intensity as the output value, string values won't get rounded)")]
		[MMFInspectorGroup("Rounding", true, 68, false, false)]
		public MMF_FloatingText.RoundingMethods RoundingMethod;

		// Token: 0x04001524 RID: 5412
		[MMFInspectorGroup("Color", true, 65, false, false)]
		[Tooltip("whether or not to force a color on the new text, if not, the default colors of the spawner will be used")]
		public bool ForceColor;

		// Token: 0x04001525 RID: 5413
		[Tooltip("the gradient to apply over the lifetime of the text")]
		[GradientUsage(true)]
		public Gradient AnimateColorGradient = new Gradient();

		// Token: 0x04001526 RID: 5414
		[MMFInspectorGroup("Lifetime", true, 66, false, false)]
		[Tooltip("whether or not to force a lifetime on the new text, if not, the default colors of the spawner will be used")]
		public bool ForceLifetime;

		// Token: 0x04001527 RID: 5415
		[Tooltip("the forced lifetime for the spawned text")]
		[MMFCondition("ForceLifetime", true)]
		public float Lifetime = 0.5f;

		// Token: 0x04001528 RID: 5416
		[MMFInspectorGroup("Position", true, 67, false, false)]
		[Tooltip("where to spawn the new text (at the position of the feedback, or on a specified Transform)")]
		public MMF_FloatingText.PositionModes PositionMode = MMF_FloatingText.PositionModes.FeedbackPosition;

		// Token: 0x04001529 RID: 5417
		[Tooltip("in transform mode, the Transform on which to spawn the new floating text")]
		[MMFEnumCondition("PositionMode", new int[] { 0 })]
		public Transform TargetTransform;

		// Token: 0x0400152A RID: 5418
		[Tooltip("the direction to apply to the new floating text (leave it to 0 to let the Spawner decide based on its settings)")]
		public Vector3 Direction = Vector3.zero;

		// Token: 0x0400152B RID: 5419
		protected Vector3 _playPosition;

		// Token: 0x0400152C RID: 5420
		protected string _value;

		// Token: 0x02000530 RID: 1328
		public enum PositionModes
		{
			// Token: 0x040023D2 RID: 9170
			TargetTransform,
			// Token: 0x040023D3 RID: 9171
			FeedbackPosition,
			// Token: 0x040023D4 RID: 9172
			PlayPosition
		}

		// Token: 0x02000531 RID: 1329
		public enum RoundingMethods
		{
			// Token: 0x040023D6 RID: 9174
			NoRounding,
			// Token: 0x040023D7 RID: 9175
			Round,
			// Token: 0x040023D8 RID: 9176
			Ceil,
			// Token: 0x040023D9 RID: 9177
			Floor
		}
	}
}
