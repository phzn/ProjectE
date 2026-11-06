using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011F1 RID: 4593
	[DisallowMultipleComponent]
	public sealed class GachaPlushField : MonoBehaviour
	{
		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06008DAB RID: 36267 RVA: 0x0039D964 File Offset: 0x0039BB64
		public bool IsSessionActive
		{
			get
			{
				return this._sessionActive;
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06008DAC RID: 36268 RVA: 0x0039D96C File Offset: 0x0039BB6C
		public IReadOnlyList<Rigidbody> LeftPlushBodies
		{
			get
			{
				return this.leftPlushBodies;
			}
		}

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06008DAD RID: 36269 RVA: 0x0039D974 File Offset: 0x0039BB74
		public IReadOnlyList<Rigidbody> RightPlushBodies
		{
			get
			{
				return this.rightPlushBodies;
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06008DAE RID: 36270 RVA: 0x0039D97C File Offset: 0x0039BB7C
		public int ConfiguredBodyCount
		{
			get
			{
				return GachaPlushField.CountValid(this.leftPlushBodies) + GachaPlushField.CountValid(this.rightPlushBodies);
			}
		}

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06008DAF RID: 36271 RVA: 0x0039D995 File Offset: 0x0039BB95
		public int ActivePlayerCollisionProxyCount
		{
			get
			{
				return this._activePlayerCollisionProxies.Count;
			}
		}

		// Token: 0x06008DB0 RID: 36272 RVA: 0x0039D9A2 File Offset: 0x0039BBA2
		private void Awake()
		{
			this.CaptureAuthoredBodySnapshots();
			this.RestoreAllBodiesToAuthoredState();
			this.WarnAboutConfiguration();
		}

		// Token: 0x06008DB1 RID: 36273 RVA: 0x0039D9B6 File Offset: 0x0039BBB6
		private void OnDisable()
		{
			this.EndSession();
		}

		// Token: 0x06008DB2 RID: 36274 RVA: 0x0039D9C0 File Offset: 0x0039BBC0
		private void FixedUpdate()
		{
			if (!this._sessionActive)
			{
				return;
			}
			this.ClampBodyVelocities(this.leftPlushBodies);
			this.ClampBodyVelocities(this.rightPlushBodies);
			this.RecoverBodiesBelowFloor(this.leftPlushBodies);
			this.RecoverBodiesBelowFloor(this.rightPlushBodies);
			this.KeepBodiesInsideCabinet(this.leftPlushBodies);
			this.KeepBodiesInsideCabinet(this.rightPlushBodies);
		}

		// Token: 0x06008DB3 RID: 36275 RVA: 0x0039DA20 File Offset: 0x0039BC20
		private void KeepBodiesInsideCabinet(Rigidbody[] bodies)
		{
			if (bodies == null)
			{
				return;
			}
			foreach (Rigidbody rigidbody in bodies)
			{
				if (!(rigidbody == null) && !rigidbody.isKinematic && rigidbody.detectCollisions)
				{
					Vector3 vector = base.transform.InverseTransformPoint(rigidbody.position);
					Vector3 vector2 = new Vector3(Mathf.Clamp(vector.x, this.interiorMinLocal.x, this.interiorMaxLocal.x), Mathf.Clamp(vector.y, this.interiorMinLocal.y, this.interiorMaxLocal.y), Mathf.Clamp(vector.z, this.interiorMinLocal.z, this.interiorMaxLocal.z));
					if ((vector2 - vector).sqrMagnitude >= 1E-06f)
					{
						rigidbody.position = base.transform.TransformPoint(vector2);
						Vector3 vector3 = base.transform.InverseTransformDirection(rigidbody.velocity);
						if (vector.x < this.interiorMinLocal.x && vector3.x < 0f)
						{
							vector3.x = 0f;
						}
						if (vector.x > this.interiorMaxLocal.x && vector3.x > 0f)
						{
							vector3.x = 0f;
						}
						if (vector.y < this.interiorMinLocal.y && vector3.y < 0f)
						{
							vector3.y = 0f;
						}
						if (vector.y > this.interiorMaxLocal.y && vector3.y > 0f)
						{
							vector3.y = 0f;
						}
						if (vector.z < this.interiorMinLocal.z && vector3.z < 0f)
						{
							vector3.z = 0f;
						}
						if (vector.z > this.interiorMaxLocal.z && vector3.z > 0f)
						{
							vector3.z = 0f;
						}
						rigidbody.velocity = base.transform.TransformDirection(vector3);
						rigidbody.angularVelocity *= 0.5f;
						if (this.debugLogs)
						{
							Debug.Log("[GachaPlushField] Pulled '" + rigidbody.name + "' back inside the cabinet.", rigidbody);
						}
					}
				}
			}
		}

		// Token: 0x06008DB4 RID: 36276 RVA: 0x0039DC80 File Offset: 0x0039BE80
		private void ClampBodyVelocities(Rigidbody[] bodies)
		{
			if (bodies == null)
			{
				return;
			}
			float num = this.maximumLinearVelocity * this.maximumLinearVelocity;
			foreach (Rigidbody rigidbody in bodies)
			{
				if (!(rigidbody == null) && !rigidbody.isKinematic)
				{
					Vector3 velocity = rigidbody.velocity;
					if (velocity.sqrMagnitude > num)
					{
						rigidbody.velocity = velocity.normalized * this.maximumLinearVelocity;
					}
				}
			}
		}

		// Token: 0x06008DB5 RID: 36277 RVA: 0x0039DCEC File Offset: 0x0039BEEC
		public bool BeginSession(GachaMachineSide side, Transform player, Animator resolvedPlayerAnimator = null)
		{
			if (!base.isActiveAndEnabled)
			{
				Debug.LogWarning("[GachaPlushField] Cannot begin a plush drop while the field is disabled.", this);
				return false;
			}
			this.EndSession();
			Rigidbody[] array = this.BodiesForSide(side);
			int num = GachaPlushField.CountValid(array);
			if (num == 0)
			{
				Debug.LogWarning(string.Format("[GachaPlushField] No plush bodies are configured for {0}.", side), this);
				return false;
			}
			int num2 = this._sessionGeneration + 1;
			this._sessionGeneration = num2;
			int num3 = num2;
			this._sessionSerial++;
			this._sessionActive = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					this.PrepareBodyForRestaging(array[i]);
				}
			}
			this._releaseRoutine = base.StartCoroutine(this.ReleaseBodies(side, array, num, player, resolvedPlayerAnimator, num3));
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaPlushField] Releasing {0} {1} plush toys.", num, side), this);
			}
			return true;
		}

		// Token: 0x06008DB6 RID: 36278 RVA: 0x0039DDCA File Offset: 0x0039BFCA
		public bool BeginSession(GachaMachineSide side, GameObject player)
		{
			return this.BeginSession(side, (player != null) ? player.transform : null, null);
		}

		// Token: 0x06008DB7 RID: 36279 RVA: 0x0039DDE6 File Offset: 0x0039BFE6
		public void EndSession()
		{
			this._sessionGeneration++;
			if (this._releaseRoutine != null)
			{
				base.StopCoroutine(this._releaseRoutine);
				this._releaseRoutine = null;
			}
			this._sessionActive = false;
			this.ReleasePlayerCollisionProxy();
			this.RestoreAllBodiesToAuthoredState();
		}

		// Token: 0x06008DB8 RID: 36280 RVA: 0x0039DE24 File Offset: 0x0039C024
		public void ReleasePlayerCollisionProxy()
		{
			this.RestorePlayerCollisions();
			this.RestoreAnimatedPlayerCollisionProxies();
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x0039DE32 File Offset: 0x0039C032
		private IEnumerator ReleaseBodies(GachaMachineSide side, Rigidbody[] selectedBodies, int validBodyCount, Transform player, Animator resolvedPlayerAnimator, int generation)
		{
			yield return null;
			yield return new WaitForFixedUpdate();
			if (generation != this._sessionGeneration || !this._sessionActive)
			{
				yield break;
			}
			this.EnableAnimatedPlayerCollisionProxies(player, resolvedPlayerAnimator);
			Random random = this.CreateSessionRandom(side);
			Animator animator = GachaPlushField.ResolveHumanoidAnimator(player, resolvedPlayerAnimator);
			List<Vector3> list = this.BuildSpawnPositions(side, validBodyCount, random, animator);
			List<GachaPlushField.ReleaseInstruction> releases = new List<GachaPlushField.ReleaseInstruction>(validBodyCount);
			int num = 0;
			foreach (Rigidbody rigidbody in selectedBodies)
			{
				if (!(rigidbody == null))
				{
					Vector3 vector = list[num++];
					Quaternion quaternion = Quaternion.Euler(GachaPlushField.NextFloat(random, -this.maximumSpawnTilt, this.maximumSpawnTilt), GachaPlushField.NextFloat(random, 0f, 360f), GachaPlushField.NextFloat(random, -this.maximumSpawnTilt, this.maximumSpawnTilt));
					rigidbody.position = base.transform.TransformPoint(vector);
					rigidbody.rotation = base.transform.rotation * quaternion;
					Vector3 vector2 = vector - this.DisplacementCenterLocal(side);
					vector2.y = 0f;
					if (vector2.sqrMagnitude < 0.0001f)
					{
						float num2 = GachaPlushField.NextFloat(random, 0f, 6.2831855f);
						vector2 = new Vector3(Mathf.Cos(num2), 0f, Mathf.Sin(num2));
					}
					else
					{
						vector2.Normalize();
					}
					Vector3 vector3 = base.transform.TransformDirection(vector2).normalized * GachaPlushField.NextFloat(random, this.horizontalReleaseSpeed.x, this.horizontalReleaseSpeed.y) + base.transform.up * GachaPlushField.NextFloat(random, this.verticalReleaseSpeed.x, this.verticalReleaseSpeed.y);
					Vector3 up = new Vector3(GachaPlushField.NextFloat(random, -1f, 1f), GachaPlushField.NextFloat(random, -1f, 1f), GachaPlushField.NextFloat(random, -1f, 1f));
					if (up.sqrMagnitude < 0.0001f)
					{
						up = Vector3.up;
					}
					Vector3 vector4 = up.normalized * GachaPlushField.NextFloat(random, this.angularReleaseSpeed.x, this.angularReleaseSpeed.y);
					releases.Add(new GachaPlushField.ReleaseInstruction(rigidbody, vector3, vector4));
				}
			}
			Physics.SyncTransforms();
			this.IgnoreNonProxyPlayerCollisions(selectedBodies, player);
			int releasedThisStep = 0;
			int num3;
			for (int i = 0; i < releases.Count; i = num3 + 1)
			{
				if (generation != this._sessionGeneration || !this._sessionActive)
				{
					yield break;
				}
				GachaPlushField.ReleaseInstruction releaseInstruction = releases[i];
				Rigidbody body = releaseInstruction.body;
				if (body != null)
				{
					this.ConfigureDynamicBody(body);
					body.detectCollisions = true;
					body.useGravity = true;
					body.isKinematic = false;
					body.WakeUp();
					body.velocity = releaseInstruction.velocity;
					body.angularVelocity = releaseInstruction.angularVelocity;
				}
				num3 = releasedThisStep;
				releasedThisStep = num3 + 1;
				if (releasedThisStep >= this.bodiesReleasedPerFixedStep && i + 1 < releases.Count)
				{
					releasedThisStep = 0;
					yield return new WaitForFixedUpdate();
				}
				num3 = i;
			}
			if (generation == this._sessionGeneration)
			{
				this._releaseRoutine = null;
			}
			yield break;
		}

		// Token: 0x06008DBA RID: 36282 RVA: 0x0039DE70 File Offset: 0x0039C070
		private static Animator ResolveHumanoidAnimator(Transform player, Animator candidate)
		{
			if (candidate != null && candidate.isHuman)
			{
				return candidate;
			}
			if (player == null)
			{
				return null;
			}
			Animator component = player.GetComponent<Animator>();
			if (component != null && component.isHuman)
			{
				return component;
			}
			foreach (Animator animator in player.GetComponentsInChildren<Animator>(true))
			{
				if (animator != null && animator.isHuman)
				{
					return animator;
				}
			}
			return null;
		}

		// Token: 0x06008DBB RID: 36283 RVA: 0x0039DEE4 File Offset: 0x0039C0E4
		private List<Vector3> BuildSpawnPositions(GachaMachineSide side, int requiredCount, Random random, Animator animator)
		{
			List<Vector3> list;
			List<Vector3> list2;
			if (this.targetSpawnOverBody && this.TryCollectBodyCoverPoints(animator, out list, out list2))
			{
				return this.BuildBodyCoveringColumns(side, list, list2, requiredCount, random);
			}
			return this.BuildShuffledGrid(this.ResolveSpawnCenterLocal(side), requiredCount, random);
		}

		// Token: 0x06008DBC RID: 36284 RVA: 0x0039DF24 File Offset: 0x0039C124
		private bool TryCollectBodyCoverPoints(Animator animator, out List<Vector3> ringPoints, out List<Vector3> crestPoints)
		{
			GachaPlushField.<>c__DisplayClass67_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.animator = animator;
			List<Vector3> list = new List<Vector3>(10);
			CS$<>8__locals1.crest = new List<Vector3>(8);
			ringPoints = list;
			crestPoints = CS$<>8__locals1.crest;
			if (CS$<>8__locals1.animator == null || !CS$<>8__locals1.animator.isHuman)
			{
				return false;
			}
			Transform boneTransform = CS$<>8__locals1.animator.GetBoneTransform(HumanBodyBones.Hips);
			Transform boneTransform2 = CS$<>8__locals1.animator.GetBoneTransform(HumanBodyBones.Chest);
			Transform boneTransform3 = CS$<>8__locals1.animator.GetBoneTransform(HumanBodyBones.Head);
			if (boneTransform == null || boneTransform2 == null)
			{
				return false;
			}
			Vector3 vector = this.<TryCollectBodyCoverPoints>g__BoneLocal|67_0(boneTransform, ref CS$<>8__locals1);
			Vector3 vector2 = this.<TryCollectBodyCoverPoints>g__BoneLocal|67_0(boneTransform2, ref CS$<>8__locals1);
			Vector3 vector3 = ((boneTransform3 != null) ? this.<TryCollectBodyCoverPoints>g__BoneLocal|67_0(boneTransform3, ref CS$<>8__locals1) : vector2);
			CS$<>8__locals1.crest.Add(vector3);
			CS$<>8__locals1.crest.Add(vector2);
			CS$<>8__locals1.crest.Add(Vector3.Lerp(vector, vector2, 0.5f));
			CS$<>8__locals1.crest.Add(vector);
			this.<TryCollectBodyCoverPoints>g__AddCrestMidpoint|67_1(HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, ref CS$<>8__locals1);
			this.<TryCollectBodyCoverPoints>g__AddCrestMidpoint|67_1(HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, ref CS$<>8__locals1);
			this.<TryCollectBodyCoverPoints>g__AddCrestMidpoint|67_1(HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot, ref CS$<>8__locals1);
			this.<TryCollectBodyCoverPoints>g__AddCrestMidpoint|67_1(HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot, ref CS$<>8__locals1);
			Vector3 vector4 = vector2 - vector;
			vector4.y = 0f;
			if (vector4.sqrMagnitude < 0.0001f)
			{
				vector4 = Vector3.forward;
			}
			vector4.Normalize();
			Vector3 vector5 = new Vector3(-vector4.z, 0f, vector4.x);
			foreach (Vector3 vector6 in new Vector3[] { vector2, vector })
			{
				list.Add(vector6 + vector5 * this.bodyRingOffset);
				list.Add(vector6 - vector5 * this.bodyRingOffset);
			}
			list.Add(vector3 + vector4 * (this.bodyRingOffset + 0.05f));
			list.Add(vector - vector4 * (this.bodyRingOffset + 0.12f));
			return CS$<>8__locals1.crest.Count > 0;
		}

		// Token: 0x06008DBD RID: 36285 RVA: 0x0039E15C File Offset: 0x0039C35C
		private List<Vector3> BuildBodyCoveringColumns(GachaMachineSide side, List<Vector3> ringPoints, List<Vector3> crestPoints, int requiredCount, Random random)
		{
			GachaPlushField.<>c__DisplayClass68_0 CS$<>8__locals1;
			CS$<>8__locals1.random = random;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.baseY = this.ResolveSpawnCenterLocal(side).y - this.spawnSizeLocal.y * 0.5f + 0.04f;
			List<Vector3> list = new List<Vector3>(requiredCount);
			int num = 0;
			while (num < ringPoints.Count && list.Count < requiredCount)
			{
				list.Add(this.<BuildBodyCoveringColumns>g__MakePosition|68_0(ringPoints[num], 0, this.bodyCoverJitter + 0.02f, ref CS$<>8__locals1));
				num++;
			}
			int num2 = 0;
			while (list.Count < requiredCount)
			{
				int num3 = 1 + num2 / Mathf.Max(1, crestPoints.Count);
				list.Add(this.<BuildBodyCoveringColumns>g__MakePosition|68_0(crestPoints[num2 % crestPoints.Count], num3, this.bodyCoverJitter, ref CS$<>8__locals1));
				num2++;
			}
			return list;
		}

		// Token: 0x06008DBE RID: 36286 RVA: 0x0039E234 File Offset: 0x0039C434
		private List<Vector3> BuildShuffledGrid(Vector3 center, int requiredCount, Random random)
		{
			int num = 8;
			int num2 = Mathf.Max(1, Mathf.CeilToInt((float)requiredCount / (float)num));
			List<Vector3> list = new List<Vector3>(num * num2);
			for (int i = 0; i < num2; i++)
			{
				float num3 = ((num2 == 1) ? 0.5f : (((float)i + 0.5f) / (float)num2));
				float num4 = center.y + (num3 - 0.5f) * this.spawnSizeLocal.y;
				for (int j = 0; j < 4; j++)
				{
					float num5 = ((float)j + 0.5f) / 4f;
					for (int k = 0; k < 2; k++)
					{
						float num6 = ((float)k + 0.5f) / 2f;
						list.Add(new Vector3(center.x + (num6 - 0.5f) * this.spawnSizeLocal.x, num4, center.z + (num5 - 0.5f) * this.spawnSizeLocal.z));
					}
				}
			}
			for (int l = list.Count - 1; l > 0; l--)
			{
				int num7 = random.Next(l + 1);
				Vector3 vector = list[l];
				list[l] = list[num7];
				list[num7] = vector;
			}
			int num8 = Mathf.Min(requiredCount, list.Count);
			if (list.Count > num8)
			{
				list.RemoveRange(num8, list.Count - num8);
			}
			for (int m = 0; m < list.Count; m++)
			{
				Vector3 vector2 = list[m];
				vector2.x += GachaPlushField.NextFloat(random, -this.horizontalSpawnJitter, this.horizontalSpawnJitter);
				vector2.z += GachaPlushField.NextFloat(random, -this.horizontalSpawnJitter, this.horizontalSpawnJitter);
				if (num2 == 1)
				{
					vector2.y += GachaPlushField.NextFloat(random, -this.spawnSizeLocal.y * 0.5f, this.spawnSizeLocal.y * 0.5f);
				}
				list[m] = vector2;
			}
			return list;
		}

		// Token: 0x06008DBF RID: 36287 RVA: 0x0039E440 File Offset: 0x0039C640
		private void PrepareBodyForRestaging(Rigidbody body)
		{
			if (body == null)
			{
				return;
			}
			if (!body.isKinematic)
			{
				body.velocity = Vector3.zero;
				body.angularVelocity = Vector3.zero;
				body.Sleep();
				body.isKinematic = true;
			}
			body.useGravity = false;
			body.detectCollisions = false;
			this.ConfigureDynamicBody(body);
		}

		// Token: 0x06008DC0 RID: 36288 RVA: 0x0039E498 File Offset: 0x0039C698
		private void ConfigureDynamicBody(Rigidbody body)
		{
			body.drag = this.bodyDrag;
			body.angularDrag = this.bodyAngularDrag;
			body.interpolation = RigidbodyInterpolation.Interpolate;
			body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			body.maxDepenetrationVelocity = this.maximumDepenetrationVelocity;
			body.maxAngularVelocity = this.maximumAngularVelocity;
			body.solverIterations = this.solverIterations;
			body.solverVelocityIterations = this.solverVelocityIterations;
		}

		// Token: 0x06008DC1 RID: 36289 RVA: 0x0039E4FB File Offset: 0x0039C6FB
		private void CaptureAuthoredBodySnapshots()
		{
			this.CaptureAuthoredBodySnapshots(this.leftPlushBodies);
			this.CaptureAuthoredBodySnapshots(this.rightPlushBodies);
		}

		// Token: 0x06008DC2 RID: 36290 RVA: 0x0039E518 File Offset: 0x0039C718
		private void CaptureAuthoredBodySnapshots(Rigidbody[] bodies)
		{
			if (bodies == null)
			{
				return;
			}
			foreach (Rigidbody rigidbody in bodies)
			{
				if (!(rigidbody == null))
				{
					bool flag = false;
					for (int j = 0; j < this._authoredBodySnapshots.Count; j++)
					{
						if (!(this._authoredBodySnapshots[j].body != rigidbody))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this._authoredBodySnapshots.Add(new GachaPlushField.PlushBodySnapshot(rigidbody));
					}
				}
			}
		}

		// Token: 0x06008DC3 RID: 36291 RVA: 0x0039E590 File Offset: 0x0039C790
		private void RestoreAllBodiesToAuthoredState()
		{
			this.CaptureAuthoredBodySnapshots();
			for (int i = 0; i < this._authoredBodySnapshots.Count; i++)
			{
				this._authoredBodySnapshots[i].Restore();
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008DC4 RID: 36292 RVA: 0x0039E5D4 File Offset: 0x0039C7D4
		private void RecoverBodiesBelowFloor(Rigidbody[] bodies)
		{
			if (bodies == null)
			{
				return;
			}
			Vector3 normalized = base.transform.up.normalized;
			Vector3 vector = base.transform.TransformPoint(new Vector3(0f, this.interiorFloorTopLocal, 0f));
			foreach (Rigidbody rigidbody in bodies)
			{
				if (!(rigidbody == null) && !rigidbody.isKinematic && rigidbody.detectCollisions)
				{
					Collider component = rigidbody.GetComponent<Collider>();
					if (!(component == null))
					{
						Bounds bounds = component.bounds;
						float num = Mathf.Abs(normalized.x) * bounds.extents.x + Mathf.Abs(normalized.y) * bounds.extents.y + Mathf.Abs(normalized.z) * bounds.extents.z;
						float num2 = Vector3.Dot(bounds.center - vector, normalized) - num;
						if (num2 < -this.recoveryDepth)
						{
							Vector3 vector2 = base.transform.InverseTransformPoint(rigidbody.position);
							vector2.x = Mathf.Clamp(vector2.x, -0.65f, 0.65f);
							vector2.z = Mathf.Clamp(vector2.z, -0.65f, 0.62f);
							rigidbody.position = base.transform.TransformPoint(vector2) + normalized * Mathf.Max(0f, this.recoveryClearance - num2);
							float num3 = Vector3.Dot(rigidbody.velocity, normalized);
							if (num3 < 0f)
							{
								rigidbody.velocity -= normalized * num3;
							}
							rigidbody.angularVelocity *= 0.5f;
							if (this.debugLogs)
							{
								Debug.Log("[GachaPlushField] Recovered tunneled plush '" + rigidbody.name + "'.", rigidbody);
							}
						}
					}
				}
			}
		}

		// Token: 0x06008DC5 RID: 36293 RVA: 0x0039E7D8 File Offset: 0x0039C9D8
		private void EnableAnimatedPlayerCollisionProxies(Transform player, Animator resolvedPlayerAnimator)
		{
			if (player == null)
			{
				return;
			}
			Animator animator = ((resolvedPlayerAnimator != null && resolvedPlayerAnimator.isHuman) ? resolvedPlayerAnimator : player.GetComponent<Animator>());
			if (animator == null || !animator.isHuman)
			{
				Animator[] componentsInChildren = player.GetComponentsInChildren<Animator>(true);
				animator = null;
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (!(componentsInChildren[i] == null) && componentsInChildren[i].isHuman)
					{
						animator = componentsInChildren[i];
						break;
					}
				}
			}
			if (animator == null)
			{
				return;
			}
			this._sharedPlayerCollisionRig = GachaCielCollisionRig.GetOrCreate(player);
			if (this._sharedPlayerCollisionRig != null && this._sharedPlayerCollisionRig.Acquire(this, animator))
			{
				IReadOnlyList<Collider> colliders = this._sharedPlayerCollisionRig.Colliders;
				for (int j = 0; j < colliders.Count; j++)
				{
					Collider collider = colliders[j];
					if (collider != null && collider.enabled && !collider.isTrigger)
					{
						this._activePlayerCollisionProxies.Add(collider);
					}
				}
				if (this._activePlayerCollisionProxies.Count > 0)
				{
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[GachaPlushField] Using {0} ", this._activePlayerCollisionProxies.Count) + "shared Ciel collision regions (including dedicated breast volumes).", this);
					}
					return;
				}
				this._sharedPlayerCollisionRig.Release(this);
				this._sharedPlayerCollisionRig = null;
			}
			for (int k = 0; k < GachaPlushField.AnimatedCollisionBones.Length; k++)
			{
				HumanBodyBones humanBodyBones = GachaPlushField.AnimatedCollisionBones[k];
				Transform boneTransform = animator.GetBoneTransform(humanBodyBones);
				if (!(boneTransform == null))
				{
					Rigidbody component = boneTransform.GetComponent<Rigidbody>();
					bool flag = false;
					if (component != null && component.isKinematic)
					{
						foreach (Collider collider2 in boneTransform.GetComponents<Collider>())
						{
							if (!(collider2 == null) && !(collider2.attachedRigidbody != component) && this._activePlayerCollisionProxies.Add(collider2))
							{
								this._playerColliderSnapshots.Add(new GachaPlushField.PlayerColliderSnapshot(collider2, collider2.enabled, collider2.isTrigger));
								collider2.isTrigger = false;
								collider2.enabled = true;
								flag = true;
							}
						}
					}
					if (!flag && (component == null || component.isKinematic))
					{
						Collider collider3 = this.CreateGeneratedPlayerCollisionProxy(animator, humanBodyBones, boneTransform);
						if (collider3 != null)
						{
							this._activePlayerCollisionProxies.Add(collider3);
						}
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaPlushField] Enabled {0} ", this._activePlayerCollisionProxies.Count) + "kinematic Ciel collision proxies.", this);
			}
			if (this._activePlayerCollisionProxies.Count < GachaPlushField.AnimatedCollisionBones.Length)
			{
				Debug.LogWarning("[GachaPlushField] Ciel collision proxy is incomplete: " + string.Format("{0}/{1} shapes.", this._activePlayerCollisionProxies.Count, GachaPlushField.AnimatedCollisionBones.Length), this);
			}
		}

		// Token: 0x06008DC6 RID: 36294 RVA: 0x0039EAC8 File Offset: 0x0039CCC8
		private void RestoreAnimatedPlayerCollisionProxies()
		{
			for (int i = 0; i < this._playerColliderSnapshots.Count; i++)
			{
				GachaPlushField.PlayerColliderSnapshot playerColliderSnapshot = this._playerColliderSnapshots[i];
				if (!(playerColliderSnapshot.collider == null))
				{
					playerColliderSnapshot.collider.isTrigger = playerColliderSnapshot.wasTrigger;
					playerColliderSnapshot.collider.enabled = playerColliderSnapshot.wasEnabled;
				}
			}
			for (int j = 0; j < this._generatedPlayerCollisionProxies.Count; j++)
			{
				GameObject gameObject = this._generatedPlayerCollisionProxies[j];
				if (!(gameObject == null))
				{
					Collider component = gameObject.GetComponent<Collider>();
					if (component != null)
					{
						component.enabled = false;
					}
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject);
					}
					else
					{
						Object.DestroyImmediate(gameObject);
					}
				}
			}
			if (this._sharedPlayerCollisionRig != null)
			{
				this._sharedPlayerCollisionRig.Release(this);
				this._sharedPlayerCollisionRig = null;
			}
			this._playerColliderSnapshots.Clear();
			this._generatedPlayerCollisionProxies.Clear();
			this._activePlayerCollisionProxies.Clear();
		}

		// Token: 0x06008DC7 RID: 36295 RVA: 0x0039EBC8 File Offset: 0x0039CDC8
		private Collider CreateGeneratedPlayerCollisionProxy(Animator animator, HumanBodyBones boneId, Transform bone)
		{
			GameObject gameObject = new GameObject("__GachaPlushProxy_" + boneId.ToString())
			{
				hideFlags = HideFlags.DontSave
			};
			gameObject.transform.SetParent(bone, false);
			int num = LayerMask.NameToLayer("BodyPart");
			gameObject.layer = ((num >= 0) ? num : base.gameObject.layer);
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.mass = 1f;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
			rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			rigidbody.detectCollisions = true;
			Collider collider = null;
			switch (boneId)
			{
			case HumanBodyBones.Hips:
				collider = GachaPlushField.ConfigureGeneratedPelvis(animator, gameObject);
				break;
			case HumanBodyBones.LeftUpperLeg:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.LeftLowerLeg, 0.3f);
				break;
			case HumanBodyBones.RightUpperLeg:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.RightLowerLeg, 0.3f);
				break;
			case HumanBodyBones.LeftLowerLeg:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.LeftFoot, 0.25f);
				break;
			case HumanBodyBones.RightLowerLeg:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.RightFoot, 0.25f);
				break;
			case HumanBodyBones.Chest:
				collider = GachaPlushField.ConfigureGeneratedChest(animator, gameObject);
				break;
			case HumanBodyBones.Head:
				collider = GachaPlushField.ConfigureGeneratedHead(animator, gameObject);
				break;
			case HumanBodyBones.LeftUpperArm:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.LeftLowerArm, 0.25f);
				break;
			case HumanBodyBones.RightUpperArm:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.RightLowerArm, 0.25f);
				break;
			case HumanBodyBones.LeftLowerArm:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.LeftHand, 0.22f);
				break;
			case HumanBodyBones.RightLowerArm:
				collider = GachaPlushField.ConfigureGeneratedCapsule(animator, gameObject, HumanBodyBones.RightHand, 0.22f);
				break;
			}
			if (collider == null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(gameObject);
				}
				else
				{
					Object.DestroyImmediate(gameObject);
				}
				return null;
			}
			collider.isTrigger = false;
			collider.enabled = true;
			this._generatedPlayerCollisionProxies.Add(gameObject);
			return collider;
		}

		// Token: 0x06008DC8 RID: 36296 RVA: 0x0039ED94 File Offset: 0x0039CF94
		private static Collider ConfigureGeneratedCapsule(Animator animator, GameObject proxy, HumanBodyBones endBoneId, float radiusScale)
		{
			Transform boneTransform = animator.GetBoneTransform(endBoneId);
			if (boneTransform == null)
			{
				return null;
			}
			Vector3 vector = proxy.transform.InverseTransformPoint(boneTransform.position);
			int num = GachaPlushField.LargestComponent(vector);
			float num2 = vector[num];
			float num3 = Mathf.Abs(num2);
			if (num3 < 0.001f)
			{
				return null;
			}
			float num4 = Mathf.Max(0.025f, num3 * radiusScale);
			CapsuleCollider capsuleCollider = proxy.AddComponent<CapsuleCollider>();
			capsuleCollider.direction = num;
			capsuleCollider.center = GachaPlushField.AxisVector(num, num2 * 0.5f);
			capsuleCollider.radius = num4;
			capsuleCollider.height = Mathf.Max(num3, num4 * 2f);
			return capsuleCollider;
		}

		// Token: 0x06008DC9 RID: 36297 RVA: 0x0039EE38 File Offset: 0x0039D038
		private static Collider ConfigureGeneratedPelvis(Animator animator, GameObject proxy)
		{
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
			Transform boneTransform2 = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
			Transform boneTransform3 = animator.GetBoneTransform(HumanBodyBones.Chest);
			if (boneTransform == null || boneTransform2 == null || boneTransform3 == null)
			{
				return null;
			}
			Vector3 vector = Vector3.Lerp(proxy.transform.position, boneTransform3.position, 0.48f);
			Bounds bounds = GachaPlushField.BoundsFromWorldPoints(proxy.transform, new Vector3[] { boneTransform.position, boneTransform2.position, vector });
			GachaPlushField.ExpandThinAxis(ref bounds, 0.55f, 0.12f);
			BoxCollider boxCollider = proxy.AddComponent<BoxCollider>();
			boxCollider.center = bounds.center;
			boxCollider.size = GachaPlushField.MaxComponents(bounds.size, new Vector3(0.16f, 0.14f, 0.12f));
			return boxCollider;
		}

		// Token: 0x06008DCA RID: 36298 RVA: 0x0039EF18 File Offset: 0x0039D118
		private static Collider ConfigureGeneratedChest(Animator animator, GameObject proxy)
		{
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
			Transform boneTransform2 = animator.GetBoneTransform(HumanBodyBones.Head);
			Transform boneTransform3 = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
			Transform boneTransform4 = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			if (boneTransform == null || boneTransform2 == null || boneTransform3 == null || boneTransform4 == null)
			{
				return null;
			}
			Vector3 vector = Vector3.Lerp(boneTransform.position, proxy.transform.position, 0.52f);
			Vector3 vector2 = Vector3.Lerp(proxy.transform.position, boneTransform2.position, 0.48f);
			Bounds bounds = GachaPlushField.BoundsFromWorldPoints(proxy.transform, new Vector3[] { vector, vector2, boneTransform3.position, boneTransform4.position });
			GachaPlushField.ExpandThinAxis(ref bounds, 0.45f, 0.14f);
			BoxCollider boxCollider = proxy.AddComponent<BoxCollider>();
			boxCollider.center = bounds.center;
			boxCollider.size = GachaPlushField.MaxComponents(bounds.size, new Vector3(0.18f, 0.18f, 0.14f));
			return boxCollider;
		}

		// Token: 0x06008DCB RID: 36299 RVA: 0x0039F034 File Offset: 0x0039D234
		private static Collider ConfigureGeneratedHead(Animator animator, GameObject proxy)
		{
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
			Transform boneTransform2 = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
			Transform boneTransform3 = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			if (boneTransform == null || boneTransform2 == null || boneTransform3 == null)
			{
				return null;
			}
			float num = Mathf.Clamp(Vector3.Distance(boneTransform2.position, boneTransform3.position) * 0.22f, 0.09f, 0.18f);
			float num2 = Mathf.Max(new float[]
			{
				Mathf.Abs(proxy.transform.lossyScale.x),
				Mathf.Abs(proxy.transform.lossyScale.y),
				Mathf.Abs(proxy.transform.lossyScale.z)
			});
			SphereCollider sphereCollider = proxy.AddComponent<SphereCollider>();
			sphereCollider.radius = num / Mathf.Max(num2, 0.0001f);
			Vector3 vector = proxy.transform.InverseTransformDirection((proxy.transform.position - boneTransform.position).normalized);
			sphereCollider.center = vector * sphereCollider.radius * 0.35f;
			return sphereCollider;
		}

		// Token: 0x06008DCC RID: 36300 RVA: 0x0039F160 File Offset: 0x0039D360
		private void IgnoreNonProxyPlayerCollisions(Rigidbody[] selectedBodies, Transform player)
		{
			if (selectedBodies == null || player == null)
			{
				return;
			}
			Collider[] componentsInChildren = player.GetComponentsInChildren<Collider>(true);
			foreach (Rigidbody rigidbody in selectedBodies)
			{
				if (!(rigidbody == null))
				{
					foreach (Collider collider in rigidbody.GetComponentsInChildren<Collider>(true))
					{
						if (!(collider == null) && collider.enabled && !collider.isTrigger)
						{
							foreach (Collider collider2 in componentsInChildren)
							{
								if (!(collider2 == null) && collider2.enabled && !(collider2 == collider) && !this._activePlayerCollisionProxies.Contains(collider2) && !Physics.GetIgnoreCollision(collider, collider2))
								{
									Physics.IgnoreCollision(collider, collider2, true);
									this._ignoredPlayerCollisions.Add(new GachaPlushField.IgnoredCollisionPair(collider, collider2));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06008DCD RID: 36301 RVA: 0x0039F25C File Offset: 0x0039D45C
		private void RestorePlayerCollisions()
		{
			for (int i = 0; i < this._ignoredPlayerCollisions.Count; i++)
			{
				GachaPlushField.IgnoredCollisionPair ignoredCollisionPair = this._ignoredPlayerCollisions[i];
				if (ignoredCollisionPair.plushCollider != null && ignoredCollisionPair.playerCollider != null)
				{
					Physics.IgnoreCollision(ignoredCollisionPair.plushCollider, ignoredCollisionPair.playerCollider, false);
				}
			}
			this._ignoredPlayerCollisions.Clear();
		}

		// Token: 0x06008DCE RID: 36302 RVA: 0x0039F2C5 File Offset: 0x0039D4C5
		private Rigidbody[] BodiesForSide(GachaMachineSide side)
		{
			if (side != GachaMachineSide.Left)
			{
				return this.rightPlushBodies;
			}
			return this.leftPlushBodies;
		}

		// Token: 0x06008DCF RID: 36303 RVA: 0x0039F2D8 File Offset: 0x0039D4D8
		private Vector3 DisplacementCenterLocal(GachaMachineSide side)
		{
			Vector3 vector = this.ResolveSpawnCenterLocal(side);
			vector.y = 1.1f;
			return vector;
		}

		// Token: 0x06008DD0 RID: 36304 RVA: 0x0039F2FC File Offset: 0x0039D4FC
		private Vector3 ResolveSpawnCenterLocal(GachaMachineSide side)
		{
			Transform transform = ((side == GachaMachineSide.Left) ? this.leftDropAnchor : this.rightDropAnchor);
			if (transform != null)
			{
				return base.transform.InverseTransformPoint(transform.position);
			}
			if (side != GachaMachineSide.Left)
			{
				return this.rightSpawnCenterLocal;
			}
			return this.leftSpawnCenterLocal;
		}

		// Token: 0x06008DD1 RID: 36305 RVA: 0x0039F348 File Offset: 0x0039D548
		private Random CreateSessionRandom(GachaMachineSide side)
		{
			int num = ((side == GachaMachineSide.Left) ? 757602046 : 1510947265);
			return new Random((this.randomSeed != 0) ? (this.randomSeed ^ (this._sessionSerial * 486187739) ^ num) : (Environment.TickCount ^ base.GetInstanceID() ^ (this._sessionSerial * 486187739) ^ num));
		}

		// Token: 0x06008DD2 RID: 36306 RVA: 0x0039F3A4 File Offset: 0x0039D5A4
		private static float NextFloat(Random random, float minimum, float maximum)
		{
			if (maximum < minimum)
			{
				float num = minimum;
				minimum = maximum;
				maximum = num;
			}
			return Mathf.Lerp(minimum, maximum, (float)random.NextDouble());
		}

		// Token: 0x06008DD3 RID: 36307 RVA: 0x0039F3C0 File Offset: 0x0039D5C0
		private static Vector3 AxisVector(int axis, float value)
		{
			Vector3 zero = Vector3.zero;
			zero[axis] = value;
			return zero;
		}

		// Token: 0x06008DD4 RID: 36308 RVA: 0x0039F3E0 File Offset: 0x0039D5E0
		private static int LargestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) > Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) > Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06008DD5 RID: 36309 RVA: 0x0039F428 File Offset: 0x0039D628
		private static int SmallestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) < Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) < Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06008DD6 RID: 36310 RVA: 0x0039F470 File Offset: 0x0039D670
		private static Bounds BoundsFromWorldPoints(Transform relativeTo, params Vector3[] points)
		{
			Bounds bounds = new Bounds(relativeTo.InverseTransformPoint(points[0]), Vector3.zero);
			for (int i = 1; i < points.Length; i++)
			{
				bounds.Encapsulate(relativeTo.InverseTransformPoint(points[i]));
			}
			return bounds;
		}

		// Token: 0x06008DD7 RID: 36311 RVA: 0x0039F4BC File Offset: 0x0039D6BC
		private static void ExpandThinAxis(ref Bounds bounds, float fractionOfLargest, float minimum)
		{
			Vector3 size = bounds.size;
			int num = GachaPlushField.SmallestComponent(size);
			int num2 = GachaPlushField.LargestComponent(size);
			size[num] = Mathf.Max(size[num], Mathf.Max(size[num2] * fractionOfLargest, minimum));
			bounds.size = size;
		}

		// Token: 0x06008DD8 RID: 36312 RVA: 0x00119C8A File Offset: 0x00117E8A
		private static Vector3 MaxComponents(Vector3 value, Vector3 minimum)
		{
			return new Vector3(Mathf.Max(value.x, minimum.x), Mathf.Max(value.y, minimum.y), Mathf.Max(value.z, minimum.z));
		}

		// Token: 0x06008DD9 RID: 36313 RVA: 0x0039F50C File Offset: 0x0039D70C
		private static int CountValid(Rigidbody[] bodies)
		{
			if (bodies == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < bodies.Length; i++)
			{
				if (bodies[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008DDA RID: 36314 RVA: 0x0039F540 File Offset: 0x0039D740
		private void WarnAboutConfiguration()
		{
			int num = GachaPlushField.CountValid(this.leftPlushBodies);
			int num2 = GachaPlushField.CountValid(this.rightPlushBodies);
			if (num == 14 && num2 == 14)
			{
				return;
			}
			Debug.LogWarning(string.Format("[GachaPlushField] Expected {0} plush bodies per side, ", 14) + string.Format("but found Left={0}, Right={1}.", num, num2), this);
		}

		// Token: 0x06008DDB RID: 36315 RVA: 0x0039F5A4 File Offset: 0x0039D7A4
		private void OnValidate()
		{
			this.spawnSizeLocal.x = Mathf.Max(0.01f, Mathf.Abs(this.spawnSizeLocal.x));
			this.spawnSizeLocal.y = Mathf.Max(0f, Mathf.Abs(this.spawnSizeLocal.y));
			this.spawnSizeLocal.z = Mathf.Max(0.01f, Mathf.Abs(this.spawnSizeLocal.z));
			this.horizontalSpawnJitter = Mathf.Max(0f, this.horizontalSpawnJitter);
			this.horizontalReleaseSpeed = GachaPlushField.OrderedNonNegative(this.horizontalReleaseSpeed);
			this.verticalReleaseSpeed = GachaPlushField.Ordered(this.verticalReleaseSpeed);
			this.angularReleaseSpeed = GachaPlushField.OrderedNonNegative(this.angularReleaseSpeed);
			this.bodiesReleasedPerFixedStep = Mathf.Max(1, this.bodiesReleasedPerFixedStep);
			this.bodyDrag = Mathf.Max(0f, this.bodyDrag);
			this.bodyAngularDrag = Mathf.Max(0f, this.bodyAngularDrag);
			this.maximumDepenetrationVelocity = Mathf.Max(0.1f, this.maximumDepenetrationVelocity);
			this.maximumLinearVelocity = Mathf.Max(0.1f, this.maximumLinearVelocity);
			this.maximumAngularVelocity = Mathf.Max(0.1f, this.maximumAngularVelocity);
			this.solverIterations = Mathf.Max(1, this.solverIterations);
			this.solverVelocityIterations = Mathf.Max(1, this.solverVelocityIterations);
			this.recoveryDepth = Mathf.Max(0.005f, this.recoveryDepth);
			this.recoveryClearance = Mathf.Max(0.001f, this.recoveryClearance);
		}

		// Token: 0x06008DDC RID: 36316 RVA: 0x00326095 File Offset: 0x00324295
		private static Vector2 Ordered(Vector2 value)
		{
			if (value.x > value.y)
			{
				return new Vector2(value.y, value.x);
			}
			return value;
		}

		// Token: 0x06008DDD RID: 36317 RVA: 0x0039F739 File Offset: 0x0039D939
		private static Vector2 OrderedNonNegative(Vector2 value)
		{
			value.x = Mathf.Max(0f, value.x);
			value.y = Mathf.Max(0f, value.y);
			return GachaPlushField.Ordered(value);
		}

		// Token: 0x06008DDE RID: 36318 RVA: 0x0039F770 File Offset: 0x0039D970
		private void OnDrawGizmosSelected()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Color color = Gizmos.color;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0.2f, 0.75f, 1f, 0.85f);
			Gizmos.DrawWireCube(this.ResolveSpawnCenterLocal(GachaMachineSide.Left), this.spawnSizeLocal);
			Gizmos.color = new Color(1f, 0.35f, 0.75f, 0.85f);
			Gizmos.DrawWireCube(this.ResolveSpawnCenterLocal(GachaMachineSide.Right), this.spawnSizeLocal);
			Gizmos.matrix = matrix;
			Gizmos.color = color;
		}

		// Token: 0x06008DE1 RID: 36321 RVA: 0x0039F9FF File Offset: 0x0039DBFF
		[CompilerGenerated]
		private Vector3 <TryCollectBodyCoverPoints>g__BoneLocal|67_0(Transform bone, ref GachaPlushField.<>c__DisplayClass67_0 A_2)
		{
			return base.transform.InverseTransformPoint(bone.position);
		}

		// Token: 0x06008DE2 RID: 36322 RVA: 0x0039FA14 File Offset: 0x0039DC14
		[CompilerGenerated]
		private void <TryCollectBodyCoverPoints>g__AddCrestMidpoint|67_1(HumanBodyBones startId, HumanBodyBones endId, ref GachaPlushField.<>c__DisplayClass67_0 A_3)
		{
			Transform boneTransform = A_3.animator.GetBoneTransform(startId);
			Transform boneTransform2 = A_3.animator.GetBoneTransform(endId);
			if (boneTransform != null && boneTransform2 != null)
			{
				A_3.crest.Add(base.transform.InverseTransformPoint(Vector3.Lerp(boneTransform.position, boneTransform2.position, 0.5f)));
			}
		}

		// Token: 0x06008DE3 RID: 36323 RVA: 0x0039FA7C File Offset: 0x0039DC7C
		[CompilerGenerated]
		private Vector3 <BuildBodyCoveringColumns>g__MakePosition|68_0(Vector3 point, int layer, float jitter, ref GachaPlushField.<>c__DisplayClass68_0 A_4)
		{
			return new Vector3(Mathf.Clamp(point.x + GachaPlushField.NextFloat(A_4.random, -jitter, jitter), this.interiorMinLocal.x + 0.06f, this.interiorMaxLocal.x - 0.06f), A_4.baseY + (float)layer * this.bodyCoverLayerHeight, Mathf.Clamp(point.z + GachaPlushField.NextFloat(A_4.random, -jitter, jitter), this.interiorMinLocal.z + 0.06f, this.interiorMaxLocal.z - 0.06f));
		}

		// Token: 0x040090CB RID: 37067
		private const int ExpectedBodiesPerSide = 14;

		// Token: 0x040090CC RID: 37068
		private const int GridColumns = 2;

		// Token: 0x040090CD RID: 37069
		private const int GridRows = 4;

		// Token: 0x040090CE RID: 37070
		private static readonly HumanBodyBones[] AnimatedCollisionBones = new HumanBodyBones[]
		{
			HumanBodyBones.Hips,
			HumanBodyBones.Chest,
			HumanBodyBones.Head,
			HumanBodyBones.LeftUpperLeg,
			HumanBodyBones.LeftLowerLeg,
			HumanBodyBones.RightUpperLeg,
			HumanBodyBones.RightLowerLeg,
			HumanBodyBones.LeftUpperArm,
			HumanBodyBones.LeftLowerArm,
			HumanBodyBones.RightUpperArm,
			HumanBodyBones.RightLowerArm
		};

		// Token: 0x040090CF RID: 37071
		[Header("Persistent Plush Wrappers")]
		[Tooltip("Pre-authored Rigidbody wrappers assigned to the left bay (14: one of each plush prefab).")]
		[SerializeField]
		private Rigidbody[] leftPlushBodies = Array.Empty<Rigidbody>();

		// Token: 0x040090D0 RID: 37072
		[Tooltip("Pre-authored Rigidbody wrappers assigned to the right bay (14: one of each plush prefab).")]
		[SerializeField]
		private Rigidbody[] rightPlushBodies = Array.Empty<Rigidbody>();

		// Token: 0x040090D1 RID: 37073
		[Header("Local Spawn Volumes")]
		[Tooltip("Authorable anchor parented under InsidePose_L. Falls back to Left Spawn Center Local.")]
		[SerializeField]
		private Transform leftDropAnchor;

		// Token: 0x040090D2 RID: 37074
		[Tooltip("Authorable anchor parented under InsidePose_R. Falls back to Right Spawn Center Local.")]
		[SerializeField]
		private Transform rightDropAnchor;

		// Token: 0x040090D3 RID: 37075
		[SerializeField]
		private Vector3 leftSpawnCenterLocal = new Vector3(-0.43f, 1.52f, -0.05f);

		// Token: 0x040090D4 RID: 37076
		[SerializeField]
		private Vector3 rightSpawnCenterLocal = new Vector3(0.43f, 1.52f, -0.05f);

		// Token: 0x040090D5 RID: 37077
		[Tooltip("Full local-space volume. Grid cells use their centers, leaving clearance at its edges. Tall enough for three stacked release layers that rain down over Ciel while staying clear of the ceiling.")]
		[SerializeField]
		private Vector3 spawnSizeLocal = new Vector3(0.62f, 0.45f, 1.05f);

		// Token: 0x040090D6 RID: 37078
		[SerializeField]
		[Min(0f)]
		private float horizontalSpawnJitter = 0.025f;

		// Token: 0x040090D7 RID: 37079
		[SerializeField]
		[Range(0f, 45f)]
		private float maximumSpawnTilt = 24f;

		// Token: 0x040090D8 RID: 37080
		[Header("Displacement Impulse")]
		[Tooltip("Keep this near zero: the toys must land ON Ciel to bury her, not scatter away from her body.")]
		[SerializeField]
		private Vector2 horizontalReleaseSpeed = new Vector2(0f, 0.1f);

		// Token: 0x040090D9 RID: 37081
		[SerializeField]
		private Vector2 verticalReleaseSpeed = new Vector2(-0.05f, 0.12f);

		// Token: 0x040090DA RID: 37082
		[SerializeField]
		private Vector2 angularReleaseSpeed = new Vector2(1.5f, 4f);

		// Token: 0x040090DB RID: 37083
		[SerializeField]
		[Min(1f)]
		private int bodiesReleasedPerFixedStep = 4;

		// Token: 0x040090DC RID: 37084
		[Header("Body Covering")]
		[Tooltip("Drop the toys in columns aimed at Ciel's posed bones so she ends up buried; the plain spawn grid is only a fallback when no humanoid animator is available.")]
		[SerializeField]
		private bool targetSpawnOverBody = true;

		// Token: 0x040090DD RID: 37085
		[Tooltip("First pass fills the valleys hugging her flanks so later toys landing ON her have shoulders to rest against instead of rolling off her sides.")]
		[SerializeField]
		[Min(0.05f)]
		private float bodyRingOffset = 0.15f;

		// Token: 0x040090DE RID: 37086
		[SerializeField]
		[Min(0f)]
		private float bodyCoverJitter = 0.05f;

		// Token: 0x040090DF RID: 37087
		[SerializeField]
		[Min(0.08f)]
		private float bodyCoverLayerHeight = 0.15f;

		// Token: 0x040090E0 RID: 37088
		[Header("Physics Safety")]
		[SerializeField]
		[Min(0f)]
		private float bodyDrag = 0.3f;

		// Token: 0x040090E1 RID: 37089
		[SerializeField]
		[Min(0f)]
		private float bodyAngularDrag = 1.1f;

		// Token: 0x040090E2 RID: 37090
		[Tooltip("Keep this low: Ciel's collision proxies follow animation and Magica breast motion by teleporting every frame, and each overlap they cause ejects a 100 g toy at this speed.")]
		[SerializeField]
		[Min(0.1f)]
		private float maximumDepenetrationVelocity = 1.2f;

		// Token: 0x040090E3 RID: 37091
		[Tooltip("Hard ceiling on plush speed inside the cabinet; anything faster than a strong toss is a solver artifact.")]
		[SerializeField]
		[Min(0.1f)]
		private float maximumLinearVelocity = 2.6f;

		// Token: 0x040090E4 RID: 37092
		[SerializeField]
		[Min(0.1f)]
		private float maximumAngularVelocity = 8f;

		// Token: 0x040090E5 RID: 37093
		[SerializeField]
		[Min(1f)]
		private int solverIterations = 12;

		// Token: 0x040090E6 RID: 37094
		[SerializeField]
		[Min(1f)]
		private int solverVelocityIterations = 4;

		// Token: 0x040090E7 RID: 37095
		[SerializeField]
		private float interiorFloorTopLocal = 0.84938f;

		// Token: 0x040090E8 RID: 37096
		[SerializeField]
		[Min(0.005f)]
		private float recoveryDepth = 0.03f;

		// Token: 0x040090E9 RID: 37097
		[SerializeField]
		[Min(0.001f)]
		private float recoveryClearance = 0.006f;

		// Token: 0x040090EA RID: 37098
		[Tooltip("Interior play volume in machine-local space, a few cm inside the shell's inner faces. Any toy whose PIVOT drifts outside (wedged into or through a wall) is pulled back in with its escape velocity cancelled.")]
		[SerializeField]
		private Vector3 interiorMinLocal = new Vector3(-0.79f, 0.78f, -0.79f);

		// Token: 0x040090EB RID: 37099
		[SerializeField]
		private Vector3 interiorMaxLocal = new Vector3(0.79f, 1.8f, 0.77f);

		// Token: 0x040090EC RID: 37100
		[Tooltip("Zero uses runtime entropy. A non-zero value is useful for deterministic tests.")]
		[SerializeField]
		private int randomSeed;

		// Token: 0x040090ED RID: 37101
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040090EE RID: 37102
		private readonly List<GachaPlushField.IgnoredCollisionPair> _ignoredPlayerCollisions = new List<GachaPlushField.IgnoredCollisionPair>(128);

		// Token: 0x040090EF RID: 37103
		private readonly List<GachaPlushField.PlayerColliderSnapshot> _playerColliderSnapshots = new List<GachaPlushField.PlayerColliderSnapshot>(GachaPlushField.AnimatedCollisionBones.Length);

		// Token: 0x040090F0 RID: 37104
		private readonly HashSet<Collider> _activePlayerCollisionProxies = new HashSet<Collider>();

		// Token: 0x040090F1 RID: 37105
		private readonly List<GameObject> _generatedPlayerCollisionProxies = new List<GameObject>(GachaPlushField.AnimatedCollisionBones.Length);

		// Token: 0x040090F2 RID: 37106
		private readonly List<GachaPlushField.PlushBodySnapshot> _authoredBodySnapshots = new List<GachaPlushField.PlushBodySnapshot>(28);

		// Token: 0x040090F3 RID: 37107
		private GachaCielCollisionRig _sharedPlayerCollisionRig;

		// Token: 0x040090F4 RID: 37108
		private Coroutine _releaseRoutine;

		// Token: 0x040090F5 RID: 37109
		private int _sessionGeneration;

		// Token: 0x040090F6 RID: 37110
		private int _sessionSerial;

		// Token: 0x040090F7 RID: 37111
		private bool _sessionActive;

		// Token: 0x020011F2 RID: 4594
		private readonly struct PlushBodySnapshot
		{
			// Token: 0x06008DE4 RID: 36324 RVA: 0x0039FB1C File Offset: 0x0039DD1C
			public PlushBodySnapshot(Rigidbody body)
			{
				this.body = body;
				Transform transform = body.transform;
				this._localPosition = transform.localPosition;
				this._localRotation = transform.localRotation;
				this._localScale = transform.localScale;
				this._mass = body.mass;
				this._drag = body.drag;
				this._angularDrag = body.angularDrag;
				this._useGravity = body.useGravity;
				this._interpolation = body.interpolation;
				this._collisionDetectionMode = body.collisionDetectionMode;
				this._constraints = body.constraints;
				this._detectCollisions = body.detectCollisions;
				this._maxDepenetrationVelocity = body.maxDepenetrationVelocity;
				this._maxAngularVelocity = body.maxAngularVelocity;
				this._solverIterations = body.solverIterations;
				this._solverVelocityIterations = body.solverVelocityIterations;
				Collider[] componentsInChildren = body.GetComponentsInChildren<Collider>(true);
				this._colliders = new GachaPlushField.PlushColliderSnapshot[componentsInChildren.Length];
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this._colliders[i] = new GachaPlushField.PlushColliderSnapshot(componentsInChildren[i]);
				}
			}

			// Token: 0x06008DE5 RID: 36325 RVA: 0x0039FC24 File Offset: 0x0039DE24
			public void Restore()
			{
				if (this.body == null)
				{
					return;
				}
				if (!this.body.isKinematic)
				{
					this.body.velocity = Vector3.zero;
					this.body.angularVelocity = Vector3.zero;
				}
				this.body.isKinematic = true;
				Transform transform = this.body.transform;
				transform.localScale = this._localScale;
				transform.localPosition = this._localPosition;
				transform.localRotation = this._localRotation;
				this.body.position = transform.position;
				this.body.rotation = transform.rotation;
				this.body.mass = this._mass;
				this.body.drag = this._drag;
				this.body.angularDrag = this._angularDrag;
				this.body.useGravity = this._useGravity;
				this.body.interpolation = this._interpolation;
				this.body.collisionDetectionMode = this._collisionDetectionMode;
				this.body.constraints = this._constraints;
				this.body.detectCollisions = this._detectCollisions;
				this.body.maxDepenetrationVelocity = this._maxDepenetrationVelocity;
				this.body.maxAngularVelocity = this._maxAngularVelocity;
				this.body.solverIterations = this._solverIterations;
				this.body.solverVelocityIterations = this._solverVelocityIterations;
				this.body.isKinematic = true;
				if (Application.isPlaying)
				{
					this.body.Sleep();
				}
				for (int i = 0; i < this._colliders.Length; i++)
				{
					this._colliders[i].Restore();
				}
			}

			// Token: 0x040090F8 RID: 37112
			public readonly Rigidbody body;

			// Token: 0x040090F9 RID: 37113
			private readonly Vector3 _localPosition;

			// Token: 0x040090FA RID: 37114
			private readonly Quaternion _localRotation;

			// Token: 0x040090FB RID: 37115
			private readonly Vector3 _localScale;

			// Token: 0x040090FC RID: 37116
			private readonly float _mass;

			// Token: 0x040090FD RID: 37117
			private readonly float _drag;

			// Token: 0x040090FE RID: 37118
			private readonly float _angularDrag;

			// Token: 0x040090FF RID: 37119
			private readonly bool _useGravity;

			// Token: 0x04009100 RID: 37120
			private readonly RigidbodyInterpolation _interpolation;

			// Token: 0x04009101 RID: 37121
			private readonly CollisionDetectionMode _collisionDetectionMode;

			// Token: 0x04009102 RID: 37122
			private readonly RigidbodyConstraints _constraints;

			// Token: 0x04009103 RID: 37123
			private readonly bool _detectCollisions;

			// Token: 0x04009104 RID: 37124
			private readonly float _maxDepenetrationVelocity;

			// Token: 0x04009105 RID: 37125
			private readonly float _maxAngularVelocity;

			// Token: 0x04009106 RID: 37126
			private readonly int _solverIterations;

			// Token: 0x04009107 RID: 37127
			private readonly int _solverVelocityIterations;

			// Token: 0x04009108 RID: 37128
			private readonly GachaPlushField.PlushColliderSnapshot[] _colliders;
		}

		// Token: 0x020011F3 RID: 4595
		private readonly struct PlushColliderSnapshot
		{
			// Token: 0x06008DE6 RID: 36326 RVA: 0x0039FDD9 File Offset: 0x0039DFD9
			public PlushColliderSnapshot(Collider collider)
			{
				this._collider = collider;
				this._enabled = collider != null && collider.enabled;
				this._isTrigger = collider != null && collider.isTrigger;
			}

			// Token: 0x06008DE7 RID: 36327 RVA: 0x0039FE12 File Offset: 0x0039E012
			public void Restore()
			{
				if (this._collider == null)
				{
					return;
				}
				this._collider.isTrigger = this._isTrigger;
				this._collider.enabled = this._enabled;
			}

			// Token: 0x04009109 RID: 37129
			private readonly Collider _collider;

			// Token: 0x0400910A RID: 37130
			private readonly bool _enabled;

			// Token: 0x0400910B RID: 37131
			private readonly bool _isTrigger;
		}

		// Token: 0x020011F4 RID: 4596
		private readonly struct ReleaseInstruction
		{
			// Token: 0x06008DE8 RID: 36328 RVA: 0x0039FE45 File Offset: 0x0039E045
			public ReleaseInstruction(Rigidbody body, Vector3 velocity, Vector3 angularVelocity)
			{
				this.body = body;
				this.velocity = velocity;
				this.angularVelocity = angularVelocity;
			}

			// Token: 0x0400910C RID: 37132
			public readonly Rigidbody body;

			// Token: 0x0400910D RID: 37133
			public readonly Vector3 velocity;

			// Token: 0x0400910E RID: 37134
			public readonly Vector3 angularVelocity;
		}

		// Token: 0x020011F5 RID: 4597
		private readonly struct IgnoredCollisionPair
		{
			// Token: 0x06008DE9 RID: 36329 RVA: 0x0039FE5C File Offset: 0x0039E05C
			public IgnoredCollisionPair(Collider plushCollider, Collider playerCollider)
			{
				this.plushCollider = plushCollider;
				this.playerCollider = playerCollider;
			}

			// Token: 0x0400910F RID: 37135
			public readonly Collider plushCollider;

			// Token: 0x04009110 RID: 37136
			public readonly Collider playerCollider;
		}

		// Token: 0x020011F6 RID: 4598
		private readonly struct PlayerColliderSnapshot
		{
			// Token: 0x06008DEA RID: 36330 RVA: 0x0039FE6C File Offset: 0x0039E06C
			public PlayerColliderSnapshot(Collider collider, bool wasEnabled, bool wasTrigger)
			{
				this.collider = collider;
				this.wasEnabled = wasEnabled;
				this.wasTrigger = wasTrigger;
			}

			// Token: 0x04009111 RID: 37137
			public readonly Collider collider;

			// Token: 0x04009112 RID: 37138
			public readonly bool wasEnabled;

			// Token: 0x04009113 RID: 37139
			public readonly bool wasTrigger;
		}
	}
}
