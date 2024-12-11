namespace GameZone.services
{
    public class DevicesServices:IDevicesServices
    {
        private readonly ApplicationDbContext context;

        public DevicesServices(ApplicationDbContext context)
        {
            this.context = context;
        }
       
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return context.Devices
                            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                            .OrderBy(d => d.Text)
                            .AsNoTracking()
                            .ToList();
        }
    }
}
