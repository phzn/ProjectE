using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0A RID: 3850
	[RequireComponent(typeof(NPCStateMachine))]
	public class NPCAlertHandler : MonoBehaviour
	{
		// Token: 0x0600738B RID: 29579 RVA: 0x002F1B28 File Offset: 0x002EFD28
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCAlertHandler_actionRegistry)
			{
				return this.actionRegistry;
			}
			if (field != UniversityGameplayField.NPCAlertHandler_alertAnimationProfile)
			{
				throw new InvalidOperationException("Unsupported NPCAlertHandler POI binding: " + field.ToString());
			}
			return this.alertAnimationProfile;
		}

		// Token: 0x0600738C RID: 29580 RVA: 0x002F1B60 File Offset: 0x002EFD60
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCAlertHandler_actionRegistry)
			{
				this.actionRegistry = (NPCActionRegistry)asset;
				return;
			}
			if (field != UniversityGameplayField.NPCAlertHandler_alertAnimationProfile)
			{
				throw new InvalidOperationException("Unsupported NPCAlertHandler POI binding: " + field.ToString());
			}
			this.alertAnimationProfile = (AlertAnimationProfileSO)asset;
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x0600738D RID: 29581 RVA: 0x002F1BAF File Offset: 0x002EFDAF
		public AlertAnimationProfileSO AlertAnimationProfile
		{
			get
			{
				return this.alertAnimationProfile;
			}
		}

		// Token: 0x0600738E RID: 29582 RVA: 0x002F1BB7 File Offset: 0x002EFDB7
		private void Awake()
		{
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._senses = base.GetComponent<NPCSenses>();
			this._controller = base.GetComponent<AStarNPCController>();
		}

		// Token: 0x0600738F RID: 29583 RVA: 0x002F1BE0 File Offset: 0x002EFDE0
		private void Update()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._fsm == null || this._senses == null)
			{
				return;
			}
			if (!this._fsm.IsInitialized)
			{
				return;
			}
			this.ProcessAttention();
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x002F1C34 File Offset: 0x002EFE34
		private void ProcessAttention()
		{
			float currentAttention = this._senses.CurrentAttention;
			DetectionSource currentDetectionSource = this._senses.CurrentDetectionSource;
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			if (currentStateType.IsCriticalState())
			{
				return;
			}
			if (currentStateType == NPCStateType.StopAndStare || currentStateType == NPCStateType.Confusion || currentStateType == NPCStateType.Searching)
			{
				return;
			}
			NPCStateType targetState = this.GetTargetState(currentAttention, currentDetectionSource, currentStateType);
			if (targetState != currentStateType)
			{
				this.Log(string.Format("Attention {0:F2} ({1}) -> requesting {2}", currentAttention, currentDetectionSource, targetState));
				this._fsm.RequestState(targetState, "AlertHandler");
			}
		}

		// Token: 0x06007391 RID: 29585 RVA: 0x002F1CC0 File Offset: 0x002EFEC0
		private NPCStateType GetTargetState(float attention, DetectionSource source, NPCStateType currentState)
		{
			float num = this.alertThreshold;
			float num2 = this.thresholdHysteresis;
			float num3 = this.lookAtThreshold - this.thresholdHysteresis;
			if (currentState == NPCStateType.LookingAt)
			{
				if (attention >= this.alertThreshold)
				{
					if (source != DetectionSource.Visual)
					{
						return NPCStateType.Searching;
					}
					return NPCStateType.StopAndStare;
				}
				else
				{
					if (attention >= num3)
					{
						return NPCStateType.LookingAt;
					}
					return NPCStateType.Wandering;
				}
			}
			else
			{
				if (!currentState.IsNormalState())
				{
					return currentState;
				}
				if (attention >= this.alertThreshold)
				{
					if (source != DetectionSource.Visual)
					{
						return NPCStateType.Searching;
					}
					return NPCStateType.StopAndStare;
				}
				else
				{
					if (attention >= this.lookAtThreshold && source == DetectionSource.Visual && this.IsLookAtAllowedForCurrentAction())
					{
						return NPCStateType.LookingAt;
					}
					return currentState;
				}
			}
		}

		// Token: 0x06007392 RID: 29586 RVA: 0x002F1D41 File Offset: 0x002EFF41
		public void TriggerStumble()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			NPCStateMachine fsm = this._fsm;
			if (fsm == null)
			{
				return;
			}
			fsm.ForceState(NPCStateType.Stumbling, "Collision");
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x002F1D6E File Offset: 0x002EFF6E
		public void TriggerDetected()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			NPCStateMachine fsm = this._fsm;
			if (fsm == null)
			{
				return;
			}
			fsm.ForceState(NPCStateType.Detected, "SensoryManager");
		}

		// Token: 0x06007394 RID: 29588 RVA: 0x002F1D9B File Offset: 0x002EFF9B
		public void TriggerFrozen()
		{
			NPCStateMachine fsm = this._fsm;
			if (fsm == null)
			{
				return;
			}
			fsm.ForceState(NPCStateType.Frozen, "Event");
		}

		// Token: 0x06007395 RID: 29589 RVA: 0x002F1DB4 File Offset: 0x002EFFB4
		public void ReturnToWandering()
		{
			NPCStateMachine fsm = this._fsm;
			if (fsm == null)
			{
				return;
			}
			fsm.ForceState(NPCStateType.Wandering, "EventEnd");
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x002F1DD0 File Offset: 0x002EFFD0
		private bool IsLookAtAllowedForCurrentAction()
		{
			if (this.actionRegistry == null || this._controller == null)
			{
				return true;
			}
			int currentAction = (int)this._controller.CurrentAction;
			return this.actionRegistry.IsLookAtAllowed(currentAction);
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x002F1E13 File Offset: 0x002F0013
		private void Log(string message)
		{
			bool flag = this.enableLogging;
		}

		// Token: 0x040076C3 RID: 30403
		[Header("Thresholds")]
		[SerializeField]
		private float lookAtThreshold = 0.15f;

		// Token: 0x040076C4 RID: 30404
		[SerializeField]
		private float alertThreshold = 0.4f;

		// Token: 0x040076C5 RID: 30405
		[SerializeField]
		private float thresholdHysteresis = 0.05f;

		// Token: 0x040076C6 RID: 30406
		[Header("Action Registry")]
		[SerializeField]
		private NPCActionRegistry actionRegistry;

		// Token: 0x040076C7 RID: 30407
		[Header("Alert Animations")]
		[SerializeField]
		private AlertAnimationProfileSO alertAnimationProfile;

		// Token: 0x040076C8 RID: 30408
		[Header("Debug")]
		[SerializeField]
		private bool enableLogging;

		// Token: 0x040076C9 RID: 30409
		private NPCStateMachine _fsm;

		// Token: 0x040076CA RID: 30410
		private NPCSenses _senses;

		// Token: 0x040076CB RID: 30411
		private AStarNPCController _controller;
	}
}
