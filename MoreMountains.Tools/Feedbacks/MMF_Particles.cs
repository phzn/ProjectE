using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F8 RID: 504
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will simply play the specified ParticleSystem (from your scene) when played.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Particles/Particles Play")]
	public class MMF_Particles : MMF_Feedback
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x000398E3 File Offset: 0x00037AE3
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x000398F1 File Offset: 0x00037AF1
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000398FA File Offset: 0x00037AFA
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000398FD File Offset: 0x00037AFD
		protected override void AutomateTargetAcquisition()
		{
			this.BoundParticleSystem = this.FindAutomatedTarget<ParticleSystem>();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003990B File Offset: 0x00037B0B
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.StopSystemOnInit)
			{
				this.StopParticles();
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00039922 File Offset: 0x00037B22
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Particles.FeedbackTypeAuthorized)
			{
				return;
			}
			this.PlayParticles(position);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003993B File Offset: 0x00037B3B
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Particles.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.StopSystemOnStopFeedback)
			{
				this.StopParticles();
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003995B File Offset: 0x00037B5B
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.InCooldown)
			{
				return;
			}
			if (this.StopSystemOnReset)
			{
				this.StopParticles();
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003997C File Offset: 0x00037B7C
		protected virtual void PlayParticles(Vector3 position)
		{
			if (this.MoveToPosition)
			{
				if (this.Mode != MMF_Particles.Modes.Emit)
				{
					this.BoundParticleSystem.transform.position = position;
					using (List<ParticleSystem>.Enumerator enumerator = this.RandomParticleSystems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ParticleSystem particleSystem = enumerator.Current;
							particleSystem.transform.position = position;
						}
						goto IL_0067;
					}
				}
				this._emitParams.position = position;
			}
			IL_0067:
			if (this.ActivateOnPlay)
			{
				this.BoundParticleSystem.gameObject.SetActive(true);
				foreach (ParticleSystem particleSystem2 in this.RandomParticleSystems)
				{
					particleSystem2.gameObject.SetActive(true);
				}
			}
			if (this.RandomParticleSystems.Count > 0)
			{
				int num = Random.Range(0, this.RandomParticleSystems.Count);
				this.HandleParticleSystemAction(this.RandomParticleSystems[num]);
				return;
			}
			if (this.BoundParticleSystem != null)
			{
				this.HandleParticleSystemAction(this.BoundParticleSystem);
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00039AAC File Offset: 0x00037CAC
		protected virtual void HandleParticleSystemAction(ParticleSystem targetParticleSystem)
		{
			if (this.ForceSimulationSpeed)
			{
				targetParticleSystem.main.simulationSpeed = Random.Range(this.ForcedSimulationSpeed.x, this.ForcedSimulationSpeed.y);
			}
			switch (this.Mode)
			{
			case MMF_Particles.Modes.Play:
				if (targetParticleSystem != null)
				{
					targetParticleSystem.Play();
					return;
				}
				break;
			case MMF_Particles.Modes.Stop:
				if (targetParticleSystem != null)
				{
					targetParticleSystem.Stop();
					return;
				}
				break;
			case MMF_Particles.Modes.Pause:
				if (targetParticleSystem != null)
				{
					targetParticleSystem.Pause();
				}
				break;
			case MMF_Particles.Modes.Emit:
				this._emitParams.applyShapeToPosition = true;
				targetParticleSystem.Emit(this._emitParams, this.EmitCount);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00039B44 File Offset: 0x00037D44
		protected virtual void StopParticles()
		{
			foreach (ParticleSystem particleSystem in this.RandomParticleSystems)
			{
				if (particleSystem != null)
				{
					particleSystem.Stop();
				}
			}
			if (this.BoundParticleSystem != null)
			{
				this.BoundParticleSystem.Stop();
			}
		}

		// Token: 0x04000F26 RID: 3878
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F27 RID: 3879
		[MMFInspectorGroup("Bound Particles", true, 41, true, false)]
		[Tooltip("whether to Play, Stop or Pause the target particle system when that feedback is played")]
		public MMF_Particles.Modes Mode;

		// Token: 0x04000F28 RID: 3880
		[Tooltip("in Emit mode, the amount of particles per emit")]
		[MMFEnumCondition("Mode", new int[] { 3 })]
		public int EmitCount = 100;

		// Token: 0x04000F29 RID: 3881
		[Tooltip("the particle system to play with this feedback")]
		public ParticleSystem BoundParticleSystem;

		// Token: 0x04000F2A RID: 3882
		[Tooltip("a list of (optional) particle systems")]
		public List<ParticleSystem> RandomParticleSystems;

		// Token: 0x04000F2B RID: 3883
		[Tooltip("if this is true, the particles will be moved to the position passed in parameters")]
		public bool MoveToPosition;

		// Token: 0x04000F2C RID: 3884
		[Tooltip("if this is true, the particle system's object will be set active on play")]
		public bool ActivateOnPlay;

		// Token: 0x04000F2D RID: 3885
		[Tooltip("if this is true, the particle system will be stopped on initialization")]
		public bool StopSystemOnInit = true;

		// Token: 0x04000F2E RID: 3886
		[Tooltip("if this is true, the particle system will be stopped on reset")]
		public bool StopSystemOnReset = true;

		// Token: 0x04000F2F RID: 3887
		[Tooltip("if this is true, the particle system will be stopped on feedback stop")]
		public bool StopSystemOnStopFeedback = true;

		// Token: 0x04000F30 RID: 3888
		[Tooltip("the duration for the player to consider. This won't impact your particle system, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04000F31 RID: 3889
		[MMFInspectorGroup("Simulation Speed", true, 43, false, false)]
		[Tooltip("whether or not to force a specific simulation speed on the target particle system(s)")]
		public bool ForceSimulationSpeed;

		// Token: 0x04000F32 RID: 3890
		[Tooltip("The min and max values at which to randomize the simulation speed, if ForceSimulationSpeed is true. A new value will be randomized every time this feedback plays")]
		[MMFCondition("ForceSimulationSpeed", true)]
		public Vector2 ForcedSimulationSpeed = new Vector2(0.1f, 1f);

		// Token: 0x04000F33 RID: 3891
		protected ParticleSystem.EmitParams _emitParams;

		// Token: 0x020004CC RID: 1228
		public enum Modes
		{
			// Token: 0x04002255 RID: 8789
			Play,
			// Token: 0x04002256 RID: 8790
			Stop,
			// Token: 0x04002257 RID: 8791
			Pause,
			// Token: 0x04002258 RID: 8792
			Emit
		}
	}
}
