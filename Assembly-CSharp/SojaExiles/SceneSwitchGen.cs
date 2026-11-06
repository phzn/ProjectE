using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SojaExiles
{
	// Token: 0x02000427 RID: 1063
	public class SceneSwitchGen : MonoBehaviour
	{
		// Token: 0x06001AE3 RID: 6883 RVA: 0x00004F31 File Offset: 0x00003131
		private void Start()
		{
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000A8688 File Offset: 0x000A6888
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SceneManager.LoadScene("Structure_01");
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SceneManager.LoadScene("Structure_02");
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SceneManager.LoadScene("Structure_03");
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				SceneManager.LoadScene("Structure_04");
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				SceneManager.LoadScene("Structure_05");
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				SceneManager.LoadScene("Structure_06");
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				SceneManager.LoadScene("Props Furniture Showcase");
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
