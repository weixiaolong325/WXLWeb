using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WXL.Model;

namespace WXL.ViewModel
{
   public class MenuVModel:WXL_MENU
    {
        public MenuVModel()
        {
            Childs = new List<MenuVModel>();
        }
        public List<MenuVModel> Childs { get; set; }
    }
}
