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

    }
}
