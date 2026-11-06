using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AFD RID: 2813
	public class CustomizationReturnNavigator : MonoBehaviour
	{
		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06004FA9 RID: 20393 RVA: 0x001EDE50 File Offset: 0x001EC050
		// (remove) Token: 0x06004FAA RID: 20394 RVA: 0x001EDE88 File Offset: 0x001EC088
		public event Action<string> ReturnRequested;

		// Token: 0x06004FAB RID: 20395 RVA: 0x001EDEC0 File Offset: 0x001EC0C0
		public void ReturnToSourceScene()
		{
			string sourceScene = this.fallbackSceneName;
			if (this.coreOrchestrator != null && this.coreOrchestrator.CurrentSession != null && !string.IsNullOrWhiteSpace(this.coreOrchestrator.CurrentSession.sourceScene))
			{
				sourceScene = this.coreOrchestrator.CurrentSession.sourceScene;
			}
			if (string.IsNullOrWhiteSpace(sourceScene) && this.ReturnRequested == null)
			{
				Debug.LogError("[CustomizationReturnNavigator] No valid scene to return to.");
				return;
			}
			if (this.coreOrchestrator != null)
			{
				this.coreOrchestrator.CloseSession();
			}
			if (this.ReturnRequested != null)
			{
				this.ReturnRequested(sourceScene);
				return;
			}
			SceneManager.LoadScene(sourceScene);
		}

		// Token: 0x0400509E RID: 20638
		[Header("References")]
		[SerializeField]
		private CustomizationCoreOrchestrator coreOrchestrator;

		// Token: 0x0400509F RID: 20639
		[Header("Fallback")]
		[SerializeField]
		private string fallbackSceneName = "CielHome";
	}
}
