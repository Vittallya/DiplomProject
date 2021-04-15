using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class Client: Human
    {

        public string PasportData { get; set; }


        public override TClient Clone<TClient>()
        {
            var human = base.Clone<Client>();

            var client = human as Client;

            client.PasportData = PasportData;

            return client as TClient;
        }

        public override void UpdateData(IDbClass dbClass)
        {

            base.UpdateData(dbClass);

            if(dbClass is Client client)
            {
                PasportData = client.PasportData;
            }
        }
    }
}
