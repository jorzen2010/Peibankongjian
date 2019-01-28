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