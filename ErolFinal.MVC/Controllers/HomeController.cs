using ErolFinal.BL.Services;

namespace ErolFinal.MVC.Controllers;

public class HomeController : Controller
{
	protected TrainerService _trainerService;

	public HomeController(TrainerService trainerService)
	{
		_trainerService = trainerService;
	}

	[HttpGet]
	[ActionName("Index")]
	public async Task<IActionResult> GetIndex()
	{
		ViewData["Trainers"] = await _trainerService.GetAllAsync();
		return View();
	}
}
