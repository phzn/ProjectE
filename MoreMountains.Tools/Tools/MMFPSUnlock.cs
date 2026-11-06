using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace MoreMountains.Tools
{
	// Token: 0x02000076 RID: 118
	[AddComponentMenu("More Mountains/Tools/Performance/MMFPSUnlock")]
	public class MMFPSUnlock : MonoBehaviour
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000DABF File Offset: 0x0000BCBF
		protected virtual void Start()
		{
			this.UpdateSettings();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000DACF File Offset: 0x0000BCCF
		protected virtual void OnValidate()
		{
			this.UpdateSettings();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000DAD7 File Offset: 0x0000BCD7
		protected virtual void UpdateSettings()
		{
			QualitySettings.vSyncCount = this.VSyncCount;
			Application.targetFrameRate = this.TargetFPS;
			OnDemandRendering.renderFrameInterval = this.RenderFrameInterval;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		protected virtual void HandleInput()
		{
			if (!this.EnableNumberShortcuts)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Keypad0))
			{
				this.TargetFPS = 300;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				this.TargetFPS = 10;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				this.TargetFPS = 20;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				this.TargetFPS = 30;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				this.TargetFPS = 40;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad5))
			{
				this.TargetFPS = 50;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				this.TargetFPS = 60;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad7))
			{
				this.TargetFPS = 70;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad8))
			{
				this.TargetFPS = 80;
				this.UpdateSettings();
			}
			if (Input.GetKeyDown(KeyCode.Keypad9))
			{
				this.TargetFPS = 90;
				this.UpdateSettings();
			}
		}

		// Token: 0x040003E3 RID: 995
		[Tooltip("the target FPS you want the game to run at, that's up to how many times Update will run every second")]
		public int TargetFPS = 300;

		// Token: 0x040003E4 RID: 996
		[Tooltip("the number of frames to wait before rendering the next one. 0 will render every frame, 1 will render every 2 frames, 5 will render every 5 frames, etc")]
		public int RenderFrameInterval;

		// Token: 0x040003E5 RID: 997
		[Range(0f, 2f)]
		[Tooltip("whether vsync should be enabled or not (on a 60Hz screen, 1 : 60fps, 2 : 30fps, 0 : don't wait for vsync)")]
		public int VSyncCount;

		// Token: 0x040003E6 RID: 998
		[Tooltip("if this is true, the user can press a number key to change the target FPS (1 : 10fps, 2 : 20fps, etc)")]
		public bool EnableNumberShortcuts;
	}
}
