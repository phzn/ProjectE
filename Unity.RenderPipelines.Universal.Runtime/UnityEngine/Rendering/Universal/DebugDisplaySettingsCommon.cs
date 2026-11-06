using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000056 RID: 86
	internal class DebugDisplaySettingsCommon : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0001407F File Offset: 0x0001227F
		public bool AreAnySettingsActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00014082 File Offset: 0x00012282
		public bool IsPostProcessingAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00014085 File Offset: 0x00012285
		public bool IsLightingActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00014088 File Offset: 0x00012288
		public bool TryGetScreenClearColor(ref Color _)
		{
			return false;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0001408B File Offset: 0x0001228B
		public IDebugDisplaySettingsPanelDisposable CreatePanel()
		{
			return new DebugDisplaySettingsCommon.SettingsPanel();
		}

		// Token: 0x0200016B RID: 363
		internal static class WidgetFactory
		{
			// Token: 0x06000A6C RID: 2668 RVA: 0x00047344 File Offset: 0x00045544
			internal static DebugUI.Widget CreateMissingDebugShadersWarning()
			{
				DebugUI.MessageBox messageBox = new DebugUI.MessageBox();
				messageBox.displayName = "Warning: the debug shader variants are missing. Ensure that the \"Strip Debug Variants\" option is disabled in URP Global Settings.";
				messageBox.style = DebugUI.MessageBox.Style.Warning;
				messageBox.isHiddenCallback = () => !(UniversalRenderPipelineGlobalSettings.instance != null) || !UniversalRenderPipelineGlobalSettings.instance.stripDebugVariants;
				return messageBox;
			}
		}

		// Token: 0x0200016C RID: 364
		[DisplayInfo(name = "Frequently Used", order = -1)]
		private class SettingsPanel : DebugDisplaySettingsPanel
		{
			// Token: 0x17000246 RID: 582
			// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00047382 File Offset: 0x00045582
			public override DebugUI.Flags Flags
			{
				get
				{
					return DebugUI.Flags.FrequentlyUsed;
				}
			}

			// Token: 0x06000A6E RID: 2670 RVA: 0x00047388 File Offset: 0x00045588
			public SettingsPanel()
			{
				base.AddWidget(DebugDisplaySettingsCommon.WidgetFactory.CreateMissingDebugShadersWarning());
				DebugUI.Widget[] items = DebugManager.instance.GetItems(DebugUI.Flags.FrequentlyUsed);
				for (int i = 0; i < items.Length; i++)
				{
					DebugUI.Widget widget = items[i];
					DebugUI.Foldout foldout = widget as DebugUI.Foldout;
					if (foldout != null)
					{
						if (foldout.contextMenuItems == null)
						{
							foldout.contextMenuItems = new List<DebugUI.Foldout.ContextMenuItem>();
						}
						foldout.contextMenuItems.Add(new DebugUI.Foldout.ContextMenuItem
						{
							displayName = "Go to Section...",
							action = delegate
							{
								int num = DebugManager.instance.PanelIndex(foldout.panel.displayName);
								if (num >= 0)
								{
									DebugManager.instance.RequestEditorWindowPanelIndex(num);
								}
							}
						});
					}
					base.AddWidget(widget);
				}
			}

			// Token: 0x040009D2 RID: 2514
			private const string k_GoToSectionString = "Go to Section...";
		}
	}
}
