using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [DataContract]
    public class Wyjatek7
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string A { get; set; }

        [DataMember]
        public int B { get; set; }
    }

    [ServiceContract]
    public interface IZadanie7
    {
        [OperationContract]
        [FaultContract(typeof(Wyjatek7))]
        void RzucWyjatek7(string a, int b);
    }

    public class Zadanie7 : IZadanie7
    {
        public void RzucWyjatek7(string a, int b)
        {
            throw new FaultException<Wyjatek7>(new Wyjatek7(), new FaultReason($"exception: {a} && {b}"));
        }
    }

    public class Server
    {
        public static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Zadanie7));
            var metadata = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadata == null)
            {
                metadata = new ServiceMetadataBehavior();
            }
            host.Description.Behaviors.Add(metadata);
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexNamedPipeBinding(), "net.pipe://localhost/metadata7");
            host.AddServiceEndpoint(typeof(IZadanie7), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad7");

            host.Open();
            Console.WriteLine("Host opened");


            Console.ReadLine();
            host.Close();
            Console.WriteLine("Host closed");
            Console.ReadLine();
        }
    }
}