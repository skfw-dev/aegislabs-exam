using System.Diagnostics;
using AegisLabsExam.Models;
using Microsoft.AspNetCore.Mvc;

namespace AegisLabsExam.Controllers;

[ApiController]
[Route("")]
public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Home Page";
        return View("Index");
    }
    
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy Policy";
        return View("Privacy");
    }

    [HttpGet("error")]
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