using System;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x020010B0 RID: 4272
	[DefaultExecutionOrder(32000)]
	public sealed class PremiumCursorController : MonoBehaviour
	{
		// Token: 0x06008161 RID: 33121 RVA: 0x0034E270 File Offset: 0x0034C470
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			if (PremiumCursorController._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[Premium Cursor]");
			Object.DontDestroyOnLoad(gameObject);
			PremiumCursorController._instance = gameObject.AddComponent<PremiumCursorController>();
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x0034E29C File Offset: 0x0034C49C
		private void Awake()
		{
			if (PremiumCursorController._instance != null && PremiumCursorController._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PremiumCursorController._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			Cursor.visible = true;
			PremiumCursorController.ApplyCursor();
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x0034E2EB File Offset: 0x0034C4EB
		private void OnEnable()
		{
			Application.onBeforeRender += this.ApplyInputVisibility;
		}

		// Token: 0x06008164 RID: 33124 RVA: 0x0034E2FE File Offset: 0x0034C4FE
		private void OnDisable()
		{
			Application.onBeforeRender -= this.ApplyInputVisibility;
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x0034E311 File Offset: 0x0034C511
		private void LateUpdate()
		{
			this.ApplyInputVisibility();
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x0034E31C File Offset: 0x0034C51C
		private void ApplyInputVisibility()
		{
			if (Application.isPlaying && !Application.isFocused)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (isActive)
			{
				if (Cursor.visible)
				{
					Cursor.visible = false;
				}
			}
			else if (this._controllerWasActive)
			{
				bool flag = Cursor.lockState != CursorLockMode.Locked;
				if (Cursor.visible != flag)
				{
					Cursor.visible = flag;
				}
			}
			this._controllerWasActive = isActive;
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x0034E379 File Offset: 0x0034C579
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				PremiumCursorController.ApplyCursor();
			}
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x0034E384 File Offset: 0x0034C584
		private static void ApplyCursor()
		{
			Texture2D texture2D = Resources.Load<Texture2D>("UI/ProjectExhibitionistCursor");
			if (texture2D == null)
			{
				Debug.LogWarning("[PremiumCursor] Texture Resources/UI/ProjectExhibitionistCursor not found.");
				return;
			}
			Cursor.SetCursor(texture2D, new Vector2(3f, 3f), CursorMode.Auto);
		}

		// Token: 0x040084AB RID: 33963
		private const string CursorResourcePath = "UI/ProjectExhibitionistCursor";

		// Token: 0x040084AC RID: 33964
		private static PremiumCursorController _instance;

		// Token: 0x040084AD RID: 33965
		private bool _controllerWasActive;
	}
}
