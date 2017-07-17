using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Ethereum.Api
{
    public class EthereumAPI
    {
        private readonly string NodeRPC;

        public EthereumAPI(string node_rpc)
        {
            NodeRPC = node_rpc;
        }

        #region Accounts(wallets)
        public void UnlockAccount(string address, string passPhrase)
        {
            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "personal_unlockAccount";
            request["params"] = new JArray(address, passPhrase);
            request["id"] = 1;

            string command = request.ToString();

            string res = CallJSonRPC(command);
        }

        public void LockAccount(string address)
        {
            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "personal_lockAccount";
            request["params"] = new JArray(address);
            request["id"] = 1;

            string command = request.ToString();

            string res = CallJSonRPC(command);
        }

        public string CreateAccount(string passPhrase)
        {
            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "personal_newAccount";
            request["params"] = new JArray(passPhrase);
            request["id"] = 1;

            string command = request.ToString();

            string res = CallJSonRPC(command);

            return JObject.Parse(res)["result"].Value<string>();
        }

        public decimal GetBalance(string address)
        {
            decimal result = 0;

            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "eth_getBalance";
            request["params"] = new JArray(address, "latest");
            request["id"] = 1;

            string res = CallJSonRPC(request.ToString());

            string balance = JObject.Parse(res)["result"].Value<string>();

            //convert from hex to decimal
            HexBigInteger hexint = new HexBigInteger(balance);
            result = UnitConversion.Convert.FromWei(hexint);

            return result;
        }
        #endregion

        #region Transactions

        /// <summary>
        /// Send transaction from -> to, returning TxHash( transaction id ), using default txFee
        /// </summary>
        /// <param name="from">wallet address</param>
        /// <param name="to">wallet address</param>
        /// <param name="amount">transaction amount in Ether</param>
        public string SendTransaction(string from, string to, decimal amount)
        {
            //отсутствует обработчик ошибок

            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "eth_sendTransaction";

            JObject param = new JObject();
            param["from"] = from;
            param["to"] = to;
            param["value"] = EthereumUtils.BigintToHex(UnitConversion.Convert.ToWei(amount, UnitConversion.EthUnit.Ether));

            request["params"] = new JArray(param);
            request["id"] = 1;

            string command = request.ToString();

            string res = CallJSonRPC(command);
            return JObject.Parse(res)["result"].Value<string>();
        }


        /// <summary>
        /// Return transaction from blockchain using transaction hash (id)
        /// </summary>

        public ETHTransaction GetTransactionByHash(string hash)
        {
            //отсутствует обработчик ошибок

            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "eth_getTransactionByHash";

            request["params"] = new JArray(hash);
            request["id"] = 1;

            string command = request.ToString();

            string json = CallJSonRPC(command);

            JObject obj = JObject.Parse(json)["result"].Value<JObject>();

            ETHTransaction trans = null;

            if (obj != null)
            {
                trans = new ETHTransaction();

                trans.hash = obj["hash"].Value<string>();

                try
                {
                    //a pending transaction.blockNumber = null. then blockNumber = 0;
                    trans.blockNumber = EthereumUtils.HexToULong(obj["blockNumber"].Value<string>());

                }
                catch (Exception)
                {
                }

                trans.nonce = EthereumUtils.HexToULong(obj["nonce"].Value<string>());
                trans.transactionIndex = EthereumUtils.HexToULong(obj["transactionIndex"].Value<string>());

                trans.value = UnitConversion.Convert.FromWei(EthereumUtils.HexToBigInteger(obj["value"].Value<string>()), UnitConversion.EthUnit.Ether);
                trans.gasPrice = UnitConversion.Convert.FromWei(EthereumUtils.HexToBigInteger(obj["gasPrice"].Value<string>()), UnitConversion.EthUnit.Ether);
                trans.gas = UnitConversion.Convert.FromWei(EthereumUtils.HexToBigInteger(obj["gas"].Value<string>()), UnitConversion.EthUnit.Ether);

                trans.from = obj["from"].Value<string>();
                trans.to = obj["to"].Value<string>();
            }

            //trans.

            return trans;
        }

        public ulong GetBlockNumber()
        {
            JObject request = new JObject();
            request["jsonrpc"] = "2.0";
            request["method"] = "eth_blockNumber";
            request["params"] = null;
            request["id"] = 1;

            string res = CallJSonRPC(request.ToString());

            string bnumber = JObject.Parse(res)["result"].Value<string>();

            return EthereumUtils.HexToULong(bnumber);
        }

        #endregion

        private string CallJSonRPC(string command)
        {
            HttpClient client = new HttpClient();

            var content = new StringContent(command, Encoding.UTF8, "application/json");

            var result = client.PostAsync(NodeRPC, content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }
    }

    public class ETHTransaction
    {
        public string hash { get; set; }
        public ulong nonce { get; set; }
        /// <summary>
        /// number of block where transaction stored, if transaction is pending or canceled value is 0
        /// </summary>
        public ulong blockNumber { get; set; }
        public ulong transactionIndex { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public decimal value { get; set; }
        public decimal gasPrice { get; set; }
        public decimal gas { get; set; }
    }
}
