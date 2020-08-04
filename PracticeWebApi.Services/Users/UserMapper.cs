using PracticeWebApi.CommonClasses;
using PracticeWebApi.CommonClasses.Users;
using PracticeWebApi.Data.Users;

namespace PracticeWebApi.Services.Users
{
    public class UserMapper : IMapper<User, UserDataEntity>
    {
        public User MapToBase(UserDataEntity dataEntity) => new User
        {
            Id = dataEntity.Id,
            Email = dataEntity.Email,
            Name = new Name
            {
                First = dataEntity.FirstName,
                Middle = dataEntity.MiddleName,
                Last = dataEntity.LastName
            },
            Address = new Address
            {
                Street = dataEntity.Address,
                City = dataEntity.City,
                State = dataEntity.State,
                ZipCode = dataEntity.Zip
            }
        };

        public UserDataEntity MapToDataEntity(User user) => new UserDataEntity
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.Name.First,
            MiddleName = user.Name.Middle,
            LastName = user.Name.Last,
            Address = user.Address.Street,
            City = user.Address.City,
            State = user.Address.State,
            Zip = user.Address.ZipCode
        };

    }
}
