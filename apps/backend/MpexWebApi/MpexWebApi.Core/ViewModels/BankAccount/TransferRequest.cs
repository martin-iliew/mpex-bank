namespace MpexWebApi.Core.ViewModels.BankAccount
{
    public class TransferRequest
    {
        public string ReceiverIBAN { get; set; }
        public decimal Amount { get; set; }
    }
}
