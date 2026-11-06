using System;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ECB RID: 3787
	public sealed class NPCAmbientLife
	{
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x0600714E RID: 29006 RVA: 0x002E00ED File Offset: 0x002DE2ED
		private NPCAmbientLifeSettings Settings
		{
			get
			{
				return NPCAmbientLifeSettings.Current;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x0600714F RID: 29007 RVA: 0x002E00F4 File Offset: 0x002DE2F4
		// (set) Token: 0x06007150 RID: 29008 RVA: 0x002E00FC File Offset: 0x002DE2FC
		public string Activity { get; private set; } = "None";

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06007151 RID: 29009 RVA: 0x002E0105 File Offset: 0x002DE305
		public NPCInterestPoint Point
		{
			get
			{
				return this.poi;
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06007152 RID: 29010 RVA: 0x002E010D File Offset: 0x002DE30D
		// (set) Token: 0x06007153 RID: 29011 RVA: 0x002E0115 File Offset: 0x002DE315
		public bool IsSpeaking { get; private set; }

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06007154 RID: 29012 RVA: 0x002E011E File Offset: 0x002DE31E
		public bool HasVisibleProp
		{
			get
			{
				return (this.drink != null && this.drink.activeSelf) || (this.bag != null && this.bag.activeSelf);
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06007155 RID: 29013 RVA: 0x002E0158 File Offset: 0x002DE358
		public bool IsShopping
		{
			get
			{
				return this.shopping;
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06007156 RID: 29014 RVA: 0x002E0160 File Offset: 0x002DE360
		public bool KeepConversation
		{
			get
			{
				return this.conversation != null && this.conversation.MemberCount >= 2 && this.socialGrace > 0f;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06007157 RID: 29015 RVA: 0x002E0187 File Offset: 0x002DE387
		public bool CanSeekCompany
		{
			get
			{
				return Time.time >= this.nextSocialVisit;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06007158 RID: 29016 RVA: 0x002E0199 File Offset: 0x002DE399
		private float SipDuration
		{
			get
			{
				return Mathf.Lerp(2.466667f, 2.633333f, (this.animator != null) ? Mathf.Clamp01(this.animator.GetFloat("Stance")) : 0f);
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06007159 RID: 29017 RVA: 0x002E01D4 File Offset: 0x002DE3D4
		internal bool IsTakingSip
		{
			get
			{
				return this.drinking && this.drinkPhase == 2;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x0600715A RID: 29018 RVA: 0x002E01EC File Offset: 0x002DE3EC
		internal float DrinkContactWeight
		{
			get
			{
				if (!this.drinking)
				{
					return 0f;
				}
				if (this.drinkPhase == 2)
				{
					return Mathf.SmoothStep(0f, 1f, (this.SipDuration - this.remaining) / 0.15f);
				}
				if (this.drinkPhase != 3)
				{
					return 0f;
				}
				return Mathf.SmoothStep(1f, 0f, (0.8333f - this.remaining) / 0.15f);
			}
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x0600715B RID: 29019 RVA: 0x002E0263 File Offset: 0x002DE463
		public Transform Root
		{
			get
			{
				if (!(this.owner != null))
				{
					return null;
				}
				return this.owner.transform;
			}
		}

		// Token: 0x0600715C RID: 29020 RVA: 0x002E0280 File Offset: 0x002DE480
		public NPCAmbientLife(NPCWanderController owner, AStarNPCController controller, NPCStateMachine fsm)
		{
			this.owner = owner;
			this.controller = controller;
			this.fsm = fsm;
			this.look = owner.GetComponent<NPCSimpleLookAt>();
			this.fade = owner.GetComponent<NPCSpawnFade>();
			this.civilian = owner.GetComponent<NPCAnimationStance>() != null;
			this.random = new Random(owner.GetInstanceID() * 397);
			if (fsm != null)
			{
				fsm.OnStateChanged += this.OnStateChanged;
			}
		}

		// Token: 0x0600715D RID: 29021 RVA: 0x002E0342 File Offset: 0x002DE542
		private float Range(float min, float max)
		{
			return Mathf.Lerp(min, max, (float)this.random.NextDouble());
		}

		// Token: 0x0600715E RID: 29022 RVA: 0x002E0357 File Offset: 0x002DE557
		private float Range(Vector2 range)
		{
			return this.Range(range.x, Mathf.Max(range.x, range.y));
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x0600715F RID: 29023 RVA: 0x002E0378 File Offset: 0x002DE578
		public bool Supported
		{
			get
			{
				NPCAmbientLifeSettings settings = this.Settings;
				if (!this.civilian || settings == null || !settings.enableAmbientLife || this.controller == null || this.controller.SuppressPOIBehavior)
				{
					return false;
				}
				Animator animator = this.controller.animator;
				if (animator == null || !animator.isHuman || animator.runtimeAnimatorController == null)
				{
					return false;
				}
				if (this.animator != animator || this.cachedController != animator.runtimeAnimatorController)
				{
					this.StopVisuals();
					this.animator = animator;
					this.cachedController = animator.runtimeAnimatorController;
					this.bodyRenderer = this.animator.GetComponentInChildren<SkinnedMeshRenderer>(true);
					this.layer = this.animator.GetLayerIndex("Ambient Life");
					this.carryLayer = this.animator.GetLayerIndex("Ambient Carry");
				}
				return this.layer >= 0;
			}
		}

		// Token: 0x06007160 RID: 29024 RVA: 0x002E0473 File Offset: 0x002DE673
		private static bool AllowsAmbientState(NPCStateType state)
		{
			return state.IsNormalState() || state == NPCStateType.LookingAt;
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06007161 RID: 29025 RVA: 0x002E0484 File Offset: 0x002DE684
		private bool CanPresent
		{
			get
			{
				return this.Supported && this.owner != null && this.owner.isActiveAndEnabled && this.owner.CanRunAmbientLife && this.animator.isActiveAndEnabled && this.controller.CurrentPosture == NPCInterestPoint.PostureType.Standing && this.controller.CurrentAction == NPCInterestPoint.ActionType.None && (this.controller.equipmentManager == null || !this.controller.equipmentManager.ArePropsLocked) && (this.fsm == null || NPCAmbientLife.AllowsAmbientState(this.fsm.CurrentStateType));
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06007162 RID: 29026 RVA: 0x002E0535 File Offset: 0x002DE735
		internal bool IsSocialAvailable
		{
			get
			{
				return this.poi != null && this.poi.gameObject.activeInHierarchy && this.owner.CurrentState == NPCWanderController.WanderState.Performing && this.CanPresent;
			}
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x002E056D File Offset: 0x002DE76D
		public bool BeginTravel()
		{
			this.End();
			if (!this.Supported)
			{
				return false;
			}
			this.ChooseCarryIdentity();
			this.travelling = true;
			return this.shopping && this.Settings.shoppingBagPrefab != null;
		}

		// Token: 0x06007164 RID: 29028 RVA: 0x002E05A8 File Offset: 0x002DE7A8
		private void ChooseCarryIdentity()
		{
			if (this.shoppingChosen)
			{
				return;
			}
			this.shopping = this.Range(0f, 1f) < this.Settings.shoppingChance;
			this.workBag = this.shopping && this.Settings.workBagPrefab != null && this.Range(0f, 1f) < this.Settings.workBagShare;
			this.shoppingChosen = true;
		}

		// Token: 0x06007165 RID: 29029 RVA: 0x002E062C File Offset: 0x002DE82C
		public bool ChoosePointActivity(NPCInterestPoint point)
		{
			return this.Supported && !(point == null) && point.AllowsAmbientLife && this.owner.CanRunAmbientLife && (!(this.fsm != null) || NPCAmbientLife.AllowsAmbientState(this.fsm.CurrentStateType)) && (!(this.controller.equipmentManager != null) || !this.controller.equipmentManager.ArePropsLocked) && (this.shopping || point.SupportsAmbientConversation || point.ambientActivity != NPCInterestPoint.AmbientActivity.Automatic || point.allowedActions == null || point.allowedActions.Count <= 1 || this.Range(0f, 1f) < this.Settings.ambientStopChance);
		}

		// Token: 0x06007166 RID: 29030 RVA: 0x002E06F8 File Offset: 0x002DE8F8
		public float BeginPoint(NPCInterestPoint point, float authoredWait)
		{
			this.End();
			if (this.shopping && this.Supported && point != null && point.AllowsAmbientLife && (point.allowedActions == null || point.allowedActions.Count == 0 || point.allowedActions.Contains(NPCInterestPoint.ActionType.None)) && this.owner.CanRunAmbientLife && (this.fsm == null || NPCAmbientLife.AllowsAmbientState(this.fsm.CurrentStateType)))
			{
				this.controller.SetAction(NPCInterestPoint.ActionType.None);
			}
			if (!this.CanPresent || point == null || !point.AllowsAmbientLife)
			{
				return authoredWait;
			}
			this.poi = point;
			this.ChooseCarryIdentity();
			if (point.SupportsAmbientConversation)
			{
				this.conversation = point.GetAmbientConversation();
				this.conversation.Join(this);
				this.Activity = "Waiting for company";
				return Mathf.Max(authoredWait, this.Range(this.Settings.socialWait));
			}
			this.drinking = !this.shopping && point.ambientActivity != NPCInterestPoint.AmbientActivity.Observe && this.Settings.drinkPrefab != null && (point.ambientActivity == NPCInterestPoint.AmbientActivity.Refreshment || this.Range(0f, 1f) < this.Settings.drinkChance);
			if (this.drinking)
			{
				this.drinking = this.FindDrinkFacing();
			}
			this.remaining = this.Range(2.5f, 5f);
			this.idleGesture = null;
			this.gestureRemaining = this.Range(1.5f, 4f);
			this.idleGazeTime = 0f;
			return Mathf.Max(authoredWait, this.Range(this.Settings.soloWait));
		}

		// Token: 0x06007167 RID: 29031 RVA: 0x002E08B4 File Offset: 0x002DEAB4
		public void Tick(float delta)
		{
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (!this.CanPresent)
			{
				this.End();
				return;
			}
			if (this.fade != null && this.fade.isActiveAndEnabled && this.fade.IsFading)
			{
				this.StopVisuals();
				return;
			}
			if (this.travelling)
			{
				if (this.owner.CurrentState != NPCWanderController.WanderState.Navigating && this.owner.CurrentState != NPCWanderController.WanderState.Aligning && this.owner.CurrentState != NPCWanderController.WanderState.WaitingNavMode)
				{
					this.StopVisuals();
					return;
				}
				if (this.shopping && this.EnsureProp(ref this.bag, false))
				{
					this.SetPose("CarryBag", delta, 1f);
					this.Activity = (this.workBag ? "Carrying a work bag" : "Shopping");
				}
				return;
			}
			else
			{
				if (this.poi == null || !this.poi.gameObject.activeInHierarchy || !this.poi.AllowsAmbientLife || this.owner.CurrentState != NPCWanderController.WanderState.Performing)
				{
					this.End();
					return;
				}
				if (this.conversation != null)
				{
					if (this.socialGrace > 0f)
					{
						this.socialGrace = Mathf.Max(0f, this.socialGrace - delta);
					}
					this.conversation.Tick(delta);
					if (this.conversation != null)
					{
						this.conversation.Present(this, delta);
					}
					return;
				}
				if (this.shopping && this.EnsureProp(ref this.bag, false))
				{
					this.SetPose("CarryBag", delta, 1f);
					this.Activity = "Resting with shopping";
					this.Observe(delta);
					return;
				}
				if (this.drinking)
				{
					if (this.turnForDrink)
					{
						this.owner.SetAmbientFacing(Quaternion.RotateTowards(this.Root.rotation, this.drinkFacing, 120f * delta));
						if (Quaternion.Angle(this.Root.rotation, this.drinkFacing) > 3f)
						{
							this.SetPose("Listen", delta, 0.65f);
							this.ClearLook();
							this.Activity = "Making room for a drink";
							return;
						}
						this.turnForDrink = false;
					}
					if (!this.EnsureProp(ref this.drink, true))
					{
						return;
					}
					this.remaining -= delta;
					if (this.remaining <= 0f)
					{
						this.drinkPhase = (this.drinkPhase + 1) % 4;
						this.remaining = ((this.drinkPhase == 0) ? this.Range(4f, 8f) : ((this.drinkPhase == 2) ? this.SipDuration : 0.833333f));
						this.ClearLook();
					}
					this.SetPose((this.drinkPhase == 0) ? "DrinkHold" : ((this.drinkPhase == 1) ? "DrinkRaise" : ((this.drinkPhase == 2) ? "DrinkSip" : "DrinkLower")), delta, 1f);
					this.Activity = ((this.drinkPhase == 0) ? "Holding a drink" : "Taking a sip");
					if (this.drinkPhase == 0)
					{
						this.Observe(delta);
						return;
					}
				}
				else
				{
					this.gestureRemaining -= delta;
					if (this.gestureRemaining <= 0f)
					{
						if (this.idleGesture != null)
						{
							this.idleGesture = null;
							this.gestureRemaining = this.Range(5f, 9f);
						}
						else
						{
							this.idleGesture = ((this.Range(0f, 1f) < 0.5f) ? "Stretch" : "LookAround");
							this.gestureRemaining = this.Settings.GestureDuration(this.idleGesture);
							this.ClearLook();
						}
					}
					this.SetPose(this.idleGesture ?? "Listen", delta, (this.idleGesture != null) ? 1f : 0.65f);
					this.Activity = ((this.idleGesture == "Stretch") ? "Stretching shoulders" : "Observing surroundings");
					if (this.idleGesture == null)
					{
						this.Observe(delta);
					}
				}
				return;
			}
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x002E0C9C File Offset: 0x002DEE9C
		private bool FindDrinkFacing()
		{
			for (int i = 0; i < 8; i++)
			{
				float num = ((i == 0) ? 0f : ((float)((i + 1) / 2) * 45f * (float)((i % 2 == 1) ? 1 : (-1))));
				Quaternion quaternion = this.Root.rotation * Quaternion.Euler(0f, num, 0f);
				Vector3 vector = quaternion * Vector3.forward;
				Vector3 vector2 = quaternion * Vector3.right;
				bool flag = true;
				int num2 = 0;
				while (num2 < 3 && flag)
				{
					Vector3 vector3 = this.Root.position + Vector3.up * (0.95f + (float)num2 * 0.3f) + vector2 * 0.2f;
					int num3 = Physics.OverlapSphereNonAlloc(vector3 + vector * 0.3f, 0.24f, this.clearanceOverlaps, -5, QueryTriggerInteraction.Ignore);
					if (num3 == this.clearanceOverlaps.Length)
					{
						flag = false;
						break;
					}
					for (int j = 0; j < num3; j++)
					{
						if (this.clearanceOverlaps[j] != null && !this.clearanceOverlaps[j].transform.IsChildOf(this.Root))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					int num4 = Physics.SphereCastNonAlloc(vector3, 0.12f, vector, this.clearanceHits, 0.65f, -5, QueryTriggerInteraction.Ignore);
					if (num4 == this.clearanceHits.Length)
					{
						flag = false;
						break;
					}
					for (int k = 0; k < num4; k++)
					{
						if (this.clearanceHits[k].transform != null && !this.clearanceHits[k].transform.IsChildOf(this.Root))
						{
							flag = false;
							break;
						}
					}
					num2++;
				}
				if (flag)
				{
					this.drinkFacing = quaternion;
					this.turnForDrink = i != 0;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007169 RID: 29033 RVA: 0x002E0E8C File Offset: 0x002DF08C
		internal void PresentConversation(NPCAmbientLife partner, bool speaking, int variant, float delta)
		{
			if (!this.CanPresent)
			{
				return;
			}
			this.IsSpeaking = speaking && partner != null;
			if (partner == null)
			{
				this.Activity = "Waiting for company";
				if (this.shopping && this.EnsureProp(ref this.bag, false))
				{
					this.SetPose("CarryBag", delta, 1f);
				}
				else
				{
					this.SetPose("Listen", delta, 0.65f);
				}
				this.Observe(delta);
				return;
			}
			this.Activity = (speaking ? "Talking" : "Listening");
			if (this.socialGrace < 0f)
			{
				this.socialGrace = 12f;
				this.greetingRemaining = (this.shopping ? 0f : this.Settings.greetingDuration);
			}
			if (this.shopping && this.EnsureProp(ref this.bag, false))
			{
				this.SetPose("CarryBag", delta, 1f);
			}
			else if (this.greetingRemaining > 0f)
			{
				this.greetingRemaining -= delta;
				this.SetPose("Greeting", delta, 1f);
				this.Activity = "Greeting company";
			}
			else
			{
				this.SetPose(speaking ? ((variant == 0) ? "TalkA" : "TalkB") : "Listen", delta, speaking ? 0.9f : 0.65f);
			}
			Vector3 vector = partner.Root.position - this.Root.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.04f)
			{
				Quaternion quaternion = Quaternion.RotateTowards(this.Root.rotation, Quaternion.LookRotation(vector), 75f * delta);
				this.owner.SetAmbientFacing(quaternion);
			}
			Transform transform = ((partner.animator != null && partner.animator.isHuman) ? partner.animator.GetBoneTransform(HumanBodyBones.Head) : partner.Root);
			this.SetLook(transform);
		}

		// Token: 0x0600716A RID: 29034 RVA: 0x002E1084 File Offset: 0x002DF284
		private void Observe(float delta)
		{
			this.idleGazeTime -= delta;
			if (this.idleGazeTime > 0f)
			{
				return;
			}
			this.idleGazeTime = this.Range(2.5f, 5.5f);
			if (this.poi != null && this.poi.interactionTarget != null)
			{
				this.SetLook(this.poi.interactionTarget);
				return;
			}
			if (this.Range(0f, 1f) < 0.3f)
			{
				this.ClearLook();
				return;
			}
			if (this.gaze == null)
			{
				this.gaze = new GameObject("Ambient gaze target");
				this.gaze.transform.SetParent(this.Root, false);
			}
			this.gaze.transform.position = this.Root.position + this.Root.forward * 5f + this.Root.right * this.Range(-3f, 3f) + Vector3.up * this.Range(1.3f, 1.8f);
			this.SetLook(this.gaze.transform);
		}

		// Token: 0x0600716B RID: 29035 RVA: 0x002E11D4 File Offset: 0x002DF3D4
		private void SetLook(Transform target)
		{
			if (this.look == null || !this.look.isActiveAndEnabled || target == null)
			{
				return;
			}
			if (this.look.IsLooking && !this.look.IsLookingAt(this.ownLookTarget))
			{
				return;
			}
			if (this.ownLookTarget != target)
			{
				this.ClearLook();
			}
			this.ownLookTarget = target;
			this.look.StartLookingAt(target);
		}

		// Token: 0x0600716C RID: 29036 RVA: 0x002E124E File Offset: 0x002DF44E
		private void ClearLook()
		{
			if (this.look != null && this.ownLookTarget != null)
			{
				this.look.StopLookingAt(this.ownLookTarget);
			}
			this.ownLookTarget = null;
		}

		// Token: 0x0600716D RID: 29037 RVA: 0x002E1284 File Offset: 0x002DF484
		private void SetPose(string state, float delta, float targetWeight = 1f)
		{
			if (this.animator == null || this.layer < 0 || this.layer >= this.animator.layerCount)
			{
				return;
			}
			if (this.carryLayer >= 0)
			{
				bool flag = state == "CarryBag";
				this.carryWeight = Mathf.MoveTowards(this.carryWeight, (float)(flag ? 1 : 0), delta * 4f);
				this.animator.SetLayerWeight(this.carryLayer, this.carryWeight);
				if (flag)
				{
					if (this.animator.GetCurrentAnimatorStateInfo(this.carryLayer).shortNameHash != Animator.StringToHash("CarryBag"))
					{
						this.animator.Play("Ambient Carry.CarryBag", this.carryLayer, 0f);
					}
					this.poseWeight = Mathf.MoveTowards(this.poseWeight, 0f, delta * 4f);
					this.animator.SetLayerWeight(this.layer, this.poseWeight);
					this.pose = null;
					return;
				}
			}
			int num = Animator.StringToHash(state);
			if (this.pose != state || (!this.animator.IsInTransition(this.layer) && this.animator.GetCurrentAnimatorStateInfo(this.layer).shortNameHash != num))
			{
				bool flag2 = state.StartsWith("Drink", StringComparison.Ordinal);
				bool flag3 = state == "Stretch" || state == "LookAround" || state == "Greeting";
				this.animator.CrossFadeInFixedTime(Animator.StringToHash("Ambient Life." + state), flag2 ? 0.15f : 0.3f, this.layer, (flag2 || flag3) ? 0f : this.Range(0f, 0.65f));
				this.pose = state;
			}
			this.poseWeight = Mathf.MoveTowards(this.poseWeight, targetWeight, delta * 3f);
			this.animator.SetLayerWeight(this.layer, this.poseWeight);
		}

		// Token: 0x0600716E RID: 29038 RVA: 0x002E1490 File Offset: 0x002DF690
		private bool EnsureProp(ref GameObject prop, bool beverage)
		{
			NPCAmbientLifeSettings settings = this.Settings;
			GameObject gameObject = (beverage ? settings.drinkPrefab : ((this.workBag && settings.workBagPrefab != null) ? settings.workBagPrefab : settings.shoppingBagPrefab));
			if (gameObject == null || this.animator == null || !this.animator.isHuman)
			{
				return false;
			}
			Transform boneTransform = this.animator.GetBoneTransform(beverage ? settings.drinkHand : settings.bagHand);
			if (boneTransform == null)
			{
				return false;
			}
			if (prop == null)
			{
				prop = Object.Instantiate<GameObject>(gameObject, boneTransform, false);
				prop.name = (beverage ? "AmbientDrink" : "AmbientShoppingBag");
				Transform[] componentsInChildren = prop.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = boneTransform.gameObject.layer;
				}
			}
			else if (prop.transform.parent != boneTransform)
			{
				prop.transform.SetParent(boneTransform, false);
			}
			Mesh mesh = ((this.bodyRenderer != null) ? this.bodyRenderer.sharedMesh : null);
			prop.transform.localPosition = (beverage ? settings.DrinkPosition(this.animator.GetFloat("Stance"), mesh) : settings.bagPosition);
			prop.transform.localRotation = Quaternion.Euler(beverage ? settings.drinkRotation : settings.bagRotation);
			if (beverage)
			{
				prop.transform.localScale = settings.DrinkScale(this.animator.GetFloat("Stance"), mesh);
			}
			if (!prop.activeSelf)
			{
				prop.SetActive(true);
			}
			return true;
		}

		// Token: 0x0600716F RID: 29039 RVA: 0x002E164C File Offset: 0x002DF84C
		private void OnStateChanged(NPCStateType previous, NPCStateType next, string source)
		{
			if (!NPCAmbientLife.AllowsAmbientState(next))
			{
				this.End();
			}
		}

		// Token: 0x06007170 RID: 29040 RVA: 0x002E165C File Offset: 0x002DF85C
		private void StopVisuals()
		{
			this.ClearLook();
			if (this.drink != null)
			{
				this.drink.SetActive(false);
			}
			if (this.bag != null)
			{
				this.bag.SetActive(false);
			}
			if (this.animator != null && this.animator.isActiveAndEnabled && this.animator.runtimeAnimatorController == this.cachedController && this.layer >= 0 && this.layer < this.animator.layerCount)
			{
				this.animator.SetLayerWeight(this.layer, 0f);
				if (this.carryLayer >= 0 && this.carryLayer < this.animator.layerCount)
				{
					this.animator.SetLayerWeight(this.carryLayer, 0f);
				}
			}
			this.pose = null;
			this.poseWeight = 0f;
			this.carryWeight = 0f;
			this.IsSpeaking = false;
			this.Activity = "None";
		}

		// Token: 0x06007171 RID: 29041 RVA: 0x002E176C File Offset: 0x002DF96C
		public void End()
		{
			NPCAmbientConversation npcambientConversation = this.conversation;
			this.conversation = null;
			if (npcambientConversation != null && this.socialGrace >= 0f && this.Settings != null)
			{
				this.nextSocialVisit = Time.time + this.Range(this.Settings.socialCooldown);
			}
			if (npcambientConversation != null)
			{
				npcambientConversation.Leave(this);
			}
			this.poi = null;
			this.travelling = false;
			this.drinking = false;
			this.turnForDrink = false;
			this.drinkPhase = 0;
			this.socialGrace = -1f;
			this.StopVisuals();
		}

		// Token: 0x06007172 RID: 29042 RVA: 0x002E1800 File Offset: 0x002DFA00
		public void Dispose()
		{
			this.End();
			if (this.fsm != null)
			{
				this.fsm.OnStateChanged -= this.OnStateChanged;
			}
			if (this.drink != null)
			{
				Object.Destroy(this.drink);
			}
			if (this.bag != null)
			{
				Object.Destroy(this.bag);
			}
			if (this.gaze != null)
			{
				Object.Destroy(this.gaze);
			}
		}

		// Token: 0x0400742A RID: 29738
		public const string LayerName = "Ambient Life";

		// Token: 0x0400742B RID: 29739
		public const string CarryLayerName = "Ambient Carry";

		// Token: 0x0400742C RID: 29740
		private readonly NPCWanderController owner;

		// Token: 0x0400742D RID: 29741
		private readonly AStarNPCController controller;

		// Token: 0x0400742E RID: 29742
		private readonly NPCStateMachine fsm;

		// Token: 0x0400742F RID: 29743
		private readonly NPCSimpleLookAt look;

		// Token: 0x04007430 RID: 29744
		private readonly NPCSpawnFade fade;

		// Token: 0x04007431 RID: 29745
		private readonly Random random;

		// Token: 0x04007432 RID: 29746
		private readonly bool civilian;

		// Token: 0x04007433 RID: 29747
		private Animator animator;

		// Token: 0x04007434 RID: 29748
		private SkinnedMeshRenderer bodyRenderer;

		// Token: 0x04007435 RID: 29749
		private RuntimeAnimatorController cachedController;

		// Token: 0x04007436 RID: 29750
		private int layer = -1;

		// Token: 0x04007437 RID: 29751
		private int carryLayer = -1;

		// Token: 0x04007438 RID: 29752
		private NPCInterestPoint poi;

		// Token: 0x04007439 RID: 29753
		private NPCAmbientConversation conversation;

		// Token: 0x0400743A RID: 29754
		private GameObject drink;

		// Token: 0x0400743B RID: 29755
		private GameObject bag;

		// Token: 0x0400743C RID: 29756
		private GameObject gaze;

		// Token: 0x0400743D RID: 29757
		private Transform ownLookTarget;

		// Token: 0x0400743E RID: 29758
		private string pose;

		// Token: 0x0400743F RID: 29759
		private float remaining;

		// Token: 0x04007440 RID: 29760
		private float idleGazeTime;

		// Token: 0x04007441 RID: 29761
		private float poseWeight;

		// Token: 0x04007442 RID: 29762
		private float carryWeight;

		// Token: 0x04007443 RID: 29763
		private float socialGrace = -1f;

		// Token: 0x04007444 RID: 29764
		private string idleGesture;

		// Token: 0x04007445 RID: 29765
		private float gestureRemaining;

		// Token: 0x04007446 RID: 29766
		private float greetingRemaining;

		// Token: 0x04007447 RID: 29767
		private float nextSocialVisit;

		// Token: 0x04007448 RID: 29768
		private readonly RaycastHit[] clearanceHits = new RaycastHit[12];

		// Token: 0x04007449 RID: 29769
		private readonly Collider[] clearanceOverlaps = new Collider[12];

		// Token: 0x0400744A RID: 29770
		private Quaternion drinkFacing;

		// Token: 0x0400744B RID: 29771
		private bool turnForDrink;

		// Token: 0x0400744C RID: 29772
		private bool shopping;

		// Token: 0x0400744D RID: 29773
		private bool shoppingChosen;

		// Token: 0x0400744E RID: 29774
		private bool travelling;

		// Token: 0x0400744F RID: 29775
		private bool drinking;

		// Token: 0x04007450 RID: 29776
		private bool workBag;

		// Token: 0x04007451 RID: 29777
		private int drinkPhase;
	}
}
