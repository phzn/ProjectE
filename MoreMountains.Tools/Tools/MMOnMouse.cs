using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x0200000E RID: 14
	public class MMOnMouse : MonoBehaviour
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002A14 File Offset: 0x00000C14
		protected virtual void OnMouseDown()
		{
			this.OnMouseDownEvent.Invoke();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A21 File Offset: 0x00000C21
		protected virtual void OnMouseDrag()
		{
			this.OnMouseDragEvent.Invoke();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A2E File Offset: 0x00000C2E
		protected virtual void OnMouseEnter()
		{
			this.OnMouseEnterEvent.Invoke();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002A3B File Offset: 0x00000C3B
		protected virtual void OnMouseExit()
		{
			this.OnMouseExitEvent.Invoke();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A48 File Offset: 0x00000C48
		protected virtual void OnMouseOver()
		{
			this.OnMouseOverEvent.Invoke();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A55 File Offset: 0x00000C55
		protected virtual void OnMouseUp()
		{
			this.OnMouseUpEvent.Invoke();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A62 File Offset: 0x00000C62
		protected virtual void OnMouseUpAsButton()
		{
			this.OnMouseUpAsButtonEvent.Invoke();
		}

		// Token: 0x04000029 RID: 41
		[Tooltip("OnMouseDown is called when the user has pressed the mouse button while over the Collider.")]
		public UnityEvent OnMouseDownEvent;

		// Token: 0x0400002A RID: 42
		[Tooltip("OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.")]
		public UnityEvent OnMouseDragEvent;

		// Token: 0x0400002B RID: 43
		[Tooltip("Called when the mouse enters the Collider.")]
		public UnityEvent OnMouseEnterEvent;

		// Token: 0x0400002C RID: 44
		[Tooltip("Called when the mouse is not any longer over the Collider.")]
		public UnityEvent OnMouseExitEvent;

		// Token: 0x0400002D RID: 45
		[Tooltip("Called every frame while the mouse is over the Collider.")]
		public UnityEvent OnMouseOverEvent;

		// Token: 0x0400002E RID: 46
		[Tooltip("OnMouseUp is called when the user has released the mouse button.")]
		public UnityEvent OnMouseUpEvent;

		// Token: 0x0400002F RID: 47
		[Tooltip("OnMouseUpAsButton is only called when the mouse is released over the same Collider as it was pressed.")]
		public UnityEvent OnMouseUpAsButtonEvent;
	}
}
