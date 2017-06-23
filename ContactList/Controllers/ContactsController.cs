using ContactList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;
using System;

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

        /*public async Task<string> Post()
        {
            string forward = await Request.Content.ReadAsStringAsync();

            var backwards = "";
            for (int i = forward.Length - 1; i >= 0; i--)
            {
                backwards += forward.Substring(i, 1);
            }
            return backwards;
        }*/

        public async Task<HttpResponseMessage> Post()
        {
            byte[] fileData = await Request.Content.ReadAsByteArrayAsync();

            if (fileData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Bitmap bitmap = new Bitmap(new MemoryStream(fileData));
            System.Console.WriteLine(bitmap.Width + ":" + bitmap.Height);

            //HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            //Response.Content = new StringContent(bitmap.Width + ":" + bitmap.Height);
            //Response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            for (int y = 0; y < bitmap.Height; y ++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    //red 50%, green 30%, blue 20%
                    Color color = bitmap.GetPixel(x, y);
                    int val = (int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    bitmap.SetPixel(x, y, Color.FromArgb(color.A, val, val, val));
                }
            }

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            fileData = ms.ToArray();

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return Response;
        }
    }
}