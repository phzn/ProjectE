using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DA_Assets.DAB;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Attributes;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using DA_Assets.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU
{
	// Token: 0x020002E1 RID: 737
	[Serializable]
	public class ScriptGenerator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x000849EE File Offset: 0x00082BEE
		public void GenerateScripts()
		{
			this.GenerateScriptsAsync();
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x000849F7 File Offset: 0x00082BF7
		internal void Serialize()
		{
			this.SerializeAsync();
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00084A00 File Offset: 0x00082C00
		private async Task SerializeAsync()
		{
			if (!SceneBackuper.TryBackupActiveScene())
			{
				DALogger.LogError(FcuLocKey.log_cant_execute_because_no_backup.Localize(Array.Empty<object>()));
			}
			else
			{
				SyncHelper[] syncHelpers = this.monoBeh.SyncHelpers.GetAllSyncHelpers();
				this.monoBeh.SyncHelpers.RestoreRootFrames(syncHelpers);
				if (syncHelpers == null || syncHelpers.Length == 0)
				{
					Debug.LogError("No SyncHelpers found.");
				}
				else
				{
					IEnumerable<GroupedSyncHelpers> rootFrameGroups = from x in syncHelpers
						group x by x.Data.RootFrame into @group
						select new GroupedSyncHelpers
						{
							RootFrame = @group.Key,
							SyncHelpers = @group.ToList<SyncHelper>()
						};
					Type[] screenTypes = ScriptGenerator.GetScriptTypes();
					await this.SerializeObjectsAsync(syncHelpers, rootFrameGroups, screenTypes);
					await this.SerializeOnClickToMethodsAsync(syncHelpers, rootFrameGroups, screenTypes);
				}
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00084A44 File Offset: 0x00082C44
		private async Task SerializeOnClickToMethodsAsync(SyncHelper[] syncHelpers, IEnumerable<GroupedSyncHelpers> rootFrameGroups, Type[] screenTypes)
		{
			try
			{
				foreach (Type screenType in screenTypes)
				{
					foreach (GroupedSyncHelpers groupedSyncHelpers in rootFrameGroups)
					{
						if (groupedSyncHelpers.RootFrame == null)
						{
							Debug.LogError("RootFrame is null.");
						}
						else
						{
							GameObject gameObject = groupedSyncHelpers.RootFrame.GameObject;
							if (gameObject == null)
							{
								Debug.LogError("GameObject for RootFrame '" + groupedSyncHelpers.RootFrame.Id + "' is null.");
							}
							else
							{
								string className = groupedSyncHelpers.RootFrame.Names.ClassName;
								string name = screenType.Name;
								if (!className.IsEmpty() && !name.IsEmpty() && !(className != name))
								{
									gameObject.GetComponent(screenType);
									screenType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									foreach (SyncHelper syncHelper in groupedSyncHelpers.SyncHelpers)
									{
										if (!(syncHelper == null))
										{
											syncHelper.gameObject == null;
										}
									}
									await Task.Yield();
								}
							}
						}
					}
					IEnumerator<GroupedSyncHelpers> enumerator = null;
					screenType = null;
				}
				Type[] array = null;
				Debug.Log("Methods serialization completed successfully.");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00004F31 File Offset: 0x00003131
		private void SerializeOnClickInspector(GameObject uitkButton, MonoBehaviour rootComponent, string methodName)
		{
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00084A90 File Offset: 0x00082C90
		private async Task SerializeObjectsAsync(SyncHelper[] syncHelpers, IEnumerable<GroupedSyncHelpers> rootFrameGroups, Type[] screenTypes)
		{
			GameObject gameObject = null;
			MonoBehaviour monoBehaviour = null;
			try
			{
				foreach (Type screenType in screenTypes)
				{
					foreach (GroupedSyncHelpers groupedSyncHelpers in rootFrameGroups)
					{
						if (groupedSyncHelpers.RootFrame == null)
						{
							Debug.LogError("RootFrame is null.");
						}
						else
						{
							gameObject = groupedSyncHelpers.RootFrame.GameObject;
							if (gameObject == null)
							{
								Debug.LogError("GameObject for RootFrame '" + groupedSyncHelpers.RootFrame.Id + "' is null.");
							}
							else
							{
								string text = null;
								string text2 = null;
								switch (this.monoBeh.Settings.ScriptGeneratorSettings.SerializationMode)
								{
								case FieldSerializationMode.SyncHelpers:
									text = groupedSyncHelpers.RootFrame.Names.ClassName;
									text2 = screenType.Name;
									break;
								case FieldSerializationMode.Attributes:
								{
									FObjectAttribute fobjectAttribute = screenType.GetCustomAttribute<FObjectAttribute>();
									if (fobjectAttribute != null)
									{
										text = groupedSyncHelpers.RootFrame.Names.FigmaName;
										text2 = fobjectAttribute.Name;
									}
									break;
								}
								case FieldSerializationMode.GameObjectNames:
									text = gameObject.name;
									text2 = screenType.Name;
									break;
								default:
									throw new NotImplementedException();
								}
								if (!text.IsEmpty() && !text2.IsEmpty() && text == text2)
								{
									monoBehaviour = gameObject.GetComponent(screenType) as MonoBehaviour;
									if (monoBehaviour == null)
									{
										monoBehaviour = gameObject.AddComponent(screenType) as MonoBehaviour;
										gameObject.SetDirtyExt();
										Debug.Log("Added " + screenType.Name + " component to " + gameObject.name);
									}
									foreach (FieldInfo fieldInfo in monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
									{
										foreach (SyncHelper syncHelper in groupedSyncHelpers.SyncHelpers)
										{
											if (!(syncHelper.gameObject == null))
											{
												string text3 = null;
												string text4 = null;
												switch (this.monoBeh.Settings.ScriptGeneratorSettings.SerializationMode)
												{
												case FieldSerializationMode.SyncHelpers:
													text3 = syncHelper.Data.Names.FieldName;
													text4 = fieldInfo.Name;
													break;
												case FieldSerializationMode.Attributes:
												{
													FObjectAttribute fobjectAttribute = fieldInfo.GetCustomAttribute<FObjectAttribute>();
													if (fobjectAttribute != null)
													{
														text3 = syncHelper.Data.Names.FigmaName;
														text4 = fobjectAttribute.Name;
													}
													break;
												}
												case FieldSerializationMode.GameObjectNames:
													text3 = syncHelper.gameObject.name;
													text4 = fieldInfo.Name;
													break;
												default:
													throw new NotImplementedException();
												}
												if (!text3.IsEmpty() && !text4.IsEmpty() && text3 == text4)
												{
													this.AssignValue(monoBehaviour, fieldInfo, syncHelper);
													break;
												}
											}
										}
									}
									gameObject.SetDirtyExt();
									await Task.Yield();
								}
							}
						}
					}
					IEnumerator<GroupedSyncHelpers> enumerator = null;
					screenType = null;
				}
				Type[] array = null;
				Debug.Log("Serialization by names completed successfully.");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00084AE4 File Offset: 0x00082CE4
		private void AssignValue(MonoBehaviour rootComponent, FieldInfo fieldInfo, SyncHelper fieldSyncHelper)
		{
			GameObject gameObject = fieldSyncHelper.gameObject;
			if (gameObject == null)
			{
				Debug.LogError("GameObject for SyncHelper with name " + fieldSyncHelper.name + " is null.");
				return;
			}
			Type fieldType = fieldInfo.FieldType;
			if (typeof(Component).IsAssignableFrom(fieldType))
			{
				Component component = gameObject.GetComponent(fieldType);
				if (component == null)
				{
					Debug.LogWarning("Component for " + fieldInfo.Name + " is null.");
					return;
				}
				fieldInfo.SetValue(rootComponent, component);
				rootComponent.SetDirtyExt();
				Debug.Log("Assigned component " + component.GetType().Name + " to field " + fieldInfo.Name);
				return;
			}
			else
			{
				if (fieldType == typeof(GameObject))
				{
					fieldInfo.SetValue(rootComponent, gameObject);
					rootComponent.SetDirtyExt();
					Debug.Log("Assigned GameObject " + gameObject.name + " to field " + fieldInfo.Name);
					return;
				}
				Debug.LogWarning("Unsupported field type " + fieldType.Name + " for field " + fieldInfo.Name);
				return;
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00084BF8 File Offset: 0x00082DF8
		private async Task GenerateScriptsAsync()
		{
			if (!SceneBackuper.TryBackupActiveScene())
			{
				DALogger.LogError(FcuLocKey.log_cant_execute_because_no_backup.Localize(Array.Empty<object>()));
			}
			else
			{
				try
				{
					SyncHelper[] allSyncHelpers = this.monoBeh.SyncHelpers.GetAllSyncHelpers();
					this.monoBeh.SyncHelpers.RestoreRootFrames(allSyncHelpers);
					IEnumerable<GroupedSyncHelpers> enumerable = from item in allSyncHelpers
						group item by item.Data.RootFrame into @group
						select new GroupedSyncHelpers
						{
							RootFrame = @group.Key,
							SyncHelpers = @group.ToList<SyncHelper>()
						};
					foreach (GroupedSyncHelpers groupedSyncHelpers in enumerable)
					{
						string text = this.GenerateScript(groupedSyncHelpers);
						Debug.Log(text);
						string className = groupedSyncHelpers.RootFrame.Names.ClassName;
						string outputPath = this.monoBeh.Settings.ScriptGeneratorSettings.OutputPath;
						Directory.CreateDirectory(outputPath);
						File.WriteAllText(Path.Combine(outputPath, className + ".cs"), text.ToString());
						await Task.Yield();
					}
					IEnumerator<GroupedSyncHelpers> enumerator = null;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00084C3C File Offset: 0x00082E3C
		public string GenerateUsings()
		{
			List<string> list = new List<string>();
			if (this.monoBeh.IsUGUI())
			{
				list.Add("using UnityEngine.UI;");
				if (this.monoBeh.UsingTextMesh())
				{
					list.Add("#if TextMeshPro");
					list.Add("using TMPro;");
					list.Add("#endif");
				}
			}
			else
			{
				list.Add("using UnityEngine.UIElements;");
				list.Add("#if UITK_LINKER_EXISTS");
				list.Add("using DA_Assets.UEL;");
				list.Add("#endif");
			}
			return string.Join(Environment.NewLine, list);
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00084CD0 File Offset: 0x00082ED0
		private string GenerateScript(GroupedSyncHelpers group)
		{
			string className = group.RootFrame.Names.ClassName;
			string text = this.GenerateUsings();
			string text2 = FcuConfig.BaseClass.text;
			string fields = this.GetFields(group.SyncHelpers);
			string methods = this.GetMethods(group.SyncHelpers);
			return string.Format(text2, new object[]
			{
				text,
				this.monoBeh.Settings.ScriptGeneratorSettings.Namespace,
				className,
				this.monoBeh.Settings.ScriptGeneratorSettings.BaseClass,
				fields,
				methods
			});
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00084D68 File Offset: 0x00082F68
		private string GetFields(List<SyncHelper> syncHelpers)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string text = "[SerializeField]";
			string text2 = "        ";
			foreach (var anon in (from item in syncHelpers.Select(delegate(SyncHelper syncHelper)
				{
					string text3 = this.DetermineComponentName(syncHelper);
					return new
					{
						SyncHelper = syncHelper,
						ComponentName = text3
					};
				})
				orderby item.ComponentName, item.SyncHelper.Data.Names.FieldName
				select item).ToList())
			{
				string fieldName = anon.SyncHelper.Data.Names.FieldName;
				string componentName = anon.ComponentName;
				stringBuilder2.AppendLine(string.Concat(new string[] { text2, text, " ", componentName, " ", fieldName, ";" }));
			}
			return string.Format("{0}\n{1}", stringBuilder, stringBuilder2);
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00084E8C File Offset: 0x0008308C
		private string GetMethods(List<SyncHelper> syncHelpers)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string text = "        ";
			foreach (var anon in (from item in syncHelpers.Where<SyncHelper>((SyncHelper x) => x.ContainsTag(FcuTag.Button)).Select(delegate(SyncHelper syncHelper)
				{
					string text2 = this.DetermineComponentName(syncHelper);
					return new
					{
						SyncHelper = syncHelper,
						ComponentName = text2
					};
				})
				orderby item.ComponentName, item.SyncHelper.Data.Names.MethodName
				select item).ToList())
			{
				string methodName = anon.SyncHelper.Data.Names.MethodName;
				string componentName = anon.ComponentName;
				stringBuilder2.AppendLine(string.Concat(new string[] { text, "public void ", methodName, "_OnClick()\n", text, "{\n\n", text, "}" }));
				stringBuilder2.AppendLine();
			}
			return string.Format("{0}\n{1}", stringBuilder, stringBuilder2);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00084FDC File Offset: 0x000831DC
		private string DetermineComponentName(SyncHelper syncHelper)
		{
			if (!this.monoBeh.IsUGUI())
			{
				return "GameObject";
			}
			Text text;
			if (syncHelper.gameObject.TryGetComponentSafe<Text>(out text))
			{
				return "Text";
			}
			TMP_Text tmp_Text;
			if (syncHelper.gameObject.TryGetComponentSafe<TMP_Text>(out tmp_Text))
			{
				return "TMP_Text";
			}
			TMP_InputField tmp_InputField;
			if (syncHelper.gameObject.TryGetComponentSafe<TMP_InputField>(out tmp_InputField))
			{
				return "TMP_InputField";
			}
			Button button;
			if (syncHelper.gameObject.TryGetComponentSafe<Button>(out button))
			{
				return "Button";
			}
			DAButton dabutton;
			if (syncHelper.gameObject.TryGetComponentSafe<DAButton>(out dabutton))
			{
				return "DAButton";
			}
			InputField inputField;
			if (syncHelper.gameObject.TryGetComponentSafe<InputField>(out inputField))
			{
				return "InputField";
			}
			return "GameObject";
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00085084 File Offset: 0x00083284
		private static Type[] GetScriptTypes()
		{
			Type[] array;
			try
			{
				Assembly assembly = Assembly.Load("Assembly-CSharp");
				if (assembly == null)
				{
					Debug.LogError("Failed to load Assembly-CSharp.");
					array = Array.Empty<Type>();
				}
				else
				{
					array = (from t in assembly.GetTypes()
						where t.IsClass && t.IsPublic && typeof(MonoBehaviour).IsAssignableFrom(t)
						select t).ToArray<Type>();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Unexpected error: " + ex.Message);
				array = Array.Empty<Type>();
			}
			return array;
		}
	}
}
