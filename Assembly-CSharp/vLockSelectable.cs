using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000131 RID: 305
public class vLockSelectable : MonoBehaviour
{
	// Token: 0x06000A21 RID: 2593 RVA: 0x0004610D File Offset: 0x0004430D
	private void OnDisable()
	{
		this.target = null;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00046118 File Offset: 0x00044318
	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null && this.target)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute<IPointerExitHandler>(EventSystem.current.currentSelectedGameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
			EventSystem.current.SetSelectedGameObject(this.target, new BaseEventData(EventSystem.current));
			ExecuteEvents.Execute<IPointerEnterHandler>(this.target, pointerEventData, ExecuteEvents.pointerEnterHandler);
			return;
		}
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			this.target = EventSystem.current.currentSelectedGameObject;
		}
	}

	// Token: 0x04000955 RID: 2389
	public GameObject target;
}
