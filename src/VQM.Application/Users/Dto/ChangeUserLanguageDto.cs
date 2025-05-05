using System.ComponentModel.DataAnnotations;

namespace VQM.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}