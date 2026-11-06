using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000381 RID: 897
	[Serializable]
	public class MainSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x000971BB File Offset: 0x000953BB
		// (set) Token: 0x0600176F RID: 5999 RVA: 0x000971C4 File Offset: 0x000953C4
		public UIFramework UIFramework
		{
			get
			{
				return this.uiFramework;
			}
			set
			{
				if (value != UIFramework.UITK)
				{
					if (value == UIFramework.NOVA)
					{
						DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "NOVA" }));
						value = UIFramework.UGUI;
					}
				}
				else
				{
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "UITK" }));
					value = UIFramework.UGUI;
				}
				base.SetValue<UIFramework>(ref this.uiFramework, value);
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x00097224 File Offset: 0x00095424
		// (set) Token: 0x06001771 RID: 6001 RVA: 0x0009722C File Offset: 0x0009542C
		public PositioningMode PositioningMode
		{
			get
			{
				return this.positioningMode;
			}
			set
			{
				if (value == PositioningMode.GameView && this.uiFramework != UIFramework.UGUI)
				{
					Debug.LogError(string.Format("{0}.{1} is not supported in {2} framework.", "PositioningMode", value, this.uiFramework));
					value = PositioningMode.Absolute;
				}
				base.SetValue<PositioningMode>(ref this.positioningMode, value);
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x0009727A File Offset: 0x0009547A
		// (set) Token: 0x06001773 RID: 6003 RVA: 0x00097282 File Offset: 0x00095482
		public PivotType PivotType
		{
			get
			{
				return this.pivotType;
			}
			set
			{
				base.SetValue<PivotType>(ref this.pivotType, value);
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001774 RID: 6004 RVA: 0x00097291 File Offset: 0x00095491
		// (set) Token: 0x06001775 RID: 6005 RVA: 0x00097299 File Offset: 0x00095499
		public int GameObjectLayer
		{
			get
			{
				return this.goLayer;
			}
			set
			{
				base.SetValue<int>(ref this.goLayer, value);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x000972A8 File Offset: 0x000954A8
		// (set) Token: 0x06001777 RID: 6007 RVA: 0x000972B0 File Offset: 0x000954B0
		public bool RawImport
		{
			get
			{
				return this.rawImport;
			}
			set
			{
				if (value && value != this.rawImport)
				{
					DALogger.LogError(FcuLocKey.log_dev_function_enabled.Localize(new object[] { FcuLocKey.label_raw_import.Localize(Array.Empty<object>()) }));
				}
				base.SetValue<bool>(ref this.rawImport, value);
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x000972EF File Offset: 0x000954EF
		// (set) Token: 0x06001779 RID: 6009 RVA: 0x000972F7 File Offset: 0x000954F7
		public bool Https
		{
			get
			{
				return this.https;
			}
			set
			{
				base.SetValue<bool>(ref this.https, value);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x00097306 File Offset: 0x00095506
		// (set) Token: 0x0600177B RID: 6011 RVA: 0x0009730E File Offset: 0x0009550E
		public int GameObjectNameMaxLenght
		{
			get
			{
				return this.gameObjectNameMaxLength;
			}
			set
			{
				base.SetValue<int>(ref this.gameObjectNameMaxLength, value);
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0009731D File Offset: 0x0009551D
		// (set) Token: 0x0600177D RID: 6013 RVA: 0x00097325 File Offset: 0x00095525
		public int TextObjectNameMaxLenght
		{
			get
			{
				return this.textObjectNameMaxLength;
			}
			set
			{
				base.SetValue<int>(ref this.textObjectNameMaxLength, value);
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x00097334 File Offset: 0x00095534
		// (set) Token: 0x0600177F RID: 6015 RVA: 0x0009733C File Offset: 0x0009553C
		[SerializeProperty("allowedNameChars")]
		public char[] AllowedNameChars
		{
			get
			{
				return this.allowedNameChars;
			}
			set
			{
				base.SetValue<char[]>(ref this.allowedNameChars, value);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x0009734B File Offset: 0x0009554B
		// (set) Token: 0x06001781 RID: 6017 RVA: 0x00097353 File Offset: 0x00095553
		public bool WindowMode
		{
			get
			{
				return this.windowMode;
			}
			set
			{
				base.SetValue<bool>(ref this.windowMode, value);
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x00097362 File Offset: 0x00095562
		// (set) Token: 0x06001783 RID: 6019 RVA: 0x0009736C File Offset: 0x0009556C
		public string ProjectUrl
		{
			get
			{
				return this.projectUrl;
			}
			set
			{
				string text = value;
				try
				{
					string text2 = "/file/";
					char c = '/';
					if (!text.IsEmpty())
					{
						if (text.Contains(text2))
						{
							text = text.GetBetween(text2, c.ToString());
						}
						else if (text.Contains(c.ToString()))
						{
							text = value.Split(c, StringSplitOptions.None)[4];
						}
					}
				}
				catch
				{
					Debug.LogError(FcuLocKey.log_incorrent_project_url.Localize(Array.Empty<object>()));
				}
				base.SetValue<string>(ref this.projectUrl, text);
			}
		}

		// Token: 0x040018FC RID: 6396
		[SerializeField]
		private UIFramework uiFramework;

		// Token: 0x040018FD RID: 6397
		[SerializeField]
		private PositioningMode positioningMode;

		// Token: 0x040018FE RID: 6398
		[SerializeField]
		private PivotType pivotType = PivotType.MiddleCenter;

		// Token: 0x040018FF RID: 6399
		[SerializeField]
		private int goLayer = 5;

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private bool rawImport;

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		private bool https = true;

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		private int gameObjectNameMaxLength = 32;

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		private int textObjectNameMaxLength = 16;

		// Token: 0x04001904 RID: 6404
		[Tooltip("Characters, aside from Latin letters and numbers, that may appear in GameObject names.\n\nSome characters will be ignored in certain cases, such as when a backslash is used in a sprite name.\n\nIf you add new characters to this list, the stable operation of the asset cannot be guaranteed.")]
		[SerializeField]
		private char[] allowedNameChars = new char[] { '_', ' ', '(', ')', '=', '.', '-', '[', ']', '+' };

		// Token: 0x04001905 RID: 6405
		[SerializeField]
		private bool windowMode;

		// Token: 0x04001906 RID: 6406
		[SerializeField]
		private string projectUrl;
	}
}
