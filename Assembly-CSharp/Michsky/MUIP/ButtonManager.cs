using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x020001C6 RID: 454
	[ExecuteInEditMode]
	public class ButtonManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
	{
		// Token: 0x06000D6C RID: 3436 RVA: 0x0005CD9A File Offset: 0x0005AF9A
		private void OnEnable()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			this.UpdateUI();
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0005CDB0 File Offset: 0x0005AFB0
		private void OnDisable()
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.disabledCG != null)
			{
				this.disabledCG.alpha = 0f;
			}
			if (this.normalCG != null)
			{
				this.normalCG.alpha = 1f;
			}
			if (this.highlightCG != null)
			{
				this.highlightCG.alpha = 0f;
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0005CE20 File Offset: 0x0005B020
		private void Initialize()
		{
			Animator animator;
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased && base.TryGetComponent<Animator>(out animator))
			{
				Object.Destroy(animator);
			}
			if (base.gameObject.GetComponent<Image>() == null)
			{
				Image image = base.gameObject.AddComponent<Image>();
				image.color = new Color(0f, 0f, 0f, 0f);
				image.raycastTarget = true;
			}
			if (this.targetCanvas == null)
			{
				this.targetCanvas = base.GetComponentInParent<Canvas>();
			}
			if (this.normalCG == null)
			{
				this.normalCG = new GameObject().AddComponent<CanvasGroup>();
				this.normalCG.gameObject.AddComponent<RectTransform>();
				this.normalCG.transform.SetParent(base.transform);
				this.normalCG.gameObject.name = "Normal";
			}
			if (this.highlightCG == null)
			{
				this.highlightCG = new GameObject().AddComponent<CanvasGroup>();
				this.highlightCG.gameObject.AddComponent<RectTransform>();
				this.highlightCG.transform.SetParent(base.transform);
				this.highlightCG.gameObject.name = "Highlight";
			}
			if (this.disabledCG == null)
			{
				this.disabledCG = new GameObject().AddComponent<CanvasGroup>();
				this.disabledCG.gameObject.AddComponent<RectTransform>();
				this.disabledCG.transform.SetParent(base.transform);
				this.disabledCG.gameObject.name = "Disabled";
			}
			if (this.useRipple && this.rippleParent != null)
			{
				this.rippleParent.SetActive(false);
			}
			else if (!this.useRipple && this.rippleParent != null)
			{
				Object.Destroy(this.rippleParent);
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("LayoutFix");
			}
			if (this.targetButton == null)
			{
				if (base.gameObject.GetComponent<Button>() == null)
				{
					this.targetButton = base.gameObject.AddComponent<Button>();
				}
				else
				{
					this.targetButton = base.GetComponent<Button>();
				}
				this.targetButton.transition = Selectable.Transition.None;
				Navigation navigation = default(Navigation);
				navigation.mode = Navigation.Mode.None;
				this.targetButton.navigation = navigation;
			}
			if (this.useUINavigation)
			{
				this.AddUINavigation();
			}
			this.isInitialized = true;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0005D08C File Offset: 0x0005B28C
		public void UpdateUI()
		{
			if (!this.autoFitContent)
			{
				if (this.mainFitter != null)
				{
					this.mainFitter.enabled = false;
				}
				if (this.mainLayout != null)
				{
					this.mainLayout.enabled = false;
				}
				if (this.targetFitter != null)
				{
					this.targetFitter.enabled = false;
					if (this.targetRect != null)
					{
						this.targetRect.anchorMin = new Vector2(0f, 0f);
						this.targetRect.anchorMax = new Vector2(1f, 1f);
						this.targetRect.offsetMin = new Vector2(0f, 0f);
						this.targetRect.offsetMax = new Vector2(0f, 0f);
					}
				}
			}
			else
			{
				if (this.mainFitter != null)
				{
					this.mainFitter.enabled = true;
				}
				if (this.mainLayout != null)
				{
					this.mainLayout.enabled = true;
				}
				if (this.targetFitter != null)
				{
					this.targetFitter.enabled = true;
				}
			}
			if (this.disabledLayout != null)
			{
				this.disabledLayout.padding = new RectOffset(this.padding.left, this.padding.right, this.padding.top, this.padding.bottom);
				this.disabledLayout.spacing = (float)this.spacing;
			}
			if (this.normalLayout != null)
			{
				this.normalLayout.padding = new RectOffset(this.padding.left, this.padding.right, this.padding.top, this.padding.bottom);
				this.normalLayout.spacing = (float)this.spacing;
			}
			if (this.highlightedLayout != null)
			{
				this.highlightedLayout.padding = new RectOffset(this.padding.left, this.padding.right, this.padding.top, this.padding.bottom);
				this.highlightedLayout.spacing = (float)this.spacing;
			}
			if (this.normalCG != null && this.isInteractable)
			{
				this.normalCG.alpha = 1f;
			}
			if (this.disabledCG != null && !this.isInteractable)
			{
				this.disabledCG.alpha = 1f;
			}
			if (this.highlightCG != null)
			{
				this.highlightCG.alpha = 0f;
			}
			if (!this.useCustomContent)
			{
				if (this.enableText)
				{
					if (this.normalText != null)
					{
						this.normalText.gameObject.SetActive(true);
						this.normalText.text = this.buttonText;
						if (!this.useCustomTextSize)
						{
							this.normalText.fontSize = this.textSize;
						}
					}
					if (this.highlightedText != null)
					{
						this.highlightedText.gameObject.SetActive(true);
						this.highlightedText.text = this.buttonText;
						if (!this.useCustomTextSize)
						{
							this.highlightedText.fontSize = this.textSize;
						}
					}
					if (this.disabledText != null)
					{
						this.disabledText.gameObject.SetActive(true);
						this.disabledText.text = this.buttonText;
						if (!this.useCustomTextSize)
						{
							this.disabledText.fontSize = this.textSize;
						}
					}
				}
				else if (!this.enableText)
				{
					if (this.normalText != null)
					{
						this.normalText.gameObject.SetActive(false);
					}
					if (this.highlightedText != null)
					{
						this.highlightedText.gameObject.SetActive(false);
					}
					if (this.disabledText != null)
					{
						this.disabledText.gameObject.SetActive(false);
					}
				}
				if (this.enableIcon)
				{
					Vector3 vector = new Vector3(this.iconScale, this.iconScale, this.iconScale);
					if (this.normalImage != null)
					{
						this.normalImage.transform.parent.gameObject.SetActive(true);
						this.normalImage.sprite = this.buttonIcon;
						this.normalImage.transform.localScale = vector;
					}
					if (this.highlightImage != null)
					{
						this.highlightImage.transform.parent.gameObject.SetActive(true);
						this.highlightImage.sprite = this.buttonIcon;
						this.highlightImage.transform.localScale = vector;
					}
					if (this.disabledImage != null)
					{
						this.disabledImage.transform.parent.gameObject.SetActive(true);
						this.disabledImage.sprite = this.buttonIcon;
						this.disabledImage.transform.localScale = vector;
					}
				}
				else
				{
					if (this.normalImage != null)
					{
						this.normalImage.transform.parent.gameObject.SetActive(false);
					}
					if (this.highlightImage != null)
					{
						this.highlightImage.transform.parent.gameObject.SetActive(false);
					}
					if (this.disabledImage != null)
					{
						this.disabledImage.transform.parent.gameObject.SetActive(false);
					}
				}
			}
			if (!Application.isPlaying || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.isInteractable)
			{
				base.StartCoroutine("SetDisabled");
			}
			else if (this.isInteractable && this.disabledCG.alpha == 1f)
			{
				base.StartCoroutine("SetNormal");
			}
			base.StartCoroutine("LayoutFix");
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0005D679 File Offset: 0x0005B879
		public void SetText(string text)
		{
			this.buttonText = text;
			this.UpdateUI();
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0005D688 File Offset: 0x0005B888
		public void SetIcon(Sprite icon)
		{
			this.buttonIcon = icon;
			this.UpdateUI();
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0005D698 File Offset: 0x0005B898
		public void Interactable(bool value)
		{
			this.isInteractable = value;
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.isInteractable)
			{
				base.StartCoroutine("SetDisabled");
				return;
			}
			if (this.isInteractable && this.disabledCG.alpha == 1f)
			{
				base.StartCoroutine("SetNormal");
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0005D6F8 File Offset: 0x0005B8F8
		public void AddUINavigation()
		{
			if (this.targetButton == null)
			{
				return;
			}
			this.targetButton.transition = Selectable.Transition.None;
			Navigation navigation = new Navigation
			{
				mode = this.navigationMode
			};
			if (this.navigationMode == Navigation.Mode.Vertical || this.navigationMode == Navigation.Mode.Horizontal)
			{
				navigation.wrapAround = this.wrapAround;
			}
			else if (this.navigationMode == Navigation.Mode.Explicit)
			{
				base.StartCoroutine("InitUINavigation", navigation);
				return;
			}
			this.targetButton.navigation = navigation;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0005D780 File Offset: 0x0005B980
		public void CreateRipple(Vector2 pos)
		{
			if (this.rippleParent != null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<Image>();
				gameObject.GetComponent<Image>().sprite = this.rippleShape;
				gameObject.name = "Ripple";
				this.rippleParent.SetActive(true);
				gameObject.transform.SetParent(this.rippleParent.transform);
				if (this.renderOnTop)
				{
					this.rippleParent.transform.SetAsLastSibling();
				}
				else
				{
					this.rippleParent.transform.SetAsFirstSibling();
				}
				if (this.centered)
				{
					gameObject.transform.localPosition = new Vector2(0f, 0f);
				}
				else
				{
					gameObject.transform.position = pos;
				}
				gameObject.AddComponent<Ripple>();
				Ripple component = gameObject.GetComponent<Ripple>();
				component.speed = this.speed;
				component.maxSize = this.maxSize;
				component.startColor = this.startColor;
				component.transitionColor = this.transitionColor;
				if (this.rippleUpdateMode == ButtonManager.RippleUpdateMode.Normal)
				{
					component.unscaledTime = false;
					return;
				}
				component.unscaledTime = true;
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0005D8A4 File Offset: 0x0005BAA4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.isInteractable || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.enableButtonSounds && this.useClickSound && this.soundSource != null)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
			this.onClick.Invoke();
			if (!this.checkForDoubleClick || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.waitingForDoubleClickInput)
			{
				this.onDoubleClick.Invoke();
				this.waitingForDoubleClickInput = false;
				return;
			}
			this.waitingForDoubleClickInput = true;
			base.StopCoroutine("CheckForDoubleClick");
			base.StartCoroutine("CheckForDoubleClick");
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0005D94C File Offset: 0x0005BB4C
		public void OnPointerDown(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.useRipple && this.isPointerOn)
			{
				if (this.targetCanvas != null && (this.targetCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.targetCanvas.renderMode == RenderMode.WorldSpace))
				{
					this.CreateRipple(this.targetCanvas.worldCamera.ScreenToWorldPoint(Input.mousePosition));
					return;
				}
				this.CreateRipple(Input.mousePosition);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0005D9D0 File Offset: 0x0005BBD0
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.enableButtonSounds && this.useHoverSound && this.soundSource != null)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased)
			{
				base.StartCoroutine("SetHighlight");
			}
			this.isPointerOn = true;
			this.onHover.Invoke();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0005DA3C File Offset: 0x0005BC3C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased)
			{
				base.StartCoroutine("SetNormal");
			}
			this.isPointerOn = false;
			this.onLeave.Invoke();
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0005DA6E File Offset: 0x0005BC6E
		public void OnSelect(BaseEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased)
			{
				base.StartCoroutine("SetHighlight");
			}
			if (this.useUINavigation)
			{
				this.onHover.Invoke();
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0005DAA1 File Offset: 0x0005BCA1
		public void OnDeselect(BaseEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased)
			{
				base.StartCoroutine("SetNormal");
			}
			if (this.useUINavigation)
			{
				this.onLeave.Invoke();
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0005DAD4 File Offset: 0x0005BCD4
		public void OnSubmit(BaseEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.animationSolution == ButtonManager.AnimationSolution.ScriptBased)
			{
				base.StartCoroutine("SetNormal");
			}
			this.onClick.Invoke();
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0005DAFF File Offset: 0x0005BCFF
		private IEnumerator LayoutFix()
		{
			yield return new WaitForSecondsRealtime(0.025f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			if (this.disabledCG != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.disabledCG.GetComponent<RectTransform>());
			}
			if (this.normalCG != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.normalCG.GetComponent<RectTransform>());
			}
			if (this.highlightCG != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.highlightCG.GetComponent<RectTransform>());
			}
			yield break;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0005DB0E File Offset: 0x0005BD0E
		private IEnumerator SetNormal()
		{
			base.StopCoroutine("SetHighlight");
			base.StopCoroutine("SetDisabled");
			while (this.normalCG.alpha < 0.99f)
			{
				this.normalCG.alpha += Time.unscaledDeltaTime * this.fadingMultiplier;
				this.highlightCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.disabledCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				yield return null;
			}
			this.normalCG.alpha = 1f;
			this.highlightCG.alpha = 0f;
			this.disabledCG.alpha = 0f;
			yield break;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0005DB1D File Offset: 0x0005BD1D
		private IEnumerator SetHighlight()
		{
			base.StopCoroutine("SetNormal");
			base.StopCoroutine("SetDisabled");
			while (this.highlightCG.alpha < 0.99f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.highlightCG.alpha += Time.unscaledDeltaTime * this.fadingMultiplier;
				this.disabledCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				yield return null;
			}
			this.normalCG.alpha = 0f;
			this.highlightCG.alpha = 1f;
			this.disabledCG.alpha = 0f;
			yield break;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0005DB2C File Offset: 0x0005BD2C
		private IEnumerator SetDisabled()
		{
			base.StopCoroutine("SetNormal");
			base.StopCoroutine("SetHighlight");
			while (this.disabledCG.alpha < 0.99f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.highlightCG.alpha -= Time.unscaledDeltaTime * this.fadingMultiplier;
				this.disabledCG.alpha += Time.unscaledDeltaTime * this.fadingMultiplier;
				yield return null;
			}
			this.normalCG.alpha = 0f;
			this.highlightCG.alpha = 0f;
			this.disabledCG.alpha = 1f;
			yield break;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0005DB3B File Offset: 0x0005BD3B
		private IEnumerator CheckForDoubleClick()
		{
			yield return new WaitForSecondsRealtime(this.doubleClickPeriod);
			this.waitingForDoubleClickInput = false;
			yield break;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0005DB4A File Offset: 0x0005BD4A
		private IEnumerator InitUINavigation(Navigation nav)
		{
			yield return new WaitForSecondsRealtime(1f);
			if (this.selectOnUp != null)
			{
				nav.selectOnUp = this.selectOnUp.GetComponent<Selectable>();
			}
			if (this.selectOnDown != null)
			{
				nav.selectOnDown = this.selectOnDown.GetComponent<Selectable>();
			}
			if (this.selectOnLeft != null)
			{
				nav.selectOnLeft = this.selectOnLeft.GetComponent<Selectable>();
			}
			if (this.selectOnRight != null)
			{
				nav.selectOnRight = this.selectOnRight.GetComponent<Selectable>();
			}
			this.targetButton.navigation = nav;
			yield break;
		}

		// Token: 0x04000DB8 RID: 3512
		public Sprite buttonIcon;

		// Token: 0x04000DB9 RID: 3513
		public string buttonText = "Button";

		// Token: 0x04000DBA RID: 3514
		[Range(0.1f, 10f)]
		public float iconScale = 1f;

		// Token: 0x04000DBB RID: 3515
		[Range(10f, 200f)]
		public float textSize = 24f;

		// Token: 0x04000DBC RID: 3516
		public bool autoFitContent = true;

		// Token: 0x04000DBD RID: 3517
		public ButtonManager.Padding padding;

		// Token: 0x04000DBE RID: 3518
		[Range(0f, 100f)]
		public int spacing = 15;

		// Token: 0x04000DBF RID: 3519
		public HorizontalLayoutGroup disabledLayout;

		// Token: 0x04000DC0 RID: 3520
		public HorizontalLayoutGroup normalLayout;

		// Token: 0x04000DC1 RID: 3521
		public HorizontalLayoutGroup highlightedLayout;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private HorizontalLayoutGroup mainLayout;

		// Token: 0x04000DC3 RID: 3523
		[SerializeField]
		private ContentSizeFitter mainFitter;

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private ContentSizeFitter targetFitter;

		// Token: 0x04000DC5 RID: 3525
		[SerializeField]
		private RectTransform targetRect;

		// Token: 0x04000DC6 RID: 3526
		public CanvasGroup normalCG;

		// Token: 0x04000DC7 RID: 3527
		public CanvasGroup highlightCG;

		// Token: 0x04000DC8 RID: 3528
		public CanvasGroup disabledCG;

		// Token: 0x04000DC9 RID: 3529
		public TextMeshProUGUI normalText;

		// Token: 0x04000DCA RID: 3530
		public TextMeshProUGUI highlightedText;

		// Token: 0x04000DCB RID: 3531
		public TextMeshProUGUI disabledText;

		// Token: 0x04000DCC RID: 3532
		public Image normalImage;

		// Token: 0x04000DCD RID: 3533
		public Image highlightImage;

		// Token: 0x04000DCE RID: 3534
		public Image disabledImage;

		// Token: 0x04000DCF RID: 3535
		public AudioSource soundSource;

		// Token: 0x04000DD0 RID: 3536
		[SerializeField]
		private GameObject rippleParent;

		// Token: 0x04000DD1 RID: 3537
		public bool isInteractable = true;

		// Token: 0x04000DD2 RID: 3538
		public bool enableIcon;

		// Token: 0x04000DD3 RID: 3539
		public bool enableText = true;

		// Token: 0x04000DD4 RID: 3540
		public bool useCustomContent;

		// Token: 0x04000DD5 RID: 3541
		[SerializeField]
		private bool useCustomTextSize;

		// Token: 0x04000DD6 RID: 3542
		public bool checkForDoubleClick = true;

		// Token: 0x04000DD7 RID: 3543
		public bool enableButtonSounds;

		// Token: 0x04000DD8 RID: 3544
		public bool useHoverSound = true;

		// Token: 0x04000DD9 RID: 3545
		public bool useClickSound = true;

		// Token: 0x04000DDA RID: 3546
		public AudioClip hoverSound;

		// Token: 0x04000DDB RID: 3547
		public AudioClip clickSound;

		// Token: 0x04000DDC RID: 3548
		public bool useUINavigation;

		// Token: 0x04000DDD RID: 3549
		public Navigation.Mode navigationMode = Navigation.Mode.Automatic;

		// Token: 0x04000DDE RID: 3550
		public GameObject selectOnUp;

		// Token: 0x04000DDF RID: 3551
		public GameObject selectOnDown;

		// Token: 0x04000DE0 RID: 3552
		public GameObject selectOnLeft;

		// Token: 0x04000DE1 RID: 3553
		public GameObject selectOnRight;

		// Token: 0x04000DE2 RID: 3554
		public bool wrapAround;

		// Token: 0x04000DE3 RID: 3555
		public bool useRipple = true;

		// Token: 0x04000DE4 RID: 3556
		[Range(0.1f, 1f)]
		public float doubleClickPeriod = 0.25f;

		// Token: 0x04000DE5 RID: 3557
		[Range(0.25f, 15f)]
		public float fadingMultiplier = 8f;

		// Token: 0x04000DE6 RID: 3558
		[SerializeField]
		private ButtonManager.AnimationSolution animationSolution = ButtonManager.AnimationSolution.ScriptBased;

		// Token: 0x04000DE7 RID: 3559
		public UnityEvent onClick = new UnityEvent();

		// Token: 0x04000DE8 RID: 3560
		public UnityEvent onDoubleClick = new UnityEvent();

		// Token: 0x04000DE9 RID: 3561
		public UnityEvent onHover = new UnityEvent();

		// Token: 0x04000DEA RID: 3562
		public UnityEvent onLeave = new UnityEvent();

		// Token: 0x04000DEB RID: 3563
		[SerializeField]
		private ButtonManager.RippleUpdateMode rippleUpdateMode = ButtonManager.RippleUpdateMode.UnscaledTime;

		// Token: 0x04000DEC RID: 3564
		[SerializeField]
		private Canvas targetCanvas;

		// Token: 0x04000DED RID: 3565
		public Sprite rippleShape;

		// Token: 0x04000DEE RID: 3566
		[Range(0.1f, 5f)]
		public float speed = 1f;

		// Token: 0x04000DEF RID: 3567
		[Range(0.5f, 25f)]
		public float maxSize = 4f;

		// Token: 0x04000DF0 RID: 3568
		public Color startColor = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04000DF1 RID: 3569
		public Color transitionColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04000DF2 RID: 3570
		[SerializeField]
		private bool renderOnTop;

		// Token: 0x04000DF3 RID: 3571
		[SerializeField]
		private bool centered;

		// Token: 0x04000DF4 RID: 3572
		private bool isInitialized;

		// Token: 0x04000DF5 RID: 3573
		private Button targetButton;

		// Token: 0x04000DF6 RID: 3574
		private bool isPointerOn;

		// Token: 0x04000DF7 RID: 3575
		private bool waitingForDoubleClickInput;

		// Token: 0x04000DF8 RID: 3576
		private const int navHelper = 1;

		// Token: 0x020001C7 RID: 455
		public enum AnimationSolution
		{
			// Token: 0x04000DFA RID: 3578
			Custom,
			// Token: 0x04000DFB RID: 3579
			ScriptBased
		}

		// Token: 0x020001C8 RID: 456
		public enum RippleUpdateMode
		{
			// Token: 0x04000DFD RID: 3581
			Normal,
			// Token: 0x04000DFE RID: 3582
			UnscaledTime
		}

		// Token: 0x020001C9 RID: 457
		[Serializable]
		public class Padding
		{
			// Token: 0x04000DFF RID: 3583
			public int left = 20;

			// Token: 0x04000E00 RID: 3584
			public int right = 20;

			// Token: 0x04000E01 RID: 3585
			public int top = 5;

			// Token: 0x04000E02 RID: 3586
			public int bottom = 5;
		}
	}
}
