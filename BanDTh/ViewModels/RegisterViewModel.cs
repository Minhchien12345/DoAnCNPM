using System.ComponentModel.DataAnnotations;
using BanDTh.Models;
namespace BanDTh.ViewModels;
public class RegisterViewModel
{
    [Required]
    public required string FullName { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, Phone]
    public required string PhoneNumber { get; set; }

    [Required]
    public required string Gender { get; set; }

    [Required, DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required, DataType(DataType.Password), Compare("Password")]
    public required string ConfirmPassword { get; set; }
}
