using System;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x0200138C RID: 5004
	[DisallowMultipleComponent]
	public sealed class CielOSMonitorSurface : MonoBehaviour
	{
		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x06009AE7 RID: 39655 RVA: 0x003F613F File Offset: 0x003F433F
		public Renderer TargetRenderer
		{
			get
			{
				if (!(this.targetRenderer != null))
				{
					return base.GetComponent<Renderer>();
				}
				return this.targetRenderer;
			}
		}

		// Token: 0x06009AE8 RID: 39656 RVA: 0x003F615C File Offset: 0x003F435C
		public int GetDisplayIndex(int materialIndex)
		{
			if (this.displayIndicesByMaterial == null || materialIndex < 0 || materialIndex >= this.displayIndicesByMaterial.Length)
			{
				return -1;
			}
			int num = this.displayIndicesByMaterial[materialIndex];
			if (num < 0 || num >= 3)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x04009FDF RID: 40927
		[SerializeField]
		private Renderer targetRenderer;

		// Token: 0x04009FE0 RID: 40928
		[Tooltip("Saved screen index for each material slot (0, 1 or 2); -1 leaves a non-screen slot alone.")]
		[SerializeField]
		private int[] displayIndicesByMaterial = new int[1];
	}
}
