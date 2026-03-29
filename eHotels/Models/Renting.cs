using System.Security.Principal;

class Renting
{
    public int RentingID{get;set;}
    public string Status{get;set;}
    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public int InvoiceNumber{get;set;}
    public string PaymentMethod{get;set;}
    public int Amount{get;set;}
    public DateOnly ProcessedDate{get;set;}

    public Renting(){}

    public Renting(int rentingID, string status, DateOnly startDate, DateOnly endDate, int invoiceNumber, string paymentMethod, int amount, DateOnly processedDate)
    {
        this.RentingID = rentingID;
        this.Status = status;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.InvoiceNumber = invoiceNumber;
        this.PaymentMethod = paymentMethod;
        this.Amount = amount;
        this.ProcessedDate = processedDate;
    }
}