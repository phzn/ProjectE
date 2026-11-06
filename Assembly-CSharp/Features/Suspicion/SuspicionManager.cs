using System;
using Features.Detection;
using Features.NPC;
using Features.Police;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Suspicion
{
	// Token: 0x02000C34 RID: 3124
	[DefaultExecutionOrder(-50)]
	public class SuspicionManager : MonoBehaviour
	{
		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005A03 RID: 23043 RVA: 0x0023403F File Offset: 0x0023223F
		// (set) Token: 0x06005A04 RID: 23044 RVA: 0x00234046 File Offset: 0x00232246
		public static SuspicionManager Instance { get; private set; }

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005A05 RID: 23045 RVA: 0x0023404E File Offset: 0x0023224E
		public float CurrentScore
		{
			get
			{
				return this._score;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005A06 RID: 23046 RVA: 0x00234056 File Offset: 0x00232256
		public int CurrentLevel
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x0023405E File Offset: 0x0023225E
		public int MaxLevel
		{
			get
			{
				if (this.levels == null || this.levels.Length == 0)
				{
					return 0;
				}
				return this.levels.Length - 1;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005A08 RID: 23048 RVA: 0x00234080 File Offset: 0x00232280
		public float TimeRemainingAtLevel
		{
			get
			{
				if (this._level <= 0 || this._level >= this.levels.Length)
				{
					return 0f;
				}
				float duration = this.levels[this._level].duration;
				if (duration <= 0f)
				{
					return 0f;
				}
				return Mathf.Max(0f, duration - this._timeAtCurrentLevel);
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005A09 RID: 23049 RVA: 0x002340E0 File Offset: 0x002322E0
		public float NextLevelProgress
		{
			get
			{
				if (this._level >= this.MaxLevel)
				{
					return 1f;
				}
				float scoreToEnter = this.levels[this._level].scoreToEnter;
				float scoreToEnter2 = this.levels[this._level + 1].scoreToEnter;
				if (scoreToEnter2 <= scoreToEnter)
				{
					return 1f;
				}
				return Mathf.Clamp01((this._score - scoreToEnter) / (scoreToEnter2 - scoreToEnter));
			}
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x00234144 File Offset: 0x00232344
		private void Awake()
		{
			if (SuspicionManager.Instance != null && SuspicionManager.Instance != this)
			{
				Debug.LogWarning("[SuspicionManager] Awake: duplicate instance on '" + base.name + "', destroying.");
				Object.Destroy(base.gameObject);
				return;
			}
			SuspicionManager.Instance = this;
			Debug.Log("[SuspicionManager] Awake: Instance set on '" + base.name + "'.");
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x002341B1 File Offset: 0x002323B1
		private void OnDestroy()
		{
			this.UnregisterDirectorListener();
			if (SuspicionManager.Instance == this)
			{
				SuspicionManager.Instance = null;
			}
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x002341CC File Offset: 0x002323CC
		private void Start()
		{
			this.TryRegisterDirectorListener();
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x002341D4 File Offset: 0x002323D4
		private void Update()
		{
			if (!this._listenerRegistered)
			{
				this.TryRegisterDirectorListener();
			}
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._timerFrozen)
			{
				return;
			}
			if (this._level <= 0)
			{
				return;
			}
			if (this._level >= this.levels.Length)
			{
				return;
			}
			float duration = this.levels[this._level].duration;
			if (duration <= 0f)
			{
				return;
			}
			if (PoliceManager.Instance != null && PoliceManager.Instance.AnyOfficerHasLineOfSight)
			{
				this._timeAtCurrentLevel = 0f;
				return;
			}
			this._timeAtCurrentLevel += NPCSimulationPause.SimulationDeltaTime;
			if (this._timeAtCurrentLevel >= duration)
			{
				this.DemoteOneLevel();
			}
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x0023428C File Offset: 0x0023248C
		private void TryRegisterDirectorListener()
		{
			if (this._listenerRegistered)
			{
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance == null)
			{
				return;
			}
			if (instance.OnDetectionStarted == null)
			{
				Debug.LogWarning("[SuspicionManager] Director.Instance found but OnDetectionStarted UnityEvent is null!");
				return;
			}
			instance.OnDetectionStarted.AddListener(new UnityAction<NPCSenses>(this.HandleDetectionStarted));
			if (instance.OnSequenceComplete != null)
			{
				instance.OnSequenceComplete.AddListener(new UnityAction(this.HandleSequenceComplete));
			}
			this._listenerRegistered = true;
			Debug.Log("[SuspicionManager] Registered to Director.OnDetectionStarted + OnSequenceComplete.");
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x0023430C File Offset: 0x0023250C
		private void UnregisterDirectorListener()
		{
			if (!this._listenerRegistered)
			{
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null)
			{
				instance.OnDetectionStarted.RemoveListener(new UnityAction<NPCSenses>(this.HandleDetectionStarted));
				if (instance.OnSequenceComplete != null)
				{
					instance.OnSequenceComplete.RemoveListener(new UnityAction(this.HandleSequenceComplete));
				}
			}
			this._listenerRegistered = false;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x0023436E File Offset: 0x0023256E
		private void HandleSequenceComplete()
		{
			this._timerFrozen = false;
			if (this.debugLogs)
			{
				Debug.Log("[SuspicionManager] OnSequenceComplete → timer ripreso.");
			}
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x0023438C File Offset: 0x0023258C
		private void HandleDetectionStarted(NPCSenses npc)
		{
			if (this.debugLogs)
			{
				Debug.Log("[SuspicionManager] HandleDetectionStarted fired: npc=" + ((npc != null) ? npc.name : "<null>"));
			}
			this._timerFrozen = true;
			if (npc == null)
			{
				return;
			}
			SuspicionSource suspicionSource = npc.GetComponent<SuspicionSource>();
			if (suspicionSource == null)
			{
				suspicionSource = npc.GetComponentInParent<SuspicionSource>();
			}
			float num = ((suspicionSource != null) ? suspicionSource.suspicionDelta : 1f);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[SuspicionManager] resolved delta={0} (source={1})", num, (suspicionSource != null) ? suspicionSource.GetType().Name : "<null, fallback to 1>"));
			}
			if (num <= 0f)
			{
				if (this.debugLogs)
				{
					Debug.Log("[SuspicionManager] Detection from " + npc.name + " ignored (delta=0)");
				}
				return;
			}
			float num2 = ((this._level < this.MaxLevel) ? this.levels[this._level + 1].scoreToEnter : this._score);
			this.AddSuspicion(Mathf.Max(0f, num2 - this._score), npc);
			this._timeAtCurrentLevel = 0f;
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x002344B8 File Offset: 0x002326B8
		public void AddSuspicion(float delta, NPCSenses source = null)
		{
			if (delta <= 0f)
			{
				return;
			}
			float score = this._score;
			this._score = Mathf.Clamp(this._score + delta, 0f, this.scoreCap);
			if (!Mathf.Approximately(score, this._score))
			{
				UnityEvent<float> onScoreChanged = this.OnScoreChanged;
				if (onScoreChanged != null)
				{
					onScoreChanged.Invoke(this._score);
				}
			}
			this._timeAtCurrentLevel = 0f;
			int num = this.ComputeLevelFromScore(this._score);
			if (num > this._level)
			{
				int level = this._level;
				this._level = num;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[SuspicionManager] Level changed: {0} → {1} (score={2:F2}, '{3}')", new object[]
					{
						level,
						this._level,
						this._score,
						this.levels[this._level].name
					}));
				}
				UnityEvent<int> onLevelChanged = this.OnLevelChanged;
				if (onLevelChanged != null)
				{
					onLevelChanged.Invoke(this._level);
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[SuspicionManager] +{0} from {1} → score={2:F2}, level={3} ('{4}')", new object[]
				{
					delta,
					(source != null) ? source.name : "<null>",
					this._score,
					this._level,
					this.levels[this._level].name
				}));
			}
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x00234624 File Offset: 0x00232824
		public void SetScore(float newScore, bool silent = false)
		{
			float num = Mathf.Clamp(newScore, 0f, this.scoreCap);
			if (Mathf.Approximately(num, this._score))
			{
				return;
			}
			this._score = num;
			if (!silent)
			{
				UnityEvent<float> onScoreChanged = this.OnScoreChanged;
				if (onScoreChanged != null)
				{
					onScoreChanged.Invoke(this._score);
				}
			}
			int num2 = this.ComputeLevelFromScore(this._score);
			if (num2 > this._level)
			{
				int level = this._level;
				this._level = num2;
				this._timeAtCurrentLevel = 0f;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[SuspicionManager] SetScore promoted: {0} → {1} (score={2:F2})", level, this._level, this._score));
				}
				UnityEvent<int> onLevelChanged = this.OnLevelChanged;
				if (onLevelChanged == null)
				{
					return;
				}
				onLevelChanged.Invoke(this._level);
			}
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x002346EC File Offset: 0x002328EC
		public void MaximizeSuspicion()
		{
			if (this.levels == null || this.levels.Length == 0)
			{
				return;
			}
			this.SetScore(Mathf.Max(this._score, this.levels[this.MaxLevel].scoreToEnter), false);
			this._timeAtCurrentLevel = 0f;
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x0023473C File Offset: 0x0023293C
		public void ResetSuspicion()
		{
			this._score = 0f;
			this._timeAtCurrentLevel = 0f;
			UnityEvent<float> onScoreChanged = this.OnScoreChanged;
			if (onScoreChanged != null)
			{
				onScoreChanged.Invoke(this._score);
			}
			if (this._level != 0)
			{
				this._level = 0;
				if (this.debugLogs)
				{
					Debug.Log("[SuspicionManager] ResetSuspicion → level 0");
				}
				UnityEvent<int> onLevelChanged = this.OnLevelChanged;
				if (onLevelChanged == null)
				{
					return;
				}
				onLevelChanged.Invoke(this._level);
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x002347AD File Offset: 0x002329AD
		public bool TryDemoteOneLevel()
		{
			if (this._level <= 0)
			{
				return false;
			}
			this.DemoteOneLevel();
			return true;
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x002347C4 File Offset: 0x002329C4
		private void DemoteOneLevel()
		{
			if (this._level <= 0)
			{
				return;
			}
			int level = this._level;
			this._level = level - 1;
			this._timeAtCurrentLevel = 0f;
			float num = ((level < this.levels.Length) ? this.levels[level].scoreToEnter : this._score);
			float scoreToEnter = this.levels[this._level].scoreToEnter;
			this._score = Mathf.Clamp(Mathf.Min(this._score, Mathf.Max(scoreToEnter, num - 0.0001f)), 0f, this.scoreCap);
			UnityEvent<float> onScoreChanged = this.OnScoreChanged;
			if (onScoreChanged != null)
			{
				onScoreChanged.Invoke(this._score);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[SuspicionManager] Timer expired: level {0} → {1} ('{2}', score allineato a {3:F2})", new object[]
				{
					level,
					this._level,
					this.levels[this._level].name,
					this._score
				}));
			}
			UnityEvent<int> onLevelChanged = this.OnLevelChanged;
			if (onLevelChanged == null)
			{
				return;
			}
			onLevelChanged.Invoke(this._level);
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x002348DC File Offset: 0x00232ADC
		private int ComputeLevelFromScore(float score)
		{
			if (this.levels == null || this.levels.Length == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < this.levels.Length && score >= this.levels[num2].scoreToEnter)
			{
				num = num2;
				num2++;
			}
			return num;
		}

		// Token: 0x04005AFF RID: 23295
		[Header("Levels (index 0 = base, ultimo = max)")]
		[Tooltip("Configurazione dei livelli di suspicion. Aggiungi/togli elementi per scalare il sistema. L'indice nell'array E' il livello (0 = Calm, 1 = Exposed, 2 = Wanted, ...). scoreToEnter del livello [i] deve essere >= scoreToEnter del livello [i-1].")]
		public SuspicionManager.SuspicionLevel[] levels = new SuspicionManager.SuspicionLevel[]
		{
			new SuspicionManager.SuspicionLevel
			{
				name = "Calm",
				scoreToEnter = 0f,
				duration = 0f
			},
			new SuspicionManager.SuspicionLevel
			{
				name = "Exposed",
				scoreToEnter = 1f,
				duration = 180f
			},
			new SuspicionManager.SuspicionLevel
			{
				name = "Wanted",
				scoreToEnter = 2f,
				duration = 180f
			},
			new SuspicionManager.SuspicionLevel
			{
				name = "Most Wanted",
				scoreToEnter = 3f,
				duration = 300f
			}
		};

		// Token: 0x04005B00 RID: 23296
		[Header("Score")]
		[Tooltip("Hard cap on the accumulator score to avoid runaway accumulation while idle at high level.")]
		[Min(1f)]
		public float scoreCap = 10f;

		// Token: 0x04005B01 RID: 23297
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04005B02 RID: 23298
		[Header("Events")]
		[Tooltip("Fired when the integer Suspicion Level changes.")]
		public UnityEvent<int> OnLevelChanged;

		// Token: 0x04005B03 RID: 23299
		[Tooltip("Fired ogni volta che lo score cambia (continuous). Listener dev'essere leggero.")]
		public UnityEvent<float> OnScoreChanged;

		// Token: 0x04005B04 RID: 23300
		private float _score;

		// Token: 0x04005B05 RID: 23301
		private int _level;

		// Token: 0x04005B06 RID: 23302
		private float _timeAtCurrentLevel;

		// Token: 0x04005B07 RID: 23303
		private bool _listenerRegistered;

		// Token: 0x04005B08 RID: 23304
		private bool _timerFrozen;

		// Token: 0x02000C35 RID: 3125
		[Serializable]
		public class SuspicionLevel
		{
			// Token: 0x04005B09 RID: 23305
			[Tooltip("Nome leggibile del livello (es. 'Calm', 'Exposed', 'Wanted'). Solo per debug/UI.")]
			public string name = "Level";

			// Token: 0x04005B0A RID: 23306
			[Tooltip("Score minimo richiesto per entrare in questo livello. Il livello 0 deve avere 0.")]
			[Min(0f)]
			public float scoreToEnter;

			// Token: 0x04005B0B RID: 23307
			[Tooltip("Secondi di permanenza al livello PRIMA di scendere automaticamente al livello precedente. 0 = nessun decay temporale (rimane indefinitamente). Una nuova detection mentre il timer scorre lo RESETTA.")]
			[Min(0f)]
			public float duration;
		}
	}
}
