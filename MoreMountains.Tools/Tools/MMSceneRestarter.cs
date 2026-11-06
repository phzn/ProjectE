using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000094 RID: 148
	[AddComponentMenu("More Mountains/Tools/Utilities/MMSceneRestarter")]
	public class MMSceneRestarter : MonoBehaviour
	{
		// Token: 0x06000334 RID: 820 RVA: 0x0001032D File Offset: 0x0000E52D
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00010335 File Offset: 0x0000E535
		protected virtual void HandleInput()
		{
			if (Input.GetKeyDown(this.RestarterKeyCode))
			{
				this.RestartScene();
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001034C File Offset: 0x0000E54C
		public virtual void RestartScene()
		{
			MMDebug.DebugLogInfo("Scene restarted by MMSceneRestarter", "", 3, true);
			MMSceneRestarter.RestartModes restartMode = this.RestartMode;
			if (restartMode != MMSceneRestarter.RestartModes.ActiveScene)
			{
				if (restartMode == MMSceneRestarter.RestartModes.SpecificScene)
				{
					this._newSceneName = this.SceneName;
				}
			}
			else
			{
				this._newSceneName = SceneManager.GetActiveScene().name;
			}
			SceneManager.LoadScene(this._newSceneName, this.LoadMode);
		}

		// Token: 0x04000460 RID: 1120
		[Header("Settings")]
		public MMSceneRestarter.RestartModes RestartMode;

		// Token: 0x04000461 RID: 1121
		[MMEnumCondition("RestartMode", new int[] { 1 })]
		public string SceneName;

		// Token: 0x04000462 RID: 1122
		public LoadSceneMode LoadMode;

		// Token: 0x04000463 RID: 1123
		[Header("Input")]
		public KeyCode RestarterKeyCode = KeyCode.Backspace;

		// Token: 0x04000464 RID: 1124
		protected string _newSceneName;

		// Token: 0x020003F0 RID: 1008
		public enum RestartModes
		{
			// Token: 0x04001E90 RID: 7824
			ActiveScene,
			// Token: 0x04001E91 RID: 7825
			SpecificScene
		}
	}
}
