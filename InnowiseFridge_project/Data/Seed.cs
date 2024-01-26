using InnowiseFridge_project.Models;
using Microsoft.EntityFrameworkCore;

namespace InnowiseFridge_project.Data;

public static class Seed
{
    public static async Task Seeding(DataContext context)
    {
        if (await context.Fridges.AnyAsync()) return;

        var fridgeModel1 = new FridgeModel()
        {
            Name = "Samsung", Year = 2020
        };
        
        var fridgeModel2 = new FridgeModel()
        {
            Name = "LG", Year = 2019
        };
        
        var fridgeModel3 = new FridgeModel()
        {
            Name = "Whirlpool", Year = 2021
        };
        
        var fridgeModel4 = new FridgeModel()
        {
            Name = "Bosch", Year = 2018
        };
        
        var fridgeModel5 = new FridgeModel()
        {
            Name = "Haier", Year = 2022
        };

        await context.AddRangeAsync(fridgeModel1, fridgeModel2, fridgeModel3, fridgeModel4, fridgeModel5);

        var product1 = new Product() { Name = "Bananas", DefaultQuantity = 5 };
        
        var product2 = new Product() { Name = "Milk", DefaultQuantity = 2 };
        
        var product3 = new Product() { Name = "Bread", DefaultQuantity = 1 };

        var product4 = new Product() { Name = "Eggs", DefaultQuantity = 12 };

        var product5 = new Product() { Name = "Coffee", DefaultQuantity = 1 };

        var product6 = new Product() { Name = "Cheese", DefaultQuantity = 200 };

        var product7 = new Product() { Name = "Tomatoes", DefaultQuantity = 3 };

        var product8 = new Product() { Name = "Potatoes", DefaultQuantity = 5 };

        var product9 = new Product() { Name = "Honey", DefaultQuantity = 250 };

        var product10 = new Product() { Name = "Pork", DefaultQuantity = 1000 };

        await context.AddRangeAsync(product1, product2, product3, product4, product5, product6, 
            product7, product8, product9, product10);

        var fridge1 = new Fridge() { Name = "KitchenFrost", OwnerName = "Anna", FridgeModel = fridgeModel1 };

        var fridge2 = new Fridge() { Name = "CoolMaster", OwnerName = "Alex", FridgeModel = fridgeModel2 };
        
        var fridge3 = new Fridge() { Name = "IceBoxPro", OwnerName = "Olga", FridgeModel = fridgeModel3 };
        
        var fridge4 = new Fridge() { Name = "ChillCrafter", OwnerName = "Max", FridgeModel = fridgeModel4 };
        
        var fridge5 = new Fridge() { Name = "FreezeMagic", OwnerName = "Elena", FridgeModel = fridgeModel5 };
        
        var fridge6 = new Fridge() { Name = "ArcticElite", OwnerName = "Ivan", FridgeModel = fridgeModel1 };
        
        var fridge7 = new Fridge() { Name = "FrostyParadise", OwnerName = "Svetlana", FridgeModel = fridgeModel2 };
        
        var fridge8 = new Fridge() { Name = "PolarPalace", OwnerName = "Dmitry", FridgeModel = fridgeModel3 };
        
        var fridge9 = new Fridge() { Name = "GlacierGem", OwnerName = "Maria", FridgeModel = fridgeModel4 };
        
        var fridge10 = new Fridge() { Name = "IcyDream", OwnerName = "Viktor", FridgeModel = fridgeModel5 };

        await context.AddRangeAsync(fridge1, fridge2, fridge3, fridge4, fridge5, fridge6, 
            fridge7, fridge8, fridge9, fridge10);

        var fridgeProduct1 = new FridgeProduct() { Fridge = fridge1, Product = product1, Quantity = 3 };
       
        var fridgeProduct2 = new FridgeProduct() { Fridge = fridge2, Product = product3, Quantity = 2 };
        
        var fridgeProduct3 = new FridgeProduct() { Fridge = fridge3, Product = product2, Quantity = 5 };
        
        var fridgeProduct4 = new FridgeProduct() { Fridge = fridge4, Product = product4, Quantity = 1 };
        
        var fridgeProduct5 = new FridgeProduct() { Fridge = fridge5, Product = product5, Quantity = 4 };
        
        var fridgeProduct6 = new FridgeProduct() { Fridge = fridge6, Product = product6, Quantity = 200 };
        
        var fridgeProduct7 = new FridgeProduct() { Fridge = fridge7, Product = product7, Quantity = 3 };
        
        var fridgeProduct8 = new FridgeProduct() { Fridge = fridge8, Product = product8, Quantity = 5 };
        
        var fridgeProduct9 = new FridgeProduct() { Fridge = fridge9, Product = product9, Quantity = 250 };
        
        var fridgeProduct10 = new FridgeProduct() { Fridge = fridge10, Product = product10, Quantity = 1000 };

        await context.AddRangeAsync(fridgeProduct1, fridgeProduct2, fridgeProduct3,
            fridgeProduct4, fridgeProduct5, fridgeProduct6, fridgeProduct7, fridgeProduct8,
            fridgeProduct9, fridgeProduct10);

        await context.SaveChangesAsync();
    } 
}