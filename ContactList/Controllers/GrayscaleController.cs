using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace ContactList.Controllers
{
    public class GrayscaleController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            byte[] fileData = await Request.Content.ReadAsByteArrayAsync();

            if (fileData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Bitmap bitmap = new Bitmap(new MemoryStream(fileData));


            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

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
            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);
            
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