using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000029 RID: 41
	public class MMDebugMenuCommands : MonoBehaviour
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x0000555C File Offset: 0x0000375C
		[MMDebugLogCommand]
		public static void Now()
		{
			MMDebug.DebugLogTime("Time.time is " + Time.time.ToString(), "", 3, true);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000558C File Offset: 0x0000378C
		[MMDebugLogCommand]
		public static void Clear()
		{
			MMDebug.DebugLogClear();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005594 File Offset: 0x00003794
		[MMDebugLogCommand]
		public static void Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000055B4 File Offset: 0x000037B4
		[MMDebugLogCommand]
		public static void Reload()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000055D4 File Offset: 0x000037D4
		[MMDebugLogCommand]
		public static void Sysinfo()
		{
			MMDebug.DebugLogTime(MMDebug.GetSystemInfo(), "", 3, true);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000055E7 File Offset: 0x000037E7
		[MMDebugLogCommand]
		public static void Quit()
		{
			MMDebugMenuCommands.InternalQuit();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000055EE File Offset: 0x000037EE
		[MMDebugLogCommand]
		public static void Exit()
		{
			MMDebugMenuCommands.InternalQuit();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000055F8 File Offset: 0x000037F8
		[MMDebugLogCommand]
		public static void Help()
		{
			string text = "LIST OF COMMANDS";
			foreach (MethodInfo methodInfo in MMDebug.Commands.OrderBy<MethodInfo, string>((MethodInfo m) => m.Name))
			{
				text = text + "\n- <color=#FFFFFF>" + methodInfo.Name + "</color>";
			}
			MMDebug.DebugLogTime(text, "#FFC400", 3, true);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000568C File Offset: 0x0000388C
		private static void InternalQuit()
		{
			Application.Quit();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005694 File Offset: 0x00003894
		[MMDebugLogCommandArgumentCount(1)]
		[MMDebugLogCommand]
		public static void Vsync(string[] args)
		{
			int num;
			if (int.TryParse(args[1], out num))
			{
				QualitySettings.vSyncCount = num;
				MMDebug.DebugLogTime("VSyncCount set to " + num.ToString(), "#FFC400", 3, true);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000056D0 File Offset: 0x000038D0
		[MMDebugLogCommandArgumentCount(1)]
		[MMDebugLogCommand]
		public static void Framerate(string[] args)
		{
			int num;
			if (int.TryParse(args[1], out num))
			{
				Application.targetFrameRate = num;
				MMDebug.DebugLogTime("Framerate set to " + num.ToString(), "#FFC400", 3, true);
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000570C File Offset: 0x0000390C
		[MMDebugLogCommandArgumentCount(1)]
		[MMDebugLogCommand]
		public static void Timescale(string[] args)
		{
			float num;
			if (float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				Time.timeScale = num;
				MMDebug.DebugLogTime("Timescale set to " + num.ToString(), "#FFC400", 3, true);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005754 File Offset: 0x00003954
		[MMDebugLogCommandArgumentCount(2)]
		[MMDebugLogCommand]
		public static void Biggest(string[] args)
		{
			int num;
			int num2;
			if (int.TryParse(args[1], out num) && int.TryParse(args[2], out num2))
			{
				MMDebug.DebugLogTime(((num >= num2) ? num : num2).ToString() + " is the biggest number", "#FFC400", 3, true);
			}
		}
	}
}
