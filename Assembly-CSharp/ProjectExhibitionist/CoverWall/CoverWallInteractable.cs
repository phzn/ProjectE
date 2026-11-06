using System;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009F1 RID: 2545
	[DisallowMultipleComponent]
	public class CoverWallInteractable : MonoBehaviour, IInteractable, IInteractionFallbackCandidate, IInteractionCandidate, IInteractionInputPriority
	{
		// Token: 0x0600484E RID: 18510 RVA: 0x001B8F2C File Offset: 0x001B712C
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.CoverWallInteractable_interactionConfig)
			{
				return this.interactionConfig;
			}
			throw new InvalidOperationException("Unsupported CoverWallInteractable POI binding: " + field.ToString());
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001B8F56 File Offset: 0x001B7156
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.CoverWallInteractable_interactionConfig)
			{
				this.interactionConfig = (InteractionConfigSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported CoverWallInteractable POI binding: " + field.ToString());
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x001B8F86 File Offset: 0x001B7186
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001B8F90 File Offset: 0x001B7190
		public void SetInteractionConfig(InteractionConfigSO config)
		{
			if (this.interactionConfig == config)
			{
				return;
			}
			this.interactionConfig = config;
			if (this.noPeekConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.noPeekConfig);
				}
				else
				{
					Object.DestroyImmediate(this.noPeekConfig);
				}
				this.noPeekConfig = null;
			}
			this.ReleaseClothingConfigs();
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001B8FED File Offset: 0x001B71ED
		public void SetPromptUI(InteractablePromptUI prompt)
		{
			this.promptUI = prompt;
			this.HidePrompt();
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x001B8FFC File Offset: 0x001B71FC
		public bool IsBeingUsed
		{
			get
			{
				return this.isInCover;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x001B9004 File Offset: 0x001B7204
		public float CandidateSqrDistance
		{
			get
			{
				vCoverPoint vCoverPoint;
				float num;
				if (!this.TryGetActionableCoverPoint(out vCoverPoint, out num))
				{
					return float.MaxValue;
				}
				return num;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004855 RID: 18517 RVA: 0x001B9024 File Offset: 0x001B7224
		public bool IsCandidateEligible
		{
			get
			{
				vCoverPoint vCoverPoint;
				float num;
				return this.TryGetActionableCoverPoint(out vCoverPoint, out num);
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001B903C File Offset: 0x001B723C
		public bool ReservesInventoryBinding(string binding)
		{
			return this.coverController != null && this.coverController.isActiveAndEnabled && !this.coverController.goingToCoverPoint && this.IsCandidateEligible && PlayerActivityGate.Allows(PlayerActivity.Cover) && string.Equals(binding, InteractionCandidateRegistry.ActiveBinding(this.coverController.enterExitInput), StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x001B909A File Offset: 0x001B729A
		private void Awake()
		{
			this.ResolvePromptReference();
			this.setup = base.GetComponent<CoverWallSetup>();
			this.HidePrompt();
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x001B90B4 File Offset: 0x001B72B4
		private void OnDestroy()
		{
			this.ReleaseClothingConfigs();
			if (this.noPeekConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this.noPeekConfig);
				return;
			}
			Object.DestroyImmediate(this.noPeekConfig);
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x001B90E9 File Offset: 0x001B72E9
		private void OnEnable()
		{
			this.ResolvePromptReference();
			InteractionCandidateRegistry.Register(this);
			this.EnsureControllerBinding();
			this.HidePrompt();
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x001B9103 File Offset: 0x001B7303
		private void Start()
		{
			this.EnsureControllerBinding();
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x001B910B File Offset: 0x001B730B
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.HidePrompt();
			this.HideHudIfCurrent();
			this.UnsubscribeFromCoverEvents();
			this.coverController = null;
			this.playerTransform = null;
			this.isInCover = false;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x001B913A File Offset: 0x001B733A
		private void OnValidate()
		{
			this.interactionDistance = Mathf.Max(0.05f, this.interactionDistance);
			this.ResolvePromptReference();
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x001B9103 File Offset: 0x001B7303
		private void Update()
		{
			this.EnsureControllerBinding();
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x001B9158 File Offset: 0x001B7358
		private void LateUpdate()
		{
			this.ResolvePromptReference();
			if (this.IsCandidateEligible && InteractionCandidateRegistry.IsWinner(this))
			{
				this.ShowPrompt();
				this.UpdatePromptPlacement();
			}
			else
			{
				this.HidePrompt();
			}
			this.MaintainCoverHud();
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x001B9190 File Offset: 0x001B7390
		private void UpdatePromptPlacement()
		{
			if (!this.promptFollowsPlayer || this.promptUI == null || this.playerTransform == null)
			{
				return;
			}
			if (this.setup == null)
			{
				this.setup = base.GetComponent<CoverWallSetup>();
			}
			CoverWallSetup.WallBounds wallBounds;
			if (this.setup == null || !this.setup.TryGetVisualBounds(out wallBounds))
			{
				return;
			}
			float num = Mathf.Clamp(base.transform.InverseTransformPoint(this.playerTransform.position).x, wallBounds.min.x, wallBounds.max.x);
			float num2 = wallBounds.min.y + Mathf.Min(this.promptHeight, wallBounds.Height + this.promptTopMargin);
			float num3 = wallBounds.max.z + this.promptForwardOffset;
			this.promptUI.transform.position = base.transform.TransformPoint(new Vector3(num, num2, num3));
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x001B928D File Offset: 0x001B748D
		private bool IsPeekAvailable()
		{
			return this.coverController != null && this.coverController.CanPeekNow;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x001B92AC File Offset: 0x001B74AC
		private void ReleaseClothingConfigs()
		{
			if (this.clothingConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.clothingConfig);
				}
				else
				{
					Object.DestroyImmediate(this.clothingConfig);
				}
				this.clothingConfig = null;
			}
			if (this.clothingNoPeekConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.clothingNoPeekConfig);
				}
				else
				{
					Object.DestroyImmediate(this.clothingNoPeekConfig);
				}
				this.clothingNoPeekConfig = null;
			}
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x001B9324 File Offset: 0x001B7524
		private InteractionConfigSO ResolveHudConfig()
		{
			if (this.interactionConfig == null)
			{
				return null;
			}
			if (this.coverController != null && this.coverController.ClothingOwnsCoverExitInput)
			{
				bool peek = this.IsPeekAvailable();
				InteractionConfigSO interactionConfigSO = (peek ? this.clothingConfig : this.clothingNoPeekConfig);
				if (interactionConfigSO == null)
				{
					interactionConfigSO = Object.Instantiate<InteractionConfigSO>(this.interactionConfig);
					interactionConfigSO.name = this.interactionConfig.name + " (clothing focused" + (peek ? ")" : ", no peek)");
					interactionConfigSO.hideFlags = HideFlags.HideAndDontSave;
					interactionConfigSO.usableActions.RemoveAll((UsableAction a) => a != null && (string.Equals(a.actionName, "Leave", StringComparison.OrdinalIgnoreCase) || (!peek && string.Equals(a.actionName, "Peek", StringComparison.OrdinalIgnoreCase))));
					if (peek)
					{
						this.clothingConfig = interactionConfigSO;
					}
					else
					{
						this.clothingNoPeekConfig = interactionConfigSO;
					}
				}
				return interactionConfigSO;
			}
			if (this.IsPeekAvailable())
			{
				return this.interactionConfig;
			}
			if (this.noPeekConfig == null)
			{
				this.noPeekConfig = Object.Instantiate<InteractionConfigSO>(this.interactionConfig);
				this.noPeekConfig.name = this.interactionConfig.name + " (no peek)";
				this.noPeekConfig.hideFlags = HideFlags.HideAndDontSave;
				this.noPeekConfig.usableActions.RemoveAll((UsableAction a) => a != null && string.Equals(a.actionName, "Peek", StringComparison.OrdinalIgnoreCase));
			}
			return this.noPeekConfig;
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x001B949A File Offset: 0x001B769A
		public bool CanInteract(GameObject player)
		{
			return this.IsCandidateEligible;
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnInteract(GameObject player)
		{
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x001B94A2 File Offset: 0x001B76A2
		public void OnActionExecuted(string actionName, GameObject player)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[CoverWallInteractable] Action notified: " + actionName, this);
			}
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x001B94BD File Offset: 0x001B76BD
		private void ResolvePromptReference()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x001B94DC File Offset: 0x001B76DC
		private void EnsureControllerBinding()
		{
			Transform transform = this.ResolvePlayerTransform();
			vBasicCoverController vBasicCoverController = CoverWallInteractable.ResolveCoverController(transform);
			bool flag = this.coverController != vBasicCoverController;
			this.playerTransform = transform;
			this.coverController = vBasicCoverController;
			if (flag && this.isInCover)
			{
				this.isInCover = false;
				this.HidePrompt();
				this.HideHudIfCurrent();
			}
			if (this.coverController == null)
			{
				if (this.subscribedController != null)
				{
					this.UnsubscribeFromCoverEvents();
				}
				this.ReconcileCoverState();
				return;
			}
			if (this.subscribedController == null || this.subscribedController != this.coverController)
			{
				this.UnsubscribeFromCoverEvents();
				this.SubscribeToCoverEvents(this.coverController);
			}
			this.ReconcileCoverState();
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x001B9588 File Offset: 0x001B7788
		private void ReconcileCoverState()
		{
			if (this.coverController != null && this.coverController.inCover && this.IsPlayerOnThisCoverLine())
			{
				if (!this.isInCover)
				{
					this.HandleEnterCover();
				}
				return;
			}
			if (this.isInCover)
			{
				this.HandleExitCover();
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x001B95D8 File Offset: 0x001B77D8
		private Transform ResolvePlayerTransform()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				return player;
			}
			if (this.playerTransform != null)
			{
				return this.playerTransform;
			}
			vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
			if (!(vThirdPersonController != null))
			{
				return null;
			}
			return vThirdPersonController.transform;
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x001B9624 File Offset: 0x001B7824
		private static vBasicCoverController ResolveCoverController(Transform player)
		{
			if (player == null)
			{
				return null;
			}
			vBasicCoverController component = player.GetComponent<vBasicCoverController>();
			if (!(component != null))
			{
				return player.GetComponentInChildren<vBasicCoverController>(true);
			}
			return component;
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x001B9658 File Offset: 0x001B7858
		private void SubscribeToCoverEvents(vBasicCoverController controller)
		{
			if (controller == null || controller.onEnterCover == null || controller.onExitCover == null)
			{
				return;
			}
			controller.onEnterCover.AddListener(new UnityAction(this.HandleEnterCover));
			controller.onExitCover.AddListener(new UnityAction(this.HandleExitCover));
			this.subscribedController = controller;
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x001B96B4 File Offset: 0x001B78B4
		private void UnsubscribeFromCoverEvents()
		{
			vBasicCoverController vBasicCoverController = this.subscribedController;
			this.subscribedController = null;
			if (vBasicCoverController == null)
			{
				return;
			}
			UnityEvent onEnterCover = vBasicCoverController.onEnterCover;
			if (onEnterCover != null)
			{
				onEnterCover.RemoveListener(new UnityAction(this.HandleEnterCover));
			}
			UnityEvent onExitCover = vBasicCoverController.onExitCover;
			if (onExitCover == null)
			{
				return;
			}
			onExitCover.RemoveListener(new UnityAction(this.HandleExitCover));
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x001B9714 File Offset: 0x001B7914
		private bool TryGetActionableCoverPoint(out vCoverPoint point, out float sqrDistance)
		{
			point = null;
			sqrDistance = float.MaxValue;
			if (!base.isActiveAndEnabled || this.promptUI == null || this.interactionConfig == null || this.coverController == null || this.isInCover || !this.coverController.CanPresentCover)
			{
				return false;
			}
			point = this.coverController.possibleCoverPoint;
			if (point == null || (point.transform != base.transform && !point.transform.IsChildOf(base.transform)))
			{
				return false;
			}
			float num = this.coverController.DistanceToPoseLine(point);
			sqrDistance = num * num;
			float num2 = Mathf.Min(Mathf.Max(0f, this.interactionDistance), Mathf.Max(0f, this.coverController.enterCoverDistance));
			return num2 > 0f && sqrDistance <= num2 * num2 + 0.0001f;
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x001B9810 File Offset: 0x001B7A10
		private bool IsPlayerOnThisCoverLine()
		{
			if (this.coverController == null || this.coverController.currentCoverPoint == null)
			{
				return false;
			}
			Transform transform = this.coverController.currentCoverPoint.transform;
			return transform == base.transform || transform.IsChildOf(base.transform);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x001B986D File Offset: 0x001B7A6D
		private void HandleEnterCover()
		{
			if (!this.IsPlayerOnThisCoverLine())
			{
				return;
			}
			this.isInCover = true;
			this.HidePrompt();
			this.ShowCoverHud();
			TutorialService.TryShowFirstUse("world_cover");
			if (this.enableDebugLogs)
			{
				Debug.Log("[CoverWallInteractable] Entered this cover; approach prompt hidden.", this);
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x001B98A9 File Offset: 0x001B7AA9
		private void HandleExitCover()
		{
			if (!this.isInCover)
			{
				return;
			}
			this.isInCover = false;
			this.HideHudIfCurrent();
			if (this.enableDebugLogs)
			{
				Debug.Log("[CoverWallInteractable] Exited this cover; action HUD released.", this);
			}
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x001B98D4 File Offset: 0x001B7AD4
		private void ShowPrompt()
		{
			if (this.promptUI == null || this.interactionConfig == null)
			{
				return;
			}
			if (this.promptShown && this.promptUI.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = !this.promptShown;
			if (!this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(true);
			}
			this.promptUI.ConfigureCompactActionOnly(0.0038f);
			this.promptUI.Show(this.interactionConfig);
			this.promptShown = true;
			if (flag)
			{
				UnityEvent onPlayerEnterRange = this.OnPlayerEnterRange;
				if (onPlayerEnterRange == null)
				{
					return;
				}
				onPlayerEnterRange.Invoke();
			}
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x001B997F File Offset: 0x001B7B7F
		private void HidePrompt()
		{
			bool flag = this.promptShown;
			this.promptShown = false;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			if (flag)
			{
				UnityEvent onPlayerExitRange = this.OnPlayerExitRange;
				if (onPlayerExitRange == null)
				{
					return;
				}
				onPlayerExitRange.Invoke();
			}
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x001B99BC File Offset: 0x001B7BBC
		private bool OwnsHudConfig(InteractionConfigSO config)
		{
			return config != null && (config == this.interactionConfig || config == this.noPeekConfig || config == this.clothingConfig || config == this.clothingNoPeekConfig);
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x001B9A0C File Offset: 0x001B7C0C
		private void ShowCoverHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			InteractionConfigSO interactionConfigSO = this.ResolveHudConfig();
			if (interactionConfigSO == null || instance == null)
			{
				return;
			}
			instance.Show(interactionConfigSO);
			this.hudShown = instance.CurrentConfig == interactionConfigSO;
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x001B9A54 File Offset: 0x001B7C54
		private void MaintainCoverHud()
		{
			if (!this.isInCover)
			{
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			InteractionConfigSO interactionConfigSO = this.ResolveHudConfig();
			if (interactionConfigSO == null || instance == null)
			{
				this.hudShown = false;
				return;
			}
			if (instance.CurrentConfig == interactionConfigSO)
			{
				this.hudShown = true;
				return;
			}
			if (this.OwnsHudConfig(instance.CurrentConfig))
			{
				instance.Show(interactionConfigSO, true);
				this.hudShown = instance.CurrentConfig == interactionConfigSO;
				return;
			}
			this.hudShown = false;
			if (instance.CurrentConfig == null)
			{
				instance.Show(interactionConfigSO);
				this.hudShown = instance.CurrentConfig == interactionConfigSO;
			}
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x001B9B00 File Offset: 0x001B7D00
		private void HideHudIfCurrent()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this.interactionConfig);
				if (this.noPeekConfig != null)
				{
					instance.HideIfCurrent(this.noPeekConfig);
				}
				if (this.clothingConfig != null)
				{
					instance.HideIfCurrent(this.clothingConfig);
				}
				if (this.clothingNoPeekConfig != null)
				{
					instance.HideIfCurrent(this.clothingNoPeekConfig);
				}
			}
			this.hudShown = false;
		}

		// Token: 0x0400485C RID: 18524
		private const float DistanceEpsilon = 0.0001f;

		// Token: 0x0400485D RID: 18525
		[Header("Configuration")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x0400485E RID: 18526
		[SerializeField]
		[Min(0.05f)]
		private float interactionDistance = 1f;

		// Token: 0x0400485F RID: 18527
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04004860 RID: 18528
		[Header("Events")]
		public UnityEvent OnPlayerEnterRange = new UnityEvent();

		// Token: 0x04004861 RID: 18529
		public UnityEvent OnPlayerExitRange = new UnityEvent();

		// Token: 0x04004862 RID: 18530
		[Header("Prompt placement")]
		[Tooltip("The world prompt follows Ciel along the wall instead of sitting at a fixed point.")]
		[SerializeField]
		private bool promptFollowsPlayer = true;

		// Token: 0x04004863 RID: 18531
		[Tooltip("Prompt height above the floor of the wall (capped to the wall top + margin for low walls).")]
		[SerializeField]
		private float promptHeight = 1.6f;

		// Token: 0x04004864 RID: 18532
		[SerializeField]
		private float promptTopMargin = 0.35f;

		// Token: 0x04004865 RID: 18533
		[Tooltip("Prompt distance from the player-facing face of the wall.")]
		[SerializeField]
		private float promptForwardOffset = 0.35f;

		// Token: 0x04004866 RID: 18534
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x04004867 RID: 18535
		private Transform playerTransform;

		// Token: 0x04004868 RID: 18536
		private vBasicCoverController coverController;

		// Token: 0x04004869 RID: 18537
		private vBasicCoverController subscribedController;

		// Token: 0x0400486A RID: 18538
		private bool isInCover;

		// Token: 0x0400486B RID: 18539
		private bool hudShown;

		// Token: 0x0400486C RID: 18540
		private bool promptShown;

		// Token: 0x0400486D RID: 18541
		private CoverWallSetup setup;

		// Token: 0x0400486E RID: 18542
		private InteractionConfigSO noPeekConfig;

		// Token: 0x0400486F RID: 18543
		private InteractionConfigSO clothingConfig;

		// Token: 0x04004870 RID: 18544
		private InteractionConfigSO clothingNoPeekConfig;

		// Token: 0x04004871 RID: 18545
		private const string PeekActionName = "Peek";
	}
}
