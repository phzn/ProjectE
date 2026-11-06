using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;

namespace DA_Assets.FCU
{
	// Token: 0x02000316 RID: 790
	[Serializable]
	public class TtfDownloader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015A3 RID: 5539 RVA: 0x0008C024 File Offset: 0x0008A224
		public Task Download(List<FontMetadata> figmaFonts)
		{
			TtfDownloader.<Download>d__0 <Download>d__;
			<Download>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Download>d__.<>4__this = this;
			<Download>d__.figmaFonts = figmaFonts;
			<Download>d__.<>1__state = -1;
			<Download>d__.<>t__builder.Start<TtfDownloader.<Download>d__0>(ref <Download>d__);
			return <Download>d__.<>t__builder.Task;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0008C070 File Offset: 0x0008A270
		public async Task DownloadFont(FontStruct missingFont, Return<FontStruct> @return)
		{
			FontItem fontItem = this.monoBeh.FontDownloader.GFontsApi.GetFontItem(missingFont.FontMetadata, missingFont.FontSubset);
			if (fontItem.IsDefault<FontItem>())
			{
				@return(new DAResult<FontStruct>
				{
					Error = new WebError(0, "Font not found in Google Fonts", null),
					Success = false
				});
			}
			else
			{
				string urlByWeight = this.monoBeh.FontDownloader.GFontsApi.GetUrlByWeight(fontItem, missingFont.FontMetadata.Weight, missingFont.FontMetadata.FontStyle);
				if (urlByWeight.IsEmpty())
				{
					@return(new DAResult<FontStruct>
					{
						Error = new WebError(0, "'" + missingFont.FontMetadata.Weight.FontWeightToString() + "' weight not found in Google Fonts", null),
						Success = false
					});
				}
				else
				{
					DARequest darequest = new DARequest
					{
						RequestType = RequestType.GetFile,
						Query = urlByWeight
					};
					DAResult<byte[]> daresult = await this.monoBeh.RequestSender.SendRequest<byte[]>(darequest);
					if (daresult.Success)
					{
						FontStruct fontStruct = missingFont;
						fontStruct.Bytes = daresult.Object;
						@return(new DAResult<FontStruct>
						{
							Object = fontStruct,
							Success = true
						});
					}
					else
					{
						@return(new DAResult<FontStruct>
						{
							Error = daresult.Error,
							Success = false
						});
					}
				}
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0008C0C4 File Offset: 0x0008A2C4
		public async Task SaveTtfFonts(List<FontStruct> downloadedFonts)
		{
			await Task.Yield();
		}
	}
}
