using System.Diagnostics;
using AegisLabsExam.Models;
using Microsoft.AspNetCore.Mvc;

namespace AegisLabsExam.Controllers;

[Route("")]
public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Home Page";
        return View("Index");
    }
    
    [Route("privacy")]
    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy Policy";
        return View("Privacy");
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View("Error", model: errorViewModel);
    }
}