using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000918 RID: 2328
	public class TriggerInteractionCandidate : MonoBehaviour, IInteractionCandidate
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003F09 RID: 16137 RVA: 0x001763F8 File Offset: 0x001745F8
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._player != null))
				{
					return float.MaxValue;
				}
				return (base.transform.position - this._player.position).sqrMagnitude;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003F0A RID: 16138 RVA: 0x0017643C File Offset: 0x0017463C
		public bool IsCandidateEligible
		{
			get
			{
				return this._playerInside && this._player != null && (this._isAvailable == null || this._isAvailable());
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003F0B RID: 16139 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0017646C File Offset: 0x0017466C
		public static bool AllowsTriggerInput(vTriggerGenericAction trigger)
		{
			if (InteractionCandidateRegistry.HasInteractedThisFrame)
			{
				return false;
			}
			TriggerInteractionCandidate triggerInteractionCandidate = ((trigger != null) ? trigger.GetComponentInParent<TriggerInteractionCandidate>(true) : null);
			return triggerInteractionCandidate == null || InteractionCandidateRegistry.IsWinner(triggerInteractionCandidate);
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x001764A8 File Offset: 0x001746A8
		public static TriggerInteractionCandidate Attach(GameObject host, vTriggerGenericAction[] triggers, InteractablePromptUI prompt, Func<bool> isAvailable, InteractionConfigSO promptConfig = null)
		{
			TriggerInteractionCandidate triggerInteractionCandidate = host.GetComponent<TriggerInteractionCandidate>();
			if (triggerInteractionCandidate == null)
			{
				triggerInteractionCandidate = host.AddComponent<TriggerInteractionCandidate>();
			}
			triggerInteractionCandidate.Unsubscribe();
			triggerInteractionCandidate.SetSuppressed(false);
			triggerInteractionCandidate._triggers = triggers;
			triggerInteractionCandidate._prompt = prompt;
			triggerInteractionCandidate._promptConfig = promptConfig;
			triggerInteractionCandidate._isAvailable = isAvailable;
			if (triggerInteractionCandidate.isActiveAndEnabled)
			{
				triggerInteractionCandidate.Subscribe();
				triggerInteractionCandidate.SuppressImmediatelyIfWorldInteractionClosed();
			}
			return triggerInteractionCandidate;
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0017650C File Offset: 0x0017470C
		private void Subscribe()
		{
			if (this._triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this._triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					if (vTriggerGenericAction.OnValidate != null)
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
						vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleValidate));
					}
					if (vTriggerGenericAction.OnInvalidate != null)
					{
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnInvalidate.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					}
					if (vTriggerGenericAction.OnPlayerExit != null)
					{
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnPlayerExit.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					}
				}
			}
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x001765F0 File Offset: 0x001747F0
		private void Unsubscribe()
		{
			if (this._triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this._triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					if (vTriggerGenericAction.OnValidate != null)
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
					}
					if (vTriggerGenericAction.OnInvalidate != null)
					{
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
					}
					if (vTriggerGenericAction.OnPlayerExit != null)
					{
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
					}
				}
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00176685 File Offset: 0x00174885
		private void HandleValidate(GameObject player)
		{
			this._playerInside = true;
			if (player != null)
			{
				this._player = player.transform;
			}
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this))
			{
				this.SetSuppressed(true);
			}
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x001766BA File Offset: 0x001748BA
		private void HandleInvalidate(GameObject player)
		{
			this._playerInside = false;
			this.SetSuppressed(false);
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x001766CA File Offset: 0x001748CA
		private void OnEnable()
		{
			this.Subscribe();
			InteractionCandidateRegistry.Register(this);
			PlayerActivityGate.Changed += this.HandleGateChanged;
			PlayerActivityGate.ResourcesChanged += this.HandleResourcesChanged;
			this.SuppressImmediatelyIfWorldInteractionClosed();
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x00176700 File Offset: 0x00174900
		private void OnDisable()
		{
			PlayerActivityGate.Changed -= this.HandleGateChanged;
			PlayerActivityGate.ResourcesChanged -= this.HandleResourcesChanged;
			InteractionCandidateRegistry.Unregister(this);
			this.Unsubscribe();
			this._playerInside = false;
			this.SetSuppressed(false);
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00176740 File Offset: 0x00174940
		private void Update()
		{
			if (!this._playerInside || this._player == null)
			{
				this._sqrDist = float.MaxValue;
				return;
			}
			this._sqrDist = (base.transform.position - this._player.position).sqrMagnitude;
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x00176798 File Offset: 0x00174998
		private void LateUpdate()
		{
			if (!this._playerInside)
			{
				return;
			}
			this.SetSuppressed(!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this));
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x001767BD File Offset: 0x001749BD
		private void HandleGateChanged(PlayerActivity _)
		{
			this.SuppressImmediatelyIfWorldInteractionClosed();
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x001767BD File Offset: 0x001749BD
		private void HandleResourcesChanged(PlayerContextResource _)
		{
			this.SuppressImmediatelyIfWorldInteractionClosed();
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x001767C5 File Offset: 0x001749C5
		private void SuppressImmediatelyIfWorldInteractionClosed()
		{
			if (this._playerInside && (!this.IsCandidateEligible || !InteractionCandidateRegistry.CanStartWorldInteraction))
			{
				this.SetSuppressed(true);
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x001767E8 File Offset: 0x001749E8
		private void SetSuppressed(bool suppressed)
		{
			if (this._suppressed == suppressed)
			{
				if (suppressed)
				{
					this.ReassertSuppressedState();
				}
				return;
			}
			if (suppressed)
			{
				this.CaptureInputStates();
			}
			this._suppressed = suppressed;
			if (suppressed)
			{
				this.ReassertSuppressedState();
				return;
			}
			for (int i = 0; i < this._savedInputs.Count; i++)
			{
				if (this._savedInputs[i].input != null)
				{
					this._savedInputs[i].input.useInput = this._savedInputs[i].useInput;
				}
			}
			this._savedInputs.Clear();
			if (this._prompt == null)
			{
				return;
			}
			if (this.IsCandidateEligible && InteractionCandidateRegistry.CanStartWorldInteraction && !this._prompt.gameObject.activeSelf)
			{
				this._prompt.gameObject.SetActive(true);
				if (this._promptConfig != null)
				{
					this._prompt.Show(this._promptConfig);
				}
			}
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x001768E0 File Offset: 0x00174AE0
		private void ReassertSuppressedState()
		{
			for (int i = 0; i < this._savedInputs.Count; i++)
			{
				if (this._savedInputs[i].input != null)
				{
					this._savedInputs[i].input.useInput = false;
				}
			}
			if (this._prompt != null && this._prompt.gameObject.activeSelf)
			{
				this._prompt.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x00176960 File Offset: 0x00174B60
		private void CaptureInputStates()
		{
			this._savedInputs.Clear();
			if (this._triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this._triggers)
			{
				GenericInput genericInput = ((vTriggerGenericAction != null) ? vTriggerGenericAction.actionInput : null);
				if (genericInput != null && !this.ContainsSavedInput(genericInput))
				{
					this._savedInputs.Add(new TriggerInteractionCandidate.SavedInputState
					{
						input = genericInput,
						useInput = genericInput.useInput
					});
				}
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x001769E4 File Offset: 0x00174BE4
		private bool ContainsSavedInput(GenericInput input)
		{
			for (int i = 0; i < this._savedInputs.Count; i++)
			{
				if (this._savedInputs[i].input == input)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003EF9 RID: 16121
		private vTriggerGenericAction[] _triggers;

		// Token: 0x04003EFA RID: 16122
		private InteractablePromptUI _prompt;

		// Token: 0x04003EFB RID: 16123
		private InteractionConfigSO _promptConfig;

		// Token: 0x04003EFC RID: 16124
		private Func<bool> _isAvailable;

		// Token: 0x04003EFD RID: 16125
		private Transform _player;

		// Token: 0x04003EFE RID: 16126
		private bool _playerInside;

		// Token: 0x04003EFF RID: 16127
		private float _sqrDist = float.MaxValue;

		// Token: 0x04003F00 RID: 16128
		private bool _suppressed;

		// Token: 0x04003F01 RID: 16129
		private readonly List<TriggerInteractionCandidate.SavedInputState> _savedInputs = new List<TriggerInteractionCandidate.SavedInputState>(2);

		// Token: 0x02000919 RID: 2329
		private struct SavedInputState
		{
			// Token: 0x04003F02 RID: 16130
			public GenericInput input;

			// Token: 0x04003F03 RID: 16131
			public bool useInput;
		}
	}
}
