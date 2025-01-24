using Application.Abstractions.Data;
using Bogus;
using Bogus.DataSets;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace DbMigrator;

public class UserDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IApplicationDbContext _context;

    public UserDataSeedContributor(IApplicationDbContext context)
    {
        _context = context;
    }

    private const int MaxUsersSeed = 300;

    public async Task SeedAsync()
    {
        var newUsers = GenData();
        var countUsersInDb = await _context.Users.CountAsync();
        if (countUsersInDb < MaxUsersSeed)
        {
            foreach (var identityUser in newUsers)
            {
                await SeedUser(identityUser);
            }
        }
    }

    public async Task SeedUser(User user)
    {
        var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (newUser is null)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }

    public List<User> GenData()
    {
        var userFaker = new Faker<User>()
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                .RuleFor(o => o.LastName, f => f.Name.LastName())
                .RuleFor(o => o.PasswordHash, f => f.Internet.Password())
            ;

        var internet = new Internet();
        var name = new Name();
        var phone = new PhoneNumbers();

        var res = new List<User>();
        for (int i = 0; i < MaxUsersSeed; i++)
        {
            res.Add(new User()
            {
                Email = internet.Email(),
                FirstName = name.FirstName(),
                LastName = name.LastName(),
                PasswordHash = internet.Password()
            });
        }

        return res;
    }
}
