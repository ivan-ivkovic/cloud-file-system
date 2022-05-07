using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using P3Mobility.CloudFileSystem.FileSystem.Files;

namespace P3Mobility.CloudFileSystem.FileSystemApi;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private List<FileModel> files = new List<FileModel>()
    {
        new FileModel
        {
            Id = Guid.Parse("013b03b8-5787-40c4-889e-adc9e8c605d1"),
            Name = "file1.txt"
        },
        new FileModel
        {
            Id = Guid.Parse("5160cb78-2aef-435e-9cb4-0008bcb9c080"),
            Name = "file2.txt"
        }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(this.files);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var file = this.files.Find(x => x.Id == id);
        if (file == null)
        {
            return NotFound();
        }

        return Ok(file);
    }
}
