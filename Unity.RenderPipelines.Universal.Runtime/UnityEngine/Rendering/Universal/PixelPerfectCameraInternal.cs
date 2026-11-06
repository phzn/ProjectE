using System;
using UnityEngine.Experimental.Rendering.Universal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	internal class PixelPerfectCameraInternal : ISerializationCallbackReceiver
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x00010350 File Offset: 0x0000E550
		internal PixelPerfectCameraInternal(IPixelPerfectCamera component)
		{
			this.m_Component = component;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00010383 File Offset: 0x0000E583
		public void OnBeforeSerialize()
		{
			this.m_SerializableComponent = this.m_Component as PixelPerfectCamera;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00010396 File Offset: 0x0000E596
		public void OnAfterDeserialize()
		{
			if (this.m_SerializableComponent != null)
			{
				this.m_Component = this.m_SerializableComponent;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000103B4 File Offset: 0x0000E5B4
		internal void CalculateCameraProperties(int screenWidth, int screenHeight)
		{
			int assetsPPU = this.m_Component.assetsPPU;
			int refResolutionX = this.m_Component.refResolutionX;
			int refResolutionY = this.m_Component.refResolutionY;
			bool upscaleRT = this.m_Component.upscaleRT;
			bool pixelSnapping = this.m_Component.pixelSnapping;
			bool cropFrameX = this.m_Component.cropFrameX;
			bool cropFrameY = this.m_Component.cropFrameY;
			bool stretchFill = this.m_Component.stretchFill;
			this.cropFrameXAndY = cropFrameY && cropFrameX;
			this.cropFrameXOrY = cropFrameY || cropFrameX;
			this.useStretchFill = this.cropFrameXAndY && stretchFill;
			this.requiresUpscaling = this.useStretchFill;
			int num = screenHeight / refResolutionY;
			int num2 = screenWidth / refResolutionX;
			this.zoom = Math.Max(1, Math.Min(num, num2));
			this.useOffscreenRT = false;
			this.offscreenRTWidth = 0;
			this.offscreenRTHeight = 0;
			if (this.cropFrameXOrY)
			{
				this.useOffscreenRT = true;
				if (!upscaleRT)
				{
					if (this.cropFrameXAndY)
					{
						this.offscreenRTWidth = this.zoom * refResolutionX;
						this.offscreenRTHeight = this.zoom * refResolutionY;
					}
					else if (cropFrameY)
					{
						this.offscreenRTWidth = screenWidth;
						this.offscreenRTHeight = this.zoom * refResolutionY;
					}
					else
					{
						this.offscreenRTWidth = this.zoom * refResolutionX;
						this.offscreenRTHeight = screenHeight;
					}
				}
				else if (this.cropFrameXAndY)
				{
					this.offscreenRTWidth = refResolutionX;
					this.offscreenRTHeight = refResolutionY;
				}
				else if (cropFrameY)
				{
					this.offscreenRTWidth = screenWidth / this.zoom / 2 * 2;
					this.offscreenRTHeight = refResolutionY;
				}
				else
				{
					this.offscreenRTWidth = refResolutionX;
					this.offscreenRTHeight = screenHeight / this.zoom / 2 * 2;
				}
			}
			else if (upscaleRT && this.zoom > 1)
			{
				this.useOffscreenRT = true;
				this.offscreenRTWidth = screenWidth / this.zoom / 2 * 2;
				this.offscreenRTHeight = screenHeight / this.zoom / 2 * 2;
			}
			if (this.useOffscreenRT)
			{
				this.pixelRect = new Rect(0f, 0f, (float)this.offscreenRTWidth, (float)this.offscreenRTHeight);
			}
			else
			{
				this.pixelRect = Rect.zero;
			}
			if (cropFrameY)
			{
				this.orthoSize = (float)refResolutionY * 0.5f / (float)assetsPPU;
			}
			else if (cropFrameX)
			{
				float num3 = ((this.pixelRect == Rect.zero) ? ((float)screenWidth / (float)screenHeight) : (this.pixelRect.width / this.pixelRect.height));
				this.orthoSize = (float)refResolutionX / num3 * 0.5f / (float)assetsPPU;
			}
			else if (upscaleRT && this.zoom > 1)
			{
				this.orthoSize = (float)this.offscreenRTHeight * 0.5f / (float)assetsPPU;
			}
			else
			{
				float num4 = ((this.pixelRect == Rect.zero) ? ((float)screenHeight) : this.pixelRect.height);
				this.orthoSize = num4 * 0.5f / (float)(this.zoom * assetsPPU);
			}
			if (upscaleRT || (!upscaleRT && pixelSnapping))
			{
				this.unitsPerPixel = 1f / (float)assetsPPU;
				return;
			}
			this.unitsPerPixel = 1f / (float)(this.zoom * assetsPPU);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000106B8 File Offset: 0x0000E8B8
		internal Rect CalculateFinalBlitPixelRect(int screenWidth, int screenHeight)
		{
			Rect rect = default(Rect);
			if (this.useStretchFill)
			{
				float num = (float)screenWidth / (float)screenHeight;
				float num2 = (float)this.m_Component.refResolutionX / (float)this.m_Component.refResolutionY;
				if (num > num2)
				{
					rect.height = (float)screenHeight;
					rect.width = (float)screenHeight * num2;
					rect.x = (float)((screenWidth - (int)rect.width) / 2);
					rect.y = 0f;
				}
				else
				{
					rect.width = (float)screenWidth;
					rect.height = (float)screenWidth / num2;
					rect.y = (float)((screenHeight - (int)rect.height) / 2);
					rect.x = 0f;
				}
				if (screenWidth % this.m_Component.refResolutionX == 0)
				{
					this.requiresUpscaling = num2 < num;
				}
				else if (screenHeight % this.m_Component.refResolutionY == 0)
				{
					this.requiresUpscaling = num2 > num;
				}
			}
			else
			{
				if (this.m_Component.upscaleRT)
				{
					rect.height = (float)(this.zoom * this.offscreenRTHeight);
					rect.width = (float)(this.zoom * this.offscreenRTWidth);
				}
				else
				{
					rect.height = (float)this.offscreenRTHeight;
					rect.width = (float)this.offscreenRTWidth;
				}
				rect.x = (float)((screenWidth - (int)rect.width) / 2);
				rect.y = (float)((screenHeight - (int)rect.height) / 2);
			}
			return rect;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00010820 File Offset: 0x0000EA20
		internal float CorrectCinemachineOrthoSize(float targetOrthoSize)
		{
			float num;
			if (this.m_Component.upscaleRT)
			{
				this.cinemachineVCamZoom = Math.Max(1, Mathf.RoundToInt(this.orthoSize / targetOrthoSize));
				num = this.orthoSize / (float)this.cinemachineVCamZoom;
			}
			else
			{
				this.cinemachineVCamZoom = Math.Max(1, Mathf.RoundToInt((float)this.zoom * this.orthoSize / targetOrthoSize));
				num = (float)this.zoom * this.orthoSize / (float)this.cinemachineVCamZoom;
			}
			if (!this.m_Component.upscaleRT && !this.m_Component.pixelSnapping)
			{
				this.unitsPerPixel = 1f / (float)(this.cinemachineVCamZoom * this.m_Component.assetsPPU);
			}
			return num;
		}

		// Token: 0x0400012A RID: 298
		[NonSerialized]
		private IPixelPerfectCamera m_Component;

		// Token: 0x0400012B RID: 299
		private PixelPerfectCamera m_SerializableComponent;

		// Token: 0x0400012C RID: 300
		internal float originalOrthoSize;

		// Token: 0x0400012D RID: 301
		internal bool hasPostProcessLayer;

		// Token: 0x0400012E RID: 302
		internal bool cropFrameXAndY;

		// Token: 0x0400012F RID: 303
		internal bool cropFrameXOrY;

		// Token: 0x04000130 RID: 304
		internal bool useStretchFill;

		// Token: 0x04000131 RID: 305
		internal int zoom = 1;

		// Token: 0x04000132 RID: 306
		internal bool useOffscreenRT;

		// Token: 0x04000133 RID: 307
		internal int offscreenRTWidth;

		// Token: 0x04000134 RID: 308
		internal int offscreenRTHeight;

		// Token: 0x04000135 RID: 309
		internal Rect pixelRect = Rect.zero;

		// Token: 0x04000136 RID: 310
		internal float orthoSize = 1f;

		// Token: 0x04000137 RID: 311
		internal float unitsPerPixel;

		// Token: 0x04000138 RID: 312
		internal int cinemachineVCamZoom = 1;

		// Token: 0x04000139 RID: 313
		internal bool requiresUpscaling;
	}
}
