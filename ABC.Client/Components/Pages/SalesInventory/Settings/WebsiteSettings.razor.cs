using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.Settings;

public partial class WebsiteSettings
{
    #region Fields
    [Inject] ApplicationDbContext ApplicationDbContext { get; set; }
    [Inject] ContentService_SQL ContentService_SQL { get; set; }


    private Content Content { get; set; } = new();
	private bool isAboutEditing = false;
	private bool isTermsEditing = false;
	private bool isReturnEditing = false;
	private bool isVissionEditing = false;
	private bool isPrivacyEditing = false;


	#endregion

	protected override async Task OnInitializedAsync()
    {
        ContentService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadContents();
    }

    private async Task LoadContents()
    {
        Content = await ContentService_SQL.GetContentInfo(ApplicationDbContext, 1);
    }

    private async Task Update()
    {
		bool updated = await ContentService_SQL.UpdateContent(ApplicationDbContext, Content);

		if (updated)
		{
			//refresh the list
			await LoadContents();
		}
		isAboutEditing = false;
	}
	private void StartAboutEditing()
	{
		isAboutEditing = true;
	}

	private void StartVisionEditing()
	{
		isVissionEditing = true;
	}

	private void StartPrivacyEditing()
	{
		isPrivacyEditing = true;
	}

	private void StartTermsEditing()
	{
		isTermsEditing = true;
	}

	private void StartReturnEditing()
	{
		isReturnEditing = true;
	}


}