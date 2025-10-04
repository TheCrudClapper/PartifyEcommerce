using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSOS.UI.Controllers;

[AllowAnonymous]
public class ContactController : Controller
{
    public IActionResult Index(CancellationToken cancellationToken)
    {
        return View();
    }
}
