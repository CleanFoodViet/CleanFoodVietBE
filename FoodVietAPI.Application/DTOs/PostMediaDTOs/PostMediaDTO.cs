using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostMediaDTOs
{
    public record PostMediaDTO
    (
        Ulid PostMediaId,
        string MediumUrl,
        string MediumType,
        DateTime UploadedAt
    );
}
