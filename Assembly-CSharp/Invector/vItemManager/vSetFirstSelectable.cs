using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Invector.vItemManager
{
	// Token: 0x020005D5 RID: 1493
	public class vSetFirstSelectable : MonoBehaviour
	{
		// Token: 0x060022C1 RID: 8897 RVA: 0x000C7847 File Offset: 0x000C5A47
		public void ApplyFirstSelectable(GameObject firstSelectable)
		{
			this.firstSelectable = firstSelectable;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000C7850 File Offset: 0x000C5A50
		private void OnEnable()
		{
			base.StartCoroutine(this.SetSelectableHandle(this.firstSelectable));
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000C7865 File Offset: 0x000C5A65
		private IEnumerator SetSelectableHandle(GameObject target)
		{
			if (base.enabled)
			{
				yield return new WaitForEndOfFrame();
				this.SetSelectable(target);
			}
			yield break;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000C787C File Offset: 0x000C5A7C
		private void SetSelectable(GameObject target)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute<IPointerExitHandler>(EventSystem.current.currentSelectedGameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
			EventSystem.current.SetSelectedGameObject(target, new BaseEventData(EventSystem.current));
			if (this.callSelectHandle)
			{
				ExecuteEvents.Execute<ISelectHandler>(target, pointerEventData, ExecuteEvents.selectHandler);
				return;
			}
			ExecuteEvents.Execute<IPointerEnterHandler>(target, pointerEventData, ExecuteEvents.pointerEnterHandler);
		}

		// Token: 0x04002308 RID: 8968
		public GameObject firstSelectable;

		// Token: 0x04002309 RID: 8969
		[Tooltip("True: Execute Selection Handler Event - False: Execute Pointer Enter Handler Event")]
		public bool callSelectHandle = true;
	}
}
