using System;
using System.Collections;
using System.Reflection;
using Features.DebugTools;
using Invector.vCharacterController;
using Invector.vCover;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009FB RID: 2555
	public class DynamicWallCoverScanner : MonoBehaviour
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060048C2 RID: 18626 RVA: 0x001BB3E4 File Offset: 0x001B95E4
		// (set) Token: 0x060048C3 RID: 18627 RVA: 0x001BB3F4 File Offset: 0x001B95F4
		public static bool Enabled
		{
			get
			{
				return PlayerPrefs.GetInt("CoverWall.DynamicWallCover.Enabled", 1) == 1;
			}
			set
			{
				PlayerPrefs.SetInt("CoverWall.DynamicWallCover.Enabled", value ? 1 : 0);
				PlayerPrefs.Save();
			}
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x001BB40C File Offset: 0x001B960C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (Object.FindObjectOfType<DynamicWallCoverScanner>(true) != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[DynamicWallCover]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<DynamicWallCoverScanner>();
			gameObject.AddComponent<CoverCrouchToggle>();
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x001BB43A File Offset: 0x001B963A
		private void OnEnable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x001BB45E File Offset: 0x001B965E
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (this.shuttingDown)
			{
				return;
			}
			this.ForceExitOwnedCover();
			this.HideCover();
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x001BB486 File Offset: 0x001B9686
		private void OnApplicationQuit()
		{
			this.shuttingDown = true;
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x001BB490 File Offset: 0x001B9690
		private void OnDestroy()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (this.relinkRoutine != null)
			{
				base.StopCoroutine(this.relinkRoutine);
				this.relinkRoutine = null;
			}
			this.ClearPossibleOwnedCoverPoint();
			if (!this.shuttingDown && this.cover != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.cover.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this.cover.gameObject);
				}
			}
			this.cover = null;
			this.coverVisual = null;
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x001BB51C File Offset: 0x001B971C
		private void OnActiveSceneChanged(Scene previous, Scene next)
		{
			if (this.shuttingDown)
			{
				return;
			}
			this.ForceExitOwnedCover();
			this.HideCover();
			this.player = null;
			this.controller = null;
			this.motor = null;
			this.nextScanTime = 0f;
			this.nextActiveValidationTime = 0f;
			this.activeValidationMisses = 0;
			this.noFloorSince = -1f;
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x001BB57C File Offset: 0x001B977C
		private void Update()
		{
			if (!DynamicWallCoverScanner.Enabled || !DynamicWallCoverScanner.IsSupportedScene(SceneManager.GetActiveScene()))
			{
				this.ForceExitOwnedCover();
				this.HideCover();
				return;
			}
			if (!this.ResolvePlayer())
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this.controller.inCover || this.controller.goingToCoverPoint)
			{
				if (this.controller.inCover && this.IsUsingOwnedCover())
				{
					this.GuardFloorWhileInCover();
					this.GuardWallWhileInCover();
				}
				return;
			}
			this.noFloorSince = -1f;
			vCoverPoint possibleCoverPoint = this.controller.possibleCoverPoint;
			if (possibleCoverPoint != null && (this.cover == null || !possibleCoverPoint.transform.IsChildOf(this.cover.transform)))
			{
				this.HideCover();
				return;
			}
			if (this.coverActive && this.IsPlayerOnSegment())
			{
				if (Time.time < this.nextActiveValidationTime)
				{
					return;
				}
				this.nextActiveValidationTime = Time.time + Mathf.Max(0.1f, this.ScanInterval * 2f);
				if (this.IsCurrentWallStillValid())
				{
					this.activeValidationMisses = 0;
					return;
				}
				int num = this.activeValidationMisses + 1;
				this.activeValidationMisses = num;
				if (num < 2)
				{
					return;
				}
			}
			this.activeValidationMisses = 0;
			if (Time.time < this.nextScanTime)
			{
				return;
			}
			this.nextScanTime = Time.time + this.ScanInterval;
			this.Scan();
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x001BB6D8 File Offset: 0x001B98D8
		private void GuardFloorWhileInCover()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.player.position + Vector3.up * 0.3f, Vector3.down, out raycastHit, 0.9f, this.floorMask, QueryTriggerInteraction.Ignore) && this.IsValidFloorHit(raycastHit))
			{
				this.noFloorSince = -1f;
				return;
			}
			if (this.noFloorSince < 0f)
			{
				this.noFloorSince = Time.time;
				return;
			}
			if (Time.time - this.noFloorSince < 0.5f)
			{
				return;
			}
			this.noFloorSince = -1f;
			Debug.LogWarning("[DynamicWallCover] No floor under Ciel while in dynamic cover: forcing exit.", this.cover);
			this.ForceExitOwnedCover();
			this.HideCover();
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x001BB78F File Offset: 0x001B998F
		internal static bool IsSupportedScene(Scene scene)
		{
			return scene.IsValid() && scene.name != "GameOver - Prison";
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x001BB7B0 File Offset: 0x001B99B0
		private void GuardWallWhileInCover()
		{
			if (Time.time < this.nextActiveValidationTime)
			{
				return;
			}
			this.nextActiveValidationTime = Time.time + Mathf.Max(0.1f, this.ScanInterval * 2f);
			if (this.IsCurrentWallStillValid())
			{
				this.activeValidationMisses = 0;
				return;
			}
			int num = this.activeValidationMisses + 1;
			this.activeValidationMisses = num;
			if (num < 2)
			{
				return;
			}
			this.activeValidationMisses = 0;
			Debug.LogWarning("[DynamicWallCover] Runtime wall disappeared while Ciel was in cover: forcing exit.", this.cover);
			this.ForceExitOwnedCover();
			this.HideCover();
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x001BB838 File Offset: 0x001B9A38
		private bool ResolvePlayer()
		{
			Transform transform = PlayerRegistry.Player;
			if (transform == null)
			{
				vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
				transform = ((vThirdPersonController != null) ? vThirdPersonController.transform : null);
			}
			if (transform == null)
			{
				if (this.player != null || this.controller != null)
				{
					this.ForceExitOwnedCover();
					this.HideCover();
					this.player = null;
					this.controller = null;
					this.motor = null;
				}
				return false;
			}
			if (transform != this.player || this.controller == null)
			{
				if (this.player != null && transform != this.player)
				{
					this.ForceExitOwnedCover();
					this.HideCover();
				}
				this.player = transform;
				this.controller = this.player.GetComponent<vBasicCoverController>() ?? this.player.GetComponentInChildren<vBasicCoverController>(true);
				this.motor = this.player.GetComponent<vThirdPersonController>() ?? this.player.GetComponentInChildren<vThirdPersonController>(true);
			}
			if (this.controller != null)
			{
				int value = this.controller.obstaclesLayer.value;
				int num = ((this.motor != null) ? this.motor.groundLayer.value : 0);
				int num2 = LayerMask.NameToLayer("Floor");
				int num3 = ((num2 >= 0) ? (1 << num2) : 0);
				this.wallMask = ((value != 0) ? value : 1);
				this.floorMask = ((num != 0) ? num : value) | num3;
				if (this.floorMask.value == 0)
				{
					this.floorMask = 1 | num3;
				}
				this.clearanceMask = this.wallMask.value | this.floorMask.value;
			}
			else
			{
				this.HideCover();
				this.player = null;
				this.motor = null;
			}
			return this.controller != null;
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x001BBA2C File Offset: 0x001B9C2C
		private bool IsUsingOwnedCover()
		{
			if (!this.coverActive || this.cover == null || this.controller == null)
			{
				return false;
			}
			vCoverPoint currentCoverPoint = this.controller.currentCoverPoint;
			return currentCoverPoint != null && currentCoverPoint.transform.IsChildOf(this.cover.transform);
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x001BBA8C File Offset: 0x001B9C8C
		private void ForceExitOwnedCover()
		{
			if (this.controller == null || (!this.controller.inCover && !this.controller.goingToCoverPoint))
			{
				return;
			}
			bool flag = this.controller.currentCoverPoint != null && this.cover != null && this.controller.currentCoverPoint.transform.IsChildOf(this.cover.transform);
			bool flag2 = this.controller.pendingCoverPoint != null && this.cover != null && this.controller.pendingCoverPoint.transform.IsChildOf(this.cover.transform);
			if (!flag && !flag2)
			{
				return;
			}
			if (DynamicWallCoverScanner.exitCoverMethod == null)
			{
				DynamicWallCoverScanner.exitCoverMethod = typeof(vBasicCoverController).GetMethod("ExitCover", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			MethodInfo methodInfo = DynamicWallCoverScanner.exitCoverMethod;
			if (methodInfo == null)
			{
				return;
			}
			methodInfo.Invoke(this.controller, new object[] { true });
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x001BBBA0 File Offset: 0x001B9DA0
		private bool IsPlayerOnSegment()
		{
			Vector3 vector = this.player.position - this.segCenter;
			float num = Vector3.Dot(vector, this.segTangent);
			float num2 = Vector3.Dot(vector, this.segNormal);
			return Mathf.Abs(num) <= this.segHalfLength + this.KeepMargin && num2 > -0.5f && num2 < this.KeepDistanceFromWall && Mathf.Abs(vector.y) < 1.5f;
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x001BBC18 File Offset: 0x001B9E18
		private bool IsCurrentWallStillValid()
		{
			if (this.player == null)
			{
				return false;
			}
			Vector3 vector = this.player.position + Vector3.up * this.activeProbeHeight;
			float num = Mathf.Max(0.1f, this.MaxWallDistance + this.KeepMargin);
			float num2 = Mathf.Max(0.2f, this.WallDistanceTolerance + 0.05f);
			int num3 = Physics.RaycastNonAlloc(vector, -this.segNormal, this.seedHits, num, this.wallMask, QueryTriggerInteraction.Ignore);
			bool flag = false;
			for (int i = 0; i < num3; i++)
			{
				RaycastHit raycastHit = this.seedHits[i];
				Vector3 vector2 = DynamicWallCoverScanner.OrientNormalAgainstRay(raycastHit.normal, -this.segNormal);
				if (this.IsValidWallHit(raycastHit) && Vector3.Dot(vector2, this.segNormal) >= this.ProbeNormalDot && Mathf.Abs(Vector3.Dot(raycastHit.point - this.segCenter, this.segNormal)) <= num2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			float num4 = Mathf.Clamp(Vector3.Dot(this.player.position - this.segCenter, this.segTangent), -this.segHalfLength, this.segHalfLength);
			Vector3 vector3 = this.segCenter + this.segTangent * num4;
			float num5;
			Vector3 vector4;
			return this.TryGetFloorY(vector3, this.segNormal, this.player.position.y, out num5, out vector4) && Mathf.Abs(num5 - this.player.position.y) <= this.MaxFloorDelta && this.IsClear(vector3, this.segNormal, num5, vector4);
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x001BBDD8 File Offset: 0x001B9FD8
		private void Scan()
		{
			int num = this.FindSeedWalls();
			int i = 0;
			while (i < num)
			{
				DynamicWallCoverScanner.SegmentBuild segmentBuild;
				if (this.TryBuildSegment(this.seedCandidates[i], out segmentBuild))
				{
					this.activeProbeHeight = segmentBuild.probeHeight;
					if (this.IsSameActiveSegment(segmentBuild.center, segmentBuild.normal, segmentBuild.length, segmentBuild.standing, segmentBuild.cornerPos, segmentBuild.cornerNeg))
					{
						return;
					}
					this.PlaceCover(segmentBuild.center, segmentBuild.normal, segmentBuild.tangent, segmentBuild.length, segmentBuild.standing, segmentBuild.cornerPos, segmentBuild.cornerNeg);
					return;
				}
				else
				{
					i++;
				}
			}
			this.HideCover();
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x001BBE84 File Offset: 0x001BA084
		private bool TryBuildSegment(DynamicWallCoverScanner.SeedCandidate candidate, out DynamicWallCoverScanner.SegmentBuild segment)
		{
			segment = default(DynamicWallCoverScanner.SegmentBuild);
			if (!this.IsValidWallHit(candidate.hit))
			{
				return false;
			}
			Vector3 normal = candidate.normal;
			normal.y = 0f;
			if (normal.sqrMagnitude < 0.001f)
			{
				return false;
			}
			normal.Normalize();
			Vector3 vector = Vector3.Cross(Vector3.up, normal);
			Vector3 normalized = vector.normalized;
			Vector3 point = candidate.hit.point;
			float num;
			if (!this.TryGetFloorY(point, normal, this.player.position.y, out num, out vector) || Mathf.Abs(num - this.player.position.y) > this.MaxFloorDelta)
			{
				return false;
			}
			float num2;
			if (!this.TryGetProtrudingOffset(point, normal, num, out num2))
			{
				return false;
			}
			Vector3 vector2 = point + normal * num2;
			float num3;
			Vector3 vector3;
			if (!this.TryGetFloorY(vector2, normal, num, out num3, out vector3) || Mathf.Abs(num3 - num) > this.MaxFloorDelta || !this.IsClear(vector2, normal, num3, vector3))
			{
				return false;
			}
			num = num3;
			bool flag;
			float num5;
			float num4 = this.ProbeSide(point, normal, normalized, num, candidate.probeHeight, out flag, out num5);
			bool flag2;
			float num7;
			float num6 = this.ProbeSide(point, normal, -normalized, num, candidate.probeHeight, out flag2, out num7);
			float num8 = Mathf.Max(num2, Mathf.Max(num5, num7));
			Vector3 vector4 = point + normal * num8;
			float num9;
			Vector3 vector5;
			if (!this.TryGetFloorY(vector4, normal, num, out num9, out vector5) || Mathf.Abs(num9 - num) > this.MaxFloorDelta || !this.IsClear(vector4, normal, num9, vector5))
			{
				return false;
			}
			bool flag3;
			float num10 = this.TrimExtentForFinalClearance(point, normal, normalized, num9, num4, num8, out flag3);
			bool flag4;
			float num11 = this.TrimExtentForFinalClearance(point, normal, -normalized, num9, num6, num8, out flag4);
			if (!flag3)
			{
				flag = false;
			}
			if (!flag4)
			{
				flag2 = false;
			}
			num4 = num10;
			num6 = num11;
			float num12 = num4 + num6;
			float num13 = ((this.controller != null) ? Mathf.Max(0f, this.controller.coverEdgeInset) : 0f);
			if (num12 < Mathf.Max(this.MinCoverLength, num13 * 2f + 0.05f))
			{
				return false;
			}
			Vector3 vector6 = point + normalized * (num4 - num6) * 0.5f;
			Vector3 vector7 = vector6 + normal * num8;
			float num14;
			Vector3 vector8;
			if (!this.TryGetFloorY(vector7, normal, num9, out num14, out vector8) || Mathf.Abs(num14 - num9) > this.MaxFloorDelta || !this.IsClear(vector7, normal, num14, vector8))
			{
				return false;
			}
			vector7.y = num14;
			if (flag)
			{
				flag = this.ValidateFinalCorner(point, normal, normalized, num4, num8, num9);
			}
			if (flag2)
			{
				flag2 = this.ValidateFinalCorner(point, normal, -normalized, num6, num8, num9);
			}
			segment.center = vector7;
			segment.normal = normal;
			segment.tangent = normalized;
			segment.length = num12;
			segment.standing = this.WallIsTallAt(vector6, normal, num14);
			segment.cornerPos = flag;
			segment.cornerNeg = flag2;
			segment.probeHeight = candidate.probeHeight;
			return true;
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x001BC184 File Offset: 0x001BA384
		private bool IsSameActiveSegment(Vector3 center, Vector3 normal, float length, bool standing, bool cornerPos, bool cornerNeg)
		{
			if (!this.coverActive || this.cover == null)
			{
				return false;
			}
			CoverWallSetup.PeekMode peekMode = ((cornerPos && cornerNeg) ? CoverWallSetup.PeekMode.Both : (cornerPos ? CoverWallSetup.PeekMode.Left : (cornerNeg ? CoverWallSetup.PeekMode.Right : CoverWallSetup.PeekMode.None)));
			return Vector3.Distance(center, this.segCenter) < 0.35f && Vector3.Angle(normal, this.segNormal) < 5f && Mathf.Abs(length - this.segHalfLength * 2f) < 0.35f && this.cover.stance == CoverWallSetup.Stance.Standing == standing && this.cover.peekMode == peekMode;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x001BC228 File Offset: 0x001BA428
		private bool TryGetProtrudingOffset(Vector3 referenceFace, Vector3 n, float floorY, out float best)
		{
			best = 0f;
			float num = Mathf.Max(0.02f, this.WallDistanceTolerance);
			float num2 = 0.8f + num + 0.02f;
			for (int i = 0; i < DynamicWallCoverScanner.FaceProbeHeights.Length; i++)
			{
				Vector3 vector = referenceFace + n * 0.8f;
				vector.y = floorY + DynamicWallCoverScanner.FaceProbeHeights[i];
				int num3 = Physics.RaycastNonAlloc(vector, -n, this.reliefHits, num2, this.wallMask, QueryTriggerInteraction.Ignore);
				bool flag = false;
				RaycastHit raycastHit = default(RaycastHit);
				float num4 = float.MaxValue;
				for (int j = 0; j < num3; j++)
				{
					RaycastHit raycastHit2 = this.reliefHits[j];
					Vector3 vector2 = DynamicWallCoverScanner.OrientNormalAgainstRay(raycastHit2.normal, -n);
					if (this.IsValidWallHit(raycastHit2) && Vector3.Dot(vector2, n) >= this.ProbeNormalDot && raycastHit2.distance < num4)
					{
						raycastHit = raycastHit2;
						num4 = raycastHit2.distance;
						flag = true;
					}
				}
				if (flag)
				{
					float num5 = Vector3.Dot(raycastHit.point - referenceFace, n);
					if (Mathf.Abs(num5) > num + 0.01f)
					{
						return false;
					}
					if (num5 > best)
					{
						best = num5;
					}
					if (this.debugDraw)
					{
						Debug.DrawLine(vector, raycastHit.point, Color.magenta, this.ScanInterval);
					}
				}
			}
			best = Mathf.Max(0f, best);
			return true;
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x001BC39C File Offset: 0x001BA59C
		private int FindSeedWalls()
		{
			this.seedCandidateCount = 0;
			Vector3 vector = this.player.forward;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			vector.Normalize();
			int num = Mathf.Clamp(this.RadialRayCount, 8, 32);
			float num2 = Mathf.Max(0.1f, this.ScanHeight);
			float num3 = Mathf.Clamp(this.LowScanHeight, 0.1f, num2);
			for (int i = 0; i < 2; i++)
			{
				float num4 = ((i == 0) ? num2 : num3);
				if (i == 1 && Mathf.Abs(num2 - num3) < 0.05f)
				{
					break;
				}
				Vector3 vector2 = this.player.position + Vector3.up * num4;
				for (int j = 0; j < num; j++)
				{
					Vector3 vector3 = Quaternion.AngleAxis((float)j * (360f / (float)num), Vector3.up) * vector;
					if (this.debugDraw)
					{
						Debug.DrawRay(vector2, vector3 * this.MaxWallDistance, Color.grey, this.ScanInterval);
					}
					int num5 = Physics.RaycastNonAlloc(vector2, vector3, this.seedHits, this.MaxWallDistance, this.wallMask, QueryTriggerInteraction.Ignore);
					if (num5 >= this.seedHits.Length)
					{
						RaycastHit[] array = Physics.RaycastAll(vector2, vector3, this.MaxWallDistance, this.wallMask, QueryTriggerInteraction.Ignore);
						for (int k = 0; k < array.Length; k++)
						{
							this.AddSeedCandidate(array[k], vector3, num4, i, j, vector);
						}
					}
					else
					{
						for (int l = 0; l < num5; l++)
						{
							this.AddSeedCandidate(this.seedHits[l], vector3, num4, i, j, vector);
						}
					}
				}
			}
			this.SortSeedCandidates();
			return Mathf.Min(this.seedCandidateCount, 16);
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x001BC580 File Offset: 0x001BA780
		private void AddSeedCandidate(RaycastHit hit, Vector3 rayDirection, float probeHeight, int heightIndex, int rayIndex, Vector3 flatForward)
		{
			if (!this.IsValidWallHit(hit))
			{
				return;
			}
			Vector3 vector = DynamicWallCoverScanner.OrientNormalAgainstRay(hit.normal, rayDirection);
			float num = Mathf.Lerp(1f, 0.5f, Mathf.Clamp01(Vector3.Dot(rayDirection, flatForward)));
			DynamicWallCoverScanner.SeedCandidate seedCandidate = new DynamicWallCoverScanner.SeedCandidate
			{
				hit = hit,
				normal = vector,
				score = hit.distance * num + ((heightIndex == 0) ? 0f : 0.05f),
				probeHeight = probeHeight,
				heightIndex = heightIndex,
				rayIndex = rayIndex,
				colliderId = hit.collider.GetInstanceID()
			};
			Vector3 vector2 = vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 0.001f)
			{
				return;
			}
			vector2.Normalize();
			for (int i = 0; i < this.seedCandidateCount; i++)
			{
				Vector3 normal = this.seedCandidates[i].normal;
				normal.y = 0f;
				normal.Normalize();
				if (Vector3.Dot(vector2, normal) >= 0.985f && Mathf.Abs(Vector3.Dot(normal, hit.point - this.seedCandidates[i].hit.point)) <= 0.08f)
				{
					Vector3 vector3 = hit.point - this.seedCandidates[i].hit.point;
					vector3 -= normal * Vector3.Dot(vector3, normal);
					vector3.y = 0f;
					if (seedCandidate.colliderId == this.seedCandidates[i].colliderId || (seedCandidate.rayIndex == this.seedCandidates[i].rayIndex && vector3.sqrMagnitude <= 0.010000001f))
					{
						if (DynamicWallCoverScanner.CompareSeedCandidates(seedCandidate, this.seedCandidates[i]) < 0)
						{
							this.seedCandidates[i] = seedCandidate;
						}
						return;
					}
				}
			}
			if (this.seedCandidateCount < this.seedCandidates.Length)
			{
				DynamicWallCoverScanner.SeedCandidate[] array = this.seedCandidates;
				int num2 = this.seedCandidateCount;
				this.seedCandidateCount = num2 + 1;
				array[num2] = seedCandidate;
				return;
			}
			int num3 = 0;
			for (int j = 1; j < this.seedCandidateCount; j++)
			{
				if (DynamicWallCoverScanner.CompareSeedCandidates(this.seedCandidates[j], this.seedCandidates[num3]) > 0)
				{
					num3 = j;
				}
			}
			if (DynamicWallCoverScanner.CompareSeedCandidates(seedCandidate, this.seedCandidates[num3]) < 0)
			{
				this.seedCandidates[num3] = seedCandidate;
			}
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x001BC824 File Offset: 0x001BAA24
		private void SortSeedCandidates()
		{
			for (int i = 1; i < this.seedCandidateCount; i++)
			{
				DynamicWallCoverScanner.SeedCandidate seedCandidate = this.seedCandidates[i];
				int num = i - 1;
				while (num >= 0 && DynamicWallCoverScanner.CompareSeedCandidates(seedCandidate, this.seedCandidates[num]) < 0)
				{
					this.seedCandidates[num + 1] = this.seedCandidates[num];
					num--;
				}
				this.seedCandidates[num + 1] = seedCandidate;
			}
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x001BC89C File Offset: 0x001BAA9C
		private static int CompareSeedCandidates(DynamicWallCoverScanner.SeedCandidate a, DynamicWallCoverScanner.SeedCandidate b)
		{
			int num = a.score.CompareTo(b.score);
			if (num != 0)
			{
				return num;
			}
			num = a.hit.distance.CompareTo(b.hit.distance);
			if (num != 0)
			{
				return num;
			}
			num = a.heightIndex.CompareTo(b.heightIndex);
			if (num != 0)
			{
				return num;
			}
			num = a.rayIndex.CompareTo(b.rayIndex);
			if (num != 0)
			{
				return num;
			}
			return a.colliderId.CompareTo(b.colliderId);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x001BC929 File Offset: 0x001BAB29
		private static Vector3 OrientNormalAgainstRay(Vector3 normal, Vector3 rayDirection)
		{
			if (Vector3.Dot(normal, rayDirection) <= 0f)
			{
				return normal;
			}
			return -normal;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x001BC944 File Offset: 0x001BAB44
		private bool IsValidWallHit(RaycastHit hit)
		{
			if (hit.collider == null)
			{
				return false;
			}
			if (Mathf.Abs(hit.normal.y) > this.MaxNormalY)
			{
				return false;
			}
			Collider collider = hit.collider;
			if (collider.isTrigger)
			{
				return false;
			}
			if (collider.attachedRigidbody != null)
			{
				return false;
			}
			Transform transform = collider.transform;
			return (!(this.player != null) || !transform.IsChildOf(this.player)) && (!(this.cover != null) || !transform.IsChildOf(this.cover.transform)) && !(collider.GetComponentInParent<vCoverPoint>() != null) && !(collider.GetComponentInParent<CoverWallSetup>() != null);
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x001BCA08 File Offset: 0x001BAC08
		private float ProbeSide(Vector3 seedPoint, Vector3 n, Vector3 dir, float floorY, float probeHeight, out bool trueCorner, out float maxBulge)
		{
			trueCorner = false;
			maxBulge = 0f;
			float num = 0f;
			float num2 = Mathf.Max(0.05f, this.ProbeStep);
			int num3 = Mathf.FloorToInt(Mathf.Max(0f, this.MaxHalfLength) / num2);
			int i = 1;
			while (i <= num3)
			{
				float num4 = (float)i * num2;
				Vector3 vector = seedPoint + dir * num4;
				RaycastHit raycastHit;
				bool flag;
				if (!this.TryProbeWallAt(seedPoint, n, dir, num4, floorY, probeHeight, out raycastHit, out flag))
				{
					if (flag)
					{
						return num;
					}
					float num5 = this.RefineOpenWallEdge(seedPoint, n, dir, floorY, probeHeight, num, num4);
					Vector3 vector2 = seedPoint + dir * num5;
					float num6;
					Vector3 vector3;
					bool flag2 = this.TryGetFloorY(vector2, n, floorY, out num6, out vector3) && Mathf.Abs(num6 - floorY) <= this.MaxFloorDelta;
					trueCorner = flag2 && !this.HasWallBeyondGap(seedPoint, n, dir, floorY, probeHeight, num4, num2);
					if (!trueCorner)
					{
						return num;
					}
					return num5;
				}
				else
				{
					Vector3 vector3;
					float num7;
					float num8;
					if (!this.TryGetFloorY(vector, n, floorY, out num7, out vector3) || Mathf.Abs(num7 - floorY) > this.MaxFloorDelta || !this.TryGetProtrudingOffset(vector, n, num7, out num8))
					{
						return num;
					}
					float num9 = Vector3.Dot(raycastHit.point - vector, n);
					float num10 = Mathf.Max(0f, Mathf.Max(num9, num8));
					Vector3 vector4 = vector + n * num10;
					float num11;
					Vector3 vector5;
					if (!this.TryGetFloorY(vector4, n, num7, out num11, out vector5) || Mathf.Abs(num11 - floorY) > this.MaxFloorDelta || !this.IsClear(vector4, n, num11, vector5))
					{
						return num;
					}
					if (num10 > maxBulge)
					{
						maxBulge = num10;
					}
					num = num4;
					i++;
				}
			}
			return num;
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x001BCBC0 File Offset: 0x001BADC0
		private bool TryProbeWallAt(Vector3 seedPoint, Vector3 n, Vector3 dir, float distance, float floorY, float probeHeight, out RaycastHit hit, out bool hitSomething)
		{
			Vector3 vector = seedPoint + dir * distance + n * 0.6f;
			vector.y = floorY + probeHeight;
			hitSomething = Physics.Raycast(vector, -n, out hit, 0.9f, this.wallMask, QueryTriggerInteraction.Ignore);
			Vector3 vector2 = (hitSomething ? DynamicWallCoverScanner.OrientNormalAgainstRay(hit.normal, -n) : Vector3.zero);
			bool flag = hitSomething && this.IsValidWallHit(hit) && Vector3.Dot(vector2, n) > this.ProbeNormalDot && Mathf.Abs(hit.distance - 0.6f) < this.WallDistanceTolerance;
			if (this.debugDraw)
			{
				Debug.DrawRay(vector, -n * 0.9f, flag ? Color.green : Color.red, this.ScanInterval);
			}
			return flag;
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x001BCCB0 File Offset: 0x001BAEB0
		private float RefineOpenWallEdge(Vector3 seedPoint, Vector3 n, Vector3 dir, float floorY, float probeHeight, float lastValidDistance, float firstInvalidDistance)
		{
			float num = lastValidDistance;
			float num2 = firstInvalidDistance;
			for (int i = 0; i < 5; i++)
			{
				float num3 = (num + num2) * 0.5f;
				RaycastHit raycastHit;
				bool flag;
				if (this.TryProbeWallAt(seedPoint, n, dir, num3, floorY, probeHeight, out raycastHit, out flag))
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x001BCCF4 File Offset: 0x001BAEF4
		private bool HasWallBeyondGap(Vector3 seedPoint, Vector3 n, Vector3 dir, float floorY, float probeHeight, float firstInvalidDistance, float step)
		{
			for (int i = 1; i <= 4; i++)
			{
				float num = firstInvalidDistance + step * (float)i;
				if (num > this.MaxHalfLength + step)
				{
					break;
				}
				RaycastHit raycastHit;
				bool flag;
				if (this.TryProbeWallAt(seedPoint, n, dir, num, floorY, probeHeight, out raycastHit, out flag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x001BCD3C File Offset: 0x001BAF3C
		private bool IsOpenPastCorner(Vector3 facePoint, Vector3 n, Vector3 dir, float floorY, Vector3 floorNormal)
		{
			float num;
			float num2;
			float num3;
			this.GetStandingCapsuleDimensions(out num, out num2, out num3);
			Vector3 vector = facePoint + dir * (num + this.FaceMargin);
			Vector3 vector2;
			Vector3 vector3;
			this.GetStandingCapsule(vector, n, floorY, floorNormal, out vector2, out vector3, out num);
			bool flag = Physics.CheckCapsule(vector2, vector3, num, this.clearanceMask, QueryTriggerInteraction.Ignore);
			if (this.debugDraw)
			{
				Debug.DrawLine(vector2, vector3, flag ? Color.red : Color.cyan, this.ScanInterval);
			}
			return !flag;
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x001BCDBC File Offset: 0x001BAFBC
		private bool TryGetFloorY(Vector3 facePoint, Vector3 n, float expectedFloorY, out float floorY, out Vector3 floorNormal)
		{
			float num;
			float num2;
			float num3;
			this.GetStandingCapsuleDimensions(out num, out num2, out num3);
			Vector3 vector = facePoint + n * (this.FaceMargin + num);
			float num4 = Mathf.Max(0.5f, this.MaxFloorDelta + 0.5f);
			float num5 = Mathf.Max(0.75f, this.MaxFloorDelta + 0.5f);
			vector.y = expectedFloorY + num4;
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, Vector3.down, out raycastHit, num4 + num5, this.floorMask, QueryTriggerInteraction.Ignore) && this.IsValidFloorHit(raycastHit))
			{
				floorY = raycastHit.point.y;
				floorNormal = raycastHit.normal;
				if (this.debugDraw)
				{
					Debug.DrawLine(vector, raycastHit.point, Color.blue, this.ScanInterval);
				}
				return true;
			}
			floorY = 0f;
			floorNormal = Vector3.up;
			return false;
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x001BCEA0 File Offset: 0x001BB0A0
		private bool IsValidFloorHit(RaycastHit hit)
		{
			if (hit.collider == null || hit.collider.isTrigger || hit.collider.attachedRigidbody != null)
			{
				return false;
			}
			float num = ((this.motor != null) ? this.motor.slopeLimit : 60f);
			num = Mathf.Clamp(num, 0f, 89f);
			return Vector3.Angle(hit.normal, Vector3.up) <= num + 0.01f;
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x001BCF30 File Offset: 0x001BB130
		private bool IsClear(Vector3 facePoint, Vector3 n, float floorY, Vector3 floorNormal)
		{
			Vector3 vector;
			Vector3 vector2;
			float num;
			this.GetStandingCapsule(facePoint, n, floorY, floorNormal, out vector, out vector2, out num);
			bool flag = Physics.CheckCapsule(vector, vector2, num, this.clearanceMask, QueryTriggerInteraction.Ignore);
			if (this.debugDraw)
			{
				Debug.DrawLine(vector, vector2, flag ? Color.red : Color.white, this.ScanInterval);
			}
			return !flag;
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x001BCF8C File Offset: 0x001BB18C
		private void GetStandingCapsule(Vector3 facePoint, Vector3 n, float floorY, Vector3 floorNormal, out Vector3 bottom, out Vector3 top, out float radius)
		{
			float num;
			float num2;
			this.GetStandingCapsuleDimensions(out radius, out num, out num2);
			Vector3 vector = facePoint + n * (this.FaceMargin + radius);
			float num3 = ((floorNormal.sqrMagnitude > 0.001f) ? Mathf.Clamp(Vector3.Dot(floorNormal.normalized, Vector3.up), 0.1f, 1f) : 1f);
			float num4 = radius / num3 - radius;
			bottom = new Vector3(vector.x, floorY + num2 + radius + num4 + 0.01f, vector.z);
			top = new Vector3(vector.x, floorY + num2 + num - radius + num4 + 0.01f, vector.z);
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x001BD050 File Offset: 0x001BB250
		private void GetStandingCapsuleDimensions(out float radius, out float height, out float bottomOffset)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			if (this.motor != null)
			{
				num = ((this.motor.colliderRadiusDefault > 0.001f) ? this.motor.colliderRadiusDefault : (this.motor.capsuleThickness * 0.5f));
				num2 = ((this.motor.colliderHeightDefault > 0.001f) ? this.motor.colliderHeightDefault : this.motor.capsuleHeight);
				num3 = ((this.motor.colliderHeightDefault > 0.001f) ? this.motor.colliderCenterDefault.y : (this.motor.capsuleOffset.y * this.motor.capsuleHeight));
			}
			Vector3 vector = ((this.motor != null) ? this.motor.transform.lossyScale : ((this.player != null) ? this.player.lossyScale : Vector3.one));
			float num4 = Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.z));
			float num5 = Mathf.Abs(vector.y);
			if (num4 < 0.001f)
			{
				num4 = 1f;
			}
			if (num5 < 0.001f)
			{
				num5 = 1f;
			}
			float num6 = num * num4;
			float num7 = num2 * num5;
			radius = Mathf.Max(0.05f, Mathf.Max(this.ClearanceRadius, num6));
			height = Mathf.Max(radius * 2f, Mathf.Max(this.ClearanceHeight, num7));
			bottomOffset = (num3 - num2 * 0.5f) * num5;
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x001BD1F8 File Offset: 0x001BB3F8
		private float TrimExtentForFinalClearance(Vector3 traceSeed, Vector3 n, Vector3 dir, float floorY, float requestedExtent, float globalBulge, out bool unchanged)
		{
			float num = 0f;
			float num2 = Mathf.Max(0.05f, this.ProbeStep);
			while (num + 0.001f < requestedExtent)
			{
				float num3 = Mathf.Min(requestedExtent, num + num2);
				Vector3 vector = traceSeed + dir * num3 + n * globalBulge;
				float num4;
				Vector3 vector2;
				if (!this.TryGetFloorY(vector, n, floorY, out num4, out vector2) || Mathf.Abs(num4 - floorY) > this.MaxFloorDelta || !this.IsClear(vector, n, num4, vector2))
				{
					unchanged = false;
					return num;
				}
				num = num3;
			}
			unchanged = true;
			return requestedExtent;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x001BD290 File Offset: 0x001BB490
		private bool ValidateFinalCorner(Vector3 traceSeed, Vector3 n, Vector3 dir, float extent, float globalBulge, float floorY)
		{
			Vector3 vector = traceSeed + dir * extent + n * globalBulge;
			float num;
			Vector3 vector2;
			return this.TryGetFloorY(vector, n, floorY, out num, out vector2) && Mathf.Abs(num - floorY) <= this.MaxFloorDelta && this.IsOpenPastCorner(vector, n, dir, num, vector2);
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x001BD2E8 File Offset: 0x001BB4E8
		private bool WallIsTallAt(Vector3 traceCenter, Vector3 n, float floorY)
		{
			Vector3 vector = traceCenter + n * 0.6f;
			vector.y = floorY + this.CrouchCheckHeight;
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(vector, -n, out raycastHit, 0.9f, this.wallMask, QueryTriggerInteraction.Ignore) && this.IsValidWallHit(raycastHit) && Vector3.Dot(DynamicWallCoverScanner.OrientNormalAgainstRay(raycastHit.normal, -n), n) > this.ProbeNormalDot && Mathf.Abs(raycastHit.distance - 0.6f) <= this.WallDistanceTolerance;
			if (this.debugDraw)
			{
				Debug.DrawRay(vector, -n * 0.9f, flag ? Color.cyan : Color.yellow, this.ScanInterval);
			}
			return flag;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x001BD3B8 File Offset: 0x001BB5B8
		private void EnsureCoverObject()
		{
			if (this.cover != null)
			{
				return;
			}
			DynamicWallCoverScanner.ResolveTemplates();
			GameObject gameObject = new GameObject("DynamicWallCover (runtime)");
			Object.DontDestroyOnLoad(gameObject);
			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject2.name = "Wall_Visual";
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.GetComponent<MeshRenderer>().enabled = false;
			gameObject2.GetComponent<BoxCollider>().enabled = false;
			this.coverVisual = gameObject2.transform;
			this.cover = gameObject.AddComponent<CoverWallSetup>();
			this.cover.wallVisual = this.coverVisual;
			this.cover.allowCrouchToggle = true;
			this.cover.interactionConfig = DynamicWallCoverScanner.configTemplate;
			if (DynamicWallCoverScanner.promptTemplate != null)
			{
				InteractablePromptUI interactablePromptUI = Object.Instantiate<InteractablePromptUI>(DynamicWallCoverScanner.promptTemplate, gameObject.transform);
				interactablePromptUI.name = "InteractUI";
				interactablePromptUI.gameObject.SetActive(true);
				gameObject.AddComponent<CoverWallInteractable>().SetPromptUI(interactablePromptUI);
			}
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x001BD4AC File Offset: 0x001BB6AC
		private void PlaceCover(Vector3 center, Vector3 n, Vector3 t, float length, bool standing, bool cornerPos, bool cornerNeg)
		{
			this.EnsureCoverObject();
			Transform transform = this.cover.transform;
			this.ClearPossibleOwnedCoverPoint();
			if (this.relinkRoutine != null)
			{
				base.StopCoroutine(this.relinkRoutine);
				this.relinkRoutine = null;
			}
			if (transform.gameObject.activeSelf)
			{
				transform.gameObject.SetActive(false);
			}
			transform.SetPositionAndRotation(center, Quaternion.LookRotation(n, Vector3.up));
			float num = (standing ? this.StandingWallHeight : this.CrouchedWallHeight);
			this.coverVisual.localPosition = new Vector3(0f, num * 0.5f, -this.WallThickness * 0.5f);
			this.coverVisual.localRotation = Quaternion.identity;
			this.coverVisual.localScale = new Vector3(length, num, this.WallThickness);
			this.cover.coverOffsetZ = this.FaceMargin;
			this.cover.maxSlideSpeed = this.SlideSpeedLimit;
			this.cover.stance = (standing ? CoverWallSetup.Stance.Standing : CoverWallSetup.Stance.Crouched);
			this.cover.peekMode = ((cornerPos && cornerNeg) ? CoverWallSetup.PeekMode.Both : (cornerPos ? CoverWallSetup.PeekMode.Left : (cornerNeg ? CoverWallSetup.PeekMode.Right : CoverWallSetup.PeekMode.None)));
			transform.gameObject.SetActive(true);
			this.cover.Rebuild();
			Physics.SyncTransforms();
			this.segCenter = center;
			this.segNormal = n;
			this.segTangent = t;
			this.segHalfLength = length * 0.5f;
			this.coverActive = true;
			this.relinkRoutine = base.StartCoroutine(this.RelinkAfterStart());
			if (this.debugDraw)
			{
				Debug.Log(string.Format("[DynamicWallCover] placed {0:0.0} m, {1}, peek={2}", length, standing ? "standing" : "crouched", this.cover.peekMode), transform);
			}
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x001BD66C File Offset: 0x001BB86C
		private IEnumerator RelinkAfterStart()
		{
			yield return new WaitForEndOfFrame();
			yield return null;
			this.relinkRoutine = null;
			if (this.coverActive && this.cover != null && this.cover.gameObject.activeSelf && this.controller != null && !this.controller.inCover && !this.controller.goingToCoverPoint)
			{
				this.cover.Rebuild();
				Physics.SyncTransforms();
			}
			yield break;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x001BD67C File Offset: 0x001BB87C
		private void HideCover()
		{
			bool flag = this.coverActive || (this.cover != null && this.cover.gameObject.activeSelf);
			this.coverActive = false;
			if (this.relinkRoutine != null)
			{
				base.StopCoroutine(this.relinkRoutine);
				this.relinkRoutine = null;
			}
			if (this.cover == null)
			{
				return;
			}
			this.ClearPossibleOwnedCoverPoint();
			if (!flag)
			{
				return;
			}
			this.cover.transform.position = Vector3.down * 1000f;
			if (this.cover.gameObject.activeSelf)
			{
				this.cover.gameObject.SetActive(false);
			}
			Physics.SyncTransforms();
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x001BD738 File Offset: 0x001BB938
		private void ClearPossibleOwnedCoverPoint()
		{
			if (this.controller != null && this.cover != null && this.controller.possibleCoverPoint != null && this.controller.possibleCoverPoint.transform.IsChildOf(this.cover.transform))
			{
				this.controller.possibleCoverPoint = null;
			}
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x001BD7A4 File Offset: 0x001BB9A4
		private static void ResolveTemplates()
		{
			if (DynamicWallCoverScanner.configTemplate != null && DynamicWallCoverScanner.promptTemplate != null)
			{
				return;
			}
			DynamicWallCoverAssets dynamicWallCoverAssets = DynamicWallCoverAssets.Load();
			if (dynamicWallCoverAssets != null)
			{
				if (DynamicWallCoverScanner.configTemplate == null)
				{
					DynamicWallCoverScanner.configTemplate = dynamicWallCoverAssets.interactionConfig;
				}
				if (DynamicWallCoverScanner.promptTemplate == null)
				{
					DynamicWallCoverScanner.promptTemplate = dynamicWallCoverAssets.promptPrefab;
				}
				if (DynamicWallCoverScanner.configTemplate != null && DynamicWallCoverScanner.promptTemplate != null)
				{
					return;
				}
			}
			else
			{
				Debug.LogWarning("[DynamicWallCover] Resources/DynamicWallCoverAssets missing: run Tools/Cover Wall/Experimental/Build Dynamic Cover Assets.");
			}
			foreach (CoverWallSetup coverWallSetup in Object.FindObjectsOfType<CoverWallSetup>(true))
			{
				if (!coverWallSetup.name.StartsWith("DynamicWallCover"))
				{
					if (DynamicWallCoverScanner.configTemplate == null && coverWallSetup.interactionConfig != null)
					{
						DynamicWallCoverScanner.configTemplate = coverWallSetup.interactionConfig;
					}
					if (DynamicWallCoverScanner.promptTemplate == null)
					{
						InteractablePromptUI componentInChildren = coverWallSetup.GetComponentInChildren<InteractablePromptUI>(true);
						if (componentInChildren != null)
						{
							DynamicWallCoverScanner.promptTemplate = componentInChildren;
						}
					}
					if (DynamicWallCoverScanner.configTemplate != null && DynamicWallCoverScanner.promptTemplate != null)
					{
						break;
					}
				}
			}
		}

		// Token: 0x040048A5 RID: 18597
		private const string PrefsKey = "CoverWall.DynamicWallCover.Enabled";

		// Token: 0x040048A6 RID: 18598
		private const string RootName = "[DynamicWallCover]";

		// Token: 0x040048A7 RID: 18599
		[Header("Scan")]
		public float ScanInterval = 0.25f;

		// Token: 0x040048A8 RID: 18600
		[Tooltip("Max distance from the player chest to the wall.")]
		public float MaxWallDistance = 1.4f;

		// Token: 0x040048A9 RID: 18601
		[Tooltip("Height (from the player's feet) of the detection ray.")]
		public float ScanHeight = 1f;

		// Token: 0x040048AA RID: 18602
		[Tooltip("Fallback ray height used for low cover and below window openings.")]
		public float LowScanHeight = 0.65f;

		// Token: 0x040048AB RID: 18603
		[Range(8f, 32f)]
		[Tooltip("Number of radial directions sampled around Ciel.")]
		public int RadialRayCount = 16;

		// Token: 0x040048AC RID: 18604
		[Tooltip("Walls steeper than this (|normal.y|) are ignored.")]
		public float MaxNormalY = 0.3f;

		// Token: 0x040048AD RID: 18605
		[Header("Wall probing")]
		public float ProbeStep = 0.25f;

		// Token: 0x040048AE RID: 18606
		public float MaxHalfLength = 6f;

		// Token: 0x040048AF RID: 18607
		public float ProbeNormalDot = 0.9f;

		// Token: 0x040048B0 RID: 18608
		[Tooltip("Max relief (m) along the wall before the segment is cut. Bigger relief inside this band moves the whole cover face outward.")]
		public float WallDistanceTolerance = 0.15f;

		// Token: 0x040048B1 RID: 18609
		[Tooltip("Gap between the wall face and the cover line (capsule clearance).")]
		public float FaceMargin = 0.08f;

		// Token: 0x040048B2 RID: 18610
		[Tooltip("Height of the ray that decides standing vs crouched (Invector crouchHeight ~1.5).")]
		public float CrouchCheckHeight = 1.45f;

		// Token: 0x040048B3 RID: 18611
		public float MinCoverLength = 0.8f;

		// Token: 0x040048B4 RID: 18612
		[Tooltip("Max floor height change along the segment (steps, desks, platforms break the cover).")]
		public float MaxFloorDelta = 0.25f;

		// Token: 0x040048B5 RID: 18613
		[Tooltip("Radius of the clearance capsule checked along Ciel's line in front of the wall.")]
		public float ClearanceRadius = 0.22f;

		// Token: 0x040048B6 RID: 18614
		[Tooltip("Full standing clearance kept available so Ciel can stand up from crouched cover.")]
		public float ClearanceHeight = 1.7f;

		// Token: 0x040048B7 RID: 18615
		[Header("Built cover")]
		public float StandingWallHeight = 2f;

		// Token: 0x040048B8 RID: 18616
		public float CrouchedWallHeight = 1f;

		// Token: 0x040048B9 RID: 18617
		public float WallThickness = 0.3f;

		// Token: 0x040048BA RID: 18618
		[Tooltip("Ciel is considered 'on' the segment while closer than this to the wall plane.")]
		public float KeepDistanceFromWall = 1.6f;

		// Token: 0x040048BB RID: 18619
		[Tooltip("Extra tolerance along the segment before it is considered left.")]
		public float KeepMargin = 0.3f;

		// Token: 0x040048BC RID: 18620
		[Tooltip("Max slide speed (m/s) along dynamic covers. Tune to match the authored walls' feel.")]
		public float SlideSpeedLimit = 1f;

		// Token: 0x040048BD RID: 18621
		[Header("Debug")]
		public bool debugDraw;

		// Token: 0x040048BE RID: 18622
		private Transform player;

		// Token: 0x040048BF RID: 18623
		private vBasicCoverController controller;

		// Token: 0x040048C0 RID: 18624
		private vThirdPersonController motor;

		// Token: 0x040048C1 RID: 18625
		private LayerMask wallMask = 1;

		// Token: 0x040048C2 RID: 18626
		private LayerMask floorMask = 1;

		// Token: 0x040048C3 RID: 18627
		private LayerMask clearanceMask = 1;

		// Token: 0x040048C4 RID: 18628
		private float nextScanTime;

		// Token: 0x040048C5 RID: 18629
		private float nextActiveValidationTime;

		// Token: 0x040048C6 RID: 18630
		private int activeValidationMisses;

		// Token: 0x040048C7 RID: 18631
		private float activeProbeHeight = 1f;

		// Token: 0x040048C8 RID: 18632
		private readonly RaycastHit[] seedHits = new RaycastHit[64];

		// Token: 0x040048C9 RID: 18633
		private readonly RaycastHit[] reliefHits = new RaycastHit[16];

		// Token: 0x040048CA RID: 18634
		private readonly DynamicWallCoverScanner.SeedCandidate[] seedCandidates = new DynamicWallCoverScanner.SeedCandidate[32];

		// Token: 0x040048CB RID: 18635
		private int seedCandidateCount;

		// Token: 0x040048CC RID: 18636
		private bool shuttingDown;

		// Token: 0x040048CD RID: 18637
		private const float VerticalClearanceSkin = 0.01f;

		// Token: 0x040048CE RID: 18638
		private const float CandidatePlaneTolerance = 0.08f;

		// Token: 0x040048CF RID: 18639
		private const float CrossColliderCandidateTolerance = 0.1f;

		// Token: 0x040048D0 RID: 18640
		private CoverWallSetup cover;

		// Token: 0x040048D1 RID: 18641
		private Transform coverVisual;

		// Token: 0x040048D2 RID: 18642
		private bool coverActive;

		// Token: 0x040048D3 RID: 18643
		private Vector3 segCenter;

		// Token: 0x040048D4 RID: 18644
		private Vector3 segNormal;

		// Token: 0x040048D5 RID: 18645
		private Vector3 segTangent;

		// Token: 0x040048D6 RID: 18646
		private float segHalfLength;

		// Token: 0x040048D7 RID: 18647
		private static InteractionConfigSO configTemplate;

		// Token: 0x040048D8 RID: 18648
		private static InteractablePromptUI promptTemplate;

		// Token: 0x040048D9 RID: 18649
		private float noFloorSince = -1f;

		// Token: 0x040048DA RID: 18650
		private static MethodInfo exitCoverMethod;

		// Token: 0x040048DB RID: 18651
		private static readonly float[] FaceProbeHeights = new float[] { 0.12f, 0.35f, 0.65f, 1f, 1.3f, 1.65f };

		// Token: 0x040048DC RID: 18652
		private Coroutine relinkRoutine;

		// Token: 0x020009FC RID: 2556
		private struct SeedCandidate
		{
			// Token: 0x040048DD RID: 18653
			public RaycastHit hit;

			// Token: 0x040048DE RID: 18654
			public Vector3 normal;

			// Token: 0x040048DF RID: 18655
			public float score;

			// Token: 0x040048E0 RID: 18656
			public float probeHeight;

			// Token: 0x040048E1 RID: 18657
			public int heightIndex;

			// Token: 0x040048E2 RID: 18658
			public int rayIndex;

			// Token: 0x040048E3 RID: 18659
			public int colliderId;
		}

		// Token: 0x020009FD RID: 2557
		private struct SegmentBuild
		{
			// Token: 0x040048E4 RID: 18660
			public Vector3 center;

			// Token: 0x040048E5 RID: 18661
			public Vector3 normal;

			// Token: 0x040048E6 RID: 18662
			public Vector3 tangent;

			// Token: 0x040048E7 RID: 18663
			public float length;

			// Token: 0x040048E8 RID: 18664
			public bool standing;

			// Token: 0x040048E9 RID: 18665
			public bool cornerPos;

			// Token: 0x040048EA RID: 18666
			public bool cornerNeg;

			// Token: 0x040048EB RID: 18667
			public float probeHeight;
		}
	}
}
