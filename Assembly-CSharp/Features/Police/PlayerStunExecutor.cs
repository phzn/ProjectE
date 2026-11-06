using System;
using Features.Detection;
using Features.GameOver;
using Features.PlayerEmotions;
using Features.TimeFreeze;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using PoolWaterTools;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Police
{
	// Token: 0x02000DED RID: 3565
	[DisallowMultipleComponent]
	public class PlayerStunExecutor : MonoBehaviour
	{
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060069B2 RID: 27058 RVA: 0x002A5B5D File Offset: 0x002A3D5D
		// (set) Token: 0x060069B3 RID: 27059 RVA: 0x002A5B64 File Offset: 0x002A3D64
		public static PlayerStunExecutor Instance { get; private set; }

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060069B4 RID: 27060 RVA: 0x002A5B6C File Offset: 0x002A3D6C
		// (set) Token: 0x060069B5 RID: 27061 RVA: 0x002A5B74 File Offset: 0x002A3D74
		public bool IsStunned { get; private set; }

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060069B6 RID: 27062 RVA: 0x002A5B7D File Offset: 0x002A3D7D
		// (set) Token: 0x060069B7 RID: 27063 RVA: 0x002A5B85 File Offset: 0x002A3D85
		public int WaterEntryGeneration { get; private set; }

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060069B8 RID: 27064 RVA: 0x002A5B8E File Offset: 0x002A3D8E
		// (set) Token: 0x060069B9 RID: 27065 RVA: 0x002A5B96 File Offset: 0x002A3D96
		public bool ExternalLockHold { get; private set; }

		// Token: 0x060069BA RID: 27066 RVA: 0x002A5BA0 File Offset: 0x002A3DA0
		public void SetExternalLockHold(bool hold)
		{
			this.ExternalLockHold = hold;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PlayerStunExecutor] ExternalLockHold = {0}", hold));
			}
			if (hold)
			{
				if (this.playerInput != null && !this.playerInput.lockInput)
				{
					this.playerInput.SetLockAllInput(true);
					return;
				}
			}
			else if (!this.IsStunned && this.playerInput != null && !TaserOfficer.IsEncounterProtected)
			{
				this.playerInput.SetLockAllInput(false);
			}
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x002A5C28 File Offset: 0x002A3E28
		private void Awake()
		{
			if (PlayerStunExecutor.Instance != null && PlayerStunExecutor.Instance != this)
			{
				Debug.LogWarning("[PlayerStunExecutor] Awake: duplicate instance on '" + base.name + "', destroying.");
				Object.Destroy(this);
				return;
			}
			PlayerStunExecutor.Instance = this;
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponentInChildren<vThirdPersonInput>();
			}
			if (this.emotionController == null)
			{
				this.emotionController = base.GetComponentInChildren<PlayerEmotionController>();
			}
			if (this.emotionController == null)
			{
				this.emotionController = PlayerEmotionController.Instance;
			}
			if (this.electroshockClip != null)
			{
				TaserSfx.RegisterElectroshockClip(this.electroshockClip);
			}
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x002A5CF6 File Offset: 0x002A3EF6
		private void OnDestroy()
		{
			this.UnsubscribeFromActiveTrigger();
			if (PlayerStunExecutor.Instance == this)
			{
				PlayerStunExecutor.Instance = null;
			}
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x002A5D14 File Offset: 0x002A3F14
		public void PlayStun(PlayerActionEntry stunEntry)
		{
			if (TaserOfficer.MustAbortAttack || TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (PoolWaterRecovery.IsPlayerInWater((this.playerInput != null) ? this.playerInput.transform : base.transform))
			{
				return;
			}
			if (this.IsStunned)
			{
				if (this.debugLogs)
				{
					Debug.Log("[PlayerStunExecutor] PlayStun ignored: already stunned");
				}
				return;
			}
			if (stunEntry == null || stunEntry.TriggerPrefab == null)
			{
				Debug.LogWarning("[PlayerStunExecutor] PlayStun: stunEntry or its TriggerPrefab is null, ignored.");
				return;
			}
			if (this.playerActionAnimator == null)
			{
				Debug.LogWarning("[PlayerStunExecutor] PlayStun: playerActionAnimator non assegnato, ignored.");
				return;
			}
			if (!DetectionPlayerNormalizer.PrepareForEncounter(null))
			{
				return;
			}
			this._stunGeneration++;
			this.IsStunned = true;
			if (this.debugLogs)
			{
				Debug.Log("[PlayerStunExecutor] Stun ON: '" + stunEntry.DisplayName + "'");
			}
			this.SuppressFaceLayer();
			TaserSfx.PlayShockOn(base.gameObject);
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(true);
				this.playerInput.cc.input = Vector2.zero;
				this.playerInput.cc.isSprinting = false;
				if (this.playerInput.cc.animator != null)
				{
					this.playerInput.cc.animator.SetFloat("InputHorizontal", 0f);
					this.playerInput.cc.animator.SetFloat("InputVertical", 0f);
					this.playerInput.cc.animator.SetFloat("InputMagnitude", 0f);
				}
			}
			this._activeEntry = stunEntry;
			this._activeTrigger = stunEntry.TriggerPrefab;
			if (this._activeTrigger != null)
			{
				this._activeTrigger.OnEndAnimation.AddListener(new UnityAction(this.HandleStunAnimationEnd));
			}
			this.playerActionAnimator.TriggerAction(stunEntry);
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x002A5F04 File Offset: 0x002A4104
		public void CancelForSceneTransition()
		{
			this._stunGeneration++;
			this.UnsubscribeFromActiveTrigger();
			if (this.IsStunned && this.playerActionAnimator != null)
			{
				this.playerActionAnimator.CancelCurrentAction();
			}
			this.RestoreFaceLayer();
			this._activeEntry = null;
			this.IsStunned = false;
			this.SetExternalLockHold(false);
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x002A5F64 File Offset: 0x002A4164
		public void CancelForWaterEntry()
		{
			if (!this.IsStunned && !this.ExternalLockHold)
			{
				return;
			}
			int waterEntryGeneration = this.WaterEntryGeneration;
			this.WaterEntryGeneration = waterEntryGeneration + 1;
			if (TaserOfficer.IsEncounterProtected || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver))
			{
				this.ForgetStunOwnedByPreviousFlow();
				return;
			}
			this._stunGeneration++;
			this.UnsubscribeFromActiveTrigger();
			bool isStunned = this.IsStunned;
			this.IsStunned = false;
			this._activeEntry = null;
			if (isStunned && this.playerActionAnimator != null)
			{
				this.playerActionAnimator.CancelCurrentAction();
			}
			if (this._faceLayerSuppressed)
			{
				this._faceLayerSuppressed = false;
				Animator animator = ((this.emotionController != null) ? this.emotionController.PlayerAnimator : null);
				if (animator != null && this._faceLayerIndex >= 0)
				{
					animator.SetLayerWeight(this._faceLayerIndex, this._savedFaceLayerWeight);
				}
			}
			this.SetExternalLockHold(false);
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x002A6054 File Offset: 0x002A4254
		private void HandleStunAnimationEnd()
		{
			if (!this.IsStunned)
			{
				return;
			}
			if (TaserOfficer.IsEncounterProtected)
			{
				this.ForgetStunOwnedByPreviousFlow();
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[PlayerStunExecutor] OnEndAnimation fired → restore");
			}
			this.ApplyPostStunEmotion();
			this.RestoreFaceLayer();
			this.UnsubscribeFromActiveTrigger();
			if (this._activeEntry != null && this._activeEntry.WaitForExitState && this.playerActionAnimator != null)
			{
				int generation = this._stunGeneration;
				this.playerActionAnimator.FinishCurrentActionWaitForExitState(delegate
				{
					if (this._stunGeneration == generation)
					{
						this.FinalizeStunRestore();
					}
				}, 5f);
				return;
			}
			if (this.playerActionAnimator != null)
			{
				float smoothFinishDuration = this.playerActionAnimator.SmoothFinishDuration;
				this.playerActionAnimator.SmoothFinishDuration = this.stunExitBlendDuration;
				this.playerActionAnimator.FinishCurrentAction();
				this.playerActionAnimator.SmoothFinishDuration = smoothFinishDuration;
			}
			this.FinalizeStunRestore();
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x002A6140 File Offset: 0x002A4340
		private void FinalizeStunRestore()
		{
			if (!this.IsStunned)
			{
				return;
			}
			if (TaserOfficer.IsEncounterProtected)
			{
				this.ForgetStunOwnedByPreviousFlow();
				return;
			}
			if (this.playerActionAnimator != null)
			{
				this.playerActionAnimator.RestoreOverrideLayer();
			}
			if (this.playerInput != null && !this.ExternalLockHold)
			{
				this.playerInput.SetLockAllInput(false);
			}
			this._activeEntry = null;
			this.IsStunned = false;
			if (this.debugLogs)
			{
				Debug.Log("[PlayerStunExecutor] Stun OFF, player riprende controllo");
			}
		}

		// Token: 0x060069C2 RID: 27074 RVA: 0x002A61BF File Offset: 0x002A43BF
		private void ForgetStunOwnedByPreviousFlow()
		{
			this._stunGeneration++;
			this.UnsubscribeFromActiveTrigger();
			this._activeEntry = null;
			this.IsStunned = false;
			this.ExternalLockHold = false;
			this._faceLayerSuppressed = false;
		}

		// Token: 0x060069C3 RID: 27075 RVA: 0x002A61F4 File Offset: 0x002A43F4
		private void SuppressFaceLayer()
		{
			if (this._faceLayerSuppressed)
			{
				return;
			}
			Animator animator = ((this.emotionController != null) ? this.emotionController.PlayerAnimator : null);
			if (animator == null)
			{
				return;
			}
			if (this._faceLayerIndex < 0)
			{
				this._faceLayerIndex = animator.GetLayerIndex(this.faceLayerName);
			}
			if (this._faceLayerIndex < 0)
			{
				Debug.LogWarning("[PlayerStunExecutor] Layer Face '" + this.faceLayerName + "' non trovato: la TaserdFace baked potrebbe essere coperta.");
				return;
			}
			this._savedFaceLayerWeight = animator.GetLayerWeight(this._faceLayerIndex);
			animator.SetLayerWeight(this._faceLayerIndex, 0f);
			this._faceLayerSuppressed = true;
		}

		// Token: 0x060069C4 RID: 27076 RVA: 0x002A629C File Offset: 0x002A449C
		private void RestoreFaceLayer()
		{
			if (!this._faceLayerSuppressed)
			{
				return;
			}
			this._faceLayerSuppressed = false;
			Animator animator = ((this.emotionController != null) ? this.emotionController.PlayerAnimator : null);
			if (animator == null || this._faceLayerIndex < 0)
			{
				return;
			}
			if (this.forceShameOnStunEnd && !string.IsNullOrEmpty(this.embSupremeFaceStateName))
			{
				if (this.emotionController != null)
				{
					this.emotionController.SetExpression(4);
				}
				if (animator.HasState(this._faceLayerIndex, Animator.StringToHash(this.embSupremeFaceStateName)))
				{
					animator.Play(this.embSupremeFaceStateName, this._faceLayerIndex, 0f);
				}
			}
			animator.SetLayerWeight(this._faceLayerIndex, this._savedFaceLayerWeight);
		}

		// Token: 0x060069C5 RID: 27077 RVA: 0x002A635C File Offset: 0x002A455C
		private void ApplyPostStunEmotion()
		{
			if (!this.forceShameOnStunEnd)
			{
				return;
			}
			if (this.emotionController != null)
			{
				this.emotionController.RestoreFacialExpression();
			}
			EmotionManager instance = EmotionManager.Instance;
			if (instance != null)
			{
				instance.SetBalance(this.postStunBalance, "PostStun");
			}
			if (this.emotionController != null)
			{
				this.emotionController.TriggerExpressionUpdate();
			}
		}

		// Token: 0x060069C6 RID: 27078 RVA: 0x002A63C4 File Offset: 0x002A45C4
		private void UnsubscribeFromActiveTrigger()
		{
			if (this._activeTrigger == null)
			{
				return;
			}
			this._activeTrigger.OnEndAnimation.RemoveListener(new UnityAction(this.HandleStunAnimationEnd));
			this._activeTrigger = null;
		}

		// Token: 0x04006C9B RID: 27803
		[Header("Refs (assegna in Inspector)")]
		[Tooltip("PlayerActionAnimator sul player (gestisce vTriggerGenericAction + restore emotion layers).")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04006C9C RID: 27804
		[Tooltip("vThirdPersonInput sul player (lock/unlock input).")]
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04006C9D RID: 27805
		[Tooltip("PlayerEmotionController sul player (usato per leggere l'Animator). Auto-fetch se non assegnato.")]
		[SerializeField]
		private PlayerEmotionController emotionController;

		// Token: 0x04006C9E RID: 27806
		[Header("Face")]
		[Tooltip("La TaserdFace e' ora BAKED nelle clip PoliceTaser1/2 (curve blendshape sul Base layer). Perche' si veda, durante lo stun va azzerato il weight del layer 'Face' (che ha mask sui blendshape del volto e weight=1 di default, quindi altrimenti la coprirebbe). Nome del layer Face nell'animator del player.")]
		[SerializeField]
		private string faceLayerName = "Face";

		// Token: 0x04006C9F RID: 27807
		[Header("Post-Stun Emotion")]
		[Tooltip("Se true, a fine stun il player NON torna allo stato emotivo precedente: forziamo EmotionBalance a 'postStunBalance' (Shame pieno) cosi' Ciel si rialza in EmbSupreme (faccia + layer Embarrassed coerenti). Lo stato emotivo reale diventa quel valore.")]
		[SerializeField]
		private bool forceShameOnStunEnd = true;

		// Token: 0x04006CA0 RID: 27808
		[Tooltip("Valore di EmotionBalance imposto a fine stun quando 'forceShameOnStunEnd' e' true. -100 = Shame massimo (EmbSupreme).")]
		[Range(-100f, 100f)]
		[SerializeField]
		private float postStunBalance = -100f;

		// Token: 0x04006CA1 RID: 27809
		[Tooltip("Nome dello stato EmbSupreme nel layer Face. Quando riaccendiamo il layer Face all'inizio del rialzo, lo forziamo direttamente a questo stato (animator.Play) PRIMA di alzarne il weight, cosi' non mostra per un frame la faccia pre-taser mentre la transizione su FaceExpressionIndex viene valutata.")]
		[SerializeField]
		private string embSupremeFaceStateName = "Emb Face Supreme";

		// Token: 0x04006CA2 RID: 27810
		[Header("Blending")]
		[Tooltip("Crossfade duration verso locomotion alla fine dello stun. Override TEMPORANEO del PlayerActionAnimator.SmoothFinishDuration: viene ripristinato il valore originale subito dopo. Tipicamente 0.4-0.5s per ammorbidire la transizione da clip di morte (personaggio a terra) verso Idle (in piedi).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float stunExitBlendDuration = 0.45f;

		// Token: 0x04006CA3 RID: 27811
		[Header("SFX")]
		[Tooltip("Clip 'electroshock' suonata sul bersaglio (player o NPC) quando viene colpito. Registrata in TaserSfx a Awake cosi' ogni sistema (TaseredState, Reporter fallback, questo PlayerStunExecutor) puo' chiamarla da una single source of truth.")]
		[SerializeField]
		private AudioClip electroshockClip;

		// Token: 0x04006CA4 RID: 27812
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006CA8 RID: 27816
		private vTriggerGenericAction _activeTrigger;

		// Token: 0x04006CA9 RID: 27817
		private PlayerActionEntry _activeEntry;

		// Token: 0x04006CAA RID: 27818
		private int _stunGeneration;

		// Token: 0x04006CAB RID: 27819
		private int _faceLayerIndex = -1;

		// Token: 0x04006CAC RID: 27820
		private float _savedFaceLayerWeight;

		// Token: 0x04006CAD RID: 27821
		private bool _faceLayerSuppressed;
	}
}
