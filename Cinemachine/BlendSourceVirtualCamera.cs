using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000031 RID: 49
	internal class BlendSourceVirtualCamera : ICinemachineCamera
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00011708 File Offset: 0x0000F908
		public BlendSourceVirtualCamera(CinemachineBlend blend)
		{
			this.Blend = blend;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00011717 File Offset: 0x0000F917
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0001171F File Offset: 0x0000F91F
		public CinemachineBlend Blend { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00011728 File Offset: 0x0000F928
		public string Name
		{
			get
			{
				return "Mid-blend";
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0001172F File Offset: 0x0000F92F
		public string Description
		{
			get
			{
				if (this.Blend != null)
				{
					return this.Blend.Description;
				}
				return "(null)";
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0001174A File Offset: 0x0000F94A
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00011752 File Offset: 0x0000F952
		public int Priority { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0001175B File Offset: 0x0000F95B
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00011763 File Offset: 0x0000F963
		public Transform LookAt { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0001176C File Offset: 0x0000F96C
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00011774 File Offset: 0x0000F974
		public Transform Follow { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0001177D File Offset: 0x0000F97D
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00011785 File Offset: 0x0000F985
		public CameraState State { get; private set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0001178E File Offset: 0x0000F98E
		public GameObject VirtualCameraGameObject
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00011791 File Offset: 0x0000F991
		public bool IsValid
		{
			get
			{
				return this.Blend != null && this.Blend.IsValid;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000258 RID: 600 RVA: 0x000117A8 File Offset: 0x0000F9A8
		public ICinemachineCamera ParentCamera
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000117AB File Offset: 0x0000F9AB
		public bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return this.Blend != null && (vcam == this.Blend.CamA || vcam == this.Blend.CamB);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000117D5 File Offset: 0x0000F9D5
		public CameraState CalculateNewState(float deltaTime)
		{
			return this.State;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000117DD File Offset: 0x0000F9DD
		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
			if (this.Blend != null)
			{
				this.Blend.UpdateCameraState(worldUp, deltaTime);
				this.State = this.Blend.State;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00011805 File Offset: 0x0000FA05
		public void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00011807 File Offset: 0x0000FA07
		public void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00011809 File Offset: 0x0000FA09
		public void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
		}
	}
}
