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

        public async Task<HttpResponseMessage> Post()
        {
            byte[] fileData = await Request.Content.ReadAsByteArrayAsync();

            if (fileData == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Bitmap bitmap = new Bitmap(new MemoryStream(fileData));

            int numClusters = 8;
            Cluster(ref bitmap, numClusters);

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            fileData = ms.ToArray();

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return Response;
        }

        public static void Cluster(ref Bitmap original, int numClusters)
        {
            bool changed = true; bool success = true;
            double[,] means = new double[numClusters, 3];
            int[] clustering = InitClustering(original.Width * original.Height, numClusters, 0);
            int maxCount = 100;
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct;
                success = UpdateMeans(original, clustering, ref means);
                changed = UpdateClustering(original, ref clustering, means);
            }
            Color[] newColors = new Color[means.GetLength(0)];
            for (int k = 0; k < means.GetLength(0); ++k)
            {
                if (means[k, 0] != -1)
                {
                    newColors[k] = Color.FromArgb((int)means[k, 0], (int)means[k, 1], (int)means[k, 2]);
                }
            }
            
            for (int i = 0; i < original.Height; ++i)
            {
                for (int j = 0; j < original.Width; ++j)
                {
                    original.SetPixel(j, i, newColors[clustering[(i * original.Width) + j]]);
                }
            }
        }

        private static int[] InitClustering(int length, int numClusters, int seed)
        {
            //Random random = new Random(seed);
            Random random = new Random();
            int[] clustering = new int[length];
            for (int i = 0; i < numClusters; ++i)
                clustering[i] = i;
            for (int i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters);
            return clustering;
        }

        private static bool UpdateMeans(Bitmap data, int[] clustering, ref double[,] means)
        {
            bool success = true;
            int numClusters = means.GetLength(0);
            int[] clusterCounts = new int[numClusters];

            for (int k = 0; k < means.GetLength(0); ++k)
            {
                means[k, 0] = 0.0;
                means[k, 1] = 0.0;
                means[k, 2] = 0.0;
            }

            for (int i = 0; i < data.Height; ++i)
            {
                for (int j = 0; j < data.Width; ++j)
                {
                    int cluster = clustering[(i * data.Width) + j];
                    ++clusterCounts[cluster];
                    Color pixel = data.GetPixel(j, i);
                    means[cluster, 0] += pixel.R; // accumulate sum
                    means[cluster, 1] += pixel.G;
                    means[cluster, 2] += pixel.B;
                }
            }

            for (int k = 0; k < means.GetLength(0); ++k)
            {
                if (clusterCounts[k] != 0)
                {
                    means[k, 0] /= clusterCounts[k];
                    means[k, 1] /= clusterCounts[k];
                    means[k, 2] /= clusterCounts[k];
                }
                else
                {
                    means[k, 0] = -1;
                }
            }

            return success;
        }

        private static bool UpdateClustering(Bitmap data, ref int[] clustering, double[,] means)
        {
            int numClusters = means.GetLength(0);
            bool changed = false;

            int[] newClustering = new int[clustering.Length];
            Array.Copy(clustering, newClustering, clustering.Length);

            double[] distances = new double[numClusters];

            for (int i = 0; i < data.Height; ++i)
            {
                for (int j = 0; j < data.Width; ++j)
                {
                    for (int k = 0; k < numClusters; ++k)
                        distances[k] = Distance(data.GetPixel(j, i), means, k);

                    int newClusterID = MinIndex(distances);
                    if (newClusterID != newClustering[(i * data.Width) + j])
                    {
                        changed = true;
                        newClustering[(i * data.Width) + j] = newClusterID;
                    }
                }
            }

            Array.Copy(newClustering, clustering, newClustering.Length);
            return changed; // at least one change
        }

        private static double Distance(Color pixel, double[,] means, int cluster)
        {
            if (means[cluster, 0] == -1)
            {
                return -1;
            }
            double sumSquaredDiffs = 0.0;
            sumSquaredDiffs += Math.Pow(pixel.R - means[cluster, 0], 2);
            sumSquaredDiffs += Math.Pow(pixel.G - means[cluster, 1], 2);
            sumSquaredDiffs += Math.Pow(pixel.B - means[cluster, 2], 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist && distances[k] != -1)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
    }
}