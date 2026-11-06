using System;

namespace UnityEngine
{
	// Token: 0x0200001D RID: 29
	public struct JointMotor2D
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00006D3C File Offset: 0x00004F3C
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00006D54 File Offset: 0x00004F54
		public float motorSpeed
		{
			get
			{
				return this.m_MotorSpeed;
			}
			set
			{
				this.m_MotorSpeed = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00006D60 File Offset: 0x00004F60
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00006D78 File Offset: 0x00004F78
		public float maxMotorTorque
		{
			get
			{
				return this.m_MaximumMotorTorque;
			}
			set
			{
				this.m_MaximumMotorTorque = value;
			}
		}

		// Token: 0x04000083 RID: 131
		private float m_MotorSpeed;

		// Token: 0x04000084 RID: 132
		private float m_MaximumMotorTorque;
	}
}
