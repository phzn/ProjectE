using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x0200140F RID: 5135
	public class CielOSSnakeApp : MonoBehaviour
	{
		// Token: 0x06009E74 RID: 40564 RVA: 0x00412BEC File Offset: 0x00410DEC
		private void Awake()
		{
			this.ResolveTheme();
			this._highScore = PlayerPrefs.GetInt("CielOS.Snake.HighScore", 0);
			this._rngState = (int)(Time.realtimeSinceStartup * 1000f) | 1;
			this.BuildUI();
			this.BuildTexture();
			this.ResetGame();
			this.Render();
		}

		// Token: 0x06009E75 RID: 40565 RVA: 0x00412C3C File Offset: 0x00410E3C
		private void BuildUI()
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = (RectTransform)base.transform;
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.GetComponent<Image>();
			if (image == null)
			{
				image = rectTransform.gameObject.AddComponent<Image>();
			}
			image.color = ((active != null) ? new Color(0.08f, 0.06f, 0.12f, 1f) : this._bg);
			Color color = ((active != null) ? active.controlText : Color.white);
			Color color2 = ((active != null) ? active.accent : this._snakeHead);
			Color color3 = ((active != null) ? new Color(active.controlBackground.r, active.controlBackground.g, active.controlBackground.b, 0.94f) : new Color(0.16f, 0.12f, 0.21f, 0.94f));
			Color color4 = new Color(color.r, color.g, color.b, 0.62f);
			RectTransform rectTransform2 = this.NewChild("TopBar", rectTransform);
			CielOSSnakeApp.SetAnchors(rectTransform2, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -64f), new Vector2(-16f, -12f));
			Image image2 = rectTransform2.gameObject.AddComponent<Image>();
			image2.color = color3;
			image2.raycastTarget = false;
			CielOSTheme.Round(image2);
			CielOSTheme.AddBorder(rectTransform2, new Color?((active != null) ? active.outline : new Color(0.78f, 0.62f, 0.9f, 0.22f)), 1f);
			CielOSSnakeApp.SetAnchors((RectTransform)this.MakeLabel(rectTransform2, "SCORE", 10f, TextAlignmentOptions.TopLeft, color4).transform, Vector2.zero, new Vector2(0.5f, 1f), new Vector2(16f, 8f), new Vector2(-8f, -8f));
			this._scoreLabel = this.MakeLabel(rectTransform2, "0", 23f, TextAlignmentOptions.BottomLeft, color);
			this._scoreLabel.fontStyle = FontStyles.Bold;
			CielOSSnakeApp.SetAnchors((RectTransform)this._scoreLabel.transform, Vector2.zero, new Vector2(0.5f, 1f), new Vector2(16f, 7f), new Vector2(-8f, -18f));
			CielOSSnakeApp.SetAnchors((RectTransform)this.MakeLabel(rectTransform2, "BEST", 10f, TextAlignmentOptions.TopRight, color4).transform, new Vector2(0.5f, 0f), Vector2.one, new Vector2(8f, 8f), new Vector2(-16f, -8f));
			this._highLabel = this.MakeLabel(rectTransform2, this._highScore.ToString(), 23f, TextAlignmentOptions.BottomRight, color2);
			this._highLabel.fontStyle = FontStyles.Bold;
			CielOSSnakeApp.SetAnchors((RectTransform)this._highLabel.transform, new Vector2(0.5f, 0f), Vector2.one, new Vector2(8f, 7f), new Vector2(-16f, -18f));
			RectTransform rectTransform3 = this.NewChild("Divider", rectTransform2);
			CielOSSnakeApp.SetAnchors(rectTransform3, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(-0.5f, 10f), new Vector2(0.5f, -10f));
			Image image3 = rectTransform3.gameObject.AddComponent<Image>();
			image3.color = ((active != null) ? new Color(active.outline.r, active.outline.g, active.outline.b, 0.36f) : new Color(0.78f, 0.62f, 0.9f, 0.18f));
			image3.raycastTarget = false;
			RectTransform rectTransform4 = this.NewChild("BoardArea", rectTransform);
			CielOSSnakeApp.SetAnchors(rectTransform4, Vector2.zero, Vector2.one, new Vector2(16f, 54f), new Vector2(-16f, -74f));
			RectTransform rectTransform5 = this.NewChild("Board", rectTransform4);
			rectTransform5.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform5.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform5.pivot = new Vector2(0.5f, 0.5f);
			AspectRatioFitter aspectRatioFitter = rectTransform5.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			aspectRatioFitter.aspectRatio = 1f;
			Image image4 = rectTransform5.gameObject.AddComponent<Image>();
			image4.color = color3;
			image4.raycastTarget = false;
			CielOSTheme.Round(image4);
			CielOSTheme.AddBorder(rectTransform5, new Color?(color2), 2f);
			RectTransform rectTransform6 = this.NewChild("Grid", rectTransform5);
			CielOSSnakeApp.SetAnchors(rectTransform6, Vector2.zero, Vector2.one, new Vector2(7f, 7f), new Vector2(-7f, -7f));
			this._board = rectTransform6.gameObject.AddComponent<RawImage>();
			this._board.raycastTarget = false;
			RectTransform rectTransform7 = this.NewChild("Status", rectTransform);
			CielOSSnakeApp.SetAnchors(rectTransform7, Vector2.zero, new Vector2(1f, 0f), new Vector2(16f, 12f), new Vector2(-16f, 44f));
			Image image5 = rectTransform7.gameObject.AddComponent<Image>();
			image5.color = color3;
			image5.raycastTarget = false;
			CielOSTheme.Round(image5);
			RectTransform rectTransform8 = this.NewChild("Label", rectTransform7);
			CielOSSnakeApp.SetAnchors(rectTransform8, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			this._statusLabel = rectTransform8.gameObject.AddComponent<TextMeshProUGUI>();
			this._statusLabel.text = "ARROWS / WASD TO MOVE   •   SPACE TO START";
			this._statusLabel.fontSize = 12f;
			this._statusLabel.enableAutoSizing = true;
			this._statusLabel.fontSizeMin = 9f;
			this._statusLabel.fontSizeMax = 12f;
			this._statusLabel.alignment = TextAlignmentOptions.Center;
			this._statusLabel.enableWordWrapping = false;
			this._statusLabel.overflowMode = TextOverflowModes.Ellipsis;
			this._statusLabel.margin = new Vector4(10f, 0f, 10f, 0f);
			this._statusLabel.color = color4;
			if (active != null && active.uiFont != null)
			{
				this._statusLabel.font = active.uiFont;
			}
			this.BindStatus();
		}

		// Token: 0x06009E76 RID: 40566 RVA: 0x00413378 File Offset: 0x00411578
		private TMP_Text MakeLabel(Transform parent, string text, float size, TextAlignmentOptions align, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = this.NewChild(text, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.alignment = align;
			textMeshProUGUI.color = color;
			textMeshProUGUI.enableWordWrapping = false;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				textMeshProUGUI.font = CielOSTheme.Active.uiFont;
			}
			if (text == "SCORE" || text == "BEST")
			{
				CielOSContentText.Bind(textMeshProUGUI, text);
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x06009E77 RID: 40567 RVA: 0x003FDF19 File Offset: 0x003FC119
		private RectTransform NewChild(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06009E78 RID: 40568 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetAnchors(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rt.anchorMin = anchorMin;
			rt.anchorMax = anchorMax;
			rt.offsetMin = offsetMin;
			rt.offsetMax = offsetMax;
		}

		// Token: 0x06009E79 RID: 40569 RVA: 0x00413418 File Offset: 0x00411618
		private void ResolveTheme()
		{
			CielOSTheme active = CielOSTheme.Active;
			if (active == null)
			{
				return;
			}
			this._snakeHead = active.accent;
			this._snakeBody = new Color(active.accent.r * 0.7f, active.accent.g * 0.7f, active.accent.b * 0.7f, 1f);
		}

		// Token: 0x06009E7A RID: 40570 RVA: 0x00413484 File Offset: 0x00411684
		private void BuildTexture()
		{
			this.gridSize = Mathf.Max(5, this.gridSize);
			this.cellPixels = Mathf.Max(8, this.cellPixels);
			int num = this.gridSize * this.cellPixels;
			this._tex = new Texture2D(num, num, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Point
			};
			this._pixels = new Color32[num * num];
			if (this._board != null)
			{
				this._board.texture = this._tex;
			}
		}

		// Token: 0x06009E7B RID: 40571 RVA: 0x00413506 File Offset: 0x00411706
		private void OnDestroy()
		{
			if (this._tex != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._tex);
				}
				else
				{
					Object.DestroyImmediate(this._tex);
				}
			}
			this._tex = null;
			this._pixels = null;
		}

		// Token: 0x06009E7C RID: 40572 RVA: 0x00413544 File Offset: 0x00411744
		private void ResetGame()
		{
			this._snake.Clear();
			int num = this.gridSize / 2;
			this._snake.Add(new Vector2Int(num, num));
			this._snake.Add(new Vector2Int(num - 1, num));
			this._snake.Add(new Vector2Int(num - 2, num));
			this._dir = Vector2Int.right;
			this._pendingDir = Vector2Int.right;
			this._score = 0;
			this._stepTimer = 0f;
			this.SpawnFood();
			this.UpdateScoreLabels();
		}

		// Token: 0x06009E7D RID: 40573 RVA: 0x004135D3 File Offset: 0x004117D3
		private void StartGame()
		{
			this.ResetGame();
			this._state = CielOSSnakeApp.State.Playing;
			this.SetStatus("ARROWS / WASD TO MOVE", Array.Empty<object>());
			this.Render();
		}

		// Token: 0x06009E7E RID: 40574 RVA: 0x004135F8 File Offset: 0x004117F8
		private void Update()
		{
			bool isActive = ControllerInput.IsActive;
			if (this._statusController != isActive)
			{
				this._statusController = isActive;
				this.BindStatus();
			}
			this.ReadDirectionInput();
			if (this._state != CielOSSnakeApp.State.Playing)
			{
				if (GameplayInput.GetKeyDown(KeyCode.Space) || GameplayInput.GetKeyDown(KeyCode.Return) || GameplayInput.GetKeyDown(KeyCode.KeypadEnter))
				{
					this.StartGame();
				}
				return;
			}
			this._stepTimer += Time.unscaledDeltaTime;
			float num = Mathf.Max(0.06f, this.stepInterval - (float)this._score * 0.003f);
			if (this._stepTimer >= num)
			{
				this._stepTimer -= num;
				this.Step();
			}
		}

		// Token: 0x06009E7F RID: 40575 RVA: 0x004136A4 File Offset: 0x004118A4
		private void ReadDirectionInput()
		{
			if (GameplayInput.GetKeyDown(KeyCode.UpArrow) || GameplayInput.GetKeyDown(KeyCode.W))
			{
				this.TrySetDir(Vector2Int.up);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.DownArrow) || GameplayInput.GetKeyDown(KeyCode.S))
			{
				this.TrySetDir(Vector2Int.down);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.LeftArrow) || GameplayInput.GetKeyDown(KeyCode.A))
			{
				this.TrySetDir(Vector2Int.left);
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.RightArrow) || GameplayInput.GetKeyDown(KeyCode.D))
			{
				this.TrySetDir(Vector2Int.right);
			}
		}

		// Token: 0x06009E80 RID: 40576 RVA: 0x00413734 File Offset: 0x00411934
		public void HandleControllerInput(Vector2 direction, bool submit)
		{
			Vector2 vector = ControllerUINavigation.Cardinal(direction);
			if (vector != Vector2.zero)
			{
				this.TrySetDir(new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)));
			}
			if (submit && this._state != CielOSSnakeApp.State.Playing)
			{
				this.StartGame();
			}
			if (!this._statusController)
			{
				this._statusController = true;
				this.BindStatus();
			}
		}

		// Token: 0x06009E81 RID: 40577 RVA: 0x0041379D File Offset: 0x0041199D
		private void TrySetDir(Vector2Int d)
		{
			if (d == -this._dir)
			{
				return;
			}
			this._pendingDir = d;
		}

		// Token: 0x06009E82 RID: 40578 RVA: 0x004137BC File Offset: 0x004119BC
		private void Step()
		{
			this._dir = this._pendingDir;
			Vector2Int vector2Int = this._snake[0] + this._dir;
			vector2Int.x = (vector2Int.x + this.gridSize) % this.gridSize;
			vector2Int.y = (vector2Int.y + this.gridSize) % this.gridSize;
			bool flag = vector2Int == this._food;
			int num = (flag ? this._snake.Count : (this._snake.Count - 1));
			for (int i = 0; i < num; i++)
			{
				if (this._snake[i] == vector2Int)
				{
					this.Die();
					return;
				}
			}
			this._snake.Insert(0, vector2Int);
			if (flag)
			{
				this._score += 10;
				CielOSSfx.PlayPickup();
				this.UpdateScoreLabels();
				this.SpawnFood();
			}
			else
			{
				this._snake.RemoveAt(this._snake.Count - 1);
			}
			this.Render();
		}

		// Token: 0x06009E83 RID: 40579 RVA: 0x004138C8 File Offset: 0x00411AC8
		private void Die()
		{
			this._state = CielOSSnakeApp.State.Dead;
			if (this._score > this._highScore)
			{
				this._highScore = this._score;
				PlayerPrefs.SetInt("CielOS.Snake.HighScore", this._highScore);
				PlayerPrefs.Save();
				this.UpdateScoreLabels();
				this.SetStatus("NEW BEST: {0}!   •   SPACE TO PLAY AGAIN", new object[] { this._score });
			}
			else
			{
				this.SetStatus("GAME OVER   •   SCORE {0}   •   SPACE TO PLAY AGAIN", new object[] { this._score });
			}
			this.Render();
		}

		// Token: 0x06009E84 RID: 40580 RVA: 0x00413958 File Offset: 0x00411B58
		private void SpawnFood()
		{
			int num = this.gridSize * this.gridSize - this._snake.Count;
			if (num <= 0)
			{
				this._food = new Vector2Int(-1, -1);
				return;
			}
			int num2 = this.NextRandom(num);
			int num3 = 0;
			for (int i = 0; i < this.gridSize; i++)
			{
				for (int j = 0; j < this.gridSize; j++)
				{
					Vector2Int vector2Int = new Vector2Int(j, i);
					if (!this._snake.Contains(vector2Int))
					{
						if (num3 == num2)
						{
							this._food = vector2Int;
							return;
						}
						num3++;
					}
				}
			}
		}

		// Token: 0x06009E85 RID: 40581 RVA: 0x004139EC File Offset: 0x00411BEC
		private int NextRandom(int maxExclusive)
		{
			uint num = (uint)this._rngState;
			num ^= num << 13;
			num ^= num >> 17;
			num ^= num << 5;
			this._rngState = (int)num;
			return (int)(num % (uint)maxExclusive);
		}

		// Token: 0x06009E86 RID: 40582 RVA: 0x00413A20 File Offset: 0x00411C20
		private void UpdateScoreLabels()
		{
			if (this._scoreLabel != null)
			{
				this._scoreLabel.text = this._score.ToString();
			}
			if (this._highLabel != null)
			{
				this._highLabel.text = this._highScore.ToString();
			}
		}

		// Token: 0x06009E87 RID: 40583 RVA: 0x00413A75 File Offset: 0x00411C75
		private void SetStatus(string msg, params object[] arguments)
		{
			this._statusEnglish = msg;
			this._statusArguments = arguments;
			this.BindStatus();
		}

		// Token: 0x06009E88 RID: 40584 RVA: 0x00413A8B File Offset: 0x00411C8B
		private void BindStatus()
		{
			if (this._statusLabel != null)
			{
				LocalizedGameText.BindComputed(this._statusLabel, new Func<string>(this.StatusPresentation)).FitTranslationToRectangle(false);
			}
		}

		// Token: 0x06009E89 RID: 40585 RVA: 0x00413AB8 File Offset: 0x00411CB8
		private string StatusPresentation()
		{
			if (this._statusController)
			{
				return CielOSContentText.Get((this._state == CielOSSnakeApp.State.Playing) ? "LS / D-PAD: MOVE   •   B: BACK" : "A: START / RESTART   •   B: BACK");
			}
			if (this._statusArguments.Length != 0)
			{
				return CielOSContentText.Format(this._statusEnglish, this._statusArguments);
			}
			return CielOSContentText.Get(this._statusEnglish);
		}

		// Token: 0x06009E8A RID: 40586 RVA: 0x00413B10 File Offset: 0x00411D10
		private void Render()
		{
			if (this._tex == null)
			{
				return;
			}
			int width = this._tex.width;
			if (this._pixels == null || this._pixels.Length != width * width)
			{
				this._pixels = new Color32[width * width];
			}
			Color32[] pixels = this._pixels;
			Color32 color = this._bg;
			Color32 color2 = this._grid;
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}
			for (int j = 0; j <= this.gridSize; j++)
			{
				int num = Mathf.Min(j * this.cellPixels, width - 1);
				for (int k = 0; k < width; k++)
				{
					pixels[k * width + num] = color2;
					pixels[num * width + k] = color2;
				}
			}
			if (this._food.x >= 0)
			{
				this.FillCell(pixels, width, this._food, this._foodColor, 3);
			}
			for (int l = this._snake.Count - 1; l >= 0; l--)
			{
				Color color3 = ((l == 0) ? this._snakeHead : this._snakeBody);
				this.FillCell(pixels, width, this._snake[l], color3, 1);
			}
			this._tex.SetPixels32(pixels);
			this._tex.Apply(false, false);
		}

		// Token: 0x06009E8B RID: 40587 RVA: 0x00413C70 File Offset: 0x00411E70
		private void FillCell(Color32[] px, int dim, Vector2Int cell, Color color, int pad)
		{
			int num = cell.x * this.cellPixels + pad;
			int num2 = cell.y * this.cellPixels + pad;
			int num3 = (cell.x + 1) * this.cellPixels - pad;
			int num4 = (cell.y + 1) * this.cellPixels - pad;
			Color32 color2 = color;
			for (int i = num2; i < num4; i++)
			{
				if (i >= 0 && i < dim)
				{
					int num5 = i * dim;
					for (int j = num; j < num3; j++)
					{
						if (j >= 0 && j < dim)
						{
							px[num5 + j] = color2;
						}
					}
				}
			}
		}

		// Token: 0x0400A2A5 RID: 41637
		private const string HighScoreKey = "CielOS.Snake.HighScore";

		// Token: 0x0400A2A6 RID: 41638
		[Header("Board")]
		[SerializeField]
		private int gridSize = 17;

		// Token: 0x0400A2A7 RID: 41639
		[SerializeField]
		private int cellPixels = 24;

		// Token: 0x0400A2A8 RID: 41640
		[SerializeField]
		private float stepInterval = 0.14f;

		// Token: 0x0400A2A9 RID: 41641
		private CielOSSnakeApp.State _state;

		// Token: 0x0400A2AA RID: 41642
		private readonly List<Vector2Int> _snake = new List<Vector2Int>();

		// Token: 0x0400A2AB RID: 41643
		private Vector2Int _dir = Vector2Int.right;

		// Token: 0x0400A2AC RID: 41644
		private Vector2Int _pendingDir = Vector2Int.right;

		// Token: 0x0400A2AD RID: 41645
		private Vector2Int _food;

		// Token: 0x0400A2AE RID: 41646
		private float _stepTimer;

		// Token: 0x0400A2AF RID: 41647
		private int _score;

		// Token: 0x0400A2B0 RID: 41648
		private int _highScore;

		// Token: 0x0400A2B1 RID: 41649
		private int _rngState;

		// Token: 0x0400A2B2 RID: 41650
		private Texture2D _tex;

		// Token: 0x0400A2B3 RID: 41651
		private Color32[] _pixels;

		// Token: 0x0400A2B4 RID: 41652
		private RawImage _board;

		// Token: 0x0400A2B5 RID: 41653
		private TMP_Text _scoreLabel;

		// Token: 0x0400A2B6 RID: 41654
		private TMP_Text _highLabel;

		// Token: 0x0400A2B7 RID: 41655
		private TMP_Text _statusLabel;

		// Token: 0x0400A2B8 RID: 41656
		private string _statusEnglish = "ARROWS / WASD TO MOVE   •   SPACE TO START";

		// Token: 0x0400A2B9 RID: 41657
		private object[] _statusArguments = Array.Empty<object>();

		// Token: 0x0400A2BA RID: 41658
		private bool _statusController;

		// Token: 0x0400A2BB RID: 41659
		private Color _bg = new Color(0.07f, 0.05f, 0.1f, 1f);

		// Token: 0x0400A2BC RID: 41660
		private Color _grid = new Color(0.13f, 0.1f, 0.18f, 1f);

		// Token: 0x0400A2BD RID: 41661
		private Color _snakeHead = new Color(0.78f, 0.55f, 0.95f, 1f);

		// Token: 0x0400A2BE RID: 41662
		private Color _snakeBody = new Color(0.55f, 0.32f, 0.72f, 1f);

		// Token: 0x0400A2BF RID: 41663
		private Color _foodColor = new Color(0.95f, 0.45f, 0.55f, 1f);

		// Token: 0x02001410 RID: 5136
		private enum State
		{
			// Token: 0x0400A2C1 RID: 41665
			Ready,
			// Token: 0x0400A2C2 RID: 41666
			Playing,
			// Token: 0x0400A2C3 RID: 41667
			Dead
		}
	}
}
