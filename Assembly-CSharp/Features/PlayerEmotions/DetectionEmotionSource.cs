using System;
using Features.Detection;
using UnityEngine;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E15 RID: 3605
	public class DetectionEmotionSource : MonoBehaviour, IContinuousEmotionSource, IEmotionSource
	{
		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x06006B38 RID: 27448 RVA: 0x002AF28A File Offset: 0x002AD48A
		// (set) Token: 0x06006B39 RID: 27449 RVA: 0x002AF291 File Offset: 0x002AD491
		public static DetectionEmotionSource Instance { get; private set; }

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06006B3A RID: 27450 RVA: 0x002AF299 File Offset: 0x002AD499
		public string SourceId
		{
			get
			{
				return "Detection";
			}
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x06006B3B RID: 27451 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public EmotionSourceCategory Category
		{
			get
			{
				return EmotionSourceCategory.Detection;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06006B3C RID: 27452 RVA: 0x002AF2A0 File Offset: 0x002AD4A0
		public EmotionPriority Priority
		{
			get
			{
				return EmotionPriority.High;
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06006B3D RID: 27453 RVA: 0x002AF2A4 File Offset: 0x002AD4A4
		public bool IsActive
		{
			get
			{
				return this._isPreDetectionActive;
			}
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x002AF2AC File Offset: 0x002AD4AC
		private void Awake()
		{
			if (DetectionEmotionSource.Instance != null && DetectionEmotionSource.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			DetectionEmotionSource.Instance = this;
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x002AF2DA File Offset: 0x002AD4DA
		private void Start()
		{
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.RegisterContinuousSource(this);
			}
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x002AF2F4 File Offset: 0x002AD4F4
		private void OnDestroy()
		{
			if (DetectionEmotionSource.Instance == this)
			{
				DetectionEmotionSource.Instance = null;
			}
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.UnregisterContinuousSource(this.SourceId);
			}
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x002AF328 File Offset: 0x002AD528
		public EmotionImpactData GetContinuousImpact()
		{
			if (this._isFlashCoverActive)
			{
				EmotionSystemConfig config = this.Config;
				if (((config != null) ? config.FlashCoverActive : null) != null)
				{
					EmotionImpactSO flashCoverActive = this.Config.FlashCoverActive;
					EmotionManager instance = EmotionManager.Instance;
					EmotionImpactData emotionImpactData = flashCoverActive.CalculateImpact(instance != null && instance.IsPlayerNaked, 0f, 1f);
					emotionImpactData.ApplicationMode = EmotionApplicationMode.PerSecond;
					return emotionImpactData;
				}
			}
			if (!this._isPreDetectionActive || this._currentAttention <= 0.01f || this.Config == null)
			{
				return null;
			}
			float num = (this.Config.PreDetectionCurve ?? AnimationCurve.Linear(0f, 0f, 1f, 1f)).Evaluate(this._currentAttention);
			EmotionImpactSO emotionImpactSO = null;
			if (this._currentDetectionSource == DetectionSource.Audio && this.Config.AudioPreDetection != null)
			{
				emotionImpactSO = this.Config.AudioPreDetection;
			}
			else if (this._currentDetectionSource == DetectionSource.Visual && this.Config.VisualPreDetection != null)
			{
				emotionImpactSO = this.Config.VisualPreDetection;
			}
			if (emotionImpactSO == null)
			{
				return null;
			}
			EmotionImpactSO emotionImpactSO2 = emotionImpactSO;
			EmotionManager instance2 = EmotionManager.Instance;
			EmotionImpactData emotionImpactData2 = emotionImpactSO2.CalculateImpact(instance2 != null && instance2.IsPlayerNaked, 0f, num);
			emotionImpactData2.ApplicationMode = EmotionApplicationMode.PerSecond;
			if (this._stanceAwarePreDetection && EmotionManager.Instance != null)
			{
				float num2 = Mathf.Abs(emotionImpactData2.Value);
				bool flag = EmotionManager.Instance.CurrentStance == EmotionStance.Confident;
				emotionImpactData2.Value = (flag ? num2 : (-num2));
			}
			return emotionImpactData2;
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x002AF4A7 File Offset: 0x002AD6A7
		public void SetStanceAwarePreDetection(bool enabled)
		{
			this._stanceAwarePreDetection = enabled;
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x002AF4B0 File Offset: 0x002AD6B0
		public void UpdatePreDetection(DetectionSource source, float attention)
		{
			this._isPreDetectionActive = attention > 0.01f;
			this._currentDetectionSource = source;
			this._currentAttention = attention;
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x002AF4CE File Offset: 0x002AD6CE
		public void StopPreDetection()
		{
			this._isPreDetectionActive = false;
			this._currentAttention = 0f;
			this._currentDetectionSource = DetectionSource.None;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x002AF4EC File Offset: 0x002AD6EC
		public void OnDetection(float npcDistance = 0f)
		{
			if (this.Config == null || this.Config.FullDetection == null || EmotionManager.Instance == null)
			{
				return;
			}
			EmotionManager.Instance.OnActiveEventStarted();
			EmotionManager.Instance.ApplyImpact(this.Config.FullDetection, npcDistance, 1f);
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x002AF550 File Offset: 0x002AD750
		public void OnPlayerChoice(PlayerChoiceType choice, float npcDistance = 0f)
		{
			if (this.Config == null || EmotionManager.Instance == null)
			{
				return;
			}
			EmotionImpactSO emotionImpactSO = null;
			switch (choice)
			{
			case PlayerChoiceType.Shame:
				emotionImpactSO = this.Config.ShameChoice;
				break;
			case PlayerChoiceType.Expose:
				emotionImpactSO = this.Config.ExposeChoice;
				break;
			case PlayerChoiceType.Flee:
				emotionImpactSO = this.Config.FleeChoice;
				break;
			}
			if (emotionImpactSO != null)
			{
				EmotionManager.Instance.ApplyImpact(emotionImpactSO, npcDistance, 1f);
			}
		}

		// Token: 0x06006B47 RID: 27463 RVA: 0x002AF5D4 File Offset: 0x002AD7D4
		public void OnSuccessfulEscape()
		{
			if (this.Config == null || this.Config.SuccessfulEscape == null || EmotionManager.Instance == null)
			{
				return;
			}
			EmotionManager.Instance.ApplyImpact(this.Config.SuccessfulEscape, 0f, 1f);
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x002AF62F File Offset: 0x002AD82F
		public void OnDetectionEnded()
		{
			this.StopPreDetection();
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnActiveEventEnded();
		}

		// Token: 0x06006B49 RID: 27465 RVA: 0x002AF646 File Offset: 0x002AD846
		public void OnFlashCoverEquipped()
		{
			this._isFlashCoverActive = true;
			this._hasFlashedThisSession = false;
			if (this.debugLogs)
			{
				Debug.Log("[DetectionEmotionSource] Flash cover equipped — continuous impact started.");
			}
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x002AF668 File Offset: 0x002AD868
		public void OnFlashCoverUnequipped()
		{
			this._isFlashCoverActive = false;
			EmotionSystemConfig config = this.Config;
			if (((config != null) ? config.FlashCoverUnequip : null) != null && EmotionManager.Instance != null)
			{
				EmotionManager.Instance.ApplyImpact(this.Config.FlashCoverUnequip, 0f, 1f);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DetectionEmotionSource] Flash cover unequipped — flashed: {0}", this._hasFlashedThisSession));
			}
			this._hasFlashedThisSession = false;
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x002AF6EC File Offset: 0x002AD8EC
		public void OnFlashExecuted(float npcDistance = 0f)
		{
			this._hasFlashedThisSession = true;
			EmotionSystemConfig config = this.Config;
			if (((config != null) ? config.FlashExecution : null) != null && EmotionManager.Instance != null)
			{
				if (!this._flashSupremeWindowActive)
				{
					this._flashSupremeWindowActive = true;
					this._preFlashBalance = EmotionManager.Instance.EmotionBalance;
				}
				this._flashSupremeUntil = Time.time + Mathf.Max(0.1f, this.Config.FlashSupremeDuration);
				EmotionManager.Instance.ApplyImpact(this.Config.FlashExecution, npcDistance, 1f);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DetectionEmotionSource] Flash executed — impact applied at distance {0}, ", npcDistance) + string.Format("supreme window {0:0.#}s (preBalance={1:0.#})", this.Config.FlashSupremeDuration, this._preFlashBalance));
				}
			}
		}

		// Token: 0x06006B4C RID: 27468 RVA: 0x002AF7D0 File Offset: 0x002AD9D0
		private void Update()
		{
			if (!this._flashSupremeWindowActive || Time.time < this._flashSupremeUntil)
			{
				return;
			}
			this._flashSupremeWindowActive = false;
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (instance.IsInActiveEvent || instance.IsDecaySuspended)
			{
				return;
			}
			instance.SetBalance(this._preFlashBalance, "FlashSupremeExpired");
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DetectionEmotionSource] Flash supreme window expired — balance restored to {0:0.#}", this._preFlashBalance));
			}
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x06006B4D RID: 27469 RVA: 0x002AF84E File Offset: 0x002ADA4E
		public bool IsFlashCoverActive
		{
			get
			{
				return this._isFlashCoverActive;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06006B4E RID: 27470 RVA: 0x002AF856 File Offset: 0x002ADA56
		public bool HasFlashedThisSession
		{
			get
			{
				return this._hasFlashedThisSession;
			}
		}

		// Token: 0x04006DF5 RID: 28149
		[Header("=== SYSTEM CONFIG ===")]
		[Tooltip("Central configuration for all emotion impacts")]
		public EmotionSystemConfig Config;

		// Token: 0x04006DF6 RID: 28150
		[Header("=== DEBUG ===")]
		[Tooltip("Enable detailed debug logging")]
		public bool debugLogs;

		// Token: 0x04006DF7 RID: 28151
		private bool _isPreDetectionActive;

		// Token: 0x04006DF8 RID: 28152
		private DetectionSource _currentDetectionSource;

		// Token: 0x04006DF9 RID: 28153
		private float _currentAttention;

		// Token: 0x04006DFA RID: 28154
		private bool _isFlashCoverActive;

		// Token: 0x04006DFB RID: 28155
		private bool _hasFlashedThisSession;

		// Token: 0x04006DFC RID: 28156
		private bool _stanceAwarePreDetection;

		// Token: 0x04006DFD RID: 28157
		private bool _flashSupremeWindowActive;

		// Token: 0x04006DFE RID: 28158
		private float _flashSupremeUntil;

		// Token: 0x04006DFF RID: 28159
		private float _preFlashBalance;
	}
}
