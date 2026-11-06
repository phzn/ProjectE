using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000039 RID: 57
	public class TimeControlPlayable : PlayableBehaviour
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000952C File Offset: 0x0000772C
		public static ScriptPlayable<TimeControlPlayable> Create(PlayableGraph graph, ITimeControl timeControl)
		{
			if (timeControl == null)
			{
				return ScriptPlayable<TimeControlPlayable>.Null;
			}
			ScriptPlayable<TimeControlPlayable> scriptPlayable = ScriptPlayable<TimeControlPlayable>.Create(graph, 0);
			scriptPlayable.GetBehaviour().Initialize(timeControl);
			return scriptPlayable;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00009558 File Offset: 0x00007758
		public void Initialize(ITimeControl timeControl)
		{
			this.m_timeControl = timeControl;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00009561 File Offset: 0x00007761
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.m_timeControl != null)
			{
				this.m_timeControl.SetTime(playable.GetTime<Playable>());
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000957C File Offset: 0x0000777C
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (this.m_timeControl == null)
			{
				return;
			}
			if (!this.m_started)
			{
				this.m_timeControl.OnControlTimeStart();
				this.m_started = true;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000095A1 File Offset: 0x000077A1
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.m_timeControl == null)
			{
				return;
			}
			if (this.m_started)
			{
				this.m_timeControl.OnControlTimeStop();
				this.m_started = false;
			}
		}

		// Token: 0x040000E1 RID: 225
		private ITimeControl m_timeControl;

		// Token: 0x040000E2 RID: 226
		private bool m_started;
	}
}
