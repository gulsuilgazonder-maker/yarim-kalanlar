using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YarimKalanlar.Filters;

/// <summary>
/// Bu attribute ile işaretlenmiş action/controller'lara
/// yalnızca session'da girişi olan yöneticiler erişebilir.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class YoneticiGerekliAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var girisYapildi = context.HttpContext.Session.GetString("AdminGirisYapti") == "true";
        if (!girisYapildi)
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
}
