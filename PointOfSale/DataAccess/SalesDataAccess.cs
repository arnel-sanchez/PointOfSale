using PointOfSale.Models.DataBaseModels;

namespace PointOfSale.DataAccess
{
    public interface ISalesDataAccess
    {
        public void AddSale(string userId, List<string> itemsId, List<string> modifiersId, decimal totalPrice);
        
        public List<Sale> GetSales(string userId);

        public void DeleteSale(string userId, string saleId);

        public void DeleteSales(string userId);

        public List<Sale> GetSales();

        public void UpdateSale(string userId, string saleId, List<string> itemsId, List<string> modifiersId, decimal totalPrice);
    }
    
    public class SalesDataAccess : ISalesDataAccess
    {
        private PointOfSaleDbContext dbContext { get; set; }

        public SalesDataAccess(PointOfSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddSale(string userId, List<string> itemsId, List<string> modifiersId, decimal totalPrice)
        {
            var items = dbContext.Items.Where(x => itemsId.Contains(x.Id)).ToList();
            var modifiers = dbContext.Modifiers.Where(x => modifiersId.Contains(x.Id)).ToList();
            var seller = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (seller == null)
            {
                throw new Exception("User not found");
            }
            if (items.Count != itemsId.Count || modifiers.Count != modifiersId.Count)
            {
                throw new Exception("Some items or modifiers not found");
            }
            if(items.Count == 0)
            {
                throw new Exception("No items found");
            }
            var sale = new Sale
            {
                Id = Guid.NewGuid().ToString(),
                Seller = seller,
                Items = items,
                Modifiers = modifiers,
                TotalPrice = totalPrice
            };
            dbContext.Sales.Add(sale);
            dbContext.SaveChanges();
        }

        public List<Sale> GetSales(string userId)
        {
            return dbContext.Sales.Where(x => x.Seller.Id == userId).ToList();
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

        public List<Sale> GetSales()
        {
            return dbContext.Sales.ToList();
        }

        public void UpdateSale(string userId, string saleId, List<string> itemsId, List<string> modifiersId, decimal totalPrice)
        {
            var items = dbContext.Items.Where(x => itemsId.Contains(x.Id)).ToList();
            var modifiers = dbContext.Modifiers.Where(x => modifiersId.Contains(x.Id)).ToList();
            var seller = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            var sale = dbContext.Sales.FirstOrDefault(x => x.Id == saleId);
            if (sale == null)
            {
                throw new Exception("Sale not found");
            }
            if (seller == null)
            {
                throw new Exception("User not found");
            }
            if (items.Count != itemsId.Count || modifiers.Count != modifiersId.Count)
            {
                throw new Exception("Some items or modifiers not found");
            }
            if (items.Count == 0)
            {
                throw new Exception("No items found");
            }
            sale.Seller = seller;
            sale.Items = items;
            sale.Modifiers = modifiers;
            sale.TotalPrice = totalPrice;
            dbContext.Sales.Update(sale);
            dbContext.SaveChanges();
        }
    }
}
