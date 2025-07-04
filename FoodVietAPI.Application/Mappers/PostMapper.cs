using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.PostEnums;

namespace CleanFoodVietAPI.Application.Mappers
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<CreatePostDTO, Post>()
                .ForMember(des => des.PostId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.Rating, opt => opt.MapFrom(src => 0))
                .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => PostStatusEnum.ACTIVE.ToString()));

            CreateMap<CreatePostMediaDTO, PostMedia>()
                .ForMember(des => des.PostMediaId, opt => opt.MapFrom(src => Ulid.NewUlid()))
                .ForMember(des => des.PostId, opt => opt.MapFrom(
                        (src, des, member, context) => context.Items["PostId"]
                    ));

            CreateMap<UpdatePostDTO, Post>();
        }
    }
}
