using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000032 RID: 50
	public class ActivationControlPlayable : PlayableBehaviour
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00008AEC File Offset: 0x00006CEC
		public static ScriptPlayable<ActivationControlPlayable> Create(PlayableGraph graph, GameObject gameObject, ActivationControlPlayable.PostPlaybackState postPlaybackState)
		{
			if (gameObject == null)
			{
				return ScriptPlayable<ActivationControlPlayable>.Null;
			}
			ScriptPlayable<ActivationControlPlayable> scriptPlayable = ScriptPlayable<ActivationControlPlayable>.Create(graph, 0);
			ActivationControlPlayable behaviour = scriptPlayable.GetBehaviour();
			behaviour.gameObject = gameObject;
			behaviour.postPlayback = postPlaybackState;
			return scriptPlayable;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00008B25 File Offset: 0x00006D25
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (this.gameObject == null)
			{
				return;
			}
			this.gameObject.SetActive(true);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00008B42 File Offset: 0x00006D42
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.gameObject != null && info.effectivePlayState == PlayState.Paused)
			{
				this.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00008B67 File Offset: 0x00006D67
		public override void ProcessFrame(Playable playable, FrameData info, object userData)
		{
			if (this.gameObject != null)
			{
				this.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00008B83 File Offset: 0x00006D83
		public override void OnGraphStart(Playable playable)
		{
			if (this.gameObject != null && this.m_InitialState == ActivationControlPlayable.InitialState.Unset)
			{
				this.m_InitialState = (this.gameObject.activeSelf ? ActivationControlPlayable.InitialState.Active : ActivationControlPlayable.InitialState.Inactive);
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008BB4 File Offset: 0x00006DB4
		public override void OnPlayableDestroy(Playable playable)
		{
			if (this.gameObject == null || this.m_InitialState == ActivationControlPlayable.InitialState.Unset)
			{
				return;
			}
			switch (this.postPlayback)
			{
			case ActivationControlPlayable.PostPlaybackState.Active:
				this.gameObject.SetActive(true);
				return;
			case ActivationControlPlayable.PostPlaybackState.Inactive:
				this.gameObject.SetActive(false);
				return;
			case ActivationControlPlayable.PostPlaybackState.Revert:
				this.gameObject.SetActive(this.m_InitialState == ActivationControlPlayable.InitialState.Active);
				return;
			default:
				return;
			}
		}

		// Token: 0x040000D1 RID: 209
		public GameObject gameObject;

		// Token: 0x040000D2 RID: 210
		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		// Token: 0x040000D3 RID: 211
		private ActivationControlPlayable.InitialState m_InitialState;

		// Token: 0x02000074 RID: 116
		public enum PostPlaybackState
		{
			// Token: 0x04000170 RID: 368
			Active,
			// Token: 0x04000171 RID: 369
			Inactive,
			// Token: 0x04000172 RID: 370
			Revert
		}

		// Token: 0x02000075 RID: 117
		private enum InitialState
		{
			// Token: 0x04000174 RID: 372
			Unset,
			// Token: 0x04000175 RID: 373
			Active,
			// Token: 0x04000176 RID: 374
			Inactive
		}
	}
}
