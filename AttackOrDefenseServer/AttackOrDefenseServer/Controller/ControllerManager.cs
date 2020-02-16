using System;
using System.Collections.Generic;
using Common;
using AttackOrDefenseServer.Servers;
using System.Reflection;

namespace AttackOrDefenseServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }
        void InitController()
        {
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            //寻找RequestCode对应的Controller（通过此Manager中的controllerDict字典来查找）
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("没有RequestCode[" + requestCode + "]对应的controller");
                return;
            }

            //寻找ActionCode对应的在Controller中的方法;(通过方法名来查找)
            string methonName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methonName);
            if (mi == null)
            {
                Console.WriteLine(requestCode + "controller中没有对应的" + methonName + "方法");
            }

            //找到后传参数调用
            object[] parameters = new object[] { data, client, server };
            object o = mi.Invoke(controller, parameters);

            //判断方法调用后是否返回了data；
            if (o == null || string.IsNullOrEmpty(o as string)) { return; }

            //返回了Data则调用SendResponse方法；
            server.SendResponse(client, actionCode, o as string);
        }
    }
}
