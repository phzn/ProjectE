using System;
using System.Collections;
using Features.Detection;
using Features.PlayerEmotes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.InteractiveStand
{
	// Token: 0x02001170 RID: 4464
	[RequireComponent(typeof(Collider))]
	public class StandInteractable : MonoBehaviour, IInteractionInputPriority, IInteractionCandidate
	{
		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x0600881A RID: 34842 RVA: 0x0008D6F5 File Offset: 0x0008B8F5
		public float CandidateSqrDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x0600881B RID: 34843 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x0600881C RID: 34844 RVA: 0x0037B104 File Offset: 0x00379304
		public bool IsCandidateEligible
		{
			get
			{
				return base.isActiveAndEnabled && this.isPlayerInside && this.profile != null && this.profile.Actions.Count > 0 && this.cachedEmoteController != null && GameplayEmoteController.CanStartContextualEmote;
			}
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x0600881E RID: 34846 RVA: 0x0037B158 File Offset: 0x00379358
		public bool ReservesInventoryBinding(string binding)
		{
			if (!InteractionCandidateRegistry.IsWinner(this))
			{
				return false;
			}
			int num = Mathf.Min(this.actionInputs.Length, this.profile.Actions.Count);
			for (int i = 0; i < num; i++)
			{
				StandActionProfileSO.StandAction standAction = this.profile.Actions[i];
				if (((standAction != null) ? standAction.Emote : null) != null && string.Equals(binding, InteractionCandidateRegistry.ActiveBinding(this.actionInputs[i]), StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600881F RID: 34847 RVA: 0x0037B1D8 File Offset: 0x003793D8
		private void Awake()
		{
			string[] array = new string[] { "DPadLeft", "DPadUp", "DPadRight", "DPadDown" };
			int num = 0;
			while (num < this.actionInputs.Length && num < array.Length)
			{
				ControllerContextBindings.Set(this.actionInputs[num], array[num]);
				num++;
			}
			Collider component = base.GetComponent<Collider>();
			if (!component.isTrigger)
			{
				component.isTrigger = true;
				Debug.LogWarning("[StandInteractable] Collider forced to isTrigger=true for detection.", this);
			}
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x0037B258 File Offset: 0x00379458
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			if (this.cachedPlayerController == null)
			{
				this.cachedPlayerController = other.GetComponentInParent<vThirdPersonController>();
				if (this.cachedPlayerController != null)
				{
					this.cachedEmoteController = this.cachedPlayerController.GetComponentInChildren<GameplayEmoteController>();
					if (this.cachedEmoteController != null)
					{
						this.cachedEmoteController.OnEmoteEnded += this.HandleEmoteEnded;
					}
				}
			}
			else if (this.cachedEmoteController != null)
			{
				this.cachedEmoteController.OnEmoteEnded -= this.HandleEmoteEnded;
				this.cachedEmoteController.OnEmoteEnded += this.HandleEmoteEnded;
			}
			if (this.cachedPlayerController == null)
			{
				return;
			}
			if (other.transform != this.cachedPlayerController.transform && other.gameObject.layer != this.cachedPlayerController.gameObject.layer)
			{
				return;
			}
			this.isPlayerInside = true;
			this.RefreshMenu();
			if (this.debugLogs)
			{
				Debug.Log("[StandInteractable] Player entered stand trigger.", this);
			}
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x0037B374 File Offset: 0x00379574
		private void HandleEmoteEnded(EmoteDefinitionSO endedEmote)
		{
			if (!this.emoteStartedByThis)
			{
				return;
			}
			this.emoteStartedByThis = false;
			if (this.isPlayerInside)
			{
				this.RefreshMenu();
			}
		}

		// Token: 0x06008822 RID: 34850 RVA: 0x0037B394 File Offset: 0x00379594
		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>();
			if (componentInParent == null || componentInParent != this.cachedPlayerController)
			{
				return;
			}
			this.isPlayerInside = false;
			this.lastIsPlayingState = false;
			this.HideMenu();
			if (this.swapRoutine != null)
			{
				base.StopCoroutine(this.swapRoutine);
				this.swapRoutine = null;
				this.pendingSwapEmote = null;
			}
			if (this.emoteStartedByThis && this.cachedEmoteController != null && this.cachedEmoteController.CurrentState == GameplayEmoteController.EmoteState.Playing)
			{
				this.cachedEmoteController.EndEmote();
				this.emoteStartedByThis = false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[StandInteractable] Player exited stand trigger.", this);
			}
		}

		// Token: 0x06008823 RID: 34851 RVA: 0x0037B450 File Offset: 0x00379650
		private void Update()
		{
			if (!this.isPlayerInside || this.profile == null || this.cachedEmoteController == null)
			{
				return;
			}
			bool flag = this.cachedEmoteController.CurrentState == GameplayEmoteController.EmoteState.Playing;
			if (!this.emoteStartedByThis && !InteractionCandidateRegistry.IsWinner(this))
			{
				this.HideMenu();
				return;
			}
			if (flag != this.lastIsPlayingState || this.lastControllerLabels != ControllerInput.IsActive)
			{
				this.lastIsPlayingState = flag;
				this.RefreshMenu();
			}
			else if (InteractionHUDController.Instance != null && InteractionHUDController.Instance.CurrentConfig == null)
			{
				this.RefreshMenu();
			}
			int num = Mathf.Min(this.actionInputs.Length, this.profile.Actions.Count);
			for (int i = 0; i < num; i++)
			{
				if (this.actionInputs[i] != null && this.actionInputs[i].GetButtonDown())
				{
					StandActionProfileSO.StandAction standAction = this.profile.Actions[i];
					if (!(((standAction != null) ? standAction.Emote : null) == null))
					{
						InteractionCandidateRegistry.MarkContextInputConsumed(this.actionInputs[i]);
						if (flag)
						{
							this.RequestSwap(i);
							return;
						}
						this.TriggerActionByIndex(i);
						return;
					}
				}
			}
			if (flag && this.emoteStartedByThis)
			{
				GenericInput endEmoteInput = this.cachedEmoteController.EndEmoteInput;
				if (endEmoteInput != null && endEmoteInput.GetButtonDown())
				{
					this.cachedEmoteController.EndEmote();
					if (this.debugLogs)
					{
						Debug.Log("[StandInteractable] Ended emote by user input.", this);
					}
				}
			}
		}

		// Token: 0x06008824 RID: 34852 RVA: 0x0037B5BC File Offset: 0x003797BC
		private void TriggerActionByIndex(int index)
		{
			if (this.profile == null || index < 0 || index >= this.profile.Actions.Count)
			{
				return;
			}
			StandActionProfileSO.StandAction standAction = this.profile.Actions[index];
			if (standAction == null || standAction.Emote == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning(string.Format("[StandInteractable] Action at index {0} has no Emote assigned.", index), this);
				}
				return;
			}
			if (this.cachedEmoteController.TryPlayContextualEmote(standAction.Emote, true))
			{
				this.emoteStartedByThis = true;
				this.RefreshMenu();
				if (this.debugLogs)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[StandInteractable] Started emote '",
						standAction.Emote.EmoteId,
						"' from action '",
						standAction.DisplayName,
						"'."
					}), this);
					return;
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[StandInteractable] Failed to start emote '" + standAction.Emote.EmoteId + "'.", this);
			}
		}

		// Token: 0x06008825 RID: 34853 RVA: 0x0037B6C4 File Offset: 0x003798C4
		private void RequestSwap(int index)
		{
			if (this.profile == null || index < 0 || index >= this.profile.Actions.Count)
			{
				return;
			}
			StandActionProfileSO.StandAction standAction = this.profile.Actions[index];
			if (standAction == null || standAction.Emote == null)
			{
				return;
			}
			this.pendingSwapEmote = standAction.Emote;
			if (this.swapRoutine != null)
			{
				base.StopCoroutine(this.swapRoutine);
			}
			this.swapRoutine = base.StartCoroutine(this.SwapRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[StandInteractable] Requested swap to '" + standAction.Emote.EmoteId + "'.", this);
			}
		}

		// Token: 0x06008826 RID: 34854 RVA: 0x0037B775 File Offset: 0x00379975
		private IEnumerator SwapRoutine()
		{
			if (this.cachedEmoteController != null && this.cachedEmoteController.CurrentState == GameplayEmoteController.EmoteState.Playing)
			{
				this.cachedEmoteController.EndEmote();
				while (this.cachedEmoteController != null && this.cachedEmoteController.CurrentState != GameplayEmoteController.EmoteState.Idle)
				{
					yield return null;
				}
			}
			this.emoteStartedByThis = false;
			EmoteDefinitionSO target = this.pendingSwapEmote;
			this.pendingSwapEmote = null;
			if (!this.isPlayerInside || target == null || this.cachedEmoteController == null)
			{
				this.swapRoutine = null;
				yield break;
			}
			float deadline = Time.unscaledTime + 1.5f;
			bool started = false;
			while (Time.unscaledTime < deadline)
			{
				if (!this.isPlayerInside)
				{
					this.swapRoutine = null;
					yield break;
				}
				started = this.cachedEmoteController.TryPlayContextualEmote(target, true);
				if (started)
				{
					break;
				}
				yield return null;
			}
			this.swapRoutine = null;
			if (started)
			{
				this.emoteStartedByThis = true;
				this.RefreshMenu();
				if (this.debugLogs)
				{
					Debug.Log("[StandInteractable] Swap completed to '" + target.EmoteId + "'.", this);
				}
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[StandInteractable] Swap to '" + target.EmoteId + "' timed out — TryPlayRapidEmote kept rejecting.", this);
			}
			yield break;
		}

		// Token: 0x06008827 RID: 34855 RVA: 0x0037B784 File Offset: 0x00379984
		private void RefreshMenu()
		{
			if (!this.isPlayerInside)
			{
				return;
			}
			if (this.profile == null || this.profile.Actions.Count == 0)
			{
				return;
			}
			if (InteractionHUDController.Instance == null)
			{
				return;
			}
			this.BuildRuntimeConfig();
			InteractionHUDController.Instance.Show(this.runtimeConfig);
		}

		// Token: 0x06008828 RID: 34856 RVA: 0x0037B7DF File Offset: 0x003799DF
		private void HideMenu()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this.runtimeConfig);
			}
		}

		// Token: 0x06008829 RID: 34857 RVA: 0x0037B800 File Offset: 0x00379A00
		private void BuildRuntimeConfig()
		{
			if (this.runtimeConfig == null)
			{
				this.runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			this.lastControllerLabels = ControllerInput.IsActive;
			ControllerContextBindings.MarkControllerLabels(this.runtimeConfig, this.lastControllerLabels);
			this.runtimeConfig.objectName = ((!string.IsNullOrEmpty(this.profile.StandName)) ? this.profile.StandName : "Stand");
			this.runtimeConfig.mainActionName = "";
			this.runtimeConfig.mainKeyDisplayLabel = "";
			this.runtimeConfig.usableActions = this.actions.Actions;
			this.actions.Begin();
			bool flag = this.cachedEmoteController != null && this.cachedEmoteController.CurrentState == GameplayEmoteController.EmoteState.Playing;
			for (int i = 0; i < this.profile.Actions.Count; i++)
			{
				StandActionProfileSO.StandAction standAction = this.profile.Actions[i];
				if (standAction != null)
				{
					string text = ((ControllerInput.IsActive && i < this.actionInputs.Length && this.actionInputs[i] != null) ? InteractionCandidateRegistry.ActiveBinding(this.actionInputs[i]) : standAction.KeyLabel);
					this.actions.Add(text, standAction.DisplayName, "");
				}
			}
			if (flag && this.emoteStartedByThis && this.cachedEmoteController != null)
			{
				GenericInput endEmoteInput = this.cachedEmoteController.EndEmoteInput;
				string text2 = ((endEmoteInput != null && !string.IsNullOrEmpty(endEmoteInput.buttonName)) ? endEmoteInput.buttonName : "Space");
				if (this.lastControllerLabels)
				{
					text2 = ((endEmoteInput != null) ? InteractionCandidateRegistry.ActiveBinding(endEmoteInput) : "A");
				}
				this.actions.Add(text2, this.endActionLabel, "");
			}
			this.actions.End();
		}

		// Token: 0x0600882A RID: 34858 RVA: 0x0037B9D4 File Offset: 0x00379BD4
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			if (this.swapRoutine != null)
			{
				base.StopCoroutine(this.swapRoutine);
				this.swapRoutine = null;
				this.pendingSwapEmote = null;
			}
			if (this.isPlayerInside)
			{
				this.HideMenu();
			}
			if (this.emoteStartedByThis && this.cachedEmoteController != null && this.cachedEmoteController.CurrentState == GameplayEmoteController.EmoteState.Playing)
			{
				this.cachedEmoteController.EndEmote();
			}
			if (this.cachedEmoteController != null)
			{
				this.cachedEmoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this.isPlayerInside = false;
			this.emoteStartedByThis = false;
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x0037BA79 File Offset: 0x00379C79
		private void OnDestroy()
		{
			if (this.runtimeConfig != null)
			{
				Object.Destroy(this.runtimeConfig);
				this.runtimeConfig = null;
			}
		}

		// Token: 0x04008AD1 RID: 35537
		[Header("Configuration")]
		[SerializeField]
		private StandActionProfileSO profile;

		// Token: 0x04008AD2 RID: 35538
		[Header("Input")]
		[Tooltip("Inputs bound to each action by index. Default Alpha1/Alpha2/Alpha3.")]
		[SerializeField]
		private GenericInput[] actionInputs = new GenericInput[]
		{
			new GenericInput("Alpha1", "", ""),
			new GenericInput("Alpha2", "", ""),
			new GenericInput("Alpha3", "", "")
		};

		// Token: 0x04008AD3 RID: 35539
		[Header("HUD Labels")]
		[Tooltip("Display label for the 'end emote' entry shown in the HUD while an emote is playing")]
		[SerializeField]
		private string endActionLabel = "End emote";

		// Token: 0x04008AD4 RID: 35540
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008AD5 RID: 35541
		private bool isPlayerInside;

		// Token: 0x04008AD6 RID: 35542
		private bool emoteStartedByThis;

		// Token: 0x04008AD7 RID: 35543
		private InteractionConfigSO runtimeConfig;

		// Token: 0x04008AD8 RID: 35544
		private readonly InteractionActionBuffer actions = new InteractionActionBuffer();

		// Token: 0x04008AD9 RID: 35545
		private vThirdPersonController cachedPlayerController;

		// Token: 0x04008ADA RID: 35546
		private GameplayEmoteController cachedEmoteController;

		// Token: 0x04008ADB RID: 35547
		private Coroutine swapRoutine;

		// Token: 0x04008ADC RID: 35548
		private EmoteDefinitionSO pendingSwapEmote;

		// Token: 0x04008ADD RID: 35549
		private bool lastIsPlayingState;

		// Token: 0x04008ADE RID: 35550
		private bool lastControllerLabels;
	}
}
