using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B2E RID: 2862
	[DisallowMultipleComponent]
	[RequireComponent(typeof(BenchSeat))]
	public sealed class BenchInteractable : MonoBehaviour, IInteractable, IInteractionInputCandidate, IInteractionCandidate, IInteractionInputPriority
	{
		// Token: 0x06005121 RID: 20769 RVA: 0x001F701C File Offset: 0x001F521C
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.BenchInteractable_promptPrefab:
				return this.promptPrefab;
			case UniversityGameplayField.BenchInteractable_sitConfig:
				return this.sitConfig;
			case UniversityGameplayField.BenchInteractable_standConfig:
				return this.standConfig;
			default:
				throw new InvalidOperationException("Unsupported BenchInteractable POI binding: " + field.ToString());
			}
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x001F7074 File Offset: 0x001F5274
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.BenchInteractable_promptPrefab:
				this.promptPrefab = (InteractablePromptUI)asset;
				return;
			case UniversityGameplayField.BenchInteractable_sitConfig:
				this.sitConfig = (InteractionConfigSO)asset;
				return;
			case UniversityGameplayField.BenchInteractable_standConfig:
				this.standConfig = (InteractionConfigSO)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported BenchInteractable POI binding: " + field.ToString());
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x001F70DB File Offset: 0x001F52DB
		public InteractionConfigSO Config
		{
			get
			{
				return this.sitConfig;
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06005124 RID: 20772 RVA: 0x001F70E3 File Offset: 0x001F52E3
		public bool IsBeingUsed
		{
			get
			{
				return this._seating != null && this._seating.IsBusy && this._seating.CurrentSeat == this.seat;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x001F7118 File Offset: 0x001F5318
		public float CandidateSqrDistance
		{
			get
			{
				return this._sqrDistance;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x001F7120 File Offset: 0x001F5320
		public bool IsCandidateEligible
		{
			get
			{
				return base.isActiveAndEnabled && this._eligible && this.seat != null && this.seat.isActiveAndEnabled && this.seat.OwnerId == 0 && !this.seat.HasStaticOccupant;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06005127 RID: 20775 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06005128 RID: 20776 RVA: 0x001F7175 File Offset: 0x001F5375
		public string CandidateInputBinding
		{
			get
			{
				return InteractionCandidateRegistry.ActiveBinding(this.interactInput);
			}
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001F7182 File Offset: 0x001F5382
		public void Configure(BenchSeat benchSeat, InteractablePromptUI worldPromptPrefab, InteractionConfigSO startConfig, InteractionConfigSO exitConfig)
		{
			this.seat = benchSeat;
			this.promptPrefab = worldPromptPrefab;
			this.sitConfig = startConfig;
			this.standConfig = exitConfig;
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001F71A1 File Offset: 0x001F53A1
		private void Awake()
		{
			if (this.seat == null)
			{
				this.seat = base.GetComponent<BenchSeat>();
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x001F71BD File Offset: 0x001F53BD
		private void OnEnable()
		{
			BenchInteractionScheduler.Add(this);
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001F71C5 File Offset: 0x001F53C5
		private void OnDisable()
		{
			BenchInteractionScheduler.Remove(this);
			InteractionCandidateRegistry.Unregister(this);
			this._eligible = false;
			this._sqrDistance = float.MaxValue;
			this.HidePrompt();
			if (this.IsBeingUsed)
			{
				this._seating.CancelSeating(true);
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x0600512D RID: 20781 RVA: 0x001F71FF File Offset: 0x001F53FF
		internal Vector3 ProximityPosition
		{
			get
			{
				if (!(this.seat != null))
				{
					return base.transform.position;
				}
				return this.seat.SeatPosition;
			}
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x000FCE90 File Offset: 0x000FB090
		internal void Wake()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001F7226 File Offset: 0x001F5426
		internal void Sleep()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._eligible = false;
			this._sqrDistance = float.MaxValue;
			this.HidePrompt();
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001F7248 File Offset: 0x001F5448
		internal void TickNearby()
		{
			this.ResolvePlayer();
			this._eligible = this.CanInteract((this._player != null) ? this._player.gameObject : null);
			if (!this._eligible)
			{
				this.HidePrompt();
				return;
			}
			if (InteractionCandidateRegistry.IsPromptWinner(this))
			{
				this.ShowPrompt();
			}
			else
			{
				this.HidePrompt();
			}
			if (this.interactInput.GetButtonDown() && InteractionCandidateRegistry.IsWinner(this))
			{
				this.OnInteract(this._player.gameObject);
			}
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001F72D0 File Offset: 0x001F54D0
		private void ResolvePlayer()
		{
			Transform player = PlayerRegistry.Player;
			if (this._player == player)
			{
				return;
			}
			this._player = player;
			this._controller = ((this._player != null) ? this._player.GetComponent<vThirdPersonController>() : null);
			this._seating = ((this._player != null) ? this._player.GetComponent<BenchPlayerSeating>() : null);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x001F7340 File Offset: 0x001F5540
		public bool CanInteract(GameObject player)
		{
			this._sqrDistance = float.MaxValue;
			if (player == null || !base.isActiveAndEnabled || this.seat == null || !this.seat.isActiveAndEnabled || this.seat.Animations == null || this.sitConfig == null || this.standConfig == null || !PlayerActivityGate.CanStartWorldInteraction)
			{
				return false;
			}
			if (!BenchInteractable.CanReachSeat(player.transform, this.seat, this.approachReach, this.seatReach, this._reachHits, out this._sqrDistance))
			{
				return false;
			}
			Vector3 vector = this.seat.CushionPosition - player.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.01f)
			{
				float num = Vector3.Dot(player.transform.forward, vector.normalized);
				if (num < -0.35f)
				{
					return false;
				}
				this._sqrDistance += (1f - num) * 0.35f;
			}
			return BenchPlayerSeating.CanStart((player.transform == this._player) ? this._controller : player.GetComponent<vThirdPersonController>()) && this.seat.IsAvailable;
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001F7494 File Offset: 0x001F5694
		public static bool CanReachSeat(Transform actor, BenchSeat bench, float approachDistance, float rootDistance, RaycastHit[] hits, out float sqrDistance)
		{
			sqrDistance = float.MaxValue;
			if (actor == null || bench == null || hits == null || hits.Length == 0)
			{
				return false;
			}
			Vector3 vector = actor.position - bench.SeatPosition;
			if (Mathf.Abs(vector.y) > 0.65f || Vector3.Dot(vector, bench.SeatRotation * Vector3.forward) < 0.12f)
			{
				return false;
			}
			vector.y = 0f;
			if (vector.sqrMagnitude > rootDistance * rootDistance)
			{
				return false;
			}
			Vector3 vector2 = bench.ApproachPosition - actor.position;
			sqrDistance = vector2.sqrMagnitude;
			if (sqrDistance > approachDistance * approachDistance)
			{
				return false;
			}
			if (!bench.HasStandingClearance(bench.ApproachPosition, actor, false))
			{
				return false;
			}
			Vector3 vector3 = actor.position + Vector3.up * 1.15f;
			Vector3 vector4 = bench.CushionPosition + bench.CushionRotation * Vector3.forward * (bench.frontEdgeDistance + 0.08f) + Vector3.up * 0.18f - vector3;
			int num = Physics.RaycastNonAlloc(vector3, vector4.normalized, hits, vector4.magnitude, -5, QueryTriggerInteraction.Ignore);
			if (num == hits.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (hits[i].collider != null && !hits[i].collider.transform.IsChildOf(actor))
				{
					return false;
				}
			}
			float magnitude = vector2.magnitude;
			if (magnitude < 0.08f)
			{
				return true;
			}
			int num2 = Physics.SphereCastNonAlloc(actor.position + Vector3.up * 0.75f, 0.18f, vector2 / magnitude, hits, magnitude, -5, QueryTriggerInteraction.Ignore);
			if (num2 == hits.Length)
			{
				return false;
			}
			for (int j = 0; j < num2; j++)
			{
				Collider collider = hits[j].collider;
				if (collider != null && !collider.transform.IsChildOf(actor))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001F76B6 File Offset: 0x001F58B6
		public bool ReservesInventoryBinding(string binding)
		{
			return this.IsCandidateEligible && PlayerActivityGate.CanStartWorldInteraction && string.Equals(binding, this.CandidateInputBinding, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001F76D8 File Offset: 0x001F58D8
		public void OnInteract(GameObject player)
		{
			this.ResolvePlayer();
			this._eligible = player != null && player.transform == this._player && this.CanInteract(player);
			if (!this._eligible || !InteractionCandidateRegistry.IsWinner(this) || !InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
			{
				return;
			}
			this._seating = BenchPlayerSeating.EnsureFor(player);
			if (this._seating != null && this._seating.TrySit(this.seat, this.standConfig))
			{
				this.HidePrompt();
			}
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x001F776E File Offset: 0x001F596E
		public void OnActionExecuted(string actionName, GameObject player)
		{
			if (this.IsBeingUsed && (string.Equals(actionName, "Stand up", StringComparison.OrdinalIgnoreCase) || string.Equals(actionName, "Leave", StringComparison.OrdinalIgnoreCase)))
			{
				this._seating.RequestStand();
				return;
			}
			this.OnInteract(player);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x001F77A8 File Offset: 0x001F59A8
		private void ShowPrompt()
		{
			if (this._prompt == null && this.promptPrefab != null)
			{
				this._prompt = Object.Instantiate<InteractablePromptUI>(this.promptPrefab, base.transform);
				this._prompt.name = "Bench interaction prompt";
				this._prompt.ConfigureCompactActionOnly(0.0038f);
			}
			if (this._prompt == null)
			{
				return;
			}
			this._prompt.transform.position = this.seat.SeatPosition + Vector3.up * this.promptHeight;
			if (!this._prompt.IsVisible)
			{
				this._prompt.Show(this.sitConfig);
			}
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x001F7865 File Offset: 0x001F5A65
		private void HidePrompt()
		{
			if (this._prompt != null)
			{
				this._prompt.Hide();
			}
		}

		// Token: 0x040051D7 RID: 20951
		[SerializeField]
		private BenchSeat seat;

		// Token: 0x040051D8 RID: 20952
		[SerializeField]
		private InteractablePromptUI promptPrefab;

		// Token: 0x040051D9 RID: 20953
		[SerializeField]
		private InteractionConfigSO sitConfig;

		// Token: 0x040051DA RID: 20954
		[SerializeField]
		private InteractionConfigSO standConfig;

		// Token: 0x040051DB RID: 20955
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x040051DC RID: 20956
		[SerializeField]
		[Min(0.2f)]
		private float approachReach = 1.15f;

		// Token: 0x040051DD RID: 20957
		[SerializeField]
		[Min(0.2f)]
		private float seatReach = 1.65f;

		// Token: 0x040051DE RID: 20958
		[SerializeField]
		[Min(0.1f)]
		private float promptHeight = 1.05f;

		// Token: 0x040051DF RID: 20959
		private readonly RaycastHit[] _reachHits = new RaycastHit[24];

		// Token: 0x040051E0 RID: 20960
		private InteractablePromptUI _prompt;

		// Token: 0x040051E1 RID: 20961
		private Transform _player;

		// Token: 0x040051E2 RID: 20962
		private vThirdPersonController _controller;

		// Token: 0x040051E3 RID: 20963
		private BenchPlayerSeating _seating;

		// Token: 0x040051E4 RID: 20964
		private bool _eligible;

		// Token: 0x040051E5 RID: 20965
		private float _sqrDistance = float.MaxValue;

		// Token: 0x040051E6 RID: 20966
		internal int ScheduledIndex = -1;

		// Token: 0x040051E7 RID: 20967
		internal int NearbyIndex = -1;
	}
}
