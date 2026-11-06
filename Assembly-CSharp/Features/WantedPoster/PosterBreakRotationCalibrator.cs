using System;
using System.IO;
using Features.MainMenu;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.WantedPoster
{
	// Token: 0x02000B6A RID: 2922
	public class PosterBreakRotationCalibrator : MonoBehaviour
	{
		// Token: 0x060052FE RID: 21246 RVA: 0x00203A28 File Offset: 0x00201C28
		private void Update()
		{
			if (this._target == null)
			{
				WantedPosterBreakObject wantedPosterBreakObject = Object.FindObjectOfType<WantedPosterBreakObject>();
				if (wantedPosterBreakObject != null)
				{
					this._target = wantedPosterBreakObject.transform;
				}
			}
			if (this._target == null)
			{
				return;
			}
			float num = (GameplayInput.GetKey(KeyCode.LeftShift) ? 1f : this.step);
			Vector3 localEulerAngles = this._target.localEulerAngles;
			if (GameplayInput.GetKeyDown(KeyCode.I))
			{
				localEulerAngles.x += num;
			}
			if (GameplayInput.GetKeyDown(KeyCode.K))
			{
				localEulerAngles.x -= num;
			}
			if (GameplayInput.GetKeyDown(KeyCode.J))
			{
				localEulerAngles.y += num;
			}
			if (GameplayInput.GetKeyDown(KeyCode.L))
			{
				localEulerAngles.y -= num;
			}
			if (GameplayInput.GetKeyDown(KeyCode.U))
			{
				localEulerAngles.z += num;
			}
			if (GameplayInput.GetKeyDown(KeyCode.O))
			{
				localEulerAngles.z -= num;
			}
			this._target.localEulerAngles = localEulerAngles;
			if (GameplayInput.GetKeyDown(KeyCode.P))
			{
				string text = string.Format("[PosterBreakRotationCalibrator] localEuler = ({0:F1}, {1:F1}, {2:F1})", localEulerAngles.x, localEulerAngles.y, localEulerAngles.z);
				Debug.Log(text);
				try
				{
					File.WriteAllText("Temp/PosterBreakRotation.log", text);
				}
				catch
				{
				}
			}
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x00203B78 File Offset: 0x00201D78
		private void OnGUI()
		{
			if (this._target == null)
			{
				return;
			}
			if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
			{
				return;
			}
			if (this._style == null)
			{
				this._style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 22,
					normal = 
					{
						textColor = Color.yellow
					}
				};
			}
			Vector3 localEulerAngles = this._target.localEulerAngles;
			GUI.Label(new Rect(20f, 20f, 800f, 40f), string.Format("Poster rot localEuler = ({0:F1}, {1:F1}, {2:F1})  [I/K=X J/L=Y U/O=Z, P=stampa]", localEulerAngles.x, localEulerAngles.y, localEulerAngles.z), this._style);
		}

		// Token: 0x04005399 RID: 21401
		[SerializeField]
		private float step = 15f;

		// Token: 0x0400539A RID: 21402
		private Transform _target;

		// Token: 0x0400539B RID: 21403
		private GUIStyle _style;
	}
}
