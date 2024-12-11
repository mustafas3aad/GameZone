namespace GameZone.services
{
    public class GamesServices:IGamesServices
    {
        private readonly ApplicationDbContext context;
        
        public IWebHostEnvironment WebHostEnvironment { get; }
        private readonly string _imagesPath;
                                                          
        public GamesServices(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            
            WebHostEnvironment = webHostEnvironment;
            _imagesPath = $"{webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public IEnumerable<Game> GetAll()
        {
            return context.Games
                
                .Include(g=>g.Category)
                .Include(g=>g.Devices)
                .ThenInclude(d=>d.Device)
                
                .AsNoTracking ()
                .ToList();
        }

        
        public Game? GetById(int id)
        {
            return context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == id);
        }



        public async Task Create(CreateGameFormViewModel model)
        {
            
            var coverName = await SaveCover(model.Cover);
           
            

            
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                
                Devices = model.SelectedDevices.Select(d=>new GameDevice {DeviceId=d }).ToList()
            };

            context.Add(game);
            context.SaveChanges();
        }
        
        public async Task<Game?> Update(EditGameFormViewModel model)
        {
           
            var game= context.Games
                
                .Include(g=>g.Devices)
                .SingleOrDefault(g=>g.Id ==model.Id);
            
            if (game == null)
                return null;


            var hasNewCover = model.Cover != null;
            
            var oldCover=game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            
            game.Devices= model.SelectedDevices.Select(d=>new GameDevice { DeviceId=d}).ToList();
            



            
            if (hasNewCover)
            {
                
                game.Cover=await SaveCover(model.Cover!);
            }

            
             var effectedRows =  context.SaveChanges();
            if(effectedRows > 0)
            {
                
                if (hasNewCover)
                {

                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            
            else
            {
                
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
                
                return null;


            }

        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = context.Games.Find(id);

            if (game == null)
                return isDeleted;

            context.Remove(game);

            
            var effectRows = context.SaveChanges();
            if (effectRows > 0)
            {
                isDeleted = true;
                
                var cover =Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }
            return isDeleted;
        }




        private async Task<string> SaveCover(IFormFile cover)
        {
            
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var Stream = File.Create(path);
            await cover.CopyToAsync(Stream);

            return coverName;
        }

       
    }
}
