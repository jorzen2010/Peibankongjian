using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyDal;
using SkyEntity;

namespace SkyService
{
    public class CommonService
    {
        public static Category GetCategoryById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Category cat = unitOfWork.categorysRepository.GetByID(id);
            return cat;
        }
    }
}
