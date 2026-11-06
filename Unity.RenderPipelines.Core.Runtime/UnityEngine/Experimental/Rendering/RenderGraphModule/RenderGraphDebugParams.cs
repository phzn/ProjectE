using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000012 RID: 18
	internal class RenderGraphDebugParams
	{
		// Token: 0x0600007B RID: 123 RVA: 0x0000424C File Offset: 0x0000244C
		public void RegisterDebug(string name, DebugUI.Panel debugPanel = null)
		{
			this.m_DebugItems = new List<DebugUI.Widget>
			{
				new DebugUI.Container
				{
					displayName = name + " Render Graph",
					children = 
					{
						new DebugUI.BoolField
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.ClearRenderTargetsAtCreation,
							getter = () => this.clearRenderTargetsAtCreation,
							setter = delegate(bool value)
							{
								this.clearRenderTargetsAtCreation = value;
							}
						},
						new DebugUI.BoolField
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.DisablePassCulling,
							getter = () => this.disablePassCulling,
							setter = delegate(bool value)
							{
								this.disablePassCulling = value;
							}
						},
						new DebugUI.BoolField
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.ImmediateMode,
							getter = () => this.immediateMode,
							setter = delegate(bool value)
							{
								this.immediateMode = value;
							}
						},
						new DebugUI.BoolField
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.EnableLogging,
							getter = () => this.enableLogging,
							setter = delegate(bool value)
							{
								this.enableLogging = value;
							}
						},
						new DebugUI.Button
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.LogFrameInformation,
							action = delegate
							{
								if (!this.enableLogging)
								{
									Debug.Log("You must first enable logging before this logging frame information.");
								}
								this.logFrameInformation = true;
							}
						},
						new DebugUI.Button
						{
							nameAndTooltip = RenderGraphDebugParams.Strings.LogResources,
							action = delegate
							{
								if (!this.enableLogging)
								{
									Debug.Log("You must first enable logging before this logging resources.");
								}
								this.logResources = true;
							}
						}
					}
				}
			}.ToArray();
			this.m_DebugPanel = ((debugPanel != null) ? debugPanel : DebugManager.instance.GetPanel((name.Length == 0) ? "Render Graph" : name, true, 0, false));
			this.m_DebugPanel.children.Add(this.m_DebugItems);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000441C File Offset: 0x0000261C
		public void UnRegisterDebug(string name)
		{
			this.m_DebugPanel.children.Remove(this.m_DebugItems);
			this.m_DebugPanel = null;
			this.m_DebugItems = null;
		}

		// Token: 0x04000063 RID: 99
		private DebugUI.Widget[] m_DebugItems;

		// Token: 0x04000064 RID: 100
		private DebugUI.Panel m_DebugPanel;

		// Token: 0x04000065 RID: 101
		public bool clearRenderTargetsAtCreation;

		// Token: 0x04000066 RID: 102
		public bool clearRenderTargetsAtRelease;

		// Token: 0x04000067 RID: 103
		public bool disablePassCulling;

		// Token: 0x04000068 RID: 104
		public bool immediateMode;

		// Token: 0x04000069 RID: 105
		public bool enableLogging;

		// Token: 0x0400006A RID: 106
		public bool logFrameInformation;

		// Token: 0x0400006B RID: 107
		public bool logResources;

		// Token: 0x02000144 RID: 324
		private static class Strings
		{
			// Token: 0x040005A4 RID: 1444
			public static readonly DebugUI.Widget.NameAndTooltip ClearRenderTargetsAtCreation = new DebugUI.Widget.NameAndTooltip
			{
				name = "Clear Render Targets At Creation",
				tooltip = "Enable to clear all render textures before any rendergraph passes to check if some clears are missing."
			};

			// Token: 0x040005A5 RID: 1445
			public static readonly DebugUI.Widget.NameAndTooltip DisablePassCulling = new DebugUI.Widget.NameAndTooltip
			{
				name = "Disable Pass Culling",
				tooltip = "Enable to temporarily disable culling to asses if a pass is culled."
			};

			// Token: 0x040005A6 RID: 1446
			public static readonly DebugUI.Widget.NameAndTooltip ImmediateMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "Immediate Mode",
				tooltip = "Enable to force render graph to execute all passes in the order you registered them."
			};

			// Token: 0x040005A7 RID: 1447
			public static readonly DebugUI.Widget.NameAndTooltip EnableLogging = new DebugUI.Widget.NameAndTooltip
			{
				name = "Enable Logging",
				tooltip = "Enable to allow HDRP to capture information in the log."
			};

			// Token: 0x040005A8 RID: 1448
			public static readonly DebugUI.Widget.NameAndTooltip LogFrameInformation = new DebugUI.Widget.NameAndTooltip
			{
				name = "Log Frame Information",
				tooltip = "Enable to log information output from each frame."
			};

			// Token: 0x040005A9 RID: 1449
			public static readonly DebugUI.Widget.NameAndTooltip LogResources = new DebugUI.Widget.NameAndTooltip
			{
				name = "Log Resources",
				tooltip = "Enable to log the current render graph's global resource usage."
			};
		}
	}
}
