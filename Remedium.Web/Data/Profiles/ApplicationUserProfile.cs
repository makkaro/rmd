using AutoMapper;
using Remedium.Web.Data.Entities;
using Remedium.Web.Data.Models;

namespace Remedium.Web.Data.Profiles
{
    public sealed class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() => CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
    }
}