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
    private readonly FolderService folderService;

    public FoldersController(FolderService folderService)
    {
        this.folderService = folderService;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        FolderModel folder =this.folderService.GetFolder(id);
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
            var createdFolder = this.folderService.CreateFolder(
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