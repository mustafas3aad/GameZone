﻿namespace GameZone.services
{
    public interface IGamesServices
    {
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        Task Create(CreateGameFormViewModel model);
        
        Task<Game?> Update(EditGameFormViewModel model);
        bool Delete(int id);
    }
}
