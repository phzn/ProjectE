using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A28 RID: 2600
	public sealed class RunSoakRenderGuard : IDisposable
	{
		// Token: 0x060049CE RID: 18894 RVA: 0x001C6885 File Offset: 0x001C4A85
		public static string CaptureFailure(bool required, bool cameraIdentityChanged, int framesWithoutObserver, int consecutiveMissingReceipts)
		{
			if (!required)
			{
				return null;
			}
			if (cameraIdentityChanged)
			{
				return "The captured Camera.main identity changed during measurement.";
			}
			if (framesWithoutObserver >= 5 || consecutiveMissingReceipts >= 5)
			{
				return "Actual Camera.main did not render for five completed game frames.";
			}
			return null;
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060049CF RID: 18895 RVA: 0x001C68A4 File Offset: 0x001C4AA4
		public RunSoakRenderGuard.FrameLedger Frames { get; } = new RunSoakRenderGuard.FrameLedger();

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x001C68AC File Offset: 0x001C4AAC
		public string CameraName { get; }

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060049D1 RID: 18897 RVA: 0x001C68B4 File Offset: 0x001C4AB4
		public int CameraInstanceId { get; }

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x001C68BC File Offset: 0x001C4ABC
		// (set) Token: 0x060049D3 RID: 18899 RVA: 0x001C68C4 File Offset: 0x001C4AC4
		public bool CameraIdentityChanged { get; private set; }

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x001C68CD File Offset: 0x001C4ACD
		public bool AllCheckedFramesRendered
		{
			get
			{
				return !this._disposed && this.IdentityMatches() && this.Frames.CheckedFrames > 0 && this.Frames.MissingFrames == 0;
			}
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x001C6900 File Offset: 0x001C4B00
		public RunSoakRenderGuard(Camera mainCamera)
		{
			if (mainCamera == null || mainCamera != Camera.main || mainCamera.cameraType != CameraType.Game)
			{
				throw new ArgumentException("Requires the actual Camera.main Game camera.", "mainCamera");
			}
			this._mainCamera = mainCamera;
			this.CameraName = mainCamera.name;
			this.CameraInstanceId = mainCamera.GetInstanceID();
			RenderPipelineManager.endCameraRendering += this.OnSrpCameraRendered;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnBuiltinCameraRendered));
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x001C699D File Offset: 0x001C4B9D
		private void OnSrpCameraRendered(ScriptableRenderContext context, Camera camera)
		{
			this.Observe(camera);
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x001C69A6 File Offset: 0x001C4BA6
		private void OnBuiltinCameraRendered(Camera camera)
		{
			this.Observe(camera);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x001C69B0 File Offset: 0x001C4BB0
		private void Observe(Camera camera)
		{
			if (this._disposed || camera != this._mainCamera)
			{
				return;
			}
			if (!this.IdentityMatches() || !camera.isActiveAndEnabled || camera.pixelWidth <= 0 || camera.pixelHeight <= 0)
			{
				return;
			}
			this.Frames.Rendered(Time.frameCount);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x001C6A07 File Offset: 0x001C4C07
		public bool IsReady(int minimumUniqueFrames, int currentFrame)
		{
			return !this._disposed && this.IdentityMatches() && this.Frames.HasConsecutiveReceiptsEndingAt(currentFrame - 1, minimumUniqueFrames);
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x001C6A2A File Offset: 0x001C4C2A
		private bool IdentityMatches()
		{
			if (this._mainCamera == null || this._mainCamera != Camera.main || this._mainCamera.cameraType != CameraType.Game)
			{
				this.CameraIdentityChanged = true;
			}
			return !this.CameraIdentityChanged;
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x001C6A6C File Offset: 0x001C4C6C
		public bool CheckPreviousFrame(int currentFrame)
		{
			if (this._disposed)
			{
				return false;
			}
			bool flag = this.IdentityMatches();
			return this.Frames.CheckPreviousFrame(currentFrame) && flag;
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x001C6A98 File Offset: 0x001C4C98
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			RenderPipelineManager.endCameraRendering -= this.OnSrpCameraRendered;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnBuiltinCameraRendered));
		}

		// Token: 0x04004B74 RID: 19316
		private readonly Camera _mainCamera;

		// Token: 0x04004B75 RID: 19317
		private bool _disposed;

		// Token: 0x02000A29 RID: 2601
		public sealed class FrameLedger
		{
			// Token: 0x17000C82 RID: 3202
			// (get) Token: 0x060049DD RID: 18909 RVA: 0x001C6AE6 File Offset: 0x001C4CE6
			// (set) Token: 0x060049DE RID: 18910 RVA: 0x001C6AEE File Offset: 0x001C4CEE
			public int UniqueRenderedFrames { get; private set; }

			// Token: 0x17000C83 RID: 3203
			// (get) Token: 0x060049DF RID: 18911 RVA: 0x001C6AF7 File Offset: 0x001C4CF7
			// (set) Token: 0x060049E0 RID: 18912 RVA: 0x001C6AFF File Offset: 0x001C4CFF
			public int CheckedFrames { get; private set; }

			// Token: 0x17000C84 RID: 3204
			// (get) Token: 0x060049E1 RID: 18913 RVA: 0x001C6B08 File Offset: 0x001C4D08
			// (set) Token: 0x060049E2 RID: 18914 RVA: 0x001C6B10 File Offset: 0x001C4D10
			public int MissingFrames { get; private set; }

			// Token: 0x17000C85 RID: 3205
			// (get) Token: 0x060049E3 RID: 18915 RVA: 0x001C6B19 File Offset: 0x001C4D19
			// (set) Token: 0x060049E4 RID: 18916 RVA: 0x001C6B21 File Offset: 0x001C4D21
			public int ConsecutiveMissingFrames { get; private set; }

			// Token: 0x060049E5 RID: 18917 RVA: 0x001C6B2C File Offset: 0x001C4D2C
			public void Rendered(int frame)
			{
				if (frame < 0)
				{
					return;
				}
				int num = frame & 7;
				if (this._receipts[num] == frame)
				{
					return;
				}
				this._receipts[num] = frame;
				int uniqueRenderedFrames = this.UniqueRenderedFrames;
				this.UniqueRenderedFrames = uniqueRenderedFrames + 1;
			}

			// Token: 0x060049E6 RID: 18918 RVA: 0x001C6B67 File Offset: 0x001C4D67
			public bool HasReceipt(int frame)
			{
				return frame >= 0 && this._receipts[frame & 7] == frame;
			}

			// Token: 0x060049E7 RID: 18919 RVA: 0x001C6B7C File Offset: 0x001C4D7C
			public bool HasConsecutiveReceiptsEndingAt(int lastFrame, int count)
			{
				if (count < 1 || count > this._receipts.Length)
				{
					return false;
				}
				for (int i = 0; i < count; i++)
				{
					if (!this.HasReceipt(lastFrame - i))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060049E8 RID: 18920 RVA: 0x001C6BB4 File Offset: 0x001C4DB4
			public bool CheckPreviousFrame(int currentFrame)
			{
				bool flag = this.HasReceipt(currentFrame - 1);
				if (currentFrame <= this._lastChecked)
				{
					return flag;
				}
				this._lastChecked = currentFrame;
				int num = this.CheckedFrames;
				this.CheckedFrames = num + 1;
				if (flag)
				{
					this.ConsecutiveMissingFrames = 0;
				}
				else
				{
					num = this.MissingFrames;
					this.MissingFrames = num + 1;
					num = this.ConsecutiveMissingFrames;
					this.ConsecutiveMissingFrames = num + 1;
				}
				return flag;
			}

			// Token: 0x04004B7A RID: 19322
			private readonly int[] _receipts = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };

			// Token: 0x04004B7B RID: 19323
			private int _lastChecked = -1;
		}
	}
}
