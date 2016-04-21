using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.DAL
{
    public class UnitOfWork : IDisposable
    {
        public ASDbContext context = new ASDbContext();
        private GenericRepository<AccessToken> accessToken { get; set; }
        public GenericRepository<AccessToken> AccessTokenRepository
        {
            get
            {
                if (accessToken == null)
                    accessToken = new GenericRepository<AccessToken>(this, context);
                return accessToken;
            }
        }

        private GenericRepository<AgentCodeList> agentCodeList { get; set; }
        public GenericRepository<AgentCodeList> AgentCodeListRepository
        {
            get
            {
                if (agentCodeList == null)
                    agentCodeList = new GenericRepository<AgentCodeList>(this, context);
                return agentCodeList;
            }
        }

        private GenericRepository<Order> order { get; set; }
        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (order == null)
                    order = new GenericRepository<Order>(this, context);
                return order;
            }
        }

        private GenericRepository<Item> item { get; set; }
        public GenericRepository<Item> ItemRepository
        {
            get
            {
                if (item == null)
                    item = new GenericRepository<Item>(this, context);
                return item;
            }
        }


        private GenericRepository<Purchase> purchase { get; set; }
        public GenericRepository<Purchase> PurchaseRepository
        {
            get
            {
                if (purchase == null)
                    purchase = new GenericRepository<Purchase>(this, context);
                return purchase;
            }
        }

        private GenericRepository<RegisterMember> registerMember { get; set; }
        public GenericRepository<RegisterMember> RegisterMemberRepository
        {
            get
            {
                if (registerMember == null)
                    registerMember = new GenericRepository<RegisterMember>(this, context);
                return registerMember;
            }
        }

        private GenericRepository<Ticket> ticket { get; set; }
        public GenericRepository<Ticket> TicketRepository
        {
            get
            {
                if (ticket == null)
                    ticket = new GenericRepository<Ticket>(this, context);
                return ticket;
            }
        }

        private GenericRepository<JsApiTicket> jsApiTicket { get; set; }
        public GenericRepository<JsApiTicket> JsApiTicketRepository
        {
            get
            {
                if (jsApiTicket == null)
                    jsApiTicket = new GenericRepository<JsApiTicket>(this, context);
                return jsApiTicket;
            }
        }

        private GenericRepository<Invitation> invitation { get; set; }
        public GenericRepository<Invitation> InvitationRepository
        {
            get
            {
                if (invitation == null)
                    invitation = new GenericRepository<Invitation>(this, context);
                return invitation;
            }
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