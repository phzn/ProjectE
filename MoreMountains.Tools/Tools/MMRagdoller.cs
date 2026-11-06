using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000183 RID: 387
	[AddComponentMenu("More Mountains/Tools/Animation/MMRagdoller")]
	public class MMRagdoller : MonoBehaviour
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00029E54 File Offset: 0x00028054
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00029E60 File Offset: 0x00028060
		public bool Ragdolling
		{
			get
			{
				return this.CurrentState > MMRagdoller.RagdollStates.Animated;
			}
			set
			{
				if (value)
				{
					if (this.CurrentState == MMRagdoller.RagdollStates.Animated)
					{
						this.SetIsKinematic(false);
						this._animator.enabled = false;
						this.CurrentState = MMRagdoller.RagdollStates.Ragdolling;
						MMAnimatorExtensions.UpdateAnimatorBool(this._animator, this._getUpFromBackAnimationParameter, false, this._animatorParameters, true);
						MMAnimatorExtensions.UpdateAnimatorBool(this._animator, this._getUpFromBellyAnimationParameter, false, this._animatorParameters, true);
						return;
					}
				}
				else if (this.CurrentState == MMRagdoller.RagdollStates.Ragdolling)
				{
					this.SetIsKinematic(true);
					this._ragdollingEndTimestamp = Time.time;
					this._animator.enabled = true;
					this.CurrentState = (this.AllowBlending ? MMRagdoller.RagdollStates.Blending : MMRagdoller.RagdollStates.Animated);
					foreach (RagdollBodyPart ragdollBodyPart in this._bodyparts)
					{
						ragdollBodyPart.StoredRotation = ragdollBodyPart.BodyPartTransform.rotation;
						ragdollBodyPart.StoredPosition = ragdollBodyPart.BodyPartTransform.position;
					}
					this._ragdolledFeetPosition = 0.5f * (this._animator.GetBoneTransform(HumanBodyBones.LeftToes).position + this._animator.GetBoneTransform(HumanBodyBones.RightToes).position);
					this._ragdolledHeadPosition = this._animator.GetBoneTransform(HumanBodyBones.Head).position;
					this._ragdolledHipPosition = this._animator.GetBoneTransform(HumanBodyBones.Hips).position;
					if (this._animator.GetBoneTransform(HumanBodyBones.Hips).forward.y > 0f)
					{
						MMAnimatorExtensions.UpdateAnimatorBool(this._animator, this._getUpFromBackAnimationParameter, true, this._animatorParameters, true);
						return;
					}
					MMAnimatorExtensions.UpdateAnimatorBool(this._animator, this._getUpFromBellyAnimationParameter, true, this._animatorParameters, true);
				}
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002A020 File Offset: 0x00028220
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002A028 File Offset: 0x00028228
		protected virtual void Initialization()
		{
			this._rigidbodies = base.GetComponentsInChildren(typeof(Rigidbody));
			this._rigidbodiesTempList = new List<Component>();
			foreach (Component component in this._rigidbodies)
			{
				if (component.gameObject.MMGetComponentNoAlloc<MMRagdollerIgnore>() == null)
				{
					this._rigidbodiesTempList.Add(component);
				}
			}
			this._rigidbodies = null;
			this._rigidbodies = this._rigidbodiesTempList.ToArray();
			if (this.CurrentState == MMRagdoller.RagdollStates.Animated)
			{
				this.SetIsKinematic(true);
			}
			else
			{
				this.SetIsKinematic(false);
			}
			foreach (Component component2 in base.GetComponentsInChildren(typeof(Transform)))
			{
				if (component2.transform != base.transform)
				{
					RagdollBodyPart ragdollBodyPart = new RagdollBodyPart
					{
						BodyPartTransform = (component2 as Transform)
					};
					this._bodyparts.Add(ragdollBodyPart);
				}
			}
			this._animator = base.gameObject.GetComponent<Animator>();
			this.RegisterAnimatorParameters();
			this._initialized = true;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002A134 File Offset: 0x00028334
		protected virtual void RegisterAnimatorParameters()
		{
			this._animatorParameters = new HashSet<int>();
			this._getUpFromBackAnimationParameter = Animator.StringToHash("GetUpFromBack");
			this._getUpFromBellyAnimationParameter = Animator.StringToHash("GetUpFromBelly");
			if (this._animator == null)
			{
				return;
			}
			if (this._animator.MMHasParameterOfType("GetUpFromBack", AnimatorControllerParameterType.Bool))
			{
				this._animatorParameters.Add(this._getUpFromBackAnimationParameter);
			}
			if (this._animator.MMHasParameterOfType("GetUpFromBelly", AnimatorControllerParameterType.Bool))
			{
				this._animatorParameters.Add(this._getUpFromBellyAnimationParameter);
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0002A1C8 File Offset: 0x000283C8
		protected virtual void SetIsKinematic(bool isKinematic)
		{
			foreach (Component component in this._rigidbodies)
			{
				if (component.transform != base.transform)
				{
					(component as Rigidbody).detectCollisions = !isKinematic;
					(component as Rigidbody).isKinematic = isKinematic;
				}
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002A21C File Offset: 0x0002841C
		public virtual void ForceRigidbodiesToSleep()
		{
			foreach (Component component in this._rigidbodies)
			{
				if (component.transform != base.transform)
				{
					(component as Rigidbody).Sleep();
				}
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002A260 File Offset: 0x00028460
		protected virtual void LateUpdate()
		{
			if (this.CurrentState == MMRagdoller.RagdollStates.Animated && this.ForceSleep)
			{
				this.ForceRigidbodiesToSleep();
			}
			this.HandleBlending();
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002A280 File Offset: 0x00028480
		protected virtual void HandleBlending()
		{
			if (this.CurrentState == MMRagdoller.RagdollStates.Blending)
			{
				if (Time.time <= this._ragdollingEndTimestamp + this._mecanimToGetUpTransitionTime)
				{
					base.transform.position = this.GetRootPosition();
					Vector3 vector = this._ragdolledHeadPosition - this._ragdolledFeetPosition;
					vector.y = 0f;
					Vector3 vector2 = 0.5f * (this._animator.GetBoneTransform(HumanBodyBones.LeftFoot).position + this._animator.GetBoneTransform(HumanBodyBones.RightFoot).position);
					Vector3 vector3 = this._animator.GetBoneTransform(HumanBodyBones.Head).position - vector2;
					vector3.y = 0f;
					base.transform.rotation *= Quaternion.FromToRotation(vector3.normalized, vector.normalized);
				}
				float num = 1f - (Time.time - this._ragdollingEndTimestamp - this._mecanimToGetUpTransitionTime) / this.RagdollToMecanimBlendDuration;
				num = Mathf.Clamp01(num);
				foreach (RagdollBodyPart ragdollBodyPart in this._bodyparts)
				{
					if (ragdollBodyPart.BodyPartTransform != base.transform)
					{
						if (ragdollBodyPart.BodyPartTransform == this._animator.GetBoneTransform(HumanBodyBones.Hips))
						{
							ragdollBodyPart.BodyPartTransform.position = Vector3.Lerp(ragdollBodyPart.BodyPartTransform.position, ragdollBodyPart.StoredPosition, num);
						}
						ragdollBodyPart.BodyPartTransform.rotation = Quaternion.Slerp(ragdollBodyPart.BodyPartTransform.rotation, ragdollBodyPart.StoredRotation, num);
					}
				}
				if (num == 0f)
				{
					this.CurrentState = MMRagdoller.RagdollStates.Animated;
					return;
				}
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002A458 File Offset: 0x00028658
		public Vector3 GetPosition()
		{
			if (!this._initialized)
			{
				this.Initialization();
			}
			if (!(this._animator.GetBoneTransform(HumanBodyBones.Hips) == null))
			{
				return this._animator.GetBoneTransform(HumanBodyBones.Hips).position;
			}
			return this.MainRigidbody.position;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002A4A4 File Offset: 0x000286A4
		protected Vector3 GetRootPosition()
		{
			Vector3 vector = ((this._animator.GetBoneTransform(HumanBodyBones.Hips) == null) ? this.MainRigidbody.position : this._animator.GetBoneTransform(HumanBodyBones.Hips).position);
			Vector3 vector2 = this._ragdolledHipPosition - vector;
			Vector3 vector3 = base.transform.position + vector2;
			RaycastHit[] array = Physics.RaycastAll(new Ray(vector3, Vector3.down));
			vector3.y = 0f;
			foreach (RaycastHit raycastHit in array)
			{
				if (!raycastHit.transform.IsChildOf(base.transform))
				{
					vector3.y = Mathf.Max(vector3.y, raycastHit.point.y);
				}
			}
			return vector3;
		}

		// Token: 0x04000AA1 RID: 2721
		[Header("Ragdoll")]
		public MMRagdoller.RagdollStates CurrentState;

		// Token: 0x04000AA2 RID: 2722
		public float RagdollToMecanimBlendDuration = 0.5f;

		// Token: 0x04000AA3 RID: 2723
		[Header("Rigidbodies")]
		public Rigidbody MainRigidbody;

		// Token: 0x04000AA4 RID: 2724
		public bool ForceSleep = true;

		// Token: 0x04000AA5 RID: 2725
		public bool AllowBlending = true;

		// Token: 0x04000AA6 RID: 2726
		protected float _mecanimToGetUpTransitionTime = 0.05f;

		// Token: 0x04000AA7 RID: 2727
		protected float _ragdollingEndTimestamp = float.MinValue;

		// Token: 0x04000AA8 RID: 2728
		protected Vector3 _ragdolledHipPosition;

		// Token: 0x04000AA9 RID: 2729
		protected Vector3 _ragdolledHeadPosition;

		// Token: 0x04000AAA RID: 2730
		protected Vector3 _ragdolledFeetPosition;

		// Token: 0x04000AAB RID: 2731
		protected List<RagdollBodyPart> _bodyparts = new List<RagdollBodyPart>();

		// Token: 0x04000AAC RID: 2732
		protected Animator _animator;

		// Token: 0x04000AAD RID: 2733
		protected List<Component> _rigidbodiesTempList;

		// Token: 0x04000AAE RID: 2734
		protected Component[] _rigidbodies;

		// Token: 0x04000AAF RID: 2735
		protected HashSet<int> _animatorParameters;

		// Token: 0x04000AB0 RID: 2736
		protected const string _getUpFromBackAnimationParameterName = "GetUpFromBack";

		// Token: 0x04000AB1 RID: 2737
		protected int _getUpFromBackAnimationParameter;

		// Token: 0x04000AB2 RID: 2738
		protected const string _getUpFromBellyAnimationParameterName = "GetUpFromBelly";

		// Token: 0x04000AB3 RID: 2739
		protected int _getUpFromBellyAnimationParameter;

		// Token: 0x04000AB4 RID: 2740
		protected bool _initialized;

		// Token: 0x02000473 RID: 1139
		public enum RagdollStates
		{
			// Token: 0x040020CD RID: 8397
			Animated,
			// Token: 0x040020CE RID: 8398
			Ragdolling,
			// Token: 0x040020CF RID: 8399
			Blending
		}
	}
}
