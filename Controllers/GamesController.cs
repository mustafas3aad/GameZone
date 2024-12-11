namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly IcategoriesServices icategoriesServices;
        private readonly IGamesServices gamesServices;

        public IDevicesServices IdevicesServices { get; }

        public GamesController(IcategoriesServices icategoriesServices,IDevicesServices idevicesServices,IGamesServices gamesServices)
        {
            this.icategoriesServices = icategoriesServices;
            IdevicesServices = idevicesServices;
            this.gamesServices = gamesServices;
        }
        public IActionResult Index()
        {
            var games = gamesServices.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = gamesServices.GetById(id);
            
            if(game == null)
            {
                return NotFound();
            }
            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                
                Categories = icategoriesServices.GetSelectListItems(),
                Devices = IdevicesServices.GetSelectListItems(),
            };
            
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel Model)
        {
            try
            {
                if (ModelState.IsValid==true)
                {
                    
                    await gamesServices.Create(Model);
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
            }
            
            Model.Categories = icategoriesServices.GetSelectListItems();
            Model.Devices = IdevicesServices.GetSelectListItems();


            return View(Model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = gamesServices.GetById(id);
            
            if (game == null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices=game.Devices.Select(d=>d.DeviceId).ToList(),
                Categories = icategoriesServices.GetSelectListItems(),
                Devices = IdevicesServices.GetSelectListItems(),
                
                CurrentCover = game.Cover
            };
            
            return View(viewModel);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel Model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    
                    var game = await gamesServices.Update(Model);
                    
                    if (game == null)
                        return BadRequest();

                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            Model.Categories = icategoriesServices.GetSelectListItems();
            Model.Devices = IdevicesServices.GetSelectListItems();


            return View(Model);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
           
            var isDeleted=gamesServices.Delete(id);

            return isDeleted? Ok():BadRequest();
        }


    }
}
