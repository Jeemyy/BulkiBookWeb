using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ModelViews
{
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser{ get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
