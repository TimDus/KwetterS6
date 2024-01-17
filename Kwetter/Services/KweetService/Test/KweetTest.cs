using KweetService.API.Models.Entity;

namespace Test
{
    public class KweetTest
    {
        public CustomerEntity customer = new CustomerEntity();

        public KweetTest()
        {
            customer.Id = 1;
            customer.CustomerName = "Test";
            customer.DisplayName = "Test";
        }

        [Fact]
        public void CreateKweet()
        {
            const string text = "this is a kweet text";
            DateTime dateTime = DateTime.Now;

            KweetEntity kweet = new KweetEntity(text, dateTime);
            kweet.Customer = customer;

            Assert.NotNull(kweet);
        }
    }
}