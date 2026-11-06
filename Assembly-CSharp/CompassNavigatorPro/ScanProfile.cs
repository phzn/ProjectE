using System;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x020002AE RID: 686
	[CreateAssetMenu(fileName = "Compass Navigator Pro Scan Profile", menuName = "Compass Navigator Pro Scan Profile", order = 1200)]
	[ExecuteAlways]
	public class ScanProfile : ScriptableObject
	{
		// Token: 0x06001370 RID: 4976 RVA: 0x0007DFDA File Offset: 0x0007C1DA
		private void Validate()
		{
			this.ValidateSettings();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0007DFE4 File Offset: 0x0007C1E4
		public void ValidateSettings()
		{
			this.maxRange = Mathf.Max(0f, this.maxRange);
			this.duration = Mathf.Max(0f, this.duration);
			this.fadeOutStart = Mathf.Max(0f, this.fadeOutStart);
			this.fadeOutDuration = Mathf.Max(0f, this.fadeOutDuration);
			this.width = Mathf.Max(0f, this.width);
			this.gridSize = Mathf.Max(0.001f, this.gridSize);
			this.gridDepthWidthControl = Mathf.Max(1f, this.gridDepthWidthControl);
			this.gridDepthStart = Mathf.Max(1f, this.gridDepthStart);
			this.edgeWidth = Mathf.Max(0.01f, this.edgeWidth);
		}

		// Token: 0x0400153A RID: 5434
		public Vector3 center;

		// Token: 0x0400153B RID: 5435
		[ColorUsage(true, true)]
		public Color color = new Color(0f, 0.5f, 1f, 0.2f);

		// Token: 0x0400153C RID: 5436
		public float maxRange = 800f;

		// Token: 0x0400153D RID: 5437
		public float duration = 2f;

		// Token: 0x0400153E RID: 5438
		public float fadeOutStart = 2f;

		// Token: 0x0400153F RID: 5439
		public float fadeOutDuration = 1f;

		// Token: 0x04001540 RID: 5440
		public float width = 100f;

		// Token: 0x04001541 RID: 5441
		public bool grid = true;

		// Token: 0x04001542 RID: 5442
		[ColorUsage(true, true)]
		public Color gridColor = new Color(1f, 1f, 0f);

		// Token: 0x04001543 RID: 5443
		public float gridSize = 0.1f;

		// Token: 0x04001544 RID: 5444
		public float gridDepthWidthControl = 150f;

		// Token: 0x04001545 RID: 5445
		public float gridDepthStart = 100f;

		// Token: 0x04001546 RID: 5446
		public bool edgeOutline = true;

		// Token: 0x04001547 RID: 5447
		[ColorUsage(true, true)]
		public Color edgeColor = Color.white;

		// Token: 0x04001548 RID: 5448
		[Range(0.001f, 0.1f)]
		public float edgeDepthThreshold = 0.01f;

		// Token: 0x04001549 RID: 5449
		[Range(0.0001f, 0.5f)]
		public float edgeColorThreshold = 0.1f;

		// Token: 0x0400154A RID: 5450
		[Range(0f, 1f)]
		public float edgeStrength = 1f;

		// Token: 0x0400154B RID: 5451
		public float edgeWidth = 1f;

		// Token: 0x0400154C RID: 5452
		public bool filled;

		// Token: 0x0400154D RID: 5453
		[ColorUsage(true, false)]
		public Color colorBlend = new Color(0f, 0f, 0f, 0.75f);

		// Token: 0x0400154E RID: 5454
		public bool scanLines = true;

		// Token: 0x0400154F RID: 5455
		public bool noise = true;

		// Token: 0x04001550 RID: 5456
		public LayerMask hitLayerMask = -1;

		// Token: 0x04001551 RID: 5457
		[Tooltip("If true, disabled POIs will also be detected, otherwise only active/enabled POIs will be used in the scan detection")]
		public bool detectDisabledPOIs = true;

		// Token: 0x04001552 RID: 5458
		[Tooltip("Audio clip to play when the scan starts")]
		public AudioClip audioClipStart;

		// Token: 0x04001553 RID: 5459
		[Tooltip("Audio clip to play when the scan finds a suitable target based on the hit layer mask")]
		public AudioClip audioClipHit;
	}
}
