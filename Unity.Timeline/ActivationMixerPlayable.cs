using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000003 RID: 3
	internal class ActivationMixerPlayable : PlayableBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static ScriptPlayable<ActivationMixerPlayable> Create(PlayableGraph graph, int inputCount)
		{
			return ScriptPlayable<ActivationMixerPlayable>.Create(graph, inputCount);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020C9 File Offset: 0x000002C9
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020D1 File Offset: 0x000002D1
		public ActivationTrack.PostPlaybackState postPlaybackState
		{
			get
			{
				return this.m_PostPlaybackState;
			}
			set
			{
				this.m_PostPlaybackState = value;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020DC File Offset: 0x000002DC
		public override void OnPlayableDestroy(Playable playable)
		{
			if (this.m_BoundGameObject == null)
			{
				return;
			}
			switch (this.m_PostPlaybackState)
			{
			case ActivationTrack.PostPlaybackState.Active:
				this.m_BoundGameObject.SetActive(true);
				return;
			case ActivationTrack.PostPlaybackState.Inactive:
				this.m_BoundGameObject.SetActive(false);
				return;
			case ActivationTrack.PostPlaybackState.Revert:
				this.m_BoundGameObject.SetActive(this.m_BoundGameObjectInitialStateIsActive);
				break;
			case ActivationTrack.PostPlaybackState.LeaveAsIs:
				break;
			default:
				return;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002144 File Offset: 0x00000344
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (this.m_BoundGameObject == null)
			{
				this.m_BoundGameObject = playerData as GameObject;
				this.m_BoundGameObjectInitialStateIsActive = this.m_BoundGameObject != null && this.m_BoundGameObject.activeSelf;
			}
			if (this.m_BoundGameObject == null)
			{
				return;
			}
			int inputCount = playable.GetInputCount<Playable>();
			bool flag = false;
			for (int i = 0; i < inputCount; i++)
			{
				if (playable.GetInputWeight(i) > 0f)
				{
					flag = true;
					break;
				}
			}
			this.m_BoundGameObject.SetActive(flag);
		}

		// Token: 0x04000001 RID: 1
		private ActivationTrack.PostPlaybackState m_PostPlaybackState;

		// Token: 0x04000002 RID: 2
		private bool m_BoundGameObjectInitialStateIsActive;

		// Token: 0x04000003 RID: 3
		private GameObject m_BoundGameObject;
	}
}
