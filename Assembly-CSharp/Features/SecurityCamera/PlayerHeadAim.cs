using System;
using UnityEngine;

namespace Features.SecurityCamera
{
	// Token: 0x02000C5D RID: 3165
	[DisallowMultipleComponent]
	public class PlayerHeadAim : MonoBehaviour
	{
		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06005B8E RID: 23438 RVA: 0x0023E6D1 File Offset: 0x0023C8D1
		public bool IsActive
		{
			get
			{
				return this._active && this._target != null;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005B8F RID: 23439 RVA: 0x0023E6E9 File Offset: 0x0023C8E9
		public float CurrentWeight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x0023E6F1 File Offset: 0x0023C8F1
		private void Awake()
		{
			this.ResolveBones();
			this._bodyRoot = base.transform;
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x0023E708 File Offset: 0x0023C908
		private void ResolveBones()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			if (this.headBone == null && this.animator != null && this.animator.isHuman)
			{
				this.headBone = this.animator.GetBoneTransform(HumanBodyBones.Head);
			}
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x0023E76C File Offset: 0x0023C96C
		public void SetTarget(Transform target)
		{
			this._target = target;
			this._active = target != null;
			if (this.debugLogs)
			{
				Debug.Log("[PlayerHeadAim] SetTarget " + ((target != null) ? target.name : "null"));
			}
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x0023E7BA File Offset: 0x0023C9BA
		public void ClearTarget()
		{
			this._active = false;
			if (this.debugLogs)
			{
				Debug.Log("[PlayerHeadAim] ClearTarget");
			}
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x0023E7D8 File Offset: 0x0023C9D8
		public bool IsTargetInFront(Transform target)
		{
			if (target == null)
			{
				return false;
			}
			Vector3 vector = target.position - base.transform.position;
			vector.y = 0f;
			return vector.sqrMagnitude < 0.0001f || Vector3.Angle(base.transform.forward, vector) <= this.maxAngle;
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x0023E840 File Offset: 0x0023CA40
		private void LateUpdate()
		{
			if (this.headBone == null)
			{
				this.ResolveBones();
				if (this.headBone == null)
				{
					return;
				}
			}
			float num = 0f;
			if (this._active && this._target != null)
			{
				Vector3 vector = this._target.position - this.headBone.position;
				vector.y = 0f;
				float num2 = ((vector.sqrMagnitude < 0.0001f) ? 0f : Vector3.Angle(this._bodyRoot.forward, vector));
				float num3 = Mathf.InverseLerp(this.maxAngle, this.maxAngle - 20f, num2);
				num = this.maxWeight * Mathf.Clamp01(num3);
			}
			float num4 = 1f - Mathf.Exp(-this.weightLerpSpeed * Time.deltaTime);
			this._weight = Mathf.Lerp(this._weight, num, num4);
			Quaternion rotation = this.headBone.rotation;
			if (this._weight <= 0.001f)
			{
				this._smoothedLookRot = rotation;
				this._hasSmoothedLookRot = true;
				this._lookRotVelocity = new Quaternion(0f, 0f, 0f, 0f);
				return;
			}
			Vector3 vector2 = this.headBone.TransformPoint(this.headForwardOffset);
			Vector3 vector3 = ((this._target != null) ? this._target.position : (vector2 + this.headBone.forward)) - vector2;
			if (vector3.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector3.normalized, Vector3.up);
			if (!this._hasSmoothedLookRot)
			{
				this._smoothedLookRot = quaternion;
				this._hasSmoothedLookRot = true;
			}
			this._smoothedLookRot = PlayerHeadAim.SmoothDampQuaternion(this._smoothedLookRot, quaternion, ref this._lookRotVelocity, this.rotationSmoothTime);
			this.headBone.rotation = Quaternion.Slerp(rotation, this._smoothedLookRot, this._weight);
		}

		// Token: 0x06005B96 RID: 23446 RVA: 0x0023EA3C File Offset: 0x0023CC3C
		private static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime)
		{
			if (smoothTime <= 0f)
			{
				return target;
			}
			if (Quaternion.Dot(current, target) < 0f)
			{
				target = new Quaternion(-target.x, -target.y, -target.z, -target.w);
			}
			float deltaTime = Time.deltaTime;
			return PlayerHeadAim.Normalize(new Quaternion(Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime, float.PositiveInfinity, deltaTime), Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime, float.PositiveInfinity, deltaTime), Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime, float.PositiveInfinity, deltaTime), Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime, float.PositiveInfinity, deltaTime)));
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x0023EB0C File Offset: 0x0023CD0C
		private static Quaternion Normalize(Quaternion q)
		{
			float num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
			if (num < 1E-06f)
			{
				return Quaternion.identity;
			}
			float num2 = 1f / num;
			return new Quaternion(q.x * num2, q.y * num2, q.z * num2, q.w * num2);
		}

		// Token: 0x04005CBB RID: 23739
		[Header("Bones")]
		[Tooltip("Bone della testa. Se null viene presa dall'Animator humanoid (HumanBodyBones.Head).")]
		[SerializeField]
		private Transform headBone;

		// Token: 0x04005CBC RID: 23740
		[Tooltip("Animator humanoid del personaggio. Se null viene cercato in children.")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04005CBD RID: 23741
		[Header("Aim")]
		[Tooltip("Limite massimo (gradi) tra il forward del corpo e la direzione verso il target. Oltre questo angolo il look si disattiva gradualmente (testa non si torce troppo).")]
		[Range(10f, 110f)]
		[SerializeField]
		private float maxAngle = 70f;

		// Token: 0x04005CBE RID: 23742
		[Tooltip("Peso massimo dell'override (0 = nessun effetto, 1 = testa completamente verso il target).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float maxWeight = 1f;

		// Token: 0x04005CBF RID: 23743
		[Tooltip("Velocita' di smoothing del peso (piu' alto = piu' rapido).")]
		[Range(1f, 20f)]
		[SerializeField]
		private float weightLerpSpeed = 8f;

		// Token: 0x04005CC0 RID: 23744
		[Tooltip("Tempo di smorzamento (secondi) dell'orientamento della testa verso il target. E' un damping critico (SmoothDamp): valori piu' alti = inseguimento piu' morbido/lento, valori piu' bassi = piu' reattivo. Tipico 0.08-0.2. Elimina la vibrazione perche' la rotazione viene smorzata nel tempo invece di ricalcolata di colpo ogni frame.")]
		[Range(0.02f, 0.5f)]
		[SerializeField]
		private float rotationSmoothTime = 0.12f;

		// Token: 0x04005CC1 RID: 23745
		[Tooltip("Offset locale del punto 'occhi' rispetto alla bone Head, usato come origine dello sguardo.")]
		[SerializeField]
		private Vector3 headForwardOffset = new Vector3(0f, 0.1f, 0f);

		// Token: 0x04005CC2 RID: 23746
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005CC3 RID: 23747
		private Transform _target;

		// Token: 0x04005CC4 RID: 23748
		private bool _active;

		// Token: 0x04005CC5 RID: 23749
		private float _weight;

		// Token: 0x04005CC6 RID: 23750
		private Transform _bodyRoot;

		// Token: 0x04005CC7 RID: 23751
		private Quaternion _smoothedLookRot;

		// Token: 0x04005CC8 RID: 23752
		private Quaternion _lookRotVelocity = new Quaternion(0f, 0f, 0f, 0f);

		// Token: 0x04005CC9 RID: 23753
		private bool _hasSmoothedLookRot;
	}
}
