using FluentValidation;
using PlastikMVC.AllDtos.UserDtos;

public class EditUserValidator : AbstractValidator<EditUserModel>
{
    public EditUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boş olamaz.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boş olamaz.");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}
