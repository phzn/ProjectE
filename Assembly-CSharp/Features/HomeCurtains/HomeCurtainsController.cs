using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.Rank;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.HomeCurtains
{
	// Token: 0x02001184 RID: 4484
	[DisallowMultipleComponent]
	public class HomeCurtainsController : MonoBehaviour, IInteractionCandidate
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600893F RID: 35135 RVA: 0x00382604 File Offset: 0x00380804
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06008940 RID: 35136 RVA: 0x0038260C File Offset: 0x0038080C
		public bool IsCandidateEligible
		{
			get
			{
				return this._playerContacts > 0 && HomeCurtainsController.Unlocked;
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x06008941 RID: 35137 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06008942 RID: 35138 RVA: 0x00382620 File Offset: 0x00380820
		private static bool Unlocked
		{
			get
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return currentRank != null && currentRank.unlocksWindowFlashing;
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06008943 RID: 35139 RVA: 0x00382644 File Offset: 0x00380844
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x06008944 RID: 35140 RVA: 0x0038264C File Offset: 0x0038084C
		private void Awake()
		{
			this._emotionGateReason = string.Format("{0}.{1}", "CielHome.CurtainsClosed", base.GetInstanceID());
			this.EnsureWindowFrameRenderLayerVisible();
			this.ForceWindowFramesToHighestLod();
			if (this.pivotSx == null)
			{
				this.pivotSx = base.transform.Find("CurtainPivotSx");
			}
			if (this.pivotDx == null)
			{
				this.pivotDx = base.transform.Find("CurtainPivotDx");
			}
			if (this.pivotSx == null || this.pivotDx == null)
			{
				Debug.LogError("[HomeCurtains] Pivot sipario non trovati (CurtainPivotSx/Dx) — esegui il menu 4. Controller disabilitato.", this);
				base.enabled = false;
				return;
			}
			this._closedScaleSx = this.pivotSx.localScale.z;
			this._closedScaleDx = this.pivotDx.localScale.z;
			if (this.cloths == null || this.cloths.Length == 0)
			{
				this.cloths = base.GetComponentsInChildren<MagicaCloth>(true);
			}
			foreach (BoxCollider boxCollider in base.GetComponents<BoxCollider>())
			{
				if (boxCollider.isTrigger)
				{
					this._windowPlaneLocalX = boxCollider.center.x;
					return;
				}
			}
		}

		// Token: 0x06008945 RID: 35141 RVA: 0x0038277C File Offset: 0x0038097C
		private void EnsureWindowFrameRenderLayerVisible()
		{
			int num = LayerMask.NameToLayer("Glass");
			Camera main = Camera.main;
			if (num >= 0 && main != null)
			{
				main.cullingMask |= 1 << num;
			}
		}

		// Token: 0x06008946 RID: 35142 RVA: 0x003827BA File Offset: 0x003809BA
		private void ForceWindowFramesToHighestLod()
		{
			this.RefreshWindowFrameLods(Time.unscaledTime);
		}

		// Token: 0x06008947 RID: 35143 RVA: 0x003827C8 File Offset: 0x003809C8
		private void RefreshWindowFrameLods(float now)
		{
			for (int i = this._windowLodGroups.Count - 1; i >= 0; i--)
			{
				if (!(this._windowLodGroups[i] != null))
				{
					this._windowLodGroups.RemoveAt(i);
					this._windowLodsForced = false;
					this._nextWindowLodRetryTime = float.NegativeInfinity;
				}
			}
			if (this._windowLodsForced)
			{
				return;
			}
			if (now < this._nextWindowLodRetryTime)
			{
				return;
			}
			this._nextWindowLodRetryTime = now + 1f;
			int num = LayerMask.NameToLayer("Glass");
			foreach (LODGroup lodgroup in Object.FindObjectsByType<LODGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!(lodgroup == null) && lodgroup.gameObject.layer == num)
				{
					bool flag = false;
					for (int k = 0; k < HomeCurtainsController.HighQualityWindowLodNames.Length; k++)
					{
						if (!(lodgroup.gameObject.name != HomeCurtainsController.HighQualityWindowLodNames[k]))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						lodgroup.ForceLOD(0);
						if (!this._windowLodGroups.Contains(lodgroup))
						{
							this._windowLodGroups.Add(lodgroup);
						}
					}
				}
			}
			this._windowLodsForced = this._windowLodGroups.Count >= 3;
		}

		// Token: 0x06008948 RID: 35144 RVA: 0x003828FC File Offset: 0x00380AFC
		private void Start()
		{
			this._lastUnlocked = HomeCurtainsController.Unlocked;
			this.SetOpen(this._lastUnlocked, false);
			RankProgressionService.Changed += this.OnRankChanged;
			base.StartCoroutine(this.BindPlayerClothColliders());
		}

		// Token: 0x06008949 RID: 35145 RVA: 0x00382934 File Offset: 0x00380B34
		private void OnEnable()
		{
			this._windowLodsForced = false;
			this._nextWindowLodRetryTime = float.NegativeInfinity;
			InteractionCandidateRegistry.Register(this);
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.ApplyEmotionPolicy(true);
		}

		// Token: 0x0600894A RID: 35146 RVA: 0x00382984 File Offset: 0x00380B84
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			InteractionCandidateRegistry.Unregister(this);
			this.ReleaseEmotionPolicy();
			this._candidateSqrDistance = float.MaxValue;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x0600894B RID: 35147 RVA: 0x003829D2 File Offset: 0x00380BD2
		private void OnDestroy()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			this.ReleaseEmotionPolicy();
			RankProgressionService.Changed -= this.OnRankChanged;
		}

		// Token: 0x0600894C RID: 35148 RVA: 0x003829FC File Offset: 0x00380BFC
		private void OnActiveSceneChanged(Scene from, Scene to)
		{
			this.ApplyEmotionPolicy(true);
		}

		// Token: 0x0600894D RID: 35149 RVA: 0x00382A08 File Offset: 0x00380C08
		private void OnRankChanged()
		{
			bool unlocked = HomeCurtainsController.Unlocked;
			if (unlocked == this._lastUnlocked)
			{
				return;
			}
			this._lastUnlocked = unlocked;
			this._lockedHintVisible = false;
			this.SetOpen(unlocked, true);
		}

		// Token: 0x0600894E RID: 35150 RVA: 0x00382A3C File Offset: 0x00380C3C
		private void Update()
		{
			this.EnsureWindowFrameRenderLayerVisible();
			this.ForceWindowFramesToHighestLod();
			this.ApplyEmotionPolicy(false);
			Transform player = PlayerRegistry.Player;
			if (this._playerContacts <= 0 || player == null)
			{
				this._candidateSqrDistance = float.MaxValue;
				return;
			}
			Vector3 vector = base.transform.InverseTransformPoint(player.position);
			vector.x = this._windowPlaneLocalX;
			this._candidateSqrDistance = (base.transform.TransformPoint(vector) - player.position).sqrMagnitude;
			if (HomeCurtainsController.Unlocked)
			{
				bool flag = InteractionCandidateRegistry.IsWinner(this);
				this.RefreshPrompt();
				if (flag && this.interactInput.GetButtonDown() && InteractionCandidateRegistry.TryCommitWorldInteraction())
				{
					this.SetOpen(!this._isOpen, true);
					return;
				}
			}
			else
			{
				bool flag2 = this.IsPlayerNearWindowPlane();
				if (flag2 != this._lockedHintVisible)
				{
					this._lockedHintVisible = flag2;
					this.RefreshPrompt();
				}
			}
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x00382B1C File Offset: 0x00380D1C
		private bool IsPlayerNearWindowPlane()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return false;
			}
			float num = Mathf.Abs(base.transform.InverseTransformPoint(player.position).x - this._windowPlaneLocalX);
			float num2 = this.lockedHintDistance + (this._lockedHintVisible ? 0.15f : 0f);
			return num <= num2;
		}

		// Token: 0x06008950 RID: 35152 RVA: 0x00382B7E File Offset: 0x00380D7E
		private void OnTriggerEnter(Collider other)
		{
			if (!HomeCurtainsController.IsPlayer(other))
			{
				return;
			}
			this._playerContacts++;
			if (this._playerContacts == 1)
			{
				this.RefreshPrompt();
			}
		}

		// Token: 0x06008951 RID: 35153 RVA: 0x00382BA6 File Offset: 0x00380DA6
		private void OnTriggerExit(Collider other)
		{
			if (!HomeCurtainsController.IsPlayer(other))
			{
				return;
			}
			this._playerContacts = Mathf.Max(0, this._playerContacts - 1);
			if (this._playerContacts == 0)
			{
				this._lockedHintVisible = false;
				this.RefreshPrompt();
			}
		}

		// Token: 0x06008952 RID: 35154 RVA: 0x00382BDC File Offset: 0x00380DDC
		private static bool IsPlayer(Collider c)
		{
			Transform player = PlayerRegistry.Player;
			return player != null && (c.transform == player || c.transform.IsChildOf(player));
		}

		// Token: 0x06008953 RID: 35155 RVA: 0x00382C18 File Offset: 0x00380E18
		private void RefreshPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this._playerContacts > 0 && HomeCurtainsController.Unlocked && InteractionCandidateRegistry.IsWinner(this))
			{
				this.promptUI.Show(this._isOpen ? this.closeConfig : this.openConfig);
				return;
			}
			if (this._playerContacts > 0 && this._lockedHintVisible && this.lockedConfig != null && !InteractionCandidateRegistry.HasEligibleCandidate)
			{
				this.promptUI.Show(this.lockedConfig);
				return;
			}
			this.promptUI.Hide();
		}

		// Token: 0x06008954 RID: 35156 RVA: 0x00382CB4 File Offset: 0x00380EB4
		private void SetOpen(bool open, bool animated)
		{
			this._isOpen = open;
			this.ApplyEmotionPolicy(true);
			if (this._anim != null)
			{
				base.StopCoroutine(this._anim);
				this._anim = null;
			}
			this.SetClothsEnabled(false);
			float num = (open ? (this._closedScaleSx * this.openScale) : this._closedScaleSx);
			float num2 = (open ? (this._closedScaleDx * this.openScale) : this._closedScaleDx);
			if (animated && base.isActiveAndEnabled)
			{
				this._anim = base.StartCoroutine(this.CurtainRoutine(num, num2));
			}
			else
			{
				HomeCurtainsController.ApplyScale(this.pivotSx, num);
				HomeCurtainsController.ApplyScale(this.pivotDx, num2);
				if (!open)
				{
					this.SetClothsEnabled(true);
				}
			}
			this.RefreshPrompt();
		}

		// Token: 0x06008955 RID: 35157 RVA: 0x00382D6C File Offset: 0x00380F6C
		private void ApplyEmotionPolicy(bool force = false)
		{
			Scene scene = base.gameObject.scene;
			Scene activeScene = SceneManager.GetActiveScene();
			bool flag = HomeCurtainsController.IsEmotionPolicySceneOwner(scene, activeScene);
			bool flag2 = HomeCurtainsController.Unlocked && this._isOpen;
			if (!flag || flag2)
			{
				if (this._ownsEmotionPolicy || (!string.IsNullOrEmpty(this._emotionGateReason) && PlayerActivityGate.HasLock(this._emotionGateReason)))
				{
					this.ReleaseEmotionPolicy();
				}
				return;
			}
			if (!PlayerActivityGate.HasLock(this._emotionGateReason))
			{
				PlayerActivityGate.Push(this._emotionGateReason, PlayerActivity.StanceSwitch);
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (this._boundEmotionController != instance)
			{
				if (this._boundEmotionController != null)
				{
					this._boundEmotionController.SetHomeBasePresentationOverride(this, false);
				}
				this._boundEmotionController = instance;
				force = true;
			}
			if (this._boundEmotionController != null && (force || !this._ownsEmotionPolicy))
			{
				this._boundEmotionController.SetHomeBasePresentationOverride(this, true);
			}
			this._ownsEmotionPolicy = true;
		}

		// Token: 0x06008956 RID: 35158 RVA: 0x00382E55 File Offset: 0x00381055
		private static bool IsEmotionPolicySceneOwner(Scene controllerScene, Scene activeScene)
		{
			return controllerScene.IsValid() && controllerScene.isLoaded && activeScene.IsValid() && HomeCurtainsController.IsEmotionPolicySceneIdentity(controllerScene.name, controllerScene.handle, activeScene.handle);
		}

		// Token: 0x06008957 RID: 35159 RVA: 0x00382E8E File Offset: 0x0038108E
		private static bool IsEmotionPolicySceneIdentity(string controllerSceneName, int controllerSceneHandle, int activeSceneHandle)
		{
			return controllerSceneName == "CielHome" && controllerSceneHandle == activeSceneHandle;
		}

		// Token: 0x06008958 RID: 35160 RVA: 0x00382EA4 File Offset: 0x003810A4
		private void ReleaseEmotionPolicy()
		{
			if (!string.IsNullOrEmpty(this._emotionGateReason))
			{
				PlayerActivityGate.Pop(this._emotionGateReason);
			}
			if (this._boundEmotionController != null)
			{
				this._boundEmotionController.SetHomeBasePresentationOverride(this, false);
			}
			this._boundEmotionController = null;
			this._ownsEmotionPolicy = false;
		}

		// Token: 0x06008959 RID: 35161 RVA: 0x00382EF2 File Offset: 0x003810F2
		private IEnumerator CurtainRoutine(float targetSx, float targetDx)
		{
			float fromSx = this.pivotSx.localScale.z;
			float fromDx = this.pivotDx.localScale.z;
			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime / Mathf.Max(0.01f, this.animDuration);
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
				HomeCurtainsController.ApplyScale(this.pivotSx, Mathf.LerpUnclamped(fromSx, targetSx, num));
				HomeCurtainsController.ApplyScale(this.pivotDx, Mathf.LerpUnclamped(fromDx, targetDx, num));
				yield return null;
			}
			HomeCurtainsController.ApplyScale(this.pivotSx, targetSx);
			HomeCurtainsController.ApplyScale(this.pivotDx, targetDx);
			this._anim = null;
			if (!this._isOpen)
			{
				this.SetClothsEnabled(true);
			}
			yield break;
		}

		// Token: 0x0600895A RID: 35162 RVA: 0x00382F10 File Offset: 0x00381110
		private static void ApplyScale(Transform pivot, float z)
		{
			Vector3 localScale = pivot.localScale;
			localScale.z = z;
			pivot.localScale = localScale;
		}

		// Token: 0x0600895B RID: 35163 RVA: 0x00382F34 File Offset: 0x00381134
		private void SetClothsEnabled(bool on)
		{
			if (this.cloths == null)
			{
				return;
			}
			foreach (MagicaCloth magicaCloth in this.cloths)
			{
				if (magicaCloth != null && magicaCloth.enabled != on)
				{
					magicaCloth.enabled = on;
				}
			}
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x00382F7C File Offset: 0x0038117C
		private IEnumerator BindPlayerClothColliders()
		{
			if (this.cloths == null || this.cloths.Length == 0)
			{
				yield break;
			}
			while (PlayerRegistry.Player == null)
			{
				yield return null;
			}
			ColliderComponent[] componentsInChildren = PlayerRegistry.Player.GetComponentsInChildren<ColliderComponent>(true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				Debug.Log("[HomeCurtains] Nessun ColliderComponent Magica sul rig del player: il tessuto non reagira' al contatto.", this);
				yield break;
			}
			foreach (MagicaCloth magicaCloth in this.cloths)
			{
				if (!(magicaCloth == null))
				{
					List<ColliderComponent> colliderList = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					bool flag = false;
					foreach (ColliderComponent colliderComponent in componentsInChildren)
					{
						if (!colliderList.Contains(colliderComponent))
						{
							colliderList.Add(colliderComponent);
							flag = true;
						}
					}
					if (flag && magicaCloth.isActiveAndEnabled)
					{
						magicaCloth.SetParameterChange();
					}
				}
			}
			yield break;
		}

		// Token: 0x04008BD0 RID: 35792
		private const string HomeSceneName = "CielHome";

		// Token: 0x04008BD1 RID: 35793
		private const string EmotionGateReasonPrefix = "CielHome.CurtainsClosed";

		// Token: 0x04008BD2 RID: 35794
		[Header("Pivot sipario (auto-resolve per nome se vuoti)")]
		[SerializeField]
		private Transform pivotSx;

		// Token: 0x04008BD3 RID: 35795
		[SerializeField]
		private Transform pivotDx;

		// Token: 0x04008BD4 RID: 35796
		[Tooltip("Scala Z (asse binario) del pivot a tenda APERTA: 1 = chiusa, ~0.12 = raccolta sul bordo.")]
		[Range(0.02f, 0.9f)]
		[SerializeField]
		private float openScale = 0.12f;

		// Token: 0x04008BD5 RID: 35797
		[Header("Animazione")]
		[Tooltip("Durata (s) della compressione/decompressione a sipario.")]
		[SerializeField]
		private float animDuration = 1.4f;

		// Token: 0x04008BD6 RID: 35798
		[Header("Interazione")]
		[Tooltip("Prompt world-space condiviso (istanza di InteractPromptUI). Wired dal setup tool.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04008BD7 RID: 35799
		[Tooltip("Config del prompt quando le tende sono CHIUSE (azione 'Open').")]
		[SerializeField]
		private InteractionConfigSO openConfig;

		// Token: 0x04008BD8 RID: 35800
		[Tooltip("Config del prompt quando le tende sono APERTE (azione 'Close').")]
		[SerializeField]
		private InteractionConfigSO closeConfig;

		// Token: 0x04008BD9 RID: 35801
		[Tooltip("Config hint quando il rank NON sblocca le tende ('Locked', key vuota = niente chip E). Mostrato solo molto vicino alla finestra.")]
		[SerializeField]
		private InteractionConfigSO lockedConfig;

		// Token: 0x04008BDA RID: 35802
		[Tooltip("Distanza (m) dal piano finestra sotto cui appare l'hint 'Locked' (il prompt normale usa tutto il trigger).")]
		[SerializeField]
		private float lockedHintDistance = 0.9f;

		// Token: 0x04008BDB RID: 35803
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04008BDC RID: 35804
		[Header("Cloth (auto-resolve dai figli se vuoto)")]
		[SerializeField]
		private MagicaCloth[] cloths;

		// Token: 0x04008BDD RID: 35805
		private float _closedScaleSx;

		// Token: 0x04008BDE RID: 35806
		private float _closedScaleDx;

		// Token: 0x04008BDF RID: 35807
		private bool _isOpen;

		// Token: 0x04008BE0 RID: 35808
		private bool _lastUnlocked;

		// Token: 0x04008BE1 RID: 35809
		private int _playerContacts;

		// Token: 0x04008BE2 RID: 35810
		private bool _lockedHintVisible;

		// Token: 0x04008BE3 RID: 35811
		private float _windowPlaneLocalX;

		// Token: 0x04008BE4 RID: 35812
		private Coroutine _anim;

		// Token: 0x04008BE5 RID: 35813
		private bool _windowLodsForced;

		// Token: 0x04008BE6 RID: 35814
		private readonly List<LODGroup> _windowLodGroups = new List<LODGroup>(4);

		// Token: 0x04008BE7 RID: 35815
		private float _nextWindowLodRetryTime;

		// Token: 0x04008BE8 RID: 35816
		private const float WindowLodRetryInterval = 1f;

		// Token: 0x04008BE9 RID: 35817
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x04008BEA RID: 35818
		private PlayerEmotionController _boundEmotionController;

		// Token: 0x04008BEB RID: 35819
		private string _emotionGateReason;

		// Token: 0x04008BEC RID: 35820
		private bool _ownsEmotionPolicy;

		// Token: 0x04008BED RID: 35821
		private static readonly string[] HighQualityWindowLodNames = new string[] { "SM_DoorFrame_Sliding", "SM_Door_Balcony_01" };
	}
}
