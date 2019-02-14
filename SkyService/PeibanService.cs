using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyDal;
using SkyEntity;

namespace SkyService
{
    public class PeibanService
    {
        public static Book GetBookById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Book book = unitOfWork.booksRepository.GetByID(id);
            return book;
        }

        public static Ren GetRenById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Ren ren = unitOfWork.rensRepository.GetByID(id);
            return ren;
        }

        public static bool GetVipByRen(int rid)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            bool status = false;
            var orders = unitOfWork.chanpinOrdersRepository.Get(filter:u=>u.VipUser==rid&&u.ProductType==1&&u.BuyChanpin==1&&u.Status==true);
            if (orders.Count() > 0)
            {
                foreach (ChanpinOrder o in orders)
                {
                    if (o.PayTime.AddYears(1) > DateTime.Now)
                    {
                        status = true;
                        return status;
                    }
                }
                return status; 
            }
            else
            {
                return status; 
            }
        }

    }
}
