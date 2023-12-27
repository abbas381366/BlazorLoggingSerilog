using BlazorLoggingSeriLog.DbModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Data.Common;

namespace BlazorLoggingSeriLog.Components.Pages;

public partial class Home:ComponentBase
{
    AddPersonelModal elemAddPersonelModal;
    [Inject] public IDbContextFactory<MyDbcontext> dbFactory { get; set; }
    [Inject] public IJSRuntime jsruntime { get; set; }
    List<Personel> listPersonel;
    async Task UpdatePersonel(Personel per)
    {
        await elemAddPersonelModal.OpenModal(per);
    }
    async Task DeletePersonel(Personel personel)
    {
        try
        {
            var ctx = await dbFactory.CreateDbContextAsync();
            var ent = ctx.Personels.FirstOrDefault(q => q.CodeMeli == personel.CodeMeli);
            if (ent == null)
            {
                await ShowMessage("خطا", "شخصی با این کد ملی پیدا نشد");
                return;
            }
            ctx.Remove(ent);
            await ctx.SaveChangesAsync();
            listPersonel.RemoveAll(q => q.CodeMeli == personel.CodeMeli);
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await ShowMessage("خطا", ex.Message);
        }
    }
    async Task UpdateList()
    {
        var ctx = await dbFactory.CreateDbContextAsync();
        listPersonel = await ctx.Personels
            .Include(q => q.DIMPersonelType)
            .ToListAsync();
        await InvokeAsync(StateHasChanged);
    }
    protected override async Task OnInitializedAsync()
    {
        await UpdateList();
    }
    async Task OpenModalPersonel()
    {

        await elemAddPersonelModal.OpenModal();
    }
    async Task OnPersonelUpdated(Personel personel)
    {
        var ctx = await dbFactory.CreateDbContextAsync();
        var entForUpdate = ctx.Personels.FirstOrDefault(q => q.CodeMeli == personel.CodeMeli);
        entForUpdate.Name = personel.Name;
        entForUpdate.Family = personel.Family;
        entForUpdate.IDDimPersonelType = personel.IDDimPersonelType;
        await ctx.SaveChangesAsync();
        await UpdateList();
        await elemAddPersonelModal.CloseModal();
    }
    async Task OnPersonelCreated(Personel personel)
    {
        try
        {

            var ctx = await dbFactory.CreateDbContextAsync();
            await ctx.AddAsync(personel);
            await ctx.SaveChangesAsync();
            await ShowMessage("نتیجه ذخیره", "ذخیره با موفقیت انجام شد");
            await elemAddPersonelModal.CloseModal();
            await UpdateList();
        }
        catch (Exception ex)
        {
            await ShowMessage("خطا", ex.Message);
        }
    }

    async Task ShowMessage(string title, string Mess)
    {

        await jsruntime.InvokeVoidAsync("ShowSweet", title, Mess);
    }
}