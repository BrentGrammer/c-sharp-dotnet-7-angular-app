# AutoMapper

- Create DTOs to represent requests/responses that map to models in the database
- Used to limit unused data and properties needed by the client and/or hide sensitive data you don't want send back and forth

1. Use automapper to map a dto to a source entity
1. Configure a mapping in a AutoMapperProfiles.cs file

```c#
var message = _mapper.Map<Message>(messageForCreationDto);
```

```c#
using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{

  // This is needed for AutoMapper to map from source to destination objects
  public class AutoMapperProfiles : Profile
  {
    // create a constructor where you can create your mappings:
    //Note: convention based naming (props in dtos and models are the same) require no configuration - automapper will map them
    // for properties that don't match extra config is needed.
    public AutoMapperProfiles()
    {

      // first param is source(Map from) and second is the destination(Map to)
      // You are putting the values of the source into the matching properties of the destination(for instance taking the vals from a request dto and putting them into a model to insert into the database)

      //ForMember is used to populate a property a certain way on the Dto using access to the source model object
      // The PhotoUrl on the destination(UserForListDto) is populated from the Source(model) by getting users photos and finding the one with isMain is true:
      // The Age prop on the dto is populated by creating a extension method for DateTime type that can be used to calculate the age on the DoB
      CreateMap<User, UserForListDto>()
        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
      CreateMap<User, UserForDetailedDto>()
        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
      CreateMap<Photo, PhotosForDetailedDto>();
      // this case is mapping the dto which represents what is received from the client to the model we're going to save in the backend
      //the previous maps were taking the model and mapping it to dtos which we send to the client.
      CreateMap<UserForUpdateDto, User>();
      CreateMap<Photo, PhotoForReturnDto>();
      CreateMap<PhotoForCreationDto, Photo>();
      CreateMap<UserForRegisterDto, User>();
      // this both allows you to map the request dto to the model for storing in the database and also allows the other way so you can also map the model to a dto to return to the client
      CreateMap<MessageForCreationDto, Message>().ReverseMap();
    }
  }
}
```

## AutoMapper Magic

- Note that AutoMapper will try to map properties that are associated with navigation props based on naming convention
- See the `MessageToReturnDto.cs` file which maps and matches props on the User model and populates them automatically based on how they are named in the Dto...
- when an object is loaded in memory in scope AutoMapper will also try to map properties that match what's in local memory...
  - see `MessagesController.cs` in the `CreateMessage()` method
