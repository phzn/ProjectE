using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.BoobTouch;
using Features.MainMenu;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace Features.BodyPhysics
{
	// Token: 0x020014F7 RID: 5367
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(31000)]
	public sealed class AnimeBodyPhysics : MonoBehaviour
	{
		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x0600A5CF RID: 42447 RVA: 0x004541E8 File Offset: 0x004523E8
		public static bool AnimeSelected
		{
			get
			{
				return GameSettingsService.BodyPhysics == GameSettingsService.BodyPhysicsStyle.Anime;
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600A5D0 RID: 42448 RVA: 0x004541F2 File Offset: 0x004523F2
		public bool DrivesBreasts
		{
			get
			{
				return this._breastsDriven;
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600A5D1 RID: 42449 RVA: 0x004541FA File Offset: 0x004523FA
		public bool DrivesButt
		{
			get
			{
				return this._buttDriven;
			}
		}

		// Token: 0x0600A5D2 RID: 42450 RVA: 0x00454204 File Offset: 0x00452404
		public static AnimeBodyPhysics Ensure(GameObject host)
		{
			if (!Application.isPlaying || host == null)
			{
				return null;
			}
			AnimeBodyPhysics component = host.GetComponent<AnimeBodyPhysics>();
			if (!(component != null))
			{
				return host.AddComponent<AnimeBodyPhysics>();
			}
			return component;
		}

		// Token: 0x0600A5D3 RID: 42451 RVA: 0x0045423C File Offset: 0x0045243C
		public static bool TryShake(Transform character, Vector3 localDirection, bool seeSaw, float strength01 = 1f)
		{
			if (character == null)
			{
				return false;
			}
			AnimeBodyPhysics animeBodyPhysics = character.GetComponentInChildren<AnimeBodyPhysics>(true);
			if (animeBodyPhysics == null)
			{
				animeBodyPhysics = character.GetComponentInParent<AnimeBodyPhysics>();
			}
			return animeBodyPhysics != null && animeBodyPhysics.Shake(localDirection, seeSaw, strength01);
		}

		// Token: 0x0600A5D4 RID: 42452 RVA: 0x00454280 File Offset: 0x00452480
		private void OnEnable()
		{
			this.Bind();
			this._hasLastRoot = false;
			this._carrier = null;
			this._fixedSteps = 0;
			this._unscaledSinceTick = 0f;
			if (this._restore == null)
			{
				this._restore = new Action(this.RestoreBones);
			}
			if (this._write == null)
			{
				this._write = new Action(this.WriteBones);
			}
			AnimeBodyPhysicsLoop.Add(this._restore, this._write);
		}

		// Token: 0x0600A5D5 RID: 42453 RVA: 0x004542F8 File Offset: 0x004524F8
		private void OnDisable()
		{
			AnimeBodyPhysicsLoop.Remove(this._restore, this._write);
			this.StopBreasts();
			this.StopButt();
		}

		// Token: 0x0600A5D6 RID: 42454 RVA: 0x00454318 File Offset: 0x00452518
		private void Bind()
		{
			if (this._bound)
			{
				return;
			}
			if (this._config == null)
			{
				this._config = AnimeBodyPhysicsConfigSO.Load();
			}
			if (this._config == null)
			{
				this._config = ScriptableObject.CreateInstance<AnimeBodyPhysicsConfigSO>();
			}
			this._animator = base.GetComponentInParent<Animator>();
			if (this._animator == null)
			{
				this._animator = base.GetComponentInChildren<Animator>(true);
			}
			this._root = ((this._animator != null) ? this._animator.transform : base.transform);
			this._wardrobe = base.GetComponentInChildren<WardrobeBodyPhysicsController>(true);
			if (this._wardrobe == null)
			{
				this._wardrobe = base.GetComponentInParent<WardrobeBodyPhysicsController>();
			}
			this._boobTouch = this._root.GetComponentInChildren<BoobTouchModeController>(true);
			this._motor = this._root.GetComponent<vThirdPersonMotor>();
			this._input = this._root.GetComponent<vThirdPersonInput>();
			this.ScanCloths();
			this._leftBreast.Spring = AnimeBodyPhysics.Find(this._root, "c_breast_01.l");
			this._rightBreast.Spring = AnimeBodyPhysics.Find(this._root, "c_breast_01.r");
			this._leftBreast.Base = AnimeBodyPhysics.Find(this._root, "c_breast_02.l");
			this._rightBreast.Base = AnimeBodyPhysics.Find(this._root, "c_breast_02.r");
			this._leftCheek.Bone = AnimeBodyPhysics.Find(this._root, "Butt_L");
			this._rightCheek.Bone = AnimeBodyPhysics.Find(this._root, "Butt_R");
			if (this._animator != null && this._animator.isHuman)
			{
				this._hips = this._animator.GetBoneTransform(HumanBodyBones.Hips);
				this._leftFoot.Bone = this._animator.GetBoneTransform(HumanBodyBones.LeftFoot);
				this._rightFoot.Bone = this._animator.GetBoneTransform(HumanBodyBones.RightFoot);
				this._leftHand.Wrist = this._animator.GetBoneTransform(HumanBodyBones.LeftHand);
				this._rightHand.Wrist = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
				this._leftHand.Knuckle = this._animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
				this._rightHand.Knuckle = this._animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
			}
			this._hasBoobsType = (this._hasIsRiding = false);
			if (this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
				{
					if (animatorControllerParameter.name == "BoobsType")
					{
						this._boobsTypeHash = animatorControllerParameter.nameHash;
						this._boobsTypeType = animatorControllerParameter.type;
						this._hasBoobsType = true;
					}
					else if (animatorControllerParameter.nameHash == AnimeBodyPhysics.IsRidingHash && animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						this._hasIsRiding = true;
					}
				}
			}
			this._bound = this._animator != null && ((this._leftBreast.Spring != null && this._rightBreast.Spring != null) || (this._leftCheek.Bone != null && this._rightCheek.Bone != null));
		}

		// Token: 0x0600A5D7 RID: 42455 RVA: 0x0045467C File Offset: 0x0045287C
		private void ScanCloths()
		{
			this._breastCloths.Clear();
			this._buttCloths.Clear();
			foreach (MagicaCloth magicaCloth in this._root.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (!(magicaCloth == null))
				{
					if (AnimeBodyPhysics.Matches(magicaCloth.gameObject.name, AnimeBodyPhysics.BreastClothNames))
					{
						this._breastCloths.Add(magicaCloth);
					}
					else if (AnimeBodyPhysics.Matches(magicaCloth.gameObject.name, AnimeBodyPhysics.ButtClothNames))
					{
						this._buttCloths.Add(magicaCloth);
					}
				}
			}
			this._expectedBreastClothCount = Mathf.Max(this._expectedBreastClothCount, this._breastCloths.Count);
			this._expectedButtClothCount = Mathf.Max(this._expectedButtClothCount, this._buttCloths.Count);
		}

		// Token: 0x0600A5D8 RID: 42456 RVA: 0x00454748 File Offset: 0x00452948
		private void RescanDestroyedCloths()
		{
			if (!AnimeBodyPhysics.AnyDestroyed(this._breastCloths) && !AnimeBodyPhysics.AnyDestroyed(this._buttCloths) && this._breastCloths.Count >= this._expectedBreastClothCount && this._buttCloths.Count >= this._expectedButtClothCount && (this._breastCloths.Count > 0 || this._leftBreast.Spring == null) && (this._buttCloths.Count > 0 || this._leftCheek.Bone == null))
			{
				return;
			}
			if (Time.unscaledTime < this._nextClothScan)
			{
				return;
			}
			this._nextClothScan = Time.unscaledTime + 0.5f;
			this.ScanCloths();
		}

		// Token: 0x0600A5D9 RID: 42457 RVA: 0x00454800 File Offset: 0x00452A00
		private static bool AnyDestroyed(List<MagicaCloth> cloths)
		{
			for (int i = 0; i < cloths.Count; i++)
			{
				if (cloths[i] == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A5DA RID: 42458 RVA: 0x00454830 File Offset: 0x00452A30
		private void LateUpdate()
		{
			if (this._bound && this._animator == null)
			{
				this.StopBreasts();
				this.StopButt();
				this._bound = false;
			}
			if (!this._bound && Time.unscaledTime >= this._nextBindAttempt)
			{
				this._nextBindAttempt = Time.unscaledTime + 0.5f;
				this.Bind();
			}
			if (!this._bound || this._animator == null)
			{
				return;
			}
			this.RescanDestroyedCloths();
			this._unscaledSinceTick += Time.unscaledDeltaTime;
			bool flag = AnimeBodyPhysics.AnimeSelected && ClothQualityService.Enabled && !this.BreastPlayActive;
			bool flag2 = flag && this.HasBreasts && AnimeBodyPhysics.AllOn(this._breastCloths);
			bool flag3 = flag && this.HasButt && AnimeBodyPhysics.AllOn(this._buttCloths);
			if (!flag2)
			{
				this.StopBreasts();
			}
			if (!flag3)
			{
				this.StopButt();
			}
			if (!flag2 && !flag3)
			{
				this._hasLastRoot = false;
				this._carrier = null;
				this._fixedSteps = 0;
				this._unscaledSinceTick = 0f;
				return;
			}
			AnimatorUpdateMode updateMode = this._animator.updateMode;
			float num;
			if (updateMode != AnimatorUpdateMode.AnimatePhysics)
			{
				if (updateMode != AnimatorUpdateMode.UnscaledTime)
				{
					num = Time.deltaTime;
				}
				else
				{
					num = Time.unscaledDeltaTime;
				}
			}
			else
			{
				if (this._fixedSteps == 0)
				{
					return;
				}
				num = (float)this._fixedSteps * Time.fixedDeltaTime;
			}
			this._fixedSteps = 0;
			if (num <= 0f)
			{
				return;
			}
			this.PrepareCarrierFrame(this.Riding && this._animator.updateMode != AnimatorUpdateMode.AnimatePhysics);
			bool flag4 = this.TrackSpeed(num);
			int num2 = this.TrackSteps(num);
			bool stopping = this.Stopping;
			this.UpdateLocomotion(num, stopping, flag4 || (!this._breastsDriven && !this._buttDriven));
			float num3 = ((num2 >= 0) ? (this.StepStrength() * (stopping ? this._config.brakingStepKick : 1f)) : 0f);
			if (flag2)
			{
				this.TickBreasts(num, num2, num3, flag4);
			}
			if (flag3)
			{
				this.TickButt(num, num2, num3, flag4);
			}
			this._lastRootPosition = this._root.position;
			this._lastRootRotation = this._root.rotation;
			this._hasLastRoot = true;
			this._unscaledSinceTick = 0f;
		}

		// Token: 0x0600A5DB RID: 42459 RVA: 0x00454A72 File Offset: 0x00452C72
		private void FixedUpdate()
		{
			this._fixedSteps++;
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x0600A5DC RID: 42460 RVA: 0x00454A82 File Offset: 0x00452C82
		private bool HasBreasts
		{
			get
			{
				return this._breastCloths.Count > 0 && this._leftBreast.Spring != null && this._rightBreast.Spring != null;
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x0600A5DD RID: 42461 RVA: 0x00454AB8 File Offset: 0x00452CB8
		private bool HasButt
		{
			get
			{
				return this._buttCloths.Count > 0 && this._leftCheek.Bone != null && this._rightCheek.Bone != null;
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x0600A5DE RID: 42462 RVA: 0x00454AEE File Offset: 0x00452CEE
		private bool BreastPlayActive
		{
			get
			{
				return this._boobTouch != null && (this._boobTouch.IsActive || this._boobTouch.IsTransitioning);
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x0600A5DF RID: 42463 RVA: 0x00454B1A File Offset: 0x00452D1A
		private bool Riding
		{
			get
			{
				return this._hasIsRiding && this._animator.GetBool(AnimeBodyPhysics.IsRidingHash);
			}
		}

		// Token: 0x0600A5E0 RID: 42464 RVA: 0x00454B38 File Offset: 0x00452D38
		private static bool AllOn(List<MagicaCloth> cloths)
		{
			for (int i = 0; i < cloths.Count; i++)
			{
				if (cloths[i] == null || !cloths[i].isActiveAndEnabled)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A5E1 RID: 42465 RVA: 0x00454B78 File Offset: 0x00452D78
		private bool TrackSpeed(float deltaTime)
		{
			if (!this._hasLastRoot || deltaTime <= 0f)
			{
				return false;
			}
			Vector3 vector = this._root.position - this._lastRootPosition;
			if (vector.sqrMagnitude > 4f)
			{
				this._speed = 0f;
				return true;
			}
			float num = (vector - this._root.up * Vector3.Dot(vector, this._root.up)).magnitude / deltaTime;
			this._speed = Mathf.Lerp(this._speed, num, 1f - Mathf.Exp(-deltaTime / 0.15f));
			return false;
		}

		// Token: 0x0600A5E2 RID: 42466 RVA: 0x00454C24 File Offset: 0x00452E24
		private Vector3 Carried(Vector3 point)
		{
			if (!this._hasLastRoot)
			{
				return point;
			}
			return this._root.position + this._root.rotation * (Quaternion.Inverse(this._lastRootRotation) * (point - this._lastRootPosition));
		}

		// Token: 0x0600A5E3 RID: 42467 RVA: 0x00454C78 File Offset: 0x00452E78
		private void PrepareCarrierFrame(bool mounted)
		{
			Rigidbody rigidbody = ((mounted && this._root.parent != null) ? this._root.parent.GetComponentInParent<Rigidbody>() : null);
			Transform transform = ((rigidbody != null && rigidbody.interpolation == RigidbodyInterpolation.None) ? rigidbody.transform : null);
			if (transform != this._carrier)
			{
				this._leftBreast.Mass.Reset();
				this._rightBreast.Mass.Reset();
				this._leftCheek.Mass.Reset();
				this._rightCheek.Mass.Reset();
				this._carrier = transform;
			}
			else if (transform != null)
			{
				this._leftBreast.Mass.Transport(this._lastCarrierPosition, this._lastCarrierRotation, transform.position, transform.rotation);
				this._rightBreast.Mass.Transport(this._lastCarrierPosition, this._lastCarrierRotation, transform.position, transform.rotation);
				this._leftCheek.Mass.Transport(this._lastCarrierPosition, this._lastCarrierRotation, transform.position, transform.rotation);
				this._rightCheek.Mass.Transport(this._lastCarrierPosition, this._lastCarrierRotation, transform.position, transform.rotation);
			}
			if (transform == null)
			{
				return;
			}
			this._lastCarrierPosition = transform.position;
			this._lastCarrierRotation = transform.rotation;
		}

		// Token: 0x0600A5E4 RID: 42468 RVA: 0x00454DF0 File Offset: 0x00452FF0
		private int TrackSteps(float deltaTime)
		{
			if (this._leftFoot.Bone == null || this._rightFoot.Bone == null || deltaTime <= 0f)
			{
				return -1;
			}
			int num = this.TrackFoot(this._leftFoot, deltaTime);
			int num2 = this.TrackFoot(this._rightFoot, deltaTime);
			if ((!(this._motor == null) && !this._motor.isGrounded) || this.Riding || this._speed < this._config.minStepSpeed || this.UpsideDown)
			{
				return -1;
			}
			if (num >= 0)
			{
				return 0;
			}
			if (num2 < 0)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x0600A5E5 RID: 42469 RVA: 0x00454E9C File Offset: 0x0045309C
		private bool UpsideDown
		{
			get
			{
				if (this._hips == null)
				{
					return false;
				}
				float num = Vector3.Dot(this._hips.position - this._root.position, this._root.up);
				return Vector3.Dot(this._leftFoot.Bone.position - this._root.position, this._root.up) > num || Vector3.Dot(this._rightFoot.Bone.position - this._root.position, this._root.up) > num;
			}
		}

		// Token: 0x0600A5E6 RID: 42470 RVA: 0x00454F50 File Offset: 0x00453150
		private int TrackFoot(AnimeBodyPhysics.Foot foot, float deltaTime)
		{
			float num = Vector3.Dot(foot.Bone.position - this._root.position, this._root.up);
			if (!foot.Tracked)
			{
				foot.Tracked = true;
				foot.Ground = (foot.Height = num);
				foot.Lifted = false;
				foot.FallSpeed = 0f;
				return -1;
			}
			float num2 = (foot.Height - num) / deltaTime;
			bool flag = foot.FallSpeed > this._config.footStopSpeed;
			foot.Height = num;
			foot.FallSpeed = num2;
			if (!foot.Lifted)
			{
				foot.Ground = Mathf.Min(foot.Ground, num);
				if (num <= foot.Ground + this._config.footLiftHeight)
				{
					return -1;
				}
				foot.Lifted = true;
				foot.Peak = num;
				return -1;
			}
			else
			{
				foot.Peak = Mathf.Max(foot.Peak, num);
				if (!flag || num2 > this._config.footStopSpeed || num > foot.Peak - this._config.footLiftHeight)
				{
					return -1;
				}
				foot.Lifted = false;
				foot.Ground = num;
				return 0;
			}
		}

		// Token: 0x0600A5E7 RID: 42471 RVA: 0x0045507B File Offset: 0x0045327B
		private float Locomotion()
		{
			return this._motion;
		}

		// Token: 0x0600A5E8 RID: 42472 RVA: 0x00455084 File Offset: 0x00453284
		private float Gait()
		{
			return Mathf.SmoothStep(0f, 1f, (this._speed - this._config.walkSpeed) / Mathf.Max(0.01f, this._config.runSpeed - this._config.walkSpeed));
		}

		// Token: 0x0600A5E9 RID: 42473 RVA: 0x004550D4 File Offset: 0x004532D4
		private void UpdateLocomotion(float deltaTime, bool stopping, bool cut)
		{
			float num = Mathf.Max(0.01f, this._config.bounceSpeed - this._config.settleSpeed);
			float num2 = (stopping ? 0f : Mathf.SmoothStep(0f, 1f, (this._speed - this._config.settleSpeed) / num));
			this._motion = (cut ? num2 : Mathf.MoveTowards(this._motion, num2, deltaTime / Mathf.Max(0.01f, this._config.settleBlendSeconds)));
			this._stillTime = ((num2 < 0.5f) ? (this._stillTime + deltaTime) : 0f);
		}

		// Token: 0x0600A5EA RID: 42474 RVA: 0x0045517D File Offset: 0x0045337D
		private float SettleWeight()
		{
			return 1f - Mathf.SmoothStep(0f, 1f, (this._stillTime - this._config.settleSeconds) / Mathf.Max(0.01f, this._config.restBlendSeconds));
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x0600A5EB RID: 42475 RVA: 0x004551BC File Offset: 0x004533BC
		private bool Stopping
		{
			get
			{
				if (this._motor == null || this._input == null || !this._input.enabled || this._input.lockInput || this._input.lockMoveInput || this._motor.lockMovement || this.Riding)
				{
					return false;
				}
				Vector3 input = this._motor.input;
				return input.x * input.x + input.z * input.z < 0.01f;
			}
		}

		// Token: 0x0600A5EC RID: 42476 RVA: 0x00455251 File Offset: 0x00453451
		private float StepStrength()
		{
			return Mathf.Pow(Mathf.Clamp01(this._speed / Mathf.Max(0.1f, this._config.fullStepSpeed)), this._config.stepSpeedCurve);
		}

		// Token: 0x0600A5ED RID: 42477 RVA: 0x00455284 File Offset: 0x00453484
		private float Variation(float random)
		{
			return 1f + random * ((float)this._random.NextDouble() * 2f - 1f);
		}

		// Token: 0x0600A5EE RID: 42478 RVA: 0x004552A8 File Offset: 0x004534A8
		private float BlendIntensity(ref float current, float target)
		{
			if (target <= 0f)
			{
				return current = 0f;
			}
			if (current < 0f)
			{
				current = target;
			}
			float num = 1f / Mathf.Max(0.01f, this._config.intensityBlendSeconds);
			current = Mathf.MoveTowards(current, target, num * this._unscaledSinceTick);
			return current;
		}

		// Token: 0x0600A5EF RID: 42479 RVA: 0x00455304 File Offset: 0x00453504
		private float Firmness(float weight)
		{
			return 1f - Mathf.Clamp01(weight);
		}

		// Token: 0x0600A5F0 RID: 42480 RVA: 0x00455314 File Offset: 0x00453514
		private float HandHold(Vector3 from, Vector3 to)
		{
			float num = Mathf.Min(AnimeBodyPhysics.HandDistance(this._leftHand, from, to), AnimeBodyPhysics.HandDistance(this._rightHand, from, to));
			if (num == 3.4028235E+38f)
			{
				return 0f;
			}
			float handHoldRadius = this._config.handHoldRadius;
			float num2 = Mathf.Max(handHoldRadius + 0.001f, this._config.handReleaseRadius);
			return 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(handHoldRadius, num2, num));
		}

		// Token: 0x0600A5F1 RID: 42481 RVA: 0x00455390 File Offset: 0x00453590
		private static float HandDistance(AnimeBodyPhysics.Hand hand, Vector3 from, Vector3 to)
		{
			if (hand.Wrist == null)
			{
				return float.MaxValue;
			}
			Vector3 position = hand.Wrist.position;
			float num = AnimeBodyPhysics.SegmentDistance(position, from, to);
			if (hand.Knuckle == null)
			{
				return num;
			}
			Vector3 position2 = hand.Knuckle.position;
			num = Mathf.Min(num, AnimeBodyPhysics.SegmentDistance(Vector3.Lerp(position, position2, 0.6f), from, to));
			return Mathf.Min(num, AnimeBodyPhysics.SegmentDistance(position2, from, to));
		}

		// Token: 0x0600A5F2 RID: 42482 RVA: 0x0045540C File Offset: 0x0045360C
		private static float SegmentDistance(Vector3 point, Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = ((sqrMagnitude > 1E-08f) ? Mathf.Clamp01(Vector3.Dot(point - from, vector) / sqrMagnitude) : 0f);
			return Vector3.Distance(point, from + vector * num);
		}

		// Token: 0x0600A5F3 RID: 42483 RVA: 0x00455460 File Offset: 0x00453660
		private static float BlendHold(float current, float target, float deltaTime)
		{
			return Mathf.MoveTowards(current, target, deltaTime / 0.12f);
		}

		// Token: 0x0600A5F4 RID: 42484 RVA: 0x00455470 File Offset: 0x00453670
		private void Follow(JiggleMass mass, Vector3 anchor, float deltaTime, float damping, float maxSwing)
		{
			Vector3 vector = ((this._carrier != null) ? mass.Anchor : this.Carried(mass.Anchor));
			if (mass.Moving && (anchor - vector).sqrMagnitude > 0.09f)
			{
				mass.Reset();
			}
			mass.Follow(anchor, deltaTime, damping, maxSwing, 6f);
		}

		// Token: 0x0600A5F5 RID: 42485 RVA: 0x004554D8 File Offset: 0x004536D8
		private void TickBreasts(float deltaTime, int step, float strike, bool teleported)
		{
			AnimeBodyPhysicsConfigSO.BreastSettings breasts = this._config.breasts;
			AnimeBodyPhysics.CaptureBreast(this._leftBreast);
			AnimeBodyPhysics.CaptureBreast(this._rightBreast);
			if (teleported || !this._breastsDriven)
			{
				this._leftBreast.Mass.Reset();
				this._rightBreast.Mass.Reset();
				this._leftBreast.Hold = (this._rightBreast.Hold = 0f);
			}
			float num = this.BlendIntensity(ref this._breastIntensity, (this._wardrobe != null) ? this._wardrobe.CurrentBreastWeight : 1f);
			float num2 = this.Firmness(num);
			float num3 = 1f + this._config.clothesFirmness * num2;
			float num4 = this.Locomotion();
			float num5 = this.SettleWeight();
			float num6 = this.Gait();
			float num7 = Mathf.Lerp(Mathf.Lerp(breasts.restFrequency, breasts.settleFrequency, num5), Mathf.Lerp(breasts.walkFrequency, breasts.frequency, num6), num4);
			float num8 = num7 * breasts.asymmetry * 0.5f;
			this._leftBreast.Mass.Frequency = Mathf.Min(12f, (num7 - num8) * num3);
			this._rightBreast.Mass.Frequency = Mathf.Min(12f, (num7 + num8) * num3);
			this._leftBreast.Hold = AnimeBodyPhysics.BlendHold(this._leftBreast.Hold, this.BreastHold(this._leftBreast), deltaTime);
			this._rightBreast.Hold = AnimeBodyPhysics.BlendHold(this._rightBreast.Hold, this.BreastHold(this._rightBreast), deltaTime);
			if (step >= 0 && strike > 0f)
			{
				Vector3 vector = -this._root.up;
				float num9 = Mathf.Lerp(breasts.walkKick, breasts.kick, num6);
				((step == 0) ? this._leftBreast : this._rightBreast).Mass.Kick(vector, num9 * strike * this.Variation(breasts.kickRandom));
				((step == 0) ? this._rightBreast : this._leftBreast).Mass.Kick(vector, -num9 * breasts.counterKick * strike * this.Variation(breasts.kickRandom));
			}
			float num10 = Mathf.Lerp(Mathf.Lerp(breasts.restDamping, breasts.settleDamping, num5), Mathf.Lerp(breasts.walkDamping, breasts.damping, num6), num4) + this._config.clothesDamping * num2;
			float num11 = Mathf.Lerp(this._config.handHoldDamping, this._config.handHoldMovingDamping, num4);
			this.FollowBreast(this._leftBreast, deltaTime, num10 + this._leftBreast.Hold * num11, breasts);
			this.FollowBreast(this._rightBreast, deltaTime, num10 + this._rightBreast.Hold * num11, breasts);
			Vector3 offset = this._leftBreast.Mass.Offset;
			Vector3 offset2 = this._rightBreast.Mass.Offset;
			float num12 = Mathf.Lerp(breasts.settleUnison, Mathf.Lerp(breasts.walkUnison, breasts.unison, num6), num4);
			Vector3 vector2 = (offset + offset2) * 0.5f * (1f - num12);
			float num13 = breasts.gain * this._config.BreastScaleFor(this.BoobsType()) * num * num3;
			float num14 = Mathf.Lerp(this._config.handHoldStrength, this._config.handHoldMovingStrength, num4);
			float num15 = breasts.settleForward * (1f - num4);
			this.SwingBreast(this._leftBreast, (offset - vector2) * num13 * (1f - this._leftBreast.Hold * num14), num15, breasts);
			this.SwingBreast(this._rightBreast, (offset2 - vector2) * num13 * (1f - this._rightBreast.Hold * num14), num15, breasts);
			this._breastsDriven = true;
		}

		// Token: 0x0600A5F6 RID: 42486 RVA: 0x004558EA File Offset: 0x00453AEA
		private float BreastHold(AnimeBodyPhysics.Breast breast)
		{
			return this.HandHold((breast.Base != null) ? breast.Base.position : breast.Spring.position, breast.Spring.position);
		}

		// Token: 0x0600A5F7 RID: 42487 RVA: 0x00455924 File Offset: 0x00453B24
		private static void CaptureBreast(AnimeBodyPhysics.Breast breast)
		{
			breast.SpringPosition = breast.Spring.localPosition;
			breast.SpringRotation = breast.Spring.localRotation;
			if (breast.Base == null)
			{
				return;
			}
			breast.BasePosition = breast.Base.localPosition;
			breast.BaseRotation = breast.Base.localRotation;
			breast.Arm = Mathf.Max(0.03f, Vector3.Distance(breast.Spring.position, breast.Base.position));
		}

		// Token: 0x0600A5F8 RID: 42488 RVA: 0x004559B0 File Offset: 0x00453BB0
		private void FollowBreast(AnimeBodyPhysics.Breast breast, float deltaTime, float damping, AnimeBodyPhysicsConfigSO.BreastSettings settings)
		{
			Transform parent = breast.Spring.parent;
			Vector3 vector = ((parent != null) ? parent.TransformPoint(breast.SpringPosition) : breast.SpringPosition);
			this.Follow(breast.Mass, vector, deltaTime, damping, settings.maxSwing);
		}

		// Token: 0x0600A5F9 RID: 42489 RVA: 0x00455A00 File Offset: 0x00453C00
		private void SwingBreast(AnimeBodyPhysics.Breast breast, Vector3 swing, float forward, AnimeBodyPhysicsConfigSO.BreastSettings settings)
		{
			float num = Vector3.Dot(swing, this._root.up) + Vector3.Dot(swing, this._root.forward) * forward;
			float num2 = Vector3.Dot(swing, this._root.right) * settings.sway;
			float maxAngle = settings.maxAngle;
			float num3 = Mathf.Clamp(num / breast.Arm * 57.29578f, -maxAngle, maxAngle);
			float num4 = Mathf.Clamp(num2 / breast.Arm * 57.29578f, -maxAngle * 0.5f, maxAngle * 0.5f);
			Transform parent = breast.Spring.parent;
			Vector3 vector = ((parent != null) ? parent.InverseTransformDirection(this._root.right) : Vector3.right);
			Vector3 vector2 = ((parent != null) ? parent.InverseTransformDirection(this._root.forward) : Vector3.forward);
			Quaternion quaternion = Quaternion.AngleAxis(-num3, vector) * Quaternion.AngleAxis(num4, vector2);
			Vector3 vector3 = ((breast.Base != null) ? breast.BasePosition : breast.SpringPosition);
			breast.TargetPosition = vector3 + quaternion * (breast.SpringPosition - vector3);
			breast.TargetRotation = quaternion * breast.SpringRotation;
			breast.TargetBaseRotation = ((breast.Base != null) ? (Quaternion.Slerp(Quaternion.identity, quaternion, settings.baseFollow) * breast.BaseRotation) : Quaternion.identity);
		}

		// Token: 0x0600A5FA RID: 42490 RVA: 0x00455B80 File Offset: 0x00453D80
		private int BoobsType()
		{
			if (!this._hasBoobsType || this._animator == null)
			{
				return 3;
			}
			if (this._boobsTypeType != AnimatorControllerParameterType.Int)
			{
				return Mathf.RoundToInt(this._animator.GetFloat(this._boobsTypeHash));
			}
			return this._animator.GetInteger(this._boobsTypeHash);
		}

		// Token: 0x0600A5FB RID: 42491 RVA: 0x00455BD8 File Offset: 0x00453DD8
		private bool Shake(Vector3 localDirection, bool seeSaw, float strength01)
		{
			if (!this._breastsDriven || localDirection.sqrMagnitude < 1E-06f)
			{
				return false;
			}
			float num = this._config.breasts.kick * 2.5f * Mathf.Clamp01(strength01);
			Vector3 vector = localDirection;
			Vector3 vector2 = localDirection;
			if (seeSaw)
			{
				vector.y = Mathf.Abs(localDirection.y);
				vector2.y = -Mathf.Abs(localDirection.y);
			}
			this._leftBreast.Mass.Kick(this._root.TransformDirection(vector).normalized, num);
			this._rightBreast.Mass.Kick(this._root.TransformDirection(vector2).normalized, num);
			return true;
		}

		// Token: 0x0600A5FC RID: 42492 RVA: 0x00455C92 File Offset: 0x00453E92
		private void StopBreasts()
		{
			if (this._breastsWritten)
			{
				AnimeBodyPhysics.RestoreBreast(this._leftBreast);
				AnimeBodyPhysics.RestoreBreast(this._rightBreast);
				this._breastsWritten = false;
			}
			this._breastsDriven = false;
			this._breastIntensity = -1f;
		}

		// Token: 0x0600A5FD RID: 42493 RVA: 0x00455CCC File Offset: 0x00453ECC
		private static void RestoreBreast(AnimeBodyPhysics.Breast breast)
		{
			if (breast.Spring != null)
			{
				breast.Spring.SetLocalPositionAndRotation(breast.SpringPosition, breast.SpringRotation);
			}
			if (breast.Base != null)
			{
				breast.Base.SetLocalPositionAndRotation(breast.BasePosition, breast.BaseRotation);
			}
		}

		// Token: 0x0600A5FE RID: 42494 RVA: 0x00455D24 File Offset: 0x00453F24
		private void TickButt(float deltaTime, int step, float strike, bool teleported)
		{
			AnimeBodyPhysicsConfigSO.ButtSettings butt = this._config.butt;
			AnimeBodyPhysics.CaptureCheek(this._leftCheek);
			AnimeBodyPhysics.CaptureCheek(this._rightCheek);
			if (teleported || !this._buttDriven)
			{
				this._leftCheek.Mass.Reset();
				this._rightCheek.Mass.Reset();
				this._leftCheek.Hold = (this._rightCheek.Hold = 0f);
			}
			float num = this.BlendIntensity(ref this._buttIntensity, (this._wardrobe != null) ? this._wardrobe.CurrentButtWeight : 1f);
			float num2 = this.Firmness(num);
			float num3 = 1f + this._config.clothesFirmness * num2;
			float num4 = this.Locomotion();
			float num5 = this.SettleWeight();
			float num6 = Mathf.Lerp(Mathf.Lerp(butt.restFrequency, butt.settleFrequency, num5), butt.frequency, num4);
			float num7 = num6 * butt.asymmetry * 0.5f;
			this._leftCheek.Mass.Frequency = Mathf.Min(12f, (num6 - num7) * num3);
			this._rightCheek.Mass.Frequency = Mathf.Min(12f, (num6 + num7) * num3);
			this._leftCheek.Hold = AnimeBodyPhysics.BlendHold(this._leftCheek.Hold, this.HandHold(this._leftCheek.Bone.position, this._leftCheek.Bone.position), deltaTime);
			this._rightCheek.Hold = AnimeBodyPhysics.BlendHold(this._rightCheek.Hold, this.HandHold(this._rightCheek.Bone.position, this._rightCheek.Bone.position), deltaTime);
			if (step >= 0 && strike > 0f)
			{
				Vector3 normalized = (-this._root.up - this._root.forward * butt.kickBack).normalized;
				((step == 0) ? this._leftCheek : this._rightCheek).Mass.Kick(normalized, butt.kick * strike * this.Variation(butt.kickRandom));
				((step == 0) ? this._rightCheek : this._leftCheek).Mass.Kick(normalized, -butt.kick * butt.counterKick * strike * this.Variation(butt.kickRandom));
			}
			float num8 = Mathf.Lerp(Mathf.Lerp(butt.restDamping, butt.settleDamping, num5), butt.damping, num4) + this._config.clothesDamping * num2;
			this.FollowCheek(this._leftCheek, deltaTime, num8, butt);
			this.FollowCheek(this._rightCheek, deltaTime, num8, butt);
			Vector3 offset = this._leftCheek.Mass.Offset;
			Vector3 offset2 = this._rightCheek.Mass.Offset;
			float num9 = Mathf.Lerp(butt.settleUnison, butt.unison, num4);
			Vector3 vector = (offset + offset2) * 0.5f * (1f - num9);
			float num10 = butt.gain * num * num3;
			float handHoldStrength = this._config.handHoldStrength;
			this.JiggleCheek(this._leftCheek, (offset - vector) * num10 * (1f - this._leftCheek.Hold * handHoldStrength), butt);
			this.JiggleCheek(this._rightCheek, (offset2 - vector) * num10 * (1f - this._rightCheek.Hold * handHoldStrength), butt);
			this._buttDriven = true;
		}

		// Token: 0x0600A5FF RID: 42495 RVA: 0x004560DC File Offset: 0x004542DC
		private static void CaptureCheek(AnimeBodyPhysics.Cheek cheek)
		{
			cheek.Position = cheek.Bone.localPosition;
			cheek.Rotation = cheek.Bone.localRotation;
			cheek.Scale = cheek.Bone.localScale;
		}

		// Token: 0x0600A600 RID: 42496 RVA: 0x00456114 File Offset: 0x00454314
		private void FollowCheek(AnimeBodyPhysics.Cheek cheek, float deltaTime, float damping, AnimeBodyPhysicsConfigSO.ButtSettings settings)
		{
			Transform parent = cheek.Bone.parent;
			Vector3 vector = ((parent != null) ? parent.TransformPoint(cheek.Position) : cheek.Position);
			this.Follow(cheek.Mass, vector, deltaTime, damping + cheek.Hold * this._config.handHoldDamping, settings.maxSwing);
		}

		// Token: 0x0600A601 RID: 42497 RVA: 0x00456174 File Offset: 0x00454374
		private void JiggleCheek(AnimeBodyPhysics.Cheek cheek, Vector3 jiggle, AnimeBodyPhysicsConfigSO.ButtSettings settings)
		{
			Vector3 up = this._root.up;
			float num = Vector3.Dot(jiggle, up);
			Vector3 vector = (jiggle - up * num) * settings.sway;
			Vector3 vector2 = Vector3.ClampMagnitude(up * num + vector, settings.maxOffset);
			Transform parent = cheek.Bone.parent;
			cheek.Target = cheek.Position + ((parent != null) ? parent.InverseTransformVector(vector2) : vector2);
			float num2 = Mathf.Clamp(-Vector3.Dot(vector2, up) * settings.squash, -settings.maxSquash, settings.maxSquash);
			if (Mathf.Abs(num2) < 0.0001f)
			{
				cheek.TargetScale = cheek.Scale;
				return;
			}
			float num3 = 1f + num2;
			float num4 = 1f / Mathf.Sqrt(num3);
			Vector3 vector3 = ((cheek.Bone.parent != null) ? (Quaternion.Inverse(cheek.Bone.parent.rotation * cheek.Rotation) * up) : (Quaternion.Inverse(cheek.Rotation) * up));
			cheek.TargetScale = Vector3.Scale(cheek.Scale, new Vector3(Mathf.Lerp(num4, num3, vector3.x * vector3.x), Mathf.Lerp(num4, num3, vector3.y * vector3.y), Mathf.Lerp(num4, num3, vector3.z * vector3.z)));
		}

		// Token: 0x0600A602 RID: 42498 RVA: 0x004562FD File Offset: 0x004544FD
		private void StopButt()
		{
			if (this._buttWritten)
			{
				AnimeBodyPhysics.RestoreCheek(this._leftCheek);
				AnimeBodyPhysics.RestoreCheek(this._rightCheek);
				this._buttWritten = false;
			}
			this._buttDriven = false;
			this._buttIntensity = -1f;
		}

		// Token: 0x0600A603 RID: 42499 RVA: 0x00456336 File Offset: 0x00454536
		private static void RestoreCheek(AnimeBodyPhysics.Cheek cheek)
		{
			if (cheek.Bone == null)
			{
				return;
			}
			cheek.Bone.SetLocalPositionAndRotation(cheek.Position, cheek.Rotation);
			cheek.Bone.localScale = cheek.Scale;
		}

		// Token: 0x0600A604 RID: 42500 RVA: 0x00456370 File Offset: 0x00454570
		private void WriteBones()
		{
			if (this == null || !base.isActiveAndEnabled)
			{
				return;
			}
			if (this._breastsDriven)
			{
				AnimeBodyPhysics.WriteBreast(this._leftBreast);
				AnimeBodyPhysics.WriteBreast(this._rightBreast);
				this._breastsWritten = true;
			}
			if (this._buttDriven)
			{
				AnimeBodyPhysics.WriteCheek(this._leftCheek);
				AnimeBodyPhysics.WriteCheek(this._rightCheek);
				this._buttWritten = true;
			}
		}

		// Token: 0x0600A605 RID: 42501 RVA: 0x004563DC File Offset: 0x004545DC
		private void RestoreBones()
		{
			if (this == null)
			{
				return;
			}
			if (this._breastsWritten)
			{
				if (AnimeBodyPhysics.StillWritten(this._leftBreast))
				{
					AnimeBodyPhysics.RestoreBreast(this._leftBreast);
				}
				if (AnimeBodyPhysics.StillWritten(this._rightBreast))
				{
					AnimeBodyPhysics.RestoreBreast(this._rightBreast);
				}
				this._breastsWritten = false;
			}
			if (this._buttWritten)
			{
				if (AnimeBodyPhysics.StillWritten(this._leftCheek))
				{
					AnimeBodyPhysics.RestoreCheek(this._leftCheek);
				}
				if (AnimeBodyPhysics.StillWritten(this._rightCheek))
				{
					AnimeBodyPhysics.RestoreCheek(this._rightCheek);
				}
				this._buttWritten = false;
			}
		}

		// Token: 0x0600A606 RID: 42502 RVA: 0x00456474 File Offset: 0x00454674
		private static bool StillWritten(AnimeBodyPhysics.Breast breast)
		{
			return breast.Spring != null && breast.Spring.localPosition == breast.TargetPosition && breast.Spring.localRotation == breast.TargetRotation && (breast.Base == null || breast.Base.localRotation == breast.TargetBaseRotation);
		}

		// Token: 0x0600A607 RID: 42503 RVA: 0x004564E7 File Offset: 0x004546E7
		private static bool StillWritten(AnimeBodyPhysics.Cheek cheek)
		{
			return cheek.Bone != null && cheek.Bone.localPosition == cheek.Target && cheek.Bone.localScale == cheek.TargetScale;
		}

		// Token: 0x0600A608 RID: 42504 RVA: 0x00456527 File Offset: 0x00454727
		private static void WriteBreast(AnimeBodyPhysics.Breast breast)
		{
			breast.Spring.SetLocalPositionAndRotation(breast.TargetPosition, breast.TargetRotation);
			if (breast.Base != null)
			{
				breast.Base.localRotation = breast.TargetBaseRotation;
			}
		}

		// Token: 0x0600A609 RID: 42505 RVA: 0x0045655F File Offset: 0x0045475F
		private static void WriteCheek(AnimeBodyPhysics.Cheek cheek)
		{
			cheek.Bone.SetLocalPositionAndRotation(cheek.Target, cheek.Rotation);
			cheek.Bone.localScale = cheek.TargetScale;
		}

		// Token: 0x0600A60A RID: 42506 RVA: 0x0045658C File Offset: 0x0045478C
		private static bool Matches(string value, string[] candidates)
		{
			for (int i = 0; i < candidates.Length; i++)
			{
				if (string.Equals(value, candidates[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A60B RID: 42507 RVA: 0x004565B8 File Offset: 0x004547B8
		private static Transform Find(Transform parent, string childName)
		{
			if (parent == null)
			{
				return null;
			}
			if (string.Equals(parent.name, childName, StringComparison.OrdinalIgnoreCase))
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = AnimeBodyPhysics.Find(parent.GetChild(i), childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0400AA55 RID: 43605
		private static readonly string[] BreastClothNames = new string[] { "BoobL", "BoobR" };

		// Token: 0x0400AA56 RID: 43606
		private static readonly string[] ButtClothNames = new string[] { "ButtL", "ButtR" };

		// Token: 0x0400AA57 RID: 43607
		private static readonly int IsRidingHash = Animator.StringToHash("IsRiding");

		// Token: 0x0400AA58 RID: 43608
		private const float TeleportDistance = 2f;

		// Token: 0x0400AA59 RID: 43609
		private const float SnapDistance = 0.3f;

		// Token: 0x0400AA5A RID: 43610
		private const float ClothRescanSeconds = 0.5f;

		// Token: 0x0400AA5B RID: 43611
		private const float HoldBlendSeconds = 0.12f;

		// Token: 0x0400AA5C RID: 43612
		private const float MaxFrequency = 12f;

		// Token: 0x0400AA5D RID: 43613
		private AnimeBodyPhysicsConfigSO _config;

		// Token: 0x0400AA5E RID: 43614
		private Animator _animator;

		// Token: 0x0400AA5F RID: 43615
		private Transform _root;

		// Token: 0x0400AA60 RID: 43616
		private Transform _hips;

		// Token: 0x0400AA61 RID: 43617
		private WardrobeBodyPhysicsController _wardrobe;

		// Token: 0x0400AA62 RID: 43618
		private BoobTouchModeController _boobTouch;

		// Token: 0x0400AA63 RID: 43619
		private vThirdPersonMotor _motor;

		// Token: 0x0400AA64 RID: 43620
		private vThirdPersonInput _input;

		// Token: 0x0400AA65 RID: 43621
		private readonly List<MagicaCloth> _breastCloths = new List<MagicaCloth>();

		// Token: 0x0400AA66 RID: 43622
		private readonly List<MagicaCloth> _buttCloths = new List<MagicaCloth>();

		// Token: 0x0400AA67 RID: 43623
		private readonly AnimeBodyPhysics.Breast _leftBreast = new AnimeBodyPhysics.Breast();

		// Token: 0x0400AA68 RID: 43624
		private readonly AnimeBodyPhysics.Breast _rightBreast = new AnimeBodyPhysics.Breast();

		// Token: 0x0400AA69 RID: 43625
		private readonly AnimeBodyPhysics.Cheek _leftCheek = new AnimeBodyPhysics.Cheek();

		// Token: 0x0400AA6A RID: 43626
		private readonly AnimeBodyPhysics.Cheek _rightCheek = new AnimeBodyPhysics.Cheek();

		// Token: 0x0400AA6B RID: 43627
		private readonly AnimeBodyPhysics.Foot _leftFoot = new AnimeBodyPhysics.Foot();

		// Token: 0x0400AA6C RID: 43628
		private readonly AnimeBodyPhysics.Foot _rightFoot = new AnimeBodyPhysics.Foot();

		// Token: 0x0400AA6D RID: 43629
		private readonly AnimeBodyPhysics.Hand _leftHand = new AnimeBodyPhysics.Hand();

		// Token: 0x0400AA6E RID: 43630
		private readonly AnimeBodyPhysics.Hand _rightHand = new AnimeBodyPhysics.Hand();

		// Token: 0x0400AA6F RID: 43631
		private readonly Random _random = new Random();

		// Token: 0x0400AA70 RID: 43632
		private Action _restore;

		// Token: 0x0400AA71 RID: 43633
		private Action _write;

		// Token: 0x0400AA72 RID: 43634
		private bool _bound;

		// Token: 0x0400AA73 RID: 43635
		private bool _breastsDriven;

		// Token: 0x0400AA74 RID: 43636
		private bool _buttDriven;

		// Token: 0x0400AA75 RID: 43637
		private bool _breastsWritten;

		// Token: 0x0400AA76 RID: 43638
		private bool _buttWritten;

		// Token: 0x0400AA77 RID: 43639
		private float _breastIntensity = -1f;

		// Token: 0x0400AA78 RID: 43640
		private float _buttIntensity = -1f;

		// Token: 0x0400AA79 RID: 43641
		private float _speed;

		// Token: 0x0400AA7A RID: 43642
		private float _motion;

		// Token: 0x0400AA7B RID: 43643
		private float _stillTime;

		// Token: 0x0400AA7C RID: 43644
		private float _nextClothScan;

		// Token: 0x0400AA7D RID: 43645
		private float _unscaledSinceTick;

		// Token: 0x0400AA7E RID: 43646
		private int _fixedSteps;

		// Token: 0x0400AA7F RID: 43647
		private Vector3 _lastRootPosition;

		// Token: 0x0400AA80 RID: 43648
		private Quaternion _lastRootRotation = Quaternion.identity;

		// Token: 0x0400AA81 RID: 43649
		private bool _hasLastRoot;

		// Token: 0x0400AA82 RID: 43650
		private Transform _carrier;

		// Token: 0x0400AA83 RID: 43651
		private Vector3 _lastCarrierPosition;

		// Token: 0x0400AA84 RID: 43652
		private Quaternion _lastCarrierRotation = Quaternion.identity;

		// Token: 0x0400AA85 RID: 43653
		private int _boobsTypeHash;

		// Token: 0x0400AA86 RID: 43654
		private AnimatorControllerParameterType _boobsTypeType;

		// Token: 0x0400AA87 RID: 43655
		private bool _hasBoobsType;

		// Token: 0x0400AA88 RID: 43656
		private bool _hasIsRiding;

		// Token: 0x0400AA89 RID: 43657
		private int _expectedBreastClothCount;

		// Token: 0x0400AA8A RID: 43658
		private int _expectedButtClothCount;

		// Token: 0x0400AA8B RID: 43659
		private float _nextBindAttempt;

		// Token: 0x020014F8 RID: 5368
		private sealed class Breast
		{
			// Token: 0x0400AA8C RID: 43660
			public Transform Spring;

			// Token: 0x0400AA8D RID: 43661
			public Transform Base;

			// Token: 0x0400AA8E RID: 43662
			public Vector3 SpringPosition;

			// Token: 0x0400AA8F RID: 43663
			public Vector3 BasePosition;

			// Token: 0x0400AA90 RID: 43664
			public Quaternion SpringRotation = Quaternion.identity;

			// Token: 0x0400AA91 RID: 43665
			public Quaternion BaseRotation = Quaternion.identity;

			// Token: 0x0400AA92 RID: 43666
			public Vector3 TargetPosition;

			// Token: 0x0400AA93 RID: 43667
			public Quaternion TargetRotation = Quaternion.identity;

			// Token: 0x0400AA94 RID: 43668
			public Quaternion TargetBaseRotation = Quaternion.identity;

			// Token: 0x0400AA95 RID: 43669
			public float Arm = 0.1f;

			// Token: 0x0400AA96 RID: 43670
			public float Hold;

			// Token: 0x0400AA97 RID: 43671
			public readonly JiggleMass Mass = new JiggleMass();
		}

		// Token: 0x020014F9 RID: 5369
		private sealed class Cheek
		{
			// Token: 0x0400AA98 RID: 43672
			public Transform Bone;

			// Token: 0x0400AA99 RID: 43673
			public Vector3 Position;

			// Token: 0x0400AA9A RID: 43674
			public Vector3 Scale = Vector3.one;

			// Token: 0x0400AA9B RID: 43675
			public Vector3 Target;

			// Token: 0x0400AA9C RID: 43676
			public Vector3 TargetScale = Vector3.one;

			// Token: 0x0400AA9D RID: 43677
			public Quaternion Rotation = Quaternion.identity;

			// Token: 0x0400AA9E RID: 43678
			public float Hold;

			// Token: 0x0400AA9F RID: 43679
			public readonly JiggleMass Mass = new JiggleMass();
		}

		// Token: 0x020014FA RID: 5370
		private sealed class Foot
		{
			// Token: 0x0400AAA0 RID: 43680
			public Transform Bone;

			// Token: 0x0400AAA1 RID: 43681
			public float Ground;

			// Token: 0x0400AAA2 RID: 43682
			public float Peak;

			// Token: 0x0400AAA3 RID: 43683
			public float Height;

			// Token: 0x0400AAA4 RID: 43684
			public float FallSpeed;

			// Token: 0x0400AAA5 RID: 43685
			public bool Tracked;

			// Token: 0x0400AAA6 RID: 43686
			public bool Lifted;
		}

		// Token: 0x020014FB RID: 5371
		private sealed class Hand
		{
			// Token: 0x0400AAA7 RID: 43687
			public Transform Wrist;

			// Token: 0x0400AAA8 RID: 43688
			public Transform Knuckle;
		}
	}
}
