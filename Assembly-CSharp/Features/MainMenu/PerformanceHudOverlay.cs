using System;
using Features.DebugTools;
using Features.UI.TimedEffects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.MainMenu
{
	// Token: 0x020010AF RID: 4271
	[DefaultExecutionOrder(30000)]
	public sealed class PerformanceHudOverlay : MonoBehaviour
	{
		// Token: 0x0600815B RID: 33115 RVA: 0x0034DE90 File Offset: 0x0034C090
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (Object.FindObjectOfType<PerformanceHudOverlay>() != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PerformanceHUD");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<PerformanceHudOverlay>();
		}

		// Token: 0x0600815C RID: 33116 RVA: 0x0034DEB8 File Offset: 0x0034C0B8
		private void Update()
		{
			float num = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
			float num2 = 1f / num;
			this._smoothedFps = ((this._smoothedFps <= 0f) ? num2 : Mathf.Lerp(this._smoothedFps, num2, 0.08f));
			if (Time.unscaledTime >= this._nextRefresh)
			{
				this._nextRefresh = Time.unscaledTime + 0.25f;
				this._displayFps = this._smoothedFps;
			}
		}

		// Token: 0x0600815D RID: 33117 RVA: 0x0034DF30 File Offset: 0x0034C130
		private void OnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (!GameSettingsService.ShowFps || FlyCamera.IsActive || FlyCameraPhotoMode.AnyOpen)
			{
				return;
			}
			bool flag = SceneManager.GetActiveScene().name == "MainMenu";
			if (!flag && !GameSettingsService.ShowGameplayUI)
			{
				return;
			}
			this.EnsureStyles();
			int num = Mathf.RoundToInt(this._displayFps);
			if (num != this._labelFps)
			{
				this._labelFps = num;
				this._fpsContent.text = num.ToString() + " FPS";
				this._labelSize = this._style.CalcSize(this._fpsContent);
			}
			Vector2 labelSize = this._labelSize;
			TextAnchor textAnchor = (flag ? TextAnchor.UpperRight : TextAnchor.UpperLeft);
			this._style.alignment = textAnchor;
			this._shadow.alignment = textAnchor;
			Rect safeArea = Screen.safeArea;
			float num2 = labelSize.x + 12f;
			float num3 = labelSize.y + 6f;
			float num4 = (float)Screen.height - safeArea.yMax + 18f;
			float num5 = (flag ? (safeArea.xMax - labelSize.x - 22f) : (safeArea.x + Mathf.Clamp(safeArea.width * 0.19f, 210f, 360f)));
			Rect rect = new Rect(num5, num4, num2, num3);
			TimedEffectHUDService instance = TimedEffectHUDService.Instance;
			Rect rect2;
			if (instance != null && instance.TryGetVisibleGuiRect(out rect2))
			{
				rect = PerformanceHudOverlay.AvoidTimedEffectOverlap(rect, rect2);
			}
			GUI.Label(new Rect(rect.x + 2f, rect.y + 2f, rect.width, rect.height), this._fpsContent, this._shadow);
			this._style.normal.textColor = ((this._displayFps < 30f) ? new Color(1f, 0.35f, 0.42f) : ((this._displayFps < 55f) ? new Color(1f, 0.78f, 0.3f) : new Color(0.95f, 0.84f, 0.95f)));
			GUI.Label(rect, this._fpsContent, this._style);
		}

		// Token: 0x0600815E RID: 33118 RVA: 0x0034E16C File Offset: 0x0034C36C
		private static Rect AvoidTimedEffectOverlap(Rect fpsRect, Rect effectRect)
		{
			Rect rect = new Rect(fpsRect.x, fpsRect.y, fpsRect.width + 2f, fpsRect.height + 2f);
			if (rect.Overlaps(effectRect))
			{
				fpsRect.y = effectRect.yMax + 8f;
			}
			return fpsRect;
		}

		// Token: 0x0600815F RID: 33119 RVA: 0x0034E1C8 File Offset: 0x0034C3C8
		private void EnsureStyles()
		{
			if (this._style != null)
			{
				return;
			}
			this._style = new GUIStyle(GUI.skin.label)
			{
				fontSize = 18,
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.UpperRight,
				padding = new RectOffset(6, 6, 3, 3)
			};
			this._shadow = new GUIStyle(this._style);
			this._shadow.normal.textColor = new Color(0f, 0f, 0f, 0.8f);
		}

		// Token: 0x040084A3 RID: 33955
		private float _smoothedFps;

		// Token: 0x040084A4 RID: 33956
		private float _displayFps;

		// Token: 0x040084A5 RID: 33957
		private float _nextRefresh;

		// Token: 0x040084A6 RID: 33958
		private GUIStyle _style;

		// Token: 0x040084A7 RID: 33959
		private GUIStyle _shadow;

		// Token: 0x040084A8 RID: 33960
		private readonly GUIContent _fpsContent = new GUIContent();

		// Token: 0x040084A9 RID: 33961
		private int _labelFps = int.MinValue;

		// Token: 0x040084AA RID: 33962
		private Vector2 _labelSize;
	}
}
