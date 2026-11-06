using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000030 RID: 48
	internal class StaticPointVirtualCamera : ICinemachineCamera
	{
		// Token: 0x06000234 RID: 564 RVA: 0x00011679 File Offset: 0x0000F879
		public StaticPointVirtualCamera(CameraState state, string name)
		{
			this.State = state;
			this.Name = name;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0001168F File Offset: 0x0000F88F
		public void SetState(CameraState state)
		{
			this.State = state;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00011698 File Offset: 0x0000F898
		// (set) Token: 0x06000237 RID: 567 RVA: 0x000116A0 File Offset: 0x0000F8A0
		public string Name { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000116A9 File Offset: 0x0000F8A9
		public string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000239 RID: 569 RVA: 0x000116B0 File Offset: 0x0000F8B0
		// (set) Token: 0x0600023A RID: 570 RVA: 0x000116B8 File Offset: 0x0000F8B8
		public int Priority { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600023B RID: 571 RVA: 0x000116C1 File Offset: 0x0000F8C1
		// (set) Token: 0x0600023C RID: 572 RVA: 0x000116C9 File Offset: 0x0000F8C9
		public Transform LookAt { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600023D RID: 573 RVA: 0x000116D2 File Offset: 0x0000F8D2
		// (set) Token: 0x0600023E RID: 574 RVA: 0x000116DA File Offset: 0x0000F8DA
		public Transform Follow { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000116E3 File Offset: 0x0000F8E3
		// (set) Token: 0x06000240 RID: 576 RVA: 0x000116EB File Offset: 0x0000F8EB
		public CameraState State { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000116F4 File Offset: 0x0000F8F4
		public GameObject VirtualCameraGameObject
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000116F7 File Offset: 0x0000F8F7
		public bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000243 RID: 579 RVA: 0x000116FA File Offset: 0x0000F8FA
		public ICinemachineCamera ParentCamera
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000116FD File Offset: 0x0000F8FD
		public bool IsLiveChild(ICinemachineCamera vcam, bool dominantChildOnly = false)
		{
			return false;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011700 File Offset: 0x0000F900
		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00011702 File Offset: 0x0000F902
		public void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00011704 File Offset: 0x0000F904
		public void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00011706 File Offset: 0x0000F906
		public void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
		}
	}
}
