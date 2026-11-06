using System;
using System.Collections.Generic;
using Features.NPC;
using MagicaCloth2;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace Features.TimeFreeze
{
	// Token: 0x02000C1B RID: 3099
	internal sealed class TimeFreezeWorldRamp
	{
		// Token: 0x060059C9 RID: 22985 RVA: 0x00231D7C File Offset: 0x0022FF7C
		public TimeFreezeWorldRamp(Transform playerRoot)
		{
			Animator animator = ((playerRoot != null) ? playerRoot.GetComponentInParent<Animator>(true) : null);
			this._playerRoot = ((animator != null) ? animator.transform : playerRoot);
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x00231E71 File Offset: 0x00230071
		public void Capture()
		{
			if (this._captured)
			{
				return;
			}
			this._captured = true;
			this._currentFactor = 1f;
			this.DiscoverParticipants();
			this._nextDynamicDiscoveryTime = Time.unscaledTime + 0.5f;
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x00231EA8 File Offset: 0x002300A8
		public void RefreshDynamicParticipants(bool force = false)
		{
			if (!this._captured || NPCSimulationPause.IsPaused)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (!force && unscaledTime < this._nextDynamicDiscoveryTime)
			{
				return;
			}
			this._nextDynamicDiscoveryTime = unscaledTime + 0.5f;
			this.PruneDestroyedParticipants();
			this.DiscoverParticipants();
			this.ApplyFactor(this._currentFactor);
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x00231F00 File Offset: 0x00230100
		private void DiscoverParticipants()
		{
			foreach (Animator animator in Object.FindObjectsByType<Animator>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(animator) && animator.enabled)
				{
					int instanceID = animator.GetInstanceID();
					if (this._animatorIds.Add(instanceID))
					{
						this._animators.Add(new TimeFreezeWorldRamp.AnimatorState
						{
							id = instanceID,
							value = animator,
							speed = animator.speed
						});
					}
				}
			}
			foreach (FollowerEntity followerEntity in Object.FindObjectsByType<FollowerEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(followerEntity) && followerEntity.enabled && followerEntity.entityExists)
				{
					int instanceID2 = followerEntity.GetInstanceID();
					if (this._followerIds.Add(instanceID2))
					{
						this._followers.Add(new TimeFreezeWorldRamp.FollowerState
						{
							id = instanceID2,
							value = followerEntity,
							speed = followerEntity.maxSpeed
						});
					}
				}
			}
			foreach (NavMeshAgent navMeshAgent in Object.FindObjectsByType<NavMeshAgent>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(navMeshAgent) && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
				{
					int instanceID3 = navMeshAgent.GetInstanceID();
					if (this._agentIds.Add(instanceID3))
					{
						this._agents.Add(new TimeFreezeWorldRamp.AgentState
						{
							id = instanceID3,
							value = navMeshAgent,
							speed = navMeshAgent.speed,
							acceleration = navMeshAgent.acceleration,
							angularSpeed = navMeshAgent.angularSpeed,
							velocity = navMeshAgent.velocity
						});
					}
				}
			}
			foreach (Rigidbody rigidbody in Object.FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(rigidbody) && !rigidbody.isKinematic && rigidbody.gameObject.activeInHierarchy)
				{
					int instanceID4 = rigidbody.GetInstanceID();
					if (this._bodyIds.Add(instanceID4))
					{
						this._bodies.Add(new TimeFreezeWorldRamp.BodyState
						{
							id = instanceID4,
							value = rigidbody,
							velocity = rigidbody.velocity,
							angularVelocity = rigidbody.angularVelocity
						});
					}
				}
			}
			foreach (ParticleSystem particleSystem in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(particleSystem))
				{
					int instanceID5 = particleSystem.GetInstanceID();
					if (this._particleIds.Add(instanceID5))
					{
						ParticleSystem.MainModule main = particleSystem.main;
						this._particles.Add(new TimeFreezeWorldRamp.ParticleState
						{
							id = instanceID5,
							value = particleSystem,
							simulationSpeed = main.simulationSpeed
						});
					}
				}
			}
			foreach (VisualEffect visualEffect in Object.FindObjectsByType<VisualEffect>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(visualEffect))
				{
					int instanceID6 = visualEffect.GetInstanceID();
					if (this._vfxIds.Add(instanceID6))
					{
						this._vfx.Add(new TimeFreezeWorldRamp.VfxState
						{
							id = instanceID6,
							value = visualEffect,
							playRate = visualEffect.playRate
						});
					}
				}
			}
			foreach (MagicaCloth magicaCloth in Object.FindObjectsByType<MagicaCloth>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!this.Skip(magicaCloth) && magicaCloth.isActiveAndEnabled)
				{
					int instanceID7 = magicaCloth.GetInstanceID();
					if (this._clothIds.Add(instanceID7))
					{
						this._cloth.Add(new TimeFreezeWorldRamp.ClothState
						{
							id = instanceID7,
							value = magicaCloth,
							timeScale = magicaCloth.GetTimeScale()
						});
					}
				}
			}
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x0023228F File Offset: 0x0023048F
		public void SetFactor(float factor)
		{
			if (!this._captured)
			{
				return;
			}
			this.RefreshDynamicParticipants(false);
			factor = Mathf.Clamp01(factor);
			this._currentFactor = factor;
			this.ApplyFactor(factor);
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x002322B8 File Offset: 0x002304B8
		private void ApplyFactor(float factor)
		{
			NPCSimulationPause.SetSimulationScale(factor);
			for (int i = 0; i < this._animators.Count; i++)
			{
				TimeFreezeWorldRamp.AnimatorState animatorState = this._animators[i];
				if (animatorState.value != null)
				{
					animatorState.value.speed = animatorState.speed * factor;
				}
			}
			for (int j = 0; j < this._followers.Count; j++)
			{
				TimeFreezeWorldRamp.FollowerState followerState = this._followers[j];
				if (followerState.value != null && followerState.value.enabled && followerState.value.entityExists)
				{
					followerState.value.maxSpeed = followerState.speed * factor;
				}
			}
			for (int k = 0; k < this._agents.Count; k++)
			{
				TimeFreezeWorldRamp.AgentState agentState = this._agents[k];
				if (!(agentState.value == null) && agentState.value.enabled && agentState.value.isOnNavMesh)
				{
					agentState.value.speed = agentState.speed * factor;
					agentState.value.acceleration = agentState.acceleration * factor;
					agentState.value.angularSpeed = agentState.angularSpeed * factor;
					agentState.value.velocity = agentState.velocity * factor;
				}
			}
			for (int l = 0; l < this._bodies.Count; l++)
			{
				TimeFreezeWorldRamp.BodyState bodyState = this._bodies[l];
				if (!(bodyState.value == null) && !bodyState.value.isKinematic)
				{
					bodyState.value.velocity = bodyState.velocity * factor;
					bodyState.value.angularVelocity = bodyState.angularVelocity * factor;
				}
			}
			for (int m = 0; m < this._particles.Count; m++)
			{
				TimeFreezeWorldRamp.ParticleState particleState = this._particles[m];
				if (!(particleState.value == null))
				{
					particleState.value.main.simulationSpeed = particleState.simulationSpeed * factor;
				}
			}
			for (int n = 0; n < this._vfx.Count; n++)
			{
				TimeFreezeWorldRamp.VfxState vfxState = this._vfx[n];
				if (vfxState.value != null)
				{
					vfxState.value.playRate = vfxState.playRate * factor;
				}
			}
			for (int num = 0; num < this._cloth.Count; num++)
			{
				TimeFreezeWorldRamp.ClothState clothState = this._cloth[num];
				if (clothState.value != null)
				{
					clothState.value.SetTimeScale(clothState.timeScale * factor);
				}
			}
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x00232587 File Offset: 0x00230787
		public void Restore()
		{
			if (!this._captured)
			{
				return;
			}
			this.RefreshDynamicParticipants(true);
			this.SetFactor(1f);
			this._captured = false;
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x002325AC File Offset: 0x002307AC
		private void PruneDestroyedParticipants()
		{
			for (int i = this._animators.Count - 1; i >= 0; i--)
			{
				if (!(this._animators[i].value != null))
				{
					this._animatorIds.Remove(this._animators[i].id);
					this._animators.RemoveAt(i);
				}
			}
			for (int j = this._followers.Count - 1; j >= 0; j--)
			{
				if (!(this._followers[j].value != null))
				{
					this._followerIds.Remove(this._followers[j].id);
					this._followers.RemoveAt(j);
				}
			}
			for (int k = this._agents.Count - 1; k >= 0; k--)
			{
				if (!(this._agents[k].value != null))
				{
					this._agentIds.Remove(this._agents[k].id);
					this._agents.RemoveAt(k);
				}
			}
			for (int l = this._bodies.Count - 1; l >= 0; l--)
			{
				if (!(this._bodies[l].value != null))
				{
					this._bodyIds.Remove(this._bodies[l].id);
					this._bodies.RemoveAt(l);
				}
			}
			for (int m = this._particles.Count - 1; m >= 0; m--)
			{
				if (!(this._particles[m].value != null))
				{
					this._particleIds.Remove(this._particles[m].id);
					this._particles.RemoveAt(m);
				}
			}
			for (int n = this._vfx.Count - 1; n >= 0; n--)
			{
				if (!(this._vfx[n].value != null))
				{
					this._vfxIds.Remove(this._vfx[n].id);
					this._vfx.RemoveAt(n);
				}
			}
			for (int num = this._cloth.Count - 1; num >= 0; num--)
			{
				if (!(this._cloth[num].value != null))
				{
					this._clothIds.Remove(this._cloth[num].id);
					this._cloth.RemoveAt(num);
				}
			}
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x00232844 File Offset: 0x00230A44
		private bool Skip(Component component)
		{
			if (component == null || !component.gameObject.scene.IsValid())
			{
				return true;
			}
			Transform transform = component.transform;
			return (this._playerRoot != null && (transform == this._playerRoot || transform.IsChildOf(this._playerRoot))) || transform.GetComponentInParent<Canvas>(true) != null;
		}

		// Token: 0x04005A85 RID: 23173
		private const float DynamicDiscoveryInterval = 0.5f;

		// Token: 0x04005A86 RID: 23174
		private readonly Transform _playerRoot;

		// Token: 0x04005A87 RID: 23175
		private readonly List<TimeFreezeWorldRamp.AnimatorState> _animators = new List<TimeFreezeWorldRamp.AnimatorState>(128);

		// Token: 0x04005A88 RID: 23176
		private readonly List<TimeFreezeWorldRamp.FollowerState> _followers = new List<TimeFreezeWorldRamp.FollowerState>(64);

		// Token: 0x04005A89 RID: 23177
		private readonly List<TimeFreezeWorldRamp.AgentState> _agents = new List<TimeFreezeWorldRamp.AgentState>(64);

		// Token: 0x04005A8A RID: 23178
		private readonly List<TimeFreezeWorldRamp.BodyState> _bodies = new List<TimeFreezeWorldRamp.BodyState>(64);

		// Token: 0x04005A8B RID: 23179
		private readonly List<TimeFreezeWorldRamp.ParticleState> _particles = new List<TimeFreezeWorldRamp.ParticleState>(64);

		// Token: 0x04005A8C RID: 23180
		private readonly List<TimeFreezeWorldRamp.VfxState> _vfx = new List<TimeFreezeWorldRamp.VfxState>(32);

		// Token: 0x04005A8D RID: 23181
		private readonly List<TimeFreezeWorldRamp.ClothState> _cloth = new List<TimeFreezeWorldRamp.ClothState>(64);

		// Token: 0x04005A8E RID: 23182
		private readonly HashSet<int> _animatorIds = new HashSet<int>();

		// Token: 0x04005A8F RID: 23183
		private readonly HashSet<int> _followerIds = new HashSet<int>();

		// Token: 0x04005A90 RID: 23184
		private readonly HashSet<int> _agentIds = new HashSet<int>();

		// Token: 0x04005A91 RID: 23185
		private readonly HashSet<int> _bodyIds = new HashSet<int>();

		// Token: 0x04005A92 RID: 23186
		private readonly HashSet<int> _particleIds = new HashSet<int>();

		// Token: 0x04005A93 RID: 23187
		private readonly HashSet<int> _vfxIds = new HashSet<int>();

		// Token: 0x04005A94 RID: 23188
		private readonly HashSet<int> _clothIds = new HashSet<int>();

		// Token: 0x04005A95 RID: 23189
		private bool _captured;

		// Token: 0x04005A96 RID: 23190
		private float _currentFactor = 1f;

		// Token: 0x04005A97 RID: 23191
		private float _nextDynamicDiscoveryTime;

		// Token: 0x02000C1C RID: 3100
		private sealed class AnimatorState
		{
			// Token: 0x04005A98 RID: 23192
			public int id;

			// Token: 0x04005A99 RID: 23193
			public Animator value;

			// Token: 0x04005A9A RID: 23194
			public float speed;
		}

		// Token: 0x02000C1D RID: 3101
		private sealed class FollowerState
		{
			// Token: 0x04005A9B RID: 23195
			public int id;

			// Token: 0x04005A9C RID: 23196
			public FollowerEntity value;

			// Token: 0x04005A9D RID: 23197
			public float speed;
		}

		// Token: 0x02000C1E RID: 3102
		private sealed class AgentState
		{
			// Token: 0x04005A9E RID: 23198
			public int id;

			// Token: 0x04005A9F RID: 23199
			public NavMeshAgent value;

			// Token: 0x04005AA0 RID: 23200
			public float speed;

			// Token: 0x04005AA1 RID: 23201
			public float acceleration;

			// Token: 0x04005AA2 RID: 23202
			public float angularSpeed;

			// Token: 0x04005AA3 RID: 23203
			public Vector3 velocity;
		}

		// Token: 0x02000C1F RID: 3103
		private sealed class ParticleState
		{
			// Token: 0x04005AA4 RID: 23204
			public int id;

			// Token: 0x04005AA5 RID: 23205
			public ParticleSystem value;

			// Token: 0x04005AA6 RID: 23206
			public float simulationSpeed;
		}

		// Token: 0x02000C20 RID: 3104
		private sealed class BodyState
		{
			// Token: 0x04005AA7 RID: 23207
			public int id;

			// Token: 0x04005AA8 RID: 23208
			public Rigidbody value;

			// Token: 0x04005AA9 RID: 23209
			public Vector3 velocity;

			// Token: 0x04005AAA RID: 23210
			public Vector3 angularVelocity;
		}

		// Token: 0x02000C21 RID: 3105
		private sealed class VfxState
		{
			// Token: 0x04005AAB RID: 23211
			public int id;

			// Token: 0x04005AAC RID: 23212
			public VisualEffect value;

			// Token: 0x04005AAD RID: 23213
			public float playRate;
		}

		// Token: 0x02000C22 RID: 3106
		private sealed class ClothState
		{
			// Token: 0x04005AAE RID: 23214
			public int id;

			// Token: 0x04005AAF RID: 23215
			public MagicaCloth value;

			// Token: 0x04005AB0 RID: 23216
			public float timeScale;
		}
	}
}
