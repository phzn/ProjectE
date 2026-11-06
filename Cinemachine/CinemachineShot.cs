using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000006 RID: 6
public sealed class CinemachineShot : PlayableAsset, IPropertyPreview
{
	// Token: 0x06000010 RID: 16 RVA: 0x00002578 File Offset: 0x00000778
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<CinemachineShotPlayable> scriptPlayable = ScriptPlayable<CinemachineShotPlayable>.Create(graph, 0);
		scriptPlayable.GetBehaviour().VirtualCamera = this.VirtualCamera.Resolve(graph.GetResolver());
		return scriptPlayable;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000025B4 File Offset: 0x000007B4
	public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		driver.AddFromName<Transform>("m_LocalPosition.x");
		driver.AddFromName<Transform>("m_LocalPosition.y");
		driver.AddFromName<Transform>("m_LocalPosition.z");
		driver.AddFromName<Transform>("m_LocalRotation.x");
		driver.AddFromName<Transform>("m_LocalRotation.y");
		driver.AddFromName<Transform>("m_LocalRotation.z");
		driver.AddFromName<Transform>("m_LocalRotation.w");
		driver.AddFromName<Camera>("field of view");
		driver.AddFromName<Camera>("near clip plane");
		driver.AddFromName<Camera>("far clip plane");
	}

	// Token: 0x04000013 RID: 19
	public string DisplayName;

	// Token: 0x04000014 RID: 20
	public ExposedReference<CinemachineVirtualCameraBase> VirtualCamera;
}
