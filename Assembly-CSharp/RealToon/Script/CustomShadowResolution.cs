using System;
using UnityEngine;

namespace RealToon.Script
{
	// Token: 0x02000438 RID: 1080
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[AddComponentMenu("RealToon/Tools/Custom Shadow Resolution")]
	public class CustomShadowResolution : MonoBehaviour
	{
		// Token: 0x06001B30 RID: 6960 RVA: 0x000A9FE8 File Offset: 0x000A81E8
		private void Update()
		{
			base.GetComponent<Light>().shadowCustomResolution = this.FinalResolution;
			this.FinalResolution = this.Value * 2;
			if (this.Reset)
			{
				this.Value = 2048;
				this.FinalResolution = 4096;
				this.Reset = false;
			}
			if (this.Value < 0)
			{
				this.Value = 0;
				this.FinalResolution = 0;
			}
		}

		// Token: 0x04001C2A RID: 7210
		[Header("Custom Shadow Resolution V1.0.0")]
		[Header("Note: Higher Shadow Resolution = More GPU RAM Usage.")]
		[Header("For RealToon Built-In RP Only")]
		[Space(10f)]
		[SerializeField]
		[Tooltip("Input value")]
		public int Value = 2048;

		// Token: 0x04001C2B RID: 7211
		[SerializeField]
		[Tooltip("Final Resolution (Value * 2)")]
		public int FinalResolution = 4096;

		// Token: 0x04001C2C RID: 7212
		[Space(10f)]
		[SerializeField]
		[Tooltip("Reset to default value")]
		public bool Reset;
	}
}
