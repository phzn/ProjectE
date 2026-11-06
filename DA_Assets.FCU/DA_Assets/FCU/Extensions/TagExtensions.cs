using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004D RID: 77
	public static class TagExtensions
	{
		// Token: 0x060001CA RID: 458 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public static bool HaveUndownloadableTags(this FObject fobject, out string reason)
		{
			bool flag = false;
			reason = null;
			if (!fobject.ContainsTag(FcuTag.Image))
			{
				reason = "fobject.ContainsTag(FcuTag.Image) == false)";
				flag = true;
			}
			else
			{
				using (List<FcuTag>.Enumerator enumerator = fobject.Data.Tags.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.GetTagConfig().CanBeDownloaded)
						{
							reason = fobject.Data.Tags.ToLine() ?? "";
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00005D64 File Offset: 0x00003F64
		public static void RemoveNotDownloadableTags(this FObject fobject)
		{
			List<FcuTag> list = new List<FcuTag>();
			foreach (FcuTag fcuTag in fobject.Data.Tags)
			{
				if (fcuTag.GetTagConfig().CanBeDownloaded)
				{
					list.Add(fcuTag);
				}
			}
			fobject.Data.Tags = list;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00005DE0 File Offset: 0x00003FE0
		public static void AddTag(this FObject fobject, FcuTag tag)
		{
			if (fobject.Data.Tags == null)
			{
				fobject.Data.Tags = new List<FcuTag>();
			}
			if (!fobject.Data.Tags.Contains(tag))
			{
				fobject.Data.Tags.Add(tag);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00005E32 File Offset: 0x00004032
		public static bool ContainsCustomButtonTags(this SyncHelper syncHelper)
		{
			return syncHelper.ContainsAnyTag(new FcuTag[]
			{
				FcuTag.BtnDefault,
				FcuTag.BtnDisabled,
				FcuTag.BtnHover,
				FcuTag.BtnPressed,
				FcuTag.BtnSelected
			});
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00005E4B File Offset: 0x0000404B
		public static bool ContainsCustomButtonTags(this FObject fobject)
		{
			return fobject.ContainsAnyTag(new FcuTag[]
			{
				FcuTag.BtnDefault,
				FcuTag.BtnDisabled,
				FcuTag.BtnHover,
				FcuTag.BtnPressed,
				FcuTag.BtnSelected
			});
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00005E64 File Offset: 0x00004064
		public static bool IsRootSprite(this FObject child, FObject parent)
		{
			bool flag = false;
			if (parent.ContainsTag(FcuTag.Frame))
			{
				NodeType type = child.Type;
				if (type - NodeType.VECTOR <= 2 || type == NodeType.REGULAR_POLYGON)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00005E94 File Offset: 0x00004094
		public static bool ContainsAnyTag(this FObject fobject, params FcuTag[] tags)
		{
			if (fobject.Data == null)
			{
				Debug.LogWarning("fobject.Data == null");
				return false;
			}
			if (fobject.Data.Tags.IsEmpty<FcuTag>())
			{
				return false;
			}
			foreach (FcuTag fcuTag in tags)
			{
				if (fobject.ContainsTag(fcuTag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00005EEC File Offset: 0x000040EC
		public static bool ContainsAnyTag(this SyncHelper syncHelper, params FcuTag[] tags)
		{
			if (((syncHelper != null) ? syncHelper.Data : null) == null)
			{
				Debug.LogWarning("syncHelper.Data == null");
				return false;
			}
			if (syncHelper.Data.Tags.IsEmpty<FcuTag>())
			{
				return false;
			}
			foreach (FcuTag fcuTag in tags)
			{
				if (syncHelper.ContainsTag(fcuTag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00005F47 File Offset: 0x00004147
		public static bool ContainsTag(this SyncHelper syncHelper, FcuTag tag)
		{
			return ((syncHelper != null) ? syncHelper.Data : null) != null && !syncHelper.Data.Tags.IsEmpty<FcuTag>() && syncHelper.Data.Tags.Contains(tag);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00005F7E File Offset: 0x0000417E
		public static bool ContainsTag(this FObject fobject, FcuTag tag)
		{
			return fobject.Data != null && !fobject.Data.Tags.IsEmpty<FcuTag>() && fobject.Data.Tags.Contains(tag);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00005FB4 File Offset: 0x000041B4
		public static TagConfig GetTagConfig(this FcuTag fcuTag)
		{
			TagConfig tagConfig = FcuConfig.TagConfigs.FirstOrDefault<TagConfig>((TagConfig x) => x.FcuTag == fcuTag);
			if (tagConfig.IsDefault<TagConfig>())
			{
				Debug.LogError(string.Format("No tag config for '{0}' tag.", fcuTag));
				return default(TagConfig);
			}
			return tagConfig;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006012 File Offset: 0x00004212
		public static string ToLine(this IList<FcuTag> tags)
		{
			return string.Join<FcuTag>(", ", tags);
		}
	}
}
