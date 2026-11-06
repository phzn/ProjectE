using System;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000070 RID: 112
	public struct Eyes
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x000382F5 File Offset: 0x000364F5
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x000382FD File Offset: 0x000364FD
		public Vector3 leftEyePosition
		{
			get
			{
				return this.m_LeftEyePosition;
			}
			set
			{
				this.m_LeftEyePosition = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00038306 File Offset: 0x00036506
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0003830E File Offset: 0x0003650E
		public Quaternion leftEyeRotation
		{
			get
			{
				return this.m_LeftEyeRotation;
			}
			set
			{
				this.m_LeftEyeRotation = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00038317 File Offset: 0x00036517
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0003831F File Offset: 0x0003651F
		public Vector3 rightEyePosition
		{
			get
			{
				return this.m_RightEyePosition;
			}
			set
			{
				this.m_RightEyePosition = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00038328 File Offset: 0x00036528
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x00038330 File Offset: 0x00036530
		public Quaternion rightEyeRotation
		{
			get
			{
				return this.m_RightEyeRotation;
			}
			set
			{
				this.m_RightEyeRotation = value;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00038339 File Offset: 0x00036539
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x00038341 File Offset: 0x00036541
		public Vector3 fixationPoint
		{
			get
			{
				return this.m_FixationPoint;
			}
			set
			{
				this.m_FixationPoint = value;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0003834A File Offset: 0x0003654A
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x00038352 File Offset: 0x00036552
		public float leftEyeOpenAmount
		{
			get
			{
				return this.m_LeftEyeOpenAmount;
			}
			set
			{
				this.m_LeftEyeOpenAmount = value;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0003835B File Offset: 0x0003655B
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x00038363 File Offset: 0x00036563
		public float rightEyeOpenAmount
		{
			get
			{
				return this.m_RightEyeOpenAmount;
			}
			set
			{
				this.m_RightEyeOpenAmount = value;
			}
		}

		// Token: 0x0400037D RID: 893
		public Vector3 m_LeftEyePosition;

		// Token: 0x0400037E RID: 894
		public Quaternion m_LeftEyeRotation;

		// Token: 0x0400037F RID: 895
		public Vector3 m_RightEyePosition;

		// Token: 0x04000380 RID: 896
		public Quaternion m_RightEyeRotation;

		// Token: 0x04000381 RID: 897
		public Vector3 m_FixationPoint;

		// Token: 0x04000382 RID: 898
		public float m_LeftEyeOpenAmount;

		// Token: 0x04000383 RID: 899
		public float m_RightEyeOpenAmount;
	}
}
