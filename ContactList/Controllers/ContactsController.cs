using ContactList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

namespace ContactList.Controllers
{
    public class ContactsController : ApiController
    {
        private Contact[] GetContacts()
        {
            var contacts = new Contact[]{
                        new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                        new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                        new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"},
                        new Contact { Id = 4, EmailAddress = "jelly@flavor.com", Name = "Peanut Butter"}
                        };

            return contacts;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return GetContacts();
        }

        public Contact Get([FromUri] int id)
        {
            var contacts = GetContacts();
            return contacts.FirstOrDefault(x => x.Id == id);
        }

        public async Task<string> Post()
        {
            string forward = await Request.Content.ReadAsStringAsync();

            var backwards = "";
            for (int i = forward.Length - 1; i >= 0; i--)
            {
                backwards += forward.Substring(i, 1);
            }
            return backwards;
        }
    }
}