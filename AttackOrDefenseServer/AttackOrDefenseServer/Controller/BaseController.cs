using Common;
using AttackOrDefenseServer.Servers;

namespace AttackOrDefenseServer.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;
        public RequestCode RequestCode
        {
            get
            {
                return requestCode;
            }
        }
        public virtual string DefaultHandler(string data, Client client, Server server) { return null; }
    }
}
