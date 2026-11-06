using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000069 RID: 105
	public class BaseInput : UIBehaviour
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x000197A7 File Offset: 0x000179A7
		public virtual string compositionString
		{
			get
			{
				return Input.compositionString;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x000197AE File Offset: 0x000179AE
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x000197B5 File Offset: 0x000179B5
		public virtual IMECompositionMode imeCompositionMode
		{
			get
			{
				return Input.imeCompositionMode;
			}
			set
			{
				Input.imeCompositionMode = value;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x000197BD File Offset: 0x000179BD
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x000197C4 File Offset: 0x000179C4
		public virtual Vector2 compositionCursorPos
		{
			get
			{
				return Input.compositionCursorPos;
			}
			set
			{
				Input.compositionCursorPos = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000197CC File Offset: 0x000179CC
		public virtual bool mousePresent
		{
			get
			{
				return Input.mousePresent;
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000197D3 File Offset: 0x000179D3
		public virtual bool GetMouseButtonDown(int button)
		{
			return Input.GetMouseButtonDown(button);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000197DB File Offset: 0x000179DB
		public virtual bool GetMouseButtonUp(int button)
		{
			return Input.GetMouseButtonUp(button);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000197E3 File Offset: 0x000179E3
		public virtual bool GetMouseButton(int button)
		{
			return Input.GetMouseButton(button);
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x000197EB File Offset: 0x000179EB
		public virtual Vector2 mousePosition
		{
			get
			{
				return Input.mousePosition;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x000197F7 File Offset: 0x000179F7
		public virtual Vector2 mouseScrollDelta
		{
			get
			{
				return Input.mouseScrollDelta;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x000197FE File Offset: 0x000179FE
		public virtual bool touchSupported
		{
			get
			{
				return Input.touchSupported;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00019805 File Offset: 0x00017A05
		public virtual int touchCount
		{
			get
			{
				return Input.touchCount;
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001980C File Offset: 0x00017A0C
		public virtual Touch GetTouch(int index)
		{
			return Input.GetTouch(index);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00019814 File Offset: 0x00017A14
		public virtual float GetAxisRaw(string axisName)
		{
			return Input.GetAxisRaw(axisName);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001981C File Offset: 0x00017A1C
		public virtual bool GetButtonDown(string buttonName)
		{
			return Input.GetButtonDown(buttonName);
		}
	}
}
