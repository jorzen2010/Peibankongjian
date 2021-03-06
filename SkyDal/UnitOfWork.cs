﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SkyEntity;


namespace SkyDal
{
    public class UnitOfWork : IDisposable
    {
        private SkyWebContext context = new SkyWebContext();



         private GenericRepository<Setting> SettingsRepository;

         public GenericRepository<Setting> settingsRepository
         {
             get
             {

                 if (this.SettingsRepository == null)
                 {
                     this.SettingsRepository = new GenericRepository<Setting>(context);
                 }
                 return SettingsRepository;
             }
         }

         private GenericRepository<SysUser> SysUsersRepository;

         public GenericRepository<SysUser> sysUsersRepository
         {
             get
             {

                 if (this.SysUsersRepository == null)
                 {
                     this.SysUsersRepository = new GenericRepository<SysUser>(context);
                 }
                 return SysUsersRepository;
             }
         }
         private GenericRepository<Category> CategorysRepository;

         public GenericRepository<Category> categorysRepository
         {
             get
             {

                 if (this.CategorysRepository == null)
                 {
                     this.CategorysRepository = new GenericRepository<Category>(context);
                 }
                 return CategorysRepository;
             }
         }

         private GenericRepository<Notice> NoticesRepository;

         public GenericRepository<Notice> noticesRepository
         {
             get
             {

                 if (this.NoticesRepository == null)
                 {
                     this.NoticesRepository = new GenericRepository<Notice>(context);
                 }
                 return NoticesRepository;
             }
         }

         private GenericRepository<Guanggao> GuanggaoReplysRepository;

         public GenericRepository<Guanggao> guanggaosRepository
         {
             get
             {

                 if (this.GuanggaoReplysRepository == null)
                 {
                     this.GuanggaoReplysRepository = new GenericRepository<Guanggao>(context);
                 }
                 return GuanggaoReplysRepository;
             }
         }

         private GenericRepository<WechatReply> WechatReplysRepository;

         public GenericRepository<WechatReply> wechatReplysRepository
         {
             get
             {

                 if (this.WechatReplysRepository == null)
                 {
                     this.WechatReplysRepository = new GenericRepository<WechatReply>(context);
                 }
                 return WechatReplysRepository;
             }
         }

         private GenericRepository<Ren> RensRepository;

         public GenericRepository<Ren> rensRepository
         {
             get
             {

                 if (this.RensRepository == null)
                 {
                     this.RensRepository = new GenericRepository<Ren>(context);
                 }
                 return RensRepository;
             }
         }

         private GenericRepository<Book> BooksRepository;

         public GenericRepository<Book> booksRepository
         {
             get
             {

                 if (this.BooksRepository == null)
                 {
                     this.BooksRepository = new GenericRepository<Book>(context);
                 }
                 return BooksRepository;
             }
         }

         private GenericRepository<Renwu> RenwusRepository;

         public GenericRepository<Renwu> renwusRepository
         {
             get
             {

                 if (this.RenwusRepository == null)
                 {
                     this.RenwusRepository = new GenericRepository<Renwu>(context);
                 }
                 return RenwusRepository;
             }
         }

         private GenericRepository<Product> ProductsRepository;

         public GenericRepository<Product> productsRepository
         {
             get
             {

                 if (this.ProductsRepository == null)
                 {
                     this.ProductsRepository = new GenericRepository<Product>(context);
                 }
                 return ProductsRepository;
             }
         }

         private GenericRepository<RenwuDaka> RenwuDakasRepository;

         public GenericRepository<RenwuDaka> renwuDakasRepository
         {
             get
             {

                 if (this.RenwuDakasRepository == null)
                 {
                     this.RenwuDakasRepository = new GenericRepository<RenwuDaka>(context);
                 }
                 return RenwuDakasRepository;
             }
         }

         private GenericRepository<ChanpinOrder> ChanpinOrdersRepository;

         public GenericRepository<ChanpinOrder> chanpinOrdersRepository
         {
             get
             {

                 if (this.ChanpinOrdersRepository == null)
                 {
                     this.ChanpinOrdersRepository = new GenericRepository<ChanpinOrder>(context);
                 }
                 return ChanpinOrdersRepository;
             }
         }

         private GenericRepository<RenKongList> RenKongListsRepository;

         public GenericRepository<RenKongList> renKongListsRepository
         {
             get
             {

                 if (this.RenKongListsRepository == null)
                 {
                     this.RenKongListsRepository = new GenericRepository<RenKongList>(context);
                 }
                 return RenKongListsRepository;
             }
         }

         private GenericRepository<ViewHistory> ViewHistorysRepository;

         public GenericRepository<ViewHistory> _viewHistorysRepository
         {
             get
             {

                 if (this.ViewHistorysRepository == null)
                 {
                     this.ViewHistorysRepository = new GenericRepository<ViewHistory>(context);
                 }
                 return ViewHistorysRepository;
             }
         }

         private GenericRepository<BijiPinglun> BijiPinglunsRepository;

         public GenericRepository<BijiPinglun> _bijiPinglunsRepository
         {
             get
             {

                 if (this.BijiPinglunsRepository == null)
                 {
                     this.BijiPinglunsRepository = new GenericRepository<BijiPinglun>(context);
                 }
                 return BijiPinglunsRepository;
             }
         }

         private GenericRepository<PinglunReply> PinglunReplysRepository;

         public GenericRepository<PinglunReply> _pinglunReplysRepository
         {
             get
             {

                 if (this.PinglunReplysRepository == null)
                 {
                     this.PinglunReplysRepository = new GenericRepository<PinglunReply>(context);
                 }
                 return PinglunReplysRepository;
             }
         }

         private GenericRepository<BijiDianzan> BijiDianzansRepository;

         public GenericRepository<BijiDianzan> _bijiDianzansRepository
         {
             get
             {

                 if (this.BijiDianzansRepository == null)
                 {
                     this.BijiDianzansRepository = new GenericRepository<BijiDianzan>(context);
                 }
                 return BijiDianzansRepository;
             }
         }

         private GenericRepository<DianzanPinglun> DianzanPinglunsRepository;

         public GenericRepository<DianzanPinglun> _dianzanPinglunsRepository
         {
             get
             {

                 if (this.DianzanPinglunsRepository == null)
                 {
                     this.DianzanPinglunsRepository = new GenericRepository<DianzanPinglun>(context);
                 }
                 return DianzanPinglunsRepository;
             }
         }
   
   
   
   

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}