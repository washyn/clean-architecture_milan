using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Bogus;
using Bogus.DataSets;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace DbMigrator;

public class UserDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher passwordHasher;
    private readonly ILogger<UserDataSeedContributor> logger;

    public UserDataSeedContributor(IApplicationDbContext context, IPasswordHasher passwordHasher, ILogger<UserDataSeedContributor> logger)
    {
        _context = context;
        this.passwordHasher = passwordHasher;
        this.logger = logger;
    }

    private const int MaxUsersSeed = 300;

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding users...");
        var newUsers = GenData();
        var countUsersInDb = await _context.Users.CountAsync();
        if (countUsersInDb < MaxUsersSeed)
        {
            foreach (var identityUser in newUsers)
            {
                await SeedUser(identityUser);
            }
        }
        logger.LogInformation("Users seeded.");
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
                PasswordHash = passwordHasher.Hash(internet.Password())
                // PasswordHash = internet.Password()
            });
        }

        return res;
    }
}
