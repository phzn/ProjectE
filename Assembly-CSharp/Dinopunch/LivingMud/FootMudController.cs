using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invector.vCharacterController;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000735 RID: 1845
	public class FootMudController : MonoBehaviour
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x0010DDCA File Offset: 0x0010BFCA
		public bool IsInMud
		{
			get
			{
				return this.isInMud;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x0010DDD2 File Offset: 0x0010BFD2
		public DynamicMudSurface CurrentSurface
		{
			get
			{
				return this.currentSurface;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x0010DDDA File Offset: 0x0010BFDA
		public MudType CurrentMudType
		{
			get
			{
				if (!(this.currentSurface != null))
				{
					return (MudType)0;
				}
				return this.currentSurface.Thickness;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x0010DDF7 File Offset: 0x0010BFF7
		public MudType ContactMudTypes
		{
			get
			{
				return ((this.leftFootTrigger != null) ? this.leftFootTrigger.ContactMudTypes : ((MudType)0)) | ((this.rightFootTrigger != null) ? this.rightFootTrigger.ContactMudTypes : ((MudType)0));
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x0010DE32 File Offset: 0x0010C032
		public float CurrentIntensity
		{
			get
			{
				return this.currentIntensity;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x0010DE3A File Offset: 0x0010C03A
		public MudFootTrigger LeftFootTrigger
		{
			get
			{
				return this.leftFootTrigger;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x0010DE42 File Offset: 0x0010C042
		public MudFootTrigger RightFootTrigger
		{
			get
			{
				return this.rightFootTrigger;
			}
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x0010DE4A File Offset: 0x0010C04A
		private void Awake()
		{
			this.ResolveFootReferences();
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x0010DE54 File Offset: 0x0010C054
		private void Start()
		{
			this.ResolveFootReferences();
			if (this.motor == null)
			{
				this.motor = base.GetComponent<vThirdPersonMotor>();
			}
			this.scalableEffects = base.GetComponentsInChildren<IFootstepMudEffect>().ToList<IFootstepMudEffect>();
			this.legacyEffects = (from e in base.GetComponentsInChildren<LivingMudEffectsPlayer>()
				where !(e is IFootstepMudEffect)
				select e).ToList<LivingMudEffectsPlayer>();
			if (this.motor != null)
			{
				this.cachedDefaultSpeedMultiplier = this.motor.defaultSpeedMultiplier;
			}
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x0010DEE8 File Offset: 0x0010C0E8
		private void ResolveFootReferences()
		{
			if (this.leftFootTrigger != null && this.rightFootTrigger != null)
			{
				return;
			}
			Transform root = base.transform.root;
			Animator componentInChildren = root.GetComponentInChildren<Animator>(true);
			if (componentInChildren != null && componentInChildren.isHuman)
			{
				if (this.leftFootTrigger == null)
				{
					Transform boneTransform = componentInChildren.GetBoneTransform(HumanBodyBones.LeftFoot);
					if (boneTransform != null)
					{
						this.leftFootTrigger = boneTransform.GetComponentInChildren<MudFootTrigger>(true);
					}
				}
				if (this.rightFootTrigger == null)
				{
					Transform boneTransform2 = componentInChildren.GetBoneTransform(HumanBodyBones.RightFoot);
					if (boneTransform2 != null)
					{
						this.rightFootTrigger = boneTransform2.GetComponentInChildren<MudFootTrigger>(true);
					}
				}
			}
			MudFootTrigger[] componentsInChildren = root.GetComponentsInChildren<MudFootTrigger>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				string text = componentsInChildren[i].name.ToLowerInvariant();
				if (this.leftFootTrigger == null && text.Contains("left"))
				{
					this.leftFootTrigger = componentsInChildren[i];
				}
				else if (this.rightFootTrigger == null && text.Contains("right"))
				{
					this.rightFootTrigger = componentsInChildren[i];
				}
			}
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x0010E008 File Offset: 0x0010C208
		private void Update()
		{
			bool flag = this.leftFootTrigger != null && this.leftFootTrigger.IsInMud;
			bool flag2 = this.rightFootTrigger != null && this.rightFootTrigger.IsInMud;
			bool flag3 = flag || flag2;
			float num = ((this.motor != null) ? this.motor.inputMagnitude : 1f);
			bool flag4 = num > this.minInputForEffects;
			this.currentIntensity = ((this.fullIntensityInput > 0f) ? Mathf.Clamp01(num / this.fullIntensityInput) : 1f);
			DynamicMudSurface dynamicMudSurface = null;
			bool flag5 = flag && (this.leftFootTrigger.CurrentSurface.Thickness & MudType.Watery) > (MudType)0;
			bool flag6 = flag2 && (this.rightFootTrigger.CurrentSurface.Thickness & MudType.Watery) > (MudType)0;
			if (flag5)
			{
				dynamicMudSurface = this.leftFootTrigger.CurrentSurface;
			}
			else if (flag6)
			{
				dynamicMudSurface = this.rightFootTrigger.CurrentSurface;
			}
			else if (flag)
			{
				dynamicMudSurface = this.leftFootTrigger.CurrentSurface;
			}
			else if (flag2)
			{
				dynamicMudSurface = this.rightFootTrigger.CurrentSurface;
			}
			if (flag3 && !this.isInMud)
			{
				this.isInMud = true;
				this.currentSurface = dynamicMudSurface;
				if (flag4)
				{
					this.TriggerEffects();
				}
			}
			if (flag3 && dynamicMudSurface != null && dynamicMudSurface != this.currentSurface)
			{
				this.StopAllEffects();
				this.currentSurface = dynamicMudSurface;
				if (flag4)
				{
					this.TriggerEffects();
				}
			}
			if (this.isInMud && flag4 && Time.time - this.lastEffectTime >= this.effectCooldown)
			{
				this.TriggerEffects();
			}
			if (this.isInMud && !flag4)
			{
				this.StopAllEffects();
			}
			if (!flag3 && this.isInMud)
			{
				this.isInMud = false;
				this.currentSurface = null;
				this.StopAllEffects();
			}
			this.UpdateSlipperyEffect();
			if (this.muddyMaterial != null && this.isInMud && flag4 && Time.time - this.lastMudAccumulationTime >= this.mudAccumulationInterval)
			{
				this.lastMudAccumulationTime = Time.time;
				this.muddyMaterial.MakeMuddy();
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x0010E224 File Offset: 0x0010C424
		private void TriggerEffects()
		{
			if (this.currentSurface == null)
			{
				return;
			}
			this.lastEffectTime = Time.time;
			MudType mudType = FootMudController.ResolveEffectThickness(this.currentSurface.Thickness);
			if (this.enableEffectSelectionLogs && Time.time - this.lastEffectSelectionLogTime >= Mathf.Max(0.05f, this.effectSelectionLogInterval))
			{
				this.lastEffectSelectionLogTime = Time.time;
				this.LogEffectSelection(mudType);
			}
			foreach (IFootstepMudEffect footstepMudEffect in this.scalableEffects)
			{
				footstepMudEffect.StartEffectsScaled(this.currentSurface, mudType, this.currentIntensity);
			}
			foreach (LivingMudEffectsPlayer livingMudEffectsPlayer in this.legacyEffects)
			{
				livingMudEffectsPlayer.StartEffects(this.currentSurface, mudType, false);
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x0010E32C File Offset: 0x0010C52C
		private static MudType ResolveEffectThickness(MudType sourceThickness)
		{
			if ((sourceThickness & MudType.Watery) != (MudType)0)
			{
				return MudType.Watery;
			}
			return sourceThickness;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x0010E338 File Offset: 0x0010C538
		private void LogEffectSelection(MudType effectiveThickness)
		{
			string text = FootMudController.DescribeSurface((this.leftFootTrigger != null) ? this.leftFootTrigger.CurrentSurface : null);
			string text2 = FootMudController.DescribeSurface((this.rightFootTrigger != null) ? this.rightFootTrigger.CurrentSurface : null);
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("[FootMudController] EffectSelect ");
			stringBuilder.Append(string.Format("raw={0} effective={1} intensity={2:F2} ", this.currentSurface.Thickness, effectiveThickness, this.currentIntensity));
			stringBuilder.Append("left=" + text + " right=" + text2);
			for (int i = 0; i < this.scalableEffects.Count; i++)
			{
				FootMudController.AppendEffectDecision(stringBuilder, this.scalableEffects[i] as MonoBehaviour, effectiveThickness, "scaled");
			}
			for (int j = 0; j < this.legacyEffects.Count; j++)
			{
				FootMudController.AppendEffectDecision(stringBuilder, this.legacyEffects[j], effectiveThickness, "legacy");
			}
			Debug.Log(stringBuilder.ToString(), this);
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x0010E45C File Offset: 0x0010C65C
		private static void AppendEffectDecision(StringBuilder sb, MonoBehaviour behaviour, MudType effectiveThickness, string channel)
		{
			if (behaviour == null)
			{
				return;
			}
			LivingMudEffectsPlayer livingMudEffectsPlayer = behaviour as LivingMudEffectsPlayer;
			if (livingMudEffectsPlayer == null)
			{
				sb.Append(string.Concat(new string[]
				{
					" | ",
					channel,
					":",
					behaviour.GetType().Name,
					"=UNKNOWN(no RequiredThickness)"
				}));
				return;
			}
			MudType requiredThickness = livingMudEffectsPlayer.RequiredThickness;
			bool flag = (requiredThickness & effectiveThickness) > (MudType)0;
			sb.Append(string.Format(" | {0}:{1} req={2} => {3}", new object[]
			{
				channel,
				behaviour.GetType().Name,
				requiredThickness,
				flag ? "MATCH" : "SKIP"
			}));
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x0010E50C File Offset: 0x0010C70C
		private static string DescribeSurface(DynamicMudSurface surface)
		{
			if (surface == null)
			{
				return "null";
			}
			return string.Format("{0}:{1}", surface.name, surface.Thickness);
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x0010E538 File Offset: 0x0010C738
		private void StopAllEffects()
		{
			foreach (IFootstepMudEffect footstepMudEffect in this.scalableEffects)
			{
				footstepMudEffect.StopEffectsScaled();
			}
			foreach (LivingMudEffectsPlayer livingMudEffectsPlayer in this.legacyEffects)
			{
				livingMudEffectsPlayer.StopEffects();
			}
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x0010E5C8 File Offset: 0x0010C7C8
		private void UpdateSlipperyEffect()
		{
			if (this.motor == null)
			{
				return;
			}
			if (!this.enableSlipperyEffect)
			{
				if (this.slipApplied)
				{
					this.motor.speedMultiplier = this.cachedDefaultSpeedMultiplier;
					this.currentSlipFactor = 1f;
					this.slipApplied = false;
				}
				return;
			}
			float num = ((this.isInMud && this.currentSurface != null && (this.currentSurface.Thickness & MudType.Sloppy) > (MudType)0) ? this.sloppySpeedMultiplier : 1f);
			this.currentSlipFactor = Mathf.MoveTowards(this.currentSlipFactor, num, this.slipBlendSpeed * Time.deltaTime);
			this.motor.speedMultiplier = this.cachedDefaultSpeedMultiplier * this.currentSlipFactor;
			this.slipApplied = Mathf.Abs(this.currentSlipFactor - 1f) > 0.01f;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x0010E6A5 File Offset: 0x0010C8A5
		private void OnDisable()
		{
			if (this.slipApplied && this.motor != null)
			{
				this.motor.speedMultiplier = this.cachedDefaultSpeedMultiplier;
				this.slipApplied = false;
			}
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x0010E6D5 File Offset: 0x0010C8D5
		private void OnDestroy()
		{
			if (this.slipApplied && this.motor != null)
			{
				this.motor.speedMultiplier = this.cachedDefaultSpeedMultiplier;
			}
		}

		// Token: 0x0400302C RID: 12332
		[Header("Foot References")]
		[Tooltip("MudFootTrigger on the left foot trigger collider")]
		[SerializeField]
		private MudFootTrigger leftFootTrigger;

		// Token: 0x0400302D RID: 12333
		[Tooltip("MudFootTrigger on the right foot trigger collider")]
		[SerializeField]
		private MudFootTrigger rightFootTrigger;

		// Token: 0x0400302E RID: 12334
		[Header("Character Motor")]
		[Tooltip("Invector motor for inputMagnitude and speedMultiplier. Auto-detected on this GO if null.")]
		[SerializeField]
		private vThirdPersonMotor motor;

		// Token: 0x0400302F RID: 12335
		[Header("Velocity Gating")]
		[Tooltip("Minimum inputMagnitude to trigger effects (0 = always, 0.1 = slight movement)")]
		[SerializeField]
		private float minInputForEffects = 0.05f;

		// Token: 0x04003030 RID: 12336
		[Tooltip("inputMagnitude at which effects reach full intensity")]
		[SerializeField]
		private float fullIntensityInput = 0.8f;

		// Token: 0x04003031 RID: 12337
		[Header("Slippery Surface (Sloppy)")]
		[Tooltip("Enable light slippery effect when walking on Sloppy mud")]
		[SerializeField]
		private bool enableSlipperyEffect;

		// Token: 0x04003032 RID: 12338
		[Tooltip("Speed multiplier while on Sloppy surface (lower = more slippery). Applied to Invector speedMultiplier.")]
		[SerializeField]
		[Range(0.3f, 1f)]
		private float sloppySpeedMultiplier = 0.7f;

		// Token: 0x04003033 RID: 12339
		[Tooltip("How fast the slippery effect blends in/out (units per second)")]
		[SerializeField]
		private float slipBlendSpeed = 3f;

		// Token: 0x04003034 RID: 12340
		[Header("Muddy Material (Optional)")]
		[Tooltip("If assigned, the material will progressively get muddy while in mud")]
		[SerializeField]
		private MuddyMaterial muddyMaterial;

		// Token: 0x04003035 RID: 12341
		[Tooltip("Seconds between each MuddyMaterial increment while in mud")]
		[SerializeField]
		private float mudAccumulationInterval = 1f;

		// Token: 0x04003036 RID: 12342
		[Header("Effect Tuning")]
		[Tooltip("Minimum seconds between effect triggers to avoid spam")]
		[SerializeField]
		private float effectCooldown = 0.3f;

		// Token: 0x04003037 RID: 12343
		[Header("Debug")]
		[SerializeField]
		private bool enableEffectSelectionLogs;

		// Token: 0x04003038 RID: 12344
		[SerializeField]
		private float effectSelectionLogInterval = 0.5f;

		// Token: 0x04003039 RID: 12345
		private bool isInMud;

		// Token: 0x0400303A RID: 12346
		private DynamicMudSurface currentSurface;

		// Token: 0x0400303B RID: 12347
		private List<IFootstepMudEffect> scalableEffects;

		// Token: 0x0400303C RID: 12348
		private List<LivingMudEffectsPlayer> legacyEffects;

		// Token: 0x0400303D RID: 12349
		private float lastEffectTime;

		// Token: 0x0400303E RID: 12350
		private float lastMudAccumulationTime;

		// Token: 0x0400303F RID: 12351
		private float currentIntensity;

		// Token: 0x04003040 RID: 12352
		private float currentSlipFactor = 1f;

		// Token: 0x04003041 RID: 12353
		private float cachedDefaultSpeedMultiplier;

		// Token: 0x04003042 RID: 12354
		private bool slipApplied;

		// Token: 0x04003043 RID: 12355
		private float lastEffectSelectionLogTime;
	}
}
