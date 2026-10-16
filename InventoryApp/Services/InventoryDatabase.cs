using InventoryApp.Models;
using SQLite;

namespace InventoryApp.Services
{
    public class InventoryDatabase
    {
        SQLiteAsyncConnection database;

        private const string DatabaseFilename = "InventorySQLite.db3";

        private const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        private readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
        
        private const int pageSize= 10;
        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(DatabasePath, Flags);

            await database.CreateTableAsync<Product>();

#if DEBUG
            //get stored product count and if there is no product in the DB add some dummy data
            var cnt = await database.ExecuteScalarAsync<int>("SELECT COUNT(ID) FROM Product");
            if(cnt == 0)
            {
                await SeedProductsAsync();
            }
#endif
        }

        public async Task<List<Product>> GetProductsAsync(int pageCount)
        {
            int offset = (pageCount - 1) * pageSize;

            return await database.Table<Product>()
                .OrderBy(p => p.ID)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetProductCountAsync()
        {
            await Init();
            return await database.Table<Product>().CountAsync();
        }
        public async Task<int> GetProductCountForNameAsync(string name)
        {
            return await database.ExecuteScalarAsync<int>("SELECT COUNT(ID) FROM Product WHERE Name LIKE '?'", $"%{name}%");
        }
        public async Task<int> GetProductCountForBarcodeAsync(string barcode)
        {
            return await database.ExecuteScalarAsync<int>("SELECT COUNT(ID) FROM Product WHERE Barcode LIKE '?'", $"%{barcode}%");
        }
        public async Task SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                var res = await database.Table<Product>().Where(p=>p.ID== product.ID).FirstOrDefaultAsync();
                if (res is not null)
                {
                    await database.UpdateAsync(product);
                }
                else
                {
                    //do nothing, it was deleted before
                    return;
                }                
            }                
            else
                await database.InsertAsync(product);

        }
        public async Task DeleteProductAsync(Product product)
        {
            await database.DeleteAsync(product);
        }
        public async Task SeedProductsAsync()
        {
            var list = new List<Product>();
            for (int i = 0; i < 1000; i++)
            {

                list.Add(new Product()
                {
                    ID = i,
                    Name = $"Test product {i}",
                    Barcode = $"barcode {i}",
                    Price = (10 * i) + 1,
                    Quantity = i
                });
            }
            
            await database.InsertAllAsync(list);
        }
        public async Task<List<Product>> SearchProductsByName(string name, int pageCount=1)
        {
            int offset = (pageCount - 1) * pageSize;

            return await database.Table<Product>()
                .Where(p => p.Name.Contains(name))
                .OrderBy(p => p.ID)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Product>> SearchProductsByBarcode(string barcode, int pageCount=1)
        {
            int offset = (pageCount - 1) * pageSize;

            var res = await database.Table<Product>()
                .Where(p => p.Barcode.Contains(barcode))
                .OrderBy(p => p.ID)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
            return res;
        }
    }
}
