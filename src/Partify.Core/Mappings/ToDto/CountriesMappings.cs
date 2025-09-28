using CSOS.Core.Domain.Entities;
using CSOS.Core.DTO.CountryDto;

namespace CSOS.Core.Mappings.ToDto;

public static class CountriesMappings
{
    public static CountryResponse ToCountryReponse(this Country country)
    {
        return new CountryResponse(country.Id, country.CountryName);
    }
}
