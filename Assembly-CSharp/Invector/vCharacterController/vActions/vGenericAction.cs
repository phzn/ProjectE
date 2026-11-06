using System;
using System.Collections;
using System.Collections.Generic;
using Features.HideFlash;
using Features.Vehicles.Traffic;
using Invector.vEventSystems;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000715 RID: 1813
	[vClassHeader("GENERIC ACTION", "Use the vTriggerGenericAction to trigger a simple animation.\n<b><size=12>You can use <color=red>vGenericActionReceiver</color> component to filter events by action name</size></b>", iconName = "triggerIcon")]
	public class vGenericAction : vActionListener
	{
		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x00107D5C File Offset: 0x00105F5C
		// (set) Token: 0x06002ECF RID: 11983 RVA: 0x00107D64 File Offset: 0x00105F64
		public virtual vTriggerGenericAction triggerAction
		{
			get
			{
				return this._triggerAction;
			}
			set
			{
				this._triggerAction = value;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x00107D6D File Offset: 0x00105F6D
		// (set) Token: 0x06002ED1 RID: 11985 RVA: 0x00107D75 File Offset: 0x00105F75
		public bool doingAction { get; set; }

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x00107D7E File Offset: 0x00105F7E
		// (set) Token: 0x06002ED3 RID: 11987 RVA: 0x00107D86 File Offset: 0x00105F86
		public virtual Camera mainCamera { get; set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x00107D8F File Offset: 0x00105F8F
		// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x00107D97 File Offset: 0x00105F97
		public virtual vThirdPersonInput tpInput { get; set; }

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x00107DA0 File Offset: 0x00105FA0
		// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x00107DA8 File Offset: 0x00105FA8
		protected virtual float _currentInputDelay { get; set; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x00107DB1 File Offset: 0x00105FB1
		// (set) Token: 0x06002ED9 RID: 11993 RVA: 0x00107DB9 File Offset: 0x00105FB9
		protected virtual Vector3 _screenCenter { get; set; }

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x00107DC2 File Offset: 0x00105FC2
		// (set) Token: 0x06002EDB RID: 11995 RVA: 0x00107DCA File Offset: 0x00105FCA
		protected virtual float timeInTrigger { get; set; }

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x00107DD3 File Offset: 0x00105FD3
		// (set) Token: 0x06002EDD RID: 11997 RVA: 0x00107DDB File Offset: 0x00105FDB
		protected virtual float animationBehaviourDelay { get; set; }

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002EDE RID: 11998 RVA: 0x00107DE4 File Offset: 0x00105FE4
		protected virtual Vector3 screenCenter
		{
			get
			{
				Vector3 screenCenter = this._screenCenter;
				screenCenter.x = (float)Screen.width * 0.5f;
				screenCenter.y = (float)Screen.height * 0.5f;
				screenCenter.z = 0f;
				return this._screenCenter = screenCenter;
			}
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00107E34 File Offset: 0x00106034
		protected override void SetUpListener()
		{
			base.actionEnter = true;
			base.actionStay = true;
			base.actionExit = true;
			this.actions = new Dictionary<Collider, vGenericAction.ActionStorage>();
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x00107E58 File Offset: 0x00106058
		protected override void Start()
		{
			base.Start();
			this.tpInput = base.GetComponent<vThirdPersonInput>();
			IActionReceiver[] componentsInChildren = base.GetComponentsInChildren<IActionReceiver>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.OnDoAction.AddListener(new UnityAction<vTriggerGenericAction>(componentsInChildren[i].OnReceiveAction));
			}
			if (this.tpInput != null)
			{
				this.tpInput.onUpdate -= this.CheckForTriggerAction;
				this.tpInput.onUpdate += this.CheckForTriggerAction;
				this.tpInput.onLateUpdate -= this.UpdateGenericAction;
				this.tpInput.onLateUpdate += this.UpdateGenericAction;
			}
			if (!this.mainCamera)
			{
				this.mainCamera = Camera.main;
			}
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00107F2C File Offset: 0x0010612C
		protected virtual void UpdateGenericAction()
		{
			if (this.triggerAction == null)
			{
				this.RestorePlayerInterpolation();
			}
			if (!this.mainCamera)
			{
				this.mainCamera = Camera.main;
			}
			if (!this.mainCamera)
			{
				return;
			}
			this.AnimationBehaviour();
			this.HandleColliders();
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x00107F80 File Offset: 0x00106180
		protected virtual void HandleColliders()
		{
			this.colliders.Clear();
			foreach (Collider collider in this.actions.Keys)
			{
				this.colliders.Add(collider);
			}
			if (!this.doingAction && this.triggerAction && !this.isLockTriggerEvents)
			{
				if (this.timeInTrigger <= 0f)
				{
					this.actions.Clear();
					this.triggerAction = null;
					return;
				}
				this.timeInTrigger -= Time.deltaTime;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06002EE3 RID: 12003 RVA: 0x00108038 File Offset: 0x00106238
		protected virtual bool inActionAnimation
		{
			get
			{
				AnimatorStateInfo animatorStateInfo;
				return this.TryGetActionAnimatorState(out animatorStateInfo);
			}
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00108050 File Offset: 0x00106250
		private bool TryGetActionAnimatorState(out AnimatorStateInfo state)
		{
			state = default(AnimatorStateInfo);
			if (!this.actionStarted || this.triggerAction == null || string.IsNullOrEmpty(this.triggerAction.playAnimation) || this.tpInput == null || this.tpInput.cc == null || this.tpInput.cc.animator == null)
			{
				return false;
			}
			Animator animator = this.tpInput.cc.animator;
			int animatorLayer = this.triggerAction.animatorLayer;
			if (!animator.isInitialized || animatorLayer < 0 || animatorLayer >= animator.layerCount)
			{
				return false;
			}
			int num = Animator.StringToHash(this.triggerAction.playAnimation);
			if (animator.IsInTransition(animatorLayer))
			{
				state = animator.GetNextAnimatorStateInfo(animatorLayer);
				if (state.shortNameHash == num || state.fullPathHash == num)
				{
					return true;
				}
			}
			state = animator.GetCurrentAnimatorStateInfo(animatorLayer);
			if (state.shortNameHash == num || state.fullPathHash == num)
			{
				return true;
			}
			state = default(AnimatorStateInfo);
			return false;
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x00108160 File Offset: 0x00106360
		private float ActionNormalizedTime
		{
			get
			{
				AnimatorStateInfo animatorStateInfo;
				if (!this.TryGetActionAnimatorState(out animatorStateInfo))
				{
					return 0f;
				}
				return animatorStateInfo.normalizedTime;
			}
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x00108184 File Offset: 0x00106384
		private void EnsureActionAnimatorListeners()
		{
			vThirdPersonController cc = this.tpInput.cc;
			Animator animator = cc.animator;
			vAnimatorStateInfos vAnimatorStateInfos = cc.animatorStateInfos;
			if (vAnimatorStateInfos == null || vAnimatorStateInfos.animator != animator)
			{
				vAnimatorStateInfos = new vAnimatorStateInfos(animator);
				cc._animatorStateInfos = vAnimatorStateInfos;
			}
			else if (vAnimatorStateInfos.stateInfos == null || vAnimatorStateInfos.stateInfos.Length != animator.layerCount)
			{
				vAnimatorStateInfos.Init();
			}
			foreach (vAnimatorTagBase vAnimatorTagBase in animator.GetBehaviours<vAnimatorTagBase>())
			{
				if (vAnimatorTagBase.stateInfos == null || !vAnimatorTagBase.stateInfos.Contains(vAnimatorStateInfos))
				{
					vAnimatorStateInfos.RegisterListener();
					return;
				}
			}
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x00108228 File Offset: 0x00106428
		protected virtual void CheckForTriggerAction()
		{
			if ((this.actions.Count == 0 && !this.triggerAction) || this.isLockTriggerEvents)
			{
				return;
			}
			vTriggerGenericAction nearAction = this.GetNearAction();
			if (!this.doingAction && this.triggerAction != nearAction)
			{
				this.triggerAction = nearAction;
				if (this.triggerAction)
				{
					this.triggerAction.OnValidate.Invoke(base.gameObject);
					this.OnEnterTriggerAction.Invoke(this.triggerAction);
				}
			}
			this.TriggerActionInput();
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x001082B8 File Offset: 0x001064B8
		protected virtual vTriggerGenericAction GetNearAction()
		{
			if (this.isLockTriggerEvents || this.doingAction || this.playingAnimation)
			{
				return null;
			}
			float num = float.PositiveInfinity;
			vTriggerGenericAction vTriggerGenericAction = null;
			foreach (Collider collider in this.actions.Keys)
			{
				if (collider)
				{
					try
					{
						vTriggerGenericAction vTriggerGenericAction2 = this.actions[collider];
						if (vTriggerGenericAction2 == null || !vTriggerGenericAction2.isActiveAndEnabled || !vTriggerGenericAction2.CanDoAction)
						{
							if (vTriggerGenericAction2 != null && this.actions[collider].isValid)
							{
								vTriggerGenericAction2.OnInvalidate.Invoke(base.gameObject);
							}
							continue;
						}
						Vector3 vector = (this.mainCamera ? this.mainCamera.WorldToScreenPoint(collider.transform.position) : this.screenCenter);
						if (this.mainCamera)
						{
							if (vTriggerGenericAction2.enabled && vTriggerGenericAction2.gameObject.activeInHierarchy && ((!vTriggerGenericAction2.activeFromForward && (vector - this.screenCenter).magnitude < num) || (this.IsInForward(vTriggerGenericAction2.transform, vTriggerGenericAction2.forwardAngle) && (vector - this.screenCenter).magnitude < num)))
							{
								num = (vector - this.screenCenter).magnitude;
								if (vTriggerGenericAction && vTriggerGenericAction != vTriggerGenericAction2)
								{
									if (this.actions[vTriggerGenericAction._collider].isValid)
									{
										vTriggerGenericAction.OnInvalidate.Invoke(base.gameObject);
									}
									vTriggerGenericAction = vTriggerGenericAction2;
								}
								else if (vTriggerGenericAction == null)
								{
									vTriggerGenericAction = vTriggerGenericAction2;
								}
							}
							else
							{
								if (this.actions[vTriggerGenericAction2._collider].isValid)
								{
									vTriggerGenericAction2.OnInvalidate.Invoke(base.gameObject);
								}
								this.OnExitTriggerAction.Invoke(this.triggerAction);
							}
						}
						else if (!vTriggerGenericAction)
						{
							vTriggerGenericAction = vTriggerGenericAction2;
						}
						else
						{
							if (this.actions[vTriggerGenericAction2._collider].isValid)
							{
								vTriggerGenericAction2.OnInvalidate.Invoke(base.gameObject);
							}
							this.OnExitTriggerAction.Invoke(this.triggerAction);
						}
						continue;
					}
					catch
					{
						break;
					}
				}
				this.actions.Remove(collider);
				return null;
			}
			return vTriggerGenericAction;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00108584 File Offset: 0x00106784
		protected virtual bool IsInForward(Transform target, float angleToCompare)
		{
			return Vector3.Angle(base.transform.forward, target.forward) <= angleToCompare;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x001085A4 File Offset: 0x001067A4
		protected virtual void AnimationBehaviour()
		{
			if (this.animationBehaviourDelay > 0f && !this.playingAnimation)
			{
				this.animationBehaviourDelay -= Time.deltaTime;
				return;
			}
			if (this.playingAnimation)
			{
				if (this.triggerAction.matchTarget != null)
				{
					if (this.debugMode)
					{
						Debug.Log("<b>GenericAction: </b><color=blue>Match Target...</color> ");
					}
					this.EvaluateToTargetPosition();
				}
				if (this.triggerAction.useTriggerRotation)
				{
					if (this.debugMode)
					{
						Debug.Log("<b>GenericAction: </b><color=blue>Rotate to Target...</color> ");
					}
					this.EvaluateToTargetRotation();
				}
				if (this.actionStarted && !this.triggerAction.endActionManualy && (this.triggerAction.inputType != vTriggerGenericAction.InputType.GetButtonTimer || !this.triggerAction.playAnimationWhileHoldingButton) && this.ActionNormalizedTime >= this.triggerAction.endExitTimeAnimation)
				{
					if (this.debugMode)
					{
						Debug.Log("<b>GenericAction: </b>Finish Animation ");
					}
					this.EndAction();
					return;
				}
			}
			else if (this.doingAction && this.actionStarted && (this.triggerAction == null || !this.triggerAction.endActionManualy))
			{
				if (this.triggerAction != null && this.triggerAction.inputType == vTriggerGenericAction.InputType.GetButtonTimer && this.triggerAction.playAnimationWhileHoldingButton)
				{
					return;
				}
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b>Force ResetTriggerSettings ");
				}
				this.EndAction();
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x0010870C File Offset: 0x0010690C
		protected virtual void EvaluateToTargetPosition()
		{
			Vector3 vector = this.triggerAction.matchTarget.position;
			switch (this.triggerAction.avatarTarget)
			{
			case AvatarTarget.LeftFoot:
				vector = this.triggerAction.matchTarget.position - base.transform.rotation * base.transform.InverseTransformPoint(this.tpInput.animator.GetBoneTransform(HumanBodyBones.LeftFoot).position);
				break;
			case AvatarTarget.RightFoot:
				vector = this.triggerAction.matchTarget.position - base.transform.rotation * base.transform.InverseTransformPoint(this.tpInput.animator.GetBoneTransform(HumanBodyBones.RightFoot).position);
				break;
			case AvatarTarget.LeftHand:
				vector = this.triggerAction.matchTarget.position - base.transform.rotation * base.transform.InverseTransformPoint(this.tpInput.animator.GetBoneTransform(HumanBodyBones.LeftHand).position);
				break;
			case AvatarTarget.RightHand:
				vector = this.triggerAction.matchTarget.position - base.transform.rotation * base.transform.InverseTransformPoint(this.tpInput.animator.GetBoneTransform(HumanBodyBones.RightHand).position);
				break;
			}
			AnimationCurve matchPositionXZCurve = this.triggerAction.matchPositionXZCurve;
			AnimationCurve matchPositionYCurve = this.triggerAction.matchPositionYCurve;
			float num = Mathf.Clamp01(this.ActionNormalizedTime);
			Vector3 vector2 = this.triggerAction.matchTarget.InverseTransformPoint(vector);
			if (!this.triggerAction.useLocalX)
			{
				vector2.x = this.triggerAction.matchTarget.InverseTransformPoint(base.transform.position).x;
			}
			if (!this.triggerAction.useLocalZ)
			{
				vector2.z = this.triggerAction.matchTarget.InverseTransformPoint(base.transform.position).z;
			}
			vector = this.triggerAction.matchTarget.TransformPoint(vector2);
			Vector3 rootPosition = this.tpInput.cc.animator.rootPosition;
			float num2 = matchPositionXZCurve.Evaluate(num);
			float num3 = matchPositionYCurve.Evaluate(num);
			if (num2 < 1f)
			{
				rootPosition.x = Mathf.Lerp(rootPosition.x, vector.x, num2);
				rootPosition.z = Mathf.Lerp(rootPosition.z, vector.z, num2);
				this.finishPositionXZMatch = true;
			}
			else if (this.finishPositionXZMatch)
			{
				this.finishPositionXZMatch = false;
				rootPosition.x = vector.x;
				rootPosition.z = vector.z;
			}
			if (num3 < 1f)
			{
				rootPosition.y = Mathf.Lerp(rootPosition.y, vector.y, num3);
				this.finishPositionYMatch = true;
			}
			else if (this.finishPositionYMatch)
			{
				this.finishPositionYMatch = false;
				rootPosition.y = vector.y;
			}
			base.transform.position = rootPosition;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x00108A20 File Offset: 0x00106C20
		protected virtual void EvaluateToTargetRotation()
		{
			Quaternion quaternion = Quaternion.Euler(new Vector3(base.transform.eulerAngles.x, this.triggerAction.transform.eulerAngles.y, base.transform.eulerAngles.z));
			Quaternion quaternion2 = this.tpInput.cc.animator.rootRotation;
			AnimationCurve matchRotationCurve = this.triggerAction.matchRotationCurve;
			float actionNormalizedTime = this.ActionNormalizedTime;
			float num = matchRotationCurve.Evaluate(actionNormalizedTime);
			if (num < 1f)
			{
				quaternion2 = Quaternion.Lerp(quaternion2, quaternion, num);
				this.finishRotationMatch = true;
			}
			else if (this.finishRotationMatch)
			{
				this.finishRotationMatch = false;
				quaternion2 = quaternion;
			}
			base.transform.rotation = quaternion2;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x00108AD4 File Offset: 0x00106CD4
		protected virtual void EndAction()
		{
			uint? num = this.endingActionGeneration;
			uint num2 = this.actionGeneration;
			if ((num.GetValueOrDefault() == num2) & (num != null))
			{
				return;
			}
			if (this.triggerAction == null)
			{
				this.ResetTriggerSettings(true);
				return;
			}
			vTriggerGenericAction triggerAction = this.triggerAction;
			uint num3 = this.actionGeneration;
			uint? num4 = this.endingActionGeneration;
			this.endingActionGeneration = new uint?(num3);
			try
			{
				this.OnEndAction.Invoke(triggerAction);
				if (!this.IsCurrentAction(triggerAction, num3))
				{
					return;
				}
				triggerAction.OnEndAnimation.Invoke();
				if (!this.IsCurrentAction(triggerAction, num3))
				{
					return;
				}
				this.OnExitTriggerAction.Invoke(triggerAction);
				if (!this.IsCurrentAction(triggerAction, num3))
				{
					return;
				}
				this.ResetTriggerSettings(true);
				if (triggerAction != null && triggerAction.destroyAfter)
				{
					base.StartCoroutine(this.DestroyActionDelay(triggerAction));
				}
			}
			finally
			{
				this.endingActionGeneration = num4;
			}
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>End Action ");
			}
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x00108BD8 File Offset: 0x00106DD8
		private bool IsCurrentAction(vTriggerGenericAction trigger, uint generation)
		{
			return trigger != null && this.triggerAction == trigger && this.actionGeneration == generation;
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x00108BFC File Offset: 0x00106DFC
		// (set) Token: 0x06002EF0 RID: 12016 RVA: 0x00108CC3 File Offset: 0x00106EC3
		public virtual bool playingAnimation
		{
			get
			{
				if (this.triggerAction == null || !this.doingAction)
				{
					return this._playingAnimation = false;
				}
				if (!this._playingAnimation && this.inActionAnimation)
				{
					vTriggerGenericAction triggerAction = this.triggerAction;
					uint num = this.actionGeneration;
					this._playingAnimation = true;
					triggerAction.OnStartAnimation.Invoke();
					if (this.doingAction && this.triggerAction == triggerAction && this.actionGeneration == num && this._playingAnimation)
					{
						this.DisablePlayerGravityAndCollision();
					}
					return this.doingAction && this.triggerAction != null && this._playingAnimation;
				}
				if (this._playingAnimation && !this.inActionAnimation)
				{
					this._playingAnimation = false;
				}
				return this._playingAnimation;
			}
			protected set
			{
				this._playingAnimation = true;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x00108CCC File Offset: 0x00106ECC
		public virtual bool actionConditions
		{
			get
			{
				return !this.doingAction && !this.playingAnimation && !this.tpInput.cc.isJumping && !this.tpInput.cc.customAction;
			}
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x00108D08 File Offset: 0x00106F08
		public override void OnActionEnter(Collider other)
		{
			if (this.isLockTriggerEvents)
			{
				return;
			}
			if (other != null && other.gameObject.CompareTag(this.actionTag) && !this.actions.ContainsKey(other))
			{
				vTriggerGenericAction[] components = other.GetComponents<vTriggerGenericAction>();
				int i = 0;
				while (i < components.Length)
				{
					vTriggerGenericAction vTriggerGenericAction = components[i];
					if (vTriggerGenericAction && vTriggerGenericAction.enabled)
					{
						this.actions.Add(other, vTriggerGenericAction);
						vTriggerGenericAction.OnPlayerEnter.Invoke(base.gameObject);
						if (this.debugMode)
						{
							string text = "<color=green>Enter in Trigger </color>";
							GameObject gameObject = other.gameObject;
							Debug.Log(text + ((gameObject != null) ? gameObject.ToString() : null), other.gameObject);
							return;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x00108DCC File Offset: 0x00106FCC
		public override void OnActionExit(Collider other)
		{
			if (this.isLockTriggerEvents)
			{
				return;
			}
			if (other.gameObject.CompareTag(this.actionTag) && this.actions.ContainsKey(other) && (!this.doingAction || other != this.triggerAction._collider))
			{
				vTriggerGenericAction vTriggerGenericAction = this.actions[other];
				this.actions.Remove(other);
				vTriggerGenericAction.OnPlayerExit.Invoke(base.gameObject);
				vTriggerGenericAction.OnInvalidate.Invoke(base.gameObject);
				this.OnExitTriggerAction.Invoke(vTriggerGenericAction);
				if (this.debugMode)
				{
					string text = "<color=red>Exit of Trigger </color> ";
					GameObject gameObject = other.gameObject;
					Debug.Log(text + ((gameObject != null) ? gameObject.ToString() : null), other.gameObject);
				}
			}
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x00108EA0 File Offset: 0x001070A0
		public override void OnActionStay(Collider other)
		{
			if (this.isLockTriggerEvents)
			{
				return;
			}
			if (other != null && !this.actions.ContainsKey(other))
			{
				this.OnActionEnter(other);
				return;
			}
			if (other != null && this.actions.ContainsKey(other))
			{
				this.actions[other].action.OnPlayerStay.Invoke(base.gameObject);
				this.timeInTrigger = 0.5f;
				if (this.debugMode)
				{
					string text = "<color=yellow>Stay in Trigger </color>";
					GameObject gameObject = other.gameObject;
					Debug.Log(text + ((gameObject != null) ? gameObject.ToString() : null), other.gameObject);
				}
			}
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x00108F47 File Offset: 0x00107147
		public virtual void FinishAction()
		{
			if (this.triggerAction && this.actionStarted && this.triggerAction.endActionManualy)
			{
				this.EndAction();
			}
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00108F74 File Offset: 0x00107174
		public virtual void CancelAction()
		{
			if (this.triggerAction && this.actionStarted)
			{
				vTriggerGenericAction triggerAction = this.triggerAction;
				triggerAction.OnCancelAction.Invoke();
				this.OnExitTriggerAction.Invoke(this.triggerAction);
				this.ResetTriggerSettings(true);
				if (triggerAction.destroyAfter)
				{
					base.StartCoroutine(this.DestroyActionDelay(triggerAction));
				}
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b>Cancel Action ");
				}
			}
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x00108FE8 File Offset: 0x001071E8
		public virtual void TriggerActionInput()
		{
			if (!this.doingAction && HideFlashController.HasHeldCoverItem)
			{
				return;
			}
			if (this.triggerAction == null || !this.triggerAction.gameObject.activeInHierarchy || !this.triggerAction.CanDoAction)
			{
				return;
			}
			if (!this.doingAction && !this.playingAnimation && !TriggerInteractionCandidate.AllowsTriggerInput(this.triggerAction))
			{
				return;
			}
			if (this.triggerAction.inputType == vTriggerGenericAction.InputType.AutoAction && this.actionConditions)
			{
				this.TriggerActionEvents();
				this.TriggerAnimation();
				return;
			}
			if (this.triggerAction.inputType == vTriggerGenericAction.InputType.GetButtonDown && this.actionConditions)
			{
				if (this.triggerAction.actionInput.GetButtonDown())
				{
					this.TriggerActionEvents();
					this.TriggerAnimation();
					return;
				}
			}
			else if (this.triggerAction.inputType == vTriggerGenericAction.InputType.GetDoubleButton && this.actionConditions)
			{
				if (this.triggerAction.actionInput.GetDoubleButtonDown(this.triggerAction.doubleButtomTime))
				{
					this.TriggerActionEvents();
					this.TriggerAnimation();
					return;
				}
			}
			else if (this.triggerAction.inputType == vTriggerGenericAction.InputType.GetButtonTimer)
			{
				if (this._currentInputDelay <= 0f)
				{
					bool flag = false;
					float num = 0f;
					if (this.triggerAction.playAnimationWhileHoldingButton)
					{
						this.TriggerActionEventsInput();
						if (this.triggerAction.actionInput.GetButtonTimer(ref num, ref flag, this.triggerAction.buttonTimer))
						{
							if (this.debugMode)
							{
								Debug.Log("<b>GenericAction: </b>Finish Action Input ");
							}
							this.triggerAction.UpdateButtonTimer(0f);
							this.triggerAction.OnFinishActionInput.Invoke();
							this.ResetActionState();
							this.EndAction();
						}
						if (this.triggerAction && this.triggerAction.actionInput.inButtomTimer)
						{
							if (this.debugMode)
							{
								Debug.Log("<b>GenericAction: </b><color=blue>Holding Input</color>  ");
							}
							this.triggerAction.UpdateButtonTimer(num);
							this.TriggerAnimation();
						}
						if (flag && this.triggerAction)
						{
							this.CancelButtonTimer();
							return;
						}
					}
					else
					{
						this.TriggerActionEventsInput();
						if (this.triggerAction.actionInput.GetButtonTimer(ref num, ref flag, this.triggerAction.buttonTimer))
						{
							if (this.debugMode)
							{
								Debug.Log("<b>GenericAction: </b>Finish Action Input ");
							}
							this.triggerAction.UpdateButtonTimer(0f);
							this.triggerAction.OnFinishActionInput.Invoke();
							this.TriggerAnimation();
						}
						if (this.triggerAction && this.triggerAction.actionInput.inButtomTimer)
						{
							if (this.debugMode)
							{
								Debug.Log("<b>GenericAction: </b><color=blue>Holding Input</color>");
							}
							this.triggerAction.UpdateButtonTimer(num);
						}
						if (flag && this.triggerAction)
						{
							this.CancelButtonTimer();
							return;
						}
					}
				}
				else
				{
					this._currentInputDelay -= Time.deltaTime;
				}
			}
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x001092B4 File Offset: 0x001074B4
		protected virtual void CancelButtonTimer()
		{
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>Cancel Action ");
			}
			this.triggerAction.OnCancelActionInput.Invoke();
			this._currentInputDelay = this.triggerAction.inputDelay;
			this.triggerAction.UpdateButtonTimer(0f);
			this.OnCancelAction.Invoke(this.triggerAction);
			this.ResetActionState();
			this.ResetTriggerSettings(false);
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x00109322 File Offset: 0x00107522
		protected virtual void TriggerActionEventsInput()
		{
			if (this.triggerAction && this.triggerAction.actionInput.GetButtonDown())
			{
				this.TriggerActionEvents();
			}
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x0010934C File Offset: 0x0010754C
		public virtual void TriggerActionEvents()
		{
			if (this.triggerAction == null)
			{
				return;
			}
			if (!this.TryAcquireCarClimbContext())
			{
				return;
			}
			this.actionGeneration += 1U;
			vTriggerGenericAction triggerAction = this.triggerAction;
			uint num = this.actionGeneration;
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>TriggerAction Events ", base.gameObject);
			}
			this.doingAction = true;
			this.OnStartAction.Invoke(triggerAction);
			if (!this.doingAction || !this.IsCurrentAction(triggerAction, num))
			{
				return;
			}
			this.OnDoAction.Invoke(triggerAction);
			if (!this.doingAction || !this.IsCurrentAction(triggerAction, num))
			{
				return;
			}
			base.StartCoroutine(triggerAction.OnPressActionDelay(base.gameObject));
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x00109400 File Offset: 0x00107600
		private bool TryAcquireCarClimbContext()
		{
			CarClimbBridge carClimbBridge = ((this.triggerAction != null) ? this.triggerAction.GetComponentInParent<CarClimbBridge>(true) : null);
			return carClimbBridge == null || carClimbBridge.TryAcquireClimbContext();
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x0010943C File Offset: 0x0010763C
		public virtual void TriggerAnimation()
		{
			if (this.triggerAction == null)
			{
				return;
			}
			if (!this.TryAcquireCarClimbContext())
			{
				return;
			}
			if (this.actionStarted || this.playingAnimation)
			{
				return;
			}
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>TriggerAnimation ", base.gameObject);
			}
			if (this.triggerAction.animatorActionState != 0)
			{
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b>Applied ActionState: " + this.triggerAction.animatorActionState.ToString() + " ", base.gameObject);
				}
				this.tpInput.cc.SetActionState(this.triggerAction.animatorActionState);
			}
			if (!string.IsNullOrEmpty(this.triggerAction.playAnimation))
			{
				if (!this.actionStarted)
				{
					if (this.debugMode)
					{
						Debug.Log("<b>GenericAction: </b>PlayAnimation: " + this.triggerAction.playAnimation + " ", base.gameObject);
					}
					this.EnsureActionAnimatorListeners();
					this.actionStarted = true;
					this._playingAnimation = false;
					this.tpInput.cc.animator.CrossFadeInFixedTime(this.triggerAction.playAnimation, this.triggerAction.crossFadeTransition, this.triggerAction.animatorLayer);
					if (!string.IsNullOrEmpty(this.triggerAction.customCameraState))
					{
						this.tpInput.ChangeCameraState(this.triggerAction.customCameraState, true);
					}
				}
				this.animationBehaviourDelay = this.triggerAction.crossFadeTransition + 0.1f;
				return;
			}
			this.actionStarted = true;
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x001095C3 File Offset: 0x001077C3
		public virtual void ResetActionState()
		{
			if (this.triggerAction && this.triggerAction.resetAnimatorActionState)
			{
				this.tpInput.cc.SetActionState(0);
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x001095F0 File Offset: 0x001077F0
		public virtual void ResetTriggerSettings(bool removeTrigger = true)
		{
			this.actionGeneration += 1U;
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>Reset Trigger Settings ");
			}
			this.EnablePlayerGravityAndCollision();
			this.ResetActionState();
			if (this.triggerAction != null && !string.IsNullOrEmpty(this.triggerAction.customCameraState))
			{
				this.tpInput.ResetCameraState();
			}
			if (this.triggerAction != null && this.triggerAction._collider != null && this.actions.ContainsKey(this.triggerAction._collider) && removeTrigger)
			{
				this.actions.Remove(this.triggerAction._collider);
			}
			this.triggerAction = null;
			this.doingAction = false;
			this.actionStarted = false;
			this._playingAnimation = false;
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x001096C8 File Offset: 0x001078C8
		public virtual void DisablePlayerGravityAndCollision()
		{
			if (this.triggerAction && this.triggerAction.disableGravity)
			{
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b><color=red>Disable Player's Gravity</color> ");
				}
				this.SuspendPlayerInterpolation(this.tpInput.cc._rigidbody);
				this.tpInput.cc._rigidbody.useGravity = false;
				if (!this.tpInput.cc._rigidbody.isKinematic)
				{
					this.tpInput.cc._rigidbody.velocity = Vector3.zero;
				}
				this.tpInput.cc._rigidbody.isKinematic = true;
			}
			if (this.triggerAction && this.triggerAction.disableCollision)
			{
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b><color=red>Disable Player's Collision</color> ");
				}
				this.tpInput.cc._capsuleCollider.isTrigger = true;
			}
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x001097BC File Offset: 0x001079BC
		public virtual void EnablePlayerGravityAndCollision()
		{
			if (this.triggerAction && this.triggerAction.disableGravity)
			{
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b><color=red>Enable Player's Gravity</color> ");
				}
				this.tpInput.cc._rigidbody.useGravity = true;
				this.tpInput.cc._rigidbody.isKinematic = false;
			}
			if (this.triggerAction && this.triggerAction.disableCollision)
			{
				if (this.debugMode)
				{
					Debug.Log("<b>GenericAction: </b><color=red>Enable Player's Collision</color> ");
				}
				this.tpInput.cc._capsuleCollider.isTrigger = false;
			}
			this.RestorePlayerInterpolation();
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x0010986C File Offset: 0x00107A6C
		private void SuspendPlayerInterpolation(Rigidbody body)
		{
			if (this.interpolationBody != body)
			{
				this.RestorePlayerInterpolation();
				if (body == null)
				{
					return;
				}
				this.interpolationBody = body;
				this.interpolationBeforeAction = body.interpolation;
			}
			if (body != null)
			{
				body.interpolation = RigidbodyInterpolation.None;
			}
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x001098BC File Offset: 0x00107ABC
		private void RestorePlayerInterpolation()
		{
			Rigidbody rigidbody = this.interpolationBody;
			this.interpolationBody = null;
			if (rigidbody != null)
			{
				rigidbody.interpolation = this.interpolationBeforeAction;
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x001098EC File Offset: 0x00107AEC
		protected virtual void OnDisable()
		{
			this.RestorePlayerInterpolation();
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x001098F4 File Offset: 0x00107AF4
		public virtual IEnumerator DestroyActionDelay(vTriggerGenericAction triggerAction)
		{
			yield return new WaitForSeconds(triggerAction.destroyDelay);
			if (triggerAction != null && triggerAction.gameObject != null)
			{
				this.OnExitTriggerAction.Invoke(triggerAction);
				Object.Destroy(triggerAction.gameObject);
			}
			if (this.debugMode)
			{
				Debug.Log("<b>GenericAction: </b>Destroy Trigger ");
			}
			yield break;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x0010990C File Offset: 0x00107B0C
		public virtual void SetLockTriggerEvents(bool value)
		{
			foreach (Collider collider in this.actions.Keys)
			{
				if (collider)
				{
					this.actions[collider].action.OnPlayerExit.Invoke(base.gameObject);
					this.actions[collider].action.OnInvalidate.Invoke(base.gameObject);
				}
			}
			this.actions.Clear();
			this.isLockTriggerEvents = value;
		}

		// Token: 0x04002F1F RID: 12063
		[vEditorToolbar("Settings", false, "", false, false)]
		[Tooltip("Tag of the object you want to access")]
		public string actionTag = "Action";

		// Token: 0x04002F20 RID: 12064
		[Tooltip("Use root motion of the animation")]
		public bool useRootMotion = true;

		// Token: 0x04002F21 RID: 12065
		[vEditorToolbar("Debug", false, "", false, false)]
		[Header("--- Debug Only ---")]
		[Tooltip("Check this to enter the debug mode")]
		public bool debugMode;

		// Token: 0x04002F22 RID: 12066
		[vReadOnly(true)]
		protected vTriggerGenericAction _triggerAction;

		// Token: 0x04002F23 RID: 12067
		[vReadOnly(true)]
		[SerializeField]
		protected bool _playingAnimation;

		// Token: 0x04002F24 RID: 12068
		[vReadOnly(true)]
		[SerializeField]
		protected bool actionStarted;

		// Token: 0x04002F25 RID: 12069
		[vReadOnly(true)]
		public bool isLockTriggerEvents;

		// Token: 0x04002F26 RID: 12070
		[vReadOnly(true)]
		[SerializeField]
		protected List<Collider> colliders = new List<Collider>();

		// Token: 0x04002F27 RID: 12071
		[vEditorToolbar("Events", false, "", false, false)]
		public vOnActionHandle OnDoAction = new vOnActionHandle();

		// Token: 0x04002F28 RID: 12072
		public vOnActionHandle OnEnterTriggerAction;

		// Token: 0x04002F29 RID: 12073
		public vOnActionHandle OnExitTriggerAction;

		// Token: 0x04002F2A RID: 12074
		public vOnActionHandle OnStartAction;

		// Token: 0x04002F2B RID: 12075
		public vOnActionHandle OnCancelAction;

		// Token: 0x04002F2C RID: 12076
		public vOnActionHandle OnEndAction;

		// Token: 0x04002F34 RID: 12084
		protected bool finishRotationMatch;

		// Token: 0x04002F35 RID: 12085
		protected bool finishPositionXZMatch;

		// Token: 0x04002F36 RID: 12086
		protected bool finishPositionYMatch;

		// Token: 0x04002F37 RID: 12087
		private uint actionGeneration;

		// Token: 0x04002F38 RID: 12088
		private uint? endingActionGeneration;

		// Token: 0x04002F39 RID: 12089
		private Rigidbody interpolationBody;

		// Token: 0x04002F3A RID: 12090
		private RigidbodyInterpolation interpolationBeforeAction;

		// Token: 0x04002F3B RID: 12091
		internal Dictionary<Collider, vGenericAction.ActionStorage> actions;

		// Token: 0x02000716 RID: 1814
		internal class ActionStorage
		{
			// Token: 0x06002F07 RID: 12039 RVA: 0x00004D1F File Offset: 0x00002F1F
			internal ActionStorage()
			{
			}

			// Token: 0x06002F08 RID: 12040 RVA: 0x001099EC File Offset: 0x00107BEC
			internal ActionStorage(vTriggerGenericAction action)
			{
				this.action = action;
				action.OnValidate.AddListener(delegate(GameObject o)
				{
					this.isValid = true;
				});
				action.OnInvalidate.AddListener(delegate(GameObject o)
				{
					this.isValid = false;
				});
			}

			// Token: 0x06002F09 RID: 12041 RVA: 0x00109A29 File Offset: 0x00107C29
			public static implicit operator vTriggerGenericAction(vGenericAction.ActionStorage storage)
			{
				return storage.action;
			}

			// Token: 0x06002F0A RID: 12042 RVA: 0x00109A31 File Offset: 0x00107C31
			public static implicit operator vGenericAction.ActionStorage(vTriggerGenericAction action)
			{
				return new vGenericAction.ActionStorage(action);
			}

			// Token: 0x04002F3C RID: 12092
			internal vTriggerGenericAction action;

			// Token: 0x04002F3D RID: 12093
			internal bool isValid;
		}
	}
}
