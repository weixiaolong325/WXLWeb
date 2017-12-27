using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.Model;

namespace WXL.ViewModel.Admin
{
    public class UserVModel
    {
        public WXL_Users User{get;set;}
        public List<WXL_Role> Roles { get; set; }
    }
}
