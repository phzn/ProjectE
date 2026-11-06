using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x02000299 RID: 665
	internal abstract class BaseRuntimePanel : Panel
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x00042ACB File Offset: 0x00040CCB
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x00042AD4 File Offset: 0x00040CD4
		public GameObject selectableGameObject
		{
			get
			{
				return this.m_SelectableGameObject;
			}
			set
			{
				bool flag = this.m_SelectableGameObject != value;
				if (flag)
				{
					this.AssignPanelToComponents(null);
					this.m_SelectableGameObject = value;
					this.AssignPanelToComponents(this);
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x00042B0B File Offset: 0x00040D0B
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x00042B14 File Offset: 0x00040D14
		public float sortingPriority
		{
			get
			{
				return this.m_SortingPriority;
			}
			set
			{
				bool flag = !Mathf.Approximately(this.m_SortingPriority, value);
				if (flag)
				{
					this.m_SortingPriority = value;
					bool flag2 = this.contextType == ContextType.Player;
					if (flag2)
					{
						UIElementsRuntimeUtility.SetPanelOrderingDirty();
					}
				}
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001312 RID: 4882 RVA: 0x00042B54 File Offset: 0x00040D54
		// (remove) Token: 0x06001313 RID: 4883 RVA: 0x00042B8C File Offset: 0x00040D8C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action destroyed;

		// Token: 0x06001314 RID: 4884 RVA: 0x00042BC4 File Offset: 0x00040DC4
		protected BaseRuntimePanel(ScriptableObject ownerObject, EventDispatcher dispatcher = null)
			: base(ownerObject, ContextType.Player, dispatcher)
		{
			this.m_RuntimePanelCreationIndex = BaseRuntimePanel.s_CurrentRuntimePanelCounter++;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00042C20 File Offset: 0x00040E20
		protected override void Dispose(bool disposing)
		{
			bool disposed = base.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					Action action = this.destroyed;
					if (action != null)
					{
						action();
					}
				}
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x00042C5C File Offset: 0x00040E5C
		// (set) Token: 0x06001317 RID: 4887 RVA: 0x00042C74 File Offset: 0x00040E74
		internal override Shader standardWorldSpaceShader
		{
			get
			{
				return this.m_StandardWorldSpaceShader;
			}
			set
			{
				bool flag = this.m_StandardWorldSpaceShader != value;
				if (flag)
				{
					this.m_StandardWorldSpaceShader = value;
					base.InvokeStandardWorldSpaceShaderChanged();
				}
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x00042CA4 File Offset: 0x00040EA4
		// (set) Token: 0x06001319 RID: 4889 RVA: 0x00042CBC File Offset: 0x00040EBC
		internal bool drawToCameras
		{
			get
			{
				return this.m_DrawToCameras;
			}
			set
			{
				bool flag = this.m_DrawToCameras != value;
				if (flag)
				{
					this.m_DrawToCameras = value;
					UIRRepaintUpdater uirrepaintUpdater = this.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater;
					if (uirrepaintUpdater != null)
					{
						uirrepaintUpdater.DestroyRenderChain();
					}
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x00042CFB File Offset: 0x00040EFB
		// (set) Token: 0x0600131B RID: 4891 RVA: 0x00042D03 File Offset: 0x00040F03
		internal int targetDisplay { get; set; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00042D0C File Offset: 0x00040F0C
		internal int screenRenderingWidth
		{
			get
			{
				return BaseRuntimePanel.getScreenRenderingWidth(this.targetDisplay);
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00042D19 File Offset: 0x00040F19
		internal int screenRenderingHeight
		{
			get
			{
				return BaseRuntimePanel.getScreenRenderingHeight(this.targetDisplay);
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00042D28 File Offset: 0x00040F28
		internal static int getScreenRenderingHeight(int display)
		{
			return (display >= 0 && display < Display.displays.Length) ? Display.displays[display].renderingHeight : Screen.height;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00042D5C File Offset: 0x00040F5C
		internal static int getScreenRenderingWidth(int display)
		{
			return (display >= 0 && display < Display.displays.Length) ? Display.displays[display].renderingWidth : Screen.width;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00042D90 File Offset: 0x00040F90
		public override void Repaint(Event e)
		{
			bool flag = this.targetTexture == null;
			if (flag)
			{
				RenderTexture active = RenderTexture.active;
				int num = ((active != null) ? active.width : this.screenRenderingWidth);
				int num2 = ((active != null) ? active.height : this.screenRenderingHeight);
				GL.Viewport(new Rect(0f, 0f, (float)num, (float)num2));
				base.Repaint(e);
			}
			else
			{
				Camera current = Camera.current;
				RenderTexture active2 = RenderTexture.active;
				Camera.SetupCurrent(null);
				RenderTexture.active = this.targetTexture;
				GL.Viewport(new Rect(0f, 0f, (float)this.targetTexture.width, (float)this.targetTexture.height));
				base.Repaint(e);
				Camera.SetupCurrent(current);
				RenderTexture.active = active2;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00042E6E File Offset: 0x0004106E
		// (set) Token: 0x06001322 RID: 4898 RVA: 0x00042E76 File Offset: 0x00041076
		public Func<Vector2, Vector2> screenToPanelSpace
		{
			get
			{
				return this.m_ScreenToPanelSpace;
			}
			set
			{
				this.m_ScreenToPanelSpace = value ?? BaseRuntimePanel.DefaultScreenToPanelSpace;
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00042E88 File Offset: 0x00041088
		internal Vector2 ScreenToPanel(Vector2 screen)
		{
			return this.screenToPanelSpace(screen) / base.scale;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00042EB4 File Offset: 0x000410B4
		internal bool ScreenToPanel(Vector2 screenPosition, Vector2 screenDelta, out Vector2 panelPosition, out Vector2 panelDelta, bool allowOutside = false)
		{
			panelPosition = this.ScreenToPanel(screenPosition);
			bool flag = !allowOutside;
			Vector2 vector;
			if (flag)
			{
				Rect layout = this.visualTree.layout;
				bool flag2 = !layout.Contains(panelPosition);
				if (flag2)
				{
					panelDelta = screenDelta;
					return false;
				}
				vector = this.ScreenToPanel(screenPosition - screenDelta);
				bool flag3 = !layout.Contains(vector);
				if (flag3)
				{
					panelDelta = screenDelta;
					return true;
				}
			}
			else
			{
				vector = this.ScreenToPanel(screenPosition - screenDelta);
			}
			panelDelta = panelPosition - vector;
			return true;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00042F64 File Offset: 0x00041164
		private void AssignPanelToComponents(BaseRuntimePanel panel)
		{
			bool flag = this.selectableGameObject == null;
			if (!flag)
			{
				List<IRuntimePanelComponent> list = ObjectListPool<IRuntimePanelComponent>.Get();
				try
				{
					this.selectableGameObject.GetComponents<IRuntimePanelComponent>(list);
					foreach (IRuntimePanelComponent runtimePanelComponent in list)
					{
						runtimePanelComponent.panel = panel;
					}
				}
				finally
				{
					ObjectListPool<IRuntimePanelComponent>.Release(list);
				}
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00042FF8 File Offset: 0x000411F8
		internal void PointerLeavesPanel(int pointerId, Vector2 position)
		{
			base.ClearCachedElementUnderPointer(pointerId, null);
			base.CommitElementUnderPointers();
			PointerDeviceState.SavePointerPosition(pointerId, position, null, this.contextType);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0004301A File Offset: 0x0004121A
		internal void PointerEntersPanel(int pointerId, Vector2 position)
		{
			PointerDeviceState.SavePointerPosition(pointerId, position, this, this.contextType);
		}

		// Token: 0x040008A7 RID: 2215
		private GameObject m_SelectableGameObject;

		// Token: 0x040008A8 RID: 2216
		private static int s_CurrentRuntimePanelCounter = 0;

		// Token: 0x040008A9 RID: 2217
		internal readonly int m_RuntimePanelCreationIndex;

		// Token: 0x040008AA RID: 2218
		private float m_SortingPriority = 0f;

		// Token: 0x040008AB RID: 2219
		internal int resolvedSortingIndex = 0;

		// Token: 0x040008AD RID: 2221
		private Shader m_StandardWorldSpaceShader;

		// Token: 0x040008AE RID: 2222
		private bool m_DrawToCameras;

		// Token: 0x040008AF RID: 2223
		internal RenderTexture targetTexture = null;

		// Token: 0x040008B0 RID: 2224
		internal Matrix4x4 panelToWorld = Matrix4x4.identity;

		// Token: 0x040008B2 RID: 2226
		internal static readonly Func<Vector2, Vector2> DefaultScreenToPanelSpace = (Vector2 p) => p;

		// Token: 0x040008B3 RID: 2227
		private Func<Vector2, Vector2> m_ScreenToPanelSpace = BaseRuntimePanel.DefaultScreenToPanelSpace;
	}
}
