using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FEE RID: 4078
	[DisallowMultipleComponent]
	public class BreathController : MonoBehaviour
	{
		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06007B78 RID: 31608 RVA: 0x0032725D File Offset: 0x0032545D
		public BreathController.BreathLevel CurrentLevel
		{
			get
			{
				return this._currentLevel;
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06007B79 RID: 31609 RVA: 0x00327265 File Offset: 0x00325465
		// (set) Token: 0x06007B7A RID: 31610 RVA: 0x0032726D File Offset: 0x0032546D
		public bool BreathEnabled
		{
			get
			{
				return this.breathEnabled;
			}
			set
			{
				if (this.breathEnabled == value)
				{
					return;
				}
				this.breathEnabled = value;
				if (!this.breathEnabled)
				{
					this.DeactivateAll();
				}
			}
		}

		// Token: 0x06007B7B RID: 31611 RVA: 0x0032728E File Offset: 0x0032548E
		private void Awake()
		{
			this._animator = base.GetComponentInChildren<Animator>();
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
			}
		}

		// Token: 0x06007B7C RID: 31612 RVA: 0x003272B6 File Offset: 0x003254B6
		private void Start()
		{
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
			}
			this.SetupPrefabs();
		}

		// Token: 0x06007B7D RID: 31613 RVA: 0x003272D8 File Offset: 0x003254D8
		private void SetupPrefabs()
		{
			if (this._setupDone)
			{
				return;
			}
			string text;
			Transform transform = this.ResolveAttachBone(out text);
			this._restInstance = this.SpawnPrefab(this.restPrefab, transform);
			this._smallInstance = this.SpawnPrefab(this.smallPrefab, transform);
			this._mediumInstance = this.SpawnPrefab(this.mediumPrefab, transform);
			this._highInstance = this.SpawnPrefab(this.highPrefab, transform);
			this._setupDone = true;
			if (this.debugMode)
			{
				Debug.Log(string.Concat(new string[] { "[Breath] Setup completato su '", transform.name, "' (via ", text, ")." }));
			}
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x00327388 File Offset: 0x00325588
		private GameObject SpawnPrefab(GameObject prefab, Transform parent)
		{
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(prefab, parent);
			gameObject.transform.localPosition = this.localPositionOffset;
			gameObject.transform.localRotation = Quaternion.Euler(this.localRotationOffset);
			gameObject.SetActive(false);
			return gameObject;
		}

		// Token: 0x06007B7F RID: 31615 RVA: 0x003273D5 File Offset: 0x003255D5
		private void OnDisable()
		{
			this.DeactivateAll();
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x003273DD File Offset: 0x003255DD
		private void OnDestroy()
		{
			this.DestroyInstance(ref this._restInstance);
			this.DestroyInstance(ref this._smallInstance);
			this.DestroyInstance(ref this._mediumInstance);
			this.DestroyInstance(ref this._highInstance);
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x0032740F File Offset: 0x0032560F
		private void DestroyInstance(ref GameObject go)
		{
			if (go == null)
			{
				return;
			}
			Object.Destroy(go);
			go = null;
		}

		// Token: 0x06007B82 RID: 31618 RVA: 0x00327428 File Offset: 0x00325628
		private void Update()
		{
			if (!this._setupDone)
			{
				return;
			}
			if (!this.breathEnabled)
			{
				if (this.AnyActive())
				{
					this.DeactivateAll();
				}
				return;
			}
			if (this.heartbeat == null)
			{
				this.heartbeat = Object.FindObjectOfType<HeartbeatController>();
				if (this.heartbeat == null)
				{
					return;
				}
			}
			float num = Mathf.Clamp01(this.heartbeat.CurrentIntensity);
			BreathController.BreathLevel breathLevel = this.ComputeTargetLevel(num, this._currentLevel);
			if (breathLevel == this._currentLevel)
			{
				return;
			}
			if (Time.time - this._lastSwitchTime < this.minStateHold)
			{
				return;
			}
			this.SwitchTo(breathLevel);
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x003274C4 File Offset: 0x003256C4
		private BreathController.BreathLevel ComputeTargetLevel(float intensity, BreathController.BreathLevel current)
		{
			switch (current)
			{
			case BreathController.BreathLevel.Rest:
				if (intensity >= this.highEnterThreshold)
				{
					return BreathController.BreathLevel.High;
				}
				if (intensity >= this.mediumEnterThreshold)
				{
					return BreathController.BreathLevel.Medium;
				}
				if (intensity >= this.smallEnterThreshold)
				{
					return BreathController.BreathLevel.Small;
				}
				return BreathController.BreathLevel.Rest;
			case BreathController.BreathLevel.Small:
				if (intensity >= this.highEnterThreshold)
				{
					return BreathController.BreathLevel.High;
				}
				if (intensity >= this.mediumEnterThreshold)
				{
					return BreathController.BreathLevel.Medium;
				}
				if (intensity < this.smallExitThreshold)
				{
					return BreathController.BreathLevel.Rest;
				}
				return BreathController.BreathLevel.Small;
			case BreathController.BreathLevel.Medium:
				if (intensity >= this.highEnterThreshold)
				{
					return BreathController.BreathLevel.High;
				}
				if (intensity >= this.mediumExitThreshold)
				{
					return BreathController.BreathLevel.Medium;
				}
				if (intensity < this.smallEnterThreshold)
				{
					return BreathController.BreathLevel.Rest;
				}
				return BreathController.BreathLevel.Small;
			case BreathController.BreathLevel.High:
				if (intensity >= this.highExitThreshold)
				{
					return BreathController.BreathLevel.High;
				}
				if (intensity >= this.mediumEnterThreshold)
				{
					return BreathController.BreathLevel.Medium;
				}
				if (intensity >= this.smallEnterThreshold)
				{
					return BreathController.BreathLevel.Small;
				}
				return BreathController.BreathLevel.Rest;
			default:
				return BreathController.BreathLevel.Rest;
			}
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x0032757C File Offset: 0x0032577C
		private void SwitchTo(BreathController.BreathLevel level)
		{
			this.DeactivateAll();
			GameObject gameObject = this.InstanceFor(level);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				BreathController.PlayEffects(gameObject);
			}
			this._currentLevel = level;
			this._lastSwitchTime = Time.time;
			if (this.debugMode)
			{
				Debug.Log(string.Format("[Breath] Switch → {0} (intensity={1:F2})", level, this.heartbeat.CurrentIntensity));
			}
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x003275EC File Offset: 0x003257EC
		private GameObject InstanceFor(BreathController.BreathLevel level)
		{
			switch (level)
			{
			case BreathController.BreathLevel.Rest:
				return this._restInstance;
			case BreathController.BreathLevel.Small:
				return this._smallInstance;
			case BreathController.BreathLevel.Medium:
				return this._mediumInstance;
			case BreathController.BreathLevel.High:
				return this._highInstance;
			default:
				return null;
			}
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x00327624 File Offset: 0x00325824
		private bool AnyActive()
		{
			return (this._restInstance != null && this._restInstance.activeSelf) || (this._smallInstance != null && this._smallInstance.activeSelf) || (this._mediumInstance != null && this._mediumInstance.activeSelf) || (this._highInstance != null && this._highInstance.activeSelf);
		}

		// Token: 0x06007B87 RID: 31623 RVA: 0x0032769F File Offset: 0x0032589F
		private void DeactivateAll()
		{
			BreathController.DeactivateInstance(this._restInstance);
			BreathController.DeactivateInstance(this._smallInstance);
			BreathController.DeactivateInstance(this._mediumInstance);
			BreathController.DeactivateInstance(this._highInstance);
			this._currentLevel = BreathController.BreathLevel.Rest;
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x003276D4 File Offset: 0x003258D4
		private static void DeactivateInstance(GameObject go)
		{
			if (go == null || !go.activeSelf)
			{
				return;
			}
			BreathController.StopEffects(go);
			go.SetActive(false);
		}

		// Token: 0x06007B89 RID: 31625 RVA: 0x003276F8 File Offset: 0x003258F8
		private Transform ResolveAttachBone(out string source)
		{
			Transform transform = ((this._animator != null) ? this._animator.transform : base.transform);
			if (this.useCustomBone && !string.IsNullOrWhiteSpace(this.customBonePath))
			{
				Transform transform2 = BreathController.FindChildByPath(transform, this.customBonePath.Trim());
				if (transform2 != null)
				{
					source = "path '" + this.customBonePath + "'";
					return transform2;
				}
			}
			if (this.useCustomBone && !string.IsNullOrWhiteSpace(this.customBoneName))
			{
				Transform transform3 = BreathController.FindChildByName(transform, this.customBoneName.Trim());
				if (transform3 != null)
				{
					source = "name '" + this.customBoneName + "'";
					return transform3;
				}
			}
			if (this._animator != null)
			{
				Transform boneTransform = this._animator.GetBoneTransform(this.attachBone);
				if (boneTransform != null)
				{
					source = string.Format("humanoid {0}", this.attachBone);
					return boneTransform;
				}
			}
			source = "root (fallback)";
			return base.transform;
		}

		// Token: 0x06007B8A RID: 31626 RVA: 0x00327808 File Offset: 0x00325A08
		private static Transform FindChildByName(Transform root, string name)
		{
			if (root == null)
			{
				return null;
			}
			if (root.name == name)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = BreathController.FindChildByName(root.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06007B8B RID: 31627 RVA: 0x0032785C File Offset: 0x00325A5C
		private static Transform FindChildByPath(Transform root, string path)
		{
			if (root == null || string.IsNullOrEmpty(path))
			{
				return null;
			}
			Transform transform = root.Find(path);
			if (transform != null)
			{
				return transform;
			}
			string[] array = path.Split('/', StringSplitOptions.None);
			string text = array[array.Length - 1];
			Transform transform2 = BreathController.FindChildByName(root, text);
			if (transform2 == null)
			{
				return null;
			}
			Transform transform3 = transform2;
			for (int i = array.Length - 2; i >= 0; i--)
			{
				transform3 = transform3.parent;
				if (transform3 == null || transform3.name != array[i])
				{
					return null;
				}
			}
			return transform2;
		}

		// Token: 0x06007B8C RID: 31628 RVA: 0x003278F4 File Offset: 0x00325AF4
		private static void PlayEffects(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			foreach (ParticleSystem particleSystem in root.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem == null))
				{
					particleSystem.Play(true);
				}
			}
			foreach (VisualEffect visualEffect in root.GetComponentsInChildren<VisualEffect>(true))
			{
				if (!(visualEffect == null))
				{
					visualEffect.Play();
				}
			}
			foreach (AudioSource audioSource in root.GetComponentsInChildren<AudioSource>(true))
			{
				if (!(audioSource == null))
				{
					audioSource.Stop();
					if (audioSource.clip != null)
					{
						audioSource.Play();
					}
				}
			}
		}

		// Token: 0x06007B8D RID: 31629 RVA: 0x003279B0 File Offset: 0x00325BB0
		private static void StopEffects(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			foreach (ParticleSystem particleSystem in root.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem == null))
				{
					particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
			foreach (VisualEffect visualEffect in root.GetComponentsInChildren<VisualEffect>(true))
			{
				if (!(visualEffect == null))
				{
					visualEffect.Stop();
				}
			}
			foreach (AudioSource audioSource in root.GetComponentsInChildren<AudioSource>(true))
			{
				if (!(audioSource == null))
				{
					audioSource.Stop();
				}
			}
		}

		// Token: 0x04007EFF RID: 32511
		[Header("Toggle")]
		[Tooltip("Flag master: se false, tutti i prefab vengono spenti ma restano instanziati. Riattivabile a runtime senza hitch.")]
		[SerializeField]
		private bool breathEnabled = true;

		// Token: 0x04007F00 RID: 32512
		[Header("References")]
		[Tooltip("Sorgente dell'intensita'. Se null, viene cercato in scena allo Start.")]
		[SerializeField]
		private HeartbeatController heartbeat;

		// Token: 0x04007F01 RID: 32513
		[Header("Attach Bone")]
		[Tooltip("Osso humanoid a cui ancorare i prefab del breath (default: Head).")]
		[SerializeField]
		private HumanBodyBones attachBone = HumanBodyBones.Head;

		// Token: 0x04007F02 RID: 32514
		[Tooltip("Se true, prova prima CustomBonePath/Name e poi ricade sull'AttachBone humanoid.")]
		[SerializeField]
		private bool useCustomBone;

		// Token: 0x04007F03 RID: 32515
		[Tooltip("Path gerarchico relativo al root del personaggio (es. 'Armature/Hips/Spine/Head/BreathSpawn').")]
		[SerializeField]
		private string customBonePath = "";

		// Token: 0x04007F04 RID: 32516
		[Tooltip("Nome esatto di un Transform discendente. Usato se CustomBonePath fallisce.")]
		[SerializeField]
		private string customBoneName = "";

		// Token: 0x04007F05 RID: 32517
		[Tooltip("Offset locale di posizione applicato a ogni prefab instanziato.")]
		[SerializeField]
		private Vector3 localPositionOffset = Vector3.zero;

		// Token: 0x04007F06 RID: 32518
		[Tooltip("Offset locale di rotazione (euler) applicato a ogni prefab instanziato.")]
		[SerializeField]
		private Vector3 localRotationOffset = Vector3.zero;

		// Token: 0x04007F07 RID: 32519
		[Header("Prefabs per livello")]
		[Tooltip("Prefab usato quando l'intensita' e' sotto la soglia 'mediumEnter'. Lascia null per niente respiro a riposo.")]
		[SerializeField]
		private GameObject restPrefab;

		// Token: 0x04007F08 RID: 32520
		[Tooltip("Prefab usato quando l'intensita' supera 'mediumEnterThreshold' ma resta sotto 'highEnterThreshold'.")]
		[SerializeField]
		private GameObject smallPrefab;

		// Token: 0x04007F09 RID: 32521
		[Tooltip("Prefab usato come livello medio. NOTA: con questo schema 3-livelli, 'Small' copre il range basso e 'Medium' quello intermedio.")]
		[SerializeField]
		private GameObject mediumPrefab;

		// Token: 0x04007F0A RID: 32522
		[Tooltip("Prefab usato quando l'intensita' supera 'highEnterThreshold'.")]
		[SerializeField]
		private GameObject highPrefab;

		// Token: 0x04007F0B RID: 32523
		[Header("Soglie con hysteresis")]
		[Tooltip("Sopra questo valore si entra in livello Small (da Rest).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float smallEnterThreshold = 0.1f;

		// Token: 0x04007F0C RID: 32524
		[Tooltip("Sotto questo valore si torna a Rest (deve essere <= smallEnter per hysteresis).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float smallExitThreshold = 0.07f;

		// Token: 0x04007F0D RID: 32525
		[Tooltip("Sopra questo valore si passa a Medium.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float mediumEnterThreshold = 0.4f;

		// Token: 0x04007F0E RID: 32526
		[Tooltip("Sotto questo valore si torna a Small (deve essere <= mediumEnter).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float mediumExitThreshold = 0.35f;

		// Token: 0x04007F0F RID: 32527
		[Tooltip("Sopra questo valore si passa a High.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float highEnterThreshold = 0.7f;

		// Token: 0x04007F10 RID: 32528
		[Tooltip("Sotto questo valore si torna a Medium (deve essere <= highEnter).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float highExitThreshold = 0.65f;

		// Token: 0x04007F11 RID: 32529
		[Tooltip("Tempo minimo (secondi) di permanenza in uno stato prima di poter switchare. Evita scatti rapidi se l'intensita' oscilla.")]
		[Min(0f)]
		[SerializeField]
		private float minStateHold = 0.5f;

		// Token: 0x04007F12 RID: 32530
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F13 RID: 32531
		private GameObject _restInstance;

		// Token: 0x04007F14 RID: 32532
		private GameObject _smallInstance;

		// Token: 0x04007F15 RID: 32533
		private GameObject _mediumInstance;

		// Token: 0x04007F16 RID: 32534
		private GameObject _highInstance;

		// Token: 0x04007F17 RID: 32535
		private Animator _animator;

		// Token: 0x04007F18 RID: 32536
		private BreathController.BreathLevel _currentLevel;

		// Token: 0x04007F19 RID: 32537
		private float _lastSwitchTime = -999f;

		// Token: 0x04007F1A RID: 32538
		private bool _setupDone;

		// Token: 0x02000FEF RID: 4079
		public enum BreathLevel
		{
			// Token: 0x04007F1C RID: 32540
			Rest,
			// Token: 0x04007F1D RID: 32541
			Small,
			// Token: 0x04007F1E RID: 32542
			Medium,
			// Token: 0x04007F1F RID: 32543
			High
		}
	}
}
