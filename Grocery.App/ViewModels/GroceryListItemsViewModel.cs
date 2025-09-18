using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        public GroceryListItemsViewModel(IGroceryListItemsService groceryListItemsService, IProductService productService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            Load(groceryList.Id);
        }

        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id)) MyGroceryListItems.Add(item);
            GetAvailableProducts();
        }

        private void GetAvailableProducts()
        {
        AvailableProducts.Clear(); //de AvailableProducts lijst is leeg.

            
            List<Product> alleProducten = _productService.GetAll();
            List<GroceryListItem> groceryListItems = _groceryListItemsService.GetAllOnGroceryListId(GroceryList.Id);
            //ophalen van de lijst met producten
            foreach (Product product in alleProducten)
            {
               
                if (product.Stock > 0) // product kan niet meer worden aangeboden als de hoeveelheid nul is.
                {
                    
                    bool staatAlOpLijst = groceryListItems.Any(item => item.ProductId == product.Id);
                    // controleert of het product al op de boodschappenlijst staat of niet
                   
                    if (!staatAlOpLijst) 
                    {
                        AvailableProducts.Add(product);//als product niet op de lijst staat gaat het in de AvailableProducts lijst
                    }
                }
            }
        }

        partial void OnGroceryListChanged(GroceryList value)
        {
            Load(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }
        [RelayCommand]
        public void AddProduct(Product product)
        {
            if (product == null || product.Id <= 0) // controleert of het product bestaat en dat de Id > dan 0.
                return;

            
            GroceryListItem groceryListItem = new GroceryListItem(0, GroceryList.Id, product.Id, 1);
            //maakt een GroceryListItem aan met Id 0 en vult de juiste ProductId en GrocerylistId in.
                

            _groceryListItemsService.Add(groceryListItem);
            //voegt het GroceryListItem toe aan de dataset met de _groceryListItemsService

            product.Stock -= 1;
            _productService.Update(product);
            //de voorraad(Stock) van het product wordt bijgewerkt en dat wordt vastgelegd

            
            GetAvailableProducts();
            //de lijst AvailableProducts wordt bijgewerkt, want dit product is niet meer beschikbaar in voorraad

            OnGroceryListChanged(GroceryList);
            // Call OnGroceryListChanged(GroceryList)



    }
    }
}
