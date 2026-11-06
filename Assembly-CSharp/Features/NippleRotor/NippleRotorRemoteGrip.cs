using System;
using System.Collections.Generic;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F5F RID: 3935
	[DefaultExecutionOrder(21000)]
	[DisallowMultipleComponent]
	public sealed class NippleRotorRemoteGrip : MonoBehaviour
	{
		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x0600767A RID: 30330 RVA: 0x0030208C File Offset: 0x0030028C
		public Transform CordSocket
		{
			get
			{
				if (this._cordSocket == null)
				{
					this._cordSocket = NippleRotorRemoteGrip.FindDeep(base.transform, "CordSocket");
				}
				return this._cordSocket;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x0600767B RID: 30331 RVA: 0x003020B8 File Offset: 0x003002B8
		public bool IsAttached
		{
			get
			{
				return this._configured && !this._released && this._hand != null && base.transform.parent == this._hand;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x0600767C RID: 30332 RVA: 0x003020F0 File Offset: 0x003002F0
		public Transform Hand
		{
			get
			{
				return this._hand;
			}
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x0600767D RID: 30333 RVA: 0x003020F8 File Offset: 0x003002F8
		public float BodyScale
		{
			get
			{
				return this._bodyScale;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x0600767E RID: 30334 RVA: 0x00302100 File Offset: 0x00300300
		public float CurlWeight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x0600767F RID: 30335 RVA: 0x00302108 File Offset: 0x00300308
		// (set) Token: 0x06007680 RID: 30336 RVA: 0x00302110 File Offset: 0x00300310
		public bool CurlEnabled
		{
			get
			{
				return this._curlEnabled;
			}
			set
			{
				this._curlEnabled = value;
			}
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x0030211C File Offset: 0x0030031C
		public void Configure(Animator animator, NippleRotorGripSettings settings)
		{
			this.RestoreWrittenBones();
			this._fingers.Clear();
			this._animator = animator;
			this._settings = settings ?? new NippleRotorGripSettings();
			this._hand = (this._palmFront = (this._palmBack = null));
			this._configured = (this._released = false);
			this._weight = 0f;
			this._body = null;
			this._nextBodyResolveTime = float.NegativeInfinity;
			this._cordSocket = NippleRotorRemoteGrip.FindDeep(base.transform, "CordSocket");
			if (animator == null || !animator.isHuman)
			{
				Debug.LogWarning("[NippleRotorRemoteGrip] A humanoid Animator is required to hold the remote.", this);
				return;
			}
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftHand);
			if (boneTransform == null)
			{
				Debug.LogWarning("[NippleRotorRemoteGrip] The avatar has no LeftHand bone.", this);
				return;
			}
			this.BindFingers(boneTransform, NippleRotorRemoteGrip.Bones(animator, HumanBodyBones.LeftThumbProximal, HumanBodyBones.LeftThumbIntermediate, HumanBodyBones.LeftThumbDistal), NippleRotorRemoteGrip.Bones(animator, HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftIndexDistal), NippleRotorRemoteGrip.Bones(animator, HumanBodyBones.LeftMiddleProximal, HumanBodyBones.LeftMiddleIntermediate, HumanBodyBones.LeftMiddleDistal), NippleRotorRemoteGrip.Bones(animator, HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftRingDistal), NippleRotorRemoteGrip.Bones(animator, HumanBodyBones.LeftLittleProximal, HumanBodyBones.LeftLittleIntermediate, HumanBodyBones.LeftLittleDistal));
			base.transform.SetParent(boneTransform, false);
			FirstPersonVisibleProp.Mark(base.gameObject);
			this._configured = true;
			this.ApplyPlacement();
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x00302250 File Offset: 0x00300450
		public void ConfigureFingers(Transform hand, NippleRotorGripSettings settings, Transform[] thumb, Transform[] index, Transform[] middle, Transform[] ring, Transform[] little)
		{
			this.RestoreWrittenBones();
			this._fingers.Clear();
			this._settings = settings ?? new NippleRotorGripSettings();
			this._released = false;
			this._weight = 0f;
			this.BindFingers(hand, thumb, index, middle, ring, little);
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x003022A0 File Offset: 0x003004A0
		public void Release()
		{
			this.RestoreWrittenBones();
			this._weight = 0f;
			this._released = true;
		}

		// Token: 0x06007684 RID: 30340 RVA: 0x003022BA File Offset: 0x003004BA
		private void OnDisable()
		{
			this.RestoreWrittenBones();
			this._weight = 0f;
		}

		// Token: 0x06007685 RID: 30341 RVA: 0x003022CD File Offset: 0x003004CD
		private void OnDestroy()
		{
			this.RestoreWrittenBones();
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x003022D5 File Offset: 0x003004D5
		private void LateUpdate()
		{
			if (this._configured && !this._released)
			{
				this.ApplyPlacement();
			}
			this.Tick(Time.deltaTime);
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x003022F8 File Offset: 0x003004F8
		public void Tick(float deltaTime)
		{
			if (this._released || this._settings == null || this._hand == null || this._fingers.Count == 0)
			{
				return;
			}
			float num = (this._curlEnabled ? 1f : 0f);
			if (deltaTime > 0f && !float.IsInfinity(deltaTime))
			{
				float curlBlendSeconds = this._settings.curlBlendSeconds;
				this._weight = ((curlBlendSeconds > 0.0001f) ? Mathf.MoveTowards(this._weight, num, deltaTime / curlBlendSeconds) : num);
			}
			for (int i = 0; i < this._fingers.Count; i++)
			{
				this.CaptureAnimatedBase(this._fingers[i]);
			}
			Vector3 vector;
			if (this._weight <= 0f || !this.TryGetPalmNormal(out vector))
			{
				return;
			}
			float num2 = ((this._settings.curlSign < 0f) ? (-1f) : 1f);
			for (int j = 0; j < this._fingers.Count; j++)
			{
				NippleRotorRemoteGrip.FingerBone fingerBone = this._fingers[j];
				fingerBone.HasAxis = fingerBone.Bone != null && this.TryGetLocalAxis(fingerBone, vector, num2, out fingerBone.LocalAxis);
			}
			for (int k = 0; k < this._fingers.Count; k++)
			{
				NippleRotorRemoteGrip.FingerBone fingerBone2 = this._fingers[k];
				if (fingerBone2.HasAxis)
				{
					float num3 = this.DegreesFor(fingerBone2.Segment) * this._weight;
					fingerBone2.Bone.localRotation = fingerBone2.AnimatedBase * Quaternion.AngleAxis(num3, fingerBone2.LocalAxis);
					fingerBone2.Written = fingerBone2.Bone.localRotation;
					fingerBone2.HasWritten = true;
				}
			}
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x003024C8 File Offset: 0x003006C8
		private void BindFingers(Transform hand, Transform[] thumb, Transform[] index, Transform[] middle, Transform[] ring, Transform[] little)
		{
			this._hand = hand;
			if (hand == null)
			{
				return;
			}
			this.AddChain(thumb, true);
			this.AddChain(index, false);
			this.AddChain(middle, false);
			this.AddChain(ring, false);
			this.AddChain(little, false);
			this._palmFront = NippleRotorRemoteGrip.First(index);
			if (this._palmFront == null)
			{
				this._palmFront = NippleRotorRemoteGrip.First(middle);
			}
			this._palmBack = NippleRotorRemoteGrip.First(little);
			if (this._palmBack == null)
			{
				this._palmBack = NippleRotorRemoteGrip.First(ring);
			}
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x00302560 File Offset: 0x00300760
		private void AddChain(Transform[] chain, bool thumb)
		{
			if (chain == null)
			{
				return;
			}
			Transform transform = this._hand;
			for (int i = 0; i < chain.Length; i++)
			{
				Transform transform2 = chain[i];
				if (!(transform2 == null))
				{
					Transform transform3 = null;
					for (int j = i + 1; j < chain.Length; j++)
					{
						if (!(chain[j] == null))
						{
							transform3 = chain[j];
							break;
						}
					}
					this._fingers.Add(new NippleRotorRemoteGrip.FingerBone
					{
						Bone = transform2,
						Next = transform3,
						Previous = transform,
						Segment = (thumb ? NippleRotorRemoteGrip.Segment.Thumb : ((i == 0) ? NippleRotorRemoteGrip.Segment.Proximal : ((i == 1) ? NippleRotorRemoteGrip.Segment.Intermediate : NippleRotorRemoteGrip.Segment.Distal))),
						AnimatedBase = transform2.localRotation
					});
					transform = transform2;
				}
			}
		}

		// Token: 0x0600768A RID: 30346 RVA: 0x00302610 File Offset: 0x00300810
		private void CaptureAnimatedBase(NippleRotorRemoteGrip.FingerBone finger)
		{
			if (finger.Bone == null)
			{
				finger.HasWritten = false;
				return;
			}
			Quaternion localRotation = finger.Bone.localRotation;
			if (finger.HasWritten && NippleRotorRemoteGrip.Same(localRotation, finger.Written))
			{
				finger.Bone.localRotation = finger.AnimatedBase;
			}
			else
			{
				finger.AnimatedBase = localRotation;
			}
			finger.HasWritten = false;
		}

		// Token: 0x0600768B RID: 30347 RVA: 0x00302678 File Offset: 0x00300878
		private bool TryGetPalmNormal(out Vector3 normal)
		{
			normal = Vector3.zero;
			if (this._palmFront == null || this._palmBack == null)
			{
				return false;
			}
			Vector3 vector = this._palmFront.position - this._hand.position;
			Vector3 vector2 = this._palmBack.position - this._hand.position;
			normal = Vector3.Cross(vector, vector2);
			if (normal.sqrMagnitude <= vector.sqrMagnitude * vector2.sqrMagnitude * 1E-06f)
			{
				return false;
			}
			normal.Normalize();
			this._lateral = this._palmFront.position - this._palmBack.position;
			this._lateral = ((this._lateral.sqrMagnitude > 1E-12f) ? this._lateral.normalized : Vector3.zero);
			return true;
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x00302764 File Offset: 0x00300964
		private bool TryGetLocalAxis(NippleRotorRemoteGrip.FingerBone finger, Vector3 palmNormal, float sign, out Vector3 localAxis)
		{
			localAxis = Vector3.zero;
			Vector3 position = finger.Bone.position;
			Vector3 vector = ((finger.Next != null) ? (finger.Next.position - position) : ((finger.Previous != null) ? (position - finger.Previous.position) : Vector3.zero));
			if (vector.sqrMagnitude < 1E-14f)
			{
				return false;
			}
			Vector3 vector2 = Vector3.Cross(vector.normalized, palmNormal);
			if (vector2.sqrMagnitude < 0.0025f)
			{
				vector2 = this._lateral;
			}
			if (vector2.sqrMagnitude < 1E-12f)
			{
				return false;
			}
			localAxis = Quaternion.Inverse(finger.Bone.rotation) * (vector2.normalized * sign);
			return true;
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x00302840 File Offset: 0x00300A40
		private float DegreesFor(NippleRotorRemoteGrip.Segment segment)
		{
			switch (segment)
			{
			case NippleRotorRemoteGrip.Segment.Thumb:
				return this._settings.thumbCurl;
			case NippleRotorRemoteGrip.Segment.Proximal:
				return this._settings.proximalCurl;
			case NippleRotorRemoteGrip.Segment.Intermediate:
				return this._settings.intermediateCurl;
			default:
				return this._settings.distalCurl;
			}
		}

		// Token: 0x0600768E RID: 30350 RVA: 0x00302890 File Offset: 0x00300A90
		private void RestoreWrittenBones()
		{
			for (int i = 0; i < this._fingers.Count; i++)
			{
				NippleRotorRemoteGrip.FingerBone fingerBone = this._fingers[i];
				if (fingerBone.Bone != null && fingerBone.HasWritten && NippleRotorRemoteGrip.Same(fingerBone.Bone.localRotation, fingerBone.Written))
				{
					fingerBone.Bone.localRotation = fingerBone.AnimatedBase;
				}
				fingerBone.HasWritten = false;
			}
		}

		// Token: 0x0600768F RID: 30351 RVA: 0x00302908 File Offset: 0x00300B08
		private void ApplyPlacement()
		{
			if (this._hand == null || this._settings == null || base.transform.parent != this._hand)
			{
				return;
			}
			float num = Mathf.Abs(this._hand.lossyScale.x);
			if (num < 1E-06f)
			{
				return;
			}
			this._bodyScale = this.ResolveBodyScale();
			base.transform.localPosition = this._hand.InverseTransformVector(this._hand.rotation * (this._settings.localPosition * this._bodyScale));
			base.transform.localRotation = Quaternion.Euler(this._settings.localEuler);
			base.transform.localScale = Vector3.one * (this._bodyScale * Mathf.Max(0.0001f, this._settings.scale) / num);
		}

		// Token: 0x06007690 RID: 30352 RVA: 0x003029FC File Offset: 0x00300BFC
		private float ResolveBodyScale()
		{
			if (this._body == null && this._animator != null && Time.unscaledTime >= this._nextBodyResolveTime)
			{
				this._nextBodyResolveTime = Time.unscaledTime + 2f;
				this._body = NippleRotorRemoteGrip.FindBody(this._animator.transform);
			}
			float num = ((this._body != null) ? Mathf.Abs(this._body.transform.lossyScale.x) : 0f);
			if (num < 1E-06f && this._animator != null)
			{
				num = Mathf.Abs(this._animator.transform.lossyScale.x);
			}
			if (num >= 1E-06f)
			{
				return num;
			}
			return 1f;
		}

		// Token: 0x06007691 RID: 30353 RVA: 0x00302ACC File Offset: 0x00300CCC
		private static SkinnedMeshRenderer FindBody(Transform root)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2.name != "Body_sotai_NSFW"))
				{
					if (skinnedMeshRenderer2.gameObject.activeInHierarchy)
					{
						return skinnedMeshRenderer2;
					}
					if (skinnedMeshRenderer == null)
					{
						skinnedMeshRenderer = skinnedMeshRenderer2;
					}
				}
			}
			return skinnedMeshRenderer;
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x00302B22 File Offset: 0x00300D22
		private static Transform[] Bones(Animator animator, HumanBodyBones proximal, HumanBodyBones intermediate, HumanBodyBones distal)
		{
			return new Transform[]
			{
				animator.GetBoneTransform(proximal),
				animator.GetBoneTransform(intermediate),
				animator.GetBoneTransform(distal)
			};
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x00302B48 File Offset: 0x00300D48
		private static Transform First(Transform[] chain)
		{
			if (chain == null)
			{
				return null;
			}
			for (int i = 0; i < chain.Length; i++)
			{
				if (chain[i] != null)
				{
					return chain[i];
				}
			}
			return null;
		}

		// Token: 0x06007694 RID: 30356 RVA: 0x00302B78 File Offset: 0x00300D78
		private static Transform FindDeep(Transform root, string name)
		{
			if (root == null || string.IsNullOrEmpty(name))
			{
				return null;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				Transform transform = NippleRotorRemoteGrip.FindDeep(child, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x00302BD8 File Offset: 0x00300DD8
		private static bool Same(Quaternion a, Quaternion b)
		{
			return Mathf.Abs(a.x - b.x) <= 1E-05f && Mathf.Abs(a.y - b.y) <= 1E-05f && Mathf.Abs(a.z - b.z) <= 1E-05f && Mathf.Abs(a.w - b.w) <= 1E-05f;
		}

		// Token: 0x040079B9 RID: 31161
		public const string BodyRendererName = "Body_sotai_NSFW";

		// Token: 0x040079BA RID: 31162
		private const float BodyRetrySeconds = 2f;

		// Token: 0x040079BB RID: 31163
		private const float RotationEpsilon = 1E-05f;

		// Token: 0x040079BC RID: 31164
		private const float ParallelLimitSqr = 0.0025f;

		// Token: 0x040079BD RID: 31165
		private readonly List<NippleRotorRemoteGrip.FingerBone> _fingers = new List<NippleRotorRemoteGrip.FingerBone>(15);

		// Token: 0x040079BE RID: 31166
		private Animator _animator;

		// Token: 0x040079BF RID: 31167
		private NippleRotorGripSettings _settings;

		// Token: 0x040079C0 RID: 31168
		private Transform _hand;

		// Token: 0x040079C1 RID: 31169
		private Transform _palmFront;

		// Token: 0x040079C2 RID: 31170
		private Transform _palmBack;

		// Token: 0x040079C3 RID: 31171
		private Transform _cordSocket;

		// Token: 0x040079C4 RID: 31172
		private SkinnedMeshRenderer _body;

		// Token: 0x040079C5 RID: 31173
		private float _nextBodyResolveTime = float.NegativeInfinity;

		// Token: 0x040079C6 RID: 31174
		private float _bodyScale = 1f;

		// Token: 0x040079C7 RID: 31175
		private Vector3 _lateral;

		// Token: 0x040079C8 RID: 31176
		private float _weight;

		// Token: 0x040079C9 RID: 31177
		private bool _configured;

		// Token: 0x040079CA RID: 31178
		private bool _released;

		// Token: 0x040079CB RID: 31179
		private bool _curlEnabled = true;

		// Token: 0x02000F60 RID: 3936
		private enum Segment
		{
			// Token: 0x040079CD RID: 31181
			Thumb,
			// Token: 0x040079CE RID: 31182
			Proximal,
			// Token: 0x040079CF RID: 31183
			Intermediate,
			// Token: 0x040079D0 RID: 31184
			Distal
		}

		// Token: 0x02000F61 RID: 3937
		private sealed class FingerBone
		{
			// Token: 0x040079D1 RID: 31185
			public Transform Bone;

			// Token: 0x040079D2 RID: 31186
			public Transform Next;

			// Token: 0x040079D3 RID: 31187
			public Transform Previous;

			// Token: 0x040079D4 RID: 31188
			public NippleRotorRemoteGrip.Segment Segment;

			// Token: 0x040079D5 RID: 31189
			public Quaternion AnimatedBase;

			// Token: 0x040079D6 RID: 31190
			public Quaternion Written;

			// Token: 0x040079D7 RID: 31191
			public bool HasWritten;

			// Token: 0x040079D8 RID: 31192
			public bool HasAxis;

			// Token: 0x040079D9 RID: 31193
			public Vector3 LocalAxis;
		}
	}
}
