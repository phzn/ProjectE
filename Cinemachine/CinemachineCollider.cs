using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x0200000F RID: 15
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineCollider.html")]
	public class CinemachineCollider : CinemachineExtension
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00004AB9 File Offset: 0x00002CB9
		public bool IsTargetObscured(ICinemachineCamera vcam)
		{
			return base.GetExtraState<CinemachineCollider.VcamExtraState>(vcam).targetObscured;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004AC7 File Offset: 0x00002CC7
		public bool CameraWasDisplaced(ICinemachineCamera vcam)
		{
			return this.GetCameraDisplacementDistance(vcam) > 0f;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004AD7 File Offset: 0x00002CD7
		public float GetCameraDisplacementDistance(ICinemachineCamera vcam)
		{
			return base.GetExtraState<CinemachineCollider.VcamExtraState>(vcam).previousDisplacement.magnitude;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004AEC File Offset: 0x00002CEC
		private void OnValidate()
		{
			this.m_DistanceLimit = Mathf.Max(0f, this.m_DistanceLimit);
			this.m_MinimumOcclusionTime = Mathf.Max(0f, this.m_MinimumOcclusionTime);
			this.m_CameraRadius = Mathf.Max(0f, this.m_CameraRadius);
			this.m_MinimumDistanceFromTarget = Mathf.Max(0.01f, this.m_MinimumDistanceFromTarget);
			this.m_OptimalTargetDistance = Mathf.Max(0f, this.m_OptimalTargetDistance);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004B67 File Offset: 0x00002D67
		protected override void OnDestroy()
		{
			RuntimeUtility.DestroyScratchCollider();
			base.OnDestroy();
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004B74 File Offset: 0x00002D74
		public List<List<Vector3>> DebugPaths
		{
			get
			{
				List<List<Vector3>> list = new List<List<Vector3>>();
				foreach (CinemachineCollider.VcamExtraState vcamExtraState in base.GetAllExtraStates<CinemachineCollider.VcamExtraState>())
				{
					if (vcamExtraState.debugResolutionPath != null && vcamExtraState.debugResolutionPath.Count > 0)
					{
						list.Add(vcamExtraState.debugResolutionPath);
					}
				}
				return list;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004BEC File Offset: 0x00002DEC
		public override float GetMaxDampTime()
		{
			return Mathf.Max(this.m_Damping, Mathf.Max(this.m_DampingWhenOccluded, this.m_SmoothingTime));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004C0C File Offset: 0x00002E0C
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			List<CinemachineCollider.VcamExtraState> allExtraStates = base.GetAllExtraStates<CinemachineCollider.VcamExtraState>();
			for (int i = 0; i < allExtraStates.Count; i++)
			{
				allExtraStates[i].previousCameraPosition += positionDelta;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004C4C File Offset: 0x00002E4C
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Body)
			{
				CinemachineCollider.VcamExtraState extraState = base.GetExtraState<CinemachineCollider.VcamExtraState>(vcam);
				extraState.targetObscured = false;
				List<Vector3> debugResolutionPath = extraState.debugResolutionPath;
				if (debugResolutionPath != null)
				{
					debugResolutionPath.RemoveRange(0, extraState.debugResolutionPath.Count);
				}
				if (this.m_AvoidObstacles)
				{
					Vector3 correctedPosition = state.CorrectedPosition;
					Quaternion quaternion = Quaternion.Euler(state.PositionDampingBypass);
					extraState.previousDisplacement = quaternion * extraState.previousDisplacement;
					Vector3 vector = this.PreserveLineOfSight(ref state, ref extraState);
					if (this.m_MinimumOcclusionTime > 0.0001f)
					{
						float currentTime = CinemachineCore.CurrentTime;
						if (vector.AlmostZero())
						{
							extraState.occlusionStartTime = 0f;
						}
						else
						{
							if (extraState.occlusionStartTime <= 0f)
							{
								extraState.occlusionStartTime = currentTime;
							}
							if (currentTime - extraState.occlusionStartTime < this.m_MinimumOcclusionTime)
							{
								vector = extraState.previousDisplacement;
							}
						}
					}
					if (this.m_SmoothingTime > 0.0001f && state.HasLookAt)
					{
						Vector3 vector2 = correctedPosition + vector;
						Vector3 vector3 = vector2 - state.ReferenceLookAt;
						float num = vector3.magnitude;
						if (num > 0.0001f)
						{
							vector3 /= num;
							if (!vector.AlmostZero())
							{
								extraState.UpdateDistanceSmoothing(num);
							}
							num = extraState.ApplyDistanceSmoothing(num, this.m_SmoothingTime);
							vector += state.ReferenceLookAt + vector3 * num - vector2;
						}
					}
					if (vector.AlmostZero())
					{
						extraState.ResetDistanceSmoothing(this.m_SmoothingTime);
					}
					Vector3 vector4 = correctedPosition + vector;
					Vector3 vector5 = (state.HasLookAt ? state.ReferenceLookAt : vector4);
					vector += this.RespectCameraRadius(vector4, vector5);
					float num2 = this.m_DampingWhenOccluded;
					if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid && this.m_DampingWhenOccluded + this.m_Damping > 0.0001f)
					{
						float sqrMagnitude = vector.sqrMagnitude;
						num2 = ((sqrMagnitude > extraState.previousDisplacement.sqrMagnitude) ? this.m_DampingWhenOccluded : this.m_Damping);
						if (sqrMagnitude < 0.0001f)
						{
							num2 = extraState.previousDampTime - Damper.Damp(extraState.previousDampTime, num2, deltaTime);
						}
						if (num2 > 0f)
						{
							bool flag = false;
							if (vcam is CinemachineVirtualCamera)
							{
								CinemachineComponentBase cinemachineComponent = (vcam as CinemachineVirtualCamera).GetCinemachineComponent(CinemachineCore.Stage.Body);
								flag = cinemachineComponent != null && cinemachineComponent.BodyAppliesAfterAim;
							}
							Vector3 vector6 = (flag ? extraState.previousDisplacement : (vector5 + quaternion * extraState.previousCameraOffset - correctedPosition));
							vector = vector6 + Damper.Damp(vector - vector6, num2, deltaTime);
						}
					}
					state.PositionCorrection += vector;
					vector4 = state.CorrectedPosition;
					if (state.HasLookAt && base.VirtualCamera.PreviousStateIsValid)
					{
						Vector3 vector7 = extraState.previousCameraPosition - state.ReferenceLookAt;
						Vector3 vector8 = vector4 - state.ReferenceLookAt;
						if (vector7.sqrMagnitude > 0.0001f && vector8.sqrMagnitude > 0.0001f)
						{
							state.PositionDampingBypass = UnityVectorExtensions.SafeFromToRotation(vector7, vector8, state.ReferenceUp).eulerAngles;
						}
					}
					extraState.previousDisplacement = vector;
					extraState.previousCameraOffset = vector4 - vector5;
					extraState.previousCameraPosition = vector4;
					extraState.previousDampTime = num2;
				}
			}
			if (stage == CinemachineCore.Stage.Aim)
			{
				CinemachineCollider.VcamExtraState extraState2 = base.GetExtraState<CinemachineCollider.VcamExtraState>(vcam);
				extraState2.targetObscured = CinemachineCollider.IsTargetOffscreen(state) || this.CheckForTargetObstructions(state);
				if (extraState2.targetObscured)
				{
					state.ShotQuality *= 0.2f;
				}
				if (!extraState2.previousDisplacement.AlmostZero())
				{
					state.ShotQuality *= 0.8f;
				}
				float num3 = 0f;
				if (this.m_OptimalTargetDistance > 0f && state.HasLookAt)
				{
					float num4 = Vector3.Magnitude(state.ReferenceLookAt - state.FinalPosition);
					if (num4 <= this.m_OptimalTargetDistance)
					{
						float num5 = this.m_OptimalTargetDistance / 2f;
						if (num4 >= num5)
						{
							num3 = 0.2f * (num4 - num5) / (this.m_OptimalTargetDistance - num5);
						}
					}
					else
					{
						num4 -= this.m_OptimalTargetDistance;
						float num6 = this.m_OptimalTargetDistance * 3f;
						if (num4 < num6)
						{
							num3 = 0.2f * (1f - num4 / num6);
						}
					}
					state.ShotQuality *= 1f + num3;
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000050B0 File Offset: 0x000032B0
		private Vector3 PreserveLineOfSight(ref CameraState state, ref CinemachineCollider.VcamExtraState extra)
		{
			Vector3 vector = Vector3.zero;
			if (state.HasLookAt && this.m_CollideAgainst != 0 && this.m_CollideAgainst != this.m_TransparentLayers)
			{
				Vector3 correctedPosition = state.CorrectedPosition;
				Vector3 referenceLookAt = state.ReferenceLookAt;
				RaycastHit raycastHit = default(RaycastHit);
				vector = this.PullCameraInFrontOfNearestObstacle(correctedPosition, referenceLookAt, this.m_CollideAgainst & ~this.m_TransparentLayers, ref raycastHit);
				Vector3 vector2 = correctedPosition + vector;
				if (raycastHit.collider != null)
				{
					extra.AddPointToDebugPath(vector2);
					if (this.m_Strategy != CinemachineCollider.ResolutionStrategy.PullCameraForward)
					{
						Vector3 vector3 = correctedPosition - referenceLookAt;
						vector2 = this.PushCameraBack(vector2, vector3, raycastHit, referenceLookAt, new Plane(state.ReferenceUp, correctedPosition), vector3.magnitude, this.m_MaximumEffort, ref extra);
					}
				}
				vector = vector2 - correctedPosition;
			}
			return vector;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005198 File Offset: 0x00003398
		private Vector3 PullCameraInFrontOfNearestObstacle(Vector3 cameraPos, Vector3 lookAtPos, int layerMask, ref RaycastHit hitInfo)
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = cameraPos - lookAtPos;
			float magnitude = vector2.magnitude;
			if (magnitude > 0.0001f)
			{
				vector2 /= magnitude;
				float num = Mathf.Max(this.m_MinimumDistanceFromTarget, 0.0001f);
				if (magnitude < num + 0.0001f)
				{
					vector = vector2 * (num - magnitude);
				}
				else
				{
					float num2 = magnitude - num;
					if (this.m_DistanceLimit > 0.0001f)
					{
						num2 = Mathf.Min(this.m_DistanceLimit, num2);
					}
					Ray ray = new Ray(cameraPos - num2 * vector2, vector2);
					num2 += 0.001f;
					if (num2 > 0.0001f)
					{
						if (this.m_Strategy == CinemachineCollider.ResolutionStrategy.PullCameraForward && this.m_CameraRadius >= 0.0001f)
						{
							if (RuntimeUtility.SphereCastIgnoreTag(lookAtPos + vector2 * this.m_CameraRadius, this.m_CameraRadius, vector2, out hitInfo, num2 - this.m_CameraRadius, layerMask, in this.m_IgnoreTag))
							{
								vector = hitInfo.point + hitInfo.normal * this.m_CameraRadius - cameraPos;
							}
						}
						else if (RuntimeUtility.RaycastIgnoreTag(ray, out hitInfo, num2, layerMask, in this.m_IgnoreTag))
						{
							float num3 = Mathf.Max(0f, hitInfo.distance - 0.001f);
							vector = ray.GetPoint(num3) - cameraPos;
						}
					}
				}
			}
			return vector;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000052F4 File Offset: 0x000034F4
		private Vector3 PushCameraBack(Vector3 currentPos, Vector3 pushDir, RaycastHit obstacle, Vector3 lookAtPos, Plane startPlane, float targetDistance, int iterations, ref CinemachineCollider.VcamExtraState extra)
		{
			Vector3 vector = Vector3.zero;
			if (!this.GetWalkingDirection(currentPos, pushDir, obstacle, ref vector))
			{
				return currentPos;
			}
			Ray ray = new Ray(currentPos, vector);
			float num = this.GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
			if (num <= 0.0001f)
			{
				return currentPos;
			}
			float num2 = CinemachineCollider.ClampRayToBounds(ray, num, obstacle.collider.bounds);
			num = Mathf.Min(num, num2 + 0.001f);
			RaycastHit raycastHit;
			Vector3 vector2;
			if (RuntimeUtility.RaycastIgnoreTag(ray, out raycastHit, num, this.m_CollideAgainst & ~this.m_TransparentLayers, in this.m_IgnoreTag))
			{
				float num3 = raycastHit.distance - 0.001f;
				vector2 = ray.GetPoint(num3);
				extra.AddPointToDebugPath(vector2);
				if (iterations > 1)
				{
					vector2 = this.PushCameraBack(vector2, vector, raycastHit, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
				}
				return vector2;
			}
			vector2 = ray.GetPoint(num);
			vector = vector2 - lookAtPos;
			float magnitude = vector.magnitude;
			RaycastHit raycastHit2;
			if (magnitude < 0.0001f || RuntimeUtility.RaycastIgnoreTag(new Ray(lookAtPos, vector), out raycastHit2, magnitude - 0.001f, this.m_CollideAgainst & ~this.m_TransparentLayers, in this.m_IgnoreTag))
			{
				return currentPos;
			}
			ray = new Ray(vector2, vector);
			extra.AddPointToDebugPath(vector2);
			num = this.GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
			if (num > 0.0001f)
			{
				if (!RuntimeUtility.RaycastIgnoreTag(ray, out raycastHit, num, this.m_CollideAgainst & ~this.m_TransparentLayers, in this.m_IgnoreTag))
				{
					vector2 = ray.GetPoint(num);
					extra.AddPointToDebugPath(vector2);
				}
				else
				{
					float num4 = raycastHit.distance - 0.001f;
					vector2 = ray.GetPoint(num4);
					extra.AddPointToDebugPath(vector2);
					if (iterations > 1)
					{
						vector2 = this.PushCameraBack(vector2, vector, raycastHit, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
					}
				}
			}
			return vector2;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000054C8 File Offset: 0x000036C8
		private bool GetWalkingDirection(Vector3 pos, Vector3 pushDir, RaycastHit obstacle, ref Vector3 outDir)
		{
			Vector3 vector = obstacle.normal;
			float num = 0.0050000004f;
			int num2 = Physics.SphereCastNonAlloc(pos, num, pushDir.normalized, this.m_CornerBuffer, 0f, this.m_CollideAgainst & ~this.m_TransparentLayers, QueryTriggerInteraction.Ignore);
			if (num2 > 1)
			{
				for (int i = 0; i < num2; i++)
				{
					if (!(this.m_CornerBuffer[i].collider == null) && (this.m_IgnoreTag.Length <= 0 || !this.m_CornerBuffer[i].collider.CompareTag(this.m_IgnoreTag)))
					{
						Type type = this.m_CornerBuffer[i].collider.GetType();
						if (type == typeof(BoxCollider) || type == typeof(SphereCollider) || type == typeof(CapsuleCollider))
						{
							Vector3 vector2 = this.m_CornerBuffer[i].collider.ClosestPoint(pos) - pos;
							if (vector2.magnitude > 1E-05f && this.m_CornerBuffer[i].collider.Raycast(new Ray(pos, vector2), out this.m_CornerBuffer[i], num))
							{
								if (!(this.m_CornerBuffer[i].normal - obstacle.normal).AlmostZero())
								{
									vector = this.m_CornerBuffer[i].normal;
									break;
								}
								break;
							}
						}
					}
				}
			}
			Vector3 vector3 = Vector3.Cross(obstacle.normal, vector);
			if (vector3.AlmostZero())
			{
				vector3 = Vector3.ProjectOnPlane(pushDir, obstacle.normal);
			}
			else
			{
				float num3 = Vector3.Dot(vector3, pushDir);
				if (Mathf.Abs(num3) < 0.0001f)
				{
					return false;
				}
				if (num3 < 0f)
				{
					vector3 = -vector3;
				}
			}
			if (vector3.AlmostZero())
			{
				return false;
			}
			outDir = vector3.normalized;
			return true;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000056D4 File Offset: 0x000038D4
		private float GetPushBackDistance(Ray ray, Plane startPlane, float targetDistance, Vector3 lookAtPos)
		{
			float num = targetDistance - (ray.origin - lookAtPos).magnitude;
			if (num < 0.0001f)
			{
				return 0f;
			}
			if (this.m_Strategy == CinemachineCollider.ResolutionStrategy.PreserveCameraDistance)
			{
				return num;
			}
			float num2;
			if (!startPlane.Raycast(ray, out num2))
			{
				num2 = 0f;
			}
			num2 = Mathf.Min(num, num2);
			if (num2 < 0.0001f)
			{
				return 0f;
			}
			float num3 = Mathf.Abs(UnityVectorExtensions.Angle(startPlane.normal, ray.direction) - 90f);
			if (num3 < 0.1f)
			{
				num2 = Mathf.Lerp(0f, num2, num3 / 0.1f);
			}
			return num2;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005778 File Offset: 0x00003978
		private static float ClampRayToBounds(Ray ray, float distance, Bounds bounds)
		{
			float num;
			if (Vector3.Dot(ray.direction, Vector3.up) > 0f)
			{
				if (new Plane(Vector3.down, bounds.max).Raycast(ray, out num) && num > 0.0001f)
				{
					distance = Mathf.Min(distance, num);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.down) > 0f && new Plane(Vector3.up, bounds.min).Raycast(ray, out num) && num > 0.0001f)
			{
				distance = Mathf.Min(distance, num);
			}
			if (Vector3.Dot(ray.direction, Vector3.right) > 0f)
			{
				if (new Plane(Vector3.left, bounds.max).Raycast(ray, out num) && num > 0.0001f)
				{
					distance = Mathf.Min(distance, num);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.left) > 0f && new Plane(Vector3.right, bounds.min).Raycast(ray, out num) && num > 0.0001f)
			{
				distance = Mathf.Min(distance, num);
			}
			if (Vector3.Dot(ray.direction, Vector3.forward) > 0f)
			{
				if (new Plane(Vector3.back, bounds.max).Raycast(ray, out num) && num > 0.0001f)
				{
					distance = Mathf.Min(distance, num);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.back) > 0f && new Plane(Vector3.forward, bounds.min).Raycast(ray, out num) && num > 0.0001f)
			{
				distance = Mathf.Min(distance, num);
			}
			return distance;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005938 File Offset: 0x00003B38
		private Vector3 RespectCameraRadius(Vector3 cameraPos, Vector3 lookAtPos)
		{
			Vector3 vector = Vector3.zero;
			if (this.m_CameraRadius < 0.0001f || this.m_CollideAgainst == 0)
			{
				return vector;
			}
			Vector3 vector2 = cameraPos - lookAtPos;
			float magnitude = vector2.magnitude;
			if (magnitude > 0.0001f)
			{
				vector2 /= magnitude;
			}
			int num = Physics.OverlapSphereNonAlloc(cameraPos, this.m_CameraRadius, CinemachineCollider.s_ColliderBuffer, this.m_CollideAgainst, QueryTriggerInteraction.Ignore);
			if (num == 0 && this.m_TransparentLayers != 0 && magnitude > this.m_MinimumDistanceFromTarget + 0.0001f)
			{
				float num2 = magnitude - this.m_MinimumDistanceFromTarget;
				RaycastHit raycastHit;
				if (RuntimeUtility.RaycastIgnoreTag(new Ray(lookAtPos + vector2 * this.m_MinimumDistanceFromTarget, vector2), out raycastHit, num2, this.m_CollideAgainst, in this.m_IgnoreTag))
				{
					Collider collider = raycastHit.collider;
					if (!collider.Raycast(new Ray(cameraPos, -vector2), out raycastHit, num2))
					{
						CinemachineCollider.s_ColliderBuffer[num++] = collider;
					}
				}
			}
			if ((num > 0 && magnitude == 0f) || magnitude > this.m_MinimumDistanceFromTarget)
			{
				SphereCollider scratchCollider = RuntimeUtility.GetScratchCollider();
				scratchCollider.radius = this.m_CameraRadius;
				Vector3 vector3 = cameraPos;
				for (int i = 0; i < num; i++)
				{
					Collider collider2 = CinemachineCollider.s_ColliderBuffer[i];
					if (this.m_IgnoreTag.Length <= 0 || !collider2.CompareTag(this.m_IgnoreTag))
					{
						if (magnitude > this.m_MinimumDistanceFromTarget)
						{
							vector2 = vector3 - lookAtPos;
							float magnitude2 = vector2.magnitude;
							if (magnitude2 > 0.0001f)
							{
								vector2 /= magnitude2;
								Ray ray = new Ray(lookAtPos, vector2);
								RaycastHit raycastHit;
								if (collider2.Raycast(ray, out raycastHit, magnitude2 + this.m_CameraRadius))
								{
									vector3 = ray.GetPoint(raycastHit.distance) - vector2 * 0.001f;
								}
							}
						}
						Vector3 vector4;
						float num3;
						if (Physics.ComputePenetration(scratchCollider, vector3, Quaternion.identity, collider2, collider2.transform.position, collider2.transform.rotation, out vector4, out num3))
						{
							vector3 += vector4 * num3;
						}
					}
				}
				vector = vector3 - cameraPos;
			}
			if (magnitude > 0.0001f && this.m_MinimumDistanceFromTarget > 0.0001f)
			{
				float num4 = Mathf.Max(this.m_MinimumDistanceFromTarget, this.m_CameraRadius) + 0.001f;
				if ((cameraPos + vector - lookAtPos).magnitude < num4)
				{
					vector = lookAtPos - cameraPos + vector2 * num4;
				}
			}
			return vector;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005BBC File Offset: 0x00003DBC
		private bool CheckForTargetObstructions(CameraState state)
		{
			if (state.HasLookAt)
			{
				Vector3 referenceLookAt = state.ReferenceLookAt;
				Vector3 correctedPosition = state.CorrectedPosition;
				Vector3 vector = referenceLookAt - correctedPosition;
				float magnitude = vector.magnitude;
				if (magnitude < Mathf.Max(this.m_MinimumDistanceFromTarget, 0.0001f))
				{
					return true;
				}
				RaycastHit raycastHit;
				if (RuntimeUtility.RaycastIgnoreTag(new Ray(correctedPosition, vector.normalized), out raycastHit, magnitude - this.m_MinimumDistanceFromTarget, this.m_CollideAgainst & ~this.m_TransparentLayers, in this.m_IgnoreTag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005C44 File Offset: 0x00003E44
		private static bool IsTargetOffscreen(CameraState state)
		{
			if (state.HasLookAt)
			{
				Vector3 vector = state.ReferenceLookAt - state.CorrectedPosition;
				vector = Quaternion.Inverse(state.CorrectedOrientation) * vector;
				if (state.Lens.Orthographic)
				{
					if (Mathf.Abs(vector.y) > state.Lens.OrthographicSize)
					{
						return true;
					}
					if (Mathf.Abs(vector.x) > state.Lens.OrthographicSize * state.Lens.Aspect)
					{
						return true;
					}
				}
				else
				{
					float num = state.Lens.FieldOfView / 2f;
					if (UnityVectorExtensions.Angle(vector.ProjectOntoPlane(Vector3.right), Vector3.forward) > num)
					{
						return true;
					}
					num = 57.29578f * Mathf.Atan(Mathf.Tan(num * 0.017453292f) * state.Lens.Aspect);
					if (UnityVectorExtensions.Angle(vector.ProjectOntoPlane(Vector3.up), Vector3.forward) > num)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000050 RID: 80
		[Header("Obstacle Detection")]
		[Tooltip("Objects on these layers will be detected")]
		public LayerMask m_CollideAgainst = 1;

		// Token: 0x04000051 RID: 81
		[TagField]
		[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		public string m_IgnoreTag = string.Empty;

		// Token: 0x04000052 RID: 82
		[Tooltip("Objects on these layers will never obstruct view of the target")]
		public LayerMask m_TransparentLayers = 0;

		// Token: 0x04000053 RID: 83
		[Tooltip("Obstacles closer to the target than this will be ignored")]
		public float m_MinimumDistanceFromTarget = 0.1f;

		// Token: 0x04000054 RID: 84
		[Space]
		[Tooltip("When enabled, will attempt to resolve situations where the line of sight to the target is blocked by an obstacle")]
		[FormerlySerializedAs("m_PreserveLineOfSight")]
		public bool m_AvoidObstacles = true;

		// Token: 0x04000055 RID: 85
		[Tooltip("The maximum raycast distance when checking if the line of sight to this camera's target is clear.  If the setting is 0 or less, the current actual distance to target will be used.")]
		[FormerlySerializedAs("m_LineOfSightFeelerDistance")]
		public float m_DistanceLimit;

		// Token: 0x04000056 RID: 86
		[Tooltip("Don't take action unless occlusion has lasted at least this long.")]
		public float m_MinimumOcclusionTime;

		// Token: 0x04000057 RID: 87
		[Tooltip("Camera will try to maintain this distance from any obstacle.  Try to keep this value small.  Increase it if you are seeing inside obstacles due to a large FOV on the camera.")]
		public float m_CameraRadius = 0.1f;

		// Token: 0x04000058 RID: 88
		[Tooltip("The way in which the Collider will attempt to preserve sight of the target.")]
		public CinemachineCollider.ResolutionStrategy m_Strategy = CinemachineCollider.ResolutionStrategy.PreserveCameraHeight;

		// Token: 0x04000059 RID: 89
		[Range(1f, 10f)]
		[Tooltip("Upper limit on how many obstacle hits to process.  Higher numbers may impact performance.  In most environments, 4 is enough.")]
		public int m_MaximumEffort = 4;

		// Token: 0x0400005A RID: 90
		[Range(0f, 2f)]
		[Tooltip("Smoothing to apply to obstruction resolution.  Nearest camera point is held for at least this long")]
		public float m_SmoothingTime;

		// Token: 0x0400005B RID: 91
		[Range(0f, 10f)]
		[Tooltip("How gradually the camera returns to its normal position after having been corrected.  Higher numbers will move the camera more gradually back to normal.")]
		[FormerlySerializedAs("m_Smoothing")]
		public float m_Damping;

		// Token: 0x0400005C RID: 92
		[Range(0f, 10f)]
		[Tooltip("How gradually the camera moves to resolve an occlusion.  Higher numbers will move the camera more gradually.")]
		public float m_DampingWhenOccluded;

		// Token: 0x0400005D RID: 93
		[Header("Shot Evaluation")]
		[Tooltip("If greater than zero, a higher score will be given to shots when the target is closer to this distance.  Set this to zero to disable this feature.")]
		public float m_OptimalTargetDistance;

		// Token: 0x0400005E RID: 94
		private const float k_PrecisionSlush = 0.001f;

		// Token: 0x0400005F RID: 95
		private RaycastHit[] m_CornerBuffer = new RaycastHit[4];

		// Token: 0x04000060 RID: 96
		private const float k_AngleThreshold = 0.1f;

		// Token: 0x04000061 RID: 97
		private static Collider[] s_ColliderBuffer = new Collider[5];

		// Token: 0x02000077 RID: 119
		public enum ResolutionStrategy
		{
			// Token: 0x040002C5 RID: 709
			PullCameraForward,
			// Token: 0x040002C6 RID: 710
			PreserveCameraHeight,
			// Token: 0x040002C7 RID: 711
			PreserveCameraDistance
		}

		// Token: 0x02000078 RID: 120
		private class VcamExtraState
		{
			// Token: 0x06000418 RID: 1048 RVA: 0x000188D8 File Offset: 0x00016AD8
			public void AddPointToDebugPath(Vector3 p)
			{
			}

			// Token: 0x06000419 RID: 1049 RVA: 0x000188DA File Offset: 0x00016ADA
			public float ApplyDistanceSmoothing(float distance, float smoothingTime)
			{
				if (this.m_SmoothedTime != 0f && smoothingTime > 0.0001f && CinemachineCore.CurrentTime - this.m_SmoothedTime < smoothingTime)
				{
					return Mathf.Min(distance, this.m_SmoothedDistance);
				}
				return distance;
			}

			// Token: 0x0600041A RID: 1050 RVA: 0x0001890E File Offset: 0x00016B0E
			public void UpdateDistanceSmoothing(float distance)
			{
				if (this.m_SmoothedDistance == 0f || distance < this.m_SmoothedDistance)
				{
					this.m_SmoothedDistance = distance;
					this.m_SmoothedTime = CinemachineCore.CurrentTime;
				}
			}

			// Token: 0x0600041B RID: 1051 RVA: 0x00018938 File Offset: 0x00016B38
			public void ResetDistanceSmoothing(float smoothingTime)
			{
				if (CinemachineCore.CurrentTime - this.m_SmoothedTime >= smoothingTime)
				{
					this.m_SmoothedDistance = (this.m_SmoothedTime = 0f);
				}
			}

			// Token: 0x040002C8 RID: 712
			public Vector3 previousDisplacement;

			// Token: 0x040002C9 RID: 713
			public Vector3 previousCameraOffset;

			// Token: 0x040002CA RID: 714
			public Vector3 previousCameraPosition;

			// Token: 0x040002CB RID: 715
			public float previousDampTime;

			// Token: 0x040002CC RID: 716
			public bool targetObscured;

			// Token: 0x040002CD RID: 717
			public float occlusionStartTime;

			// Token: 0x040002CE RID: 718
			public List<Vector3> debugResolutionPath;

			// Token: 0x040002CF RID: 719
			private float m_SmoothedDistance;

			// Token: 0x040002D0 RID: 720
			private float m_SmoothedTime;
		}
	}
}
