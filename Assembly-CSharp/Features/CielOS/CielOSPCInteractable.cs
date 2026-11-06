using System;
using Features.Tutorials;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x0200138D RID: 5005
	[RequireComponent(typeof(Collider))]
	public class CielOSPCInteractable : MonoBehaviour, IInteractionCandidate
	{
		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x06009AEA RID: 39658 RVA: 0x003F61AA File Offset: 0x003F43AA
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x06009AEB RID: 39659 RVA: 0x003F61B4 File Offset: 0x003F43B4
		public bool IsCandidateEligible
		{
			get
			{
				CielOSController cielOSController = ((this.controller != null) ? this.controller : CielOSController.Instance);
				return this._playerInRange && cielOSController != null && !cielOSController.IsOpen && cielOSController.CanOpen;
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x06009AEC RID: 39660 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06009AED RID: 39661 RVA: 0x003F6200 File Offset: 0x003F4400
		private void Awake()
		{
			this._interactionZone = base.GetComponent<Collider>();
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x06009AEE RID: 39662 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06009AEF RID: 39663 RVA: 0x003F624D File Offset: 0x003F444D
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._playerContacts = 0;
			this._playerInRange = false;
			this._playerTransform = null;
			this._candidateSqrDistance = float.MaxValue;
			this.HidePrompt();
		}

		// Token: 0x06009AF0 RID: 39664 RVA: 0x003F627C File Offset: 0x003F447C
		private void Update()
		{
			CielOSController cielOSController = ((this.controller != null) ? this.controller : CielOSController.Instance);
			bool flag = cielOSController != null && cielOSController.IsOpen;
			bool flag2 = cielOSController != null && cielOSController.CanOpen;
			this._candidateSqrDistance = ((this._playerTransform != null) ? (((this._interactionZone != null) ? this._interactionZone.bounds.center : base.transform.position) - this._playerTransform.position).sqrMagnitude : float.MaxValue);
			bool flag3 = InteractionCandidateRegistry.IsWinner(this);
			if (this._playerInRange && !flag && flag2 && flag3)
			{
				this.ShowPrompt();
			}
			else
			{
				this.HidePrompt();
			}
			if (!this._playerInRange || flag || !flag2 || !flag3)
			{
				return;
			}
			if (GameplayInput.GetKeyDown(this.interactionKey) && InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				this.TryUse();
			}
		}

		// Token: 0x06009AF1 RID: 39665 RVA: 0x003F6388 File Offset: 0x003F4588
		public bool TryUse()
		{
			CielOSController cielOSController = ((this.controller != null) ? this.controller : CielOSController.Instance);
			return !(cielOSController == null) && !cielOSController.IsOpen && cielOSController.CanOpen && (TutorialService.TryShowFirstUse("cielhome_pc") || cielOSController.TryOpen());
		}

		// Token: 0x06009AF2 RID: 39666 RVA: 0x003F63E0 File Offset: 0x003F45E0
		private void OnTriggerEnter(Collider other)
		{
			Transform transform;
			if (!this.IsPlayer(other, out transform))
			{
				return;
			}
			this._playerContacts++;
			this._playerTransform = transform;
			this._playerInRange = this._playerContacts > 0;
		}

		// Token: 0x06009AF3 RID: 39667 RVA: 0x003F6420 File Offset: 0x003F4620
		private void OnTriggerExit(Collider other)
		{
			Transform transform;
			if (!this.IsPlayer(other, out transform))
			{
				return;
			}
			this._playerContacts = Mathf.Max(0, this._playerContacts - 1);
			this._playerInRange = this._playerContacts > 0;
			if (this._playerInRange)
			{
				return;
			}
			this._playerTransform = null;
			this._candidateSqrDistance = float.MaxValue;
			this.HidePrompt();
		}

		// Token: 0x06009AF4 RID: 39668 RVA: 0x003F6480 File Offset: 0x003F4680
		private bool IsPlayer(Collider other, out Transform player)
		{
			player = PlayerRegistry.Player;
			if (player != null)
			{
				return other != null && (other.transform == player || other.transform.IsChildOf(player));
			}
			if (other == null)
			{
				return false;
			}
			Transform root = other.transform.root;
			if (!root.CompareTag(this.playerTag))
			{
				return false;
			}
			player = root;
			return true;
		}

		// Token: 0x06009AF5 RID: 39669 RVA: 0x003F64F4 File Offset: 0x003F46F4
		private void ShowPrompt()
		{
			if (this.promptUI == null || this._promptShown)
			{
				return;
			}
			if (this.config != null)
			{
				this.promptUI.Show(this.config);
			}
			else
			{
				this.promptUI.gameObject.SetActive(true);
			}
			this._promptShown = true;
		}

		// Token: 0x06009AF6 RID: 39670 RVA: 0x003F6551 File Offset: 0x003F4751
		private void HidePrompt()
		{
			if (this.promptUI == null || !this._promptShown)
			{
				return;
			}
			this.promptUI.Hide();
			this._promptShown = false;
		}

		// Token: 0x04009FE1 RID: 40929
		[Header("Interaction")]
		[Tooltip("Key the player presses to open the OS.")]
		[SerializeField]
		private KeyCode interactionKey = KeyCode.E;

		// Token: 0x04009FE2 RID: 40930
		[Tooltip("Tag of the player object.")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x04009FE3 RID: 40931
		[Header("References")]
		[Tooltip("Controller to open. If null, uses CielOSController.Instance.")]
		[SerializeField]
		private CielOSController controller;

		// Token: 0x04009FE4 RID: 40932
		[Header("Prompt UI (same as ShowerStation)")]
		[Tooltip("Shared world-space interaction prompt (same prefab style as the shower / vehicles). If null, found in children. Shown when the player is in range.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04009FE5 RID: 40933
		[Tooltip("Config shown by the prompt (objectName / key / action), e.g. PC / E / Use.")]
		[SerializeField]
		private InteractionConfigSO config;

		// Token: 0x04009FE6 RID: 40934
		private bool _playerInRange;

		// Token: 0x04009FE7 RID: 40935
		private bool _promptShown;

		// Token: 0x04009FE8 RID: 40936
		private Transform _playerTransform;

		// Token: 0x04009FE9 RID: 40937
		private Collider _interactionZone;

		// Token: 0x04009FEA RID: 40938
		private int _playerContacts;

		// Token: 0x04009FEB RID: 40939
		private float _candidateSqrDistance = float.MaxValue;
	}
}
