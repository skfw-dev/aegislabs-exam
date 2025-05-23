﻿using System.Data;
using AegisLabsExam.Helpers;
using AegisLabsExam.Repositories;
using AegisLabsExam.Schemas;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace AegisLabsExam.Controllers;

[ApiController]
[Route("pdf")]
public class PdfController(IDatabaseHelper dbHelper, IDatabaseHelperScripts dbHelperScripts, IPersonRepository personRepository) : Controller
{
    private IDatabaseHelper DbHelper => dbHelper;
    private IDatabaseHelperScripts DatabaseHelperScripts => dbHelperScripts;
    private IPersonRepository PersonRepository => personRepository;
    private DatabaseHelperScriptsExecuteDelegate GetPersonsExec => DatabaseHelperScripts.Exec("GetPersons");
    
    [HttpGet("")]
    public IActionResult Index()
    {
        var data = new { message = "Hello World" };
        Response.ContentType = "application/json";
        return Ok(data);
    }
    
    [HttpGet("download")]
    public IActionResult Download()
    {
        var results = GetPersonsExec();
        var persons = results.Convert(row => new Person
        {
            Id = row.Field<string>("id")!,
            Name = row.Field<string>("name")!,
            Age = row.Field<int>("age"),
        });
        
        ViewData["Title"] = "Invoice";
        ViewData["Persons"] = persons;
        
        return new ViewAsPdf(viewName: "Invoice", viewData: ViewData)
        {
            PageSize = Size.A4,
            PageOrientation = Orientation.Portrait,
            // FileName = "invoice.pdf",
        };
        // return View("Invoice");
    }
}
