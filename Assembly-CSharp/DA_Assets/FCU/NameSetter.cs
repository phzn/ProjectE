using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DA_Assets.Constants;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002D2 RID: 722
	[Serializable]
	public class NameSetter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001425 RID: 5157 RVA: 0x00081E14 File Offset: 0x00080014
		public void SetNames(FObject fobject)
		{
			FNames fnames = new FNames();
			fnames.FigmaName = fobject.Name;
			fnames.ObjectName = this.GetFcuName(fobject, FcuNameType.Object, null);
			fnames.UitkGuid = this.GetFcuName(fobject, FcuNameType.UitkGuid, null);
			fnames.LocKey = this.GetFcuName(fobject, FcuNameType.LocKey, null);
			fnames.FieldName = this.GetFcuName(fobject, FcuNameType.Field, null);
			fnames.MethodName = this.GetFcuName(fobject, FcuNameType.Method, null);
			fnames.ClassName = this.GetFcuName(fobject, FcuNameType.Class, null);
			fnames.HumanizedTextPrefabName = this.GetFcuName(fobject, FcuNameType.HumanizedTextPrefabName, null);
			fobject.Data.Names = fnames;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00081EAA File Offset: 0x000800AA
		private string GetSpritePath(FObject fobject)
		{
			return URIHelpersRef.MakeAssetUri(this.monoBeh.SpriteProcessor.GetSprite(fobject));
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00081EC4 File Offset: 0x000800C4
		private string GetFontPath(FObject fobject)
		{
			Font fontFromArray = this.monoBeh.FontLoader.GetFontFromArray<Font>(fobject, this.monoBeh.FontLoader.TtfFonts);
			FcuLogger.Debug(string.Format("UITK_Converter | Font | {0} | {1}", fobject.Data.NameHierarchy, fontFromArray), FcuDebugSettingsFlags.LogDefault);
			return URIHelpersRef.MakeAssetUri(fontFromArray);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00081F18 File Offset: 0x00080118
		internal async Task Set_UITK_Names(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				if (!fobject.Data.SpritePath.IsEmpty())
				{
					fobject.Data.Names.UITK_SpritePath = this.GetSpritePath(fobject);
				}
				else if (fobject.ContainsTag(FcuTag.Text))
				{
					fobject.Data.Names.UITK_FontPath = this.GetFontPath(fobject);
				}
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00081F64 File Offset: 0x00080164
		internal async Task SetNames(List<FObject> fobjects, FcuNameType nameType)
		{
			Action<FObject> <>9__1;
			await Task.Run(delegate
			{
				IEnumerable<FObject> fobjects2 = fobjects;
				Action<FObject> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(FObject fobject)
					{
						fobject.Data.Names.Names[nameType] = this.GetFcuName(fobject, nameType, null);
					});
				}
				Parallel.ForEach<FObject>(fobjects2, action);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00081FB8 File Offset: 0x000801B8
		public string GetFcuName(FObject fobject, FcuNameType nameType, string overrideName = null)
		{
			string text = ((overrideName == null) ? fobject.Name : overrideName);
			try
			{
				if (overrideName == null)
				{
					this.BaseNameProcess(ref text);
				}
				switch (nameType)
				{
				case FcuNameType.Object:
					if (this.monoBeh.IsUITK())
					{
						text = text.RemoveNonLettersAndNonNumbers("-");
						if (text.Length > 0 && char.IsDigit(text[0]))
						{
							text = "_" + text;
						}
					}
					break;
				case FcuNameType.Field:
				case FcuNameType.Method:
				case FcuNameType.Class:
					text = this.GetCsSharpName(text, nameType, fobject);
					return text;
				case FcuNameType.File:
					if (fobject.IsSprite())
					{
						string name = fobject.Name;
						this.BaseNameProcess(ref name);
						text = "";
						text += name;
						text += " ";
						text = text + fobject.Data.Scale.ToDotString() + "x";
						text += " ";
						text += fobject.Data.Hash.ToString();
						text = text + "." + fobject.Data.ImageFormat.ToLower();
						return text;
					}
					break;
				case FcuNameType.UitkGuid:
					text = Guid.NewGuid().ToString();
					text = text.Replace("-", "");
					return text;
				case FcuNameType.UssClass:
					text = this.GetUssClassName(fobject);
					break;
				case FcuNameType.LocKey:
					text = this.GetLocKey(text, fobject);
					if (text == null)
					{
						return null;
					}
					break;
				case FcuNameType.HumanizedTextPrefabName:
					if (fobject.Type == NodeType.TEXT)
					{
						return this.monoBeh.NameHumanizer.GetHumanizedTextPrefabName(fobject);
					}
					return null;
				}
				text = text.RemoveRepeatsForNonAlphanumericCharacters();
				text = text.Trim();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			string text2;
			if (this.TryRestoreName(text, fobject, out text2))
			{
				text = this.GetFcuName(fobject, nameType, text2);
			}
			else if (overrideName == null)
			{
				this.SubstringName(nameType, fobject, ref text);
			}
			return text;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x000821D4 File Offset: 0x000803D4
		private string GetLocKey(string name, FObject fobject)
		{
			int num = 0;
			float num2;
			DateTime dateTime;
			TimeSpan timeSpan;
			if (name.IsEmpty())
			{
				num = 1;
			}
			else if (name.TryParseFloat(out num2))
			{
				num = 2;
			}
			else if (DateTime.TryParse(name, out dateTime))
			{
				num = 3;
			}
			else if (TimeSpan.TryParse(name, out timeSpan))
			{
				num = 4;
			}
			else if (name.IsValidEmail())
			{
				num = 5;
			}
			else if (name.IsSpecialNumber())
			{
				num = 6;
			}
			else if (name.IsPhoneNumber())
			{
				num = 7;
			}
			else if (name.StartsWith("http://") || name.StartsWith("https://"))
			{
				num = 8;
			}
			if (num != 0)
			{
				return null;
			}
			name = name.RemoveNonLettersAndNonNumbers("_");
			if (name.IsAllUpper())
			{
				switch (this.monoBeh.Settings.LocalizationSettings.LocKeyCaseType)
				{
				case LocalizationKeyCaseType.snake_case:
					name = name.ToLower();
					break;
				case LocalizationKeyCaseType.UPPER_SNAKE_CASE:
					name = name.ToUpper();
					break;
				case LocalizationKeyCaseType.PascalCase:
					name = name.ToPascalCase();
					break;
				}
			}
			else
			{
				switch (this.monoBeh.Settings.LocalizationSettings.LocKeyCaseType)
				{
				case LocalizationKeyCaseType.snake_case:
					name = name.ToSnakeCase();
					break;
				case LocalizationKeyCaseType.UPPER_SNAKE_CASE:
					name = name.ToUpperSnakeCase();
					break;
				case LocalizationKeyCaseType.PascalCase:
					name = name.ToPascalCase();
					break;
				}
			}
			return name;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00082308 File Offset: 0x00080508
		private void BaseNameProcess(ref string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			string text = Regex.Escape(new string(this.monoBeh.Settings.MainSettings.AllowedNameChars));
			string text2 = "[^a-zA-Z0-9" + text + "]";
			name = Regex.Replace(name, text2, " ");
			name = name.RemoveInvalidCharsFromFileName(' ');
			while (!string.IsNullOrEmpty(name) && !Regex.IsMatch(name[0].ToString(), "^[a-zA-Z]"))
			{
				name = name.Remove(0, 1);
			}
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0008239C File Offset: 0x0008059C
		private string GetUssClassName(FObject fobject)
		{
			string text;
			if (this.GetManualUssClassName(fobject, out text))
			{
				return text;
			}
			string fcuName = this.GetFcuName(fobject, FcuNameType.Object, null);
			return string.Format("style-{0}-{1}", fcuName, fobject.Data.Hash);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x000823DC File Offset: 0x000805DC
		private void SubstringName(FcuNameType nameType, FObject fobject, ref string name)
		{
			switch (nameType)
			{
			case FcuNameType.Object:
			case FcuNameType.File:
			case FcuNameType.HumanizedTextPrefabName:
				if (fobject.Type == NodeType.TEXT)
				{
					name = name.SubstringSafe(this.monoBeh.Settings.MainSettings.TextObjectNameMaxLenght);
					return;
				}
				name = name.SubstringSafe(this.monoBeh.Settings.MainSettings.GameObjectNameMaxLenght);
				return;
			case FcuNameType.Field:
				name = name.SubstringSafe(this.monoBeh.Settings.ScriptGeneratorSettings.FieldNameMaxLenght);
				return;
			case FcuNameType.Method:
				name = name.SubstringSafe(this.monoBeh.Settings.ScriptGeneratorSettings.MethodNameMaxLenght);
				return;
			case FcuNameType.UitkGuid:
			case FcuNameType.UssClass:
			case FcuNameType.UxmlPath:
				break;
			case FcuNameType.Class:
				name = name.SubstringSafe(this.monoBeh.Settings.ScriptGeneratorSettings.ClassNameMaxLenght);
				return;
			case FcuNameType.LocKey:
				name = name.SubstringSafe(this.monoBeh.Settings.LocalizationSettings.LocKeyMaxLenght);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x000824DC File Offset: 0x000806DC
		private string GetCsSharpName(string name, FcuNameType nameType, FObject fobject)
		{
			char c = ' ';
			switch (nameType)
			{
			case FcuNameType.Field:
				c = '_';
				break;
			case FcuNameType.Method:
				c = 'M';
				break;
			case FcuNameType.Class:
				c = 'C';
				break;
			}
			try
			{
				name = name.RemoveNonLettersAndNonNumbers("_");
				name = name.Trim();
			}
			catch
			{
			}
			string text;
			if (this.TryRestoreName(name, fobject, out text))
			{
				name = text;
			}
			name = name.ToPascalCase();
			if (char.IsDigit(name[0]) || c == '_' || CsSharpKeywords.Keywords.Contains(name))
			{
				if (c == 'C' || c == 'M')
				{
					name = c.ToString() + name;
					name = name.MakeCharUpper(2);
				}
				else
				{
					name = c.ToString() + name;
					name = name.MakeCharLower(1);
				}
			}
			this.SubstringName(nameType, fobject, ref name);
			int num = 0;
			switch (nameType)
			{
			case FcuNameType.Field:
				this.FindNameInDict(name, ref this._fieldNames, out num);
				break;
			case FcuNameType.Method:
				this.FindNameInDict(name, ref this._methodNames, out num);
				break;
			case FcuNameType.Class:
				if (this._classNames.Count < 1)
				{
					int maxFileNumber = AssetTools.GetMaxFileNumber("Assets", name, "cs");
					if (maxFileNumber >= 0)
					{
						this._classNames.TryAdd(name, maxFileNumber);
					}
				}
				this.FindNameInDict(name, ref this._classNames, out num);
				break;
			}
			if (num > 0)
			{
				name = string.Format("{0}_{1}", name, num);
			}
			return name;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0008265C File Offset: 0x0008085C
		private void FindNameInDict(string name, ref ConcurrentDictionary<string, int> dict, out int number)
		{
			if (dict.TryGetValue(name, out number))
			{
				dict[name] = number + 1;
				number = dict[name];
				return;
			}
			dict.TryAdd(name, 0);
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0008268A File Offset: 0x0008088A
		public void ClearNames()
		{
			this._fieldNames.Clear();
			this._methodNames.Clear();
			this._classNames.Clear();
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000826B0 File Offset: 0x000808B0
		private bool TryRestoreName(string name, FObject fobject, out string restoredName)
		{
			restoredName = null;
			bool flag = !name.IsEmpty() && Regex.Matches(name, "[a-zA-Z]").Count > 0;
			if (name.IsEmpty() || !flag)
			{
				string text = (this.monoBeh.IsUITK() ? "-" : " ");
				restoredName = string.Format("{0}{1}{2}", "GameObject", text, fobject.Id.GetNumberOnly());
				return true;
			}
			return false;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0008272C File Offset: 0x0008092C
		public bool GetManualUssClassName(FObject fobject, out string className)
		{
			className = "";
			string name = fobject.Name;
			if (string.IsNullOrWhiteSpace(name) || !name.StartsWith(".") || !name.Contains(string.Format(" {0} ", FcuConfig.RealTagSeparator)))
			{
				return false;
			}
			int num = 1;
			int num2 = name.IndexOf(string.Format(" {0} ", FcuConfig.RealTagSeparator));
			if (num2 == -1)
			{
				return false;
			}
			className = name.Substring(num, num2 - num);
			return true;
		}

		// Token: 0x040015BE RID: 5566
		private ConcurrentDictionary<string, int> _fieldNames = new ConcurrentDictionary<string, int>();

		// Token: 0x040015BF RID: 5567
		private ConcurrentDictionary<string, int> _methodNames = new ConcurrentDictionary<string, int>();

		// Token: 0x040015C0 RID: 5568
		private ConcurrentDictionary<string, int> _classNames = new ConcurrentDictionary<string, int>();
	}
}
