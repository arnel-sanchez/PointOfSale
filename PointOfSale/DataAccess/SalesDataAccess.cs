using PointOfSale.Models.DataBaseModels;

namespace PointOfSale.DataAccess
{
    public interface ISalesDataAccess
    {
        public void AddSale(string userId, List<(string, List<string>)> itemsModifiersIds);
        
        public List<Sale> GetSales(DateTime dateTime, string userId = null);

        public void DeleteSale(string userId, string saleId);

        public void DeleteSales(string userId);

        public void UpdateSale(string userId, string saleId, List<(string, List<string>)> itemsModifiersIds);
    }
    
    public class SalesDataAccess : ISalesDataAccess
    {
        private PointOfSaleDbContext dbContext { get; set; }

        public SalesDataAccess(PointOfSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddSale(string userId, List<(string, List<string>)> itemsModifiersIds)
        {
            double totalPrice = 0;
            List<ModifiersByItem> items = new List<ModifiersByItem>();
            foreach (var itemModifierId in itemsModifiersIds)
            {
                double price = 0;
                var item = dbContext.Items.Find(itemModifierId.Item1);
                if (item == null)
                {
                    throw new Exception($"Item {itemModifierId.Item1} not found");
                }
                price = item.Price;
                List<Modifier> modifiers = new List<Modifier>();
                foreach (var modifierId in itemModifierId.Item2)
                {
                    var modifier = dbContext.Modifiers.Find(modifierId);
                    if (modifier == null)
                    {
                        throw new Exception($"Modifier {modifierId} not found");
                    }
                    if (modifier.Add)
                    {
                        price += modifier.Price;
                    }
                    else
                    {
                        price -= modifier.Price;
                    }
                    modifiers.Add(modifier);
                }
                items.Add(new ModifiersByItem { Id = Guid.NewGuid().ToString(), Item = item, Modifiers = modifiers });
                totalPrice += price;
            }
            var seller = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (seller == null)
            {
                throw new Exception($"User {userId} not found");
            }
            if(items.Count == 0)
            {
                throw new Exception("Items not found");
            }
            var sale = new Sale
            {
                Id = Guid.NewGuid().ToString(),
                Seller = seller,
                Items = items,
                TotalPrice = totalPrice,
                DateTime = DateTime.UtcNow
            };
            dbContext.Sales.Add(sale);
            dbContext.SaveChanges();
        }

        public List<Sale> GetSales(DateTime dateTime, string userId = null)
        {
            if (userId == null)
            {
                return dbContext.Sales.Where(x => x.DateTime == dateTime).ToList();
            }
            else
            {
                return dbContext.Sales.Where(x => x.DateTime == dateTime && x.Seller.Id == userId).ToList();
            }
        }

        public void DeleteSale(string userId, string saleId)
        {
            var sale = dbContext.Sales.FirstOrDefault(x => x.Id == saleId && x.Seller.Id == userId);
            if (sale == null)
            {
                throw new Exception("Sale not found");
            }
            dbContext.Sales.Remove(sale);
            dbContext.SaveChanges();
        }

        public void DeleteSales(string userId)
        {
            dbContext.Sales.RemoveRange(dbContext.Sales.Where(x => x.Seller.Id == userId));
        }

        public void UpdateSale(string userId, string saleId, List<(string, List<string>)> itemsModifiersIds)
        {
            double totalPrice = 0;
            List<ModifiersByItem> items = new List<ModifiersByItem>();
            foreach (var itemModifierId in itemsModifiersIds)
            {
                double price = 0;
                var item = dbContext.Items.Find(itemModifierId.Item1);
                if (item == null)
                {
                    throw new Exception($"Item {itemModifierId.Item1} not found");
                }
                price = item.Price;
                List<Modifier> modifiers = new List<Modifier>();
                foreach (var modifierId in itemModifierId.Item2)
                {
                    var modifier = dbContext.Modifiers.Find(modifierId);
                    if (modifier == null)
                    {
                        throw new Exception($"Modifier {modifierId} not found");
                    }
                    if (modifier.Add)
                    {
                        price += modifier.Price;
                    }
                    else
                    {
                        price -= modifier.Price;
                    }
                    modifiers.Add(modifier);
                }
                totalPrice += price;
                items.Add(new ModifiersByItem { Id = Guid.NewGuid().ToString(), Item = item, Modifiers = modifiers });
            }
            var seller = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (seller == null)
            {
                throw new Exception($"User {userId} not found");
            }
            if (items.Count == 0)
            {
                throw new Exception("Items not found");
            }
            var sale = dbContext.Sales.FirstOrDefault(x => x.Id == saleId && x.Seller.Id == userId);
            if (sale == null)
            {
                throw new Exception($"Sale {saleId} not found");
            }
            sale.Seller = seller;
            sale.Items = items;
            sale.TotalPrice = totalPrice;
            dbContext.Sales.Update(sale);
            dbContext.SaveChanges();
        }
    }
}
