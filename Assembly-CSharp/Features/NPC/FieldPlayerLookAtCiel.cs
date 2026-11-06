using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EC4 RID: 3780
	[DisallowMultipleComponent]
	public class FieldPlayerLookAtCiel : MonoBehaviour
	{
		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06007124 RID: 28964 RVA: 0x002DEF21 File Offset: 0x002DD121
		public float EffectiveLookRange
		{
			get
			{
				return NpcVisionDifficultyPolicy.ApplyRadius(this.lookRange);
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06007125 RID: 28965 RVA: 0x002DEF2E File Offset: 0x002DD12E
		public float EffectiveExitRange
		{
			get
			{
				return NpcVisionDifficultyPolicy.ApplyRadius(this.lookRange + this.exitHysteresis);
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06007126 RID: 28966 RVA: 0x002DEF42 File Offset: 0x002DD142
		// (set) Token: 0x06007127 RID: 28967 RVA: 0x002DEF4A File Offset: 0x002DD14A
		internal NPCAmbientSuspension ProximitySuspension { get; set; }

		// Token: 0x06007128 RID: 28968 RVA: 0x002DEF54 File Offset: 0x002DD154
		private void Awake()
		{
			if (this.lookAt == null || !this.lookAt.transform.IsChildOf(base.transform))
			{
				this.lookAt = base.GetComponent<NPCSimpleLookAt>() ?? base.GetComponentInChildren<NPCSimpleLookAt>();
			}
			if (this.animator == null || !this.animator.transform.IsChildOf(base.transform))
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			if (this.animator != null)
			{
				this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			}
			this.CacheExpressionParam();
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x002DEFEF File Offset: 0x002DD1EF
		private void OnEnable()
		{
			this._nextCheckTime = 0f;
			NPCSimulationPause.RegisterParticipant(this);
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x002DF002 File Offset: 0x002DD202
		private void OnDisable()
		{
			if (this._isLooking)
			{
				NPCSimpleLookAt npcsimpleLookAt = this.lookAt;
				if (npcsimpleLookAt != null)
				{
					npcsimpleLookAt.StopLooking();
				}
				this.ApplyExpression(this.neutralExpressionIndex);
				this._isLooking = false;
			}
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x002DF030 File Offset: 0x002DD230
		private void Update()
		{
			if (this.ProximitySuspension != null && this.ProximitySuspension.IsSuspended)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (!PlayerExposureState.IsExposed)
			{
				if (this._isLooking)
				{
					this.StopLooking();
				}
				return;
			}
			if (Time.time < this._nextCheckTime)
			{
				return;
			}
			this._nextCheckTime = Time.time + this.checkInterval;
			Transform transform = this.ResolveTarget();
			if (transform == null)
			{
				if (this._isLooking)
				{
					this.StopLooking();
				}
				return;
			}
			float sqrMagnitude = (transform.position - base.transform.position).sqrMagnitude;
			float effectiveLookRange = this.EffectiveLookRange;
			float num = effectiveLookRange * effectiveLookRange;
			float effectiveExitRange = this.EffectiveExitRange;
			float num2 = effectiveExitRange * effectiveExitRange;
			bool flag = sqrMagnitude <= (this._isLooking ? num2 : num);
			if (this._sight == null)
			{
				this._sight = new NPCPassiveLineOfSight(this, (base.GetComponent<SeatedRandomReaction>() != null) ? 1.1f : 1.6f);
			}
			bool flag2 = flag && this._sight.CanSee(transform);
			if (!this._isLooking && flag2)
			{
				this.StartLooking(transform);
				return;
			}
			if (this._isLooking && !flag2)
			{
				this.StopLooking();
			}
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x002DF160 File Offset: 0x002DD360
		private Transform ResolveTarget()
		{
			if (this.manualTarget != null)
			{
				return this.manualTarget;
			}
			if (this._cachedPlayer != null && this._cachedPlayer.gameObject.activeInHierarchy)
			{
				return this._cachedPlayer;
			}
			if (string.IsNullOrEmpty(this.playerTag))
			{
				return null;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.playerTag);
			this._cachedPlayer = ((gameObject != null) ? gameObject.transform : null);
			return this._cachedPlayer;
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x002DF1E4 File Offset: 0x002DD3E4
		private void StartLooking(Transform target)
		{
			this._isLooking = true;
			NPCSimpleLookAt npcsimpleLookAt = this.lookAt;
			if (npcsimpleLookAt != null)
			{
				npcsimpleLookAt.StartLookingAt(target);
			}
			this.ApplyExpression(this.expressionWhenLooking);
			if (this.debugLog)
			{
				Debug.Log("[FieldLookAt] " + base.name + ": started looking at " + target.name);
			}
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x002DF240 File Offset: 0x002DD440
		private void StopLooking()
		{
			this._isLooking = false;
			NPCSimpleLookAt npcsimpleLookAt = this.lookAt;
			if (npcsimpleLookAt != null)
			{
				npcsimpleLookAt.StopLooking();
			}
			this.ApplyExpression(this.neutralExpressionIndex);
			if (this.debugLog)
			{
				Debug.Log("[FieldLookAt] " + base.name + ": stopped looking");
			}
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x002DF294 File Offset: 0x002DD494
		private void CacheExpressionParam()
		{
			this._hasExpressionParam = false;
			if (this.animator == null || string.IsNullOrEmpty(this.expressionParamName))
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && animatorControllerParameter.name == this.expressionParamName)
				{
					this._expressionParamHash = animatorControllerParameter.nameHash;
					this._hasExpressionParam = true;
					return;
				}
			}
			if (this.debugLog)
			{
				Debug.LogWarning(string.Concat(new string[] { "[FieldLookAt] ", base.name, ": animator parameter '", this.expressionParamName, "' (int) not found" }));
			}
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x002DF354 File Offset: 0x002DD554
		private void ApplyExpression(int value)
		{
			if (!this._hasExpressionParam || value < 0)
			{
				return;
			}
			this.animator.SetInteger(this._expressionParamHash, value);
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x002DF378 File Offset: 0x002DD578
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			Gizmos.color = (this._isLooking ? Color.green : new Color(1f, 1f, 0f, 0.4f));
			Gizmos.DrawWireSphere(base.transform.position, this.lookRange);
			Gizmos.color = new Color(1f, 0.4f, 0f, 0.25f);
			Gizmos.DrawWireSphere(base.transform.position, this.lookRange + this.exitHysteresis);
		}

		// Token: 0x040073EA RID: 29674
		[Header("Look At Controller")]
		[Tooltip("NPCSimpleLookAt sullo stesso GameObject (o nei figli). Auto-trovato se vuoto.")]
		public NPCSimpleLookAt lookAt;

		// Token: 0x040073EB RID: 29675
		[Header("Target")]
		[Tooltip("Tag del giocatore da cercare. Default 'Player' come nel resto della codebase.")]
		public string playerTag = "Player";

		// Token: 0x040073EC RID: 29676
		[Tooltip("Se assegnato manualmente bypassa la ricerca per tag.")]
		public Transform manualTarget;

		// Token: 0x040073ED RID: 29677
		[Header("Range")]
		[Tooltip("Distanza massima entro cui il giocatore in campo guarda Ciel (metri).")]
		[Min(0.1f)]
		public float lookRange = 12f;

		// Token: 0x040073EE RID: 29678
		[Tooltip("Distanza di uscita = lookRange + questo valore (isteresi per evitare flicker ai bordi).")]
		[Min(0f)]
		public float exitHysteresis = 1f;

		// Token: 0x040073EF RID: 29679
		[Tooltip("Intervallo (secondi) tra i check di distanza. Più alto = meno CPU sui 50+ giocatori in campo.")]
		[Min(0f)]
		public float checkInterval = 0.25f;

		// Token: 0x040073F0 RID: 29680
		[Header("Facial Expression (opzionale)")]
		[Tooltip("Animator su cui scrivere il parametro espressione. Auto-trovato se vuoto.")]
		public Animator animator;

		// Token: 0x040073F1 RID: 29681
		[Tooltip("Nome del parametro int sull'Animator. 'FaceExpressionIndex' per NPCMasterAnimator, 'ReactionIndex' per NPC_StadiumFan, vuoto per disabilitare.")]
		public string expressionParamName = "FaceExpressionIndex";

		// Token: 0x040073F2 RID: 29682
		[Tooltip("Indice espressione da impostare quando Ciel è nel range visivo.")]
		public int expressionWhenLooking = 1;

		// Token: 0x040073F3 RID: 29683
		[Tooltip("Indice espressione da ripristinare quando Ciel esce dal range. -1 = non resettare.")]
		public int neutralExpressionIndex;

		// Token: 0x040073F4 RID: 29684
		[Header("Debug")]
		public bool debugLog;

		// Token: 0x040073F5 RID: 29685
		public bool drawGizmos;

		// Token: 0x040073F6 RID: 29686
		private Transform _cachedPlayer;

		// Token: 0x040073F8 RID: 29688
		private NPCPassiveLineOfSight _sight;

		// Token: 0x040073F9 RID: 29689
		private float _nextCheckTime;

		// Token: 0x040073FA RID: 29690
		private bool _isLooking;

		// Token: 0x040073FB RID: 29691
		private int _expressionParamHash;

		// Token: 0x040073FC RID: 29692
		private bool _hasExpressionParam;
	}
}
