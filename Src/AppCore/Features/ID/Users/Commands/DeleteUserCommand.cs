using AppCore.Data;
using AppCore.Features.Shop.ProductAgg.Tag.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;


namespace AppCore.Features.ID.Users.Commands
{
    public record DeleteUserCommand(long Id,long? DeletedBy);


    public class DeleteTagHandler
    {
        private readonly AppDbContext _context;

        public DeleteTagHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(DeleteUserCommand command)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(t => t.Id == command.Id && !t.IsDelete);

            if (user is null)
                return ResultOperation.ToFailedResult("کاربر مورد نظر پیدا نشد.");

            user.IsDelete = true;
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = command.DeletedBy;

            await _context.SaveChangesAsync();

            return ResultOperation.ToSuccessResult("کاربر با موفقیت حذف شد.");
        }
    }

}
