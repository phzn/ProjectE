using System;
using System.Collections;
using System.Collections.Generic;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006B2 RID: 1714
	[vClassHeader("RAGDOLL SYSTEM", true, "ragdollIcon", true, "Every gameobject children of the character must have their tag added in the IgnoreTag List.")]
	public class vRagdoll : vMonoBehaviour
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x000EC113 File Offset: 0x000EA313
		// (set) Token: 0x06002A0B RID: 10763 RVA: 0x000EC11B File Offset: 0x000EA31B
		public bool ignoreGetUpAnimation
		{
			get
			{
				return this._ignoreGetUpAnimation;
			}
			set
			{
				this._ignoreGetUpAnimation = value;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002A0C RID: 10764 RVA: 0x000EC124 File Offset: 0x000EA324
		// (set) Token: 0x06002A0D RID: 10765 RVA: 0x000EC12C File Offset: 0x000EA32C
		protected Vector3 localY { get; set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002A0E RID: 10766 RVA: 0x000EC135 File Offset: 0x000EA335
		// (set) Token: 0x06002A0F RID: 10767 RVA: 0x000EC140 File Offset: 0x000EA340
		private bool ragdolled
		{
			get
			{
				return this.state > vRagdoll.RagdollState.animated;
			}
			set
			{
				if (value)
				{
					if (this.state == vRagdoll.RagdollState.animated)
					{
						this.setKinematic(false);
						this.setCollider(false);
						this.animator.enabled = false;
						this.state = vRagdoll.RagdollState.ragdolled;
						return;
					}
				}
				else
				{
					this.characterHips.SetParent(this.hipsParent);
					this.isActive = false;
					this.ragdollingEndTime = Time.time;
					if (this.state == vRagdoll.RagdollState.ragdolled)
					{
						this.setKinematic(true);
						this.setCollider(true);
						this.animator.enabled = true;
						this.state = vRagdoll.RagdollState.blendToAnim;
						foreach (vRagdoll.BodyPart bodyPart in this.bodyParts)
						{
							bodyPart.storedRotation = bodyPart.transform.rotation;
							bodyPart.storedPosition = bodyPart.transform.position;
						}
						this.ragdolledFeetPosition = 0.5f * (this.animator.GetBoneTransform(HumanBodyBones.LeftToes).position + this.animator.GetBoneTransform(HumanBodyBones.RightToes).position);
						this.ragdolledHeadPosition = this.animator.GetBoneTransform(HumanBodyBones.Head).position;
						this.ragdolledHipPosition = this.animator.GetBoneTransform(HumanBodyBones.Hips).position;
						float y = this.characterHips.TransformDirection(this.localY).y;
						if (!this.ignoreGetUpAnimation && this.animator.enabled && base.gameObject.activeInHierarchy)
						{
							if (y > 0f)
							{
								this.animator.Play("StandUp@FromBack");
								return;
							}
							this.animator.Play("StandUp@FromBelly");
							return;
						}
					}
					else if (this.state == vRagdoll.RagdollState.animated)
					{
						this.setKinematic(true);
						this.setCollider(true);
						this.animator.enabled = true;
					}
				}
			}
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000EC31C File Offset: 0x000EA51C
		protected virtual void Start()
		{
			this._parentRigb = base.GetComponent<Rigidbody>();
			this.iChar = base.GetComponent<vICharacter>();
			if (this.iChar != null)
			{
				this.iChar.onActiveRagdoll.AddListener(new UnityAction<vDamage>(this.ActivateRagdoll));
			}
			if (!this.collisionSource)
			{
				GameObject gameObject = new GameObject("ragdollAudioSource");
				gameObject.transform.SetParent(base.gameObject.transform);
				gameObject.transform.position = base.transform.position;
				this.collisionSource = gameObject.AddComponent<AudioSource>();
			}
			this.LoadBodyPart();
			if (this.startRagdolled)
			{
				base.Invoke("ActivateRagdoll", 0.1f);
			}
			this._ragdollContainer = vObjectContainer.root.gameObject;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000EC3E4 File Offset: 0x000EA5E4
		protected virtual void OnDisable()
		{
			this.inStabilize = false;
			base.Invoke("ChangeToHipsParent", 0.01f);
			this.setKinematic(true);
			this.setCollider(true);
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000EC40C File Offset: 0x000EA60C
		protected virtual void ChangeToHipsParent()
		{
			if (this.characterHips.parent != this.hipsParent)
			{
				this.characterHips.position = base.transform.position;
				this.characterHips.SetParent(this.hipsParent);
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000EC458 File Offset: 0x000EA658
		public virtual void RestoreRagdoll()
		{
			if (this.isActive)
			{
				this.ChangeToHipsParent();
				this.iChar.ResetRagdoll();
				this.isActive = false;
				this.state = vRagdoll.RagdollState.animated;
				this.inStabilize = false;
				this.setKinematic(true);
				this.setCollider(true);
				this.animator.enabled = true;
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000EC4B0 File Offset: 0x000EA6B0
		public void LoadBodyPart()
		{
			this.bodyParts.Clear();
			if (!this.animator)
			{
				this.animator = base.GetComponent<Animator>();
			}
			this.characterChest = this.animator.GetBoneTransform(HumanBodyBones.Chest);
			this.characterHips = this.animator.GetBoneTransform(HumanBodyBones.Hips);
			this.localY = this.characterHips.InverseTransformDirection(base.transform.forward);
			this.hipsParent = this.characterHips.parent;
			if (this.characterHips)
			{
				Component[] componentsInChildren = this.characterHips.GetComponentsInChildren(typeof(Transform));
				this.bodyParts.Add(new vRagdoll.BodyPart(this.characterHips));
				foreach (Component component in componentsInChildren)
				{
					if (!this.ignoreTags.Contains(component.tag) && component)
					{
						Transform transform = component as Transform;
						Rigidbody rigidbody;
						if (transform != base.transform && transform.TryGetComponent<Rigidbody>(out rigidbody))
						{
							vRagdoll.BodyPart bodyPart = new vRagdoll.BodyPart(transform);
							if (bodyPart.rigidbody != null)
							{
								bodyPart.rigidbody.isKinematic = true;
							}
							this.bodyParts.Add(bodyPart);
						}
					}
				}
				this.setKinematic(true);
				this.setCollider(true);
			}
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000EC5FC File Offset: 0x000EA7FC
		protected virtual void LateUpdate()
		{
			if (Time.timeScale <= 0f)
			{
				return;
			}
			if (this.animator == null)
			{
				return;
			}
			if (!this.updateBehaviour && this.animator.updateMode == AnimatorUpdateMode.AnimatePhysics)
			{
				return;
			}
			this.updateBehaviour = false;
			this.RagdollBehaviour();
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000EC64C File Offset: 0x000EA84C
		protected virtual void FixedUpdate()
		{
			this.updateBehaviour = true;
			if (!this.isActive)
			{
				return;
			}
			if (!this.EnsureRagdollContainer() || !this.characterHips)
			{
				this.RestoreRagdollSafely();
				return;
			}
			if (this.characterHips.parent != this._ragdollContainer.transform)
			{
				this.characterHips.SetParent(this._ragdollContainer.transform);
			}
			if (this.ragdolled && !this.inStabilize && !this.keepRagdolled && (this.iChar == null || !this.iChar.isDead))
			{
				this.ragdolled = false;
				base.StartCoroutine(this.ResetPlayer(1.1f));
				return;
			}
			if ((this.animator != null && !this.animator.isActiveAndEnabled && this.ragdolled) || (this.animator == null && this.ragdolled))
			{
				base.transform.position = this.characterHips.position;
			}
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000EC750 File Offset: 0x000EA950
		protected virtual void OnDestroy()
		{
			try
			{
				if (this._ragdollContainer && this.characterHips && this.characterHips.parent == this._ragdollContainer.transform)
				{
					Object.Destroy(this.characterHips.gameObject);
				}
			}
			catch (UnityException ex)
			{
				Debug.LogWarning(ex.Message, base.gameObject);
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000EC7C8 File Offset: 0x000EA9C8
		protected virtual void ResetCollisionSound()
		{
			this.inApplyCollisionSound = false;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000EC7D1 File Offset: 0x000EA9D1
		public virtual void ActivateRagdollWithDelayToGetUp()
		{
			this.ActivateRagdoll(null, this.debugTimeToStayRagdolled);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
		protected virtual void KeepRagdolled(float time)
		{
			CielRagdollRecovery component = base.GetComponent<CielRagdollRecovery>();
			if (component != null && component.isActiveAndEnabled && component.IsRecovering)
			{
				base.CancelInvoke("ResetStayRagdolled");
				this.keepRagdolled = true;
				return;
			}
			if (time > 0f)
			{
				this.keepRagdolled = true;
			}
			base.CancelInvoke("ResetStayRagdolled");
			base.Invoke("ResetStayRagdolled", time);
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000EC846 File Offset: 0x000EAA46
		public virtual void ResetStayRagdolled()
		{
			this.keepRagdolled = false;
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000EC84F File Offset: 0x000EAA4F
		public virtual void ActivateRagdoll()
		{
			this.ActivateRagdoll(null);
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000EC858 File Offset: 0x000EAA58
		public virtual void ActivateRagdoll(vDamage damage, float timeToStayRagdolled)
		{
			if (this.isActive || (damage != null && !damage.activeRagdoll) || this.state == vRagdoll.RagdollState.blendToAnim)
			{
				return;
			}
			this.ActivateRagdoll(damage);
			this.KeepRagdolled(timeToStayRagdolled);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000EC888 File Offset: 0x000EAA88
		public virtual void ActivateRagdoll(vDamage damage)
		{
			if (this.isActive || (damage != null && !damage.activeRagdoll) || this.state == vRagdoll.RagdollState.blendToAnim)
			{
				return;
			}
			if (!this.EnsureRagdollContainer() || !this.characterHips)
			{
				Debug.LogWarning("[vRagdoll] Cannot activate: ragdoll container or hips are unavailable.", this);
				return;
			}
			CielRagdollRecovery component = base.GetComponent<CielRagdollRecovery>();
			if (component != null && component.isActiveAndEnabled)
			{
				component.BeginCrashRecovery();
			}
			base.StopAllCoroutines();
			if (damage != null && damage.senselessTime > 0f)
			{
				this.KeepRagdolled(damage.senselessTime);
			}
			this.inApplyCollisionSound = true;
			this.isActive = true;
			if (base.transform.parent != null && !base.transform.parent.gameObject.isStatic)
			{
				base.transform.parent = null;
			}
			this.inStabilize = true;
			this.ragdolled = true;
			if (this.iChar != null)
			{
				this.iChar.EnableRagdoll();
			}
			base.StartCoroutine(this.RagdollStabilizer(2f));
			this.characterHips.SetParent(this._ragdollContainer.transform);
			base.Invoke("ResetCollisionSound", 0.2f);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000EC9B0 File Offset: 0x000EABB0
		private bool EnsureRagdollContainer()
		{
			if (this._ragdollContainer)
			{
				return true;
			}
			Transform root = vObjectContainer.root;
			if (!root)
			{
				return false;
			}
			this._ragdollContainer = root.gameObject;
			return this._ragdollContainer;
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000EC9F4 File Offset: 0x000EABF4
		private void RestoreRagdollSafely()
		{
			base.StopAllCoroutines();
			base.CancelInvoke("ResetStayRagdolled");
			this.keepRagdolled = false;
			this.inStabilize = false;
			this.isActive = false;
			this.state = vRagdoll.RagdollState.animated;
			if (this.iChar != null)
			{
				this.iChar.ResetRagdoll();
			}
			if (this.animator != null)
			{
				this.animator.enabled = true;
			}
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000ECA5C File Offset: 0x000EAC5C
		public virtual void OnRagdollCollisionEnter(vRagdollCollision ragdolCollision)
		{
			if (!this.inApplyCollisionSound && ragdolCollision.ImpactForce > 1f && this.collisionSource)
			{
				this.collisionSource.clip = this.collisionClip;
				this.collisionSource.volume = ragdolCollision.ImpactForce * 0.05f;
				if (!this.collisionSource.isPlaying)
				{
					this.inApplyCollisionSound = true;
					this.collisionSource.Play();
					base.Invoke("ResetCollisionSound", 0.2f);
				}
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000ECAE2 File Offset: 0x000EACE2
		protected virtual IEnumerator RagdollStabilizer(float delay)
		{
			float rdStabilize = float.PositiveInfinity;
			yield return new WaitForSeconds(delay);
			while (rdStabilize > ((this.iChar != null && this.iChar.isDead) ? 0.0001f : 0.1f) && this.animator != null && !this.animator.isActiveAndEnabled)
			{
				rdStabilize = this.characterChest.GetComponent<Rigidbody>().velocity.magnitude;
				yield return new WaitForEndOfFrame();
			}
			if (this.iChar != null && this.iChar.isDead)
			{
				yield return new WaitForEndOfFrame();
				this.DestroyComponents();
			}
			this.inStabilize = false;
			yield break;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000ECAF8 File Offset: 0x000EACF8
		protected virtual IEnumerator ResetPlayer(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			if (this.iChar != null)
			{
				this.iChar.ResetRagdoll();
			}
			yield break;
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000ECB10 File Offset: 0x000EAD10
		protected virtual void RagdollBehaviour()
		{
			if (this.iChar == null || this.iChar.currentHealth <= 0f)
			{
				return;
			}
			if (this.iChar == null || !this.iChar.ragdolled)
			{
				return;
			}
			if (this.state == vRagdoll.RagdollState.blendToAnim)
			{
				if (Time.time <= this.ragdollingEndTime + this.mecanimToGetUpTransitionTime)
				{
					Vector3 vector = this.ragdolledHipPosition - this.animator.GetBoneTransform(HumanBodyBones.Hips).position;
					Vector3 vector2 = base.transform.position + vector;
					foreach (RaycastHit raycastHit in Physics.RaycastAll(new Ray(vector2 + Vector3.up, Vector3.down), 1f, this.groundLayer, QueryTriggerInteraction.Ignore))
					{
						if (!raycastHit.transform.IsChildOf(base.transform))
						{
							vector2.y = Mathf.Max(vector2.y, raycastHit.point.y);
						}
					}
					base.transform.position = vector2;
					Vector3 vector3 = this.ragdolledHeadPosition - this.ragdolledFeetPosition;
					vector3.y = 0f;
					Vector3 vector4 = 0.5f * (this.animator.GetBoneTransform(HumanBodyBones.LeftFoot).position + this.animator.GetBoneTransform(HumanBodyBones.RightFoot).position);
					Vector3 vector5 = this.animator.GetBoneTransform(HumanBodyBones.Head).position - vector4;
					vector5.y = 0f;
					this.AlignGetUpHeading(vector5, vector3);
				}
				float num = 1f - (Time.time - this.ragdollingEndTime - this.mecanimToGetUpTransitionTime) / this.ragdollToMecanimBlendTime;
				num = Mathf.Clamp01(num);
				foreach (vRagdoll.BodyPart bodyPart in this.bodyParts)
				{
					if (bodyPart.transform != base.transform)
					{
						if (bodyPart.transform == this.animator.GetBoneTransform(HumanBodyBones.Hips))
						{
							bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, bodyPart.storedPosition, num);
						}
						bodyPart.transform.rotation = Quaternion.Slerp(bodyPart.transform.rotation, bodyPart.storedRotation, num);
					}
				}
				if (num == 0f)
				{
					this.state = vRagdoll.RagdollState.animated;
					return;
				}
			}
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000ECDAC File Offset: 0x000EAFAC
		protected void AlignGetUpHeading(Vector3 animatedDirection, Vector3 ragdolledDirection)
		{
			animatedDirection.y = 0f;
			ragdolledDirection.y = 0f;
			if (animatedDirection.sqrMagnitude < 0.0001f || ragdolledDirection.sqrMagnitude < 0.0001f)
			{
				return;
			}
			float num = Vector3.SignedAngle(animatedDirection, ragdolledDirection, Vector3.up);
			base.transform.rotation = Quaternion.AngleAxis(num, Vector3.up) * base.transform.rotation;
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000ECE24 File Offset: 0x000EB024
		protected virtual void setKinematic(bool newValue)
		{
			foreach (vRagdoll.BodyPart bodyPart in this.bodyParts)
			{
				if (bodyPart.transform != null && !this.ignoreTags.Contains(bodyPart.transform.tag) && bodyPart.rigidbody && bodyPart.rigidbody.isKinematic != newValue)
				{
					bodyPart.rigidbody.isKinematic = newValue;
					if (!newValue)
					{
						Vector3 vector = new Vector3(this._parentRigb.velocity.x * this.horizontalMultiplier, this._parentRigb.velocity.y * this.verticalMultiplier, this._parentRigb.velocity.z * this.horizontalMultiplier);
						bodyPart.rigidbody.velocity = vector;
					}
				}
			}
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000ECF24 File Offset: 0x000EB124
		protected virtual void setCollider(bool newValue)
		{
			foreach (vRagdoll.BodyPart bodyPart in this.bodyParts)
			{
				if (bodyPart.transform != null && !this.ignoreTags.Contains(bodyPart.transform.tag) && !bodyPart.transform.Equals(base.transform) && bodyPart.collider)
				{
					if (this.disableColliders)
					{
						bodyPart.collider.enabled = !newValue;
					}
					else
					{
						bodyPart.collider.isTrigger = newValue;
					}
				}
			}
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000ECFDC File Offset: 0x000EB1DC
		protected virtual void DestroyComponents()
		{
			if (this.removePhysicsAfterDie)
			{
				MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].transform != base.transform)
					{
						Object.Destroy(componentsInChildren[i]);
					}
				}
				CharacterJoint[] componentsInChildren2 = base.GetComponentsInChildren<CharacterJoint>();
				if (componentsInChildren2 != null)
				{
					foreach (CharacterJoint characterJoint in componentsInChildren2)
					{
						if (!this.ignoreTags.Contains(characterJoint.gameObject.tag) && characterJoint.transform != base.transform)
						{
							Object.Destroy(characterJoint);
						}
					}
				}
				Rigidbody[] componentsInChildren3 = base.GetComponentsInChildren<Rigidbody>();
				if (componentsInChildren3 != null)
				{
					foreach (Rigidbody rigidbody in componentsInChildren3)
					{
						if (!this.ignoreTags.Contains(rigidbody.gameObject.tag) && rigidbody.transform != base.transform)
						{
							Object.Destroy(rigidbody);
						}
					}
				}
				Collider[] componentsInChildren4 = base.GetComponentsInChildren<Collider>();
				if (componentsInChildren4 != null)
				{
					foreach (Collider collider in componentsInChildren4)
					{
						if (!this.ignoreTags.Contains(collider.gameObject.tag) && collider.transform != base.transform)
						{
							Object.Destroy(collider);
						}
					}
				}
			}
		}

		// Token: 0x04002A19 RID: 10777
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool startRagdolled;

		// Token: 0x04002A1A RID: 10778
		[vButton("Active Ragdoll And Keep Ragdolled", "ActivateRagdollWithDelayToGetUp", typeof(vRagdoll), true)]
		[vButton("Active Ragdoll", "ActivateRagdoll", typeof(vRagdoll), true)]
		[SerializeField]
		protected float debugTimeToStayRagdolled;

		// Token: 0x04002A1B RID: 10779
		[vEditorToolbar("Settings", false, "", false, false)]
		public LayerMask groundLayer = 1;

		// Token: 0x04002A1C RID: 10780
		public bool keepRagdolled;

		// Token: 0x04002A1D RID: 10781
		public bool invertGetUpAnim;

		// Token: 0x04002A1E RID: 10782
		public bool _ignoreGetUpAnimation;

		// Token: 0x04002A1F RID: 10783
		public bool removePhysicsAfterDie;

		// Token: 0x04002A20 RID: 10784
		[Tooltip("SHOOTER: Keep false to use detection hit on each children collider, don't forget to change the layer to BodyPart from hips to all children. MELEE: Keep true to only hit the main Capsule Collider.")]
		public bool disableColliders;

		// Token: 0x04002A21 RID: 10785
		public AudioSource collisionSource;

		// Token: 0x04002A22 RID: 10786
		public AudioClip collisionClip;

		// Token: 0x04002A23 RID: 10787
		[Header("Add Tags for Weapons or Itens here:")]
		public List<string> ignoreTags = new List<string> { "Weapon", "Ignore Ragdoll" };

		// Token: 0x04002A24 RID: 10788
		public AnimatorStateInfo stateInfo;

		// Token: 0x04002A25 RID: 10789
		[Range(0f, 2f)]
		[Tooltip("The velocity of the parent rigidbody will be applied to the Ragdoll when enabled, creating a more realistic physics")]
		public float horizontalMultiplier = 1f;

		// Token: 0x04002A26 RID: 10790
		[Range(0f, 2f)]
		public float verticalMultiplier = 0.5f;

		// Token: 0x04002A27 RID: 10791
		internal vICharacter iChar;

		// Token: 0x04002A28 RID: 10792
		protected Animator animator;

		// Token: 0x04002A29 RID: 10793
		protected Rigidbody _parentRigb;

		// Token: 0x04002A2A RID: 10794
		internal Transform characterChest;

		// Token: 0x04002A2B RID: 10795
		internal Transform characterHips;

		// Token: 0x04002A2C RID: 10796
		[vEditorToolbar("Debug", false, "", false, false)]
		[vReadOnly(true)]
		public bool isActive;

		// Token: 0x04002A2D RID: 10797
		[vReadOnly(true)]
		public bool inStabilize;

		// Token: 0x04002A2E RID: 10798
		[vReadOnly(true)]
		public bool updateBehaviour;

		// Token: 0x04002A2F RID: 10799
		[vReadOnly(true)]
		public vRagdoll.RagdollState state;

		// Token: 0x04002A31 RID: 10801
		private readonly float ragdollToMecanimBlendTime = 0.25f;

		// Token: 0x04002A32 RID: 10802
		private readonly float mecanimToGetUpTransitionTime = 0.05f;

		// Token: 0x04002A33 RID: 10803
		private float ragdollingEndTime = -100f;

		// Token: 0x04002A34 RID: 10804
		private Vector3 ragdolledHipPosition;

		// Token: 0x04002A35 RID: 10805
		private Vector3 ragdolledHeadPosition;

		// Token: 0x04002A36 RID: 10806
		private Vector3 ragdolledFeetPosition;

		// Token: 0x04002A37 RID: 10807
		private readonly List<vRagdoll.BodyPart> bodyParts = new List<vRagdoll.BodyPart>();

		// Token: 0x04002A38 RID: 10808
		private Transform hipsParent;

		// Token: 0x04002A39 RID: 10809
		private bool inApplyCollisionSound;

		// Token: 0x04002A3A RID: 10810
		private GameObject _ragdollContainer;

		// Token: 0x020006B3 RID: 1715
		public enum RagdollState
		{
			// Token: 0x04002A3C RID: 10812
			animated,
			// Token: 0x04002A3D RID: 10813
			ragdolled,
			// Token: 0x04002A3E RID: 10814
			blendToAnim
		}

		// Token: 0x020006B4 RID: 1716
		private class BodyPart
		{
			// Token: 0x06002A2A RID: 10794 RVA: 0x000ED1C2 File Offset: 0x000EB3C2
			public BodyPart(Transform t)
			{
				this.transform = t;
				this.rigidbody = t.GetComponent<Rigidbody>();
				this.collider = t.GetComponent<Collider>();
			}

			// Token: 0x04002A3F RID: 10815
			public Transform transform;

			// Token: 0x04002A40 RID: 10816
			public Rigidbody rigidbody;

			// Token: 0x04002A41 RID: 10817
			public Collider collider;

			// Token: 0x04002A42 RID: 10818
			public Vector3 storedPosition;

			// Token: 0x04002A43 RID: 10819
			public Quaternion storedRotation;
		}
	}
}
