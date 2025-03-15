//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CarRescueSystem.DAL.Repository.Interface;

//namespace CarRescueSystem.DAL.Repository.Implement
//{
//    public class PaymentRepository : IPaymentRepository
//    {
//        private static readonly List<PaymentTransaction> Transactions = new();

//        public void SaveTransaction(PaymentTransaction transaction)
//        {
//            Transactions.Add(transaction);
//        }

//        public PaymentTransaction GetTransaction(long paymentId)
//        {
//            return Transactions.Find(t => t.PaymentId == paymentId);
//        }
//    }

//}
