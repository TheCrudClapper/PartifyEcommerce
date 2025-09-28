namespace CSOS.Core.DTO.CategoryDto;

public record CategoryResponse(
    int Id,
    string Name,
    string Description,
    string CategoryImage
    );