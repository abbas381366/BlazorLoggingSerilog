using BlazorLoggingSeriLog.DbModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.JSInterop;

namespace BlazorLoggingSeriLog.Components;

public partial class AddPersonelModal:ComponentBase
{
    string modalMode = "add";
    dynamic openedmodal;
    string modalID = "a" + Guid.NewGuid().ToString().Split('-')[0];
    [Parameter] public EventCallback<Personel> OnPersonelCreated { get; set; }
    [Parameter] public EventCallback<Personel> OnPersonelUpdated { get; set; }
    [Inject] public IDbContextFactory<MyDbcontext> dbFactory { get; set; }
    [Inject] public IJSRuntime JSRuntime { get; set; }
    Personel personel = new();
    List<DIMPersonelType> listPersonelType;
    protected override async Task OnInitializedAsync()
    {
        var ctx = await dbFactory.CreateDbContextAsync();
        listPersonelType = await ctx.DIMPersonelTypes.ToListAsync();
        await InvokeAsync(StateHasChanged);
    }
    public async Task CloseModal()
    {
        await JSRuntime.InvokeVoidAsync("CloseModal", modalID);
    }
    public async Task OpenModal(Personel pers)
    {
        modalMode = "update";
        personel = pers;
        await InvokeAsync(StateHasChanged);
        await JSRuntime.InvokeAsync<dynamic>("OpenModal", modalID);
    }
    public async Task OpenModal()
    {
        modalMode = "add";
        personel = new();
        await JSRuntime.InvokeAsync<dynamic>("OpenModal", modalID);
    }
    async Task OnSaveClicked()
    {
        if (modalMode == "add")
        {

            await OnPersonelCreated.InvokeAsync(personel);
        }
        else if (modalMode == "update")
        {
            await OnPersonelUpdated.InvokeAsync(personel);
        }
    }
}