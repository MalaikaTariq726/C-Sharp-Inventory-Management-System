using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DbProject.Resources
{
    public class cart
    {
        
            String productName;
            int quantity = 0;
            double price;

            public cart(string productName, int quantity, double price)
            {
                this.ProductName = productName;
                this.quantity = quantity;
                this.price = price;
            }

        public string ProductName { get => productName; set => productName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
       
    }
}
