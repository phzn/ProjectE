using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000034 RID: 52
	public class DirectorControlPlayable : PlayableBehaviour
	{
		// Token: 0x06000279 RID: 633 RVA: 0x00008C64 File Offset: 0x00006E64
		public static ScriptPlayable<DirectorControlPlayable> Create(PlayableGraph graph, PlayableDirector director)
		{
			if (director == null)
			{
				return ScriptPlayable<DirectorControlPlayable>.Null;
			}
			ScriptPlayable<DirectorControlPlayable> scriptPlayable = ScriptPlayable<DirectorControlPlayable>.Create(graph, 0);
			scriptPlayable.GetBehaviour().director = director;
			return scriptPlayable;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00008C96 File Offset: 0x00006E96
		public override void OnPlayableDestroy(Playable playable)
		{
			if (this.director != null && this.director.playableAsset != null)
			{
				this.director.Stop();
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008CC4 File Offset: 0x00006EC4
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.director == null || !this.director.isActiveAndEnabled || this.director.playableAsset == null)
			{
				return;
			}
			this.m_SyncTime |= info.evaluationType == FrameData.EvaluationType.Evaluate || this.DetectDiscontinuity(playable, info);
			this.SyncSpeed((double)info.effectiveSpeed);
			this.SyncStart(playable.GetGraph<Playable>(), playable.GetTime<Playable>());
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00008D41 File Offset: 0x00006F41
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			this.m_SyncTime = true;
			if (this.director != null && this.director.playableAsset != null)
			{
				this.m_AssetDuration = this.director.playableAsset.duration;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008D84 File Offset: 0x00006F84
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.director != null && this.director.playableAsset != null)
			{
				if (info.effectivePlayState == PlayState.Playing)
				{
					this.director.Pause();
					return;
				}
				this.director.Stop();
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00008DD4 File Offset: 0x00006FD4
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (this.director == null || !this.director.isActiveAndEnabled || this.director.playableAsset == null)
			{
				return;
			}
			if (this.m_SyncTime || this.DetectOutOfSync(playable))
			{
				this.UpdateTime(playable);
				if (this.director.playableGraph.IsValid())
				{
					this.director.playableGraph.Evaluate();
					this.director.playableGraph.SynchronizeEvaluation(playable.GetGraph<Playable>());
				}
				else
				{
					this.director.Evaluate();
				}
			}
			this.m_SyncTime = false;
			this.SyncStop(playable.GetGraph<Playable>(), playable.GetTime<Playable>());
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008E90 File Offset: 0x00007090
		private void SyncSpeed(double speed)
		{
			if (this.director.playableGraph.IsValid())
			{
				int rootPlayableCount = this.director.playableGraph.GetRootPlayableCount();
				for (int i = 0; i < rootPlayableCount; i++)
				{
					Playable rootPlayable = this.director.playableGraph.GetRootPlayable(i);
					if (rootPlayable.IsValid<Playable>())
					{
						rootPlayable.SetSpeed(speed);
					}
				}
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00008EF8 File Offset: 0x000070F8
		private void SyncStart(PlayableGraph graph, double time)
		{
			if (this.director.state == PlayState.Playing || !graph.IsPlaying() || (this.director.extrapolationMode == DirectorWrapMode.None && time > this.m_AssetDuration))
			{
				return;
			}
			if (graph.IsMatchFrameRateEnabled())
			{
				this.director.Play(graph.GetFrameRate());
				return;
			}
			this.director.Play();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008F5C File Offset: 0x0000715C
		private void SyncStop(PlayableGraph graph, double time)
		{
			if (this.director.state == PlayState.Paused || (graph.IsPlaying() && (this.director.extrapolationMode != DirectorWrapMode.None || time < this.m_AssetDuration)))
			{
				return;
			}
			if (this.director.state == PlayState.Paused)
			{
				return;
			}
			if ((this.director.extrapolationMode == DirectorWrapMode.None && time > this.m_AssetDuration) || !graph.IsPlaying())
			{
				this.director.Pause();
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008FD5 File Offset: 0x000071D5
		private bool DetectDiscontinuity(Playable playable, FrameData info)
		{
			return Math.Abs(playable.GetTime<Playable>() - playable.GetPreviousTime<Playable>() - info.m_DeltaTime * (double)info.m_EffectiveSpeed) > DiscreteTime.tickValue;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00009000 File Offset: 0x00007200
		private bool DetectOutOfSync(Playable playable)
		{
			double num = playable.GetTime<Playable>();
			if (playable.GetTime<Playable>() >= this.m_AssetDuration)
			{
				switch (this.director.extrapolationMode)
				{
				case DirectorWrapMode.Hold:
					num = this.m_AssetDuration;
					break;
				case DirectorWrapMode.Loop:
					num %= this.m_AssetDuration;
					break;
				case DirectorWrapMode.None:
					num = this.m_AssetDuration;
					break;
				}
			}
			return !Mathf.Approximately((float)num, (float)this.director.time);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00009078 File Offset: 0x00007278
		private void UpdateTime(Playable playable)
		{
			double num = Math.Max(0.1, this.director.playableAsset.duration);
			switch (this.director.extrapolationMode)
			{
			case DirectorWrapMode.Hold:
				this.director.time = Math.Min(num, Math.Max(0.0, playable.GetTime<Playable>()));
				return;
			case DirectorWrapMode.Loop:
				this.director.time = Math.Max(0.0, playable.GetTime<Playable>() % num);
				return;
			case DirectorWrapMode.None:
				this.director.time = Math.Min(num, Math.Max(0.0, playable.GetTime<Playable>()));
				return;
			default:
				return;
			}
		}

		// Token: 0x040000D4 RID: 212
		public PlayableDirector director;

		// Token: 0x040000D5 RID: 213
		private bool m_SyncTime;

		// Token: 0x040000D6 RID: 214
		private double m_AssetDuration = double.MaxValue;
	}
}
