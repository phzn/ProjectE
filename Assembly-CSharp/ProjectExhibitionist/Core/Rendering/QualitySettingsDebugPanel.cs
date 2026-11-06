using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000ABD RID: 2749
	public class QualitySettingsDebugPanel : MonoBehaviour
	{
		// Token: 0x06004E0D RID: 19981 RVA: 0x001E42B0 File Offset: 0x001E24B0
		private void Update()
		{
			this._frameCount++;
			if (Time.time > this._lastUpdateTime + 1f)
			{
				this.currentFPS = this._frameCount;
				this._frameCount = 0;
				this._lastUpdateTime = Time.time;
			}
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance != null)
			{
				this.currentGraphicsProfile = instance.CurrentPresetName;
				this.currentRenderScale = instance.Model.renderScale;
			}
			ChunkManager instance2 = ChunkManager.Instance;
			if (instance2 != null && instance2.Config != null)
			{
				this.currentChunkPreset = instance2.Config.activePreset.ToString();
			}
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x001E4361 File Offset: 0x001E2561
		[ContextMenu("Test/Graphics: Low")]
		public void GraphicsLow()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityLevel(GraphicsSettingsSO.QualityLevel.Low);
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x001E4373 File Offset: 0x001E2573
		[ContextMenu("Test/Graphics: Medium")]
		public void GraphicsMedium()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityLevel(GraphicsSettingsSO.QualityLevel.Medium);
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x001E4385 File Offset: 0x001E2585
		[ContextMenu("Test/Graphics: High")]
		public void GraphicsHigh()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityLevel(GraphicsSettingsSO.QualityLevel.High);
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x001E4397 File Offset: 0x001E2597
		[ContextMenu("Test/Graphics: Ultra")]
		public void GraphicsUltra()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityLevel(GraphicsSettingsSO.QualityLevel.Ultra);
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x001E43A9 File Offset: 0x001E25A9
		[ContextMenu("Test/Chunk: Low")]
		public void ChunkLow()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityPreset(ChunkManagerConfig.QualityPreset.Low);
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x001E43BB File Offset: 0x001E25BB
		[ContextMenu("Test/Chunk: Medium")]
		public void ChunkMedium()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityPreset(ChunkManagerConfig.QualityPreset.Medium);
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x001E43CD File Offset: 0x001E25CD
		[ContextMenu("Test/Chunk: High")]
		public void ChunkHigh()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityPreset(ChunkManagerConfig.QualityPreset.High);
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x001E43DF File Offset: 0x001E25DF
		[ContextMenu("Test/Chunk: Ultra")]
		public void ChunkUltra()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetQualityPreset(ChunkManagerConfig.QualityPreset.Ultra);
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x001E43F1 File Offset: 0x001E25F1
		[ContextMenu("Test/Render Scale 0.5")]
		public void RenderScale05()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetRenderScale(0.5f);
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x001E4407 File Offset: 0x001E2607
		[ContextMenu("Test/Render Scale 0.75")]
		public void RenderScale075()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetRenderScale(0.75f);
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x001E441D File Offset: 0x001E261D
		[ContextMenu("Test/Render Scale 1.0")]
		public void RenderScale10()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetRenderScale(1f);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x001E4433 File Offset: 0x001E2633
		[ContextMenu("Test/Render Scale 1.5")]
		public void RenderScale15()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetRenderScale(1.5f);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x001E4449 File Offset: 0x001E2649
		[ContextMenu("Test/VSync ON")]
		public void VSyncOn()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetVSync(true);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x001E445B File Offset: 0x001E265B
		[ContextMenu("Test/VSync OFF")]
		public void VSyncOff()
		{
			GraphicsSettingsController instance = GraphicsSettingsController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetVSync(false);
		}

		// Token: 0x04004F68 RID: 20328
		[Header("Test Graphics Quality")]
		[SerializeField]
		private bool showGraphicsButtons = true;

		// Token: 0x04004F69 RID: 20329
		[Header("Test Chunk Performance")]
		[SerializeField]
		private bool showChunkButtons = true;

		// Token: 0x04004F6A RID: 20330
		[Header("Runtime Info")]
		[SerializeField]
		[ReadOnly]
		private string currentGraphicsProfile = "-";

		// Token: 0x04004F6B RID: 20331
		[SerializeField]
		[ReadOnly]
		private string currentChunkPreset = "-";

		// Token: 0x04004F6C RID: 20332
		[SerializeField]
		[ReadOnly]
		private float currentRenderScale = 1f;

		// Token: 0x04004F6D RID: 20333
		[SerializeField]
		[ReadOnly]
		private int currentFPS;

		// Token: 0x04004F6E RID: 20334
		private float _lastUpdateTime;

		// Token: 0x04004F6F RID: 20335
		private int _frameCount;
	}
}
