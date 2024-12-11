namespace GameZone.services
{
    public class categoriesServices:IcategoriesServices
    {
        private readonly ApplicationDbContext context;

        public categoriesServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return context.categories
                            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                            .OrderBy(c => c.Text)
                            .ToList();  
        }
    }
}
