using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200013C RID: 316
	internal static class TreeViewHelpers<T, TDefaultController> where TDefaultController : BaseTreeViewController, IDefaultTreeViewController<T>
	{
		// Token: 0x06000A5E RID: 2654 RVA: 0x00029274 File Offset: 0x00027474
		internal static void SetRootItems(BaseTreeView treeView, IList<TreeViewItemData<T>> rootItems, Func<TDefaultController> createController)
		{
			TDefaultController tdefaultController = treeView.viewController as TDefaultController;
			bool flag = tdefaultController != null;
			if (flag)
			{
				tdefaultController.SetRootItems(rootItems);
			}
			else
			{
				TDefaultController tdefaultController2 = createController();
				treeView.SetViewController(tdefaultController2);
				tdefaultController2.SetRootItems(rootItems);
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000292D4 File Offset: 0x000274D4
		internal static IEnumerable<TreeViewItemData<T>> GetSelectedItems(BaseTreeView treeView)
		{
			BaseTreeViewController viewController = treeView.viewController;
			TDefaultController defaultController = viewController as TDefaultController;
			bool flag = defaultController != null;
			if (flag)
			{
				foreach (int index in treeView.selectedIndices)
				{
					yield return defaultController.GetTreeViewItemDataForIndex(index);
				}
				IEnumerator<int> enumerator = null;
				yield break;
			}
			BaseTreeViewController viewController2 = treeView.viewController;
			bool flag2 = ((viewController2 != null) ? viewController2.GetType().GetGenericTypeDefinition() : null) == typeof(TDefaultController).GetGenericTypeDefinition();
			if (flag2)
			{
				BaseTreeViewController viewController3 = treeView.viewController;
				Type objectType = ((viewController3 != null) ? viewController3.GetType().GetGenericArguments()[0] : null);
				throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1}) and is not recognized by the controller.", typeof(T), objectType));
			}
			throw new ArgumentException("GetSelectedItems<T>() only works when using the default controller. Use your controller along with the selectedIndices enumerable instead.");
			yield break;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000292E4 File Offset: 0x000274E4
		internal static T GetItemDataForIndex(BaseTreeView treeView, int index)
		{
			TDefaultController tdefaultController = treeView.viewController as TDefaultController;
			bool flag = tdefaultController != null;
			T t;
			if (flag)
			{
				t = tdefaultController.GetDataForIndex(index);
			}
			else
			{
				BaseTreeViewController viewController = treeView.viewController;
				object obj = ((viewController != null) ? viewController.GetItemForIndex(index) : null);
				Type type = ((obj != null) ? obj.GetType() : null);
				bool flag2 = type == typeof(T);
				if (!flag2)
				{
					bool flag3;
					if (type == null)
					{
						BaseTreeViewController viewController2 = treeView.viewController;
						flag3 = ((viewController2 != null) ? viewController2.GetType().GetGenericTypeDefinition() : null) == typeof(TDefaultController).GetGenericTypeDefinition();
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						BaseTreeViewController viewController3 = treeView.viewController;
						type = ((viewController3 != null) ? viewController3.GetType().GetGenericArguments()[0] : null);
					}
					throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1}) and is not recognized by the controller.", typeof(T), type));
				}
				t = (T)((object)obj);
			}
			return t;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000293DC File Offset: 0x000275DC
		internal static T GetItemDataForId(BaseTreeView treeView, int id)
		{
			TDefaultController tdefaultController = treeView.viewController as TDefaultController;
			bool flag = tdefaultController != null;
			T t;
			if (flag)
			{
				t = tdefaultController.GetDataForId(id);
			}
			else
			{
				BaseTreeViewController viewController = treeView.viewController;
				object obj = ((viewController != null) ? viewController.GetItemForIndex(treeView.viewController.GetIndexForId(id)) : null);
				Type type = ((obj != null) ? obj.GetType() : null);
				bool flag2 = type == typeof(T);
				if (!flag2)
				{
					bool flag3;
					if (type == null)
					{
						BaseTreeViewController viewController2 = treeView.viewController;
						flag3 = ((viewController2 != null) ? viewController2.GetType().GetGenericTypeDefinition() : null) == typeof(TDefaultController).GetGenericTypeDefinition();
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						BaseTreeViewController viewController3 = treeView.viewController;
						type = ((viewController3 != null) ? viewController3.GetType().GetGenericArguments()[0] : null);
					}
					throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1}) and is not recognized by the controller.", typeof(T), type));
				}
				t = (T)((object)obj);
			}
			return t;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000294E0 File Offset: 0x000276E0
		internal static void AddItem(BaseTreeView treeView, TreeViewItemData<T> item, int parentId = -1, int childIndex = -1, bool rebuildTree = true)
		{
			TDefaultController tdefaultController = treeView.viewController as TDefaultController;
			bool flag = tdefaultController != null;
			if (flag)
			{
				tdefaultController.AddItem(in item, parentId, childIndex, rebuildTree);
				if (rebuildTree)
				{
					treeView.RefreshItems();
				}
				return;
			}
			Type type = null;
			BaseTreeViewController viewController = treeView.viewController;
			bool flag2 = ((viewController != null) ? viewController.GetType().GetGenericTypeDefinition() : null) == typeof(TDefaultController).GetGenericTypeDefinition();
			if (flag2)
			{
				BaseTreeViewController viewController2 = treeView.viewController;
				type = ((viewController2 != null) ? viewController2.GetType().GetGenericArguments()[0] : null);
			}
			throw new ArgumentException(string.Format("Type parameter ({0}) differs from data source ({1})and is not recognized by the controller.", typeof(T), type));
		}
	}
}
