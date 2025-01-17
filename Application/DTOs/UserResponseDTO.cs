using FarmersConnect.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record UserResponseDTO(UserDataDTO? user,bool flag, string message);
}
