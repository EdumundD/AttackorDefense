using System;
using System.Linq;
using System.Text;
using Common;

namespace AttackOrDefenseServer.Servers
{
    class Message
    {
        private byte[] dataBuffer = new byte[1024];
        private int startIndex = 0;

        public byte[] GetData { get { return dataBuffer; } }
        public int GetStartIndex { get { return startIndex; } }

        public int GetRemainSize { get { return dataBuffer.Length - startIndex; } }

        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> processDataCallBack)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(dataBuffer, 0);
                if((startIndex - 4)>= count)
                {
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(dataBuffer, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(dataBuffer, 8);
                    string s = Encoding.UTF8.GetString(dataBuffer, 12, count - 8);
                    processDataCallBack(requestCode, actionCode, s);
                    Array.Copy(dataBuffer, 4 + count, dataBuffer,0, startIndex - count - 4);
                    startIndex -= (count + 4);
                }
                else { break; }
            }
        }
        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] dataAmountBytes = BitConverter.GetBytes(actionCodeBytes.Length + dataBytes.Length);
            return dataAmountBytes.Concat(actionCodeBytes).ToArray<byte>().Concat(dataBytes).ToArray<byte>();
        }
    }
}
