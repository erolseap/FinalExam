using ErolFinal.BL.Services;
using ErolFinal.DAL.Models;
using ErolFinal.MVC.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ErolFinal.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    protected IWebHostEnvironment _webHostEnvironment;
    protected TrainerService _trainerService;

    public HomeController(IWebHostEnvironment webHostEnvironment, TrainerService trainerService)
    {
        _webHostEnvironment = webHostEnvironment;
        _trainerService = trainerService;
    }

    #region Create
    [HttpGet]
    [ActionName("CreateTrainer")]
    public IActionResult GetCreateTrainer()
    {
        return View();
    }

    [HttpPost]
    [ActionName("CreateTrainer")]
    public async Task<IActionResult> PostCreateTrainer(HomeCreateTrainerVM data)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please set all the required values!");
            return View();
        }
        if (data.Name == "Nigger")
        {
            ModelState.AddModelError("Name", "This name of trainer is not permitted!");
            return View();
        }
        if (data.Category == "Front development")
        {
            ModelState.AddModelError("Category", "No-no-no, mr fish!");
            return View();
        }
        var entity = new TrainerModel()
        {
            Name = data.Name,
            Category = data.Category,
            Description = data.Description,
            //ImgFilename = data.ImgFilename,
            FacebookUrl = data.FacebookUrl,
            TwitterUrl = data.TwitterUrl,
            BehanceUrl = data.BehanceUrl
        };

        await SetTrainerImageAsync(entity, data.Img);
        await _trainerService.CreateAsync(entity);

        return RedirectToAction("Index");
    }
    #endregion

    #region Read
    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetIndex()
    {
        ViewData["Trainers"] = await _trainerService.GetAllAsync();
        return View();
    }
    #endregion

    #region Update
    [HttpGet]
    [ActionName("EditTrainer")]
    public async Task<IActionResult> GetEditTrainerAsync(int id)
    {
        var entity = await _trainerService.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        return View(entity!);
    }

    [HttpPost]
    [ActionName("EditTrainer")]
    public async Task<IActionResult> PostEditTrainer(int id, HomeEditTrainerVM data)
    {
        var entity = await _trainerService.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        if (id != data.Id)
        {
            return BadRequest();
        }

        {
            if (data.Name != null)
            {
                if (data.Name == "Nigger")
                {
                    ModelState.AddModelError("Name", "This name of trainer is not permitted!");
                    return View();
                }
                entity.Name = data.Name;
            }
            if (data.Category != null)
            {
                if (data.Category == "Front development")
                {
                    ModelState.AddModelError("Category", "No-no-no, mr fish!");
                    return View();
                }
                entity.Category = data.Category;
            }
            if (data.Description != null)
            {
                entity.Description = data.Description;
            }
            if (data.FacebookUrl != null)
            {
                entity.FacebookUrl = data.FacebookUrl;
            }
            if (data.TwitterUrl != null)
            {
                entity.TwitterUrl = data.TwitterUrl;
            }
            if (data.BehanceUrl != null)
            {
                entity.BehanceUrl = data.BehanceUrl;
            }
            if (data.Img != null)
            {
                await SetTrainerImageAsync(entity, data.Img);
            }
        }

        await _trainerService.UpdateAsync(id, entity);

        return RedirectToAction("Index");
    }
    #endregion

    #region Delete
    [HttpPost]
    [ActionName("DeleteTrainer")]
    public async Task<IActionResult> PostDeleteTrainer(int id)
    {
        var entity = await _trainerService.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        await _trainerService.DeleteAsync(id);

        return RedirectToAction("Index");
    }
    #endregion

    #region Non actions
    [NonAction]
    protected async Task SetTrainerImageAsync(TrainerModel model, IFormFile formFile)
    {
        if (formFile == null)
        {
            throw new ArgumentNullException(nameof(formFile));
        }
        if (!formFile.ContentType.StartsWith("image/"))
        {
            throw new ArgumentException($"The provided form file must be an image, {formFile.ContentType} given");
        }
        var baseUploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "uploads");
        if (!Directory.Exists(baseUploadsPath))
        {
            Directory.CreateDirectory(baseUploadsPath);
        }

        var fileName = model.ImgFilename;
        if (fileName == string.Empty)
        {
            fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(formFile.FileName)}";
        }
        var finalFullPath = Path.Combine(baseUploadsPath, fileName);
        
        using (var fileDescriptor = System.IO.File.Open(finalFullPath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileDescriptor);
        }

        model.ImgFilename = fileName;
    }
    #endregion
}
