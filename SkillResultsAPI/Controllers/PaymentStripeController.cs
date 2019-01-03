using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stripe;

namespace SkillResultsAPI.Controllers
{
    public class PaymentStripeController : ApiController
    {

        // GET api/createcustomer
        /// <summary>
        /// Creates Customer in Stripe
        /// </summary>
        /// <returns></returns>
        [Route("api/createcustomer/")]
        public Stripe.Customer GetCreateCustomer()
        {

            StripeConfiguration.SetApiKey(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);

            var options = new CustomerCreateOptions
            {
                Description = "Customer for jenny4.rosen@example.com",
                SourceToken = "tok_amex"
            };

            var service = new CustomerService();
            Customer customer = service.Create(options);

            return customer;

        }

        // GET api/subscribe
        /// <summary>
        /// Takes Customer and Plan IDs and creates Stripe Subscription
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        [Route("api/subscribe/")]
        public Stripe.Subscription GetSubscribeCustomer(string customer, string plan)
        {

            StripeConfiguration.SetApiKey(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);

            var items = new List<SubscriptionItemOption> {
            new SubscriptionItemOption {PlanId = plan}
};
            var options = new SubscriptionCreateOptions
            {
                CustomerId = customer,
                Items = items,
            };
            var service = new SubscriptionService();
            Subscription subscription = service.Create(options);

            return subscription;

        }


    }
}
