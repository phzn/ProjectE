using System;
using System.Collections.Generic;
using System.IO;
using Features.NPC;
using Features.RenderingPrecision;
using Pathfinding;
using UnityEngine;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BA7 RID: 2983
	public class ScaredDriverFleeRunner : MonoBehaviour
	{
		// Token: 0x06005597 RID: 21911 RVA: 0x0021365F File Offset: 0x0021185F
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x00213674 File Offset: 0x00211874
		private void OnWorldOriginShifted(Vector3 delta)
		{
			if (this._done)
			{
				return;
			}
			this._fleeTo += delta;
			this._startPos += delta;
			this._dashTarget += delta;
			if (this._waypoints != null)
			{
				for (int i = 0; i < this._waypoints.Count; i++)
				{
					List<Vector3> waypoints = this._waypoints;
					int num = i;
					waypoints[num] += delta;
				}
			}
			if (this._pathPending)
			{
				this.RequestFleePath();
			}
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x0021370C File Offset: 0x0021190C
		public static void Begin(GameObject npc, NPCRuntimeSpawner spawner, Vector3 fleeTo, float fleeSpeed, float despawnSeconds)
		{
			ScaredDriverFleeRunner scaredDriverFleeRunner = npc.GetComponent<ScaredDriverFleeRunner>();
			if (scaredDriverFleeRunner == null)
			{
				scaredDriverFleeRunner = npc.AddComponent<ScaredDriverFleeRunner>();
			}
			scaredDriverFleeRunner._spawner = spawner;
			scaredDriverFleeRunner._deadline = Time.time + Mathf.Max(2f, despawnSeconds);
			scaredDriverFleeRunner._minRunUntil = Time.time + 1.5f;
			scaredDriverFleeRunner._nextNudge = 0f;
			scaredDriverFleeRunner._dashAt = Time.time + 1.6f;
			scaredDriverFleeRunner._fleeTo = fleeTo;
			scaredDriverFleeRunner._fleeSpeed = fleeSpeed;
			scaredDriverFleeRunner._startPos = npc.transform.position;
			scaredDriverFleeRunner._waypoints = null;
			scaredDriverFleeRunner._pathPending = false;
			scaredDriverFleeRunner._pathRequestVersion++;
			scaredDriverFleeRunner._wpIndex = 0;
			scaredDriverFleeRunner._phase = ScaredDriverFleeRunner.Phase.Nudge;
			scaredDriverFleeRunner._moved = false;
			scaredDriverFleeRunner._done = false;
			scaredDriverFleeRunner._controller = npc.GetComponent<AStarNPCController>();
			scaredDriverFleeRunner._follower = npc.GetComponent<FollowerEntity>();
			NPCSimpleLookAt component = npc.GetComponent<NPCSimpleLookAt>();
			if (component != null)
			{
				component.StopLooking();
			}
			if (scaredDriverFleeRunner._follower != null && scaredDriverFleeRunner._restoreMaxSpeed < 0f)
			{
				scaredDriverFleeRunner._restoreMaxSpeed = scaredDriverFleeRunner._follower.maxSpeed;
			}
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x00213830 File Offset: 0x00211A30
		private void Update()
		{
			if (this._done)
			{
				return;
			}
			if (!this._moved)
			{
				Vector3 vector = base.transform.position - this._startPos;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.36f)
				{
					this._moved = true;
				}
			}
			switch (this._phase)
			{
			case ScaredDriverFleeRunner.Phase.Nudge:
				if (!this._moved && Time.time >= this._nextNudge)
				{
					this._nextNudge = Time.time + 0.75f;
					if (this._controller != null)
					{
						if (this._controller.IsNavigationLocked)
						{
							this._controller.UnlockNavigation(false);
						}
						this._controller.SetDestination(this._fleeTo);
					}
					if (this._follower != null)
					{
						this._follower.maxSpeed = this._fleeSpeed;
					}
				}
				if (!this._moved && Time.time >= this._dashAt)
				{
					this.BeginManualFlee();
				}
				break;
			case ScaredDriverFleeRunner.Phase.Dash:
				if (this.StepTowards(this._dashTarget) && this._waypoints != null)
				{
					this._wpIndex = 0;
					this._phase = ScaredDriverFleeRunner.Phase.Path;
				}
				break;
			case ScaredDriverFleeRunner.Phase.Path:
				if (this._waypoints != null && this._wpIndex < this._waypoints.Count && this.StepTowards(this._waypoints[this._wpIndex]))
				{
					this._wpIndex++;
				}
				break;
			}
			if (Time.time < this._minRunUntil)
			{
				return;
			}
			bool flag = base.transform.position.y < -5f;
			bool flag2 = (this._phase == ScaredDriverFleeRunner.Phase.Nudge && this._moved && this._controller != null && this._controller.HasReachedDestination()) || (this._phase == ScaredDriverFleeRunner.Phase.Path && this._waypoints != null && this._wpIndex >= this._waypoints.Count);
			if (!flag && !flag2 && Time.time < this._deadline)
			{
				return;
			}
			this._done = true;
			NPCRuntimeSpawner spawner = this._spawner;
			GameObject gameObject = base.gameObject;
			this.Cleanup();
			Object.Destroy(this);
			if (spawner != null)
			{
				spawner.DespawnNPC(gameObject, false);
				return;
			}
			Object.Destroy(gameObject);
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x00213A74 File Offset: 0x00211C74
		private bool StepTowards(Vector3 target)
		{
			Vector3 position = base.transform.position;
			Vector3 vector = target - position;
			vector.y = 0f;
			if (vector.sqrMagnitude <= 0.040000003f)
			{
				return true;
			}
			Vector3 normalized = vector.normalized;
			RaycastHit raycastHit;
			if (Physics.Raycast(position + Vector3.up * 1.2f, normalized, out raycastHit, 0.9f, -1, QueryTriggerInteraction.Ignore) && !raycastHit.collider.transform.IsChildOf(base.transform) && !raycastHit.collider.CompareTag("Player") && !raycastHit.collider.CompareTag("NPC"))
			{
				return true;
			}
			base.transform.position = position + normalized * (this._fleeSpeed * Time.deltaTime);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(normalized), 10f * Time.deltaTime);
			return false;
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x00213B70 File Offset: 0x00211D70
		private void BeginManualFlee()
		{
			this._phase = ScaredDriverFleeRunner.Phase.Dash;
			Vector3 vector = this._fleeTo - this._startPos;
			vector.y = 0f;
			vector = ((vector.sqrMagnitude > 0.001f) ? vector.normalized : base.transform.forward);
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			this._dashTarget = this._startPos + vector * 6f;
			AstarPath active = AstarPath.active;
			bool flag = false;
			if (active != null)
			{
				Vector3[] array = new Vector3[]
				{
					this._startPos + vector * 6f,
					this._startPos + normalized * 5f,
					this._startPos - normalized * 5f,
					this._startPos + normalized * 8f,
					this._startPos - normalized * 8f,
					this._startPos + vector * 6f + normalized * 5f,
					this._startPos + vector * 6f - normalized * 5f
				};
				Vector3[] array2 = new Vector3[]
				{
					this._fleeTo,
					this._fleeTo + normalized * 6f,
					this._fleeTo - normalized * 6f,
					this._startPos + vector * 20f + normalized * 6f,
					this._startPos + vector * 20f - normalized * 6f
				};
				foreach (Vector3 vector2 in array)
				{
					NNInfo nearest = active.GetNearest(vector2);
					if (nearest.node != null && nearest.node.Walkable)
					{
						foreach (Vector3 vector3 in array2)
						{
							NNInfo nearest2 = active.GetNearest(vector3);
							if (nearest2.node != null && nearest2.node.Walkable && PathUtilities.IsPathPossible(nearest.node, nearest2.node))
							{
								this._dashTarget = nearest.position;
								this._fleeTo = nearest2.position;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (this._follower != null)
			{
				this._follower.enabled = false;
			}
			if (this._controller != null)
			{
				this._controller.SetSpeedOverride(this._fleeSpeed);
			}
			string text = "astar=null";
			if (active != null)
			{
				NNInfo nearest3 = active.GetNearest(this._startPos);
				text = ((nearest3.node == null) ? "nodeUnderStart=null" : string.Format("nodeUnderStart walkable={0} area={1} at={2:F1}", nearest3.node.Walkable, nearest3.node.Area, nearest3.position));
			}
			ScaredDriverFleeRunner.Diag(string.Format("FLEE-PLAN paired={0} {1} dash={2:F1} dest={3:F1}", new object[] { flag, text, this._dashTarget, this._fleeTo }));
			if (active != null && flag)
			{
				this.RequestFleePath();
				return;
			}
			this._waypoints = new List<Vector3> { this._fleeTo };
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x00213F74 File Offset: 0x00212174
		private void RequestFleePath()
		{
			ScaredDriverFleeRunner.<>c__DisplayClass26_0 CS$<>8__locals1 = new ScaredDriverFleeRunner.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			ScaredDriverFleeRunner.<>c__DisplayClass26_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._pathRequestVersion + 1;
			this._pathRequestVersion = num;
			CS$<>8__locals2.version = num;
			this._pathPending = true;
			AstarPath.StartPath(ABPath.Construct(this._dashTarget, this._fleeTo, delegate(Path result)
			{
				if (CS$<>8__locals1.<>4__this != null && CS$<>8__locals1.version == CS$<>8__locals1.<>4__this._pathRequestVersion)
				{
					CS$<>8__locals1.<>4__this.OnFleePathComplete(result);
				}
			}), false, false);
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00213FD0 File Offset: 0x002121D0
		private void OnFleePathComplete(Path p)
		{
			if (this._done)
			{
				return;
			}
			this._pathPending = false;
			p.Claim(this);
			if (!p.error && p.vectorPath != null && p.vectorPath.Count > 0)
			{
				this._waypoints = new List<Vector3>(p.vectorPath);
			}
			ScaredDriverFleeRunner.Diag(string.Format("FLEE-PATH error={0} waypoints={1}", p.error, (this._waypoints != null) ? this._waypoints.Count : 0));
			p.Release(this, false);
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x00214060 File Offset: 0x00212260
		private static void Diag(string line)
		{
			try
			{
				File.AppendAllText(Path.Combine("Temp", "RideLimiterState.txt"), string.Format("[{0:F1}s] {1}\n", Time.time, line));
			}
			catch
			{
			}
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x002140AC File Offset: 0x002122AC
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			this._pathRequestVersion++;
			this._pathPending = false;
			if (this._done)
			{
				return;
			}
			this._done = true;
			this.Cleanup();
			Object.Destroy(this);
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x002140FC File Offset: 0x002122FC
		private void Cleanup()
		{
			if (this._controller != null)
			{
				this._controller.ClearSpeedOverride();
			}
			if (this._follower != null)
			{
				this._follower.enabled = true;
				if (this._restoreMaxSpeed >= 0f)
				{
					this._follower.maxSpeed = this._restoreMaxSpeed;
				}
			}
		}

		// Token: 0x040055DD RID: 21981
		private NPCRuntimeSpawner _spawner;

		// Token: 0x040055DE RID: 21982
		private AStarNPCController _controller;

		// Token: 0x040055DF RID: 21983
		private FollowerEntity _follower;

		// Token: 0x040055E0 RID: 21984
		private float _restoreMaxSpeed = -1f;

		// Token: 0x040055E1 RID: 21985
		private float _fleeSpeed;

		// Token: 0x040055E2 RID: 21986
		private Vector3 _fleeTo;

		// Token: 0x040055E3 RID: 21987
		private Vector3 _startPos;

		// Token: 0x040055E4 RID: 21988
		private Vector3 _dashTarget;

		// Token: 0x040055E5 RID: 21989
		private List<Vector3> _waypoints;

		// Token: 0x040055E6 RID: 21990
		private int _wpIndex;

		// Token: 0x040055E7 RID: 21991
		private float _deadline;

		// Token: 0x040055E8 RID: 21992
		private float _minRunUntil;

		// Token: 0x040055E9 RID: 21993
		private float _nextNudge;

		// Token: 0x040055EA RID: 21994
		private float _dashAt;

		// Token: 0x040055EB RID: 21995
		private ScaredDriverFleeRunner.Phase _phase;

		// Token: 0x040055EC RID: 21996
		private bool _moved;

		// Token: 0x040055ED RID: 21997
		private bool _done;

		// Token: 0x040055EE RID: 21998
		private bool _pathPending;

		// Token: 0x040055EF RID: 21999
		private int _pathRequestVersion;

		// Token: 0x02000BA8 RID: 2984
		private enum Phase
		{
			// Token: 0x040055F1 RID: 22001
			Nudge,
			// Token: 0x040055F2 RID: 22002
			Dash,
			// Token: 0x040055F3 RID: 22003
			Path
		}
	}
}
