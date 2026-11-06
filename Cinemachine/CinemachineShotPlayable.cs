using System;
using Cinemachine;
using UnityEngine.Playables;

// Token: 0x02000007 RID: 7
internal sealed class CinemachineShotPlayable : PlayableBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000013 RID: 19 RVA: 0x00002637 File Offset: 0x00000837
	public bool IsValid
	{
		get
		{
			return this.VirtualCamera != null;
		}
	}

	// Token: 0x04000015 RID: 21
	public CinemachineVirtualCameraBase VirtualCamera;
}
