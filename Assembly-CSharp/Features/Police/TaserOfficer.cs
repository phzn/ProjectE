using System;
using System.Collections;
using Features.Detection;
using Features.GameOver;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RunModes;
using Features.TimeFreeze;
using PoolWaterTools;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DFF RID: 3583
	[DisallowMultipleComponent]
	public class TaserOfficer : MonoBehaviour
	{
		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06006AAD RID: 27309 RVA: 0x002ACFC2 File Offset: 0x002AB1C2
		// (set) Token: 0x06006AAE RID: 27310 RVA: 0x002ACFCA File Offset: 0x002AB1CA
		public bool IsFiring { get; private set; }

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06006AAF RID: 27311 RVA: 0x002ACFD3 File Offset: 0x002AB1D3
		public bool IsFireSequenceActive
		{
			get
			{
				return this._fireSequenceActive;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x002ACFDB File Offset: 0x002AB1DB
		public bool IsReady
		{
			get
			{
				return !TimeFreezeController.IsAnyActive && !this.AttackBlocked && !TaserOfficer.IsSimulationStopped && !this.IsFiring && this._fireCooldownRemaining <= 0f;
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x002AD00D File Offset: 0x002AB20D
		private bool AttackBlocked
		{
			get
			{
				return TaserOfficer.MustAbortAttack || NPCPoolRecovery.IsActorRecovering(this) || PoolWaterRecovery.IsPlayerInWater(this._fireTarget);
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x06006AB2 RID: 27314 RVA: 0x002AD02B File Offset: 0x002AB22B
		public static bool MustAbortAttack
		{
			get
			{
				return !RunModeService.EncountersEnabled || DetectionTransitionGate.IsBlocked || TaserOfficer.IsEncounterProtected || PlayerStealthBridge.IsFullyHidden || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver);
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06006AB3 RID: 27315 RVA: 0x002AD064 File Offset: 0x002AB264
		public static bool IsEncounterProtected
		{
			get
			{
				return (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive);
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06006AB4 RID: 27316 RVA: 0x002A9A51 File Offset: 0x002A7C51
		private static bool IsSimulationStopped
		{
			get
			{
				return NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f;
			}
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x002AD09C File Offset: 0x002AB29C
		public bool IsPlayerInFireRange(Vector3 playerPos)
		{
			float num = Vector3.Distance(this.GetMuzzlePos(), playerPos);
			return num >= this.minFireRange && num <= this.fireRange;
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x002AD0CD File Offset: 0x002AB2CD
		private void Awake()
		{
			this.ResolveTaserRig();
			base.enabled = false;
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x002AD0DC File Offset: 0x002AB2DC
		private void Update()
		{
			if (this.AttackBlocked)
			{
				if (this._fireSequenceActive)
				{
					this.CancelFire();
				}
				return;
			}
			if (this._fireCooldownRemaining <= 0f || NPCSimulationPause.IsPaused)
			{
				return;
			}
			this._fireCooldownRemaining = Mathf.Max(0f, this._fireCooldownRemaining - NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x002AD131 File Offset: 0x002AB331
		private void OnDisable()
		{
			this.CancelFire();
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x002AD13C File Offset: 0x002AB33C
		private void ResolveTaserRig()
		{
			if (this._taserRoot == null)
			{
				this._taserRoot = TaserOfficer.FindDeepChild(base.transform, this.taserRootName);
			}
			if (this._taserRoot == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning(string.Concat(new string[] { "[TaserOfficer.", base.name, "] No '", this.taserRootName, "' GO found in rig. Taser cannot be used." }));
				}
				return;
			}
			if (this._muzzle == null)
			{
				this._muzzle = TaserOfficer.FindDeepChild(this._taserRoot, this.muzzleChildName);
				if (this._muzzle == null)
				{
					this._muzzle = this._taserRoot;
				}
			}
			this.EnsureAudioSource();
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x002AD204 File Offset: 0x002AB404
		private void EnsureAudioSource()
		{
			if (this._audioSource != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("TaserAudio");
			gameObject.transform.SetParent((this._muzzle != null) ? this._muzzle : base.transform, false);
			this._audioSource = gameObject.AddComponent<AudioSource>();
			this._audioSource.playOnAwake = false;
			this._audioSource.spatialBlend = 1f;
			this._audioSource.minDistance = 2f;
			this._audioSource.maxDistance = 20f;
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x002AD29C File Offset: 0x002AB49C
		private static Transform FindDeepChild(Transform root, string name)
		{
			if (root == null || string.IsNullOrEmpty(name))
			{
				return null;
			}
			if (root.name == name)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = TaserOfficer.FindDeepChild(root.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x002AD2F7 File Offset: 0x002AB4F7
		public void SetTaserVisible(bool visible)
		{
			if (this._taserRoot == null)
			{
				this.ResolveTaserRig();
			}
			if (this._taserRoot != null)
			{
				this._taserRoot.gameObject.SetActive(visible);
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06006ABD RID: 27325 RVA: 0x002AD32C File Offset: 0x002AB52C
		public bool HasTaserRig
		{
			get
			{
				return this._taserRoot != null && this._muzzle != null;
			}
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x002AD34C File Offset: 0x002AB54C
		public bool CanFire(StealthActor player)
		{
			if (DetectionTransitionGate.IsBlocked)
			{
				return false;
			}
			if (TaserOfficer.IsSimulationStopped || !this.IsReady || player == null || this._muzzle == null)
			{
				return false;
			}
			if (PoolWaterRecovery.IsPlayerInWater(player.transform))
			{
				return false;
			}
			Vector3 position = player.Position;
			if (!this.IsPlayerInFireRange(position))
			{
				return false;
			}
			Vector3 muzzlePos = this.GetMuzzlePos();
			Vector3 vector = position - muzzlePos;
			float magnitude = vector.magnitude;
			if (magnitude < 0.01f)
			{
				return false;
			}
			if (this.beamObstacleMask != 0)
			{
				RaycastHit[] array = Physics.RaycastAll(muzzlePos, vector / magnitude, magnitude, this.beamObstacleMask, QueryTriggerInteraction.Ignore);
				for (int i = 0; i < array.Length; i++)
				{
					Collider collider = array[i].collider;
					if (!TaserOfficer.IsLivingCollider(collider) && !collider.transform.IsChildOf(base.transform) && !(collider.transform == base.transform))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x002AD44C File Offset: 0x002AB64C
		private static bool IsLivingCollider(Collider col)
		{
			return !(col == null) && (TaserOfficer.IsPoliceCollider(col) || col.GetComponentInParent<StealthActor>() != null || col.GetComponentInParent<NPCStateMachine>() != null || col.GetComponentInParent<NPCSenses>() != null);
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x002AD49F File Offset: 0x002AB69F
		public static bool IsPoliceCollider(Collider col)
		{
			return !(col == null) && (col.GetComponentInParent<TaserOfficer>() != null || col.GetComponentInParent<PoliceOfficerChaser>() != null);
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x002AD4CD File Offset: 0x002AB6CD
		public Coroutine FireAt(StealthActor player)
		{
			if (!base.isActiveAndEnabled || !this.IsReady || player == null || PoolWaterRecovery.IsPlayerInWater(player.transform))
			{
				return null;
			}
			return base.StartCoroutine(this.FireSequence(player));
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x002AD504 File Offset: 0x002AB704
		public void CancelFire()
		{
			base.StopAllCoroutines();
			this._fireSequenceActive = false;
			this._fireTarget = null;
			this.IsFiring = false;
			if (this._audioSource != null)
			{
				this._audioSource.Stop();
			}
			Animator componentInChildren = base.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.runtimeAnimatorController != null)
			{
				componentInChildren.SetBool("TaserAiming", false);
				componentInChildren.ResetTrigger("TaserShoot");
			}
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x002AD57A File Offset: 0x002AB77A
		private IEnumerator FireSequence(StealthActor player)
		{
			this._fireTarget = ((player != null) ? player.transform : null);
			if (this.AttackBlocked)
			{
				this._fireTarget = null;
				yield break;
			}
			this._fireSequenceActive = true;
			this.IsFiring = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[] { "[TaserOfficer.", base.name, "] FireSequence start (player=", player.name, ")" }));
			}
			Animator animator = base.GetComponentInChildren<Animator>();
			if (animator != null)
			{
				animator.SetBool("TaserAiming", true);
			}
			if (this._audioSource != null && this.windUpClip != null)
			{
				this._audioSource.PlayOneShot(this.windUpClip);
			}
			float windUpElapsed = 0f;
			while (windUpElapsed < this.windUpDuration)
			{
				if (this.AttackBlocked)
				{
					this.CancelFire();
					yield break;
				}
				windUpElapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			while (TaserOfficer.IsSimulationStopped || TimeFreezeController.IsAnyActive)
			{
				if (this.AttackBlocked)
				{
					this.CancelFire();
					yield break;
				}
				yield return null;
			}
			if (this.AttackBlocked)
			{
				this.CancelFire();
				yield break;
			}
			Vector3 muzzlePos = this.GetMuzzlePos();
			Vector3 vector = ((player != null) ? (player.Position - muzzlePos).normalized : base.transform.forward);
			if (animator != null)
			{
				animator.SetTrigger("TaserShoot");
			}
			if (this._audioSource != null && this.fireClip != null)
			{
				this._audioSource.PlayOneShot(this.fireClip);
			}
			if (this.boltPrefab != null)
			{
				Object.Instantiate<TaserBoltProjectile>(this.boltPrefab, muzzlePos, Quaternion.LookRotation(vector, Vector3.up)).Init(vector, this.boltSpeed, this.boltMaxDistance, this.boltHitRadius, this.beamHitMask, this.beamObstacleMask, this.hitVariants, this.debugLogs, player);
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[TaserOfficer." + base.name + "] boltPrefab non assegnato: nessun proiettile sparato.");
			}
			this._fireCooldownRemaining = this.fireCooldown;
			this.IsFiring = false;
			float holdElapsed = 0f;
			while (holdElapsed < 0.6f)
			{
				if (this.AttackBlocked)
				{
					this.CancelFire();
					yield break;
				}
				holdElapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			if (animator != null)
			{
				animator.SetBool("TaserAiming", false);
			}
			this._fireSequenceActive = false;
			this._fireTarget = null;
			yield break;
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x002AD590 File Offset: 0x002AB790
		private Vector3 GetMuzzlePos()
		{
			if (!(this._muzzle != null))
			{
				return base.transform.position + Vector3.up;
			}
			return this._muzzle.position;
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x002AD5C4 File Offset: 0x002AB7C4
		private void OnDrawGizmosSelected()
		{
			Vector3 muzzlePos = this.GetMuzzlePos();
			Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
			Gizmos.DrawWireSphere(muzzlePos, this.fireRange);
			Gizmos.color = new Color(1f, 0.2f, 0f, 0.6f);
			Gizmos.DrawWireSphere(muzzlePos, this.minFireRange);
		}

		// Token: 0x04006D64 RID: 28004
		[Header("Fire Range")]
		[Tooltip("Maximum distance at which the officer attempts to fire the Taser. Long-range sniper role.")]
		public float fireRange = 18f;

		// Token: 0x04006D65 RID: 28005
		[Tooltip("Minimum distance under which the officer prefers to keep chasing instead of firing. Tenuto basso (3m) cosi' il taser puo' sparare anche da distanza ravvicinata: a questo range puo' comunque mancare se il player si muove di lato durante il volo del bolt.")]
		public float minFireRange = 3f;

		// Token: 0x04006D66 RID: 28006
		[Header("Timing")]
		[Tooltip("Wind-up duration (s) prima dello sparo. Durante questo tempo c'e' solo audio cue + animazione del NPC che alza il taser (no laser sight visivo). Finestra di pre-dodge: il player ha questo tempo per uscire dalla linea di tiro.")]
		public float windUpDuration = 1.5f;

		// Token: 0x04006D67 RID: 28007
		[Tooltip("Cooldown between fire attempts (s).")]
		public float fireCooldown = 5f;

		// Token: 0x04006D68 RID: 28008
		[Header("Electric Bolt (projectile)")]
		[Tooltip("Prefab del proiettile elettrico sparato a fine wind-up. Deve avere il componente TaserBoltProjectile (configurato a runtime via Init). Tipicamente un GO con ParticleSystem ciccione + trail. Se null, il colpo non parte e viene loggato un warning.")]
		public TaserBoltProjectile boltPrefab;

		// Token: 0x04006D69 RID: 28009
		[Tooltip("Velocita' di volo del bolt (m/s). Più bassa = più schivabile.")]
		public float boltSpeed = 8f;

		// Token: 0x04006D6A RID: 28010
		[Tooltip("Distanza massima percorsa dal bolt prima di auto-distruggersi (m). Dovrebbe essere >= fireRange per coprire l'intera area di mira.")]
		public float boltMaxDistance = 20f;

		// Token: 0x04006D6B RID: 28011
		[Tooltip("Raggio della sfera di collisione del bolt (m). Più grande = più difficile schivare.")]
		public float boltHitRadius = 0.5f;

		// Token: 0x04006D6C RID: 28012
		[Tooltip("Layers su cui il bolt puo' colpire il player (deve includere il layer del Player).")]
		public LayerMask beamHitMask = -1;

		// Token: 0x04006D6D RID: 28013
		[Tooltip("Layers che bloccano il bolt (muri, coperture). Se il bolt entra in collisione con uno di questi layer, si distrugge senza colpire il player.")]
		public LayerMask beamObstacleMask = 0;

		// Token: 0x04006D6E RID: 28014
		[Header("Auto-discovery")]
		[Tooltip("Name of the Taser root GameObject under the right hand bone. This is the GO toggled on/off to show the weapon. Default matches the PoliceBase prefab: hand_r/Taser.")]
		public string taserRootName = "Taser";

		// Token: 0x04006D6F RID: 28015
		[Tooltip("Name of the GameObject to use as beam origin (muzzle). Default matches the PoliceBase prefab: Taser_head.")]
		public string muzzleChildName = "Taser_head";

		// Token: 0x04006D70 RID: 28016
		[Header("Hit Effect (stun anim)")]
		[Tooltip("Lista di animazioni di stun da pescare random quando il beam va a segno. Ogni elemento e' un PlayerActionEntry (prefab vTriggerGenericAction + Duration). Tipicamente 2 entry: Death_ElectricShock01 e Death_ElectricShock02. Se vuoto, il colpo non ha effetto visivo sul player (solo log).")]
		public PlayerActionEntry[] hitVariants;

		// Token: 0x04006D71 RID: 28017
		[Header("Audio")]
		[Tooltip("Wind-up sound (charging) (optional).")]
		public AudioClip windUpClip;

		// Token: 0x04006D72 RID: 28018
		[Tooltip("Fire sound (shot) (optional).")]
		public AudioClip fireClip;

		// Token: 0x04006D73 RID: 28019
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04006D74 RID: 28020
		private Transform _taserRoot;

		// Token: 0x04006D75 RID: 28021
		private Transform _muzzle;

		// Token: 0x04006D76 RID: 28022
		private AudioSource _audioSource;

		// Token: 0x04006D77 RID: 28023
		private float _fireCooldownRemaining;

		// Token: 0x04006D78 RID: 28024
		private bool _fireSequenceActive;

		// Token: 0x04006D79 RID: 28025
		private Transform _fireTarget;
	}
}
