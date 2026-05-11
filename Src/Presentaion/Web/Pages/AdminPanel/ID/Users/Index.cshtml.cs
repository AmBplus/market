using AppCore.Features.ID.Users.Commands;
using AppCore.Features.ID.Users.Queries.GetUserById;
using AppCore.Features.ID.Users.Queries.Shared;
using Framework;
using Framework.Aspc.Helper;
using Framework.Helpers;
using Framework.ResultHelper;
using Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Wolverine;




namespace Web.Pages.AdminPanel.ID.Users;

public class IndexModel : BasePageModel
{
    public IMessageBus Bus { get; }

    public IndexModel(IMessageBus bus)
    {
        Bus = bus;
    }

    [BindProperty]
    public UserUpsertModel UserModel { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnGetUserPartial([FromQuery] long id)
    {
        if (id == 0)
            UserModel = new UserUpsertModel();
        else
        {
            var result = await Bus.InvokeAsync<ResultOperation<UserDto>>(new GetUserByIdQuery(id));
            if (!result.IsSuccess)
                return NotFound();

            UserModel = UserUpsertModel.MapToViewModel(result.Data);
        }

        return Partial("_UpsertPartial", UserModel);
    }

    public async Task<IActionResult> OnPostEdit()
    {
        if (!ModelState.IsValid)
            return new BadRequestObjectResult("مدل ارسال شده نامعتبر است.\n" + ModelState.GetAllErrorMessages());

        ResultOperation result;

        if (UserModel.Id == 0)
        {
            var createResult = await Bus.InvokeAsync<ResultOperation<long>>(UserModel.MapToCreate());
            result = createResult.GetResultOperation();
        }
        else
        {
            result = await Bus.InvokeAsync<ResultOperation>(UserModel.MapToUpdate());
        }

        return result.IsSuccess
            ? new OkObjectResult(result)
            : new BadRequestObjectResult(result);
    }

}
[Framework.Helpers.HasMappingsAttribute]
public class UserUpsertModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام کاربری الزامی است")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "ایمیل")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string? Email { get; set; }

    [Display(Name = "رمز عبور")]
    public string? Password { get; set; }

    [Display(Name = "موبایل")]
    public string? Mobile { get; set; }

    [Display(Name = "نام")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    public string? LastName { get; set; }

    [Display(Name = "نام پدر")]
    public string? FatherName { get; set; }

    [Display(Name = "کد ملی")]
    public string? NationalId { get; set; }

    [Display(Name = "شماره پاسپورت")]
    public string? PassportNumber { get; set; }

    [Display(Name = "ملیت")]
    public string? Nationality { get; set; }

    [Display(Name = "تاریخ تولد")]
    [MaxLength(10, ErrorMessage = "حداکثر طول تاریخ 10 کاراکتر است")]
    public string? BirthDate { get; set; }

    [Display(Name = "آدرس")]
    public string? Address { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; } = true;

    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<UserDto, UserUpsertModel>()
            .Map(dest => dest.BirthDate, src => src.BirthDate.HasValue ? src.BirthDate.Value.ToPersianDateString() : null);

        config.NewConfig<UserUpsertModel, CreateUserCommand>()
            .Map(dest => dest.BirthDate, src => !string.IsNullOrWhiteSpace(src.BirthDate) ? src.BirthDate.ToDateTimeFromPersian() : (DateTime?)null);

        config.NewConfig<UserUpsertModel, UpdateUserCommand>()
            .Map(dest => dest.BirthDate, src => !string.IsNullOrWhiteSpace(src.BirthDate) ? src.BirthDate.ToDateTimeFromPersian() : (DateTime?)null);
    }
    public static UserUpsertModel MapToViewModel(UserDto dto) => dto.Adapt<UserUpsertModel>();
    public CreateUserCommand MapToCreate() =>  this.Adapt<CreateUserCommand>();
    public UpdateUserCommand MapToUpdate() =>  this.Adapt<UpdateUserCommand>();
    
}