using System;

using Microsoft.AspNetCore.Mvc;

using CloudFileSystem.FileSystem.Exceptions;
using CloudFileSystem.FileSystem.Files;

namespace CloudFileSystem.FileSystemApi;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileGetter fileGetter;
    private readonly FileCreator fileCreator;

    public FilesController(FileGetter fileGetter, FileCreator fileCreator)
    {
        this.fileGetter = fileGetter;
        this.fileCreator = fileCreator;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(this.fileGetter.GetFiles());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var file = this.fileGetter.GetFile(id);
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
            FileResponseModel file = this.fileCreator.CreateFile(model);
            return Created($"/files/{file.Id}", file);
        }
        catch (FileSystemException)
        {
            return BadRequest();
        }
    }
}
