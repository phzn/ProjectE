using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000A RID: 10
	[AddComponentMenu("")]
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	public class CinemachineUniversalPixelPerfect : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002EC4 File Offset: 0x000010C4
		private void OnEnable()
		{
			Debug.LogError("CinemachineUniversalPixelPerfect is now deprecated and doesn't function properly. Instead, use the one from Cinemachine v2.4.0 or newer.");
		}
	}
}
