using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000983 RID: 2435
	[DisallowMultipleComponent]
	public sealed class PhotoBoothMirrorSwitch : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004315 RID: 17173 RVA: 0x00191340 File Offset: 0x0018F540
		public InteractionConfigSO Config
		{
			get
			{
				if (!(this._runtimeConfig != null))
				{
					return this.interactionConfigTemplate;
				}
				return this._runtimeConfig;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public bool IsBeingUsed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004317 RID: 17175 RVA: 0x0019135D File Offset: 0x0018F55D
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004318 RID: 17176 RVA: 0x00191365 File Offset: 0x0018F565
		public bool IsCandidateEligible
		{
			get
			{
				return this._eligible;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004319 RID: 17177 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600431A RID: 17178 RVA: 0x0019136D File Offset: 0x0018F56D
		public bool IsMirrorOn
		{
			get
			{
				return this.reflection != null && this.reflection.IsReflectionEnabled;
			}
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0019138A File Offset: 0x0018F58A
		public void Configure(PhotoBoothPlanarMirror targetReflection, Renderer targetRenderer, PhotoBoothController owner, BoxCollider bounds, InteractablePromptUI prompt, InteractionConfigSO config)
		{
			this.reflection = targetReflection;
			this.mirrorRenderer = targetRenderer;
			this.booth = owner;
			this.interiorBounds = bounds;
			this.promptUI = prompt;
			this.interactionConfigTemplate = config;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x001913B9 File Offset: 0x0018F5B9
		public void ConfigureStandalone(PhotoBoothPlanarMirror targetReflection, Renderer targetRenderer, InteractablePromptUI prompt, InteractionConfigSO config, Vector3 surfaceNormal, string displayName)
		{
			this.Configure(targetReflection, targetRenderer, null, null, prompt, config);
			this.standaloneMirror = true;
			this.localSurfaceNormal = surfaceNormal.normalized;
			this.mirrorDisplayName = displayName;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x001913E4 File Offset: 0x0018F5E4
		private void Awake()
		{
			if (this.interactInput == null)
			{
				this.interactInput = new GenericInput("F", "X", "X");
			}
			else
			{
				this.interactInput.keyboard = "F";
			}
			if (this.reflection == null)
			{
				this.reflection = base.GetComponent<PhotoBoothPlanarMirror>();
			}
			if (this.mirrorRenderer == null)
			{
				this.mirrorRenderer = base.GetComponent<Renderer>();
			}
			if (this.booth == null)
			{
				this.booth = base.GetComponentInParent<PhotoBoothController>();
			}
			this.BuildRuntimeConfig();
			this.ForceOff();
			this.SetPromptVisible(false);
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x00191486 File Offset: 0x0018F686
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
			if (Application.isPlaying)
			{
				this.ForceOff();
			}
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x0019149B File Offset: 0x0018F69B
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.SetPromptVisible(false);
			this.ForceOff();
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x001914B0 File Offset: 0x0018F6B0
		private void OnDestroy()
		{
			if (this._runtimeConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._runtimeConfig);
				return;
			}
			Object.DestroyImmediate(this._runtimeConfig);
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x001914E0 File Offset: 0x0018F6E0
		private void Update()
		{
			this.ResolvePlayer();
			this.RefreshCandidateState();
			if (this._player == null)
			{
				this.SetPromptVisible(false);
				return;
			}
			bool flag = this._eligible && InteractionCandidateRegistry.IsWinner(this);
			if (flag && this.standaloneMirror && this.promptUI != null && this.mirrorRenderer != null)
			{
				Vector3 up = base.transform.root.up;
				this.promptUI.transform.position = this.mirrorRenderer.bounds.ClosestPoint(this._player.transform.position + up * 1.4f) + up * 0.24f + base.transform.TransformDirection(this.localSurfaceNormal) * 0.12f;
			}
			this.SetPromptVisible(flag);
			if (flag && this.interactInput.GetButtonDown())
			{
				if (!InteractionCandidateRegistry.TryCommitWorldInteraction())
				{
					return;
				}
				this.OnInteract(this._player);
			}
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00191604 File Offset: 0x0018F804
		private void RefreshCandidateState()
		{
			if (this._player == null)
			{
				this._candidateSqrDistance = float.MaxValue;
				this._eligible = false;
				this.ForceOff();
				return;
			}
			Vector3 vector = this._player.transform.position + base.transform.root.up * (this.standaloneMirror ? 1.4f : 0.35f);
			Vector3 vector2 = ((this.mirrorRenderer != null) ? (this.standaloneMirror ? this.mirrorRenderer.bounds.ClosestPoint(vector) : this.mirrorRenderer.bounds.center) : base.transform.position);
			this._candidateSqrDistance = (this.standaloneMirror ? (vector - vector2) : Vector3.ProjectOnPlane(this._player.transform.position - vector2, base.transform.root.up)).sqrMagnitude;
			bool flag = this.IsPlayerInsideBooth();
			if (!flag && this.IsMirrorOn)
			{
				this.ForceOff();
			}
			this._eligible = flag && (this.booth == null || !this.booth.IsSessionActive) && this._candidateSqrDistance <= this.interactionDistance * this.interactionDistance && PlayerActivityGate.Allows(PlayerActivity.Interact);
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x0019176B File Offset: 0x0018F96B
		public bool CanInteract(GameObject player)
		{
			return player != null && this._eligible;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x0019177E File Offset: 0x0018F97E
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player) || this.reflection == null)
			{
				return;
			}
			this.reflection.SetReflectionEnabled(!this.reflection.IsReflectionEnabled);
			this.RefreshPromptLabel();
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x001917B7 File Offset: 0x0018F9B7
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x001917C0 File Offset: 0x0018F9C0
		public void ForceOff()
		{
			if (this.reflection != null)
			{
				this.reflection.SetReflectionEnabled(false);
			}
			this.RefreshPromptLabel();
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x001917E4 File Offset: 0x0018F9E4
		private void ResolvePlayer()
		{
			if (this._player != null)
			{
				return;
			}
			if (PlayerRegistry.Player != null)
			{
				this._player = PlayerRegistry.Player.gameObject;
			}
			if (this._player == null)
			{
				vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
				if (vThirdPersonController != null)
				{
					this._player = vThirdPersonController.gameObject;
				}
			}
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00191848 File Offset: 0x0018FA48
		private bool IsPlayerInsideBooth()
		{
			if (this._player == null)
			{
				return false;
			}
			if (this.standaloneMirror)
			{
				if (this.mirrorRenderer == null || !this.mirrorRenderer.enabled)
				{
					return false;
				}
				Vector3 vector = this._player.transform.position - this.mirrorRenderer.bounds.center;
				return Vector3.Dot(base.transform.TransformDirection(this.localSurfaceNormal), vector) >= -0.05f && this._candidateSqrDistance <= this.autoOffDistance * this.autoOffDistance;
			}
			else
			{
				if (this.interiorBounds == null)
				{
					return this.booth != null && (this._player.transform.position - this.booth.transform.position).sqrMagnitude < 6.25f;
				}
				Bounds bounds = this.interiorBounds.bounds;
				bounds.Expand(0.12f);
				Vector3 vector2 = ((this.booth != null) ? this.booth.transform.up : base.transform.root.up);
				Vector3 vector3 = this._player.transform.position + vector2 * 0.35f;
				return bounds.Contains(vector3);
			}
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x001919B8 File Offset: 0x0018FBB8
		private void BuildRuntimeConfig()
		{
			if (this._runtimeConfig != null || this.interactionConfigTemplate == null)
			{
				return;
			}
			this._runtimeConfig = Object.Instantiate<InteractionConfigSO>(this.interactionConfigTemplate);
			this._runtimeConfig.name = this.mirrorDisplayName + " (Runtime)";
			this._runtimeConfig.hideFlags = HideFlags.DontSave;
			this._runtimeConfig.objectName = this.mirrorDisplayName;
			this._runtimeConfig.objectNameColor = new Color(1f, 0.42f, 0.7f, 1f);
			this._runtimeConfig.mainKeyDisplayLabel = "F";
			this._runtimeConfig.secondaryActionName = string.Empty;
			this._runtimeConfig.secondaryKeyDisplayLabel = string.Empty;
			this.RefreshPromptLabel();
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00191A88 File Offset: 0x0018FC88
		private void RefreshPromptLabel()
		{
			if (this._runtimeConfig == null)
			{
				this.BuildRuntimeConfig();
			}
			if (this._runtimeConfig == null)
			{
				return;
			}
			this._runtimeConfig.mainActionName = (this.IsMirrorOn ? "TURN MIRROR OFF" : "TURN MIRROR ON");
			if (this.promptUI != null && this._promptVisible)
			{
				this.promptUI.Show(this._runtimeConfig);
			}
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00191B00 File Offset: 0x0018FD00
		private void SetPromptVisible(bool visible)
		{
			if (this._promptVisible == visible && (this.promptUI == null || this.promptUI.gameObject.activeSelf == visible))
			{
				return;
			}
			this._promptVisible = visible;
			if (this.promptUI == null)
			{
				return;
			}
			if (visible)
			{
				this.RefreshPromptLabel();
				this.promptUI.Show(this._runtimeConfig);
			}
			this.promptUI.gameObject.SetActive(visible);
		}

		// Token: 0x0400431B RID: 17179
		[SerializeField]
		private PhotoBoothPlanarMirror reflection;

		// Token: 0x0400431C RID: 17180
		[SerializeField]
		private Renderer mirrorRenderer;

		// Token: 0x0400431D RID: 17181
		[SerializeField]
		private PhotoBoothController booth;

		// Token: 0x0400431E RID: 17182
		[SerializeField]
		private BoxCollider interiorBounds;

		// Token: 0x0400431F RID: 17183
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04004320 RID: 17184
		[SerializeField]
		private InteractionConfigSO interactionConfigTemplate;

		// Token: 0x04004321 RID: 17185
		[SerializeField]
		[Min(0.5f)]
		private float interactionDistance = 1.25f;

		// Token: 0x04004322 RID: 17186
		[SerializeField]
		private GenericInput interactInput = new GenericInput("F", "X", "X");

		// Token: 0x04004323 RID: 17187
		[SerializeField]
		private bool standaloneMirror;

		// Token: 0x04004324 RID: 17188
		[SerializeField]
		[Min(1.25f)]
		private float autoOffDistance = 2.5f;

		// Token: 0x04004325 RID: 17189
		[SerializeField]
		private Vector3 localSurfaceNormal = Vector3.back;

		// Token: 0x04004326 RID: 17190
		[SerializeField]
		private string mirrorDisplayName = "PHOTO BOOTH MIRROR";

		// Token: 0x04004327 RID: 17191
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x04004328 RID: 17192
		private GameObject _player;

		// Token: 0x04004329 RID: 17193
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x0400432A RID: 17194
		private bool _eligible;

		// Token: 0x0400432B RID: 17195
		private bool _promptVisible;
	}
}
