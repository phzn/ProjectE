using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Detection
{
	// Token: 0x0200124C RID: 4684
	[DisallowMultipleComponent]
	public sealed class DetectionShotProtection : MonoBehaviour
	{
		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600919E RID: 37278 RVA: 0x003B9B6C File Offset: 0x003B7D6C
		public bool IsActive
		{
			get
			{
				return this._active && base.isActiveAndEnabled;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600919F RID: 37279 RVA: 0x003B9B7E File Offset: 0x003B7D7E
		public int HiddenRendererCount
		{
			get
			{
				return this._hidden.Count;
			}
		}

		// Token: 0x060091A0 RID: 37280 RVA: 0x003B9B8C File Offset: 0x003B7D8C
		public void Begin(Camera outputCamera)
		{
			if (outputCamera == null)
			{
				return;
			}
			if (this._output != outputCamera || this._visibility == null)
			{
				DetectionRenderVisibilityScope visibility = this._visibility;
				if (visibility != null)
				{
					visibility.Dispose();
				}
				this._output = outputCamera;
				this._visibility = new DetectionRenderVisibilityScope(outputCamera);
			}
			if (!this._active)
			{
				this._nextActorRefresh = 0f;
				foreach (DetectionShotCameraExtension detectionShotCameraExtension in this._shots.Values)
				{
					if (detectionShotCameraExtension != null)
					{
						detectionShotCameraExtension.ResetShot();
					}
				}
			}
			this._active = true;
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
			CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
			this.RefreshActors();
		}

		// Token: 0x060091A1 RID: 37281 RVA: 0x003B9C7C File Offset: 0x003B7E7C
		public void RegisterCamera(CinemachineVirtualCamera camera, Transform subject, bool preserveSubjectCloseUp = false)
		{
			if (camera == null || subject == null)
			{
				return;
			}
			DetectionShotCameraExtension detectionShotCameraExtension = camera.GetComponent<DetectionShotCameraExtension>();
			if (detectionShotCameraExtension == null)
			{
				detectionShotCameraExtension = camera.gameObject.AddComponent<DetectionShotCameraExtension>();
				detectionShotCameraExtension.hideFlags = HideFlags.DontSave;
			}
			detectionShotCameraExtension.Owner = this;
			detectionShotCameraExtension.Subject = subject;
			detectionShotCameraExtension.PreserveSubjectCloseUp = preserveSubjectCloseUp;
			detectionShotCameraExtension.enabled = true;
			detectionShotCameraExtension.ResetShot();
			this._shots[camera] = detectionShotCameraExtension;
			this.CacheSubject(subject);
			this._hidden.Clear();
			DetectionRenderVisibilityScope visibility = this._visibility;
			if (visibility == null)
			{
				return;
			}
			visibility.Restore();
		}

		// Token: 0x060091A2 RID: 37282 RVA: 0x003B9D14 File Offset: 0x003B7F14
		public void IncludeActor(Transform root)
		{
			if (root == null || !this._actorRoots.Add(root))
			{
				return;
			}
			DetectionShotProtection.Actor actor = new DetectionShotProtection.Actor
			{
				Root = root
			};
			root.GetComponentsInChildren<Renderer>(true, actor.Renderers);
			this._actors.Add(actor);
		}

		// Token: 0x060091A3 RID: 37283 RVA: 0x003B9D60 File Offset: 0x003B7F60
		public void IncludeProtectedActor(Transform root)
		{
			if (root == null)
			{
				return;
			}
			this.IncludeActor(root);
			foreach (DetectionShotProtection.Actor actor in this._actors)
			{
				if (actor.Root != null && DetectionShotProtection.SameActor(actor.Root, root))
				{
					actor.CanHide = false;
				}
			}
			this.CacheSubject(root);
		}

		// Token: 0x060091A4 RID: 37284 RVA: 0x003B9DE8 File Offset: 0x003B7FE8
		public void Suspend()
		{
			this._active = false;
			foreach (DetectionShotCameraExtension detectionShotCameraExtension in this._shots.Values)
			{
				if (detectionShotCameraExtension != null)
				{
					detectionShotCameraExtension.ResetShot();
				}
			}
			this._hidden.Clear();
			DetectionRenderVisibilityScope visibility = this._visibility;
			if (visibility != null)
			{
				visibility.Restore();
			}
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
		}

		// Token: 0x060091A5 RID: 37285 RVA: 0x003B9E84 File Offset: 0x003B8084
		public void EndProtection()
		{
			this.Suspend();
			DetectionRenderVisibilityScope visibility = this._visibility;
			if (visibility != null)
			{
				visibility.Dispose();
			}
			this._visibility = null;
			this._output = null;
			foreach (DetectionShotCameraExtension detectionShotCameraExtension in this._shots.Values)
			{
				if (!(detectionShotCameraExtension == null) && !(detectionShotCameraExtension.Owner != this))
				{
					detectionShotCameraExtension.Owner = null;
					detectionShotCameraExtension.Subject = null;
					detectionShotCameraExtension.PreserveSubjectCloseUp = false;
					detectionShotCameraExtension.ResetShot();
				}
			}
			this._shots.Clear();
			this._subjects.Clear();
			this._actors.Clear();
			this._actorRoots.Clear();
		}

		// Token: 0x060091A6 RID: 37286 RVA: 0x003B9F58 File Offset: 0x003B8158
		private void OnDisable()
		{
			this.EndProtection();
		}

		// Token: 0x060091A7 RID: 37287 RVA: 0x003B9F58 File Offset: 0x003B8158
		private void OnDestroy()
		{
			this.EndProtection();
		}

		// Token: 0x060091A8 RID: 37288 RVA: 0x003B9F60 File Offset: 0x003B8160
		private void RefreshActors()
		{
			if (Time.unscaledTime < this._nextActorRefresh)
			{
				return;
			}
			this._nextActorRefresh = Time.unscaledTime + 0.5f;
			for (int i = this._actors.Count - 1; i >= 0; i--)
			{
				if (this._actors[i].Root == null)
				{
					this._actorRoots.Remove(this._actors[i].Root);
					this._actors.RemoveAt(i);
				}
			}
			if (SensoryManager.Instance != null)
			{
				IReadOnlyList<NPCSenses> registeredNPCs = SensoryManager.Instance.GetRegisteredNPCs();
				for (int j = 0; j < registeredNPCs.Count; j++)
				{
					if (registeredNPCs[j] != null)
					{
						this.IncludeActor(registeredNPCs[j].transform);
					}
				}
			}
			foreach (DetectionShotProtection.Actor actor in this._actors)
			{
				if (actor.Root != null && actor.Root.gameObject.activeInHierarchy && this._output != null && (actor.Root.position - this._output.transform.position).sqrMagnitude < 900f)
				{
					actor.Root.GetComponentsInChildren<Renderer>(true, actor.Renderers);
				}
			}
		}

		// Token: 0x060091A9 RID: 37289 RVA: 0x003BA0E8 File Offset: 0x003B82E8
		private void CacheSubject(Transform root)
		{
			if (this._subjects.ContainsKey(root))
			{
				return;
			}
			DetectionShotProtection.SubjectPoints subjectPoints = new DetectionShotProtection.SubjectPoints
			{
				Root = root,
				Renderers = root.GetComponentsInChildren<Renderer>(true)
			};
			Animator componentInChildren = root.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isHuman && componentInChildren.avatar != null && componentInChildren.avatar.isValid)
			{
				subjectPoints.Head = componentInChildren.GetBoneTransform(HumanBodyBones.Head);
				subjectPoints.Chest = componentInChildren.GetBoneTransform(HumanBodyBones.Chest);
				subjectPoints.Hips = componentInChildren.GetBoneTransform(HumanBodyBones.Hips);
				subjectPoints.LeftShoulder = componentInChildren.GetBoneTransform(HumanBodyBones.LeftUpperArm);
				subjectPoints.RightShoulder = componentInChildren.GetBoneTransform(HumanBodyBones.RightUpperArm);
				subjectPoints.LeftFoot = componentInChildren.GetBoneTransform(HumanBodyBones.LeftFoot);
				subjectPoints.RightFoot = componentInChildren.GetBoneTransform(HumanBodyBones.RightFoot);
			}
			this._subjects.Add(root, subjectPoints);
		}

		// Token: 0x060091AA RID: 37290 RVA: 0x003BA1C0 File Offset: 0x003B83C0
		private int GetSubjectPoints(Transform root)
		{
			this.CacheSubject(root);
			DetectionShotProtection.SubjectPoints subjectPoints = this._subjects[root];
			if (subjectPoints.Head != null && subjectPoints.Hips != null)
			{
				this._points[0] = subjectPoints.Head.position;
				this._points[1] = ((subjectPoints.Chest != null) ? subjectPoints.Chest.position : Vector3.Lerp(subjectPoints.Head.position, subjectPoints.Hips.position, 0.55f));
				this._points[2] = subjectPoints.Hips.position;
				this._points[3] = ((subjectPoints.LeftShoulder != null) ? subjectPoints.LeftShoulder.position : (this._points[1] - root.right * 0.15f));
				this._points[4] = ((subjectPoints.RightShoulder != null) ? subjectPoints.RightShoulder.position : (this._points[1] + root.right * 0.15f));
				this._points[5] = ((subjectPoints.LeftFoot != null) ? subjectPoints.LeftFoot.position : root.position);
				this._points[6] = ((subjectPoints.RightFoot != null) ? subjectPoints.RightFoot.position : root.position);
				return 7;
			}
			bool flag = false;
			Bounds bounds = new Bounds(root.position + Vector3.up * 0.9f, new Vector3(0.5f, 1.8f, 0.4f));
			foreach (Renderer renderer in subjectPoints.Renderers)
			{
				if (DetectionShotProtection.IsModelRenderer(renderer))
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			this._points[0] = bounds.center + Vector3.up * bounds.extents.y * 0.8f;
			this._points[1] = bounds.center;
			this._points[2] = bounds.center - Vector3.up * bounds.extents.y * 0.7f;
			this._points[3] = bounds.center - root.right * bounds.extents.x * 0.65f;
			this._points[4] = bounds.center + root.right * bounds.extents.x * 0.65f;
			return 5;
		}

		// Token: 0x060091AB RID: 37291 RVA: 0x003BA4D6 File Offset: 0x003B86D6
		private static bool IsModelRenderer(Renderer renderer)
		{
			return renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy && (renderer is SkinnedMeshRenderer || renderer is MeshRenderer);
		}

		// Token: 0x060091AC RID: 37292 RVA: 0x003BA50B File Offset: 0x003B870B
		private static bool SameActor(Transform a, Transform b)
		{
			return a == b || a.IsChildOf(b) || b.IsChildOf(a);
		}

		// Token: 0x060091AD RID: 37293 RVA: 0x003BA528 File Offset: 0x003B8728
		private bool IsCharacter(Collider collider, Transform subject)
		{
			if (DetectionShotProtection.SameActor(collider.transform, subject))
			{
				return true;
			}
			foreach (Transform transform in this._subjects.Keys)
			{
				if (transform != null && DetectionShotProtection.SameActor(collider.transform, transform))
				{
					return true;
				}
			}
			foreach (DetectionShotProtection.Actor actor in this._actors)
			{
				if (actor.Root != null && DetectionShotProtection.SameActor(collider.transform, actor.Root))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060091AE RID: 37294 RVA: 0x003BA60C File Offset: 0x003B880C
		private bool ActorBlocks(DetectionShotProtection.Actor actor, Vector3 cameraPosition, Vector3 point, Transform subject)
		{
			return !(actor.Root == null) && actor.Root.gameObject.activeInHierarchy && !DetectionShotProtection.SameActor(actor.Root, subject) && this.RenderersBlock(actor.Renderers, cameraPosition, point, actor.Root);
		}

		// Token: 0x060091AF RID: 37295 RVA: 0x003BA660 File Offset: 0x003B8860
		private bool RenderersBlock(IList<Renderer> renderers, Vector3 cameraPosition, Vector3 point, Transform root)
		{
			Vector3 vector = point - cameraPosition;
			float magnitude = vector.magnitude;
			if (magnitude < 0.02f)
			{
				return false;
			}
			if ((root.position - point).sqrMagnitude > (magnitude + 3f) * (magnitude + 3f))
			{
				return false;
			}
			Ray ray = new Ray(cameraPosition, vector / magnitude);
			foreach (Renderer renderer in renderers)
			{
				if (DetectionShotProtection.IsModelRenderer(renderer) && !renderer.forceRenderingOff && (!(this._output != null) || (this._output.cullingMask & (1 << renderer.gameObject.layer)) != 0))
				{
					Bounds bounds = renderer.bounds;
					bounds.Expand(0.04f);
					float num;
					if (bounds.IntersectRay(ray, out num) && num < magnitude - 0.02f)
					{
						Bounds bounds2;
						Matrix4x4 matrix4x;
						float num2;
						if (!DetectionShotProtection.TryGetOrientedBounds(renderer, out bounds2, out matrix4x, out num2))
						{
							return true;
						}
						Matrix4x4 inverse = matrix4x.inverse;
						Vector3 vector2 = inverse.MultiplyPoint3x4(cameraPosition);
						Vector3 vector3 = inverse.MultiplyPoint3x4(point) - vector2;
						float magnitude2 = vector3.magnitude;
						if (magnitude2 >= 0.0001f)
						{
							bounds2.Expand(0.04f / num2);
							if (bounds2.IntersectRay(new Ray(vector2, vector3 / magnitude2), out num) && num < magnitude2 - 0.001f)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060091B0 RID: 37296 RVA: 0x003BA810 File Offset: 0x003B8A10
		private bool CameraOverlapsRenderers(IList<Renderer> renderers, Vector3 position)
		{
			foreach (Renderer renderer in renderers)
			{
				if (DetectionShotProtection.IsModelRenderer(renderer) && !renderer.forceRenderingOff && (this._output.cullingMask & (1 << renderer.gameObject.layer)) != 0 && renderer.bounds.SqrDistance(position) < this.CameraRadius * this.CameraRadius)
				{
					Bounds bounds;
					Matrix4x4 matrix4x;
					float num;
					if (!DetectionShotProtection.TryGetOrientedBounds(renderer, out bounds, out matrix4x, out num))
					{
						return true;
					}
					Vector3 vector = bounds.ClosestPoint(matrix4x.inverse.MultiplyPoint3x4(position));
					if ((matrix4x.MultiplyPoint3x4(vector) - position).sqrMagnitude < this.CameraRadius * this.CameraRadius)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060091B1 RID: 37297 RVA: 0x003BA908 File Offset: 0x003B8B08
		private static bool TryGetOrientedBounds(Renderer renderer, out Bounds bounds, out Matrix4x4 localToWorld, out float minimumScale)
		{
			bounds = renderer.localBounds;
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			localToWorld = ((skinnedMeshRenderer != null && skinnedMeshRenderer.rootBone != null) ? skinnedMeshRenderer.rootBone.localToWorldMatrix : renderer.localToWorldMatrix);
			Vector3 vector = localToWorld.GetColumn(0);
			Vector3 vector2 = localToWorld.GetColumn(1);
			Vector3 vector3 = localToWorld.GetColumn(2);
			minimumScale = Mathf.Min(vector.magnitude, Mathf.Min(vector2.magnitude, vector3.magnitude));
			if (minimumScale < 0.0001f || Mathf.Abs(Vector3.Dot(vector.normalized, vector2.normalized)) > 0.001f || Mathf.Abs(Vector3.Dot(vector.normalized, vector3.normalized)) > 0.001f || Mathf.Abs(Vector3.Dot(vector2.normalized, vector3.normalized)) > 0.001f)
			{
				return false;
			}
			Vector3 extents = bounds.extents;
			Vector3 vector4 = new Vector3(Mathf.Abs(vector.x) * extents.x + Mathf.Abs(vector2.x) * extents.y + Mathf.Abs(vector3.x) * extents.z, Mathf.Abs(vector.y) * extents.x + Mathf.Abs(vector2.y) * extents.y + Mathf.Abs(vector3.y) * extents.z, Mathf.Abs(vector.z) * extents.x + Mathf.Abs(vector2.z) * extents.y + Mathf.Abs(vector3.z) * extents.z);
			Bounds bounds2 = renderer.bounds;
			return (localToWorld.MultiplyPoint3x4(bounds.center) - bounds2.center).sqrMagnitude < 0.0004f && (vector4 - bounds2.extents).sqrMagnitude < 0.0004f;
		}

		// Token: 0x060091B2 RID: 37298 RVA: 0x003BAB18 File Offset: 0x003B8D18
		private bool CameraOverlapsCharacter(Vector3 position, Transform portraitSubject)
		{
			foreach (DetectionShotProtection.Actor actor in this._actors)
			{
				if (actor.Root != null && (portraitSubject == null || !DetectionShotProtection.SameActor(actor.Root, portraitSubject)) && this.CameraOverlapsRenderers(actor.Renderers, position))
				{
					return true;
				}
			}
			foreach (DetectionShotProtection.SubjectPoints subjectPoints in this._subjects.Values)
			{
				if (subjectPoints.Root != null && !this._actorRoots.Contains(subjectPoints.Root) && (portraitSubject == null || !DetectionShotProtection.SameActor(subjectPoints.Root, portraitSubject)) && this.CameraOverlapsRenderers(subjectPoints.Renderers, position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060091B3 RID: 37299 RVA: 0x003BAC34 File Offset: 0x003B8E34
		private static bool InFrame(Vector3 point, Vector3 position, Quaternion rotation, LensSettings lens)
		{
			Vector3 vector = Quaternion.Inverse(rotation) * (point - position);
			if (vector.z <= Mathf.Max(0.01f, lens.NearClipPlane))
			{
				return false;
			}
			float num = (lens.Orthographic ? lens.OrthographicSize : (vector.z * Mathf.Tan(lens.FieldOfView * 0.017453292f * 0.5f)));
			return Mathf.Abs(vector.y) <= num && Mathf.Abs(vector.x) <= num * lens.Aspect;
		}

		// Token: 0x060091B4 RID: 37300 RVA: 0x003BACC8 File Offset: 0x003B8EC8
		private int VisiblePointMask(Vector3 position, Quaternion rotation, LensSettings lens, int count)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (DetectionShotProtection.InFrame(this._points[i], position, rotation, lens))
				{
					num |= 1 << i;
				}
			}
			return num;
		}

		// Token: 0x060091B5 RID: 37301 RVA: 0x003BAD04 File Offset: 0x003B8F04
		private float OcclusionScore(Vector3 position, Quaternion rotation, LensSettings lens, Transform subject, int count, int authoredVisiblePoints)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				if (!DetectionShotProtection.InFrame(this._points[i], position, rotation, lens))
				{
					if ((authoredVisiblePoints & (1 << i)) != 0)
					{
						num += 20f;
					}
				}
				else
				{
					num2++;
					float num3 = 0f;
					foreach (DetectionShotProtection.Actor actor in this._actors)
					{
						if (this.ActorBlocks(actor, position, this._points[i], subject))
						{
							num3 = Mathf.Max(num3, actor.CanHide ? 1f : 40f);
						}
					}
					foreach (DetectionShotProtection.SubjectPoints subjectPoints in this._subjects.Values)
					{
						if (subjectPoints.Root != null && !this._actorRoots.Contains(subjectPoints.Root) && !DetectionShotProtection.SameActor(subjectPoints.Root, subject) && this.RenderersBlock(subjectPoints.Renderers, position, this._points[i], subjectPoints.Root))
						{
							num3 = Mathf.Max(num3, 40f);
						}
					}
					num += num3 * ((i == 0) ? 2f : 1f);
				}
			}
			if (num2 != 0 || authoredVisiblePoints == 0)
			{
				return num;
			}
			return 1000f;
		}

		// Token: 0x060091B6 RID: 37302 RVA: 0x003BAEA4 File Offset: 0x003B90A4
		private bool TryClearPosition(Vector3 pivot, Vector3 ideal, Transform subject, out Vector3 position, bool preserveSubjectCloseUp = false)
		{
			position = ideal;
			Vector3 vector = ideal - pivot;
			float magnitude = vector.magnitude;
			float num = (preserveSubjectCloseUp ? Mathf.Min(this.MinimumSubjectDistance, this.CameraRadius) : this.MinimumSubjectDistance);
			if (magnitude < num)
			{
				return false;
			}
			Vector3 vector2 = vector / magnitude;
			int num2 = Physics.SphereCastNonAlloc(pivot, this.CameraRadius, vector2, this._hits, magnitude, this.EnvironmentLayers, QueryTriggerInteraction.Ignore);
			if (num2 == this._hits.Length)
			{
				return false;
			}
			float num3 = magnitude;
			for (int i = 0; i < num2; i++)
			{
				if (!this.IsCharacter(this._hits[i].collider, subject))
				{
					num3 = Mathf.Min(num3, this._hits[i].distance - 0.02f);
				}
			}
			if (num3 < num)
			{
				return false;
			}
			position = pivot + vector2 * num3;
			num2 = Physics.OverlapSphereNonAlloc(position, this.CameraRadius, this._overlaps, this.EnvironmentLayers, QueryTriggerInteraction.Ignore);
			if (num2 == this._overlaps.Length)
			{
				return false;
			}
			for (int j = 0; j < num2; j++)
			{
				if (!this.IsCharacter(this._overlaps[j], subject))
				{
					return false;
				}
			}
			return !this.CameraOverlapsCharacter(position, preserveSubjectCloseUp ? subject : null);
		}

		// Token: 0x060091B7 RID: 37303 RVA: 0x003BB004 File Offset: 0x003B9204
		private void CandidateOffset(int index, out float yaw, out float lift, out float distanceScale)
		{
			distanceScale = 1f;
			if (index >= 6)
			{
				int num = index - 6;
				yaw = DetectionShotProtection.RecoveryAngles[num % DetectionShotProtection.RecoveryAngles.Length];
				int num2 = num / DetectionShotProtection.RecoveryAngles.Length;
				lift = ((num2 < 3) ? 0f : ((num2 == 3) ? this.MaximumLift : Mathf.Lerp(this.MaximumLift, Mathf.Max(this.MaximumLift, this.MaximumRecoveryLift), (num2 == 4) ? 0.5f : 1f)));
				distanceScale = ((num2 < 3) ? (1f + 0.5f * (float)num2) : 1f);
				return;
			}
			yaw = ((index == 1) ? (-this.MaximumYaw * 0.55f) : ((index == 2) ? (this.MaximumYaw * 0.55f) : ((index == 3) ? (-this.MaximumYaw) : ((index == 4) ? this.MaximumYaw : 0f))));
			lift = ((index == 5) ? this.MaximumLift : 0f);
		}

		// Token: 0x060091B8 RID: 37304 RVA: 0x003BB0FC File Offset: 0x003B92FC
		private Vector3 CandidatePosition(int index, Vector3 authored, Vector3 aim)
		{
			float num;
			float num2;
			float num3;
			this.CandidateOffset(index, out num, out num2, out num3);
			return DetectionShotProtection.OffsetPosition(authored, aim, num, num2, num3);
		}

		// Token: 0x060091B9 RID: 37305 RVA: 0x003BB120 File Offset: 0x003B9320
		private Vector3 CorrectionPivot(Vector3 aim)
		{
			if ((aim - this._points[1]).sqrMagnitude <= 2.25f)
			{
				return aim;
			}
			return this._points[1];
		}

		// Token: 0x060091BA RID: 37306 RVA: 0x003BB15C File Offset: 0x003B935C
		private static Vector3 OffsetPosition(Vector3 authored, Vector3 aim, float yaw, float lift, float distanceScale)
		{
			Vector3 vector = Quaternion.AngleAxis(yaw, Vector3.up) * (authored - aim);
			vector.x *= distanceScale;
			vector.z *= distanceScale;
			return aim + vector + Vector3.up * lift;
		}

		// Token: 0x060091BB RID: 37307 RVA: 0x003BB1B4 File Offset: 0x003B93B4
		private bool TrySelectCandidate(Transform subject, Vector3 authored, Vector3 aim, Quaternion orientation, LensSettings lens, int pointCount, int authoredVisiblePoints, int held, bool hasSolution, out int best, out Vector3 position, bool preserveSubjectCloseUp = false)
		{
			best = held;
			position = authored;
			Vector3 vector = this.CorrectionPivot(aim);
			float num = float.PositiveInfinity;
			int num2 = 6;
			for (int i = -1; i < num2; i++)
			{
				int num3 = ((i < 0) ? held : i);
				Vector3 vector2;
				if (this.TryClearPosition(vector, this.CandidatePosition(num3, authored, vector), subject, out vector2, preserveSubjectCloseUp))
				{
					float num4;
					float num5;
					float num6;
					this.CandidateOffset(num3, out num4, out num5, out num6);
					float num7 = this.OcclusionScore(vector2, DetectionShotProtection.Reaim(authored, vector2, aim, orientation), lens, subject, pointCount, authoredVisiblePoints);
					num7 += Mathf.Abs(num4) * 0.004f + num5 * 1.5f + (num6 - 1f) * 0.5f;
					if (num3 == 0 && num7 == 0f)
					{
						best = num3;
						position = vector2;
						return true;
					}
					if (num7 < num - (hasSolution ? 0.35f : 0.001f))
					{
						best = num3;
						position = vector2;
						num = num7;
					}
					if (num7 == 0f)
					{
						break;
					}
				}
				if (i == 5 && num >= 20f)
				{
					num2 = 6 + DetectionShotProtection.RecoveryAngles.Length * 6;
				}
			}
			return !float.IsPositiveInfinity(num) && num < 1000f;
		}

		// Token: 0x060091BC RID: 37308 RVA: 0x003BB2E4 File Offset: 0x003B94E4
		private static Quaternion Reaim(Vector3 before, Vector3 after, Vector3 aim, Quaternion rotation)
		{
			if ((aim - before).sqrMagnitude < 0.0001f || (aim - after).sqrMagnitude < 0.0001f)
			{
				return rotation;
			}
			Vector3 normalized = (aim - before).normalized;
			Vector3 normalized2 = (aim - after).normalized;
			Vector3 vector = Vector3.up;
			if (Vector3.Cross(normalized, vector).sqrMagnitude < 0.0001f || Vector3.Cross(normalized2, vector).sqrMagnitude < 0.0001f)
			{
				vector = rotation * Vector3.up;
				if (Vector3.Cross(normalized, vector).sqrMagnitude < 0.0001f || Vector3.Cross(normalized2, vector).sqrMagnitude < 0.0001f)
				{
					vector = ((Mathf.Abs(normalized.z) + Mathf.Abs(normalized2.z) < Mathf.Abs(normalized.x) + Mathf.Abs(normalized2.x)) ? Vector3.forward : Vector3.right);
				}
			}
			return Quaternion.LookRotation(normalized2, vector) * Quaternion.Inverse(Quaternion.LookRotation(normalized, vector)) * rotation;
		}

		// Token: 0x060091BD RID: 37309 RVA: 0x003BB404 File Offset: 0x003B9604
		internal void CorrectShot(DetectionShotCameraExtension shot, ref CameraState state, float deltaTime)
		{
			if (!this.IsActive || this._output == null || shot.Subject == null || !this.TryAlternateAngles)
			{
				return;
			}
			this.RefreshActors();
			int subjectPoints = this.GetSubjectPoints(shot.Subject);
			Vector3 correctedPosition = state.CorrectedPosition;
			Vector3 vector = (state.HasLookAt ? state.ReferenceLookAt : this._points[1]);
			Vector3 vector2 = this.CorrectionPivot(vector);
			Quaternion correctedOrientation = state.CorrectedOrientation;
			int num = this.VisiblePointMask(correctedPosition, correctedOrientation, state.Lens, subjectPoints);
			if (deltaTime < 0f)
			{
				shot.ResetShot();
			}
			Vector3 vector3;
			bool flag = this.TryClearPosition(vector2, this.CandidatePosition(shot.Candidate, correctedPosition, vector2), shot.Subject, out vector3, shot.PreserveSubjectCloseUp);
			if (!shot.HasSolution || !flag || Time.unscaledTime >= shot.NextEvaluation)
			{
				shot.NextEvaluation = Time.unscaledTime + 0.2f;
				int num2;
				flag = this.TrySelectCandidate(shot.Subject, correctedPosition, vector, correctedOrientation, state.Lens, subjectPoints, num, shot.Candidate, shot.HasSolution, out num2, out vector3, shot.PreserveSubjectCloseUp);
				shot.Candidate = num2;
			}
			Vector3 vector5;
			if (flag)
			{
				float num3;
				float num4;
				float num5;
				this.CandidateOffset(shot.Candidate, out num3, out num4, out num5);
				float num6 = ((!shot.HasSolution || deltaTime < 0f) ? 1f : (1f - Mathf.Exp(-Time.unscaledDeltaTime * 10f)));
				shot.Yaw = Mathf.Lerp(shot.Yaw, num3, num6);
				shot.Lift = Mathf.Lerp(shot.Lift, num4, num6);
				shot.DistanceScale = Mathf.Lerp(shot.DistanceScale, num5, num6);
				shot.HasSolution = true;
				Vector3 vector4 = DetectionShotProtection.OffsetPosition(correctedPosition, vector2, shot.Yaw, shot.Lift, shot.DistanceScale);
				if (!this.TryClearPosition(vector2, vector4, shot.Subject, out vector5, shot.PreserveSubjectCloseUp))
				{
					vector5 = vector3;
					shot.Yaw = num3;
					shot.Lift = num4;
					shot.DistanceScale = num5;
				}
			}
			else
			{
				shot.HasSolution = false;
				if (!shot.HasSafePosition || !this.TryClearPosition(vector2, shot.LastSafePosition, shot.Subject, out vector5, shot.PreserveSubjectCloseUp))
				{
					return;
				}
			}
			state.PositionCorrection += vector5 - correctedPosition;
			state.OrientationCorrection = Quaternion.Inverse(state.RawOrientation) * DetectionShotProtection.Reaim(correctedPosition, vector5, vector, correctedOrientation);
		}

		// Token: 0x060091BE RID: 37310 RVA: 0x003BB67C File Offset: 0x003B987C
		private void OnCameraUpdated(CinemachineBrain brain)
		{
			if (!this.IsActive || brain == null || brain.OutputCamera != this._output)
			{
				return;
			}
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = brain.ActiveVirtualCamera as CinemachineVirtualCameraBase;
			DetectionShotCameraExtension detectionShotCameraExtension;
			if (!(cinemachineVirtualCameraBase == null) && this._shots.TryGetValue(cinemachineVirtualCameraBase, out detectionShotCameraExtension) && !(detectionShotCameraExtension == null) && !(detectionShotCameraExtension.Subject == null))
			{
				if (this.CorrectBlendedOutput(brain, detectionShotCameraExtension))
				{
					detectionShotCameraExtension.LastSafePosition = this._output.transform.position;
					detectionShotCameraExtension.HasSafePosition = true;
				}
				this.UpdateVisibility(detectionShotCameraExtension.Subject);
				return;
			}
			this._hidden.Clear();
			DetectionRenderVisibilityScope visibility = this._visibility;
			if (visibility == null)
			{
				return;
			}
			visibility.Restore();
		}

		// Token: 0x060091BF RID: 37311 RVA: 0x003BB73C File Offset: 0x003B993C
		private bool CorrectBlendedOutput(CinemachineBrain brain, DetectionShotCameraExtension shot)
		{
			shot.ObserveBlend(brain.ActiveBlend);
			if (!this.TryAlternateAngles)
			{
				shot.ResetBlend();
				return false;
			}
			this.GetSubjectPoints(shot.Subject);
			CameraState currentCameraState = brain.CurrentCameraState;
			Vector3 vector = (currentCameraState.HasLookAt ? currentCameraState.ReferenceLookAt : this._points[1]);
			Vector3 vector2 = this.CorrectionPivot(vector);
			Vector3 position = this._output.transform.position;
			Quaternion rotation = this._output.transform.rotation;
			Vector3 vector3;
			if (!shot.HoldBlendDestination && this.TryClearPosition(vector2, position, shot.Subject, out vector3, shot.PreserveSubjectCloseUp))
			{
				this._output.transform.SetPositionAndRotation(vector3, DetectionShotProtection.Reaim(position, vector3, vector, rotation));
				return true;
			}
			CameraState state = shot.VirtualCamera.State;
			vector = (state.HasLookAt ? state.ReferenceLookAt : this._points[1]);
			vector2 = this.CorrectionPivot(vector);
			if (this.TryClearPosition(vector2, state.FinalPosition, shot.Subject, out vector3, shot.PreserveSubjectCloseUp))
			{
				shot.HoldBlendDestination = brain.ActiveBlend != null;
				this._output.transform.SetPositionAndRotation(vector3, DetectionShotProtection.Reaim(state.FinalPosition, vector3, vector, state.FinalOrientation));
				return true;
			}
			int subjectPoints = this.GetSubjectPoints(shot.Subject);
			int num = this.VisiblePointMask(state.FinalPosition, state.FinalOrientation, state.Lens, subjectPoints);
			int num2;
			if (this.TrySelectCandidate(shot.Subject, state.FinalPosition, vector, state.FinalOrientation, state.Lens, subjectPoints, num, 0, false, out num2, out vector3, shot.PreserveSubjectCloseUp) || (shot.HasSafePosition && this.TryClearPosition(vector2, shot.LastSafePosition, shot.Subject, out vector3, shot.PreserveSubjectCloseUp)))
			{
				shot.HoldBlendDestination = brain.ActiveBlend != null;
				this._output.transform.SetPositionAndRotation(vector3, DetectionShotProtection.Reaim(state.FinalPosition, vector3, vector, state.FinalOrientation));
				return true;
			}
			return false;
		}

		// Token: 0x060091C0 RID: 37312 RVA: 0x003BB94C File Offset: 0x003B9B4C
		private void UpdateVisibility(Transform subject)
		{
			this._hidden.Clear();
			if (!this.IsActive || this._output == null || subject == null)
			{
				DetectionRenderVisibilityScope visibility = this._visibility;
				if (visibility == null)
				{
					return;
				}
				visibility.Restore();
				return;
			}
			else
			{
				this.RefreshActors();
				int subjectPoints = this.GetSubjectPoints(subject);
				foreach (DetectionShotProtection.Actor actor in this._actors)
				{
					if (actor.CanHide && !(actor.Root == null) && !DetectionShotProtection.SameActor(actor.Root, subject))
					{
						bool flag = false;
						for (int i = 0; i < subjectPoints; i++)
						{
							Vector3 vector = this._output.WorldToViewportPoint(this._points[i]);
							if (vector.z > this._output.nearClipPlane && vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f && this.ActorBlocks(actor, this._output.transform.position, this._points[i], subject))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							foreach (Renderer renderer in actor.Renderers)
							{
								if (renderer != null && !this._hidden.Contains(renderer))
								{
									this._hidden.Add(renderer);
								}
							}
						}
					}
				}
				DetectionRenderVisibilityScope visibility2 = this._visibility;
				if (visibility2 == null)
				{
					return;
				}
				visibility2.SetHiddenRenderers(this._hidden);
				return;
			}
		}

		// Token: 0x040095EA RID: 38378
		[Tooltip("Try small variations of the selected shot before hiding an occluding NPC.")]
		public bool TryAlternateAngles = true;

		// Token: 0x040095EB RID: 38379
		[Range(0f, 40f)]
		public float MaximumYaw = 30f;

		// Token: 0x040095EC RID: 38380
		[Range(0f, 0.5f)]
		public float MaximumLift = 0.25f;

		// Token: 0x040095ED RID: 38381
		[Tooltip("Extra height to try only when the normal shot cannot clear nearby characters or walls.")]
		[Range(0f, 1.5f)]
		public float MaximumRecoveryLift = 0.8f;

		// Token: 0x040095EE RID: 38382
		[Min(0.05f)]
		public float CameraRadius = 0.15f;

		// Token: 0x040095EF RID: 38383
		[Min(0.1f)]
		public float MinimumSubjectDistance = 0.35f;

		// Token: 0x040095F0 RID: 38384
		public LayerMask EnvironmentLayers = -1;

		// Token: 0x040095F1 RID: 38385
		private readonly List<DetectionShotProtection.Actor> _actors = new List<DetectionShotProtection.Actor>();

		// Token: 0x040095F2 RID: 38386
		private readonly HashSet<Transform> _actorRoots = new HashSet<Transform>();

		// Token: 0x040095F3 RID: 38387
		private readonly Dictionary<Transform, DetectionShotProtection.SubjectPoints> _subjects = new Dictionary<Transform, DetectionShotProtection.SubjectPoints>();

		// Token: 0x040095F4 RID: 38388
		private readonly Dictionary<CinemachineVirtualCameraBase, DetectionShotCameraExtension> _shots = new Dictionary<CinemachineVirtualCameraBase, DetectionShotCameraExtension>();

		// Token: 0x040095F5 RID: 38389
		private readonly List<Renderer> _hidden = new List<Renderer>();

		// Token: 0x040095F6 RID: 38390
		private readonly Vector3[] _points = new Vector3[8];

		// Token: 0x040095F7 RID: 38391
		private readonly RaycastHit[] _hits = new RaycastHit[64];

		// Token: 0x040095F8 RID: 38392
		private readonly Collider[] _overlaps = new Collider[32];

		// Token: 0x040095F9 RID: 38393
		private Camera _output;

		// Token: 0x040095FA RID: 38394
		private DetectionRenderVisibilityScope _visibility;

		// Token: 0x040095FB RID: 38395
		private bool _active;

		// Token: 0x040095FC RID: 38396
		private float _nextActorRefresh;

		// Token: 0x040095FD RID: 38397
		private const int LocalCandidateCount = 6;

		// Token: 0x040095FE RID: 38398
		private const int RecoveryTierCount = 6;

		// Token: 0x040095FF RID: 38399
		private static readonly float[] RecoveryAngles = new float[] { 0f, -30f, 30f, -60f, 60f, -90f, 90f, -120f, 120f, 180f };

		// Token: 0x0200124D RID: 4685
		private sealed class Actor
		{
			// Token: 0x04009600 RID: 38400
			public Transform Root;

			// Token: 0x04009601 RID: 38401
			public bool CanHide = true;

			// Token: 0x04009602 RID: 38402
			public readonly List<Renderer> Renderers = new List<Renderer>();
		}

		// Token: 0x0200124E RID: 4686
		private sealed class SubjectPoints
		{
			// Token: 0x04009603 RID: 38403
			public Transform Root;

			// Token: 0x04009604 RID: 38404
			public Renderer[] Renderers;

			// Token: 0x04009605 RID: 38405
			public Transform Head;

			// Token: 0x04009606 RID: 38406
			public Transform Chest;

			// Token: 0x04009607 RID: 38407
			public Transform Hips;

			// Token: 0x04009608 RID: 38408
			public Transform LeftShoulder;

			// Token: 0x04009609 RID: 38409
			public Transform RightShoulder;

			// Token: 0x0400960A RID: 38410
			public Transform LeftFoot;

			// Token: 0x0400960B RID: 38411
			public Transform RightFoot;
		}
	}
}
