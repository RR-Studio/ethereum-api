using Newtonsoft.Json.Linq;
using Stock.Ethereum.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Text;

namespace ethereum_api
{
    class Program
    {
        public static string NodeUrl = "http://localhost:8545";
        //public static string NodeUrl = "http://176.112.213.5:8545";
        static void Main(string[] args)
        {
            //   --rpcaddr "0.0.0.0"   aloow access remote

            //--testnet

            //https://github.com/ethereum/go-ethereum/wiki/Management-APIs
            //https://github.com/ethereum/wiki/wiki/JSON-RPC
            //http://faucet.ropsten.be:3001/     testnet add ETH

            //https://github.com/ethereum/go-ethereum/wiki/Contracts-and-Transactions
            //https://forum.ethereum.org/discussion/4313/send-transactions-to-ethereum-contracts-via-json-rpc-api

            //https://ropsten.etherscan.io

            EthereumAPI api = new EthereumAPI(NodeUrl);


            //create account

            //string wallet = CreateAccount("password");

            //create transaction
            
            api.UnlockAccount("0xe31cbd6bfc7fc67238d761045f0567c57a7b34c2", "aviator2011!");
            string tx = api.SendTransaction("0xe31cbd6bfc7fc67238d761045f0567c57a7b34c2", "0xe4c8f070f325989396c8cc345912d32698da15fd", 1);
            api.LockAccount("0xe31cbd6bfc7fc67238d761045f0567c57a7b34c2");
            
            //Console.WriteLine(res);


            //get transaction info
            //ETHTransaction tx = api.GetTransactionByHash("0xaa0739da8ed019618d141662cc75bec2101829a60d26d275b11530e23794d389");

            //get highest block nubmber
            //ulong nmb = api.GetBlockNumber();

            //Console.WriteLine(ether);

            //for (ulong i = 300000; i < 400000; i++)
            //{
                //List<ETHTransaction> list = api.GetBlockByNumber(i);
                //Console.WriteLine(i);
            //}

            

        }
    }
}