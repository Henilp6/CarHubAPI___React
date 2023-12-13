using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CarHub_Web.Models.Dto
{
    public class ApplicationUserRoleDTO :IdentityUserRole<string>
    {
        
    }

}
