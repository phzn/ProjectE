using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x02000161 RID: 353
	internal class DefaultDragAndDropClient : DragAndDropData, IDragAndDrop
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0002DDAF File Offset: 0x0002BFAF
		public override DragVisualMode visualMode
		{
			get
			{
				return this.m_VisualMode;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0002DDB7 File Offset: 0x0002BFB7
		public override object source
		{
			get
			{
				return this.GetGenericData("__unity-drag-and-drop__source-view");
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0002DDC4 File Offset: 0x0002BFC4
		public override IEnumerable<Object> unityObjectReferences
		{
			get
			{
				return this.m_UnityObjectReferences;
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002DDCC File Offset: 0x0002BFCC
		public override object GetGenericData(string key)
		{
			return this.m_GenericData.ContainsKey(key) ? this.m_GenericData[key] : null;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002DDFB File Offset: 0x0002BFFB
		public override void SetGenericData(string key, object value)
		{
			this.m_GenericData[key] = value;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002DE0C File Offset: 0x0002C00C
		public void StartDrag(StartDragArgs args, Vector3 pointerPosition)
		{
			bool flag = args.unityObjectReferences != null;
			if (flag)
			{
				this.m_UnityObjectReferences = args.unityObjectReferences.ToArray<Object>();
			}
			this.m_VisualMode = args.visualMode;
			foreach (object obj in args.genericData)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				this.m_GenericData[(string)dictionaryEntry.Key] = dictionaryEntry.Value;
			}
			bool flag2 = string.IsNullOrWhiteSpace(args.title);
			if (!flag2)
			{
				VisualElement visualElement = this.source as VisualElement;
				VisualElement visualElement2 = ((visualElement != null) ? visualElement.panel.visualTree : null);
				bool flag3 = visualElement2 == null;
				if (!flag3)
				{
					if (this.m_DraggedInfoLabel == null)
					{
						this.m_DraggedInfoLabel = new Label
						{
							pickingMode = PickingMode.Ignore,
							style = 
							{
								position = Position.Absolute
							}
						};
					}
					this.m_DraggedInfoLabel.text = args.title;
					this.m_DraggedInfoLabel.style.top = pointerPosition.y;
					this.m_DraggedInfoLabel.style.left = pointerPosition.x;
					visualElement2.Add(this.m_DraggedInfoLabel);
				}
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002DF80 File Offset: 0x0002C180
		public void UpdateDrag(Vector3 pointerPosition)
		{
			bool flag = this.m_DraggedInfoLabel == null;
			if (!flag)
			{
				this.m_DraggedInfoLabel.style.top = pointerPosition.y;
				this.m_DraggedInfoLabel.style.left = pointerPosition.x;
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void AcceptDrag()
		{
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
		public void SetVisualMode(DragVisualMode mode)
		{
			this.m_VisualMode = mode;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002DFDF File Offset: 0x0002C1DF
		public void DragCleanup()
		{
			this.m_UnityObjectReferences = null;
			Hashtable genericData = this.m_GenericData;
			if (genericData != null)
			{
				genericData.Clear();
			}
			this.SetVisualMode(DragVisualMode.None);
			Label draggedInfoLabel = this.m_DraggedInfoLabel;
			if (draggedInfoLabel != null)
			{
				draggedInfoLabel.RemoveFromHierarchy();
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0002E015 File Offset: 0x0002C215
		public DragAndDropData data
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04000573 RID: 1395
		private readonly Hashtable m_GenericData = new Hashtable();

		// Token: 0x04000574 RID: 1396
		private Label m_DraggedInfoLabel;

		// Token: 0x04000575 RID: 1397
		private DragVisualMode m_VisualMode;

		// Token: 0x04000576 RID: 1398
		private IEnumerable<Object> m_UnityObjectReferences;
	}
}
