using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	internal struct MarkerList : ISerializationCallbackReceiver
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00008392 File Offset: 0x00006592
		public List<IMarker> markers
		{
			get
			{
				this.BuildCache();
				return this.m_Cache;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000083A0 File Offset: 0x000065A0
		public MarkerList(int capacity)
		{
			this.m_Objects = new List<ScriptableObject>(capacity);
			this.m_Cache = new List<IMarker>(capacity);
			this.m_CacheDirty = true;
			this.m_HasNotifications = false;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000083C8 File Offset: 0x000065C8
		public void Add(ScriptableObject item)
		{
			if (item == null)
			{
				return;
			}
			this.m_Objects.Add(item);
			this.m_CacheDirty = true;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000083E7 File Offset: 0x000065E7
		public bool Remove(IMarker item)
		{
			if (!(item is ScriptableObject))
			{
				throw new InvalidOperationException("Supplied type must be a ScriptableObject");
			}
			return this.Remove((ScriptableObject)item, item.parent.timelineAsset, item.parent);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00008419 File Offset: 0x00006619
		public bool Remove(ScriptableObject item, TimelineAsset timelineAsset, PlayableAsset thingToDirty)
		{
			if (!this.m_Objects.Contains(item))
			{
				return false;
			}
			this.m_Objects.Remove(item);
			this.m_CacheDirty = true;
			TimelineUndo.PushDestroyUndo(timelineAsset, thingToDirty, item);
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008448 File Offset: 0x00006648
		public void Clear()
		{
			this.m_Objects.Clear();
			this.m_CacheDirty = true;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000845C File Offset: 0x0000665C
		public bool Contains(ScriptableObject item)
		{
			return this.m_Objects.Contains(item);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000846A File Offset: 0x0000666A
		public IEnumerable<IMarker> GetMarkers()
		{
			return this.markers;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00008472 File Offset: 0x00006672
		public int Count
		{
			get
			{
				return this.markers.Count;
			}
		}

		// Token: 0x170000AC RID: 172
		public IMarker this[int idx]
		{
			get
			{
				return this.markers[idx];
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000848D File Offset: 0x0000668D
		public List<ScriptableObject> GetRawMarkerList()
		{
			return this.m_Objects;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008498 File Offset: 0x00006698
		public IMarker CreateMarker(Type type, double time, TrackAsset owner)
		{
			if (!typeof(ScriptableObject).IsAssignableFrom(type) || !typeof(IMarker).IsAssignableFrom(type))
			{
				throw new InvalidOperationException("The requested type needs to inherit from ScriptableObject and implement IMarker");
			}
			if (!owner.supportsNotifications && typeof(INotification).IsAssignableFrom(type))
			{
				throw new InvalidOperationException("Markers implementing the INotification interface cannot be added on tracks that do not support notifications");
			}
			ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type);
			IMarker marker = (IMarker)scriptableObject;
			marker.time = time;
			TimelineCreateUtilities.SaveAssetIntoObject(scriptableObject, owner);
			this.Add(scriptableObject);
			marker.Initialize(owner);
			return marker;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00008522 File Offset: 0x00006722
		public bool HasNotifications()
		{
			this.BuildCache();
			return this.m_HasNotifications;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008530 File Offset: 0x00006730
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00008532 File Offset: 0x00006732
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.m_CacheDirty = true;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000853C File Offset: 0x0000673C
		private void BuildCache()
		{
			if (this.m_CacheDirty)
			{
				this.m_Cache = new List<IMarker>(this.m_Objects.Count);
				this.m_HasNotifications = false;
				foreach (ScriptableObject scriptableObject in this.m_Objects)
				{
					if (scriptableObject != null)
					{
						this.m_Cache.Add(scriptableObject as IMarker);
						if (scriptableObject is INotification)
						{
							this.m_HasNotifications = true;
						}
					}
				}
				this.m_CacheDirty = false;
			}
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		[HideInInspector]
		private List<ScriptableObject> m_Objects;

		// Token: 0x040000C9 RID: 201
		[HideInInspector]
		[NonSerialized]
		private List<IMarker> m_Cache;

		// Token: 0x040000CA RID: 202
		private bool m_CacheDirty;

		// Token: 0x040000CB RID: 203
		private bool m_HasNotifications;
	}
}
