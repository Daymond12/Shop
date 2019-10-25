using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.Infraestructure
{
    using Shop.UIForms.ViewModels;
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }

      
        
    }
}
