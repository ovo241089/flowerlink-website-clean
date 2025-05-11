using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerLink.Models.TapPayment
{
    public class PaymentViewModel
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OrderNo { get; set; }
        public string Description { get; set; }
    }
    
    public class PaymentModel
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        public bool CustomerInitiated { get; set; }
        public bool ThreeDSecure { get; set; }
        public bool SaveCard { get; set; }
        public string Description { get; set; }
        //public string StatementDescriptor { get; set; }
        public Metadata Metadata { get; set; }
        public Reference Reference { get; set; }
        public Receipt Receipt { get; set; }
        public Customer Customer { get; set; }
        public Merchant Merchant { get; set; }
        public Source Source { get; set; }
        public bool AuthorizeDebit { get; set; }
        //public Auto Auto { get; set; }
        public Post Post { get; set; }
        public Redirect Redirect { get; set; }
    }

    public class Metadata
    {
        public string Udf1 { get; set; }
        public string Udf2 { get; set; }
        public string Udf3 { get; set; }
    }

    public class Reference
    {
        public string Transaction { get; set; }
        public string Track { get; set; }
        public string Payment { get; set; }
        public string TraceId { get; set; }
        public string Order { get; set; }
        public string Acquirer { get; set; }
        public string Gateway { get; set; }
    }

    public class Receipt
    {
        public string Id { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Phone Phone { get; set; }
    }

    public class Phone
    {
        public string CountryCode { get; set; }
        public string Number { get; set; }
    }

    public class Merchant
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Id { get; set; }
    }

    public class Source
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public string PaymentType { get; set; }
        public string Channel { get; set; }
        public bool OnFile { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class Auto
    {
        public string Type { get; set; }
        public int Time { get; set; }
    }

    public class Post
    {
        public string Status { get; set; }
        public string Url { get; set; }
    }

    public class Redirect
    {
        public string Status { get; set; }
        public string Url { get; set; }
    }

    public class PaymentResponse
    {
        public string Id { get; set; } // The unique identifier for the payment transaction, which you can use for tracking.
        public string Object { get; set; }
        public bool CustomerInitiated { get; set; }
        public bool AuthorizeDebit { get; set; }
        public bool LiveMode { get; set; }
        public string ApiVersion { get; set; }
        public string Method { get; set; }
        public string Status { get; set; } // The status of the payment (e.g., INITIATED, COMPLETED, FAILED). This is critical to determine the outcome of the transaction.
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool ThreeDSecure { get; set; }
        public bool SaveCard { get; set; }
        public string Product { get; set; }
        public Transaction Transaction { get; set; } // Url: A link to the payment page (for further actions if needed).
        public Reference Reference { get; set; }
        public Response ResponseDetails { get; set; } // Code: A status code indicating the result of the transaction (e.g., "100" for success).
        public Receipt Receipt { get; set; }
        public Customer Customer { get; set; }
        public Source Source { get; set; }
        public Redirect Redirect { get; set; }
        public Post Post { get; set; }
        public Auto Auto { get; set; }
        public Merchant Merchant { get; set; }
        public Metadata Metadata { get; set; }
        public Order Order { get; set; }
    }

    public class Transaction
    {
        public string Timezone { get; set; }
        public string Created { get; set; }
        public string Url { get; set; }
        public Expiry Expiry { get; set; }
        public bool Asynchronous { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string AuthorizationId { get; set; }
    }

    public class Expiry
    {
        public int Period { get; set; }
        public string Type { get; set; }
    }

    public class Response
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class VerificationResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public bool LiveMode { get; set; }
        public bool CustomerInitiated { get; set; }
        public string ApiVersion { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool ThreeDSecure { get; set; }
        public bool CardThreeDSecure { get; set; }
        public bool SaveCard { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public Metadata Metadata { get; set; }
        public Order Order { get; set; }
        public Transaction Transaction { get; set; }
        public Reference Reference { get; set; }
        public Response ResponseDetails { get; set; }
        public CardSecurity CardSecurity { get; set; }
        public Security Security { get; set; }
        public Acquirer Acquirer { get; set; }
        public Gateway Gateway { get; set; }
        public Card Card { get; set; }
        public Receipt Receipt { get; set; }
        public Customer Customer { get; set; }
        public Merchant Merchant { get; set; }
        public Source Source { get; set; }
        public Redirect Redirect { get; set; }
        public Post Post { get; set; }
        public Authentication Authentication { get; set; }
        public List<Activity> Activities { get; set; }
        public bool AutoReversed { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
    }

    public class CardSecurity
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Security
    {
        public ThreeDSecure ThreeDSecure { get; set; }
    }

    public class ThreeDSecure
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }

    public class Acquirer
    {
        public AcquirerResponse Response { get; set; }
    }

    public class AcquirerResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Gateway
    {
        public GatewayResponse Response { get; set; }
    }

    public class GatewayResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Card
    {
        public string Object { get; set; }
        public string FirstSix { get; set; }
        public string FirstEight { get; set; }
        public string Scheme { get; set; }
        public string Brand { get; set; }
        public string LastFour { get; set; }
    }


    public class Authentication
    {
        public string Id { get; set; }
    }

    public class Activity
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string TxnId { get; set; }
    }

}