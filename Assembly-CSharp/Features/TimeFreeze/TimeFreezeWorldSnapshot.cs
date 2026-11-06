using System;
using System.Collections.Generic;
using Features.NPC;
using Features.RunModes;
using MagicaCloth2;
using Pathfinding;
using ProjectExhibitionist;
using ProjectExhibitionist.Subway;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.VFX;
using VehicleSpawnPlugin;

namespace Features.TimeFreeze
{
	// Token: 0x02000C23 RID: 3107
	internal sealed class TimeFreezeWorldSnapshot
	{
		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060059D9 RID: 23001 RVA: 0x002328B1 File Offset: 0x00230AB1
		public bool IsFrozen
		{
			get
			{
				return this._frozen;
			}
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x002328BC File Offset: 0x00230ABC
		public TimeFreezeWorldSnapshot(Transform playerRoot, float minimumPitch)
		{
			Animator animator = ((playerRoot != null) ? playerRoot.GetComponentInParent<Animator>(true) : null);
			this._playerRoot = ((animator != null) ? animator.transform : playerRoot);
			this._audio = new TimeFreezeAudioSnapshot(this._playerRoot, minimumPitch, null);
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x002329D0 File Offset: 0x00230BD0
		public void Capture()
		{
			if (this._captured)
			{
				return;
			}
			this._captured = true;
			this._audio.Capture();
			foreach (Animator animator in Object.FindObjectsByType<Animator>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(animator) && animator.enabled)
				{
					this._animators.Add(new TimeFreezeWorldSnapshot.AnimatorState
					{
						value = animator,
						speed = animator.speed
					});
				}
			}
			foreach (FollowerEntity followerEntity in Object.FindObjectsByType<FollowerEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(followerEntity) && followerEntity.enabled && followerEntity.entityExists)
				{
					this._followers.Add(new TimeFreezeWorldSnapshot.FollowerState
					{
						value = followerEntity,
						stopped = followerEntity.isStopped,
						speed = followerEntity.maxSpeed
					});
				}
			}
			foreach (NavMeshAgent navMeshAgent in Object.FindObjectsByType<NavMeshAgent>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(navMeshAgent) && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
				{
					this._agents.Add(new TimeFreezeWorldSnapshot.AgentState
					{
						value = navMeshAgent,
						stopped = navMeshAgent.isStopped
					});
				}
			}
			foreach (Rigidbody rigidbody in Object.FindObjectsByType<Rigidbody>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(rigidbody) && !rigidbody.isKinematic && rigidbody.gameObject.activeInHierarchy)
				{
					this._bodies.Add(new TimeFreezeWorldSnapshot.BodyState
					{
						value = rigidbody,
						kinematic = rigidbody.isKinematic,
						useGravity = rigidbody.useGravity,
						sleeping = rigidbody.IsSleeping(),
						velocity = rigidbody.velocity,
						angularVelocity = rigidbody.angularVelocity
					});
				}
			}
			foreach (ParticleSystem particleSystem in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(particleSystem))
				{
					ParticleSystem.MainModule main = particleSystem.main;
					this._particles.Add(new TimeFreezeWorldSnapshot.ParticleState
					{
						value = particleSystem,
						playing = particleSystem.isPlaying,
						paused = particleSystem.isPaused,
						simulationSpeed = main.simulationSpeed
					});
				}
			}
			foreach (VisualEffect visualEffect in Object.FindObjectsByType<VisualEffect>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(visualEffect))
				{
					this._vfx.Add(new TimeFreezeWorldSnapshot.VfxState
					{
						value = visualEffect,
						paused = visualEffect.pause,
						playRate = visualEffect.playRate
					});
				}
			}
			foreach (PlayableDirector playableDirector in Object.FindObjectsByType<PlayableDirector>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(playableDirector) && playableDirector.enabled)
				{
					this._directors.Add(new TimeFreezeWorldSnapshot.DirectorState
					{
						value = playableDirector,
						state = playableDirector.state
					});
				}
			}
			foreach (MagicaCloth magicaCloth in Object.FindObjectsByType<MagicaCloth>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(magicaCloth) && magicaCloth.isActiveAndEnabled)
				{
					this._cloth.Add(new TimeFreezeWorldSnapshot.ClothState
					{
						value = magicaCloth,
						timeScale = magicaCloth.GetTimeScale()
					});
				}
			}
			foreach (Animation animation in Object.FindObjectsByType<Animation>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(animation) && animation.enabled)
				{
					this._legacyAnimation.Add(new TimeFreezeWorldSnapshot.BehaviourState
					{
						value = animation,
						enabled = true
					});
				}
			}
			foreach (ElevatorSimple elevatorSimple in Object.FindObjectsByType<ElevatorSimple>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(elevatorSimple) && elevatorSimple.enabled)
				{
					this._elevators.Add(new TimeFreezeWorldSnapshot.ElevatorState
					{
						value = elevatorSimple,
						paused = elevatorSimple.SimulationPaused
					});
				}
			}
			foreach (SubwayDoorSystem subwayDoorSystem in Object.FindObjectsByType<SubwayDoorSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(subwayDoorSystem) && subwayDoorSystem.enabled)
				{
					this._subwayDoors.Add(new TimeFreezeWorldSnapshot.SubwayDoorState
					{
						value = subwayDoorSystem,
						paused = subwayDoorSystem.SimulationPaused
					});
				}
			}
			foreach (DoorSystemController doorSystemController in Object.FindObjectsByType<DoorSystemController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(doorSystemController) && doorSystemController.enabled)
				{
					this._doorSystems.Add(new TimeFreezeWorldSnapshot.DoorSystemState
					{
						value = doorSystemController,
						paused = doorSystemController.SimulationPaused
					});
				}
			}
			foreach (DoorInteraction doorInteraction in Object.FindObjectsByType<DoorInteraction>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(doorInteraction) && doorInteraction.enabled)
				{
					this._doorInteractions.Add(new TimeFreezeWorldSnapshot.DoorInteractionState
					{
						value = doorInteraction,
						paused = doorInteraction.SimulationPaused
					});
				}
			}
			foreach (RunModeRuntimeController runModeRuntimeController in Object.FindObjectsByType<RunModeRuntimeController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!this.SkipWorldComponent(runModeRuntimeController) && runModeRuntimeController.enabled)
				{
					this._runModes.Add(new TimeFreezeWorldSnapshot.RunModeState
					{
						value = runModeRuntimeController,
						paused = runModeRuntimeController.SimulationPaused
					});
				}
			}
			foreach (MonoBehaviour monoBehaviour in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				if (!(monoBehaviour == null) && monoBehaviour.enabled && !this.SkipWorldComponent(monoBehaviour) && TimeFreezeWorldSnapshot.ExplicitWorldMoverTypes.Contains(monoBehaviour.GetType().Name))
				{
					this._explicitMovers.Add(new TimeFreezeWorldSnapshot.BehaviourState
					{
						value = monoBehaviour,
						enabled = true
					});
				}
			}
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x00232F92 File Offset: 0x00231192
		public void SetAudioSlowFactor(float factor)
		{
			this._audio.SetWorldPitchFactor(factor);
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x00232FA0 File Offset: 0x002311A0
		public void Freeze()
		{
			if (this._frozen)
			{
				return;
			}
			if (!this._captured)
			{
				this.Capture();
			}
			this._frozen = true;
			this._npcPauseLease = NPCSimulationPause.AcquirePause();
			this._vehiclePauseLease = VehicleSimulationPause.AcquirePause();
			for (int i = 0; i < this._followers.Count; i++)
			{
				TimeFreezeWorldSnapshot.FollowerState followerState = this._followers[i];
				if (!(followerState.value == null) && followerState.value.enabled && followerState.value.entityExists)
				{
					followerState.value.maxSpeed = 0f;
					followerState.value.isStopped = true;
				}
			}
			for (int j = 0; j < this._agents.Count; j++)
			{
				TimeFreezeWorldSnapshot.AgentState agentState = this._agents[j];
				if (agentState.value != null && agentState.value.enabled && agentState.value.isOnNavMesh)
				{
					agentState.value.isStopped = true;
				}
			}
			for (int k = 0; k < this._animators.Count; k++)
			{
				if (this._animators[k].value != null)
				{
					this._animators[k].value.speed = 0f;
				}
			}
			for (int l = 0; l < this._bodies.Count; l++)
			{
				TimeFreezeWorldSnapshot.BodyState bodyState = this._bodies[l];
				if (!(bodyState.value == null))
				{
					bodyState.value.velocity = Vector3.zero;
					bodyState.value.angularVelocity = Vector3.zero;
					bodyState.value.useGravity = false;
					bodyState.value.isKinematic = true;
				}
			}
			for (int m = 0; m < this._particles.Count; m++)
			{
				TimeFreezeWorldSnapshot.ParticleState particleState = this._particles[m];
				if (!(particleState.value == null))
				{
					particleState.value.main.simulationSpeed = 0f;
					if (particleState.playing)
					{
						particleState.value.Pause(true);
					}
				}
			}
			for (int n = 0; n < this._vfx.Count; n++)
			{
				if (this._vfx[n].value != null)
				{
					this._vfx[n].value.pause = true;
				}
			}
			for (int num = 0; num < this._directors.Count; num++)
			{
				if (this._directors[num].value != null && this._directors[num].state == PlayState.Playing)
				{
					this._directors[num].value.Pause();
				}
			}
			for (int num2 = 0; num2 < this._cloth.Count; num2++)
			{
				if (this._cloth[num2].value != null)
				{
					this._cloth[num2].value.SetTimeScale(0f);
				}
			}
			for (int num3 = 0; num3 < this._legacyAnimation.Count; num3++)
			{
				if (this._legacyAnimation[num3].value != null)
				{
					this._legacyAnimation[num3].value.enabled = false;
				}
			}
			for (int num4 = 0; num4 < this._elevators.Count; num4++)
			{
				if (this._elevators[num4].value != null)
				{
					this._elevators[num4].value.SimulationPaused = true;
				}
			}
			for (int num5 = 0; num5 < this._subwayDoors.Count; num5++)
			{
				if (this._subwayDoors[num5].value != null)
				{
					this._subwayDoors[num5].value.SimulationPaused = true;
				}
			}
			for (int num6 = 0; num6 < this._doorSystems.Count; num6++)
			{
				if (this._doorSystems[num6].value != null)
				{
					this._doorSystems[num6].value.SimulationPaused = true;
				}
			}
			for (int num7 = 0; num7 < this._doorInteractions.Count; num7++)
			{
				if (this._doorInteractions[num7].value != null)
				{
					this._doorInteractions[num7].value.SimulationPaused = true;
				}
			}
			for (int num8 = 0; num8 < this._runModes.Count; num8++)
			{
				if (this._runModes[num8].value != null)
				{
					this._runModes[num8].value.SimulationPaused = true;
				}
			}
			for (int num9 = 0; num9 < this._explicitMovers.Count; num9++)
			{
				if (this._explicitMovers[num9].value != null)
				{
					this._explicitMovers[num9].value.enabled = false;
				}
			}
			this._audio.Freeze();
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x002334E8 File Offset: 0x002316E8
		public void Thaw(float audioFadeDuration = 0.25f)
		{
			if (!this._frozen)
			{
				return;
			}
			for (int i = 0; i < this._animators.Count; i++)
			{
				if (this._animators[i].value != null)
				{
					this._animators[i].value.speed = this._animators[i].speed;
				}
			}
			for (int j = 0; j < this._followers.Count; j++)
			{
				TimeFreezeWorldSnapshot.FollowerState followerState = this._followers[j];
				if (!(followerState.value == null) && followerState.value.enabled && followerState.value.entityExists)
				{
					followerState.value.maxSpeed = followerState.speed;
					followerState.value.isStopped = followerState.stopped;
				}
			}
			for (int k = 0; k < this._agents.Count; k++)
			{
				TimeFreezeWorldSnapshot.AgentState agentState = this._agents[k];
				if (agentState.value != null && agentState.value.enabled && agentState.value.isOnNavMesh)
				{
					agentState.value.isStopped = agentState.stopped;
				}
			}
			for (int l = 0; l < this._bodies.Count; l++)
			{
				TimeFreezeWorldSnapshot.BodyState bodyState = this._bodies[l];
				if (!(bodyState.value == null))
				{
					bodyState.value.isKinematic = bodyState.kinematic;
					bodyState.value.useGravity = bodyState.useGravity;
					bodyState.value.velocity = bodyState.velocity;
					bodyState.value.angularVelocity = bodyState.angularVelocity;
					if (bodyState.sleeping)
					{
						bodyState.value.Sleep();
					}
				}
			}
			for (int m = 0; m < this._particles.Count; m++)
			{
				TimeFreezeWorldSnapshot.ParticleState particleState = this._particles[m];
				if (!(particleState.value == null))
				{
					particleState.value.main.simulationSpeed = particleState.simulationSpeed;
					if (particleState.playing)
					{
						particleState.value.Play(true);
					}
					else if (particleState.paused)
					{
						particleState.value.Pause(true);
					}
				}
			}
			for (int n = 0; n < this._vfx.Count; n++)
			{
				TimeFreezeWorldSnapshot.VfxState vfxState = this._vfx[n];
				if (!(vfxState.value == null))
				{
					vfxState.value.playRate = vfxState.playRate;
					vfxState.value.pause = vfxState.paused;
				}
			}
			for (int num = 0; num < this._directors.Count; num++)
			{
				if (this._directors[num].value != null && this._directors[num].state == PlayState.Playing)
				{
					this._directors[num].value.Resume();
				}
			}
			for (int num2 = 0; num2 < this._cloth.Count; num2++)
			{
				if (this._cloth[num2].value != null)
				{
					this._cloth[num2].value.SetTimeScale(this._cloth[num2].timeScale);
				}
			}
			for (int num3 = 0; num3 < this._legacyAnimation.Count; num3++)
			{
				if (this._legacyAnimation[num3].value != null)
				{
					this._legacyAnimation[num3].value.enabled = this._legacyAnimation[num3].enabled;
				}
			}
			for (int num4 = 0; num4 < this._explicitMovers.Count; num4++)
			{
				if (this._explicitMovers[num4].value != null)
				{
					this._explicitMovers[num4].value.enabled = this._explicitMovers[num4].enabled;
				}
			}
			for (int num5 = 0; num5 < this._elevators.Count; num5++)
			{
				if (this._elevators[num5].value != null)
				{
					this._elevators[num5].value.SimulationPaused = this._elevators[num5].paused;
				}
			}
			for (int num6 = 0; num6 < this._subwayDoors.Count; num6++)
			{
				if (this._subwayDoors[num6].value != null)
				{
					this._subwayDoors[num6].value.SimulationPaused = this._subwayDoors[num6].paused;
				}
			}
			for (int num7 = 0; num7 < this._doorSystems.Count; num7++)
			{
				if (this._doorSystems[num7].value != null)
				{
					this._doorSystems[num7].value.SimulationPaused = this._doorSystems[num7].paused;
				}
			}
			for (int num8 = 0; num8 < this._doorInteractions.Count; num8++)
			{
				if (this._doorInteractions[num8].value != null)
				{
					this._doorInteractions[num8].value.SimulationPaused = this._doorInteractions[num8].paused;
				}
			}
			for (int num9 = 0; num9 < this._runModes.Count; num9++)
			{
				if (this._runModes[num9].value != null)
				{
					this._runModes[num9].value.SimulationPaused = this._runModes[num9].paused;
				}
			}
			IDisposable npcPauseLease = this._npcPauseLease;
			if (npcPauseLease != null)
			{
				npcPauseLease.Dispose();
			}
			this._npcPauseLease = null;
			IDisposable vehiclePauseLease = this._vehiclePauseLease;
			if (vehiclePauseLease != null)
			{
				vehiclePauseLease.Dispose();
			}
			this._vehiclePauseLease = null;
			this._audio.Thaw(audioFadeDuration);
			this._frozen = false;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x00233B2D File Offset: 0x00231D2D
		public void RestoreAll()
		{
			this.Thaw(0f);
			this._audio.RestoreImmediate();
			this._audio.Dispose();
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x00233B50 File Offset: 0x00231D50
		private bool SkipWorldComponent(Component component)
		{
			return component == null || !component.gameObject.scene.IsValid() || this.IsUnderPlayer(component.transform) || TimeFreezeWorldSnapshot.IsUiOwned(component.transform);
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x00233B98 File Offset: 0x00231D98
		private bool IsUnderPlayer(Transform target)
		{
			return target != null && this._playerRoot != null && (target == this._playerRoot || target.IsChildOf(this._playerRoot));
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x00233BCF File Offset: 0x00231DCF
		private static bool IsUiOwned(Transform target)
		{
			return target != null && target.GetComponentInParent<Canvas>(true) != null;
		}

		// Token: 0x04005AB1 RID: 23217
		private static readonly HashSet<string> ExplicitWorldMoverTypes = new HashSet<string> { "AzureCoreSystem", "MallEscalatorConveyor" };

		// Token: 0x04005AB2 RID: 23218
		private readonly Transform _playerRoot;

		// Token: 0x04005AB3 RID: 23219
		private readonly TimeFreezeAudioSnapshot _audio;

		// Token: 0x04005AB4 RID: 23220
		private readonly List<TimeFreezeWorldSnapshot.AnimatorState> _animators = new List<TimeFreezeWorldSnapshot.AnimatorState>(128);

		// Token: 0x04005AB5 RID: 23221
		private readonly List<TimeFreezeWorldSnapshot.FollowerState> _followers = new List<TimeFreezeWorldSnapshot.FollowerState>(64);

		// Token: 0x04005AB6 RID: 23222
		private readonly List<TimeFreezeWorldSnapshot.AgentState> _agents = new List<TimeFreezeWorldSnapshot.AgentState>(32);

		// Token: 0x04005AB7 RID: 23223
		private readonly List<TimeFreezeWorldSnapshot.BodyState> _bodies = new List<TimeFreezeWorldSnapshot.BodyState>(64);

		// Token: 0x04005AB8 RID: 23224
		private readonly List<TimeFreezeWorldSnapshot.ParticleState> _particles = new List<TimeFreezeWorldSnapshot.ParticleState>(64);

		// Token: 0x04005AB9 RID: 23225
		private readonly List<TimeFreezeWorldSnapshot.VfxState> _vfx = new List<TimeFreezeWorldSnapshot.VfxState>(32);

		// Token: 0x04005ABA RID: 23226
		private readonly List<TimeFreezeWorldSnapshot.DirectorState> _directors = new List<TimeFreezeWorldSnapshot.DirectorState>(16);

		// Token: 0x04005ABB RID: 23227
		private readonly List<TimeFreezeWorldSnapshot.ClothState> _cloth = new List<TimeFreezeWorldSnapshot.ClothState>(64);

		// Token: 0x04005ABC RID: 23228
		private readonly List<TimeFreezeWorldSnapshot.BehaviourState> _legacyAnimation = new List<TimeFreezeWorldSnapshot.BehaviourState>(16);

		// Token: 0x04005ABD RID: 23229
		private readonly List<TimeFreezeWorldSnapshot.BehaviourState> _explicitMovers = new List<TimeFreezeWorldSnapshot.BehaviourState>(16);

		// Token: 0x04005ABE RID: 23230
		private readonly List<TimeFreezeWorldSnapshot.ElevatorState> _elevators = new List<TimeFreezeWorldSnapshot.ElevatorState>(8);

		// Token: 0x04005ABF RID: 23231
		private readonly List<TimeFreezeWorldSnapshot.SubwayDoorState> _subwayDoors = new List<TimeFreezeWorldSnapshot.SubwayDoorState>(8);

		// Token: 0x04005AC0 RID: 23232
		private readonly List<TimeFreezeWorldSnapshot.DoorSystemState> _doorSystems = new List<TimeFreezeWorldSnapshot.DoorSystemState>(4);

		// Token: 0x04005AC1 RID: 23233
		private readonly List<TimeFreezeWorldSnapshot.DoorInteractionState> _doorInteractions = new List<TimeFreezeWorldSnapshot.DoorInteractionState>(32);

		// Token: 0x04005AC2 RID: 23234
		private readonly List<TimeFreezeWorldSnapshot.RunModeState> _runModes = new List<TimeFreezeWorldSnapshot.RunModeState>(2);

		// Token: 0x04005AC3 RID: 23235
		private bool _captured;

		// Token: 0x04005AC4 RID: 23236
		private bool _frozen;

		// Token: 0x04005AC5 RID: 23237
		private IDisposable _npcPauseLease;

		// Token: 0x04005AC6 RID: 23238
		private IDisposable _vehiclePauseLease;

		// Token: 0x02000C24 RID: 3108
		private sealed class AnimatorState
		{
			// Token: 0x04005AC7 RID: 23239
			public Animator value;

			// Token: 0x04005AC8 RID: 23240
			public float speed;
		}

		// Token: 0x02000C25 RID: 3109
		private sealed class FollowerState
		{
			// Token: 0x04005AC9 RID: 23241
			public FollowerEntity value;

			// Token: 0x04005ACA RID: 23242
			public bool stopped;

			// Token: 0x04005ACB RID: 23243
			public float speed;
		}

		// Token: 0x02000C26 RID: 3110
		private sealed class AgentState
		{
			// Token: 0x04005ACC RID: 23244
			public NavMeshAgent value;

			// Token: 0x04005ACD RID: 23245
			public bool stopped;
		}

		// Token: 0x02000C27 RID: 3111
		private sealed class BodyState
		{
			// Token: 0x04005ACE RID: 23246
			public Rigidbody value;

			// Token: 0x04005ACF RID: 23247
			public bool kinematic;

			// Token: 0x04005AD0 RID: 23248
			public bool useGravity;

			// Token: 0x04005AD1 RID: 23249
			public bool sleeping;

			// Token: 0x04005AD2 RID: 23250
			public Vector3 velocity;

			// Token: 0x04005AD3 RID: 23251
			public Vector3 angularVelocity;
		}

		// Token: 0x02000C28 RID: 3112
		private sealed class ParticleState
		{
			// Token: 0x04005AD4 RID: 23252
			public ParticleSystem value;

			// Token: 0x04005AD5 RID: 23253
			public bool playing;

			// Token: 0x04005AD6 RID: 23254
			public bool paused;

			// Token: 0x04005AD7 RID: 23255
			public float simulationSpeed;
		}

		// Token: 0x02000C29 RID: 3113
		private sealed class VfxState
		{
			// Token: 0x04005AD8 RID: 23256
			public VisualEffect value;

			// Token: 0x04005AD9 RID: 23257
			public bool paused;

			// Token: 0x04005ADA RID: 23258
			public float playRate;
		}

		// Token: 0x02000C2A RID: 3114
		private sealed class DirectorState
		{
			// Token: 0x04005ADB RID: 23259
			public PlayableDirector value;

			// Token: 0x04005ADC RID: 23260
			public PlayState state;
		}

		// Token: 0x02000C2B RID: 3115
		private sealed class ClothState
		{
			// Token: 0x04005ADD RID: 23261
			public MagicaCloth value;

			// Token: 0x04005ADE RID: 23262
			public float timeScale;
		}

		// Token: 0x02000C2C RID: 3116
		private sealed class BehaviourState
		{
			// Token: 0x04005ADF RID: 23263
			public Behaviour value;

			// Token: 0x04005AE0 RID: 23264
			public bool enabled;
		}

		// Token: 0x02000C2D RID: 3117
		private sealed class ElevatorState
		{
			// Token: 0x04005AE1 RID: 23265
			public ElevatorSimple value;

			// Token: 0x04005AE2 RID: 23266
			public bool paused;
		}

		// Token: 0x02000C2E RID: 3118
		private sealed class SubwayDoorState
		{
			// Token: 0x04005AE3 RID: 23267
			public SubwayDoorSystem value;

			// Token: 0x04005AE4 RID: 23268
			public bool paused;
		}

		// Token: 0x02000C2F RID: 3119
		private sealed class DoorSystemState
		{
			// Token: 0x04005AE5 RID: 23269
			public DoorSystemController value;

			// Token: 0x04005AE6 RID: 23270
			public bool paused;
		}

		// Token: 0x02000C30 RID: 3120
		private sealed class DoorInteractionState
		{
			// Token: 0x04005AE7 RID: 23271
			public DoorInteraction value;

			// Token: 0x04005AE8 RID: 23272
			public bool paused;
		}

		// Token: 0x02000C31 RID: 3121
		private sealed class RunModeState
		{
			// Token: 0x04005AE9 RID: 23273
			public RunModeRuntimeController value;

			// Token: 0x04005AEA RID: 23274
			public bool paused;
		}
	}
}
