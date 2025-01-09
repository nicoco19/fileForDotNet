using examenJanvierV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examenJanvierV2.ViewModels
{
    internal class ProductModel : INotifyPropertyChanged
    {
        private readonly Product _produit;

        public Product Product { get { return _produit; } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ProductModel(Product produit)
        {
            this._produit = produit;
        }

        public int ProductId
        {
            get { return _produit.ProductId; }
        }

        public string ProductName
        {
            get { return _produit.ProductName; }
        }

        public string Category
        {
            get { return _produit.Category.CategoryName; }
        }

        public string Fournisseur
        {
            get { return _produit.Supplier.ContactName; }
        }

        public bool Discontinued
        {
            get { return _produit.Discontinued; }
            set { _produit.Discontinued = value; }
        }
    }
}
