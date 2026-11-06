using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancerPro
{
	// Token: 0x02000057 RID: 87
	public class GPUIDebuggerCanvas : MonoBehaviour
	{
		// Token: 0x06000346 RID: 838 RVA: 0x00011D84 File Offset: 0x0000FF84
		private void OnEnable()
		{
			this._rsgUIs = new Dictionary<int, GPUIDebuggerCanvas.DebugUIData>();
			this._callback = new Action<GPUIDataBuffer<GPUIVisibilityData>>(this.VisibilityCallback);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00011DA4 File Offset: 0x0000FFA4
		private void OnDisable()
		{
			if (this._rsgUIs != null)
			{
				foreach (GPUIDebuggerCanvas.DebugUIData debugUIData in this._rsgUIs.Values)
				{
					if (debugUIData != null)
					{
						Object.Destroy(debugUIData.uiGO);
					}
				}
				this._rsgUIs = null;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00011E14 File Offset: 0x00010014
		private void Update()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			if (this.contentTransform != null)
			{
				bool flag = this.showVisibilityToggle != null && this.showVisibilityToggle.isOn;
				foreach (KeyValuePair<int, GPUIRenderSourceGroup> keyValuePair in GPUIRenderingSystem.Instance.RenderSourceGroupProvider)
				{
					if (!this._rsgUIs.ContainsKey(keyValuePair.Key))
					{
						GameObject gameObject = new GameObject(keyValuePair.Value.Name);
						gameObject.transform.parent = this.contentTransform;
						gameObject.transform.localScale = Vector3.one;
						Text text = gameObject.AddComponent<Text>();
						text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
						text.fontSize = GPUIDebuggerCanvas.FONT_SIZE;
						text.resizeTextForBestFit = true;
						text.resizeTextMaxSize = GPUIDebuggerCanvas.FONT_SIZE;
						text.resizeTextMinSize = 1;
						text.color = Color.white;
						GPUIDebuggerCanvas.DebugUIData debugUIData = new GPUIDebuggerCanvas.DebugUIData(gameObject, text, keyValuePair.Value.Name, keyValuePair.Value.LODGroupData.Length);
						RectTransform component = gameObject.GetComponent<RectTransform>();
						component.anchorMin = Vector2.up;
						component.anchorMax = Vector2.one;
						component.anchoredPosition = new Vector2(0f, GPUIDebuggerCanvas.START_Y - GPUIDebuggerCanvas.SPACING_Y * (float)this._rsgUIs.Count);
						component.sizeDelta = new Vector2(GPUIDebuggerCanvas.WIDTH, GPUIDebuggerCanvas.HEIGHT);
						component.offsetMin = new Vector2(10f, component.offsetMin.y);
						component.offsetMax = new Vector2(10f, component.offsetMax.y);
						this._rsgUIs.Add(keyValuePair.Key, debugUIData);
					}
				}
				foreach (KeyValuePair<int, GPUIDebuggerCanvas.DebugUIData> keyValuePair2 in this._rsgUIs)
				{
					GPUIRenderSourceGroup gpuirenderSourceGroup;
					if (GPUIRenderingSystem.Instance.RenderSourceGroupProvider.TryGetData(keyValuePair2.Key, out gpuirenderSourceGroup))
					{
						keyValuePair2.Value.UpdateText(gpuirenderSourceGroup.InstanceCount, flag);
					}
				}
				this.contentTransform.sizeDelta = new Vector2(0f, GPUIDebuggerCanvas.SPACING_Y * (float)this._rsgUIs.Count);
				if (flag)
				{
					GPUICameraData firstValue = GPUIRenderingSystem.Instance.CameraDataProvider.GetFirstValue();
					if (firstValue != null && firstValue._visibilityBuffer != null)
					{
						firstValue._visibilityBuffer.AsyncDataRequest(this._callback, false);
					}
				}
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000120E0 File Offset: 0x000102E0
		private void VisibilityCallback(GPUIDataBuffer<GPUIVisibilityData> buffer)
		{
			NativeArray<GPUIVisibilityData> requestedData = buffer.GetRequestedData();
			if (!requestedData.IsCreated)
			{
				return;
			}
			GPUIVisibilityBuffer gpuivisibilityBuffer = buffer as GPUIVisibilityBuffer;
			foreach (KeyValuePair<int, GPUIDebuggerCanvas.DebugUIData> keyValuePair in this._rsgUIs)
			{
				int num;
				if (gpuivisibilityBuffer.cameraData.TryGetVisibilityBufferIndex(keyValuePair.Key, out num) && requestedData.Length > num)
				{
					for (int i = 0; i < keyValuePair.Value.lodCount; i++)
					{
						keyValuePair.Value.visibleCounts[i] = (int)requestedData[num + i].visibleCount;
					}
				}
			}
		}

		// Token: 0x04000256 RID: 598
		public Toggle showVisibilityToggle;

		// Token: 0x04000257 RID: 599
		public RectTransform contentTransform;

		// Token: 0x04000258 RID: 600
		private Dictionary<int, GPUIDebuggerCanvas.DebugUIData> _rsgUIs;

		// Token: 0x04000259 RID: 601
		private Action<GPUIDataBuffer<GPUIVisibilityData>> _callback;

		// Token: 0x0400025A RID: 602
		private static readonly float START_Y = -25f;

		// Token: 0x0400025B RID: 603
		private static readonly float SPACING_Y = 50f;

		// Token: 0x0400025C RID: 604
		private static readonly float HEIGHT = 45f;

		// Token: 0x0400025D RID: 605
		private static readonly float WIDTH = 380f;

		// Token: 0x0400025E RID: 606
		private static readonly int FONT_SIZE = 18;

		// Token: 0x02000058 RID: 88
		private class DebugUIData
		{
			// Token: 0x0600034C RID: 844 RVA: 0x000121D1 File Offset: 0x000103D1
			public DebugUIData(GameObject uiGO, Text text, string name, int lodCount)
			{
				this.uiGO = uiGO;
				this.text = text;
				this.name = name;
				text.text = name;
				this.lodCount = lodCount;
				this.visibleCounts = new int[8];
				this._instanceCount = -1;
			}

			// Token: 0x0600034D RID: 845 RVA: 0x00012210 File Offset: 0x00010410
			public void UpdateText(int instanceCount, bool isShowVisibility)
			{
				if (this._instanceCount == instanceCount && !this._isShowVisibility && this._isShowVisibility == isShowVisibility)
				{
					return;
				}
				this._isShowVisibility = isShowVisibility;
				this._instanceCount = instanceCount;
				this._text = "<b>" + this.name + "</b>\nIC: " + this._instanceCount.ToString();
				if (this._isShowVisibility)
				{
					for (int i = 0; i < this.lodCount; i++)
					{
						this._text = string.Concat(new string[]
						{
							this._text,
							"    LOD",
							i.ToString(),
							": ",
							this.visibleCounts[i].ToString()
						});
					}
				}
				this.text.text = this._text;
			}

			// Token: 0x0400025F RID: 607
			public GameObject uiGO;

			// Token: 0x04000260 RID: 608
			public Text text;

			// Token: 0x04000261 RID: 609
			public string name;

			// Token: 0x04000262 RID: 610
			private int _instanceCount;

			// Token: 0x04000263 RID: 611
			public int lodCount;

			// Token: 0x04000264 RID: 612
			public int[] visibleCounts;

			// Token: 0x04000265 RID: 613
			private string _text;

			// Token: 0x04000266 RID: 614
			private bool _isShowVisibility;
		}
	}
}
