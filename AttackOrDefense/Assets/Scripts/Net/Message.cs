//
// @brief: 消息解析类(沾包、分包处理)
// @version: 1.0.0
// @author lhy
// @date: 2019/11/20
// 
// 
//

using System;
using Common;
using System.Text;
using System.Linq;

public class Message {

    private byte[] dataBuffer = new byte[1024 * 1024 * 2];
    private int startIndex = 0;

    public byte[] GetData { get { return dataBuffer; } }
    public int GetStartIndex { get { return startIndex; } }
    public int GetRemainSize { get { return dataBuffer.Length - startIndex; } }
    public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallBack)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(dataBuffer, 0);
            if ((startIndex - 4) >= count)
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(dataBuffer, 4);
                string s = Encoding.UTF8.GetString(dataBuffer, 8, count - 4);
                processDataCallBack(actionCode, s);
                Array.Copy(dataBuffer, 4 + count, dataBuffer, 0, startIndex - count - 4);
                startIndex -= (count + 4);
            }
            else { break; }
        }
    }
    public static byte[] PackData(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] dataAmountBytes = BitConverter.GetBytes(requestCodeBytes.Length + actionCodeBytes.Length + dataBytes.Length);
        return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>()
            .Concat(actionCodeBytes).ToArray<byte>()
            .Concat(dataBytes).ToArray<byte>();
    }
}
