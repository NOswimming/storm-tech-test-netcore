using Microsoft.AspNetCore.Identity;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.EntityModelMappers.TodoItems
{
    public class UserSummaryViewmodelFactory
    {
        public static UserSummaryViewmodel Create(IdentityUser identityUser)
        {
            var userSummaryViewmodel = new UserSummaryViewmodel(identityUser.UserName, identityUser.Email);

            // Add gravatar profile information if available;
            var gravatarProfile = GravatarProfileService.GetService().GetGravatarProfile(identityUser.Email);
            if (gravatarProfile != null)
            {
                userSummaryViewmodel.FullName = gravatarProfile.FullName;
            }

            return userSummaryViewmodel;
        }
    }
}