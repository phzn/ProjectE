using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace ProjectExhibitionist.Map
{
	// Token: 0x020009C3 RID: 2499
	public sealed class MallEscalatorConveyor : MonoBehaviour
	{
		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x001A9548 File Offset: 0x001A7748
		public bool GoesDown
		{
			get
			{
				return this._goesDown;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x001A9550 File Offset: 0x001A7750
		public float Speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x001A9558 File Offset: 0x001A7758
		public Collider WalkSurface
		{
			get
			{
				return this._walkSurface;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600464D RID: 17997 RVA: 0x001A9560 File Offset: 0x001A7760
		public Transform EscalatorRoot
		{
			get
			{
				return this._escalatorRoot;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x001A9568 File Offset: 0x001A7768
		public Transform VisualStepsRoot
		{
			get
			{
				return this._visualSteps;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x001A9570 File Offset: 0x001A7770
		public Vector3 LowerWorldPoint
		{
			get
			{
				if (!(this._escalatorRoot != null))
				{
					return base.transform.position;
				}
				return this._escalatorRoot.TransformPoint(this._lowerLocalPoint);
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004650 RID: 18000 RVA: 0x001A959D File Offset: 0x001A779D
		public Vector3 UpperWorldPoint
		{
			get
			{
				if (!(this._escalatorRoot != null))
				{
					return base.transform.position;
				}
				return this._escalatorRoot.TransformPoint(this._upperLocalPoint);
			}
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x001A95CC File Offset: 0x001A77CC
		public void Configure(Transform escalatorRoot, Transform visualSteps, Collider walkSurface, bool goesDown, Vector3[] stepPath, Vector3 lowerLocalPoint, Vector3 upperLocalPoint)
		{
			this._escalatorRoot = escalatorRoot;
			this._visualSteps = visualSteps;
			this._walkSurface = walkSurface;
			this._goesDown = goesDown;
			this._stepPath = stepPath;
			this._lowerLocalPoint = lowerLocalPoint;
			this._upperLocalPoint = upperLocalPoint;
			this.CachePathMetrics();
			this.CacheSteps();
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x001A961A File Offset: 0x001A781A
		private void Awake()
		{
			this._ownBody = base.GetComponent<Rigidbody>();
			this.CachePathMetrics();
			this.CacheSteps();
			if (Application.isPlaying && base.GetComponent<MallEscalatorNpcRecruiter>() == null)
			{
				base.gameObject.AddComponent<MallEscalatorNpcRecruiter>();
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x001A9655 File Offset: 0x001A7855
		private void OnEnable()
		{
			this.CachePathMetrics();
			this.CacheSteps();
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x001A9663 File Offset: 0x001A7863
		private void FixedUpdate()
		{
			this._movedBodies.Clear();
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x001A9670 File Offset: 0x001A7870
		private void Update()
		{
			if (this._escalatorRoot == null || this._steps == null || this._steps.Length == 0)
			{
				return;
			}
			if (this._renderPath == null || this._renderDistances == null || this._renderLength <= 0.01f || this._stepPitch <= 0.001f)
			{
				this.CachePathMetrics();
			}
			if (this._renderLength <= 0.01f || this._stepPitch <= 0.001f)
			{
				return;
			}
			float num = (float)this._steps.Length * this._stepPitch;
			if (num < this._renderLength)
			{
				num = this._renderLength;
			}
			this._travelDistance += Time.deltaTime * this._speed * (this._goesDown ? (-1f) : 1f);
			for (int i = 0; i < this._steps.Length; i++)
			{
				Transform transform = this._steps[i];
				if (!(transform == null))
				{
					float num2 = Mathf.Repeat((float)i * this._stepPitch + this._travelDistance, num);
					bool flag = num2 <= this._renderLength;
					if (flag)
					{
						transform.localPosition = this.SamplePath(num2);
					}
					MeshRenderer meshRenderer = ((this._stepRenderers != null && i < this._stepRenderers.Length) ? this._stepRenderers[i] : null);
					if (meshRenderer != null && meshRenderer.enabled != flag)
					{
						meshRenderer.enabled = flag;
					}
				}
			}
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x001A97D8 File Offset: 0x001A79D8
		private void OnTriggerStay(Collider other)
		{
			if (this._escalatorRoot == null || other == null || other.isTrigger)
			{
				return;
			}
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			vThirdPersonMotor vThirdPersonMotor = null;
			if (attachedRigidbody != null && !attachedRigidbody.TryGetComponent<vThirdPersonMotor>(out vThirdPersonMotor))
			{
				vThirdPersonMotor = attachedRigidbody.GetComponentInParent<vThirdPersonMotor>();
			}
			if (vThirdPersonMotor != null && vThirdPersonMotor.enabled)
			{
				Collider collider = vThirdPersonMotor.groundHit.collider;
				if (!vThirdPersonMotor.isGrounded || this._walkSurface == null || collider == null || (collider != this._walkSurface && !collider.transform.IsChildOf(this._walkSurface.transform)))
				{
					return;
				}
				int instanceID = vThirdPersonMotor.GetInstanceID();
				if (this._movedBodies.Add(instanceID))
				{
					Rigidbody rigidbody = ((attachedRigidbody != null) ? attachedRigidbody : vThirdPersonMotor.GetComponent<Rigidbody>());
					Vector3 vector = ((rigidbody != null) ? rigidbody.worldCenterOfMass : vThirdPersonMotor.transform.position);
					Vector3 vector2 = this.GetWorldPathDirection(vector);
					if (this._goesDown)
					{
						vector2 = -vector2;
					}
					Vector3 vector3 = Vector3.ProjectOnPlane(vector2, Vector3.up).normalized * this._speed;
					vThirdPersonMotor.SetExternalPlatformVelocity(vector3);
				}
				return;
			}
			else
			{
				global::UnityEngine.CharacterController characterController = other as global::UnityEngine.CharacterController;
				if (characterController == null)
				{
					other.TryGetComponent<global::UnityEngine.CharacterController>(out characterController);
				}
				bool flag = characterController != null && characterController.enabled;
				bool flag2 = attachedRigidbody != null && attachedRigidbody != this._ownBody && !attachedRigidbody.isKinematic;
				if (!flag && !flag2)
				{
					return;
				}
				Vector3 vector4 = this.GetWorldPathDirection(other.bounds.center);
				if (this._goesDown)
				{
					vector4 = -vector4;
				}
				Vector3 vector5 = vector4 * (this._speed * Time.fixedDeltaTime);
				if (flag)
				{
					int instanceID2 = characterController.GetInstanceID();
					if (this._movedBodies.Add(instanceID2))
					{
						characterController.Move(vector5);
					}
					return;
				}
				if (attachedRigidbody == null || attachedRigidbody == this._ownBody || attachedRigidbody.isKinematic)
				{
					return;
				}
				int instanceID3 = attachedRigidbody.GetInstanceID();
				if (this._movedBodies.Add(instanceID3))
				{
					attachedRigidbody.MovePosition(attachedRigidbody.position + vector5);
				}
				return;
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x001A9A2C File Offset: 0x001A7C2C
		private Vector3 GetWorldPathDirection(Vector3 worldPosition)
		{
			if (this._stepPath == null || this._stepPath.Length < 2)
			{
				return (this._escalatorRoot.TransformPoint(this._upperLocalPoint) - this._escalatorRoot.TransformPoint(this._lowerLocalPoint)).normalized;
			}
			Vector3 vector = this._escalatorRoot.InverseTransformPoint(worldPosition);
			float num = float.PositiveInfinity;
			Vector3 vector2 = this._stepPath[1] - this._stepPath[0];
			int num2 = 0;
			while (num2 + 1 < this._stepPath.Length)
			{
				Vector3 vector3 = this._stepPath[num2];
				Vector3 vector4 = this._stepPath[num2 + 1] - vector3;
				float sqrMagnitude = vector4.sqrMagnitude;
				if (sqrMagnitude >= 1E-06f)
				{
					float num3 = Mathf.Clamp01(Vector3.Dot(vector - vector3, vector4) / sqrMagnitude);
					float sqrMagnitude2 = (vector - (vector3 + vector4 * num3)).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
						vector2 = vector4;
					}
				}
				num2++;
			}
			return this._escalatorRoot.TransformVector(vector2).normalized;
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x001A9B60 File Offset: 0x001A7D60
		private void CacheSteps()
		{
			if (this._visualSteps == null)
			{
				this._steps = null;
				this._stepRenderers = null;
				return;
			}
			if (Application.isPlaying && this._stepPitch > 0.001f && this._renderLength > 0.01f)
			{
				int num = Mathf.CeilToInt(this._renderLength / this._stepPitch);
				int childCount = this._visualSteps.childCount;
				if (childCount > 0 && num > childCount)
				{
					Transform child = this._visualSteps.GetChild(childCount - 1);
					for (int i = childCount; i < num; i++)
					{
						Object.Instantiate<Transform>(child, this._visualSteps).name = string.Format("MallEscalatorMovingTread_Extra_{0:00}", i);
					}
				}
			}
			this._steps = new Transform[this._visualSteps.childCount];
			this._stepRenderers = new MeshRenderer[this._steps.Length];
			for (int j = 0; j < this._steps.Length; j++)
			{
				this._steps[j] = this._visualSteps.GetChild(j);
				this._steps[j].localRotation = Quaternion.identity;
				this._stepRenderers[j] = this._steps[j].GetComponent<MeshRenderer>();
			}
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x001A9C90 File Offset: 0x001A7E90
		private void CachePathMetrics()
		{
			this._renderPath = null;
			this._renderDistances = null;
			this._renderLength = 0f;
			this._stepPitch = 0f;
			if (this._escalatorRoot == null || this._stepPath == null || this._stepPath.Length < 2)
			{
				return;
			}
			float num = 0f;
			for (int i = 1; i < this._stepPath.Length; i++)
			{
				num += Vector3.Distance(this._escalatorRoot.TransformPoint(this._stepPath[i - 1]), this._escalatorRoot.TransformPoint(this._stepPath[i]));
			}
			if (num <= 0.01f)
			{
				return;
			}
			this._stepPitch = num / (float)(this._stepPath.Length - 1);
			this._renderPath = this.BuildRenderPath();
			this._renderDistances = new float[this._renderPath.Length];
			for (int j = 1; j < this._renderPath.Length; j++)
			{
				this._renderDistances[j] = this._renderDistances[j - 1] + Vector3.Distance(this._escalatorRoot.TransformPoint(this._renderPath[j - 1]), this._escalatorRoot.TransformPoint(this._renderPath[j]));
			}
			this._renderLength = this._renderDistances[this._renderDistances.Length - 1];
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x001A9DE4 File Offset: 0x001A7FE4
		private Vector3[] BuildRenderPath()
		{
			int num = this._stepPath.Length;
			if (this._hiddenRunLength <= 0.001f)
			{
				return this._stepPath;
			}
			Vector3 vector = this._stepPath[0] - this._stepPath[1];
			vector.y = 0f;
			vector = ((vector.sqrMagnitude > 1E-06f) ? vector.normalized : Vector3.right);
			Vector3 vector2 = this._stepPath[num - 1] - this._stepPath[num - 2];
			vector2.y = 0f;
			vector2 = ((vector2.sqrMagnitude > 1E-06f) ? vector2.normalized : Vector3.left);
			Vector3[] array = new Vector3[num + 4];
			array[0] = this._stepPath[0] + vector * (this._hiddenRunLength + this._hiddenRunDiveLength) + Vector3.down * (0.01f + this._hiddenRunDiveDepth);
			array[1] = this._stepPath[0] + vector * this._hiddenRunLength + Vector3.down * 0.01f;
			for (int i = 0; i < num; i++)
			{
				array[i + 2] = this._stepPath[i];
			}
			array[num + 2] = this._stepPath[num - 1] + vector2 * this._hiddenRunLength + Vector3.down * 0.01f;
			array[num + 3] = this._stepPath[num - 1] + vector2 * (this._hiddenRunLength + this._hiddenRunDiveLength) + Vector3.down * (0.01f + this._hiddenRunDiveDepth);
			return array;
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x001A9FD8 File Offset: 0x001A81D8
		private Vector3 SamplePath(float distance)
		{
			int i = 1;
			int num = this._renderDistances.Length;
			while (i < num)
			{
				int num2 = i + (num - i) / 2;
				if (this._renderDistances[num2] < distance)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			int num3 = i - 1;
			int num4 = Mathf.Min(num3 + 1, this._renderPath.Length - 1);
			float num5 = this._renderDistances[num4] - this._renderDistances[num3];
			float num6 = ((num5 > 1E-06f) ? ((distance - this._renderDistances[num3]) / num5) : 0f);
			return Vector3.LerpUnclamped(this._renderPath[num3], this._renderPath[num4], num6);
		}

		// Token: 0x040045DE RID: 17886
		[SerializeField]
		private Transform _escalatorRoot;

		// Token: 0x040045DF RID: 17887
		[SerializeField]
		private Transform _visualSteps;

		// Token: 0x040045E0 RID: 17888
		[SerializeField]
		private Collider _walkSurface;

		// Token: 0x040045E1 RID: 17889
		[SerializeField]
		private bool _goesDown;

		// Token: 0x040045E2 RID: 17890
		[SerializeField]
		[Min(0.1f)]
		private float _speed = 0.55f;

		// Token: 0x040045E3 RID: 17891
		[SerializeField]
		private Vector3 _lowerLocalPoint = new Vector3(-0.141f, 1.064f, 1.05f);

		// Token: 0x040045E4 RID: 17892
		[SerializeField]
		private Vector3 _upperLocalPoint = new Vector3(-13.887f, 7.056f, 1.05f);

		// Token: 0x040045E5 RID: 17893
		[SerializeField]
		private Vector3[] _stepPath;

		// Token: 0x040045E6 RID: 17894
		[SerializeField]
		[Min(0f)]
		private float _hiddenRunLength = 0.35f;

		// Token: 0x040045E7 RID: 17895
		[SerializeField]
		[Min(0f)]
		private float _hiddenRunDiveLength = 0.25f;

		// Token: 0x040045E8 RID: 17896
		[SerializeField]
		[Min(0f)]
		private float _hiddenRunDiveDepth = 0.35f;

		// Token: 0x040045E9 RID: 17897
		private const float HiddenRunSettle = 0.01f;

		// Token: 0x040045EA RID: 17898
		private readonly HashSet<int> _movedBodies = new HashSet<int>();

		// Token: 0x040045EB RID: 17899
		private Transform[] _steps;

		// Token: 0x040045EC RID: 17900
		private MeshRenderer[] _stepRenderers;

		// Token: 0x040045ED RID: 17901
		private Vector3[] _renderPath;

		// Token: 0x040045EE RID: 17902
		private float[] _renderDistances;

		// Token: 0x040045EF RID: 17903
		private float _renderLength;

		// Token: 0x040045F0 RID: 17904
		private float _stepPitch;

		// Token: 0x040045F1 RID: 17905
		private float _travelDistance;

		// Token: 0x040045F2 RID: 17906
		private Rigidbody _ownBody;
	}
}
