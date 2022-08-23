using Microsoft.EntityFrameworkCore;
using PointOfSale.Models.DataBaseModels;

namespace PointOfSale.DataAccess
{
    public interface IItemsDataAccess
    {
        public void AddItem(string name, double price, string description, int quantity, string category, string image, string code, List<string> modifiersIds);

        public Item GetItems(string id);

        public List<Item> GetItems();

        public void UpdateItem(string id, string name, double price, string description, int quantity, string category, string image, string code, List<string> modifiersIds);

        public void DeleteItem(string id);

        public void AssignDisassign(string itemId, string modifierId);
    }
    
    public class ItemsDataAccess : IItemsDataAccess
    {
        private PointOfSaleDbContext dbContext { get; set; }

        public ItemsDataAccess(PointOfSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddItem(string name, double price, string description, int quantity, string category, string image, string code, List<string> modifiersIds)
        {
            //agregar la ruta donde se muestra la imagen
            var item = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Category = category,
                Code = code,
                Description = description,
                Image = image,
                Name = name,
                Price = price,
                Quantity = quantity,
                Modifiers = dbContext.Modifiers.Where(x => modifiersIds.Contains(x.Id) == true).ToList()
            };

            dbContext.Items.Add(item);
            dbContext.SaveChanges();
        }

        public Item GetItems(string id)
        {
            return dbContext.Items.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Item> GetItems()
        {
            return dbContext.Items.ToList();
        }

        public void UpdateItem(string id, string name, double price, string description, int quantity, string category, string image, string code, List<string> modifiersIds)
        {
            var item = dbContext.Items.Where(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            item.Category = category;
            item.Code = code;
            item.Description = description;
            item.Image = image;
            item.Name = name;
            item.Price = price;
            item.Quantity = quantity;
            item.Modifiers = dbContext.Modifiers.Where(x => modifiersIds.Contains(x.Id) == true).ToList();

            dbContext.Items.Update(item);
            dbContext.SaveChanges();
        }

        public void DeleteItem(string id)
        {
            var item = dbContext.Items.Where(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item not found");
            }
            dbContext.Items.Remove(item);
            dbContext.SaveChanges();
        }

        public void AssignDisassign(string itemId, string modifierId)
        {
            var item = dbContext.Items.Include(x => x.Modifiers).Where(x => x.Id == itemId).First();
            var modifier = dbContext.Modifiers.Where(x => x.Id == modifierId).First();
            if (item.Modifiers.Contains(modifier))
            {
                item.Modifiers.Remove(modifier);
            }
            else
            {
                item.Modifiers.Add(modifier);
            }
            dbContext.Items.Update(item);
            dbContext.SaveChanges();
        }
    }
}
