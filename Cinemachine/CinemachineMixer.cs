using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000005 RID: 5
internal sealed class CinemachineMixer : PlayableBehaviour
{
	// Token: 0x0600000B RID: 11 RVA: 0x00002371 File Offset: 0x00000571
	public override void OnPlayableDestroy(Playable playable)
	{
		if (this.m_BrainOverrideStack != null)
		{
			this.m_BrainOverrideStack.ReleaseCameraOverride(this.m_BrainOverrideId);
		}
		this.m_BrainOverrideId = -1;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002393 File Offset: 0x00000593
	public override void PrepareFrame(Playable playable, FrameData info)
	{
		this.m_PreviewPlay = false;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000239C File Offset: 0x0000059C
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		this.m_BrainOverrideStack = playerData as ICameraOverrideStack;
		if (this.m_BrainOverrideStack == null)
		{
			return;
		}
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		bool flag = false;
		float num4 = 1f;
		for (int i = 0; i < playable.GetInputCount<Playable>(); i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			ScriptPlayable<CinemachineShotPlayable> scriptPlayable = (ScriptPlayable<T>)playable.GetInput(i);
			CinemachineShotPlayable behaviour = scriptPlayable.GetBehaviour();
			if (behaviour != null && behaviour.IsValid && playable.GetPlayState<Playable>() == PlayState.Playing && inputWeight > 0f)
			{
				num2 = num3;
				num3 = i;
				num4 = inputWeight;
				if (++num == 2)
				{
					Playable input = playable.GetInput(num2);
					flag = scriptPlayable.GetTime<ScriptPlayable<CinemachineShotPlayable>>() >= input.GetTime<Playable>();
					if (scriptPlayable.GetTime<ScriptPlayable<CinemachineShotPlayable>>() == input.GetTime<Playable>())
					{
						flag = scriptPlayable.GetDuration<ScriptPlayable<CinemachineShotPlayable>>() < input.GetDuration<Playable>();
						break;
					}
					break;
				}
			}
		}
		if (num == 1 && num4 < 1f && playable.GetInput(num3).GetTime<Playable>() > playable.GetInput(num3).GetDuration<Playable>() / 2.0)
		{
			flag = true;
		}
		if (flag)
		{
			int num5 = num3;
			int num6 = num2;
			num2 = num5;
			num3 = num6;
			num4 = 1f - num4;
		}
		ICinemachineCamera cinemachineCamera = null;
		if (num2 >= 0)
		{
			cinemachineCamera = ((ScriptPlayable<T>)playable.GetInput(num2)).GetBehaviour().VirtualCamera;
		}
		ICinemachineCamera cinemachineCamera2 = null;
		if (num3 >= 0)
		{
			cinemachineCamera2 = ((ScriptPlayable<T>)playable.GetInput(num3)).GetBehaviour().VirtualCamera;
		}
		this.m_BrainOverrideId = this.m_BrainOverrideStack.SetCameraOverride(this.m_BrainOverrideId, cinemachineCamera, cinemachineCamera2, num4, this.GetDeltaTime(info.deltaTime));
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000253B File Offset: 0x0000073B
	private float GetDeltaTime(float deltaTime)
	{
		if (this.m_PreviewPlay || Application.isPlaying)
		{
			return deltaTime;
		}
		if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Playback && TargetPositionCache.HasCurrentTime)
		{
			return 0f;
		}
		return -1f;
	}

	// Token: 0x0400000F RID: 15
	public static CinemachineMixer.MasterDirectorDelegate GetMasterPlayableDirector;

	// Token: 0x04000010 RID: 16
	private ICameraOverrideStack m_BrainOverrideStack;

	// Token: 0x04000011 RID: 17
	private int m_BrainOverrideId = -1;

	// Token: 0x04000012 RID: 18
	private bool m_PreviewPlay;

	// Token: 0x0200006C RID: 108
	// (Invoke) Token: 0x06000408 RID: 1032
	public delegate PlayableDirector MasterDirectorDelegate();
}
