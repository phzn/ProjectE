using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000256 RID: 598
	public static class RuntimePanelUtils
	{
		// Token: 0x06001123 RID: 4387 RVA: 0x0003DED0 File Offset: 0x0003C0D0
		public static Vector2 ScreenToPanel(IPanel panel, Vector2 screenPosition)
		{
			return ((BaseRuntimePanel)panel).ScreenToPanel(screenPosition);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0003DEF0 File Offset: 0x0003C0F0
		public static Vector2 CameraTransformWorldToPanel(IPanel panel, Vector3 worldPosition, Camera camera)
		{
			Vector2 vector = camera.WorldToScreenPoint(worldPosition);
			vector.y = (float)Screen.height - vector.y;
			return ((BaseRuntimePanel)panel).ScreenToPanel(vector);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0003DF30 File Offset: 0x0003C130
		public static Rect CameraTransformWorldToPanelRect(IPanel panel, Vector3 worldPosition, Vector2 worldSize, Camera camera)
		{
			worldSize.y = -worldSize.y;
			Vector2 vector = RuntimePanelUtils.CameraTransformWorldToPanel(panel, worldPosition, camera);
			Vector3 vector2 = worldPosition + camera.worldToCameraMatrix.MultiplyVector(worldSize);
			Vector2 vector3 = RuntimePanelUtils.CameraTransformWorldToPanel(panel, vector2, camera);
			return new Rect(vector, vector3 - vector);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0003DF8C File Offset: 0x0003C18C
		public static void ResetDynamicAtlas(this IPanel panel)
		{
			BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
			bool flag = baseVisualElementPanel == null;
			if (!flag)
			{
				DynamicAtlas dynamicAtlas = baseVisualElementPanel.atlas as DynamicAtlas;
				if (dynamicAtlas != null)
				{
					dynamicAtlas.Reset();
				}
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0003DFC4 File Offset: 0x0003C1C4
		public static void SetTextureDirty(this IPanel panel, Texture2D texture)
		{
			BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
			bool flag = baseVisualElementPanel == null;
			if (!flag)
			{
				DynamicAtlas dynamicAtlas = baseVisualElementPanel.atlas as DynamicAtlas;
				if (dynamicAtlas != null)
				{
					dynamicAtlas.SetDirty(texture);
				}
			}
		}
	}
}
