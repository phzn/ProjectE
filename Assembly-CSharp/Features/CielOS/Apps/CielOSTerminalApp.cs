using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x02001411 RID: 5137
	public class CielOSTerminalApp : MonoBehaviour
	{
		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x06009E8D RID: 40589 RVA: 0x00413E18 File Offset: 0x00412018
		private string Prompt
		{
			get
			{
				return this.user + "@" + this.host + ":~$ ";
			}
		}

		// Token: 0x06009E8E RID: 40590 RVA: 0x00413E38 File Offset: 0x00412038
		private void Awake()
		{
			this._windowManager = Object.FindObjectOfType<CielOSWindowManager>();
			LocalizedGameText.BindComputed(this.output, new Func<string>(this.BuildOutput));
			if (this.input != null)
			{
				LocalizedGameText.BindFontOnly(this.input.textComponent);
				TMP_Text tmp_Text = this.input.placeholder as TMP_Text;
				if (tmp_Text != null)
				{
					CielOSContentText.Bind(tmp_Text, "type a command...");
				}
			}
		}

		// Token: 0x06009E8F RID: 40591 RVA: 0x00413EA6 File Offset: 0x004120A6
		private void OnEnable()
		{
			if (!this._greeted)
			{
				this._greeted = true;
				this.AppendSourceLine("CielOS Terminal v1.0  (CS student edition)");
				this.AppendSourceLine("Type 'help' for a list of commands.");
				this.AppendLine(string.Empty);
			}
			this.HookInput();
			this.FocusInput();
		}

		// Token: 0x06009E90 RID: 40592 RVA: 0x00413EE4 File Offset: 0x004120E4
		private void HookInput()
		{
			if (this.input == null)
			{
				return;
			}
			this.input.lineType = TMP_InputField.LineType.SingleLine;
			this.input.onSubmit.RemoveListener(new UnityAction<string>(this.OnSubmit));
			this.input.onSubmit.AddListener(new UnityAction<string>(this.OnSubmit));
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x00413F44 File Offset: 0x00412144
		private void OnDisable()
		{
			this.ResetUnlockPrompt();
			if (this.input != null)
			{
				this.input.onSubmit.RemoveListener(new UnityAction<string>(this.OnSubmit));
			}
		}

		// Token: 0x06009E92 RID: 40594 RVA: 0x00413F76 File Offset: 0x00412176
		private void Update()
		{
			if (this._unlockPrompt != CielOSTerminalApp.UnlockPrompt.None && GameplayInput.GetKeyDown(KeyCode.Escape))
			{
				this.ResetUnlockPrompt();
				this.AppendSourceLine("Cancelled.");
			}
		}

		// Token: 0x06009E93 RID: 40595 RVA: 0x00413F9A File Offset: 0x0041219A
		private void OnSubmit(string text)
		{
			if (!GameplayInput.GetKeyDown(KeyCode.Return) && !GameplayInput.GetKeyDown(KeyCode.KeypadEnter) && !ControllerTextKeyboard.IsCommitting)
			{
				this.FocusInput();
				return;
			}
			this.SubmitLine(text);
			this.FocusInput();
		}

		// Token: 0x06009E94 RID: 40596 RVA: 0x00413FCC File Offset: 0x004121CC
		private void SubmitLine(string text)
		{
			if (this.input != null)
			{
				this.input.SetTextWithoutNotify(string.Empty);
			}
			string text2 = text ?? string.Empty;
			if (this._unlockPrompt != CielOSTerminalApp.UnlockPrompt.None)
			{
				this.SubmitUnlockPrompt(text2);
				return;
			}
			string text3 = text2.Trim();
			int num = CielOSTerminalApp.CommandSeparator(text3);
			bool flag = string.Equals((num < 0) ? text3 : text3.Substring(0, num), "unlock", StringComparison.OrdinalIgnoreCase);
			this.AppendLine(this.Prompt + (flag ? "unlock" : text2));
			if (text3.Length == 0)
			{
				return;
			}
			this._history.Add(flag ? "unlock" : text3);
			this.Execute(text3);
		}

		// Token: 0x06009E95 RID: 40597 RVA: 0x00414080 File Offset: 0x00412280
		private static int CommandSeparator(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				if (char.IsWhiteSpace(line[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06009E96 RID: 40598 RVA: 0x004140B0 File Offset: 0x004122B0
		private void Execute(string commandLine)
		{
			int num = CielOSTerminalApp.CommandSeparator(commandLine);
			string text = ((num < 0) ? commandLine : commandLine.Substring(0, num)).ToLowerInvariant();
			string text2 = ((num < 0) ? string.Empty : commandLine.Substring(num).Trim());
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num2 <= 1449533269U)
			{
				if (num2 <= 946971642U)
				{
					if (num2 != 689258020U)
					{
						if (num2 == 946971642U)
						{
							if (text == "help")
							{
								this.AppendSourceLine("Available commands:");
								this.AppendSourceLine("  help              show this help");
								this.AppendSourceLine("  neofetch          system info");
								this.AppendSourceLine("  ls                list installed apps");
								this.AppendSourceLine("  launch <app>      open an app by id (e.g. launch terminal)");
								this.AppendSourceLine("  whoami            print current user");
								this.AppendSourceLine("  echo <text>       print text");
								this.AppendSourceLine("  clear             clear the screen");
								goto IL_0301;
							}
						}
					}
					else if (text == "sudo")
					{
						this.AppendSourceLine("Nice try. You're already root in your own head, Ciel.");
						goto IL_0301;
					}
				}
				else if (num2 != 1335831723U)
				{
					if (num2 != 1446109160U)
					{
						if (num2 == 1449533269U)
						{
							if (text == "unlock")
							{
								this.BeginUnlock(text2);
								goto IL_0301;
							}
						}
					}
					else if (text == "ls")
					{
						this.ListApps();
						goto IL_0301;
					}
				}
				else if (text == "hello")
				{
					this.AppendSourceLine("hi <3");
					goto IL_0301;
				}
			}
			else
			{
				if (num2 > 3131618372U)
				{
					if (num2 != 3546203337U)
					{
						if (num2 != 3567113348U)
						{
							if (num2 != 4055382118U)
							{
								goto IL_02EC;
							}
							if (!(text == "launch"))
							{
								goto IL_02EC;
							}
						}
						else
						{
							if (!(text == "echo"))
							{
								goto IL_02EC;
							}
							this.AppendLine(text2);
							goto IL_0301;
						}
					}
					else if (!(text == "open"))
					{
						goto IL_02EC;
					}
					this.LaunchApp(text2);
					goto IL_0301;
				}
				if (num2 != 1550717474U)
				{
					if (num2 != 3127345223U)
					{
						if (num2 == 3131618372U)
						{
							if (text == "whoami")
							{
								this.AppendLine(this.user);
								goto IL_0301;
							}
						}
					}
					else if (text == "neofetch")
					{
						this.AppendLine("  " + this.user + "@" + this.host);
						this.AppendLine("  -----------------");
						this.AppendSourceLine("  OS: CielOS 1.0 (Kali-flavoured)");
						this.AppendSourceLine("  Host: Ciel's Battlestation");
						this.AppendSourceLine("  Shell: cielsh 1.0");
						this.AppendSourceLine("  WM: CielWM");
						this.AppendSourceLine("  CPU: VirtuCore i9 (vibes)");
						goto IL_0301;
					}
				}
				else if (text == "clear")
				{
					this._outputLines.Clear();
					this._buffer.Clear();
					this.RenderBuffer();
					return;
				}
			}
			IL_02EC:
			this.AppendFormatLine("cielsh: command not found: {0}", new object[] { text });
			IL_0301:
			this.AppendLine(string.Empty);
		}

		// Token: 0x06009E97 RID: 40599 RVA: 0x004143CC File Offset: 0x004125CC
		private void BeginUnlock(string argument)
		{
			if (!string.IsNullOrEmpty(argument))
			{
				this.AppendSourceLine("Type unlock by itself, then enter the password in the hidden field.");
				return;
			}
			if (!RunModeService.IsAtHome)
			{
				this.AppendSourceLine("Unlock is available only on the home PC.");
				return;
			}
			if (Time.realtimeSinceStartup < CielOSTerminalApp._nextUnlockAttempt)
			{
				this.AppendSourceLine("Please wait a few seconds before trying again.");
				return;
			}
			this._unlockPrompt = CielOSTerminalApp.UnlockPrompt.Password;
			if (this.input != null)
			{
				this._normalInputType = this.input.inputType;
				this.input.inputType = TMP_InputField.InputType.Password;
				this.input.ForceLabelUpdate();
			}
			this.AppendSourceLine("Password (hidden). Type CANCEL or press Escape to cancel.");
		}

		// Token: 0x06009E98 RID: 40600 RVA: 0x00414460 File Offset: 0x00412660
		private void SubmitUnlockPrompt(string value)
		{
			if (string.Equals(value.Trim(), "cancel", StringComparison.OrdinalIgnoreCase))
			{
				this.ResetUnlockPrompt();
				this.AppendSourceLine("Cancelled.");
				return;
			}
			bool flag = CreatorAccessCredentials.Verify(value);
			this.ResetUnlockPrompt();
			if (!flag)
			{
				CielOSTerminalApp._nextUnlockAttempt = Time.realtimeSinceStartup + 3f;
				this.AppendSourceLine("Access denied. Try again in a few seconds.");
				return;
			}
			string text;
			if (!RankProgressionService.ApplyCreatorBoost(out text))
			{
				this.AppendSourceLine(text);
				return;
			}
			this.AppendSourceLine("Game unlocked.");
		}

		// Token: 0x06009E99 RID: 40601 RVA: 0x004144D8 File Offset: 0x004126D8
		private void ResetUnlockPrompt()
		{
			if (this.input != null)
			{
				this.input.SetTextWithoutNotify(string.Empty);
				if (this._unlockPrompt == CielOSTerminalApp.UnlockPrompt.Password)
				{
					this.input.inputType = this._normalInputType;
				}
				this.input.ForceLabelUpdate();
			}
			this._unlockPrompt = CielOSTerminalApp.UnlockPrompt.None;
		}

		// Token: 0x06009E9A RID: 40602 RVA: 0x00414530 File Offset: 0x00412730
		private void ListApps()
		{
			CielOSAppCatalogSO cielOSAppCatalogSO = ((this._windowManager != null) ? this._windowManager.Catalog : null);
			if (cielOSAppCatalogSO == null)
			{
				this.AppendSourceLine("(no app catalog available)");
				return;
			}
			this.AppendSourceLine("Installed apps (use 'launch <id>'):");
			foreach (CielOSAppDefinition cielOSAppDefinition in cielOSAppCatalogSO.apps)
			{
				if (!(cielOSAppDefinition == null) && !cielOSAppDefinition.hidden && cielOSAppDefinition.IsAvailable)
				{
					string id = cielOSAppDefinition.appId;
					string englishName = cielOSAppDefinition.displayName;
					this.AppendOutput(() => string.Format("  {0,-16} {1}", id, GameLocalization.GetSource("cielos_shell", englishName)));
				}
			}
		}

		// Token: 0x06009E9B RID: 40603 RVA: 0x00414604 File Offset: 0x00412804
		private void LaunchApp(string appId)
		{
			if (string.IsNullOrWhiteSpace(appId))
			{
				this.AppendSourceLine("usage: launch <app-id>");
				return;
			}
			if (this._windowManager == null)
			{
				this.AppendSourceLine("(window manager unavailable)");
				return;
			}
			string text = appId.Trim();
			CielOSWindow cielOSWindow = this._windowManager.LaunchApp(text);
			this.AppendFormatLine((cielOSWindow != null) ? "opening {0}..." : "unknown app: {0}", new object[] { text });
		}

		// Token: 0x06009E9C RID: 40604 RVA: 0x00414678 File Offset: 0x00412878
		private void AppendLine(string line)
		{
			this.AppendOutput(() => line);
		}

		// Token: 0x06009E9D RID: 40605 RVA: 0x004146A4 File Offset: 0x004128A4
		private void AppendSourceLine(string english)
		{
			this.AppendOutput(() => CielOSContentText.Get(english));
		}

		// Token: 0x06009E9E RID: 40606 RVA: 0x004146D0 File Offset: 0x004128D0
		private void AppendFormatLine(string english, params object[] arguments)
		{
			this.AppendOutput(() => CielOSContentText.Format(english, arguments));
		}

		// Token: 0x06009E9F RID: 40607 RVA: 0x00414703 File Offset: 0x00412903
		private void AppendOutput(Func<string> line)
		{
			this._outputLines.Add(line);
			this.RenderBuffer();
		}

		// Token: 0x06009EA0 RID: 40608 RVA: 0x00414718 File Offset: 0x00412918
		private string BuildOutput()
		{
			this._buffer.Clear();
			foreach (Func<string> func in this._outputLines)
			{
				this._buffer.AppendLine(func());
			}
			return this._buffer.ToString();
		}

		// Token: 0x06009EA1 RID: 40609 RVA: 0x00414790 File Offset: 0x00412990
		private void RenderBuffer()
		{
			if (this.output != null)
			{
				this.output.text = this.BuildOutput();
			}
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.ScrollToBottomNextFrame());
			}
		}

		// Token: 0x06009EA2 RID: 40610 RVA: 0x004147C6 File Offset: 0x004129C6
		private IEnumerator ScrollToBottomNextFrame()
		{
			if (this.output != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.output.rectTransform);
			}
			yield return null;
			if (this.scrollRect != null)
			{
				this.scrollRect.verticalNormalizedPosition = 0f;
			}
			yield break;
		}

		// Token: 0x06009EA3 RID: 40611 RVA: 0x004147D5 File Offset: 0x004129D5
		private void FocusInput()
		{
			if (this.input == null)
			{
				return;
			}
			this.input.ActivateInputField();
			this.input.Select();
		}

		// Token: 0x0400A2C4 RID: 41668
		[Header("References")]
		[Tooltip("Scrollable output text (TMP). Appended to as commands run.")]
		[SerializeField]
		private TMP_Text output;

		// Token: 0x0400A2C5 RID: 41669
		[Tooltip("Input field where the player types commands.")]
		[SerializeField]
		private TMP_InputField input;

		// Token: 0x0400A2C6 RID: 41670
		[Tooltip("ScrollRect kept pinned to the bottom as output grows.")]
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x0400A2C7 RID: 41671
		[Header("Identity")]
		[SerializeField]
		private string user = "ciel";

		// Token: 0x0400A2C8 RID: 41672
		[SerializeField]
		private string host = "cielos";

		// Token: 0x0400A2C9 RID: 41673
		private readonly List<string> _history = new List<string>();

		// Token: 0x0400A2CA RID: 41674
		private readonly StringBuilder _buffer = new StringBuilder();

		// Token: 0x0400A2CB RID: 41675
		private readonly List<Func<string>> _outputLines = new List<Func<string>>();

		// Token: 0x0400A2CC RID: 41676
		private CielOSWindowManager _windowManager;

		// Token: 0x0400A2CD RID: 41677
		private bool _greeted;

		// Token: 0x0400A2CE RID: 41678
		private CielOSTerminalApp.UnlockPrompt _unlockPrompt;

		// Token: 0x0400A2CF RID: 41679
		private TMP_InputField.InputType _normalInputType;

		// Token: 0x0400A2D0 RID: 41680
		private static float _nextUnlockAttempt;

		// Token: 0x02001412 RID: 5138
		private enum UnlockPrompt
		{
			// Token: 0x0400A2D2 RID: 41682
			None,
			// Token: 0x0400A2D3 RID: 41683
			Password
		}
	}
}
