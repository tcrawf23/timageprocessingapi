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

            Bitmap original = new Bitmap(new MemoryStream(fileData));

            //HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            //Response.Content = new StringContent(bitmap.Width + ":" + bitmap.Height);
            //Response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            /*for (int y = 0; y < bitmap.Height; y ++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    //red 30%, green 59%, blue 11%
                    Color color = bitmap.GetPixel(x, y);
                    int val = (int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    bitmap.SetPixel(x, y, Color.FromArgb(color.A, val, val, val));
                }
            }*/

            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
            new float[][]
            {
                new float[] {.3f, .3f, .3f, 0, 0},
                new float[] {.59f, .59f, .59f, 0, 0},
                new float[] {.11f, .11f, .11f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            MemoryStream ms = new MemoryStream();
            newBitmap.Save(ms, ImageFormat.Jpeg);
            fileData = ms.ToArray();

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return Response;
        }
    }
}