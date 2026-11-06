using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200002B RID: 43
	[AssetFileNameExtension("signal", new string[] { })]
	public class SignalAsset : ScriptableObject
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000244 RID: 580 RVA: 0x00008644 File Offset: 0x00006844
		// (remove) Token: 0x06000245 RID: 581 RVA: 0x00008678 File Offset: 0x00006878
		internal static event Action<SignalAsset> OnEnableCallback;

		// Token: 0x06000246 RID: 582 RVA: 0x000086AB File Offset: 0x000068AB
		private void OnEnable()
		{
			if (SignalAsset.OnEnableCallback != null)
			{
				SignalAsset.OnEnableCallback(this);
			}
		}
	}
}
