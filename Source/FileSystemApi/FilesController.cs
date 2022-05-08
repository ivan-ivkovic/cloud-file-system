using System;

using Microsoft.AspNetCore.Mvc;

using P3Mobility.CloudFileSystem.FileSystem.Exceptions;
using P3Mobility.CloudFileSystem.FileSystem.Files;

namespace P3Mobility.CloudFileSystem.FileSystemApi;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileService fileService;

    public FilesController(FileService fileService)
    {
        this.fileService = fileService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(this.fileService.GetFiles());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var file = this.fileService.GetFile(id);
        if (file.IsEmpty())
        {
            return NotFound();
        }

        return Ok(file);
    }

    [HttpPost()]
    public IActionResult Create(CreateOrUpdateFileModel model)
    {
        try
        {
            FileModel file = this.fileService.CreateFile(model);
            return Created($"/files/{file.Id}", file);
        }
        catch (FileSystemException)
        {
            return BadRequest();
        }
    }
}
