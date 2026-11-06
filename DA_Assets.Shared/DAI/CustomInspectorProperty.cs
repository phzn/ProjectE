using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x0200003F RID: 63
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public abstract class CustomInspectorProperty : PropertyAttribute
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000062FD File Offset: 0x000044FD
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00006305 File Offset: 0x00004505
		public ComponentType Type { get; protected set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000630E File Offset: 0x0000450E
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00006316 File Offset: 0x00004516
		public GUIContent Label { get; protected set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000631F File Offset: 0x0000451F
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00006327 File Offset: 0x00004527
		public float MinValue { get; protected set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006330 File Offset: 0x00004530
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00006338 File Offset: 0x00004538
		public float MaxValue { get; protected set; }

		// Token: 0x0600013C RID: 316 RVA: 0x00006341 File Offset: 0x00004541
		protected CustomInspectorProperty(ComponentType type, GUIContent label, float minValue = 0f, float maxValue = 1f)
		{
			this.Type = type;
			this.Label = label;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}
	}
}
