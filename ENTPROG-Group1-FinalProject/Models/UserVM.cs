using System.ComponentModel.DataAnnotations;

public class UserVM
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; }

    [Required(ErrorMessage = "Approval status is required.")]
    public bool IsApproved { get; set; }  // Is the user approved by admin?
}
