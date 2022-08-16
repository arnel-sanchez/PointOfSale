using PointOfSale.Definitions;
using PointOfSale.Models.AuthModels;

namespace PointOfSale.DataAccess
{
    public interface IUserDataAccess
    {
        public List<UserDTO> GetSellers();
    }

    public class UsersDataAccess : IUserDataAccess
    {
        private PointOfSaleDbContext dbContext;

        public UsersDataAccess(PointOfSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UserDTO> GetSellers()
        {
            List<UserDTO> res = new List<UserDTO>();
            var users = dbContext.Users.Where(x => x.Role == Roles.Seller).ToList();
            foreach (var user in users)
            {
                res.Add(new UserDTO { Id = user.Id, Name = user.Name, LastName = user.LastName });
            }

            return res;
        }
    }
}
