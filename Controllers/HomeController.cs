using System.Diagnostics;

namespace mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        Console.WriteLine("========================================================================");
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return View("Feed");
        }
        return View("Landing");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
