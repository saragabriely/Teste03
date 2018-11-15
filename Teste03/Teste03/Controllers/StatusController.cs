using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;

namespace Teste03.Controllers
{
    public class StatusController
    {
        public string url = "https://webapptestem.azurewebsites.net/api/status/";

        #region GET - Status - ID
        public async Task<Status> GetStatus(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response = await client.GetStringAsync(webService);

                var status = JsonConvert.DeserializeObject<Status>(response);

                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
