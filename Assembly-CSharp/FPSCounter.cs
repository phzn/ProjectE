using System;
using Features.MainMenu;
using UnityEngine;

// Token: 0x02000015 RID: 21
public sealed class FPSCounter : MonoBehaviour
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600007A RID: 122 RVA: 0x000052D1 File Offset: 0x000034D1
	// (set) Token: 0x0600007B RID: 123 RVA: 0x000052D8 File Offset: 0x000034D8
	public static FPSCounter Instance { get; private set; }

	// Token: 0x0600007C RID: 124 RVA: 0x000052E0 File Offset: 0x000034E0
	private void Awake()
	{
		if (FPSCounter.Instance != null && FPSCounter.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		FPSCounter.Instance = this;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x0000530E File Offset: 0x0000350E
	private void OnDestroy()
	{
		if (FPSCounter.Instance == this)
		{
			FPSCounter.Instance = null;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00004F31 File Offset: 0x00003131
	public void SetVisible(bool visible)
	{
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004F31 File Offset: 0x00003131
	public void ResetMinMax()
	{
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005323 File Offset: 0x00003523
	public void ToggleVisibility()
	{
		GameSettingsService.SetShowFps(!GameSettingsService.ShowFps);
	}
}
