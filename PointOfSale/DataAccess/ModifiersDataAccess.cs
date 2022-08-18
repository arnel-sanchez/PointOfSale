using PointOfSale.Models.DataBaseModels;

namespace PointOfSale.DataAccess
{
    public interface IModifiersDataAccess
    {
        public void AddModifiers(string name, string description, double price, bool add);

        public void DeleteModifiers(string id);

        public void UpdateModifiers(string id, string name, string description, double price, bool add);

        public List<Modifier> GetModifiers();

        public Modifier GetModifiers(string id);
    }
    
    public class ModifiersDataAccess : IModifiersDataAccess
    {
        private PointOfSaleDbContext dbContext { get; set; }
        
        public ModifiersDataAccess(PointOfSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddModifiers(string name, string description, double price, bool add)
        {
            var modifier = new Modifier
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Price = price,
                Add = add
            };

            dbContext.Modifiers.Add(modifier);
            dbContext.SaveChanges();
        }

        public void DeleteModifiers(string id)
        {
            var modifier = dbContext.Modifiers.Where(x => x.Id == id).FirstOrDefault();
            
            if(modifier == null)
            {
                throw new Exception("Modifier not found");
            }

            dbContext.Modifiers.Remove(modifier);
            dbContext.SaveChanges();
        }

        public void UpdateModifiers(string id, string name, string description, double price, bool add)
        {
            var modifier = dbContext.Modifiers.Where(x => x.Id == id).FirstOrDefault();

            if (modifier == null)
            {
                throw new Exception("Modifier not found");
            }

            modifier.Name = name;
            modifier.Description = description;
            modifier.Price = price;
            modifier.Add = add;

            dbContext.Modifiers.Update(modifier);
            dbContext.SaveChanges();
        }

        public List<Modifier> GetModifiers()
        {
            return dbContext.Modifiers.ToList();
        }

        public Modifier GetModifiers(string id)
        {
            var modifier = dbContext.Modifiers.Where(x => x.Id == id).FirstOrDefault();

            if (modifier == null)
            {
                throw new Exception("Modifier not found");
            }

            return modifier;
        }
    }
}
