using System;
using Microsoft.AspNetCore.Mvc;
using P3Mobility.CloudFileSystem.FileSystem.Exceptions;
using P3Mobility.CloudFileSystem.FileSystem.Folders;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;

namespace P3Mobility.CloudFileSystem.FileSystemApi;

[ApiController]
[Route("[controller]")]
public class FoldersController : ControllerBase
{
    private readonly FolderGetter folderGetter;
    private readonly FolderCreator folderCreator;

    public FoldersController(FolderGetter folderGetter, FolderCreator folderCreator)
    {
        this.folderGetter = folderGetter;
        this.folderCreator = folderCreator;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        FolderResponseModel folder =this.folderGetter.GetFolder(id);
        if (folder.IsEmpty())
        {
            return NotFound();
        }

        return Ok(folder);
    }

    [HttpPost()]
    public IActionResult CreateFolder(CreateFolderModel createFolderModel)
    {
        try
        {
            var createdFolder = this.folderCreator.CreateFolder(
                createFolderModel.ParentFolderId,
                createFolderModel.FolderName
            );
            return Created($"/folders/{createdFolder.Id}", createdFolder);
        }
        catch (FileSystemException)
        {
            return BadRequest();
        }
    }
}