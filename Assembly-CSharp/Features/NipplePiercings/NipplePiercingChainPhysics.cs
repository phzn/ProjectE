using System;
using UnityEngine;

namespace Features.NipplePiercings
{
	// Token: 0x02000F65 RID: 3941
	[DisallowMultipleComponent]
	public sealed class NipplePiercingChainPhysics : MonoBehaviour
	{
		// Token: 0x060076BD RID: 30397 RVA: 0x00303C64 File Offset: 0x00301E64
		public void Configure(Transform left, Transform right, Transform[] chainNodes, Vector3[] restPositions)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (chainNodes == null || restPositions == null || chainNodes.Length < 2 || chainNodes.Length != restPositions.Length)
			{
				throw new ArgumentException("A piercing chain needs matching node and rest-position arrays with at least two entries.");
			}
			for (int i = 0; i < chainNodes.Length; i++)
			{
				if (chainNodes[i] == null || !NipplePiercingChainPhysics.IsFinite(restPositions[i]))
				{
					throw new ArgumentException("Piercing chain nodes and rest positions must be valid.");
				}
			}
			this.leftEndpoint = left;
			this.rightEndpoint = right;
			this.nodes = (Transform[])chainNodes.Clone();
			this.restPositionsLocal = new Vector3[this.nodes.Length];
			this.restRotationsLocal = new Quaternion[this.nodes.Length];
			Quaternion quaternion = Quaternion.Inverse(base.transform.rotation);
			this.restEndpointRotationLocal = quaternion * left.rotation;
			for (int j = 0; j < this.nodes.Length; j++)
			{
				this.restPositionsLocal[j] = base.transform.InverseTransformPoint(restPositions[j]);
				this.restRotationsLocal[j] = quaternion * this.nodes[j].rotation;
			}
			this._initialized = false;
			this.ResetSimulation();
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x00303D9F File Offset: 0x00301F9F
		public void Configure(Transform root, Transform[] chainNodes, Vector3[] restPositions)
		{
			this.Configure(root, null, chainNodes, restPositions);
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x00303DAB File Offset: 0x00301FAB
		private void OnEnable()
		{
			this.ResetSimulation();
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x00303DB3 File Offset: 0x00301FB3
		private void OnDisable()
		{
			this._initialized = false;
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x00303DBC File Offset: 0x00301FBC
		public void ResetSimulation()
		{
			if (!this.EnsureStorage() || !this.UpdateTargetCurve())
			{
				return;
			}
			this.ResetToTargets();
		}

		// Token: 0x060076C2 RID: 30402 RVA: 0x00303DD8 File Offset: 0x00301FD8
		public void StepSimulation(float dt)
		{
			if (!base.isActiveAndEnabled || !this.EnsureStorage() || !this.UpdateTargetCurve())
			{
				return;
			}
			float num = Mathf.Max(base.transform.lossyScale.magnitude / Mathf.Sqrt(3f), 0.001f);
			float num2 = this.teleportDistance * num;
			bool flag = (this._targets[0] - this._lastLeft).sqrMagnitude > num2 * num2;
			if (this.rightEndpoint != null)
			{
				flag |= (this._targets[this.nodes.Length - 1] - this._lastRight).sqrMagnitude > num2 * num2;
			}
			if (!this._initialized || !NipplePiercingChainPhysics.IsFinite(dt) || dt < 0f || dt > 0.25f || flag)
			{
				this.ResetToTargets();
				return;
			}
			if (this.rightEndpoint == null && this.nodes.Length == 2)
			{
				this.StepPendant(dt, num);
				return;
			}
			float num3 = Mathf.Min(dt, 0.06666667f);
			int num4 = Mathf.Max(1, Mathf.CeilToInt(num3 / 0.008333334f));
			float num5 = num3 / (float)num4;
			Array.Copy(this._targets, this._frameTargets, this.nodes.Length);
			for (int i = 0; i < num4; i++)
			{
				for (int j = 0; j < this.nodes.Length; j++)
				{
					Vector3 vector = (this._frameTargets[j] - this._lastTargets[j]) / (float)num4 * ((dt > 0f) ? (1f - this.motionInfluence) : 1f);
					this._positions[j] += vector;
					this._previous[j] += vector;
					this._targets[j] = Vector3.Lerp(this._lastTargets[j], this._frameTargets[j], ((float)i + 1f) / (float)num4);
				}
				for (int k = 0; k < this._segmentLengths.Length; k++)
				{
					this._segmentLengths[k] = Vector3.Distance(this._targets[k], this._targets[k + 1]);
				}
				if (num5 > 0f)
				{
					float num6 = ((this._previousStep > 0f) ? (num5 / this._previousStep) : 1f);
					float num7 = Mathf.Exp(-Mathf.Max(18f, this.damping) * num5);
					for (int l = 1; l < this.nodes.Length; l++)
					{
						if (!this.IsPinned(l))
						{
							Vector3 vector2 = this._positions[l];
							Vector3 vector3 = (vector2 - this._previous[l]) * (num6 * num7);
							Vector3 vector4 = Physics.gravity * (this.gravityMultiplier * num) + (this._targets[l] - vector2) * this.restSpring;
							this._positions[l] = vector2 + vector3 + vector4 * (num5 * num5);
							this._previous[l] = vector2;
						}
					}
					this._previousStep = num5;
				}
				this.PinEndpoints();
				for (int m = 0; m < this.constraintIterations; m++)
				{
					if ((m & 1) == 0)
					{
						for (int n = 0; n < this.nodes.Length - 1; n++)
						{
							this.ConstrainSegment(n);
						}
					}
					else
					{
						for (int num8 = this.nodes.Length - 2; num8 >= 0; num8--)
						{
							this.ConstrainSegment(num8);
						}
					}
					this.KeepInFrontOfChest(num);
					this.LimitConnectorOffset(num);
					this.PinEndpoints();
				}
			}
			for (int num9 = 0; num9 < this._positions.Length; num9++)
			{
				if (!NipplePiercingChainPhysics.IsFinite(this._positions[num9]))
				{
					this.ResetToTargets();
					return;
				}
			}
			this.RememberEndpoints();
			this.ApplyNodePoses();
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x00304228 File Offset: 0x00302428
		private void StepPendant(float dt, float scale)
		{
			if (dt > 0.1f || Quaternion.Angle(this._lastEndpointRotation, this.leftEndpoint.rotation) > 75f)
			{
				this.ResetToTargets();
				return;
			}
			Quaternion quaternion = base.transform.rotation * this._fitRotationLocal;
			Vector3 normalized = (this.restPositionsLocal[1] - this.restPositionsLocal[0]).normalized;
			float num = this._segmentLengths[0];
			if (num < 1E-06f)
			{
				this.ResetToTargets();
				return;
			}
			if (dt > 0f)
			{
				Vector3 vector = (this._targets[1] - this._lastRestPoint) / dt;
				Vector3 vector2 = Vector3.ClampMagnitude((vector - this._lastRestVelocity) / dt, 60f * scale);
				this._lastRestVelocity = vector;
				int num2 = Mathf.Max(1, Mathf.CeilToInt(dt * 120f));
				float num3 = dt / (float)num2;
				for (int i = 0; i < num2; i++)
				{
					this._filteredAcceleration = Vector3.Lerp(this._filteredAcceleration, vector2, 1f - Mathf.Exp(-20f * num3));
					Vector3 vector3 = Physics.gravity * (this.gravityMultiplier * scale * 0.06f) - this._filteredAcceleration * this.motionInfluence;
					Vector3 vector4 = Vector3.Cross(normalized, Quaternion.Inverse(quaternion) * vector3) / num;
					this._pendantVelocity += (vector4 - this._pendantAngle * 180f - this._pendantVelocity * 24f) * num3;
					this._pendantVelocity = Vector3.ClampMagnitude(this._pendantVelocity, 2.5f);
					this._pendantAngle += this._pendantVelocity * num3;
					float num4 = this.maximumPendantAngle * 0.017453292f;
					if (this._pendantAngle.sqrMagnitude > num4 * num4)
					{
						Vector3 normalized2 = this._pendantAngle.normalized;
						this._pendantAngle = normalized2 * num4;
						float num5 = Vector3.Dot(this._pendantVelocity, normalized2);
						if (num5 > 0f)
						{
							this._pendantVelocity -= normalized2 * num5;
						}
					}
				}
			}
			else
			{
				this._lastRestVelocity = (this._filteredAcceleration = Vector3.zero);
			}
			Vector3 vector5 = this.RotatePendant(normalized);
			Vector3 vector6 = Quaternion.Inverse(quaternion) * this.leftEndpoint.forward;
			float num6 = -this.chestClearance * scale / num;
			if (Vector3.Dot(vector5, vector6) < num6)
			{
				float num7 = Mathf.Clamp(num6, -1f, 1f);
				Vector3 vector7 = Vector3.ProjectOnPlane(vector5, vector6).normalized;
				Vector3 vector8 = Vector3.ProjectOnPlane(normalized, vector6);
				float num8 = Mathf.Sqrt(1f - num7 * num7);
				if (vector8.sqrMagnitude > 1E-10f && num8 > 1E-06f)
				{
					float num9 = Mathf.Acos(Mathf.Clamp((Mathf.Cos(this.maximumPendantAngle * 0.017453292f) - num7 * Vector3.Dot(normalized, vector6)) / (num8 * vector8.magnitude), -1f, 1f));
					vector7 = Vector3.RotateTowards(vector8.normalized, vector7, num9, 0f);
				}
				vector5 = vector7 * Mathf.Sqrt(1f - num7 * num7) + vector6 * num7;
				float num10;
				Vector3 vector9;
				NipplePiercingChainPhysics.RotationBetween(normalized, vector5).ToAngleAxis(out num10, out vector9);
				this._pendantAngle = vector9 * (Mathf.DeltaAngle(0f, num10) * 0.017453292f);
				Vector3 normalized3 = Vector3.Cross(vector5, vector6).normalized;
				float num11 = Vector3.Dot(this._pendantVelocity, normalized3);
				if (num11 < 0f)
				{
					this._pendantVelocity -= normalized3 * num11;
				}
			}
			this._positions[0] = this._targets[0];
			this._positions[1] = this._targets[0] + quaternion * vector5 * num;
			this._previous[0] = this._positions[0];
			this._previous[1] = this._positions[1];
			this._lastRestPoint = this._targets[1];
			this.RememberEndpoints();
			this.ApplyNodePoses();
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x003046C4 File Offset: 0x003028C4
		private Vector3 RotatePendant(Vector3 rest)
		{
			float magnitude = this._pendantAngle.magnitude;
			if (magnitude <= 1E-08f)
			{
				return rest;
			}
			return Quaternion.AngleAxis(magnitude * 57.29578f, this._pendantAngle / magnitude) * rest;
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x00304708 File Offset: 0x00302908
		private void LimitConnectorOffset(float scale)
		{
			if (this.rightEndpoint == null)
			{
				return;
			}
			float num = 0.012f * scale;
			for (int i = 1; i < this.nodes.Length - 1; i++)
			{
				Vector3 vector = this._positions[i] - this._targets[i];
				if (vector.sqrMagnitude > num * num)
				{
					Vector3 normalized = vector.normalized;
					Vector3 vector2 = normalized * num - vector;
					Vector3 vector3 = this._positions[i] - this._previous[i];
					this._positions[i] += vector2;
					this._previous[i] += vector2;
					float num2 = Vector3.Dot(vector3, normalized);
					if (num2 > 0f)
					{
						this._previous[i] += normalized * num2;
					}
				}
			}
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x0030481C File Offset: 0x00302A1C
		private bool EnsureStorage()
		{
			if (this.leftEndpoint == null || this.nodes == null || this.nodes.Length < 2 || this.restPositionsLocal == null || this.restPositionsLocal.Length != this.nodes.Length || this.restRotationsLocal == null || this.restRotationsLocal.Length != this.nodes.Length)
			{
				return false;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] == null)
				{
					return false;
				}
			}
			if (this._positions != null && this._positions.Length == this.nodes.Length)
			{
				return true;
			}
			this._positions = new Vector3[this.nodes.Length];
			this._previous = new Vector3[this.nodes.Length];
			this._targets = new Vector3[this.nodes.Length];
			this._lastTargets = new Vector3[this.nodes.Length];
			this._frameTargets = new Vector3[this.nodes.Length];
			this._segmentLengths = new float[this.nodes.Length - 1];
			this._initialized = false;
			return true;
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x00304940 File Offset: 0x00302B40
		private bool UpdateTargetCurve()
		{
			Vector3 vector = base.transform.InverseTransformPoint(this.leftEndpoint.position);
			if (!NipplePiercingChainPhysics.IsFinite(vector))
			{
				return false;
			}
			int num = this.nodes.Length - 1;
			Vector3 vector2 = this.restPositionsLocal[0];
			if (this.rightEndpoint != null)
			{
				Vector3 vector3 = base.transform.InverseTransformPoint(this.rightEndpoint.position);
				if (!NipplePiercingChainPhysics.IsFinite(vector3))
				{
					return false;
				}
				Vector3 vector4 = this.restPositionsLocal[num] - vector2;
				Vector3 vector5 = vector3 - vector;
				float sqrMagnitude = vector4.sqrMagnitude;
				this._fitRotationLocal = ((sqrMagnitude > 1E-10f && vector5.sqrMagnitude > 1E-10f) ? Quaternion.FromToRotation(vector4, vector5) : Quaternion.identity);
				for (int i = 0; i < this.nodes.Length; i++)
				{
					float num2 = ((sqrMagnitude > 1E-10f) ? (Vector3.Dot(this.restPositionsLocal[i] - vector2, vector4) / sqrMagnitude) : ((float)i / (float)num));
					Vector3 vector6 = this.restPositionsLocal[i] - Vector3.LerpUnclamped(vector2, this.restPositionsLocal[num], num2);
					this._targets[i] = base.transform.TransformPoint(Vector3.LerpUnclamped(vector, vector3, num2) + this._fitRotationLocal * vector6);
				}
			}
			else
			{
				Quaternion quaternion = Quaternion.Inverse(base.transform.rotation) * this.leftEndpoint.rotation;
				this._fitRotationLocal = quaternion * Quaternion.Inverse(this.restEndpointRotationLocal);
				for (int j = 0; j < this.nodes.Length; j++)
				{
					this._targets[j] = base.transform.TransformPoint(vector + this._fitRotationLocal * (this.restPositionsLocal[j] - vector2));
				}
			}
			for (int k = 0; k < num; k++)
			{
				this._segmentLengths[k] = Vector3.Distance(this._targets[k], this._targets[k + 1]);
			}
			return true;
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x00304B80 File Offset: 0x00302D80
		private void ResetToTargets()
		{
			Array.Copy(this._targets, this._positions, this.nodes.Length);
			Array.Copy(this._targets, this._previous, this.nodes.Length);
			this._previousStep = 0f;
			this._pendantAngle = (this._pendantVelocity = (this._filteredAcceleration = Vector3.zero));
			this._lastRestVelocity = Vector3.zero;
			this._lastRestPoint = this._targets[this.nodes.Length - 1];
			this._initialized = true;
			this.RememberEndpoints();
			if (this.rightEndpoint == null && this.nodes.Length == 2 && this._segmentLengths[0] > 1E-06f)
			{
				this.StepPendant(0f, Mathf.Max(base.transform.lossyScale.magnitude / Mathf.Sqrt(3f), 0.001f));
				return;
			}
			this.ApplyNodePoses();
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x00304C7C File Offset: 0x00302E7C
		private void RememberEndpoints()
		{
			this._lastLeft = this._targets[0];
			this._lastRight = this._targets[this.nodes.Length - 1];
			this._lastEndpointRotation = this.leftEndpoint.rotation;
			Array.Copy(this._targets, this._lastTargets, this.nodes.Length);
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x00304CE0 File Offset: 0x00302EE0
		private bool IsPinned(int index)
		{
			return index == 0 || (this.rightEndpoint != null && index == this.nodes.Length - 1);
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x00304D04 File Offset: 0x00302F04
		private void PinEndpoints()
		{
			this._positions[0] = (this._previous[0] = this._targets[0]);
			if (this.rightEndpoint != null)
			{
				int num = this.nodes.Length - 1;
				this._positions[num] = (this._previous[num] = this._targets[num]);
			}
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x00304D78 File Offset: 0x00302F78
		private void ConstrainSegment(int index)
		{
			int num = index + 1;
			Vector3 vector = this._positions[num] - this._positions[index];
			float magnitude = vector.magnitude;
			if (magnitude < 1E-07f)
			{
				return;
			}
			Vector3 vector2 = vector * ((magnitude - this._segmentLengths[index]) / magnitude);
			bool flag = this.IsPinned(index);
			bool flag2 = this.IsPinned(num);
			if (flag && flag2)
			{
				return;
			}
			if (flag)
			{
				this._positions[num] -= vector2;
				return;
			}
			if (flag2)
			{
				this._positions[index] += vector2;
				return;
			}
			this._positions[index] += vector2 * 0.5f;
			this._positions[num] -= vector2 * 0.5f;
		}

		// Token: 0x060076CD RID: 30413 RVA: 0x00304E78 File Offset: 0x00303078
		private void KeepInFrontOfChest(float scale)
		{
			Vector3 vector = this.leftEndpoint.forward;
			if (this.rightEndpoint != null)
			{
				vector += this.rightEndpoint.forward;
			}
			if (vector.sqrMagnitude < 1E-10f)
			{
				vector = this.leftEndpoint.forward;
			}
			vector.Normalize();
			int num = this.nodes.Length - 1;
			Vector3 vector2 = this.restPositionsLocal[num] - this.restPositionsLocal[0];
			float sqrMagnitude = vector2.sqrMagnitude;
			float num2 = -this.chestClearance * scale;
			for (int i = 1; i < this.nodes.Length; i++)
			{
				if (!this.IsPinned(i))
				{
					Vector3 vector3 = this._targets[0];
					if (this.rightEndpoint != null)
					{
						float num3 = ((sqrMagnitude > 1E-10f) ? Mathf.Clamp01(Vector3.Dot(this.restPositionsLocal[i] - this.restPositionsLocal[0], vector2) / sqrMagnitude) : ((float)i / (float)num));
						vector3 = Vector3.Lerp(this._targets[0], this._targets[num], num3);
					}
					float num4 = num2 - Vector3.Dot(this._positions[i] - vector3, vector);
					if (num4 > 0f)
					{
						float num5 = Vector3.Dot(this._positions[i] - this._previous[i], vector);
						this._positions[i] += vector * num4;
						this._previous[i] += vector * num4;
						if (num5 < 0f)
						{
							this._previous[i] += vector * num5;
						}
					}
				}
			}
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x0030507C File Offset: 0x0030327C
		private void ApplyNodePoses()
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				int num = Mathf.Max(0, i - 1);
				int num2 = Mathf.Min(this.nodes.Length - 1, i + 1);
				Vector3 vector = base.transform.TransformVector(this._fitRotationLocal * (this.restPositionsLocal[num2] - this.restPositionsLocal[num]));
				Vector3 vector2 = this._positions[num2] - this._positions[num];
				Quaternion quaternion = base.transform.rotation * this._fitRotationLocal * this.restRotationsLocal[i];
				if ((!(this.rightEndpoint == null) || i != 0) && vector.sqrMagnitude > 1E-10f && vector2.sqrMagnitude > 1E-10f)
				{
					quaternion = NipplePiercingChainPhysics.RotationBetween(vector, vector2) * quaternion;
				}
				this.nodes[i].SetPositionAndRotation(this._positions[i], quaternion);
			}
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x0030519C File Offset: 0x0030339C
		private static Quaternion RotationBetween(Vector3 from, Vector3 to)
		{
			from.Normalize();
			to.Normalize();
			Vector3 vector = Vector3.Cross(from, to);
			float magnitude = vector.magnitude;
			float num = Mathf.Clamp(Vector3.Dot(from, to), -1f, 1f);
			if (magnitude > 1E-08f)
			{
				return Quaternion.AngleAxis(Mathf.Atan2(magnitude, num) * 57.29578f, vector / magnitude);
			}
			if (num < 0f)
			{
				return Quaternion.FromToRotation(from, to);
			}
			return Quaternion.identity;
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x00305215 File Offset: 0x00303415
		private static bool IsFinite(Vector3 value)
		{
			return NipplePiercingChainPhysics.IsFinite(value.x) && NipplePiercingChainPhysics.IsFinite(value.y) && NipplePiercingChainPhysics.IsFinite(value.z);
		}

		// Token: 0x060076D1 RID: 30417 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool IsFinite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x040079F5 RID: 31221
		[SerializeField]
		private Transform leftEndpoint;

		// Token: 0x040079F6 RID: 31222
		[SerializeField]
		private Transform rightEndpoint;

		// Token: 0x040079F7 RID: 31223
		[SerializeField]
		private Transform[] nodes = Array.Empty<Transform>();

		// Token: 0x040079F8 RID: 31224
		[SerializeField]
		private Vector3[] restPositionsLocal = Array.Empty<Vector3>();

		// Token: 0x040079F9 RID: 31225
		[SerializeField]
		private Quaternion[] restRotationsLocal = Array.Empty<Quaternion>();

		// Token: 0x040079FA RID: 31226
		[SerializeField]
		private Quaternion restEndpointRotationLocal = Quaternion.identity;

		// Token: 0x040079FB RID: 31227
		[SerializeField]
		[Min(0f)]
		private float damping = 8f;

		// Token: 0x040079FC RID: 31228
		[SerializeField]
		[Min(0f)]
		private float restSpring = 36f;

		// Token: 0x040079FD RID: 31229
		[SerializeField]
		[Range(0f, 1f)]
		private float gravityMultiplier = 0.35f;

		// Token: 0x040079FE RID: 31230
		[SerializeField]
		[Range(4f, 40f)]
		private int constraintIterations = 24;

		// Token: 0x040079FF RID: 31231
		[SerializeField]
		[Min(0.01f)]
		private float teleportDistance = 0.35f;

		// Token: 0x04007A00 RID: 31232
		[SerializeField]
		[Min(0f)]
		private float chestClearance = 0.001f;

		// Token: 0x04007A01 RID: 31233
		[SerializeField]
		[Range(0f, 1f)]
		private float motionInfluence = 0.1f;

		// Token: 0x04007A02 RID: 31234
		[SerializeField]
		[Range(1f, 30f)]
		private float maximumPendantAngle = 14f;

		// Token: 0x04007A03 RID: 31235
		private const float PendantSpring = 180f;

		// Token: 0x04007A04 RID: 31236
		private const float PendantDamping = 24f;

		// Token: 0x04007A05 RID: 31237
		private const float MaximumAngularSpeed = 2.5f;

		// Token: 0x04007A06 RID: 31238
		private Vector3 _pendantAngle;

		// Token: 0x04007A07 RID: 31239
		private Vector3 _pendantVelocity;

		// Token: 0x04007A08 RID: 31240
		private Vector3 _filteredAcceleration;

		// Token: 0x04007A09 RID: 31241
		private Vector3 _lastRestPoint;

		// Token: 0x04007A0A RID: 31242
		private Vector3 _lastRestVelocity;

		// Token: 0x04007A0B RID: 31243
		private Quaternion _lastEndpointRotation;

		// Token: 0x04007A0C RID: 31244
		private Vector3[] _positions;

		// Token: 0x04007A0D RID: 31245
		private Vector3[] _previous;

		// Token: 0x04007A0E RID: 31246
		private Vector3[] _targets;

		// Token: 0x04007A0F RID: 31247
		private Vector3[] _lastTargets;

		// Token: 0x04007A10 RID: 31248
		private Vector3[] _frameTargets;

		// Token: 0x04007A11 RID: 31249
		private float[] _segmentLengths;

		// Token: 0x04007A12 RID: 31250
		private Quaternion _fitRotationLocal = Quaternion.identity;

		// Token: 0x04007A13 RID: 31251
		private Vector3 _lastLeft;

		// Token: 0x04007A14 RID: 31252
		private Vector3 _lastRight;

		// Token: 0x04007A15 RID: 31253
		private float _previousStep;

		// Token: 0x04007A16 RID: 31254
		private bool _initialized;
	}
}
