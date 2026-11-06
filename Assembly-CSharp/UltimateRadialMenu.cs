using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000153 RID: 339
[ExecuteAlways]
[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("Ultimate Radial Menu/Ultimate Radial Menu")]
public class UltimateRadialMenu : MonoBehaviour
{
	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00048422 File Offset: 0x00046622
	// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0004842A File Offset: 0x0004662A
	public Canvas ParentCanvas { get; private set; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00048433 File Offset: 0x00046633
	// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0004843B File Offset: 0x0004663B
	public bool IsWorldSpaceRadialMenu { get; private set; }

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00048444 File Offset: 0x00046644
	public float GetAnglePerButton
	{
		get
		{
			return this.overallAngle / (float)this.UltimateRadialButtonList.Count;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00048459 File Offset: 0x00046659
	// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00048461 File Offset: 0x00046661
	public float GetCurrentInputAngle { get; private set; }

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0004846A File Offset: 0x0004666A
	// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x00048472 File Offset: 0x00046672
	public float GetCurrentInputDistance { get; private set; }

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0004847B File Offset: 0x0004667B
	// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x00048483 File Offset: 0x00046683
	public float CalculatedMinRange { get; private set; }

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0004848C File Offset: 0x0004668C
	// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00048494 File Offset: 0x00046694
	public float CalculatedMaxRange { get; private set; }

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0004849D File Offset: 0x0004669D
	// (set) Token: 0x06000ABC RID: 2748 RVA: 0x000484A5 File Offset: 0x000466A5
	public RectTransform BaseTransform { get; private set; }

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000484AE File Offset: 0x000466AE
	public Vector3 BasePosition
	{
		get
		{
			if (this.BaseTransform == null)
			{
				return Vector3.zero;
			}
			return this.BaseTransform.position;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000484CF File Offset: 0x000466CF
	// (set) Token: 0x06000ABF RID: 2751 RVA: 0x000484D7 File Offset: 0x000466D7
	public int CurrentButtonIndex { get; private set; }

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000484E0 File Offset: 0x000466E0
	// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x000484E8 File Offset: 0x000466E8
	public bool IsEnabled { get; private set; }

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x000484F1 File Offset: 0x000466F1
	// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x000484F9 File Offset: 0x000466F9
	public bool InputInRange { get; private set; }

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00048502 File Offset: 0x00046702
	// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x0004850A File Offset: 0x0004670A
	public bool Interactable { get; set; }

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00048513 File Offset: 0x00046713
	// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0004851B File Offset: 0x0004671B
	public bool PointerActive { get; private set; }

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000AC8 RID: 2760 RVA: 0x00048524 File Offset: 0x00046724
	// (remove) Token: 0x06000AC9 RID: 2761 RVA: 0x0004855C File Offset: 0x0004675C
	public event Action<int> OnButtonEnter;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000ACA RID: 2762 RVA: 0x00048594 File Offset: 0x00046794
	// (remove) Token: 0x06000ACB RID: 2763 RVA: 0x000485CC File Offset: 0x000467CC
	public event Action<int> OnButtonExit;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000ACC RID: 2764 RVA: 0x00048604 File Offset: 0x00046804
	// (remove) Token: 0x06000ACD RID: 2765 RVA: 0x0004863C File Offset: 0x0004683C
	public event Action<int, float> OnButtonHover;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000ACE RID: 2766 RVA: 0x00048674 File Offset: 0x00046874
	// (remove) Token: 0x06000ACF RID: 2767 RVA: 0x000486AC File Offset: 0x000468AC
	public event Action<int> OnButtonInputDown;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000AD0 RID: 2768 RVA: 0x000486E4 File Offset: 0x000468E4
	// (remove) Token: 0x06000AD1 RID: 2769 RVA: 0x0004871C File Offset: 0x0004691C
	public event Action<int> OnButtonInputUp;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000AD2 RID: 2770 RVA: 0x00048754 File Offset: 0x00046954
	// (remove) Token: 0x06000AD3 RID: 2771 RVA: 0x0004878C File Offset: 0x0004698C
	public event Action<int> OnButtonInteract;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000AD4 RID: 2772 RVA: 0x000487C4 File Offset: 0x000469C4
	// (remove) Token: 0x06000AD5 RID: 2773 RVA: 0x000487FC File Offset: 0x000469FC
	public event Action<int> OnButtonSelected;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06000AD6 RID: 2774 RVA: 0x00048834 File Offset: 0x00046A34
	// (remove) Token: 0x06000AD7 RID: 2775 RVA: 0x0004886C File Offset: 0x00046A6C
	public event Action OnMenuFocused;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06000AD8 RID: 2776 RVA: 0x000488A4 File Offset: 0x00046AA4
	// (remove) Token: 0x06000AD9 RID: 2777 RVA: 0x000488DC File Offset: 0x00046ADC
	public event Action OnMenuLostFocus;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06000ADA RID: 2778 RVA: 0x00048914 File Offset: 0x00046B14
	// (remove) Token: 0x06000ADB RID: 2779 RVA: 0x0004894C File Offset: 0x00046B4C
	public event Action OnMenuEnabled;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06000ADC RID: 2780 RVA: 0x00048984 File Offset: 0x00046B84
	// (remove) Token: 0x06000ADD RID: 2781 RVA: 0x000489BC File Offset: 0x00046BBC
	public event Action OnMenuDisabled;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06000ADE RID: 2782 RVA: 0x000489F4 File Offset: 0x00046BF4
	// (remove) Token: 0x06000ADF RID: 2783 RVA: 0x00048A2C File Offset: 0x00046C2C
	public event Action OnUpdatePositioning;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000AE0 RID: 2784 RVA: 0x00048A64 File Offset: 0x00046C64
	// (remove) Token: 0x06000AE1 RID: 2785 RVA: 0x00048A9C File Offset: 0x00046C9C
	public event Action<int> OnButtonCountModified;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000AE2 RID: 2786 RVA: 0x00048AD4 File Offset: 0x00046CD4
	// (remove) Token: 0x06000AE3 RID: 2787 RVA: 0x00048B0C File Offset: 0x00046D0C
	public event Action<Vector2, float, bool, bool> OnProcessInput;

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00048B44 File Offset: 0x00046D44
	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.radialMenuName != string.Empty)
		{
			if (UltimateRadialMenu.UltimateRadialMenus.ContainsKey(this.radialMenuName))
			{
				UltimateRadialMenu.UltimateRadialMenus.Remove(this.radialMenuName);
			}
			UltimateRadialMenu.UltimateRadialMenus.Add(this.radialMenuName, base.GetComponent<UltimateRadialMenu>());
		}
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.CurrentButtonIndex = -1;
		this.buttonIndexOnInputDown = -1;
		this.ResetRadialMenu();
		this.BaseTransform = base.GetComponent<RectTransform>();
		if (this.initialState == UltimateRadialMenu.InitialState.Disabled)
		{
			this.IsEnabled = false;
			this.Interactable = false;
			this.DisableImmediate();
		}
		else
		{
			this.IsEnabled = true;
			this.Interactable = true;
		}
		for (int i = 0; i < this.UltimateRadialButtonList.Count; i++)
		{
			this.UltimateRadialButtonList[i].radialMenu = this;
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00048C28 File Offset: 0x00046E28
	private void Start()
	{
		EventSystem eventSystem = Object.FindAnyObjectByType<EventSystem>();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.ParentCanvas == null)
		{
			this.UpdateParentCanvas();
			if (this.ParentCanvas == null)
			{
				Debug.LogError(UltimateRadialMenu.FormatDebug("This component is not with a Canvas object. Disabling this component to avoid any errors", "Please exit play mode and make sure that this Ultimate Radial Menu object is placed within a Canvas in your scene", base.gameObject.name));
				base.enabled = false;
				return;
			}
		}
		if (this.usePointer && !this.pointerColorChange)
		{
			this.PointerActive = true;
		}
		this.UpdatePositioning();
		this.RegisterInputManager(eventSystem);
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00048CB0 File Offset: 0x00046EB0
	private void RegisterInputManager(EventSystem eventSystem)
	{
		this.UpdateParentCanvas();
		if (this.ParentCanvas == null || !base.enabled)
		{
			return;
		}
		UltimateRadialMenuInputManager ultimateRadialMenuInputManager = base.GetComponent<UltimateRadialMenuInputManager>();
		if (ultimateRadialMenuInputManager == null)
		{
			ultimateRadialMenuInputManager = UltimateRadialMenuInputManager.Instance;
		}
		if (ultimateRadialMenuInputManager == null && eventSystem != null)
		{
			ultimateRadialMenuInputManager = eventSystem.GetComponent<UltimateRadialMenuInputManager>();
		}
		if (ultimateRadialMenuInputManager == null)
		{
			ultimateRadialMenuInputManager = ((eventSystem != null) ? eventSystem.gameObject : base.gameObject).AddComponent<UltimateRadialMenuInputManager>();
		}
		ultimateRadialMenuInputManager.AddRadialMenuToList(this, ref this.inputManager);
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00048D3C File Offset: 0x00046F3C
	private void OnTransformParentChanged()
	{
		this.UpdateParentCanvas();
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00048D44 File Offset: 0x00046F44
	public void UpdateParentCanvas()
	{
		this.ParentCanvas = null;
		this.canvasRectTrans = null;
		Transform transform = base.transform.parent;
		if (transform == null)
		{
			return;
		}
		while (transform != null)
		{
			if (transform.transform.GetComponent<Canvas>())
			{
				this.ParentCanvas = transform.transform.GetComponent<Canvas>();
				this.canvasRectTrans = this.ParentCanvas.GetComponent<RectTransform>();
				return;
			}
			transform = transform.transform.parent;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00048DC0 File Offset: 0x00046FC0
	internal bool CanProcessInput
	{
		get
		{
			return base.isActiveAndEnabled && this.BaseTransform != null && this.ParentCanvas != null && this.canvasRectTrans != null && this.canvasGroup != null && this.inputManager != null;
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00048E1C File Offset: 0x0004701C
	public void ProcessInput(Vector2 input, float distance, bool inputDown, bool inputUp)
	{
		if (this.BaseTransform == null || this.ParentCanvas == null || this.canvasRectTrans == null || this.canvasGroup == null || this.inputManager == null)
		{
			return;
		}
		if (!this.IsWorldSpaceRadialMenu)
		{
			input = this.BaseTransform.InverseTransformPoint(this.BasePosition + input);
		}
		if (this.parentCanvasSize != this.canvasRectTrans.sizeDelta)
		{
			this.UpdatePositioning();
		}
		if (this.IsEnabled && this.Interactable)
		{
			this.InputInRange = false;
			float num = Mathf.Atan2(input.y, input.x) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			this.GetCurrentInputAngle = num;
			this.GetCurrentInputDistance = distance;
			int i = 0;
			while (i < this.UltimateRadialButtonList.Count)
			{
				if (distance < this.CalculatedMinRange || distance > this.CalculatedMaxRange)
				{
					if (!this.inputInRangeLastFrame)
					{
						break;
					}
					this.ResetRadialMenu();
					Action onMenuLostFocus = this.OnMenuLostFocus;
					if (onMenuLostFocus != null)
					{
						onMenuLostFocus();
					}
					Action onRadialMenuLostFocus = this.OnRadialMenuLostFocus;
					if (onRadialMenuLostFocus == null)
					{
						break;
					}
					onRadialMenuLostFocus();
					break;
				}
				else if (this.CurrentButtonIndex >= 0 && this.CurrentButtonIndex < this.UltimateRadialButtonList.Count && this.UltimateRadialButtonList[this.CurrentButtonIndex].IsInAngle(num))
				{
					this.InputInRange = true;
					this.buttonHoverTime += Time.deltaTime;
					Action<int, float> onButtonHover = this.OnButtonHover;
					if (onButtonHover == null)
					{
						break;
					}
					onButtonHover(this.CurrentButtonIndex, this.buttonHoverTime);
					break;
				}
				else if (this.UltimateRadialButtonList[i].IsInAngle(num))
				{
					this.InputInRange = true;
					if (this.CurrentButtonIndex >= 0 && this.CurrentButtonIndex < this.UltimateRadialButtonList.Count)
					{
						this.buttonIndexOnInputDown = -1;
						this.UltimateRadialButtonList[this.CurrentButtonIndex].OnExit();
						Action<int> onButtonExit = this.OnButtonExit;
						if (onButtonExit != null)
						{
							onButtonExit(this.CurrentButtonIndex);
						}
						Action<int> onRadialButtonExit = this.OnRadialButtonExit;
						if (onRadialButtonExit != null)
						{
							onRadialButtonExit(this.CurrentButtonIndex);
						}
					}
					this.CurrentButtonIndex = i;
					this.buttonHoverTime = 0f;
					this.UltimateRadialButtonList[i].OnEnter();
					if (this.inputManager.invokeAction == UltimateRadialMenuInputManager.InvokeAction.OnButtonEnter)
					{
						this.UltimateRadialButtonList[this.CurrentButtonIndex].OnInteract();
					}
					Action<int> onButtonEnter = this.OnButtonEnter;
					if (onButtonEnter != null)
					{
						onButtonEnter(i);
					}
					Action<int> onRadialButtonEnter = this.OnRadialButtonEnter;
					if (onRadialButtonEnter != null)
					{
						onRadialButtonEnter(i);
					}
					if (this.usePointer && this.pointerTransform != null)
					{
						this.pointerTargetRotation = Quaternion.Euler(0f, 0f, this.UltimateRadialButtonList[this.CurrentButtonIndex].angle - this.pointerRotationOffset);
						if (!this.PointerActive || this.pointerSnapOption == UltimateRadialMenu.PointerSnapOption.Instant)
						{
							this.pointerTransform.localRotation = this.pointerTargetRotation;
						}
					}
					if (this.displayButtonName && this.nameText != null)
					{
						this.nameText.text = ((!this.UltimateRadialButtonList[i].buttonDisabled) ? this.UltimateRadialButtonList[i].name : "");
					}
					if (this.displayButtonDescription && this.descriptionText != null)
					{
						this.descriptionText.text = ((!this.UltimateRadialButtonList[i].buttonDisabled) ? this.UltimateRadialButtonList[i].description : "");
						break;
					}
					break;
				}
				else
				{
					if (i == this.UltimateRadialButtonList.Count - 1)
					{
						this.ResetRadialMenu();
						Action onMenuLostFocus2 = this.OnMenuLostFocus;
						if (onMenuLostFocus2 != null)
						{
							onMenuLostFocus2();
						}
						Action onRadialMenuLostFocus2 = this.OnRadialMenuLostFocus;
						if (onRadialMenuLostFocus2 != null)
						{
							onRadialMenuLostFocus2();
						}
					}
					i++;
				}
			}
			if (this.inputInRangeLastFrame && this.UltimateRadialButtonList.Count == 0)
			{
				Action onMenuLostFocus3 = this.OnMenuLostFocus;
				if (onMenuLostFocus3 != null)
				{
					onMenuLostFocus3();
				}
				Action onRadialMenuLostFocus3 = this.OnRadialMenuLostFocus;
				if (onRadialMenuLostFocus3 != null)
				{
					onRadialMenuLostFocus3();
				}
				this.ResetRadialMenu();
			}
			if (this.CurrentButtonIndex >= 0 && !this.UltimateRadialButtonList[this.CurrentButtonIndex].buttonDisabled && inputDown && this.InputInRange)
			{
				if (!this.UltimateRadialButtonList[this.CurrentButtonIndex].Selected)
				{
					this.UltimateRadialButtonList[this.CurrentButtonIndex].OnInputDown();
				}
				if (this.inputManager.invokeAction == UltimateRadialMenuInputManager.InvokeAction.OnButtonDown)
				{
					this.UltimateRadialButtonList[this.CurrentButtonIndex].OnInteract();
					if (this.inputManager.disableOnInteract && !this.IsWorldSpaceRadialMenu)
					{
						this.Disable();
					}
				}
				this.buttonIndexOnInputDown = this.CurrentButtonIndex;
				Action<int> onButtonInputDown = this.OnButtonInputDown;
				if (onButtonInputDown != null)
				{
					onButtonInputDown(this.CurrentButtonIndex);
				}
				Action<int> onRadialButtonInputDown = this.OnRadialButtonInputDown;
				if (onRadialButtonInputDown != null)
				{
					onRadialButtonInputDown(this.CurrentButtonIndex);
				}
			}
			if (this.CurrentButtonIndex >= 0 && !this.UltimateRadialButtonList[this.CurrentButtonIndex].buttonDisabled && inputUp && this.InputInRange)
			{
				if (!this.UltimateRadialButtonList[this.CurrentButtonIndex].Selected)
				{
					this.UltimateRadialButtonList[this.CurrentButtonIndex].OnInputUp();
				}
				if ((this.inputManager.invokeAction == UltimateRadialMenuInputManager.InvokeAction.OnButtonClick && this.CurrentButtonIndex == this.buttonIndexOnInputDown) || this.inputManager.invokeAction == UltimateRadialMenuInputManager.InvokeAction.OnButtonUp)
				{
					this.UltimateRadialButtonList[this.CurrentButtonIndex].OnInteract();
					if (this.inputManager.disableOnInteract && !this.IsWorldSpaceRadialMenu)
					{
						this.Disable();
					}
				}
				this.buttonIndexOnInputDown = -1;
				Action<int> onButtonInputUp = this.OnButtonInputUp;
				if (onButtonInputUp != null)
				{
					onButtonInputUp(this.CurrentButtonIndex);
				}
				Action<int> onRadialButtonInputUp = this.OnRadialButtonInputUp;
				if (onRadialButtonInputUp != null)
				{
					onRadialButtonInputUp(this.CurrentButtonIndex);
				}
			}
			this.ProcessPointer();
			this.ProcessToggle();
			Action<Vector2, float, bool, bool> onProcessInput = this.OnProcessInput;
			if (onProcessInput != null)
			{
				onProcessInput(input, distance, inputDown, inputUp);
			}
			Action<Vector2, float, bool, bool> onRadialMenuProcessInput = this.OnRadialMenuProcessInput;
			if (onRadialMenuProcessInput != null)
			{
				onRadialMenuProcessInput(input, distance, inputDown, inputUp);
			}
			if (this.InputInRange && !this.inputInRangeLastFrame)
			{
				Action onMenuFocused = this.OnMenuFocused;
				if (onMenuFocused != null)
				{
					onMenuFocused();
				}
			}
			this.inputInRangeLastFrame = this.InputInRange;
			return;
		}
		this.ProcessPointer();
		this.ProcessToggle();
		Action<Vector2, float, bool, bool> onProcessInput2 = this.OnProcessInput;
		if (onProcessInput2 != null)
		{
			onProcessInput2(input, distance, inputDown, inputUp);
		}
		Action<Vector2, float, bool, bool> onRadialMenuProcessInput2 = this.OnRadialMenuProcessInput;
		if (onRadialMenuProcessInput2 == null)
		{
			return;
		}
		onRadialMenuProcessInput2(input, distance, inputDown, inputUp);
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000494C0 File Offset: 0x000476C0
	private void ResetRadialMenu()
	{
		if (this.CurrentButtonIndex >= 0 && this.CurrentButtonIndex < this.UltimateRadialButtonList.Count)
		{
			this.UltimateRadialButtonList[this.CurrentButtonIndex].OnExit();
			Action<int> onButtonExit = this.OnButtonExit;
			if (onButtonExit != null)
			{
				onButtonExit(this.CurrentButtonIndex);
			}
			Action<int> onRadialButtonExit = this.OnRadialButtonExit;
			if (onRadialButtonExit != null)
			{
				onRadialButtonExit(this.CurrentButtonIndex);
			}
		}
		this.CurrentButtonIndex = -1;
		this.buttonIndexOnInputDown = -1;
		this.GetCurrentInputAngle = 0f;
		this.GetCurrentInputDistance = 0f;
		if (this.displayButtonName && this.nameText != null)
		{
			this.nameText.text = "";
		}
		if (this.displayButtonDescription && this.descriptionText != null)
		{
			this.descriptionText.text = "";
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000495A0 File Offset: 0x000477A0
	private void ProcessPointer()
	{
		if (!this.usePointer || this.pointerTransform == null)
		{
			return;
		}
		if (this.pointerColorChange)
		{
			if (this.inputInRangeLastFrame != this.InputInRange)
			{
				if (this.InputInRange)
				{
					if (!this.changeOverTime || this.fadeInDuration <= 0f)
					{
						this.PointerActive = true;
						this.pointerImage.color = this.pointerActiveColor;
						if (this.fadeOutDuration > 0f && this.pointerFadeOut)
						{
							this.pointerFadeOut = false;
							this.pointerLerpValue = 0f;
						}
					}
					else
					{
						this.pointerFadeIn = true;
						this.pointerFadeColor = this.pointerImage.color;
						this.fadeInSpeed = 1f / this.fadeInDuration;
						this.pointerLerpValue = 0f;
						if (this.pointerFadeOut)
						{
							this.pointerFadeOut = false;
						}
					}
				}
				else if (!this.changeOverTime || this.fadeOutDuration <= 0f)
				{
					this.PointerActive = false;
					this.pointerImage.color = this.pointerNormalColor;
					if (this.fadeInDuration > 0f && this.pointerFadeIn)
					{
						this.pointerFadeIn = false;
						this.pointerLerpValue = 0f;
					}
				}
				else
				{
					this.pointerFadeOut = true;
					this.pointerFadeColor = this.pointerImage.color;
					this.fadeOutSpeed = 1f / this.fadeOutDuration;
					this.pointerLerpValue = 0f;
					if (this.pointerFadeIn)
					{
						this.pointerFadeIn = false;
					}
				}
			}
			if (this.pointerFadeIn)
			{
				this.PointerActive = true;
				this.pointerLerpValue += Time.unscaledDeltaTime * this.fadeInSpeed;
				this.pointerImage.color = Color.Lerp(this.pointerFadeColor, this.pointerActiveColor, this.pointerLerpValue);
				if (this.pointerLerpValue >= 1f)
				{
					this.pointerImage.color = this.pointerActiveColor;
					this.pointerFadeIn = false;
					this.pointerLerpValue = 0f;
				}
			}
			else if (this.pointerFadeOut)
			{
				this.pointerLerpValue += Time.unscaledDeltaTime * this.fadeOutSpeed;
				this.pointerImage.color = Color.Lerp(this.pointerFadeColor, this.pointerNormalColor, this.pointerLerpValue);
				if (this.pointerLerpValue >= 1f)
				{
					this.pointerFadeOut = false;
					this.pointerImage.color = this.pointerNormalColor;
					this.pointerLerpValue = 0f;
					this.PointerActive = false;
				}
			}
		}
		if (!this.PointerActive)
		{
			return;
		}
		if (this.pointerSnapOption == UltimateRadialMenu.PointerSnapOption.Free)
		{
			this.pointerTransform.localRotation = Quaternion.Slerp(this.pointerTransform.localRotation, Quaternion.Euler(0f, 0f, this.GetCurrentInputAngle - this.pointerRotationOffset), Time.unscaledDeltaTime * this.pointerTargetSpeed);
			return;
		}
		if (this.pointerSnapOption == UltimateRadialMenu.PointerSnapOption.Smooth)
		{
			this.pointerTransform.localRotation = Quaternion.Slerp(this.pointerTransform.localRotation, this.pointerTargetRotation, Time.unscaledDeltaTime * this.pointerTargetSpeed);
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x000498B8 File Offset: 0x00047AB8
	private void ProcessToggle()
	{
		if (!this.menuToggleOverTime)
		{
			return;
		}
		if (this.toggleIn)
		{
			this.toggleLerpValue += Time.unscaledDeltaTime * this.toggleInSpeed;
			if (this.menuToggleAlpha)
			{
				this.canvasGroup.alpha = Mathf.Lerp(this.toggleAlphaValue, 1f, this.toggleLerpValue);
			}
			if (this.menuToggleScale)
			{
				this.BaseTransform.localScale = Vector3.one * Mathf.Lerp(this.toggleScaleValue, 1f, this.toggleLerpValue);
			}
			if (this.IsEnabled && this.toggleLerpValue >= 1f)
			{
				this.toggleIn = false;
				this.toggleLerpValue = 0f;
				this.Interactable = true;
				if (this.menuToggleAlpha)
				{
					this.canvasGroup.alpha = 1f;
				}
				if (this.menuToggleScale)
				{
					this.BaseTransform.localScale = Vector3.one;
					return;
				}
			}
		}
		else if (this.toggleOut)
		{
			this.toggleLerpValue += Time.unscaledDeltaTime * this.toggleOutSpeed;
			if (this.menuToggleAlpha)
			{
				this.canvasGroup.alpha = Mathf.Lerp(this.toggleAlphaValue, 0f, this.toggleLerpValue);
			}
			if (this.menuToggleScale)
			{
				this.BaseTransform.localScale = Vector3.one * Mathf.Lerp(this.toggleScaleValue, 0f, this.toggleLerpValue);
			}
			if (this.toggleLerpValue >= 1f)
			{
				this.toggleOut = false;
				this.toggleLerpValue = 0f;
				if (this.menuToggleAlpha)
				{
					this.canvasGroup.alpha = 0f;
				}
				if (this.menuToggleScale)
				{
					this.BaseTransform.localScale = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00049A88 File Offset: 0x00047C88
	private Vector2 GetImageAspectRatio(Sprite sprite)
	{
		Vector2 one = Vector2.one;
		Vector2 vector = new Vector2(sprite.rect.width, sprite.rect.height);
		float num = ((vector.x > vector.y) ? vector.x : vector.y);
		one.x = vector.x / num;
		one.y = vector.y / num;
		return one;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00049AFC File Offset: 0x00047CFC
	private void CreateRadialButtonAtIndex(int buttonIndex)
	{
		if (this.CurrentButtonIndex >= 0 && this.CurrentButtonIndex < this.UltimateRadialButtonList.Count)
		{
			this.UltimateRadialButtonList[this.CurrentButtonIndex].OnExit();
		}
		if (buttonIndex < 0)
		{
			buttonIndex = 0;
		}
		if (buttonIndex > this.UltimateRadialButtonList.Count)
		{
			buttonIndex = this.UltimateRadialButtonList.Count;
		}
		if (this.radialMenuStyle != null && this.UltimateRadialButtonList.Count >= this.radialMenuStyle.maxButtonCount)
		{
			Debug.LogWarning(UltimateRadialMenu.FormatDebug("The current button count is out of range for this style. The buttons may look strange because there is no corresponding button sprite to use with this count", "To ensure that the buttons look good on the menu, please limit the number of buttons to correspond to the style you are using. Otherwise, consider using a radial menu that doesn't have a set number of buttons to use", base.gameObject.name));
		}
		if (this.UltimateRadialButtonPool.Count > 0)
		{
			this.UltimateRadialButtonList.Insert(buttonIndex, this.GetRadialButtonFromPool());
		}
		else
		{
			this.UltimateRadialButtonList.Insert(buttonIndex, new UltimateRadialMenu.UltimateRadialButton());
			this.UltimateRadialButtonList[buttonIndex].radialMenu = this;
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<RectTransform>();
			gameObject.AddComponent<CanvasRenderer>();
			gameObject.AddComponent<Image>();
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
			if (this.BaseTransform == null)
			{
				this.BaseTransform = base.GetComponent<RectTransform>();
			}
			gameObject2.transform.SetParent(this.BaseTransform);
			gameObject2.gameObject.name = "Radial Menu Button";
			gameObject2.transform.SetAsLastSibling();
			this.UltimateRadialButtonList[buttonIndex].buttonTransform = gameObject2.GetComponent<RectTransform>();
			this.UltimateRadialButtonList[buttonIndex].buttonTransform.anchorMin = new Vector2(0.5f, 0.5f);
			this.UltimateRadialButtonList[buttonIndex].buttonTransform.anchorMax = new Vector2(0.5f, 0.5f);
			this.UltimateRadialButtonList[buttonIndex].buttonTransform.pivot = new Vector2(0.5f, 0.5f);
			this.UltimateRadialButtonList[buttonIndex].buttonTransform.localScale = Vector3.one;
			this.UltimateRadialButtonList[buttonIndex].radialImage = gameObject2.GetComponent<Image>();
			this.UltimateRadialButtonList[buttonIndex].radialImage.sprite = this.normalSprite;
			if (this.UltimateRadialButtonList[buttonIndex].radialImage.sprite != null)
			{
				this.UltimateRadialButtonList[buttonIndex].radialImage.color = this.normalColor;
			}
			else
			{
				this.UltimateRadialButtonList[buttonIndex].radialImage.color = Color.clear;
			}
			if (this.useButtonIcon)
			{
				GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
				gameObject3.transform.SetParent(this.UltimateRadialButtonList[buttonIndex].buttonTransform);
				gameObject3.gameObject.name = "Icon " + buttonIndex.ToString("00");
				this.UltimateRadialButtonList[buttonIndex].iconTransform = gameObject3.GetComponent<RectTransform>();
				this.UltimateRadialButtonList[buttonIndex].buttonTransform.anchorMin = new Vector2(0.5f, 0.5f);
				this.UltimateRadialButtonList[buttonIndex].buttonTransform.anchorMax = new Vector2(0.5f, 0.5f);
				this.UltimateRadialButtonList[buttonIndex].buttonTransform.pivot = new Vector2(0.5f, 0.5f);
				this.UltimateRadialButtonList[buttonIndex].iconTransform.localScale = Vector3.one;
				this.UltimateRadialButtonList[buttonIndex].icon = gameObject3.GetComponent<Image>();
				this.UltimateRadialButtonList[buttonIndex].icon.sprite = null;
				this.UltimateRadialButtonList[buttonIndex].icon.color = Color.clear;
			}
			if (this.useButtonText)
			{
				GameObject gameObject4 = new GameObject();
				gameObject4.AddComponent<RectTransform>();
				gameObject4.AddComponent<CanvasRenderer>();
				GameObject gameObject5 = Object.Instantiate<GameObject>(gameObject4, Vector3.zero, Quaternion.identity);
				gameObject5.transform.SetParent(this.UltimateRadialButtonList[buttonIndex].buttonTransform);
				gameObject5.gameObject.name = "Text " + buttonIndex.ToString("00");
				this.UltimateRadialButtonList[buttonIndex].text = gameObject5.AddComponent<Text>();
				this.UltimateRadialButtonList[buttonIndex].text.text = "";
				this.UltimateRadialButtonList[buttonIndex].text.alignment = TextAnchor.MiddleCenter;
				this.UltimateRadialButtonList[buttonIndex].text.resizeTextForBestFit = true;
				this.UltimateRadialButtonList[buttonIndex].text.resizeTextMinSize = 0;
				this.UltimateRadialButtonList[buttonIndex].text.resizeTextMaxSize = 300;
				if (this.buttonTextFont != null)
				{
					this.UltimateRadialButtonList[buttonIndex].text.font = this.buttonTextFont;
				}
				this.UltimateRadialButtonList[buttonIndex].text.color = this.textNormalColor;
				this.UltimateRadialButtonList[buttonIndex].text.rectTransform.localScale = Vector3.one;
				if (this.buttonTextOutline)
				{
					gameObject5.AddComponent<Outline>().effectColor = this.buttonTextOutlineColor;
				}
				Object.Destroy(gameObject4);
			}
			Object.Destroy(gameObject);
		}
		this.RadialMenuButtonCountModified();
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0004A058 File Offset: 0x00048258
	private void FindRadialButtonIndex(ref int buttonIndex)
	{
		if (buttonIndex < 0)
		{
			for (int i = 0; i < this.UltimateRadialButtonList.Count; i++)
			{
				if (!this.UltimateRadialButtonList[i].Registered && !this.UltimateRadialButtonList[i].buttonDisabled)
				{
					buttonIndex = i;
					return;
				}
			}
			this.CreateRadialButtonAtIndex(1000);
			buttonIndex = this.UltimateRadialButtonList.Count - 1;
			return;
		}
		if (buttonIndex >= this.UltimateRadialButtonList.Count)
		{
			this.CreateRadialButtonAtIndex(1000);
			buttonIndex = this.UltimateRadialButtonList.Count - 1;
			return;
		}
		if (this.UltimateRadialButtonList[buttonIndex].Registered || this.UltimateRadialButtonList[buttonIndex].buttonDisabled)
		{
			this.CreateRadialButtonAtIndex(buttonIndex);
		}
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0004A120 File Offset: 0x00048320
	private UltimateRadialMenu.UltimateRadialButton GetRadialButtonFromPool()
	{
		UltimateRadialMenu.UltimateRadialButton ultimateRadialButton = this.UltimateRadialButtonPool[0];
		ultimateRadialButton.buttonTransform.gameObject.SetActive(true);
		this.UltimateRadialButtonPool.Remove(ultimateRadialButton);
		return ultimateRadialButton;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0004A15C File Offset: 0x0004835C
	private void SendRadialButtonToPool(int buttonIndex)
	{
		this.UltimateRadialButtonPool.Add(this.UltimateRadialButtonList[buttonIndex]);
		this.UltimateRadialButtonList[buttonIndex].buttonTransform.gameObject.SetActive(false);
		this.UltimateRadialButtonList.RemoveAt(buttonIndex);
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0004A1A8 File Offset: 0x000483A8
	private void RadialMenuButtonCountModified()
	{
		if (this.radialMenuStyle != null)
		{
			for (int i = 0; i < this.radialMenuStyle.RadialMenuStyles.Count; i++)
			{
				if (360f / (float)this.radialMenuStyle.RadialMenuStyles[i].buttonCount <= this.GetAnglePerButton)
				{
					this.currentStyleIndex = i;
					break;
				}
			}
			this.normalSprite = this.radialMenuStyle.RadialMenuStyles[this.currentStyleIndex].normalSprite;
			if (this.spriteSwap)
			{
				this.highlightedSprite = this.radialMenuStyle.RadialMenuStyles[this.currentStyleIndex].highlightedSprite ?? this.normalSprite;
				this.pressedSprite = this.radialMenuStyle.RadialMenuStyles[this.currentStyleIndex].pressedSprite ?? this.normalSprite;
				this.selectedSprite = this.radialMenuStyle.RadialMenuStyles[this.currentStyleIndex].selectedSprite ?? this.normalSprite;
				this.disabledSprite = this.radialMenuStyle.RadialMenuStyles[this.currentStyleIndex].disabledSprite ?? this.normalSprite;
			}
			for (int j = 0; j < this.UltimateRadialButtonList.Count; j++)
			{
				if (this.UltimateRadialButtonList[j].buttonDisabled && this.spriteSwap)
				{
					this.UltimateRadialButtonList[j].radialImage.sprite = this.disabledSprite;
				}
				else if (this.UltimateRadialButtonList[j].Selected && this.spriteSwap)
				{
					this.UltimateRadialButtonList[j].radialImage.sprite = this.selectedSprite;
				}
				else
				{
					this.UltimateRadialButtonList[j].radialImage.sprite = this.normalSprite;
				}
			}
		}
		if (this.usePointer && this.pointerImage != null)
		{
			if (this.usePointerStyle)
			{
				for (int k = 0; k < this.PointerStyles.Count; k++)
				{
					if (this.UltimateRadialButtonList.Count <= this.PointerStyles[k].buttonCount)
					{
						this.pointerImage.sprite = this.PointerStyles[k].pointerSprite;
						break;
					}
				}
			}
			if (this.pointerSiblingIndex != UltimateRadialMenu.SetSiblingIndex.Disabled)
			{
				if (this.pointerSiblingIndex == UltimateRadialMenu.SetSiblingIndex.Last)
				{
					this.pointerImage.transform.SetAsLastSibling();
				}
				else if (this.pointerSiblingIndex == UltimateRadialMenu.SetSiblingIndex.First && this.pointerImage.transform.GetSiblingIndex() > 0)
				{
					this.pointerImage.transform.SetAsFirstSibling();
				}
			}
		}
		Action<int> onButtonCountModified = this.OnButtonCountModified;
		if (onButtonCountModified != null)
		{
			onButtonCountModified(this.UltimateRadialButtonList.Count);
		}
		Action<int> onRadialMenuButtonCountModified = this.OnRadialMenuButtonCountModified;
		if (onRadialMenuButtonCountModified != null)
		{
			onRadialMenuButtonCountModified(this.UltimateRadialButtonList.Count);
		}
		this.UpdatePositioning();
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0004A498 File Offset: 0x00048698
	public void UpdatePositioning()
	{
		if (this.ParentCanvas == null)
		{
			this.UpdateParentCanvas();
		}
		if (this.ParentCanvas == null)
		{
			if (Application.isPlaying)
			{
				Debug.LogError(UltimateRadialMenu.FormatDebug("There is no parent canvas object", "Please make sure that the Ultimate Radial Menu is placed within a canvas", base.gameObject.name));
			}
			return;
		}
		this.parentCanvasSize = this.canvasRectTrans.sizeDelta;
		float num = ((this.scalingAxis == UltimateRadialMenu.ScalingAxis.Height) ? this.canvasRectTrans.sizeDelta.y : this.canvasRectTrans.sizeDelta.x) * (this.menuSize / 10f);
		if (this.BaseTransform == null)
		{
			this.BaseTransform = base.GetComponent<RectTransform>();
		}
		this.BaseTransform.localScale = Vector3.one;
		if (this.BaseTransform.pivot != Vector2.one / 2f)
		{
			this.BaseTransform.pivot = Vector2.one / 2f;
		}
		Vector2 vector = new Vector2(this.horizontalPosition - 50f, this.verticalPosition - 50f) / 100f;
		this.BaseTransform.localPosition = this.canvasRectTrans.sizeDelta * vector - Vector2.one * num * vector;
		this.BaseTransform.sizeDelta = new Vector2(num, num);
		if (this.overallAngle < 360f && this.centerOffset != -Vector2.one)
		{
			this.BaseTransform.localPosition -= this.BaseTransform.sizeDelta / 2f - this.BaseTransform.sizeDelta * this.centerOffset;
		}
		this.BaseTransform.localRotation = Quaternion.identity;
		this.defaultPosition = this.BaseTransform.position;
		this.CalculatedMinRange = this.BaseTransform.sizeDelta.x / 2f * this.minRange;
		if (this.infiniteMaxRange)
		{
			this.CalculatedMaxRange = float.PositiveInfinity;
		}
		else
		{
			this.CalculatedMaxRange = this.BaseTransform.sizeDelta.x / 2f * this.maxRange;
		}
		if (this.ParentCanvas.renderMode == RenderMode.WorldSpace)
		{
			this.IsWorldSpaceRadialMenu = true;
			if (!this.BaseTransform.GetComponent<BoxCollider>() && Application.isPlaying)
			{
				this.BaseTransform.gameObject.AddComponent<BoxCollider>();
			}
			this.BaseTransform.GetComponent<BoxCollider>().isTrigger = true;
			this.BaseTransform.GetComponent<BoxCollider>().size = new Vector3(this.BaseTransform.sizeDelta.x * this.maxRange, this.BaseTransform.sizeDelta.y * this.maxRange, 0.001f);
			this.BaseTransform.localPosition += new Vector3(0f, 0f, this.depthPosition);
		}
		else
		{
			this.IsWorldSpaceRadialMenu = false;
			if (this.BaseTransform.GetComponent<BoxCollider>() && Application.isPlaying)
			{
				Object.DestroyImmediate(this.BaseTransform.GetComponent<BoxCollider>());
			}
		}
		if (this.displayButtonName && this.nameText != null)
		{
			Vector2 zero = Vector2.zero;
			zero.x += this.BaseTransform.sizeDelta.x * ((this.nameTextHorizontalPosition - 50f) / 100f);
			zero.y += this.BaseTransform.sizeDelta.y * ((this.nameTextVerticalPosition - 50f) / 100f);
			this.nameText.rectTransform.sizeDelta = new Vector2(this.BaseTransform.sizeDelta.x * this.nameTextSize, this.BaseTransform.sizeDelta.x * this.nameTextSize) * new Vector2(this.nameTextRatioX, this.nameTextRatioY);
			this.nameText.rectTransform.localPosition = zero;
			this.nameText.rectTransform.localRotation = Quaternion.identity;
		}
		if (this.displayButtonDescription && this.descriptionText != null)
		{
			Vector2 zero2 = Vector2.zero;
			zero2.x += this.BaseTransform.sizeDelta.x * ((this.descriptionTextHorizontalPosition - 50f) / 100f);
			zero2.y += this.BaseTransform.sizeDelta.y * ((this.descriptionTextVerticalPosition - 50f) / 100f);
			this.descriptionText.rectTransform.sizeDelta = new Vector2(this.BaseTransform.sizeDelta.x * this.descriptionTextSize, this.BaseTransform.sizeDelta.x * this.descriptionTextSize) * new Vector2(this.descriptionTextRatioX, this.descriptionTextRatioY);
			this.descriptionText.rectTransform.localPosition = zero2;
			this.descriptionText.rectTransform.localRotation = Quaternion.identity;
		}
		float getAnglePerButton = this.GetAnglePerButton;
		float num2 = -getAnglePerButton * 0.017453292f;
		float num3 = 0f;
		if (this.overallAngle == 360f)
		{
			switch (this.angleOffset)
			{
			case UltimateRadialMenu.AngleOffset.Centered:
				num3 = this.GetAnglePerButton / 2f;
				break;
			case UltimateRadialMenu.AngleOffset.OnlyEven:
				if (this.UltimateRadialButtonList.Count % 2 == 0)
				{
					num3 = this.GetAnglePerButton / 2f;
				}
				break;
			case UltimateRadialMenu.AngleOffset.OnlyOdd:
				if (this.UltimateRadialButtonList.Count % 2 != 0)
				{
					num3 = this.GetAnglePerButton / 2f;
				}
				break;
			}
		}
		float num4 = -this.startingAngle + num3 - this.GetAnglePerButton / 2f;
		float num5 = 1.5707964f + -this.startingAngle * 0.017453292f;
		if (this.overallAngle < 360f)
		{
			num4 = -this.centerAngle + this.overallAngle / 2f - this.GetAnglePerButton / 2f;
			num5 = (90f - (this.centerAngle - this.overallAngle / 2f)) * 0.017453292f;
		}
		Vector2 vector2 = this.BaseTransform.sizeDelta * this.menuButtonSize;
		if (this.normalSprite != null)
		{
			vector2 *= this.GetImageAspectRatio(this.normalSprite);
		}
		float num6 = this.BaseTransform.sizeDelta.x / 2f * this.radialMenuButtonRadius - vector2.y / 2f;
		for (int i = 0; i < this.UltimateRadialButtonList.Count; i++)
		{
			this.UltimateRadialButtonList[i].buttonIndex = i;
			if (this.UltimateRadialButtonList[i].radialImage == null)
			{
				this.UltimateRadialButtonList[i].radialImage = this.UltimateRadialButtonList[i].buttonTransform.GetComponent<Image>();
			}
			this.UltimateRadialButtonList[i].buttonTransform.sizeDelta = vector2;
			Vector3 zero3 = Vector3.zero;
			zero3.x += Mathf.Cos(num2 * (float)i + num5 + num3 * 0.017453292f + num2 / 2f) * num6;
			zero3.y += Mathf.Sin(num2 * (float)i + num5 + num3 * 0.017453292f + num2 / 2f) * num6;
			this.UltimateRadialButtonList[i].buttonTransform.localPosition = zero3;
			this.UltimateRadialButtonList[i].buttonTransform.localScale = ((this.scaleTransform && this.UltimateRadialButtonList[i].buttonDisabled) ? (Vector3.one * this.disabledScaleModifier) : Vector3.one);
			if (this.scaleTransform)
			{
				this.UltimateRadialButtonList[i].normalPosition = zero3;
				if (this.positionModifier != 0f)
				{
					this.UltimateRadialButtonList[i].highlightedPosition = zero3 + zero3.normalized * (num6 * this.positionModifier);
				}
				else
				{
					this.UltimateRadialButtonList[i].highlightedPosition = zero3;
				}
				if (this.pressedPositionModifier != 0f)
				{
					this.UltimateRadialButtonList[i].pressedPosition = zero3 + zero3.normalized * (num6 * this.pressedPositionModifier);
				}
				else
				{
					this.UltimateRadialButtonList[i].pressedPosition = zero3;
				}
				if (this.selectedPositionModifier != 0f)
				{
					this.UltimateRadialButtonList[i].selectedPosition = zero3 + zero3.normalized * (num6 * this.selectedPositionModifier);
				}
				else
				{
					this.UltimateRadialButtonList[i].selectedPosition = zero3;
				}
				if (this.disabledPositionModifier != 0f)
				{
					this.UltimateRadialButtonList[i].disabledPosition = zero3 + zero3.normalized * (num6 * this.disabledPositionModifier);
					if (this.UltimateRadialButtonList[i].buttonDisabled)
					{
						this.UltimateRadialButtonList[i].buttonTransform.localPosition = this.UltimateRadialButtonList[i].disabledPosition;
					}
				}
				else
				{
					this.UltimateRadialButtonList[i].disabledPosition = zero3;
				}
			}
			this.UltimateRadialButtonList[i].angle = 90f + -this.startingAngle + num3 + -getAnglePerButton * (float)i - getAnglePerButton / 2f;
			if (this.overallAngle < 360f)
			{
				this.UltimateRadialButtonList[i].angle = 90f + (-this.centerAngle + this.overallAngle / 2f) + num3 + -getAnglePerButton * (float)i - getAnglePerButton / 2f;
			}
			this.UltimateRadialButtonList[i].angleRange = getAnglePerButton / 2f * this.buttonInputAngle;
			if (this.UltimateRadialButtonList[i].angle <= -360f)
			{
				this.UltimateRadialButtonList[i].angle += 360f;
			}
			if (this.UltimateRadialButtonList[i].angle < 0f)
			{
				this.UltimateRadialButtonList[i].angle += 360f;
			}
			Vector3 zero4 = Vector3.zero;
			if (this.followOrbitalRotation)
			{
				zero4 = new Vector3(0f, 0f, -getAnglePerButton * (float)i + num4);
			}
			this.UltimateRadialButtonList[i].buttonTransform.localRotation = Quaternion.Euler(zero4);
			if (this.useButtonIcon && this.UltimateRadialButtonList[i].icon != null)
			{
				float num7 = this.iconHorizontalPosition;
				float num8 = this.iconVerticalPosition;
				float num9 = this.iconSize;
				float num10 = this.iconRotation;
				if (this.UltimateRadialButtonList[i].useIconUnique)
				{
					num7 = this.UltimateRadialButtonList[i].iconHorizontalPosition;
					num8 = this.UltimateRadialButtonList[i].iconVerticalPosition;
					num9 = this.UltimateRadialButtonList[i].iconSize;
					num10 = this.UltimateRadialButtonList[i].iconRotation;
				}
				Vector2 vector3 = Vector3.zero;
				vector3.x += this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.x * (num7 / 100f) - this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.x / 2f;
				vector3.y += this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.y * (num8 / 100f) - this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.y / 2f;
				this.UltimateRadialButtonList[i].icon.rectTransform.sizeDelta = new Vector2(this.BaseTransform.sizeDelta.x * num9, this.BaseTransform.sizeDelta.x * num9) * ((this.UltimateRadialButtonList[i].icon.sprite == null) ? Vector2.one : this.GetImageAspectRatio(this.UltimateRadialButtonList[i].icon.sprite));
				this.UltimateRadialButtonList[i].icon.rectTransform.localPosition = vector3;
				if (this.UltimateRadialButtonList[i].iconTransform == null)
				{
					this.UltimateRadialButtonList[i].iconTransform = this.UltimateRadialButtonList[i].icon.rectTransform;
				}
				if (this.iconLocalRotation)
				{
					if (this.iconSmartRotation)
					{
						float num11 = this.UltimateRadialButtonList[i].radialImage.rectTransform.localRotation.eulerAngles.z;
						if (num11 < 0f)
						{
							num11 += 360f;
						}
						if (num11 > 90f && num11 < 270f)
						{
							num10 += 180f;
						}
					}
				}
				else
				{
					num10 = -this.UltimateRadialButtonList[i].buttonTransform.localRotation.eulerAngles.z + (this.UltimateRadialButtonList[i].useIconUnique ? this.UltimateRadialButtonList[i].iconRotation : (-this.iconRotation));
				}
				this.UltimateRadialButtonList[i].iconNormalScale = (this.UltimateRadialButtonList[i].invertScaleY ? new Vector3(1f, -1f, 1f) : new Vector3(1f, 1f, 1f));
				this.UltimateRadialButtonList[i].iconTransform.localScale = (this.UltimateRadialButtonList[i].buttonDisabled ? (this.UltimateRadialButtonList[i].iconNormalScale * this.iconDisabledScaleModifier) : this.UltimateRadialButtonList[i].iconNormalScale);
				this.UltimateRadialButtonList[i].icon.rectTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, num10));
			}
			if (this.UltimateRadialButtonList[i].text != null)
			{
				this.UltimateRadialButtonList[i].text.rectTransform.sizeDelta = new Vector2(this.BaseTransform.sizeDelta.x * this.textSize, this.BaseTransform.sizeDelta.x * this.textSize) * new Vector2(this.textAreaRatioX, this.textAreaRatioY);
				if (this.textLocalPosition)
				{
					Vector2 zero5 = Vector2.zero;
					Vector2 vector4 = new Vector2(this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.x, this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.y) * 1.25f;
					if (this.relativeToIcon && this.UltimateRadialButtonList[i].iconTransform != null)
					{
						vector4 = new Vector2(this.UltimateRadialButtonList[i].iconTransform.sizeDelta.x, this.UltimateRadialButtonList[i].iconTransform.sizeDelta.y) * 1.25f;
					}
					zero5.x += vector4.x * (this.textHorizontalPosition / 100f) - vector4.x / 2f;
					zero5.y += vector4.y * (this.textVerticalPosition / 100f) - vector4.y / 2f;
					this.UltimateRadialButtonList[i].text.rectTransform.localPosition = zero5;
					if (this.textLocalRotation)
					{
						float num12 = this.UltimateRadialButtonList[i].radialImage.rectTransform.localRotation.eulerAngles.z;
						if (num12 < 0f)
						{
							num12 += 360f;
						}
						if (num12 > 90f && num12 < 270f)
						{
							this.UltimateRadialButtonList[i].text.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
						}
						else
						{
							this.UltimateRadialButtonList[i].text.rectTransform.localRotation = Quaternion.identity;
						}
					}
					else
					{
						this.UltimateRadialButtonList[i].text.rectTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -this.UltimateRadialButtonList[i].buttonTransform.localRotation.eulerAngles.z));
					}
				}
				else
				{
					Vector3 vector5 = Vector3.zero;
					Vector2 vector6 = new Vector2(this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.x, this.UltimateRadialButtonList[i].buttonTransform.sizeDelta.y) * 1.25f;
					if (this.relativeToIcon && this.UltimateRadialButtonList[i].iconTransform != null)
					{
						vector6 = new Vector2(this.UltimateRadialButtonList[i].iconTransform.sizeDelta.x, this.UltimateRadialButtonList[i].iconTransform.sizeDelta.y) * 1.25f;
					}
					vector5.x += vector6.x * (this.textHorizontalPosition / 100f) - vector6.x / 2f;
					vector5.y += vector6.y * (this.textVerticalPosition / 100f) - vector6.y / 2f;
					if (this.ParentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
					{
						vector5 = this.BaseTransform.transform.TransformPoint(vector5);
						vector5 -= this.BaseTransform.position;
					}
					if (this.relativeToIcon && this.UltimateRadialButtonList[i].iconTransform != null)
					{
						this.UltimateRadialButtonList[i].text.rectTransform.position = this.UltimateRadialButtonList[i].iconTransform.position + vector5;
					}
					else
					{
						this.UltimateRadialButtonList[i].text.rectTransform.position = this.UltimateRadialButtonList[i].buttonTransform.position + vector5;
					}
					this.UltimateRadialButtonList[i].text.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -this.UltimateRadialButtonList[i].buttonTransform.localRotation.eulerAngles.z);
				}
			}
			if (Application.isPlaying && this.scaleTransform && this.UltimateRadialButtonList[i].Selected)
			{
				this.UltimateRadialButtonList[i].buttonTransform.localScale = Vector3.one * this.selectedScaleModifier;
				this.UltimateRadialButtonList[i].buttonTransform.localPosition = this.UltimateRadialButtonList[i].selectedPosition;
			}
		}
		this.CurrentButtonIndex = -1;
		this.BaseTransform.localScale = ((this.menuToggleOverTime && this.menuToggleScale && !this.IsEnabled && Application.isPlaying) ? Vector3.zero : Vector3.one);
		if (this.usePointer && this.pointerImage != null)
		{
			if (this.pointerTransform == null)
			{
				this.pointerTransform = this.pointerImage.rectTransform;
			}
			if (this.pointerTransform.pivot != Vector2.one / 2f)
			{
				this.pointerTransform.pivot = Vector2.one / 2f;
			}
			this.pointerTransform.sizeDelta = Vector2.one * (this.BaseTransform.sizeDelta.x * this.pointerSize);
			this.pointerTransform.localPosition = Vector3.zero;
			if (!Application.isPlaying && this.UltimateRadialButtonList.Count > 0)
			{
				this.pointerTransform.localRotation = Quaternion.Euler(0f, 0f, this.UltimateRadialButtonList[0].angle - this.pointerRotationOffset);
			}
			this.pointerTargetSpeed = 1f / this.pointerTargetTime;
		}
		Action onUpdatePositioning = this.OnUpdatePositioning;
		if (onUpdatePositioning == null)
		{
			return;
		}
		onUpdatePositioning();
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0004BA87 File Offset: 0x00049C87
	public void RegisterButton(Action ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0004BAB5 File Offset: 0x00049CB5
	public void RegisterButton(Action<int> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0004BAE3 File Offset: 0x00049CE3
	public void RegisterButton(Action<string> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0004BB14 File Offset: 0x00049D14
	public void Enable()
	{
		if (this.IsEnabled)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
			this.Awake();
			this.Start();
		}
		this.IsEnabled = true;
		if (this.menuToggleOverTime)
		{
			this.Interactable = false;
			if (this.toggleInDuration <= 0f)
			{
				this.canvasGroup.alpha = 1f;
				if (this.menuToggleScale)
				{
					this.BaseTransform.localScale = Vector3.one;
				}
				this.Interactable = true;
				if (this.toggleOutDuration > 0f && this.toggleOut)
				{
					this.toggleOut = false;
					this.toggleLerpValue = 0f;
				}
			}
			else
			{
				if (!this.menuToggleAlpha)
				{
					this.canvasGroup.alpha = 1f;
				}
				this.toggleIn = true;
				this.toggleAlphaValue = this.canvasGroup.alpha;
				this.toggleScaleValue = this.BaseTransform.localScale.x;
				this.toggleInSpeed = 1f / this.toggleInDuration;
				this.toggleLerpValue = 0f;
				if (this.toggleOut)
				{
					this.toggleOut = false;
				}
			}
		}
		else
		{
			this.canvasGroup.alpha = 1f;
			this.Interactable = true;
		}
		Action onMenuEnabled = this.OnMenuEnabled;
		if (onMenuEnabled != null)
		{
			onMenuEnabled();
		}
		Action onRadialMenuEnabled = this.OnRadialMenuEnabled;
		if (onRadialMenuEnabled == null)
		{
			return;
		}
		onRadialMenuEnabled();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004BC84 File Offset: 0x00049E84
	public void Disable()
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this.IsEnabled = false;
		if (this.menuToggleOverTime)
		{
			if (this.toggleOutDuration <= 0f)
			{
				this.canvasGroup.alpha = 0f;
				if (this.menuToggleScale)
				{
					this.BaseTransform.localScale = Vector3.zero;
				}
				if (this.toggleInDuration > 0f && this.toggleIn)
				{
					this.toggleIn = false;
					this.toggleLerpValue = 0f;
				}
			}
			else
			{
				this.toggleOut = true;
				this.toggleAlphaValue = this.canvasGroup.alpha;
				this.toggleScaleValue = this.BaseTransform.localScale.x;
				this.toggleOutSpeed = 1f / this.toggleOutDuration;
				this.toggleLerpValue = 0f;
				if (this.toggleIn)
				{
					this.toggleIn = false;
				}
			}
		}
		else
		{
			this.canvasGroup.alpha = 0f;
			this.Interactable = false;
		}
		if (this.usePointer && this.pointerImage != null)
		{
			this.PointerActive = false;
			this.pointerFadeIn = false;
			this.pointerFadeOut = false;
			this.pointerImage.color = this.pointerNormalColor;
			this.pointerLerpValue = 0f;
		}
		this.inputInRangeLastFrame = (this.InputInRange = false);
		Action onMenuDisabled = this.OnMenuDisabled;
		if (onMenuDisabled != null)
		{
			onMenuDisabled();
		}
		Action onRadialMenuDisabled = this.OnRadialMenuDisabled;
		if (onRadialMenuDisabled != null)
		{
			onRadialMenuDisabled();
		}
		this.ResetRadialMenu();
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0004BE04 File Offset: 0x0004A004
	public void DisableImmediate()
	{
		this.IsEnabled = false;
		this.ResetRadialMenu();
		if (this.menuToggleOverTime)
		{
			this.canvasGroup.alpha = 0f;
			if (this.menuToggleScale)
			{
				this.BaseTransform.localScale = Vector3.zero;
			}
			if (this.toggleInDuration > 0f && this.toggleIn)
			{
				this.toggleIn = false;
				this.toggleLerpValue = 0f;
			}
		}
		else
		{
			this.canvasGroup.alpha = 0f;
		}
		Action onMenuDisabled = this.OnMenuDisabled;
		if (onMenuDisabled != null)
		{
			onMenuDisabled();
		}
		Action onRadialMenuDisabled = this.OnRadialMenuDisabled;
		if (onRadialMenuDisabled == null)
		{
			return;
		}
		onRadialMenuDisabled();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0004BEA8 File Offset: 0x0004A0A8
	public void CreateEmptyButton()
	{
		this.CreateRadialButtonAtIndex(1000);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
	public void ClearMenu(int buttonCount = 0)
	{
		if (this.radialMenuStyle != null && buttonCount < this.radialMenuStyle.minButtonCount)
		{
			buttonCount = this.radialMenuStyle.minButtonCount;
		}
		for (int i = this.UltimateRadialButtonList.Count - 1; i >= 0; i--)
		{
			if (this.UltimateRadialButtonList[i].buttonDisabled)
			{
				this.UltimateRadialButtonList[i].OnEnable();
			}
			this.UltimateRadialButtonList[i].ClearButtonInformation();
			if (i >= buttonCount)
			{
				this.SendRadialButtonToPool(i);
			}
		}
		this.RadialMenuButtonCountModified();
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0004BF4C File Offset: 0x0004A14C
	public void RemoveButton(int buttonIndex)
	{
		if (this.UltimateRadialButtonList.Count <= 0)
		{
			return;
		}
		if (buttonIndex > this.UltimateRadialButtonList.Count)
		{
			buttonIndex = this.UltimateRadialButtonList.Count - 1;
		}
		this.UltimateRadialButtonList[buttonIndex].ClearButtonInformation();
		if (this.radialMenuStyle == null || this.UltimateRadialButtonList.Count > this.radialMenuStyle.minButtonCount)
		{
			this.SendRadialButtonToPool(buttonIndex);
			this.RadialMenuButtonCountModified();
		}
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0004BFCC File Offset: 0x0004A1CC
	public void ClearButtonInformations()
	{
		for (int i = 0; i < this.UltimateRadialButtonList.Count; i++)
		{
			if (this.UltimateRadialButtonList[i].buttonDisabled)
			{
				this.UltimateRadialButtonList[i].OnEnable();
			}
			this.UltimateRadialButtonList[i].ClearButtonInformation();
		}
		this.ResetRadialMenu();
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0004C02C File Offset: 0x0004A22C
	public void SetPosition(Vector3 position, bool local = false)
	{
		if (this.BaseTransform != null)
		{
			if (this.IsWorldSpaceRadialMenu)
			{
				if (this.ParentCanvas == null)
				{
					this.UpdateParentCanvas();
					if (this.ParentCanvas == null)
					{
						Debug.LogError(UltimateRadialMenu.FormatDebug("There is no parent canvas object, which means this object is not placed within a Canvas object in your scene", "Please make sure that the Ultimate Radial Menu is placed within a canvas", base.gameObject.name));
						return;
					}
				}
				this.ParentCanvas.GetComponent<RectTransform>().position = position;
				return;
			}
			if (local)
			{
				this.BaseTransform.localPosition = position;
				return;
			}
			this.BaseTransform.position = position;
		}
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0004C0BF File Offset: 0x0004A2BF
	public void ResetPosition()
	{
		if (this.BaseTransform != null)
		{
			this.BaseTransform.position = this.defaultPosition;
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0004C0E0 File Offset: 0x0004A2E0
	public void SetParent(Transform parent, Vector3 localPosition, Quaternion localRotation)
	{
		if (this.ParentCanvas == null)
		{
			this.UpdateParentCanvas();
			if (this.ParentCanvas == null)
			{
				Debug.LogError(UltimateRadialMenu.FormatDebug("There is no parent canvas object, which means this object is not placed within a Canvas object in your scene", "Please make sure that the Ultimate Radial Menu is placed within a canvas", base.gameObject.name));
				return;
			}
		}
		this.ParentCanvas.transform.SetParent(parent);
		this.ParentCanvas.GetComponent<RectTransform>().localRotation = localRotation;
		this.ParentCanvas.GetComponent<RectTransform>().localPosition = localPosition;
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0004C162 File Offset: 0x0004A362
	public void EnablePointer()
	{
		this.inputInRangeLastFrame = false;
		this.InputInRange = true;
		this.ProcessPointer();
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0004C178 File Offset: 0x0004A378
	private static string FormatDebug(string error, string solution, string objectName)
	{
		return string.Concat(new string[] { "<b>Ultimate Radial Menu</b>\n<color=red><b>×</b></color> <i><b>Error:</b></i> ", error, ".\n<color=green><b>√</b></color> <i><b>Solution:</b></i> ", solution, ".\n<color=cyan><b>∙</b></color> <i><b>Object:</b></i> ", objectName, "\n" });
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0004C1B4 File Offset: 0x0004A3B4
	private static bool ConfirmUltimateRadialMenu(string radialMenuName)
	{
		if (!UltimateRadialMenu.UltimateRadialMenus.ContainsKey(radialMenuName))
		{
			Debug.LogWarning(UltimateRadialMenu.FormatDebug("There is no Ultimate Radial Menu registered with the name: " + radialMenuName + " in the scene", "Please exit play mode and ensure that your Ultimate Radial Menu has the name: " + radialMenuName + " registered in the <b>Script Reference</b> section", "Unknown (user script)"));
			return false;
		}
		return true;
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x0004C200 File Offset: 0x0004A400
	public static UltimateRadialMenu ReturnComponent(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return null;
		}
		return UltimateRadialMenu.UltimateRadialMenus[radialMenuName];
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0004C217 File Offset: 0x0004A417
	public static void RegisterButton(string radialMenuName, Action ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0004C235 File Offset: 0x0004A435
	public static void RegisterButton(string radialMenuName, Action<int> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0004C253 File Offset: 0x0004A453
	public static void RegisterButton(string radialMenuName, Action<string> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0004C271 File Offset: 0x0004A471
	public static void Enable(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].Enable();
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0004C28C File Offset: 0x0004A48C
	public static void Disable(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].Disable();
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0004C2A7 File Offset: 0x0004A4A7
	public static void DisableImmediate(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].DisableImmediate();
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0004C2C2 File Offset: 0x0004A4C2
	public static void CreateEmptyButton(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].CreateEmptyButton();
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0004C2DD File Offset: 0x0004A4DD
	public static void ClearMenu(string radialMenuName, int buttonCount = 0)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].ClearMenu(buttonCount);
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0004C2F9 File Offset: 0x0004A4F9
	public static void RemoveButton(string radialMenuName, int buttonIndex)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RemoveButton(buttonIndex);
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0004C315 File Offset: 0x0004A515
	public static void ClearButtonInformations(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].ClearButtonInformations();
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0004C330 File Offset: 0x0004A530
	public static void SetPosition(string radialMenuName, Vector3 position, bool local = false)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].SetPosition(position, local);
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0004C34D File Offset: 0x0004A54D
	public static void ResetPosition(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].ResetPosition();
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0004C368 File Offset: 0x0004A568
	public static void SetParent(string radialMenuName, Transform parent, Vector3 localPosition, Quaternion localRotation)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].SetParent(parent, localPosition, localRotation);
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0004C386 File Offset: 0x0004A586
	[Obsolete("Please use the IsEnabled property.")]
	public bool RadialMenuActive
	{
		get
		{
			return this.IsEnabled;
		}
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0004BA87 File Offset: 0x00049C87
	[Obsolete("Please use the RegisterButton function.")]
	public void RegisterToRadialMenu(Action ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0004BAB5 File Offset: 0x00049CB5
	[Obsolete("Please use the RegisterButton function.")]
	public void RegisterToRadialMenu(Action<int> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0004BAE3 File Offset: 0x00049CE3
	[Obsolete("Please use the RegisterButton function.")]
	public void RegisterToRadialMenu(Action<string> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		this.FindRadialButtonIndex(ref buttonIndex);
		this.UltimateRadialButtonList[buttonIndex].RegisterButtonInfo(buttonInfo);
		this.UltimateRadialButtonList[buttonIndex].AddCallback(ButtonCallback);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0004C38E File Offset: 0x0004A58E
	[Obsolete("Please use the Enable function.")]
	public void EnableRadialMenu()
	{
		this.Enable();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0004C396 File Offset: 0x0004A596
	[Obsolete("Please use the Disable function.")]
	public void DisableRadialMenu()
	{
		this.Disable();
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0004C39E File Offset: 0x0004A59E
	[Obsolete("Please use the DisableImmediate function.")]
	public void DisableRadialMenuImmediate()
	{
		this.DisableImmediate();
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0004C3A6 File Offset: 0x0004A5A6
	[Obsolete("Please use the CreateEmptyButton function.")]
	public void CreateEmptyRadialButton()
	{
		this.CreateEmptyButton();
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0004C3AE File Offset: 0x0004A5AE
	[Obsolete("Please use the ClearMenu function.")]
	public void RemoveAllRadialButtons(int buttonCount = 0)
	{
		this.ClearMenu(buttonCount);
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0004C3B7 File Offset: 0x0004A5B7
	[Obsolete("Please use the RemoveButton function.")]
	public void RemoveRadialButton(int buttonIndex)
	{
		this.RemoveButton(buttonIndex);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0004C3C0 File Offset: 0x0004A5C0
	[Obsolete("Please use the ClearButtonInformations function.")]
	public void ClearRadialButtonInformations()
	{
		this.ClearButtonInformations();
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0004C200 File Offset: 0x0004A400
	[Obsolete("Please use the ReturnComponent function.")]
	public static UltimateRadialMenu GetUltimateRadialMenu(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return null;
		}
		return UltimateRadialMenu.UltimateRadialMenus[radialMenuName];
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004C217 File Offset: 0x0004A417
	[Obsolete("Please use the RegisterButton function.")]
	public static void RegisterToRadialMenu(string radialMenuName, Action ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0004C235 File Offset: 0x0004A435
	[Obsolete("Please use the RegisterButton function.")]
	public static void RegisterToRadialMenu(string radialMenuName, Action<int> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0004C253 File Offset: 0x0004A453
	[Obsolete("Please use the RegisterButton function.")]
	public static void RegisterToRadialMenu(string radialMenuName, Action<string> ButtonCallback, UltimateRadialButtonInfo buttonInfo, int buttonIndex = -1)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RegisterButton(ButtonCallback, buttonInfo, buttonIndex);
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0004C271 File Offset: 0x0004A471
	[Obsolete("Please use the Enable function.")]
	public static void EnableRadialMenu(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].Enable();
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0004C28C File Offset: 0x0004A48C
	[Obsolete("Please use the Disable function.")]
	public static void DisableRadialMenu(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].Disable();
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0004C2A7 File Offset: 0x0004A4A7
	[Obsolete("Please use the DisableImmediate function.")]
	public static void DisableRadialMenuImmediate(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].DisableImmediate();
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0004C2C2 File Offset: 0x0004A4C2
	[Obsolete("Please use the CreateEmptyButton function.")]
	public static void CreateEmptyRadialButton(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].CreateEmptyButton();
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0004C3C8 File Offset: 0x0004A5C8
	[Obsolete("Please use the ClearMenu function.")]
	public static void RemoveAllRadialButtons(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].ClearMenu(0);
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0004C2F9 File Offset: 0x0004A4F9
	[Obsolete("Please use the RemoveButton function.")]
	public static void RemoveRadialButton(string radialMenuName, int buttonIndex)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].RemoveButton(buttonIndex);
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0004C315 File Offset: 0x0004A515
	[Obsolete("Please use the ClearButtonInformations function.")]
	public static void ClearRadialButtonInformations(string radialMenuName)
	{
		if (!UltimateRadialMenu.ConfirmUltimateRadialMenu(radialMenuName))
		{
			return;
		}
		UltimateRadialMenu.UltimateRadialMenus[radialMenuName].ClearButtonInformations();
	}

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000B29 RID: 2857 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
	// (remove) Token: 0x06000B2A RID: 2858 RVA: 0x0004C41C File Offset: 0x0004A61C
	[Obsolete("Please use the OnButtonEnter callback.")]
	public event Action<int> OnRadialButtonEnter;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000B2B RID: 2859 RVA: 0x0004C454 File Offset: 0x0004A654
	// (remove) Token: 0x06000B2C RID: 2860 RVA: 0x0004C48C File Offset: 0x0004A68C
	[Obsolete("Please use the OnButtonExit callback.")]
	public event Action<int> OnRadialButtonExit;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000B2D RID: 2861 RVA: 0x0004C4C4 File Offset: 0x0004A6C4
	// (remove) Token: 0x06000B2E RID: 2862 RVA: 0x0004C4FC File Offset: 0x0004A6FC
	[Obsolete("Please use the OnButtonInputDown callback.")]
	public event Action<int> OnRadialButtonInputDown;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000B2F RID: 2863 RVA: 0x0004C534 File Offset: 0x0004A734
	// (remove) Token: 0x06000B30 RID: 2864 RVA: 0x0004C56C File Offset: 0x0004A76C
	[Obsolete("Please use the OnButtonInputUp callback.")]
	public event Action<int> OnRadialButtonInputUp;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000B31 RID: 2865 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
	// (remove) Token: 0x06000B32 RID: 2866 RVA: 0x0004C5DC File Offset: 0x0004A7DC
	[Obsolete("Please use the OnButtonInteract callback.")]
	public event Action<int> OnRadialButtonInteract;

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000B33 RID: 2867 RVA: 0x0004C614 File Offset: 0x0004A814
	// (remove) Token: 0x06000B34 RID: 2868 RVA: 0x0004C64C File Offset: 0x0004A84C
	[Obsolete("Please use the OnButtonSelected callback.")]
	public event Action<int> OnRadialButtonSelected;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000B35 RID: 2869 RVA: 0x0004C684 File Offset: 0x0004A884
	// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x0004C6BC File Offset: 0x0004A8BC
	[Obsolete("Please use the OnMenuLostFocus callback.")]
	public event Action OnRadialMenuLostFocus;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06000B37 RID: 2871 RVA: 0x0004C6F4 File Offset: 0x0004A8F4
	// (remove) Token: 0x06000B38 RID: 2872 RVA: 0x0004C72C File Offset: 0x0004A92C
	[Obsolete("Please use the OnMenuEnabled callback.")]
	public event Action OnRadialMenuEnabled;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06000B39 RID: 2873 RVA: 0x0004C764 File Offset: 0x0004A964
	// (remove) Token: 0x06000B3A RID: 2874 RVA: 0x0004C79C File Offset: 0x0004A99C
	[Obsolete("Please use the OnMenuDisabled callback.")]
	public event Action OnRadialMenuDisabled;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000B3B RID: 2875 RVA: 0x0004C7D4 File Offset: 0x0004A9D4
	// (remove) Token: 0x06000B3C RID: 2876 RVA: 0x0004C80C File Offset: 0x0004AA0C
	[Obsolete("Please use the OnButtonCountModified callback.")]
	public event Action<int> OnRadialMenuButtonCountModified;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000B3D RID: 2877 RVA: 0x0004C844 File Offset: 0x0004AA44
	// (remove) Token: 0x06000B3E RID: 2878 RVA: 0x0004C87C File Offset: 0x0004AA7C
	[Obsolete("Please use the OnProcessInput callback.")]
	public event Action<Vector2, float, bool, bool> OnRadialMenuProcessInput;

	// Token: 0x040009EF RID: 2543
	private bool inputInRangeLastFrame;

	// Token: 0x040009F0 RID: 2544
	private int buttonIndexOnInputDown = -1;

	// Token: 0x040009F1 RID: 2545
	private Vector3 defaultPosition;

	// Token: 0x040009F2 RID: 2546
	private RectTransform canvasRectTrans;

	// Token: 0x040009F3 RID: 2547
	private Vector2 parentCanvasSize;

	// Token: 0x040009F4 RID: 2548
	public UltimateRadialMenu.ScalingAxis scalingAxis = UltimateRadialMenu.ScalingAxis.Height;

	// Token: 0x040009F5 RID: 2549
	public float menuSize = 5f;

	// Token: 0x040009F6 RID: 2550
	public float horizontalPosition = 50f;

	// Token: 0x040009F7 RID: 2551
	public float verticalPosition = 50f;

	// Token: 0x040009F8 RID: 2552
	public float depthPosition;

	// Token: 0x040009F9 RID: 2553
	public float menuButtonSize = 0.25f;

	// Token: 0x040009FA RID: 2554
	public float radialMenuButtonRadius = 1f;

	// Token: 0x040009FB RID: 2555
	public float startingAngle;

	// Token: 0x040009FC RID: 2556
	public float overallAngle = 360f;

	// Token: 0x040009FD RID: 2557
	public float centerAngle;

	// Token: 0x040009FE RID: 2558
	public UltimateRadialMenu.AngleOffset angleOffset;

	// Token: 0x040009FF RID: 2559
	public bool followOrbitalRotation = true;

	// Token: 0x04000A00 RID: 2560
	public float minRange = 0.25f;

	// Token: 0x04000A01 RID: 2561
	public float maxRange = 1.5f;

	// Token: 0x04000A02 RID: 2562
	public bool infiniteMaxRange;

	// Token: 0x04000A03 RID: 2563
	public float buttonInputAngle;

	// Token: 0x04000A04 RID: 2564
	[SerializeField]
	private Vector2 centerOffset = -Vector2.one;

	// Token: 0x04000A05 RID: 2565
	public UltimateRadialMenuStyle radialMenuStyle;

	// Token: 0x04000A06 RID: 2566
	private int currentStyleIndex;

	// Token: 0x04000A07 RID: 2567
	public Sprite normalSprite;

	// Token: 0x04000A08 RID: 2568
	public Color normalColor = Color.white;

	// Token: 0x04000A09 RID: 2569
	public UltimateRadialMenu.InitialState initialState;

	// Token: 0x04000A0A RID: 2570
	[SerializeField]
	[Tooltip("Determines if the menu should toggle itself from enabled and disabled over time or not.")]
	private bool menuToggleOverTime;

	// Token: 0x04000A0B RID: 2571
	[SerializeField]
	[Tooltip("Should the menu toggle using the canvas group alpha?")]
	private bool menuToggleAlpha = true;

	// Token: 0x04000A0C RID: 2572
	[SerializeField]
	[Tooltip("Should the menu toggle by scaling the buttons?")]
	private bool menuToggleScale;

	// Token: 0x04000A0D RID: 2573
	[SerializeField]
	[Tooltip("The time in seconds to enable the menu.")]
	private float toggleInDuration = 0.25f;

	// Token: 0x04000A0E RID: 2574
	[SerializeField]
	[Tooltip("The time in seconds to disable the menu.")]
	private float toggleOutDuration = 0.25f;

	// Token: 0x04000A0F RID: 2575
	private CanvasGroup canvasGroup;

	// Token: 0x04000A10 RID: 2576
	private bool toggleIn;

	// Token: 0x04000A11 RID: 2577
	private bool toggleOut;

	// Token: 0x04000A12 RID: 2578
	private float toggleLerpValue;

	// Token: 0x04000A13 RID: 2579
	private float toggleAlphaValue;

	// Token: 0x04000A14 RID: 2580
	private float toggleScaleValue;

	// Token: 0x04000A15 RID: 2581
	private float toggleInSpeed;

	// Token: 0x04000A16 RID: 2582
	private float toggleOutSpeed;

	// Token: 0x04000A18 RID: 2584
	private bool pointerFadeIn;

	// Token: 0x04000A19 RID: 2585
	private bool pointerFadeOut;

	// Token: 0x04000A1A RID: 2586
	private float pointerLerpValue;

	// Token: 0x04000A1B RID: 2587
	private Color pointerFadeColor = Color.white;

	// Token: 0x04000A1C RID: 2588
	private float fadeInSpeed;

	// Token: 0x04000A1D RID: 2589
	private float fadeOutSpeed;

	// Token: 0x04000A1E RID: 2590
	[SerializeField]
	[Tooltip("Should an image be used to point to the current radial button?")]
	private bool usePointer;

	// Token: 0x04000A1F RID: 2591
	[SerializeField]
	[Tooltip("The image to be used for the pointer.")]
	private Image pointerImage;

	// Token: 0x04000A20 RID: 2592
	private RectTransform pointerTransform;

	// Token: 0x04000A21 RID: 2593
	[SerializeField]
	[Tooltip("The size of the pointer image.")]
	[Range(0f, 2f)]
	private float pointerSize = 1f;

	// Token: 0x04000A22 RID: 2594
	[SerializeField]
	[Tooltip("The time in seconds for the pointer to target the new button.")]
	private float pointerTargetTime = 0.25f;

	// Token: 0x04000A23 RID: 2595
	private float pointerTargetSpeed = 5f;

	// Token: 0x04000A24 RID: 2596
	private Quaternion pointerTargetRotation;

	// Token: 0x04000A25 RID: 2597
	[SerializeField]
	[Tooltip("Determines how the pointer will interact with the input and the buttons.")]
	private UltimateRadialMenu.PointerSnapOption pointerSnapOption = UltimateRadialMenu.PointerSnapOption.Smooth;

	// Token: 0x04000A26 RID: 2598
	[SerializeField]
	[Tooltip("The rotation offset to apply to the pointer image.")]
	private float pointerRotationOffset = 90f;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	[Tooltip("Determines if the pointer should change color or not.")]
	private bool pointerColorChange;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	[Tooltip("Should the pointer change color over time or instantly?")]
	private bool changeOverTime;

	// Token: 0x04000A29 RID: 2601
	[SerializeField]
	[Tooltip("The time in seconds that the pointer should fade in.")]
	private float fadeInDuration = 0.25f;

	// Token: 0x04000A2A RID: 2602
	[SerializeField]
	[Tooltip("The time in seconds that the pointer should fade out.")]
	private float fadeOutDuration = 0.5f;

	// Token: 0x04000A2B RID: 2603
	[SerializeField]
	[Tooltip("The default color of the pointer image.")]
	private Color pointerNormalColor = Color.white;

	// Token: 0x04000A2C RID: 2604
	[SerializeField]
	[Tooltip("The active color of the pointer image.")]
	private Color pointerActiveColor = Color.white;

	// Token: 0x04000A2D RID: 2605
	[SerializeField]
	[Tooltip("Determines if the sibling index should be set for the pointer object or not, and where it should be placed.")]
	private UltimateRadialMenu.SetSiblingIndex pointerSiblingIndex;

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	[Tooltip("Should this pointer use a pointer style for changing the sprite according to the button count?")]
	private bool usePointerStyle;

	// Token: 0x04000A2F RID: 2607
	public List<UltimateRadialMenu.PointerStyle> PointerStyles = new List<UltimateRadialMenu.PointerStyle>();

	// Token: 0x04000A30 RID: 2608
	public bool displayButtonName;

	// Token: 0x04000A31 RID: 2609
	public Text nameText;

	// Token: 0x04000A32 RID: 2610
	public float nameTextRatioX = 1f;

	// Token: 0x04000A33 RID: 2611
	public float nameTextRatioY = 1f;

	// Token: 0x04000A34 RID: 2612
	public float nameTextSize = 0.25f;

	// Token: 0x04000A35 RID: 2613
	public float nameTextHorizontalPosition = 50f;

	// Token: 0x04000A36 RID: 2614
	public float nameTextVerticalPosition = 50f;

	// Token: 0x04000A37 RID: 2615
	public bool displayButtonDescription;

	// Token: 0x04000A38 RID: 2616
	public Text descriptionText;

	// Token: 0x04000A39 RID: 2617
	public float descriptionTextRatioX = 1f;

	// Token: 0x04000A3A RID: 2618
	public float descriptionTextRatioY = 1f;

	// Token: 0x04000A3B RID: 2619
	public float descriptionTextSize = 0.25f;

	// Token: 0x04000A3C RID: 2620
	public float descriptionTextHorizontalPosition = 50f;

	// Token: 0x04000A3D RID: 2621
	public float descriptionTextVerticalPosition = 50f;

	// Token: 0x04000A3E RID: 2622
	public bool useButtonIcon;

	// Token: 0x04000A3F RID: 2623
	public float iconSize = 0.25f;

	// Token: 0x04000A40 RID: 2624
	public float iconRotation;

	// Token: 0x04000A41 RID: 2625
	public float iconHorizontalPosition = 50f;

	// Token: 0x04000A42 RID: 2626
	public float iconVerticalPosition = 50f;

	// Token: 0x04000A43 RID: 2627
	public bool iconLocalRotation;

	// Token: 0x04000A44 RID: 2628
	public bool iconSmartRotation;

	// Token: 0x04000A45 RID: 2629
	public Color iconNormalColor = Color.white;

	// Token: 0x04000A46 RID: 2630
	public bool useButtonText;

	// Token: 0x04000A47 RID: 2631
	public float textAreaRatioX = 1f;

	// Token: 0x04000A48 RID: 2632
	public float textAreaRatioY = 0.25f;

	// Token: 0x04000A49 RID: 2633
	public float textSize = 0.25f;

	// Token: 0x04000A4A RID: 2634
	public float textHorizontalPosition = 50f;

	// Token: 0x04000A4B RID: 2635
	public float textVerticalPosition = 50f;

	// Token: 0x04000A4C RID: 2636
	public bool displayNameOnButton = true;

	// Token: 0x04000A4D RID: 2637
	public bool textLocalPosition = true;

	// Token: 0x04000A4E RID: 2638
	public bool textLocalRotation = true;

	// Token: 0x04000A4F RID: 2639
	public Color textNormalColor = Color.white;

	// Token: 0x04000A50 RID: 2640
	public bool relativeToIcon;

	// Token: 0x04000A51 RID: 2641
	public Font nameFont;

	// Token: 0x04000A52 RID: 2642
	public Font descriptionFont;

	// Token: 0x04000A53 RID: 2643
	public Font buttonTextFont;

	// Token: 0x04000A54 RID: 2644
	public bool nameOutline;

	// Token: 0x04000A55 RID: 2645
	public bool descriptionOutline;

	// Token: 0x04000A56 RID: 2646
	public bool buttonTextOutline;

	// Token: 0x04000A57 RID: 2647
	public Color buttonTextOutlineColor = Color.white;

	// Token: 0x04000A58 RID: 2648
	public bool spriteSwap;

	// Token: 0x04000A59 RID: 2649
	public bool colorChange = true;

	// Token: 0x04000A5A RID: 2650
	public bool scaleTransform;

	// Token: 0x04000A5B RID: 2651
	public bool iconColorChange;

	// Token: 0x04000A5C RID: 2652
	public bool iconScaleTransform;

	// Token: 0x04000A5D RID: 2653
	public bool textColorChange;

	// Token: 0x04000A5E RID: 2654
	public Sprite highlightedSprite;

	// Token: 0x04000A5F RID: 2655
	public Color highlightedColor = Color.white;

	// Token: 0x04000A60 RID: 2656
	public float highlightedScaleModifier = 1.1f;

	// Token: 0x04000A61 RID: 2657
	public float positionModifier;

	// Token: 0x04000A62 RID: 2658
	public Color iconHighlightedColor = Color.white;

	// Token: 0x04000A63 RID: 2659
	public float iconHighlightedScaleModifier = 1.1f;

	// Token: 0x04000A64 RID: 2660
	public Color textHighlightedColor = Color.white;

	// Token: 0x04000A65 RID: 2661
	public Sprite pressedSprite;

	// Token: 0x04000A66 RID: 2662
	public Color pressedColor = Color.white;

	// Token: 0x04000A67 RID: 2663
	public float pressedScaleModifier = 1.05f;

	// Token: 0x04000A68 RID: 2664
	public float pressedPositionModifier;

	// Token: 0x04000A69 RID: 2665
	public Color iconPressedColor = Color.white;

	// Token: 0x04000A6A RID: 2666
	public float iconPressedScaleModifier = 1f;

	// Token: 0x04000A6B RID: 2667
	public Color textPressedColor = Color.white;

	// Token: 0x04000A6C RID: 2668
	public bool selectButtonOnInteract;

	// Token: 0x04000A6D RID: 2669
	public bool toggleSelection;

	// Token: 0x04000A6E RID: 2670
	public bool allowMultipleSelected;

	// Token: 0x04000A6F RID: 2671
	public Sprite selectedSprite;

	// Token: 0x04000A70 RID: 2672
	public Color selectedColor = Color.white;

	// Token: 0x04000A71 RID: 2673
	public float selectedScaleModifier = 1f;

	// Token: 0x04000A72 RID: 2674
	public float selectedPositionModifier;

	// Token: 0x04000A73 RID: 2675
	public Color iconSelectedColor = Color.white;

	// Token: 0x04000A74 RID: 2676
	public float iconSelectedScaleModifier = 1f;

	// Token: 0x04000A75 RID: 2677
	public Color textSelectedColor = Color.white;

	// Token: 0x04000A76 RID: 2678
	public Sprite disabledSprite;

	// Token: 0x04000A77 RID: 2679
	public Color disabledColor = Color.white;

	// Token: 0x04000A78 RID: 2680
	public float disabledScaleModifier = 1f;

	// Token: 0x04000A79 RID: 2681
	public float disabledPositionModifier;

	// Token: 0x04000A7A RID: 2682
	public Color iconDisabledColor = Color.white;

	// Token: 0x04000A7B RID: 2683
	public float iconDisabledScaleModifier = 1f;

	// Token: 0x04000A7C RID: 2684
	public Color textDisabledColor = Color.white;

	// Token: 0x04000A7D RID: 2685
	public List<UltimateRadialMenu.UltimateRadialButton> UltimateRadialButtonList = new List<UltimateRadialMenu.UltimateRadialButton>();

	// Token: 0x04000A7E RID: 2686
	private List<UltimateRadialMenu.UltimateRadialButton> UltimateRadialButtonPool = new List<UltimateRadialMenu.UltimateRadialButton>();

	// Token: 0x04000A7F RID: 2687
	public UltimateRadialMenuInputManager inputManager;

	// Token: 0x04000A80 RID: 2688
	[NonSerialized]
	internal UltimateRadialMenuInputManager.UltimateRadialMenuInformation inputInformation;

	// Token: 0x04000A81 RID: 2689
	private static Dictionary<string, UltimateRadialMenu> UltimateRadialMenus = new Dictionary<string, UltimateRadialMenu>();

	// Token: 0x04000A82 RID: 2690
	public string radialMenuName = string.Empty;

	// Token: 0x04000A86 RID: 2694
	private float buttonHoverTime;

	// Token: 0x02000154 RID: 340
	public enum ScalingAxis
	{
		// Token: 0x04000A9E RID: 2718
		Width,
		// Token: 0x04000A9F RID: 2719
		Height
	}

	// Token: 0x02000155 RID: 341
	public enum AngleOffset
	{
		// Token: 0x04000AA1 RID: 2721
		Centered,
		// Token: 0x04000AA2 RID: 2722
		OffCenter,
		// Token: 0x04000AA3 RID: 2723
		OnlyEven,
		// Token: 0x04000AA4 RID: 2724
		OnlyOdd
	}

	// Token: 0x02000156 RID: 342
	public enum InitialState
	{
		// Token: 0x04000AA6 RID: 2726
		Enabled,
		// Token: 0x04000AA7 RID: 2727
		Disabled
	}

	// Token: 0x02000157 RID: 343
	public enum PointerSnapOption
	{
		// Token: 0x04000AA9 RID: 2729
		Instant,
		// Token: 0x04000AAA RID: 2730
		Smooth,
		// Token: 0x04000AAB RID: 2731
		Free
	}

	// Token: 0x02000158 RID: 344
	public enum SetSiblingIndex
	{
		// Token: 0x04000AAD RID: 2733
		Disabled,
		// Token: 0x04000AAE RID: 2734
		First,
		// Token: 0x04000AAF RID: 2735
		Last
	}

	// Token: 0x02000159 RID: 345
	[Serializable]
	public class PointerStyle
	{
		// Token: 0x04000AB0 RID: 2736
		public int buttonCount;

		// Token: 0x04000AB1 RID: 2737
		public Sprite pointerSprite;
	}

	// Token: 0x0200015A RID: 346
	[Serializable]
	public class UltimateRadialButton
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0004CBED File Offset: 0x0004ADED
		public bool Registered
		{
			get
			{
				return this.registered || (this.unityEvent != null && this.unityEvent.GetPersistentEventCount() > 0);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0004CC12 File Offset: 0x0004AE12
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0004CC1A File Offset: 0x0004AE1A
		public bool Selected { get; private set; }

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000B45 RID: 2885 RVA: 0x0004CC24 File Offset: 0x0004AE24
		// (remove) Token: 0x06000B46 RID: 2886 RVA: 0x0004CC5C File Offset: 0x0004AE5C
		public event Action OnRadialButtonInteract;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000B47 RID: 2887 RVA: 0x0004CC94 File Offset: 0x0004AE94
		// (remove) Token: 0x06000B48 RID: 2888 RVA: 0x0004CCCC File Offset: 0x0004AECC
		public event Action<int> OnRadialButtonInteractWithId;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000B49 RID: 2889 RVA: 0x0004CD04 File Offset: 0x0004AF04
		// (remove) Token: 0x06000B4A RID: 2890 RVA: 0x0004CD3C File Offset: 0x0004AF3C
		public event Action<string> OnRadialButtonInteractWithKey;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000B4B RID: 2891 RVA: 0x0004CD74 File Offset: 0x0004AF74
		// (remove) Token: 0x06000B4C RID: 2892 RVA: 0x0004CDAC File Offset: 0x0004AFAC
		public event Action OnClearButtonInformation;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000B4D RID: 2893 RVA: 0x0004CDE4 File Offset: 0x0004AFE4
		// (remove) Token: 0x06000B4E RID: 2894 RVA: 0x0004CE1C File Offset: 0x0004B01C
		private event Action<bool> OnSelectedStateChanged;

		// Token: 0x06000B4F RID: 2895 RVA: 0x0004CE54 File Offset: 0x0004B054
		public bool IsInAngle(float inputAngle)
		{
			return Mathf.Abs(inputAngle - this.angle) <= this.angleRange || Mathf.Abs(inputAngle - 360f - this.angle) <= this.angleRange || Mathf.Abs(inputAngle - (this.angle - 360f)) <= this.angleRange;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0004CEB0 File Offset: 0x0004B0B0
		public void OnEnter()
		{
			if (this.buttonDisabled)
			{
				return;
			}
			if (!this.Selected)
			{
				if (this.radialMenu.spriteSwap)
				{
					if (this.radialMenu.highlightedSprite != null)
					{
						this.radialImage.sprite = this.radialMenu.highlightedSprite;
					}
					else
					{
						this.radialImage.sprite = this.radialMenu.normalSprite;
					}
				}
				if (this.radialMenu.colorChange && this.radialImage.sprite != null)
				{
					this.radialImage.color = this.radialMenu.highlightedColor;
				}
				if (this.radialMenu.scaleTransform)
				{
					this.buttonTransform.localScale = Vector3.one * this.radialMenu.highlightedScaleModifier;
					this.buttonTransform.localPosition = this.highlightedPosition;
				}
				if (this.radialMenu.useButtonIcon && this.icon != null && this.icon.sprite != null)
				{
					if (this.radialMenu.iconColorChange)
					{
						this.icon.color = this.radialMenu.iconHighlightedColor;
					}
					if (this.radialMenu.iconScaleTransform)
					{
						this.iconTransform.localScale = this.iconNormalScale * this.radialMenu.iconHighlightedScaleModifier;
					}
				}
				if (this.radialMenu.useButtonText && this.text != null && this.radialMenu.textColorChange)
				{
					this.text.color = this.radialMenu.textHighlightedColor;
				}
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0004D050 File Offset: 0x0004B250
		public void OnExit()
		{
			if (this.buttonDisabled)
			{
				return;
			}
			if (!Application.isPlaying || !this.Selected)
			{
				if (this.radialMenu.spriteSwap && this.radialMenu.normalSprite != null)
				{
					this.radialImage.sprite = this.radialMenu.normalSprite;
				}
				if (this.radialMenu.colorChange && this.radialImage.sprite != null)
				{
					this.radialImage.color = this.radialMenu.normalColor;
				}
				if (this.radialMenu.scaleTransform)
				{
					this.radialImage.GetComponent<RectTransform>().localScale = Vector3.one;
					this.radialImage.GetComponent<RectTransform>().localPosition = this.normalPosition;
				}
				if (this.radialMenu.useButtonIcon && this.icon != null && this.icon.sprite != null)
				{
					if (this.radialMenu.iconColorChange)
					{
						this.icon.color = this.radialMenu.iconNormalColor;
					}
					if (this.radialMenu.iconScaleTransform)
					{
						this.iconTransform.localScale = this.iconNormalScale;
					}
				}
				if (this.radialMenu.useButtonText && this.text != null && this.radialMenu.textColorChange)
				{
					this.text.color = this.radialMenu.textNormalColor;
				}
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0004D1CC File Offset: 0x0004B3CC
		public void OnInputDown()
		{
			if (this.buttonDisabled)
			{
				return;
			}
			if (this.radialMenu.spriteSwap && this.radialMenu.pressedSprite != null)
			{
				this.radialImage.sprite = this.radialMenu.pressedSprite;
			}
			if (this.radialMenu.colorChange && this.radialImage.sprite != null)
			{
				this.radialImage.color = this.radialMenu.pressedColor;
			}
			if (this.radialMenu.scaleTransform)
			{
				this.radialImage.GetComponent<RectTransform>().localScale = Vector3.one * this.radialMenu.pressedScaleModifier;
				this.radialImage.GetComponent<RectTransform>().localPosition = this.pressedPosition;
			}
			if (this.radialMenu.useButtonIcon && this.icon != null && this.icon.sprite != null)
			{
				if (this.radialMenu.iconColorChange)
				{
					this.icon.color = this.radialMenu.iconPressedColor;
				}
				if (this.radialMenu.iconScaleTransform)
				{
					this.iconTransform.localScale = this.iconNormalScale * this.radialMenu.iconPressedScaleModifier;
				}
			}
			if (this.radialMenu.useButtonText && this.text != null && this.radialMenu.textColorChange)
			{
				this.text.color = this.radialMenu.textPressedColor;
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0004D353 File Offset: 0x0004B553
		public void OnInputUp()
		{
			if (this.buttonDisabled)
			{
				return;
			}
			if (this.buttonIndex == this.radialMenu.CurrentButtonIndex)
			{
				this.OnEnter();
				return;
			}
			this.OnExit();
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0004D380 File Offset: 0x0004B580
		public void OnInteract()
		{
			if (this.buttonDisabled)
			{
				return;
			}
			if (this.radialMenu.selectButtonOnInteract)
			{
				if (this.radialMenu.toggleSelection && this.Selected)
				{
					this.OnDeselect();
				}
				else
				{
					this.OnSelect();
				}
			}
			if (this.unityEvent != null)
			{
				this.unityEvent.Invoke();
			}
			if (this.OnRadialButtonInteract != null)
			{
				this.OnRadialButtonInteract();
			}
			else if (this.OnRadialButtonInteractWithId != null)
			{
				this.OnRadialButtonInteractWithId(this.id);
			}
			else
			{
				Action<string> onRadialButtonInteractWithKey = this.OnRadialButtonInteractWithKey;
				if (onRadialButtonInteractWithKey != null)
				{
					onRadialButtonInteractWithKey(this.key);
				}
			}
			Action<int> onButtonInteract = this.radialMenu.OnButtonInteract;
			if (onButtonInteract != null)
			{
				onButtonInteract(this.buttonIndex);
			}
			Action<int> onRadialButtonInteract = this.radialMenu.OnRadialButtonInteract;
			if (onRadialButtonInteract == null)
			{
				return;
			}
			onRadialButtonInteract(this.buttonIndex);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0004D458 File Offset: 0x0004B658
		public void OnSelect()
		{
			if (this.buttonDisabled || this.Selected)
			{
				return;
			}
			this.Selected = true;
			if (this.radialMenu.selectButtonOnInteract && !this.radialMenu.allowMultipleSelected)
			{
				for (int i = 0; i < this.radialMenu.UltimateRadialButtonList.Count; i++)
				{
					if (i != this.buttonIndex && this.radialMenu.UltimateRadialButtonList[i].Selected)
					{
						this.radialMenu.UltimateRadialButtonList[i].OnDeselect();
					}
				}
			}
			if (this.radialMenu.spriteSwap && this.radialMenu.selectedSprite != null)
			{
				this.radialImage.sprite = this.radialMenu.selectedSprite;
			}
			if (this.radialMenu.colorChange && this.radialImage.sprite != null)
			{
				this.radialImage.color = this.radialMenu.selectedColor;
			}
			if (this.radialMenu.scaleTransform)
			{
				this.buttonTransform.localScale = Vector3.one * this.radialMenu.selectedScaleModifier;
				this.buttonTransform.localPosition = this.selectedPosition;
			}
			if (this.radialMenu.useButtonIcon && this.icon != null && this.icon.sprite != null)
			{
				if (this.radialMenu.iconColorChange)
				{
					this.icon.color = this.radialMenu.iconSelectedColor;
				}
				if (this.radialMenu.iconScaleTransform)
				{
					this.iconTransform.localScale = this.iconNormalScale * this.radialMenu.iconSelectedScaleModifier;
				}
			}
			if (this.radialMenu.useButtonText && this.text != null && this.radialMenu.textColorChange)
			{
				this.text.color = this.radialMenu.textSelectedColor;
			}
			Action<int> onButtonSelected = this.radialMenu.OnButtonSelected;
			if (onButtonSelected != null)
			{
				onButtonSelected(this.buttonIndex);
			}
			Action<int> onRadialButtonSelected = this.radialMenu.OnRadialButtonSelected;
			if (onRadialButtonSelected != null)
			{
				onRadialButtonSelected(this.buttonIndex);
			}
			Action<bool> onSelectedStateChanged = this.OnSelectedStateChanged;
			if (onSelectedStateChanged == null)
			{
				return;
			}
			onSelectedStateChanged(this.Selected);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0004D6A0 File Offset: 0x0004B8A0
		public void OnDeselect()
		{
			if (this.buttonDisabled || !this.Selected)
			{
				return;
			}
			this.Selected = false;
			this.OnExit();
			if (this.IsInAngle(this.radialMenu.GetCurrentInputAngle) && this.radialMenu.GetCurrentInputDistance < this.radialMenu.CalculatedMaxRange && this.radialMenu.GetCurrentInputDistance > this.radialMenu.CalculatedMinRange)
			{
				this.OnEnter();
			}
			Action<bool> onSelectedStateChanged = this.OnSelectedStateChanged;
			if (onSelectedStateChanged == null)
			{
				return;
			}
			onSelectedStateChanged(this.Selected);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0004D72A File Offset: 0x0004B92A
		public void OnEnable()
		{
			if (this.radialImage == null)
			{
				return;
			}
			this.buttonDisabled = false;
			this.OnExit();
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0004D748 File Offset: 0x0004B948
		public void OnDisable()
		{
			if (this.radialImage == null)
			{
				return;
			}
			this.buttonDisabled = true;
			this.Selected = false;
			if (this.radialMenu.spriteSwap && this.radialMenu.disabledSprite != null)
			{
				this.radialImage.sprite = this.radialMenu.disabledSprite;
			}
			if (this.radialMenu.colorChange && this.radialImage.sprite != null)
			{
				this.radialImage.color = this.radialMenu.disabledColor;
			}
			if (this.radialMenu.scaleTransform)
			{
				this.radialImage.GetComponent<RectTransform>().localScale = Vector3.one * this.radialMenu.disabledScaleModifier;
				this.radialImage.GetComponent<RectTransform>().localPosition = this.disabledPosition;
			}
			if (this.radialMenu.useButtonIcon && this.icon != null && this.icon.sprite != null)
			{
				if (this.radialMenu.iconColorChange)
				{
					this.icon.color = this.radialMenu.iconDisabledColor;
				}
				if (this.radialMenu.iconScaleTransform)
				{
					this.iconTransform.localScale = this.iconNormalScale * this.radialMenu.iconDisabledScaleModifier;
				}
			}
			if (this.radialMenu.useButtonText && this.text != null && this.radialMenu.textColorChange)
			{
				this.text.color = this.radialMenu.textDisabledColor;
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0004D8E4 File Offset: 0x0004BAE4
		public void UpdateIcon(Sprite newIcon)
		{
			if (this.icon == null)
			{
				Debug.LogError(UltimateRadialMenu.FormatDebug("The button's icon image component is not assigned", "Please make sure that the radial menu has the <b>Button Icon</b> option enabled", this.radialMenu.gameObject.name));
				return;
			}
			this.icon.sprite = newIcon;
			if (this.icon.sprite == null)
			{
				this.icon.color = Color.clear;
				return;
			}
			this.icon.color = this.radialMenu.iconNormalColor;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0004D96C File Offset: 0x0004BB6C
		public void UpdateText(string newText)
		{
			if (this.text == null)
			{
				Debug.LogError(UltimateRadialMenu.FormatDebug("The button's text component is not assigned", "Please make sure that the radial menu has the <b>Button Text</b> option enabled", this.radialMenu.gameObject.name));
				return;
			}
			this.text.text = newText;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0004D9B8 File Offset: 0x0004BBB8
		public void UpdateName(string newName)
		{
			this.name = newName;
			if (this.radialMenu.displayNameOnButton && this.text != null)
			{
				this.text.text = this.name;
			}
			if (this.radialMenu.displayButtonName && this.radialMenu.nameText != null && this.radialMenu.CurrentButtonIndex == this.buttonIndex)
			{
				this.radialMenu.nameText.text = ((!this.buttonDisabled) ? this.name : "");
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0004DA50 File Offset: 0x0004BC50
		public void UpdateDescription(string newDescription)
		{
			this.description = newDescription;
			if (this.radialMenu.displayButtonDescription && this.radialMenu.descriptionText != null && this.radialMenu.CurrentButtonIndex == this.buttonIndex)
			{
				this.radialMenu.descriptionText.text = ((!this.buttonDisabled) ? this.description : "");
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0004DABC File Offset: 0x0004BCBC
		public void AddCallback(Action ButtonCallback)
		{
			this.OnRadialButtonInteract += ButtonCallback;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0004DAC5 File Offset: 0x0004BCC5
		public void AddCallback(Action<int> ButtonCallback)
		{
			this.OnRadialButtonInteractWithId += ButtonCallback;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0004DACE File Offset: 0x0004BCCE
		public void AddCallback(Action<string> ButtonCallback)
		{
			this.OnRadialButtonInteractWithKey += ButtonCallback;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004DAD8 File Offset: 0x0004BCD8
		public void RegisterButtonInfo(UltimateRadialButtonInfo buttonInfo)
		{
			this.registered = true;
			buttonInfo.radialButton = this;
			if (buttonInfo.id == 0)
			{
				buttonInfo.id = this.id;
			}
			else
			{
				this.id = buttonInfo.id;
			}
			if (buttonInfo.key == string.Empty)
			{
				buttonInfo.key = this.key;
			}
			else
			{
				this.key = buttonInfo.key;
			}
			if (buttonInfo.name == string.Empty)
			{
				buttonInfo.name = this.name;
			}
			else
			{
				this.name = buttonInfo.name;
			}
			if (buttonInfo.description == string.Empty)
			{
				buttonInfo.description = this.description;
			}
			else
			{
				this.description = buttonInfo.description;
			}
			if (this.icon != null && this.radialMenu.useButtonIcon)
			{
				if (buttonInfo.icon != null)
				{
					this.icon.sprite = buttonInfo.icon;
					this.icon.color = this.radialMenu.iconNormalColor;
				}
				else if (this.icon.sprite != null)
				{
					buttonInfo.icon = this.icon.sprite;
					this.icon.color = this.radialMenu.iconNormalColor;
				}
				else
				{
					this.icon.color = Color.clear;
				}
			}
			if (this.text != null && this.radialMenu.displayNameOnButton)
			{
				this.text.text = buttonInfo.name;
			}
			if (buttonInfo.Selected)
			{
				this.OnSelect();
			}
			this.OnSelectedStateChanged += buttonInfo.OnSelectedStateChanged;
			this.OnClearButtonInformation += buttonInfo.OnClearButtonInformation;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0004DC9C File Offset: 0x0004BE9C
		public void ClearButtonInformation()
		{
			this.registered = false;
			this.buttonDisabled = false;
			this.key = "";
			this.id = -1;
			this.name = "";
			this.description = "";
			this.Selected = false;
			this.OnExit();
			if (this.icon != null)
			{
				this.icon.sprite = null;
				this.icon.color = Color.clear;
			}
			if (this.useIconUnique)
			{
				this.useIconUnique = false;
			}
			if (this.text != null)
			{
				this.text.text = "";
			}
			Action onClearButtonInformation = this.OnClearButtonInformation;
			if (onClearButtonInformation != null)
			{
				onClearButtonInformation();
			}
			this.OnRadialButtonInteract = null;
			this.OnRadialButtonInteractWithId = null;
			this.OnRadialButtonInteractWithKey = null;
			this.unityEvent = null;
			this.OnClearButtonInformation = null;
			this.OnSelectedStateChanged = null;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0004DD7E File Offset: 0x0004BF7E
		[Obsolete("Please use the OnEnable function")]
		public void EnableButton()
		{
			this.OnEnable();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0004DD86 File Offset: 0x0004BF86
		[Obsolete("Please use the OnDisable function")]
		public void DisableButton()
		{
			this.OnDisable();
		}

		// Token: 0x04000AB2 RID: 2738
		private bool registered;

		// Token: 0x04000AB4 RID: 2740
		public UltimateRadialMenu radialMenu;

		// Token: 0x04000AB5 RID: 2741
		public RectTransform buttonTransform;

		// Token: 0x04000AB6 RID: 2742
		public Image radialImage;

		// Token: 0x04000AB7 RID: 2743
		public bool buttonDisabled;

		// Token: 0x04000AB8 RID: 2744
		public string name;

		// Token: 0x04000AB9 RID: 2745
		public string description;

		// Token: 0x04000ABA RID: 2746
		public int buttonIndex = -1;

		// Token: 0x04000ABB RID: 2747
		public float angle;

		// Token: 0x04000ABC RID: 2748
		public float angleRange;

		// Token: 0x04000ABD RID: 2749
		public RectTransform iconTransform;

		// Token: 0x04000ABE RID: 2750
		public Image icon;

		// Token: 0x04000ABF RID: 2751
		public bool useIconUnique;

		// Token: 0x04000AC0 RID: 2752
		public float iconSize;

		// Token: 0x04000AC1 RID: 2753
		public float iconHorizontalPosition;

		// Token: 0x04000AC2 RID: 2754
		public float iconVerticalPosition;

		// Token: 0x04000AC3 RID: 2755
		public float iconRotation;

		// Token: 0x04000AC4 RID: 2756
		public bool invertScaleY;

		// Token: 0x04000AC5 RID: 2757
		public Vector3 iconNormalScale;

		// Token: 0x04000AC6 RID: 2758
		public Text text;

		// Token: 0x04000AC7 RID: 2759
		public Vector3 normalPosition;

		// Token: 0x04000AC8 RID: 2760
		public Vector3 highlightedPosition;

		// Token: 0x04000AC9 RID: 2761
		public Vector3 pressedPosition;

		// Token: 0x04000ACA RID: 2762
		public Vector3 selectedPosition;

		// Token: 0x04000ACB RID: 2763
		public Vector3 disabledPosition;

		// Token: 0x04000ACC RID: 2764
		public string key;

		// Token: 0x04000ACD RID: 2765
		public int id;

		// Token: 0x04000AD2 RID: 2770
		public UnityEvent unityEvent;
	}
}
