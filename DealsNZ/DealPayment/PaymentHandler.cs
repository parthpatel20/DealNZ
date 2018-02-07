﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;
using System.Configuration;
using System.Web.UI;
 
namespace DealsNZ.DealPayment
{
    public class PaymentHandler
    {
        // Authenticate with PayPal
        public Dictionary<string, string> config;
        OAuthTokenCredential auth;
        APIContext apiContext;

        // Shipping Price
        public const double _fixedShipping = 0;
        const int _taxpercent = 0;
        const string _Currency = "NZD";
        int userId;
        public APIContext GetApiContext()
        {
            config = PayPal.Api.ConfigManager.Instance.GetProperties();
            auth = new OAuthTokenCredential(config);
            apiContext = new APIContext(auth.GetAccessToken());
            return apiContext;
        }

        public PaymentHandler(List<PaypalItem> items,int uid)
        {
            if(apiContext==null)
            {
                GetApiContext();
            }
            userId = uid;
           
            var payer = new Payer() { payment_method = "paypal"};

            var guid = Convert.ToString((new Random()).Next(100000));

            //string DomainName = "http://localhost:20629";
            string DomainName = "dealnz-001-site1.itempurl.com";
            //string DomainName = "https://localhost:44394";
            var redirUrls = new RedirectUrls()
            {
                cancel_url = DomainName+"/Cancel/",
                return_url = DomainName + "/Wallet/PaymentSucess/"
            };

            // Add Items to List
            ItemList itemList = new ItemList();
            itemList.items = new List<Item>();

            double subtotal=0;
            double taxamount = 0;
            foreach(PaypalItem item in items)
            {
                List<NameValuePair> nl = new List<NameValuePair>();
                NameValuePair sizeadd = new NameValuePair();
                sizeadd.name = "Size";
                sizeadd.value = item.size;
                nl.Add(sizeadd);

                PayPal.Api.Item tmp = new Item
                {
                  
                    currency = item.currency,
                    price = item.price.ToString(),
                    quantity = "1",
                    sku = item.sku,
                    
                };

                subtotal += ( item.price * 1);
                itemList.items.Add(tmp);
            }

            taxamount =0;

            var details = new Details()
            {
                tax = taxamount.ToString(),
                shipping = "0",
                subtotal = subtotal.ToString()
            };

            double total = taxamount + _fixedShipping + subtotal;

            var amount = new Amount()
            {
                currency = _Currency,
                total = total.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            Random rand = new Random(DateTime.Now.Second);
            String invoice = "INV-" + System.DateTime.Now.Ticks.ToString();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = invoice,
                amount = amount,
                item_list = itemList
            });

            PayPal.Api.Payment payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                redirect_urls = redirUrls,
                transactions = transactionList
            };

            var createdPayment = payment.Create(apiContext);

        //    PopulateOrder(invoice , items, amount);

            var links = createdPayment.links.GetEnumerator();
            while (links.MoveNext())
            {
                var link = links.Current;
                if (link.rel.ToLower().Trim().Equals("approval_url"))
                {
                    HttpContext.Current.Response.Redirect(link.href);
                }
            }
            
        }
        
    }

}


   
        