using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record PostListDTO
    (
        Ulid PostId,
        string Title,
        decimal Price,
        string Currency,
        string ThumbNail,
        string GardenerName,
        string GardenerAvatar
    );
}
