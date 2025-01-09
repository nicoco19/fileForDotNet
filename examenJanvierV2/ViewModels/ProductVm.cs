using examenJanvierV2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1.ViewModels;

namespace examenJanvierV2.ViewModels
{
    internal class ProductVm : INotifyPropertyChanged
    {
        private NorthwindContext dc = new NorthwindContext();

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ProductModel> _products;
        private DelegateCommand _UpdateProduct;
        private ProductModel _selectedProduct;
        private ObservableCollection<ProductSaleModel> _salesContry;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DelegateCommand MarkAsDiscontinuedCommand
        {
            get { return _UpdateProduct = _UpdateProduct ?? new DelegateCommand(UpdateProduct); }
        }

        public ProductModel SelectedProduit
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduit));
                }
            }
        }

        public ObservableCollection<ProductModel> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = LoadProducts();
                }

                return _products;
            }
            set { _products = value; OnPropertyChanged(nameof(Products));}
        }

        public ObservableCollection<ProductSaleModel> SaleForContry 
        {
           get
            {
                if (_salesContry == null)
                {
                    _salesContry = GetProductsSoldByCountry();
                }
                return _salesContry;
            }

        }

        private ObservableCollection<ProductModel> LoadProducts()
        {
            // Filtrer directement lors de la requête à la base de données pour n'inclure que les produits non discontinués
            ObservableCollection<ProductModel> localCollection = new ObservableCollection<ProductModel>(
                dc.Products.Where(p => !p.Discontinued).Select(p => new ProductModel(p)).ToList()
            );

            return localCollection;
        }

        public ObservableCollection<ProductSaleModel> GetProductsSoldByCountry()
        {
            // Création de la liste qui contiendra les résultats
            var query = from orderDetail in dc.OrderDetails
                        join order in dc.Orders on orderDetail.OrderId equals order.OrderId
                        join product in dc.Products on orderDetail.ProductId equals product.ProductId
                        join customer in dc.Customers on order.CustomerId equals customer.CustomerId
                        where orderDetail.Quantity > 0  // Assurer qu'il y a eu une vente
                        group product by customer.Country into productGroup
                        select new ProductSaleModel
                        {
                            Country = productGroup.Key, // Le pays
                            ProductsSold = productGroup.Count() // Le nombre de produits vendus
                        };

            var resultList = query.OrderByDescending(x => x.ProductsSold).ToList();  // Convertir en liste

            // Convertir en ObservableCollection et retourner
            return new ObservableCollection<ProductSaleModel>(resultList);
        }


        private void UpdateProduct()
        {
            // Rechercher le produit dans la base de données
            Product verif = dc.Products.Where(e => e.ProductId == SelectedProduit.Product.ProductId).SingleOrDefault();
            if (verif != null)
            {
                verif.Discontinued = true;
                Products.Remove(SelectedProduit);
                OnPropertyChanged("Products");


                dc.SaveChanges();
                

                MessageBox.Show("Produit mis à jour avec succès !");
            }
            else
            {
                MessageBox.Show("Le produit sélectionné n'existe pas dans la base de données.");
            }
        }

        private void SaveProduct()
        {
            Product verif = dc.Products.Where(e => e.ProductId == SelectedProduit.Product.ProductId).SingleOrDefault();
            if (verif == null)
            {
                dc.Products.Add(SelectedProduit.Product);
            }

            dc.SaveChanges();
            MessageBox.Show("Enregistrement en base de données fait");
        }
    }
}
