using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000037 RID: 55
	public class ParticleControlPlayable : PlayableBehaviour
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00009148 File Offset: 0x00007348
		public static ScriptPlayable<ParticleControlPlayable> Create(PlayableGraph graph, ParticleSystem component, uint randomSeed)
		{
			if (component == null)
			{
				return ScriptPlayable<ParticleControlPlayable>.Null;
			}
			ScriptPlayable<ParticleControlPlayable> scriptPlayable = ScriptPlayable<ParticleControlPlayable>.Create(graph, 0);
			scriptPlayable.GetBehaviour().Initialize(component, randomSeed);
			return scriptPlayable;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000917B File Offset: 0x0000737B
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00009183 File Offset: 0x00007383
		public ParticleSystem particleSystem { get; private set; }

		// Token: 0x0600028C RID: 652 RVA: 0x0000918C File Offset: 0x0000738C
		public void Initialize(ParticleSystem ps, uint randomSeed)
		{
			this.m_RandomSeed = Math.Max(1U, randomSeed);
			this.particleSystem = ps;
			ParticleControlPlayable.SetRandomSeed(this.particleSystem, this.m_RandomSeed);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000091B4 File Offset: 0x000073B4
		private static void SetRandomSeed(ParticleSystem particleSystem, uint randomSeed)
		{
			if (particleSystem == null)
			{
				return;
			}
			particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (particleSystem.useAutoRandomSeed)
			{
				particleSystem.useAutoRandomSeed = false;
				particleSystem.randomSeed = randomSeed;
			}
			for (int i = 0; i < particleSystem.subEmitters.subEmittersCount; i++)
			{
				ParticleControlPlayable.SetRandomSeed(particleSystem.subEmitters.GetSubEmitterSystem(i), randomSeed += 1U);
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000921C File Offset: 0x0000741C
		public override void PrepareFrame(Playable playable, FrameData data)
		{
			if (this.particleSystem == null || !this.particleSystem.gameObject.activeInHierarchy)
			{
				this.m_LastPlayableTime = float.MaxValue;
				return;
			}
			float num = (float)playable.GetTime<Playable>();
			float time = this.particleSystem.time;
			if (this.m_LastPlayableTime > num || !Mathf.Approximately(time, this.m_LastParticleTime))
			{
				this.Simulate(num, true);
			}
			else if (this.m_LastPlayableTime < num)
			{
				this.Simulate(num - this.m_LastPlayableTime, false);
			}
			this.m_LastPlayableTime = num;
			this.m_LastParticleTime = this.particleSystem.time;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000092BA File Offset: 0x000074BA
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			this.m_LastPlayableTime = float.MaxValue;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000092C7 File Offset: 0x000074C7
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			this.m_LastPlayableTime = float.MaxValue;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000092D4 File Offset: 0x000074D4
		private void Simulate(float time, bool restart)
		{
			float maximumDeltaTime = Time.maximumDeltaTime;
			if (restart)
			{
				this.particleSystem.Simulate(0f, false, true, false);
			}
			while (time > maximumDeltaTime)
			{
				this.particleSystem.Simulate(maximumDeltaTime, false, false, false);
				time -= maximumDeltaTime;
			}
			if (time > 0f)
			{
				this.particleSystem.Simulate(time, false, false, false);
			}
		}

		// Token: 0x040000DB RID: 219
		private const float kUnsetTime = 3.4028235E+38f;

		// Token: 0x040000DC RID: 220
		private float m_LastPlayableTime = float.MaxValue;

		// Token: 0x040000DD RID: 221
		private float m_LastParticleTime = float.MaxValue;

		// Token: 0x040000DE RID: 222
		private uint m_RandomSeed = 1U;
	}
}
