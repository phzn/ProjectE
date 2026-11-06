using System;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200002E RID: 46
	public class CinemachineBlend
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00011130 File Offset: 0x0000F330
		public float BlendWeight
		{
			get
			{
				if (this.BlendCurve == null || this.BlendCurve.length < 2 || this.IsComplete)
				{
					return 1f;
				}
				return Mathf.Clamp01(this.BlendCurve.Evaluate(this.TimeInBlend / this.Duration));
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0001117E File Offset: 0x0000F37E
		public bool IsValid
		{
			get
			{
				return (this.CamA != null && this.CamA.IsValid) || (this.CamB != null && this.CamB.IsValid);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000111AC File Offset: 0x0000F3AC
		public bool IsComplete
		{
			get
			{
				return this.TimeInBlend >= this.Duration || !this.IsValid;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000111C8 File Offset: 0x0000F3C8
		public string Description
		{
			get
			{
				StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
				if (this.CamB == null || !this.CamB.IsValid)
				{
					stringBuilder.Append("(none)");
				}
				else
				{
					stringBuilder.Append("[");
					stringBuilder.Append(this.CamB.Name);
					stringBuilder.Append("]");
				}
				stringBuilder.Append(" ");
				stringBuilder.Append((int)(this.BlendWeight * 100f));
				stringBuilder.Append("% from ");
				if (this.CamA == null || !this.CamA.IsValid)
				{
					stringBuilder.Append("(none)");
				}
				else
				{
					stringBuilder.Append("[");
					stringBuilder.Append(this.CamA.Name);
					stringBuilder.Append("]");
				}
				string text = stringBuilder.ToString();
				CinemachineDebug.ReturnToPool(stringBuilder);
				return text;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000112B0 File Offset: 0x0000F4B0
		public bool Uses(ICinemachineCamera cam)
		{
			if (cam == this.CamA || cam == this.CamB)
			{
				return true;
			}
			BlendSourceVirtualCamera blendSourceVirtualCamera = this.CamA as BlendSourceVirtualCamera;
			if (blendSourceVirtualCamera != null && blendSourceVirtualCamera.Blend.Uses(cam))
			{
				return true;
			}
			blendSourceVirtualCamera = this.CamB as BlendSourceVirtualCamera;
			return blendSourceVirtualCamera != null && blendSourceVirtualCamera.Blend.Uses(cam);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00011310 File Offset: 0x0000F510
		public CinemachineBlend(ICinemachineCamera a, ICinemachineCamera b, AnimationCurve curve, float duration, float t)
		{
			this.CamA = a;
			this.CamB = b;
			this.BlendCurve = curve;
			this.TimeInBlend = t;
			this.Duration = duration;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00011340 File Offset: 0x0000F540
		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			if (this.CamA != null && this.CamA.IsValid)
			{
				this.CamA.UpdateCameraState(worldUp, deltaTime);
			}
			if (this.CamB != null && this.CamB.IsValid)
			{
				this.CamB.UpdateCameraState(worldUp, deltaTime);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00011394 File Offset: 0x0000F594
		public CameraState State
		{
			get
			{
				if (this.CamA == null || !this.CamA.IsValid)
				{
					if (this.CamB == null || !this.CamB.IsValid)
					{
						return CameraState.Default;
					}
					return this.CamB.State;
				}
				else
				{
					if (this.CamB == null || !this.CamB.IsValid)
					{
						return this.CamA.State;
					}
					return CameraState.Lerp(this.CamA.State, this.CamB.State, this.BlendWeight);
				}
			}
		}

		// Token: 0x040001B2 RID: 434
		public ICinemachineCamera CamA;

		// Token: 0x040001B3 RID: 435
		public ICinemachineCamera CamB;

		// Token: 0x040001B4 RID: 436
		public AnimationCurve BlendCurve;

		// Token: 0x040001B5 RID: 437
		public float TimeInBlend;

		// Token: 0x040001B6 RID: 438
		public float Duration;
	}
}
