namespace WebApi.Helpers;

using AutoMapper;
using Models.Users;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateRequest -> User
        CreateMap<CreateRequest, User>();

        // UpdateRequest -> User
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, _, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null)
                    {
                        return false;
                    }

                    if (prop is string srgProp && string.IsNullOrEmpty(srgProp))
                    {
                        return false;
                    }

                    // ignore null role
                    if (x.DestinationMember.Name == "Role" && src.Role == null)
                    {
                        return false;
                    }

                    return true;
                }
            ));
    }
}