using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Identity;
using ECommerce.AppCore.Features.Identity.Users.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace Tests.Features.Identity;

public class UserCommandTests
{
    private async Task<ECommerceDbContext> GetDbContextWithSeedAsync()
    {
        var context = TestHelper.GetInMemoryDbContext();

        // Seed a role for testing
        context.Roles.Add(new Role
        {
            Name = "Admin",
            DisplayName = "مدیر سیستم",
            Description = "دسترسی کامل",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task CreateUser_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreateUserHandler(context);
        var command = new CreateUserCommand
        {
            UserName = "testuser",
            Email = "test@example.com",
            PhoneNumber = "09121112233",
            FirstName = "علی",
            LastName = "محمدی",
            Password = "P@ssw0rd123",
            NationalCode = "1234567890",
            RoleIds = new List<long> { 1 }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("کاربر با موفقیت ایجاد شد", result.MessageSingle);

        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");
        Assert.NotNull(user);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("علی", user.FirstName);
        Assert.True(user.IsActive);

        var userRole = await context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
        Assert.NotNull(userRole);
        Assert.Equal(1, userRole.RoleId);
    }

    [Fact]
    public async Task CreateUser_DuplicateUserName_ReturnsFailedResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var handler = new CreateUserHandler(context);

        var firstCommand = new CreateUserCommand
        {
            UserName = "duplicateuser",
            FirstName = "کاربر",
            LastName = "اول",
            Password = "P@ssw0rd123"
        };
        await handler.Handle(firstCommand, CancellationToken.None);

        var secondCommand = new CreateUserCommand
        {
            UserName = "duplicateuser",
            FirstName = "کاربر",
            LastName = "دوم",
            Password = "P@ssw0rd456"
        };

        // Act
        var result = await handler.Handle(secondCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("نام کاربری تکراری است", result.MessageSingle);
    }

    [Fact]
    public async Task UpdateUser_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var createHandler = new CreateUserHandler(context);
        await createHandler.Handle(new CreateUserCommand
        {
            UserName = "updateuser",
            FirstName = "نام قدیم",
            LastName = "نام خانوادگی قدیم",
            Password = "P@ssw0rd123",
            Email = "old@example.com"
        }, CancellationToken.None);

        var updateHandler = new UpdateUserHandler(context);
        var command = new UpdateUserCommand
        {
            Id = 1,
            FirstName = "نام جدید",
            LastName = "نام خانوادگی جدید",
            Email = "new@example.com",
            PhoneNumber = "09129998877",
            IsActive = true
        };

        // Act
        var result = await updateHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("کاربر با موفقیت ویرایش شد", result.MessageSingle);

        var user = await context.Users.FindAsync((long)1);
        Assert.NotNull(user);
        Assert.Equal("نام جدید", user.FirstName);
        Assert.Equal("new@example.com", user.Email);
    }

    [Fact]
    public async Task UpdateUser_NotFound_ReturnsFailedResult()
    {
        // Arrange
        var context = TestHelper.GetInMemoryDbContext();
        var handler = new UpdateUserHandler(context);
        var command = new UpdateUserCommand
        {
            Id = 999,
            FirstName = "نام",
            LastName = "نام خانوادگی",
            IsActive = true
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("کاربر یافت نشد", result.MessageSingle);
    }

    [Fact]
    public async Task DeleteUser_Success_ReturnsSuccessResult()
    {
        // Arrange
        var context = await GetDbContextWithSeedAsync();
        var createHandler = new CreateUserHandler(context);
        await createHandler.Handle(new CreateUserCommand
        {
            UserName = "deleteuser",
            FirstName = "کاربر",
            LastName = "حذفی",
            Password = "P@ssw0rd123"
        }, CancellationToken.None);

        var deleteHandler = new DeleteUserHandler(context);
        var command = new DeleteUserCommand { Id = 1 };

        // Act
        var result = await deleteHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("کاربر با موفقیت حذف شد", result.MessageSingle);

        var user = await context.Users.FindAsync((long)1);
        Assert.NotNull(user);
        Assert.True(user.IsDeleted);
        Assert.NotNull(user.DeletedAt);
    }

    [Fact]
    public async Task DeleteUser_NotFound_ReturnsFailedResult()
    {
        // Arrange
        var context = TestHelper.GetInMemoryDbContext();
        var handler = new DeleteUserHandler(context);
        var command = new DeleteUserCommand { Id = 999 };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("کاربر یافت نشد", result.MessageSingle);
    }
}
