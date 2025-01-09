using examenV1Semptembre.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace examenV1Semptembre.ViewModels
{
    internal class ProductVM : INotifyPropertyChanged
    {
        private NorthwindContext dc = new NorthwindContext();

        public event PropertyChangedEventHandler PropertyChanged;
        private ProductModel _selectedProduct;
        private ObservableCollection<ProductModel> _products;
        private ObservableCollection<TotalVentesProduitModel> _totalVentesProduits;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        }

        public ObservableCollection<TotalVentesProduitModel> TotalVentesProduits
        {
            get
            {
                if (_totalVentesProduits == null)
                {
                    _totalVentesProduits = GetTotalVentesParProduit();
                }

                return _totalVentesProduits;
            }
        }


        private ObservableCollection<ProductModel> LoadProducts()
        {
            ObservableCollection<ProductModel> localCollection = new ObservableCollection<ProductModel>();
            foreach (var item in dc.Products)
            {
                localCollection.Add(new ProductModel(item));
            }
            return localCollection;
        }

        private ObservableCollection<TotalVentesProduitModel> GetTotalVentesParProduit()
        {
            var totalVentesParProduit = dc.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new TotalVentesProduitModel
                {
                    ProductId = g.Key,
                    TotalVentes = g.Sum(od => od.Quantity * (od.UnitPrice))
                })
                .ToList();

            return new ObservableCollection<TotalVentesProduitModel>(totalVentesParProduit);
        }

        public void UpdateProduct()
        {
            Product verif = dc.Products.Where(e => e.ProductId == SelectedProduit.Product.ProductId).SingleOrDefault();
            if (verif != null)
            {
                verif.ProductName = SelectedProduit.ProductName;
                verif.QuantityPerUnit = SelectedProduit.QuantityPerUnit;
                dc.SaveChanges();
                MessageBox.Show("Produit mis à jour avec succès !");
            }
            else
            {
                MessageBox.Show("Le produit sélectionné n'existe pas dans la base de données.");
            }
        }

        public void SaveProduct()
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
