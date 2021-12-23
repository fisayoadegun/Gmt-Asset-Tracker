using AutoMapper;
using Gmt_Asset_Tracker.Models;

namespace Gmt_Asset_Tracker
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserRegistrationModel, User>()
				.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
		}
	}
}