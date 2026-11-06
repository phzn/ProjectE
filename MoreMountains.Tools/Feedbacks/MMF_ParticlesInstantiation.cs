using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F9 RID: 505
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will instantiate the specified ParticleSystem at the specified position on Start or on Play, optionally nesting them.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Particles/Particles Instantiation")]
	public class MMF_ParticlesInstantiation : MMF_Feedback
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00039BF6 File Offset: 0x00037DF6
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x00039C04 File Offset: 0x00037E04
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.DeclaredDuration);
			}
			set
			{
				this.DeclaredDuration = value;
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00039C0D File Offset: 0x00037E0D
		protected override void CustomInitialization(MMF_Player owner)
		{
			if (!this.Active)
			{
				return;
			}
			this.CacheParticleSystem();
			this.CreatePools(owner);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00039C25 File Offset: 0x00037E25
		protected virtual bool ShouldCache
		{
			get
			{
				return (this.Mode == MMF_ParticlesInstantiation.Modes.OnDemand && this.CachedRecycle) || this.Mode == MMF_ParticlesInstantiation.Modes.Cached;
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00039C44 File Offset: 0x00037E44
		protected virtual void CreatePools(MMF_Player owner)
		{
			if (this.Mode != MMF_ParticlesInstantiation.Modes.Pool)
			{
				return;
			}
			if (this.ParticlesPrefab == null && this.RandomParticlePrefabs.Count == 0)
			{
				return;
			}
			if (!this._poolCreatedOrFound)
			{
				if (this._objectPooler != null)
				{
					this._objectPooler.DestroyObjectPool();
					owner.ProxyDestroy(this._objectPooler.gameObject);
				}
				GameObject gameObject = new GameObject();
				gameObject.name = this.Owner.name + "_ObjectPooler";
				this._objectPooler = gameObject.AddComponent<MMMiniObjectPooler>();
				this._objectPooler.GameObjectToPool = this.ParticlesPrefab.gameObject;
				this._objectPooler.PoolSize = this.ObjectPoolSize;
				this._objectPooler.NestWaitingPool = this.NestParticles;
				if (this.ParentTransform != null)
				{
					this._objectPooler.transform.SetParent(this.ParentTransform);
				}
				else
				{
					this._objectPooler.transform.SetParent(this.Owner.transform);
				}
				this._objectPooler.MutualizeWaitingPools = this.MutualizePools;
				this._objectPooler.FillObjectPool();
				if (this.Owner != null && gameObject.transform.parent == null)
				{
					SceneManager.MoveGameObjectToScene(gameObject, this.Owner.gameObject.scene);
				}
				this._poolCreatedOrFound = true;
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00039DAC File Offset: 0x00037FAC
		protected virtual void CacheParticleSystem()
		{
			if (!this.ShouldCache)
			{
				return;
			}
			this.InstantiateParticleSystem();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00039DC0 File Offset: 0x00037FC0
		protected virtual void InstantiateParticleSystem()
		{
			Transform transform = null;
			if (this.NestParticles)
			{
				if (this.PositionMode == MMF_ParticlesInstantiation.PositionModes.FeedbackPosition)
				{
					transform = this.Owner.transform;
				}
				if (this.PositionMode == MMF_ParticlesInstantiation.PositionModes.Transform)
				{
					transform = this.InstantiateParticlesPosition;
				}
			}
			if (this.RandomParticlePrefabs.Count > 0)
			{
				if (this.ShouldCache)
				{
					this._instantiatedRandomParticleSystems = new List<ParticleSystem>();
					using (List<ParticleSystem>.Enumerator enumerator = this.RandomParticlePrefabs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ParticleSystem particleSystem = enumerator.Current;
							ParticleSystem particleSystem2 = Object.Instantiate<ParticleSystem>(particleSystem, transform);
							if (transform == null)
							{
								SceneManager.MoveGameObjectToScene(particleSystem2.gameObject, this.Owner.gameObject.scene);
							}
							particleSystem2.Stop();
							this._instantiatedRandomParticleSystems.Add(particleSystem2);
						}
						goto IL_016A;
					}
				}
				int num = Random.Range(0, this.RandomParticlePrefabs.Count);
				this._instantiatedParticleSystem = Object.Instantiate<ParticleSystem>(this.RandomParticlePrefabs[num], transform);
				if (transform == null)
				{
					SceneManager.MoveGameObjectToScene(this._instantiatedParticleSystem.gameObject, this.Owner.gameObject.scene);
				}
			}
			else
			{
				if (this.ParticlesPrefab == null)
				{
					return;
				}
				this._instantiatedParticleSystem = Object.Instantiate<ParticleSystem>(this.ParticlesPrefab, transform);
				this._instantiatedParticleSystem.Stop();
				if (transform == null)
				{
					SceneManager.MoveGameObjectToScene(this._instantiatedParticleSystem.gameObject, this.Owner.gameObject.scene);
				}
			}
			IL_016A:
			if (this._instantiatedParticleSystem != null)
			{
				this.PositionParticleSystem(this._instantiatedParticleSystem);
			}
			if (this._instantiatedRandomParticleSystems != null && this._instantiatedRandomParticleSystems.Count > 0)
			{
				foreach (ParticleSystem particleSystem3 in this._instantiatedRandomParticleSystems)
				{
					this.PositionParticleSystem(particleSystem3);
				}
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00039FBC File Offset: 0x000381BC
		protected virtual void PositionParticleSystem(ParticleSystem system)
		{
			if (this.InstantiateParticlesPosition == null && this.Owner != null)
			{
				this.InstantiateParticlesPosition = this.Owner.transform;
			}
			if (system != null)
			{
				system.Stop();
				system.transform.position = this.GetPosition(this.Owner.transform.position);
				if (this.ApplyRotation)
				{
					system.transform.rotation = this.GetRotation(this.Owner.transform);
				}
				if (this.ApplyScale)
				{
					system.transform.localScale = this.GetScale(this.Owner.transform);
				}
				system.Clear();
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003A074 File Offset: 0x00038274
		protected virtual Quaternion GetRotation(Transform target)
		{
			switch (this.PositionMode)
			{
			case MMF_ParticlesInstantiation.PositionModes.FeedbackPosition:
				return this.Owner.transform.rotation;
			case MMF_ParticlesInstantiation.PositionModes.Transform:
				return this.InstantiateParticlesPosition.rotation;
			case MMF_ParticlesInstantiation.PositionModes.WorldPosition:
				return Quaternion.identity;
			case MMF_ParticlesInstantiation.PositionModes.Script:
				return this.Owner.transform.rotation;
			default:
				return this.Owner.transform.rotation;
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003A0E4 File Offset: 0x000382E4
		protected virtual Vector3 GetScale(Transform target)
		{
			switch (this.PositionMode)
			{
			case MMF_ParticlesInstantiation.PositionModes.FeedbackPosition:
				return this.Owner.transform.localScale;
			case MMF_ParticlesInstantiation.PositionModes.Transform:
				return this.InstantiateParticlesPosition.localScale;
			case MMF_ParticlesInstantiation.PositionModes.WorldPosition:
				return this.Owner.transform.localScale;
			case MMF_ParticlesInstantiation.PositionModes.Script:
				return this.Owner.transform.localScale;
			default:
				return this.Owner.transform.localScale;
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003A160 File Offset: 0x00038360
		protected virtual Vector3 GetPosition(Vector3 position)
		{
			switch (this.PositionMode)
			{
			case MMF_ParticlesInstantiation.PositionModes.FeedbackPosition:
				return this.Owner.transform.position + this.Offset;
			case MMF_ParticlesInstantiation.PositionModes.Transform:
				return this.InstantiateParticlesPosition.position + this.Offset;
			case MMF_ParticlesInstantiation.PositionModes.WorldPosition:
				return this.TargetWorldPosition + this.Offset;
			case MMF_ParticlesInstantiation.PositionModes.Script:
				return this._scriptPosition + this.Offset;
			default:
				return this._scriptPosition + this.Offset;
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003A1F4 File Offset: 0x000383F4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ParticlesInstantiation.FeedbackTypeAuthorized)
			{
				return;
			}
			this._scriptPosition = position;
			if (this.Mode == MMF_ParticlesInstantiation.Modes.Pool)
			{
				if (this._objectPooler != null)
				{
					this._newGameObject = this._objectPooler.GetPooledGameObject();
					this._instantiatedParticleSystem = this._newGameObject.MMFGetComponentNoAlloc<ParticleSystem>();
					if (this._instantiatedParticleSystem != null)
					{
						this.PositionParticleSystem(this._instantiatedParticleSystem);
						this._newGameObject.SetActive(true);
					}
				}
			}
			else if (!this.ShouldCache)
			{
				this.InstantiateParticleSystem();
			}
			else
			{
				this.GrabCachedParticleSystem();
			}
			if (this._instantiatedParticleSystem != null)
			{
				if (this.ForceSetActiveOnPlay)
				{
					this._instantiatedParticleSystem.gameObject.SetActive(true);
				}
				this._instantiatedParticleSystem.Stop();
				this._instantiatedParticleSystem.transform.position = this.GetPosition(position);
				this.PositionParticleSystem(this._instantiatedParticleSystem);
				this._instantiatedParticleSystem.gameObject.SetActive(true);
				this.PlayTargetParticleSystem(this._instantiatedParticleSystem);
			}
			if (this._instantiatedRandomParticleSystems != null && this._instantiatedRandomParticleSystems.Count > 0)
			{
				foreach (ParticleSystem particleSystem in this._instantiatedRandomParticleSystems)
				{
					if (this.ForceSetActiveOnPlay)
					{
						particleSystem.gameObject.SetActive(true);
					}
					particleSystem.Stop();
					particleSystem.transform.position = this.GetPosition(position);
				}
				int num = Random.Range(0, this._instantiatedRandomParticleSystems.Count);
				this.PlayTargetParticleSystem(this._instantiatedRandomParticleSystems[num]);
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003A3AC File Offset: 0x000385AC
		protected virtual void PlayTargetParticleSystem(ParticleSystem targetParticleSystem)
		{
			if (this.ForceSimulationSpeed)
			{
				targetParticleSystem.main.simulationSpeed = Random.Range(this.ForcedSimulationSpeed.x, this.ForcedSimulationSpeed.y);
			}
			targetParticleSystem.Play();
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003A3F0 File Offset: 0x000385F0
		protected virtual void GrabCachedParticleSystem()
		{
			if (this.RandomParticlePrefabs.Count > 0)
			{
				int num = Random.Range(0, this.RandomParticlePrefabs.Count);
				this._instantiatedParticleSystem = this._instantiatedRandomParticleSystems[num];
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003A430 File Offset: 0x00038630
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ParticlesInstantiation.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this._instantiatedParticleSystem != null)
			{
				ParticleSystem instantiatedParticleSystem = this._instantiatedParticleSystem;
				if (instantiatedParticleSystem != null)
				{
					instantiatedParticleSystem.Stop();
				}
			}
			if (this._instantiatedRandomParticleSystems != null && this._instantiatedRandomParticleSystems.Count > 0)
			{
				foreach (ParticleSystem particleSystem in this._instantiatedRandomParticleSystems)
				{
					particleSystem.Stop();
				}
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0003A4C8 File Offset: 0x000386C8
		protected override void CustomReset()
		{
			base.CustomReset();
			if (!this.Active || !MMF_ParticlesInstantiation.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.InCooldown)
			{
				return;
			}
			if (this.StopOnReset && this._instantiatedParticleSystem != null)
			{
				this._instantiatedParticleSystem.Stop();
			}
			if (this._instantiatedRandomParticleSystems != null && this._instantiatedRandomParticleSystems.Count > 0)
			{
				foreach (ParticleSystem particleSystem in this._instantiatedRandomParticleSystems)
				{
					particleSystem.Stop();
				}
			}
		}

		// Token: 0x04000F34 RID: 3892
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F35 RID: 3893
		[MMFInspectorGroup("Particles Instantiation", true, 37, true, false)]
		[Tooltip("whether the particle system should be cached or created on demand the first time")]
		public MMF_ParticlesInstantiation.Modes Mode = MMF_ParticlesInstantiation.Modes.Pool;

		// Token: 0x04000F36 RID: 3894
		[Tooltip("the initial and planned size of this object pool")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public int ObjectPoolSize = 5;

		// Token: 0x04000F37 RID: 3895
		[Tooltip("whether or not to create a new pool even if one already exists for that same prefab")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public bool MutualizePools;

		// Token: 0x04000F38 RID: 3896
		[Tooltip("if specified, the instantiated object (or the pool of objects) will be parented to this transform ")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Transform ParentTransform;

		// Token: 0x04000F39 RID: 3897
		[Tooltip("if this is false, a brand new particle system will be created every time")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool CachedRecycle = true;

		// Token: 0x04000F3A RID: 3898
		[Tooltip("the particle system to spawn")]
		public ParticleSystem ParticlesPrefab;

		// Token: 0x04000F3B RID: 3899
		[Tooltip("the possible random particle systems")]
		public List<ParticleSystem> RandomParticlePrefabs;

		// Token: 0x04000F3C RID: 3900
		[Tooltip("if this is true, the particle system game object will be activated on Play, useful if you've somehow disabled it in a past Play")]
		public bool ForceSetActiveOnPlay;

		// Token: 0x04000F3D RID: 3901
		[Tooltip("if this is true, the particle system will be stopped every time the feedback is reset - usually before play")]
		public bool StopOnReset;

		// Token: 0x04000F3E RID: 3902
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04000F3F RID: 3903
		[MMFInspectorGroup("Position", true, 29, false, false)]
		[Tooltip("the selected position mode")]
		public MMF_ParticlesInstantiation.PositionModes PositionMode;

		// Token: 0x04000F40 RID: 3904
		[Tooltip("the position at which to spawn this particle system")]
		[MMFEnumCondition("PositionMode", new int[] { 1 })]
		public Transform InstantiateParticlesPosition;

		// Token: 0x04000F41 RID: 3905
		[Tooltip("the world position to move to when in WorldPosition mode")]
		[MMFEnumCondition("PositionMode", new int[] { 2 })]
		public Vector3 TargetWorldPosition;

		// Token: 0x04000F42 RID: 3906
		[Tooltip("an offset to apply to the instantiation position")]
		public Vector3 Offset;

		// Token: 0x04000F43 RID: 3907
		[Tooltip("whether or not the particle system should be nested in hierarchy or floating on its own")]
		[MMFEnumCondition("PositionMode", new int[] { 1, 0 })]
		public bool NestParticles = true;

		// Token: 0x04000F44 RID: 3908
		[Tooltip("whether or not to also apply rotation")]
		public bool ApplyRotation;

		// Token: 0x04000F45 RID: 3909
		[Tooltip("whether or not to also apply scale")]
		public bool ApplyScale;

		// Token: 0x04000F46 RID: 3910
		[MMFInspectorGroup("Simulation Speed", true, 43, false, false)]
		[Tooltip("whether or not to force a specific simulation speed on the target particle system(s)")]
		public bool ForceSimulationSpeed;

		// Token: 0x04000F47 RID: 3911
		[Tooltip("The min and max values at which to randomize the simulation speed, if ForceSimulationSpeed is true. A new value will be randomized every time this feedback plays")]
		[MMFCondition("ForceSimulationSpeed", true)]
		public Vector2 ForcedSimulationSpeed = new Vector2(0.1f, 1f);

		// Token: 0x04000F48 RID: 3912
		protected ParticleSystem _instantiatedParticleSystem;

		// Token: 0x04000F49 RID: 3913
		protected List<ParticleSystem> _instantiatedRandomParticleSystems;

		// Token: 0x04000F4A RID: 3914
		protected MMMiniObjectPooler _objectPooler;

		// Token: 0x04000F4B RID: 3915
		protected GameObject _newGameObject;

		// Token: 0x04000F4C RID: 3916
		protected bool _poolCreatedOrFound;

		// Token: 0x04000F4D RID: 3917
		protected Vector3 _scriptPosition;

		// Token: 0x020004CD RID: 1229
		public enum PositionModes
		{
			// Token: 0x0400225A RID: 8794
			FeedbackPosition,
			// Token: 0x0400225B RID: 8795
			Transform,
			// Token: 0x0400225C RID: 8796
			WorldPosition,
			// Token: 0x0400225D RID: 8797
			Script
		}

		// Token: 0x020004CE RID: 1230
		public enum Modes
		{
			// Token: 0x0400225F RID: 8799
			Cached,
			// Token: 0x04002260 RID: 8800
			OnDemand,
			// Token: 0x04002261 RID: 8801
			Pool
		}
	}
}
